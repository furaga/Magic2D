using Magic2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///SegmentationTest のテスト クラスです。すべての
    ///SegmentationTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class SegmentationTest
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
        ///SetEditingSegment のテスト
        ///</summary>
        [TestMethod()]
        public void SetGetEditingSegmentTest()
        {
            Segmentation_Accessor target = new Segmentation_Accessor(); // TODO: 適切な値に初期化してください

            Assert.AreEqual(target.GetEditingSegment().name, "");

            target.AssignSegmentRootAs("root", null, null);
            Assert.IsNull(target.GetEditingSegmentRoot());
            
            target.SetEditingSegmentRoot("root");
            Assert.IsNotNull(target.GetEditingSegmentRoot());

            target.SetEditingSegment("Full");
            Assert.AreEqual(target.GetEditingSegment().name, "Full");
        }

    }
}
