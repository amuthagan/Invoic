using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class QcpModel : ViewModelBase
    {
        private DataView _dtPartNoDetails;
        private DataView _dtRouteNoDetails;
        private DataView _dtSeqNoDetails;
        private DataView _dtPCCSDetails;
        private DataView _dtPccsRevisionDetails;

        private DataView _dtCmbFeature;
        private DataView _dtCmbProcess;
        private DataView _dtCmbSplChar;
        private DataView _dtCmbControl;
        private string _partNo = "";
        private string _partDesc = "";
        private string _seqNo = "";
        private string _operdesc = "";
        private decimal _routeNo;

        public string PartNo
        {
            get { return _partNo; }
            set
            {
                _partNo = value;
                NotifyPropertyChanged("PartNo");
            }
        }

        public string SeqNo
        {
            get { return _seqNo; }
            set
            {
                _seqNo = value;
                NotifyPropertyChanged("SeqNo");
            }
        }

        public string OperDesc
        {
            get { return _operdesc; }
            set
            {
                _operdesc = value;
                NotifyPropertyChanged("OperDesc");
            }
        }

        public decimal RouteNo
        {
            get { return _routeNo; }
            set
            {
                _routeNo = value;
                NotifyPropertyChanged("RouteNo");
            }
        }

        public string PartDesc
        {
            get { return _partDesc; }
            set
            {
                _partDesc = value;
                NotifyPropertyChanged("PartDesc");
            }
        }

        private string _editGenBtn = "Generate F5";
        public string EditGenBtn
        {
            get { return _editGenBtn; }
            set
            {
                _editGenBtn = value;
                NotifyPropertyChanged("EditGenBtn");
            }
        }

        public DataView PartNoDetails
        {
            get { return _dtPartNoDetails; }
            set
            {
                _dtPartNoDetails = value;
                NotifyPropertyChanged("PartNoDetails");
            }
        }

        public DataView RouteNoDetails
        {
            get { return _dtRouteNoDetails; }
            set
            {
                _dtRouteNoDetails = value;
                NotifyPropertyChanged("RouteNoDetails");
            }
        }

        public DataView SeqNoDetails
        {
            get { return _dtSeqNoDetails; }
            set
            {
                _dtSeqNoDetails = value;
                NotifyPropertyChanged("SeqNoDetails");
            }
        }

        public DataView PCCSDetails
        {
            get { return _dtPCCSDetails; }
            set
            {
                _dtPCCSDetails = value;
                NotifyPropertyChanged("PCCSDetails");
            }
        }

        public DataView PccsRevisionDetails
        {
            get { return _dtPccsRevisionDetails; }
            set
            {
                _dtPccsRevisionDetails = value;
                NotifyPropertyChanged("PccsRevisionDetails");
            }
        }
        public DataView FeatureCmb
        {
            get { return _dtCmbFeature; }
            set
            {
                _dtCmbFeature = value;
                NotifyPropertyChanged("FeatureCmb");
            }
        }
        public DataView Process
        {
            get { return _dtCmbProcess; }
            set
            {
                _dtCmbProcess = value;
                NotifyPropertyChanged("Process");
            }
        }
        public DataView SplChar
        {
            get { return _dtCmbSplChar; }
            set
            {
                _dtCmbSplChar = value;
                NotifyPropertyChanged("SplChar");
            }
        }
        public DataView Control
        {
            get { return _dtCmbControl; }
            set
            {
                _dtCmbControl = value;
                NotifyPropertyChanged("Control");
            }
        }

        private string bif_proj;
        public string BIF_PROJ
        {
            get { return bif_proj; }
            set
            {
                bif_proj = value;
                NotifyPropertyChanged("BIF_PROJ");
            }
        }

        private string part_desc;
        public string PART_DESC
        {
            get { return part_desc; }
            set
            {
                part_desc = value;
                NotifyPropertyChanged("PART_DESC");
            }
        }

        private string cust_name;
        public string CUST_NAME
        {
            get { return cust_name; }
            set
            {
                cust_name = value;
                NotifyPropertyChanged("CUST_NAME");
            }
        }

        private DateTime? enqu_recd_on;
        public DateTime? ENQU_RECD_ON
        {
            get { return enqu_recd_on; }
            set
            {
                enqu_recd_on = value;
                NotifyPropertyChanged("ENQU_RECD_ON");
            }
        }

        private string cust_dwg_no;
        public string CUST_DWG_NO
        {
            get { return cust_dwg_no; }
            set
            {
                cust_dwg_no = value;
                NotifyPropertyChanged("CUST_DWG_NO");
            }
        }
    }
}
