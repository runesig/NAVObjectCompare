using NAVObjectCompare.ExportFinexe;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompareWinClient.Model
{
    public enum ImportTypes
    {
        Server = 0,
        File = 1
    }

    public class ImportSetupModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ImportSetupModel(string name)
        {
            _name = name;
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged("Name"); }
        }

        private ImportTypes _importType = ImportTypes.Server;
        public ImportTypes ImportType
        {
            get { return _importType; }
            set { _importType = value; }
        }

        private string _serverSetupName;
        public string ServerSetupName
        {
            get { return _serverSetupName; }
            set { _serverSetupName = value; RaisePropertyChanged("ServerSetupName"); }
        }

        private bool _modified;
        public bool Modified
        {
            get { return _modified; }
            set { _modified = value; CreateFilter(); RaisePropertyChanged("Modified"); }
        }

        private DateTime? _dateFrom;
        public DateTime? DateFrom
        {
            get { return _dateFrom; }
            set { _dateFrom = value; CreateFilter(); RaisePropertyChanged("DateFrom"); }
        }

        private DateTime? _dateTo;
        public DateTime? DateTo
        {
            get { return _dateTo; }
            set { _dateTo = value; CreateFilter(); RaisePropertyChanged("DateTo"); }
        }

        private string _versionList;
        public string VersionList
        {
            get { return _versionList; }
            set { _versionList = value; CreateFilter(); RaisePropertyChanged("VersionList"); }
        }

        private bool _customFilter;
        public bool CustomFilter
        {
            get { return _customFilter; }
            set { _customFilter = value; RaisePropertyChanged("CustomFilter"); }
        }

        private string _filter;
        public string Filter
        {
            get { return _filter; }
            set { _filter = value; RaisePropertyChanged("Filter"); }
        }

        private string _importFileName;
        public string ImportFileName
        {
            get { return _importFileName; }
            set { _importFileName = value; RaisePropertyChanged("ImportFileName"); }
        }

        private void CreateFilter()
        {
            Filter = ExportFilter.Create(
                _modified,
                _dateFrom,
                _dateTo,
                _versionList,
                _customFilter,
                _filter);
        }

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
