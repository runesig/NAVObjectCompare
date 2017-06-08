namespace NAVObjectCompareTest
{
    partial class FormTest
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
            this.StringDateB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StringTimeB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VersionListB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoOfLinesA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NoOfLinesB = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsEqual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Difference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.comparedDataGridView)).BeginInit();
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
            this.StringDateB,
            this.StringTimeB,
            this.VersionListB,
            this.NoOfLinesA,
            this.NoOfLinesB,
            this.IsEqual,
            this.Difference});
            this.comparedDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comparedDataGridView.Location = new System.Drawing.Point(0, 0);
            this.comparedDataGridView.Name = "comparedDataGridView";
            this.comparedDataGridView.Size = new System.Drawing.Size(1428, 593);
            this.comparedDataGridView.TabIndex = 0;
            this.comparedDataGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.comparedDataGridView_CellMouseDoubleClick);
            this.comparedDataGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.comparedDataGridView_DataBindingComplete);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "Type";
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // Object_Name
            // 
            this.Object_Name.DataPropertyName = "Name";
            this.Object_Name.HeaderText = "Name";
            this.Object_Name.Name = "Object_Name";
            this.Object_Name.ReadOnly = true;
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
            // NoOfLinesA
            // 
            this.NoOfLinesA.DataPropertyName = "NoOfLinesA";
            this.NoOfLinesA.HeaderText = "No Of Lines Object A";
            this.NoOfLinesA.Name = "NoOfLinesA";
            this.NoOfLinesA.ReadOnly = true;
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
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1428, 593);
            this.Controls.Add(this.comparedDataGridView);
            this.Name = "FormTest";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.comparedDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView comparedDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Object_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn StringDateA;
        private System.Windows.Forms.DataGridViewTextBoxColumn StringTimeA;
        private System.Windows.Forms.DataGridViewTextBoxColumn VersionListA;
        private System.Windows.Forms.DataGridViewTextBoxColumn StringDateB;
        private System.Windows.Forms.DataGridViewTextBoxColumn StringTimeB;
        private System.Windows.Forms.DataGridViewTextBoxColumn VersionListB;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoOfLinesA;
        private System.Windows.Forms.DataGridViewTextBoxColumn NoOfLinesB;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsEqual;
        private System.Windows.Forms.DataGridViewTextBoxColumn Difference;
    }
}

