using FLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///FMathTest のテスト クラスです。すべての
    ///FMathTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class FMathTest
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
        ///SqDistance のテスト
        ///</summary>
        [TestMethod()]
        public void SqDistanceTest()
        {
            Assert.AreEqual(FMath.SqDistance(Point.Empty, Point.Empty), 0, 1e-4);
            Assert.AreEqual(FMath.SqDistance(new Point(1, 1), new PointF(1, 2)), 1, 1e-4);
            Assert.AreEqual(FMath.SqDistance(new Point(1, 1), new PointF(2, 1)), 1, 1e-4);
            Assert.AreEqual(FMath.SqDistance(new Point(1, 1), new PointF(2, 2)), 2, 1e-4);
        }

        /// <summary>
        ///IsCrossed のテスト
        ///</summary>
        [TestMethod()]
        public void IsCrossedTest()
        {
            PointF p1 = new PointF(-1, -1);
            PointF p2 = new PointF(1, 1);

            PointF p3 = new PointF(-1, -1);
            PointF p4 = new PointF(1, 1);
            Assert.AreEqual(FMath.IsCrossed(p1, p2, p3, p4), false);

            p3 = new PointF(-1, -1);
            p4 = new PointF(0, 0);
            Assert.AreEqual(FMath.IsCrossed(p1, p2, p3, p4), false);

            p3 = new PointF(0, 0);
            p4 = new PointF(1, 1);
            Assert.AreEqual(FMath.IsCrossed(p1, p2, p3, p4), false);

            p3 = new PointF(-1, -1);
            p4 = new PointF(1, 0);
            Assert.AreEqual(FMath.IsCrossed(p1, p2, p3, p4), true);

            p3 = new PointF(-1, 0);
            p4 = new PointF(1, 1);
            Assert.AreEqual(FMath.IsCrossed(p1, p2, p3, p4), true);

            p3 = new PointF(0, 0);
            p4 = new PointF(1, -1);
            Assert.AreEqual(FMath.IsCrossed(p1, p2, p3, p4), true);

            p3 = new PointF(-1, 1);
            p4 = new PointF(0, 0);
            Assert.AreEqual(FMath.IsCrossed(p1, p2, p3, p4), true);

            p3 = new PointF(-1, 1);
            p4 = new PointF(1, -1);
            Assert.AreEqual(FMath.IsCrossed(p1, p2, p3, p4), true);
        }

        /// <summary>
        ///SplitPath のテスト
        ///</summary>
        [TestMethod()]
        public void SplitPathTest()
        {
            List<PointF> p = new List<PointF>();
            for (int i = 0; i < 10; i++)
                p.Add(new PointF(10 * i, 10 * i));

            FMath.SplitPath(null, null, false);
            FMath.SplitPath(new List<PointF>(), new List<PointF>(), false);
            FMath.SplitPath(new List<PointF>(), new List<PointF>() { p[0] }, false);

            var paths = FMath.SplitPath(new List<PointF>() { p[0], p[9], p[4], p[5] }, p, true);
            CheckPointList(paths[0], new List<PointF>() { p[4], p[5] });
            CheckPointList(paths[1], new List<PointF>() { p[9], p[0] });
        }

        void CheckPointList(List<PointF> ls0, List<PointF> ls1)
        {
            Assert.AreEqual(ls0.Count, ls1.Count);
            for (int i = 0; i < ls0.Count; i++)
                Assert.AreEqual(ls0[i], ls1[i]);
        }

        /// <summary>
        ///GetCurvature のテスト
        ///</summary>
        [TestMethod()]
        public void GetCurvatureTest()
        {
            Assert.AreEqual(FMath.GetCurvature(null), 0, 1e-4);
            Assert.AreEqual(FMath.GetCurvature(new List<PointF>()), 0, 1e-4);
            List<PointF> curve = new List<PointF>()
            {
                new PointF(10, 10), new PointF(10, 110), new PointF(10, 210), 
            };
            Assert.AreEqual(FMath.GetCurvature(curve), 0, 1e-4);

            curve = new List<PointF>()
            {
                new PointF(10, 10), new PointF(110, 10), new PointF(210, 10), 
            };
            Assert.AreEqual(FMath.GetCurvature(curve), 0, 1e-4);

            curve = new List<PointF>()
            {
                new PointF(10, 10), new PointF(110, 110), new PointF(210, 210), 
            };
            Assert.AreEqual(FMath.GetCurvature(curve), 0, 1e-4);

            curve = new List<PointF>()
            {
                new PointF(10, 10), new PointF(110, 110), new PointF(210, 210), new PointF(250, 250), 
            };
            Assert.AreEqual(FMath.GetCurvature(curve), 0, 1e-4);

            curve = new List<PointF>()
            {
                new PointF(10, 10), new PointF(110, 110), new PointF(210, 210), new PointF(250, 250), 
            };
            Assert.AreEqual(FMath.GetCurvature(curve), 0, 1e-4);

            curve = new List<PointF>()
            {
                new PointF(10, 10), new PointF(110, 110), new PointF(110, 10), 
            };
            var k1 = FMath.GetCurvature(curve);
            curve = new List<PointF>()
            {
                new PointF(10, 10), new PointF(110, 110), new PointF(100, 10), 
            };
            var k2 = FMath.GetCurvature(curve);
            Assert.IsTrue(k1 > 0);
            Assert.IsTrue(k2 > k1);
        }
    }
}
