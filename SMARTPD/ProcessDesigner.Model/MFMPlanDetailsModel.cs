using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class MFMPlanDetailsModel : ViewModelBase
    {
        private bool _isDocumentation = false;
        public bool IsDocumentation
        {
            get { return _isDocumentation; }
            set
            {
                _isDocumentation = value;
                NotifyPropertyChanged("IsDocumentation");
            }
        }

        private bool _isForging = false;
        public bool IsForging
        {
            get { return _isForging; }
            set
            {
                _isForging = value;
                NotifyPropertyChanged("IsForging");
            }
        }

        private bool _isPPAP = false;
        public bool IsPPAP
        {
            get { return _isPPAP; }
            set
            {
                _isPPAP = value;
                NotifyPropertyChanged("IsPPAP");
            }
        }

        private bool _isTools = false;
        public bool IsTools
        {
            get { return _isTools; }
            set
            {
                _isTools = value;
                NotifyPropertyChanged("IsTools");
            }
        }

        private bool _isSecondary = false;
        public bool IsSecondary
        {
            get { return _isSecondary; }
            set
            {
                _isSecondary = value;
                NotifyPropertyChanged("IsSecondary");
            }
        }

        private bool _isPSWAwaiting = false;
        public bool IsAwaitingPSW
        {
            get { return _isPSWAwaiting; }
            set
            {
                _isPSWAwaiting = value;
                NotifyPropertyChanged("IsAwaitingPSW");
            }
        }

        private bool _isPSWApproved = false;
        public bool IsApprovedPSW
        {
            get { return _isPSWApproved; }
            set
            {
                _isPSWApproved = value;
                NotifyPropertyChanged("IsApprovedPSW");
            }
        }

        private string _groupHeader = "Awaiting";
        public string GroupHeader
        {
            get { return _groupHeader; }
            set
            {
                _groupHeader = value;
                NotifyPropertyChanged("GroupHeader");
            }
        }

        private string _productCount = "0";
        public string ProductCount
        {
            get { return _productCount; }
            set
            {
                _productCount = value;
                NotifyPropertyChanged("ProductCount");
            }
        }

        private string _month = "";
        public string Month
        {
            get { return _month; }
            set
            {
                _month = value;
                NotifyPropertyChanged("Month");
            }
        }

        private DataView _dvMFMPlanDetails = null;
        public DataView DVMFMPlanDetails
        {
            get { return _dvMFMPlanDetails; }
            set
            {
                _dvMFMPlanDetails = value;
                NotifyPropertyChanged("DVMFMPlanDetails");
            }
        }
    }
}
