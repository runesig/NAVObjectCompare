using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using NAVObjectCompare;

namespace NAVObjectCompareTest
{
    public partial class FormTest : Form
    {
        Compare _compare = null;

        public FormTest()
        {
            InitializeComponent();
        }

        private void FormTest_Load(object sender, EventArgs e)
        {
            _compare = new Compare(@"C:\temp\Objects\AllObjectsTEST.txt", @"C:\temp\Objects\AllObjectsSandbox.txt");
            _compare.RunCompare();

            comparedDataGridView.AutoGenerateColumns = false;
            comparedDataGridView.DataSource = _compare.GetList();
        }

        private void comparedDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            OpenEditor(e.RowIndex);
        }

        private void comparedDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dgvr in comparedDataGridView.Rows)
            {
                DataRowView drv = dgvr.DataBoundItem as DataRowView;
                ObjectsCompared objectCompared = (ObjectsCompared)dgvr.DataBoundItem;
                if (objectCompared != null)
                {

                    if (!objectCompared.Equal)
                        dgvr.DefaultCellStyle.BackColor = System.Drawing.Color.MistyRose;

                }
            }
        }

        // Private
        private void OpenEditor(int rowIndex)
        {
            string filePathEditor = ConfigurationManager.AppSettings["BeyondComparePath"];

            ObjectsCompared objectCompared = _compare.GetList()[rowIndex];

            Editor editor = new Editor(filePathEditor);
            editor.ObjectsA = _compare.NavObjectsA;
            editor.ObjectsB = _compare.NavObjectsB;

            editor.OpenEditor(objectCompared);
        }
    }
}
