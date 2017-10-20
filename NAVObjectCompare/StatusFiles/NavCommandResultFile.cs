using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NAVObjectCompare.StatusFiles
{
    public enum ResultStatus { Unknown, OK, Error }

    public class CommandResultStatus
    {
        public ResultStatus Status { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }

    public class NavCommandResultFile : StatusFile
    {
        private ResultStatus _status = ResultStatus.Unknown;
        private DateTime _date = DateTime.Now;
        private string _message = string.Empty;

        public NavCommandResultFile(string filePath) : base(filePath)
        {
        }

        public CommandResultStatus GetStatus()
        {
            base.ReadFile();

            return new CommandResultStatus 
            { 
                Status = _status, 
                Date = _date, 
                Message = _message 
            };
        }

        protected override void ReadLine(string line)
        {
            string[] parts = line.Split(']');

            for (int i = 0; i < parts.Length; i++)
            {
                parts[i] = parts[i].Substring(1).Replace("[", string.Empty);

                switch(i)
                {
                    case 0:
                        _status = GetResultStatus(parts[i]);
                        break;
                    case 1:
                        _date = GetResultDate(parts[i]);
                        break;
                    case 2:
                        _message = GetResultMessage(parts[i]);
                        break;
                }
            }
        }

        private ResultStatus GetResultStatus(string part)
        {
            int statusInt = -1;
            if (!int.TryParse(part, out statusInt))
                statusInt = -1;

            switch(statusInt)
            {
                case -1:
                    return ResultStatus.Unknown;
                case 0:
                    return ResultStatus.OK;
                case 1:
                    return ResultStatus.Error;
            }

            return ResultStatus.Error;
        }    
        
        private DateTime GetResultDate(string part)
        {
            DateTime resultDate = DateTime.Now;
            if (!DateTime.TryParse(part, out resultDate))
                resultDate = DateTime.Now;

            return resultDate;
        }

        private string GetResultMessage(string part)
        {
            return part;
        }

    }
}
