using Magic2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
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

            target.ToBitmap().Save("../../../before.png");
            target.TranslateControlPoint(new PointF(0, 0), new PointF(-100, -100), true);
            target.ToBitmap().Save("../../../after.png");
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

        /// <summary>
        ///ARAPDeformation コンストラクター のテスト
        ///</summary>
        [TestMethod()]
        public void ARAPDeformationConstructorTest()
        {
            ARAPDeformation refARAP = new ARAPDeformation(new List<PointF>()
                {
                    new PointF(0, 0),
                    new PointF(100, 0),
                    new PointF(100, 100),
                    new PointF(0, 100),
                }, null); // TODO: 適切な値に初期化してください
            ARAPDeformation target = new ARAPDeformation(refARAP);

            var path1 = refARAP.GetPath();
            var path2 = target.GetPath();
            Assert.AreEqual(path1.Count, path2.Count);
            for (int i = 0; i < path1.Count; i++)
            {
                Assert.AreEqual(path1[i].X, path2[i].X, 1e-4);
                Assert.AreEqual(path1[i].Y, path2[i].Y, 1e-4);
            }
        }

        [TestMethod()]
        public void FloodFillTest()
        {
            List<int> labels = FMath.FloodFill<Object>(10, new Dictionary<int, List<int>>(), null, null);
            Assert.AreEqual(new HashSet<int>(labels).Count, 10);

            labels = FMath.FloodFill<Object>(10, new Dictionary<int, List<int>>() { { 0, new List<int>() { 2, 8, 4, 6, 0, 2 } } }, null, null);
            Assert.AreEqual(new HashSet<int>(labels).Count, 6);

            Dictionary<int, List<int>> edges = new Dictionary<int, List<int>>();
            for (int i = 0; i < 10; i++)
            {
                edges[i] = new List<int>();
                for (int j = 0; j < 10; j++)
                    edges[i].Add(j);
            }
            labels = FMath.FloodFill<Object>(10, edges, cond, null);
            Assert.AreEqual(new HashSet<int>(labels).Count, 2);
        }

        bool cond(int i, int j, int n, Object obj)
        {
            if (i < 5 && 5 <= j)
                return false;
            if (j < 5 && 5 <= i)
                return false;
            return true;
        }

        /// <summary>
        ///Parting のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void PartingTest()
        {
            ARAPDeformation_Accessor target = new ARAPDeformation_Accessor(); // TODO: 適切な値に初期化してください
            List<PointF> pts = new List<PointF>();
            List<int> labels = new List<int>();

            Dictionary<PointF, int> ptToIdx = new Dictionary<PointF, int>();
            for (int i = 0; i < 10 * 5; i++)
            {
                pts.Add(new PointF((i % 10) * 10, (i / 10) * 10));
                ptToIdx[pts.Last()] = i;
            }

            List<FTriangle> _tris = FLib.DelaunayTriangle.DelaunayTriangulation(pts, new RectangleF(0, 0, 100, 50));
            List<TriMesh> tris = new List<TriMesh>();
            foreach (var t in _tris)
            {
                var mesh = new TriMesh(ptToIdx[t.p1], ptToIdx[t.p2], ptToIdx[t.p3]);
                tris.Add(mesh);
            }

            ARAPDeformation_Accessor.Parting(pts, tris, new List<PointF>(), labels);
            Assert.AreEqual(new HashSet<int>(labels).Count, 1);

            List<PointF> partingLine1 = new List<PointF>() { new PointF(52, -1), new PointF(52, 20), new PointF(52, 51) };
            List<PointF> partingLine2 = new List<PointF>() { new PointF(50, -1), new PointF(50, 20), new PointF(50, 51) };
            List<PointF> partingLine3 = new List<PointF>() { new PointF(0, -50), new PointF(100, 50) };
            List<PointF> partingLine4 = new List<PointF>() { new PointF(0, -50), new PointF(50, 100), new PointF(100, -50) };

            ARAPDeformation_Accessor.Parting(pts, tris, partingLine1, labels);
            ToBitmap(labels, 10, 5, partingLine1, tris).Save("../../labels1.png");
            Assert.AreEqual(new HashSet<int>(labels).Count, 2);

            ARAPDeformation_Accessor.Parting(pts, tris, partingLine2, labels);
            ToBitmap(labels, 10, 5, partingLine2, tris).Save("../../labels2.png");
            Assert.AreEqual(new HashSet<int>(labels).Count, 2);

            ARAPDeformation_Accessor.Parting(pts, tris, partingLine3, labels);
            ToBitmap(labels, 10, 5, partingLine3, tris).Save("../../labels3.png");
            Assert.AreEqual(new HashSet<int>(labels).Count, 2);

            ARAPDeformation_Accessor.Parting(pts, tris, partingLine4, labels);
            ToBitmap(labels, 10, 5, partingLine4, tris).Save("../../labels4.png");
            Assert.AreEqual(new HashSet<int>(labels).Count, 3);
        }

        Bitmap ToBitmap(List<int> labels, int w, int h, List<PointF> partingLine, List<TriMesh> tris)
        {
            Brush[] colors = new[] {
                Brushes.Red,
                Brushes.Blue,
                Brushes.Green,
                Brushes.Yellow,
                Brushes.Black,
                Brushes.DarkRed,
                Brushes.DarkBlue,
                Brushes.DarkGreen,
                Brushes.Orange,
                Brushes.Gray,
                Brushes.LightBlue,
            };

            Bitmap bmp = new Bitmap(w * 16, h * 16);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                foreach (var t in tris)
                {
                    PointF pt0 = IdxToPoint( t.idx0, w, 10);
                    PointF pt1 = IdxToPoint( t.idx1, w, 10);
                    PointF pt2 = IdxToPoint( t.idx2, w, 10);

                    if (FMath.IsCrossed(pt0, pt1, partingLine))
                        g.DrawLine(new Pen(Brushes.Blue), pt0, pt1);
                    else
                        g.DrawLine(new Pen(Brushes.Black), pt0, pt1);

                    if (FMath.IsCrossed(pt1, pt2, partingLine))
                        g.DrawLine(new Pen(Brushes.Blue), pt1, pt2);
                    else
                        g.DrawLine(new Pen(Brushes.Black), pt1, pt2);

                    if (FMath.IsCrossed(pt0, pt2, partingLine))
                        g.DrawLine(new Pen(Brushes.Blue), pt0, pt2);
                    else
                        g.DrawLine(new Pen(Brushes.Black), pt0, pt2);
                }

                for (int y = 0; y < h; y++)
                    for (int x = 0; x < w; x++)
                        g.FillEllipse(colors[labels[x + y * w] % colors.Length], x * 10, y * 10, 3, 3);
                if (partingLine != null && partingLine.Count >= 2)
                    g.DrawLines(new Pen(Brushes.Black), partingLine.ToArray());
            }

            return bmp;
        }

        PointF IdxToPoint(int idx, int w, float ratio)
        {
            return new PointF((idx % w) * ratio, (idx / w) * ratio); 
        }
    }
}
