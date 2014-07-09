using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DelaunayTriangle
{
    public partial class Form1 : Form
    {
        FLib.DelaunayTriangle dt;

        List<PointF> pt = new List<PointF>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          button1_Click(null, null);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            foreach (var t in dt.triangleList)
                t.Draw(e.Graphics);

            foreach (var p in pt)
                e.Graphics.FillEllipse(Brushes.Yellow, new RectangleF(p.X - 2, p.Y - 2, 4, 4));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int cnt;
            if (!int.TryParse(textBox1.Text, out cnt))
                cnt = 100;

            pt.Clear();
            var rand = new Random();
            for (int i = 0; i < cnt; i++)
            {
                int x = rand.Next(0, pictureBox1.Width);
                int y = rand.Next(0, pictureBox1.Height);
                pt.Add(new PointF(x, y));
            }

            System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            dt = new FLib.DelaunayTriangle(pt, new RectangleF(0, 0, pictureBox1.Width, pictureBox1.Height));

            Console.WriteLine("cnt = " + cnt +  ", elapsed time = " + sw.ElapsedMilliseconds + "ms");

            pictureBox1.Invalidate();
        }
    }
}
