using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ProcessDesigner.Model
{
    public class DDPerformanceSummaryModel : ViewModelBase
    {

        /// <summary>
        /// Last Month Cost Sheet Received
        /// </summary>
        private string _cSR_LM;
        public string CSR_LM
        {
            get { return _cSR_LM; }
            set
            {
                _cSR_LM = value;
                NotifyPropertyChanged("CSR_LM");
            }
        }

        /// <summary>
        /// This Month Cost Sheet Received
        /// </summary>
        private string _cSR_TM;
        public string CSR_TM
        {
            get { return _cSR_TM; }
            set
            {
                _cSR_TM = value;
                NotifyPropertyChanged("CSR_TM");
            }
        }

        /// <summary>
        /// This Year Cost Sheet Received
        /// </summary>
        private string _cSR_TY;
        public string CSR_TY
        {
            get { return _cSR_TY; }
            set
            {
                _cSR_TY = value;
                NotifyPropertyChanged("CSR_TY");
            }
        }

        /// <summary>
        /// Last Year Cost Sheet Received
        /// </summary>
        private string _cSR_LY;
        public string CSR_LY
        {
            get { return _cSR_LY; }
            set
            {
                _cSR_LY = value;
                NotifyPropertyChanged("CSR_LY");
            }
        }

        /// <summary>
        /// Last Month Cost Sheet Completed
        /// </summary>
        private string _cSC_LM;
        public string CSC_LM
        {
            get { return _cSC_LM; }
            set
            {
                _cSC_LM = value;
                NotifyPropertyChanged("CSC_LM");
            }
        }

        /// <summary>
        /// This Month Cost Sheet Completed
        /// </summary>
        private string _cSC_TM;
        public string CSC_TM
        {
            get { return _cSC_TM; }
            set
            {
                _cSC_TM = value;
                NotifyPropertyChanged("CSC_TM");
            }
        }

        /// <summary>
        /// This Year Cost Sheet Completed
        /// </summary>
        private string _cSC_TY;
        public string CSC_TY
        {
            get { return _cSC_TY; }
            set
            {
                _cSC_TY = value;
                NotifyPropertyChanged("CSC_TY");
            }
        }

        /// <summary>
        /// Last Year Cost Sheet Completed
        /// </summary>
        private string _cSC_LY;
        public string CSC_LY
        {
            get { return _cSC_LY; }
            set
            {
                _cSC_LY = value;
                NotifyPropertyChanged("CSC_LY");
            }
        }

        /// <summary>
        /// Last Month Part No Allotted
        /// </summary>
        private string _pNA_LM;
        public string PNA_LM
        {
            get { return _pNA_LM; }
            set
            {
                _pNA_LM = value;
                NotifyPropertyChanged("PNA_LM");
            }
        }

        /// <summary>
        /// This Month Part No Allotted
        /// </summary>
        private string _pNA_TM;
        public string PNA_TM
        {
            get { return _pNA_TM; }
            set
            {
                _pNA_TM = value;
                NotifyPropertyChanged("PNA_TM");
            }
        }

        /// <summary>
        /// This Year Part No Allotted
        /// </summary>
        private string _pNA_TY;
        public string PNA_TY
        {
            get { return _pNA_TY; }
            set
            {
                _pNA_TY = value;
                NotifyPropertyChanged("PNA_TY");
            }
        }

        /// <summary>
        /// Last Year Part No Allotted
        /// </summary>
        private string _pNA_LY;
        public string PNA_LY
        {
            get { return _pNA_LY; }
            set
            {
                _pNA_LY = value;
                NotifyPropertyChanged("PNA_LY");
            }
        }

        /// <summary>
        /// Last Month Document Released
        /// </summary>
        private string _dR_LM;
        public string DR_LM
        {
            get { return _dR_LM; }
            set
            {
                _dR_LM = value;
                NotifyPropertyChanged("DR_LM");
            }
        }

        /// <summary>
        /// This Month Document Released
        /// </summary>
        private string _dR_TM;
        public string DR_TM
        {
            get { return _dR_TM; }
            set
            {
                _dR_TM = value;
                NotifyPropertyChanged("DR_TM");
            }
        }

        /// <summary>
        /// This Year Document Released
        /// </summary>
        private string _dR_TY;
        public string DR_TY
        {
            get { return _dR_TY; }
            set
            {
                _dR_TY = value;
                NotifyPropertyChanged("DR_TY");
            }
        }

        /// <summary>
        /// Last Year Document Released
        /// </summary>
        private string _dR_LY;
        public string DR_LY
        {
            get { return _dR_LY; }
            set
            {
                _dR_LY = value;
                NotifyPropertyChanged("DR_LY");
            }
        }

        /// <summary>
        /// Last Month Samples Submitted
        /// </summary>
        private string _sS_LM;
        public string SS_LM
        {
            get { return _sS_LM; }
            set
            {
                _sS_LM = value;
                NotifyPropertyChanged("SS_LM");
            }
        }

        /// <summary>
        /// This Month Samples Submitted
        /// </summary>
        private string _sS_TM;
        public string SS_TM
        {
            get { return _sS_TM; }
            set
            {
                _sS_TM = value;
                NotifyPropertyChanged("SS_TM");
            }
        }

        /// <summary>
        /// This Year Samples Submitted
        /// </summary>
        private string _sS_TY;
        public string SS_TY
        {
            get { return _sS_TY; }
            set
            {
                _sS_TY = value;
                NotifyPropertyChanged("SS_TY");
            }
        }

        /// <summary>
        /// Last Year Samples Submitted
        /// </summary>
        private string _sS_LY;
        public string SS_LY
        {
            get { return _sS_LY; }
            set
            {
                _sS_LY = value;
                NotifyPropertyChanged("SS_LY");
            }
        }

        /// <summary>
        /// Cost Sheet Pending Domestic
        /// </summary>
        private string _cSPD;
        public string CSPD
        {
            get { return _cSPD; }
            set
            {
                _cSPD = value;
                NotifyPropertyChanged("CSPD");
            }
        }

        /// <summary>
        /// Cost Sheet Pending Export
        /// </summary>
        private string _cSPE;
        public string CSPE
        {
            get { return _cSPE; }
            set
            {
                _cSPE = value;
                NotifyPropertyChanged("CSPE");
            }
        }

        /// <summary>
        /// Part No Allotment Pending
        /// </summary>
        private string _pNAP;
        public string PNAP
        {
            get { return _pNAP; }
            set
            {
                _pNAP = value;
                NotifyPropertyChanged("PNAP");
            }
        }


    }
}
