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
using System.Configuration;
using System.Windows.Threading;

namespace NAVObjectCompareWinClient
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
            InitApplication();
        }

        private void InitApplication()
        {
            // Sewt Lables
            // SetFileNameLabels(string.Empty, string.Empty);

            // Progress Bar
            this.processProgessBar.Visibility = Visibility.Collapsed;
            this.processProgessBar.Minimum = 0;
            this.processProgessBar.Maximum = 100;
            this.processProgessBar.Value = 0;

            // Editor
            string filePathEditor = ConfigurationManager.AppSettings["BeyondComparePath"];
            _editor = new Editor(filePathEditor);
            _editor.OnReCompareObject += _editor_OnReCompareObject;

            // AddFilterFieldsComboBox();
            // AddShowItemsComboBox();

            // comparedDataGrid.RowStyle.Triggers.Add(new DataTrigger() { Binding = new Binding() { Source = "{Binding Equal}" }, Value = "False", Setters.Add(new Setter() { Property = BackgroundProperty, Value = Color.FromRgb(0,0,0)) } });
        }



        async private void CompareAndFillGrid(string filePathA, string filePathB)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                await HideOrShowProgressBarAsync();

                RunComparison(filePathA, filePathB);

                PopulateGrid();

                SetSourceLabels(filePathA, filePathB);

                await HideOrShowProgressBarAsync();
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
            finally
            {
                Mouse.OverrideCursor = null;
            }
        }

        private void _compare_OnCompared(object source, CompareEventArgs e)
        {
            processProgessBar.Dispatcher.Invoke(() => processProgessBar.Value = e.PercentageDone, DispatcherPriority.Background);
        }

        private void _editor_OnReCompareObject(object source, EditorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RunComparison(string filePathA, string filePathB)
        {
            if (_compare == null)
            {
                _compare = new Compare(); // Start New Compare
                _compare.CompareFilePathA = filePathA;
                _compare.CompareFilePathB = filePathB;
                _compare.OnCompared += _compare_OnCompared;
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

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGridRow dataGridRow = (DataGridRow)e.Source;
                DataRowView dataRowView = (DataRowView)dataGridRow.Item;

                string internalId = (string)dataRowView["InternalId"];
                OpenEditor(internalId);
            }
            catch(Exception ex)
            {
                MessageHelper.ShowError(ex);
            }

        }

        #region GUI

        private void openMenu_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog();
        }

        private void exitMenu_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Goodbye");
        }

        private void SetSourceLabels(string sourceA, string sourceB)
        {
            this.statusSourceA.Text = sourceA;
            this.statusSourceB.Text = sourceB;
        }

        #endregion GUI

        #region FileHandling

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
                    MessageHelper.ShowError(ex);
                }
            }
        }

        private void OpenSaveFileDialog(Dictionary<string, NavObject> objects, string initFilename, string tag)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = string.Format("Export {0}: NAV Object File(s)", tag);
            saveDialog.Filter = "Txt files|*.txt";
            saveDialog.FileName = initFilename;

            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    // ExportObjects(objects, saveDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowError(ex);
                }
            }
        }

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
                MessageHelper.ShowError(ex);
            }
        }

        private async Task HideOrShowProgressBarAsync()
        {
            if(processProgessBar.Visibility == Visibility.Visible)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false);
                this.Dispatcher.Invoke(new Action(() => { processProgessBar.Visibility = Visibility.Collapsed; }));
            }
            else
            {
                await Task.Delay(TimeSpan.FromMilliseconds(2)).ConfigureAwait(false);
                this.Dispatcher.Invoke(new Action(() => { processProgessBar.Visibility = Visibility.Visible; }));
            }
        }

        #endregion FileHandling
    }
}
