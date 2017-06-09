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
using NAVObjectCompare;

namespace NAVObjectCompareTest
{
    public partial class FormMain : Form
    {
        Compare _compare = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            //CompareAndFillGrid((@"C:\temp\Objects\ObjectCompareTEST.txt", @"C:\temp\Objects\ObjectCompareSandbox.txt");
            //CompareAndFillGrid((@"C:\temp\Objects\AllObjectsTEST.txt", @"C:\temp\Objects\AllObjectsSandbox.txt");
            //CompareAndFillGrid((@"C:\temp\Objects\VehicleAreaPreProd090617.txt", @"C:\temp\Objects\VehicleAreaSandbox090617.txt");

            // CompareAndFillGrid(@"C:\temp\Objects\VehicleAreaPreProd090617.txt", @"C:\temp\Objects\VehicleAreaSandbox090617.txt");

            SetFileNameLabels(string.Empty, string.Empty);
        }

        private void comparedDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            OpenEditor(e.RowIndex);
        }

        private void comparedDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //foreach (DataGridViewRow dgvr in comparedDataGridView.Rows)
            //{
            //    DataRowView drv = dgvr.DataBoundItem as DataRowView;
            //    if (drv["Equal"].ToString() = "False")
            //    {

            //        if (!objectCompared.Equal)
            //            dgvr.DefaultCellStyle.BackColor = System.Drawing.Color.MistyRose;

            //    }
            //}

            //foreach (DataGridViewRow dgvr in comparedDataGridView.Rows)
            //{
            //    DataRowView drv = dgvr.DataBoundItem as DataRowView;
            //    ObjectsCompared objectCompared = (ObjectsCompared)dgvr.DataBoundItem;
            //    if (objectCompared != null)
            //    {

            //        if (!objectCompared.Equal)
            //            dgvr.DefaultCellStyle.BackColor = System.Drawing.Color.MistyRose;

            //    }
            //}
        }

        // Private
        private void OpenEditor(int rowIndex)
        {
            string filePathEditor = ConfigurationManager.AppSettings["BeyondComparePath"];

            if ((rowIndex > -1) && (_compare.GetList().Count > rowIndex))
            {

                ObjectsCompared objectCompared = _compare.GetList()[rowIndex];

                Editor editor = new Editor(filePathEditor);
                editor.ObjectsA = _compare.NavObjectsA;
                editor.ObjectsB = _compare.NavObjectsB;

                editor.OpenEditor(objectCompared);
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
        }

        private void CompareAndFillGrid(string filePathA, string filePathB)
        {
            _compare = new Compare(filePathA, filePathB);
            _compare.RunCompare();

            // populate list
            DataTable ListAsDataTable = BuildDataTable<ObjectsCompared>(_compare.GetList());
            DataView ListAsDataView = ListAsDataTable.DefaultView;

            var bindingList = new BindingList<ObjectsCompared>(_compare.GetList());
            var source = new BindingSource(bindingList, null);

            comparedDataGridView.AutoGenerateColumns = false;
            comparedDataGridView.DataSource = ListAsDataTable;

            SetFileNameLabels(filePathA, filePathB);
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

        public static DataTable BuildDataTable<T>(IList<T> lst)
        {
            DataTable tbl = CreateTable<T>();
            Type entType = typeof(T);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);

            foreach (T item in lst)
            {
                DataRow row = tbl.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item);
                }
                tbl.Rows.Add(row);
            }
            return tbl;
        }

        private static DataTable CreateTable<T>()
        {
            Type entType = typeof(T);

            DataTable tbl = new DataTable(entType.Name);

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entType);
            foreach (PropertyDescriptor prop in properties)
            {
                tbl.Columns.Add(prop.Name, prop.PropertyType);
            }
            return tbl;
        }

        private void notEqualCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (notEqualCheckBox.Checked)
                (comparedDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Format("Equal = {0}", "False");
            else
                (comparedDataGridView.DataSource as DataTable).DefaultView.RowFilter = string.Empty;
        }
    }
}
