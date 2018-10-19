using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;

namespace ProcessDesigner.BLL
{
    public class CrossLinkingcharGrid : INotifyPropertyChanged
    {
        private string _subtype = string.Empty;
        private string _featurecode = string.Empty;
        private string _type = string.Empty;
        private string _subtypes = string.Empty;
        private string _linkedwith = string.Empty;
        private decimal _sno;
        private string _prdcode = string.Empty;

        public string SubType
        {
            get { return _subtype; }
            set
            {
                _subtype = value;
                NotifyPropertyChanged("SubType");
            }
        }
        public string FeatureCode
        {
            get { return _featurecode; }
            set
            {
                _featurecode = value;
                NotifyPropertyChanged("FeatureCode");
            }
        }
        public string Type
        {
            get { return _type; }
            set
            {
                _subtype = value;
                NotifyPropertyChanged("Type");
            }
        }
        public string SubTypes
        {
            get { return _subtypes; }
            set
            {
                _subtypes = value;
                NotifyPropertyChanged("SubTypes");
            }
        }
        public string LinkedWith
        {
            get { return _linkedwith; }
            set
            {
                _linkedwith = value;
                NotifyPropertyChanged("LinkedWith");
            }
        }
        public decimal Sno
        {
            get { return _sno; }
            set
            {
                _sno = value;
                NotifyPropertyChanged("Sno");
            }
        }
        public string PrdCode
        {
            get { return _prdcode; }
            set
            {
                _prdcode = value;
                NotifyPropertyChanged("PrdCode");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
