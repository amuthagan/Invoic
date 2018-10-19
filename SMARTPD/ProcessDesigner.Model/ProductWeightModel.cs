using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class ProductWeightModel : ViewModelBase
    {
        private string _cireference = "";
        private string _weightoption = "";
        private double _total;
        private double _cheesweight;
        private string _mode = "";
        private string _status = "";
        private DataView _dvshape = null;
        private DataView _dvshapedetails = null;
        private DataTable dtDeletedRecords = null;

        private int _ciref_no_fk = -1;
        public int CIREF_NO_FK
        {
            get { return _ciref_no_fk; }
            set
            {
                _ciref_no_fk = value;
                NotifyPropertyChanged("CIREF_NO_FK");
            }
        }

        public string CIreference
        {
            get { return _cireference; }
            set
            {
                _cireference = value;
                NotifyPropertyChanged("CIreference");
            }
        }


        public string WeightOption
        {
            get { return _weightoption; }
            set
            {
                _weightoption = value;
                NotifyPropertyChanged("WeightOption");
            }
        }

        public double Total
        {
            get { return _total; }
            set
            {
                _total = value;
                NotifyPropertyChanged("Total");
            }
        }

        public double CheesWeight
        {
            get { return _cheesweight; }
            set
            {
                _cheesweight = value;
                NotifyPropertyChanged("CheesWeight");
            }
        }

        public string Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                NotifyPropertyChanged("Mode");
            }
        }

        public DataView DVShape
        {
            get { return _dvshape; }
            set
            {
                _dvshape = value;
                NotifyPropertyChanged("DVShape");
            }
        }

        public DataView DVShapeDetails
        {
            get { return _dvshapedetails; }
            set
            {
                _dvshapedetails = value;
                NotifyPropertyChanged("DVShapeDetails");
            }
        }

        public DataTable DTDeletedRecords
        {
            get { return dtDeletedRecords; }
            set
            {
                dtDeletedRecords = value;
                NotifyPropertyChanged("DTDeletedRecords");
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

    }
}
