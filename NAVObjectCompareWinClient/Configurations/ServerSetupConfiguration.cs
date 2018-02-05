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
    public class ServerSetupConfiguration
    {
        public static ObservableCollection<ServerSetupModel> GetServerSetups()
        {
            ObservableCollection<ServerSetupModel> serverSetups = new ObservableCollection<ServerSetupModel>();

            ServerSetupSection serverSetupElements = ConfigurationManager.GetSection(ServerSetupSection.SectionName) as ServerSetupSection;

            if (serverSetupElements == null)
                throw new Exception("No Server Settings information found at all in App Config file.");

            foreach (ServerSetupElement serverSetupElement in serverSetupElements.ServerSetups.Cast<ServerSetupElement>())
            {
                serverSetups.Add(serverSetupElement.ToServerSetupModel());
            }

            return serverSetups;
        }

        public static ServerSetupModel GetServerSetup(string name)
        {
            ServerSetupSection serverSetupElements = ConfigurationManager.GetSection(ServerSetupSection.SectionName) as ServerSetupSection;

            ServerSetupElement serverSetupElement = (ServerSetupElement)serverSetupElements.ServerSetups[name];

            return serverSetupElement.ToServerSetupModel();
        }

        public static void Save(ServerSetupModel serverSetup)
        {
            System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ServerSetupSection serverSetupSection = configuration.GetSection(ServerSetupSection.SectionName) as ServerSetupSection;

            ServerSetupElement serverSetupElement;

            // Try get the existing name (key)
            serverSetupElement = (ServerSetupElement)serverSetupSection.ServerSetups[serverSetup.Name];
            // If it does not exists then try the prious name
            if (serverSetupElement == null) 
                serverSetupElement = (ServerSetupElement)serverSetupSection.ServerSetups[serverSetup.PreviousName];

            if ((serverSetup.IsNew) || (serverSetupElement == null))
                SaveNew(serverSetup, configuration, serverSetupSection);
            else
                SaveExisting(serverSetup, configuration, serverSetupElement);
        }

        private static void SaveNew(ServerSetupModel serverSetup, System.Configuration.Configuration configuration, ServerSetupSection serverSetupSection)
        {
            ServerSetupElement serverSetupElement = new ServerSetupElement();
            serverSetupElement.Fill(serverSetup);

            serverSetupSection.ServerSetups.Add(serverSetupElement);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(ServerSetupSection.SectionName);
        }

        private static void SaveExisting(ServerSetupModel serverSetup, System.Configuration.Configuration configuration, ServerSetupElement serverSetupElement)
        {
            if (serverSetupElement != null)
            {
                serverSetupElement.Fill(serverSetup);
                configuration.Save(ConfigurationSaveMode.Modified);
            }
            else
            {
                throw new Exception("Server Setup Element cannot be saved as it does not exists.");
            }

            ConfigurationManager.RefreshSection(ServerSetupSection.SectionName);
        }

        public static void Delete(ServerSetupModel serverSetup)
        {
            System.Configuration.Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ServerSetupSection serverSetupSection = configuration.GetSection(ServerSetupSection.SectionName) as ServerSetupSection;

            ServerSetupElement serverSetupElement = (ServerSetupElement)serverSetupSection.ServerSetups[serverSetup.Name];

            serverSetupSection.ServerSetups.Remove(serverSetupElement);
            configuration.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(ServerSetupSection.SectionName);
        }
    }
}
