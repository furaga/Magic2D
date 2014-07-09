using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using OpenCvSharp;
using FLib;

namespace Magic2D
{
    public class ARAPDeformation
    {
        public List<PointF> orgMeshPointList = new List<PointF>();
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

        public struct TriMesh
        {
            public int idx0;
            public int idx1;
            public int idx2;
            public TriMesh(int idx0, int idx1, int idx2)
            {
                this.idx0 = idx0;
                this.idx1 = idx1;
                this.idx2 = idx2;
            }
        }

        public List<PointF> GetPath()
        {
            if (meshPointList == null || meshPointList.Count <= pathCnt)
                return new List<PointF>();
            return meshPointList.Take(pathCnt).ToList();
        }

        public ARAPDeformation()
        {

        }

        public ARAPDeformation(List<PointF> path, List<PointF> partingLine)
        {
            if (path == null || path.Count <= 2)
                return;

            CreateMesh(path, 32, 16, orgMeshPointList, meshList);

 //           Parting(orgMeshPointList, path, partingLine, meshPtToPart);

            meshPointList = new List<PointF>(orgMeshPointList);

            pathCnt = path.Count;

            for (int i = 0; i < pathCnt; i++)
                Debug.Assert(path[i] == meshPointList[i]);  
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

            var points = CreateMeshPoints(path, gridSize, minDist);

            Dictionary<PointF, int> ptToIdx = new Dictionary<PointF, int>();

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
        private void Parting(List<PointF> pts, List<PointF> path, List<PointF> partingLine, List<int> outPtToPart)
        {
           throw new NotImplementedException();

            /*
            outPtToPart.Clear();

            // 交差判定の結果
            // List<PointF> crossPts = new List<PointF>();
            List<int> crossPathIdxs = new List<int>();
            List<int> crossPartingIdxs = new List<int>();

            // 同じ領域にあるパス点の組
            Dictionary<int, int> constraints = new Dictionary<int, int>();
            List<int> curves = new List<int>();

            int pOutIdx = -1;
            int pInIdx = -1;
            for (int i = 0; i < partingLine.Count; i++)
            {
                PointF p1 = partingLine[i];
                PointF p2 = partingLine[(i + 1) % partingLine.Count];
                for (int j = 0; j < path.Count; i++)
                {
                    PointF p3 = path[j];
                    PointF p4 = path[(j + 1) % path.Count];
                    if (FMath.IsCrossed(p1, p2, p3, p4))
                    {
                        crossPartingIdxs.Add(i);
                        crossPathIdxs.Add(j);

                        int inIdx, outIdx;
                        float dir = (p4.X - p3.X) * (p1.Y - p3.Y) + (p4.Y - p3.Y) * (p1.X - p3.X);
                        if (dir > 0)
                        {
                            inIdx = j;
                            outIdx = (j + 1) % path.Count;
                        }
                        else
                        {
                            inIdx = (j + 1) % path.Count;
                            outIdx = j;
                        }

                        if (pOutIdx <= -1)
                        {
                            pInIdx = inIdx;
                            pOutIdx = outIdx;
                        }
                        else
                        {
                            constraints[pInIdx] = inIdx;
                            constraints[pOutIdx] = outIdx;
                            pOutIdx = pInIdx = -1;
                        }
                    }
                }
            }

            // 小領域のパス
            List<List<int>> smallPaths = new List<List<int>>();


            */
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
                PointF pt0 = orgMeshPointList[t.idx0];
                PointF pt1 = orgMeshPointList[t.idx1];
                PointF pt2 = orgMeshPointList[t.idx2];
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
                g.Clear(Color.White);

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
            meshPtToPart.Clear();
            for (int i = 0; i < meshPointList.Count; i++)
                meshPtToPart.Add(0);

            controlsToPart.Clear();
            for (int i = 0; i < controls.Count; i++)
                controlsToPart.Add(0);

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
                        weights[i + offset] = (float)(1 / (0.01 + Math.Pow(FMath.Distance(orgControls[i], orgMeshPointList[vIdx]), 2)));
                    else
                        weights[i + offset] = 0;
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