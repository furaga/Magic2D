using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using FLib;

namespace Magic2D
{
    // セグメントの幾何情報
    // 大域的な位置や角度、ARAPのメッシュ情報'arap'、接合面や骨格情報などをもつ
    public class SegmentMeshInfo
    {
        public PointF position = PointF.Empty;
        public float angle = 0;
        public PointF scale = new PointF(1, 1);
        public bool reverse = false;

        // 元セグメントから得られる情報
        readonly public ARAPDeformation arap;
        readonly public List<CharacterRange> sections = new List<CharacterRange>();
        readonly public SkeletonAnnotation an;
//        readonly public Dictionary<PointF, List<BoneAnnotation>> controlToBone = new Dictionary<PointF, List<BoneAnnotation>>();
        readonly public Dictionary<BoneAnnotation, List<PointF>> boneToControls = new Dictionary<BoneAnnotation, List<PointF>>();

        readonly public Dictionary<BoneAnnotation, CrossBoneSection> crossDict = new Dictionary<BoneAnnotation, CrossBoneSection>();
        private List<SegmentMeshInfo> list;

        public List<PointF> GetPath()
        {
            if (arap == null)
                return null;
            return arap.GetPath();
        }

public SegmentMeshInfo(Segment seg, bool initControlPoints)
        {
            if (seg == null)
                return;

            if (seg.path != null && seg.path.Count > 3)
            {
                // メッシュを生成
                var shiftPath = ShiftPath(seg.path, -seg.offset.X, -seg.offset.Y);
                var sdPath = PathSubdivision.Subdivide(shiftPath, 10);
                sdPath.RemoveAt(sdPath.Count - 1); // 終点（始点と同じ）は消す

                var partingLine = new List<PointF>();
                if (seg.partingLine != null)
                    partingLine = ShiftPath(seg.partingLine, -seg.offset.X, -seg.offset.Y);

                arap = new ARAPDeformation(sdPath, partingLine);

                // 接合面の情報をコピー
                if (seg.section != null || seg.section.Count > 0)
                {
                    var _sections = FMath.SplitPathRange(ShiftPath(seg.section, -seg.offset.X, -seg.offset.Y), shiftPath, true);
                    sections = new List<CharacterRange>();
                    foreach (var r in _sections)
                    {
                        int i1 = r.First;
                        int i2 = r.First + r.Length - 1;
                        try
                        {
                            int j1 = sdPath.IndexOf(shiftPath[i1]);
                            int j2 = sdPath.IndexOf(shiftPath[i2]);
                            sections.Add(new CharacterRange(j1, j2 - j1 + 1));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e + e.StackTrace);
                        }
                    }
                }
            }

            if (seg.an != null)
            {
                // スケルトンをコピー
                an = new SkeletonAnnotation(seg.an, false);
                foreach (var j in an.joints)
                    j.position = new PointF(j.position.X - seg.offset.X, j.position.Y - seg.offset.Y);
            }

            // 接合面とボーンの交差情報
            if (sections != null && sections.Count >= 1 && seg.an != null && seg.an.bones != null)
                crossDict = GetBoneSectionCrossDict(GetPath(), sections, an);

            if (initControlPoints && arap != null)
            {
                // 制御点を初期
                InitializeControlPoints(arap, an, 30, sections, boneToControls);
                arap.BeginDeformation();
            }
        }

        public SegmentMeshInfo(List<PointF> path, List<PointF> partingLine, List<PointF> section, SkeletonAnnotation an, bool initControlPoints) :
            this(
                new Segment("_dummy", null)
                {
                    path = path.Concat(new[] { path[0] }).ToList(), // 終点を追加する
                    partingLine = partingLine,
                    section = section,
                    an = an
                },
                initControlPoints)
        {
        }

