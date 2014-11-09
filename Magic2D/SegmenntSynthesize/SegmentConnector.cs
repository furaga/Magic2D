using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using FLib;
using Magic2D;

namespace Magic2D
{
    /// <summary>
    /// 複数のセグメントをスケルトン情報をもとに結合する
    /// 1. スケルトンに合わせて各セグメントをざっくり移動・ボーン方向にARAP
    /// 2. セグメント同士が自然に繋がるように位置・角度・スケールを調整
    /// 3. 繋ぎ目が重なるようにARAP
    /// 4. つながった複数のセグメントから新しいセグメントを生成                                                                                                                                                                                                                                                                                                                                         /// </summary>
    public class SegmentConnector
    {
        public List<SegmentMeshInfo> meshes = new List<SegmentMeshInfo>();
        public SkeletonAnnotation an;

        public class ConnectPair
        {
            public BoneAnnotation bone;
            public SegmentMeshInfo meshInfo1;
            public CharacterRange sectionRange1;
            public SegmentMeshInfo meshInfo2;
            public CharacterRange sectionRange2;
            public ConnectPair(BoneAnnotation bone, SegmentMeshInfo meshInfo1, CharacterRange section1, SegmentMeshInfo meshInfo2, CharacterRange section2)
            {
                this.bone = bone;
                this.meshInfo1 = meshInfo1;
                this.sectionRange1 = section1;
                this.meshInfo2 = meshInfo2;
                this.sectionRange2 = section2;
            }
        }

        //------------------------------------------------------------------

        public SegmentConnector(List<SegmentMeshInfo> meshes, SkeletonAnnotation an, List<int> meshOrder)
        {
            var deformedMeshes = new List<SegmentMeshInfo>();
            foreach (var m in meshes)
                deformedMeshes.Add(new SegmentMeshInfo(m));

            // スケルトンが指定されていなければ、meshes内のanを統合したものを用意する
            if (an == null)
                return;

            this.an = new SkeletonAnnotation(an, true);

            var pairs = GetSectionPairs(deformedMeshes, this.an);
            Deform(deformedMeshes, this.an, pairs);

            this.meshes = Connect(deformedMeshes, pairs, meshOrder);
        }

        private List<SegmentMeshInfo> Connect(List<SegmentMeshInfo> deformedMeshes, List<ConnectPair> pairs, List<int> meshOrder)
        {
            return deformedMeshes;

            List<List<SegmentMeshInfo>> meshGroups = new List<List<SegmentMeshInfo>>();
            foreach (var p in pairs)
            {
                int idx = -1;
                for (int i = 0; i < meshGroups.Count; i++)
                {
                    if (meshGroups[i].Contains(p.meshInfo1) || meshGroups[i].Contains(p.meshInfo2))
                    {
                        idx = i;
                        break;
                    }
                }
                if (idx <= -1)
                {
                    meshGroups.Add(new List<SegmentMeshInfo>() { p.meshInfo1, p.meshInfo2 });
                }
                else
                {
                    if (!meshGroups[idx].Contains(p.meshInfo1))
                        meshGroups[idx].Add(p.meshInfo1);
                    if (!meshGroups[idx].Contains(p.meshInfo2))
                        meshGroups[idx].Add(p.meshInfo2);
                }
            }

            for (int i = 0; i < meshGroups.Count; i++)
                meshGroups[i] = meshGroups[i].OrderBy(m => meshOrder[deformedMeshes.IndexOf(m)]).ToList();

            List<SegmentMeshInfo> meshes = new List<SegmentMeshInfo>();
            for (int i = 0; i < meshGroups.Count; i++)
                meshes.Add(new SegmentMeshInfo(meshGroups[i], an));

            return meshes;
        }

        static bool AreEqualBone(BoneAnnotation b1, BoneAnnotation b2)
        {
            return (b1.src.name == b2.src.name && b1.dst.name == b2.dst.name) ||
                   (b1.src.name == b2.dst.name && b1.dst.name == b2.src.name);
        }

        static List<ConnectPair> GetSectionPairs(List<SegmentMeshInfo> meshes, SkeletonAnnotation an)
        {
            if (an == null)
                return null;

            if (meshes == null)
                return null;

            var pairs = new List<ConnectPair>();
            foreach (var b in an.bones)
            {
                var crosses = new Dictionary<SegmentMeshInfo, CrossBoneSection>();
                foreach (var m in meshes)
                    if (m.crossDict.ContainsKey(b))
                        crosses[m] = m.crossDict[b];

                if (crosses.Count == 2 && crosses.Values.First().dir * crosses.Values.Last().dir < 0)
                {
                    var kv1 = crosses.First();
                    var kv2 = crosses.Last();
                    pairs.Add(new ConnectPair(b, kv1.Key, kv1.Value.sectionRange, kv2.Key, kv2.Value.sectionRange));
                }
            }

            return pairs;
        }

