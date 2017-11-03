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
using NAVObjectCompareWinClient.Configuration;
using NAVObjectCompareWinClient.Helpers;
using NAVObjectCompareWinClient.Model;

namespace NAVObjectCompareWinClient
{
    /// <summary>
    /// Interaction logic for ImportFiles.xaml
    /// </summary>
    public partial class ImportFiles : Window
    {
        public List<ServerSetupModel> ServerSetups = ServerSetupConfiguration.GetServerSetups();

        public ImportFiles()
        {
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

        private void ModifiedCheckBoxA_Checked(object sender, RoutedEventArgs e)
        {
            CreateFilterA();
        }

        private void CreateFilterA()
        {
            CustomFilterTextBoxA.Text = ExportFilter.Create(
                ModifiedCheckBoxA.IsChecked,
                DateFromDatePickerA.SelectedDate,
                DateToDatePickerA.SelectedDate,
                VersionListTextBoxA.Text,
                CustomCheckBoxA.IsChecked,
                CustomFilterTextBoxA.Text);
        }

        private void EditServerButtonA_Click(object sender, RoutedEventArgs e)
        {
            ServerSetup serverSetup = new ServerSetup();
            serverSetup.ShowDialog();

            if ((serverSetup.DialogResult.HasValue) && (serverSetup.DialogResult.Value))
            {
                MessageBox.Show("User clicked OK");
            }
            else
            {
                MessageBox.Show("User clicked Cancel");
            }

        }

        private void EditServerButtonB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ModifiedCheckBoxB_Checked(object sender, RoutedEventArgs e)
        {

        }

    }
}
