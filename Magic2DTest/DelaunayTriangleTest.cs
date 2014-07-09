using DelaunayTriangle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Collections.Generic;
using FLib;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///DelaunayTriangleTest のテスト クラスです。すべての
    ///DelaunayTriangleTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class DelaunayTriangleTest
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
        ///DelaunayTriangulation のテスト
        ///</summary>
        [TestMethod()]
        public void DelaunayTriangulationTest()
        {
            List<PointF> points = new List<PointF>()
            {
                new Point(0, 0),
                new Point(10, 0),
                new Point(10, 10),
                new Point(0, 10),
                new Point(5, 5),
            };

            var tris = DelaunayTriangle.DelaunayTriangle.DelaunayTriangulation(points, new RectangleF(-1, -1, 12, 12));
            Assert.AreEqual(tris.Count, 4);

            tris = DelaunayTriangle.DelaunayTriangle.DelaunayTriangulation(points, RectangleF.Empty);
            Assert.AreEqual(tris.Count, 0);

            tris = DelaunayTriangle.DelaunayTriangle.DelaunayTriangulation(null, RectangleF.Empty);
            Assert.AreEqual(tris.Count, 0);
        }

    }
}
