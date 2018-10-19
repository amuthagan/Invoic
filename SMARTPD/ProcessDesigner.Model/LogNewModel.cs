using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class LogNewModel
    {
        private string _partno;
        public string PART_NO
        {
            get { return _partno; }
            set
            {
                _partno = value;
                //NotifyPropertyChanged("PART_NO");
            }
        }

        private string _uName;
        public string UNAME
        {
            get { return _uName; }
            set
            {
                _uName = value;
                //NotifyPropertyChanged("UNAME");
            }
        }

        private string _acc_Date;
        public string ACC_DATE
        {
            get { return _acc_Date; }
            set
            {
                _acc_Date = value;
                //NotifyPropertyChanged("ACC_DATE");
            }
        }

        private string _ipAddress;
        public string IPADDRESS
        {
            get { return _ipAddress; }
            set
            {
                _ipAddress = value;
                //NotifyPropertyChanged("IPADDRESS");
            }
        }

        private string _sheet_accessed;
        public string SHEET_ACCESSED
        {
            get { return _sheet_accessed; }
            set
            {
                _sheet_accessed = value;
                //NotifyPropertyChanged("SHEET_ACCESSED");
            }
        }

    }
}
