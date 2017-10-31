using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAVObjectCompare.ExportFinexe;

namespace NavObjectCompareTests
{
    [TestClass]
    public class ExportFinexeUnitTest
    {
        [TestMethod]
        public void TestExport()
        {
            string exportedObjectsPath = string.Empty;
            string message = string.Empty;

            ExportFinexeHandling fileHandeling = new ExportFinexeHandling();
            fileHandeling.OnExportError += FileHandeling_OnExportError;
            fileHandeling.FinsqlPath = @"C:\Program Files (x86)\Microsoft Dynamics NAV\71\RoleTailored Client\finsql.exe";
            fileHandeling.ServerName = @"DESKTOP-BI4KASN\NAVDEMO";
            fileHandeling.Database = @"Demo Database NAV (7-1)";
            fileHandeling.NTAuthentication = true;

            fileHandeling.ExportObjects(out exportedObjectsPath, out message);
        }

        private void FileHandeling_OnExportError(object source, ExportErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.ToString());
        }
    }
}
