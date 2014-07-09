using Magic2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Collections.Generic;
using FLib;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///ARAPDeformationTest のテスト クラスです。すべての
    ///ARAPDeformationTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class ARAPDeformationTest
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
        ///IsPointInPolygon のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void IsPointInPolygonTest()
        {
            var path = new List<PointF>()
            {
                new PointF(0, 0),
                new PointF(5, 5),
                new PointF(10, 0),
                new PointF(10, 10),
                new PointF(0, 10),
            };

            Assert.IsTrue(FMath.IsPointInPolygon(new PointF(5, 7), path));
            Assert.IsTrue(FMath.IsPointInPolygon(new PointF(4, 5), path));
            Assert.IsTrue(FMath.IsPointInPolygon(new PointF(4, 4), path));
            Assert.IsTrue(FMath.IsPointInPolygon(new PointF(7, 4), path));
            Assert.IsTrue(FMath.IsPointInPolygon(new PointF(3, 9), path));

            Assert.IsFalse(FMath.IsPointInPolygon(new PointF(-1, -1), path));
            Assert.IsFalse(FMath.IsPointInPolygon(new PointF(0, 0), path));
            Assert.IsFalse(FMath.IsPointInPolygon(new PointF(5, 3), path));
            Assert.IsFalse(FMath.IsPointInPolygon(new PointF(-1, 5), path));
            Assert.IsFalse(FMath.IsPointInPolygon(new PointF(11, 5), path));
            Assert.IsFalse(FMath.IsPointInPolygon(new PointF(5, 11), path));
             Assert.IsFalse(FMath.IsPointInPolygon(new PointF(0, 5), path));
             Assert.IsFalse(FMath.IsPointInPolygon(new PointF(5, 10.00001f), path));
            Assert.IsFalse(FMath.IsPointInPolygon(new PointF(5, 4.9999f), path));
        }

        /// <summary>
        ///CreateMesh のテスト
        ///</summary>
        [TestMethod()]
        public void CreateMeshTest()
        {
            List<PointF> path = new List<PointF>()
            {
                new PointF(0, 0),
                new PointF(100, 0),
                new PointF(100, 100),
                new PointF(0, 100),
            };

            List<PointF> pts = new List<PointF>();
            List<ARAPDeformation.TriMesh> meshes = new List<ARAPDeformation.TriMesh>();
            ARAPDeformation.CreateMesh(path, 50, 3, pts, meshes);

            Assert.AreEqual(pts.Count, 9);
            Assert.AreEqual(meshes.Count, 8);

            path = new List<PointF>()
            {
                new PointF(0, 0),
                new PointF(50, 50),
                new PointF(100, 0),
                new PointF(100, 100),
                new PointF(0, 100),
            };
            ARAPDeformation.CreateMesh(path, 10, 3, pts, meshes);

            Assert.AreEqual(pts.Count, 61 + 5);
        }

        [TestMethod()]
        public void CreateMeshTest1()
        {
            var path = new List<PointF>()
            {
                new PointF(117, 238),
                new PointF(171, 101),
                new PointF(213, 314),
            };

            List<PointF> pts = new List<PointF>();
            List<ARAPDeformation.TriMesh> meshes = new List<ARAPDeformation.TriMesh>();
            ARAPDeformation.CreateMesh(path, 32, 16, pts, meshes);

            Assert.AreEqual(pts.Count, 8);
//            Assert.AreEqual(meshes.Count, 6);
        }

        /// <summary>
        ///TranslateControlPoint のテスト
        ///</summary>
        [TestMethod()]
        public void TranslateControlPointTest()
        {
            ARAPDeformation target = new ARAPDeformation(new List<PointF>()
                {
                    new PointF(0, 0),
                    new PointF(100, 0),
                    new PointF(100, 100),
                    new PointF(0, 100),
                }, null); // TODO: 適切な値に初期化してください
            target.BeginDeformation();
            target.EndDeformation();

            target.AddControlPoint(new PointF(0, 0), new PointF(0, 0));
            target.AddControlPoint(new PointF(100, 0), new PointF(100, 0));
            target.AddControlPoint(new PointF(100, 100), new PointF(100, 100));
            //target.AddControlPoint(new PointF(50, 50));
            //        target.AddControlPoint(new PointF(150, 50));
            //        target.AddControlPoint(new PointF(150, 150));
            target.BeginDeformation();

            //target.TranslateControlPoint(new PointF(50, 50), new PointF(-50, -50));
            //target.TranslateControlPoint(new PointF(150, 50), new PointF(100, -50));
            //target.TranslateControlPoint(new PointF(150, 150), new PointF(50, 50));

            target.TranslateControlPoint(new PointF(0, 0), new PointF(-100, -100), true);
            Assert.AreEqual(target.meshPointList[0].X, -100, 1);
            Assert.AreEqual(target.meshPointList[0].Y, -100, 1);

            target.TranslateControlPoint(new PointF(100, 0), new PointF(0, -100), false);
            Assert.AreEqual(target.meshPointList[0].X, -100, 1);
            Assert.AreEqual(target.meshPointList[0].Y, -100, 1);

            target.TranslateControlPoint(new PointF(100, 100), new PointF(0, 0), true);

            target.EndDeformation();

            Assert.AreEqual(target.meshPointList[0].X, -100, 1);
            Assert.AreEqual(target.meshPointList[0].Y, -100, 1);
            Assert.AreEqual(target.meshPointList[1].X, 0, 1);
            Assert.AreEqual(target.meshPointList[1].Y, -100, 1);
            Assert.AreEqual(target.meshPointList[2].X, 0, 1);
            Assert.AreEqual(target.meshPointList[2].Y, 0, 1);
            Assert.AreEqual(target.meshPointList[3].X, -100, 1);
            Assert.AreEqual(target.meshPointList[3].Y, 0, 1);

            var path = target.GetPath();
            Assert.AreEqual(path[0].X, -100, 1);
            Assert.AreEqual(path[0].Y, -100, 1);
            Assert.AreEqual(path[1].X, 0, 1);
            Assert.AreEqual(path[1].Y, -100, 1);
            Assert.AreEqual(path[2].X, 0, 1);
            Assert.AreEqual(path[2].Y, 0, 1);
            Assert.AreEqual(path[3].X, -100, 1);
            Assert.AreEqual(path[3].Y, 0, 1);
        }
    }
}
