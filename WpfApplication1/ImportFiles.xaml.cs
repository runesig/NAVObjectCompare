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

        private void filePathButtonA_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;
            string emptyFilePath = string.Empty;

            if (Dialogs.OpenFile(false, ref filePath, ref emptyFilePath))
                filePathTextBoxA.Text = filePath;
        }

        private void filePathButtonB_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;
            string emptyFilePath = string.Empty;

            if (Dialogs.OpenFile(false, ref filePath, ref emptyFilePath))
                filePathTextBoxB.Text = filePath;
        }

        private void finSQLPathButtonA_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;

            if (Dialogs.OpenFinsqlexe(ref filePath))
                finSQLPathTextBoxA.Text = filePath;
        }

        private void finSQLPathButtonB_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;

            if (Dialogs.OpenFinsqlexe(ref filePath))
                finSQLPathTextBoxB.Text = filePath;

            //DateTime? selectedDateFrom = dateFromDatePickerA.SelectedDate;
            //DateTime? selectedDateTo = dateToDatePickerA.SelectedDate;
        }
    }
}
