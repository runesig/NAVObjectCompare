using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using System.Diagnostics;
using NAVObjectCompare;
using NAVObjectCompareWinClient.Helpers;
using NAVObjectCompareWinClient.FileNotification;

namespace NAVObjectCompareWinClient
{
    public partial class FormMain : Form
    {
        Editor _editor = null;
        Compare _compare = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            InitApplication();
        }

        private void InitApplication()
        {
            // Sewt Lables
            SetFileNameLabels(string.Empty, string.Empty);

            // Editor
            string filePathEditor = ConfigurationManager.AppSettings["BeyondComparePath"];
            _editor = new Editor(filePathEditor);
            _editor.OnReCompareObject += _editor_OnReCompareObject;
        }

        private void _editor_OnReCompareObject(object source, EditorEventArgs e)
        {
            try
            {
                _compare.FindDifferencesA(e.NavObject.InternalId);
                _compare.FindDifferencesB(e.NavObject.InternalId);

                PopulateGrid();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        // Private
        private void OpenEditor(string internalId)
        {
            if (!string.IsNullOrEmpty(internalId))
            {
                ObjectsCompared objectCompared = null;
                foreach (ObjectsCompared searchCompared in _compare.GetList())
                {
                    if (searchCompared.InternalId == internalId)
                        objectCompared = searchCompared;
                }

                if (objectCompared == null)
                    return;

                _editor.ObjectsA = _compare.NavObjectsA;
                _editor.ObjectsB = _compare.NavObjectsB;

                _editor.OpenEditor(objectCompared);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog();
        }

        private void OpenFileDialog()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Open NAV Object File(s)";
            openDialog.Filter = "Txt files|*.txt";
            openDialog.Multiselect = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (openDialog.FileNames.Length > 1)
                    {
                        // Get the two first ones
                        string filePathA = openDialog.FileNames[0];
                        string filePathB = openDialog.FileNames[1];

                        CompareAndFillGrid(filePathA, filePathB);
                    }
                    else
                        CompareAndFillGrid(openDialog.FileName, string.Empty);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void CompareAndFillGrid(string filePathA, string filePathB)
        {
            // Testing Start
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // Testing Stop

            // Set Cursor
            Cursor.Current = Cursors.WaitCursor;

            _compare = new Compare(filePathA, filePathB);
            _compare.RunCompare();

            PopulateGrid();

            SetFileNameLabels(filePathA, filePathB);

            Cursor.Current = Cursors.Default;

            // Testing Start
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);
            // Testing Stop
        }

        private void PopulateGrid()
        {
            // Populate list
            DataTable ListAsDataTable = DataTableHelper.BuildDataTable<ObjectsCompared>(_compare.GetList());
            DataView ListAsDataView = ListAsDataTable.DefaultView;

            var bindingList = new BindingList<ObjectsCompared>(_compare.GetList());
            var source = new BindingSource(bindingList, null);

            if (InvokeRequired)
            {
                // after we've done all the processing, 
                this.Invoke(new MethodInvoker(delegate {

                    comparedDataGridView.AutoGenerateColumns = false;
                    comparedDataGridView.DataSource = ListAsDataView;

                    SetRowFilters();
                }));
                return;
            }
            else
            {
                comparedDataGridView.AutoGenerateColumns = false;
                comparedDataGridView.DataSource = ListAsDataView;

                SetRowFilters();
            }
        }

        private void SetFileNameLabels(string filePathA, string filePathB)
        {
            if(!string.IsNullOrEmpty(filePathA))
                fileALabel.Text = string.Format("File A: {0}", Path.GetFileName(filePathA));
            else
                fileALabel.Text = string.Format("File A Not Set");

            if (!string.IsNullOrEmpty(filePathB))
                fileBLabel.Text = string.Format("File B: {0}", Path.GetFileName(filePathB));
            else
                fileBLabel.Text = string.Format("File B Not Set");
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void notEqualCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            SetRowFilters();
        }

        #region DataGridEvents

        private void comparedDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridViewHelper.SetRowColors(comparedDataGridView, e);
        }

        private void comparedDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // dgDocRow is DataGrid
            BindingManagerBase bm = this.comparedDataGridView.BindingContext[this.comparedDataGridView.DataSource, this.comparedDataGridView.DataMember];
            DataRow dr = ((DataRowView)bm.Current).Row;

            string internalId = (string)dr["InternalId"];
            OpenEditor(internalId);
        }

        private void comparedDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
        }
        #endregion DataGridEvents

        private void SetRowFilters()
        {
            if (comparedDataGridView.DataSource == null)
                return;

            if (notEqualCheckBox.Checked)
                (comparedDataGridView.DataSource as DataView).RowFilter = string.Format("Equal = {0}", "False");
            else
                (comparedDataGridView.DataSource as DataView).RowFilter = string.Empty;
        }

    }
}
