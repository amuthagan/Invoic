using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class ISIRModel
    {
        private string _partno;
        public string PART_NO
        {
            get { return _partno; }
            set { _partno = value; }
        }

        private string _customer;
        public string CUST_NAME
        {
            get { return _customer; }
            set { _customer = value; }
        }

        private string _description;
        public string DESCRIPTION
        {
            get { return _description; }
            set { _description = value; }
        }

        //private System.Nullable<System.DateTime> _date;
        //public System.Nullable<System.DateTime> DATE
        //{
        //    get { return _date; }
        //    set
        //    {
        //        if ((this._date != value))
        //        {

        //            this._date = value;
        //            //this.NotifyPropertyChanged("DATE");

        //        }
        //    }
        //}

        private Nullable<DateTime> _date;
        public Nullable<DateTime> DATE
        {
            get { return _date; }
            set { _date = value; }
        }

        private string _date1;
        public string DATE1
        {
            get { return _date1; }
            set { _date1 = value; }
        }

        private string _noofsample;
        public string NOOFSAMPLE
        {
            get { return _noofsample; }
            set { _noofsample = value; }
        }

        private string _specificreq;
        public string SPECIFICREQ
        {
            get { return _specificreq; }
            set { _specificreq = value; }
        }

        private string _heattreatment;
        public string HEATTREATMENT
        {
            get { return _heattreatment; }
            set { _heattreatment = value; }
        }

        private string _otherspecification;
        public string OTHERSPECIFICATION
        {
            get { return _otherspecification; }
            set { _otherspecification = value; }
        }

        private bool _partsubmission = true;
        public bool PARTSUBMISSION
        {
            get { return _partsubmission; }
            set { _partsubmission = value; }
        }

        private bool _materialtestresults = true;
        public bool MATERIALTESTRESULTS
        {
            get { return _materialtestresults; }
            set { _materialtestresults = value; }
        }

        private bool _dimensionaltest = true;
        public bool DIMENSIONALTEST
        {
            get { return _dimensionaltest; }
            set { _dimensionaltest = value; }
        }

        private bool _controlplan = true;
        public bool CONTROLPLAN
        {
            get { return _controlplan; }
            set { _controlplan = value; }
        }

        private bool _materialtestcertificate = true;
        public bool MATERIALTESTCERTIFICATE
        {
            get { return _materialtestcertificate; }
            set { _materialtestcertificate = value; }
        }

        private bool _processflow = true;
        public bool PROCESSFLOW
        {
            get { return _processflow; }
            set { _processflow = value; }
        }

        private bool _performancetest = true;
        public bool PERFORMANCETEST
        {
            get { return _performancetest; }
            set { _performancetest = value; }
        }

        private bool _processFMEA;
        public bool PROCESSFMEA
        {
            get { return _processFMEA; }
            set { _processFMEA = value; }
        }
    }
}
