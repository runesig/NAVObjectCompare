﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using NAVObjectCompare.StatusFiles;

namespace NAVSourceControl.ObjectFiles
{
    public delegate void FileExportedEventHandling(object source, FileExportedEventArgs e);
    public delegate void ExportErrorEventHandling(object source, ExportErrorEventArgs e);
    public class FileHandling
    {
        public event FileExportedEventHandling OnFileExported;
        public event ExportErrorEventHandling OnExportError;

        public string FinsqlPath { get; set; }
        public string ServerName { get; set; }
        public string Database { get; set; }
        public bool NTAuthentication { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public void ExportObjects()
        {
            try
            {
                ClearFiles();

                string command = CreateCommand();

                ProcessStartInfo startInfo = new ProcessStartInfo(this.FinsqlPath, command);
                startInfo.UseShellExecute = true;
                startInfo.CreateNoWindow = true;

                Process proc = new Process();
                proc.StartInfo = startInfo;
                proc.Start();
                proc.WaitForExit();

                CheckStatus(GetLogStatus());
            }
            catch (Exception ex)
            {
                if (OnExportError != null)
                    OnExportError(this, new ExportErrorEventArgs(ex));
            }
        }

        private void CheckStatus(LogStatus logStatus)
        {
            if ((logStatus.ResultStatus == ResultStatus.Error) || (logStatus.ResultStatus == ResultStatus.Unknown))
            {
                if (!string.IsNullOrEmpty(logStatus.LogId))
                    throw new Exception(string.Format(@"{0} {1}", logStatus.LogId, logStatus.LogMessage));
                else
                    throw new Exception(logStatus.ResultMessage);
            }
        }

        private LogStatus GetLogStatus()
        {
            NavCommandResultFile commandResult = new NavCommandResultFile(NavCommandResultFullPath());
            CommandResultStatus commandResultStatus = commandResult.GetStatus();

            QueryExportLogFile exportLog = new QueryExportLogFile(ExportLogFullPath());
            QueryExportLog exportLogStatus = exportLog.GetStatus();

            return new LogStatus {
                Date = commandResultStatus.Date,
                ResultMessage = commandResultStatus.Message,
                ResultStatus = commandResultStatus.Status,
                LogId = exportLogStatus.LogId,
                LogMessage = exportLogStatus.Message
            };    
        }



        private void ClearFiles()
        {
            if (File.Exists(ExportFileFullPath()))
                File.Delete(ExportFileFullPath());

            if (File.Exists(ExportLogFullPath()))
                File.Delete(ExportLogFullPath());

            if (File.Exists(NavCommandResultFullPath()))
                File.Delete(NavCommandResultFullPath());
        }

        private string CreateCommand()
        {
            return string.Format(@"command={0}, file={1}, servername={2}, database={3}, filter={4}, {5}, logfile={6}",
                ExportCommand(),
                ExportFileFullPath(),
                this.ServerName,
                this.Database,
                Filter(),
                Credentials(),
                ExportLogFullPath());
        }

        private string ExportCommand()
        {
            return "exportobjects";
        }

        private string Filter()
        {            
            // filter=""Type=Table;ID=50000..50100""
            return string.Format(@"Date={0}", DateTime.Now.ToShortDateString());
        }

        private string Credentials()
        {
            if (this.NTAuthentication)
                return "ntauthentication=yes";

            return string.Format("username={0}, password={1}, ntauthentication={2}", this.Username, this.Password, "no");
        }


        // Folders Start
        private string NavCommandResultFullPath()
        {
            return Path.Combine(ExporFilesFolder(), "navcommandresult.txt");
        }
        private string ExportLogFullPath()
        {
            return Path.Combine(ExporFilesFolder(), "queryexportlog.txt");
        }

        private string ExporFilesFolder()
        {
            return Path.Combine(GetObjectFileFolder(), "Export");
        }

        private string ExportFileFullPath()
        {
            return Path.Combine(ExporFilesFolder(), "QueryExport.txt");
        }

        public static string GetObjectFileFolder()
        {
            string tempPath = System.IO.Path.GetTempPath();
            string dateFolder = string.Format("{0}{1}{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string objectFilePath = Path.Combine(tempPath, dateFolder);

            if (!Directory.Exists(objectFilePath))
            {
                Directory.CreateDirectory(objectFilePath);
            }
            return objectFilePath;
        }
        // Folders Stop
    }

    public class FileExportedEventArgs : EventArgs
    {
        public FileExportedEventArgs(string filePath)
        {
            this.Filepath = filePath;
        }
        public string Filepath { get; set; }
    }

    public class ExportErrorEventArgs : EventArgs
    {
        public ExportErrorEventArgs(Exception ex)
        {
            this.Exception = ex;
        }
        public Exception Exception { get; set;}
    }
}
