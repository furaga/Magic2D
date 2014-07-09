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
using System.Diagnostics;
using FLib;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNAColor = Microsoft.Xna.Framework.Color;
using XNAMatrix = Microsoft.Xna.Framework.Matrix;
using XNAVector3 = Microsoft.Xna.Framework.Vector3;
using XNAVector2 = Microsoft.Xna.Framework.Vector2;
using XNAMathHelper = Microsoft.Xna.Framework.MathHelper;

namespace Magic2D
{
    public class CompositionCanvasControl : XNAControl
    {
        BasicEffect basicEffect;
        XNAVector3 orgCameraPosition = new XNAVector3(0, 0, 500);
        XNAVector3 cameraPosition = new XNAVector3(0, 0, 500);

        List<VertexPositionColorTexture> vertexList = new List<VertexPositionColorTexture>();
        DynamicVertexBuffer vertexBuffer;
        Texture2D dummyTexture;

        public Composition composition { get; set; }

        bool saveRenderedSegment = false;

        protected override void Initialize()
        {
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.TextureEnabled = true;
            basicEffect.VertexColorEnabled = true;
            basicEffect.View = XNAMatrix.CreateLookAt(
                    orgCameraPosition,
                    XNAVector3.Zero,
                    XNAVector3.Up
                );
            basicEffect.Projection = XNAMatrix.CreatePerspectiveFieldOfView(
                    XNAMathHelper.ToRadians(45.0f),
                    (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height,
                    1.0f,
                    10000.0f
                );
            basicEffect.Texture = null;

            dummyTexture =  XNATexture.Load(GraphicsDevice, "./dummyTexture.png");
        }

        //-------------------------------------------------------------

        /// <summary>
        /// 世界座標をクライアント座標に変換
        /// WorldToClient
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public PointF PointToClient(PointF pt)
        {
            if (GraphicsDevice == null || basicEffect == null)
                return pt;
            var v = GraphicsDevice.Viewport.Project(PointFToXNAVector3(pt), basicEffect.Projection, basicEffect.View, XNAMatrix.Identity);

            return new PointF(v.X, v.Y);
        }

        /// <summary>
        /// クライアント座標を世界座標に変換
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public PointF PointToWorld(PointF pt)
        {
            if (GraphicsDevice == null || basicEffect == null)
                return pt;
            if (composition == null || composition.transform == null)
                return pt;
            var p = Unproject(new XNAVector3(pt.X, pt.Y, 0), basicEffect.Projection, basicEffect.View, XNAMatrix.Identity, cameraPosition);
            return new PointF(p.X, -p.Y);
        }
        
        //-------------------------------------------------------------

        protected override void Draw()
        {
            GraphicsDevice.Clear(XNAColor.White);

            if (composition != null)
                DrawComposition(composition);
        }


        private void DrawComposition(Composition composition)
        {
            Matrix transform = composition.transform;
            if (transform == null)
                return;

            UpdateCamera(transform);

            Texture2D refTex = ToTexture2D(composition.referenceImage);
            if (refTex != null)
                DrawTexture(refTex, 0, 0, refTex.Width, refTex.Height);

            DrawPoint(0, 0, XNAColor.Yellow);
            DrawPoint(20, 20, XNAColor.Blue, 8);


            // TODO

            Segment highlightSegment = null;
            if (composition.editingSegment != null)
                highlightSegment = composition.editingSegment;

            if (composition.editingUnit != null)
                DrawCompositedUnit(composition.editingUnit, composition);

            if (composition.an != null)
            {
                var highlight = new Dictionary<JointAnnotation, XNAColor>();
                if (composition.editingJoint != null)
                    highlight[composition.editingJoint] = XNAColor.Red;
                if (composition.nearestJoint != null)
                    highlight[composition.nearestJoint] = XNAColor.Yellow;
                DrawSkeleton(composition.an, highlight, XNAColor.DarkOrange);
            }
        }

        void UpdateCamera(Matrix transform)
        {
            float ox = transform.OffsetX;
            float oy = transform.OffsetY;
            float scale = transform.Elements.ElementAt(0);

            Console.WriteLine(string.Format("({0}, {1}), {2}", ox, oy, scale));

            if (scale <= 0)
                return;

            XNAVector3 zoomCamPos = orgCameraPosition;
            zoomCamPos.Z /= scale;

            var zoomView = XNAMatrix.CreateLookAt(
                    zoomCamPos,
                    XNAVector3.Zero,
                    XNAVector3.Up
                );

            XNAVector3 offset = new XNAVector3(ox, oy, 0);

            XNAVector3 p0 = Unproject(XNAVector3.Zero, basicEffect.Projection, zoomView, XNAMatrix.Identity, zoomCamPos);
            XNAVector3 p1 = Unproject(offset, basicEffect.Projection, zoomView, XNAMatrix.Identity, zoomCamPos);

            cameraPosition = p0 - p1;
            cameraPosition.Z = zoomCamPos.Z;
            
            XNAVector3 targetPos = cameraPosition;
            targetPos.Z = 0;

            basicEffect.View = XNAMatrix.CreateLookAt(
                    cameraPosition,
                    targetPos,
                    XNAVector3.Up
                );
        }

        XNAVector3 Unproject(XNAVector3 v, XNAMatrix projection, XNAMatrix view, XNAMatrix world, XNAVector3 camPos)
        {
            XNAVector3 p0 = GraphicsDevice.Viewport.Unproject(new XNAVector3(v.X, v.Y, 0), projection, view, world);
            XNAVector3 p1 = GraphicsDevice.Viewport.Unproject(new XNAVector3(v.X, v.Y, 1), projection, view, world);
            if (Math.Abs(p1.Z - p0.Z) <= 1e-4)
                return v;
            XNAVector3 dir = XNAVector3.Normalize(p1 - p0);
            XNAVector3 p = camPos + dir * (v.Z - camPos.Z) / dir.Z;
            return p;
        }

        private void DrawCompositedUnit(ComposedUnit composedUnit, Composition composition)
        {
            if (composedUnit == null)
                return;
            if (composition == null)
                return;
            if (composedUnit.segments == null)
                return;
            for (int i = 0; i < composedUnit.segments.Count; i++)
            {
                Segment seg = composedUnit.segments[i];
                if (seg == null)
                    continue;
                if (!composedUnit.transformDict.ContainsKey(seg.name))
                    continue;
                SegmentMeshInfo transform = composedUnit.transformDict[seg.name];
                DrawSegmentMeshes(seg, transform, XNAColor.White, true);
                if (seg == composition.editingSegment)
                {
                    DrawSegmentMeshes(seg, transform, new XNAColor(0.2f, 0.2f, 0.2f, 0.1f), false);
                    DrawSegmentMeshLines(transform, XNAColor.Blue);
                    DrawSkeleton(transform.an, new Dictionary<JointAnnotation, XNAColor>(), XNAColor.Gray);

                    Dictionary<PointF , XNAColor> ptColorDict = new Dictionary<PointF,XNAColor>();
                    if (composition.editingControlPoint != null)
                        ptColorDict[composition.editingControlPoint.Value] = XNAColor.Red;
                    if (composition.nearestControlPoint != null)
                        ptColorDict[composition.nearestControlPoint.Value] = XNAColor.Yellow;
                    DrawSegmentControlPoints(transform, ptColorDict, XNAColor.Black);
                }
            }
        }

        private void DrawSegmentControlPoints(SegmentMeshInfo transform, Dictionary<PointF, XNAColor> ptColorDict, XNAColor defaultColor)
        {
            if (transform == null || transform.arap == null)
                return;
            foreach (var pt in transform.arap.controlPoints)
            {
                if (ptColorDict.ContainsKey(pt))
                    DrawPoint((int)pt.X, (int)pt.Y, ptColorDict[pt], 10);
                else
                    DrawPoint((int)pt.X, (int)pt.Y, defaultColor, 10);
            }
        }

        private void DrawSegmentMeshes(Segment seg, SegmentMeshInfo transform, XNAColor color, bool textureEnabled)
        {
            if (seg == null || seg.bmp == null || transform == null || transform.arap == null)
                return;

            Texture2D texture = null;

            if (textureEnabled)
            {
                texture = ToTexture2D(seg.bmp);
                if (texture == null || texture.Width <= 0 || texture.Height <= 0)
                    return;
            }

            if (saveRenderedSegment)
            {
                var bmp = transform.arap.ToBitmap();
                if (bmp != null)
                    bmp.Save("./" + seg.name + ".bmp");
            }

            List<PointF> vts = transform.arap.GetMeshVertexList();
            List<PointF> cds = !textureEnabled ? null : transform.arap.GetMeshCoordList(texture.Width, texture.Height);

            if (textureEnabled && (cds == null || cds.Count != vts.Count))
                return;

            vertexList.Clear();

            for (int i = 0; i < vts.Count; i += 3)
            {
                XNAVector3 pos1 = new XNAVector3(vts[i].X, -vts[i].Y, 0);
                XNAVector3 pos2 = new XNAVector3(vts[i + 1].X, -vts[i + 1].Y, 0);
                XNAVector3 pos3 = new XNAVector3(vts[i + 2].X, -vts[i + 2].Y, 0);
                XNAVector2 coord1 = !textureEnabled ? XNAVector2.Zero : new XNAVector2(cds[i].X, cds[i].Y);
                XNAVector2 coord2 = !textureEnabled ? XNAVector2.Zero : new XNAVector2(cds[i + 1].X, cds[i + 1].Y);
                XNAVector2 coord3 = !textureEnabled ? XNAVector2.Zero : new XNAVector2(cds[i + 2].X, cds[i + 2].Y);
                vertexList.Add(new VertexPositionColorTexture(pos1, color, coord1));
                vertexList.Add(new VertexPositionColorTexture(pos2, color, coord2));
                vertexList.Add(new VertexPositionColorTexture(pos3, color, coord3));
            }

            bool succeed = FlushVertexList(vertexList, true, textureEnabled, texture);
            if (!succeed)
                return;

            DrawMeshes(vts.Count / 3);
        }
            
        private void DrawSegmentMeshLines(SegmentMeshInfo transform, XNAColor color)
        {
            if (transform == null || transform.arap == null)
                return;

            List<PointF> vts = transform.arap.GetMeshVertexList();
            vertexList.Clear();

            for (int i = 0; i < vts.Count; i += 3)
            {
                XNAVector3 pos1 = new XNAVector3(vts[i].X, -vts[i].Y, 0);
                XNAVector3 pos2 = new XNAVector3(vts[i + 1].X, -vts[i + 1].Y, 0);
                XNAVector3 pos3 = new XNAVector3(vts[i + 2].X, -vts[i + 2].Y, 0);
                vertexList.Add(new VertexPositionColorTexture(pos1, color, XNAVector2.Zero));
                vertexList.Add(new VertexPositionColorTexture(pos2, color, XNAVector2.Zero));
                vertexList.Add(new VertexPositionColorTexture(pos2, color, XNAVector2.Zero));
                vertexList.Add(new VertexPositionColorTexture(pos3, color, XNAVector2.Zero));
                vertexList.Add(new VertexPositionColorTexture(pos3, color, XNAVector2.Zero));
                vertexList.Add(new VertexPositionColorTexture(pos1, color, XNAVector2.Zero));
            }

            bool succeed = FlushVertexList(vertexList, true, false, null);
            if (!succeed)
                return;

            DrawLineList(vts.Count / 2);
        }

        //-------------------------------------------

        // プリミティブ

        Dictionary<Bitmap, Texture2D> bmpToTex = new Dictionary<Bitmap, Texture2D>();

        Texture2D ToTexture2D(Bitmap bmp, bool reflesh = false)
        {
            if (bmp == null)
                return null;

            if (reflesh)
            {
                if (bmpToTex.ContainsKey(bmp))
                    bmpToTex[bmp].Dispose();
                var tex = XNATexture.Load(GraphicsDevice, bmp);
                bmpToTex[bmp] = tex;
                return tex;
            }
            else
            {
                if (bmpToTex.ContainsKey(bmp))
                    return bmpToTex[bmp];
                var tex = XNATexture.Load(GraphicsDevice, bmp);
                bmpToTex[bmp] = tex;
                return tex;
            }
        }

        XNAVector3 PointFToXNAVector3(PointF point)
        {
            return new XNAVector3(point.X, -point.Y, 0);
        }

        int SkeletonToLineList(SkeletonAnnotation skeleton, XNAColor color, List<VertexPositionColorTexture> vertexList)
        {
            if (vertexList == null)
                return 0;

            vertexList.Clear();

            if (skeleton == null || skeleton.bones == null || skeleton.bones.Count <= 0)
                return 0;

            float minx = skeleton.joints.Select(j => j.position.X).Min();
            float miny = skeleton.joints.Select(j => j.position.Y).Min();
            float maxx = skeleton.joints.Select(j => j.position.X).Max();
            float maxy = skeleton.joints.Select(j => j.position.Y).Max();

            float ox = (maxx - minx) / 2;
            float oy = (maxy - miny) / 2;

            for (int i = 0; i < skeleton.bones.Count; i++)
            {
                if (skeleton.bones[i].src == null || skeleton.bones[i].dst == null)
                    continue;
                XNAVector3 srcPos = PointFToXNAVector3(skeleton.bones[i].src.position);
                XNAVector3 dstPos = PointFToXNAVector3(skeleton.bones[i].dst.position);

                vertexList.Add(new VertexPositionColorTexture(srcPos, color, XNAVector2.Zero));
                vertexList.Add(new VertexPositionColorTexture(dstPos, color, XNAVector2.Zero));
            }
            
            return vertexList.Count / 2;
        }

        // 頂点をグラボに送信
        bool FlushVertexList(List<VertexPositionColorTexture> vertexList, bool vertexColorEnabled, bool textureEnabled, Texture2D texture)
        {
            if (vertexList == null || vertexList.Count <= 0)
                return false;

            if (textureEnabled && texture == null)
                return false;

            if (vertexBuffer == null || vertexBuffer.VertexCount != vertexList.Count)
                vertexBuffer = new DynamicVertexBuffer(GraphicsDevice, VertexPositionColorTexture.VertexDeclaration, vertexList.Count, BufferUsage.None);

            vertexBuffer.SetData(vertexList.ToArray(), 0, vertexList.Count, SetDataOptions.Discard);

            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.DepthStencilState = DepthStencilState.None;
            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            basicEffect.VertexColorEnabled = vertexColorEnabled;
            basicEffect.TextureEnabled = textureEnabled;
            if (textureEnabled)
                basicEffect.Texture = texture;

            return true;
        }

        void DrawLineList(int lineNum)
        {
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.LineList, 0, lineNum);
            }
        }

