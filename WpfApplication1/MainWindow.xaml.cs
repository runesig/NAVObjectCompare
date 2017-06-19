using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using Microsoft.Win32;
using NAVObjectCompare;
using NAVObjectCompare.Models;
using NAVObjectCompareWinClient.Helpers;
using NAVObjectCompare.Editor;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Editor _editor = null;
        Compare _compare = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void openMenu_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog();
        }

        private void OpenFileDialog()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Open NAV Object File(s)";
            openDialog.Filter = "Txt files|*.txt";
            openDialog.Multiselect = true;

            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
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
                    {
                        CompareAndFillGrid(openDialog.FileName, string.Empty);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void CompareAndFillGrid(string filePathA, string filePathB)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                RunComparison(filePathA, filePathB);

                PopulateGrid();

                // SetFileNameLabels(_compare.CompareFilePathA, _compare.CompareFilePathB);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void RunComparison(string filePathA, string filePathB)
        {
            if (_compare == null)
            {
                _compare = new Compare(); // Start New Compare
                _compare.CompareFilePathA = filePathA;
                _compare.CompareFilePathB = filePathB;
            }
            else
            {
                // A Comparison have been done previously Check what to do
                if ((string.IsNullOrEmpty(filePathB)) && (_compare.NavObjectsA.Count > 0) && (_compare.NavObjectsB.Count > 0))
                {
                    _compare = new Compare(); // Start a new Compare
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
            DataTable listAsDataTable = DataTableHelper.BuildDataTable<NavObjectsCompared>(_compare.GetList());
            DataView listAsDataView = listAsDataTable.DefaultView;

            // comparedDataGrid.AutoGenerateColumns = false;
            comparedDataGrid.ItemsSource = listAsDataView;

            //var bindingList = new BindingList<NavObjectsCompared>(_compare.GetList());
            //var source = new BindingSource(bindingList, null);

            //if (InvokeRequired)
            //{
            //    // after we've done all the processing, 
            //    this.Invoke(new MethodInvoker(delegate
            //    {

            //        comparedDataGridView.AutoGenerateColumns = false;
            //        comparedDataGridView.DataSource = ListAsDataView;

            //        SetRowFilters();
            //    }));
            //    return;
            //}
            //else
            //{
            //    comparedDataGridView.AutoGenerateColumns = false;
            //    comparedDataGridView.DataSource = ListAsDataView;

            //    SetRowFilters();
            //}

            SetRowFilters();
        }

        #region Filters
        private void SetRowFilters()
        {
            if (comparedDataGrid.ItemsSource == null)
                return;

            (comparedDataGrid.ItemsSource as DataView).RowFilter = CreateRowFilter();

            //int count = (comparedDataGridView.DataSource as DataView).Count;
            //SetStatus(count);
        }

        private string CreateRowFilter()
        {
            string rowFilter = string.Empty;

            //rowFilter = CreateEqualFilter();
            //rowFilter = CreateComboRowFilter(rowFilter);

            // return string.Format("Equal = {0}", "False");

            // 

            return rowFilter;
        }

        //private string CreateEqualFilter()
        //{
        //    string rowFilter = string.Empty;

        //    ComboboxItem selectedItem = (ComboboxItem)showComboBox.SelectedItem;
        //    if (selectedItem == null)
        //        return string.Empty;

        //    switch (selectedItem.Value)
        //    {
        //        case "ALL":
        //            rowFilter = string.Empty;
        //            break;
        //        case "ALLEQUAL":
        //            rowFilter = string.Format("Equal = {0}", "True");
        //            break;
        //        case "ALLNONEQUAL":
        //            rowFilter = string.Format("Equal = {0}", "False");
        //            break;
        //        case "OBJECTPROPERTIES":
        //            rowFilter = string.Format("Equal = {0} AND CodeEqual = {1} AND ObjectPropertiesEqual = {2}", "False", "True", "False");
        //            break;
        //        case "CODEDIFF":
        //            rowFilter = string.Format("Equal = {0} AND CodeEqual = {1}", "False", "False");
        //            break;
        //    }

        //    return rowFilter;
        //}

        //private string CreateComboRowFilter(string existingFilter)
        //{
        //    string rowFilter = existingFilter;

        //    ComboboxItem item = (ComboboxItem)fieldFilterComboBox.SelectedItem;
        //    string comboFilter = string.Empty;

        //    if (!string.IsNullOrEmpty(filterTextBox.Text) && (item != null))
        //    {
        //        if (!string.IsNullOrEmpty(existingFilter))
        //            comboFilter = string.Format(" AND CONVERT({0}, System.String) LIKE '%{1}%'", item.Value, filterTextBox.Text);
        //        else
        //            comboFilter = string.Format("CONVERT({0}, System.String) LIKE '%{1}%'", item.Value, filterTextBox.Text);

        //        rowFilter += comboFilter;
        //    }

        //    return rowFilter;
        //}

        #endregion Filters
    }
}
