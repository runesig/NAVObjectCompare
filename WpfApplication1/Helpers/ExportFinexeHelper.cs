using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAVObjectCompare.ExportFinexe;
using NAVObjectCompareWinClient.Model;
using NAVObjectCompareWinClient.Configurations;

namespace NAVObjectCompareWinClient.Helpers
{
    public class ExportResult
    {
        public bool Success { get; set; }
        public string ExportedObjectsPath { get; set; }
        public string Message { get; set; }
    }

    public class ExportFinexeHelper
    {
        async private Task<ExportResult> CheckImportType(ImportSetupModel importSetupModel)
        {
            ExportResult result = new ExportResult { Success = false, ExportedObjectsPath = string.Empty, Message = string.Empty };

            switch (importSetupModel.ImportType)
            {
                case ImportTypes.Server:
                    result = await ExportObjectsFromFinExe(QueryExportTag.QueryExportA, importSetupModel);
                    break;
                case ImportTypes.File:
                    result = new ExportResult { Success = true, ExportedObjectsPath = importSetupModel.ImportFileName, Message = string.Empty }; 
                    break;
            }

            return result;
        }

        async public Task<ExportResult> ExportObjectsFromFinExe(QueryExportTag exportTag, ImportSetupModel importSetupModel)
        {
            var result = await Task.Factory.StartNew(() =>
            {
                ServerSetupModel serverSetup = ServerSetupConfiguration.GetServerSetup(importSetupModel.ServerSetupName);

                ExportFinexeHandling fileHandeling = new ExportFinexeHandling();
                // fileHandeling.OnExportError += FileHandeling_OnExportError;
                fileHandeling.FinsqlPath = serverSetup.FinSQLPath;
                fileHandeling.ServerName = serverSetup.Server;
                fileHandeling.Database = serverSetup.Database;
                fileHandeling.NTAuthentication = serverSetup.UseNTAuthentication;
                fileHandeling.QueryExportTag = exportTag;
                if (!serverSetup.UseNTAuthentication)
                {
                    fileHandeling.Username = serverSetup.UserName;
                    fileHandeling.Password = serverSetup.Password;
                }
                fileHandeling.Filter = importSetupModel.Filter;


                if (!fileHandeling.ExportObjects(out string exportedObjectsPath, out string message))
                {
                    return  new ExportResult { Success = false, ExportedObjectsPath = exportedObjectsPath, Message = message };
                }

                return new ExportResult { Success = true, ExportedObjectsPath = exportedObjectsPath, Message = message };
            });

            return result;
        }
    }
}
