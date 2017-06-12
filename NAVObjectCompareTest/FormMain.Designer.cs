namespace NAVObjectCompareWinClient
{
    partial class FormMain
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
            this.comparedDataGridView = new System.Windows.Forms.DataGridView();
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.notEqualCheckBox = new System.Windows.Forms.CheckBox();
            this.fileBLabel = new System.Windows.Forms.Label();
            this.fileALabel = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Object_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StringDateA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StringTimeA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VersionListA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoOfLinesA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StringDateB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StringTimeB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VersionListB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoOfLinesB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsEqual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Difference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.comparedDataGridView)).BeginInit();
            this.panelToolbar.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comparedDataGridView
            // 
            this.comparedDataGridView.AllowUserToAddRows = false;
            this.comparedDataGridView.AllowUserToDeleteRows = false;
            this.comparedDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.comparedDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Type,
            this.Object_Name,
            this.StringDateA,
            this.StringTimeA,
            this.VersionListA,
            this.NoOfLinesA,
            this.StringDateB,
            this.StringTimeB,
            this.VersionListB,
            this.NoOfLinesB,
            this.IsEqual,
            this.Difference});
            this.comparedDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comparedDataGridView.Location = new System.Drawing.Point(0, 89);
            this.comparedDataGridView.Name = "comparedDataGridView";
            this.comparedDataGridView.Size = new System.Drawing.Size(1428, 504);
            this.comparedDataGridView.TabIndex = 0;
            this.comparedDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.comparedDataGridView_CellMouseDoubleClick);
            this.comparedDataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.comparedDataGridView_CellPainting);
            this.comparedDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.comparedDataGridView_DataBindingComplete);
            // 
            // panelToolbar
            // 
            this.panelToolbar.Controls.Add(this.notEqualCheckBox);
            this.panelToolbar.Controls.Add(this.fileBLabel);
            this.panelToolbar.Controls.Add(this.fileALabel);
            this.panelToolbar.Controls.Add(this.menuStrip1);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Size = new System.Drawing.Size(1428, 89);
            this.panelToolbar.TabIndex = 1;
            // 
            // notEqualCheckBox
            // 
            this.notEqualCheckBox.AutoSize = true;
            this.notEqualCheckBox.Location = new System.Drawing.Point(12, 27);
            this.notEqualCheckBox.Name = "notEqualCheckBox";
            this.notEqualCheckBox.Size = new System.Drawing.Size(134, 17);
            this.notEqualCheckBox.TabIndex = 3;
            this.notEqualCheckBox.Text = "Show Only Differences";
            this.notEqualCheckBox.UseVisualStyleBackColor = true;
            this.notEqualCheckBox.CheckedChanged += new System.EventHandler(this.notEqualCheckBox_CheckedChanged);
            // 
            // fileBLabel
            // 
            this.fileBLabel.AutoSize = true;
            this.fileBLabel.Location = new System.Drawing.Point(805, 64);
            this.fileBLabel.Name = "fileBLabel";
            this.fileBLabel.Size = new System.Drawing.Size(93, 13);
            this.fileBLabel.TabIndex = 2;
            this.fileBLabel.Text = "B: No file selected";
            // 
            // fileALabel
            // 
            this.fileALabel.AutoSize = true;
            this.fileALabel.Location = new System.Drawing.Point(303, 64);
            this.fileALabel.Name = "fileALabel";
            this.fileALabel.Size = new System.Drawing.Size(93, 13);
            this.fileALabel.TabIndex = 1;
            this.fileALabel.Text = "A: No file selected";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1428, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // Id
            // 
            this.Id.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Width = 128;
            // 
            // Type
            // 
            this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Type.DataPropertyName = "Type";
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Width = 129;
            // 
            // Object_Name
            // 
            this.Object_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Object_Name.DataPropertyName = "Name";
            this.Object_Name.HeaderText = "Name";
            this.Object_Name.Name = "Object_Name";
            this.Object_Name.ReadOnly = true;
            this.Object_Name.Width = 5;
            // 
            // StringDateA
            // 
            this.StringDateA.DataPropertyName = "StringDateA";
            this.StringDateA.HeaderText = "Date Object A";
            this.StringDateA.Name = "StringDateA";
            this.StringDateA.ReadOnly = true;
            // 
            // StringTimeA
            // 
            this.StringTimeA.DataPropertyName = "StringTimeA";
            this.StringTimeA.HeaderText = "Time Object A";
            this.StringTimeA.Name = "StringTimeA";
            this.StringTimeA.ReadOnly = true;
            // 
            // VersionListA
            // 
            this.VersionListA.DataPropertyName = "VersionListA";
            this.VersionListA.HeaderText = "Version List A";
            this.VersionListA.Name = "VersionListA";
            this.VersionListA.ReadOnly = true;
            // 
            // NoOfLinesA
            // 
            this.NoOfLinesA.DataPropertyName = "NoOfLinesA";
            this.NoOfLinesA.HeaderText = "No Of Lines Object A";
            this.NoOfLinesA.Name = "NoOfLinesA";
            this.NoOfLinesA.ReadOnly = true;
            // 
            // StringDateB
            // 
            this.StringDateB.DataPropertyName = "StringDateB";
            this.StringDateB.HeaderText = "Date Object B";
            this.StringDateB.Name = "StringDateB";
            this.StringDateB.ReadOnly = true;
            // 
            // StringTimeB
            // 
            this.StringTimeB.DataPropertyName = "StringTimeB";
            this.StringTimeB.HeaderText = "Time Object B";
            this.StringTimeB.Name = "StringTimeB";
            this.StringTimeB.ReadOnly = true;
            // 
            // VersionListB
            // 
            this.VersionListB.DataPropertyName = "VersionListB";
            this.VersionListB.HeaderText = "Version List B";
            this.VersionListB.Name = "VersionListB";
            this.VersionListB.ReadOnly = true;
            // 
            // NoOfLinesB
            // 
            this.NoOfLinesB.DataPropertyName = "NoOfLinesB";
            this.NoOfLinesB.HeaderText = "No Of Lines Object B";
            this.NoOfLinesB.Name = "NoOfLinesB";
            this.NoOfLinesB.ReadOnly = true;
            // 
            // IsEqual
            // 
            this.IsEqual.DataPropertyName = "Equal";
            this.IsEqual.HeaderText = "Objects Are Equal";
            this.IsEqual.Name = "IsEqual";
            this.IsEqual.ReadOnly = true;
            // 
            // Difference
            // 
            this.Difference.DataPropertyName = "Difference";
            this.Difference.HeaderText = "Difference Comment";
            this.Difference.Name = "Difference";
            this.Difference.ReadOnly = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1428, 593);
            this.Controls.Add(this.comparedDataGridView);
            this.Controls.Add(this.panelToolbar);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Object Compare";
            this.Load += new System.EventHandler(this.FormTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.comparedDataGridView)).EndInit();
            this.panelToolbar.ResumeLayout(false);
            this.panelToolbar.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView comparedDataGridView;
        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.Label fileBLabel;
        private System.Windows.Forms.Label fileALabel;
        private System.Windows.Forms.CheckBox notEqualCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Object_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn StringDateA;
        private System.Windows.Forms.DataGridViewTextBoxColumn StringTimeA;
        private System.Windows.Forms.DataGridViewTextBoxColumn VersionListA;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoOfLinesA;
        private System.Windows.Forms.DataGridViewTextBoxColumn StringDateB;
        private System.Windows.Forms.DataGridViewTextBoxColumn StringTimeB;
        private System.Windows.Forms.DataGridViewTextBoxColumn VersionListB;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoOfLinesB;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsEqual;
        private System.Windows.Forms.DataGridViewTextBoxColumn Difference;
    }
}

