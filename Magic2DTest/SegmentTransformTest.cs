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
    ///SegmentTransformTest のテスト クラスです。すべての
    ///SegmentTransformTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class SegmentTransformTest
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
        ///GetCrossingBoneWithPath のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void GetCrossingBoneWithPathTest()
        {
            SkeletonAnnotation an = new SkeletonAnnotation(null);
            an.joints.Add(new JointAnnotation("0", new PointF(0, 0)));
            an.joints.Add(new JointAnnotation("1", new PointF(100, 0)));
            an.joints.Add(new JointAnnotation("2", new PointF(0, 50)));
            an.joints.Add(new JointAnnotation("3", new PointF(0, 100)));
            an.joints.Add(new JointAnnotation("4", new PointF(50, 150)));

            an.bones.Add(new BoneAnnotation(an.joints[0], an.joints[1]));
            an.bones.Add(new BoneAnnotation(an.joints[0], an.joints[2]));
            an.bones.Add(new BoneAnnotation(an.joints[3], an.joints[4]));

            Assert.AreEqual(SegmentMeshInfo_Accessor.GetCrossingBoneWithPath(an, null), null);
            Assert.AreEqual(SegmentMeshInfo_Accessor.GetCrossingBoneWithPath(an, new List<PointF>()), null);

            List<PointF> path = new List<PointF>() { new PointF(0, -50), new PointF(50, -50) };
            Assert.AreEqual(SegmentMeshInfo_Accessor.GetCrossingBoneWithPath(an, path), null);

            path = new List<PointF>() { new PointF(50, -50), new PointF(50, 50) }; 
            Assert.AreEqual(SegmentMeshInfo_Accessor.GetCrossingBoneWithPath(an, path), an.bones[0]);

            // 複数ボーンと交差してたら最初に見つかったものを返す
            path = new List<PointF>() { new PointF(50, -50), new PointF(-50, 50) }; 
            Assert.AreEqual(SegmentMeshInfo_Accessor.GetCrossingBoneWithPath(an, path), an.bones[0]);

            path = new List<PointF>() { new PointF(50, 100), new PointF(50, 150),  new PointF(0, 150) };
            Assert.AreEqual(SegmentMeshInfo_Accessor.GetCrossingBoneWithPath(an, path), an.bones[2]);
        }

        /// <summary>
        ///InitializeControlPoints のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void InitializeControlPointsTest()
        {
            SegmentMeshInfo_Accessor.InitializeControlPoints(null, null, 0, null, null);

            ARAPDeformation arap = new ARAPDeformation(new List<PointF>() { 
                new PointF(-10, -10),
                new PointF(10, -10),
                new PointF(-10, 10),
                new PointF(10, 10),
            }, new List<PointF>());

            SkeletonAnnotation an = new SkeletonAnnotation(null);
            an.joints.Add(new JointAnnotation("0", new PointF(0, 0)));
            an.joints.Add(new JointAnnotation("1", new PointF(100, 0)));
            an.joints.Add(new JointAnnotation("2", new PointF(0, 50)));
            an.joints.Add(new JointAnnotation("3", new PointF(0, 100)));
            an.joints.Add(new JointAnnotation("4", new PointF(99, 100)));
            an.bones.Add(new BoneAnnotation(an.joints[0], an.joints[1]));
            an.bones.Add(new BoneAnnotation(an.joints[0], an.joints[2]));
            an.bones.Add(new BoneAnnotation(an.joints[3], an.joints[4]));

            Dictionary<BoneAnnotation, List<PointF>> boneToControl = new Dictionary<BoneAnnotation,List<PointF>>();

            SegmentMeshInfo_Accessor.InitializeControlPoints(arap, an, 50, null, boneToControl);
            Assert.AreEqual(arap.controlPoints.Count, 6);
            Assert.AreEqual(boneToControl.Sum(kv => kv.Value.Count), 7);

            Assert.AreEqual(arap.controlPoints[0].X, 0);
            Assert.AreEqual(arap.controlPoints[1].X, 50, 1e-4);
            Assert.AreEqual(arap.controlPoints[2].X, 100);

            Assert.AreEqual(arap.controlPoints[3].X, 0); Assert.AreEqual(arap.controlPoints[3].Y, 50);

            Assert.AreEqual(arap.controlPoints[4].X, 0);
            Assert.AreEqual(arap.controlPoints[5].X, 99);
        }
        
        /// <summary>
        ///GetSkeletalTransforms のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void GetSkeletalTransformsTest()
        {
            SkeletonAnnotation orgAn = new SkeletonAnnotation(null);
            orgAn.joints.Add(new JointAnnotation("0", new PointF(0, 0)));
            orgAn.joints.Add(new JointAnnotation("1", new PointF(100, 100)));
            orgAn.joints.Add(new JointAnnotation("2", new PointF(-500, -50)));
            orgAn.bones.Add(new BoneAnnotation(orgAn.joints[0], orgAn.joints[1]));
            orgAn.bones.Add(new BoneAnnotation(orgAn.joints[1], orgAn.joints[2]));
            
            SkeletonAnnotation an = new SkeletonAnnotation(null);
            an.joints.Add(new JointAnnotation("0", new PointF(0, 0)));
            an.joints.Add(new JointAnnotation("1", new PointF(100, 0)));
            an.joints.Add(new JointAnnotation("2", new PointF(0, 50)));
            an.bones.Add(new BoneAnnotation(an.joints[0], an.joints[1]));
            an.bones.Add(new BoneAnnotation(an.joints[1], an.joints[2]));

            var dict = SegmentMeshInfo_Accessor.GetSkeletalTransforms(an, orgAn);
            Assert.AreEqual(dict.Count, 2);
            Assert.IsTrue(dict.ContainsKey(an.bones[0]));
            Assert.IsTrue(dict.ContainsKey(an.bones[1]));

            var pts = new[] { orgAn.joints[0].position, orgAn.joints[1].position };
            dict[an.bones[0]].TransformPoints(pts);
            Assert.AreEqual(pts[0].X, 0, 1e-4);
            Assert.AreEqual(pts[0].Y, 0, 1e-4);
            Assert.AreEqual(pts[1].X, 100, 1e-4);
            Assert.AreEqual(pts[1].Y, 0, 1e-4);

            pts = new[] { orgAn.joints[1].position, orgAn.joints[2].position };
            dict[an.bones[1]].TransformPoints(pts);
            Assert.AreEqual(pts[0].X, 100, 1e-4);
            Assert.AreEqual(pts[0].Y, 0, 1e-4);
            Assert.AreEqual(pts[1].X, 0, 1e-4);
            Assert.AreEqual(pts[1].Y, 50, 1e-4);

        }

        /// <summary>
        ///UpdateSkeletalControlPoints のテスト
        ///</summary>
        [TestMethod()]
        public void UpdateSkeletalControlPointsTest()
        {
            Segment seg = new Segment("seg", null);
            SkeletonAnnotation segmentAnnotaiton = new SkeletonAnnotation(null);
            segmentAnnotaiton.joints.Add(new JointAnnotation("0", new PointF(0, 0)));
            segmentAnnotaiton.joints.Add(new JointAnnotation("1", new PointF(100, 100)));
            segmentAnnotaiton.joints.Add(new JointAnnotation("2", new PointF(-500, -50)));
            segmentAnnotaiton.bones.Add(new BoneAnnotation(segmentAnnotaiton.joints[0], segmentAnnotaiton.joints[1]));
            segmentAnnotaiton.bones.Add(new BoneAnnotation(segmentAnnotaiton.joints[1], segmentAnnotaiton.joints[2]));
            seg.an = segmentAnnotaiton;
            
            SegmentMeshInfo target = new SegmentMeshInfo(seg, true);

            SkeletonAnnotation refAnnotation = new SkeletonAnnotation(null);
            refAnnotation.joints.Add(new JointAnnotation("0", new PointF(0, 0)));
            refAnnotation.joints.Add(new JointAnnotation("1", new PointF(100, 0)));
            refAnnotation.joints.Add(new JointAnnotation("2", new PointF(0, 50)));
            refAnnotation.bones.Add(new BoneAnnotation(refAnnotation.joints[0], refAnnotation.joints[1]));
            refAnnotation.bones.Add(new BoneAnnotation(refAnnotation.joints[1], refAnnotation.joints[2]));
            refAnnotation.bones.Add(new BoneAnnotation(refAnnotation.joints[0], refAnnotation.joints[2]));

            target.UpdateSkeletalControlPoints(refAnnotation);


        }

        /// <summary>
        ///SplitPath のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Magic2D.exe")]
        public void SplitPathTest()
        {
            List<PointF> p = new List<PointF>();
            for (int i = 0; i < 10; i++)
                p.Add(new PointF(10 * i, 10 * i));

            SegmentMeshInfo_Accessor.SplitPath(null, null);
            SegmentMeshInfo_Accessor.SplitPath(new List<PointF>(), new List<PointF>());
            SegmentMeshInfo_Accessor.SplitPath(new List<PointF>(), new List<PointF>() { p[0] });

            var paths = SegmentMeshInfo_Accessor.SplitPath(new List<PointF>() { p[2], p[1], p[4], p[5] }, p);
            CheckPointList(paths[0], new List<PointF>() { p[1], p[2] });
            CheckPointList(paths[1], new List<PointF>() { p[4], p[5] });
        }

        void CheckPointList(List<PointF> ls0, List<PointF> ls1)
        {
            Assert.AreEqual(ls0.Count, ls1.Count);
            for (int i = 0; i < ls0.Count; i++)
                Assert.AreEqual(ls0[i], ls1[i]);
        }
    }
}
