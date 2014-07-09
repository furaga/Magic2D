using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using FLib;

namespace Magic2D
{
    public partial class Form1 : Form
    {
        void IntializeView()
        {
            skeletonSplitContainer.Panel1.Controls.Add(sourceImageView);
            RefleshAllImageView();
        }


        //----------------------------------------------------------
        // 画像リストの更新
        //----------------------------------------------------------

        void RefleshAllImageView()
        {
            UpdateSourceImageView(true);
            UpdateSegmentImageView(true);
            UpdateComposedImageView(true);
        }
        void UpdateSourceImageView(bool reflesh)
        {
            UpdateImageView(sourceImageDict, sourceImageList, sourceImageView, reflesh);
        }
        void UpdateSegmentImageView(bool reflesh)
        {
            UpdateImageView(segmentImageDict, segmentImageList, segmentImageView, reflesh);
        }
        void UpdateComposedImageView(bool reflesh)
        {
            UpdateImageView(composedImageDict, composedImageList, composedImageView, reflesh);
        }

        void UpdateImageView(Dictionary<string, Bitmap> imageDict, ImageList imageList, ListView imageView, bool reflesh)
        {
            if (reflesh)
            {
                for (int i = 0; i < imageList.Images.Count; i++)
                    imageList.Images[i].Dispose();
                imageList.Images.Clear();
                imageView.Items.Clear();
            }
            foreach (var kv in imageDict)
            {
                string key = kv.Key;

                // キーが同じだったら上書き
                bool exist = false;
                for (int i = imageView.Items.Count - 1; i >= 0; i--)
                {
                    string key1 = imageView.Items[i].ImageKey;
                    if (key == key1)
                    {
                        exist = true;
                        if (reflesh)
                        {
                            imageList.Images[imageView.Items[i].ImageKey].Dispose();
                            imageList.Images.RemoveByKey(imageView.Items[i].ImageKey);
                            imageView.Items.RemoveAt(i);
                            exist = false;
                        }
                    }
                }

                if (!exist)
                {
                    imageList.Images.Add(kv.Key, BitmapHandler.CreateThumbnail(kv.Value, imageList.ImageSize.Width, imageList.ImageSize.Height));
                    imageView.Items.Add(key, kv.Key);
                }
            }
        }

        private void AllSelectImageView(ListView listView)
        {
            for (int i = 0; i < listView.Items.Count; i++)
            {
                listView.Items[i].Selected = true;
            }
        }

        //----------------------------------------------------------
        // タブの移動
        //----------------------------------------------------------

        void ShowSkeletonFittingTab(string key)
        {
            tabControl.SelectTab("tabSkeletonFitting");
            refSkeletonCanvas.Invalidate();
            skeletonFittingCanvas.Invalidate();
        }

        void ShowSegmentationTab(string key)
        {
            tabControl.SelectTab("tabSegmentation");
        }

        void MoveSourceImageView(string tabName)
        {
            if (tabName == "tabSourceImages")
                sourceImagesSplitContainer.Panel2.Controls.Add(sourceImageView);
            if (tabName == "tabSkeletonFitting")
                skeletonSplitContainer.Panel1.Controls.Add(sourceImageView);
            if (tabName == "tabSegmentation")
                segmentSplitContainer.Panel1.Controls.Add(sourceImageView);
        }

        void UpdateSegmentImageView(string tabName)
        {
            if (segmentation == null || composition == null)
                return;

            if (tabName == "tabComposition")
            {
                composition.UpdateSegmentDict(segmentation, true);
                UpdateImageView(composition.segmentImageDict, segmentImageList, segmentImageView, false);
            }
        }        
        Pen bonePen = new Pen(Brushes.DarkBlue, 2)
        {
            CustomEndCap = new AdjustableArrowCap(8, 8)
        };
        Pen bonePen1 = new Pen(Brushes.Cyan, 2)
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

