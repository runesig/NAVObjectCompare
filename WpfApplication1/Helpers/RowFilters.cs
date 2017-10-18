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
        public const string FILTEROBJECTPROPERTIES = "OBJECTPROPERTIES";
        public const string FILTERCODEDIFF = "CODEDIFF";


        public static void AddItemsShowComboBoxItems(ref ComboBox showComboBox)
        {
            showComboBox.Items.Add(new ComboboxItem { Text = "Show all", Value = RowFilters.FILTERALL });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show all equal", Value = RowFilters.FILTERALLEQUAL });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show all non equal", Value = RowFilters.FILTERALLNONEQUAL });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show only date,time differences", Value = RowFilters.FILTEROBJECTPROPERTIES });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show code differences", Value = RowFilters.FILTERCODEDIFF });

            showComboBox.SelectedIndex = 0;
        }

        public static void AddFilterFieldsComboBoxItems(DataGrid comparedDataGrid, ref ComboBox fieldFilterComboBox)
        {
            foreach (DataGridTextColumn column in comparedDataGrid.Columns)
            {
                Binding binding = (Binding)column.Binding;
                string path = binding.Path.Path;

                fieldFilterComboBox.Items.Add(new ComboboxItem { Text = column.Header.ToString(), Value = path });
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
                    rowFilter = string.Format("Equal = {0}", "True");
                    break;
                case FILTERALLNONEQUAL:
                    rowFilter = string.Format("Equal = {0}", "False");
                    break;
                case FILTEROBJECTPROPERTIES:
                    rowFilter = string.Format("Equal = {0} AND CodeEqual = {1} AND ObjectPropertiesEqual = {2}", "False", "True", "False");
                    break;
                case FILTERCODEDIFF:
                    rowFilter = string.Format("Equal = {0} AND CodeEqual = {1}", "False", "False");
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
