using DelaunayTriangle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using FLib;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///TriangleTest のテスト クラスです。すべての
    ///TriangleTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class TriangleTest
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
        ///Equals のテスト
        ///</summary>
        [TestMethod()]
        public void EqualsTest()
        {
            PointF p1 = new PointF(0, 0);
            PointF p2 = new PointF(10, 0);
            PointF p3 = new PointF(0, 10);
            PointF p4 = new PointF(10, 10);
            Triangle t0 = new Triangle(p1, p2, p3);
            Triangle t1 = new Triangle(p2, p1, p3);
            Triangle t2 = new Triangle(p3, p2, p1);
            Triangle t3 = new Triangle(p3, p2, p4);

            Assert.AreEqual(t0, t1);
            Assert.AreEqual(t0, t2);
            Assert.AreEqual(t2, t1);
            Assert.AreNotEqual(t0, t3);

            Assert.IsTrue(t0 == t1);
            Assert.IsTrue(t0 == t2);
            Assert.IsTrue(t2 == t1);
            Assert.IsFalse(t1 == t3);
            Assert.IsTrue(t1 != t3);

            Dictionary<Triangle, bool> dict = new Dictionary<Triangle,bool>()
            {
                { t0, true },
                { t3, true },
            };
            Assert.IsTrue(dict.ContainsKey(t0));
            Assert.IsTrue(dict.ContainsKey(t1));
            Assert.IsTrue(dict.ContainsKey(t2));
            Assert.IsTrue(dict.ContainsKey(t3));
        }
    }
}
