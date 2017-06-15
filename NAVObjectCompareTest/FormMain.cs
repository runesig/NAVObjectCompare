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

            AddFilterFieldsComboBox();
            AddShowItemsComboBox();
        }

        private void AddFilterFieldsComboBox()
        {
            foreach (DataGridViewColumn column in comparedDataGridView.Columns)
            {
                AddItem(fieldFilterComboBox, column.HeaderText, column.DataPropertyName);
            }
        }

        private void AddShowItemsComboBox()
        {
            AddItem(showComboBox, "Show all", "ALL");
            AddItem(showComboBox, "Show all equal", "ALLEQUAL");
            AddItem(showComboBox, "Show all non equal", "ALLNONEQUAL");
            AddItem(showComboBox, "Show only date,time differences", "OBJECTPROPERTIES");
            AddItem(showComboBox, "Show code differences", "CODEDIFF");
        }

        private void AddItem(ComboBox combo, string text, string value)
        {
            ComboboxItem item = new ComboboxItem();
            item.Text = text;
            item.Value = value;
            combo.Items.Add(item);
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

        #endregion DataGridEvents

        private void SetRowFilters()
        {
            if (comparedDataGridView.DataSource == null)
                return;

            (comparedDataGridView.DataSource as DataView).RowFilter = CreateRowFilter();

            int count = (comparedDataGridView.DataSource as DataView).Count;
            SetStatus(count);
        }

        private void SetStatus(int numberOfObjects)
        {
            toolStripStatusLabel.Text = string.Format("{0} object(s)", numberOfObjects);
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

        private void exportAFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string initFilename = string.Format("{0}_edited.txt", Path.GetFileNameWithoutExtension(_compare.FilenameA));

            ExportFileDialog(_compare.NavObjectsA, initFilename, "A");
        }

        private void exportBFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string initFilename = string.Format("{0}_edited.txt", Path.GetFileNameWithoutExtension(_compare.FilenameB));

            ExportFileDialog(_compare.NavObjectsB, initFilename, "B");
        }

        private void ExportFileDialog(Dictionary<string, NavObject> objects, string initFilename, string tag)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = string.Format("Export {0}: NAV Object File(s)", tag);
            saveDialog.Filter = "Txt files|*.txt";
            saveDialog.FileName = initFilename;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Export(objects, saveDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void Export(Dictionary<string, NavObject> objects, string filePath)
        {

            // Set Cursor
            Cursor.Current = Cursors.WaitCursor;

            ObjectFile.Export(objects, filePath);

            // Set Cursor
            Cursor.Current = Cursors.Default;
        }

    }
}
