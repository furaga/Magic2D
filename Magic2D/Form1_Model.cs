using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Diagnostics;
namespace Magic2D
{
    public partial class Form1 : Form
    {
        Dictionary<string, Bitmap> sourceImageDict = new Dictionary<string, Bitmap>();
        Dictionary<string, Bitmap> composedImageDict = new Dictionary<string, Bitmap>();

        List<Operation> operationHistory = new List<Operation>();
        int operationIndex = -1;

        OpenFileDialog projectOpenFileDialog = new OpenFileDialog();
        SaveFileDialog projectSaveFileDialog = new SaveFileDialog();
        OpenFileDialog sourceImageOpenFileDialog = new OpenFileDialog();

        //

        Dictionary<string, SkeletonAnnotation> skeletonAnnotationDict = new Dictionary<string, SkeletonAnnotation>();
        string editingAnnotationKey = "";
        Matrix skeletonFittingCanvasTransform = new Matrix();
        float skeletonFittingCanvasScale = 1;
        BoneAnnotation addingBone = null;
        JointAnnotation nearestJoint = null;
        BoneAnnotation nearestBone = null;
        JointAnnotation selectJoint = null;
        BoneAnnotation selectBone = null;
        FormRefSkeleton formRefSkeleton = null;
        SkeletonAnnotation refSkeletonAnnotation = null;
        Matrix refSkeletonCanvasTransform = new Matrix();
        JointAnnotation refSkeletonNearestJoint = null;

        //

        Segmentation segmentation = new Segmentation();

        //

        Composition composition = new Composition();
        OpenFileDialog segmentImageOpenFileDialog = new OpenFileDialog();
        OpenFileDialog referenceImageOpenFileDialog = new OpenFileDialog();

        //

        List<AnimeCell> animeCells = new List<AnimeCell>();
        List<AnimeCell> addingCells = new List<AnimeCell>();
        List<AnimeCell> selectCells = new List<AnimeCell>();
        AnimeCell prevCell = null;
        bool playing;
        Stopwatch playStopwatch = new Stopwatch();
        float playTime = 0;

    }
}