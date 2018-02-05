using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAVObjectCompareWinClient.ViewModel
{
    public class CompareViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const string _dateHeaderName = "Date";

        private string _prettyNameA;
        private string _prettyNameB;

        private string _dateNameA;
        private string _dateNameB;

        public string PrettyNameA
        {
            get { return _prettyNameA; }
            set
            {
                _prettyNameA = value;
                SetHeaderNameProperties();
            }
        }
        public string PrettyNameB
        {
            get { return _prettyNameB; }
            set
            {
                _prettyNameB = value;
                SetHeaderNameProperties();
            }
        }

        private void SetHeaderNameProperties()
        {
            _dateNameA = string.Format("{0} {1}", _dateHeaderName, this.PrettyNameA);
            NotifyPropertyChanged("DateNameA");

            _dateNameB = string.Format("{0} {1}", _dateHeaderName, this.PrettyNameB);
            NotifyPropertyChanged("DateNameB");
        }
        public string DateNameA { get { return _dateNameA; }  }
        public string DateNameB { get { return _dateNameB; } }

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
