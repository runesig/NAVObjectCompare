using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NAVObjectCompareWinClient.Helpers
{
    public class MessageHelper
    {
        public static void ShowError(Exception ex)
        {
            string message = string.Empty;

            if (!IsShowFullException())
                message = ex.Message;
            else
                message = ex.ToString();

            ShowError(message);
        }

        public static void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static bool IsShowFullException()
        {
            try
            {
                string showFullException = ConfigurationManager.AppSettings["ShowFullException"];

                if (bool.TryParse(showFullException, out bool isShowFullException))
                    return isShowFullException;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return false;
        }
    }
}
