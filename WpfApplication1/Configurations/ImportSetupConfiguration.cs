using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAVObjectCompareWinClient.Model;
using NAVObjectCompareWinClient.Configurations;
using System.Configuration;
using System.Collections.ObjectModel;

namespace NAVObjectCompareWinClient.Configurations
{
    public class ImportSetupConfiguration
    {
        public static ObservableCollection<ImportSetupModel> GetImportSetups()
        {
            ObservableCollection<ImportSetupModel> importSetups = new ObservableCollection<ImportSetupModel>();

            ImportSetupSection importSetupElements = ConfigurationManager.GetSection(ServerSetupSection.SectionName) as ImportSetupSection;

            foreach (ImportSetupElement importSetupElement in importSetupElements.ImportSetups.Cast<ImportSetupElement>())
            {
                importSetups.Add(importSetupElement.ToImportSetupModel());
            }

            return importSetups;
        }

        public static ImportSetupModel GetImportSetup(string name)
        {
            ImportSetupSection importSetupElements = ConfigurationManager.GetSection(ImportSetupSection.SectionName) as ImportSetupSection;

            ImportSetupElement importSetupElement = (ImportSetupElement)importSetupElements.ImportSetups[name];

            return importSetupElement.ToImportSetupModel();
        }
    }
}
