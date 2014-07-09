using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using FLib;
namespace Magic2D
{
    public partial class FormRefSkeleton : Form
    {
        Pen bonePen = new Pen(Brushes.DarkBlue, 2)
        {
            CustomEndCap = new AdjustableArrowCap(8, 8)
        };
        Pen editingBonePen = new Pen(Brushes.Red, 2)
        {
            CustomEndCap = new AdjustableArrowCap(8, 8)
        };
        Pen selectBonePen = new Pen(Brushes.Yellow, 2)
        {
            CustomEndCap = new AdjustableArrowCap(8, 8)
        };

        public SkeletonAnnotation an;
        JointAnnotation selectJoint = null;
        JointAnnotation nearestJoint = null;
        Microsoft.Xna.Framework.Input.KeyboardState prevState;
        Matrix transform = new Matrix();

        public void Initialize(Bitmap bmp)
        {
            an = new SkeletonAnnotation(bmp);
            selectJoint = null;
            nearestJoint = null;
            transform = new Matrix();
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            Microsoft.Xna.Framework.Input.KeyboardState newState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            if (Form1.IsDown(newState, prevState, Microsoft.Xna.Framework.Input.Keys.Delete))
            {
                DeleteJointAnnotation(selectJoint);
                SelectJointAnnotation(null);
                pictureBox1.Invalidate();
            }
            prevState = newState;
        }

        private void FormRefSkeleton_Load(object sender, EventArgs e)
        {
            Application.Idle += new EventHandler(Application_Idle);
        }

        void AddJointAnnotation(JointAnnotation joint)
        {
            an.joints.Add(joint);
        }

        void DeleteJointAnnotation(JointAnnotation joint)
        {
            if (an.joints.Contains(joint))
                an.joints.Remove(joint);
        }

        void SelectJointAnnotation(JointAnnotation joint)
        {
            if (an.joints.Contains(joint))
                selectJoint = joint;
            else
                selectJoint = null;
        }

        public FormRefSkeleton()
        {
            InitializeComponent();
            Initialize(new Bitmap(pictureBox1.Image as Bitmap));
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (addRadioButton.Checked)
            {
                AddJointAnnotation(new JointAnnotation(jointNameTextBox.Text, Form1.InvertCoordinate(e.Location, transform)));
                UpdateTreeView();
            }
            if (selectRadioButton.Checked)
            {
                SelectJointAnnotation(an.GetNearestJoint(Form1.InvertCoordinate(e.Location, transform), 20, new Matrix()));
                if (selectJoint != null)
                    UpdateJointNameTextBox(selectJoint.name);
            }
            if (an != null)
                pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectRadioButton.Checked)
                nearestJoint = an.GetNearestJoint(Form1.InvertCoordinate(e.Location, transform), 20, new Matrix());
            if (an != null)
                pictureBox1.Invalidate();
        }

        private void jointNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (selectJoint != null)
            {
                selectJoint.name = jointNameTextBox.Text;
                UpdateTreeView();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (an == null)
                return;
            if (an.bmp == null)
                return;

            transform = new Matrix();
            var fitSize = BitmapHandler.GetFittingSize(an.bmp, pictureBox1.Width, pictureBox1.Height);
            transform.Scale(fitSize.Width / an.bmp.Width, fitSize.Height / an.bmp.Height);

            e.Graphics.Transform = transform;

            e.Graphics.Clear(Color.White);

            // bmp
            e.Graphics.DrawImage(an.bmp, Point.Empty);
            // joints
            foreach (JointAnnotation joint in an.joints)
                e.Graphics.FillEllipse(Brushes.Orange, new RectangleF(joint.position.X - 5, joint.position.Y - 5, 10, 10));
            // nearestJoint
            if (nearestJoint != null)
                e.Graphics.FillEllipse(Brushes.Red, new RectangleF(nearestJoint.position.X - 5, nearestJoint.position.Y - 5, 10, 10));
            // selectJoint
            if (selectJoint != null)
                e.Graphics.FillEllipse(Brushes.Yellow, new RectangleF(selectJoint.position.X - 5, selectJoint.position.Y - 5, 10, 10));
        }

        void UpdateTreeView()
        {
            for (int i = 0; i < an.joints.Count; i++)
            {
                if (treeView1.Nodes.Count <= i)
                    treeView1.Nodes.Add(an.joints[i].name);
                if (treeView1.Nodes[i].Text != an.joints[i].name)
                    treeView1.Nodes[i].Text = an.joints[i].name;
            }
            while (treeView1.Nodes.Count > an.joints.Count)
                treeView1.Nodes.RemoveAt(an.joints.Count);
        }

        private void UpdateJointNameTextBox(string p)
        {
            jointNameTextBox.Text = p;
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                SelectJointAnnotation(an.joints[treeView1.SelectedNode.Index]);
                if (selectJoint != null)
                {
                    pictureBox1.Invalidate();
                    UpdateJointNameTextBox(selectJoint.name);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Open();
        }

        void Save()
        {
            try
            {
                saveFileDialog1.RestoreDirectory = true;
                saveFileDialog1.Filter = "Template of skeleton (*.skl)|*.skl";
                if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Save(saveFileDialog1.FileName);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + e.StackTrace);
            }
        }

        void Save(string filepath)
        {
            if (an == null)
                return;
            filepath = Path.GetFullPath(filepath);
            string[] lines = an.joints.Select(j => j.name + ":" + j.position.X + "," + j.position.Y).ToArray();
            File.WriteAllLines(filepath, lines);
        }

        void Open()
        {
            try
            {
                openFileDialog1.RestoreDirectory = true;
                openFileDialog1.Filter = "Template of skeleton (*.skl)|*.skl";
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Open(openFileDialog1.FileName);
                }
                pictureBox1.Invalidate();
                UpdateTreeView();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + e.StackTrace);
            }
        }

        public void Open(string filepath)
        {
            an = SkeletonAnnotation.Load(filepath, an.bmp);
        }
    }
}