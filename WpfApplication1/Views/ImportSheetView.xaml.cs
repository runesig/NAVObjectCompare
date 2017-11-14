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
    public partial class ImportSheetView : Window
    {
        public ImportSheetModel SelectedImportSetupModelA { get; private set; }
        public ImportSheetModel SelectedImportSetupModelB { get; private set; }

        private ImportSheetViewModel _importFilesViewModel;

        public ImportSheetView()
        {
            _importFilesViewModel = new ImportSheetViewModel();
            DataContext = _importFilesViewModel;
            InitializeComponent();
        }

        private void FilePathButtonA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = string.Empty;
                string emptyFilePath = string.Empty;

                if (Dialogs.OpenFile(false, ref filePath, ref emptyFilePath))
                    _importFilesViewModel.ImportFiles.ImportSetupA.ImportFileName = filePath;
            }
            catch(Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private void FilePathButtonB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = string.Empty;
                string emptyFilePath = string.Empty;

                if (Dialogs.OpenFile(false, ref filePath, ref emptyFilePath))
                    _importFilesViewModel.ImportFiles.ImportSetupB.ImportFileName = filePath;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }
        }

        private void EditServerButtonA_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EditServerSetup(ServerComboBoxA);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }            
        }

        private void EditServerButtonB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EditServerSetup(ServerComboBoxB);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }            
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // First Save Everything
                _importFilesViewModel.SaveImportSetup();

                SelectedImportSetupModelA = _importFilesViewModel.ImportFiles.ImportSetupA;
                SelectedImportSetupModelB = _importFilesViewModel.ImportFiles.ImportSetupB;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void EditServerSetup(ComboBox serverSetupCombobox)
        {
            ServerSetupModel serverSetupModel = (ServerSetupModel)serverSetupCombobox.SelectedItem;

            ServerSetupView serverSetup = InitServerSetup(serverSetupModel);
            serverSetup.ShowDialog();

            // Refresh
            _importFilesViewModel.SetServerSetups();

            if ((serverSetup.DialogResult.HasValue) && (serverSetup.DialogResult.Value))
            {
                serverSetupCombobox.SelectedItem = serverSetup.SelectedServerSetup; // Set to view model
            }
        }

        private static ServerSetupView InitServerSetup(ServerSetupModel serverSetupModel)
        {
            ServerSetupView serverSetup;

            if (serverSetupModel != null)
                serverSetup = new ServerSetupView(serverSetupModel.Name);
            else
                serverSetup = new ServerSetupView();

            return serverSetup;
        }
    }
}
