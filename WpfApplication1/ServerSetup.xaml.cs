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
    /// Interaction logic for ServerSetup.xaml
    /// </summary>
    public partial class ServerSetup : Window
    {
        public ServerSetup()
        {
            InitializeComponent();
        }


        private void FinSQLPathButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = string.Empty;

            if (Dialogs.OpenFinsqlexe(ref filePath))
                finSQLPathTextBox.Text = filePath;
        }
    }
}
