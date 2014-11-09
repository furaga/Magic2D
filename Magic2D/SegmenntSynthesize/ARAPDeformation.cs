using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using OpenCvSharp;
using FLib;

namespace Magic2D
{
    public class ARAPDeformation
    {
        public List<PointF> texcoordList = new List<PointF>(); // texcoord用
        public List<PointF> orgMeshPointList = new List<PointF>(); // ARAP用
        public List<PointF> meshPointList = new List<PointF>();
        public List<TriMesh> meshList = new List<TriMesh>();

        int pathCnt = 0;

        Pen pen = new Pen(Brushes.Black, 2);
        Pen penB = new Pen(Brushes.Red, 3);

        public List<PointF> controlPoints { get { return controls; } }
        List<PointF> controls = new List<PointF>();
        List<PointF> orgControls = new List<PointF>();
        List<int> meshPtToPart = new List<int>();
        List<int> controlsToPart = new List<int>();

        float[] weights;
        float[] A00;
        float[] A01;
        float[] A10;
        float[] A11;
        PointF[] D;

        public List<PointF> GetPath()
        {
            if (meshPointList == null || meshPointList.Count < pathCnt)
                return new List<PointF>();
            return meshPointList.Take(pathCnt).ToList();
        }

        public void ClearControlPoints()
        {
            controls.Clear();
            controlsToPart.Clear();
            orgControls.Clear();
            weights = A00 = A01 = A10 = A11 = null;
            D = null;
        }

        public ARAPDeformation()
        {

        }

        public ARAPDeformation(ARAPDeformation aRAPDeformation)
        {
            if (aRAPDeformation == null)
                return;

            orgMeshPointList = new List<PointF>(aRAPDeformation.orgMeshPointList);
            texcoordList = new List<PointF>(aRAPDeformation.texcoordList);
            meshPointList = new List<PointF>(aRAPDeformation.meshPointList);
            meshList = new List<TriMesh>(aRAPDeformation.meshList);

            pathCnt = aRAPDeformation.pathCnt;

            controls = new List<PointF>(aRAPDeformation.controls);
            orgControls = new List<PointF>(aRAPDeformation.orgControls);
            meshPtToPart = new List<int>(aRAPDeformation.meshPtToPart);
            controlsToPart = new List<int>(aRAPDeformation.controlsToPart);

            if (aRAPDeformation.weights != null)
                weights = aRAPDeformation.weights.ToArray();
            if (aRAPDeformation.A00 != null)
                A00 = aRAPDeformation.A00.ToArray();
            if (aRAPDeformation.A01 != null)
                A01 = aRAPDeformation.A01.ToArray();
            if (aRAPDeformation.A10 != null)
                A10 = aRAPDeformation.A10.ToArray();
            if (aRAPDeformation.A11 != null)
                A11 = aRAPDeformation.A11.ToArray();
            if (aRAPDeformation.D != null)
                D = aRAPDeformation.D.ToArray();
        }

        public static ARAPDeformation Combine(ARAPDeformation arap1, ARAPDeformation arap2, CharacterRange sectionRange1, CharacterRange sectionRange2)
        {
            var arap = new ARAPDeformation();

            // パスの統合
            var path1 = arap1.GetPath();
            var path2 = arap2.GetPath();
            var path = CombinePath(path1, path2, sectionRange1, sectionRange2);

            // メッシュの統合
            CombineMesh(arap1, arap2, path, arap);

            // partの統合。接合面付近のpartを同じにする

            
            throw new NotImplementedException();
        }

        private static void CombineMesh(ARAPDeformation arap1, ARAPDeformation arap2, List<PointF> path, ARAPDeformation outArap)
        {
            // 点群の統合

            // meshPointListは現在の値で、orgMeshPointList = meshPointListと再初期化
            outArap.pathCnt = path.Count;
            outArap.meshPointList = new List<PointF>();

            Dictionary<PointF, int> ptToIdx = new Dictionary<PointF,int>();

            int idx = 0;
            foreach (var ls in new[] { path, arap1.meshPointList, arap2.meshPointList })
            {
                foreach (var p in ls)
                {
                    if (!ptToIdx.ContainsKey(p))
                        continue;
                    outArap.meshPointList.Add(p);
                    ptToIdx[p] = idx;
                    idx++;
                }
            }

            outArap.orgMeshPointList = new List<PointF>(outArap.meshPointList);


            // 三角形の統合
            outArap.meshList = new List<TriMesh>();
//            outArap.meshList.Add(new TriMesh(

            
            // texcoordはそのまま

        }

