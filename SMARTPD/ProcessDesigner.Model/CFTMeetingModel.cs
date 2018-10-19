using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using System.ComponentModel.DataAnnotations;
using System.Data;
namespace ProcessDesigner.Model
{
    public class CFTMeetingModel : ViewModelBase
    {
        private DataView _dtCtm1 = null;
        public DataView DtCtm1
        {
            get { return _dtCtm1; }
            set
            {
                _dtCtm1 = value;
                NotifyPropertyChanged("DtCtm1");
            }

        }
        private DataView _dtCtm2;
        public DataView DtCtm2
        {
            get { return _dtCtm2; }
            set
            {
                _dtCtm2 = value;
                NotifyPropertyChanged("DtCtm2");
            }
        }
        private DataView _dtCtm3;
        public DataView DtCtm3
        {
            get { return _dtCtm3; }
            set
            {
                _dtCtm3 = value;
                NotifyPropertyChanged("DtCtm3");
            }
        }
        private DataView _dtCtm4;
        public DataView DtCtm4
        {
            get { return _dtCtm4; }
            set
            {
                _dtCtm4 = value;
                NotifyPropertyChanged("DtCtm4");
            }
        }
        private DataView _dtCtm5;
        public DataView DtCtm5
        {
            get { return _dtCtm5; }
            set
            {
                _dtCtm5 = value;
                NotifyPropertyChanged("DtCtm5");
            }
        }
        private DataView _dtCtm6;
        public DataView DtCtm6
        {
            get { return _dtCtm6; }
            set
            {
                _dtCtm6 = value;
                NotifyPropertyChanged("DtCtm6");
            }
        }
        private DataView _dtCtm7;
        public DataView DtCtm7
        {
            get { return _dtCtm7; }
            set
            {
                _dtCtm7 = value;
                NotifyPropertyChanged("DtCtm7");
            }
        }

        private DataView _dtPartNumber;
        public DataView DtPartNumber
        {
            get { return _dtPartNumber; }
            set
            {
                _dtPartNumber = value;
                NotifyPropertyChanged("DtPartNumber");
            }
        }

        private DataView _dtSpecialCharacteristics;
        public DataView DtSpecialCharacteristics
        {
            get { return _dtSpecialCharacteristics; }
            set
            {
                _dtSpecialCharacteristics = value;
                NotifyPropertyChanged("DtSpecialCharacteristics");
            }
        }
        private DataView _dtTimigChart;
        public DataView DtTimigChart
        {
            get { return _dtTimigChart; }
            set
            {
                _dtTimigChart = value;
                NotifyPropertyChanged("DtTimigChart");
            }
        }
        private DataView _dtMidGrid;
        public DataView DtMidGrid
        {
            get { return _dtMidGrid; }
            set
            {
                _dtMidGrid = value;
                NotifyPropertyChanged("DtMidGrid");
            }
        }

