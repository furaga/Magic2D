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
    // 各セグメントを指定した骨格'refSkeleton'にフィットするように変形する
    public class SkeletonFitting
    {
        public static void Fitting(SegmentMeshInfo mesh, SkeletonAnnotation refSkeleton)
        {
            mesh.position = Point.Empty;
            mesh.angle = 0;

            if (mesh.arap == null)
                return;

            Dictionary<BoneAnnotation, List<PointF>> sCps = mesh.boneToControls;

            foreach (var kv in sCps)
            {
                BoneAnnotation b = kv.Key;
                List<PointF> orgPts = kv.Value;
                BoneAnnotation br = GetBoneAnnotation(refSkeleton, b);
                if (br == null)
                    continue;
                if (orgPts.Count <= 1)
                    continue;
                for (int i = 0; i < orgPts.Count; i++)
                {
                    float t = (float)i / (orgPts.Count - 1);
                    float x = br.src.position.X * (1 - t) + br.dst.position.X * t;
                    float y = br.src.position.Y * (1 - t) + br.dst.position.Y * t;
                    PointF? pt = mesh.arap.OrgToCurControlPoint(orgPts[i]);
                    if (pt == null || !pt.HasValue)
                        continue;
                    mesh.arap.TranslateControlPoint(pt.Value, new PointF(x, y), false);
                }
            }

            mesh.arap.FlushDefomation();
        }

        static BoneAnnotation GetBoneAnnotation(SkeletonAnnotation an, BoneAnnotation b)
        {
            if (an == null || b == null || an.bones == null)
                return null;
            foreach (var bb in an.bones)
                if (b == bb)
                    return bb;
            return null;
        }
    }
}