        void DrawLineStrip(int lineNum)
        {
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.LineStrip, 0, lineNum);
            }
        }

        void DrawMeshes(int triMeshNum)
        {
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, triMeshNum);
            }
        }

        void DrawSkeleton(SkeletonAnnotation skeleton, Dictionary<JointAnnotation, XNAColor> highlight, XNAColor defaultColor)
        {
            int lineNum = SkeletonToLineList(skeleton, XNAColor.Red, vertexList);
            if (lineNum <= 0)
                return;
            
            bool succeed = FlushVertexList(vertexList, true, false, null);
            if (!succeed)
                return;

            DrawLineList(lineNum);

            for (int i = 0; i < skeleton.joints.Count; i++)
            {
                if (highlight.ContainsKey(skeleton.joints[i]))
                    DrawPoint((int)skeleton.joints[i].position.X, (int)skeleton.joints[i].position.Y, highlight[skeleton.joints[i]], 10);
                else
                    DrawPoint((int)skeleton.joints[i].position.X, (int)skeleton.joints[i].position.Y, defaultColor, 10);
            }
        }

        void DrawTexture(Texture2D texture, int x, int y, int w, int h)
        {
            Stopwatch sw = Stopwatch.StartNew();

            int x0 = x, x1 = x + w;
            int y0 = y, y1 = y + h;

            vertexList.Clear();

            vertexList.Add(new VertexPositionColorTexture(new XNAVector3(x0, -y0, 0), XNAColor.White, new XNAVector2(0, 0)));
            vertexList.Add(new VertexPositionColorTexture(new XNAVector3(x1, -y1, 0), XNAColor.White, new XNAVector2(1, 1)));
            vertexList.Add(new VertexPositionColorTexture(new XNAVector3(x1, -y0, 0), XNAColor.White, new XNAVector2(1, 0)));
            
            vertexList.Add(new VertexPositionColorTexture(new XNAVector3(x0, -y0, 0), XNAColor.White, new XNAVector2(0, 0)));
            vertexList.Add(new VertexPositionColorTexture(new XNAVector3(x0, -y1, 0), XNAColor.White, new XNAVector2(0, 1)));
            vertexList.Add(new VertexPositionColorTexture(new XNAVector3(x1, -y1, 0), XNAColor.White, new XNAVector2(1, 1)));

            bool succeed = FlushVertexList(vertexList, true, true, texture);
            if (!succeed)
                return;

            DrawMeshes(2);

            Console.WriteLine("Draw: " + sw.ElapsedMilliseconds + " ms");
        }

        void DrawPoint(int x, int y, XNAColor color, int size = 2)
        {
            int x0 = x - size / 2, x1 = x + size - size / 2;
            int y0 = y - size / 2, y1 = y + size - size / 2;

            vertexList.Clear();

            vertexList.Add(new VertexPositionColorTexture(new XNAVector3(x0, -y0, 0), color, XNAVector2.Zero));
            vertexList.Add(new VertexPositionColorTexture(new XNAVector3(x1, -y1, 0), color, XNAVector2.Zero));
            vertexList.Add(new VertexPositionColorTexture(new XNAVector3(x1, -y0, 0), color, XNAVector2.Zero));

            vertexList.Add(new VertexPositionColorTexture(new XNAVector3(x0, -y0, 0), color, XNAVector2.Zero));
            vertexList.Add(new VertexPositionColorTexture(new XNAVector3(x0, -y1, 0), color, XNAVector2.Zero));
            vertexList.Add(new VertexPositionColorTexture(new XNAVector3(x1, -y1, 0), color, XNAVector2.Zero));

            bool succeed = FlushVertexList(vertexList, true, false, null);
            if (!succeed)
                return;

            DrawMeshes(2);
        }

        void DrawPath(List<PointF> path, XNAColor color, bool closed)
        {
            if (path.Count <= 1)
                return;

            vertexList.Clear();

            for (int i = 0; i < path.Count; i++)
                vertexList.Add(new VertexPositionColorTexture(PointFToXNAVector3(path[i]), color, XNAVector2.Zero));

            if (closed)
                vertexList.Add(new VertexPositionColorTexture(PointFToXNAVector3(path[0]), color, XNAVector2.Zero));

            bool succeed = FlushVertexList(vertexList, true, false, null);
            if (!succeed)
                return;

            DrawLineStrip(vertexList.Count - 1);
        }
    }
}