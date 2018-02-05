using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;

namespace NAVObjectCompareWinClient.Helpers
{
    public class DataTableHelper
    {
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
    }
}
