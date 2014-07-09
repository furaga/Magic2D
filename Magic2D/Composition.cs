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
    public class Composition : IDisposable
    {
        public enum OperationType
        {
            Skeleton,
            Segment,
            ControlPoint,
        }
        
        Dictionary<string, Segment> segmentDict = new Dictionary<string, Segment>();
        Dictionary<string, ComposedUnit> unitDict = new Dictionary<string, ComposedUnit>();
        public Dictionary<string, Bitmap> segmentImageDict = new Dictionary<string, Bitmap>();

        public Matrix transform { get; private set; }
        public Bitmap referenceImage { get; private set; }
        public ComposedUnit editingUnit { get; private set; }
        public SkeletonAnnotation an { get; private set; }

        float scale = 1;

        PointF prevPoint = Point.Empty;

        public JointAnnotation editingJoint { get; private set; }
        public JointAnnotation nearestJoint { get; private set; }
        public Segment editingSegment { get; private set; }
        public PointF? editingControlPoint { get; private set; }
        public PointF? nearestControlPoint { get; private set; }

        public Composition()
        {
            transform = new Matrix();
            editingUnit = new ComposedUnit();
            an = new SkeletonAnnotation(null);
        }

        public Composition(string refSkeletonPath)
        {
            transform = new Matrix();
            editingUnit = new ComposedUnit();
            an = SkeletonAnnotation.Load(refSkeletonPath, null);
            if (an == null)
                an = new SkeletonAnnotation(null);
        }

        public void UpdateSegmentDict(Segmentation segmentation, bool reflesh)
        {
            segmentDict.Clear();
            foreach (var kv in segmentation.segmentRootDict)
            {
                foreach (var seg in kv.Value.segments)
                {
                    if (seg.bmp != null)
                    {
                        segmentDict[kv.Key + "." + seg.name] = seg;
                        segmentImageDict[kv.Key + "." + seg.name] = seg.bmp;
                    }
                }
            }
        }

        public void Dispose()
        {
            if (referenceImage != null)
            {
                referenceImage.Dispose();
                referenceImage = null;
            }
            transform = new Matrix();

            if (editingUnit != null && !unitDict.Values.Contains(editingUnit))
                editingUnit.Dispose();
            editingUnit = null;

            foreach (var unit in unitDict.Values)
                unit.Dispose();

            unitDict.Clear();
        }

        // CompositionCanvasControlを引数に持つのはMVCに反する・・・
        // でも、画面上のオブジェクトとの当たり判定に必要
        public Operation OnMouseDown(MouseButtons button, PointF point, OperationType op, CompositionCanvasControl canvas)
        {
            if (button == MouseButtons.Right)
            {
                prevPoint = point;
       //         return null;
            }

            editingJoint = null;
     //       editingSegment = null;
     //       editingControlPoint = null;

            switch (op)
            {
                case OperationType.Skeleton:
                    if (canvas == null || canvas.IsDisposed)
                        break;
                    editingJoint = GetNearestJoint(an, point, 20, canvas);

                    if (editingJoint == null && nearestJoint == null)
                    {
                        if (button == MouseButtons.Left)
                            prevPoint = point;
                    }
                    break;
                case OperationType.Segment:
                    if (editingUnit == null)
                        break;
                    editingSegment = GetSegment(editingUnit.segments, editingUnit.transformDict, canvas.PointToWorld(point));
                    if (button == MouseButtons.Left)
                        prevPoint = point;
                    break;
                case OperationType.ControlPoint:
                    if (button != MouseButtons.Left)
                        break;
                    editingControlPoint = null;
                    var transform = GetTransform(editingSegment);
                    if (transform == null || transform.arap == null)
                        break;
                    var nearest = GetNearestControlPoint(transform.arap.controlPoints, point, 20, canvas);
                    if (nearest == null)
                    {
                        var pt = canvas.PointToWorld(point);
                        transform.arap.AddControlPoint(pt, transform.Invert(pt));
                        transform.arap.EndDeformation();
                        transform.arap.BeginDeformation();
                    }
                    else
                    {
                        editingControlPoint = nearest;
                        prevPoint = point;
                    }
                    break;
            }

            return null;
        }


        public Operation OnMouseMove(MouseButtons button, PointF point, OperationType op, CompositionCanvasControl canvas)
        {
            if (button == MouseButtons.Right)
            {
                if (!prevPoint.IsEmpty)
                    Pan(point.X - prevPoint.X, point.Y - prevPoint.Y);
                prevPoint = point;
      //          return null;
            }

            nearestJoint = null;

            SegmentMeshInfo transform;

            switch (op)
            {
                case OperationType.Skeleton:
                    if (editingJoint != null)
                    {
                        if (button == MouseButtons.Left)
                            editingJoint.position = canvas.PointToWorld(new Point((int)point.X, (int)point.Y));
                    }
                    else
                    {
                        nearestJoint = GetNearestJoint(an, point, 20, canvas);
                    }

                    if (editingJoint == null && nearestJoint == null)
                    {
                        if (button == MouseButtons.Left)
                        {
                            // 全体を動かす
                            var src = canvas.PointToWorld(new Point((int)prevPoint.X, (int)prevPoint.Y));
                            var dst = canvas.PointToWorld(new Point((int)point.X, (int)point.Y));
                            foreach (var j in an.joints)
                                j.position = new PointF(j.position.X + dst.X - src.X, j.position.Y + dst.Y - src.Y);
                            prevPoint = point;
                        }
                    }

                    // ボーンを動かしたらセグメントも調整
                    if (button == MouseButtons.Left)
                    {
                        if (editingUnit != null)
                        {
                            foreach (var seg in editingUnit.segments)
                            {
                                SkeletonFitting(seg);
                                UpdateSkeletalControlPoints(seg);
                                FlushDeformation(seg);
                                // ConectSegments(seg);
                            }
                        }
                    }

                    break;
                case OperationType.Segment:
                    if (button == MouseButtons.Left)
                    {
                        transform = GetTransform(editingSegment);
                        if (transform == null)
                            break;
                        var src = canvas.PointToWorld(prevPoint);
                        var dst = canvas.PointToWorld(point);
                        transform.Translate(dst.X - src.X, dst.Y - src.Y);
                        prevPoint = point;
                    }
                    break;
                case OperationType.ControlPoint:
                    transform = GetTransform(editingSegment);
                    if (transform == null || transform.arap == null)
                        break;
                    if (button != MouseButtons.Left)
                    {
                        nearestControlPoint = GetNearestControlPoint(transform.arap.controlPoints, point, 20, canvas);
                        break;
                    }
                    nearestControlPoint = null;
                    if (editingControlPoint != null)
                    {
                        var pt = canvas.PointToWorld(point);
                        transform.arap.TranslateControlPoint(editingControlPoint.Value, pt, true);
                        editingControlPoint = pt;
                    }
                    break;
            }

            return null;
        }

        public Operation OnMouseUp(MouseButtons button, Point point, OperationType op, CompositionCanvasControl canvas)
        {
            if (button == MouseButtons.Right)
            {
                if (!prevPoint.IsEmpty)
                    Pan(point.X - prevPoint.X, point.Y - prevPoint.Y);
                prevPoint = point;
     //           return null;
            }

            SegmentMeshInfo transform;

            switch (op)
            {
                case OperationType.Skeleton:
                    if (editingJoint != null)
                        editingJoint.position = canvas.PointToWorld(new Point((int)point.X, (int)point.Y));
                    editingJoint = null;
                    if (editingJoint == null && nearestJoint == null)
                    {
                        if (button == MouseButtons.Left)
                            prevPoint = point;
                    }
                    break;
                case OperationType.Segment:
                    if (button == MouseButtons.Left)
                        prevPoint = point;
                    break;
                case OperationType.ControlPoint:
                    if (button != MouseButtons.Left)
                        break;
                    if (FMath.SqDistance(prevPoint, point) <= 1)
                    {
                        transform = GetTransform(editingSegment);
                        if (transform == null || transform.arap == null)
                            break;
                        // 既存の制御点をクリックしたら消す
                        if (editingControlPoint == null || !editingControlPoint.HasValue)
                            break;
                        transform.arap.RemoveControlPoint(editingControlPoint.Value);
                        transform.arap.EndDeformation();
                        transform.arap.BeginDeformation();
                    }
                    editingControlPoint = null;
                    break;
            }

            return null;
        }
        public Operation CreateEditingUnit()
        {
            editingUnit = new ComposedUnit();
            return null;
        }

        public Operation SetEditingUnit(ComposedUnit unit)
        {
            editingUnit = unit;
            return null;
        }

        public Operation AssignComposedUnit(string key)
        {
            return AssignComposedUnit(key, editingUnit);
        }

        public Operation AssignComposedUnit(string key, ComposedUnit unit)
        {
            if (unit == null)
                return null;

            if (unitDict.ContainsKey(key))
                unitDict[key].Dispose();
            unitDict[key] = unit;
            return null;
        }

        public Operation RemoveComposedUnit(string key)
        {
            if (unitDict.ContainsKey(key))
            {
                unitDict[key].Dispose();
                unitDict.Remove(key);
            }
            return null;
        }

        JointAnnotation GetNearestJoint(SkeletonAnnotation an, PointF point, float threshold, CompositionCanvasControl canvas)
        {
            JointAnnotation nearest = null;
            float minSqDist = threshold * threshold;
            foreach (var joint in an.joints)
            {
                PointF pt = canvas.PointToClient(new Point((int)joint.position.X, (int)joint.position.Y));
                float dx = point.X - pt.X;
                float dy = point.Y - pt.Y;
                float sqDist = dx * dx + dy * dy;
                if (minSqDist > sqDist)
                {
                    nearest = joint;
                    minSqDist = sqDist;
                }
            }
            return nearest;
        }
        
        PointF? GetNearestControlPoint(List<PointF> cpts, PointF point, float threshold, CompositionCanvasControl canvas)
        {
            bool found = false;
            PointF nearest = Point.Empty;
            float minSqDist = threshold * threshold;
            foreach (var cpt in cpts)
            {
                PointF pt = canvas.PointToClient(new Point((int)cpt.X, (int)cpt.Y));
                float dx = point.X - pt.X;
                float dy = point.Y - pt.Y;
                float sqDist = dx * dx + dy * dy;
                if (minSqDist > sqDist)
                {
                    nearest = cpt;
                    minSqDist = sqDist;
                    found = true;
                }
            }
            if (found)
                return nearest;
            else
                return null;
        }

        public SegmentMeshInfo GetTransform(Segment seg)
        {
            if (seg == null)
                return null;
            if (editingUnit == null)
                return null;
            if (!editingUnit.transformDict.ContainsKey(seg.name))
                return null;
            return editingUnit.transformDict[seg.name];
        }
        Segment GetSegment(List<Segment> segments, Dictionary<string, SegmentMeshInfo> transformDict, PointF point)
        {
            for (int i = segments.Count - 1; i >= 0; i--)
            {
                var seg = segments[i];
                if (seg == null || !transformDict.ContainsKey(seg.name))
                    continue;
                var transform = transformDict[seg.name];
                if (transform == null || transform.arap == null)
                    continue;
                if (FMath.IsPointInPolygon(point, transform.arap.GetPath()))
                    return segments[i];
            }
            return null;
        }

        public void RemoveSegment(Segment seg)
        {
            if (editingUnit == null)
                return;
            if (seg == null)
                return;
            for (int i = 0; i < editingUnit.segments.Count; i++)
            {
                if (editingUnit.segments[i].name == seg.name)
                {
                    editingUnit.segments.RemoveAt(i);
                    i--;
                }
            }
        }

        public Operation SetEditingSegment(Segment seg)
        {
            editingSegment = seg;
            return null;
        }

        public Operation AssignSegment(string key, out Segment newSeg)
        {
            newSeg = null;
            if (!segmentDict.ContainsKey(key))
                return null;
            newSeg = editingUnit.AssignSegment(key, segmentDict[key]);
            return null;
        }
        // 座標変換
        public void Pan(float dx, float dy)
        {
            transform.Translate(dx, dy, MatrixOrder.Append);
        }
        public void Zoom(float delta)
        {
            float prev = scale;
            if (prev <= 1e-4)
                return;

            scale += delta;
            scale = FMath.Clamp(scale, 0.1f, 15f);
            float ratio = scale / prev;
            if (Math.Abs(1 - ratio) <= 1e-4)
                return;

            transform.Scale(ratio, ratio, MatrixOrder.Append);
        }
        public void Zoom(float zoom, PointF pan)
        {
            Zoom(zoom);
        }

        //---------------------------------------------------------------

        public Operation Backward(Segment seg)
        {
            if (editingUnit == null)
                return null;

            if (seg == null)
                return null;

            int idx0 = -1;
            for (int i = 0; i < editingUnit.segments.Count; i++)
                if (editingUnit.segments[i].name == seg.name)
                    idx0 = i;
            if (idx0 < 0)
                return null;

            int idx1 = idx0 - 1;
            if (idx1 < 0)
                return null;

            var seg_t = editingUnit.segments[idx0];
            editingUnit.segments[idx0] = editingUnit.segments[idx1];
            editingUnit.segments[idx1] = seg_t;

            // todo
            return null;
        }

        public Operation Forward(Segment seg)
        {
            if (editingUnit == null)
                return null;

            if (seg == null)
                return null;

            int idx0 = -1;
            for (int i = 0; i < editingUnit.segments.Count; i++)
                if (editingUnit.segments[i].name == seg.name)
                    idx0 = i;
            if (idx0 < 0)
                return null;

            int idx1 = idx0 + 1;
            if (idx1 >= editingUnit.segments.Count)
                return null;

            var seg_t = editingUnit.segments[idx0];
            editingUnit.segments[idx0] = editingUnit.segments[idx1];
            editingUnit.segments[idx1] = seg_t;

            // todo
            return null;
        }

        public Operation Back(Segment seg)
        {
            if (editingUnit == null)
                return null;
            if (seg == null)
                return null;
            if (!editingUnit.segments.Select(s => s.name).Contains(seg.name))
                return null;
            while (editingUnit.segments[0].name != seg.name)
                Backward(seg);
            return null;
        }

        public Operation Front(Segment seg)
        {
            if (editingUnit == null)
                return null;
            if (seg == null)
                return null;
            if (!editingUnit.segments.Select(s => s.name).Contains(seg.name))
                return null;
            while (editingUnit.segments[editingUnit.segments.Count - 1].name != seg.name)
                Forward(seg);
            return null;
        }

        //---------------------------------------------------------------

        public Operation SetReferenceImage(Bitmap bmp)
        {
            Bitmap prev = referenceImage == null ? null : new Bitmap(referenceImage);
            Bitmap cur = bmp == null ? null : new Bitmap(bmp);
            if (referenceImage != null)
            {
                referenceImage.Dispose();
                referenceImage = null;
            }
            referenceImage = bmp == null ? null : new Bitmap(bmp);
            return new Operation()
            {
                funcName = "SetReferenceImage",
                instance = this,
                parameters = new List<object>() { prev, cur }
            };
        }

        public void undo_SetReferenceImage(Bitmap prev, Bitmap cur)
        {
            SetReferenceImage(prev);
        }

        public void redo_SetReferenceImage(Bitmap prev, Bitmap cur)
        {
            SetReferenceImage(cur);
        }

        public void SkeletonFitting(Segment seg)
        {
            var transform = GetTransform(seg);
            if (transform == null)
                return;
            transform.SkeletonFitting(an);
        }

        private void UpdateSkeletalControlPoints(Segment seg)
        {
            var transform = GetTransform(seg);
            if (transform == null)
                return;
            transform.UpdateSkeletalControlPoints(an);
        }

        private void FlushDeformation(Segment seg)
        {
            var transform = GetTransform(seg);
            if (transform == null || transform.arap == null)
                return;
            transform.arap.FlushDefomation();
        }
    }

    // 一枚絵
    public class ComposedUnit
    {
        public List<Segment> segments = new List<Segment>();
        public Dictionary<string, SegmentMeshInfo> transformDict = new Dictionary<string, SegmentMeshInfo>();

        public ComposedUnit()
        {

        }

        public void Dispose()
        {

        }

        public Segment AssignSegment(string key, Segment seg)
        {
            if (seg == null)
                return null;
            var newSeg = new Segment(seg, key);
            if (segments.Select(s => s.name).Contains(newSeg.name))
                return null;
            segments.Add(newSeg);
            transformDict[key] = new SegmentMeshInfo(seg, true);
            return newSeg;
        }

        public SegmentMeshInfo GetTransform(string key)
        {
            if (transformDict.ContainsKey(key))
                return transformDict[key];
            return null;
        }
    }
    
    // ボーンと切り口の交差の仕方の情報
    public class CrossBoneSection
    {
        public BoneAnnotation bone;
        public CharacterRange sectionRange;
        public int dir; // boneのセグメントからの露出方向。1ならsrc -> dst。-1ならdst -> src
        public CrossBoneSection(BoneAnnotation bone, CharacterRange sectionRange, int dir)
        {
            this.bone = bone;
            this.sectionRange = sectionRange;
            this.dir = dir;
        }
    }

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
        readonly public Dictionary<PointF, List<BoneAnnotation>> controlToBone = new Dictionary<PointF, List<BoneAnnotation>>();

        readonly public Dictionary<BoneAnnotation, CrossBoneSection> crossDict = new Dictionary<BoneAnnotation, CrossBoneSection>();
       
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
                var path = ShiftPath(seg.path, -seg.offset.X, -seg.offset.Y);
                path.RemoveAt(path.Count - 1); // 終点（始点と同じ）は消す

                var partingLine = new List<PointF>();
                if (seg.partingLine == null)
                    partingLine = ShiftPath(seg.partingLine, -seg.offset.X, -seg.offset.Y);
                
                arap = new ARAPDeformation(path, partingLine);

                // 接合面の情報をコピー
                if (seg.section != null || seg.section.Count > 0)
                    sections = FMath.SplitPathRange(seg.section, path, true);
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
                InitializeControlPoints(arap, an, 30, sections, controlToBone);
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

        static void InitializeControlPoints(ARAPDeformation arap, SkeletonAnnotation an, int linearSpan, List<CharacterRange> sections, Dictionary<PointF, List<BoneAnnotation>> controlToBone)
        {
            if (arap == null)
                return;

            controlToBone.Clear();
            arap.controlPoints.Clear();

            // ボーン沿いに制御点を追加
            if (an != null && linearSpan >= 1)
            {
                foreach (var b in an.bones)
                {
                    PointF p0 = b.src.position;
                    PointF p1 = b.dst.position;
                    float dist = FMath.Distance(p0, p1);
                    int ptNum = Math.Max(2, (int)(dist / linearSpan) + 1);
                    for (int i = 0; i < ptNum; i++)
                    {
                        float t = (float)i / (ptNum - 1);
                        PointF p = i == 0 ? p0 : i == ptNum - 1 ? p1 : FMath.Interpolate(p0, p1, t);

                        if (!controlToBone.ContainsKey(p))
                        {
                            controlToBone[p] = new List<BoneAnnotation>() { b };
                            arap.AddControlPoint(p, p);
                        }
                        else
                        {
                            controlToBone[p].Add(b);
                        }
                    }
                }
            }
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

        public void SkeletonFitting(SkeletonAnnotation refSkeleton)
        {
            if (an == null || refSkeleton == null)
                return;
            if (an.bones == null || refSkeleton.bones == null)
                return;

            bool found = false;
            BoneAnnotation from = null, to = null;

            foreach (var br in refSkeleton.bones)
            {
                foreach (var b in an.bones)
                {
                    if (b.src.name == br.src.name && b.dst.name == br.dst.name)
                    {
                        from = b;
                        to = br;
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }

            if (from == null || to == null)
                return;

            if (!reverse)
            {
                double angle1 = Math.Atan2(from.dst.position.Y - from.src.position.Y, from.dst.position.X - from.src.position.X);
                double angle2 = Math.Atan2(to.dst.position.Y - to.src.position.Y, to.dst.position.X - to.src.position.X);
                angle = (float)FMath.ToDegree((float)(angle2 - angle1));
            }
            else
            {
                double angle1 = Math.Atan2(-from.dst.position.Y + from.src.position.Y, from.dst.position.X - from.src.position.X);
                double angle2 = Math.Atan2(to.dst.position.Y - to.src.position.Y, to.dst.position.X - to.src.position.X);
                angle = (float)(angle2 - angle1);
            }
         
            float len1 = FMath.Distance(from.src.position, from.dst.position);
            float len2 = FMath.Distance(to.src.position, to.dst.position);
            scale = new PointF(len2 / len1, len2 / len1);

            position = PointF.Empty;
            var _pts = new[] { from.src.position };
            GetTransform().TransformPoints(_pts);
            position = new PointF(to.src.position.X - _pts[0].X, to.src.position.Y - _pts[0].Y);

            if (an != null)
            {
                _pts = an.joints.Select(j => j.position).ToArray();
                GetTransform().TransformPoints(_pts);
                for (int i = 0; i < an.joints.Count; i++)
                    an.joints[i].position = _pts[i];
            }

            if (arap != null)
                arap.Transform(GetTransform());
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

            PointF[] _pt = new [] { pt};
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

            foreach (var kv in controlToBone)
            {
                var orgPt = kv.Key;
                var bones = kv.Value;
                if (bones.Count <= 0)
                    continue;
                if (!transformDict.ContainsKey(bones[0]))
                    continue;
                var transform = transformDict[bones[0]];
                var pt = arap.OrgToCurControlPoint(orgPt);
                if (pt == null)
                    continue;
                var to = FMath.Transform(pt.Value, transform);
                arap.TranslateControlPoint(pt.Value, to, false);
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
