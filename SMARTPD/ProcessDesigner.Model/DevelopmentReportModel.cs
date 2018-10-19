using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProcessDesigner.Model
{
    public class DevelopmentReportModel : ViewModelBase
    {
        private string _partNo = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Part Number is Required")]
        public string PartNo
        {
            get { return _partNo; }
            set
            {
                _partNo = value;
                NotifyPropertyChanged("PartNo");
            }
        }

        private string _partNoDesc = "";
        public string PartNoDesc
        {
            get { return _partNoDesc; }
            set
            {
                _partNoDesc = value;
                NotifyPropertyChanged("PartNoDesc");
            }
        }
        private string _runNo = "1";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Run Number is Required")]
        public string RunNo
        {
            get { return _runNo; }
            set
            {
                _runNo = value;
                NotifyPropertyChanged("RunNo");
            }
        }

        private DataView _dtPartNoDetails;
        public DataView PartNoDetails
        {
            get { return _dtPartNoDetails; }
            set
            {
                _dtPartNoDetails = value;
                NotifyPropertyChanged("PartNoDetails");
            }
        }

        private DataView _dtRunNoDetails;
        public DataView RunNoDetails
        {
            get { return _dtRunNoDetails; }
            set
            {
                _dtRunNoDetails = value;
                NotifyPropertyChanged("RunNoDetails");
            }
        }
        private DateTime? _runDate;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Run Date is Required")]
        public DateTime? RunDate
        {
            get { return _runDate; }
            set
            {
                _runDate = value;
                NotifyPropertyChanged("RunDate");
            }
        }

        private DataView _dtDevMainDetails;
        public DataView DevMainDetails
        {
            get { return _dtDevMainDetails; }
            set
            {
                _dtDevMainDetails = value;
                NotifyPropertyChanged("DevMainDetails");
            }
        }

        private DataView _dtDesignAssumptionDetails;
        public DataView DesignAssumptionDetails
        {
            get { return _dtDesignAssumptionDetails; }
            set
            {
                _dtDesignAssumptionDetails = value;
                NotifyPropertyChanged("DesignAssumptionDetails");
            }
        }

        private DataView _dtLogDetails;
        public DataView LogDetails
        {
            get { return _dtLogDetails; }
            set
            {
                _dtLogDetails = value;
                NotifyPropertyChanged("LogDetails");
            }
        }

        private DataView _dtShortClosureDetails;
        public DataView ShortClosureDetails
        {
            get { return _dtShortClosureDetails; }
            set
            {
                _dtShortClosureDetails = value;
                NotifyPropertyChanged("ShortClosureDetails");
            }
        }

        private string _recordOfCFTDiscussion = "";
        public string RecordOfCFTDiscussion
        {
            get { return _recordOfCFTDiscussion; }
            set
            {
                _recordOfCFTDiscussion = value;
                NotifyPropertyChanged("RecordOfCFTDiscussion");
            }
        }

        private bool _doYouHaveCustComplaint = false;
        public bool IsDoYouHaveCustComplaint
        {
            get { return _doYouHaveCustComplaint; }
            set
            {
                _doYouHaveCustComplaint = value;
                NotifyPropertyChanged("IsDoYouHaveCustComplaint");
            }
        }

        private string _labelNatureOfComplaint = "";
        public string LabelNatureOfComplaint
        {
            get { return _labelNatureOfComplaint; }
            set
            {
                _labelNatureOfComplaint = value;
                NotifyPropertyChanged("LabelNatureOfComplaint");
            }
        }

        private string _natureOfComplaint = "";
        public string NatureOfComplaint
        {
            get { return _natureOfComplaint; }
            set
            {
                _natureOfComplaint = value;
                NotifyPropertyChanged("NatureOfComplaint");
            }
        }

        private string _dadRep = "";
        public string DADRep
        {
            get { return _dadRep; }
            set
            {
                _dadRep = value;
                NotifyPropertyChanged("DADRep");
            }
        }

        private string _zapRep = "";
        public string ZapRep
        {
            get { return _zapRep; }
            set
            {
                _zapRep = value;
                NotifyPropertyChanged("ZapRep");
            }
        }

        private string _noOfForginShift = "";
        public string NoOfForginShift
        {
            get { return _noOfForginShift; }
            set
            {
                _noOfForginShift = value;
                NotifyPropertyChanged("NoOfForginShift");
            }
        }

        private string _toolRWDesignTotal = "";
        public string ToolRWDesignTotal
        {
            get { return _toolRWDesignTotal; }
            set
            {
                _toolRWDesignTotal = value;
                NotifyPropertyChanged("ToolRWDesignTotal");
            }
        }

        private string _toolRWMfgTotal = "";
        public string ToolRWMfgTotal
        {
            get { return _toolRWMfgTotal; }
            set
            {
                _toolRWMfgTotal = value;
                NotifyPropertyChanged("ToolRWMfgTotal");
            }
        }
    }
}
