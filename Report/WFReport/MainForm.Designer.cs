namespace WFReport
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.splLeftPanel = new System.Windows.Forms.Splitter();
            this.gbxAdditional = new System.Windows.Forms.GroupBox();
            this.btnMainFiltersDelete = new System.Windows.Forms.Button();
            this.btnMainFiltersEdit = new System.Windows.Forms.Button();
            this.btnMainFiltersAdd = new System.Windows.Forms.Button();
            this.lbxAdditionalFilters = new System.Windows.Forms.ListBox();
            this.gbxColRowAgg = new System.Windows.Forms.GroupBox();
            this.btnFiltersAggregate = new System.Windows.Forms.Button();
            this.btnFiltersRows = new System.Windows.Forms.Button();
            this.btnFiltersColumn = new System.Windows.Forms.Button();
            this.cboAggregate = new System.Windows.Forms.ComboBox();
            this.cboRows = new System.Windows.Forms.ComboBox();
            this.lblAggregate = new System.Windows.Forms.Label();
            this.cboColumns = new System.Windows.Forms.ComboBox();
            this.lblRows = new System.Windows.Forms.Label();
            this.lblColumns = new System.Windows.Forms.Label();
            this.gbxAllFields = new System.Windows.Forms.GroupBox();
            this.lbxImportantFields = new System.Windows.Forms.CheckedListBox();
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.mimFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mimFileNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mimFileOpenReport = new System.Windows.Forms.ToolStripMenuItem();
            this.mimFileOpenMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.mspOpenSave = new System.Windows.Forms.ToolStripSeparator();
            this.mimFileSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mimFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mspSavePrint = new System.Windows.Forms.ToolStripSeparator();
            this.mimFilePrint = new System.Windows.Forms.ToolStripMenuItem();
            this.mspPrintExit = new System.Windows.Forms.ToolStripSeparator();
            this.mimFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mimOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mimOptionsOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.mimOptionsAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tbrMain = new System.Windows.Forms.ToolStrip();
            this.tbxNew = new System.Windows.Forms.ToolStripButton();
            this.tbxOpenReport = new System.Windows.Forms.ToolStripButton();
            this.tbxSave = new System.Windows.Forms.ToolStripButton();
            this.tbxPrint = new System.Windows.Forms.ToolStripButton();
            this.mspToolbar = new System.Windows.Forms.ToolStripSeparator();
            this.tbxHelp = new System.Windows.Forms.ToolStripButton();
            this.ofdReport = new System.Windows.Forms.OpenFileDialog();
            this.sfdReport = new System.Windows.Forms.SaveFileDialog();
            this.ofdMetadata = new System.Windows.Forms.OpenFileDialog();
            this.pnlLeft.SuspendLayout();
            this.gbxAdditional.SuspendLayout();
            this.gbxColRowAgg.SuspendLayout();
            this.gbxAllFields.SuspendLayout();
            this.menuMain.SuspendLayout();
            this.tbrMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLeft
            // 
            this.pnlLeft.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pnlLeft.Controls.Add(this.btnReload);
            this.pnlLeft.Controls.Add(this.btnGenerate);
            this.pnlLeft.Controls.Add(this.splLeftPanel);
            this.pnlLeft.Controls.Add(this.gbxAdditional);
            this.pnlLeft.Controls.Add(this.gbxColRowAgg);
            this.pnlLeft.Controls.Add(this.gbxAllFields);
            this.pnlLeft.Location = new System.Drawing.Point(0, 52);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(271, 509);
            this.pnlLeft.TabIndex = 0;
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(11, 478);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(115, 23);
            this.btnReload.TabIndex = 2;
            this.btnReload.Text = "Reload data";
            this.btnReload.UseVisualStyleBackColor = true;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(141, 478);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(115, 23);
            this.btnGenerate.TabIndex = 2;
            this.btnGenerate.Text = "Generate graph";
            this.btnGenerate.UseVisualStyleBackColor = true;
            // 
            // splLeftPanel
            // 
            this.splLeftPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.splLeftPanel.Location = new System.Drawing.Point(268, 0);
            this.splLeftPanel.Name = "splLeftPanel";
            this.splLeftPanel.Size = new System.Drawing.Size(3, 509);
            this.splLeftPanel.TabIndex = 1;
            this.splLeftPanel.TabStop = false;
            this.splLeftPanel.SplitterMoving += new System.Windows.Forms.SplitterEventHandler(this.splitter1_SplitterMoving);
            this.splLeftPanel.Move += new System.EventHandler(this.splitter1_Move);
            // 
            // gbxAdditional
            // 
            this.gbxAdditional.Controls.Add(this.btnMainFiltersDelete);
            this.gbxAdditional.Controls.Add(this.btnMainFiltersEdit);
            this.gbxAdditional.Controls.Add(this.btnMainFiltersAdd);
            this.gbxAdditional.Controls.Add(this.lbxAdditionalFilters);
            this.gbxAdditional.Location = new System.Drawing.Point(11, 356);
            this.gbxAdditional.Name = "gbxAdditional";
            this.gbxAdditional.Size = new System.Drawing.Size(251, 116);
            this.gbxAdditional.TabIndex = 0;
            this.gbxAdditional.TabStop = false;
            this.gbxAdditional.Text = "Additional filters";
            // 
            // btnMainFiltersDelete
            // 
            this.btnMainFiltersDelete.Location = new System.Drawing.Point(186, 90);
            this.btnMainFiltersDelete.Name = "btnMainFiltersDelete";
            this.btnMainFiltersDelete.Size = new System.Drawing.Size(59, 23);
            this.btnMainFiltersDelete.TabIndex = 2;
            this.btnMainFiltersDelete.Text = "Delete";
            this.btnMainFiltersDelete.UseVisualStyleBackColor = true;
            // 
            // btnMainFiltersEdit
            // 
            this.btnMainFiltersEdit.Location = new System.Drawing.Point(121, 90);
            this.btnMainFiltersEdit.Name = "btnMainFiltersEdit";
            this.btnMainFiltersEdit.Size = new System.Drawing.Size(59, 23);
            this.btnMainFiltersEdit.TabIndex = 2;
            this.btnMainFiltersEdit.Text = "Edit...";
            this.btnMainFiltersEdit.UseVisualStyleBackColor = true;
            // 
            // btnMainFiltersAdd
            // 
            this.btnMainFiltersAdd.Location = new System.Drawing.Point(7, 90);
            this.btnMainFiltersAdd.Name = "btnMainFiltersAdd";
            this.btnMainFiltersAdd.Size = new System.Drawing.Size(59, 23);
            this.btnMainFiltersAdd.TabIndex = 2;
            this.btnMainFiltersAdd.Text = "Add...";
            this.btnMainFiltersAdd.UseVisualStyleBackColor = true;
            // 
            // lbxAdditionalFilters
            // 
            this.lbxAdditionalFilters.FormattingEnabled = true;
            this.lbxAdditionalFilters.Location = new System.Drawing.Point(7, 20);
            this.lbxAdditionalFilters.Name = "lbxAdditionalFilters";
            this.lbxAdditionalFilters.Size = new System.Drawing.Size(238, 69);
            this.lbxAdditionalFilters.TabIndex = 0;
            // 
            // gbxColRowAgg
            // 
            this.gbxColRowAgg.Controls.Add(this.btnFiltersAggregate);
            this.gbxColRowAgg.Controls.Add(this.btnFiltersRows);
            this.gbxColRowAgg.Controls.Add(this.btnFiltersColumn);
            this.gbxColRowAgg.Controls.Add(this.cboAggregate);
            this.gbxColRowAgg.Controls.Add(this.cboRows);
            this.gbxColRowAgg.Controls.Add(this.lblAggregate);
            this.gbxColRowAgg.Controls.Add(this.cboColumns);
            this.gbxColRowAgg.Controls.Add(this.lblRows);
            this.gbxColRowAgg.Controls.Add(this.lblColumns);
            this.gbxColRowAgg.Location = new System.Drawing.Point(12, 202);
            this.gbxColRowAgg.Name = "gbxColRowAgg";
            this.gbxColRowAgg.Size = new System.Drawing.Size(251, 148);
            this.gbxColRowAgg.TabIndex = 0;
            this.gbxColRowAgg.TabStop = false;
            this.gbxColRowAgg.Text = "Columns, rows, aggregate";
            // 
            // btnFiltersAggregate
            // 
            this.btnFiltersAggregate.Location = new System.Drawing.Point(186, 118);
            this.btnFiltersAggregate.Name = "btnFiltersAggregate";
            this.btnFiltersAggregate.Size = new System.Drawing.Size(59, 23);
            this.btnFiltersAggregate.TabIndex = 2;
            this.btnFiltersAggregate.Text = "filters...";
            this.btnFiltersAggregate.UseVisualStyleBackColor = true;
            // 
            // btnFiltersRows
            // 
            this.btnFiltersRows.Location = new System.Drawing.Point(186, 77);
            this.btnFiltersRows.Name = "btnFiltersRows";
            this.btnFiltersRows.Size = new System.Drawing.Size(59, 23);
            this.btnFiltersRows.TabIndex = 2;
            this.btnFiltersRows.Text = "filters...";
            this.btnFiltersRows.UseVisualStyleBackColor = true;
            // 
            // btnFiltersColumn
            // 
            this.btnFiltersColumn.Location = new System.Drawing.Point(186, 36);
            this.btnFiltersColumn.Name = "btnFiltersColumn";
            this.btnFiltersColumn.Size = new System.Drawing.Size(59, 23);
            this.btnFiltersColumn.TabIndex = 2;
            this.btnFiltersColumn.Text = "filters...";
            this.btnFiltersColumn.UseVisualStyleBackColor = true;
            // 
            // cboAggregate
            // 
            this.cboAggregate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAggregate.FormattingEnabled = true;
            this.cboAggregate.Location = new System.Drawing.Point(10, 119);
            this.cboAggregate.Name = "cboAggregate";
            this.cboAggregate.Size = new System.Drawing.Size(169, 21);
            this.cboAggregate.TabIndex = 1;
            // 
            // cboRows
            // 
            this.cboRows.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRows.FormattingEnabled = true;
            this.cboRows.Location = new System.Drawing.Point(10, 78);
            this.cboRows.Name = "cboRows";
            this.cboRows.Size = new System.Drawing.Size(169, 21);
            this.cboRows.TabIndex = 1;
            // 
            // lblAggregate
            // 
            this.lblAggregate.AutoSize = true;
            this.lblAggregate.Location = new System.Drawing.Point(7, 102);
            this.lblAggregate.Name = "lblAggregate";
            this.lblAggregate.Size = new System.Drawing.Size(80, 13);
            this.lblAggregate.TabIndex = 0;
            this.lblAggregate.Text = "Aggregate data";
            // 
            // cboColumns
            // 
            this.cboColumns.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboColumns.FormattingEnabled = true;
            this.cboColumns.Location = new System.Drawing.Point(10, 37);
            this.cboColumns.Name = "cboColumns";
            this.cboColumns.Size = new System.Drawing.Size(169, 21);
            this.cboColumns.TabIndex = 1;
            // 
            // lblRows
            // 
            this.lblRows.AutoSize = true;
            this.lblRows.Location = new System.Drawing.Point(7, 61);
            this.lblRows.Name = "lblRows";
            this.lblRows.Size = new System.Drawing.Size(34, 13);
            this.lblRows.TabIndex = 0;
            this.lblRows.Text = "Rows";
            // 
            // lblColumns
            // 
            this.lblColumns.AutoSize = true;
            this.lblColumns.Location = new System.Drawing.Point(7, 20);
            this.lblColumns.Name = "lblColumns";
            this.lblColumns.Size = new System.Drawing.Size(47, 13);
            this.lblColumns.TabIndex = 0;
            this.lblColumns.Text = "Columns";
            // 
            // gbxAllFields
            // 
            this.gbxAllFields.Controls.Add(this.lbxImportantFields);
            this.gbxAllFields.Location = new System.Drawing.Point(11, 3);
            this.gbxAllFields.Name = "gbxAllFields";
            this.gbxAllFields.Size = new System.Drawing.Size(251, 193);
            this.gbxAllFields.TabIndex = 0;
            this.gbxAllFields.TabStop = false;
            this.gbxAllFields.Text = "Select fields important for the report";
            // 
            // lbxImportantFields
            // 
            this.lbxImportantFields.FormattingEnabled = true;
            this.lbxImportantFields.Location = new System.Drawing.Point(7, 20);
            this.lbxImportantFields.Name = "lbxImportantFields";
            this.lbxImportantFields.Size = new System.Drawing.Size(238, 169);
            this.lbxImportantFields.TabIndex = 0;
            this.lbxImportantFields.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // menuMain
            // 
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mimFile,
            this.mimOptions});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1008, 24);
            this.menuMain.TabIndex = 1;
            this.menuMain.Text = "menuStrip1";
            // 
            // mimFile
            // 
            this.mimFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mimFileNew,
            this.mimFileOpenReport,
            this.mimFileOpenMetadata,
            this.mspOpenSave,
            this.mimFileSave,
            this.mimFileSaveAs,
            this.mspSavePrint,
            this.mimFilePrint,
            this.mspPrintExit,
            this.mimFileExit});
            this.mimFile.Name = "mimFile";
            this.mimFile.Size = new System.Drawing.Size(37, 20);
            this.mimFile.Text = "&File";
            // 
            // mimFileNew
            // 
            this.mimFileNew.Image = ((System.Drawing.Image)(resources.GetObject("mimFileNew.Image")));
            this.mimFileNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mimFileNew.Name = "mimFileNew";
            this.mimFileNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mimFileNew.Size = new System.Drawing.Size(201, 22);
            this.mimFileNew.Text = "&New";
            // 
            // mimFileOpenReport
            // 
            this.mimFileOpenReport.Image = ((System.Drawing.Image)(resources.GetObject("mimFileOpenReport.Image")));
            this.mimFileOpenReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mimFileOpenReport.Name = "mimFileOpenReport";
            this.mimFileOpenReport.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mimFileOpenReport.Size = new System.Drawing.Size(201, 22);
            this.mimFileOpenReport.Text = "&Open Report";
            this.mimFileOpenReport.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // mimFileOpenMetadata
            // 
            this.mimFileOpenMetadata.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mimFileOpenMetadata.Name = "mimFileOpenMetadata";
            this.mimFileOpenMetadata.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.mimFileOpenMetadata.Size = new System.Drawing.Size(201, 22);
            this.mimFileOpenMetadata.Text = "Open &Metadata";
            this.mimFileOpenMetadata.Click += new System.EventHandler(this.openMetadataToolStripMenuItem_Click);
            // 
            // mspOpenSave
            // 
            this.mspOpenSave.Name = "mspOpenSave";
            this.mspOpenSave.Size = new System.Drawing.Size(198, 6);
            // 
            // mimFileSave
            // 
            this.mimFileSave.Image = ((System.Drawing.Image)(resources.GetObject("mimFileSave.Image")));
            this.mimFileSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mimFileSave.Name = "mimFileSave";
            this.mimFileSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mimFileSave.Size = new System.Drawing.Size(201, 22);
            this.mimFileSave.Text = "&Save";
            // 
            // mimFileSaveAs
            // 
            this.mimFileSaveAs.Name = "mimFileSaveAs";
            this.mimFileSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.mimFileSaveAs.Size = new System.Drawing.Size(201, 22);
            this.mimFileSaveAs.Text = "Save &As";
            // 
            // mspSavePrint
            // 
            this.mspSavePrint.Name = "mspSavePrint";
            this.mspSavePrint.Size = new System.Drawing.Size(198, 6);
            // 
            // mimFilePrint
            // 
            this.mimFilePrint.Image = ((System.Drawing.Image)(resources.GetObject("mimFilePrint.Image")));
            this.mimFilePrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mimFilePrint.Name = "mimFilePrint";
            this.mimFilePrint.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.mimFilePrint.Size = new System.Drawing.Size(201, 22);
            this.mimFilePrint.Text = "&Print";
            // 
            // mspPrintExit
            // 
            this.mspPrintExit.Name = "mspPrintExit";
            this.mspPrintExit.Size = new System.Drawing.Size(198, 6);
            // 
            // mimFileExit
            // 
            this.mimFileExit.Name = "mimFileExit";
            this.mimFileExit.Size = new System.Drawing.Size(201, 22);
            this.mimFileExit.Text = "E&xit";
            // 
            // mimOptions
            // 
            this.mimOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mimOptionsOptions,
            this.mimOptionsAbout});
            this.mimOptions.Name = "mimOptions";
            this.mimOptions.Size = new System.Drawing.Size(61, 20);
            this.mimOptions.Text = "&Options";
            // 
            // mimOptionsOptions
            // 
            this.mimOptionsOptions.Name = "mimOptionsOptions";
            this.mimOptionsOptions.Size = new System.Drawing.Size(152, 22);
            this.mimOptionsOptions.Text = "&Options";
            // 
            // mimOptionsAbout
            // 
            this.mimOptionsAbout.Name = "mimOptionsAbout";
            this.mimOptionsAbout.Size = new System.Drawing.Size(152, 22);
            this.mimOptionsAbout.Text = "&About";
            // 
            // tbrMain
            // 
            this.tbrMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbxNew,
            this.tbxOpenReport,
            this.tbxSave,
            this.tbxPrint,
            this.mspToolbar,
            this.tbxHelp});
            this.tbrMain.Location = new System.Drawing.Point(0, 24);
            this.tbrMain.Name = "tbrMain";
            this.tbrMain.Size = new System.Drawing.Size(1008, 25);
            this.tbrMain.TabIndex = 2;
            this.tbrMain.Text = "toolStrip1";
            // 
            // tbxNew
            // 
            this.tbxNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbxNew.Image = ((System.Drawing.Image)(resources.GetObject("tbxNew.Image")));
            this.tbxNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbxNew.Name = "tbxNew";
            this.tbxNew.Size = new System.Drawing.Size(23, 22);
            this.tbxNew.Text = "&New";
            // 
            // tbxOpenReport
            // 
            this.tbxOpenReport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbxOpenReport.Image = ((System.Drawing.Image)(resources.GetObject("tbxOpenReport.Image")));
            this.tbxOpenReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbxOpenReport.Name = "tbxOpenReport";
            this.tbxOpenReport.Size = new System.Drawing.Size(23, 22);
            this.tbxOpenReport.Text = "&Open";
            this.tbxOpenReport.Click += new System.EventHandler(this.openToolStripButton_Click);
            // 
            // tbxSave
            // 
            this.tbxSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbxSave.Image = ((System.Drawing.Image)(resources.GetObject("tbxSave.Image")));
            this.tbxSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbxSave.Name = "tbxSave";
            this.tbxSave.Size = new System.Drawing.Size(23, 22);
            this.tbxSave.Text = "&Save";
            // 
            // tbxPrint
            // 
            this.tbxPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbxPrint.Image = ((System.Drawing.Image)(resources.GetObject("tbxPrint.Image")));
            this.tbxPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbxPrint.Name = "tbxPrint";
            this.tbxPrint.Size = new System.Drawing.Size(23, 22);
            this.tbxPrint.Text = "&Print";
            // 
            // mspToolbar
            // 
            this.mspToolbar.Name = "mspToolbar";
            this.mspToolbar.Size = new System.Drawing.Size(6, 25);
            // 
            // tbxHelp
            // 
            this.tbxHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbxHelp.Image = ((System.Drawing.Image)(resources.GetObject("tbxHelp.Image")));
            this.tbxHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbxHelp.Name = "tbxHelp";
            this.tbxHelp.Size = new System.Drawing.Size(23, 22);
            this.tbxHelp.Text = "He&lp";
            this.tbxHelp.Click += new System.EventHandler(this.helpToolStripButton_Click);
            // 
            // ofdReport
            // 
            this.ofdReport.FileName = "openFileDialog1";
            // 
            // ofdMetadata
            // 
            this.ofdMetadata.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.tbrMain);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.menuMain);
            this.MainMenuStrip = this.menuMain;
            this.MinimumSize = new System.Drawing.Size(1024, 600);
            this.Name = "MainForm";
            this.Text = "OLOLAP";
            this.Resize += new System.EventHandler(this.MainResize);
            this.pnlLeft.ResumeLayout(false);
            this.gbxAdditional.ResumeLayout(false);
            this.gbxColRowAgg.ResumeLayout(false);
            this.gbxColRowAgg.PerformLayout();
            this.gbxAllFields.ResumeLayout(false);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.tbrMain.ResumeLayout(false);
            this.tbrMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.GroupBox gbxAllFields;
        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem mimFile;
        private System.Windows.Forms.ToolStripMenuItem mimFileNew;
        private System.Windows.Forms.ToolStripMenuItem mimFileOpenReport;
        private System.Windows.Forms.ToolStripSeparator mspOpenSave;
        private System.Windows.Forms.ToolStripMenuItem mimFileSave;
        private System.Windows.Forms.ToolStripMenuItem mimFileSaveAs;
        private System.Windows.Forms.ToolStripSeparator mspSavePrint;
        private System.Windows.Forms.ToolStripMenuItem mimFilePrint;
        private System.Windows.Forms.ToolStripSeparator mspPrintExit;
        private System.Windows.Forms.ToolStripMenuItem mimFileExit;
        private System.Windows.Forms.ToolStripMenuItem mimOptions;
        private System.Windows.Forms.ToolStripMenuItem mimOptionsOptions;
        private System.Windows.Forms.ToolStripMenuItem mimOptionsAbout;
        private System.Windows.Forms.ToolStrip tbrMain;
        private System.Windows.Forms.ToolStripButton tbxNew;
        private System.Windows.Forms.ToolStripButton tbxOpenReport;
        private System.Windows.Forms.ToolStripButton tbxSave;
        private System.Windows.Forms.ToolStripButton tbxPrint;
        private System.Windows.Forms.ToolStripSeparator mspToolbar;
        private System.Windows.Forms.ToolStripButton tbxHelp;
        private System.Windows.Forms.Splitter splLeftPanel;
        private System.Windows.Forms.CheckedListBox lbxImportantFields;
        private System.Windows.Forms.GroupBox gbxColRowAgg;
        private System.Windows.Forms.Label lblColumns;
        private System.Windows.Forms.ComboBox cboColumns;
        private System.Windows.Forms.Button btnFiltersColumn;
        private System.Windows.Forms.Button btnFiltersAggregate;
        private System.Windows.Forms.Button btnFiltersRows;
        private System.Windows.Forms.ComboBox cboAggregate;
        private System.Windows.Forms.ComboBox cboRows;
        private System.Windows.Forms.Label lblAggregate;
        private System.Windows.Forms.Label lblRows;
        private System.Windows.Forms.GroupBox gbxAdditional;
        private System.Windows.Forms.ListBox lbxAdditionalFilters;
        private System.Windows.Forms.Button btnMainFiltersDelete;
        private System.Windows.Forms.Button btnMainFiltersEdit;
        private System.Windows.Forms.Button btnMainFiltersAdd;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.ToolStripMenuItem mimFileOpenMetadata;
        private System.Windows.Forms.OpenFileDialog ofdReport;
        private System.Windows.Forms.SaveFileDialog sfdReport;
        private System.Windows.Forms.OpenFileDialog ofdMetadata;
    }
}

