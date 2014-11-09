using Magic2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
namespace Magic2DTest
{
    
    
    /// <summary>
    ///SkeletonFittingTest のテスト クラスです。すべての
    ///SkeletonFittingTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class SkeletonFittingTest
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
        ///SkeletonFitting のテスト
        ///</summary>
      //  [TestMethod()]
        public void FittingTest()
        {
            // セグメント
            SegmentMeshInfo target;
            SkeletonAnnotation an = new SkeletonAnnotation(null);

            an.joints = new List<JointAnnotation>()
                {
                    new JointAnnotation("0", new PointF(10, 10)),
                    new JointAnnotation("1", new PointF(110, 110)),
                };
            an.bones = new List<BoneAnnotation>()
                {
                    new BoneAnnotation(an.joints[0], an.joints[1])
                };

            List<PointF> path = Util.circlePoints(0, 0, 1, 20);
            for (int i = 0; i < path.Count; i++)
            {
                path[i] = new PointF(path[i].X * 100, path[i].Y * 50);
            }

            target = null;// new SegmentMeshInfo();
            target.MoveTo(100, 200);
            target.Scale(1.2f, 1.2f);
            target.Rotate(30);

            // フィットさせたいスケルトン
            SkeletonAnnotation refAnnotation = new SkeletonAnnotation(null);
            refAnnotation.joints.Add(new JointAnnotation("0", new System.Drawing.PointF(80, 50)));
            refAnnotation.joints.Add(new JointAnnotation("1", new System.Drawing.PointF(30, 50)));
            refAnnotation.bones.Add(new BoneAnnotation(refAnnotation.joints[0], refAnnotation.joints[1]));

            SkeletonFitting.Fitting(target, refAnnotation);

            Assert.AreEqual(target.position.X, 0, 1e-4);
            Assert.AreEqual(target.position.Y, 0, 1e-4);
            Assert.AreEqual(target.scale.X, 1.2, 1e-4);
            Assert.AreEqual(target.scale.Y, 1.2, 1e-4);
            Assert.AreEqual(target.angle, 0, 1e-4);

            CheckList(target.an.joints.Select(j => j.position).ToList(), new List<PointF>() { new PointF(80, 50), new PointF(30, 50) }, 1e-4);
        }

        void CheckList(List<PointF> p1, List<PointF> p2, double e)
        {
            Assert.AreEqual(p1.Count, p2.Count);
            for (int i = 0; i < p1.Count; i++)
            {
                Assert.AreEqual(p1[i].X, p2[i].X, e);
                Assert.AreEqual(p1[i].Y, p2[i].Y, e);
            }
        }
    }
}
