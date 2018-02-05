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
using System.Configuration;
using System.Windows.Threading;
using NAVObjectCompare.Compare;
using NAVObjectCompare.Models;
using NAVObjectCompareWinClient.Helpers;
using NAVObjectCompare.Editor;
using System.ComponentModel;
using NAVObjectCompareWinClient.Configurations;
using NAVObjectCompareWinClient.Model;
using NAVObjectCompare.ExportFinexe;
using System.Collections.ObjectModel;
using NAVObjectCompareWinClient.ViewModel;

namespace NAVObjectCompareWinClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CompareView : Window
    {
        Editor _editor = null;
        ObjectCompare _compare = null;
        private CompareViewModel _compareViewModel;

        public CompareView()
        {
            _compareViewModel = new CompareViewModel();
            DataContext = _compareViewModel;

            InitializeComponent();
            InitApplication();
        }

        private void InitApplication()
        {
            // Sewt Lables
            // SetFileNameLabels(string.Empty, string.Empty);

            // Progress Bar
            InitProgressBar();
            // Editor
            InitEditor();
            // Filters
            InitFilters();
        }

        private void InitEditor()
        {
            ConfigurationAppSettings appSettings = new ConfigurationAppSettings();

            _editor = new Editor(appSettings.EditorPath);
            _editor.OnReCompareObject += _editor_OnReCompareObject;
            _editor.OnEditorError += _editor_OnEditorError;
        }

        private void InitFilters()
        {
            RowFilters.AddFilterFieldsComboBoxItems(comparedDataGrid, ref fieldFilterComboBox);
            RowFilters.AddItemsShowComboBoxItems(ref showComboBox);

            SetGridRowCountStatus();
        }

        private void InitProgressBar()
        {
            ProcessProgessBar.Visibility = Visibility.Collapsed;
            ProcessProgessBar.Minimum = 0;
            ProcessProgessBar.Maximum = 100;
            ProcessProgessBar.Value = 0;
        }

        #region EventHandling

        private void _compare_OnCompared(int percentCompleted)
        {
            ProcessProgessBar.Dispatcher.Invoke(() => ProcessProgessBar.Value = percentCompleted, DispatcherPriority.Background);
        }

        private void _editor_OnReCompareObject(object source, EditorEventArgs e)
        {
            this.Dispatcher.Invoke(() => ReCompare(e.NavObject.InternalId), DispatcherPriority.Background);
        }

        private void _editor_OnEditorError(object source, EditorErrorEventArgs e)
        {
            this.Dispatcher.Invoke(() => MessageHelper.ShowError(e.Exception), DispatcherPriority.Normal);
        }

        async private void ReCompare(string internalId)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                await HideOrShowProgressBarAsync();

                await Task.Factory.StartNew(() =>
                {
                    _compare.FindDifferencesA(internalId);
                    _compare.FindDifferencesB(internalId);
                });

                PopulateGrid();

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

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            string filePathWorkspace = string.Empty;

            if (Dialogs.OpenWorkspace(ref filePathWorkspace))
                ReadWorkspaceFile(filePathWorkspace);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (Dialogs.SaveWorkspace(out string filePathWorkspace))
                SaveWorkspaceFile(filePathWorkspace);
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ImportSheet_Click(object sender, RoutedEventArgs e)
        {
            ImportSheetView importFiles = new ImportSheetView() { Owner = this };
            importFiles.ShowDialog();

            if ((importFiles.DialogResult.HasValue) && (importFiles.DialogResult.Value))
            {
                ImportSheetModel importSetupModelA = importFiles.SelectedImportSetupModelA;
                ImportSheetModel importSetupModelB = importFiles.SelectedImportSetupModelB;

                ExportAndCompare(importSetupModelA, importSetupModelB);
            }
        }

        private void ImportFiles_Click(object sender, RoutedEventArgs e)
        {
            string filePathA = string.Empty;
            string filePathB = string.Empty;

            if (Dialogs.OpenFile(true, ref filePathA, ref filePathB))
                CompareAndFillGrid(filePathA, filePathB);
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Menu Stop

        async private void ExportAndCompare(ImportSheetModel importSetupModelA, ImportSheetModel importSetupModelB)
        {
            string filePathA = string.Empty;
            string filePathB = string.Empty;

            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                await HideOrShowProgressBarAsync();

                ExportFinexeHelper finExeHelper = new ExportFinexeHelper();

                if (importSetupModelA.ImportType == ImportTypes.Server)
                {
                    ExportResult resultA = await finExeHelper.ExportObjectsFromFinExe(QueryExportTag.QueryExportA, importSetupModelA);

                    if (resultA.Success)
                        filePathA = resultA.ExportedObjectsPath;
                    else
                        MessageHelper.ShowError(resultA.Message);

                    if (!resultA.Success)
                        return;
                }
                else
                {
                    filePathA = importSetupModelA.ImportFileName;
                }

                // B Start
                if (importSetupModelB.ImportType == ImportTypes.Server)
                {
                    ExportResult resultB = await finExeHelper.ExportObjectsFromFinExe(QueryExportTag.QueryExportB, importSetupModelB);
                    if (resultB.Success)
                        filePathB = resultB.ExportedObjectsPath;
                    else
                        MessageHelper.ShowError(resultB.Message);

                    if (!resultB.Success)
                        return;
                }
                else
                {
                    filePathB = importSetupModelB.ImportFileName;
                }

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


            CompareAndFillGrid(filePathA, filePathB);
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGridRow dataGridRow = (DataGridRow)e.Source;
                NavObjectsCompared navObjectsCompared = (NavObjectsCompared)dataGridRow.Item;

                OpenEditor(navObjectsCompared.InternalId);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private void ShowComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetRowFilters();
        }

        private void FieldFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
                SetRowFilters();
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            var selectedValue = GetSelectedGridValue();

            if(selectedValue != null)
                Clipboard.SetText(selectedValue.ToString());

        }

        #endregion EventHandling


        #region Async Functions

        async private void CompareAndFillGrid(string filePathA, string filePathB)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                await HideOrShowProgressBarAsync();


                await Task.Factory.StartNew(() =>
                {
                    RunComparison(filePathA, filePathB);
                });

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

        private async Task HideOrShowProgressBarAsync()
        {
            if (ProcessProgessBar.Visibility == Visibility.Visible)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(500)).ConfigureAwait(false);
                this.Dispatcher.Invoke(new Action(() => 
                {
                    ProcessProgessBar.Visibility = Visibility.Collapsed;
                    GridRowCountTextBox.Visibility = Visibility.Visible;
                }));
            }
            else
            {
                await Task.Delay(TimeSpan.FromMilliseconds(2)).ConfigureAwait(false);
                this.Dispatcher.Invoke(new Action(() => 
                {
                    ProcessProgessBar.Visibility = Visibility.Visible;
                    GridRowCountTextBox.Visibility = Visibility.Collapsed;
                }));                
            }
        }

        #endregion Async Functions

        #region Private Functions

        private void RunComparison(string filePathA, string filePathB)
        {
            try
            {
                if (_compare == null)
                {
                    _compare = new ObjectCompare()
                    {
                        CompareFilePathA = filePathA,
                        CompareFilePathB = filePathB
                    }; // Start New Compare
                    ;
                    _compare.OnCompared += _compare_OnCompared;
                }
                else
                {
                    // A Comparison have been done previously Check what to do
                    if ((string.IsNullOrEmpty(filePathB)) && (_compare.NavObjectsA?.Count > 0) && (_compare.NavObjectsB?.Count > 0))
                    {
                        _compare = new ObjectCompare() { CompareFilePathA = filePathA }; // Start a new Compare
                        _compare.OnCompared += _compare_OnCompared;
                    }
                    else if ((string.IsNullOrEmpty(filePathB)) && (_compare.NavObjectsA?.Count > 0) && (_compare.NavObjectsB?.Count == 0)) // Compare has been done only for file A then add B
                    {
                        _compare.CompareFilePathB = filePathA; // Set the A file to path B to add the new one
                        _compare.OnCompared += _compare_OnCompared;
                    }
                    else if ((!string.IsNullOrEmpty(filePathA)) && (!string.IsNullOrEmpty(filePathB)))
                    {
                        // Totally New Compare
                        _compare = new ObjectCompare() { CompareFilePathA = filePathA, CompareFilePathB = filePathB };
                        _compare.OnCompared += _compare_OnCompared;
                    }
                }

                _compareViewModel.PrettyNameA = _compare.SourcePrettyNameA;
                _compareViewModel.PrettyNameB = _compare.SourcePrettyNameB;
                _compare.RunCompare();
            }
            catch(Exception ex)
            {
                MessageHelper.ShowError(ex);
            }            
        }

        private void PopulateGrid()
        {
            comparedDataGrid.AutoGenerateColumns = false;
            comparedDataGrid.ItemsSource = _compare.GetObservableCollection();

            SetRowFilters();
        }

        private void SetRowFilters()
        {
            if (_compare == null)
                return;

            ComboboxItem showComboBoxItem = (ComboboxItem)showComboBox.SelectedItem;
            ComboboxItem fieldFilterComboboxItem = (ComboboxItem)fieldFilterComboBox.SelectedItem;
            string fieldFilter = fieldFilterTextBox.Text;

            ObservableCollection<NavObjectsCompared> collection = _compare.GetObservableCollection();
            
            comparedDataGrid.ItemsSource = RowFilters.CreateFilter(collection, showComboBoxItem, fieldFilterComboboxItem, fieldFilter);

            SetGridRowCountStatus();
        }

        private void SetSourceLabels(string sourceA, string sourceB)
        {
            StatusSourceA.Text = string.Format("A: {0}", sourceA);
            StatusSourceB.Text = string.Format("B: {0}", sourceB);
        }

        private void SetGridRowCountStatus()
        {
            int count = comparedDataGrid.ItemsSource == null ? 0 : ((ObservableCollection<NavObjectsCompared>)comparedDataGrid.ItemsSource).Count;
            GridRowCountTextBox.Text = string.Format("No of Rows: {0}", count);
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


        #endregion Private Functions


        private object GetSelectedGridValue()
        {
            try
            {
                var selectedItem = comparedDataGrid.CurrentItem;

                string boundPropertyName = GetSelectedColumn();
                PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(selectedItem);

                PropertyDescriptor property = properties[boundPropertyName];

                return property.GetValue(selectedItem);
            }
            catch(Exception ex)
            {
                MessageHelper.ShowError(ex);
            }

            return null;
        }

        private string GetSelectedColumn()
        {
            DataGridBoundColumn currentColumn = comparedDataGrid.CurrentColumn as DataGridBoundColumn;
            Binding binding = currentColumn.Binding as Binding;

            return binding.Path.Path;
        }

        private void FilterToValue_Click(object sender, RoutedEventArgs e)
        {
            //NAVObjectCompareWinClient.Helpers.ComboboxItem selectedItem = RowFilters.GetComboBoxItem(fieldFilterComboBox, GetSelectedColumn());

            //fieldFilterComboBox.SelectedItem = selectedItem;
            //fieldFilterTextBox.Text = GetSelectedGridValue().ToString();

            //SetRowFilters();
        }

        private void SaveWorkspaceFile(string filepath)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                if (_compare == null)
                {
                    MessageHelper.ShowError("Nothing to save.");
                    return;
                }

                using (System.IO.FileStream writeStream = System.IO.File.OpenWrite(filepath))
                {
                    _compare.Serialize(writeStream);
                }               
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

        private void ReadWorkspaceFile(string filepath)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;

                using (System.IO.FileStream readStream = System.IO.File.OpenRead(filepath))
                {
                    _compare = new ObjectCompare();
                    _compare.Deserialize(readStream);
                }

                PopulateGrid();
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

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            SetSelectAllInGrid(true);
        }

        private void DeSelectAll_Click(object sender, RoutedEventArgs e)
        {
            SetSelectAllInGrid(false);
        }

        private void ExportFilesA_Click(object sender, RoutedEventArgs e)
        {
            ExportAllSelected(ObjectCompare.ObjectSource.A);
        }

        private void ExportFilesB_Click(object sender, RoutedEventArgs e)
        {
            ExportAllSelected(ObjectCompare.ObjectSource.B);
        }


        private void ExportAllSelected(ObjectCompare.ObjectSource objectSource)
        {
            try
            {
                IEnumerable<NavObjectsCompared> collection = ((ObservableCollection<NavObjectsCompared>)comparedDataGrid.ItemsSource).Where(s => s.Selected == true);

                if (Dialogs.SaveFile(objectSource.ToString(), out string filePath))
                    _compare.ExportObjects(objectSource, collection, filePath);
            }
            catch(Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private void SetSelectAllInGrid(bool select)
        {
            ObservableCollection<NavObjectsCompared> collection = ((ObservableCollection<NavObjectsCompared>)comparedDataGrid.ItemsSource);

            if (collection == null)
                return;

            foreach (NavObjectsCompared compared in collection)
            {
                compared.Selected = select;
            }
            comparedDataGrid.Items.Refresh();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutView aboutView = new AboutView(this);
            aboutView.ShowDialog();
        }
    }
}
