using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompare.ExportFinexe
{
    public class ExportFilter
    {

        public static string Create(bool? modified, DateTime? dateFrom, DateTime? dateTo, string versionList, bool? custom, string customString)
        {
            // filter=""Type=Table;ID=50000..50100""

            string modifiedFilter = GetModifiedFilter(modified);

            string dateFilter = GetDateFilter(dateFrom, dateTo);

            string versionListFilter = GetVersionFilter(versionList);

            if ((custom.HasValue) && (custom.Value == true))
                return customString;

            string filterString = string.Empty;
            AppendToFilter(ref filterString, modifiedFilter);
            AppendToFilter(ref filterString, dateFilter);
            AppendToFilter(ref filterString, versionListFilter);

            return filterString;
        }

        public static void AppendToFilter(ref string filterString, string toAppend)
        {
            if (string.IsNullOrEmpty(filterString))
                filterString = toAppend;
            else
            {
                if (!string.IsNullOrEmpty(toAppend))
                    filterString = string.Format("{0};{1}", filterString, toAppend);
            }
        }

        private static string GetVersionFilter(string versionList)
        {
            string versionListFilter = string.Empty;
            if (!string.IsNullOrEmpty(versionList))
                versionListFilter = string.Format(@"Version List={0}", versionList);
            return versionListFilter;
        }

        private static string GetDateFilter(DateTime? dateFrom, DateTime? dateTo)
        {
            string dateFilter = string.Empty;
            if ((dateFrom.HasValue) && (dateTo.HasValue))
            {
                dateFilter = string.Format("Date={0}..{1}", dateFrom?.ToShortDateString(), dateTo?.ToShortDateString());
            }
            else if ((dateFrom.HasValue) && (!dateTo.HasValue))
            {
                dateFilter = string.Format("Date={0}", dateFrom?.ToShortDateString());
            }
            else if ((!dateFrom.HasValue) && (dateTo.HasValue))
            {
                dateFilter = string.Format("Date=..{0}", dateTo?.ToShortDateString());
            }

            return dateFilter;
        }

        private static string GetModifiedFilter(bool? modified)
        {
            string modifiedFilter = string.Empty;
            if ((modified.HasValue) && (modified.Value == true))
                modifiedFilter = @"Modified=yes";

            return modifiedFilter;
        }
    }
}
