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
using NAVObjectCompare.Compare;
using NAVObjectCompare.Models;
using NAVObjectCompareWinClient.Helpers;
using NAVObjectCompare.Editor;

namespace NAVObjectCompareWinClient
{
    public partial class FormMain : Form
    {
        Editor _editor = null;
        ObjectCompare _compare = null;

        #region FormEvents

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            InitApplication();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_compare == null)
                return;

            if(_compare.IsEditedA() || _compare.IsEditedB())
            {
                var window = MessageBox.Show(
                  "You have done changes to the objects that have not been exported. Are you sure you want to close the application?",
                  "Close Application",
                  MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                e.Cancel = (window == DialogResult.No);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog();
        }

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

        private void filterTextBox_TextChanged(object sender, EventArgs e)
        {
            SetRowFilters();
        }

        private void fieldFilterComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            SetRowFilters();
        }

        private void showComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetRowFilters();
        }

        private void saveAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string initFilename = string.Format("{0}_edited.txt", Path.GetFileNameWithoutExtension(_compare.CompareFilePathA));

            OpenSaveFileDialog(_compare.NavObjectsA, initFilename, "A");
        }

        private void saveBFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string initFilename = string.Format("{0}_edited.txt", Path.GetFileNameWithoutExtension(_compare.CompareFilePathB));

            OpenSaveFileDialog(_compare.NavObjectsB, initFilename, "B");
        }

        private void _editor_OnReCompareObject(object source, EditorEventArgs e)
        {
            try
            {
                _compare.FindDifferencesA(e.NavObject.InternalId);
                _compare.FindDifferencesB(e.NavObject.InternalId);

                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
   
        private void gridToExcelToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ExcelHelper.ExportDataGridToExcel(comparedDataGridView);
        }

        #endregion FormEvents

        private void InitApplication()
        {
            // Sewt Lables
            SetFileNameLabels(string.Empty, string.Empty);

            // Editor
            string filePathEditor = ConfigurationManager.AppSettings["BeyondComparePath"];
            _editor = new Editor(filePathEditor);
            _editor.OnReCompareObject += _editor_OnReCompareObject;

            AddFilterFieldsComboBox();
            AddShowItemsComboBox();
        }

        #region ComboBoxes
        private void AddFilterFieldsComboBox()
        {
            foreach (DataGridViewColumn column in comparedDataGridView.Columns)
            {
                AddComboBoxItem(fieldFilterComboBox, column.HeaderText, column.DataPropertyName);
            }
        }

        private void AddShowItemsComboBox()
        {
            AddComboBoxItem(showComboBox, "Show all", "ALL");
            AddComboBoxItem(showComboBox, "Show all equal", "ALLEQUAL");
            AddComboBoxItem(showComboBox, "Show all non equal", "ALLNONEQUAL");
            AddComboBoxItem(showComboBox, "Show only date,time differences", "OBJECTPROPERTIES");
            AddComboBoxItem(showComboBox, "Show code differences", "CODEDIFF");
        }

        private void AddComboBoxItem(ComboBox combo, string text, string value)
        {
            ComboboxItem item = new ComboboxItem();
            item.Text = text;
            item.Value = value;
            combo.Items.Add(item);
        }

        #endregion ComboBoxes

        #region FileHandling
        private void OpenEditor(string internalId)
        {
            try
            {
                if (!string.IsNullOrEmpty(internalId))
                {
                    NavObjectsCompared objectCompared = null;
                    foreach (NavObjectsCompared searchCompared in _compare.GetList())
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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

        private void OpenSaveFileDialog(Dictionary<string, NavObject> objects, string initFilename, string tag)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = string.Format("Export {0}: NAV Object File(s)", tag);
            saveDialog.Filter = "Txt files|*.txt";
            saveDialog.FileName = initFilename;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ExportObjects(objects, saveDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        #endregion FileHandling

        #region DataGridMethods
        private void CompareAndFillGrid(string filePathA, string filePathB)
        {
            // Testing Start
            Stopwatch sw = new Stopwatch();
            sw.Start();
            // Testing Stop

            // Set Cursor
            Cursor.Current = Cursors.WaitCursor;

            RunComparison(filePathA, filePathB);

            PopulateGrid();

            SetFileNameLabels(_compare.CompareFilePathA, _compare.CompareFilePathB);

            Cursor.Current = Cursors.Default;

            // Testing Start
            sw.Stop();
            Console.WriteLine("Elapsed={0}", sw.Elapsed);
            // Testing Stop
        }

        private void RunComparison(string filePathA, string filePathB)
        {
            if (_compare == null)
            {
                _compare = new ObjectCompare(); // Start New Compare
                _compare.CompareFilePathA = filePathA;
                _compare.CompareFilePathB = filePathB;
            }
            else
            {
                // A Comparison have been done previously Check what to do
                if ((string.IsNullOrEmpty(filePathB)) && (_compare.NavObjectsA.Count > 0) && (_compare.NavObjectsB.Count > 0))
                {
                    _compare = new ObjectCompare(); // Start a new Compare
                    _compare.CompareFilePathA = filePathA;
                }
                else if ((string.IsNullOrEmpty(filePathB)) && (_compare.NavObjectsA.Count > 0) && (_compare.NavObjectsB.Count == 0)) // Compare has been done only for file A then add B
                {
                    _compare.CompareFilePathB = filePathA; // Set the A file to path B to add the new one
                }
            }

            _compare.RunCompare();
        }

        private void PopulateGrid()
        {
            // Populate list
            DataTable ListAsDataTable = DataTableHelper.BuildDataTable<NavObjectsCompared>(_compare.GetList());
            DataView ListAsDataView = ListAsDataTable.DefaultView;

            var bindingList = new BindingList<NavObjectsCompared>(_compare.GetList());
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

        #endregion DataGridMethods

        #region Filters
        private void SetRowFilters()
        {
            if (comparedDataGridView.DataSource == null)
                return;

            (comparedDataGridView.DataSource as DataView).RowFilter = CreateRowFilter();

            int count = (comparedDataGridView.DataSource as DataView).Count;
            SetStatus(count);
        }

        private string CreateRowFilter()
        {
            string rowFilter = string.Empty;

            rowFilter = CreateEqualFilter();
            rowFilter = CreateComboRowFilter(rowFilter);

            return rowFilter;
        }

        private string CreateEqualFilter()
        {
            string rowFilter = string.Empty;

            ComboboxItem selectedItem = (ComboboxItem)showComboBox.SelectedItem;
            if (selectedItem == null)
                return string.Empty;

            switch(selectedItem.Value)
            {
                case "ALL":
                    rowFilter = string.Empty;
                    break;
                case "ALLEQUAL":
                    rowFilter = string.Format("Equal = {0}", "True");
                    break;
                case "ALLNONEQUAL":
                    rowFilter = string.Format("Equal = {0}", "False");
                    break;
                case "OBJECTPROPERTIES":
                    rowFilter = string.Format("Equal = {0} AND CodeEqual = {1} AND ObjectPropertiesEqual = {2}", "False", "True", "False");
                    break;
                case "CODEDIFF":
                    rowFilter = string.Format("Equal = {0} AND CodeEqual = {1}", "False", "False");
                    break;                   
            }

            return rowFilter;
        }

        private string CreateComboRowFilter(string existingFilter)
        {
            string rowFilter = existingFilter;

            ComboboxItem item = (ComboboxItem)fieldFilterComboBox.SelectedItem;
            string comboFilter = string.Empty;

            if (!string.IsNullOrEmpty(filterTextBox.Text) && (item != null))
            {
             if(!string.IsNullOrEmpty(existingFilter))
                comboFilter = string.Format(" AND CONVERT({0}, System.String) LIKE '%{1}%'", item.Value, filterTextBox.Text);
             else
                comboFilter = string.Format("CONVERT({0}, System.String) LIKE '%{1}%'", item.Value, filterTextBox.Text);

                rowFilter += comboFilter;
            }

            return rowFilter;
        }

        #endregion Filters

        private void SetFileNameLabels(string filePathA, string filePathB)
        {
            if (!string.IsNullOrEmpty(filePathA))
            {
                fileALabel.Text = string.Format("File A: {0}", Path.GetFileName(filePathA));
                fileALabel.BackColor = DataGridViewHelper.GetColorA;
            }
            else
                fileALabel.Text = string.Format("File A Not Set");

            if (!string.IsNullOrEmpty(filePathB))
            {
                fileBLabel.Text = string.Format("File B: {0}", Path.GetFileName(filePathB));
                fileBLabel.BackColor = DataGridViewHelper.GetColorB;
            }
            else
                fileBLabel.Text = string.Format("File B Not Set");
        }

        private void SetStatus(int numberOfObjects)
        {
            toolStripStatusLabel.Text = string.Format("{0} object(s)", numberOfObjects);
        }

        private void ExportObjects(Dictionary<string, NavObject> objects, string filePath)
        {

            // Set Cursor
            Cursor.Current = Cursors.WaitCursor;

            ObjectFile.Export(objects, filePath);

            // Set Cursor
            Cursor.Current = Cursors.Default;
        }
    }
}
