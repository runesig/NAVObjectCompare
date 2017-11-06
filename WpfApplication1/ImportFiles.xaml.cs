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
using NAVObjectCompare.ExportFinexe;
using NAVObjectCompareWinClient.Configurations;
using NAVObjectCompareWinClient.Helpers;
using NAVObjectCompareWinClient.Model;
using NAVObjectCompareWinClient.ViewModel;

namespace NAVObjectCompareWinClient
{
    /// <summary>
    /// Interaction logic for ImportFiles.xaml
    /// </summary>
    public partial class ImportFiles : Window
    {
        private ImportFilesViewModel _importFilesViewModel;

        public ImportFiles()
        {
            _importFilesViewModel = new ImportFilesViewModel();
            DataContext = _importFilesViewModel;
            InitializeComponent();
        }

        private void FilePathButtonA_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;
            string emptyFilePath = string.Empty;

            if (Dialogs.OpenFile(false, ref filePath, ref emptyFilePath))
                FilePathTextBoxA.Text = filePath;
        }

        private void FilePathButtonB_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;
            string emptyFilePath = string.Empty;

            if (Dialogs.OpenFile(false, ref filePath, ref emptyFilePath))
                FilePathTextBoxB.Text = filePath;
        }

        private void EditServerButtonA_Click(object sender, RoutedEventArgs e)
        {
            EditServerSetup(ServerComboBoxA);
        }

        private void EditServerButtonB_Click(object sender, RoutedEventArgs e)
        {
            EditServerSetup(ServerComboBoxB);
        }

        private void EditServerSetup(ComboBox serverSetupCombobox)
        {
            ServerSetupModel serverSetupModel = (ServerSetupModel)serverSetupCombobox.SelectedItem;

            ServerSetup serverSetup = InitServerSetup(serverSetupModel);
            serverSetup.ShowDialog();

            // Refresh
            _importFilesViewModel.RefreshServerSetups();

            if ((serverSetup.DialogResult.HasValue) && (serverSetup.DialogResult.Value))
            {
                serverSetupCombobox.SelectedItem = serverSetup.SelectedServerSetup;
            }
        }

        private static ServerSetup InitServerSetup(ServerSetupModel serverSetupModel)
        {
            ServerSetup serverSetup;

            if (serverSetupModel != null)
                serverSetup = new ServerSetup(serverSetupModel.Name);
            else
                serverSetup = new ServerSetup();

            return serverSetup;
        }

    }
}
