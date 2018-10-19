using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class MachineBookingModel : ViewModelBase
    {
        private string _pART_NO;
        private string _plant;
        private string _fromDate;
        private string _toDate;
        private string _mACHINE_NAME;
        private string _pG_CATEGORY;
        private string _cOST_CENT_CODE;
        private string _pPAP_PLAN;
        private string _dOC_REL_DT_ACTUAL;
        private string _tOOLS_READY_ACTUAL_DT;
        private string _fORGING_ACTUAL_DT;
        private string _sECONDARY_ACTUAL_DT;
        private string _sAMP_SUBMIT_DATE;
        private DataView _dtPlantDetails;

        public string PART_NO
        {
            get { return _pART_NO; }
            set
            {
                _pART_NO = value;
            }
        }

        public string PLANT
        {
            get { return _plant; }
            set
            {
                _plant = value;
            }
        }


        public DataView PlantDetails
        {
            get { return _dtPlantDetails; }
            set
            {
                _dtPlantDetails = value;
                NotifyPropertyChanged("PlantDetails");
            }
        }

        private Nullable<DateTime> _fromdate;
        public Nullable<DateTime> FROMDATE
        {
            get { return _fromdate; }
            set
            {
                _fromdate = value;
                NotifyPropertyChanged("FROMDATE");
            }
        }

        private Nullable<DateTime> _todate;
        public Nullable<DateTime> TODATE
        {
            get { return _todate; }
            set
            {
                _todate = value;
                NotifyPropertyChanged("TODATE");
            }
        }

        //public string FROMDATE
        //{
        //    get { return _fromDate; }
        //    set
        //    {
        //        _fromDate = value;
        //    }
        //}

        //public string TODATE
        //{
        //    get { return _toDate; }
        //    set
        //    {
        //        _toDate = value;
        //    }
        //}

        public string MACHINE_NAME
        {
            get { return _mACHINE_NAME; }
            set
            {
                _mACHINE_NAME = value;
            }
        }

        public string PG_CATEGORY
        {
            get { return _pG_CATEGORY; }
            set
            {
                _pG_CATEGORY = value;
            }
        }

        public string COST_CENT_CODE
        {
            get { return _cOST_CENT_CODE; }
            set
            {
                _cOST_CENT_CODE = value;
            }
        }

        public string PPAP_PLAN
        {
            get { return _pPAP_PLAN; }
            set
            {
                _pPAP_PLAN = value;
            }
        }

        public string DOC_REL_DT_ACTUAL
        {
            get { return _dOC_REL_DT_ACTUAL; }
            set
            {
                _dOC_REL_DT_ACTUAL = value;
            }
        }

        public string TOOLS_READY_ACTUAL_DT
        {
            get { return _tOOLS_READY_ACTUAL_DT; }
            set
            {
                _tOOLS_READY_ACTUAL_DT = value;
            }
        }

        public string FORGING_ACTUAL_DT
        {
            get { return _fORGING_ACTUAL_DT; }
            set
            {
                _fORGING_ACTUAL_DT = value;
            }
        }

        public string SECONDARY_ACTUAL_DT
        {
            get { return _sECONDARY_ACTUAL_DT; }
            set
            {
                _sECONDARY_ACTUAL_DT = value;
            }
        }

        public string SAMP_SUBMIT_DATE
        {
            get { return _sAMP_SUBMIT_DATE; }
            set
            {
                _sAMP_SUBMIT_DATE = value;
            }
        }
    }
}
