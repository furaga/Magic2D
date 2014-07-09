using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Diagnostics;
using FLib;

namespace FLib
{
    public class TeleRegistration
    {
        public class Pair
        {
            public Segment seg1;
            public Segment seg2;
            public int idx1;
            public int idx2;

            public Pair(Segment seg1, int idx1, Segment seg2, int idx2)
            {
                this.seg1 = seg1;
                this.idx1 = idx1;
                this.seg2 = seg2;
                this.idx2 = idx2;
            }
        }

        public class Segment
        {
            public List<PointF> path = new List<PointF>();
            public List<List<PointF>> curves = new List<List<PointF>>();

            public static Segment FromPathSections(List<PointF> path, List<List<PointF>> sections)
            {
                return null;
            }

            public static Segment FromPathCurves(List<PointF> path, List<List<PointF>> curves)
            {
                return new Segment()
                {
                    path = new List<PointF>(path),
                    curves = new List<List<PointF>>(curves),
                };
            }
        }
        
        List<Segment> segments = new List<Segment>();
        List<Pair> pairs = new List<Pair>();
        Dictionary<Segment, Matrix> transformDict = new Dictionary<Segment, Matrix>();

        public TeleRegistration(List<Segment> segments, List<Pair> pairs)
        {
            if (segments == null)
                return;

            this.segments = new List<Segment>(segments);
            var transforms = Registrate(this.segments, pairs);

            Debug.Assert(transforms.Count == this.segments.Count);

            for (int i = 0; i < transforms.Count; i++)
                transformDict[segments[i]] = transforms[i];
        }

        // セグメントのリストと包括線の対応関係から、自然につながるための各セグメントの移動・拡大・回転を求める
        public static List<Matrix> Registrate(List<Segment> segments, List<Pair> pairs)
        {
            // エネルギー最小化式を解く


            return new List<Matrix>();
        }
    }

}
