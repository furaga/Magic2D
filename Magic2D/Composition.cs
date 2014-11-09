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
                    var transform = GetMeshInfo(editingSegment);
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
                                Fitting(seg);
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
                        transform = GetMeshInfo(editingSegment);
                        if (transform == null)
                            break;
                        var src = canvas.PointToWorld(prevPoint);
                        var dst = canvas.PointToWorld(point);
                        transform.Translate(dst.X - src.X, dst.Y - src.Y);
                        prevPoint = point;
                    }
                    break;
                case OperationType.ControlPoint:
                    transform = GetMeshInfo(editingSegment);
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
                        transform = GetMeshInfo(editingSegment);
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

        public SegmentMeshInfo GetMeshInfo(Segment seg)
        {
            if (seg == null)
                return null;
            if (editingUnit == null)
                return null;
            if (!editingUnit.transformDict.ContainsKey(seg.name))
                return null;
            return editingUnit.transformDict[seg.name];
        }

        public void SetMeshInfo(string name, SegmentMeshInfo m)
        {
            if (m == null)
                return;
            if (editingUnit == null)
                return;
            if (!editingUnit.transformDict.ContainsKey(name))
                return;
            editingUnit.transformDict[name] = m;
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
            if (editingUnit == null)
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

        public void Fitting(Segment seg)
        {
            var m = GetMeshInfo(seg);
            if (m == null)
                return;
            SkeletonFitting.Fitting(m, an);
        }

        private void UpdateSkeletalControlPoints(Segment seg)
        {
            var transform = GetMeshInfo(seg);
            if (transform == null)
                return;
            transform.UpdateSkeletalControlPoints(an);
        }

        private void FlushDeformation(Segment seg)
        {
            var transform = GetMeshInfo(seg);
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

}
