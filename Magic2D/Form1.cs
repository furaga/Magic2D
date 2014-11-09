using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FLib;
namespace Magic2D
{
    public partial class Form1 : Form
    {
        Microsoft.Xna.Framework.Input.KeyboardState prevState;
        bool onCtrl = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Application.Idle += new EventHandler(Application_Idle);

            MouseWheel += Form1_MouseWheel;

            cellListView.AllowDrop = true;

            Initialize();

            // for debug
            OpenProject("../../../Test/dummyProject.m2d");
            refSkeletonAnnotation = OpenRefSkeleton("../../../Test/refSkeleton.skl", refSkeletonCanvas.Image as Bitmap);


            RefleshAllImageView();
            segmentListView.Nodes.Clear();
        }

        // tood
        void Form1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (tabControl.SelectedTab.Name == "tabSkeletonFitting")
            {
                skeletonFittingCanvasZoom(e.Delta * 0.001f, skeletonFittingCanvas.PointToClient(this.PointToScreen(e.Location)));
                skeletonFittingCanvas.Invalidate();
            }
            if (tabControl.SelectedTab.Name == "tabSegmentation")
            {
                segmentation.Zoom(e.Delta * 0.001f, segmentCanvas.PointToClient(this.PointToScreen(e.Location)));
                segmentCanvas.Invalidate();
            }
            if (tabControl.SelectedTab.Name == "tabComposition")
            {
                composition.Zoom(e.Delta * 0.001f, segmentCanvas.PointToClient(this.PointToScreen(e.Location)));
                compositionCanvas.Invalidate();
            }
        }


        private void Application_Idle(object sender, EventArgs e)
        {
            Microsoft.Xna.Framework.Input.KeyboardState newState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
            onCtrl = newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl) ||
                        newState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightControl);

            if (formRefSkeleton != null)
            {
                prevState = newState;
                return;
            }

            // Undo
            if (onCtrl && IsDown(newState, prevState, Microsoft.Xna.Framework.Input.Keys.Z))
            {
                Undo();
                if (tabControl.SelectedTab.Name == "tabSourceImages")
                    RefleshAllImageView();
                if (tabControl.SelectedTab.Name == "tabSkeletonFitting")
                {
                    skeletonFittingCanvas.Invalidate();
                    refSkeletonCanvas.Invalidate();
                }
                if (tabControl.SelectedTab.Name == "tabAnimation")
                {
                    seekbar.Maximum = animeCells.Sum(c => c.durationMilliseconds) / 10;
                    cellListView.Invalidate();
                }
                if (tabControl.SelectedTab.Name == "tabSegmentation")
                    segmentCanvas.Invalidate();
            }
            // Redo
            if (onCtrl && IsDown(newState, prevState, Microsoft.Xna.Framework.Input.Keys.Y))
            {
                Redo();
                if (tabControl.SelectedTab.Name == "tabSourceImages")
                    RefleshAllImageView();
                if (tabControl.SelectedTab.Name == "tabSkeletonFitting")
                {
                    skeletonFittingCanvas.Invalidate();
                    refSkeletonCanvas.Invalidate();
                }
                if (tabControl.SelectedTab.Name == "tabAnimation")
                {
                    seekbar.Maximum = animeCells.Sum(c => c.durationMilliseconds) / 10;
                    cellListView.Invalidate();
                }
                if (tabControl.SelectedTab.Name == "tabSegmentation")
                    segmentCanvas.Invalidate();
            }

            // Tab:SkeletonFitting
            if (IsDown(newState, prevState, Microsoft.Xna.Framework.Input.Keys.Delete) && tabControl.SelectedTab.Name == "tabSkeletonFitting")
            {
                if (!jointNameTextBox.Focused)
                {
                    var an = GetEditingAnnotation();
                    DeleteJointAnnotation(an, selectJoint);
                    DeleteBoneAnnotation(an, selectBone);
                    SelectJointAnnotation(an, null);
                    SelectBoneAnnotation(an, null);
                    skeletonFittingCanvas.Invalidate();
                }
            }


            // Tab:Composition
            if (IsDown(newState, prevState, Microsoft.Xna.Framework.Input.Keys.Delete) && tabControl.SelectedTab.Name == "tabComposition")
            {
                if (composition != null)
                {
                    composition.RemoveSegment(composition.editingSegment);
                    composition.SetEditingSegment(null);
                    compositionCanvas.Invalidate();
                }
            }

            // Tab:Animation
            if (tabControl.SelectedTab.Name == "tabAnimation")
            {
                if (IsDown(newState, prevState, Microsoft.Xna.Framework.Input.Keys.Delete))
                {
                    DeleteAnimeCells(selectCells);
                    selectCells.Clear();

                    seekbar.Maximum = animeCells.Sum(c => c.durationMilliseconds) / 10;
                    cellListView.Invalidate();
                }
            }

            prevState = newState;
        }

        public static bool IsDown(Microsoft.Xna.Framework.Input.KeyboardState newState, Microsoft.Xna.Framework.Input.KeyboardState prevState, Microsoft.Xna.Framework.Input.Keys key)
        {
            return newState.IsKeyDown(key) && prevState.IsKeyUp(key);
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            MoveSourceImageView(tabControl.SelectedTab.Name);
            UpdateSegmentImageView(tabControl.SelectedTab.Name);
        }

        //-------------------------------------------------------------------
        // Tab: Source Images
        //-------------------------------------------------------------------

        private void buttonScreenshot_Click(object sender, EventArgs e)
        {
            TakeScreenshotOfSourceImage();
            UpdateSourceImageView(false);
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            LoadSourceImage();
            UpdateSourceImageView(false);
        }

        private void sourceImageView_KeyDown(object sender, KeyEventArgs e)
        {
            //
            if (e.KeyCode == Keys.Delete)
            {
                if (sourceImageView.SelectedItems.Count <= 0)
                    return;
                if (tabControl.SelectedTab.Name != "tabSourceImages")
                    return;
                for (int i = sourceImageView.SelectedItems.Count - 1; i >= 0; i--)
                {
                    var item = sourceImageView.SelectedItems[i];
                    DeleteImage(sourceImageDict, item.ImageKey);
                }
                UpdateSourceImageView(true);
            }


            // 全選択
            if (onCtrl && e.KeyCode == Keys.A)
                AllSelectImageView(sourceImageView);
        }

        private void sourceImageView_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.LControlKey ||
                e.KeyCode == Keys.ControlKey ||
                e.KeyCode == Keys.LControlKey ||
                e.KeyCode == Keys.RControlKey)
                onCtrl = false;
        }

        private void sourceImageView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sourceImageView.SelectedItems.Count >= 1)
            {
                if (tabControl.SelectedTab.Name == "tabSourceImages" || tabControl.SelectedTab.Name == "tabSkeletonFitting")
                {
                    SetEditingAnnotation(sourceImageView.SelectedItems[0].ImageKey);
                    ShowSkeletonFittingTab(sourceImageView.SelectedItems[0].ImageKey);
                }

                if (tabControl.SelectedTab.Name == "tabSegmentation")
                {
                    string key = sourceImageView.SelectedItems[0].ImageKey;
                    if (!segmentation.segmentRootDict.ContainsKey(key))
                        segmentation.AssignSegmentRootAs(key, sourceImageDict[key], null);
                    segmentation.SetEditingSegmentRoot(key);
                    if (skeletonAnnotationDict.ContainsKey(key))
                        segmentation.GetEditingSegmentRoot().an = skeletonAnnotationDict[key];
                    segmentation.CreateEditingSegment();

                    segmentSegmentButton.Checked = true;

                    InitVisibleSegmentKeys(segmentation.GetEditingSegmentRoot());
                    UpdateSegmentListView();
                    segmentCanvas.Invalidate();
                }
            }
        }

        private void skeletonSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sourceImageView.SelectedItems.Count >= 1)
            {
                SetEditingAnnotation(sourceImageView.SelectedItems[0].ImageKey);
                ShowSkeletonFittingTab(sourceImageView.SelectedItems[0].ImageKey);
            }
        }

        private void segmentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sourceImageView.SelectedItems.Count >= 1)
            {
                // 
                ShowSegmentationTab(sourceImageView.SelectedItems[0].ImageKey);
            }
        }

        private void menuSourceImages_Opening(object sender, CancelEventArgs e)
        {
            e.Cancel = sourceImageView.SelectedItems.Count <= 0;
        }

        //---------------------------------------------------------------------

        private void openOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProject();
            RefleshAllImageView();
        }

        private void saveSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject();
            RefleshAllImageView();
        }

        private void skeletonFittingCanvas_Paint(object sender, PaintEventArgs e)
        {
            DrawSkeletonFittingCanvas(e.Graphics, segmentCanvas.Width, segmentCanvas.Height, jointNameTextBox.Text);
        }

        private void skeletonFittingCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            SkeletonAnnotation an = GetEditingAnnotation();
            if (an == null)
                return;

            PointF convLocation = InvertCoordinate(e.Location, skeletonFittingCanvasTransform);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                // joint
                if (jointAddRadioButton.Checked)
                    AssignJointAnnotation(an, new JointAnnotation(jointNameTextBox.Text, convLocation));

                if (jointSelectRadioButton.Checked)
                {
                    JointAnnotation joint = an.GetNearestJoint(e.Location, 20, skeletonFittingCanvasTransform);
                    SelectJointAnnotation(an, joint);
                    if (joint != null)
                        jointNameTextBox.Text = joint.name;
                }

                // bone
                if (boneAddRadioButton.Checked)
                {
                    var joint = an.GetNearestJoint(e.Location, 20, skeletonFittingCanvasTransform);
                    if (joint != null)
                        CreateOrCompleteEditingBone(an, joint);
                    else
                        DeleteEditingBone();
                }

                if (boneSelectRadioButton.Checked)
                    SelectBoneAnnotation(an, an.GetNearestBone(e.Location, 20, skeletonFittingCanvasTransform));

                skeletonFittingCanvas.Invalidate();
            }

            // 画像をずらす
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (tabControl.SelectedTab.Name == "tabSkeletonFitting")
                    skeletonFittingCanvasPrevMousePos = e.Location;
            }
        }

        PointF skeletonFittingCanvasPrevMousePos = new PointF();

        private void skeletonFittingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            SkeletonAnnotation an = GetEditingAnnotation();
            if (an == null)
                return;

            if (jointSelectRadioButton.Checked || jointAddRadioButton.Checked || boneAddRadioButton.Checked)
                nearestJoint = an.GetNearestJoint(e.Location, 20, skeletonFittingCanvasTransform);
            if (boneSelectRadioButton.Checked || boneAddRadioButton.Checked)
                nearestBone = an.GetNearestBone(e.Location, 20, skeletonFittingCanvasTransform);

            PointF convLocation = InvertCoordinate(e.Location, skeletonFittingCanvasTransform);
            if (addingBone != null)
                UpdateEditingBone(an, new JointAnnotation("[dummy]", convLocation));

            skeletonFittingCanvas.Invalidate();

            // 画像をずらす
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (tabControl.SelectedTab.Name == "tabSkeletonFitting")
                {
                    PointF pos = e.Location;
                    skeletonFittingCanvasPan(pos.X - skeletonFittingCanvasPrevMousePos.X, pos.Y - skeletonFittingCanvasPrevMousePos.Y);
                    skeletonFittingCanvasPrevMousePos = pos;
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ShowSegmentationTab(editingAnnotationKey);
        }

        private void editRefJointsButton_Click(object sender, EventArgs e)
        {
            formRefSkeleton = new FormRefSkeleton();
            formRefSkeleton.ShowDialog();
            if (formRefSkeleton.an != null && formRefSkeleton.an.bmp != null)
                refSkeletonAnnotation = formRefSkeleton.an;
            formRefSkeleton.Dispose();
            formRefSkeleton = null;
            skeletonFittingCanvas.Invalidate();
            refSkeletonCanvas.Invalidate();
        }

        private void refSkeletonCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            refSkeletonNearestJoint = refSkeletonAnnotation.GetNearestJoint(e.Location, 20, refSkeletonCanvasTransform);
            if (refSkeletonNearestJoint != null)
                jointNameTextBox.Text = refSkeletonNearestJoint.name;
            refSkeletonCanvas.Invalidate();
        }

        private void refSkeletonCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            refSkeletonNearestJoint = refSkeletonAnnotation.GetNearestJoint(e.Location, 20, refSkeletonCanvasTransform);
            refSkeletonCanvas.Invalidate();
        }

        private void refSkeletonCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            if (refSkeletonAnnotation == null)
                return;

            var size = BitmapHandler.GetFittingSize(refSkeletonAnnotation.bmp, refSkeletonCanvas.Width, refSkeletonCanvas.Height);
            refSkeletonCanvasTransform = new Matrix();
            refSkeletonCanvasTransform.Scale(size.Width / refSkeletonAnnotation.bmp.Width, size.Height / refSkeletonAnnotation.bmp.Height);

            e.Graphics.Transform = refSkeletonCanvasTransform;

            e.Graphics.DrawImage(refSkeletonAnnotation.bmp, Point.Empty);

            int radius = 10;
            foreach (JointAnnotation joint in refSkeletonAnnotation.joints)
                e.Graphics.FillEllipse(joint.name == jointNameTextBox.Text ? Brushes.Orange : Brushes.Blue, joint.position.X - radius, joint.position.Y - radius, 2 * radius, 2 * radius);

            if (refSkeletonNearestJoint != null)
                e.Graphics.FillEllipse(Brushes.Red, refSkeletonNearestJoint.position.X - radius, refSkeletonNearestJoint.position.Y - radius, 2 * radius, 2 * radius);
        }

        private void jointNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (selectJoint != null)
                selectJoint.name = jointNameTextBox.Text;

            skeletonFittingCanvas.Invalidate();
            refSkeletonCanvas.Invalidate();
        }

        private void skeletonFittingCanvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                SkeletonAnnotation an = GetEditingAnnotation();
                if (jointSelectRadioButton.Checked || jointAddRadioButton.Checked)
                    DeleteJointAnnotation(an, an.GetNearestJoint(e.Location, 20, skeletonFittingCanvasTransform));
                if (boneSelectRadioButton.Checked || boneAddRadioButton.Checked)
                    DeleteBoneAnnotation(an, an.GetNearestBone(e.Location, 20, skeletonFittingCanvasTransform));
            }
        }

        //-------------------------------------------------

        private void composedImageView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            composedImageView.DoDragDrop(e.Item as ListViewItem, DragDropEffects.Move);
        }

        private void composedImageView_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void cellListView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
            List<string> imageKeys = new List<string>();
            var item = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
            if (item != null)
                imageKeys.Add(item.ImageKey);
            SetAddingCells(imageKeys);

            cellListView.Invalidate();
        }

        private void cellListView_DragOver(object sender, DragEventArgs e)
        {
            Point pt = cellListView.PointToClient(new Point(e.X, e.Y));
            int idx = GetPrevCellFromPoint(animeCells, pt.X, pt.Y, cellListView.Width, cellListView.Height);
            if (idx >= -1)
                prevCell = idx < 0 ? null : animeCells[idx];
            cellListView.Invalidate();
        }

        private void cellListView_DragDrop(object sender, DragEventArgs e)
        {
            // リストビューからドラッグアンドドロップされた
            if (e.Data.GetData(typeof(ListViewItem)) != null)
            {
                Point pt = cellListView.PointToClient(new Point(e.X, e.Y));
                int idx = GetPrevCellFromPoint(animeCells, pt.X, pt.Y, cellListView.Width, cellListView.Height);
                if (idx >= -1)
                {
                    prevCell = idx < 0 ? null : animeCells[idx];
                    AssignAnimeCells(addingCells, prevCell);
                    selectCells.Clear();
                    selectCells.AddRange(addingCells);
                    if (selectCells.Count >= 0)
                        animeDurationTextBox.Text = selectCells[0].durationMilliseconds + "";
                    seekbar.Maximum = animeCells.Sum(c => c.durationMilliseconds) / 10;
                    cellListView.Invalidate();
                }
            }

            // picturebox内でドラッグアンドドロップされた
            if (e.Data.GetData(typeof(List<AnimeCell>)) != null)
            {
                Point pt = cellListView.PointToClient(new Point(e.X, e.Y));
                int idx = GetPrevCellFromPoint(animeCells, pt.X, pt.Y, cellListView.Width, cellListView.Height);
                if (idx >= -1)
                {
                    prevCell = idx < 0 ? null : animeCells[idx];

                    // セルを移動する
                    addingCells = e.Data.GetData(typeof(List<AnimeCell>)) as List<AnimeCell>;
                    if (!addingCells.Contains(prevCell))
                    {
                        DeleteAnimeCells(addingCells);
                        AssignAnimeCells(addingCells, prevCell);
                    }

                    selectCells.Clear();
                    selectCells.AddRange(addingCells);
                    if (selectCells.Count >= 0)
                        animeDurationTextBox.Text = selectCells[0].durationMilliseconds + "";

                    seekbar.Maximum = animeCells.Sum(c => c.durationMilliseconds) / 10;
                    cellListView.Invalidate();
                }
            }
        }

        private void cellListView_MouseDown(object sender, MouseEventArgs e)
        {
            // セルの選択
            int idx = GetPrevCellFromPoint(animeCells, e.X, e.Y, cellListView.Width, cellListView.Height);
            idx += 1;
            selectCells.Clear();
            if (idx >= 0)
            {
                if (idx < animeCells.Count)
                {
                    selectCells.Add(animeCells[idx]);
                    if (selectCells.Count >= 0)
                        animeDurationTextBox.Text = selectCells[0].durationMilliseconds + "";
                }
                if (idx - 1 < animeCells.Count)
                    prevCell = idx - 1 < 0 ? null : animeCells[idx - 1];
            }
            cellListView.Invalidate();

            cellListView.DoDragDrop(new List<AnimeCell>(selectCells), DragDropEffects.Move);
        }
        private void cellListView_Paint(object sender, PaintEventArgs e)
        {
            DrawAnimeCells(e.Graphics, composedImageDict, animeCells, addingCells, prevCell, selectCells, cellListView.Width, cellListView.Height);
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (!playing)
            {
                PlayAnimation(animeTimer);
                playButton.Text = "□";
            }
            else
            {
                StopAnimation(animeTimer);
                playButton.Text = "▷";
            }
        }

        void animeTimer_Tick(object sender, EventArgs e)
        {
            playTime += playStopwatch.ElapsedMilliseconds;
            if (loopCheckBox.Checked)
            {
                playTime = playTime % (seekbar.Maximum * 10);
                playStopwatch.Restart();
            }
            else
            {
                if (playTime >= seekbar.Maximum * 10)
                {
                    StopAnimation(animeTimer);
                    playButton.Text = "▷";
                }
                else
                {
                    playStopwatch.Restart();
                }
            }
            seekbar.Scroll -= seekbar_Scroll;
            seekbar.Value = Math.Min(seekbar.Maximum, (int)playTime / 10);
            seekbar.Scroll += seekbar_Scroll;

            animeView.Invalidate();
        }

        private void seekbar_Scroll(object sender, EventArgs e)
        {
            playTime = seekbar.Value * 10;
            animeView.Invalidate();
        }

        private void animeView_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            float elapsedTime = 0;
            int i = -1;
            for (i = 0; i < animeCells.Count; i++)
            {
                elapsedTime += animeCells[i].durationMilliseconds;
                if (playTime <= elapsedTime)
                    break;
            }

            if (i < 0 || animeCells.Count <= i)
                return;

            AnimeCell cell = animeCells[i];
            if (!composedImageDict.ContainsKey(cell.key))
                return;

            Bitmap bmp = composedImageDict[cell.key];
            SizeF size = BitmapHandler.GetFittingSize(bmp, animeView.Width, animeView.Height);
            float ox = (animeView.Width - size.Width) * 0.5f;
            float oy = (animeView.Height - size.Height) * 0.5f;
            e.Graphics.DrawImage(bmp, ox, oy, size.Width, size.Height);
        }


        private void animeDurationTextBox_TextChanged(object sender, EventArgs e)
        {
            if (selectCells != null && selectCells.Count >= 1)
            {
                string text = animeDurationTextBox.Text;
                int duration;
                if (int.TryParse(text, out duration))
                {
                    foreach (var cell in selectCells)
                        cell.durationMilliseconds = duration;
                }
            }
        }


        //--------------------------------------------------------------------
        // tab:segmentation
        //--------------------------------------------------------------------

        Point mouseDownPoint = Point.Empty;

        private void segmentCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            segmentCanvas.Focus();

            mouseDownPoint = e.Location;
            if (segmentation == null)
                return;
            Operation op = segmentation.OnMouseDown(e.Button, e.Location, GetSegmentOperation());
            if (op != null)
                AddOperation(op);
            segmentCanvas.Invalidate();
        }

        private void segmentCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (segmentation == null)
                return;
            Operation op = segmentation.OnMouseMove(e.Button, e.Location, GetSegmentOperation());
            if (op != null)
                AddOperation(op);
            segmentCanvas.Invalidate();
        }

        private void segmentCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int dx = mouseDownPoint.X - e.X;
                int dy = mouseDownPoint.Y - e.Y;
                int len = dx * dx + dy * dy;
                if (len <= 1)
                {
                    segmentContextMenuStrip.Show(segmentCanvas.PointToScreen(e.Location));
                    return;
                }
            }

            if (segmentation == null)
                return;
            Operation op = segmentation.OnMouseUp(e.Button, e.Location, GetSegmentOperation());
            if (op != null)
                AddOperation(op);
            segmentCanvas.Invalidate();
        }

        SegmentOperation GetSegmentOperation()
        {
            if (segmentSegmentButton.Checked)
                return SegmentOperation.Segment;
            if (segmentSkeletonButton.Checked)
                return SegmentOperation.SkeletonAnnotation;
            if (segmentSectionButton.Checked)
                return SegmentOperation.Section;
            if (segmentPartingButton.Checked)
                return SegmentOperation.PartingLine;
            return SegmentOperation.Segment;
        }


        private void segmentCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Transform = segmentation.transform;
            e.Graphics.Clear(Color.White);

            // パーツを出力
            if (segmentation == null)
                return;

            var root = segmentation.GetEditingSegmentRoot();
            if (root == null)
                return;

            // 各セグメント画像の描画
            for (int i = 0; i < segmentListView.Nodes.Count; i++)
            {
                if (!visibleSegmentKeys.Contains(segmentListView.Nodes[i].Text))
                    continue;
                string name = segmentListView.Nodes[i].Text;
                if (!root.segments.Any(s => s.name == name))
                    continue;

                Segment seg = root.segments.First(s => s.name == name);
                if (seg.bmp != null)
                    e.Graphics.DrawImage(seg.bmp, seg.offset);
            }

            if (root.an != null && visibleSkeletonKeys.Contains("Full"))
            {
                foreach (var b in root.an.bones)
                    e.Graphics.DrawLine(bonePen, b.src.position, b.dst.position);
            }

            Segment editSeg = segmentation.GetEditingSegment();
            if (editSeg == null)
                return;

            // 選択中のセグメントの骨格
            if (editSeg.an != null)
            {
                if (visibleSkeletonKeys.Contains(editSeg.name))
                {
                    foreach (var b in editSeg.an.bones)
                        e.Graphics.DrawLine(bonePen1, b.src.position, b.dst.position);
                }
            }

            // 選択中のセグメントの境界線
            if (visiblePathKeys.Contains(editSeg.name))
            {
                editSeg.DrawPath(e.Graphics, pathPen, closedPathPen);
                for (int j = 0; j < editSeg.path.Count; j++)
                {
                    if (editSeg.Closed && j == editSeg.path.Count - 1)
                        break;
                    PointF pt = editSeg.path[j];
                    e.Graphics.FillRectangle(closedPathBrush, pt.X - 2, pt.Y - 2, 4, 4);
                }
            }


            // 選択中のセグメントの分割線
            if (editSeg.partingLine != null)
            {
                if (visiblePartingLineKeys.Contains(editSeg.name))
                {
                    if (editSeg.partingLine.Count >= 2)
                        e.Graphics.DrawCurve(partingLinePen, editSeg.partingLine.ToArray());
                    foreach (var pt in editSeg.partingLine)
                        e.Graphics.FillRectangle(partingLineBrush, pt.X - 2, pt.Y - 2, 4, 4);
                }
            }

            // 選択中のセグメントの断面
            if (editSeg.section != null)
            {
                if (visibleSectionKeys.Contains(editSeg.name))
                {
                    foreach (var pt in editSeg.section)
                        e.Graphics.FillRectangle(sectionBrush, pt.X - 2, pt.Y - 2, 4, 4);
                }
            }

            // フォーカスがあたっているボーンもしくは点をハイライト
            if (editSeg.nearestPoint != null)
                e.Graphics.FillRectangle(Brushes.Red, editSeg.nearestPoint.X - 2, editSeg.nearestPoint.Y - 2, 4, 4);
            if (editSeg.nearestBone != null)
                e.Graphics.DrawLine(editingBonePen, editSeg.nearestBone.src.position, editSeg.nearestBone.dst.position);
        }

        private void AddSegmentButton_Click(object sender, EventArgs e)
        {
            var root = segmentation.GetEditingSegmentRoot();
            if (root == null)
                return;
            root.AssignNewPathSegment(GetNewKey("segment", root.segments.Select(s => s.name).ToList()));
            UpdateSegmentListView();
        }

        private void segmentListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var root = segmentation.GetEditingSegmentRoot();
                if (root == null)
                    return;
                if (segmentListView.SelectedNode == null)
                    return;
                root.DeleteSegment(segmentListView.SelectedNode.Text);
                UpdateSegmentListView();
            }
        }

        private void segmentListView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent == null)
            {
                if (e.Node.Checked)
                    visibleSegmentKeys.Add(e.Node.Text);
                else
                    visibleSegmentKeys.Remove(e.Node.Text);
            }

            var root = e.Node;
            while (root.Parent != null)
                root = root.Parent;

            if (root.Text == "Full")
                segmentation.CreateEditingSegment();
            else
                segmentation.SetEditingSegment(root.Text);

            switch (e.Node.Text)
            {
                case "Segment":
                    if (e.Node.Checked)
                        visiblePathKeys.Add(root.Text);
                    else
                        visiblePathKeys.Remove(root.Text);
                    break;
                case "Section":
                    if (e.Node.Checked)
                        visibleSectionKeys.Add(root.Text);
                    else
                        visibleSectionKeys.Remove(root.Text);
                    break;
                case "Skeleton":
                    if (e.Node.Checked)
                        visibleSkeletonKeys.Add(root.Text);
                    else
                        visibleSkeletonKeys.Remove(root.Text);
                    break;
                case "Parting lines":
                    if (e.Node.Checked)
                        visiblePartingLineKeys.Add(root.Text);
                    else
                        visiblePartingLineKeys.Remove(root.Text);
                    break;
            }

            segmentCanvas.Invalidate();
        }

        private void segmentListView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            while (node.Parent != null)
                node = node.Parent;
            if (node.Text == "Full")
                segmentation.CreateEditingSegment();
            else
                segmentation.SetEditingSegment(node.Text);
            segmentCanvas.Invalidate();
        }

        private void segmentSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var root = GetEditingSegmentRoot();
            var seg = GetEditingSegment();
            seg.UpdateBitmap(root);

            if (root != null && seg != null)
            {
                string key = GetNewKey(segmentNameTextBox.Text, root.segments.Select(s => s.name).ToList());
                AddVisibleSegmentListViews(key);
                segmentation.AssignEditingSegmentAs(key);
            }
            UpdateSegmentListView();
            segmentCanvas.Invalidate();
        }

        private void deleteLastPointDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var seg = GetEditingSegment();
            if (seg == null)
                return;
            if (seg is PathSegment)
            {
                if (seg.path.Count >= 1)
                {
                    var op = (seg as PathSegment).DeletePathPoint(seg.path.Last(), new Matrix());
                    AddOperation(op);
                }
            }
        }

        private void newSegmentNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (segmentation == null)
                return;
            segmentation.ResetEditingSegment();
            segmentSegmentButton.Checked = true;
        }

        SegmentRoot GetEditingSegmentRoot()
        {
            if (segmentation == null)
                return null;
            return segmentation.GetEditingSegmentRoot();
        }

        Segment GetEditingSegment()
        {
            if (segmentation == null)
                return null;
            return segmentation.GetEditingSegment();
        }

        private void clearPathCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var seg = GetEditingSegment();
            if (seg == null)
                return;
            if (seg is PathSegment)
            {
                var pseg = seg as PathSegment;
                var op = seg.Clear();
                AddOperation(op);
            }
        }

        private void segmentNameTextBox_TextChanged(object sender, EventArgs e)
        {
            var seg = GetEditingSegment();
            if (seg == null)
                return;
            seg.name = segmentNameTextBox.Text;
            UpdateSegmentListView();
        }

        //--------------------------------------------------------------------

        private void LoadReferenceImageButton_Click(object sender, EventArgs e)
        {
            referenceImageOpenFileDialog.RestoreDirectory = true;
            referenceImageOpenFileDialog.Filter = "*.png,*.bmp,*.jpg|*.png;*.bmp;*.jpg";
            if (referenceImageOpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var filename = referenceImageOpenFileDialog.FileName;
                using (var _bmp = new Bitmap(filename))
                {
                    Bitmap bmp = new Bitmap(_bmp);
                    string key = System.IO.Path.GetFileNameWithoutExtension(filename);
                    AddOperation(composition.SetReferenceImage(bmp));
                }
            }

            referenceImageView.Invalidate();
            compositionCanvas.Invalidate();
        }

        private void referenceImageView_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            if (composition.referenceImage != null)
            {
                SizeF size = BitmapHandler.GetFittingSize(composition.referenceImage, referenceImageView.Width, referenceImageView.Height);
                float ox = -0.5f * (size.Width - referenceImageView.Width);
                float oy = -0.5f * (size.Height - referenceImageView.Height);
                e.Graphics.DrawImage(composition.referenceImage, ox, oy, size.Width, size.Height);
            }
        }
        
        private void segmentImageView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (segmentImageView.SelectedItems.Count >= 1)
            {
                Segment seg;
                composition.AssignSegment(segmentImageView.SelectedItems[0].Text, out seg);
                if (seg != null)
                {
                    composition.SetEditingSegment(seg);
                    composition.Fitting(seg);


                    //
                    //
                    //
                    var meshes = composition.editingUnit.segments.Select(s => composition.GetMeshInfo(s)).ToList();
                    var connector = new SegmentConnector(meshes, composition.an, null);
                    for (int i = 0; i < connector.meshes.Count; i++)
                        composition.SetMeshInfo(composition.editingUnit.segments[i].name, connector.meshes[i]);
                }
                segmentMoveButton.Checked = true;
                compositionCanvas.Invalidate();
            }
        }

        private void compositionCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownPoint = e.Location;

            compositionCanvas.Focus();

            if (composition == null)
                return;
            composition.OnMouseDown(e.Button, e.Location, GetCompositionOperationType(), compositionCanvas);
            compositionCanvas.Invalidate();

            var transform = composition.GetMeshInfo(composition.editingSegment);
            if (transform == null)
                return;
            UpdateSegmentTransformView(transform);
        }

        private void compositionCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (composition == null)
                return;

            composition.OnMouseMove(e.Button, e.Location, GetCompositionOperationType(), compositionCanvas);

            compositionCanvas.Invalidate();

            var transform = composition.GetMeshInfo(composition.editingSegment);
            if (transform == null)
                return;
            UpdateSegmentTransformView(transform);
        }
        
        private void compositionCanvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int dx = mouseDownPoint.X - e.X;
                int dy = mouseDownPoint.Y - e.Y;
                int len = dx * dx + dy * dy;
                if (len <= 1)
                {
                    compositionContextMenuStrip.Show(compositionCanvas.PointToScreen(e.Location));
                    return;
                }
            }

            if (composition == null)
                return;
            composition.OnMouseUp(e.Button, e.Location, GetCompositionOperationType(), compositionCanvas);
            compositionCanvas.Invalidate();

            var transform = composition.GetMeshInfo(composition.editingSegment);
            if (transform == null)
                return;
            UpdateSegmentTransformView(transform);
        }

        Composition.OperationType GetCompositionOperationType()
        {
            if (skeletonMoveButton.Checked)
                return Composition.OperationType.Skeleton;
            if (segmentMoveButton.Checked)
                return Composition.OperationType.Segment;
            if (controlPointMoveButton.Checked)
                return Composition.OperationType.ControlPoint;
            return Composition.OperationType.Skeleton;
        }

        private void posTrackbar_Scroll(object sender, EventArgs e)
        {
            if (composition == null)
                return;
            var transform = composition.GetMeshInfo(composition.editingSegment);
            if (transform == null)
                return;
            float x = posXTrackbar.Value;
            float y = posYTrackbar.Value;
            transform.MoveTo(x, y);
            compositionCanvas.Invalidate();
        }

        private void rotTrackbar_Scroll(object sender, EventArgs e)
        {
            if (composition == null)
                return;
            var transform = composition.GetMeshInfo(composition.editingSegment);
            if (transform == null)
                return;
            float deg = rotTrackbar.Value;
            transform.Rotate(deg);
            compositionCanvas.Invalidate();
        }

        private void scaleTrackbar_Scroll(object sender, EventArgs e)
        {
            if (composition == null)
                return;
            var transform = composition.GetMeshInfo(composition.editingSegment);
            if (transform == null)
                return;
            float x = scaleXTrackbar.Value * 0.1f;
            float y = scaleYTrackbar.Value * 0.1f;
            transform.Scale(x, y);
            compositionCanvas.Invalidate();
        }

        private void reverseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (composition == null)
                return;
            var transform = composition.GetMeshInfo(composition.editingSegment);
            if (transform == null)
                return;
            transform.ReverseX(reverseCheckBox.Checked);
            compositionCanvas.Invalidate();
        }

        private void brindForwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (composition == null)
                return;
            composition.Forward(composition.editingSegment);
            compositionCanvas.Invalidate();
        }

        private void bringToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (composition == null)
                return;
            composition.Front(composition.editingSegment);
            compositionCanvas.Invalidate();
        }

        private void sendBackwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (composition == null)
                return;
            composition.Backward(composition.editingSegment);
            compositionCanvas.Invalidate();
        }

        private void sendToBackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (composition == null)
                return;
            composition.Back(composition.editingSegment);
            compositionCanvas.Invalidate();
        }

    }
}