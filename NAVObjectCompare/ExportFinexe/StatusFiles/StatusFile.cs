using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NAVObjectCompare.ExportFinexe.StatusFiles
{
    public class LogStatus
    {
        public ResultStatus ResultStatus { get; set; }
        public DateTime Date { get; set; }
        public string ResultMessage { get; set; }
        public string LogId { get; set; }
        public string LogMessage { get; set; }
    }

    public abstract class StatusFile
    {
        public string FullFilePath { get; set; }

        public StatusFile(string fullFilePath)
        {
            this.FullFilePath = fullFilePath;
        }

        protected void ReadFile()
        {
            if (!File.Exists(this.FullFilePath))
                return;

            var lines = File.ReadAllLines(this.FullFilePath, Encoding.Default);

            string newLine = string.Empty;
            for (int i = 0; i < lines.Length; i++)
            {
                if (string.IsNullOrEmpty(lines[i]))
                    newLine += " ";

                newLine += lines[i];
            }
            ReadLine(newLine);
        }
        protected abstract void ReadLine(string line);
    }
}
