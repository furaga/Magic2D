using Magic2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///FormRefSkeletonTest のテスト クラスです。すべての
    ///FormRefSkeletonTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class FormRefSkeletonTest
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
        ///AddJointAnnotation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void AddJointAnnotationTest()
        {
            FormRefSkeleton_Accessor target = new FormRefSkeleton_Accessor(); // TODO: 適切な値に初期化してください
            target.Initialize(target.pictureBox1.Image as Bitmap);
            target.AddJointAnnotation(new JointAnnotation("head", new Point(50, 100)));
            Assert.AreEqual(target.an.joints.Count, 1);
            target.AddJointAnnotation(new JointAnnotation("head", new Point(80, 100)));
            Assert.AreEqual(target.an.joints.Count, 2);
            target.AddJointAnnotation(new JointAnnotation("waist", new Point(100, 100)));
            Assert.AreEqual(target.an.joints.Count, 3);
        }

        /// <summary>
        ///DeleteJointAnnotation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void DeleteJointAnnotationTest()
        {
            FormRefSkeleton_Accessor target = new FormRefSkeleton_Accessor(); // TODO: 適切な値に初期化してください
            target.Initialize(target.pictureBox1.Image as Bitmap);
            target.AddJointAnnotation(new JointAnnotation("head", new Point(50, 100)));
            target.AddJointAnnotation(new JointAnnotation("head", new Point(80, 100)));
            target.AddJointAnnotation(new JointAnnotation("waist", new Point(100, 100)));

            target.DeleteJointAnnotation(new JointAnnotation("waist", new Point(100, 100)));
            Assert.AreEqual(target.an.joints.Count, 3);
            target.DeleteJointAnnotation(null);
            Assert.AreEqual(target.an.joints.Count, 3);
            target.DeleteJointAnnotation(target.an.joints[1]);
            Assert.AreEqual(target.an.joints.Count, 2);
            target.DeleteJointAnnotation(target.an.joints[0]);
            Assert.AreEqual(target.an.joints.Count, 1);
            target.DeleteJointAnnotation(target.an.joints[0]);
            Assert.AreEqual(target.an.joints.Count, 0);
            target.DeleteJointAnnotation(new JointAnnotation("waist", new Point(100, 100)));
            Assert.AreEqual(target.an.joints.Count, 0);
            target.DeleteJointAnnotation(null);
            Assert.AreEqual(target.an.joints.Count, 0);
        }

        /// <summary>
        ///SelectJointAnnotation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void SelectJointAnnotationTest()
        {
            FormRefSkeleton_Accessor target = new FormRefSkeleton_Accessor(); // TODO: 適切な値に初期化してください
            target.Initialize(target.pictureBox1.Image as Bitmap);
            target.AddJointAnnotation(new JointAnnotation("head", new Point(50, 100)));
            target.AddJointAnnotation(new JointAnnotation("waist", new Point(100, 100)));
            Assert.AreEqual(target.selectJoint, null);

            target.SelectJointAnnotation(target.an.joints[0]);
            Assert.AreEqual(target.selectJoint, target.an.joints[0]);

            target.SelectJointAnnotation(null);
            Assert.AreEqual(target.selectJoint, null);

            target.Initialize(target.pictureBox1.Image as Bitmap);
            Assert.AreEqual(target.selectJoint, null);
        }

        /// <summary>
        ///Save のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void SaveOpenTest()
        {
            FormRefSkeleton_Accessor target = new FormRefSkeleton_Accessor(); // TODO: 適切な値に初期化してください
            string filepath = "../../../Test2/refSkeleton.skl";
            target.AddJointAnnotation(new JointAnnotation("j0", new PointF(10, 110)));
            target.AddJointAnnotation(new JointAnnotation("j0", new PointF(20, 120)));
            target.AddJointAnnotation(new JointAnnotation("j1", new PointF(30, 130)));
            target.AddJointAnnotation(new JointAnnotation("j2", new PointF(40, 140)));

            target.Save(filepath);
            target.Open(filepath);
            Assert.AreEqual(target.an.joints.Count, 4);

            Assert.AreEqual(target.an.joints[0].name, "j0");
            Assert.AreEqual(target.an.joints[1].name, "j0");
            Assert.AreEqual(target.an.joints[2].name, "j1");
            Assert.AreEqual(target.an.joints[3].name, "j2");

            Assert.AreEqual(target.an.joints[0].position.X, 10, 1e-4f);
            Assert.AreEqual(target.an.joints[1].position.X, 20, 1e-4f);
            Assert.AreEqual(target.an.joints[2].position.X, 30, 1e-4f);
            Assert.AreEqual(target.an.joints[3].position.X, 40, 1e-4f);

            Assert.AreEqual(target.an.joints[0].position.Y, 110, 1e-4f);
            Assert.AreEqual(target.an.joints[1].position.Y, 120, 1e-4f);
            Assert.AreEqual(target.an.joints[2].position.Y, 130, 1e-4f);
            Assert.AreEqual(target.an.joints[3].position.Y, 140, 1e-4f);
        }
    }
}
