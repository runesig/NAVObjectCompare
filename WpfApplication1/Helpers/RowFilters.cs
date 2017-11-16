using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace NAVObjectCompareWinClient.Helpers
{
    public class RowFilters
    {
        public const string FILTERALL = "ALL";
        public const string FILTERALLEQUAL = "ALLEQUAL";
        public const string FILTERALLNONEQUAL = "ALLNONEQUAL";
        public const string FILTERALLUNEXISTING = "FILTERALLUNEXISTING";
        public const string FILTERALLNONEQUALANDUNEXISTING = "FILTERALLNONEQUALANDUNEXISTING";
        public const string FILTERALLEDITED = "ALLEDITED";
        public const string FILTEROBJECTPROPERTIES = "OBJECTPROPERTIES";
        public const string FILTERCODEDIFF = "CODEDIFF";


        public static void AddItemsShowComboBoxItems(ref ComboBox showComboBox)
        {
            showComboBox.Items.Add(new ComboboxItem { Text = "Show all", Value = RowFilters.FILTERALL });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show all equal", Value = RowFilters.FILTERALLEQUAL });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show all non equal", Value = RowFilters.FILTERALLNONEQUAL });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show all unexisting", Value = RowFilters.FILTERALLUNEXISTING });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show all non equal and unexisting", Value = RowFilters.FILTERALLNONEQUALANDUNEXISTING });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show all edited", Value = RowFilters.FILTERALLEDITED });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show only date,time or version differences", Value = RowFilters.FILTEROBJECTPROPERTIES });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show code differences", Value = RowFilters.FILTERCODEDIFF });

            showComboBox.SelectedIndex = 0;
        }

        public static void AddFilterFieldsComboBoxItems(DataGrid comparedDataGrid, ref ComboBox fieldFilterComboBox)
        {
            foreach (DataGridColumn column in comparedDataGrid.Columns)
            {
                if(column is DataGridTextColumn)
                {
                    DataGridTextColumn textColumn = column as DataGridTextColumn;
                    Binding binding = (Binding)textColumn.Binding;
                    string path = binding.Path.Path;

                    fieldFilterComboBox.Items.Add(new ComboboxItem { Text = column.Header.ToString(), Value = path });
                }
            }

            fieldFilterComboBox.SelectedIndex = 0;
        }

        public static string CreateFilter(ComboboxItem showComboboxItem, ComboboxItem fieldComboBoxItem, string fieldFilterText)
        {
            string rowFilter = string.Empty;

            string showFilter = CreateShowFilter(showComboboxItem);
            string fieldFilter = CreateFieldFilter(fieldComboBoxItem, fieldFilterText);

            AddToRowFilter(ref rowFilter, showFilter);
            AddToRowFilter(ref rowFilter, fieldFilter);

            return rowFilter;
        }

        public static ComboboxItem GetComboBoxItem(ComboBox comboBox, string value)
        {
            foreach(ComboboxItem item in comboBox.Items)
            {
                if (item.Value == value)
                    return item;
            }

            return null;
        }

        private static void AddToRowFilter(ref string rowFilter, string filterToAdd)
        {
            if (!string.IsNullOrEmpty(filterToAdd))
            {
                if (string.IsNullOrEmpty(rowFilter))
                    rowFilter = filterToAdd;
                else
                    rowFilter += " AND " + filterToAdd;
            }
        }

        private static string CreateShowFilter(ComboboxItem showComboboxItem)
        {
            string rowFilter = string.Empty;

            switch (showComboboxItem.Value)
            {
                case FILTERALL:
                    rowFilter = string.Empty;
                    break;
                case FILTERALLEQUAL:
                    rowFilter = string.Format("Status = {0}", "0");
                    break;
                case FILTERALLNONEQUAL:
                    rowFilter = string.Format("Status = {0}", "1");
                    break;
                case FILTERALLUNEXISTING:
                    rowFilter = string.Format("Status = {0}", "2");
                    break;
                case FILTERALLNONEQUALANDUNEXISTING:
                    rowFilter = string.Format("Status > {0}", "0");
                    break;
                case FILTERALLEDITED:
                    rowFilter = string.Format("Edited = {0}", "True");
                    break;
                case FILTEROBJECTPROPERTIES:
                    rowFilter = string.Format("Status = {0} AND CodeEqual = {1} AND ObjectPropertiesEqual = {2}", "1", "True", "False");
                    break;
                case FILTERCODEDIFF:
                    rowFilter = string.Format("Status = {0} AND CodeEqual = {1} AND ObjectPropertiesEqual = {2}", "1", "False", "True");
                    break;
            }

            return rowFilter;
        }

        private static string CreateFieldFilter(ComboboxItem fieldComboBoxItem, string filterText)
        {
            if (!string.IsNullOrEmpty(filterText) && (fieldComboBoxItem != null))
                return string.Format("CONVERT({0}, System.String) LIKE '%{1}%'", fieldComboBoxItem.Value, filterText);

            return string.Empty;
        }
    }
}