        static void Deform(List<SegmentMeshInfo> meshes, SkeletonAnnotation an, List<ConnectPair> pairs)
        {
            if (meshes == null || meshes.Count <= 0)
                return;

            if (meshes.Count == 1)
            {
                SkeletonFitting.Fitting(meshes[0], an);
                return;
            }

            float delta = 0;
            for (int i = 0; i < 5; i++)
            {
                delta = AdjustScale(meshes, an, pairs);
                if (delta <= 1e-4)
                    break;
                foreach (var m in meshes)
                    SkeletonFitting.Fitting(m, an);
            }

            foreach (var m in meshes)
                SkeletonFitting.Fitting(m, an);

            //            AdjustRotation(meshes, an, pairs);
            AdjustPosition(meshes, an, pairs);
            ExpandSegments(meshes, an, pairs);
        }

        static Tuple<CharacterRange, CharacterRange> SectionToCurves(List<PointF> path, CharacterRange sectionRange, int maxPtNum, float maxAngle)
        {
            if (sectionRange.Length <= 0 || path == null || path.Count < 5 || maxPtNum < 3)
                return null;

            float cos = (float)Math.Cos(maxAngle);

            //------------------------------------------------------

            int start1 = sectionRange.First;
            while (start1 < 0)
                start1 += path.Count;
            start1 = start1 % path.Count;

            int end1 = start1 - 2;

            for (int i = 0; i < maxPtNum - 2; i++)
            {
                int idx = (start1 - 2 - i + path.Count) % path.Count;
                var curve = new List<PointF>();
                for (int j = idx; j < idx + 3; j++) 
                    curve.Add(path[Rem(j, path.Count)]);
                if (FMath.GetAngleCos(curve) < cos)
                    break;
                end1 = idx;
            }

            CharacterRange r1 = new CharacterRange();
            if (start1 > end1)
                r1 = new CharacterRange(end1, start1 - end1 + 1);
            if (start1 < end1)
                // 始点と終点をまたがっている場合
                r1 = new CharacterRange(end1,  start1 + path.Count - end1 + 1);
            
            if (r1.Length <= 0)
                return null;

            //------------------------------------------------------

            int start2 = sectionRange.First + sectionRange.Length - 1;
            while (start2 < 0)
                start2 += path.Count;
            start2 = start2 % path.Count;

            int end2 = start2 + 2;
            
            for (int i = 0; i < maxPtNum - 2; i++)
            {
                int idx = (start2 + i) % path.Count;
                var curve = path.Skip(idx).Take(3).ToList();
                if (FMath.GetAngleCos(curve) < cos)
                    break;
                end2 = start2 + i + 2;
            }

            CharacterRange r2 = new CharacterRange();

            if (start2 < end2)
                r2 = new CharacterRange(start2, end2 - start2 + 1);
            if (start2 > end2)
                r2 = new CharacterRange(start2, end2 + path.Count - start2 + 1);

            if (r2.Length <= 0)
                return null;

            return new Tuple<CharacterRange,CharacterRange>(r1, r2);
        }

        static void AdjustRotation(List<SegmentMeshInfo> meshes, SkeletonAnnotation an, List<ConnectPair> pairs)
        {

        }

        static float AdjustScale(List<SegmentMeshInfo> meshes, SkeletonAnnotation an, List<ConnectPair> pairs)
        {
            float delta = 0;

            // ボーンのsrc側のセグメントの接合面の幅にdst側のセグメントの幅を揃える
            foreach (var b in an.bones)
            {
                foreach (var p in pairs)
                {
                    if (p.bone != b)
                        continue;
                    var m1 = p.meshInfo1;
                    var m2 = p.meshInfo2;
                    if (m1.arap == null || m2.arap == null)
                        continue;

                    var path1 = m1.arap.GetPath();
                    var path2 = m2.arap.GetPath();
                    
                    var curves1 = SectionToCurves(path1, p.sectionRange1, 5, 30);
                    var curves2 = SectionToCurves(path2, p.sectionRange2, 5, 30);
                    if (curves1 == null || curves2 == null)
                        continue;
                    
                    float width1 = GetSectionWidth(path1, curves1, b);
                    float width2 = GetSectionWidth(path2, curves2, b);

                    // サイズを合わせる
                    if (Math.Abs(width1 - width2) > 1e-4)
                        m2.Scale(width1 / width2, width1 / width2);

                    delta += width1 * width1 / (width2 * width2);
                }
            }

            return delta;
        }

