﻿using System;
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

namespace NAVObjectCompareWinClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Editor _editor = null;
        ObjectCompare _compare = null;

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
            InitProgressBar();
            // Editor
            InitEditor();
            // Filters
            InitFilters();
        }

        private void InitEditor()
        {
            string filePathEditor = ConfigurationManager.AppSettings["BeyondComparePath"];
            _editor = new Editor(filePathEditor);
            _editor.OnReCompareObject += _editor_OnReCompareObject;
        }

        private void InitFilters()
        {
            RowFilters.AddFilterFieldsComboBoxItems(comparedDataGrid, ref fieldFilterComboBox);
            RowFilters.AddItemsShowComboBoxItems(ref showComboBox);
        }

        private void InitProgressBar()
        {
            processProgessBar.Visibility = Visibility.Collapsed;
            processProgessBar.Minimum = 0;
            processProgessBar.Maximum = 100;
            processProgessBar.Value = 0;
        }

        #region EventHandling

        private void _compare_OnCompared(int percentCompleted)
        {
            processProgessBar.Dispatcher.Invoke(() => processProgessBar.Value = percentCompleted, DispatcherPriority.Background);
        }

        private void _editor_OnReCompareObject(object source, EditorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void openMenu_Click(object sender, RoutedEventArgs e)
        {
            string filePathA = string.Empty;
            string filePathB = string.Empty;

            if(Dialogs.OpenFile(ref filePathA, ref filePathB))
                CompareAndFillGrid(filePathA, filePathB);
        }

        private void exitMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGridRow dataGridRow = (DataGridRow)e.Source;
                DataRowView dataRowView = (DataRowView)dataGridRow.Item;

                string internalId = (string)dataRowView["InternalId"];
                OpenEditor(internalId);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private void showComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetRowFilters();
        }

        private void fieldFilterTextBox_KeyDown(object sender, KeyEventArgs e)
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
            if (processProgessBar.Visibility == Visibility.Visible)
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

        #endregion Async Functions

        #region Private Functions

        private void RunComparison(string filePathA, string filePathB)
        {
            if (_compare == null)
            {
                _compare = new ObjectCompare(); // Start New Compare
                _compare.CompareFilePathA = filePathA;
                _compare.CompareFilePathB = filePathB;
                _compare.OnCompared += _compare_OnCompared;
            }
            else
            {
                // A Comparison have been done previously Check what to do
                if ((string.IsNullOrEmpty(filePathB)) && (_compare.NavObjectsA.Count > 0) && (_compare.NavObjectsB.Count > 0))
                {
                    _compare = new ObjectCompare(); // Start a new Compare
                    _compare.CompareFilePathA = filePathA;
                    _compare.OnCompared += _compare_OnCompared;
                }
                else if ((string.IsNullOrEmpty(filePathB)) && (_compare.NavObjectsA.Count > 0) && (_compare.NavObjectsB.Count == 0)) // Compare has been done only for file A then add B
                {
                    _compare.CompareFilePathB = filePathA; // Set the A file to path B to add the new one
                    _compare.OnCompared += _compare_OnCompared;
                }
                else if ((!string.IsNullOrEmpty(filePathA)) && (!string.IsNullOrEmpty(filePathB)))
                {
                    // Totally New Compare
                    _compare = new ObjectCompare();
                    _compare.CompareFilePathA = filePathA;
                    _compare.CompareFilePathB = filePathB;
                    _compare.OnCompared += _compare_OnCompared;
                }
            }

            _compare.RunCompare();
        }

        private void PopulateGrid()
        {
            DataTable listAsDataTable = DataTableHelper.BuildDataTable<NavObjectsCompared>(_compare.GetList());
            DataView listAsDataView = listAsDataTable.DefaultView;

            comparedDataGrid.AutoGenerateColumns = false;
            comparedDataGrid.ItemsSource = listAsDataView;

            SetRowFilters();
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

        private void SetRowFilters()
        {
            ComboboxItem showComboBoxItem = (ComboboxItem)showComboBox.SelectedItem;
            ComboboxItem fieldFilterComboboxItem = (ComboboxItem)fieldFilterComboBox.SelectedItem;
            string fieldFilter = fieldFilterTextBox.Text;

            if (comparedDataGrid.ItemsSource == null)
                return;

            (comparedDataGrid.ItemsSource as DataView).RowFilter = RowFilters.CreateFilter(showComboBoxItem, fieldFilterComboboxItem, fieldFilter);

            //int count = (comparedDataGridView.DataSource as DataView).Count;
            //SetStatus(count);
        }

        private void SetSourceLabels(string sourceA, string sourceB)
        {
            statusSourceA.Text = string.Format("A: {0}", sourceA);
            statusSourceB.Text = string.Format("B: {0}", sourceB);
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
            NAVObjectCompareWinClient.Helpers.ComboboxItem selectedItem = RowFilters.GetComboBoxItem(fieldFilterComboBox, GetSelectedColumn());

            fieldFilterComboBox.SelectedItem = selectedItem;
            fieldFilterTextBox.Text = GetSelectedGridValue().ToString();

            SetRowFilters();
        }
    }
}
