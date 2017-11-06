using NAVObjectCompare.ExportFinexe;
using NAVObjectCompareWinClient.Configurations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompareWinClient.Model
{
    public class ImportFilesModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ServerSetupModel> ServerSetups { get; private set; }

        // A Start
        private string _selectedServerSetupA;
        public string SelectedServerSetupA
        {
            get { return _selectedServerSetupA; }
            set { _selectedServerSetupA = value; RaisePropertyChanged("SelectedServerSetupA"); }
        }

        private bool _modifiedA;
        public bool ModifiedA
        {
            get { return _modifiedA; }
            set { _modifiedA = value; CreateFilterA(); RaisePropertyChanged("ModifiedA"); }
        }

        private DateTime? _dateFromA;
        public DateTime? DateFromA
        {
            get { return _dateFromA; }
            set { _dateFromA = value; CreateFilterA(); RaisePropertyChanged("DateFromA"); }
        }

        private DateTime? _dateToA;
        public DateTime? DateToA
        {
            get { return _dateToA; }
            set { _dateToA = value; CreateFilterA(); RaisePropertyChanged("DateToA"); }
        }

        private string _versionListA;
        public string VersionListA
        {
            get { return _versionListA; }
            set { _versionListA = value; CreateFilterA(); RaisePropertyChanged("VersionListA"); }
        }

        private bool _customFilterA;
        public bool CustomFilterA
        {
            get { return _customFilterA; }
            set { _customFilterA = value; RaisePropertyChanged("CustomFilterA"); }
        }

        private string _filterA;
        public string FilterA
        {
            get { return _filterA; }
            set { _filterA = value; RaisePropertyChanged("FilterA"); }
        }
        // A Stop

        // B Start
        private string _selectedServerSetupB;
        public string SelectedServerSetupB
        {
            get { return _selectedServerSetupB; }
            set { _selectedServerSetupB = value; RaisePropertyChanged("SelectedServerSetupB"); }
        }

        private bool _modifiedB;
        public bool ModifiedB
        {
            get { return _modifiedB; }
            set { _modifiedB = value; CreateFilterB(); RaisePropertyChanged("ModifiedB"); }
        }

        private DateTime? _dateFromB;
        public DateTime? DateFromB
        {
            get { return _dateFromB; }
            set { _dateFromB = value; CreateFilterB(); RaisePropertyChanged("DateFromB"); }
        }

        private DateTime? _dateToB;
        public DateTime? DateToB
        {
            get { return _dateToB; }
            set { _dateToB = value; CreateFilterB(); RaisePropertyChanged("DateToB"); }
        }

        private string _versionListB;
        public string VersionListB
        {
            get { return _versionListB; }
            set { _versionListB = value; CreateFilterB(); RaisePropertyChanged("VersionListB"); }
        }

        private bool _customFilterB;
        public bool CustomFilterB
        {
            get { return _customFilterB; }
            set { _customFilterB = value; RaisePropertyChanged("CustomFilterB"); }
        }

        private string _filterB;
        public string FilterB
        {
            get { return _filterB; }
            set { _filterB = value; RaisePropertyChanged("FilterB"); }
        }
        // B Stop

        private void CreateFilterA()
        {
            FilterA = ExportFilter.Create(
                _modifiedA,
                _dateFromA,
                _dateToA,
                _versionListA,
                _customFilterA,
                _filterA);
        }

        private void CreateFilterB()
        {
            FilterB = ExportFilter.Create(
                _modifiedB,
                _dateFromB,
                _dateToB,
                _versionListB,
                _customFilterB,
                _filterB);
        }

        public ImportFilesModel()
        {
            ServerSetups = ServerSetupConfiguration.GetServerSetups();
        }

        public void RefreshServerSetups()
        {
            ServerSetups = ServerSetupConfiguration.GetServerSetups();
            RaisePropertyChanged("ServerSetups");
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
