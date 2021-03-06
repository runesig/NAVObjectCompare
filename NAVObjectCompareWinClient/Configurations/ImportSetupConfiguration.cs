﻿using System;
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
        public static ObservableCollection<ImportSheetModel> GetImportSetups()
        {
            ObservableCollection<ImportSheetModel> importSetups = new ObservableCollection<ImportSheetModel>();

            ImportSetupSection importSetupElements = ConfigurationManager.GetSection(ImportSetupSection.SectionName) as ImportSetupSection;

            foreach (ImportSetupElement importSetupElement in importSetupElements.ImportSetups.Cast<ImportSetupElement>())
            {
                importSetups.Add(importSetupElement.ToImportSetupModel());
            }

            return importSetups;
        }

        public static ImportSheetModel GetImportSetup(string name)
        {
            ImportSetupSection importSetupElements = ConfigurationManager.GetSection(ImportSetupSection.SectionName) as ImportSetupSection;

            ImportSetupElement importSetupElement = (ImportSetupElement)importSetupElements.ImportSetups[name];
            if(importSetupElement != null)
                return importSetupElement.ToImportSetupModel();

            return new ImportSheetModel(name);
        }

        public static void Save(ImportSheetModel importSetup)
        {
            System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ImportSetupSection importSetupSection = configuration.GetSection(ImportSetupSection.SectionName) as ImportSetupSection;

            ImportSetupElement importSetupElement;

            // Try get the existing name (key)
            importSetupElement = (ImportSetupElement)importSetupSection.ImportSetups[importSetup.Name];

            if (importSetupElement == null)
                SaveNew(importSetup, configuration, importSetupSection);
            else
                SaveExisting(importSetup, configuration, importSetupElement);
        }

        private static void SaveNew(ImportSheetModel importSetup, System.Configuration.Configuration configuration, ImportSetupSection importSetupSection)
        {
            ImportSetupElement importSetupElement = new ImportSetupElement();
            importSetupElement.Fill(importSetup);

            importSetupSection.ImportSetups.Add(importSetupElement);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(ImportSetupSection.SectionName);
        }

        private static void SaveExisting(ImportSheetModel importSetup, System.Configuration.Configuration configuration, ImportSetupElement importSetupElement)
        {
            if (importSetupElement != null)
            {
                importSetupElement.Fill(importSetup);
                configuration.Save(ConfigurationSaveMode.Modified);
            }
            else
            {
                throw new Exception("Import Setup Element cannot be saved.");
            }

            ConfigurationManager.RefreshSection(ImportSetupSection.SectionName);
        }
    }
}