        static List<PointF> CombinePath(List<PointF> path1, List<PointF> path2, CharacterRange sectionRange1, CharacterRange sectionRange2)
        {
            if (path1 == null || path2 == null)
                return new List<PointF>();

            // 切断面の両端以外は除去
            var path = new List<PointF>();

            int start1 = (sectionRange1.First + sectionRange1.Length) % path1.Count;
            int end1 = sectionRange1.First % path2.Count;
            for (int i = start1; i != end1; i = (i + 1) % path1.Count)
                path.Add(path1[i]);

            int start2 = path2.IndexOf(path1[end1]);
            int end2 = path2.IndexOf(path1[start1]);
            int dir;
            // sectionRange2.Firstの方の端点に近い
            if (Math.Abs(start2 - sectionRange2.First) < Math.Abs(start2 - sectionRange2.First - sectionRange2.Length))
                dir = -1;
            else
                dir = 1;
            for (int i = start2; i != end2; i = (i + path2.Count + dir) % path2.Count)
                path.Add(path2[i]);

            return path;
        }


        public ARAPDeformation(List<PointF> path, List<PointF> partingLine)
        {
            if (path == null || path.Count <= 2)
                return;

            CreateMesh(path, 32, 16, orgMeshPointList, meshList);

            Parting(orgMeshPointList, meshList, partingLine, meshPtToPart);

            meshPointList = new List<PointF>(orgMeshPointList);
            texcoordList = new List<PointF>(orgMeshPointList);

            pathCnt = path.Count;
        }

        //--------------------------------------------------------

        public static void CreateMesh(List<PointF> path, int gridSize, int minDist, List<PointF> outPts, List<TriMesh> outTris)
        {
            if (outPts == null || outTris == null)
                return;

            if (path == null || path.Count <= 2)
                return;

            outPts.Clear();
            outTris.Clear();

            TriangleNET.Triangulate(path, outPts, outTris);

            for (int i = 0; i < path.Count; i++)
                Debug.Assert(FMath.SqDistance(path[i], outPts[i]) <= 1e-4 * 1e-4);

            /*
            var points = CreateMeshPoints(path, gridSize, minDist);

            Dictionary<PointF, int> ptToIdx = new Dictionary<PointF, int>();

//            TriangleNet.Tools.

            for (int i = 0; i < points.Count; i++)
            {
                outPts.Add(points[i]);
                ptToIdx[points[i]] = i;
            }

            float minx = path.Select(p => p.X).Min();
            float miny = path.Select(p => p.Y).Min();
            float maxx = path.Select(p => p.X).Max();
            float maxy = path.Select(p => p.Y).Max();

            DelaunayTriangle dt = new DelaunayTriangle(points, new RectangleF(minx - 1, miny - 1, 2 + maxx - minx, 2 + maxy - miny));
            foreach (var t in dt.triangleList)
            {
                var mesh = new TriMesh(ptToIdx[t.p1], ptToIdx[t.p2], ptToIdx[t.p3]);
                outTris.Add(mesh);
            }
             * */
        }

        public static List<PointF> CreateMeshPoints(List<PointF> path, int gridSize, int threhold)
        {
            if (path == null || path.Count <= 2)
                return new List<PointF>();

            // パス
            var points = new List<PointF>(path);
            
            float minx = path.Select(p => p.X).Min();
            float maxx = path.Select(p => p.X).Max();
            float miny = path.Select(p => p.Y).Min();
            float maxy = path.Select(p => p.Y).Max();

            // 外側
            for (int i = 0; i < path.Count; i++)
            {
                PointF pt1 = path[i];
                PointF pt2 = path[(i + 1) % path.Count];
                float cx = (pt1.X + pt2.X) * 0.5f;
                float cy = (pt1.Y + pt2.Y) * 0.5f;
                float vx = pt2.Y - pt1.Y;
                float vy = pt1.X - pt2.X;
                float len = (float)Math.Sqrt(vx * vx + vy * vy);
                if (len <= 1e-4f)
                    continue;
                vx /= len;
                vy /= len;

                PointF pt = new PointF(cx + vx, cy + vy);
                float shift = 5;
                if (FMath.IsPointInPolygon(pt, path))
                    // (vx, vy) がパスの内側を向いている
                    points.Add(new PointF(cx - vx * shift, cy - vy * shift));
                else
                    points.Add(new PointF(cx + vx * shift, cy + vy * shift));
            }

            // 内側
            for (float y = miny + gridSize; y < maxy; y += gridSize)
                for (float x = minx + gridSize; x < maxx; x += gridSize)
                {
                    var pt = new PointF(x, y);
                    if (!FMath.IsPointInPolygon(pt, path))
                        continue;
                    float mindist = float.MaxValue;
                    for (int i = 0; i < path.Count; i++)
                    {
                        float dist = FMath.GetDistanceToLine(pt, path[i], path[(i + 1) % path.Count]);
                        if (mindist > dist)
                            mindist = dist;
                    }
                    if (mindist <= threhold)
                        continue;
                    points.Add(pt);
                }

            return points;
        }

