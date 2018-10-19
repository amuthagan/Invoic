using System.ComponentModel;
using System.Data;

namespace ProcessDesigner.Model
{
    public class CostSheetSearchModel : INotifyPropertyChanged
    {
        private string _cIReference;
        private int _ci_info_pk;
        private string _prodDesc;
        private string _custName;
        private string _custDwgNo;
        private string _partNo;
        private string _responsibility;
        private string _loccode;

        private bool _chkShowAll;
        private bool _chkShowPending;
        private bool _chkPendingPartNoAllocation;
        private bool _chkShowExpertOnly;
        private bool _chkShowDomesticOnly;

        private DataView _dtCostSheetSearchMaster;
        public event PropertyChangedEventHandler PropertyChanged;

        public string CI_REFERENCE
        {
            get { return _cIReference; }
            set
            {
                _cIReference = value;
            }
        }

        public int CI_INFO_PK
        {
            get { return _ci_info_pk; }
            set
            {
                _ci_info_pk = value;
            }
        }

        public string PROD_DESC
        {
            get { return _prodDesc; }
            set
            {
                _prodDesc = value;
            }
        }

        public string CUST_DWG_NO
        {
            get { return _custDwgNo; }
            set
            {
                _custDwgNo = value;
            }
        }

        public string CUST_NAME
        {
            get { return _custName; }
            set
            {
                _custName = value;
            }
        }

        public string PART_NO
        {
            get { return _partNo; }
            set
            {
                _partNo = value;
            }
        }

        public string RESPONSIBILITY
        {
            get { return _responsibility; }
            set
            {
                _responsibility = value;
            }
        }

        public string LOCATION
        {
            get { return _loccode; }
            set
            {
                _loccode = value;
            }
        }

        public bool ChkShowDomesticOnly
        {
            get { return _chkShowDomesticOnly; }
            set
            {
                _chkShowDomesticOnly = value;
            }
        }

        public bool ChkShowExpertOnly
        {
            get { return _chkShowExpertOnly; }
            set
            {
                _chkShowExpertOnly = value;
            }
        }

        public bool ChkPendingPartNoAllocation
        {
            get { return _chkPendingPartNoAllocation; }
            set
            {
                _chkPendingPartNoAllocation = value;
            }
        }

        public bool ChkShowPending
        {
            get { return _chkShowPending; }
            set
            {
                _chkShowPending = value;
            }
        }

        public bool ChkShowAll
        {
            get { return _chkShowAll; }
            set
            {
                _chkShowAll = value;
            }
        }
        public DataView CostSheetSearchMasterDetails
        {
            get { return _dtCostSheetSearchMaster; }
            set
            {
                _dtCostSheetSearchMaster = value;
                NotifyPropertyChanged("CostSheetSearchMasterDetails");
            }
        }

        private DataView _dtCustName;
        public DataView CustName
        {
            get { return _dtCustName; }
            set
            {
                _dtCustName = value;
                NotifyPropertyChanged("CustName");
            }
        }

        private DataView _dtCustDwgNo;
        public DataView CustDwgNo
        {
            get { return _dtCustDwgNo; }
            set
            {
                _dtCustDwgNo = value;
                NotifyPropertyChanged("CustDwgNo");
            }
        }

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
