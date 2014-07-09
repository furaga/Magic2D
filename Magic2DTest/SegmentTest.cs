using Magic2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using FLib;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///SegmentTest のテスト クラスです。すべての
    ///SegmentTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class SegmentTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 追加のテスト属性
        // 
        //テストを作成するときに、次の追加属性を使用することができます:
        //
        //クラスの最初のテストを実行する前にコードを実行するには、ClassInitialize を使用
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //クラスのすべてのテストを実行した後にコードを実行するには、ClassCleanup を使用
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //各テストを実行する前にコードを実行するには、TestInitialize を使用
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //各テストを実行した後にコードを実行するには、TestCleanup を使用
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///UpdateBitmap のテスト
        ///</summary>
        [TestMethod()]
        unsafe public void UpdateBitmapTest()
        {
            string key = string.Empty; // TODO: 適切な値に初期化してください

            var seg = SetPath();
            Assert.AreEqual(seg.path.Count, 5);
            Assert.IsTrue(seg.Closed);

            using (var bmp = CreatGradBitmap(200, 200))
            {
                SegmentRoot root = new SegmentRoot(bmp, null);
                seg.UpdateBitmap(root);

                Assert.IsNotNull(seg.bmp);
                Assert.AreEqual(seg.bmp.Width, 127, 2);
                Assert.AreEqual(seg.bmp.Height, 127, 2);
                Assert.AreEqual(seg.bmp.GetPixel(0, 0).A, 0);
                Assert.AreEqual(seg.bmp.GetPixel(seg.bmp.Width / 2, seg.bmp.Height / 2).A, 255);
            }

            var seg2 = SetPath2();
            using (var bmp = CreatGradBitmap(200, 200))
            {
                SegmentRoot root = new SegmentRoot(bmp, null);
                seg2.UpdateBitmap(root);
                Assert.IsNotNull(seg2.bmp);
                Assert.AreEqual(seg.bmp.GetPixel(0, 0).A, 0);
                Assert.AreEqual(seg.bmp.GetPixel(seg.bmp.Width / 2, seg.bmp.Height / 2).A, 255);
            }
        }

        PathSegment SetPath()
        {
            Matrix transform = new Matrix();
            transform.Translate(100, 100, MatrixOrder.Append);
            transform.Scale(0.5f, 0.5f, MatrixOrder.Append);

            // (50, 50), (150, 50), 150, 150), (50, 150) =>
            // (150, 150), (250, 150), (250, 250), (150, 250) =>
            // (75, 75), (125, 75), (125, 125), (75, 125)
            PathSegment seg = new PathSegment("", null);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(75, 75), transform, SegmentOperation.Segment);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(125, 75), transform, SegmentOperation.Segment);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(125, 125), transform, SegmentOperation.Segment);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(75, 125), transform, SegmentOperation.Segment);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(75, 75), transform, SegmentOperation.Segment);
            return seg;
        }

        PathSegment SetPath2()
        {
            Matrix transform = new Matrix();
            transform.Translate(100, 100, MatrixOrder.Append);
            transform.Scale(0.5f, 0.5f, MatrixOrder.Append);

            // (50, 50), (150, 50), 150, 150), (50, 150) =>
            // (150, 150), (250, 150), (250, 250), (150, 250) =>
            // (75, 75), (125, 75), (125, 125), (75, 125)
            PathSegment seg = new PathSegment("", null);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(75, 180), transform, SegmentOperation.Segment);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(100, 180), transform, SegmentOperation.Segment);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(100, 210), transform, SegmentOperation.Segment);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(75, 210), transform, SegmentOperation.Segment);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(75, 180), transform, SegmentOperation.Segment);
            return seg;
        }

        PathSegment_Accessor GetPathSegment_Accessor()
        {
            Matrix transform = new Matrix();
            transform.Translate(100, 100, MatrixOrder.Append);
            transform.Scale(0.5f, 0.5f, MatrixOrder.Append);

            // (50, 50), (150, 50), 150, 150), (50, 150) =>
            // (150, 150), (250, 150), (250, 250), (150, 250) =>
            // (75, 75), (125, 75), (125, 125), (75, 125)
            PathSegment_Accessor seg = new PathSegment_Accessor("", null);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(75, 75), transform, SegmentOperation.Segment);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(125, 75), transform, SegmentOperation.Segment);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(125, 125), transform, SegmentOperation.Segment);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(75, 125), transform, SegmentOperation.Segment);
            seg.OnMouseDown(System.Windows.Forms.MouseButtons.Left, new Point(75, 75), transform, SegmentOperation.Segment);
            return seg;
        }

        unsafe Bitmap CreatGradBitmap(int w, int h)
        {
            var bmp = new Bitmap(200, 200, PixelFormat.Format32bppArgb);
            using (var iter = new BitmapIterator(bmp, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb))
            {
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        iter.Data[iter.Stride * y + 4 * x + 0] = 255;
                        iter.Data[iter.Stride * y + 4 * x + 1] = (byte)(255f * x / w);
                        iter.Data[iter.Stride * y + 4 * x + 2] = (byte)(255f * y / h);
                        iter.Data[iter.Stride * y + 4 * x + 3] = 255;
                    }
                }
            }
            return bmp;
        }

        /// <summary>
        ///DrawPath のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void DrawPathTest()
        {
            Segment seg = SetPath();
            Bitmap bmp = seg.DrawPath(new Rectangle(0, 0, 200, 200));
            Assert.AreEqual(bmp.GetPixel(0, 0).G, 0);
            foreach (var pt in seg.path)
                Assert.AreEqual(bmp.GetPixel((int)pt.X, (int)pt.Y).G, 255);
            bmp.Dispose();

            bmp = seg.DrawPath(new Rectangle(20, 20, 160, 160));
            Assert.AreEqual(bmp.GetPixel(0, 0).G, 0);
            foreach (var pt in seg.path)
                Assert.AreEqual(bmp.GetPixel((int)pt.X - 20, (int)pt.Y - 20).G, 255);
            bmp.Dispose();
        }

        /// <summary>
        ///GetExpandedBound のテスト
        ///</summary>
        [TestMethod()]
        public void GetExpandedBoundTest()
        {
            Segment seg = SetPath();
            
            var rect = seg.GetBound(seg.path);
            Assert.AreEqual(rect.X, 50, 1e-4);
            Assert.AreEqual(rect.Y, 50, 1e-4);
            Assert.AreEqual(rect.Width, 100, 1e-4);
            Assert.AreEqual(rect.Height, 100, 1e-4);
            
            rect = seg.GetExpandedBound(seg.path);
            Assert.AreEqual(rect.X, 31, 1e-4);
            Assert.AreEqual(rect.Y, 31, 1e-4);
            Assert.AreEqual(rect.Width, 139, 1e-4);
            Assert.AreEqual(rect.Height, 139, 1e-4);
        }

        /// <summary>
        ///GetPixelsInPath のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void GetPixelsInPathTest()
        {
            PathSegment target = SetPath();

            var pathBmp = target.DrawPath(new Rectangle(0, 0, 200, 200));
            Rectangle pixelBound;
            bool[] pixels = target.GetPixelsInPath(pathBmp, out pixelBound);
            Assert.AreEqual(pixelBound.Width * pixelBound.Height, pixels.Length);
            Assert.IsTrue(100 * 100 <= pixels.Length);
            Assert.IsTrue(pixels.Length <= 130 * 130);
            Assert.IsTrue(pixels[pixels.Length / 2]);
        }

        /// <summary>
        ///GetNearestPoint のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void GetNearestPointTest()
        {
            Segment_Accessor target = new Segment_Accessor("", null);
            var path = new List<PointF>()
            {
                new PointF(0, 0),
                new PointF(0, 100),
                new PointF(100, 100),
                new PointF(100, 0),
            };
            var nearest = target.GetNearestPoint(new PointF(1, 1), null, 20, new Matrix());
            Assert.AreEqual(nearest.X, 0, 1e-4);
            Assert.AreEqual(nearest.Y, 0, 1e-4);

            nearest = target.GetNearestPoint(new PointF(1, 1), new List<PointF>(), 20, new Matrix());
            Assert.AreEqual(nearest.X, 0, 1e-4);
            Assert.AreEqual(nearest.Y, 0, 1e-4);

            nearest = target.GetNearestPoint(new PointF(1, 1), path, 20, new Matrix());
            Assert.AreEqual(nearest.X, 0, 1e-4);
            Assert.AreEqual(nearest.Y, 0, 1e-4);

            nearest = target.GetNearestPoint(new PointF(1, 99), path, 20, new Matrix());
            Assert.AreEqual(nearest.X, 0, 1e-4);
            Assert.AreEqual(nearest.Y, 100, 1e-4);

            nearest = target.GetNearestPoint(new PointF(99, 99), path, 20, new Matrix());
            Assert.AreEqual(nearest.X, 100, 1e-4);
            Assert.AreEqual(nearest.Y, 100, 1e-4);
            
            nearest = target.GetNearestPoint(new PointF(99, 1), path, 20, new Matrix());
            Assert.AreEqual(nearest.X, 100, 1e-4);
            Assert.AreEqual(nearest.Y, 0, 1e-4);
        }

        /// <summary>
        ///OnMouseMove のテスト
        ///</summary>
        [TestMethod()]
        public void OnMouseMoveTest()
        {
            SegmentRoot root = new SegmentRoot(null, new SkeletonAnnotation(null));
            root.an.joints.Add(new JointAnnotation("0", new PointF(10, 10)));
            root.an.joints.Add(new JointAnnotation("1", new PointF(100, 100)));
            root.an.bones.Add(new BoneAnnotation(root.an.joints[0], root.an.joints[1]));

            Segment target = new Segment("", root);
            Assert.IsNotNull(target.root);
            Assert.IsNotNull(target.root.an);

            //
            target.OnMouseMove(MouseButtons.None, new PointF(200, 200), new Matrix(), SegmentOperation.SkeletonAnnotation);
            Assert.IsNull(target.nearestBone);
            Assert.IsTrue(target.nearestPoint.IsEmpty);

            target.OnMouseMove(MouseButtons.None, new PointF(20, 10), new Matrix(), SegmentOperation.SkeletonAnnotation);
            Assert.IsNotNull(target.nearestBone);
            Assert.IsTrue(target.nearestPoint.IsEmpty);
            Assert.IsTrue(root.an.bones.Contains(target.nearestBone));

            //
            target.OnMouseMove(MouseButtons.None, new PointF(20, 10), new Matrix(), SegmentOperation.Segment);
            Assert.IsNull(target.nearestBone);
            Assert.IsTrue(target.nearestPoint.IsEmpty);

            target.path.Add(new PointF(10, 10));
            target.OnMouseMove(MouseButtons.None, new PointF(20, 10), new Matrix(), SegmentOperation.Segment);
            Assert.IsNull(target.nearestBone);
            Assert.IsTrue(target.nearestPoint.IsEmpty);


            //
            target.OnMouseMove(MouseButtons.None, new PointF(20, 10), new Matrix(), SegmentOperation.Section);
            Assert.IsNull(target.nearestBone);
            Assert.IsFalse(target.nearestPoint.IsEmpty);
            Assert.IsTrue(target.path.Contains(target.nearestPoint));

            //
            target.OnMouseMove(MouseButtons.None, new PointF(20, 10), new Matrix(), SegmentOperation.PartingLine);
            Assert.IsNull(target.nearestBone);
            Assert.IsTrue(target.nearestPoint.IsEmpty);

            target.partingLine.Add(new PointF(10, 10));
            target.OnMouseMove(MouseButtons.None, new PointF(15, 10), new Matrix(), SegmentOperation.PartingLine);
            Assert.IsNull(target.nearestBone);
            Assert.IsFalse(target.nearestPoint.IsEmpty);
            Assert.IsTrue(target.partingLine.Contains(target.nearestPoint));
        }
    }
}