        public SegmentMeshInfo(SegmentMeshInfo m)
        {
            position = m.position;
            angle = m.angle;
            scale = m.scale;
            reverse = m.reverse;

            arap = new ARAPDeformation(m.arap);

            sections = new List<CharacterRange>(m.sections);

            an = new SkeletonAnnotation(m.an, true);

            boneToControls = new Dictionary<BoneAnnotation, List<PointF>>(m.boneToControls);
            crossDict = new Dictionary<BoneAnnotation, CrossBoneSection>(m.crossDict);
        }

        public SegmentMeshInfo(List<SegmentMeshInfo> list, SkeletonAnnotation an)
        {
            position = Point.Empty;
            angle = 0;
            scale = Point.Empty;
            reverse = false;

            arap = null; // TODO  ARAPDeformation.Combine();

            Dictionary<BoneAnnotation, CrossBoneSection> crossDict = new Dictionary<BoneAnnotation,CrossBoneSection>();
            foreach (var m in list)
                foreach (var kv in m.crossDict)
                {
                    if (!crossDict.ContainsKey(kv.Key))
                        crossDict[kv.Key] = kv.Value;
                    else
                        crossDict.Remove(kv.Key);
                }

            sections = crossDict.Values.Select(c => c.sectionRange).ToList();

            this.an = new SkeletonAnnotation(an, true);
            boneToControls = new Dictionary<BoneAnnotation, List<PointF>>();
        }

        // 接合面とボーンの交差判定
        // ボーンのひとつの端点は内で逆の端点は外
        static Dictionary<BoneAnnotation, CrossBoneSection> GetBoneSectionCrossDict(List<PointF> path, List<CharacterRange> sections, SkeletonAnnotation an)
        {
            var crossDict = new Dictionary<BoneAnnotation, CrossBoneSection>();

            if (path == null || path.Count <= 0 || an == null || an.bones == null)
                return crossDict;

            foreach (var section in sections)
            {
                var pts = new List<PointF>();
                for (int i = section.First; i < section.First + section.Length; i++)
                    pts.Add(path[i % path.Count]);

                foreach (var b in an.bones)
                {
                    if (FMath.IsCrossed(b.src.position, b.dst.position, pts))
                    {
                        bool srcIn = FMath.IsPointInPolygon(b.src.position, path);
                        bool dstIn = FMath.IsPointInPolygon(b.dst.position, path);
                        if (srcIn && !dstIn)
                            crossDict[b] = new CrossBoneSection(b, section, 1);
                        if (!srcIn && dstIn)
                            crossDict[b] = new CrossBoneSection(b, section, -1);
                    }
                }
            }

            return crossDict;
        }

        static List<PointF> ShiftPath(List<PointF> path, float offsetx, float offsety)
        {
            if (path == null)
                return new List<PointF>();
            List<PointF> _path = new List<PointF>();
            for (int i = 0; i < path.Count; i++)
                _path.Add(new PointF(path[i].X + offsetx, path[i].Y + offsety));
            return _path;
        }

        // pathはrefPath上の点集合。ひとつづきの点集合ごとに分割する
        // たとえば
        // path: p1, p2, p3, p4
        // refPath: p1, p3, p0, p2, p4
        // のとき{ { { p1, p3 }, { p2, p4 } }を返す
        public static List<List<PointF>> SplitPath(List<PointF> path, List<PointF> refPath)
        {
            List<List<PointF>> ans = new List<List<PointF>>();

            if (path == null || refPath == null)
                return ans;

            List<PointF> ls = new List<PointF>();
            foreach (var pt in refPath)
            {
                if (path.Contains(pt))
                {
                    ls.Add(pt);
                }
                else if (ls.Count >= 1)
                {
                    ans.Add(ls);
                    ls = new List<PointF>();
                }
            }
            return ans;
        }

        static void InitializeControlPoints(ARAPDeformation arap, SkeletonAnnotation an, int linearSpan, List<CharacterRange> sections, Dictionary<BoneAnnotation, List<PointF>> boneToControls)
        {
            SetSkeletalControlPoints(arap, an, linearSpan, boneToControls);
        }

