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
using FLib;

namespace Magic2D
{
    public partial class Form1 : Form
    {
        void Initialize()
        {
            for (int i = sourceImageDict.Count - 1; i >= 0; i--)
                DeleteImage(sourceImageDict, sourceImageDict.Keys.ElementAt(i));
            for (int i = composedImageDict.Count - 1; i >= 0; i--)
                DeleteImage(composedImageDict, composedImageDict.Keys.ElementAt(i));
            for (int i = operationHistory.Count - 1; i >= 0; i--)
            {
                DisposeOperation(operationHistory[i]);
                operationHistory.RemoveAt(i);
            }
            operationIndex = -1;

            //------------------------------------------------------------

            for (int i = skeletonAnnotationDict.Count - 1; i >= 0; i--)
                skeletonAnnotationDict.Values.ElementAt(i).bmp.Dispose();
            skeletonAnnotationDict.Clear();
            editingAnnotationKey = "";
            skeletonFittingCanvasTransform = new Matrix();
            skeletonFittingCanvasScale = 1;
            addingBone = null;

            if (formRefSkeleton != null && !formRefSkeleton.IsDisposed)
                formRefSkeleton.Dispose();
            formRefSkeleton = null;
            refSkeletonAnnotation = null;
            refSkeletonCanvasTransform = new Matrix();
            //------------------------------------------------------------

            if (segmentation != null)
                segmentation.Dispose();
            segmentation = new Segmentation();

            //------------------------------------------------------------

            if (composition != null)
                composition.Dispose();
            composition = new Composition("./refSkeleton.skl");
            compositionCanvas.composition = composition;

            //------------------------------------------------------------

            animeCells.Clear();
            addingCells.Clear();
            selectCells.Clear();
            prevCell = null;
            playing = false;
            playStopwatch = new System.Diagnostics.Stopwatch();
            playTime = 0;
        }

        //----------------------------------------------------------------------

