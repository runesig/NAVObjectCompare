using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompare.ExportFinexe.StatusFiles
{
    public class QueryExportLog
    {
        public string LogId { get; set; }
        public string Message { get; set; }
    }
    public class QueryExportLogFile : StatusFile
    {
        private string _exportLogId = string.Empty;
        private string _exportLogMessage = string.Empty;

        public QueryExportLogFile(string filePath)
            : base(filePath)
        {
        }

        public QueryExportLog GetStatus()
        {
            base.ReadFile();

            return new QueryExportLog
            {
                LogId = _exportLogId,
                Message = _exportLogMessage
            };
        }

        protected override void ReadLine(string line)
        {
            string[] parts = line.Split(']');

            for (int i = 0; i < parts.Length; i++)
            {
                if (string.IsNullOrEmpty(parts[i]))
                    continue;

                parts[i] = parts[i].Replace("[", string.Empty);

                switch (i)
                {
                    case 0:
                        _exportLogId = parts[i];
                        break;
                    case 1:
                        _exportLogMessage = parts[i];
                        break;
                }
            }
        }
    }
}