        public static void SetSkeletalControlPoints(ARAPDeformation arap, SkeletonAnnotation an, int linearSpan, Dictionary<BoneAnnotation, List<PointF>> boneToControls)
        {
            if (arap == null)
                return;

            boneToControls.Clear();
            arap.ClearControlPoints();

            // ボーン沿いに制御点を追加
            HashSet<PointF> pts = new HashSet<PointF>();
            if (an != null && linearSpan >= 1)
            {
                foreach (var b in an.bones)
                {
                    PointF p0 = b.src.position;
                    PointF p1 = b.dst.position;
                    float dist = FMath.Distance(p0, p1);
                    int ptNum = Math.Max(2, (int)(dist / linearSpan) + 1);

                    boneToControls[b] = new List<PointF>();

                    for (int i = 0; i < ptNum; i++)
                    {
                        float t = (float)i / (ptNum - 1);
                        PointF p = i == 0 ? p0 : i == ptNum - 1 ? p1 : FMath.Interpolate(p0, p1, t);
                        if (!pts.Contains(p))
                            arap.AddControlPoint(p, p);
                        boneToControls[b].Add(p);
                        pts.Add(p);
                    }
                }
            }
        }

        public static void SetPathControlPoints(ARAPDeformation arap)
        {
            if (arap == null)
                return;

            arap.ClearControlPoints();

            var path = arap.GetPath();
            if (path == null)
                return;

            for (int i = 0; i < path.Count; i++)
                arap.AddControlPoint(arap.meshPointList[i], arap.orgMeshPointList[i]);
        }

        static BoneAnnotation GetCrossingBoneWithPath(SkeletonAnnotation an, List<PointF> section)
        {
            if (an == null || an.bones == null)
                return null;
            if (section == null)
                return null;

            foreach (var b in an.bones)
            {
                PointF p0 = b.src.position;
                PointF p1 = b.dst.position;
                for (int i = 0; i < section.Count - 1; i++)
                {
                    if (FMath.IsCrossed(p0, p1, section[i], section[i + 1]))
                        return b;
                }
            }

            return null;
        }
        
        public void Translate(float x, float y)
        {
            if (arap != null)
                arap.Translate(x, y);
            position.X += x;
            position.Y += y;
        }

        public void MoveTo(float x, float y)
        {
            if (arap != null)
                arap.Translate(x - position.X, y - position.Y);
            position = new PointF(x, y);
        }

        public void Rotate(float deg)
        {
            if (arap != null)
            {
                Matrix transform = new Matrix();
                transform.RotateAt(deg - angle, position, MatrixOrder.Append);
                arap.Transform(transform);
            }
            angle = deg;
        }

        public void Scale(float x, float y)
        {
            if (x <= 0 || y <= 0)
                return;
            if (scale.X <= 0 || scale.Y <= 0)
                return;
            if (arap != null)
            {
                Matrix transform = new Matrix();
                transform.Translate(-position.X, -position.Y, MatrixOrder.Append);
                transform.Rotate(-angle, MatrixOrder.Append);
                transform.Scale(x / scale.X, y / scale.Y, MatrixOrder.Append);
                transform.Rotate(angle, MatrixOrder.Append);
                transform.Translate(position.X, position.Y, MatrixOrder.Append);
                arap.Transform(transform);
            }
            scale = new PointF(x, y);
        }

        public void ReverseX(bool reverse)
        {
            if (this.reverse == reverse)
                return;

            if (arap != null)
            {
                Matrix transform = new Matrix();
                transform.Translate(-position.X, -position.Y, MatrixOrder.Append);
                transform.Rotate(-angle, MatrixOrder.Append);
                transform.Scale(-1, 1, MatrixOrder.Append);
                transform.Rotate(angle, MatrixOrder.Append);
                transform.Translate(position.X, position.Y, MatrixOrder.Append);
                arap.Transform(transform);
                this.reverse = reverse;
            }
        }