        static float GetSectionWidth(List<PointF> path, Tuple<CharacterRange, CharacterRange> curves, BoneAnnotation b)
        {
            float width = 0;
            int cnt = Math.Min(curves.Item1.Length, curves.Item2.Length);
            for (int i = 0; i < cnt; i++)
            {
                int idx1 = curves.Item1.First + curves.Item1.Length - 1 - i;
                int idx2 = curves.Item2.First + i;
                while (idx1 < 0)
                    idx1 += path.Count;
                while (idx2 < 0)
                    idx2 += path.Count;
                PointF pt1 = path[idx1 % path.Count];
                PointF pt2 = path[idx2 % path.Count];
                PointF pt = new PointF(pt2.X - pt1.X + b.src.position.X, pt2.Y - pt1.Y + b.src.position.Y);
                width += FMath.GetDistanceToLine(pt, b.src.position, b.dst.position, false);
            }
            width /= cnt;
            return width;
        }

        static float GetSectionHeight(List<PointF> path, Tuple<CharacterRange, CharacterRange> curves, BoneAnnotation b)
        {
            if (path == null || curves == null || b == null)
                return 0;

            if (curves.Item1 == null || curves.Item2 == null)
                return 0;

            float height = 0;

            float boneLen = FMath.Distance(b.src.position, b.dst.position);
            if (boneLen <= 1e-4)
                return 0;

            PointF x, y;
            GetCoordinateFromBone(b, out x, out y);

            // 各セグメントのボーンからのズレを求める
            int cnt = Math.Min(curves.Item1.Length, curves.Item2.Length);
            for (int i = 0; i < cnt; i++)
            {
                int idx1 = curves.Item1.First + curves.Item1.Length - 1 - i;
                int idx2 = curves.Item2.First + i;
                while (idx1 < 0)
                    idx1 += path.Count;
                while (idx2 < 0)
                    idx2 += path.Count;
                PointF pt1 = path[idx1 % path.Count];
                PointF pt2 = path[idx2 % path.Count];
                pt1.X -= b.src.position.X;
                pt1.Y -= b.src.position.Y;
                pt2.X -= b.src.position.X;
                pt2.Y -= b.src.position.Y;
                height += pt1.X * y.X + pt1.Y * y.Y;
                height += pt2.X * y.X + pt2.Y * y.Y;
            }

            height /= 2 * cnt;

            return height;
        }

        static void GetCoordinateFromBone(BoneAnnotation b, out PointF x, out PointF y)
        {
            x = new PointF(1, 0);
            y = new PointF(0, 1);

            float boneLen = FMath.Distance(b.src.position, b.dst.position);
            if (boneLen <= 1e-4)
                return;

            float dx = (b.dst.position.X - b.src.position.X) / boneLen;
            float dy = (b.dst.position.Y - b.src.position.Y) / boneLen;

            x = new PointF(dx, dy);
            y = new PointF(dy, -dx);
        }


        private static void AdjustPosition(List<SegmentMeshInfo> meshes, SkeletonAnnotation an, List<ConnectPair> pairs)
        {
            foreach (var b in an.bones)
            {
                foreach (var p in pairs)
                {
                    if (p.bone != b)
                        continue;
                    var m1 = p.meshInfo1;
                    var m2 = p.meshInfo2;
                    if (m1.arap == null || m2.arap == null)
                        continue;
                    var path1 = m1.arap.GetPath();
                    var path2 = m2.arap.GetPath();
                    var curves1 = SectionToCurves(path1, p.sectionRange1, 5, 30);
                    var curves2 = SectionToCurves(path2, p.sectionRange2, 5, 30);
                    if (curves1 == null || curves2 == null)
                        continue;

                    float height1 = GetSectionHeight(path1, curves1, b);
                    float height2 = GetSectionHeight(path2, curves2, b);

                    PointF x, y;
                    GetCoordinateFromBone(b, out x, out y);

                    // サイズを合わせる
                    if (Math.Abs(height1 - height2) > 1e-4)
                        m2.Translate(y.X * (height1 - height2), y.Y * (height1 - height2));
                }
            }
        }

        static int Rem(int idx, int n)
        {
            if (n <= 0)
                return idx;
            while (idx < 0)
                idx += n;
            return idx % n;
        }

        // 接合面に近づく向きに曲線の点をついかする
        static List<List<PointF>> GetSortedCurves(List<PointF> path, Tuple<CharacterRange, CharacterRange> ranges, BoneAnnotation baseBone)
        {
            List<List<PointF>> curves = new List<List<PointF>>();

            foreach (var range in new[] { ranges.Item1, ranges.Item2 })
            {
                var ls = new List<PointF>();
                for (int i = range.First; i < range.First + range.Length; i++)
                    ls.Add(path[Rem(i, path.Count)]);
                curves.Add(ls);
            }

            // 向きを揃える
            curves[1].Reverse();
            
            // ボーンに対する位置関係を揃える
            PointF pt = path[Rem(ranges.Item2.First, path.Count)];
            if (FMath.GetSide(pt, baseBone.src.position, baseBone.dst.position) < 0)
            {
                var ls = curves[0];
                curves[0] = curves[1];
                curves[1] = ls;
            }

            return curves;
        }

