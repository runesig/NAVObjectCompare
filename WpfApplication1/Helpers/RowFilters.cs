using NAVObjectCompare.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
        public const string FILTERALLSELECTED = "ALLSELECTED";
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
            showComboBox.Items.Add(new ComboboxItem { Text = "Show all selected", Value = RowFilters.FILTERALLSELECTED });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show only date,time or version differences", Value = RowFilters.FILTEROBJECTPROPERTIES });
            showComboBox.Items.Add(new ComboboxItem { Text = "Show only code differences", Value = RowFilters.FILTERCODEDIFF });

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

        public static ObservableCollection<NavObjectsCompared> CreateFilter(ObservableCollection<NavObjectsCompared> collection, ComboboxItem showComboboxItem, ComboboxItem fieldComboBoxItem, string fieldFilterText)
        {
            ObservableCollection<NavObjectsCompared> collectionShowFilter = CreateShowLinq(collection, showComboboxItem);
            return CreateFieldFilter(collectionShowFilter, fieldComboBoxItem, fieldFilterText);
        }


        private static ObservableCollection<NavObjectsCompared> CreateShowLinq(ObservableCollection<NavObjectsCompared> collection, ComboboxItem showComboboxItem)
        {
            switch (showComboboxItem.Value)
            {
                case FILTERALL:
                    return collection;
                case FILTERALLEQUAL:
                    return new ObservableCollection<NavObjectsCompared>(collection.Where(w => w.Status == NavObjectsCompared.EqualStatus.Equal));
                case FILTERALLNONEQUAL:
                    return new ObservableCollection<NavObjectsCompared>(collection.Where(w => w.Status == NavObjectsCompared.EqualStatus.Unequal));
                case FILTERALLUNEXISTING:
                    return new ObservableCollection<NavObjectsCompared>(collection.Where(w => w.Status == NavObjectsCompared.EqualStatus.Unexisting));
                case FILTERALLNONEQUALANDUNEXISTING:
                    return new ObservableCollection<NavObjectsCompared>(collection.Where(w => w.Status > NavObjectsCompared.EqualStatus.Equal));
                case FILTERALLEDITED:
                    return new ObservableCollection<NavObjectsCompared>(collection.Where(w => w.Edited == true));
                case FILTERALLSELECTED:
                    return new ObservableCollection<NavObjectsCompared>(collection.Where(w => w.Selected == true));
                case FILTEROBJECTPROPERTIES:
                    return new ObservableCollection<NavObjectsCompared>(collection.Where(w => w.Status == NavObjectsCompared.EqualStatus.Unequal).Where(c => c.CodeEqual == true).Where(p => p.ObjectPropertiesEqual == false));
                case FILTERCODEDIFF:
                    return new ObservableCollection<NavObjectsCompared>(collection.Where(w => w.Status == NavObjectsCompared.EqualStatus.Unequal).Where(c => c.CodeEqual == false).Where(p => p.ObjectPropertiesEqual == true));
            }

            return collection;
        }

        private static ObservableCollection<NavObjectsCompared> CreateFieldFilter(ObservableCollection<NavObjectsCompared> collection, ComboboxItem fieldComboBoxItem, string filterText)
        {
            if (!string.IsNullOrEmpty(filterText) && (fieldComboBoxItem != null))
            {
                IQueryable<NavObjectsCompared> coll = collection.AsQueryable<NavObjectsCompared>();
                var query = Simplified<NavObjectsCompared>(coll, fieldComboBoxItem.Value, filterText);

                return new ObservableCollection<NavObjectsCompared>(query);
            }

            return collection;
        }

        private static IQueryable<T> Simplified<T>(IQueryable<T> query, string propertyName, string propertyValue)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName);
            return Simplified<T>(query, propertyInfo, propertyValue);
        }

        private static IQueryable<T> Simplified<T>(IQueryable<T> query, PropertyInfo propertyInfo, string propertyValue)
        {
            var eParam = Expression.Parameter(typeof(T), "e");
            var method = typeof(string).GetMethod("Contains");
            var call = Expression.Call(Expression.Property(eParam, propertyInfo), method, Expression.Constant(propertyValue));
            var lambda = Expression.Lambda<Func<T, bool>>(call, eParam);

            return query.Where(lambda);
        }
    }
}