        public Matrix GetTransform()
        {
            Matrix transform = new Matrix();
            if (scale.X <= 0 || scale.Y <= 0)
                return transform;

            if (reverse)
                transform.Scale(-1, 1, MatrixOrder.Append);
            transform.Scale(scale.X, scale.Y, MatrixOrder.Append);
            transform.Rotate(angle, MatrixOrder.Append);
            transform.Translate(position.X, position.Y, MatrixOrder.Append);

            return transform;
        }

        public PointF Invert(PointF pt)
        {
            Matrix transform = new Matrix();
            if (scale.X <= 0 || scale.Y <= 0)
                return pt;
            transform.Translate(-position.X, -position.Y, MatrixOrder.Append);
            transform.Rotate(-angle, MatrixOrder.Append);
            transform.Scale(1 / scale.X, 1 / scale.Y, MatrixOrder.Append);
            if (reverse)
                transform.Scale(-1, 1, MatrixOrder.Append);

            PointF[] _pt = new[] { pt };
            transform.TransformPoints(_pt);
            return _pt[0];
        }

        public void UpdateSkeletalControlPoints(SkeletonAnnotation refAnnotation)
        {
            if (an == null)
                return;

            if (arap == null)
                return;

            var orgAn = new SkeletonAnnotation(an, false);

            foreach (var j in an.joints)
            {
                foreach (var jr in refAnnotation.joints)
                {
                    if (j.name == jr.name)
                    {
                        j.position = jr.position;
                        break;
                    }
                }
            }

            var transformDict = GetSkeletalTransforms(an, orgAn);

            foreach (var kv in boneToControls)
            {
                if (kv.Value == null || kv.Value.Count <= 0)
                    continue;
                var bone = kv.Key;
                foreach (var orgPt in kv.Value)
                {
                    if (!transformDict.ContainsKey(bone))
                        continue;
                    var transform = transformDict[bone];
                    var pt = arap.OrgToCurControlPoint(orgPt);
                    if (pt == null)
                        continue;
                    var to = FMath.Transform(pt.Value, transform);
                    arap.TranslateControlPoint(pt.Value, to, false);
                }
            }
        }

        static Dictionary<BoneAnnotation, Matrix> GetSkeletalTransforms(SkeletonAnnotation an, SkeletonAnnotation orgAn)
        {
            Dictionary<BoneAnnotation, Matrix> transformDict = new Dictionary<BoneAnnotation, Matrix>();

            for (int i = 0; i < an.bones.Count; i++)
            {
                var b = an.bones[i];
                var ob = orgAn.bones[i];

                float angle1 = (float)Math.Atan2(ob.dst.position.Y - ob.src.position.Y, ob.dst.position.X - ob.src.position.X);
                float angle2 = (float)Math.Atan2(b.dst.position.Y - b.src.position.Y, b.dst.position.X - b.src.position.X);
                angle1 = FMath.ToDegree(angle1);
                angle2 = FMath.ToDegree(angle2);

                float len1 = FMath.Distance(ob.src.position, ob.dst.position);
                float len2 = FMath.Distance(b.src.position, b.dst.position);

                if (len1 <= 1e-4)
                {
                    transformDict[b] = new Matrix();
                    continue;
                }

                Matrix transform = new Matrix();
                transform.Translate(-ob.src.position.X, -ob.src.position.Y, MatrixOrder.Append);
                transform.Rotate(-angle1, MatrixOrder.Append);
                transform.Scale(len2 / len1, len2 / len1, MatrixOrder.Append);
                transform.Rotate(angle2, MatrixOrder.Append);
                transform.Translate(b.src.position.X, b.src.position.Y, MatrixOrder.Append);

                transformDict[b] = transform;
            }

            return transformDict;
        }
    }
}
