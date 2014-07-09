namespace Magic2D
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("", 0);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("", 1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("", 2);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Head");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Neck");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("・・・");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Joints", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Parting lines");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Full", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("LShoulder");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("LElbow");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("LWrist");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Joints", new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode8,
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Parting lines");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("LArm", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode11});
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("ノード13");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("・・・", new System.Windows.Forms.TreeNode[] {
            treeNode13});
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("0", "comp0.png");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("2", "comp1.png");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("", "comp2.png");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openOToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabSourceImages = new System.Windows.Forms.TabPage();
            this.sourceImagesSplitContainer = new System.Windows.Forms.SplitContainer();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonScreenshot = new System.Windows.Forms.Button();
            this.sourceImageView = new System.Windows.Forms.ListView();
            this.menuSourceImages = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.skeletonSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.segmentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sourceImageList = new System.Windows.Forms.ImageList(this.components);
            this.tabSkeletonFitting = new System.Windows.Forms.TabPage();
            this.skeletonSplitContainer = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.skeletonFittingCanvas = new System.Windows.Forms.PictureBox();
            this.splitContainer6 = new System.Windows.Forms.SplitContainer();
            this.boneSelectRadioButton = new System.Windows.Forms.RadioButton();
            this.jointSelectRadioButton = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.jointNameTextBox = new System.Windows.Forms.TextBox();
            this.jointAddRadioButton = new System.Windows.Forms.RadioButton();
            this.boneAddRadioButton = new System.Windows.Forms.RadioButton();
            this.splitContainer7 = new System.Windows.Forms.SplitContainer();
            this.refSkeletonCanvas = new System.Windows.Forms.PictureBox();
            this.editRefJointsButton = new System.Windows.Forms.Button();
            this.tabSegmentation = new System.Windows.Forms.TabPage();
            this.segmentSplitContainer = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.segmentCanvas = new System.Windows.Forms.PictureBox();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.segmentListView = new System.Windows.Forms.TreeView();
            this.segmentSkeletonButton = new System.Windows.Forms.RadioButton();
            this.segmentSectionButton = new System.Windows.Forms.RadioButton();
            this.segmentPartingButton = new System.Windows.Forms.RadioButton();
            this.segmentSegmentButton = new System.Windows.Forms.RadioButton();
            this.segmentNameTextBox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.tabComposition = new System.Windows.Forms.TabPage();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.splitContainer10 = new System.Windows.Forms.SplitContainer();
            this.splitContainer8 = new System.Windows.Forms.SplitContainer();
            this.segmentImageView = new System.Windows.Forms.ListView();
            this.segmentImageList = new System.Windows.Forms.ImageList(this.components);
            this.loadSegmentImageButton = new System.Windows.Forms.Button();
            this.compositionCanvas = new Magic2D.CompositionCanvasControl();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.reverseCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.scaleYTrackbar = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.scaleXTrackbar = new System.Windows.Forms.TrackBar();
            this.panel2 = new System.Windows.Forms.Panel();
            this.controlPointMoveButton = new System.Windows.Forms.RadioButton();
            this.segmentMoveButton = new System.Windows.Forms.RadioButton();
            this.skeletonMoveButton = new System.Windows.Forms.RadioButton();
            this.referenceImageView = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.LoadReferenceImageButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.posYTrackbar = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.rotTrackbar = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.posXTrackbar = new System.Windows.Forms.TrackBar();
            this.tabAnimation = new System.Windows.Forms.TabPage();
            this.splitContainer9 = new System.Windows.Forms.SplitContainer();
            this.composedImageView = new System.Windows.Forms.ListView();
            this.composedImageList = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer11 = new System.Windows.Forms.SplitContainer();
            this.splitContainer14 = new System.Windows.Forms.SplitContainer();
            this.cellListView = new System.Windows.Forms.PictureBox();
            this.animeDurationTextBox = new System.Windows.Forms.TextBox();
            this.splitContainer12 = new System.Windows.Forms.SplitContainer();
            this.animeView = new System.Windows.Forms.PictureBox();
            this.splitContainer13 = new System.Windows.Forms.SplitContainer();
            this.playButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.seekbar = new System.Windows.Forms.TrackBar();
            this.loopCheckBox = new System.Windows.Forms.CheckBox();
            this.animeTimer = new System.Windows.Forms.Timer(this.components);
            this.segmentContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.segmentSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearPathCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteLastPointDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compositionContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.brindForwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendBackwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bringToFrontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newSegmentNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabSourceImages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sourceImagesSplitContainer)).BeginInit();
            this.sourceImagesSplitContainer.Panel1.SuspendLayout();
            this.sourceImagesSplitContainer.Panel2.SuspendLayout();
            this.sourceImagesSplitContainer.SuspendLayout();
            this.menuSourceImages.SuspendLayout();
            this.tabSkeletonFitting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.skeletonSplitContainer)).BeginInit();
            this.skeletonSplitContainer.Panel2.SuspendLayout();
            this.skeletonSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.skeletonFittingCanvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).BeginInit();
            this.splitContainer6.Panel1.SuspendLayout();
            this.splitContainer6.Panel2.SuspendLayout();
            this.splitContainer6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).BeginInit();
            this.splitContainer7.Panel1.SuspendLayout();
            this.splitContainer7.Panel2.SuspendLayout();
            this.splitContainer7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.refSkeletonCanvas)).BeginInit();
            this.tabSegmentation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.segmentSplitContainer)).BeginInit();
            this.segmentSplitContainer.Panel2.SuspendLayout();
            this.segmentSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.segmentCanvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.tabComposition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer10)).BeginInit();
            this.splitContainer10.Panel1.SuspendLayout();
            this.splitContainer10.Panel2.SuspendLayout();
            this.splitContainer10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).BeginInit();
            this.splitContainer8.Panel1.SuspendLayout();
            this.splitContainer8.Panel2.SuspendLayout();
            this.splitContainer8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleYTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleXTrackbar)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.referenceImageView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posYTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotTrackbar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.posXTrackbar)).BeginInit();
            this.tabAnimation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).BeginInit();
            this.splitContainer9.Panel1.SuspendLayout();
            this.splitContainer9.Panel2.SuspendLayout();
            this.splitContainer9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer11)).BeginInit();
            this.splitContainer11.Panel1.SuspendLayout();
            this.splitContainer11.Panel2.SuspendLayout();
            this.splitContainer11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer14)).BeginInit();
            this.splitContainer14.Panel1.SuspendLayout();
            this.splitContainer14.Panel2.SuspendLayout();
            this.splitContainer14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cellListView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer12)).BeginInit();
            this.splitContainer12.Panel1.SuspendLayout();
            this.splitContainer12.Panel2.SuspendLayout();
            this.splitContainer12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.animeView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer13)).BeginInit();
            this.splitContainer13.Panel1.SuspendLayout();
            this.splitContainer13.Panel2.SuspendLayout();
            this.splitContainer13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seekbar)).BeginInit();
            this.segmentContextMenuStrip.SuspendLayout();
            this.compositionContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileFToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(817, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileFToolStripMenuItem
            // 
            this.fileFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openOToolStripMenuItem,
            this.saveSToolStripMenuItem,
            this.toolStripSeparator1,
            this.exportEToolStripMenuItem});
            this.fileFToolStripMenuItem.Name = "fileFToolStripMenuItem";
            this.fileFToolStripMenuItem.Size = new System.Drawing.Size(57, 22);
            this.fileFToolStripMenuItem.Text = "File(&F)";
            // 
            // openOToolStripMenuItem
            // 
            this.openOToolStripMenuItem.Name = "openOToolStripMenuItem";
            this.openOToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openOToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openOToolStripMenuItem.Text = "Open(&O)";
            this.openOToolStripMenuItem.Click += new System.EventHandler(this.openOToolStripMenuItem_Click);
            // 
            // saveSToolStripMenuItem
            // 
            this.saveSToolStripMenuItem.Name = "saveSToolStripMenuItem";
            this.saveSToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveSToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.saveSToolStripMenuItem.Text = "Save(&S)";
            this.saveSToolStripMenuItem.Click += new System.EventHandler(this.saveSToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(174, 6);
            // 
            // exportEToolStripMenuItem
            // 
            this.exportEToolStripMenuItem.Name = "exportEToolStripMenuItem";
            this.exportEToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.exportEToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.exportEToolStripMenuItem.Text = "Export(&E)";
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabSourceImages);
            this.tabControl.Controls.Add(this.tabSkeletonFitting);
            this.tabControl.Controls.Add(this.tabSegmentation);
            this.tabControl.Controls.Add(this.tabComposition);
            this.tabControl.Controls.Add(this.tabAnimation);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.tabControl.Location = new System.Drawing.Point(0, 26);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(817, 588);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabSourceImages
            // 
            this.tabSourceImages.Controls.Add(this.sourceImagesSplitContainer);
            this.tabSourceImages.Location = new System.Drawing.Point(4, 29);
            this.tabSourceImages.Name = "tabSourceImages";
            this.tabSourceImages.Padding = new System.Windows.Forms.Padding(3);
            this.tabSourceImages.Size = new System.Drawing.Size(809, 555);
            this.tabSourceImages.TabIndex = 0;
            this.tabSourceImages.Text = "Source Images";
            this.tabSourceImages.UseVisualStyleBackColor = true;
            // 
            // sourceImagesSplitContainer
            // 
            this.sourceImagesSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceImagesSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.sourceImagesSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.sourceImagesSplitContainer.Name = "sourceImagesSplitContainer";
            this.sourceImagesSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // sourceImagesSplitContainer.Panel1
            // 
            this.sourceImagesSplitContainer.Panel1.Controls.Add(this.buttonLoad);
            this.sourceImagesSplitContainer.Panel1.Controls.Add(this.buttonScreenshot);
            // 
            // sourceImagesSplitContainer.Panel2
            // 
            this.sourceImagesSplitContainer.Panel2.Controls.Add(this.sourceImageView);
            this.sourceImagesSplitContainer.Size = new System.Drawing.Size(803, 549);
            this.sourceImagesSplitContainer.SplitterDistance = 34;
            this.sourceImagesSplitContainer.TabIndex = 0;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(126, 3);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(90, 28);
            this.buttonLoad.TabIndex = 1;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonScreenshot
            // 
            this.buttonScreenshot.Location = new System.Drawing.Point(5, 3);
            this.buttonScreenshot.Name = "buttonScreenshot";
            this.buttonScreenshot.Size = new System.Drawing.Size(115, 28);
            this.buttonScreenshot.TabIndex = 0;
            this.buttonScreenshot.Text = "Screenshot";
            this.buttonScreenshot.UseVisualStyleBackColor = true;
            this.buttonScreenshot.Click += new System.EventHandler(this.buttonScreenshot_Click);
            // 
            // sourceImageView
            // 
            this.sourceImageView.ContextMenuStrip = this.menuSourceImages;
            this.sourceImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceImageView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.sourceImageView.LargeImageList = this.sourceImageList;
            this.sourceImageView.Location = new System.Drawing.Point(0, 0);
            this.sourceImageView.Name = "sourceImageView";
            this.sourceImageView.Size = new System.Drawing.Size(803, 511);
            this.sourceImageView.SmallImageList = this.sourceImageList;
            this.sourceImageView.TabIndex = 0;
            this.sourceImageView.UseCompatibleStateImageBehavior = false;
            this.sourceImageView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.sourceImageView_KeyDown);
            this.sourceImageView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.sourceImageView_KeyUp);
            this.sourceImageView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sourceImageView_MouseDoubleClick);
            // 
            // menuSourceImages
            // 
            this.menuSourceImages.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.menuSourceImages.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.skeletonSToolStripMenuItem,
            this.segmentationToolStripMenuItem});
            this.menuSourceImages.Name = "menuSourceImages";
            this.menuSourceImages.Size = new System.Drawing.Size(187, 52);
            this.menuSourceImages.Opening += new System.ComponentModel.CancelEventHandler(this.menuSourceImages_Opening);
            // 
            // skeletonSToolStripMenuItem
            // 
            this.skeletonSToolStripMenuItem.Name = "skeletonSToolStripMenuItem";
            this.skeletonSToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.skeletonSToolStripMenuItem.Text = "Skeleton";
            this.skeletonSToolStripMenuItem.Click += new System.EventHandler(this.skeletonSToolStripMenuItem_Click);
            // 
            // segmentationToolStripMenuItem
            // 
            this.segmentationToolStripMenuItem.Name = "segmentationToolStripMenuItem";
            this.segmentationToolStripMenuItem.Size = new System.Drawing.Size(186, 24);
            this.segmentationToolStripMenuItem.Text = "Segmentation";
            this.segmentationToolStripMenuItem.Click += new System.EventHandler(this.segmentationToolStripMenuItem_Click);
            // 
            // sourceImageList
            // 
            this.sourceImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("sourceImageList.ImageStream")));
            this.sourceImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.sourceImageList.Images.SetKeyName(0, "scr0.png");
            this.sourceImageList.Images.SetKeyName(1, "scr1.png");
            this.sourceImageList.Images.SetKeyName(2, "scr2.png");
            // 
            // tabSkeletonFitting
            // 
            this.tabSkeletonFitting.Controls.Add(this.skeletonSplitContainer);
            this.tabSkeletonFitting.Location = new System.Drawing.Point(4, 29);
            this.tabSkeletonFitting.Name = "tabSkeletonFitting";
            this.tabSkeletonFitting.Padding = new System.Windows.Forms.Padding(3);
            this.tabSkeletonFitting.Size = new System.Drawing.Size(809, 555);
            this.tabSkeletonFitting.TabIndex = 1;
            this.tabSkeletonFitting.Text = "Skeleton Fitting";
            this.tabSkeletonFitting.UseVisualStyleBackColor = true;
            // 
            // skeletonSplitContainer
            // 
            this.skeletonSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skeletonSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.skeletonSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.skeletonSplitContainer.Name = "skeletonSplitContainer";
            // 
            // skeletonSplitContainer.Panel2
            // 
            this.skeletonSplitContainer.Panel2.Controls.Add(this.splitContainer4);
            this.skeletonSplitContainer.Size = new System.Drawing.Size(803, 549);
            this.skeletonSplitContainer.SplitterDistance = 187;
            this.skeletonSplitContainer.TabIndex = 1;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.skeletonFittingCanvas);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.splitContainer6);
            this.splitContainer4.Size = new System.Drawing.Size(612, 549);
            this.splitContainer4.SplitterDistance = 398;
            this.splitContainer4.TabIndex = 0;
            // 
            // skeletonFittingCanvas
            // 
            this.skeletonFittingCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.skeletonFittingCanvas.Location = new System.Drawing.Point(0, 0);
            this.skeletonFittingCanvas.Name = "skeletonFittingCanvas";
            this.skeletonFittingCanvas.Size = new System.Drawing.Size(398, 549);
            this.skeletonFittingCanvas.TabIndex = 0;
            this.skeletonFittingCanvas.TabStop = false;
            this.skeletonFittingCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.skeletonFittingCanvas_Paint);
            this.skeletonFittingCanvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.skeletonFittingCanvas_MouseClick);
            this.skeletonFittingCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.skeletonFittingCanvas_MouseDown);
            this.skeletonFittingCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.skeletonFittingCanvas_MouseMove);
            // 
            // splitContainer6
            // 
            this.splitContainer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer6.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer6.Location = new System.Drawing.Point(0, 0);
            this.splitContainer6.Name = "splitContainer6";
            this.splitContainer6.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer6.Panel1
            // 
            this.splitContainer6.Panel1.Controls.Add(this.boneSelectRadioButton);
            this.splitContainer6.Panel1.Controls.Add(this.jointSelectRadioButton);
            this.splitContainer6.Panel1.Controls.Add(this.label11);
            this.splitContainer6.Panel1.Controls.Add(this.label10);
            this.splitContainer6.Panel1.Controls.Add(this.jointNameTextBox);
            this.splitContainer6.Panel1.Controls.Add(this.jointAddRadioButton);
            this.splitContainer6.Panel1.Controls.Add(this.boneAddRadioButton);
            // 
            // splitContainer6.Panel2
            // 
            this.splitContainer6.Panel2.Controls.Add(this.splitContainer7);
            this.splitContainer6.Size = new System.Drawing.Size(210, 549);
            this.splitContainer6.SplitterDistance = 134;
            this.splitContainer6.TabIndex = 6;
            // 
            // boneSelectRadioButton
            // 
            this.boneSelectRadioButton.AutoSize = true;
            this.boneSelectRadioButton.Location = new System.Drawing.Point(72, 74);
            this.boneSelectRadioButton.Name = "boneSelectRadioButton";
            this.boneSelectRadioButton.Size = new System.Drawing.Size(73, 24);
            this.boneSelectRadioButton.TabIndex = 7;
            this.boneSelectRadioButton.Text = "Select";
            this.boneSelectRadioButton.UseVisualStyleBackColor = true;
            // 
            // jointSelectRadioButton
            // 
            this.jointSelectRadioButton.AutoSize = true;
            this.jointSelectRadioButton.Location = new System.Drawing.Point(72, 26);
            this.jointSelectRadioButton.Name = "jointSelectRadioButton";
            this.jointSelectRadioButton.Size = new System.Drawing.Size(73, 24);
            this.jointSelectRadioButton.TabIndex = 6;
            this.jointSelectRadioButton.Text = "Select";
            this.jointSelectRadioButton.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(4, 53);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 20);
            this.label11.TabIndex = 5;
            this.label11.Text = "Bone";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 20);
            this.label10.TabIndex = 4;
            this.label10.Text = "Joint";
            // 
            // jointNameTextBox
            // 
            this.jointNameTextBox.Location = new System.Drawing.Point(7, 103);
            this.jointNameTextBox.Name = "jointNameTextBox";
            this.jointNameTextBox.Size = new System.Drawing.Size(199, 28);
            this.jointNameTextBox.TabIndex = 3;
            this.jointNameTextBox.Text = "Head";
            this.jointNameTextBox.TextChanged += new System.EventHandler(this.jointNameTextBox_TextChanged);
            // 
            // jointAddRadioButton
            // 
            this.jointAddRadioButton.AutoSize = true;
            this.jointAddRadioButton.Checked = true;
            this.jointAddRadioButton.Location = new System.Drawing.Point(8, 26);
            this.jointAddRadioButton.Name = "jointAddRadioButton";
            this.jointAddRadioButton.Size = new System.Drawing.Size(58, 24);
            this.jointAddRadioButton.TabIndex = 0;
            this.jointAddRadioButton.TabStop = true;
            this.jointAddRadioButton.Text = "Add";
            this.jointAddRadioButton.UseVisualStyleBackColor = true;
            // 
            // boneAddRadioButton
            // 
            this.boneAddRadioButton.AutoSize = true;
            this.boneAddRadioButton.Location = new System.Drawing.Point(8, 73);
            this.boneAddRadioButton.Name = "boneAddRadioButton";
            this.boneAddRadioButton.Size = new System.Drawing.Size(58, 24);
            this.boneAddRadioButton.TabIndex = 1;
            this.boneAddRadioButton.Text = "Add";
            this.boneAddRadioButton.UseVisualStyleBackColor = true;
            // 
            // splitContainer7
            // 
            this.splitContainer7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer7.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer7.Location = new System.Drawing.Point(0, 0);
            this.splitContainer7.Name = "splitContainer7";
            this.splitContainer7.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer7.Panel1
            // 
            this.splitContainer7.Panel1.Controls.Add(this.refSkeletonCanvas);
            // 
            // splitContainer7.Panel2
            // 
            this.splitContainer7.Panel2.Controls.Add(this.editRefJointsButton);
            this.splitContainer7.Size = new System.Drawing.Size(210, 411);
            this.splitContainer7.SplitterDistance = 370;
            this.splitContainer7.TabIndex = 6;
            // 
            // refSkeletonCanvas
            // 
            this.refSkeletonCanvas.BackgroundImage = global::Magic2D.Properties.Resources.refSkeleton;
            this.refSkeletonCanvas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.refSkeletonCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.refSkeletonCanvas.Image = global::Magic2D.Properties.Resources.refSkeleton;
            this.refSkeletonCanvas.InitialImage = null;
            this.refSkeletonCanvas.Location = new System.Drawing.Point(0, 0);
            this.refSkeletonCanvas.Name = "refSkeletonCanvas";
            this.refSkeletonCanvas.Size = new System.Drawing.Size(210, 370);
            this.refSkeletonCanvas.TabIndex = 2;
            this.refSkeletonCanvas.TabStop = false;
            this.refSkeletonCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.refSkeletonCanvas_Paint);
            this.refSkeletonCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.refSkeletonCanvas_MouseDown);
            this.refSkeletonCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.refSkeletonCanvas_MouseMove);
            // 
            // editRefJointsButton
            // 
            this.editRefJointsButton.Location = new System.Drawing.Point(5, 4);
            this.editRefJointsButton.Name = "editRefJointsButton";
            this.editRefJointsButton.Size = new System.Drawing.Size(183, 28);
            this.editRefJointsButton.TabIndex = 5;
            this.editRefJointsButton.Text = "Edit reference joints";
            this.editRefJointsButton.UseVisualStyleBackColor = true;
            this.editRefJointsButton.Click += new System.EventHandler(this.editRefJointsButton_Click);
            // 
            // tabSegmentation
            // 
            this.tabSegmentation.Controls.Add(this.segmentSplitContainer);
            this.tabSegmentation.Location = new System.Drawing.Point(4, 29);
            this.tabSegmentation.Name = "tabSegmentation";
            this.tabSegmentation.Padding = new System.Windows.Forms.Padding(3);
            this.tabSegmentation.Size = new System.Drawing.Size(809, 555);
            this.tabSegmentation.TabIndex = 2;
            this.tabSegmentation.Text = "Segmentation";
            this.tabSegmentation.UseVisualStyleBackColor = true;
            // 
            // segmentSplitContainer
            // 
            this.segmentSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.segmentSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.segmentSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.segmentSplitContainer.Name = "segmentSplitContainer";
            // 
            // segmentSplitContainer.Panel2
            // 
            this.segmentSplitContainer.Panel2.Controls.Add(this.splitContainer2);
            this.segmentSplitContainer.Size = new System.Drawing.Size(803, 549);
            this.segmentSplitContainer.SplitterDistance = 188;
            this.segmentSplitContainer.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.segmentCanvas);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer2.Size = new System.Drawing.Size(611, 549);
            this.splitContainer2.SplitterDistance = 370;
            this.splitContainer2.TabIndex = 1;
            // 
            // segmentCanvas
            // 
            this.segmentCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.segmentCanvas.Location = new System.Drawing.Point(0, 0);
            this.segmentCanvas.Name = "segmentCanvas";
            this.segmentCanvas.Size = new System.Drawing.Size(370, 549);
            this.segmentCanvas.TabIndex = 0;
            this.segmentCanvas.TabStop = false;
            this.segmentCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.segmentCanvas_Paint);
            this.segmentCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.segmentCanvas_MouseDown);
            this.segmentCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.segmentCanvas_MouseMove);
            this.segmentCanvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.segmentCanvas_MouseUp);
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.segmentListView);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.segmentSkeletonButton);
            this.splitContainer3.Panel2.Controls.Add(this.segmentSectionButton);
            this.splitContainer3.Panel2.Controls.Add(this.segmentPartingButton);
            this.splitContainer3.Panel2.Controls.Add(this.segmentSegmentButton);
            this.splitContainer3.Panel2.Controls.Add(this.segmentNameTextBox);
            this.splitContainer3.Panel2.Controls.Add(this.button2);
            this.splitContainer3.Panel2.Controls.Add(this.button1);
            this.splitContainer3.Panel2.Controls.Add(this.panel1);
            this.splitContainer3.Size = new System.Drawing.Size(237, 549);
            this.splitContainer3.SplitterDistance = 212;
            this.splitContainer3.TabIndex = 0;
            // 
            // segmentListView
            // 
            this.segmentListView.CheckBoxes = true;
            this.segmentListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.segmentListView.Location = new System.Drawing.Point(0, 0);
            this.segmentListView.Name = "segmentListView";
            treeNode1.Checked = true;
            treeNode1.Name = "ノード0";
            treeNode1.Text = "Head";
            treeNode2.Checked = true;
            treeNode2.Name = "ノード1";
            treeNode2.Text = "Neck";
            treeNode3.Checked = true;
            treeNode3.Name = "ノード2";
            treeNode3.Text = "・・・";
            treeNode4.Checked = true;
            treeNode4.Name = "ノード1";
            treeNode4.Text = "Joints";
            treeNode5.Checked = true;
            treeNode5.Name = "ノード7";
            treeNode5.Text = "Parting lines";
            treeNode6.Checked = true;
            treeNode6.Name = "ノード0";
            treeNode6.Text = "Full";
            treeNode7.Checked = true;
            treeNode7.Name = "ノード9";
            treeNode7.Text = "LShoulder";
            treeNode8.Checked = true;
            treeNode8.Name = "ノード10";
            treeNode8.Text = "LElbow";
            treeNode9.Checked = true;
            treeNode9.Name = "ノード11";
            treeNode9.Text = "LWrist";
            treeNode10.Checked = true;
            treeNode10.Name = "ノード8";
            treeNode10.Text = "Joints";
            treeNode11.Checked = true;
            treeNode11.Name = "ノード6";
            treeNode11.Text = "Parting lines";
            treeNode12.Checked = true;
            treeNode12.Name = "ノード4";
            treeNode12.Text = "LArm";
            treeNode13.Name = "ノード13";
            treeNode13.Text = "ノード13";
            treeNode14.Checked = true;
            treeNode14.Name = "ノード12";
            treeNode14.Text = "・・・";
            this.segmentListView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode12,
            treeNode14});
            this.segmentListView.Size = new System.Drawing.Size(237, 212);
            this.segmentListView.TabIndex = 0;
            this.segmentListView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.segmentListView_AfterCheck);
            this.segmentListView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.segmentListView_AfterSelect);
            this.segmentListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.segmentListView_KeyDown);
            // 
            // segmentSkeletonButton
            // 
            this.segmentSkeletonButton.AutoSize = true;
            this.segmentSkeletonButton.Location = new System.Drawing.Point(5, 117);
            this.segmentSkeletonButton.Name = "segmentSkeletonButton";
            this.segmentSkeletonButton.Size = new System.Drawing.Size(94, 24);
            this.segmentSkeletonButton.TabIndex = 7;
            this.segmentSkeletonButton.Text = "Skeleton";
            this.segmentSkeletonButton.UseVisualStyleBackColor = true;
            // 
            // segmentSectionButton
            // 
            this.segmentSectionButton.AutoSize = true;
            this.segmentSectionButton.Location = new System.Drawing.Point(24, 76);
            this.segmentSectionButton.Name = "segmentSectionButton";
            this.segmentSectionButton.Size = new System.Drawing.Size(84, 24);
            this.segmentSectionButton.TabIndex = 8;
            this.segmentSectionButton.Text = "Section";
            this.segmentSectionButton.UseVisualStyleBackColor = true;
            // 
            // segmentPartingButton
            // 
            this.segmentPartingButton.AutoSize = true;
            this.segmentPartingButton.Location = new System.Drawing.Point(5, 142);
            this.segmentPartingButton.Name = "segmentPartingButton";
            this.segmentPartingButton.Size = new System.Drawing.Size(115, 24);
            this.segmentPartingButton.TabIndex = 5;
            this.segmentPartingButton.Text = "Parting line";
            this.segmentPartingButton.UseVisualStyleBackColor = true;
            // 
            // segmentSegmentButton
            // 
            this.segmentSegmentButton.AutoSize = true;
            this.segmentSegmentButton.Checked = true;
            this.segmentSegmentButton.Location = new System.Drawing.Point(5, 47);
            this.segmentSegmentButton.Name = "segmentSegmentButton";
            this.segmentSegmentButton.Size = new System.Drawing.Size(96, 24);
            this.segmentSegmentButton.TabIndex = 6;
            this.segmentSegmentButton.TabStop = true;
            this.segmentSegmentButton.Text = "Segment";
            this.segmentSegmentButton.UseVisualStyleBackColor = true;
            // 
            // segmentNameTextBox
            // 
            this.segmentNameTextBox.Location = new System.Drawing.Point(4, 9);
            this.segmentNameTextBox.Name = "segmentNameTextBox";
            this.segmentNameTextBox.Size = new System.Drawing.Size(226, 28);
            this.segmentNameTextBox.TabIndex = 0;
            this.segmentNameTextBox.Text = "part0";
            this.segmentNameTextBox.TextChanged += new System.EventHandler(this.segmentNameTextBox_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(122, 73);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 35);
            this.button2.TabIndex = 1;
            this.button2.Text = "Lazy Brush";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(21, 73);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(98, 35);
            this.button1.TabIndex = 0;
            this.button1.Text = "Path Tool";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.trackBar1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Location = new System.Drawing.Point(21, 114);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(213, 146);
            this.panel1.TabIndex = 2;
            this.panel1.Visible = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(111, 116);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(98, 27);
            this.button3.TabIndex = 3;
            this.button3.Text = "Calculate";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1.Location = new System.Drawing.Point(64, 54);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(145, 24);
            this.textBox1.TabIndex = 4;
            this.textBox1.Text = "5.0";
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.Color.White;
            this.trackBar1.Location = new System.Drawing.Point(4, 81);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(205, 45);
            this.trackBar1.TabIndex = 3;
            this.trackBar1.Value = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Width";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(3, 25);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(50, 24);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "BG";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(3, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(48, 24);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "FG";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // tabComposition
            // 
            this.tabComposition.Controls.Add(this.splitContainer5);
            this.tabComposition.Location = new System.Drawing.Point(4, 29);
            this.tabComposition.Name = "tabComposition";
            this.tabComposition.Padding = new System.Windows.Forms.Padding(3);
            this.tabComposition.Size = new System.Drawing.Size(809, 555);
            this.tabComposition.TabIndex = 3;
            this.tabComposition.Text = "Composition";
            this.tabComposition.UseVisualStyleBackColor = true;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer5.Location = new System.Drawing.Point(3, 3);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.splitContainer10);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.radioButton3);
            this.splitContainer5.Panel2.Controls.Add(this.radioButton5);
            this.splitContainer5.Panel2.Controls.Add(this.radioButton4);
            this.splitContainer5.Panel2.Controls.Add(this.reverseCheckBox);
            this.splitContainer5.Panel2.Controls.Add(this.label4);
            this.splitContainer5.Panel2.Controls.Add(this.scaleYTrackbar);
            this.splitContainer5.Panel2.Controls.Add(this.label7);
            this.splitContainer5.Panel2.Controls.Add(this.label8);
            this.splitContainer5.Panel2.Controls.Add(this.scaleXTrackbar);
            this.splitContainer5.Panel2.Controls.Add(this.panel2);
            this.splitContainer5.Panel2.Controls.Add(this.referenceImageView);
            this.splitContainer5.Panel2.Controls.Add(this.label12);
            this.splitContainer5.Panel2.Controls.Add(this.LoadReferenceImageButton);
            this.splitContainer5.Panel2.Controls.Add(this.label6);
            this.splitContainer5.Panel2.Controls.Add(this.posYTrackbar);
            this.splitContainer5.Panel2.Controls.Add(this.label3);
            this.splitContainer5.Panel2.Controls.Add(this.label5);
            this.splitContainer5.Panel2.Controls.Add(this.rotTrackbar);
            this.splitContainer5.Panel2.Controls.Add(this.label2);
            this.splitContainer5.Panel2.Controls.Add(this.posXTrackbar);
            this.splitContainer5.Size = new System.Drawing.Size(803, 549);
            this.splitContainer5.SplitterDistance = 514;
            this.splitContainer5.TabIndex = 0;
            // 
            // splitContainer10
            // 
            this.splitContainer10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer10.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer10.Location = new System.Drawing.Point(0, 0);
            this.splitContainer10.Name = "splitContainer10";
            // 
            // splitContainer10.Panel1
            // 
            this.splitContainer10.Panel1.Controls.Add(this.splitContainer8);
            // 
            // splitContainer10.Panel2
            // 
            this.splitContainer10.Panel2.Controls.Add(this.compositionCanvas);
            this.splitContainer10.Size = new System.Drawing.Size(514, 549);
            this.splitContainer10.SplitterDistance = 171;
            this.splitContainer10.TabIndex = 0;
            // 
            // splitContainer8
            // 
            this.splitContainer8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer8.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer8.Location = new System.Drawing.Point(0, 0);
            this.splitContainer8.Name = "splitContainer8";
            this.splitContainer8.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer8.Panel1
            // 
            this.splitContainer8.Panel1.Controls.Add(this.segmentImageView);
            // 
            // splitContainer8.Panel2
            // 
            this.splitContainer8.Panel2.Controls.Add(this.loadSegmentImageButton);
            this.splitContainer8.Size = new System.Drawing.Size(171, 549);
            this.splitContainer8.SplitterDistance = 511;
            this.splitContainer8.TabIndex = 0;
            // 
            // segmentImageView
            // 
            this.segmentImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.segmentImageView.LargeImageList = this.segmentImageList;
            this.segmentImageView.Location = new System.Drawing.Point(0, 0);
            this.segmentImageView.Name = "segmentImageView";
            this.segmentImageView.Size = new System.Drawing.Size(171, 511);
            this.segmentImageView.SmallImageList = this.segmentImageList;
            this.segmentImageView.TabIndex = 0;
            this.segmentImageView.UseCompatibleStateImageBehavior = false;
            this.segmentImageView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.segmentImageView_MouseDoubleClick);
            // 
            // segmentImageList
            // 
            this.segmentImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.segmentImageList.ImageSize = new System.Drawing.Size(64, 64);
            this.segmentImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // loadSegmentImageButton
            // 
            this.loadSegmentImageButton.Location = new System.Drawing.Point(5, 4);
            this.loadSegmentImageButton.Name = "loadSegmentImageButton";
            this.loadSegmentImageButton.Size = new System.Drawing.Size(75, 27);
            this.loadSegmentImageButton.TabIndex = 0;
            this.loadSegmentImageButton.Text = "Load";
            this.loadSegmentImageButton.UseVisualStyleBackColor = true;
            this.loadSegmentImageButton.Click += new System.EventHandler(this.loadSegmentImageButton_Click);
            // 
            // compositionCanvas
            // 
            this.compositionCanvas.composition = null;
            this.compositionCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compositionCanvas.Location = new System.Drawing.Point(0, 0);
            this.compositionCanvas.Name = "compositionCanvas";
            this.compositionCanvas.Resolution = new System.Drawing.Size(1600, 1200);
            this.compositionCanvas.Size = new System.Drawing.Size(339, 549);
            this.compositionCanvas.TabIndex = 1;
            this.compositionCanvas.Text = "compositionCanvasControl1";
            this.compositionCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.compositionCanvas_MouseDown);
            this.compositionCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.compositionCanvas_MouseMove);
            this.compositionCanvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.compositionCanvas_MouseUp);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(7, 460);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(112, 24);
            this.radioButton3.TabIndex = 1;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Disconnect";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(7, 509);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(166, 24);
            this.radioButton5.TabIndex = 3;
            this.radioButton5.Text = "Connect manually";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(7, 484);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(195, 24);
            this.radioButton4.TabIndex = 2;
            this.radioButton4.Text = "Connect automatcally";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // reverseCheckBox
            // 
            this.reverseCheckBox.AutoSize = true;
            this.reverseCheckBox.Location = new System.Drawing.Point(7, 395);
            this.reverseCheckBox.Name = "reverseCheckBox";
            this.reverseCheckBox.Size = new System.Drawing.Size(90, 24);
            this.reverseCheckBox.TabIndex = 31;
            this.reverseCheckBox.Text = "Reverse";
            this.reverseCheckBox.UseVisualStyleBackColor = true;
            this.reverseCheckBox.CheckedChanged += new System.EventHandler(this.reverseCheckBox_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(2, 310);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 20);
            this.label4.TabIndex = 30;
            this.label4.Text = "Scale";
            // 
            // scaleYTrackbar
            // 
            this.scaleYTrackbar.BackColor = System.Drawing.Color.White;
            this.scaleYTrackbar.Location = new System.Drawing.Point(19, 359);
            this.scaleYTrackbar.Maximum = 100;
            this.scaleYTrackbar.Minimum = 1;
            this.scaleYTrackbar.Name = "scaleYTrackbar";
            this.scaleYTrackbar.Size = new System.Drawing.Size(264, 45);
            this.scaleYTrackbar.TabIndex = 27;
            this.scaleYTrackbar.Value = 10;
            this.scaleYTrackbar.Scroll += new System.EventHandler(this.scaleTrackbar_Scroll);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 359);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 20);
            this.label7.TabIndex = 29;
            this.label7.Text = "Y";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 333);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 20);
            this.label8.TabIndex = 28;
            this.label8.Text = "X";
            // 
            // scaleXTrackbar
            // 
            this.scaleXTrackbar.BackColor = System.Drawing.Color.White;
            this.scaleXTrackbar.Location = new System.Drawing.Point(19, 333);
            this.scaleXTrackbar.Maximum = 100;
            this.scaleXTrackbar.Minimum = 1;
            this.scaleXTrackbar.Name = "scaleXTrackbar";
            this.scaleXTrackbar.Size = new System.Drawing.Size(264, 45);
            this.scaleXTrackbar.TabIndex = 26;
            this.scaleXTrackbar.Value = 10;
            this.scaleXTrackbar.Scroll += new System.EventHandler(this.scaleTrackbar_Scroll);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.controlPointMoveButton);
            this.panel2.Controls.Add(this.segmentMoveButton);
            this.panel2.Controls.Add(this.skeletonMoveButton);
            this.panel2.Location = new System.Drawing.Point(3, 86);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(279, 66);
            this.panel2.TabIndex = 25;
            // 
            // controlPointMoveButton
            // 
            this.controlPointMoveButton.AutoSize = true;
            this.controlPointMoveButton.Location = new System.Drawing.Point(139, 33);
            this.controlPointMoveButton.Name = "controlPointMoveButton";
            this.controlPointMoveButton.Size = new System.Drawing.Size(130, 24);
            this.controlPointMoveButton.TabIndex = 2;
            this.controlPointMoveButton.Text = "Control point";
            this.controlPointMoveButton.UseVisualStyleBackColor = true;
            // 
            // segmentMoveButton
            // 
            this.segmentMoveButton.AutoSize = true;
            this.segmentMoveButton.Location = new System.Drawing.Point(7, 33);
            this.segmentMoveButton.Name = "segmentMoveButton";
            this.segmentMoveButton.Size = new System.Drawing.Size(96, 24);
            this.segmentMoveButton.TabIndex = 1;
            this.segmentMoveButton.Text = "Segment";
            this.segmentMoveButton.UseVisualStyleBackColor = true;
            // 
            // skeletonMoveButton
            // 
            this.skeletonMoveButton.AutoSize = true;
            this.skeletonMoveButton.Checked = true;
            this.skeletonMoveButton.Location = new System.Drawing.Point(7, 3);
            this.skeletonMoveButton.Name = "skeletonMoveButton";
            this.skeletonMoveButton.Size = new System.Drawing.Size(94, 24);
            this.skeletonMoveButton.TabIndex = 0;
            this.skeletonMoveButton.TabStop = true;
            this.skeletonMoveButton.Text = "Skeleton";
            this.skeletonMoveButton.UseVisualStyleBackColor = true;
            // 
            // referenceImageView
            // 
            this.referenceImageView.Location = new System.Drawing.Point(87, 7);
            this.referenceImageView.Name = "referenceImageView";
            this.referenceImageView.Size = new System.Drawing.Size(120, 75);
            this.referenceImageView.TabIndex = 22;
            this.referenceImageView.TabStop = false;
            this.referenceImageView.Paint += new System.Windows.Forms.PaintEventHandler(this.referenceImageView_Paint);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(2, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(86, 20);
            this.label12.TabIndex = 24;
            this.label12.Text = "Reference";
            // 
            // LoadReferenceImageButton
            // 
            this.LoadReferenceImageButton.Location = new System.Drawing.Point(207, 53);
            this.LoadReferenceImageButton.Name = "LoadReferenceImageButton";
            this.LoadReferenceImageButton.Size = new System.Drawing.Size(75, 30);
            this.LoadReferenceImageButton.TabIndex = 23;
            this.LoadReferenceImageButton.Text = "Load";
            this.LoadReferenceImageButton.UseVisualStyleBackColor = true;
            this.LoadReferenceImageButton.Click += new System.EventHandler(this.LoadReferenceImageButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(-2, 163);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Position";
            // 
            // posYTrackbar
            // 
            this.posYTrackbar.BackColor = System.Drawing.Color.White;
            this.posYTrackbar.Location = new System.Drawing.Point(15, 214);
            this.posYTrackbar.Maximum = 600;
            this.posYTrackbar.Minimum = -600;
            this.posYTrackbar.Name = "posYTrackbar";
            this.posYTrackbar.Size = new System.Drawing.Size(264, 45);
            this.posYTrackbar.TabIndex = 5;
            this.posYTrackbar.Scroll += new System.EventHandler(this.posTrackbar_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 212);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 20);
            this.label3.TabIndex = 11;
            this.label3.Text = "Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 20);
            this.label5.TabIndex = 21;
            this.label5.Text = "Rotation";
            // 
            // rotTrackbar
            // 
            this.rotTrackbar.BackColor = System.Drawing.Color.White;
            this.rotTrackbar.Location = new System.Drawing.Point(4, 274);
            this.rotTrackbar.Maximum = 360;
            this.rotTrackbar.Name = "rotTrackbar";
            this.rotTrackbar.Size = new System.Drawing.Size(277, 45);
            this.rotTrackbar.TabIndex = 15;
            this.rotTrackbar.Scroll += new System.EventHandler(this.rotTrackbar_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 186);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "X";
            // 
            // posXTrackbar
            // 
            this.posXTrackbar.BackColor = System.Drawing.Color.White;
            this.posXTrackbar.Location = new System.Drawing.Point(15, 186);
            this.posXTrackbar.Maximum = 800;
            this.posXTrackbar.Minimum = -800;
            this.posXTrackbar.Name = "posXTrackbar";
            this.posXTrackbar.Size = new System.Drawing.Size(264, 45);
            this.posXTrackbar.TabIndex = 4;
            this.posXTrackbar.Scroll += new System.EventHandler(this.posTrackbar_Scroll);
            // 
            // tabAnimation
            // 
            this.tabAnimation.Controls.Add(this.splitContainer9);
            this.tabAnimation.Location = new System.Drawing.Point(4, 29);
            this.tabAnimation.Name = "tabAnimation";
            this.tabAnimation.Padding = new System.Windows.Forms.Padding(3);
            this.tabAnimation.Size = new System.Drawing.Size(809, 555);
            this.tabAnimation.TabIndex = 4;
            this.tabAnimation.Text = "Animation";
            this.tabAnimation.UseVisualStyleBackColor = true;
            // 
            // splitContainer9
            // 
            this.splitContainer9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer9.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer9.Location = new System.Drawing.Point(3, 3);
            this.splitContainer9.Name = "splitContainer9";
            this.splitContainer9.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer9.Panel1
            // 
            this.splitContainer9.Panel1.Controls.Add(this.composedImageView);
            // 
            // splitContainer9.Panel2
            // 
            this.splitContainer9.Panel2.Controls.Add(this.splitContainer11);
            this.splitContainer9.Size = new System.Drawing.Size(803, 549);
            this.splitContainer9.SplitterDistance = 139;
            this.splitContainer9.TabIndex = 0;
            // 
            // composedImageView
            // 
            this.composedImageView.AllowDrop = true;
            this.composedImageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.composedImageView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem4,
            listViewItem5,
            listViewItem6});
            this.composedImageView.LargeImageList = this.composedImageList;
            this.composedImageView.Location = new System.Drawing.Point(0, 0);
            this.composedImageView.Name = "composedImageView";
            this.composedImageView.Size = new System.Drawing.Size(803, 139);
            this.composedImageView.SmallImageList = this.composedImageList;
            this.composedImageView.TabIndex = 0;
            this.composedImageView.UseCompatibleStateImageBehavior = false;
            this.composedImageView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.composedImageView_ItemDrag);
            this.composedImageView.DragOver += new System.Windows.Forms.DragEventHandler(this.composedImageView_DragOver);
            // 
            // composedImageList
            // 
            this.composedImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("composedImageList.ImageStream")));
            this.composedImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.composedImageList.Images.SetKeyName(0, "comp0.png");
            this.composedImageList.Images.SetKeyName(1, "comp1.png");
            this.composedImageList.Images.SetKeyName(2, "comp2.png");
            // 
            // splitContainer11
            // 
            this.splitContainer11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer11.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer11.Location = new System.Drawing.Point(0, 0);
            this.splitContainer11.Name = "splitContainer11";
            // 
            // splitContainer11.Panel1
            // 
            this.splitContainer11.Panel1.Controls.Add(this.splitContainer14);
            // 
            // splitContainer11.Panel2
            // 
            this.splitContainer11.Panel2.Controls.Add(this.splitContainer12);
            this.splitContainer11.Size = new System.Drawing.Size(803, 406);
            this.splitContainer11.SplitterDistance = 333;
            this.splitContainer11.TabIndex = 0;
            // 
            // splitContainer14
            // 
            this.splitContainer14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer14.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer14.Location = new System.Drawing.Point(0, 0);
            this.splitContainer14.Name = "splitContainer14";
            this.splitContainer14.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer14.Panel1
            // 
            this.splitContainer14.Panel1.Controls.Add(this.cellListView);
            // 
            // splitContainer14.Panel2
            // 
            this.splitContainer14.Panel2.Controls.Add(this.animeDurationTextBox);
            this.splitContainer14.Size = new System.Drawing.Size(333, 406);
            this.splitContainer14.SplitterDistance = 360;
            this.splitContainer14.TabIndex = 0;
            // 
            // cellListView
            // 
            this.cellListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cellListView.Location = new System.Drawing.Point(0, 0);
            this.cellListView.Name = "cellListView";
            this.cellListView.Size = new System.Drawing.Size(333, 360);
            this.cellListView.TabIndex = 0;
            this.cellListView.TabStop = false;
            this.cellListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.cellListView_DragDrop);
            this.cellListView.DragEnter += new System.Windows.Forms.DragEventHandler(this.cellListView_DragEnter);
            this.cellListView.DragOver += new System.Windows.Forms.DragEventHandler(this.cellListView_DragOver);
            this.cellListView.Paint += new System.Windows.Forms.PaintEventHandler(this.cellListView_Paint);
            this.cellListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.cellListView_MouseDown);
            // 
            // animeDurationTextBox
            // 
            this.animeDurationTextBox.Location = new System.Drawing.Point(5, 7);
            this.animeDurationTextBox.Name = "animeDurationTextBox";
            this.animeDurationTextBox.Size = new System.Drawing.Size(325, 28);
            this.animeDurationTextBox.TabIndex = 0;
            this.animeDurationTextBox.Text = "33";
            this.animeDurationTextBox.TextChanged += new System.EventHandler(this.animeDurationTextBox_TextChanged);
            // 
            // splitContainer12
            // 
            this.splitContainer12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer12.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer12.Location = new System.Drawing.Point(0, 0);
            this.splitContainer12.Name = "splitContainer12";
            this.splitContainer12.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer12.Panel1
            // 
            this.splitContainer12.Panel1.Controls.Add(this.animeView);
            // 
            // splitContainer12.Panel2
            // 
            this.splitContainer12.Panel2.Controls.Add(this.splitContainer13);
            this.splitContainer12.Size = new System.Drawing.Size(466, 406);
            this.splitContainer12.SplitterDistance = 367;
            this.splitContainer12.TabIndex = 0;
            // 
            // animeView
            // 
            this.animeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.animeView.Location = new System.Drawing.Point(0, 0);
            this.animeView.Name = "animeView";
            this.animeView.Size = new System.Drawing.Size(466, 367);
            this.animeView.TabIndex = 0;
            this.animeView.TabStop = false;
            this.animeView.Paint += new System.Windows.Forms.PaintEventHandler(this.animeView_Paint);
            // 
            // splitContainer13
            // 
            this.splitContainer13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer13.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer13.Location = new System.Drawing.Point(0, 0);
            this.splitContainer13.Name = "splitContainer13";
            // 
            // splitContainer13.Panel1
            // 
            this.splitContainer13.Panel1.Controls.Add(this.playButton);
            // 
            // splitContainer13.Panel2
            // 
            this.splitContainer13.Panel2.Controls.Add(this.splitContainer1);
            this.splitContainer13.Size = new System.Drawing.Size(466, 35);
            this.splitContainer13.SplitterDistance = 42;
            this.splitContainer13.TabIndex = 0;
            // 
            // playButton
            // 
            this.playButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.playButton.Location = new System.Drawing.Point(0, 0);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(42, 35);
            this.playButton.TabIndex = 0;
            this.playButton.Text = "▷";
            this.playButton.UseVisualStyleBackColor = true;
            this.playButton.Click += new System.EventHandler(this.playButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.seekbar);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.loopCheckBox);
            this.splitContainer1.Size = new System.Drawing.Size(420, 35);
            this.splitContainer1.SplitterDistance = 349;
            this.splitContainer1.TabIndex = 0;
            // 
            // seekbar
            // 
            this.seekbar.BackColor = System.Drawing.Color.White;
            this.seekbar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seekbar.Location = new System.Drawing.Point(0, 0);
            this.seekbar.Name = "seekbar";
            this.seekbar.Size = new System.Drawing.Size(349, 35);
            this.seekbar.TabIndex = 0;
            this.seekbar.Scroll += new System.EventHandler(this.seekbar_Scroll);
            // 
            // loopCheckBox
            // 
            this.loopCheckBox.AutoSize = true;
            this.loopCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loopCheckBox.Location = new System.Drawing.Point(0, 0);
            this.loopCheckBox.Name = "loopCheckBox";
            this.loopCheckBox.Size = new System.Drawing.Size(67, 35);
            this.loopCheckBox.TabIndex = 0;
            this.loopCheckBox.Text = "Loop";
            this.loopCheckBox.UseVisualStyleBackColor = true;
            // 
            // animeTimer
            // 
            this.animeTimer.Interval = 16;
            // 
            // segmentContextMenuStrip
            // 
            this.segmentContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.segmentSToolStripMenuItem,
            this.clearPathCToolStripMenuItem,
            this.deleteLastPointDToolStripMenuItem,
            this.toolStripSeparator3,
            this.newSegmentNToolStripMenuItem});
            this.segmentContextMenuStrip.Name = "segmentContextMenuStrip";
            this.segmentContextMenuStrip.Size = new System.Drawing.Size(192, 120);
            // 
            // segmentSToolStripMenuItem
            // 
            this.segmentSToolStripMenuItem.Name = "segmentSToolStripMenuItem";
            this.segmentSToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.segmentSToolStripMenuItem.Text = "Segment(&S)";
            this.segmentSToolStripMenuItem.Click += new System.EventHandler(this.segmentSToolStripMenuItem_Click);
            // 
            // clearPathCToolStripMenuItem
            // 
            this.clearPathCToolStripMenuItem.Name = "clearPathCToolStripMenuItem";
            this.clearPathCToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.clearPathCToolStripMenuItem.Text = "Clear path(&C)";
            this.clearPathCToolStripMenuItem.Click += new System.EventHandler(this.clearPathCToolStripMenuItem_Click);
            // 
            // deleteLastPointDToolStripMenuItem
            // 
            this.deleteLastPointDToolStripMenuItem.Name = "deleteLastPointDToolStripMenuItem";
            this.deleteLastPointDToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.deleteLastPointDToolStripMenuItem.Text = "Delete last point(&D)";
            this.deleteLastPointDToolStripMenuItem.Click += new System.EventHandler(this.deleteLastPointDToolStripMenuItem_Click);
            // 
            // compositionContextMenuStrip
            // 
            this.compositionContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.brindForwardToolStripMenuItem,
            this.sendBackwardToolStripMenuItem,
            this.toolStripSeparator2,
            this.bringToFrontToolStripMenuItem,
            this.sendToBackToolStripMenuItem});
            this.compositionContextMenuStrip.Name = "compositionContextMenuStrip";
            this.compositionContextMenuStrip.Size = new System.Drawing.Size(166, 98);
            // 
            // brindForwardToolStripMenuItem
            // 
            this.brindForwardToolStripMenuItem.Name = "brindForwardToolStripMenuItem";
            this.brindForwardToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.brindForwardToolStripMenuItem.Text = "Brind forward";
            this.brindForwardToolStripMenuItem.Click += new System.EventHandler(this.brindForwardToolStripMenuItem_Click);
            // 
            // sendBackwardToolStripMenuItem
            // 
            this.sendBackwardToolStripMenuItem.Name = "sendBackwardToolStripMenuItem";
            this.sendBackwardToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.sendBackwardToolStripMenuItem.Text = "Send backward";
            this.sendBackwardToolStripMenuItem.Click += new System.EventHandler(this.sendBackwardToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(162, 6);
            // 
            // bringToFrontToolStripMenuItem
            // 
            this.bringToFrontToolStripMenuItem.Name = "bringToFrontToolStripMenuItem";
            this.bringToFrontToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.bringToFrontToolStripMenuItem.Text = "Bring to front";
            this.bringToFrontToolStripMenuItem.Click += new System.EventHandler(this.bringToFrontToolStripMenuItem_Click);
            // 
            // sendToBackToolStripMenuItem
            // 
            this.sendToBackToolStripMenuItem.Name = "sendToBackToolStripMenuItem";
            this.sendToBackToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.sendToBackToolStripMenuItem.Text = "Send to back";
            this.sendToBackToolStripMenuItem.Click += new System.EventHandler(this.sendToBackToolStripMenuItem_Click);
            // 
            // newSegmentNToolStripMenuItem
            // 
            this.newSegmentNToolStripMenuItem.Name = "newSegmentNToolStripMenuItem";
            this.newSegmentNToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.newSegmentNToolStripMenuItem.Text = "New segment(&N)";
            this.newSegmentNToolStripMenuItem.Click += new System.EventHandler(this.newSegmentNToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(188, 6);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 614);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabSourceImages.ResumeLayout(false);
            this.sourceImagesSplitContainer.Panel1.ResumeLayout(false);
            this.sourceImagesSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sourceImagesSplitContainer)).EndInit();
            this.sourceImagesSplitContainer.ResumeLayout(false);
            this.menuSourceImages.ResumeLayout(false);
            this.tabSkeletonFitting.ResumeLayout(false);
            this.skeletonSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.skeletonSplitContainer)).EndInit();
            this.skeletonSplitContainer.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.skeletonFittingCanvas)).EndInit();
            this.splitContainer6.Panel1.ResumeLayout(false);
            this.splitContainer6.Panel1.PerformLayout();
            this.splitContainer6.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer6)).EndInit();
            this.splitContainer6.ResumeLayout(false);
            this.splitContainer7.Panel1.ResumeLayout(false);
            this.splitContainer7.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer7)).EndInit();
            this.splitContainer7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.refSkeletonCanvas)).EndInit();
            this.tabSegmentation.ResumeLayout(false);
            this.segmentSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.segmentSplitContainer)).EndInit();
            this.segmentSplitContainer.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.segmentCanvas)).EndInit();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.tabComposition.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer10.Panel1.ResumeLayout(false);
            this.splitContainer10.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer10)).EndInit();
            this.splitContainer10.ResumeLayout(false);
            this.splitContainer8.Panel1.ResumeLayout(false);
            this.splitContainer8.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer8)).EndInit();
            this.splitContainer8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scaleYTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scaleXTrackbar)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.referenceImageView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posYTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rotTrackbar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.posXTrackbar)).EndInit();
            this.tabAnimation.ResumeLayout(false);
            this.splitContainer9.Panel1.ResumeLayout(false);
            this.splitContainer9.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer9)).EndInit();
            this.splitContainer9.ResumeLayout(false);
            this.splitContainer11.Panel1.ResumeLayout(false);
            this.splitContainer11.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer11)).EndInit();
            this.splitContainer11.ResumeLayout(false);
            this.splitContainer14.Panel1.ResumeLayout(false);
            this.splitContainer14.Panel2.ResumeLayout(false);
            this.splitContainer14.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer14)).EndInit();
            this.splitContainer14.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cellListView)).EndInit();
            this.splitContainer12.Panel1.ResumeLayout(false);
            this.splitContainer12.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer12)).EndInit();
            this.splitContainer12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.animeView)).EndInit();
            this.splitContainer13.Panel1.ResumeLayout(false);
            this.splitContainer13.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer13)).EndInit();
            this.splitContainer13.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.seekbar)).EndInit();
            this.segmentContextMenuStrip.ResumeLayout(false);
            this.compositionContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileFToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabSourceImages;
        private System.Windows.Forms.TabPage tabSkeletonFitting;
        private System.Windows.Forms.TabPage tabSegmentation;
        private System.Windows.Forms.TabPage tabComposition;
        private System.Windows.Forms.SplitContainer sourceImagesSplitContainer;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonScreenshot;
        private System.Windows.Forms.ListView sourceImageView;
        private System.Windows.Forms.ImageList sourceImageList;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PictureBox segmentCanvas;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TreeView segmentListView;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.PictureBox skeletonFittingCanvas;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox segmentNameTextBox;
        private System.Windows.Forms.RadioButton boneAddRadioButton;
        private System.Windows.Forms.RadioButton jointAddRadioButton;
        private System.Windows.Forms.ImageList segmentImageList;
        private System.Windows.Forms.TextBox jointNameTextBox;
        private System.Windows.Forms.PictureBox refSkeletonCanvas;
        private System.Windows.Forms.Button editRefJointsButton;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.TabPage tabAnimation;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.ListView segmentImageView;
        private System.Windows.Forms.TrackBar posXTrackbar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar rotTrackbar;
        private System.Windows.Forms.SplitContainer splitContainer6;
        private System.Windows.Forms.SplitContainer splitContainer7;
        private System.Windows.Forms.SplitContainer splitContainer9;
        private System.Windows.Forms.SplitContainer splitContainer11;
        private System.Windows.Forms.ListView composedImageView;
        private System.Windows.Forms.SplitContainer splitContainer12;
        private System.Windows.Forms.SplitContainer splitContainer13;
        private System.Windows.Forms.Button playButton;
        private System.Windows.Forms.TrackBar seekbar;
        private System.Windows.Forms.SplitContainer splitContainer14;
        private System.Windows.Forms.TextBox animeDurationTextBox;
        private System.Windows.Forms.PictureBox cellListView;
        private System.Windows.Forms.PictureBox animeView;
        private System.Windows.Forms.ImageList composedImageList;
        private System.Windows.Forms.ContextMenuStrip menuSourceImages;
        private System.Windows.Forms.ToolStripMenuItem skeletonSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem segmentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openOToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveSToolStripMenuItem;
        private System.Windows.Forms.SplitContainer skeletonSplitContainer;
        private System.Windows.Forms.RadioButton segmentPartingButton;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RadioButton boneSelectRadioButton;
        private System.Windows.Forms.RadioButton jointSelectRadioButton;
        private System.Windows.Forms.SplitContainer segmentSplitContainer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exportEToolStripMenuItem;
        private System.Windows.Forms.Timer animeTimer;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox loopCheckBox;
        private System.Windows.Forms.RadioButton segmentSkeletonButton;
        private System.Windows.Forms.RadioButton segmentSegmentButton;
        private System.Windows.Forms.ContextMenuStrip segmentContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem segmentSToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteLastPointDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearPathCToolStripMenuItem;
        private System.Windows.Forms.RadioButton segmentSectionButton;
        private System.Windows.Forms.SplitContainer splitContainer10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button LoadReferenceImageButton;
        private System.Windows.Forms.PictureBox referenceImageView;
        private System.Windows.Forms.SplitContainer splitContainer8;
        private System.Windows.Forms.Button loadSegmentImageButton;
        private CompositionCanvasControl compositionCanvas;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton segmentMoveButton;
        private System.Windows.Forms.RadioButton skeletonMoveButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar scaleYTrackbar;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TrackBar scaleXTrackbar;
        private System.Windows.Forms.TrackBar posYTrackbar;
        private System.Windows.Forms.CheckBox reverseCheckBox;
        private System.Windows.Forms.ContextMenuStrip compositionContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem brindForwardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bringToFrontToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem sendBackwardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToBackToolStripMenuItem;
        private System.Windows.Forms.RadioButton controlPointMoveButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem newSegmentNToolStripMenuItem;
    }
}

