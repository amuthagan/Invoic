using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class MFMDevelopmentModel : ViewModelBase
    {
        private bool _awaitingDoc = true;
        public bool AwaitingDoc
        {
            get { return _awaitingDoc; }
            set
            {
                _awaitingDoc = value;
                NotifyPropertyChanged("AwaitingDoc");
            }
        }

        private bool _awaitingTools = false;
        public bool AwaitingTools
        {
            get { return _awaitingTools; }
            set
            {
                _awaitingTools = value;
                NotifyPropertyChanged("AwaitingTools");
            }
        }

        private bool _awaitingForging = false;
        public bool AwaitingForging
        {
            get { return _awaitingForging; }
            set
            {
                _awaitingForging = value;
                NotifyPropertyChanged("AwaitingForging");
            }
        }

        private bool _awaitingSecondary = false;
        public bool AwaitingSecondary
        {
            get { return _awaitingSecondary; }
            set
            {
                _awaitingSecondary = value;
                NotifyPropertyChanged("AwaitingSecondary");
            }
        }

        private bool _awaitingPPAP = false;
        public bool AwaitingPPAP
        {
            get { return _awaitingPPAP; }
            set
            {
                _awaitingPPAP = value;
                NotifyPropertyChanged("AwaitingPPAP");
            }
        }

        private bool _awaitingPSWApproval = false;
        public bool AwaitingPSWApproval
        {
            get { return _awaitingPSWApproval; }
            set
            {
                _awaitingPSWApproval = value;
                NotifyPropertyChanged("AwaitingPSWApproval");
            }
        }

        private bool _pswApproved = false;
        public bool PSWApproved
        {
            get { return _pswApproved; }
            set
            {
                _pswApproved = value;
                NotifyPropertyChanged("PSWApproved");
            }
        }

        private bool _noofShifts = false;
        public bool NoOfShifts
        {
            get { return _noofShifts; }
            set
            {
                _noofShifts = value;
                NotifyPropertyChanged("NoOfShifts");
            }
        }

        private bool _firstTimeRight = false;
        public bool FirstTimeRight
        {
            get { return _firstTimeRight; }
            set
            {
                _firstTimeRight = value;
                NotifyPropertyChanged("FirstTimeRight");
            }
        }

        private bool _customerComp = false;
        public bool CustomerComp
        {
            get { return _customerComp; }
            set
            {
                _customerComp = value;
                NotifyPropertyChanged("CustomerComp");
            }
        }

        private bool _leadTime = false;
        public bool LeadTime
        {
            get { return _leadTime; }
            set
            {
                _leadTime = value;
                NotifyPropertyChanged("LeadTime");
            }
        }

        private DataView _dvLocation = null;
        public DataView DVLocation
        {
            get { return _dvLocation; }
            set
            {
                _dvLocation = value;
                NotifyPropertyChanged("DVLocation");
            }
        }

        private DataView _dvCustomer = null;
        public DataView DVCustomer
        {
            get { return _dvCustomer; }
            set
            {
                _dvCustomer = value;
                NotifyPropertyChanged("DVCustomer");
            }
        }

        private String _cust_code = "";
        public String CUST_CODE
        {
            get { return _cust_code; }
            set
            {
                _cust_code = value;
                NotifyPropertyChanged("CUST_CODE");
            }
        }

        private String _loc_code = "";
        public String LOC_CODE
        {
            get { return _loc_code; }
            set
            {
                _loc_code = value;
                NotifyPropertyChanged("LOC_CODE");
            }
        }

        private Nullable<DateTime> _start_date = null;
        public Nullable<DateTime> START_DATE
        {
            get { return _start_date; }
            set
            {
                _start_date = value;
                NotifyPropertyChanged("START_DATE");
            }
        }

        private Nullable<DateTime> _end_date = null;
        public Nullable<DateTime> END_DATE
        {
            get { return _end_date; }
            set
            {
                _end_date = value;
                NotifyPropertyChanged("END_DATE");
            }
        }

        private String _stage_start = "";
        public String STAGE_START
        {
            get { return _stage_start; }
            set
            {
                _stage_start = value;
                NotifyPropertyChanged("STAGE_START");
            }
        }

        private String _stage_end = "";
        public String STAGE_END
        {
            get { return _stage_end; }
            set
            {
                _stage_end = value;
                NotifyPropertyChanged("STAGE_END");
            }
        }

        private String _target_time = "";
        public String TARGET_TIME
        {
            get { return _target_time; }
            set
            {
                _target_time = value;
                NotifyPropertyChanged("TARGET_TIME");
            }
        }

        private DataView _dvLeadTime = null;
        public DataView DVLeadTime
        {
            get { return _dvLeadTime; }
            set
            {
                _dvLeadTime = value;
                NotifyPropertyChanged("DVLeadTime");
            }
        }

        private DataTable _dtLeadTimePrint = null;
        public DataTable DTLeadTimePrint
        {
            get { return _dtLeadTimePrint; }
            set
            {
                _dtLeadTimePrint = value;
                NotifyPropertyChanged("DTLeadTimePrint");
            }
        }

        private DataView _dvPSW = null;
        public DataView DVPSW
        {
            get { return _dvPSW; }
            set
            {
                _dvPSW = value;
                NotifyPropertyChanged("DVPSW");
            }
        }

        
        private String _pswcount = "";
        public String PSWCount
        {
            get { return _pswcount; }
            set
            {
                _pswcount = value;
                NotifyPropertyChanged("PSWCount");
            }
        }

        private DataTable _dtPSWPrint = null;
        public DataTable DTPSWPrint
        {
            get { return _dtPSWPrint; }
            set
            {
                _dtPSWPrint = value;
                NotifyPropertyChanged("DTPSWPrint");
            }
        }
    }
}
