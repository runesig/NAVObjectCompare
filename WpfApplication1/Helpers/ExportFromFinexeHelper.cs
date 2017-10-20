using NAVSourceControl.ObjectFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompareWinClient.Helpers
{
    public class ExportFromFinexeHelper
    {
        public void Export()
        {
            FileHandling fileHandeling = new FileHandling();
            fileHandeling.OnExportError += FileHandeling_OnExportError;
            fileHandeling.FinsqlPath = @"C:\Program Files (x86)\Microsoft Dynamics NAV\71\RoleTailored Client\finsql.exe";
            fileHandeling.ServerName = @"DESKTOP-BI4KASN\NAVDEMO";
            fileHandeling.Database = @"Demo Database NAV (7-1)";
            fileHandeling.NTAuthentication = true;

            fileHandeling.ExportObjects();
        }

        private void FileHandeling_OnExportError(object source, ExportErrorEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
