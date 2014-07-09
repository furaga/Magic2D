using Magic2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing.Drawing2D;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///PathSegmentTest のテスト クラスです。すべての
    ///PathSegmentTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class PathSegmentTest
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
        ///OnMouseDown のテスト
        ///</summary>
        [TestMethod()]
        public void OnMouseDownTest()
        {
            SegmentRoot root = null; // TODO: 適切な値に初期化してください
            PathSegment_Accessor target = new PathSegment_Accessor("", root); // TODO: 適切な値に初期化してください
            for (int i = 0; i < 10; i++)
            {
                target.OnMouseDown(MouseButtons.Left, new PointF(0, 100 * i), new Matrix(), SegmentOperation.Segment);
                Assert.AreEqual(target.path.Count, i + 1);
            }
            target.OnMouseDown(MouseButtons.Right, new Point(1, 0), new Matrix(), SegmentOperation.Segment);
            Assert.AreEqual(target.path.Count, 10);
        }

        /// <summary>
        ///IsCrossed のテスト
        ///</summary>
        [TestMethod()]
        public void IsCrossedTest()
        {
            SegmentRoot root = null; // TODO: 適切な値に初期化してください
            PathSegment_Accessor target = new PathSegment_Accessor("", root); // TODO: 適切な値に初期化してください
            Assert.AreEqual(target.IsCrossed(null), false);

            List<PointF> path = new List<PointF>();
            Assert.AreEqual(target.IsCrossed(path.ToArray()), false);

            path.Add(new PointF(0, 0));
            path.Add(new PointF(1, 0));
            path.Add(new PointF(1, 1));
            path.Add(new PointF(0, 1));
            path.Add(new PointF(0, 0));
            Assert.AreEqual(target.IsCrossed(path.ToArray()), true);

            path[path.Count - 1] = new PointF(0.1f, 0.1f);
            Assert.AreEqual(target.IsCrossed(path.ToArray()), false);

            path[path.Count - 1] = new PointF(0, -1);
            Assert.AreEqual(target.IsCrossed(path.ToArray()), true);

            path[path.Count - 1] = new PointF(1, -1);
            Assert.AreEqual(target.IsCrossed(path.ToArray()), true);
        }

        /// <summary>
        ///IsClosed のテスト
        ///</summary>
        [TestMethod()]
        public void IsClosedTest()
        {
            SegmentRoot root = null; // TODO: 適切な値に初期化してください
            PathSegment_Accessor target = new PathSegment_Accessor("", root); // TODO: 適切な値に初期化してください
            Assert.AreEqual(target.IsClosed(null, 0.1f), false);

            List<PointF> path = new List<PointF>();
            Assert.AreEqual(target.IsClosed(path.ToArray(), 0.1f), false);

            path.Add(new PointF(0, 0));
            Assert.AreEqual(target.IsClosed(path.ToArray(), 0.1f), false);

            path.Add(new PointF(1, 0));
            Assert.AreEqual(target.IsClosed(path.ToArray(), 0.1f), false);

            path.Add(new PointF(1, 1));
            Assert.AreEqual(target.IsClosed(path.ToArray(), 0.1f), false);

            path.Add(new PointF(0, 0));
            Assert.AreEqual(target.IsClosed(path.ToArray(), 0.1f), true);

            path.RemoveAt(path.Count - 1);
            path.Add(new PointF(0, 1));
            Assert.AreEqual(target.IsClosed(path.ToArray(), 0.1f), false);

            path.Add(new PointF(0.01f, 0.01f));
            Assert.AreEqual(target.IsClosed(path.ToArray(), 0.1f), true);
            Assert.AreEqual(target.IsClosed(path.ToArray(), 0.01f), false);
        }

        /// <summary>
        ///AssignPathPoint のテスト
        ///</summary>
        [TestMethod()]
        public void AssignDeletePathPointTest()
        {
            SegmentRoot root = null; // TODO: 適切な値に初期化してください
            PathSegment_Accessor target = new PathSegment_Accessor("", root); // TODO: 適切な値に初期化してください

            target.AssignPathPoint(new PointF(0, 0), new Matrix());
            Assert.AreEqual(target.path.Count, 1);
            Assert.AreEqual(target.Closed, false);

            target.AssignPathPoint(new PointF(100, 0), new Matrix());
            Assert.AreEqual(target.path.Count, 2);
            Assert.AreEqual(target.Closed, false);

            target.AssignPathPoint(new PointF(100, 100), new Matrix());
            Assert.AreEqual(target.path.Count, 3);
            Assert.AreEqual(target.Closed, false);

            target.AssignPathPoint(new PointF(0, 0), new Matrix());
            Assert.AreEqual(target.path.Count, 4);
            Assert.AreEqual(target.Closed, true);

            target.DeletePathPoint(new PointF(0, 0), new Matrix());
            Assert.AreEqual(target.path.Count, 3);
            Assert.AreEqual(target.Closed, false);

            target.AssignPathPoint(new PointF(1, 1), new Matrix());
            Assert.AreEqual(target.path.Count, 4);
            Assert.AreEqual(target.Closed, true);

            var transform = new Matrix();
            target.DeletePathPoint(new PointF(0, 0), transform);
            Assert.AreEqual(target.path.Count, 3);
            Assert.AreEqual(target.Closed, false);

            transform.Scale(10, 10);
            target.AssignPathPoint(new PointF(1f, 1f), transform);
            Assert.AreEqual(target.path.Count, 4);
            Assert.AreEqual(target.Closed, false);
        }
    }
}
