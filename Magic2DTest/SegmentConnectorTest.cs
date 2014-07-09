using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Magic2D;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///SegmentConnectorTest のテスト クラスです。すべての
    ///SegmentConnectorTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class SegmentConnectorTest
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
        ///GetSectionCorrespondences のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TeleRegistration.exe")]
        public void GetSectionPairsTest()
        {
            var pairs = SegmentConnector_Accessor.GetSectionPairs(null, null);
            Assert.IsNull(pairs);
            pairs = SegmentConnector_Accessor.GetSectionPairs(null, new SkeletonAnnotation(null));
            Assert.IsNull(pairs);
            pairs = SegmentConnector_Accessor.GetSectionPairs(new List<SegmentMeshInfo>(), null);
            Assert.IsNull(pairs);

            SkeletonAnnotation an = new SkeletonAnnotation(null);
            an.joints.Add(new JointAnnotation("0", new PointF(100, 100)));
            an.joints.Add(new JointAnnotation("1", new PointF(200, 200)));
            an.joints.Add(new JointAnnotation("2", new PointF(300, 100)));
            an.bones.Add(new BoneAnnotation(an.joints[0], an.joints[1]));
            an.bones.Add(new BoneAnnotation(an.joints[1], an.joints[2]));

            var _seg1 = new Segment("seg1", null);
            _seg1.path = circlePoints(100, 100, 30, 20);
            _seg1.section = _seg1.path.Take(5).ToList();
            _seg1.an = new SkeletonAnnotation(null);
            _seg1.an.joints.Add(new JointAnnotation("0", new PointF(100, 100)));
            _seg1.an.joints.Add(new JointAnnotation("1", new PointF(200, 200)));
            _seg1.an.bones.Add(new BoneAnnotation(_seg1.an.joints[0], _seg1.an.joints[1]));

            var _seg2 = new Segment("seg2", null);
            _seg2.path = circlePoints(200, 200, 60, 20);
            _seg2.section = _seg2.path.Skip(10).Take(4).Concat(_seg2.path.Skip(15).Take(4)).ToList();
            _seg2.an = new SkeletonAnnotation(null);
            _seg2.an.joints.Add(new JointAnnotation("0", new PointF(100, 100)));
            _seg2.an.joints.Add(new JointAnnotation("1", new PointF(200, 200)));
            _seg2.an.joints.Add(new JointAnnotation("2", new PointF(300, 100)));
            _seg2.an.bones.Add(new BoneAnnotation(_seg2.an.joints[0], _seg2.an.joints[1]));
            _seg2.an.bones.Add(new BoneAnnotation(_seg2.an.joints[1], _seg2.an.joints[2]));

            var _seg3 = new Segment("seg3", null);
            _seg3.path = circlePoints(300, 150, 90, 20);
            _seg3.section = _seg3.path.Skip(5).Take(5).ToList();
            _seg3.an = new SkeletonAnnotation(null);
            _seg3.an.joints.Add(new JointAnnotation("1", new PointF(200, 200)));
            _seg3.an.joints.Add(new JointAnnotation("2", new PointF(300, 100)));
            _seg3.an.bones.Add(new BoneAnnotation(_seg3.an.joints[0], _seg3.an.joints[1]));

            var seg1 = new SegmentMeshInfo(_seg1, false);
            var seg2 = new SegmentMeshInfo(_seg2, false);
            var seg3 = new SegmentMeshInfo(_seg3, false);

            var _pairs = SegmentConnector_Accessor.GetSectionPairs(new List<SegmentMeshInfo>() { seg1, seg2, seg3 }, an);
            Assert.AreEqual(_pairs.Count, 2);

            Assert.AreEqual(_pairs[0].bone, an.bones[0]);
            Assert.AreEqual(_pairs[0].meshInfo1, seg1);
            Assert.AreEqual(_pairs[0].meshInfo2, seg2);
            Assert.AreEqual(_pairs[1].bone, an.bones[1]);
            Assert.AreEqual(_pairs[1].meshInfo1, seg2);
            Assert.AreEqual(_pairs[1].meshInfo2, seg3);
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
        
        /// <summary>
        ///SectionToCurves のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TeleRegistration.exe")]
        public void SectionToCurvesTest()
        {
            Bitmap bmp = new Bitmap(128, 128);
            for (int y = 0; y < 128; y++)
                for (int x = 0; x < 128; x++)
                {
                    float dx = (x - 64) / 60f;
                    float dy = (y - 64) / 60f;
                    float a = (float)Math.Sqrt(dx * dx + dy * dy);
                    int alpha = (int)FLib.FMath.Clamp((1 - a) * 255, 0, 255);
                    bmp.SetPixel(x, y, Color.FromArgb(alpha, 255, 255, 255));
                }
            bmp.Save("../../../particleTex.png");


            var path = circlePoints(100, 100, 50, 20);

            Assert.IsNull(SegmentConnector_Accessor.SectionToCurves(null, new CharacterRange(0, 6), 5, 60));
            Assert.IsNull(SegmentConnector_Accessor.SectionToCurves(new List<PointF>(), new CharacterRange(0, 6), 5, 60));
            Assert.IsNull(SegmentConnector_Accessor.SectionToCurves(path, new CharacterRange(0, 0), 5, 60));
            Assert.IsNull(SegmentConnector_Accessor.SectionToCurves(path, new CharacterRange(0, 0), 2, 60));
            
            var curves = SegmentConnector_Accessor.SectionToCurves(path, new CharacterRange(8, 4), 5, 60);
            Assert.AreEqual(curves.Item1.First, 4);
            Assert.AreEqual(curves.Item1.Length, 5);
            Assert.AreEqual(curves.Item2.First, 11);
            Assert.AreEqual(curves.Item2.Length, 5);

            curves = SegmentConnector_Accessor.SectionToCurves(path, new CharacterRange(-3, 5), 5, 60);
            Assert.AreEqual(curves.Item1.First, 13);
            Assert.AreEqual(curves.Item1.Length, 5);
            Assert.AreEqual(curves.Item2.First, 1);
            Assert.AreEqual(curves.Item2.Length, 5);

            curves = SegmentConnector_Accessor.SectionToCurves(path, new CharacterRange(-3, 2), 5, 60);
            Assert.AreEqual(curves.Item1.First, 13);
            Assert.AreEqual(curves.Item1.Length, 5);
            Assert.AreEqual(curves.Item2.First, 18);
            Assert.AreEqual(curves.Item2.Length, 5);

            curves = SegmentConnector_Accessor.SectionToCurves(path, new CharacterRange(1, 2), 5, 60);
            Assert.AreEqual(curves.Item1.First, 17);
            Assert.AreEqual(curves.Item1.Length, 5);
            Assert.AreEqual(curves.Item2.First, 2);
            Assert.AreEqual(curves.Item2.Length, 5);

            curves = SegmentConnector_Accessor.SectionToCurves(path, new CharacterRange(5, 2), 5, 0);
            Assert.AreEqual(curves.Item1.First, 3);
            Assert.AreEqual(curves.Item1.Length, 3);
            Assert.AreEqual(curves.Item2.First, 6);
            Assert.AreEqual(curves.Item2.Length, 3);

            path = circlePoints(50, 100, 40, 20);
            curves = SegmentConnector_Accessor.SectionToCurves(path, new CharacterRange(0, 5), 5, 30);
            Assert.AreEqual(curves.Item1.First, 16);
            Assert.AreEqual(curves.Item1.Length, 5);
            Assert.AreEqual(curves.Item2.First, 4);
            Assert.AreEqual(curves.Item2.Length, 5);
        }


        /*
        /// <summary>
        ///AdjustRotation のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TeleRegistration.exe")]
        public void AdjustRotationTest()
        {
            List<SegmentMeshInfo> ms = new List<SegmentMeshInfo>();
            var path1 = circlePoints(100, 100, 40, 20);
            var path2 = circlePoints(200, 200, 30, 20);
            var r1 =  new CharacterRange(0, 5);
            var r2 =  new CharacterRange(10, 5);
            var section1 = path1.Skip(r1.First).Take(r1.Length).ToList();
            var section2 = path2.Skip(r2.First).Take(r2.Length).ToList();
            var an = new SkeletonAnnotation(null);
            an.joints.Add(new JointAnnotation("0", new PointF(100, 100)));
            an.joints.Add(new JointAnnotation("1", new PointF(200, 200)));
            an.bones.Add(new BoneAnnotation(an.joints[0], an.joints[1]));

            ms.Add(new SegmentMeshInfo(path1, null, section1, an, false));
            ms.Add(new SegmentMeshInfo(path2, null, section2, an, false));

            var pairs = new List<SegmentConnector.ConnectPair>();
            pairs.Add(new SegmentConnector.ConnectPair(an.bones[0], ms[0], r1, ms[1], r2));

//            Assert.AreNotEqual(GetSectionWidth(ms[0], r1, an.bones[0]), GetSectionWidth(ms[1], r2, an.bones[0]), 1e-4f);

            SegmentConnector_Accessor.AdjustRotation(ms, an, pairs);

            //            Assert.AreEqual(GetSectionWidth(ms[0], r1, an.bones[0]), GetSectionWidth(ms[1], r2, an.bones[0]), 1e-4f);
            Assert.Inconclusive("値を返さないメソッドは確認できません。");
        }
        */

        /// <summary>
        ///AdjustScale のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TeleRegistration.exe")]
        public void AdjustScaleTest()
        {
            List<SegmentMeshInfo> ms = new List<SegmentMeshInfo>();
            var path1 = circlePoints(100, 100, 40, 20);
            var path2 = circlePoints(200, 200, 30, 20);
            var r1 = new CharacterRange(0, 5);
            var r2 = new CharacterRange(10, 5);
            var section1 = path1.Skip(r1.First).Take(r1.Length).ToList();
            var section2 = path2.Skip(r2.First).Take(r2.Length).ToList();
            var curves1 = SegmentConnector_Accessor.SectionToCurves(path1, r1,  5, 30);
            var curves2 = SegmentConnector_Accessor.SectionToCurves(path2, r2,  5, 30);
            var an = new SkeletonAnnotation(null);
            an.joints.Add(new JointAnnotation("0", new PointF(100, 100)));
            an.joints.Add(new JointAnnotation("1", new PointF(200, 200)));
            an.bones.Add(new BoneAnnotation(an.joints[0], an.joints[1]));

            ms.Add(new SegmentMeshInfo(path1, null, section1, an, false));
            ms.Add(new SegmentMeshInfo(path2, null, section2, an, false));

            var pairs = new List<SegmentConnector.ConnectPair>();
            pairs.Add(new SegmentConnector.ConnectPair(an.bones[0], ms[0], r1, ms[1], r2));

            var w1 = SegmentConnector_Accessor.GetSectionWidth(ms[0].GetPath(), curves1, an.bones[0]);
            var w2 = SegmentConnector_Accessor.GetSectionWidth(ms[1].GetPath(), curves2, an.bones[0]);

            Assert.AreNotEqual(w1, w2, 1e-4);

            SegmentConnector_Accessor.AdjustScale(ms, an, pairs);

            w1 = SegmentConnector_Accessor.GetSectionWidth(ms[0].GetPath(), curves1, an.bones[0]);
            w2 = SegmentConnector_Accessor.GetSectionWidth(ms[1].GetPath(), curves2, an.bones[0]);
            Assert.AreEqual(w1, w2, 1e-4);
        }

        /// <summary>
        ///AdjustPosition のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TeleRegistration.exe")]
        public void AdjustPositionTest()
        {
            var pts00 = circlePoints(100, 100, 50, 20);
            var pts11 = circlePoints(200, 200, 50, 20);

            SkeletonAnnotation an = new SkeletonAnnotation(null);
            an.joints.Add(new JointAnnotation("00", new PointF(100, 100)));
            an.joints.Add(new JointAnnotation("10", new PointF(200, 100)));

            an.bones.Clear();
            an.bones.Add(new BoneAnnotation(an.joints[0], an.joints[1]));

            var ms = new List<SegmentMeshInfo>();
            ms.Add(new SegmentMeshInfo(pts00, null, pts00.Skip(-2).Take(5).ToList(), an, false));
            ms.Add(new SegmentMeshInfo(pts11, null, pts11.Skip(8).Take(5).ToList(), an, false));

            var pairs = new List<SegmentConnector.ConnectPair>();
            pairs.Add(new SegmentConnector.ConnectPair(an.bones[0], ms[0], new CharacterRange(-2, 5), ms[1], new CharacterRange(8, 5)));

            SegmentConnector_Accessor.AdjustPosition(new List<SegmentMeshInfo>(), an, pairs);

            var path = ms[1].GetPath();
            Assert.AreEqual(path.Average(pt => pt.X), 200, 5);
            Assert.AreEqual(path.Average(pt => pt.Y), 100, 5);
        }

        /// <summary>
        ///ExpandSegments のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TeleRegistration.exe")]
        public void ExpandSegmentsTest()
        {
            List<SegmentMeshInfo> meshes = new List<SegmentMeshInfo>();

            var p1 = circlePoints(50, 100, 40, 20);
            var r1 = new CharacterRange(0, 6);
            var _s = p1.Skip(r1.First).Take(r1.Length).ToList();
            SkeletonAnnotation an = new SkeletonAnnotation(null)
            {
                bones = new List<BoneAnnotation>() { GetBone(new PointF(50, 100), new PointF(150, 200)) }
            };
            meshes.Add(new SegmentMeshInfo(p1, null, _s, an, false));

            var p2 = circlePoints(150, 200, 40, 20);
            var r2 = new CharacterRange(10, 6);
            var _s2 = p2.Skip(r2.First).Take(r2.Length).ToList();
            meshes.Add(new SegmentMeshInfo(p2, null, _s2, an, false));

            List<SegmentConnector.ConnectPair> pairs = new List<SegmentConnector.ConnectPair>()
            {
                new SegmentConnector.ConnectPair(an.bones.First(), meshes[0], r1, meshes[1], r2)
            };

            int cnt = 0;
            foreach (var m in meshes)
            {
                var path = m.GetPath();
                foreach (var p in path)
                    m.arap.AddControlPoint(p, p);
                m.arap.BeginDeformation();
                m.arap.ToBitmap().Save("../../../Test2/mesh" + cnt + "_before.png");
                cnt++;
            }

            // ちゃんと領域がかぶっているか
            SegmentConnector_Accessor.ExpandSegments(meshes, an, pairs);

            var path1 = meshes[0].GetPath();
            var path2 = meshes[1].GetPath();

            Assert.AreEqual(path1[0].X, path2[16].X, 0.1);
            Assert.AreEqual(path1[19].X, path2[15].X, 0.1);
            Assert.AreEqual(path1[0].Y, path2[16].Y, 0.1);
            Assert.AreEqual(path1[19].Y, path2[15].Y, 0.1);

            Assert.AreEqual(path1[5].X, path2[9].X, 0.1);
            Assert.AreEqual(path1[6].X, path2[10].X, 0.1);
            Assert.AreEqual(path1[5].Y, path2[9].Y, 0.1);
            Assert.AreEqual(path1[6].Y, path2[10].Y, 0.1);

            cnt = 0;
            foreach (var m in meshes)
            {
                m.arap.EndDeformation();
                m.arap.ToBitmap().Save("../../../Test2/mesh" + cnt + "_after.png");
                cnt++;
            }
            
            // 接合面もきちんと動いているか
            PointF x, y;
            SegmentConnector_Accessor.GetCoordinateFromBone(an.bones[0], out x, out y);
            foreach (var m in meshes)
            {
                var path = m.GetPath();
                foreach (var r in m.sections)
                {
                    float _x0 = path[r.First].X - an.bones[0].src.position.X;
                    float _y0 = path[r.First].Y - an.bones[0].src.position.Y;
                    float xx0 = _x0 * x.X + _y0 * x.Y;
                    float yy0 = _x0 * y.X + _y0 * y.Y;
                    float _x1 = path[r.First + r.Length - 1].X - an.bones[0].src.position.X;
                    float _y1 = path[r.First + r.Length - 1].Y - an.bones[0].src.position.Y;
                    float xx1 = _x1 * x.X + _y1 * x.Y;
                    float yy1 = _x1 * y.X + _y1 * y.Y;

                    if (yy0 > yy1)
                        FLib.FMath.Swap(ref yy0, ref yy1);

                    var s = path.Skip(r.First  +1).Take(r.Length - 2).ToList();
                    foreach (var p in s)
                    {
                        float _x = p.X - an.bones[0].src.position.X;
                        float _y = p.Y - an.bones[0].src.position.Y;
                        float xx = _x * x.X + _y * x.Y;
                        float yy = _x * y.X + _y * y.Y;
                        if (m == meshes[0])
                        {
                            Assert.IsTrue(xx > xx0);
                            Assert.IsTrue(xx > xx1);
                        }
                        else
                        {
                            Assert.IsTrue(xx < xx0);
                            Assert.IsTrue(xx < xx1);
                        }
                        Assert.IsTrue(yy0 < yy);
                        Assert.IsTrue(yy < yy1);
                    }
                }
            }

        }

        /// <summary>
        ///GetSectionWidth のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TeleRegistration.exe")]
        public void GetSectionWidthTest()
        {
            List<PointF> path = new List<PointF>();
            for (int i = 0; i < 10; i++)
                path.Add(new PointF(10 * i, -10 * i));
            for (int i = 0; i < 10; i++)
                path.Add(new PointF(10 * (9 - i), 10 * (9 - i)));

            var curves = new Tuple<CharacterRange, CharacterRange>(new CharacterRange(1, 2), new CharacterRange(-3, 2));
            BoneAnnotation b = new BoneAnnotation(new JointAnnotation("", new PointF(0, 0)), new JointAnnotation("", new PointF(100, 0)));

            Assert.AreEqual(SegmentConnector_Accessor.GetSectionWidth(path, curves, b), 30, 1e-4f);
        }

        /// <summary>
        ///GetSectionHeight のテスト
        ///</summary>
        [TestMethod()]
        [DeploymentItem("TeleRegistration.exe")]
        public void GetSectionHeightTest()
        {
            SegmentConnector_Accessor.GetSectionHeight(null, null, null);

            List<PointF> path = circlePoints(100, 100, 50, 20);
            var curves = SegmentConnector_Accessor.SectionToCurves(path, new CharacterRange(-2, 5), 3, 30);
           
            float height = SegmentConnector_Accessor.GetSectionHeight(path, curves, GetBone(new PointF(100, 100), new PointF(200, 100)));

            Assert.AreEqual(height, 0, 1e-4);
        }

        BoneAnnotation GetBone(PointF p1, PointF p2)
        {
            return new BoneAnnotation(new JointAnnotation("0", p1), new JointAnnotation("1", p2));
        }
    }
}
