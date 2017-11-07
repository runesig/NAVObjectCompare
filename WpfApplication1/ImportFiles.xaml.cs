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
        public ImportSetupModel SelectedImportSetupModelA { get; private set; }
        public ImportSetupModel SelectedImportSetupModelB { get; private set; }

        private ImportFilesViewModel _importFilesViewModel;

        public ImportFiles()
        {
            _importFilesViewModel = new ImportFilesViewModel();
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

            ServerSetup serverSetup = InitServerSetup(serverSetupModel);
            serverSetup.ShowDialog();

            // Refresh
            _importFilesViewModel.SetServerSetups();

            if ((serverSetup.DialogResult.HasValue) && (serverSetup.DialogResult.Value))
            {
                serverSetupCombobox.SelectedItem = serverSetup.SelectedServerSetup; // Set to view model
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

    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.Equals(true) ? parameter : Binding.DoNothing;
        }
    }
}
