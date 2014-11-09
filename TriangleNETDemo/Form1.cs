using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TriangleNETDemo
{
    public partial class Form1 : Form
    {
        List<PointF> path = new List<PointF>();
        Pen pen = new Pen(Brushes.Black);
        List<FLib.FTriangle> tris;

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            PointF offset = new PointF(150, 150);
            foreach (var t in tris)
                e.Graphics.DrawLines(pen, new [] { t.p1, t.p2, t.p3, t.p1 }.Select(pt => new PointF(pt.X + offset.X, pt.Y + offset.Y)).ToArray());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            const int num = 200;
            for (int i = 0; i < num; i++)
            {
                double angle = 2 * Math.PI * i / num;
                path.Add(new PointF(100 * (float)Math.Cos(angle), 50 * (float)Math.Sin(angle)));
            }
            tris = FLib.TriangleNET.Triangulate(path);
        }

        private void openOToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
