using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAVObjectCompareWinClient.Model;
using NAVObjectCompareWinClient.Configuration;
using System.Configuration;

namespace NAVObjectCompareWinClient.Configuration
{
    public class ServerSetupConfiguration
    {
        private const string _configSection = "ServerSetupGroup/ServerSetups";

        public static List<ServerSetupModel> GetServerSetups()
        {
            List<ServerSetupModel> serverSetups = new List<ServerSetupModel>();

            ServerSetupSection serverSetupElements = ConfigurationManager.GetSection(_configSection) as ServerSetupSection;

            if (serverSetupElements == null)
                throw new Exception("No Server Settings information found at all in App Config file.");

            foreach (ServerSetupElement serverSetupElement in serverSetupElements.ServerSetups.Cast<ServerSetupElement>())
            {
                serverSetups.Add(serverSetupElement.ToServerSetupModel());
            }

            return serverSetups;
        }
    }
}
