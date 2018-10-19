using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class PartSubmissionWarrantModel : ViewModelBase
    {

        //public Action CloseAction { get; set; }

        private string _partno;
        public string PART_NO
        {
            get { return _partno; }
            set { _partno = value; }
        }

        private string _partname;
        public string PART_DESC
        {
            get { return _partname; }
            set { _partname = value; }
        }

        private string _customerpartno;
        public string CUST_DWG_NO
        {
            get { return _customerpartno; }
            set { _customerpartno = value; }
        }

        private string _showndrawingno;
        public string SHOWNDRAWINGNO
        {
            get { return _showndrawingno; }
            set { _showndrawingno = value; }
        }

        private string _ex_no;
        public string EX_NO
        {
            get { return _ex_no; }
            set { _ex_no = value; }
        }

        private string _revision_no;
        public string REVISION_NO
        {
            get { return _revision_no; }
            set { _revision_no = value; }
        }

        private string _weight;
        public string WEIGHT
        {
            get { return _weight; }
            set { _weight = value; }
        }

        private string _checkaidno;
        public string CHECKAIDNO
        {
            get { return _checkaidno; }
            set { _checkaidno = value; }
        }

        private string _engineeringchangelvl;
        public string CUST_DWG_NO_ISSUE
        {
            get { return _engineeringchangelvl; }
            set { _engineeringchangelvl = value; }
        }

        private string _checkaidengglvl;
        public string CHECKAIDENGGLVL
        {
            get { return _checkaidengglvl; }
            set { _checkaidengglvl = value; }
        }

        private string _addenggchange;
        public string ADDENGGCHANGE
        {
            get { return _addenggchange; }
            set { _addenggchange = value; }
        }

        private string _purchaseorderno;
        public string PURCHASEORDERNO
        {
            get { return _purchaseorderno; }
            set { _purchaseorderno = value; }
        }

        private string _customername;
        public string CUSTOMERNAME
        {
            get { return _customername; }
            set { _customername = value; }
        }

        private string _buyer;
        public string BUYER
        {
            get { return _buyer; }
            set { _buyer = value; }
        }

        //private string _application;
        //public string Application
        //{
        //    get { return _application; }
        //    set { _application = value; }
        //}
        private DateTime? _date1;
        public DateTime? CUST_STD_DATE
        {
            get { return _date1; }
            set
            {
                _date1 = value;
                NotifyPropertyChanged("CUST_STD_DATE");
            }
        }

        //private DateTime _date1;
        //public DateTime CUST_STD_DATE
        //{
        //    get { return _date1; }
        //    set { _date1 = value; }
        //}

        private string _date2;
        public string DATE2
        {
            get { return _date2; }
            set { _date2 = value; }
        }

        private string _date3;
        public string DATE3
        {
            get { return _date3; }
            set { _date3 = value; }
        }

        //private System.Nullable<System.DateTime> _date1;
        //public System.Nullable<System.DateTime> DATE1
        //{
        //    get { return _date2; }
        //    set
        //    {
        //        if ((this._date1 != value))
        //        {

        //            this._date1 = value;
        //            this.NotifyPropertyChanged("DATE1");

        //        }
        //    }
        //}

        private string _putname = "";
        public string PUTNAME
        {
            get { return _putname; }
            set
            {
                _putname = value;
                NotifyPropertyChanged("PUTNAME");

            }
        }

        private string _putapplication = "";
        public string PUTAPPLICATION
        {
            get { return _putapplication; }
            set
            {
                _putapplication = value;
                NotifyPropertyChanged("PUTAPPLICATION");

            }
        }

        //private DataView _name;
        //public DataView NAME
        //{
        //    get { return _name; }
        //    set
        //    {
        //        _name = value;
        //        NotifyPropertyChanged("NAME");
        //    }
        //}

        private string _title;
        public string TITLE
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged("TITLE");

            }
        }

        private string _runrate;
        public string RUNRATE
        {
            get { return _runrate; }
            set { _runrate = value; }
        }

        private bool _safetyyes;
        public bool SAFETYYES
        {
            get { return _safetyyes; }
            set { _safetyyes = value; }
        }

        private bool _safetyno = true;
        public bool SAFETYNO
        {
            get { return _safetyno; }
            set { _safetyno = value; }
        }

        private bool _dimensional = true;
        public bool DIMENSIONAL
        {
            get { return _dimensional; }
            set { _dimensional = value; }
        }

        private bool _materials = true;
        public bool MATERIALS
        {
            get { return _materials; }
            set { _materials = value; }
        }

        private bool _appearance;
        public bool APPEARANCE
        {
            get { return _appearance; }
            set { _appearance = value; }
        }

        private bool _reportablesubstanceyes;
        public bool REPORTABLESUBSTANCEYES
        {
            get { return _reportablesubstanceyes; }
            set { _reportablesubstanceyes = value; }
        }

        private bool _reportablesubstanceno = true;
        public bool REPORTABLESUBSTANCENO
        {
            get { return _reportablesubstanceno; }
            set { _reportablesubstanceno = value; }
        }

        private bool _reportablesubstancenotapplicable;
        public bool REPORTABLESUBSTANCENOTAPPLICABLE
        {
            get { return _reportablesubstancenotapplicable; }
            set { _reportablesubstancenotapplicable = value; }
        }

        private bool _plasticpartyes;
        public bool PLASTICPARTYES
        {
            get { return _plasticpartyes; }
            set { _plasticpartyes = value; }
        }

        private bool _plasticpartno;
        public bool PLASTICPARTNO
        {
            get { return _plasticpartno; }
            set { _plasticpartno = value; }
        }

        private bool _plasticpartnotapplicable = true;
        public bool PLASTICPARTNOTAPPLICABLE
        {
            get { return _plasticpartnotapplicable; }
            set { _plasticpartnotapplicable = value; }
        }

        private bool _initialsubmission = true;
        public bool INITIALSUBMISSION
        {
            get { return _initialsubmission; }
            set { _initialsubmission = value; }
        }

        private bool _engineeringchanges;
        public bool ENGINEERINGCHANGES
        {
            get { return _engineeringchanges; }
            set { _engineeringchanges = value; }
        }

        private bool _tooling;
        public bool TOOLING
        {
            get { return _tooling; }
            set { _tooling = value; }
        }

        private bool _correctiondiscrepancy;
        public bool CORRECTIONDISCREPANCY
        {
            get { return _correctiondiscrepancy; }
            set { _correctiondiscrepancy = value; }
        }

        private bool _toolinginactive;
        public bool TOOLINGINACTIVE
        {
            get { return _toolinginactive; }
            set { _toolinginactive = value; }
        }

        private bool _optionalconstruction;
        public bool OPTIONALCONSTRUCTION
        {
            get { return _optionalconstruction; }
            set { _optionalconstruction = value; }
        }

        private bool _supplier;
        public bool SUPPLIER
        {
            get { return _supplier; }
            set { _supplier = value; }
        }

        private bool _partprocessing;
        public bool PARTPROCESSING
        {
            get { return _partprocessing; }
            set { _partprocessing = value; }
        }

        private bool _partproduced;
        public bool PARTPRODUCED
        {
            get { return _partproduced; }
            set { _partproduced = value; }
        }

        private bool _other;
        public bool OTHER
        {
            get { return _other; }
            set { _other = value; }
        }

        private string _othertxt;
        public string OTHERTXT
        {
            get { return _othertxt; }
            set { _othertxt = value; }
        }

        private bool _rsl1;
        public bool RSL1
        {
            get { return _rsl1; }
            set { _rsl1 = value; }
        }

        private bool subsupplier;
        public bool SUBSUPPLIER
        {
            get { return subsupplier; }
            set { subsupplier = value; }
        }
        private bool _rsl2;
        public bool RSL2
        {
            get { return _rsl2; }
            set { _rsl2 = value; }
        }

        private bool _rsl3 = true;
        public bool RSL3
        {
            get { return _rsl3; }
            set { _rsl3 = value; }
        }

        private bool _rsl4;
        public bool RSL4
        {
            get { return _rsl4; }
            set { _rsl4 = value; }
        }

        private bool _rsl5;
        public bool RSL5
        {
            get { return _rsl5; }
            set { _rsl5 = value; }
        }

        private bool _dimensionalmeasurements = true;
        public bool DIMENSIONALMEASUREMENTS
        {
            get { return _dimensionalmeasurements; }
            set { _dimensionalmeasurements = value; }
        }

        private bool _materialandfunctionaltests = true;
        public bool MATERIALANDFUNCTIONALTESTS
        {
            get { return _materialandfunctionaltests; }
            set { _materialandfunctionaltests = value; }
        }

        private bool _appearancecriteria;
        public bool APPEARANCECRITERIA
        {
            get { return _appearancecriteria; }
            set { _appearancecriteria = value; }
        }

        private bool _statisticalprocesspackage;
        public bool STATISTICALPROCESSPACKAGE
        {
            get { return _statisticalprocesspackage; }
            set { _statisticalprocesspackage = value; }
        }

        private bool _statisticalprocesspackageyes = true;
        public bool STATISTICALPROCESSPACKAGEYES
        {
            get { return _statisticalprocesspackageyes; }
            set { _statisticalprocesspackageyes = value; }
        }

        private bool _statisticalprocesspackageno;
        public bool STATISTICALPROCESSPACKAGENO
        {
            get { return _statisticalprocesspackageno; }
            set { _statisticalprocesspackageno = value; }
        }

        private bool _csqsryes;
        public bool CSQSRYES
        {
            get { return _csqsryes; }
            set
            {
                _csqsryes = value;
            }
        }

        private bool _csqsrno = true;
        public bool CSQSRNO
        {
            get { return _csqsrno; }
            set
            {
                _csqsrno = value;
            }
        }

        private string _loc;
        public string LOC
        {
            get { return _loc; }
            set { _loc = value; }
        }

        private string _address;
        public string ADDRESS
        {
            get { return _address; }
            set { _address = value; }
        }

        private string _phoneno;
        public string PHONENO
        {
            get { return _phoneno; }
            set { _phoneno = value; }
        }

        private string _faxno;
        public string FAXNO
        {
            get { return _faxno; }
            set { _faxno = value; }
        }
    }
}
