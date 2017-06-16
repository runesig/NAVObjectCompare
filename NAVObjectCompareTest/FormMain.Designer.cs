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
            this.CodeEqual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ObjectPropertiesEqual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Difference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelToolbar = new System.Windows.Forms.Panel();
            this.showComboBox = new System.Windows.Forms.ComboBox();
            this.filePanel = new System.Windows.Forms.Panel();
            this.fileBLabel = new System.Windows.Forms.Label();
            this.fileALabel = new System.Windows.Forms.Label();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.fieldFilterComboBox = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportBFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.comparedDataGridView)).BeginInit();
            this.panelToolbar.SuspendLayout();
            this.filePanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip.SuspendLayout();
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
            this.CodeEqual,
            this.ObjectPropertiesEqual,
            this.Difference});
            this.comparedDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comparedDataGridView.Location = new System.Drawing.Point(0, 89);
            this.comparedDataGridView.Name = "comparedDataGridView";
            this.comparedDataGridView.Size = new System.Drawing.Size(1428, 504);
            this.comparedDataGridView.TabIndex = 0;
            this.comparedDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.comparedDataGridView_CellMouseDoubleClick);
            this.comparedDataGridView.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.comparedDataGridView_CellPainting);
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
            // CodeEqual
            // 
            this.CodeEqual.DataPropertyName = "CodeEqual";
            this.CodeEqual.HeaderText = "Code is Equal";
            this.CodeEqual.Name = "CodeEqual";
            // 
            // ObjectPropertiesEqual
            // 
            this.ObjectPropertiesEqual.DataPropertyName = "ObjectPropertiesEqual";
            this.ObjectPropertiesEqual.HeaderText = "Date and Time is Equal";
            this.ObjectPropertiesEqual.Name = "ObjectPropertiesEqual";
            // 
            // Difference
            // 
            this.Difference.DataPropertyName = "Difference";
            this.Difference.HeaderText = "Difference Comment";
            this.Difference.Name = "Difference";
            this.Difference.ReadOnly = true;
            // 
            // panelToolbar
            // 
            this.panelToolbar.Controls.Add(this.showComboBox);
            this.panelToolbar.Controls.Add(this.filePanel);
            this.panelToolbar.Controls.Add(this.filterTextBox);
            this.panelToolbar.Controls.Add(this.fieldFilterComboBox);
            this.panelToolbar.Controls.Add(this.menuStrip1);
            this.panelToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolbar.Location = new System.Drawing.Point(0, 0);
            this.panelToolbar.Name = "panelToolbar";
            this.panelToolbar.Size = new System.Drawing.Size(1428, 89);
            this.panelToolbar.TabIndex = 1;
            // 
            // showComboBox
            // 
            this.showComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.showComboBox.FormattingEnabled = true;
            this.showComboBox.Location = new System.Drawing.Point(13, 26);
            this.showComboBox.Name = "showComboBox";
            this.showComboBox.Size = new System.Drawing.Size(364, 21);
            this.showComboBox.TabIndex = 7;
            this.showComboBox.SelectedIndexChanged += new System.EventHandler(this.showComboBox_SelectedIndexChanged);
            // 
            // filePanel
            // 
            this.filePanel.Controls.Add(this.fileBLabel);
            this.filePanel.Controls.Add(this.fileALabel);
            this.filePanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.filePanel.Location = new System.Drawing.Point(1119, 24);
            this.filePanel.Name = "filePanel";
            this.filePanel.Size = new System.Drawing.Size(309, 65);
            this.filePanel.TabIndex = 6;
            // 
            // fileBLabel
            // 
            this.fileBLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fileBLabel.AutoSize = true;
            this.fileBLabel.Location = new System.Drawing.Point(7, 37);
            this.fileBLabel.Name = "fileBLabel";
            this.fileBLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fileBLabel.Size = new System.Drawing.Size(93, 13);
            this.fileBLabel.TabIndex = 3;
            this.fileBLabel.Text = "B: No file selected";
            this.fileBLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // fileALabel
            // 
            this.fileALabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.fileALabel.AutoSize = true;
            this.fileALabel.Location = new System.Drawing.Point(8, 10);
            this.fileALabel.Name = "fileALabel";
            this.fileALabel.Size = new System.Drawing.Size(93, 13);
            this.fileALabel.TabIndex = 2;
            this.fileALabel.Text = "A: No file selected";
            this.fileALabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // filterTextBox
            // 
            this.filterTextBox.Location = new System.Drawing.Point(147, 58);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(230, 20);
            this.filterTextBox.TabIndex = 5;
            this.filterTextBox.TextChanged += new System.EventHandler(this.filterTextBox_TextChanged);
            // 
            // fieldFilterComboBox
            // 
            this.fieldFilterComboBox.FormattingEnabled = true;
            this.fieldFilterComboBox.Location = new System.Drawing.Point(13, 57);
            this.fieldFilterComboBox.Name = "fieldFilterComboBox";
            this.fieldFilterComboBox.Size = new System.Drawing.Size(121, 21);
            this.fieldFilterComboBox.TabIndex = 4;
            this.fieldFilterComboBox.SelectedValueChanged += new System.EventHandler(this.fieldFilterComboBox_SelectedValueChanged);
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
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportAFileToolStripMenuItem,
            this.exportBFileToolStripMenuItem});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.saveToolStripMenuItem.Text = "Export";
            // 
            // exportAFileToolStripMenuItem
            // 
            this.exportAFileToolStripMenuItem.Name = "exportAFileToolStripMenuItem";
            this.exportAFileToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.exportAFileToolStripMenuItem.Text = "Export A File";
            this.exportAFileToolStripMenuItem.Click += new System.EventHandler(this.exportAFileToolStripMenuItem_Click);
            // 
            // exportBFileToolStripMenuItem
            // 
            this.exportBFileToolStripMenuItem.Name = "exportBFileToolStripMenuItem";
            this.exportBFileToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.exportBFileToolStripMenuItem.Text = "Export B File";
            this.exportBFileToolStripMenuItem.Click += new System.EventHandler(this.exportBFileToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 571);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1428, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1428, 593);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.comparedDataGridView);
            this.Controls.Add(this.panelToolbar);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Object Compare";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.comparedDataGridView)).EndInit();
            this.panelToolbar.ResumeLayout(false);
            this.panelToolbar.PerformLayout();
            this.filePanel.ResumeLayout(false);
            this.filePanel.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView comparedDataGridView;
        private System.Windows.Forms.Panel panelToolbar;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ComboBox fieldFilterComboBox;
        private System.Windows.Forms.Panel filePanel;
        private System.Windows.Forms.Label fileALabel;
        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.Label fileBLabel;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ComboBox showComboBox;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn CodeEqual;
        private System.Windows.Forms.DataGridViewTextBoxColumn ObjectPropertiesEqual;
        private System.Windows.Forms.DataGridViewTextBoxColumn Difference;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem exportAFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportBFileToolStripMenuItem;
    }
}

