using NAVObjectCompareWinClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NAVObjectCompareWinClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Create the startup window
            CompareView compareView = new CompareView()
            {
                DataContext = new CompareViewModel(),
                // Title = "Dynamics NAV Object Compare"
            };

            // Show the window
            compareView.Show();
        }
    }
}
