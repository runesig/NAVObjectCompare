using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAVObjectCompare.ExportFinexe;

namespace NAVObjectCompareWinClient.Helpers
{
    public class ExportFromFinexeHelper
    {
        public bool Export(out string exportedObjectsPath, out string message)
        {
            ExportFinexeHandling fileHandeling = new ExportFinexeHandling();
            fileHandeling.OnExportError += FileHandeling_OnExportError;
            fileHandeling.FinsqlPath = @"C:\Program Files (x86)\Microsoft Dynamics NAV\71\RoleTailored Client\finsql.exe";
            fileHandeling.ServerName = @"DESKTOP-BI4KASN\NAVDEMO";
            fileHandeling.Database = @"Demo Database NAV (7-1)";
            fileHandeling.NTAuthentication = true;

            return fileHandeling.ExportObjects(out exportedObjectsPath, out message);
        }

        private void FileHandeling_OnExportError(object source, ExportErrorEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
