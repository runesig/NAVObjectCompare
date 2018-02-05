using NAVObjectCompare.ExportFinexe;
using NAVObjectCompareWinClient.Configurations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompareWinClient.Model
{
    public class ImportFilesModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ServerSetupModel> ServerSetups { get; private set; }
        public ImportSheetModel ImportSetupA { get; private set; }
        public ImportSheetModel ImportSetupB { get; private set; }

        public void SetServerSetups(ObservableCollection<ServerSetupModel> serverSetups)
        {
            ServerSetups = serverSetups;
            RaisePropertyChanged("ServerSetups");
        }

        public void SetImportSetups(ImportSheetModel importSetupA, ImportSheetModel importSetupB)
        {
            ImportSetupA = importSetupA;
            ImportSetupB = importSetupB;
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
