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
        public static bool OpenFile(bool multiSelect, ref string filePathA, ref string filePathB)
        {
            try
            {
                OpenFileDialog openDialog = new OpenFileDialog()
                {
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Title = "Open NAV Object File(s)",
                    Filter = "Txt files|*.txt",
                    Multiselect = true
                };


                Nullable<bool> result = openDialog.ShowDialog();

                if (result == true)
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

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }

            return false;
        }

        public static bool OpenFinsqlexe(ref string finsqlexePath)
        {
            try
            {
                OpenFileDialog openDialog = new OpenFileDialog()
                {
                    InitialDirectory = ProgramFilesx86(),
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Title = "finsql.exe Path",
                    Filter = "exe files|*.exe",
                    Multiselect = false
                };

                Nullable<bool> result = openDialog.ShowDialog();

                if (result == true)
                {
                    finsqlexePath = openDialog.FileName;

                    if (!string.IsNullOrEmpty(finsqlexePath))
                        return true;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowError(ex);
            }

            return false;
        }

        private static string ProgramFilesx86()
        {
            if (8 == IntPtr.Size
                || (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"))))
            {
                return Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            }

            return Environment.GetEnvironmentVariable("ProgramFiles");
        }

        public static bool SaveFile(Dictionary<string, NavObject> objects, string initFilename, string tag)
        {
            SaveFileDialog saveDialog = new SaveFileDialog()
            {
                Title = string.Format("Export {0}: NAV Object File(s)", tag),
                Filter = "Txt files|*.txt",
                FileName = initFilename
            };

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