        // pathをpartinLineで分割した小さい領域が複数できる
        // ptsの各点が各領域のどれに含まれるかの情報をoutPtToPartに格納する
        // 分割線は交差しないものとする
        static void Parting(List<PointF> pts, List<TriMesh> tris, List<PointF> partingLine, List<int> outPtToPart)
        {
            if (outPtToPart == null)
                return;
            if (pts == null)
                return;

            // デフォルトでは全部0にする
            for (int i = 0; i < pts.Count; i++)
                outPtToPart.Add(0);

            if (partingLine == null || partingLine.Count <= 1)
                return;

            Dictionary<int, List<int>> edges = GetEdges(pts, tris);
            HashSet<int> boarderEdgeCodes = GetEdgeCodeNearLine(pts, edges, partingLine);
            var labels = FMath.FloodFill(pts.Count, edges, cond, boarderEdgeCodes);
            FillPointsNearBoarder(pts, edges, boarderEdgeCodes, labels);

            outPtToPart.Clear();
            if (labels != null && labels.Count == pts.Count)
            {
                for (int i = 0; i < labels.Count; i++)
                    outPtToPart.Add(labels[i]);
            }
        }

        static Dictionary<int, List<int>> GetEdges(List<PointF> pts, List<TriMesh> tris)
        {
            Dictionary<int, List<int>> edges = new Dictionary<int, List<int>>();

            for (int i = 0; i < pts.Count; i++)
                edges[i] = new List<int>();

            if (pts == null || tris == null || pts.Count <= 0 || tris.Count <= 0)
                return edges;

            foreach (var t in tris)
            {
                if (!edges.ContainsKey(t.idx0) || !edges.ContainsKey(t.idx1) || !edges.ContainsKey(t.idx2))
                    continue;
                edges[t.idx0].AddRange(new[] { t.idx1 });
                edges[t.idx1].AddRange(new[] { t.idx2 });
                edges[t.idx2].AddRange(new[] { t.idx0 });
            }

            return edges;
        }

        static HashSet<int> GetEdgeCodeNearLine(List<PointF> pts,  Dictionary<int, List<int>> es, List<PointF> line)
        {
            HashSet<int> edgeCodes = new HashSet<int>(); // 分割線と交差しているエッジ

            if (pts == null || es == null || line == null)
                return edgeCodes;

            foreach (var ee in es)
            {
                foreach (int _j in ee.Value)
                {
                    int i = ee.Key;
                    int j = _j;
                    if (i > j)
                        FMath.Swap(ref i, ref j);
                    if (FMath.IsCrossed(pts[i], pts[j], line))
                        edgeCodes.Add(i * pts.Count + j);
                }
            }
            return edgeCodes;
        }

        static void FillPointsNearBoarder(List<PointF> pts, Dictionary<int, List<int>> edges, HashSet<int> _edgeCodes, List<int> outLabels)
        {
            var edgeCodes = new HashSet<int>(_edgeCodes);

            var targets = new HashSet<int>();
            foreach (var e in edgeCodes)
            {
                targets.Add(e / pts.Count);
                targets.Add(e % pts.Count);
            }

            while (targets.Count > 0)
            {
                HashSet<int> remove = new HashSet<int>();

                foreach (int i in targets.ToArray())
                {
                    var ls = edges[i];
                    bool found = false;

                    // 境界付近の点は近傍の境界外の点の色で塗り替える
                    foreach (int j in ls)
                    {
                        if (targets.Contains(j))
                            continue;
                        outLabels[i] = outLabels[j];
                        found = true;
                    }

                    // 塗り替えられてない場合は次回に持ち越し
                    if (!found)
                        continue;

                    remove.Add(i);
                }

                foreach (int i in remove)
                {
                    var ls = edges[i];
                    foreach (int j in ls)
                        edgeCodes.Remove(Math.Min(j, i) * pts.Count + Math.Max(j, i));
                    targets.Remove(i);
                }
            }
        }