        void DrawSkeletonFittingCanvas(Graphics g, int renderWidth, int renderHeight, string highlightJointName)
        {
            if (skeletonAnnotationDict.ContainsKey(editingAnnotationKey))
            {
                g.Transform = skeletonFittingCanvasTransform;

                SkeletonAnnotation an = skeletonAnnotationDict[editingAnnotationKey];

                // bmp
                g.DrawImage(an.bmp, Point.Empty);

                // joints
                foreach (JointAnnotation joint in an.joints)
                {
                    PointF pos = joint.position;
                    g.FillEllipse(joint.name == highlightJointName ? Brushes.Orange : Brushes.Blue, new RectangleF(pos.X - 5, pos.Y - 5, 10, 10));
                }

                // nearestJoint
                if (nearestJoint != null)
                    g.FillEllipse(Brushes.Red, new RectangleF(nearestJoint.position.X - 5, nearestJoint.position.Y - 5, 10, 10));

                // selectJoint
                if (selectJoint != null)
                    g.FillEllipse(Brushes.Yellow, new RectangleF(selectJoint.position.X - 5, selectJoint.position.Y - 5, 10, 10));

                // bones
                foreach (BoneAnnotation bone in an.bones)
                {
                    PointF src = bone.src.position;
                    PointF dst = bone.dst.position;
                    g.DrawLine(bonePen, src, dst);
                }

                // editingBone
                if (addingBone != null)
                {
                    PointF src = addingBone.src.position;
                    PointF dst = addingBone.dst.position;
                    g.DrawLine(editingBonePen, src, dst);
                }

                if (nearestBone != null)
                {
                    PointF src = nearestBone.src.position;
                    PointF dst = nearestBone.dst.position;
                    g.DrawLine(editingBonePen, src, dst);
                }

                // selectBone
                if (selectBone != null)
                {
                    PointF src = selectBone.src.position;
                    PointF dst = selectBone.dst.position;
                    g.DrawLine(selectBonePen, src, dst);
                }

            }
        }

        private void skeletonFittingCanvasPan(float dx, float dy)
        {
            skeletonFittingCanvasTransform.Translate(dx, dy, MatrixOrder.Append);
        }

        private void skeletonFittingCanvasZoom(float delta)
        {
            float prev = skeletonFittingCanvasScale;
            if (prev <= 1e-4)
                return;

            skeletonFittingCanvasScale += delta;
            skeletonFittingCanvasScale = FMath.Clamp(skeletonFittingCanvasScale, 0.1f, 15f);
            float ratio = skeletonFittingCanvasScale / prev;
            if (Math.Abs(1 - ratio) <= 1e-4)
                return;

            skeletonFittingCanvasTransform.Scale(ratio, ratio, MatrixOrder.Append);
        }

        void skeletonFittingCanvasZoom(float zoom, PointF pan)
        {
            skeletonFittingCanvasPan(-pan.X, -pan.Y);
            skeletonFittingCanvasZoom(zoom);
            skeletonFittingCanvasPan(pan.X, pan.Y);
        }

        //----------------------------------------------------------
        // tab:Animation
        //----------------------------------------------------------

        Pen cellHighlightPen = new Pen(Color.FromArgb(123, 194, 223), 3);
        const int cellWidth = 94;
        const int cellHeight = 57;
        const int cellGridWidth = cellWidth;
        const int cellGridHeight = cellHeight + 10;

        private void DrawAnimeCells(Graphics g, Dictionary<string, Bitmap> imageDict, List<AnimeCell> cells, List<AnimeCell> addingCells, AnimeCell prevCell, List<AnimeCell> selectCells, int fw, int fh)
        {
            g.Clear(Color.White);

            int gw = fw / cellGridWidth;
            int gh = cells.Count / gw + (cells.Count % gw >= 1 ? 1 : 0);

            int idx = 0;
            for (int gy = 0; gy < gh; gy++)
            {
                for (int gx = 0; gx < gw; gx++)
                {
                    Rectangle rect = new Rectangle(gx * cellGridWidth, gy * cellGridHeight, cellWidth, cellHeight);

                    if (idx >= cells.Count)
                        continue;

                    g.FillRectangle(Brushes.LightGray, rect);
                    if (imageDict.ContainsKey(cells[idx].key))
                    {
                        Bitmap bmp = imageDict[cells[idx].key];
                        SizeF size = BitmapHandler.GetFittingSize(bmp, cellWidth, cellHeight);
                        float ox = (cellWidth - size.Width) * 0.5f;
                        float oy = (cellHeight - size.Height) * 0.5f;
                        g.DrawImage(bmp, rect.X + ox,rect.Y + oy, size.Width, size.Height);
                    }

                    idx++;
                }
            }

            foreach (var cell in selectCells)
            {
                if (prevCell != null && !cells.Contains(cell))
                    continue;
                int i = cells.IndexOf(cell);
                int gx = i % gw;
                int gy = i / gw;
                Rectangle rect = new Rectangle(gx * cellGridWidth, gy * cellGridHeight, cellWidth, cellHeight);
                g.DrawRectangle(cellHighlightPen, rect);
            }

            if (prevCell == null || cells.Contains(prevCell))
            {
                int i = prevCell == null ? -1 : cells.IndexOf(prevCell);
                i += 1;
                int gx = i % gw;
                int gy = i / gw;
                Rectangle rect = new Rectangle(gx * cellGridWidth, gy * cellGridHeight, 3, cellGridHeight);
                g.FillRectangle(Brushes.Black, rect);
            }
        }

