using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Spline
{
    public partial class Form1 : Form
    {
        List<PointF> spline = new List<PointF>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PointF p1 = PointF.Empty;
            PointF v1 = new PointF(300, 100);
            PointF p2 = new PointF(300, 300);
            PointF v2 = new PointF(300, 400);
            for (int i = 0; i < 100; i++)
                spline.Add(HelmitteInterporate(p1, v1, p2, v2, i * 0.01f));
        }

        PointF HelmitteInterporate(PointF p1, PointF v1, PointF p2, PointF v2, float t)
        {
            PointF[] p = new[] { p1, v1, p2, v2 };

            float k = t;
            float[] s = new[] { k * k * k, k * k, k };
            float[] mx = new[] { p[0].X, p[1].X, p[2].X, p[3].X };
            float[] my = new[] { p[0].Y, p[1].Y, p[2].Y, p[3].Y };

            float x = ((2 * mx[0]) + (mx[1]) - (2 * mx[2]) + (mx[3])) * s[0] +
              ((-3 * mx[0]) - (2 * mx[1]) + (3 * mx[2]) - (mx[3])) * s[1] +
              (mx[1]) * s[2] + (mx[0]);
            float y = ((2 * my[0]) + (my[1]) - (2 * my[2]) + (my[3])) * s[0] +
              ((-3 * my[0]) - (2 * my[1]) + (3 * my[2]) - (my[3])) * s[1] +
              (my[1]) * s[2] + (my[0]);

            return new PointF(x, y);
        }


        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.DrawLines(new Pen(Brushes.Red), spline.ToArray());
        }
    }
}