        static bool cond(int i, int j, int n, HashSet<int> set)
        {
            if (i > j)
                FMath.Swap(ref i, ref j);

            if (!set.Contains(i * n + j))
                return true;

            return false;
        }

        //--------------------------------------------------------
        
        public List<PointF> GetMeshVertexList()
        {
            List<PointF> pts = new List<PointF>();
            foreach (var t in meshList)
            {
                PointF pt0 = meshPointList[t.idx0];
                PointF pt1 = meshPointList[t.idx1];
                PointF pt2 = meshPointList[t.idx2];
                pts.Add(pt0);
                pts.Add(pt1);
                pts.Add(pt2);
            }
            return pts;
        }

        public  List<PointF> GetMeshCoordList(int w, int h)
        {
            if (w <= 0 || h <= 0)
                return null;

            List<PointF> pts = new List<PointF>();
            foreach (var t in meshList)
            {
                PointF pt0 = texcoordList[t.idx0];
                PointF pt1 = texcoordList[t.idx1];
                PointF pt2 = texcoordList[t.idx2];
                pts.Add(new PointF(pt0.X / w, pt0.Y / h));
                pts.Add(new PointF(pt1.X / w, pt1.Y / h));
                pts.Add(new PointF(pt2.X / w, pt2.Y / h));
            }

            return pts;
        }

        //--------------------------------------------------------

        public Bitmap ToBitmap()
        {
            int maxx = (int)meshPointList.Select(p => p.X).Max() + 1;
            int maxy = (int)meshPointList.Select(p => p.Y).Max() + 1;
            if (maxx <= 0 || maxy <= 0)
                return null;
            Bitmap bmp = new Bitmap(maxx, maxy, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);

                g.DrawLines(penB, GetPath().ToArray());
                foreach (var t in meshList)
                {
                    PointF pt0 = meshPointList[t.idx0];
                    PointF pt1 = meshPointList[t.idx1];
                    PointF pt2 = meshPointList[t.idx2];
                    g.DrawLines(pen, new PointF[] { pt0, pt1, pt2, pt0 });
                }

                var path = GetPath();
                if (path != null)
                {
                    foreach (var p in path)
                        g.FillRectangle(Brushes.Orange, p.X - 2, p.Y - 2, 4, 4);
                }
                if (controlPoints != null)
                {
                    foreach (var p in controlPoints)
                        g.FillRectangle(Brushes.Cyan, p.X - 2, p.Y - 2, 4, 4);
                }
            }
            return bmp;
        }