        private string _ctm1 = "";
        public string Ctm1
        {
            get { return _ctm1; }
            set
            {
                _ctm1 = value;
                NotifyPropertyChanged("Ctm1");
            }
        }
        private string _ctm2 = "";
        public string Ctm2
        {
            get { return _ctm2; }
            set
            {
                _ctm2 = value;
                NotifyPropertyChanged("Ctm2");
            }
        }
        private string _ctm3 = "";
        public string Ctm3
        {
            get { return _ctm3; }
            set
            {
                _ctm3 = value;
                NotifyPropertyChanged("Ctm3");
            }
        }
        private string _ctm4 = "";
        public string Ctm4
        {
            get { return _ctm4; }
            set
            {
                _ctm4 = value;
                NotifyPropertyChanged("Ctm4");
            }
        }
        private string _ctm5 = "";
        public string Ctm5
        {
            get { return _ctm5; }
            set
            {
                _ctm5 = value;
                NotifyPropertyChanged("Ctm5");
            }
        }
        private string _ctm6 = "";
        public string Ctm6
        {
            get { return _ctm6; }
            set
            {
                _ctm6 = value;
                NotifyPropertyChanged("Ctm6");
            }
        }
        private string _ctm7 = "";
        public string Ctm7
        {
            get { return _ctm7; }
            set
            {
                _ctm7 = value;
                NotifyPropertyChanged("Ctm7");
            }
        }
        private string _partNo = "";
        public string PartNo
        {
            get { return _partNo; }
            set
            {
                _partNo = value;
                NotifyPropertyChanged("PartNo");
            }
        }
        private string _partDesc = "";
        public string PartDesc
        {
            get { return _partDesc; }
            set
            {
                _partDesc = value;
                NotifyPropertyChanged("PartDesc");
            }
        }
        private string _rmSpec = "";
        public string RMSpec
        {
            get { return _rmSpec; }
            set
            {
                _rmSpec = value;
                NotifyPropertyChanged("RMSpec");
            }
        }
        private string _packingSpec = "";
        public string PackingSpec
        {
            get { return _packingSpec; }
            set
            {
                _packingSpec = value;
                NotifyPropertyChanged("PackingSpec");
            }
        }
        private string _cheesWt = "";
        public string CheesWt
        {
            get { return _cheesWt; }
            set
            {
                _cheesWt = value;
                NotifyPropertyChanged("CheesWt");
            }
        }
        private string _finishWt = "";
        public string FinishWt
        {
            get { return _finishWt; }
            set
            {
                _finishWt = value;
                NotifyPropertyChanged("FinishWt");
            }
        }
        private string _customer = "";
        public string Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                NotifyPropertyChanged("Customer");
            }
        }
        private string _annualRequirments = "";
        public string AnnualRequirments
        {
            get { return _annualRequirments; }
            set
            {
                _annualRequirments = value;
                NotifyPropertyChanged("AnnualRequirments");
            }
        }
        private string _ppapProductionQty = "";
        public string PPAPProductionQty
        {
            get { return _ppapProductionQty; }
            set
            {
                _ppapProductionQty = value;
                NotifyPropertyChanged("PPAPProductionQty");
            }
        }
        private string _ppapSampleQty = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Sample Qty is Required")]
        public string PPAPSampleQty
        {
            get { return _ppapSampleQty; }
            set
            {
                _ppapSampleQty = value;
                NotifyPropertyChanged("PPAPSampleQty");
            }
        }
        private string _remarks = "";
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                _remarks = value;
                NotifyPropertyChanged("Remarks");
            }
        }
        private string _custPartNo = "";
        public string CustPartNo
        {
            get { return _custPartNo; }
            set
            {
                _custPartNo = value;
                NotifyPropertyChanged("CustPartNo");
            }
        }
        private DateTime? _custRequiredDate = null;
        public DateTime? CustRequiredDate
        {
            get { return _custRequiredDate; }
            set
            {
                _custRequiredDate = value;
                NotifyPropertyChanged("CustRequiredDate");
            }
        }
        private string _custPartName = "";
        public string CustPartName
        {
            get { return _custPartName; }
            set
            {
                _custPartName = value;
                NotifyPropertyChanged("CustPartName");
            }
        }
        private string _location = "";
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                NotifyPropertyChanged("Location");
            }
        }
        private DateTime? _cftMeetingIssueDate = null;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Meeting Issue Date is Required")]
        public DateTime? CFTMeetingIssueDate
        {
            get { return _cftMeetingIssueDate; }
            set
            {
                _cftMeetingIssueDate = value;
                NotifyPropertyChanged("CFTMeetingIssueDate");
            }
        }
        private string _cftMeetingIssueNo = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Meeting Issue No is Required")]
        public string CFTMeetingIssueNo
        {
            get { return _cftMeetingIssueNo; }
            set
            {
                _cftMeetingIssueNo = value;
                NotifyPropertyChanged("CFTMeetingIssueNo");
            }
        }
        private DateTime? _cftMeetingDate = null;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Meeting Date is Required")]
        public DateTime? CFTMeetingDate
        {
            get { return _cftMeetingDate; }
            set
            {
                _cftMeetingDate = value;
                NotifyPropertyChanged("CFTMeetingDate");
            }
        }
        private string _application = "";
        public string Application
        {
            get { return _application; }
            set
            {
                _application = value;
                NotifyPropertyChanged("Application");
            }
        }
        private string _pg = "";
        public string PG
        {
            get { return _pg; }
            set
            {
                _pg = value;
                NotifyPropertyChanged("PG");
            }
        }
        private string _isSafetyPart = "";
        public string IsSafetyPart
        {
            get { return _isSafetyPart; }
            set
            {
                _isSafetyPart = value;
                NotifyPropertyChanged("IsSafetyPart");
            }
        }
        private string _isProtoType = "";
        public string IsProtoType
        {
            get { return _isProtoType; }
            set
            {
                _isProtoType = value;
                NotifyPropertyChanged("IsProtoType");
            }
        }
        private bool _rbtnM = false;
        public bool rbtnMIsChecked
        {
            get { return _rbtnM; }
            set
            {
                _rbtnM = value;
                NotifyPropertyChanged("rbtnMIsChecked");
            }
        }
        private bool _rbtnK = false;
        public bool rbtnKIsChecked
        {
            get { return _rbtnK; }
            set
            {
                _rbtnK = value;
                NotifyPropertyChanged("rbtnKIsChecked");
            }
        }
        private bool _rbtnY = false;
        public bool rbtnYIsChecked
        {
            get { return _rbtnY; }
            set
            {
                _rbtnY = value;
                NotifyPropertyChanged("rbtnYIsChecked");
            }
        }
        private bool _chkSPYesIsChecked = false;
        public bool ChkSPYesIsChecked
        {
            get { return _chkSPYesIsChecked; }
            set
            {
                _chkSPYesIsChecked = value;
                NotifyPropertyChanged("ChkSPYesIsChecked");
            }
        }
        private bool _chkSPNoIsChecked = true;
        public bool ChkSPNoIsChecked
        {
            get { return _chkSPNoIsChecked; }
            set
            {
                _chkSPNoIsChecked = value;
                NotifyPropertyChanged("ChkSPNoIsChecked");
            }
        }
        private bool _chkPTYesIsChecked = false;
        public bool ChkPTYesIsChecked
        {
            get { return _chkPTYesIsChecked; }
            set
            {
                _chkPTYesIsChecked = value;
                NotifyPropertyChanged("ChkPTYesIsChecked");
            }
        }
        private bool _chkPTNoIsChecked = true;
        public bool ChkPTNoIsChecked
        {
            get { return _chkPTNoIsChecked; }
            set
            {
                _chkPTNoIsChecked = value;
                NotifyPropertyChanged("ChkPTNoIsChecked");
            }
        }
    }
}
