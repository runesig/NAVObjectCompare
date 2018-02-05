using NAVObjectCompare.ExportFinexe;
using NAVObjectCompareWinClient.Configurations;
using NAVObjectCompareWinClient.Helpers;
using NAVObjectCompareWinClient.Model;
using NAVObjectCompareWinClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NAVObjectCompareWinClient
{
    /// <summary>
    /// Interaction logic for ServerSetup.xaml
    /// </summary>
    public partial class ServerSetupView : Window
    {
        private ServerSetupViewModel _serverSetupViewModel;

        public ServerSetupModel SelectedServerSetup { get; private set; }

        public ServerSetupView()
        {
            InitializeComponent();
            _serverSetupViewModel = new ServerSetupViewModel();
            DataContext = _serverSetupViewModel;
        }

        public ServerSetupView(string name) : this()
        {
            _serverSetupViewModel.GetModel(name);
        }

        private void FinSQLPathButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;

            if (Dialogs.OpenFinsqlexe(ref filePath))
                _serverSetupViewModel.ServerSetup.FinSQLPath = filePath;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(_serverSetupViewModel.ServerSetup.Name))
            {
                DialogResult = false;
                SelectedServerSetup = null;
                return;
            }

            _serverSetupViewModel.Save();
            SelectedServerSetup = _serverSetupViewModel.ServerSetup;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            SelectedServerSetup = null;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Ask first?
            _serverSetupViewModel.Delete();
            DialogResult = false;
            SelectedServerSetup = null;
        }
    }
}
