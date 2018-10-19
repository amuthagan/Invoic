using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class AuditReportModel : ViewModelBase
    {
        private bool nineDigitPartNo = false;
        public bool NineDigitPartNo
        {
            get { return nineDigitPartNo; }
            set
            {
                nineDigitPartNo  = value;               
                NotifyPropertyChanged("NineDigitPartNo");
                if (value) SixDigitPartNo = false;
            }
        }

        private bool _sixDigitPartNo = false;
        public bool SixDigitPartNo
        {
            get { return _sixDigitPartNo; }
            set
            {
                _sixDigitPartNo = value;
                NotifyPropertyChanged("SixDigitPartNo");
                if (value) NineDigitPartNo = false;
            }
        }

        private DataView _dvauditreport = null;
        public DataView DVAuditReport
        {
            get { return _dvauditreport; }
            set
            {
                _dvauditreport = value;
                NotifyPropertyChanged("DVAuditReport");
            }
        }
    }
}
