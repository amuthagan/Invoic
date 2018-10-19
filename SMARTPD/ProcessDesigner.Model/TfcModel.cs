using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class TfcModel : ViewModelBase
    {
        private string _partNo = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Part Number is required.")]
        public string PartNo
        {
            get { return _partNo; }
            set
            {
                _partNo = value;
                NotifyPropertyChanged("PartNo");
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

        private DataView _gridload;
        public DataView GRIDLOAD
        {
            get { return _gridload; }
            set
            {
                _gridload = value;
                NotifyPropertyChanged("GRIDLOAD");
            }
        }

        private DataView _gridloadrisk;
        public DataView GRIDLOADRISK
        {
            get { return _gridloadrisk; }
            set
            {
                _gridloadrisk = value;
                NotifyPropertyChanged("GRIDLOADRISK");
            }
        }

        private string _issueno;
        public string PRD_ISSUE_NO
        {
            get { return _issueno; }
            set
            {
                _issueno = value;
                NotifyPropertyChanged("PRD_ISSUE_NO");
            }
        }

        private string _pcrno;
        public string PCR_NO
        {
            get { return _pcrno; }
            set
            {
                _pcrno = value;
                NotifyPropertyChanged("PCR_NO");
            }
        }

        private string _impact1;
        public string IMPACT1
        {
            get { return _impact1; }
            set
            {
                _impact1 = value;
                NotifyPropertyChanged("IMPACT1");
            }
        }

        private string _impact2;
        public string IMPACT2
        {
            get { return _impact2; }
            set
            {
                _impact2 = value;
                NotifyPropertyChanged("IMPACT2");
            }
        }

        private string _impact3;
        public string IMPACT3
        {
            get { return _impact3; }
            set
            {
                _impact3 = value;
                NotifyPropertyChanged("IMPACT3");
            }
        }
        private string _impact4;
        public string IMPACT4
        {
            get { return _impact4; }
            set
            {
                _impact4 = value;
                NotifyPropertyChanged("IMPACT4");
            }
        }
        private string _impact5;
        public string IMPACT5
        {
            get { return _impact5; }
            set
            {
                _impact5 = value;
                NotifyPropertyChanged("IMPACT5");
            }
        }
        private string _impact6;
        public string IMPACT6
        {
            get { return _impact6; }
            set
            {
                _impact6 = value;
                NotifyPropertyChanged("IMPACT6");
            }
        }
        private string _impact7;
        public string IMPACT7
        {
            get { return _impact7; }
            set
            {
                _impact7 = value;
                NotifyPropertyChanged("IMPACT7");
            }
        }
        private string _remarks1;
        public string REMARKS1
        {
            get { return _remarks1; }
            set
            {
                _remarks1 = value;
                NotifyPropertyChanged("REMARKS1");
            }
        }
        private string _remarks2;
        public string REMARKS2
        {
            get { return _remarks2; }
            set
            {
                _remarks2 = value;
                NotifyPropertyChanged("REMARKS2");
            }
        }
        private string _remarks3;
        public string REMARKS3
        {
            get { return _remarks3; }
            set
            {
                _remarks3 = value;
                NotifyPropertyChanged("REMARKS3");
            }
        }
        private string _remarks4;
        public string REMARKS4
        {
            get { return _remarks4; }
            set
            {
                _remarks4 = value;
                NotifyPropertyChanged("REMARKS4");
            }
        }
        private string _remarks5;
        public string REMARKS5
        {
            get { return _remarks5; }
            set
            {
                _remarks5 = value;
                NotifyPropertyChanged("REMARKS5");
            }
        }
        private string _remarks6;
        public string REMARKS6
        {
            get { return _remarks6; }
            set
            {
                _remarks6 = value;
                NotifyPropertyChanged("REMARKS6");
            }
        }
        private string _remarks7;
        public string REMARKS7
        {
            get { return _remarks7; }
            set
            {
                _remarks7 = value;
                NotifyPropertyChanged("REMARKS7");
            }
        }
        private DateTime? _pcrdate;
        public DateTime? PCR_DATE
        {
            get { return _pcrdate; }
            set
            {
                _pcrdate = value;
                NotifyPropertyChanged("PCR_DATE");
            }
        }

        private string _issuedate;
        public string PRD_ISSUE_DATE
        {
            get { return _issuedate; }
            set
            {
                _issuedate = value;
                NotifyPropertyChanged("PRD_ISSUE_DATE");
            }
        }

        private string _custissueno;
        public string CUST_DWG_ISSUE_NO
        {
            get { return _custissueno; }
            set
            {
                _custissueno = value;
                NotifyPropertyChanged("CUST_DWG_ISSUE_NO");
            }
        }

        private string _custstddate;
        public string CUST_STD_DATE
        {
            get { return _custstddate; }
            set
            {
                _custstddate = value;
                NotifyPropertyChanged("CUST_STD_DATE");
            }
        }

        private string _autoPart;
        public string AUTO_PART
        {
            get { return _autoPart; }
            set
            {
                _autoPart = value;
                NotifyPropertyChanged("AUTO_PART");
            }
        }

        private bool _autoPartYes;
        public bool AUTOPARTYES
        {
            get { return _autoPartYes; }
            set
            {
                _autoPartYes = value;
                NotifyPropertyChanged("AUTOPARTYES");
            }
        }

        private bool _autoPartNo;
        public bool AUTOPARTNO
        {
            get { return _autoPartNo; }
            set
            {
                _autoPartNo = value;
                NotifyPropertyChanged("AUTOPARTNO");
            }
        }

        private string _partDesc;
        public string PartDesc
        {
            get { return _partDesc; }
            set
            {
                _partDesc = value;
                NotifyPropertyChanged("PartDesc");
            }
        }

        private string _customerPartNo;
        public string CUSTOMERPARTNO
        {
            get { return _customerPartNo; }
            set
            {
                _customerPartNo = value;
                NotifyPropertyChanged("CUSTOMERPARTNO");
            }
        }

        private string _customerName;
        public string CUSTOMERNAME
        {
            get { return _customerName; }
            set
            {
                _customerName = value;
                NotifyPropertyChanged("CUSTOMERNAME");
            }
        }

        private string _application;
        public string APPLICATION
        {
            get { return _application; }
            set
            {
                _application = value;
                NotifyPropertyChanged("APPLICATION");
            }
        }

        private string _date;
        public string DATE
        {
            get { return _date; }
            set
            {
                _date = value;
                NotifyPropertyChanged("DATE");
            }
        }

        private string _allow;
        public string ALLOW
        {
            get { return _allow; }
            set
            {
                _allow = value;
                NotifyPropertyChanged("ALLOW");
            }
        }

        private string _routeno;
        public string ROUTENO
        {
            get { return _routeno; }
            set
            {
                _routeno = value;
                NotifyPropertyChanged("ROUTENO");
            }
        }

        private string _custprog;
        public string CUST_PROG
        {
            get { return _custprog; }
            set
            {
                _custprog = value;
                NotifyPropertyChanged("CUST_PROG");
            }
        }

        private bool _protoType;
        public bool PROTOTYPE
        {
            get { return _protoType; }
            set
            {
                _protoType = value;
                NotifyPropertyChanged("PROTOTYPE");
            }
        }

        private bool _preLaunch = true;
        public bool PRELAUNCH
        {
            get { return _preLaunch; }
            set
            {
                _preLaunch = value;
                NotifyPropertyChanged("PRELAUNCH");
            }
        }

        private bool _production;
        public bool PRODUCTION
        {
            get { return _production; }
            set
            {
                _production = value;
                NotifyPropertyChanged("PRODUCTION");
            }
        }

        private bool _feasibleProduct;
        public bool FEASIBLEPRODUCT
        {
            get { return _feasibleProduct; }
            set
            {
                _feasibleProduct = value;
                NotifyPropertyChanged("FEASIBLEPRODUCT");
            }
        }

        private bool _feasibleChange;
        public bool FEASIBLECHANGE
        {
            get { return _feasibleChange; }
            set
            {
                _feasibleChange = value;
                NotifyPropertyChanged("FEASIBLECHANGE");
            }
        }

        private bool _notfeasible;
        public bool NOTFEASIBLE
        {
            get { return _notfeasible; }
            set
            {
                _notfeasible = value;
                NotifyPropertyChanged("NOTFEASIBLE");
            }
        }

        private string _q1;
        public string Q1
        {
            get { return _q1; }
            set { _q1 = value; }
        }

        private string _q2;
        public string Q2
        {
            get { return _q2; }
            set { _q2 = value; }
        }

        private string _q3;
        public string Q3
        {
            get { return _q3; }
            set { _q3 = value; }
        }

        private string _q4;
        public string Q4
        {
            get { return _q4; }
            set { _q4 = value; }
        }

        private string _q5;
        public string Q5
        {
            get { return _q5; }
            set { _q5 = value; }
        }

        private string _q6;
        public string Q6
        {
            get { return _q6; }
            set { _q6 = value; }
        }

        private string _q7;
        public string Q7
        {
            get { return _q7; }
            set { _q7 = value; }
        }

        private string _q8;
        public string Q8
        {
            get { return _q8; }
            set { _q8 = value; }
        }

        private string _q9;
        public string Q9
        {
            get { return _q9; }
            set { _q9 = value; }
        }

        private string _q10;
        public string Q10
        {
            get { return _q10; }
            set { _q10 = value; }
        }

        private string _q11;
        public string Q11
        {
            get { return _q11; }
            set { _q11 = value; }
        }

        private string _q12;
        public string Q12
        {
            get { return _q12; }
            set { _q12 = value; }
        }

        private string _q13;
        public string Q13
        {
            get { return _q13; }
            set { _q13 = value; }
        }

        private string _q14;
        public string Q14
        {
            get { return _q14; }
            set { _q14 = value; }
        }

        private string _conclusion;
        public string CONCLUSION
        {
            get { return _conclusion; }
            set
            {
                _conclusion = value;
                NotifyPropertyChanged("CONCLUSION");
            }
        }
    }
}