        void OpenProject()
        {
            projectOpenFileDialog.RestoreDirectory = true;
            projectOpenFileDialog.Filter = "*.m2d|*.m2d";
            if (projectOpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                OpenProject(projectOpenFileDialog.FileName);
        }

        public void OpenProject(string filepath)
        {
            Initialize();

            filepath = Path.GetFullPath(filepath);
            string root = Path.GetDirectoryName(filepath);
            if (!File.Exists(filepath))
                return;
            // todo
            // File.ReadeAllText(filepath, "");
            
            OpenImages(root, "1_sourceImages", sourceImageDict);
            OpenAnnotations(root, "2_annotations", skeletonAnnotationDict);
            OpenSegmentation(root, "3_segmentation", segmentation);
            OpenImages(root, "4_composedImages", composedImageDict);
            OpenAnimation(root, "5_animation", animeCells);
        }

        public static SkeletonAnnotation OpenRefSkeleton(string filepath, Bitmap refSkeletonBmp)
        {
            filepath = Path.GetFullPath(filepath);
            string root = Path.GetDirectoryName(filepath);
            if (!File.Exists(filepath))
                return null;

            return SkeletonAnnotation.Load(filepath, refSkeletonBmp);
        }

        public static void OpenImages(string root, string dirName, Dictionary<string, Bitmap> imageDict, List<Operation> opHist, ref int opIdx)
        {
            imageDict.Clear();
            string dir = Path.Combine(root, dirName);
            if (Directory.Exists(dir))
            {
                foreach (var f in Directory.GetFiles(dir))
                {
                    string key = Path.GetFileNameWithoutExtension(f);
                    using (var _bmp = new Bitmap(f))
                        AssignImage(imageDict, key, new Bitmap(_bmp), opHist, ref opIdx, false);
                }
            }
        }

        public void OpenImages(string root, string dirName, Dictionary<string, Bitmap> imageDict)
        {
            OpenImages(root, dirName, imageDict, operationHistory, ref operationIndex);
        }

        public static void OpenAnnotations(string root, string dirName, Dictionary<string, SkeletonAnnotation> anDict)
        {
            foreach (var an in anDict)
                an.Value.bmp.Dispose();

            anDict.Clear();

            string dir = Path.Combine(root, dirName);
            if (!Directory.Exists(dir))
                return;

            string p = Path.Combine(dir, "skeletonAnnotation.ska");
            if (!File.Exists(p))
                return;

            var lines = File.ReadAllLines(p);

            SkeletonAnnotation ann = null;

            foreach (var line in lines)
            {
                if (line.StartsWith("SkeletonAnnotation"))
                {
                    string key = line.Split('[').Last().Trim('[', ']');
                    anDict[key] = ann = new SkeletonAnnotation(null);
                }
                if (line.StartsWith("JointAnnotation"))
                {
                    if (ann == null)
                        continue;
                    string[] tokens = line.Split(',');
                    if (tokens.Length != 4)
                        continue;
                    string name = tokens[1];
                    float x, y;
                    if (!float.TryParse(tokens[2], out x) || !float.TryParse(tokens[3], out y))
                        continue;
                    ann.joints.Add(new JointAnnotation(name, new PointF(x, y)));
                }
                if (line.StartsWith("BoneAnnotation"))
                {
                    if (ann == null)
                        continue;
                    string[] tokens = line.Split(',');
                    if (tokens.Length != 3)
                        continue;
                    int srcIdx, dstIdx;
                    if (!int.TryParse(tokens[1], out srcIdx) || !int.TryParse(tokens[2], out dstIdx))
                        continue;
                    if (srcIdx < 0 && ann.joints.Count <= srcIdx)
                        continue;
                    if (dstIdx < 0 && ann.joints.Count <= dstIdx)
                        continue;
                    ann.bones.Add(new BoneAnnotation(ann.joints[srcIdx], ann.joints[dstIdx]));
                }
            }

            foreach (var kv in anDict)
            {
                string f = Path.Combine(dir, kv.Key + ".png");
                if (!File.Exists(f))
                    continue;
                using (var _bmp = new Bitmap(f))
                    anDict[kv.Key].bmp = new Bitmap(_bmp);
            }
        }

        public static List<PointF> StringToPointList(string text)
        {
            var ls = new List<PointF>();

            string[] tokens = text.Split(' ');
            for (int i = 0; i < tokens.Length; i++)
            {
                string[] ptTokens = tokens[i].Split(',');
                if (ptTokens.Length != 2)
                    return ls;
                float x, y;
                if (float.TryParse(ptTokens[0], out x) && float.TryParse(ptTokens[1], out y))
                    ls.Add(new PointF(x, y));
            }

            return ls;
        }

        void OpenSegmentation(string root, string dirName, Segmentation segmentation)
        {
            OpenSegmentation(root, dirName, segmentation, operationHistory, ref operationIndex);
        }

        public static void OpenSegmentation(string root, string dirName, Segmentation segmentation, List<Operation> opHist, ref int opIdx)
        {
            if (segmentation == null)
                return;

            segmentation.Clear();

            string dir = Path.Combine(root, dirName);
            if (!Directory.Exists(dir))
                return;

            // パラメータの読み込み
            string f = Path.Combine(dir, "segmentation.seg");
            if (!File.Exists(f))
                return;
            string[] lines = File.ReadAllLines(f);
            SegmentRoot sroot = null;
            Segment seg = null;
            foreach (var line in lines)
            {
                if (line.StartsWith("SegmentRoot:"))
                {
                    string key = line.Substring("SegmentRoot:".Length).Trim();
                    segmentation.segmentRootDict[key] = new SegmentRoot(null, null);
                    sroot = segmentation.segmentRootDict[key];
                    sroot.segments.Clear();
                }
                if (root == null)
                    continue;
                if (line.StartsWith("PathSegment:"))
                {
                    string name = line.Substring("PathSegment:".Length).Trim();
                    seg = new PathSegment(name, sroot);
                    sroot.segments.Add(seg);
                }
                if (seg == null)
                    continue;
                if (line.StartsWith("closed:"))
                {
                    string closedText = line.Substring("closed:".Length).Trim();
                    bool closed;
                    if (bool.TryParse(closedText, out closed))
                        seg._SetClosed(closed);
                }
                    if (line.StartsWith("offset:"))
                {
                    string offsetText = line.Substring("offset:".Length).Trim();
                    string[] tokens = offsetText.Split(',');
                    if (tokens.Length != 2)
                        continue;
                    int x, y;
                    if (int.TryParse(tokens[0], out x) && int.TryParse(tokens[1], out y))
                        seg.offset = new Point(x, y);
                }
                if (line.StartsWith("path:"))
                {
                    string pathText = line.Substring("path:".Length).Trim();
                    seg.path = StringToPointList(pathText);
                }
                if (line.StartsWith("section:"))
                {
                    string sectionText = line.Substring("section:".Length).Trim();
                    seg.section = StringToPointList(sectionText);
                }
                if (line.StartsWith("partingLine:"))
                {
                    string partingText = line.Substring("partingLine:".Length).Trim();
                    seg.partingLine = StringToPointList(partingText);
                }
            }

            // 画像の読み込み
            foreach (var kv in segmentation.segmentRootDict)
            {
                var bmpDict = new Dictionary<string, Bitmap>();
                OpenImages(dir, kv.Key + "_bmp", bmpDict, opHist, ref opIdx);

                if (bmpDict.ContainsKey("_root_"))
                    kv.Value.bmp = bmpDict["_root_"];

                foreach (var sg in kv.Value.segments)
                {
                    if (bmpDict.ContainsKey(sg.name))
                        sg.bmp = bmpDict[sg.name];
                }
            }

            // スケルトンの読み込み
            foreach (var kv in segmentation.segmentRootDict)
            {
                var anDict = new Dictionary<string, SkeletonAnnotation>();
                OpenAnnotations(dir, kv.Key + "_skeleton", anDict);

                if (anDict.ContainsKey("_root_"))
                    kv.Value.an = anDict["_root_"];

                foreach (var sg in kv.Value.segments)
                {
                    if (anDict.ContainsKey(sg.name))
                        sg.an = anDict[sg.name];
                }
            }
        }

        void OpenAnimation(string root, string dirName, List<AnimeCell> animeCells)
        {
            animeCells.Clear();

            string dir = Path.Combine(root, dirName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            string f = Path.Combine(dir, "animation.ani");
            if (!File.Exists(f))
                return;

            string[] lines = File.ReadAllLines(f);

            foreach (var line in lines)
            {
                string[] tokens = line.Split(',');
                if (tokens.Length != 2)
                    continue;
                if (!composedImageDict.ContainsKey(tokens[0]))
                    continue;
                int duration;
                if (!int.TryParse(tokens[1], out duration))
                    continue;
                animeCells.Add(new AnimeCell(tokens[0], duration));
            }
        }
        
        void SaveProject()
        {
            projectSaveFileDialog.RestoreDirectory = true;
            projectSaveFileDialog.Filter = "*.m2d|*.m2d";
            if (projectSaveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                SaveProject(projectSaveFileDialog.FileName);
        }

        public void SaveProject(string filepath)
        {
            filepath = Path.GetFullPath(filepath);
            string root = Path.GetDirectoryName(filepath);
            File.WriteAllText(filepath, "");
            SaveImages(root, "1_sourceImages", sourceImageDict);
            SaveAnnotations(root, "2_annotations", skeletonAnnotationDict);
            SaveSegmentation(root, "3_segmentation", segmentation);
            SaveImages(root, "4_composedImages", composedImageDict);
            SaveAnimation(root, "5_animation", animeCells);
        }

        void SaveImages(string root, string dirName, Dictionary<string, Bitmap> imageDict)
        {
            // 空のディレクトリを作って素材画像を保存
            string dir = Path.Combine(root, dirName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            foreach (var f in Directory.GetFiles(dir))
                File.Delete(f);
            foreach (var kv in imageDict)
            {
                string f = Path.Combine(dir, kv.Key + ".png");
                kv.Value.Save(f);
            }
        }

        void SaveAnnotations(string root, string dirName, Dictionary<string, SkeletonAnnotation> anDict)
        {
            try
            {
                string dir = Path.Combine(root, dirName);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                foreach (var f in Directory.GetFiles(dir))
                    File.Delete(f);

                List<string> lines = new List<string>();

                for (int i = 0; i < anDict.Count; i++)
                {
                    var kv = anDict.ElementAt(i);
                    string f = Path.Combine(dir, kv.Key + ".png");
                    if (kv.Value.bmp != null)
                        kv.Value.bmp.Save(f);

                    lines.Add("SkeletonAnnotation[" + kv.Key + "]");

                    for (int j = 0; j < kv.Value.joints.Count; j++)
                    {
                        JointAnnotation joint = kv.Value.joints[j];
                        lines.Add("JointAnnotation[" + j + "]," + joint.name + "," + joint.position.X + "," + joint.position.Y);
                    }
                    for (int j = 0; j < kv.Value.bones.Count; j++)
                    {
                        BoneAnnotation bone = kv.Value.bones[j];
                        if (kv.Value.joints.Contains(bone.src) && kv.Value.joints.Contains(bone.dst))
                        {
                            int srcIdx = kv.Value.joints.IndexOf(bone.src);
                            int dstIdx = kv.Value.joints.IndexOf(bone.dst);
                            lines.Add("BoneAnnotation[" + j + "]," + srcIdx + "," + dstIdx);
                        }
                    }

                    lines.Add("");
                }

                string p = Path.Combine(dir, "skeletonAnnotation.ska");
                File.WriteAllLines(p, lines.ToArray());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + e.StackTrace);
            }
        }

        public static void DeleteDirectory(System.IO.DirectoryInfo hDirectoryInfo)
        {
            try
            {
                // すべてのファイルの読み取り専用属性を解除する
                foreach (System.IO.FileInfo cFileInfo in hDirectoryInfo.GetFiles())
                {
                    if ((cFileInfo.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                    {
                        cFileInfo.Attributes = System.IO.FileAttributes.Normal;
                    }
                }

                // サブディレクトリ内の読み取り専用属性を解除する (再帰)
                foreach (System.IO.DirectoryInfo hDirInfo in hDirectoryInfo.GetDirectories())
                {
                    DeleteDirectory(hDirInfo);
                }

                // このディレクトリの読み取り専用属性を解除する
                if ((hDirectoryInfo.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                {
                    hDirectoryInfo.Attributes = System.IO.FileAttributes.Directory;
                }

                // このディレクトリを削除する
                hDirectoryInfo.Delete(true);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString() + e.StackTrace);
            }
        }

        void SaveSegmentation(string root, string dirName, Segmentation segmentation)
        {
            if (segmentation == null)
                return;

            string dir = Path.Combine(root, dirName);
            if (Directory.Exists(dir))
                DeleteDirectory(new DirectoryInfo(dir));

            Directory.CreateDirectory(dir);

            foreach (var kv in segmentation.segmentRootDict)
            {
                var anDict = new Dictionary<string, SkeletonAnnotation>();
                if (kv.Value.an != null)
                    anDict["_root_"] = kv.Value.an;
                foreach (var seg in kv.Value.segments)
                {
                    if (seg.an != null)
                        anDict[seg.name] = seg.an;
                }
                SaveAnnotations(dir, kv.Key + "_skeleton", anDict);
            }

            foreach (var kv in segmentation.segmentRootDict)
            {
                var bmpDict = new Dictionary<string, Bitmap>();
                if (kv.Value.bmp != null)
                    bmpDict["_root_"] = kv.Value.bmp;
                foreach (var seg in kv.Value.segments)
                {
                    if (seg.bmp != null)
                        bmpDict[seg.name] = seg.bmp;
                }
                SaveImages(dir, kv.Key + "_bmp", bmpDict);
            }

            List<string> lines = new List<string>();
            foreach (var kv in segmentation.segmentRootDict)
            {
                lines.Add("SegmentRoot:" + kv.Key);
                foreach (var seg in kv.Value.segments)
                {
                    lines.Add("PathSegment:" + seg.name);
                    lines.Add("offset:" + seg.offset.X + "," + seg.offset.Y);
                    lines.Add("closed:" + seg.Closed);
                    lines.Add("path:" + string.Join(" ", seg.path.Select(p => p.X + "," + p.Y).ToArray()));
                    lines.Add("section:" + string.Join(" ", seg.section.Select(p => p.X + "," + p.Y).ToArray()));
                    lines.Add("partingLine:" + string.Join(" ", seg.partingLine.Select(p => p.X + "," + p.Y).ToArray()));
                }
            }

            string f = Path.Combine(dir, "segmentation.seg");
            File.WriteAllLines(f, lines.ToArray());
        }

        void SaveAnimation(string root, string dirName, List<AnimeCell> animeCells)
        {
            string dir = Path.Combine(root, dirName);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            List<string> lines = new List<string>();
            foreach (var cell in animeCells)
                lines.Add(cell.key + "," + cell.durationMilliseconds);
            string f = Path.Combine(dir, "animation.ani");
            File.WriteAllLines(f, lines.ToArray());
        }

        //----------------------------------------------------------------------

        void Undo()
        {
            if (0 <= operationIndex && operationIndex < operationHistory.Count)
            {
                var op = operationHistory[operationIndex];
                var inst = op.instance ?? this;
                inst.GetType().GetMethod("undo_" + op.funcName).Invoke(inst, op.parameters.ToArray());
            }
            operationIndex = Math.Max(-1, operationIndex - 1);
        }

        void Redo()
        {
            int prev = operationIndex;
            operationIndex = Math.Min(operationHistory.Count - 1, operationIndex + 1);
            if (prev != operationIndex)
            {
                if (0 <= operationIndex && operationIndex < operationHistory.Count)
                {
                    var op = operationHistory[operationIndex];
                    var inst = op.instance ?? this;
                    inst.GetType().GetMethod("redo_" + op.funcName).Invoke(inst, op.parameters.ToArray());
                }
            }
        }

        void AddOperation(Operation op)
        {
            AddOperation(op, operationHistory, ref operationIndex);
        }

        public static void AddOperation(Operation op, List<Operation> operationHistory, ref int operationIndex)
        {
            if (op == null)
                return;

            operationIndex = operationIndex + 1;
            if (operationIndex >= operationHistory.Count)
            {
                operationHistory.Add(op);
                operationIndex = operationHistory.Count - 1;
            }
            else
            {
                for (int i = operationHistory.Count - 1; i >= operationIndex; i--)
                {
                    DisposeOperation(operationHistory[i], operationHistory, ref operationIndex);
                    operationHistory.RemoveAt(i);
                }
                operationHistory.Add(op);
            }
        }

        void DisposeOperation(Operation op)
        {
            DisposeOperation(op, operationHistory, ref operationIndex);
        }

        public static void DisposeOperation(Operation op, List<Operation> operationHistory, ref int operationIndex)
        {
            if (op == null) return;
            if (string.IsNullOrWhiteSpace(op.funcName)) return;
            if (op.parameters == null) return;

            for (int i = 0; i < op.parameters.Count; i++)
            {
                // bitmapなどの引数は破棄する
                if (op.parameters[i] is IDisposable)
                    (op.parameters[i] as IDisposable).Dispose();
            }
        }

        //-------------------------------------
        // 記録操作:画像登録
        //-------------------------------------

        public static void AssignImage(Dictionary<string, Bitmap> imageDict, string key, Bitmap bmp, List<Operation> opHist, ref int opIdx, bool recordOperation = true)
        {
            DeleteImage(imageDict, key, opHist, ref opIdx);

            if (recordOperation)
                AddOperation(new Operation()
                {
                    funcName = "AssignImage",
                    parameters = new List<object>()
                    {
                        imageDict as object,
                        key as object,
                        new Bitmap(bmp) as object,
                    }
                }, opHist, ref opIdx);

            imageDict[key] = bmp;
        }

        public void AssignImage(Dictionary<string, Bitmap> imageDict, string key, Bitmap bmp, bool recordOperation = true)
        {
            AssignImage(imageDict, key, bmp, operationHistory, ref operationIndex, recordOperation);
        }

        public void redo_AssignImage(Dictionary<string, Bitmap> imageDict, string key, Bitmap bmp)
        {
            AssignImage(imageDict, key, new Bitmap(bmp), false);
        }
        public void undo_AssignImage(Dictionary<string, Bitmap> imageDict, string key, Bitmap bmp)
        {
            DeleteImage(imageDict, key, null, false);
        }

        //-------------------------------------
        // 記録操作:画像削除
        //-------------------------------------

        public static void DeleteImage(Dictionary<string, Bitmap> imageDict, string key, List<Operation> opHist, ref int opIdx, bool recordOperation = true, Bitmap dummy_bmp = null)
        {
            if (imageDict.ContainsKey(key))
            {
                if (recordOperation)
                    AddOperation(new Operation()
                    {
                        funcName = "DeleteImage",
                        parameters = new List<object>()
                        {
                            imageDict as object,
                            key as object,
                            new Bitmap(imageDict[key]) as object,
                        }
                    }, opHist, ref opIdx);

                imageDict[key].Dispose();
                imageDict.Remove(key);
            }
        }

        void DeleteImage(Dictionary<string, Bitmap> imageDict, string key, Bitmap dummy_bmp = null, bool recordOperation = true)
        {
            DeleteImage(imageDict, key, operationHistory, ref operationIndex, recordOperation, dummy_bmp);
        }

        public void redo_DeleteImage(Dictionary<string, Bitmap> imageDict, string key, Bitmap dummy_bmp)
        {
            DeleteImage(imageDict, key, null, false);
        }
        public void undo_DeleteImage(Dictionary<string, Bitmap> imageDict, string key, Bitmap bmp)
        {
            AssignImage(imageDict, key, new Bitmap(bmp), false);
        }

        //----------------------------------------------------------------------
        // Tab:SourceImage
        //----------------------------------------------------------------------

        string GetNewSourceImageKey(string prefix)
        {
            return GetNewKey(prefix, sourceImageDict.Keys.ToList());
        }

        string GetNewKey(string prefix, List<string> keys)
        {
            string key;
            int id = 0;
            while (true)
            {
                key = prefix + (id++);
                if (!keys.Any(k => k.StartsWith(key)))
                    break;
            }
            return key;
        }

        void TakeScreenshotOfSourceImage()
        {
            Bitmap bmp = UI.TakeScreenshotInteractive(this);
            if (bmp != null)
            {
                string key = GetNewSourceImageKey("scr");
                System.IO.Path.GetFileName(sourceImageOpenFileDialog.FileName);
                AssignImage(sourceImageDict, key, bmp);
            }
        }

        void LoadSourceImage()
        {
            sourceImageOpenFileDialog.RestoreDirectory = true;
            sourceImageOpenFileDialog.Filter = "*.png,*.bmp,*.jpg|*.png;*.bmp;*.jpg";
            sourceImageOpenFileDialog.Multiselect = true;
            if (sourceImageOpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (var filename in sourceImageOpenFileDialog.FileNames)
                {
                    using (var _bmp = new Bitmap(filename))
                    {
                        Bitmap bmp = new Bitmap(_bmp);
                        string key = System.IO.Path.GetFileNameWithoutExtension(filename);
                        AssignImage(sourceImageDict, key, bmp);
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        // Tab:Skeleton Fitting
        //----------------------------------------------------------------------

        void SetEditingAnnotation(string key, bool record = true)
        {
            if (sourceImageDict.ContainsKey(key))
            {
                if (record)
                    AddOperation(new Operation()
                    {
                        funcName = "SetEditingAnnotation",
                        parameters = new List<object>()
                        {
                            editingAnnotationKey,
                            key,
                        }
                    });

                if (skeletonAnnotationDict.ContainsKey(key))
                {
                    if (skeletonAnnotationDict[key].bmp != null)
                        skeletonAnnotationDict[key].bmp.Dispose();
                    skeletonAnnotationDict[key].bmp = new Bitmap(sourceImageDict[key]);
                }
                else
                {
                    skeletonAnnotationDict[key] = new SkeletonAnnotation(new Bitmap(sourceImageDict[key]));
                }
                editingAnnotationKey = key;
            }
        }
        public void redo_SetEditingAnnotation(string prevKey, string key)
        {
            SetEditingAnnotation(key, false);
        }
        public void undo_SetEditingAnnotation(string prevKey, string key)
        {
            SetEditingAnnotation(prevKey, false);
        }

        SkeletonAnnotation GetEditingAnnotation()
        {
            if (skeletonAnnotationDict.ContainsKey(editingAnnotationKey))
                return skeletonAnnotationDict[editingAnnotationKey];
            return null;
        }

        private void DeleteEditingBone()
        {
            addingBone = null;
        }

        private void CreateOrCompleteEditingBone(SkeletonAnnotation an, JointAnnotation joint)
        {
            if (an == null || joint == null)
            {
                DeleteEditingBone();
                return;
            }

            if (addingBone == null)
            {
                addingBone = new BoneAnnotation(joint, new JointAnnotation("[dummy]", joint.position));
                selectBone = null;
                selectJoint = null;
            }
            else
            {
                addingBone.dst = joint;
                if (addingBone.src != addingBone.dst)
                    AssignBoneAnnotation(an, addingBone);
                addingBone = new BoneAnnotation(joint, new JointAnnotation("[dummy]", joint.position));
                selectBone = null;
                selectJoint = null;
            }
        }
        private void UpdateEditingBone(SkeletonAnnotation an, JointAnnotation joint)
        {
            if (an == null || joint == null)
            {
                DeleteEditingBone();
                return;
            }
            if (addingBone != null)
                addingBone.dst = joint;
        }
        
        //
        // joint
        //
        private void AssignJointAnnotation(SkeletonAnnotation an, JointAnnotation joint, bool record = true)
        {
            if (an == null || joint == null)
            {
                DeleteEditingBone();
                return;
            }

            if (an.joints.Any(j => j.name == joint.name && j.position == joint.position))
                return;

            if (record)
                AddOperation(new Operation()
                {
                    funcName = "AssignJointAnnotation",
                    parameters = new List<object>() { an, joint, },
                });

            an.joints.Add(joint);
        }

        private void DeleteJointAnnotation(SkeletonAnnotation an, JointAnnotation joint, bool record = true)
        {
            if (an == null || joint == null)
            {
                DeleteEditingBone();
                return;
            }

            if (record)
                AddOperation(new Operation()
                {
                    funcName = "DeleteJointAnnotation",
                    parameters = new List<object>() { an, joint, new List<BoneAnnotation>(an.bones) },
                });

            // 関連するボーンを削除
            for (int i = an.bones.Count - 1; i >= 0; i--)
            {
                if ((an.bones[i].src.name == joint.name && an.bones[i].src.position == joint.position) ||
                    (an.bones[i].dst.name == joint.name && an.bones[i].dst.position == joint.position))
                    an.bones.RemoveAt(i);
            }
            // 関節を削除
            for (int i = an.joints.Count - 1; i >= 0; i--)
            {
                if (an.joints[i].name == joint.name && an.joints[i].position == joint.position)
                    an.joints.RemoveAt(i);
            }
        }
        public void redo_AssignJointAnnotation(SkeletonAnnotation an, JointAnnotation joint)
        {
            AssignJointAnnotation(an, joint, false);
        }
        public void undo_AssignJointAnnotation(SkeletonAnnotation an, JointAnnotation joint)
        {
            DeleteJointAnnotation(an, joint, false);
        }
        public void redo_DeleteJointAnnotation(SkeletonAnnotation an, JointAnnotation joint, List<BoneAnnotation> bones)
        {
            DeleteJointAnnotation(an, joint, false);
        }
        public void undo_DeleteJointAnnotation(SkeletonAnnotation an, JointAnnotation joint, List<BoneAnnotation> bones)
        {
            AssignJointAnnotation(an, joint, false);
            foreach (var bone in bones)
                AssignBoneAnnotation(an, bone, false);
        }


        //
        // bone
        //
        private void AssignBoneAnnotation(SkeletonAnnotation an, BoneAnnotation bone, bool record = true)
        {
            if (an == null || bone == null)
            {
                DeleteEditingBone();
                return;
            }

            if (bone.src == bone.dst)
                return;
            if (an.bones.Any(b => b.src == bone.src && b.dst == bone.dst))
                return;
            if (an.bones.Any(b => b.dst == bone.src && b.src == bone.dst))
                return;

            if (record)
                AddOperation(new Operation()
                {
                    funcName = "AssignBoneAnnotation",
                    parameters = new List<object>() { an, bone, },
                });

            an.bones.Add(bone);
        }
        private void DeleteBoneAnnotation(SkeletonAnnotation an, BoneAnnotation bone, bool record = true)
        {
            if (an == null || bone == null)
            {
                DeleteEditingBone();
                return;
            }

            if (record)
                AddOperation(new Operation()
                {
                    funcName = "DeleteBoneAnnotation",
                    parameters = new List<object>() { an, bone, },
                });

            for (int i = an.bones.Count - 1; i >= 0; i--)
            {
                if (an.bones[i].src == bone.src && an.bones[i].dst == bone.dst)
                    an.bones.RemoveAt(i);
            }
        }
        public void redo_AssignBoneAnnotation(SkeletonAnnotation an, BoneAnnotation bone)
        {
            AssignBoneAnnotation(an, bone, false);
        }
        public void undo_AssignBoneAnnotation(SkeletonAnnotation an, BoneAnnotation bone)
        {
            DeleteBoneAnnotation(an, bone, false);
        }
        public void redo_DeleteBoneAnnotation(SkeletonAnnotation an, BoneAnnotation bone)
        {
            DeleteBoneAnnotation(an, bone, false);
        }
        public void undo_DeleteBoneAnnotation(SkeletonAnnotation an, BoneAnnotation bone)
        {
            AssignBoneAnnotation(an, bone, false);
        }

        private void SelectJointAnnotation(SkeletonAnnotation an, JointAnnotation jointAnnotation)
        {
            addingBone = null;
            selectBone = null;
            selectJoint = an.joints.Contains(jointAnnotation) ? jointAnnotation : null;
        }

        private void SelectBoneAnnotation(SkeletonAnnotation an, BoneAnnotation boneAnnotation)
        {
            addingBone = null;
            selectBone = an.bones.Contains(boneAnnotation) ? boneAnnotation : null;
            selectJoint = null;
        }

        //
        // pointを transform * baseRectSize を(scale.X,scale.Y)とする座標系へと拡大変換
        // 
        public static PointF InvertCoordinate(PointF point, Matrix transform)
        {
            Matrix invMatrix = transform.Clone();
            if (!invMatrix.IsInvertible)
                return point;
            invMatrix.Invert();
            PointF[] pt = new [] { point } ;
            invMatrix.TransformPoints(pt);
            return pt[0];
        }

        //--------------------------------------------------------------
        // tab:Animation
        //--------------------------------------------------------------

        private void SetAddingCells(List<string> imageKeys)
        {
            if (imageKeys.Count <= 0)
                addingCells = null;
            addingCells = new List<AnimeCell>();
            for (int i = 0; i < imageKeys.Count; i++)
                addingCells.Add(new AnimeCell(imageKeys[i]));
        }

        private void AssignAnimeCells(List<AnimeCell> cells, AnimeCell prevCell)
        {
            var prevCells = new List<AnimeCell>();
            prevCells.Add(prevCell);
            for (int i = 0; i < cells.Count - 1; i++)
                prevCells.Add(cells[i]);
            AssignAnimeCells(cells, prevCells, true);
        }

        private void AssignAnimeCells(List<AnimeCell> cells, List<AnimeCell> prevCells, bool record = true)
        {
            if (cells == null || cells.Count <= 0 || prevCells.Count (c => c == null || animeCells.Contains(c)) <= 0)
                return;

            if (record)
            {
                AddOperation(new Operation()
                {
                    funcName = "AssignAnimeCells",
                    parameters = new List<object>()
                    {
                        new List<AnimeCell>(cells),
                        new List<AnimeCell>(prevCells),
                    }
                });
            }

            bool[] added = new bool[prevCells.Count];
            while (true)
            {
                bool addAny = false;
                for (int i = 0; i < cells.Count; i++)
                {
                    if (added[i])
                        continue;
                    if (prevCells[i] == null)
                    {
                        animeCells.Insert(0, cells[i]);
                        added[i] = true;
                        addAny = true;
                    }
                    else if (animeCells.Contains(prevCells[i]))
                    {
                        int idx = animeCells.IndexOf(prevCells[i]) + 1;
                        animeCells.Insert(idx, cells[i]);
                        added[i] = true;
                        addAny = true;
                    }
                }
                if (!addAny)
                    break;
            }
        }
             
        private void DeleteAnimeCells(List<AnimeCell> cells, bool record = true)
        {
            if (cells == null || cells.Count <= 0 || cells.Count(c => animeCells.Contains(c)) <= 0)
                return;

            if (record)
            {
                var prevCells = new List<AnimeCell>();
                for (int i = 0; i < cells.Count; i++)
                {
                    if (!animeCells.Contains(cells[i]))
                        prevCells.Add(null);
                    else
                    {
                        int idx = animeCells.IndexOf(cells[i]) - 1;
                        if (idx < 0 || animeCells.Count <= idx)
                            prevCells.Add(null);
                        else
                            prevCells.Add(animeCells[idx]);
                    }
                }
                AddOperation(new Operation()
                {
                    funcName = "DeleteAnimeCells",
                    parameters = new List<object>()
                    {
                        new List<AnimeCell>(cells),
                        prevCells,
                    }
                });
            }

            for (int i = cells.Count - 1; i >= 0; i--)
                animeCells.Remove(cells[i]);
        }

        public void redo_AssignAnimeCells(List<AnimeCell> cells, List<AnimeCell> prevCells)
        {
            AssignAnimeCells(cells, prevCells, false);
        }
        public void undo_AssignAnimeCells(List<AnimeCell> cells, List<AnimeCell> prevCell)
        {
            DeleteAnimeCells(cells, false);
        }
        public void redo_DeleteAnimeCells(List<AnimeCell> cells, List<AnimeCell> prevCells)
        {
            DeleteAnimeCells(cells, false);
        }
        public void undo_DeleteAnimeCells(List<AnimeCell> cells, List<AnimeCell> prevCells)
        {
            AssignAnimeCells(cells, prevCells, false);
        }

        private void PlayAnimation(Timer timer)
        {
            playing = true;
            timer.Interval = 33;
            timer.Tick += animeTimer_Tick;
            timer.Enabled = true;
            playStopwatch.Restart();
        }

        private void StopAnimation(Timer timer)
        {
            playing = false;
            timer.Enabled = false;
            playStopwatch.Stop();
        }
    }
}
