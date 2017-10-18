using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using NAVObjectCompare.Models;

namespace NAVObjectCompareWinClient.Helpers
{
    public class Dialogs
    {
        public static bool OpenFile(ref string filePathA, ref string filePathB)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Open NAV Object File(s)";
            openDialog.Filter = "Txt files|*.txt";
            openDialog.Multiselect = true;

            Nullable<bool> result = openDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    if (openDialog.FileNames.Length > 1)
                    {
                        // Get the two first ones
                        filePathA = openDialog.FileNames[0];
                        filePathB = openDialog.FileNames[1];
                    }
                    else
                    {
                        filePathA = openDialog.FileName;
                        filePathB = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowError(ex);
                }

                return true;
            }

            return false;
        }

        public static bool SaveFile(Dictionary<string, NavObject> objects, string initFilename, string tag)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = string.Format("Export {0}: NAV Object File(s)", tag);
            saveDialog.Filter = "Txt files|*.txt";
            saveDialog.FileName = initFilename;

            Nullable<bool> result = saveDialog.ShowDialog();

            if (result == true)
            {
                try
                {
                    // ExportObjects(objects, saveDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageHelper.ShowError(ex);
                }

                return true;
            }

            return false;
        }
    }
}