        // テクスチャマッピングをする
        unsafe public Bitmap ToBitmap(Bitmap texture)
        {
            int maxx = (int)meshPointList.Select(p => p.X).Max() + 1;
            int maxy = (int)meshPointList.Select(p => p.Y).Max() + 1;
            if (maxx <= 0 || maxy <= 0)
                return null;
            Bitmap bmp = new Bitmap(maxx, maxy, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
            }

            if (bmp != null)
            {
                using (var inIter = new BitmapIterator(texture, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                using (var outIter = new BitmapIterator(bmp, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    for (int i = 0; i < meshList.Count; i++)
                    {
                        var tri = meshList[i];

                        PointF pt0 = meshPointList[tri.idx0];
                        PointF pt1 = meshPointList[tri.idx1];
                        PointF pt2 = meshPointList[tri.idx2];

                        PointF cd0 = texcoordList[tri.idx0];
                        PointF cd1 = texcoordList[tri.idx1];
                        PointF cd2 = texcoordList[tri.idx2];

                        List<PointF> pts = new List<PointF>() { 
                        pt0, pt1, pt2,
                    };

                        pts = pts.OrderBy(p => p.Y).ToList();

                        // ラスタライズ
                        float v1_dx = pt1.X - pt0.X;
                        float v1_dy = pt1.Y - pt0.Y;
                        float v2_dx = pt2.X - pt0.X;
                        float v2_dy = pt2.Y - pt0.Y;
                        float v1_len = (float)Math.Sqrt(v1_dx * v1_dx + v1_dy * v1_dy);
                        if (v1_len <= 1e-4)
                            continue;
                        float v2_len = (float)Math.Sqrt(v2_dx * v2_dx + v2_dy * v2_dy);
                        if (v2_len <= 1e-4)
                            continue;

                        float v1_x = v1_dx / v1_len;
                        float v1_y = v1_dy / v1_len;
                        float v2_x = v2_dx / v2_len;
                        float v2_y = v2_dy / v2_len;

                        float ds = 1 / v1_len;
                        float dt = 1 / v2_len;

                        for (float t = 0; t <= 1; t += dt)
                        {
                            for (float s = 0; t + s <= 1; s += ds)
                            {
                                float x = pt0.X + v1_dx * s + v2_dx * t;
                                float y = pt0.Y + v1_dy * s + v2_dy * t;
                                float cx = cd0.X * (1 - s - t) + cd1.X * s + cd2.X * t;
                                float cy = cd0.Y * (1 - s - t) + cd1.Y * s + cd2.Y * t;

                                int offset_out = (int)x * 4 + (int)y * outIter.Stride;
                                int offset_in = (int)cx * 4 + (int)cy * inIter.Stride;

                                outIter.Data[offset_out + 0] = inIter.Data[offset_in + 0];
                                outIter.Data[offset_out + 1] = inIter.Data[offset_in + 1];
                                outIter.Data[offset_out + 2] = inIter.Data[offset_in + 2];
                                outIter.Data[offset_out + 3] = inIter.Data[offset_in + 3];
                            }
                        }
                    }
                }
            }

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawLines(penB, GetPath().ToArray());
                foreach (var t in meshList)
                {
                    PointF pt0 = meshPointList[t.idx0];
                    PointF pt1 = meshPointList[t.idx1];
                    PointF pt2 = meshPointList[t.idx2];
                    g.DrawLines(pen, new PointF[] { pt0, pt1, pt2, pt0 });
                }
            }
            return bmp;

        }
        
        //--------------------------------------------------------

        public void Translate(float x, float y)
        {
            for (int i = 0; i < meshPointList.Count; i++)
                meshPointList[i] = new PointF(meshPointList[i].X + x, meshPointList[i].Y + y);
            for (int i = 0; i < controls.Count; i++)
                controls[i] = new PointF(controls[i].X + x, controls[i].Y + y);
        }

        public void Transform(Matrix transform)
        {
            var _meshPointList = meshPointList.ToArray();
            transform.TransformPoints(_meshPointList);
            for (int i = 0; i < meshPointList.Count; i++)
                meshPointList[i] = _meshPointList[i];

            if (controls.Count >= 1)
            {
                _meshPointList = controls.ToArray();
                transform.TransformPoints(_meshPointList);
                for (int i = 0; i < controls.Count; i++)
                    controls[i] = _meshPointList[i];
            }
        }

        //--------------------------------------------------------

        // ARAP image deformation


        public void AddControlPoint(PointF pt, PointF orgPt)
        {
            if (!controls.Contains(pt))
            {
                controls.Add(pt);
                orgControls.Add(orgPt);

                float minDist = float.MaxValue;
                int minIdx = -1;

                for (int i = 0; i < meshPointList.Count; i++)
                {
                    float dist = FMath.SqDistance(meshPointList[i], orgPt);
                    if (minDist > dist)
                    {
                        minDist = dist;
                        minIdx = i;
                    }
                }

                if (minIdx >= 0)
                    controlsToPart.Add(meshPtToPart[minIdx]);
                else
                    controlsToPart.Add(-1);
            }
        }

        public void RemoveControlPoint(PointF pt)
        {
            if (controls.Contains(pt))
            {
                orgControls.RemoveAt(controls.IndexOf(pt));
                controls.Remove(pt);
            }
        }

        public void TranslateControlPoint(PointF pt, PointF to, bool flush)
        {
            if (!controls.Contains(pt))
                return;

            // orgControls[controls.IndexOf(pt)] = controls[controls.IndexOf(pt)]
            controls[controls.IndexOf(pt)] = to;

            if (flush)
                FlushDefomation();
        }
        public void FlushDefomation()
        {
            RigidMLS();
        }

        public void BeginDeformation()
        {
            Precompute();
        }

        public void EndDeformation()
        {
            weights = null;
            A00 = null;
            A01 = null;
            A10 = null;
            A11 = null;
            D = null;
        }




        /*
        // ボーンに沿うようにメッシュの頂点を微調整し、そこに制御点を打つ
        public List<int> AddBoneConstraint(AnnotationBone bone, float threshold)
        {
            var idxes = new List<int>();
            var src = bone.jointSrc.PositionInBmp;
            var dst = bone.jointDst.PositionInBmp;
            var dir = bone.Dir;
            var maxt = Vector3.Dot(dir, dst - src);

            for (int i = 0; i < orgMeshPointList.Count; i++)
            {
                var p = orgMeshPointList[i];
                var t = Vector3.Dot(dir, p - src);
                if (0 <= t && t <= maxt)
                {
                    var h = src + t * dir;
                    float dist = Vector3.DistanceSquared(p, h);
                    if (dist <= threshold)
                    {
                        orgMeshPointList[i] = h;
                        meshPointList[i] = h;
                        orgControls.Add(h);
                        controls.Add(h);
                        controlsToPart.Add(meshPtToPart[i]);
                        idxes.Add(orgControls.Count - 1);
                    }
                }
            }

            return idxes;
        }
        */

        public void Precompute()
        {
            // todo
            /*
            meshPtToPart.Clear();
            for (int i = 0; i < meshPointList.Count; i++)
                meshPtToPart.Add(0);

            controlsToPart.Clear();
            for (int i = 0; i < controls.Count; i++)
                controlsToPart.Add(0);
            */

            //------------

            if (controls.Count < 3)
                return;

            weights = new float[meshPointList.Count * controls.Count];
            A00 = new float[meshPointList.Count * controls.Count];
            A01 = new float[meshPointList.Count * controls.Count];
            A10 = new float[meshPointList.Count * controls.Count];
            A11 = new float[meshPointList.Count * controls.Count];
            D = new PointF[meshPointList.Count];

            for (int vIdx = 0; vIdx < meshPointList.Count; vIdx++)
            {
                int offset = vIdx * controls.Count;
                for (int i = 0; i < controls.Count; i++)
                {
                    if (meshPtToPart[vIdx] == controlsToPart[i])
                    {
                        // ぴったり同じ位置だったら無限の重みを与える
                        if (FMath.Distance(orgControls[i], orgMeshPointList[vIdx]) <= 1e-4)
                            weights[i + offset] = float.PositiveInfinity;
                        else
                            weights[i + offset] = (float)(1 / (0.01 + Math.Pow(FMath.Distance(orgControls[i], orgMeshPointList[vIdx]), 2)));
                    }
                    else
                    {
                        weights[i + offset] = 0;
                    }
                }

                PointF? Pa = CompWeightAvg(orgControls, weights, vIdx);
                if (Pa == null || !Pa.HasValue)
                    return;

                PointF[] Ph = new PointF[orgControls.Count];
                for (int i = 0; i < orgControls.Count; i++)
                {
                    if (!orgControls[i].IsEmpty)
                    {
                        Ph[i].X = orgControls[i].X - Pa.Value.X;
                        Ph[i].Y = orgControls[i].Y - Pa.Value.Y;
                    }
                }

                float mu = 0;
                for (int i = 0; i < controls.Count; i++)
                    mu += (float)(Ph[i].X * Ph[i].X + Ph[i].Y * Ph[i].Y) * weights[i + offset];

                D[vIdx].X = orgMeshPointList[vIdx].X - Pa.Value.X;
                D[vIdx].Y = orgMeshPointList[vIdx].Y - Pa.Value.Y;
                for (int i = 0; i < controls.Count; i++)
                {
                    int idx = i + offset;
                    A00[idx] = weights[idx] / mu * (Ph[i].X * D[vIdx].X + Ph[i].Y * D[vIdx].Y);
                    A01[idx] = -weights[idx] / mu * (Ph[i].X * (-D[vIdx].Y) + Ph[i].Y * D[vIdx].X);
                    A10[idx] = -weights[idx] / mu * (-Ph[i].Y * D[vIdx].X + Ph[i].X * D[vIdx].Y);
                    A11[idx] = weights[idx] / mu * (Ph[i].Y * D[vIdx].Y + Ph[i].X * D[vIdx].X);
                }
            }
        }

        float[] Ortho(float[] v, int i) { return new float[] { -v[3 * i + 1], v[3 * i], 0 }; }
        
        float LengthSquared(float[] vecs, int i)
        {
            float x = vecs[3 * i];
            float y = vecs[3 * i + 1];
            float z = vecs[3 * i + 2];
            return x * x + y * y + z * z;
        }

        float Dot(float[] v0, int i, float[] v1, int j)
        {
            return v0[3 * i + 0] * v1[3 * j + 0] +
                v0[3 * i + 1] * v1[3 * j + 1] +
                v0[3 * i + 2] * v1[3 * j + 2];
        }

        public void RigidMLS()
        {
            if (controls.Count < 3)
                return;

            if (weights == null || A00 == null || A01 == null || A10 == null || A11 == null || D == null)
                return;

            for (int vIdx = 0; vIdx < meshPointList.Count; vIdx++)
            {
                int offset = vIdx * controls.Count;
                bool flg = false;
                for (int i = offset; i < offset + controls.Count; i++)
                {
                    if (float.IsInfinity(weights[i]))
                    {
                        // infに吹き飛んでたらcontrols[i]自体を返す
                        meshPointList[vIdx] = controls[i - offset];
                        flg = true;
                        break;
                    }
                }
                if (flg)
                    continue;

                PointF? Qa = CompWeightAvg(controls, weights, vIdx);
                if (Qa == null || !Qa.HasValue)
                    continue;

                meshPointList[vIdx] = Qa.Value;
                float fx = 0;
                float fy = 0;
                for (int i = 0; i < controls.Count; i++)
                {
                    int idx = i + vIdx * controls.Count;
                    float qx = controls[i].X - Qa.Value.X;
                    float qy = controls[i].Y - Qa.Value.Y;
                    fx += qx * A00[idx] + qy * A10[idx];
                    fy += qx * A01[idx] + qy * A11[idx];
                }
                float lenD = (float)Math.Sqrt(D[vIdx].X * D[vIdx].X + D[vIdx].Y * D[vIdx].Y);
                float lenf = (float)Math.Sqrt(fx * fx + fy * fy);
                float k = lenD / (0.01f + lenf);
                PointF pt = meshPointList[vIdx];
                pt.X += fx * k;
                pt.Y += fy * k;
                meshPointList[vIdx] = pt;
            }
        }

        PointF? CompWeightAvg(List<PointF> controls, float[] weight, int vIdx)
        {
            PointF pos_sum = PointF.Empty;

            int offset = vIdx * controls.Count;

            float w_sum = 0;
            for (int i = offset; i < offset + controls.Count; i++)
                w_sum += weight[i];

            if (w_sum <= 0)
                return null;

            for (int i = offset; i < offset + controls.Count; i++)
            {
                var p = controls[i - offset];
                pos_sum.X += p.X * weight[i];
                pos_sum.Y += p.Y * weight[i];
            }

            pos_sum.X /= w_sum;
            pos_sum.Y /= w_sum;

            return pos_sum;
        }

        public PointF? OrgToCurControlPoint(PointF orgPt)
        {
            if (!orgControls.Contains(orgPt))
                return null;
            int idx = orgControls.IndexOf(orgPt);
            return controls[idx];
        }

        public static List<PointF> Deform(List<PointF> pts, List<Tuple<PointF, PointF>> moves)
        {
            List<PointF> newPts = new List<PointF>();
            for (int i = 0; i < pts.Count; i++)
            {
                bool finish = false;
                List<float> ws = new List<float>();
                float w_sum = 0;

                foreach (var mv in moves)
                {
                    if (mv.Item1 == pts[i])
                    {
                        newPts.Add(mv.Item2);
                        break;
                    }
                    float w = (float)(1 / (0.01 + Math.Pow(FMath.Distance(mv.Item1, pts[i]), 2)));
                    ws.Add(w);
                    w_sum += w;
                }

                if (finish)
                    continue;

                if (w_sum <= 1e-4)
                {
                    newPts.Add(pts[i]);
                    continue;
                }

                float inv_w_sum = 1 / w_sum;
                for (int j = 0; j < moves.Count; j++)
                    ws[j] *= inv_w_sum;

                float x = pts[i].X;
                float y = pts[i].Y;
                for (int j = 0; j < moves.Count; j++)
                {
                    var mv = moves[j];
                    float dx = mv.Item2.X - mv.Item1.X;
                    float dy = mv.Item2.Y - mv.Item1.Y;
                    x += dx * ws[j];
                    y += dy * ws[j];
                }
                newPts.Add(new PointF(x, y));
            }
            return newPts;
        }

    }
}