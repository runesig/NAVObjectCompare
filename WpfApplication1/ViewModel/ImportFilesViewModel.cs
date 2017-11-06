using NAVObjectCompareWinClient.Configurations;
using NAVObjectCompareWinClient.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NAVObjectCompareWinClient.ViewModel
{
    public class ImportFilesViewModel
    {
        public ImportFilesModel ImportFiles { get; set; }

        public ImportFilesViewModel()
        {
            ImportFiles = new ImportFilesModel();
        }

        public void RefreshServerSetups()
        {
            ImportFiles.RefreshServerSetups();
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
