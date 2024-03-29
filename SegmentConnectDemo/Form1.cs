﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace SegmentConnectDemo
{
    public partial class FormSCD : Form
    {
        Magic2D.Segmentation segmentation = new Magic2D.Segmentation();
        Magic2D.Composition composition = new Magic2D.Composition();
        int dummy_idx = 0;

        List<Magic2D.Segment> inputs = new List<Magic2D.Segment>();

        List<Magic2D.Segment> outputs = new List<Magic2D.Segment>();
        Dictionary<Magic2D.Segment, Magic2D.SegmentMeshInfo> outmeshes = new Dictionary<Magic2D.Segment,Magic2D.SegmentMeshInfo>();
        Dictionary<Magic2D.Segment, Bitmap> outBmps = new Dictionary<Magic2D.Segment, Bitmap>();

        Magic2D.SkeletonAnnotation an;

        public FormSCD()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string dir = "../../../Test/";
            if (!Directory.Exists(dir))
            {
                MessageBox.Show("not foud: " + dir);
                Application.Exit();
            }

            // セグメントを読み込み
            Magic2D.Form1.OpenSegmentation(dir, "3_segmentation", segmentation, null, ref dummy_idx);

            // リストを作成
            composition.UpdateSegmentDict(segmentation, true);
            while (true)
            {
                if (!composition.segmentImageDict.Any(kv => kv.Key.EndsWith(".Full")))
                    break;
                var _kv = composition.segmentImageDict.First(kv => kv.Key.EndsWith(".Full"));
                composition.segmentImageDict.Remove(_kv.Key);
            }
            Magic2D.Form1.UpdateImageView(composition.segmentImageDict, imageList1, listView1, true);

            an = Magic2D.SkeletonAnnotation.Load("./refSkeleton.skl");

            inputs.Clear();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (listView1.SelectedItems.Count != 1)
                    return;
                string key = listView1.SelectedItems[0].Text;
                
                if (! composition.segmentImageDict.ContainsKey(key))
                    return;
                
                Magic2D.Segment seg;
                composition.AssignSegment(listView1.SelectedItems[0].Text, out seg);

                if (seg == null)
                    return;

                inputs.Add(new Magic2D.Segment(seg));

                outputs = Connect(inputs);

                pictureBox1.Invalidate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex + ex.StackTrace);
            }
        }

        List<Magic2D.Segment> Connect(List<Magic2D.Segment> segments)
        {
            List<Magic2D.SegmentMeshInfo> meshes = new List<Magic2D.SegmentMeshInfo>();
            foreach (var seg in segments)
                meshes.Add(new Magic2D.SegmentMeshInfo(seg, true));

            var connector = new Magic2D.SegmentConnector(meshes, an, null);

            for (int i = 0; i < connector.meshes.Count; i++)
            {
                outmeshes[segments[i]] = connector.meshes[i];
                if (connector.meshes[i].arap != null)
                {
                    var bmp = connector.meshes[i].arap.ToBitmap();
                    outBmps[segments[i]] = connector.meshes[i].arap.ToBitmap(segments[i].bmp);
                }
            }

            return segments;
        }

        Pen pen = new Pen(Brushes.Red, 1) { CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(4, 4) };
        Pen pen2 = new Pen(Brushes.Blue, 1) { CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(4, 4) };

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            foreach (var b in an.bones)
                e.Graphics.DrawLine(pen2, b.src.position, b.dst.position);
            
            for (int i = 0; i < outputs.Count; i++)
            {
                if (outputs[i].bmp == null)
                    continue;
                var seg = outputs[i];
//                e.Graphics.DrawImage(seg.bmp, seg.offset);
                if (seg.an == null || seg.an.bones == null)
                    continue;

                if (outBmps.ContainsKey(seg))
                    e.Graphics.DrawImage(outBmps[seg], Point.Empty);
//                foreach (var b in seg.an.bones)
  //                  e.Graphics.DrawLine(pen, b.src.position, b.dst.position);
            }
        }

        private void clearCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            composition.editingUnit.segments.Clear();
            inputs.Clear();
            outputs.Clear();
            outmeshes.Clear();
            outBmps.Clear();

            pictureBox1.Invalidate();
        }

        private void runRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            outputs = Connect(inputs);
            pictureBox1.Invalidate();
        }

        Magic2D.JointAnnotation movingJoint = null;

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (an == null)
                return;
            
            try
            {
                if (movingJoint == null)
                {
                    float minPt = an.joints.Min(j => FLib.FMath.Distance(j.position, e.Location));
                    movingJoint = an.joints.First(j => FLib.FMath.Distance(j.position, e.Location) == minPt);
                    pictureBox1.Invalidate();
                }
                else
                {
                    movingJoint = null;
                }
            }
            catch (Exception)
            {

            }
            
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (movingJoint != null)
            {
                movingJoint.position = e.Location;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}
