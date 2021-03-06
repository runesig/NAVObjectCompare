﻿using NAVObjectCompareWinClient.Configurations;
using NAVObjectCompareWinClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NAVObjectCompareWinClient.ViewModel
{
    public class ImportSheetViewModel
    {
        private const string ImportSetupNameA = "A";
        private const string ImportSetupNameB = "B";

        public ImportFilesModel ImportFiles { get; set; }

        public ImportSheetViewModel()
        {
            ImportFiles = new ImportFilesModel();
            SetServerSetups();
            SetImportSetups();
        }

        public void SetImportSetups()
        {
            ImportSheetModel importSetupModelA = ImportSetupConfiguration.GetImportSetup(ImportSetupNameA);
            ImportSheetModel importSetupModelB = ImportSetupConfiguration.GetImportSetup(ImportSetupNameB);

            ImportFiles.SetImportSetups(importSetupModelA, importSetupModelB);
        }

        public void SaveImportSetup()
        {
            ImportSetupConfiguration.Save(ImportFiles.ImportSetupA);
            ImportSetupConfiguration.Save(ImportFiles.ImportSetupB);
        }

        public void SetServerSetups()
        {
            ObservableCollection<ServerSetupModel> serverSetups = ServerSetupConfiguration.GetServerSetups();
            ImportFiles.SetServerSetups(serverSetups);
        }

        public void AddNewServerSetup(ServerSetupModel serverSetupModel)
        {
            ImportFiles.ServerSetups.Add(serverSetupModel);
        }

        public void DeleteServerSetup(ServerSetupModel serverSetupModel)
        {
            ImportFiles.ServerSetups.Remove(serverSetupModel);
        }
    }
}
