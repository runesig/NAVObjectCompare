using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAVObjectCompareWinClient.Model;
using NAVObjectCompareWinClient.Configurations;
using System.ComponentModel;

namespace NAVObjectCompareWinClient.ViewModel
{
    public class ServerSetupViewModel
    {
        public ServerSetupModel ServerSetup { get; set; }

        public ServerSetupViewModel(bool isNew)
        {
            ServerSetup = new ServerSetupModel(isNew);
        }

        public void GetModel(string name)
        {
            ServerSetup = ServerSetupConfiguration.GetServerSetup(name);
        }

        public void Save()
        {
            ServerSetupConfiguration.Save(ServerSetup);
        }

        public void Delete()
        {
            ServerSetupConfiguration.Delete(ServerSetup);
        }
    }
}
