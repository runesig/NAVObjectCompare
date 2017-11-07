using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompareWinClient.Model
{
    public class ServerSetupModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ServerSetupModel(string name)
        {
            _name = name;

            if (string.IsNullOrEmpty(name))
                IsNew = true;
        }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name;  }
            set { _previousName = _name; _name = value; RaisePropertyChanged("Name"); }
        }

        private string _previousName = string.Empty;
        public string PreviousName
        {
            get { return _previousName; }
        }

        private string _finSQLPath = string.Empty;
        public string FinSQLPath
        {
            get { return _finSQLPath; }
            set { _finSQLPath = value; RaisePropertyChanged("FinSQLPath"); }
        }

        private string _server = string.Empty;
        public string Server
        {
            get { return _server; } set { _server = value; RaisePropertyChanged("Server"); }
        }

        private string _database = string.Empty;
        public string Database
        {
            get { return _database; } set { _database = value; RaisePropertyChanged("Database"); }
        }

        private bool _useNTAuthentication = false;
        public bool UseNTAuthentication
        {
            get { return _useNTAuthentication; }
            set { _useNTAuthentication = value; RaisePropertyChanged("UseNTAuthentication"); }
        }

        private string _userName = string.Empty;
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; RaisePropertyChanged("UserName"); }
        }

        private string _password = string.Empty;
        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged("Password"); }
        }

        public bool IsNew { get; set; }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //public override string ToString()
        //{
        //    return Name;
        //}

        public override bool Equals(object obj)
        {
            return obj is ServerSetupModel && ((ServerSetupModel)obj).Name.Equals(Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
