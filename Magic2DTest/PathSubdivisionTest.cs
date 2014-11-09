using Magic2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///PathSubdivisionTest のテスト クラスです。すべての
    ///PathSubdivisionTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class PathSubdivisionTest
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
        ///Subdivide のテスト
        ///</summary>
        [TestMethod()]
        public void SubdivideTest()
        {
            List<PointF> path = Util.circlePoints(110, 110, 100, 10); // TODO: 適切な値に初期化してください
            float course = 10F; // TODO: 適切な値に初期化してください
            List<PointF> actual;
            actual = PathSubdivision.Subdivide(path, course);

            Bitmap bmp = new Bitmap(300, 300);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.DrawCurve(new Pen(Brushes.Black, 2), path.ToArray());// actual.ToArray());
                int cnt = 0;
                foreach (var pt in actual)
                    g.FillRectangle(new SolidBrush(Color.FromArgb(255, 255, (int)(255f * cnt++ / actual.Count))), new RectangleF(pt.X - 2, pt.Y - 2, 4, 4));
            }

            var f = System.IO.Path.GetFullPath("../../subdiv_result.png");
            bmp.Save(f);
            System.Diagnostics.Process.Start(f);
        }

        /// <summary>
        ///SubdivideSegment のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void SubdivideSegmentTest()
        {
            var ls1 = PathSubdivision_Accessor.SubdivideSegment(new PointF(0, 0), new Point(100, 0), -10);
            var ls2 = PathSubdivision_Accessor.SubdivideSegment(new PointF(0, 0), new Point(100, 0), 0);
            var ls3 = PathSubdivision_Accessor.SubdivideSegment(new PointF(0, 0), new Point(100, 0), 10);
            var ls4 = PathSubdivision_Accessor.SubdivideSegment(new PointF(0, 0), new Point(100, 0), 30);
            var ls5 = PathSubdivision_Accessor.SubdivideSegment(new PointF(0, 0), new Point(100, 0), 50);
            var ls6 = PathSubdivision_Accessor.SubdivideSegment(new PointF(0, 0), new Point(100, 0), 60);
            var ls7 = PathSubdivision_Accessor.SubdivideSegment(new PointF(0, 0), new Point(100, 0), 100);
            var ls8 = PathSubdivision_Accessor.SubdivideSegment(new PointF(0, 0), new Point(100, 0), 110);

            Assert.AreEqual(ls1, null);
            Assert.AreEqual(ls2, null);
            Assert.AreEqual(ls3.Count, 9);
            Assert.AreEqual(ls4.Count, 2);
            Assert.AreEqual(ls5.Count, 1);
            Assert.AreEqual(ls6.Count, 0);
            Assert.AreEqual(ls7.Count, 0);
            Assert.AreEqual(ls8.Count, 0);

            var expected = new[] { new PointF(100 / 3f, 0), new PointF(100 * 2 / 3f, 0) };
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(ls4[i].X, expected[i].X, 1e-4);
                Assert.AreEqual(ls4[i].Y, expected[i].Y, 1e-4);
            }
        }
    }
}