        private static void ExpandSegments(List<SegmentMeshInfo> meshes, SkeletonAnnotation an, List<ConnectPair> pairs)
        {
            int _cnt = 0;
            foreach (var m in meshes)
            {
                if (m.arap == null)
                    continue;
                m.arap.controlPoints.Clear();
                SegmentMeshInfo.SetPathControlPoints(m.arap);
                m.arap.BeginDeformation();
                m.arap.ToBitmap().Save("../../../Test2/" + (_cnt++) + ".png");
            }

            foreach (var b in an.bones)
            {
                foreach (var p in pairs)
                {
                    if (p.bone != b)
                        continue;
                    
                    var m1 = p.meshInfo1;
                    var m2 = p.meshInfo2;
                    if (m1.arap == null || m2.arap == null)
                        continue;
                    
                    var path1 = m1.arap.GetPath();
                    var path2 = m2.arap.GetPath();
                    var ranges1 = SectionToCurves(path1, p.sectionRange1, 5, 30);
                    var ranges2 = SectionToCurves(path2, p.sectionRange2, 5, 30);
                    if (ranges1 == null || ranges2 == null)
                        continue;
                    
                    var curves1 = GetSortedCurves(path1, ranges1, b);
                    var curves2 = GetSortedCurves(path2, ranges2, b);
                    if (curves1.Count != 2 || curves2.Count != 2)
                        continue;

                    List<Tuple<PointF, PointF>> move1 = new List<Tuple<PointF, PointF>>();
                    List<Tuple<PointF, PointF>> move2 = new List<Tuple<PointF, PointF>>();
                    
                    for (int i = 0; i < 2; i++)
                    {
                        var p1 = curves1[i].First();
                        var v1 = new PointF(p1.X - curves1[i].Last().X, p1.Y - curves1[i].Last().Y);
                        var p2 = curves2[i].First();
                        var v2 = new PointF(curves2[i].Last().X - p2.X, curves2[i].Last().Y - p2.Y);
                        
                        // ２点かぶらせる
                        int cnt = curves1[i].Count + curves2[i].Count - 2;
                        if (cnt <= 1)
                            continue;

                        for (int j = 0; j < curves1[i].Count; j++)
                        {
                            PointF to = FMath.HelmitteInterporate(p1, v1, p2, v2, (float)j / (cnt - 1));
                            if (j == curves1[i].Count - 1)
                                move1.Add(new Tuple<PointF,PointF>(curves1[i][j], to));
                            m1.arap.TranslateControlPoint(curves1[i][j], to, false);
                        }
                        for (int j = 0; j < curves2[i].Count; j++)
                        {
                            PointF to = FMath.HelmitteInterporate(p1, v1, p2, v2, (float)(-j + cnt - 1) / (cnt - 1));
                            if (j == curves2[i].Count - 1)
                                move2.Add(new Tuple<PointF, PointF>(curves2[i][j], to));
                            m2.arap.TranslateControlPoint(curves2[i][j], to, false);
                        }
                    }

                    List<PointF> sections1 = new List<PointF>();
                    List<PointF> sections2 = new List<PointF>();
                    
                    for (int i = p.sectionRange1.First + 1; i < p.sectionRange1.First + p.sectionRange1.Length - 1; i++)
                        sections1.Add(path1[Rem(i, path1.Count)]);
                    for (int i = p.sectionRange2.First + 1; i < p.sectionRange2.First + p.sectionRange2.Length - 1; i++)
                        sections2.Add(path2[Rem(i, path2.Count)]);

                    List<PointF> newSection1 = ARAPDeformation.Deform(sections1, move1);
                    List<PointF> newSection2 = ARAPDeformation.Deform(sections2, move2);

                    if (newSection1.Count == sections1.Count)
                    {
                        for (int i = 0; i < newSection1.Count; i++)
                            m1.arap.TranslateControlPoint(sections1[i], newSection1[i], false);
                    }
                    if (newSection2.Count == sections2.Count)
                    {
                        for (int i = 0; i < newSection2.Count; i++)
                            m2.arap.TranslateControlPoint(sections2[i], newSection2[i], false);
                    }

                    m1.arap.FlushDefomation();
                    m2.arap.FlushDefomation();
                }
            }

            foreach (var m in meshes)
            {
                if (m.arap == null)
                    continue;
                m.arap.EndDeformation();
            }
        }

    }
}