        private int GetPrevCellFromPoint(List<AnimeCell> cells, int x, int y, int fw, int fh)
        {
            if (x < 0 || fw <= x || y < 0 || fh <= y)
                return -2;

            int gw = fw / cellGridWidth;
            int gh = cells.Count / gw + (cells.Count % gw >= 1 ? 1 : 0);

            int gy = y / cellGridHeight;
            int gx = x / cellGridWidth;
            int idx = gx + gy * gw;
            if (idx < 0)
                return -2;

            return Math.Min(cells.Count - 1, idx - 1);
        }

        //--------------------------------------------------------------

        Pen pathPen = new Pen(Brushes.LightGray, 1);
        Pen closedPathPen = new Pen(Brushes.Yellow, 1);
        Pen partingLinePen = new Pen(Brushes.MediumPurple, 2);
        Pen sectionPen = new Pen(Brushes.LightGreen, 2);
        
        Brush pathBrush = Brushes.Orange;
        Brush closedPathBrush = Brushes.Orange;
        Brush partingLineBrush = Brushes.Purple;
        Brush sectionBrush = Brushes.Green;


        List<string> visibleSegmentKeys = new List<string>();
        List<string> visiblePathKeys = new List<string>();
        List<string> visibleSectionKeys = new List<string>();
        List<string> visibleSkeletonKeys = new List<string>();
        List<string> visiblePartingLineKeys = new List<string>();

        private void UpdateSegmentListView()
        {
            /*
             * - seg
             *   - section
             *   - skeleton
             *   - parting line
             */


            if (segmentation == null)
                return;
            var root = segmentation.GetEditingSegmentRoot();
            if (root == null)
                return;

            segmentListView.AfterCheck -= segmentListView_AfterCheck;

            segmentListView.Nodes.Clear();
            for (int i = 0; i < root.segments.Count; i++)
            {
                string name = root.segments[i].name;
                var n = new TreeNode(name) { Checked = visibleSegmentKeys.Contains(name) };
                var seg = GetEditingSegment();
                n.Nodes.Add(new TreeNode("Segment") { Checked = visibleSegmentKeys.Contains(name) });
                n.Nodes.Add(new TreeNode("Section") { Checked = visibleSectionKeys.Contains(name) });
                n.Nodes.Add(new TreeNode("Skeleton") { Checked = visibleSkeletonKeys.Contains(name) });
                n.Nodes.Add(new TreeNode("Parting lines") { Checked = visiblePartingLineKeys.Contains(name) });
                segmentListView.Nodes.Add(n);
            }
            
            segmentListView.AfterCheck += segmentListView_AfterCheck;
        }



        private void ClearSegmentListViews()
        {
            visibleSegmentKeys.Clear();
            visibleSectionKeys.Clear();
            visibleSkeletonKeys.Clear();
            visiblePartingLineKeys.Clear();
            visiblePathKeys.Clear();
            AddVisibleSegmentListViews("");
        }
        private void AddVisibleSegmentListViews(string key)
        {
            visibleSegmentKeys.Add(key);
            visiblePathKeys.Add(key);
            visibleSectionKeys.Add(key);
            visibleSkeletonKeys.Add(key);
            visiblePartingLineKeys.Add(key);
        }

        private void InitVisibleSegmentKeys(SegmentRoot root)
        {
            if (root == null)
                return;
            ClearSegmentListViews();
            foreach (var seg in root.segments)
                AddVisibleSegmentListViews(seg.name);
        }

        //--------------------------------------------------------

        private void UpdateSegmentTransformView(SegmentMeshInfo transform)
        {
            posXTrackbar.Value = Math.Min(posXTrackbar.Maximum, Math.Max(posXTrackbar.Minimum, (int)transform.position.X));
            posYTrackbar.Value = Math.Min(posYTrackbar.Maximum, Math.Max(posYTrackbar.Minimum, (int)transform.position.Y));
            rotTrackbar.Value = Math.Min(rotTrackbar.Maximum, Math.Max(rotTrackbar.Minimum, (int)transform.angle));
            scaleXTrackbar.Value = Math.Min(scaleXTrackbar.Maximum, Math.Max(scaleXTrackbar.Minimum, (int)transform.scale.X * 10));
            scaleYTrackbar.Value = Math.Min(scaleYTrackbar.Maximum, Math.Max(scaleYTrackbar.Minimum, (int)transform.scale.Y * 10));
        }
    }
}
