﻿using Magic2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///SkeletonAnnotationTest のテスト クラスです。すべての
    ///SkeletonAnnotationTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class SkeletonAnnotationTest
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
        ///SkeletonAnnotation コンストラクター のテスト
        ///</summary>
        [TestMethod()]
        public void SkeletonAnnotationConstructorTest()
        {
            SkeletonAnnotation an = null; // TODO: 適切な値に初期化してください
            bool copyBmp = false; // TODO: 適切な値に初期化してください
            SkeletonAnnotation target = new SkeletonAnnotation(an, true);
            target = new SkeletonAnnotation(an, false);
        }
    }
}
