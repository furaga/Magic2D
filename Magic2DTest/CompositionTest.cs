using Magic2D;
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
using System.Drawing.Imaging;
using FLib;

namespace Magic2DTest
{
    
    
    /// <summary>
    ///CompositionTest のテスト クラスです。すべての
    ///CompositionTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class CompositionTest
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
        ///CreateEditingUnit のテスト
        ///</summary>
        [TestMethod()]
        public void CreateEditingUnitTest()
        {
            Composition_Accessor target = new Composition_Accessor(); // TODO: 適切な値に初期化してください
            Assert.IsNotNull(target.editingUnit);

            var tmp = target.editingUnit;

            target.SetEditingUnit(null);
            Assert.IsNull(target.editingUnit);
            target.CreateEditingUnit();
            Assert.IsNotNull(target.editingUnit);
            Assert.AreNotEqual(target.editingUnit, tmp);

            target.SetEditingUnit(tmp);
            Assert.IsNotNull(target.editingUnit);
            Assert.AreEqual(target.editingUnit, tmp);
        }

        /// <summary>
        ///OnMouseDown のテスト
        ///</summary>
        [TestMethod()]
        public void OnMouseDownTest()
        {
            Composition target = new Composition(); // TODO: 適切な値に初期化してください
 //           target.OnMouseDown(MouseButtons.Left, new PointF(50, 50));
            // todo
        }

        /// <summary>
        ///OnMouseMove のテスト
        ///</summary>
        [TestMethod()]
        public void OnMouseMoveTest()
        {
            Composition target = new Composition(); // TODO: 適切な値に初期化してください
   //         target.OnMouseMove(MouseButtons.Left, new PointF(50, 50));
            // todo
        }

        /// <summary>
        ///OnMouseUp のテスト
        ///</summary>
        [TestMethod()]
        public void OnMouseUpTest()
        {
            Composition target = new Composition(); // TODO: 適切な値に初期化してください
     //       target.OnMouseUp(MouseButtons.Left, new PointF(50, 50));
            // todo
        }

        /// <summary>
        ///Pull のテスト
        ///</summary>
        [TestMethod()]
        public void PushPullTest()
        {
            Composition_Accessor target = new Composition_Accessor(); // TODO: 適切な値に初期化してください

            Segment seg0 = new Segment("0", null);
            Segment seg1 = new Segment("1", null);
            Segment seg2 = new Segment("2", null);
            target.editingUnit.AssignSegment(seg0.name, seg0);
            target.editingUnit.AssignSegment(seg1.name, seg1);
            target.editingUnit.AssignSegment(seg2.name, seg2);

            Assert.AreEqual(string.Join("", target.editingUnit.segments.Select(seg => seg.name)), "012");

            target.Backward(null);
            Assert.AreEqual(string.Join("", target.editingUnit.segments.Select(seg => seg.name)), "012");

            target.Backward(seg2);            
            Assert.AreEqual(string.Join("", target.editingUnit.segments.Select(seg => seg.name)), "021");
            target.Backward(seg2);
            Assert.AreEqual(string.Join("", target.editingUnit.segments.Select(seg => seg.name)), "201");
            target.Backward(seg2);
            Assert.AreEqual(string.Join("", target.editingUnit.segments.Select(seg => seg.name)), "201");

            target.Forward(seg0);
            Assert.AreEqual(string.Join("", target.editingUnit.segments.Select(seg => seg.name)), "210");
            target.Forward(seg0);
            Assert.AreEqual(string.Join("", target.editingUnit.segments.Select(seg => seg.name)), "210");
            target.Backward(seg1);
            Assert.AreEqual(string.Join("", target.editingUnit.segments.Select(seg => seg.name)), "120");

            target.Back(seg0);
            Assert.AreEqual(string.Join("", target.editingUnit.segments.Select(seg => seg.name)), "012");
            target.Front(seg0);
            Assert.AreEqual(string.Join("", target.editingUnit.segments.Select(seg => seg.name)), "120");
        }

        /// <summary>
        ///SetEditingUnit のテスト
        ///</summary>
        [TestMethod()]
        public void SetEditingUnitTest()
        {
            Composition_Accessor target = new Composition_Accessor(); // TODO: 適切な値に初期化してください
            ComposedUnit unit = new ComposedUnit();
            target.SetEditingUnit(null);
            Assert.AreEqual(target.editingUnit, null);
            target.SetEditingUnit(unit);
            Assert.AreEqual(target.editingUnit, unit);
        }

        /// <summary>
        ///AssignComposedUnit のテスト
        ///</summary>
        [TestMethod()]
        public void AssignComposedUnitTest()
        {
            Composition_Accessor target = new Composition_Accessor(); // TODO: 適切な値に初期化してください
            target.AssignComposedUnit("0");
            Assert.AreEqual(target.unitDict.Count, 1);
            Assert.AreEqual(target.unitDict["0"], target.editingUnit);

            target.AssignComposedUnit("1", null);
            Assert.AreEqual(target.unitDict.Count, 1);
            Assert.AreEqual(target.unitDict["0"], target.editingUnit);

            target.RemoveComposedUnit("1");
            Assert.AreEqual(target.unitDict.Count, 1);
            Assert.AreEqual(target.unitDict["0"], target.editingUnit);

            target.RemoveComposedUnit("0");
            Assert.AreEqual(target.unitDict.Count, 0);
        }
    }
}
