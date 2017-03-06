namespace LevelInspector
{
    partial class MapForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapForm));
            this.canvasBox = new System.Windows.Forms.PictureBox();
            this.zoomInput = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.tableTabs = new System.Windows.Forms.TabControl();
            this.entityTab = new System.Windows.Forms.TabPage();
            this.morphCheckbox = new System.Windows.Forms.CheckBox();
            this.editEntityButton = new System.Windows.Forms.Button();
            this.entityTable = new System.Windows.Forms.ListView();
            this.deselectEntityTableButton = new System.Windows.Forms.Button();
            this.entityTableCheckbox = new System.Windows.Forms.CheckBox();
            this.connect9EntityCheckbox = new System.Windows.Forms.CheckBox();
            this.entityTableInfo = new System.Windows.Forms.Label();
            this.columnTab = new System.Windows.Forms.TabPage();
            this.editColumnButton = new System.Windows.Forms.Button();
            this.deselectColumnsButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.highlightColumnsCheckbox = new System.Windows.Forms.CheckBox();
            this.columnTableInfo = new System.Windows.Forms.Label();
            this.columnTableCheckbox = new System.Windows.Forms.CheckBox();
            this.columnTable = new System.Windows.Forms.ListView();
            this.blockTexTab = new System.Windows.Forms.TabPage();
            this.cubePreviewTopPanel = new System.Windows.Forms.Panel();
            this.cubePreviewTopPicture = new System.Windows.Forms.PictureBox();
            this.cubePreviewBottomPanel = new System.Windows.Forms.Panel();
            this.cubePreviewBottomPicture = new System.Windows.Forms.PictureBox();
            this.editBlockTexButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.deselectBlockTexButton = new System.Windows.Forms.Button();
            this.highlightBlockTexCheckbox = new System.Windows.Forms.CheckBox();
            this.blockTexTableCheckbox = new System.Windows.Forms.CheckBox();
            this.blockTexTableInfo = new System.Windows.Forms.Label();
            this.blockTexTable = new System.Windows.Forms.ListView();
            this.unknownTab = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.unknownTableInfo = new System.Windows.Forms.Label();
            this.unknownTable247264 = new System.Windows.Forms.ListView();
            this.unknown358222Tab = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.unknownTable358222Info = new System.Windows.Forms.Label();
            this.unknownTable358222 = new System.Windows.Forms.ListView();
            this.logTextbox = new System.Windows.Forms.TextBox();
            this.dotsizeInput = new System.Windows.Forms.NumericUpDown();
            this.mouseInfoLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.mapByteInput = new System.Windows.Forms.NumericUpDown();
            this.dotSizeLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.writeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.canvasBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomInput)).BeginInit();
            this.settingsPanel.SuspendLayout();
            this.tableTabs.SuspendLayout();
            this.entityTab.SuspendLayout();
            this.columnTab.SuspendLayout();
            this.blockTexTab.SuspendLayout();
            this.cubePreviewTopPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cubePreviewTopPicture)).BeginInit();
            this.cubePreviewBottomPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cubePreviewBottomPicture)).BeginInit();
            this.unknownTab.SuspendLayout();
            this.unknown358222Tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dotsizeInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapByteInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            //
            // canvasBox
            //
            this.canvasBox.Location = new System.Drawing.Point(0, 0);
            this.canvasBox.Name = "canvasBox";
            this.canvasBox.Size = new System.Drawing.Size(512, 320);
            this.canvasBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.canvasBox.TabIndex = 0;
            this.canvasBox.TabStop = false;
            this.canvasBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvasBox_MouseDown);
            this.canvasBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvasBox_MouseMove);
            this.canvasBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvasBox_MouseUp);
            //
            // zoomInput
            //
            this.zoomInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.zoomInput.Location = new System.Drawing.Point(83, 28);
            this.zoomInput.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.zoomInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.zoomInput.Name = "zoomInput";
            this.zoomInput.Size = new System.Drawing.Size(60, 26);
            this.zoomInput.TabIndex = 2;
            this.zoomInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.zoomInput.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.zoomInput.ValueChanged += new System.EventHandler(this.zoomInput_ValueChanged);
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(80, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Zoom";
            //
            // settingsPanel
            //
            this.settingsPanel.Controls.Add(this.tableTabs);
            this.settingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsPanel.Location = new System.Drawing.Point(0, 0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(622, 621);
            this.settingsPanel.TabIndex = 4;
            //
            // tableTabs
            //
            this.tableTabs.Controls.Add(this.entityTab);
            this.tableTabs.Controls.Add(this.columnTab);
            this.tableTabs.Controls.Add(this.blockTexTab);
            this.tableTabs.Controls.Add(this.unknownTab);
            this.tableTabs.Controls.Add(this.unknown358222Tab);
            this.tableTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableTabs.Location = new System.Drawing.Point(0, 0);
            this.tableTabs.Name = "tableTabs";
            this.tableTabs.SelectedIndex = 0;
            this.tableTabs.Size = new System.Drawing.Size(622, 621);
            this.tableTabs.TabIndex = 15;
            //
            // entityTab
            //
            this.entityTab.Controls.Add(this.morphCheckbox);
            this.entityTab.Controls.Add(this.editEntityButton);
            this.entityTab.Controls.Add(this.entityTable);
            this.entityTab.Controls.Add(this.deselectEntityTableButton);
            this.entityTab.Controls.Add(this.entityTableCheckbox);
            this.entityTab.Controls.Add(this.connect9EntityCheckbox);
            this.entityTab.Controls.Add(this.entityTableInfo);
            this.entityTab.Location = new System.Drawing.Point(4, 22);
            this.entityTab.Name = "entityTab";
            this.entityTab.Padding = new System.Windows.Forms.Padding(3);
            this.entityTab.Size = new System.Drawing.Size(614, 595);
            this.entityTab.TabIndex = 0;
            this.entityTab.Text = "Entities";
            this.entityTab.UseVisualStyleBackColor = true;
            //
            // morphCheckbox
            //
            this.morphCheckbox.AutoSize = true;
            this.morphCheckbox.Checked = true;
            this.morphCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.morphCheckbox.Location = new System.Drawing.Point(203, 6);
            this.morphCheckbox.Name = "morphCheckbox";
            this.morphCheckbox.Size = new System.Drawing.Size(84, 17);
            this.morphCheckbox.TabIndex = 11;
            this.morphCheckbox.Text = "morph areas";
            this.morphCheckbox.UseVisualStyleBackColor = true;
            this.morphCheckbox.CheckedChanged += new System.EventHandler(this.morphCheckbox_CheckedChanged);
            //
            // editEntityButton
            //
            this.editEntityButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editEntityButton.Enabled = false;
            this.editEntityButton.Location = new System.Drawing.Point(456, 2);
            this.editEntityButton.Name = "editEntityButton";
            this.editEntityButton.Size = new System.Drawing.Size(75, 23);
            this.editEntityButton.TabIndex = 10;
            this.editEntityButton.Text = "edit";
            this.editEntityButton.UseVisualStyleBackColor = true;
            this.editEntityButton.Click += new System.EventHandler(this.editEntityButton_Click);
            //
            // entityTable
            //
            this.entityTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.entityTable.HideSelection = false;
            this.entityTable.Location = new System.Drawing.Point(0, 28);
            this.entityTable.Margin = new System.Windows.Forms.Padding(0);
            this.entityTable.Name = "entityTable";
            this.entityTable.Size = new System.Drawing.Size(614, 567);
            this.entityTable.TabIndex = 6;
            this.entityTable.UseCompatibleStateImageBehavior = false;
            this.entityTable.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.entityTable_ColumnClick);
            this.entityTable.SelectedIndexChanged += new System.EventHandler(this.entityTable_SelectedIndexChanged);
            this.entityTable.KeyUp += new System.Windows.Forms.KeyEventHandler(this.entityTable_KeyUp);
            this.entityTable.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.entityTable_MouseDoubleClick);
            this.entityTable.MouseUp += new System.Windows.Forms.MouseEventHandler(this.entityTable_MouseUp);
            //
            // deselectEntityTableButton
            //
            this.deselectEntityTableButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deselectEntityTableButton.Location = new System.Drawing.Point(537, 2);
            this.deselectEntityTableButton.Name = "deselectEntityTableButton";
            this.deselectEntityTableButton.Size = new System.Drawing.Size(75, 23);
            this.deselectEntityTableButton.TabIndex = 8;
            this.deselectEntityTableButton.Text = "deselect";
            this.deselectEntityTableButton.UseVisualStyleBackColor = true;
            this.deselectEntityTableButton.Click += new System.EventHandler(this.deselectButton_Click);
            //
            // entityTableCheckbox
            //
            this.entityTableCheckbox.AutoSize = true;
            this.entityTableCheckbox.Checked = true;
            this.entityTableCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.entityTableCheckbox.Location = new System.Drawing.Point(6, 6);
            this.entityTableCheckbox.Name = "entityTableCheckbox";
            this.entityTableCheckbox.Size = new System.Drawing.Size(65, 17);
            this.entityTableCheckbox.TabIndex = 4;
            this.entityTableCheckbox.Text = "Enabled";
            this.entityTableCheckbox.UseVisualStyleBackColor = true;
            this.entityTableCheckbox.CheckedChanged += new System.EventHandler(this.entityTableCheckbox_CheckedChanged);
            //
            // connect9EntityCheckbox
            //
            this.connect9EntityCheckbox.AutoSize = true;
            this.connect9EntityCheckbox.Checked = true;
            this.connect9EntityCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.connect9EntityCheckbox.Location = new System.Drawing.Point(77, 6);
            this.connect9EntityCheckbox.Name = "connect9EntityCheckbox";
            this.connect9EntityCheckbox.Size = new System.Drawing.Size(120, 17);
            this.connect9EntityCheckbox.TabIndex = 9;
            this.connect9EntityCheckbox.Text = "walls and waypoints";
            this.connect9EntityCheckbox.UseVisualStyleBackColor = true;
            this.connect9EntityCheckbox.CheckedChanged += new System.EventHandler(this.connect9Checkbox_CheckedChanged);
            //
            // entityTableInfo
            //
            this.entityTableInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.entityTableInfo.Location = new System.Drawing.Point(167, 8);
            this.entityTableInfo.Name = "entityTableInfo";
            this.entityTableInfo.Size = new System.Drawing.Size(283, 13);
            this.entityTableInfo.TabIndex = 5;
            this.entityTableInfo.Text = "------------";
            this.entityTableInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // columnTab
            //
            this.columnTab.Controls.Add(this.editColumnButton);
            this.columnTab.Controls.Add(this.deselectColumnsButton);
            this.columnTab.Controls.Add(this.label3);
            this.columnTab.Controls.Add(this.highlightColumnsCheckbox);
            this.columnTab.Controls.Add(this.columnTableInfo);
            this.columnTab.Controls.Add(this.columnTableCheckbox);
            this.columnTab.Controls.Add(this.columnTable);
            this.columnTab.Location = new System.Drawing.Point(4, 22);
            this.columnTab.Name = "columnTab";
            this.columnTab.Padding = new System.Windows.Forms.Padding(3);
            this.columnTab.Size = new System.Drawing.Size(658, 595);
            this.columnTab.TabIndex = 1;
            this.columnTab.Text = "Columns";
            this.columnTab.UseVisualStyleBackColor = true;
            //
            // editColumnButton
            //
            this.editColumnButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editColumnButton.Enabled = false;
            this.editColumnButton.Location = new System.Drawing.Point(500, 2);
            this.editColumnButton.Name = "editColumnButton";
            this.editColumnButton.Size = new System.Drawing.Size(75, 23);
            this.editColumnButton.TabIndex = 13;
            this.editColumnButton.Text = "edit";
            this.editColumnButton.UseVisualStyleBackColor = true;
            this.editColumnButton.Click += new System.EventHandler(this.editColumnButton_Click);
            //
            // deselectColumnsButton
            //
            this.deselectColumnsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deselectColumnsButton.Location = new System.Drawing.Point(581, 2);
            this.deselectColumnsButton.Name = "deselectColumnsButton";
            this.deselectColumnsButton.Size = new System.Drawing.Size(75, 23);
            this.deselectColumnsButton.TabIndex = 12;
            this.deselectColumnsButton.Text = "deselect";
            this.deselectColumnsButton.UseVisualStyleBackColor = true;
            this.deselectColumnsButton.Click += new System.EventHandler(this.deselectColumnsButton_Click);
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(246, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "doubleclick jumps to block texture record";
            //
            // highlightColumnsCheckbox
            //
            this.highlightColumnsCheckbox.AutoSize = true;
            this.highlightColumnsCheckbox.Checked = true;
            this.highlightColumnsCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.highlightColumnsCheckbox.Location = new System.Drawing.Point(77, 6);
            this.highlightColumnsCheckbox.Name = "highlightColumnsCheckbox";
            this.highlightColumnsCheckbox.Size = new System.Drawing.Size(148, 17);
            this.highlightColumnsCheckbox.TabIndex = 10;
            this.highlightColumnsCheckbox.Text = "highlight selection on map";
            this.highlightColumnsCheckbox.UseVisualStyleBackColor = true;
            this.highlightColumnsCheckbox.CheckedChanged += new System.EventHandler(this.highlightColumnsCheckbox_CheckedChanged);
            //
            // columnTableInfo
            //
            this.columnTableInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.columnTableInfo.Location = new System.Drawing.Point(211, 8);
            this.columnTableInfo.Name = "columnTableInfo";
            this.columnTableInfo.Size = new System.Drawing.Size(283, 13);
            this.columnTableInfo.TabIndex = 9;
            this.columnTableInfo.Text = "------------";
            this.columnTableInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // columnTableCheckbox
            //
            this.columnTableCheckbox.AutoSize = true;
            this.columnTableCheckbox.Checked = true;
            this.columnTableCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.columnTableCheckbox.Location = new System.Drawing.Point(6, 6);
            this.columnTableCheckbox.Name = "columnTableCheckbox";
            this.columnTableCheckbox.Size = new System.Drawing.Size(65, 17);
            this.columnTableCheckbox.TabIndex = 8;
            this.columnTableCheckbox.Text = "Enabled";
            this.columnTableCheckbox.UseVisualStyleBackColor = true;
            this.columnTableCheckbox.CheckedChanged += new System.EventHandler(this.columnTableCheckbox_CheckedChanged);
            //
            // columnTable
            //
            this.columnTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.columnTable.HideSelection = false;
            this.columnTable.Location = new System.Drawing.Point(0, 28);
            this.columnTable.Margin = new System.Windows.Forms.Padding(0);
            this.columnTable.Name = "columnTable";
            this.columnTable.Size = new System.Drawing.Size(658, 567);
            this.columnTable.TabIndex = 7;
            this.columnTable.UseCompatibleStateImageBehavior = false;
            this.columnTable.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.columnTable_ColumnClick);
            this.columnTable.SelectedIndexChanged += new System.EventHandler(this.columnTable_SelectedIndexChanged);
            this.columnTable.KeyUp += new System.Windows.Forms.KeyEventHandler(this.columnTable_KeyUp);
            this.columnTable.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.columnTable_MouseDoubleClick);
            this.columnTable.MouseUp += new System.Windows.Forms.MouseEventHandler(this.columnTable_MouseUp);
            //
            // blockTexTab
            //
            this.blockTexTab.Controls.Add(this.cubePreviewTopPanel);
            this.blockTexTab.Controls.Add(this.cubePreviewBottomPanel);
            this.blockTexTab.Controls.Add(this.editBlockTexButton);
            this.blockTexTab.Controls.Add(this.label4);
            this.blockTexTab.Controls.Add(this.deselectBlockTexButton);
            this.blockTexTab.Controls.Add(this.highlightBlockTexCheckbox);
            this.blockTexTab.Controls.Add(this.blockTexTableCheckbox);
            this.blockTexTab.Controls.Add(this.blockTexTableInfo);
            this.blockTexTab.Controls.Add(this.blockTexTable);
            this.blockTexTab.Location = new System.Drawing.Point(4, 22);
            this.blockTexTab.Name = "blockTexTab";
            this.blockTexTab.Size = new System.Drawing.Size(658, 595);
            this.blockTexTab.TabIndex = 2;
            this.blockTexTab.Text = "Block Textures";
            this.blockTexTab.UseVisualStyleBackColor = true;
            //
            // cubePreviewTopPanel
            //
            this.cubePreviewTopPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cubePreviewTopPanel.Controls.Add(this.cubePreviewTopPicture);
            this.cubePreviewTopPanel.Location = new System.Drawing.Point(427, 363);
            this.cubePreviewTopPanel.Name = "cubePreviewTopPanel";
            this.cubePreviewTopPanel.Size = new System.Drawing.Size(200, 200);
            this.cubePreviewTopPanel.TabIndex = 16;
            this.cubePreviewTopPanel.Visible = false;
            //
            // cubePreviewTopPicture
            //
            this.cubePreviewTopPicture.Location = new System.Drawing.Point(3, 3);
            this.cubePreviewTopPicture.Name = "cubePreviewTopPicture";
            this.cubePreviewTopPicture.Size = new System.Drawing.Size(194, 194);
            this.cubePreviewTopPicture.TabIndex = 0;
            this.cubePreviewTopPicture.TabStop = false;
            //
            // cubePreviewBottomPanel
            //
            this.cubePreviewBottomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cubePreviewBottomPanel.Controls.Add(this.cubePreviewBottomPicture);
            this.cubePreviewBottomPanel.Location = new System.Drawing.Point(427, 157);
            this.cubePreviewBottomPanel.Name = "cubePreviewBottomPanel";
            this.cubePreviewBottomPanel.Size = new System.Drawing.Size(200, 200);
            this.cubePreviewBottomPanel.TabIndex = 16;
            this.cubePreviewBottomPanel.Visible = false;
            //
            // cubePreviewBottomPicture
            //
            this.cubePreviewBottomPicture.Location = new System.Drawing.Point(3, 3);
            this.cubePreviewBottomPicture.Name = "cubePreviewBottomPicture";
            this.cubePreviewBottomPicture.Size = new System.Drawing.Size(194, 194);
            this.cubePreviewBottomPicture.TabIndex = 0;
            this.cubePreviewBottomPicture.TabStop = false;
            //
            // editBlockTexButton
            //
            this.editBlockTexButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.editBlockTexButton.Enabled = false;
            this.editBlockTexButton.Location = new System.Drawing.Point(500, 2);
            this.editBlockTexButton.Name = "editBlockTexButton";
            this.editBlockTexButton.Size = new System.Drawing.Size(75, 23);
            this.editBlockTexButton.TabIndex = 15;
            this.editBlockTexButton.Text = "edit";
            this.editBlockTexButton.UseVisualStyleBackColor = true;
            this.editBlockTexButton.Click += new System.EventHandler(this.editBlockTexButton_Click);
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(252, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "doubleclick shows block textures";
            //
            // deselectBlockTexButton
            //
            this.deselectBlockTexButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.deselectBlockTexButton.Location = new System.Drawing.Point(581, 2);
            this.deselectBlockTexButton.Name = "deselectBlockTexButton";
            this.deselectBlockTexButton.Size = new System.Drawing.Size(75, 23);
            this.deselectBlockTexButton.TabIndex = 13;
            this.deselectBlockTexButton.Text = "deselect";
            this.deselectBlockTexButton.UseVisualStyleBackColor = true;
            this.deselectBlockTexButton.Click += new System.EventHandler(this.deselectBlockTexButton_Click);
            //
            // highlightBlockTexCheckbox
            //
            this.highlightBlockTexCheckbox.AutoSize = true;
            this.highlightBlockTexCheckbox.Checked = true;
            this.highlightBlockTexCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.highlightBlockTexCheckbox.Location = new System.Drawing.Point(77, 6);
            this.highlightBlockTexCheckbox.Name = "highlightBlockTexCheckbox";
            this.highlightBlockTexCheckbox.Size = new System.Drawing.Size(148, 17);
            this.highlightBlockTexCheckbox.TabIndex = 12;
            this.highlightBlockTexCheckbox.Text = "highlight selection on map";
            this.highlightBlockTexCheckbox.UseVisualStyleBackColor = true;
            this.highlightBlockTexCheckbox.CheckedChanged += new System.EventHandler(this.highlightBlockTexCheckbox_CheckedChanged);
            //
            // blockTexTableCheckbox
            //
            this.blockTexTableCheckbox.AutoSize = true;
            this.blockTexTableCheckbox.Checked = true;
            this.blockTexTableCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.blockTexTableCheckbox.Location = new System.Drawing.Point(6, 6);
            this.blockTexTableCheckbox.Name = "blockTexTableCheckbox";
            this.blockTexTableCheckbox.Size = new System.Drawing.Size(65, 17);
            this.blockTexTableCheckbox.TabIndex = 11;
            this.blockTexTableCheckbox.Text = "Enabled";
            this.blockTexTableCheckbox.UseVisualStyleBackColor = true;
            this.blockTexTableCheckbox.CheckedChanged += new System.EventHandler(this.blockTexTableCheckbox_CheckedChanged);
            //
            // blockTexTableInfo
            //
            this.blockTexTableInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.blockTexTableInfo.Location = new System.Drawing.Point(212, 8);
            this.blockTexTableInfo.Name = "blockTexTableInfo";
            this.blockTexTableInfo.Size = new System.Drawing.Size(282, 13);
            this.blockTexTableInfo.TabIndex = 10;
            this.blockTexTableInfo.Text = "------------";
            this.blockTexTableInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // blockTexTable
            //
            this.blockTexTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blockTexTable.HideSelection = false;
            this.blockTexTable.Location = new System.Drawing.Point(0, 28);
            this.blockTexTable.Margin = new System.Windows.Forms.Padding(0);
            this.blockTexTable.Name = "blockTexTable";
            this.blockTexTable.Size = new System.Drawing.Size(658, 567);
            this.blockTexTable.TabIndex = 8;
            this.blockTexTable.UseCompatibleStateImageBehavior = false;
            this.blockTexTable.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.blockTexTable_ColumnClick);
            this.blockTexTable.SelectedIndexChanged += new System.EventHandler(this.blockTexTable_SelectedIndexChanged);
            this.blockTexTable.KeyUp += new System.Windows.Forms.KeyEventHandler(this.blockTexTable_KeyUp);
            this.blockTexTable.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.blockTexTable_MouseDoubleClick);
            this.blockTexTable.MouseUp += new System.Windows.Forms.MouseEventHandler(this.blockTexTable_MouseUp);
            //
            // unknownTab
            //
            this.unknownTab.Controls.Add(this.button1);
            this.unknownTab.Controls.Add(this.label5);
            this.unknownTab.Controls.Add(this.button2);
            this.unknownTab.Controls.Add(this.checkBox1);
            this.unknownTab.Controls.Add(this.checkBox2);
            this.unknownTab.Controls.Add(this.unknownTableInfo);
            this.unknownTab.Controls.Add(this.unknownTable247264);
            this.unknownTab.Location = new System.Drawing.Point(4, 22);
            this.unknownTab.Name = "unknownTab";
            this.unknownTab.Size = new System.Drawing.Size(658, 595);
            this.unknownTab.TabIndex = 3;
            this.unknownTab.Text = "unknown@247264";
            this.unknownTab.UseVisualStyleBackColor = true;
            //
            // button1
            //
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(500, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "edit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            //
            // label5
            //
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(252, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(163, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "doubleclick shows block textures";
            this.label5.Visible = false;
            //
            // button2
            //
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(581, 1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 20;
            this.button2.Text = "deselect";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            //
            // checkBox1
            //
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(77, 5);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(148, 17);
            this.checkBox1.TabIndex = 19;
            this.checkBox1.Text = "highlight selection on map";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.Visible = false;
            //
            // checkBox2
            //
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(6, 5);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(65, 17);
            this.checkBox2.TabIndex = 18;
            this.checkBox2.Text = "Enabled";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.Visible = false;
            //
            // unknownTableInfo
            //
            this.unknownTableInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unknownTableInfo.Location = new System.Drawing.Point(212, 7);
            this.unknownTableInfo.Name = "unknownTableInfo";
            this.unknownTableInfo.Size = new System.Drawing.Size(282, 13);
            this.unknownTableInfo.TabIndex = 17;
            this.unknownTableInfo.Text = "------------";
            this.unknownTableInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // unknownTable247264
            //
            this.unknownTable247264.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unknownTable247264.HideSelection = false;
            this.unknownTable247264.Location = new System.Drawing.Point(0, 27);
            this.unknownTable247264.Margin = new System.Windows.Forms.Padding(0);
            this.unknownTable247264.Name = "unknownTable247264";
            this.unknownTable247264.Size = new System.Drawing.Size(658, 567);
            this.unknownTable247264.TabIndex = 16;
            this.unknownTable247264.UseCompatibleStateImageBehavior = false;
            this.unknownTable247264.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.unknownTable247264_ColumnClick);
            //
            // unknown358222Tab
            //
            this.unknown358222Tab.Controls.Add(this.button3);
            this.unknown358222Tab.Controls.Add(this.label6);
            this.unknown358222Tab.Controls.Add(this.button4);
            this.unknown358222Tab.Controls.Add(this.checkBox3);
            this.unknown358222Tab.Controls.Add(this.checkBox4);
            this.unknown358222Tab.Controls.Add(this.unknownTable358222Info);
            this.unknown358222Tab.Controls.Add(this.unknownTable358222);
            this.unknown358222Tab.Location = new System.Drawing.Point(4, 22);
            this.unknown358222Tab.Name = "unknown358222Tab";
            this.unknown358222Tab.Size = new System.Drawing.Size(658, 595);
            this.unknown358222Tab.TabIndex = 4;
            this.unknown358222Tab.Text = "unknown@358222";
            this.unknown358222Tab.UseVisualStyleBackColor = true;
            //
            // button3
            //
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(500, 1);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 29;
            this.button3.Text = "edit";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            //
            // label6
            //
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(252, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(163, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "doubleclick shows block textures";
            this.label6.Visible = false;
            //
            // button4
            //
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(581, 1);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 27;
            this.button4.Text = "deselect";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            //
            // checkBox3
            //
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(77, 5);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(148, 17);
            this.checkBox3.TabIndex = 26;
            this.checkBox3.Text = "highlight selection on map";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.Visible = false;
            //
            // checkBox4
            //
            this.checkBox4.AutoSize = true;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Location = new System.Drawing.Point(6, 5);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(65, 17);
            this.checkBox4.TabIndex = 25;
            this.checkBox4.Text = "Enabled";
            this.checkBox4.UseVisualStyleBackColor = true;
            this.checkBox4.Visible = false;
            //
            // unknownTable358222Info
            //
            this.unknownTable358222Info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.unknownTable358222Info.Location = new System.Drawing.Point(212, 7);
            this.unknownTable358222Info.Name = "unknownTable358222Info";
            this.unknownTable358222Info.Size = new System.Drawing.Size(282, 13);
            this.unknownTable358222Info.TabIndex = 24;
            this.unknownTable358222Info.Text = "------------";
            this.unknownTable358222Info.TextAlign = System.Drawing.ContentAlignment.TopRight;
            //
            // unknownTable358222
            //
            this.unknownTable358222.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.unknownTable358222.HideSelection = false;
            this.unknownTable358222.Location = new System.Drawing.Point(0, 27);
            this.unknownTable358222.Margin = new System.Windows.Forms.Padding(0);
            this.unknownTable358222.Name = "unknownTable358222";
            this.unknownTable358222.Size = new System.Drawing.Size(658, 567);
            this.unknownTable358222.TabIndex = 23;
            this.unknownTable358222.UseCompatibleStateImageBehavior = false;
            this.unknownTable358222.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.unknownTable358222_ColumnClick);
            //
            // logTextbox
            //
            this.logTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextbox.Location = new System.Drawing.Point(581, 9);
            this.logTextbox.Multiline = true;
            this.logTextbox.Name = "logTextbox";
            this.logTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextbox.Size = new System.Drawing.Size(284, 47);
            this.logTextbox.TabIndex = 10;
            this.logTextbox.Visible = false;
            //
            // dotsizeInput
            //
            this.dotsizeInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dotsizeInput.Location = new System.Drawing.Point(149, 28);
            this.dotsizeInput.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.dotsizeInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.dotsizeInput.Name = "dotsizeInput";
            this.dotsizeInput.Size = new System.Drawing.Size(60, 26);
            this.dotsizeInput.TabIndex = 14;
            this.dotsizeInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.dotsizeInput.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.dotsizeInput.ValueChanged += new System.EventHandler(this.dotsizeInput_ValueChanged);
            //
            // mouseInfoLabel
            //
            this.mouseInfoLabel.AutoSize = true;
            this.mouseInfoLabel.Location = new System.Drawing.Point(232, 35);
            this.mouseInfoLabel.Name = "mouseInfoLabel";
            this.mouseInfoLabel.Size = new System.Drawing.Size(266, 13);
            this.mouseInfoLabel.TabIndex = 13;
            this.mouseInfoLabel.Text = "left click on map shows coords, right click selects node";
            //
            // label2
            //
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Map byte";
            //
            // mapByteInput
            //
            this.mapByteInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mapByteInput.Location = new System.Drawing.Point(17, 28);
            this.mapByteInput.Maximum = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.mapByteInput.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            -2147483648});
            this.mapByteInput.Name = "mapByteInput";
            this.mapByteInput.Size = new System.Drawing.Size(60, 26);
            this.mapByteInput.TabIndex = 11;
            this.mapByteInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.mapByteInput.Value = new decimal(new int[] {
            2,
            0,
            0,
            -2147483648});
            this.mapByteInput.ValueChanged += new System.EventHandler(this.mapByteInput_ValueChanged);
            //
            // dotSizeLabel
            //
            this.dotSizeLabel.AutoSize = true;
            this.dotSizeLabel.Location = new System.Drawing.Point(147, 12);
            this.dotSizeLabel.Name = "dotSizeLabel";
            this.dotSizeLabel.Size = new System.Drawing.Size(55, 13);
            this.dotSizeLabel.TabIndex = 3;
            this.dotSizeLabel.Text = "Dot radius";
            //
            // splitContainer1
            //
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 64);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            //
            // splitContainer1.Panel1
            //
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.AutoScrollMinSize = new System.Drawing.Size(340, 0);
            this.splitContainer1.Panel1.Controls.Add(this.canvasBox);
            //
            // splitContainer1.Panel2
            //
            this.splitContainer1.Panel2.Controls.Add(this.settingsPanel);
            this.splitContainer1.Size = new System.Drawing.Size(1246, 621);
            this.splitContainer1.SplitterDistance = 614;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 5;
            //
            // panel1
            //
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.writeButton);
            this.panel1.Controls.Add(this.logTextbox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.dotSizeLabel);
            this.panel1.Controls.Add(this.mouseInfoLabel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.zoomInput);
            this.panel1.Controls.Add(this.dotsizeInput);
            this.panel1.Controls.Add(this.mapByteInput);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1243, 64);
            this.panel1.TabIndex = 15;
            //
            // writeButton
            //
            this.writeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.writeButton.Enabled = false;
            this.writeButton.Location = new System.Drawing.Point(1085, 14);
            this.writeButton.Name = "writeButton";
            this.writeButton.Size = new System.Drawing.Size(144, 36);
            this.writeButton.TabIndex = 15;
            this.writeButton.Text = "WRITE TO OUTPUT";
            this.writeButton.UseVisualStyleBackColor = true;
            this.writeButton.Click += new System.EventHandler(this.writeButton_Click);
            //
            // MapForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1246, 685);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MapForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Level Map";
            ((System.ComponentModel.ISupportInitialize)(this.canvasBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoomInput)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.tableTabs.ResumeLayout(false);
            this.entityTab.ResumeLayout(false);
            this.entityTab.PerformLayout();
            this.columnTab.ResumeLayout(false);
            this.columnTab.PerformLayout();
            this.blockTexTab.ResumeLayout(false);
            this.blockTexTab.PerformLayout();
            this.cubePreviewTopPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cubePreviewTopPicture)).EndInit();
            this.cubePreviewBottomPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cubePreviewBottomPicture)).EndInit();
            this.unknownTab.ResumeLayout(false);
            this.unknownTab.PerformLayout();
            this.unknown358222Tab.ResumeLayout(false);
            this.unknown358222Tab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dotsizeInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mapByteInput)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox canvasBox;
        private System.Windows.Forms.NumericUpDown zoomInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.CheckBox entityTableCheckbox;
        private System.Windows.Forms.Label entityTableInfo;
        private System.Windows.Forms.ListView entityTable;
        private System.Windows.Forms.Button deselectEntityTableButton;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox connect9EntityCheckbox;
        private System.Windows.Forms.TextBox logTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown mapByteInput;
        private System.Windows.Forms.Label mouseInfoLabel;
        private System.Windows.Forms.NumericUpDown dotsizeInput;
        private System.Windows.Forms.Label dotSizeLabel;
        private System.Windows.Forms.TabControl tableTabs;
        private System.Windows.Forms.TabPage entityTab;
        private System.Windows.Forms.TabPage columnTab;
        private System.Windows.Forms.TabPage blockTexTab;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox columnTableCheckbox;
        private System.Windows.Forms.ListView columnTable;
        private System.Windows.Forms.Label columnTableInfo;
        private System.Windows.Forms.CheckBox blockTexTableCheckbox;
        private System.Windows.Forms.Label blockTexTableInfo;
        private System.Windows.Forms.ListView blockTexTable;
        private System.Windows.Forms.CheckBox highlightColumnsCheckbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox highlightBlockTexCheckbox;
        private System.Windows.Forms.Button deselectColumnsButton;
        private System.Windows.Forms.Button deselectBlockTexButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button writeButton;
        private System.Windows.Forms.Button editEntityButton;
        private System.Windows.Forms.Button editColumnButton;
        private System.Windows.Forms.Button editBlockTexButton;
        private System.Windows.Forms.Panel cubePreviewBottomPanel;
        private System.Windows.Forms.PictureBox cubePreviewBottomPicture;
        private System.Windows.Forms.Panel cubePreviewTopPanel;
        private System.Windows.Forms.PictureBox cubePreviewTopPicture;
        private System.Windows.Forms.TabPage unknownTab;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label unknownTableInfo;
        private System.Windows.Forms.ListView unknownTable247264;
        private System.Windows.Forms.TabPage unknown358222Tab;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.Label unknownTable358222Info;
        private System.Windows.Forms.ListView unknownTable358222;
        private System.Windows.Forms.CheckBox morphCheckbox;
    }
}
