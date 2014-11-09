using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Magic2DTest
{
    public class Util
    {
        public static List<PointF> circlePoints(int cx, int cy, int r, int num)
        {
            List<PointF> pts = new List<PointF>();
            for (int i = 0; i < num; i++)
            {
                double rad = 2 * Math.PI * i / num;
                pts.Add(new PointF(cx + r * (float)Math.Cos(rad), cy + r * (float)Math.Sin(rad)));
            }
            return pts;
        }
        
    }
}
