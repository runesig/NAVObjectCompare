using NAVObjectCompare.ExportFinexe;
using NAVObjectCompareWinClient.Helpers;
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
    /// Interaction logic for ImportFiles.xaml
    /// </summary>
    public partial class ImportFiles : Window
    {
        public ImportFiles()
        {
            InitializeComponent();
        }

        private void FilePathButtonA_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;
            string emptyFilePath = string.Empty;

            if (Dialogs.OpenFile(false, ref filePath, ref emptyFilePath))
                filePathTextBoxA.Text = filePath;
        }

        private void FilePathButtonB_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;
            string emptyFilePath = string.Empty;

            if (Dialogs.OpenFile(false, ref filePath, ref emptyFilePath))
                filePathTextBoxB.Text = filePath;
        }

        private void ModifiedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CreateFilter();
        }

        private void CreateFilter()
        {
            customFilterTextBoxA.Text = ExportFilter.Create(
                modifiedCheckBox.IsChecked,
                dateFromDatePickerA.SelectedDate,
                dateToDatePickerA.SelectedDate,
                versionListTextBoxA.Text,
                customCheckBoxA.IsChecked,
                customFilterTextBoxA.Text);
        }

        private void EditServerButtonA_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
