using Magic2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using FLib;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Magic2DTest
{


    /// <summary>
    ///Form1Test のテスト クラスです。すべての
    ///Form1Test 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class Form1Test
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
        ///Initialize のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void InitializeTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください

            target.Initialize();
            Assert.AreEqual(target.sourceImageDict.Count, 0);
            Assert.AreEqual(target.segmentImageDict.Count, 0);
            Assert.AreEqual(target.composedImageDict.Count, 0);
            Assert.AreEqual(target.operationHistory.Count, 0);
            Assert.AreEqual(target.operationIndex, -1);
            Assert.AreEqual(target.skeletonFittingCanvasScale, 1, 1e-4);
            Assert.IsNull(target.nearestJoint);
            Assert.IsNull(target.nearestBone);

            AssignImages(target);
            target.skeletonFittingCanvasZoom(1);
            target.skeletonFittingCanvasPan(10, 10);
            target.Initialize();
            Assert.AreEqual(target.sourceImageDict.Count, 0);
            Assert.AreEqual(target.segmentImageDict.Count, 0);
            Assert.AreEqual(target.composedImageDict.Count, 0);
            Assert.AreEqual(target.operationHistory.Count, 0);
            Assert.AreEqual(target.operationIndex, -1);
            Assert.AreEqual(target.skeletonFittingCanvasScale, 1, 1e-4);
            Assert.IsNull(target.nearestJoint);
            Assert.IsNull(target.nearestBone);

        }

        void AssignImages(Form1_Accessor target)
        {
            target.AssignImage(target.sourceImageDict, "src0", new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            target.AssignImage(target.sourceImageDict, "src1", new Bitmap(20, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            target.AssignImage(target.sourceImageDict, "src2", new Bitmap(30, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            target.AssignImage(target.segmentImageDict, "seg0", new Bitmap(10, 20, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            target.AssignImage(target.segmentImageDict, "seg1", new Bitmap(20, 20, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            target.AssignImage(target.composedImageDict, "com0", new Bitmap(10, 30, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
        }

        /// <summary>
        ///RefleshAllImageView のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void RefleshAllImageViewTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            target.Initialize();
            target.RefleshAllImageView();

            Assert.AreEqual(target.sourceImageDict.Count, target.sourceImageList.Images.Count);
            Assert.AreEqual(target.sourceImageDict.Count, target.sourceImageView.Items.Count);

            Assert.AreEqual(target.segmentImageDict.Count, target.segmentImageList.Images.Count);
            Assert.AreEqual(target.segmentImageDict.Count, target.segmentImageView.Items.Count);

            Assert.AreEqual(target.composedImageDict.Count, target.composedImageList.Images.Count);
            Assert.AreEqual(target.composedImageDict.Count, target.composedImageView.Items.Count);
        }

        /// <summary>
        ///AssignSourceImage のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void AssignSourceImageTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください

            target.Initialize();
            Assert.AreEqual(target.sourceImageDict.Count, 0);

            AssignSourceImageTestForImageDict(target, target.sourceImageDict);
            AssignSourceImageTestForImageDict(target, target.segmentImageDict);
            AssignSourceImageTestForImageDict(target, target.composedImageDict);
        }

        void AssignSourceImageTestForImageDict(Form1_Accessor target, Dictionary<string, Bitmap> imageDict)
        {
            target.AssignImage(imageDict, "0", new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            Assert.AreEqual(imageDict.Count, 1);

            target.AssignImage(imageDict, "1", new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            Assert.AreEqual(imageDict.Count, 2);

            target.AssignImage(imageDict, "2", new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            Assert.AreEqual(imageDict.Count, 3);

            target.AssignImage(imageDict, "1", new Bitmap(20, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            Assert.AreEqual(imageDict.Count, 3);
            Assert.AreEqual(imageDict["1"].Width, 20);

            target.AssignImage(imageDict, "0", new Bitmap(20, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            Assert.AreEqual(imageDict.Count, 3);
            Assert.AreEqual(imageDict["0"].Width, 20);

            target.AssignImage(imageDict, "3", new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            Assert.AreEqual(imageDict.Count, 4);
            Assert.AreEqual(imageDict["3"].Width, 10);

            target.AssignImage(imageDict, "0", new Bitmap(30, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            Assert.AreEqual(imageDict.Count, 4);
            Assert.AreEqual(imageDict["0"].Width, 30);
        }


        /// <summary>
        ///GetNewSourceImageKey のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void GetNewSourceImageKeyTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            target.AssignImage(target.sourceImageDict, "scr1", new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            target.AssignImage(target.sourceImageDict, "scr3", new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            string prefix = "scr";

            string key1 = target.GetNewSourceImageKey(prefix);
            Assert.AreEqual(key1, "scr0");

            target.AssignImage(target.sourceImageDict, key1, new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);

            string key2 = target.GetNewSourceImageKey(prefix);
            Assert.AreEqual(key2, "scr2");

            target.AssignImage(target.sourceImageDict, key2, new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);

            string key3 = target.GetNewSourceImageKey(prefix);
            Assert.AreEqual(key3, "scr4");
        }

        /// <summary>
        ///DeleteSourceImage のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void DeleteSourceImageTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください

            target.Initialize();
            Assert.AreEqual(target.sourceImageDict.Count, 0);

            target.DeleteImage(target.sourceImageDict, "0");
            Assert.AreEqual(target.sourceImageDict.Count, 0);

            target.AssignImage(target.sourceImageDict, "0", new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            target.AssignImage(target.sourceImageDict, "1", new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);

            target.DeleteImage(target.sourceImageDict, "1");
            Assert.AreEqual(target.sourceImageDict.Count, 1);

            target.DeleteImage(target.sourceImageDict, "2");
            Assert.AreEqual(target.sourceImageDict.Count, 1);

            target.DeleteImage(target.sourceImageDict, "0");
            Assert.AreEqual(target.sourceImageDict.Count, 0);
        }

        /// <summary>
        ///UpdateImageView のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void UpdateImageViewTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            target.Initialize();
            target.RefleshAllImageView();
            UpdateImageViewTestForImageView(target, target.sourceImageList, target.sourceImageView);
        }

        void UpdateImageViewTestForImageView(Form1_Accessor target, ImageList imageList, ListView imageView)
        {
            var bmp = new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            target.UpdateImageView(new Dictionary<string, Bitmap>
                {
                    {"0" , bmp}, {"1" , bmp},
                }, imageList, imageView, false);
            CheckListViewCount(imageList, imageView, 2);

            target.UpdateImageView(new Dictionary<string, Bitmap>
                {
                    {"0" , bmp}, {"1" , bmp}, {"2" , bmp},
                }, imageList, imageView, false);
            CheckListViewCount(imageList, imageView, 3);

            target.UpdateImageView(new Dictionary<string, Bitmap>
                {
                    {"2" , bmp},
                }, target.sourceImageList, target.sourceImageView, true);
            CheckListViewCount(imageList, imageView, 1);
        }

        void CheckListViewCount(ImageList imageList, ListView imageView, int count)
        {
            Assert.AreEqual(imageList.Images.Count, count);
            Assert.AreEqual(imageView.Items.Count, count);
        }
        void CheckListViewContainsKey(ImageList imageList, ListView imageView, string key)
        {
            Assert.IsTrue(imageList.Images.ContainsKey(key));
            bool contains = false;
            for (int i = 0; i < imageView.Items.Count; i++)
            {
                if (imageView.Items[i].ImageKey == key)
                    contains = true;
            }
            Assert.IsTrue(contains);
        }


        /// <summary>
        ///Undo のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void UndoRedoTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            Dictionary<string, Bitmap> imageDict = target.sourceImageDict;
            var bmp = new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            target.Initialize();

            // 空
            target.Undo();
            CheckOperationHistoryIndex(target, 0, -1);
            target.Redo();
            CheckOperationHistoryIndex(target, 0, -1);

            // コマンド実行
            target.AssignImage(imageDict, "0", new Bitmap(bmp), true);
            target.AssignImage(imageDict, "1", new Bitmap(bmp), true);
            target.DeleteImage(imageDict, "0", null, true);
            Assert.AreEqual(imageDict.Count, 1);
            Assert.IsTrue(imageDict.All(kv => kv.Value.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            CheckOperationHistoryIndex(target, 3, 2);

            // Undo, Redo繰り返す
            target.Undo();
            Assert.AreEqual(imageDict.Count, 2);
            Assert.IsTrue(imageDict.All(kv => kv.Value.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            CheckOperationHistoryIndex(target, 3, 1);

            target.Undo();
            Assert.AreEqual(imageDict.Count, 1);
            Assert.IsTrue(imageDict.All(kv => kv.Value.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            CheckOperationHistoryIndex(target, 3, 0);

            target.Redo();
            Assert.AreEqual(imageDict.Count, 2);
            Assert.IsTrue(imageDict.All(kv => kv.Value.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            CheckOperationHistoryIndex(target, 3, 1);

            target.Redo();
            Assert.AreEqual(imageDict.Count, 1);
            Assert.IsTrue(imageDict.All(kv => kv.Value.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            CheckOperationHistoryIndex(target, 3, 2);

            target.Redo();
            Assert.AreEqual(imageDict.Count, 1);
            Assert.IsTrue(imageDict.All(kv => kv.Value.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            CheckOperationHistoryIndex(target, 3, 2);

            target.Redo();
            Assert.IsTrue(imageDict.All(kv => kv.Value.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            target.Undo();
            Assert.AreEqual(imageDict.Count, 2);
            Assert.IsTrue(imageDict.All(kv => kv.Value.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            target.Undo();
            Assert.AreEqual(imageDict.Count, 1);
            Assert.IsTrue(imageDict.All(kv => kv.Value.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            target.Undo();
            Assert.AreEqual(imageDict.Count, 0);
            Assert.IsTrue(imageDict.All(kv => kv.Value.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            target.Undo();
            Assert.AreEqual(imageDict.Count, 0);
            Assert.IsTrue(imageDict.All(kv => kv.Value.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            CheckOperationHistoryIndex(target, 3, -1);

            target.AssignImage(imageDict, "2", new Bitmap(bmp), true);
            Assert.AreEqual(imageDict.Count, 1);
            Assert.IsTrue(imageDict.All(kv => kv.Value.PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            CheckOperationHistoryIndex(target, 1, 0);
        }


        /// <summary>
        ///Undo のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void UndoRedoTest2()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            Dictionary<string, Bitmap> imageDict = target.sourceImageDict;
            var bmp = new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            target.Initialize();

            // コマンド実行
            target.AssignImage(imageDict, "0", new Bitmap(bmp), true);
            target.AssignImage(imageDict, "1", new Bitmap(bmp), true);

            target.SetEditingAnnotation("1", true);

            var an = target.skeletonAnnotationDict["1"];
            target.AssignJointAnnotation(an, new JointAnnotation("j0", new PointF(50, 50)), true);
            target.AssignJointAnnotation(an, new JointAnnotation("j1", new PointF(50, 100)), true);
            target.AssignBoneAnnotation(an, new BoneAnnotation(an.joints[0], an.joints[1]), true);
            target.DeleteBoneAnnotation(an, new BoneAnnotation(an.joints[0], an.joints[1]), true);

            target.DeleteImage(imageDict, "0", null, true);
            target.AssignJointAnnotation(an, new JointAnnotation("j2", new PointF(100, 100)), true);
            target.AssignBoneAnnotation(an, new BoneAnnotation(an.joints[0], an.joints[1]), true);
            target.AssignBoneAnnotation(an, new BoneAnnotation(an.joints[1], an.joints[2]), true);
            target.DeleteJointAnnotation(an, new JointAnnotation("j1", new PointF(50, 100)), true);

            for (int i = 0; i < 11; i++)
            {
                target.Undo();
            }
            for (int i = 0; i < 11; i++)
            {
                target.Redo();
            }
        }


        void CheckOperationHistoryIndex(Form1_Accessor target, int hist, int idx)
        {
            Assert.IsTrue(target.operationHistory.All(op => op.funcName != "DeleteImage" || (op.parameters[2] as Bitmap).PixelFormat != System.Drawing.Imaging.PixelFormat.Undefined));
            Assert.AreEqual(target.operationHistory.Count, hist);
            Assert.AreEqual(target.operationIndex, idx);
        }

        /// <summary>
        ///AddOperation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void AddOperationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            var bmp = new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            target.AddOperation(null);
            Assert.AreEqual(target.operationHistory.Count, 0);
            Assert.AreEqual(target.operationIndex, -1);

            target.AddOperation(new Operation { funcName = "f1", parameters = new List<object>() { new System.Drawing.Bitmap(bmp) } });
            Assert.AreEqual(target.operationHistory.Count, 1);
            Assert.AreEqual(target.operationIndex, 0);

            target.AddOperation(new Operation { funcName = "f2", parameters = new List<object>() { new System.Drawing.Bitmap(bmp) } });
            Assert.AreEqual(target.operationHistory.Count, 2);
            Assert.AreEqual(target.operationIndex, 1);

            target.operationIndex = 0;
            target.AddOperation(new Operation { funcName = "f3", parameters = new List<object>() { new System.Drawing.Bitmap(bmp) } });
            Assert.AreEqual(target.operationHistory.Count, 2);
            Assert.AreEqual(target.operationIndex, 1);

            target.operationIndex = -1;
            target.AddOperation(new Operation { funcName = "f4", parameters = new List<object>() { new System.Drawing.Bitmap(bmp) } });
            Assert.AreEqual(target.operationHistory.Count, 1);
            Assert.AreEqual(target.operationIndex, 0);
        }

        /// <summary>
        ///SaveProject のテスト
        ///</summary>
        [TestMethod()]
        public void SaveProjectTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください

            AssignImages(target);

            string filepath = "../../../Test2/dummyProject.m2d";
            string srcDir = "../../../Test2/1_sourceImages/";
            string comDir = "../../../Test2/4_composedImages/";

            for (int i = 0; i < 2; i++)
            {
                target.SaveProject(filepath);
                Assert.IsTrue(File.Exists(filepath));
                Assert.IsTrue(Directory.Exists(srcDir));
                Assert.IsTrue(Directory.Exists(comDir));
                Assert.AreEqual(Directory.GetFiles(srcDir).Length, 3);
                Assert.AreEqual(Directory.GetFiles(comDir).Length, 1);
            }

            target.DeleteImage(target.sourceImageDict, "src1", null, true);
            target.AssignImage(target.composedImageDict, "com1", new Bitmap(20, 30, System.Drawing.Imaging.PixelFormat.Format32bppArgb), true);
            Assert.AreNotEqual(target.sourceImageDict.Count, 3);
            Assert.AreNotEqual(target.composedImageDict.Count, 1);
            Assert.AreNotEqual(target.operationHistory.Count, 0);
            Assert.AreNotEqual(target.operationIndex, -1);

            for (int i = 0; i < 2; i++)
            {
                target.OpenProject(filepath);
                Assert.AreEqual(target.sourceImageDict.Count, 3);
                Assert.AreEqual(target.composedImageDict.Count, 1);
                Assert.AreEqual(target.operationHistory.Count, 0);
                Assert.AreEqual(target.operationIndex, -1);
            }
        }

        /// <summary>
        ///SetEditingAnnotation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void SetEditingAnnotationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください

            target.Initialize();
            AssignImages(target);

            target.SetEditingAnnotation("src0");
            Assert.AreEqual(target.editingAnnotationKey, "src0");
            Assert.AreEqual(target.skeletonAnnotationDict["src0"].bmp.Size, target.sourceImageDict["src0"].Size);

            target.SetEditingAnnotation("src2");
            Assert.AreEqual(target.editingAnnotationKey, "src2");
            Assert.AreEqual(target.skeletonAnnotationDict["src2"].bmp.Size, target.sourceImageDict["src2"].Size);
        }

        /// <summary>
        ///GetEditingAnnotation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void GetEditingAnnotationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            Assert.AreEqual(target.GetEditingAnnotation(), null);
            target.skeletonAnnotationDict["an0"] = CreateSkeletonAnnotation();
            target.skeletonAnnotationDict["an1"] = CreateSkeletonAnnotation();
            target.AssignImage(target.sourceImageDict, "an0", new Bitmap(100, 100, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            target.AssignImage(target.sourceImageDict, "an1", new Bitmap(100, 100, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            target.SetEditingAnnotation("an0");
            Assert.AreEqual(target.GetEditingAnnotation(), target.skeletonAnnotationDict["an0"]);
            target.SetEditingAnnotation("an1");
            Assert.AreEqual(target.GetEditingAnnotation(), target.skeletonAnnotationDict["an1"]);
        }

        SkeletonAnnotation CreateSkeletonAnnotation()
        {
            var bmp = new Bitmap(100, 100, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            var an = new SkeletonAnnotation(bmp);
            an.joints = new List<JointAnnotation>()
            {
                new JointAnnotation("head", new PointF(50, 20)),
                new JointAnnotation("waist", new PointF(50, 60)),
                new JointAnnotation("lfoot", new PointF(30, 80)),
                new JointAnnotation("rfoot", new PointF(70, 80)),
            };
            an.bones = new List<BoneAnnotation>()
            {
                new BoneAnnotation(an.joints[1], an.joints[0]),
                new BoneAnnotation(an.joints[1], an.joints[2]),
                new BoneAnnotation(an.joints[1], an.joints[3]),
            };
            return an;
        }

        /// <summary>
        ///AssignJointAnnotation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void AssignJointAnnotationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            target.Initialize();

            SkeletonAnnotation an = CreateSkeletonAnnotation();
            Assert.AreEqual(an.joints.Count, 4);

            target.AssignJointAnnotation(an, new JointAnnotation("j0", new PointF(100, 100)), true);
            Assert.AreEqual(an.joints.Count, 5);

            target.AssignJointAnnotation(an, new JointAnnotation("head", new PointF(50, 20)), true);
            Assert.AreEqual(an.joints.Count, 5);

            target.AssignJointAnnotation(an, new JointAnnotation("", new PointF(100, 100)), true);
            Assert.AreEqual(an.joints.Count, 6);

            target.AssignJointAnnotation(an, new JointAnnotation("j0", new PointF(120, 100)), true);
            Assert.AreEqual(an.joints.Count, 7);

        }

        /// <summary>
        ///AssignBoneAnnotation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void AssignBoneAnnotationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            target.Initialize();

            SkeletonAnnotation an = CreateSkeletonAnnotation();
            Assert.AreEqual(an.bones.Count, 3);

            target.AssignBoneAnnotation(an, new BoneAnnotation(an.joints[2], an.joints[0]), true);
            Assert.AreEqual(an.bones.Count, 4);

            target.AssignBoneAnnotation(an, new BoneAnnotation(an.joints[1], an.joints[0]), true);
            Assert.AreEqual(an.bones.Count, 4);

            target.AssignBoneAnnotation(an, new BoneAnnotation(an.joints[0], an.joints[1]), true);
            Assert.AreEqual(an.bones.Count, 4);

            target.AssignBoneAnnotation(an, new BoneAnnotation(an.joints[1], an.joints[1]), true);
            Assert.AreEqual(an.bones.Count, 4);

            target.AssignBoneAnnotation(an, new BoneAnnotation(an.joints[3], an.joints[0]), true);
            Assert.AreEqual(an.bones.Count, 5);
        }

        /// <summary>
        ///CreateOrCompleteEditingBone のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void CreateOrCompleteEditingBoneTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            SkeletonAnnotation an = CreateSkeletonAnnotation(); // TODO: 適切な値に初期化してください

            target.Initialize();

            target.CreateOrCompleteEditingBone(an, an.joints[0]);
            Assert.AreEqual(target.addingBone.src, an.joints[0]);
            Assert.AreEqual(an.bones.Count, 3);

            target.CreateOrCompleteEditingBone(an, an.joints[3]);
            Assert.AreEqual(target.addingBone.src, an.joints[3]);
            Assert.AreEqual(an.bones.Count, 4);
            Assert.AreEqual(an.bones[3].src.name, an.joints[0].name);
            Assert.AreEqual(an.bones[3].dst.name, an.joints[3].name);

            target.CreateOrCompleteEditingBone(an, an.joints[2]);
            Assert.AreEqual(target.addingBone.src, an.joints[2]);
            Assert.AreEqual(an.bones.Count, 5);
            Assert.AreEqual(an.bones[4].src.name, an.joints[3].name);
            Assert.AreEqual(an.bones[4].dst.name, an.joints[2].name);

            target.CreateOrCompleteEditingBone(an, an.joints[2]);
            Assert.AreEqual(target.addingBone.src, an.joints[2]);
            Assert.AreEqual(an.bones.Count, 5);

            target.CreateOrCompleteEditingBone(an, an.joints[3]);
            Assert.AreEqual(target.addingBone.src, an.joints[3]);
            Assert.AreEqual(an.bones.Count, 5);
        }

        /// <summary>
        ///DeleteBoneAnnotation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void DeleteBoneAnnotationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            SkeletonAnnotation an = CreateSkeletonAnnotation();
            Assert.AreEqual(an.bones.Count, 3);
            target.DeleteBoneAnnotation(an, new BoneAnnotation(an.joints[0], an.joints[1]), true);
            Assert.AreEqual(an.bones.Count, 3);
            target.DeleteBoneAnnotation(an, new BoneAnnotation(an.joints[1], an.joints[0]), true);
            Assert.AreEqual(an.bones.Count, 2);
            target.DeleteBoneAnnotation(an, new BoneAnnotation(an.joints[1], an.joints[2]), true);
            Assert.AreEqual(an.bones.Count, 1);
            target.DeleteBoneAnnotation(an, new BoneAnnotation(an.joints[1], an.joints[3]), true);
            Assert.AreEqual(an.bones.Count, 0);

            target.DeleteBoneAnnotation(an, new BoneAnnotation(an.joints[1], an.joints[0]), true);
            target.DeleteBoneAnnotation(an, new BoneAnnotation(an.joints[1], an.joints[2]), true);
            target.DeleteBoneAnnotation(an, new BoneAnnotation(an.joints[1], an.joints[3]), true);
            Assert.AreEqual(an.bones.Count, 0);
        }

        /// <summary>
        ///DeleteEditingBone のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void DeleteEditingBoneTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            var an = CreateSkeletonAnnotation();

            target.Initialize();
            Assert.IsNull(target.addingBone);

            target.AssignImage(target.sourceImageDict, "an0", new Bitmap(100, 100, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            target.CreateOrCompleteEditingBone(an, an.joints[0]);
            Assert.IsNotNull(target.addingBone);

            target.DeleteEditingBone();
            Assert.IsNull(target.addingBone);
        }

        /// <summary>
        ///DeleteJointAnnotation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void DeleteJointAnnotationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            var an = CreateSkeletonAnnotation();
            Assert.AreEqual(an.joints.Count, 4);
            Assert.AreEqual(an.bones.Count, 3);

            target.DeleteJointAnnotation(an, an.joints[1], true);
            Assert.AreEqual(an.joints.Count, 3);
            Assert.AreEqual(an.bones.Count, 0);

            target.DeleteJointAnnotation(an, null, true);
            Assert.AreEqual(an.joints.Count, 3);
            target.DeleteJointAnnotation(an, new JointAnnotation("", PointF.Empty), true);
            Assert.AreEqual(an.joints.Count, 3);
            target.DeleteJointAnnotation(an, an.joints[0], true);
            Assert.AreEqual(an.joints.Count, 2);
            target.DeleteJointAnnotation(an, an.joints[0], true);
            Assert.AreEqual(an.joints.Count, 1);
            target.DeleteJointAnnotation(an, an.joints[0], true);
            Assert.AreEqual(an.joints.Count, 0);
            target.DeleteJointAnnotation(an, new JointAnnotation("", PointF.Empty), true);
            Assert.AreEqual(an.joints.Count, 0);
        }

        /// <summary>
        ///GetNearestJoint のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void GetNearestJointTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            SkeletonAnnotation an = CreateSkeletonAnnotation();

            Matrix transform = target.skeletonFittingCanvasTransform;
            PointF p0 = new PointF(an.joints[0].position.X + 10, an.joints[0].position.Y);
            PointF p1 = new PointF(an.joints[1].position.X + 2, an.joints[1].position.Y - 2);
            PointF p2 = new PointF(an.joints[1].position.X + 2, an.joints[0].position.Y - 2);
            PointF p3 = new PointF(50f, an.joints[2].position.Y + 20);
            Assert.AreEqual(an.GetNearestJoint(p0, 11, transform).name, an.joints[0].name);
            Assert.AreEqual(an.GetNearestJoint(p1, 3, transform).name, an.joints[1].name);
            Assert.AreEqual(an.GetNearestJoint(p2, 2, transform), null);
            Assert.AreEqual(an.GetNearestJoint(p3, 30, transform).name, an.joints[2].name);

            target.skeletonFittingCanvasZoom(1);
            p0 = new PointF(p0.X * 2, p0.Y * 2);
            p1 = new PointF(p1.X * 2, p1.Y * 2);
            p2 = new PointF(p2.X * 2, p2.Y * 2);
            p3 = new PointF(p3.X * 2, p3.Y * 2);
            transform = target.skeletonFittingCanvasTransform;
            Assert.AreEqual(an.GetNearestJoint(p0, 22, transform).name, an.joints[0].name);
            Assert.AreEqual(an.GetNearestJoint(p1, 6, transform).name, an.joints[1].name);
            Assert.AreEqual(an.GetNearestJoint(p2, 4, transform), null);
            Assert.AreEqual(an.GetNearestJoint(p3, 60, transform).name, an.joints[2].name);
        }

        /// <summary>
        ///UpdateEditingBone のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void UpdateEditingBoneTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            SkeletonAnnotation an = CreateSkeletonAnnotation();
            var j0 = new JointAnnotation("[dummy]", new PointF(10, 10));
            var j1 = new JointAnnotation("[dummy]", new PointF(10, 100));
            target.UpdateEditingBone(an, j0);
            Assert.AreEqual(target.addingBone, null);

            target.CreateOrCompleteEditingBone(an, an.joints[0]);
            target.UpdateEditingBone(an, j0);
            Assert.AreEqual(target.addingBone.dst, j0);

            target.UpdateEditingBone(an, j1);
            Assert.AreEqual(target.addingBone.dst, j1);
        }

        /// <summary>
        ///GetNearestBone のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void GetNearestBoneTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            SkeletonAnnotation an = CreateSkeletonAnnotation();

            Assert.AreEqual(an.GetNearestBone(new Point(50, 20), 20, target.skeletonFittingCanvasTransform), an.bones[0]);
            Assert.AreEqual(an.GetNearestBone(new Point(29, 20), 20, target.skeletonFittingCanvasTransform), null);
            Assert.AreEqual(an.GetNearestBone(new Point(40, 80), 9, target.skeletonFittingCanvasTransform), an.bones[1]);
            Assert.AreEqual(an.GetNearestBone(new Point(60, 80), 9, target.skeletonFittingCanvasTransform), an.bones[2]);
            Assert.AreEqual(an.GetNearestBone(new Point(50, 80), 100, target.skeletonFittingCanvasTransform), an.bones[1]);

            target.skeletonFittingCanvasZoom(1);
            Assert.AreEqual(an.GetNearestBone(new Point(100, 40), 40, target.skeletonFittingCanvasTransform), an.bones[0]);
            Assert.AreEqual(an.GetNearestBone(new Point(59, 40), 40, target.skeletonFittingCanvasTransform), null);
            Assert.AreEqual(an.GetNearestBone(new Point(80, 160), 18, target.skeletonFittingCanvasTransform), an.bones[1]);
            Assert.AreEqual(an.GetNearestBone(new Point(120, 160), 18, target.skeletonFittingCanvasTransform), an.bones[2]);
            Assert.AreEqual(an.GetNearestBone(new Point(100, 160), 200, target.skeletonFittingCanvasTransform), an.bones[1]);
        }

        /// <summary>
        ///SelectBoneAnnotation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void SelectBoneAnnotationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            SkeletonAnnotation an = CreateSkeletonAnnotation();
            target.SelectBoneAnnotation(an, null);
            Assert.AreEqual(target.addingBone, null);
            Assert.AreEqual(target.selectJoint, null);
            Assert.AreEqual(target.selectBone, null);

            target.SelectBoneAnnotation(an, an.bones[0]);
            Assert.AreEqual(target.addingBone, null);
            Assert.AreEqual(target.selectJoint, null);
            Assert.AreEqual(target.selectBone, an.bones[0]);

            target.SelectBoneAnnotation(an, new BoneAnnotation(an.joints[1], an.joints[2]));
            Assert.AreEqual(target.addingBone, null);
            Assert.AreEqual(target.selectJoint, null);
            //仕様変更：2014/06/25
            //Assert.AreEqual(target.selectBone, null);
            Assert.AreEqual(target.selectBone, an.bones[1]);
        }

        /// <summary>
        ///SelectJointAnnotation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void SelectJointAnnotationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            SkeletonAnnotation an = CreateSkeletonAnnotation();

            target.SelectJointAnnotation(an, null);
            Assert.AreEqual(target.addingBone, null);
            Assert.AreEqual(target.selectJoint, null);
            Assert.AreEqual(target.selectBone, null);

            target.SelectJointAnnotation(an, an.joints[2]);
            Assert.AreEqual(target.addingBone, null);
            Assert.AreEqual(target.selectJoint, an.joints[2]);
            Assert.AreEqual(target.selectBone, null);

            target.SelectJointAnnotation(an, new JointAnnotation(an.joints[1].name, an.joints[1].position));
            Assert.AreEqual(target.addingBone, null);
            Assert.AreEqual(target.selectJoint, null);
            Assert.AreEqual(target.selectBone, null);
        }

        /// <summary>
        ///GetDistanceToLine のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void GetDistanceToLineTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください

            float actual = FMath.GetDistanceToLine(new Point(10, 10), new Point(0, 0), new Point(0, 20));
            Assert.AreEqual(actual, 10, 0.01f);

            actual = FMath.GetDistanceToLine(new Point(10, 10), new Point(0, 0), new Point(20, 0));
            Assert.AreEqual(actual, 10, 0.01f);

            actual = FMath.GetDistanceToLine(new Point(10, 10), new Point(0, 0), new Point(20, 20));
            Assert.AreEqual(actual, 0, 0.01f);

            actual = FMath.GetDistanceToLine(new Point(10, 10), new Point(0, 0), new Point(-20, 20));
            Assert.AreEqual(actual, 10 * Math.Sqrt(2), 0.01f);

            actual = FMath.GetDistanceToLine(new Point(10, 10), new Point(-1, 1), new Point(-20, 20));
            Assert.AreEqual(actual, float.MaxValue);
        }


        /// <summary>
        ///skeletonFittingCanvasZoom のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void skeletonFittingCanvasZoomTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください


            target.Initialize();
            PointF actual;

            actual = Form1.InvertCoordinate(Point.Empty, target.skeletonFittingCanvasTransform);
            Assert.AreEqual(actual.X, 0, 1e-4);
            Assert.AreEqual(actual.Y, 0, 1e-4);

            target.skeletonFittingCanvasPan(100, 200);
            actual = Form1.InvertCoordinate(new Point(100, 200), target.skeletonFittingCanvasTransform);
            Assert.AreEqual(actual.X, 0, 1e-4);
            Assert.AreEqual(actual.Y, 0, 1e-4);

            target.skeletonFittingCanvasZoom(1f);
            actual = Form1.InvertCoordinate(new Point(200, 400), target.skeletonFittingCanvasTransform);
            Assert.AreEqual(actual.X, 0, 1e-4);
            Assert.AreEqual(actual.Y, 0, 1e-4);

            target.skeletonFittingCanvasZoom(-2f);
            actual = Form1.InvertCoordinate(new Point(10, 20), target.skeletonFittingCanvasTransform);
            Assert.AreEqual(actual.X, 0, 1e-4);
            Assert.AreEqual(actual.Y, 0, 1e-4);

            target.skeletonFittingCanvasZoom(20f);
            actual = Form1.InvertCoordinate(new Point(1500, 3000), target.skeletonFittingCanvasTransform);
            Assert.AreEqual(actual.X, 0, 1e-4);
            Assert.AreEqual(actual.Y, 0, 1e-4);

        }

        /// <summary>
        ///OpenRefSkeleton のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void OpenRefSkeletonTest()
        {
            var an = Form1.OpenRefSkeleton("../../../Test/refSkeleton.skl", null);
            Assert.IsNotNull(an);
            Assert.AreNotEqual(an.joints.Count, 0);
        }

        /// <summary>
        ///SaveAnnotations OpenAnnotations のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void SaveOpenAnnotationsTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            string root = "../../../Test2/"; // TODO: 適切な値に初期化してください
            string dirName = "2_skeletonAnnotations"; // TODO: 適切な値に初期化してください
            Dictionary<string, SkeletonAnnotation> anDict = new Dictionary<string,SkeletonAnnotation>(); // TODO: 適切な値に初期化してください

            var an0 = CreateSkeletonAnnotation();
            var an1 = CreateSkeletonAnnotation();

            anDict["an0"] = an0;
            anDict["an1"] = an1;

            target.SaveAnnotations(root, dirName, anDict);

            anDict.Clear();
            anDict["an2"] = CreateSkeletonAnnotation();

            Form1.OpenAnnotations(root, dirName, anDict);
            Assert.AreEqual(anDict.Count, 2);
            Assert.IsTrue(anDict.ContainsKey("an0"));
            Assert.IsTrue(anDict.ContainsKey("an1"));
            for (int i = 0; i < anDict.Count; i++)
            {
                var an = anDict.Values.ElementAt(i);
                Assert.IsNotNull(an.bmp);
                Assert.AreEqual(an.bmp.Width, an0.bmp.Width);
                Assert.AreEqual(an.bmp.Height, an0.bmp.Height);
                Assert.AreEqual(an.joints.Count, an0.joints.Count);
                Assert.AreEqual(an.bones.Count, an0.bones.Count);
                for (int j = 0; j < an.joints.Count; j++)
                {
                    Assert.AreEqual(an.joints[j].name, an0.joints[j].name);
                    Assert.AreEqual(an.joints[j].position.X, an0.joints[j].position.X, 1e-4f);
                    Assert.AreEqual(an.joints[j].position.Y, an0.joints[j].position.Y, 1e-4f);
                }
                for (int j = 0; j < an.bones.Count; j++)
                {
                    Assert.AreEqual(an.joints.IndexOf(an.bones[j].src), an0.joints.IndexOf(an0.bones[j].src));
                    Assert.AreEqual(an.joints.IndexOf(an.bones[j].dst), an0.joints.IndexOf(an0.bones[j].dst));
                }
            }
        }

        /// <summary>
        ///DeleteAnimeCells のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void DeleteAnimeCellsTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            AnimeCell c0 = new AnimeCell("0");
            AnimeCell c1 = new AnimeCell("1");
            AnimeCell c2 = new AnimeCell("2");
            AnimeCell c3 = new AnimeCell("3");
            
            target.AssignAnimeCells(new List<AnimeCell>() { c0, c1, c2, c3 }, null);

            target.DeleteAnimeCells(new List<AnimeCell>() { c1, c3 }, true);
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { c0, c2 });
            
            target.DeleteAnimeCells(new List<AnimeCell>() { c0, c2, c3 }, true);
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { });
            
            target.DeleteAnimeCells(new List<AnimeCell>() { c1, null }, true);
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { });
        }

        /// <summary>
        ///AssignAnimeCells のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void AssignAnimeCellsTest1()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            AnimeCell c0 = new AnimeCell("0");
            AnimeCell c1 = new AnimeCell("1");
            AnimeCell c2 = new AnimeCell("2");
            AnimeCell c3 = new AnimeCell("3");
            AnimeCell c4 = new AnimeCell("4");
            AnimeCell c5 = new AnimeCell("5");
            AnimeCell c6 = new AnimeCell("6");
            AnimeCell c7 = new AnimeCell("7");
            target.AssignAnimeCells(new List<AnimeCell>() { c0, c1 }, null);
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { c0, c1 });

            target.AssignAnimeCells(new List<AnimeCell>() { c2, c3 }, c1);
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { c0, c1, c2, c3 });
            
            target.AssignAnimeCells(new List<AnimeCell>() { c4, c5 }, c2);
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { c0, c1, c2, c4, c5, c3 });

            target.AssignAnimeCells(new List<AnimeCell>() { c6, c7 }, new List<AnimeCell>() { c0, c4 }, true);
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { c0, c6, c1, c2, c4, c7, c5, c3 });
        }

        void CheckAnimeCellsOrder(Form1_Accessor target,  List<AnimeCell> order)
        {
            Assert.AreEqual(target.animeCells.Count, order.Count);
            for (int i = 0; i < order.Count; i++)
                Assert.AreEqual(target.animeCells[i].key, order[i].key);
        }

        /// <summary>
        ///SetAddingCells のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void SetAddingCellsTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください

            target.Initialize();
            target.SetAddingCells(new List<string>() { "key0", "key1", } );
            Assert.AreEqual(target.addingCells.Count, 2);

            target.Initialize();
            Assert.AreEqual(target.addingCells.Count, 0);
        }
        
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void UndoRedoAnimeCellsTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            AnimeCell c0 = new AnimeCell("0");
            AnimeCell c1 = new AnimeCell("1");
            AnimeCell c2 = new AnimeCell("2");
            AnimeCell c3 = new AnimeCell("3");

            target.Initialize();

            target.AssignAnimeCells(new List<AnimeCell>() { c0, c1 }, null);
            target.AssignAnimeCells(new List<AnimeCell>() { c2 }, null);
            target.DeleteAnimeCells(new List<AnimeCell>() { c0, c1, c2 }, true);
            target.DeleteAnimeCells(new List<AnimeCell>() { c0 }, true);
            target.AssignAnimeCells(new List<AnimeCell>() { c3 }, null);
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { c3 });

            target.Undo();
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { });

            target.Undo();
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { c2, c0, c1 });

            target.Undo();
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { c0, c1 });

            target.Undo(); target.Undo();
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { });

            target.Redo();
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { c0, c1 });

            target.Redo();
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { c2, c0, c1 });

            target.Redo();
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { });

            target.Redo(); target.Redo();
            CheckAnimeCellsOrder(target, new List<AnimeCell>() { c3 });
        
        }

        /// <summary>
        ///GetPrevCellFromPoint のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void GetPrevCellFromPointTest()
        {
//            94 57
            AnimeCell c0 = new AnimeCell("0");
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            List<AnimeCell> cells = new List<AnimeCell>(); // TODO: 適切な値に初期化してください
            for (int i = 0; i < 5; i++) cells.Add(c0);
            Assert.AreEqual(target.GetPrevCellFromPoint(cells, 0, 0, 200, 200), -1);
            Assert.AreEqual(target.GetPrevCellFromPoint(cells, -1, 0, 200, 200), -2);
            Assert.AreEqual(target.GetPrevCellFromPoint(cells, 0, -1, 200, 200), -2);
            Assert.AreEqual(target.GetPrevCellFromPoint(cells, 201, 0, 200, 200), -2);
            Assert.AreEqual(target.GetPrevCellFromPoint(cells, 0, 201, 200, 200), -2);
            Assert.AreEqual(target.GetPrevCellFromPoint(cells, 91, 30, 200, 200), -1);
            Assert.AreEqual(target.GetPrevCellFromPoint(cells, 100, 70, 200, 200), 2);
            Assert.AreEqual(target.GetPrevCellFromPoint(cells, 180, 100, 200, 200), 2);
            Assert.AreEqual(target.GetPrevCellFromPoint(cells, 180, 180, 200, 200), 4);
        }

        /// <summary>
        ///OpenAnimation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void OpenAnimationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            string root = "../../../Test2/"; // TODO: 適切な値に初期化してください
            string dirName = "5_animation"; // TODO: 適切な値に初期化してください
            AnimeCell c0 = new AnimeCell("0", 10);
            AnimeCell c1 = new AnimeCell("1", 11);
            AnimeCell c2 = new AnimeCell("2", 12);

            target.AssignAnimeCells(new List<AnimeCell>() { c0, c1, c2 }, null);
            Assert.AreEqual(target.animeCells.Count, 3);

            target.SaveAnimation(root, dirName, target.animeCells);

            target.animeCells.Clear();
            Assert.AreEqual(target.animeCells.Count, 0);

            target.OpenAnimation(root, dirName, target.animeCells);
            Assert.AreEqual(target.animeCells.Count, 0);

            target.AssignImage(target.composedImageDict, "0", new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            target.AssignImage(target.composedImageDict, "1", new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            target.AssignImage(target.composedImageDict, "2", new Bitmap(10, 10, System.Drawing.Imaging.PixelFormat.Format32bppArgb));
            target.OpenAnimation(root, dirName, target.animeCells);
            Assert.AreEqual(target.animeCells.Count, 3);
            Assert.AreEqual(target.animeCells[0].key, "0");
            Assert.AreEqual(target.animeCells[0].durationMilliseconds, 10);
            Assert.AreEqual(target.animeCells[1].key, "1");
            Assert.AreEqual(target.animeCells[1].durationMilliseconds, 11);
            Assert.AreEqual(target.animeCells[2].key, "2");
            Assert.AreEqual(target.animeCells[2].durationMilliseconds, 12);
        }

        /// <summary>
        ///OpenAnimation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void UndoRedoSegmentationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
//            target.segment = new Segmentation();
            // todo
        }

        /// <summary>
        ///SetVisibleSegmentKeys のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void SetVisibleSegmentKeysTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            SegmentRoot root = null; // TODO: 適切な値に初期化してください
            target.InitVisibleSegmentKeys(root);
            Assert.AreEqual(target.visibleSegmentKeys.Count, 0);

            // "", "Full"がデフォで入る
            root = new SegmentRoot(null, null);
            target.InitVisibleSegmentKeys(root);
            Assert.AreEqual(target.visibleSegmentKeys.Count, 2);

            root.segments.Add(new PathSegment("0", root));
            target.InitVisibleSegmentKeys(root);
            Assert.AreEqual(target.visibleSegmentKeys.Count, 3);

            root.segments.Add(new PathSegment("1", root));
            target.InitVisibleSegmentKeys(root);
            Assert.AreEqual(target.visibleSegmentKeys.Count, 4);

            target.InitVisibleSegmentKeys(root);
            Assert.AreEqual(target.visibleSegmentKeys.Count, 4);
        }

        /// <summary>
        ///GetSegmentOperation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void GetSegmentOperationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください

            target.segmentSegmentButton.Checked = true;
            Assert.AreEqual(target.GetSegmentOperation(), SegmentOperation.Segment);

            target.segmentPartingButton.Checked = true;
            Assert.AreEqual(target.GetSegmentOperation(), SegmentOperation.PartingLine);

            target.segmentSectionButton.Checked = true;
            Assert.AreEqual(target.GetSegmentOperation(), SegmentOperation.Section);

            target.segmentSkeletonButton.Checked = true;
            Assert.AreEqual(target.GetSegmentOperation(), SegmentOperation.SkeletonAnnotation);
        }

        /// <summary>
        ///OpenSegmentation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void OpenSegmentationTest()
        {
            Form1_Accessor target = new Form1_Accessor(); // TODO: 適切な値に初期化してください
            string root = "../../../Test2/"; // TODO: 適切な値に初期化してください
            string dirName = "3_segmentation"; // TODO: 適切な値に初期化してください

            var segmentation = CreateSegmentation();
            target.SaveSegmentation(root, dirName, segmentation);
            segmentation = new Segmentation();
            target.OpenSegmentation(root, dirName, segmentation);

            CheckSegmentation(segmentation);
        }

        Segmentation CreateSegmentation()
        {
            Segmentation segmentation = new Segmentation();

            segmentation.Pan(2, 2);

            for (int i = 0; i < 3; i++)
            {
                var bmp = new Bitmap(200, 200, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                SegmentRoot root = new SegmentRoot(bmp, CreateSkeletonAnnotation());
                for (int j = 0; j < 2; j++)
                {
                    var sg = new PathSegment("" + j, root)
                    {
                        an = CreateSkeletonAnnotation(),
                        partingLine = new List<PointF>() { new PointF(50, 50), new PointF(100, 50), new PointF(100, 100), new PointF(50, 50) },
                        path = new List<PointF>() { new PointF(100, 100), new PointF(10, 10) },
                        section = new List<PointF>() { new PointF(100, 10), new PointF(100, 100), new PointF(10, 10) },
                        offset = new Point(10, 20)
                    };
                    sg._SetClosed(true);
                    root.segments.Add(sg);
                }

                segmentation.segmentRootDict["root" + i] = root;
            }

            CheckSegmentation(segmentation);

            return segmentation;
        }

        void CheckSegmentation(Segmentation segmentation)
        {
            Assert.AreEqual(segmentation.segmentRootDict.Count, 3);
            //   Assert.IsFalse(segmentation.transform.IsIdentity);
            for (int i = 0; i < 3; i++)
            {
                Assert.IsNotNull(segmentation.segmentRootDict.Values.ElementAt(i).an);
                Assert.IsNotNull(segmentation.segmentRootDict.Values.ElementAt(i).bmp);
                Assert.AreEqual(segmentation.segmentRootDict.Values.ElementAt(i).segments.Count, 3);
                for (int j = 0; j < 3; j++)
                {
                    var sg = segmentation.segmentRootDict.Values.ElementAt(i).segments[j];
                    if (sg.name == "Full")
                        continue;
                    Assert.AreEqual(sg.Closed, true);
                    Assert.AreEqual(sg.offset.X, 10, 1e-4);
                    Assert.AreEqual(sg.offset.Y, 20, 1e-4);
                    Assert.AreEqual(sg.partingLine.Count, 4);
                    Assert.AreEqual(sg.path.Count, 2);
                    Assert.AreEqual(sg.section.Count, 3);
                }
            }
        }


        /// <summary>
        ///SetReferenceImage のテスト
        ///</summary>
        [TestMethod()]
        public void SetReferenceImageTest()
        {
            Form1_Accessor target = new Form1_Accessor();
            Assert.IsNull(target.composition.referenceImage);

            Bitmap bmp1 = new Bitmap(10, 20, PixelFormat.Format32bppArgb);
            Bitmap bmp2 = new Bitmap(20, 30, PixelFormat.Format32bppArgb);

            target.AddOperation(target.composition.SetReferenceImage(bmp1));
            Assert.IsNotNull(target.composition.referenceImage);

            target.AddOperation(target.composition.SetReferenceImage(bmp2));
            Assert.IsNotNull(target.composition.referenceImage);
            Assert.AreEqual(target.composition.referenceImage.Width, 20);

            target.Undo();
            Assert.IsNotNull(target.composition.referenceImage);
            Assert.AreEqual(target.composition.referenceImage.Width, 10);

            target.Undo();
            Assert.IsNull(target.composition.referenceImage);

            target.Redo();
            Assert.IsNotNull(target.composition.referenceImage);
            Assert.AreEqual(target.composition.referenceImage.Width, 10);

            target.Undo();
            Assert.IsNull(target.composition.referenceImage);

            target.Undo();
            Assert.IsNull(target.composition.referenceImage);

            target.Redo();
            Assert.IsNotNull(target.composition.referenceImage);
            Assert.AreEqual(target.composition.referenceImage.Width, 10);

            target.Redo();
            Assert.IsNotNull(target.composition.referenceImage);
            Assert.AreEqual(target.composition.referenceImage.Width, 20);
            
            target.Redo();
            Assert.IsNotNull(target.composition.referenceImage);
            Assert.AreEqual(target.composition.referenceImage.Width, 20);

            target.Undo();
            Assert.IsNotNull(target.composition.referenceImage);
            Assert.AreEqual(target.composition.referenceImage.Width, 10);

            target.AddOperation(target.composition.SetReferenceImage(null));
            Assert.IsNull(target.composition.referenceImage);
        }

    }
}