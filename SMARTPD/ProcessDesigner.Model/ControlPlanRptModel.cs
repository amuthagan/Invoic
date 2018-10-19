using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class ControlPlanRptModel : ViewModelBase
    {
        private string _partNo = "";
        private string _seqNo = "";
        private string _routeNo = "";
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
         [Required(AllowEmptyStrings = false, ErrorMessage = "Sequence Number is Required")]
        public string SeqNo
        {
            get { return _seqNo; }
            set
            {
                _seqNo = value;
                NotifyPropertyChanged("SeqNo");
            }
        }
         [Required(AllowEmptyStrings = false, ErrorMessage = "Process Number is Required")]
        public string RouteNo
        {
            get { return _routeNo; }
            set
            {
                _routeNo = value;
                NotifyPropertyChanged("RouteNo");
            }
        }


        private string _controlPlanNo = "";
        public string ControlPlanNo
        {
            get { return _controlPlanNo; }
            set
            {
                _controlPlanNo = value;
                NotifyPropertyChanged("ControlPlanNo");
            }
        }

        private string _keyContactPerson = "";
        public string KeyContactPerson
        {
            get { return _keyContactPerson; }
            set
            {
                _keyContactPerson = value;
                NotifyPropertyChanged("KeyContactPerson");
            }
        }
        private string _fax = "";
        public string Fax
        {
            get { return _fax; }
            set
            {
                _fax = value;
                NotifyPropertyChanged("Fax");
            }
        }
        private string _phone = "";
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                NotifyPropertyChanged("Phone");
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
        /// <summary>
        /// ////////////
        /// </summary>
        /// 
        private DataView _dtKeyContactPerson;
        public DataView dtKeyContactPerson
        {
            get { return _dtKeyContactPerson; }
            set
            {
                _dtKeyContactPerson = value;
                NotifyPropertyChanged("dtKeyContactPerson");
            }
        }
        private DataView _dtFax;
        public DataView DtFax
        {
            get { return _dtFax; }
            set
            {
                _dtFax = value;
                NotifyPropertyChanged("DtFax");
            }
        }
        private DataView _dtSeqNumber;
        public DataView DtSeqNumber
        {
            get { return _dtSeqNumber; }
            set
            {
                _dtSeqNumber = value;
                NotifyPropertyChanged("DtSeqNumber");
            }
        }
        private DataView _dtphone;
        public DataView DtPhone
        {
            get { return _dtphone; }
            set
            {
                _dtphone = value;
                NotifyPropertyChanged("DtPhone");
            }
        }

        private DataView _dtCtm1;
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
        private DateTime? _supplierApprDate;
        public DateTime? SupplierApprDate
        {
            get { return _supplierApprDate; }
            set
            {
                _supplierApprDate = value;
                NotifyPropertyChanged("SupplierApprDate");
            }
        }
        private DateTime? _otherApprDate;
        public DateTime? OtherApprDate
        {
            get { return _otherApprDate; }
            set
            {
                _otherApprDate = value;
                NotifyPropertyChanged("OtherApprDate");
            }
        }
        private string _controlPlanType = "";
        public string ControlPlanType
        {
            get { return _controlPlanType; }
            set
            {
                _controlPlanType = value;
                NotifyPropertyChanged("ControlPlanType");
            }
        }
    }
    public enum Member_Dept
    {
        DESIGN,
        MKT,
        PRODU,
        QUALITY,
        PROCESS,
        OTHERS
    }
}
