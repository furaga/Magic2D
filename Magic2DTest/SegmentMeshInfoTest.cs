using Magic2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///SegmentMeshInfoTest のテスト クラスです。すべての
    ///SegmentMeshInfoTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class SegmentMeshInfoTest
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
        ///GetBoneSectionCrossDict のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void GetBoneSectionCrossDictTest()
        {
            var _seg1 = new Segment("seg1", null);
            _seg1.path = circlePoints(100, 100, 30, 20);
            _seg1.section = _seg1.path.Take(5).ToList();
            _seg1.an = new SkeletonAnnotation(null);
            _seg1.an.joints.Add(new JointAnnotation("0", new PointF(100, 100)));
            _seg1.an.joints.Add(new JointAnnotation("1", new PointF(200, 200)));
            _seg1.an.bones.Add(new BoneAnnotation(_seg1.an.joints[0], _seg1.an.joints[1]));

            var dict = SegmentMeshInfo_Accessor.GetBoneSectionCrossDict(_seg1.path, new List<CharacterRange>() { new CharacterRange(0, 5) }, _seg1.an);
            Assert.AreEqual(dict.Count, 1);

            var seg1 = new SegmentMeshInfo(_seg1, false);
            Assert.AreEqual(seg1.crossDict.Count, 1);
        }

        public List<PointF> circlePoints(int cx, int cy, int r, int num)
        {
            List<PointF> pts = new List<PointF>();
            for (int i = 0; i < num; i++)
            {
                double rad = 2 * Math.PI * i / num;
                pts.Add(new PointF(cx + r * (float)Math.Cos(rad), cy + r * (float)Math.Sin(rad)));
            }
            return pts;
        }
    }
}
