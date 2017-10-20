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
            Console.WriteLine(e.Exception.ToString());
        }
    }
}
