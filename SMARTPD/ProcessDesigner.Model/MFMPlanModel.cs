using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class MFMPlanModel : ViewModelBase
    {
        private string _part_no;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Part Number is required.")]
        public string PART_NO
        {
            get { return _part_no; }
            set
            {
                _part_no = value;
                NotifyPropertyChanged("PART_NO");
            }
        }

        private string _part_desc;
        public string PART_DESC
        {
            get { return _part_desc; }
            set
            {
                _part_desc = value;
                NotifyPropertyChanged("PART_DESC");
            }
        }

        private DataView _dvProductMaster = null;
        public DataView DVProductMaster
        {
            get { return _dvProductMaster; }
            set
            {
                _dvProductMaster = value;
                NotifyPropertyChanged("DVProductMaster");
            }
        }

        private DataView _dvUsers = null;
        public DataView DVUsers
        {
            get { return _dvUsers; }
            set
            {
                _dvUsers = value;
                NotifyPropertyChanged("DVUsers");
            }
        }

        private string _cust_name;
        public string CUST_NAME
        {
            get { return _cust_name; }
            set
            {
                _cust_name = value;
                NotifyPropertyChanged("CUST_NAME");
            }
        }

        private Nullable<DateTime> _doc_rel_dt_plan;
        public Nullable<DateTime> DOC_REL_DT_PLAN
        {
            get { return _doc_rel_dt_plan; }
            set
            {
                _doc_rel_dt_plan = value;
                NotifyPropertyChanged("DOC_REL_DT_PLAN");
            }
        }

        private Nullable<DateTime> _doc_rel_dt_actual;
        public Nullable<DateTime> DOC_REL_DT_ACTUAL
        {
            get { return _doc_rel_dt_actual; }
            set
            {
                _doc_rel_dt_actual = value;
                NotifyPropertyChanged("DOC_REL_DT_ACTUAL");
            }
        }

        private Nullable<DateTime> _tools_ready_dt_plan;
        public Nullable<DateTime> TOOLS_READY_DT_PLAN
        {
            get { return _tools_ready_dt_plan; }
            set
            {
                _tools_ready_dt_plan = value;
                NotifyPropertyChanged("TOOLS_READY_DT_PLAN");
            }
        }

        private Nullable<DateTime> _tools_ready_actual_dt;
        public Nullable<DateTime> TOOLS_READY_ACTUAL_DT
        {
            get { return _tools_ready_actual_dt; }
            set
            {
                _tools_ready_actual_dt = value;
                NotifyPropertyChanged("TOOLS_READY_ACTUAL_DT");
            }
        }

        private Nullable<DateTime> _forging_plan_dt;
        public Nullable<DateTime> FORGING_PLAN_DT
        {
            get { return _forging_plan_dt; }
            set
            {
                _forging_plan_dt = value;
                NotifyPropertyChanged("FORGING_PLAN_DT");
            }
        }

        private Nullable<DateTime> _forging_actual_dt;
        public Nullable<DateTime> FORGING_ACTUAL_DT
        {
            get { return _forging_actual_dt; }
            set
            {
                _forging_actual_dt = value;
                NotifyPropertyChanged("FORGING_ACTUAL_DT");
            }
        }

        private Nullable<DateTime> _secondary_plan_dt;
        public Nullable<DateTime> SECONDARY_PLAN_DT
        {
            get { return _secondary_plan_dt; }
            set
            {
                _secondary_plan_dt = value;
                NotifyPropertyChanged("SECONDARY_PLAN_DT");
            }
        }

        private Nullable<DateTime> _secondary_actual_dt;
        public Nullable<DateTime> SECONDARY_ACTUAL_DT
        {
            get { return _secondary_actual_dt; }
            set
            {
                _secondary_actual_dt = value;
                NotifyPropertyChanged("SECONDARY_ACTUAL_DT");
            }
        }

        private Nullable<DateTime> _heat_treatment_plan_dt;
        public Nullable<DateTime> HEAT_TREATMENT_PLAN_DT
        {
            get { return _heat_treatment_plan_dt; }
            set
            {
                _heat_treatment_plan_dt = value;
                NotifyPropertyChanged("HEAT_TREATMENT_PLAN_DT");
            }
        }

        private Nullable<DateTime> _heat_treatment_actual;
        public Nullable<DateTime> HEAT_TREATMENT_ACTUAL
        {
            get { return _heat_treatment_actual; }
            set
            {
                _heat_treatment_actual = value;
                NotifyPropertyChanged("HEAT_TREATMENT_ACTUAL");
            }
        }

        private Nullable<DateTime> _issr_plan_dt;
        public Nullable<DateTime> ISSR_PLAN_DT
        {
            get { return _issr_plan_dt; }
            set
            {
                _issr_plan_dt = value;
                NotifyPropertyChanged("ISSR_PLAN_DT");
            }
        }

        private Nullable<DateTime> _issr_actual_dt;
        public Nullable<DateTime> ISSR_ACTUAL_DT
        {
            get { return _issr_actual_dt; }
            set
            {
                _issr_actual_dt = value;
                NotifyPropertyChanged("ISSR_ACTUAL_DT");
            }
        }

        private Nullable<DateTime> _ppap_plan;
        public Nullable<DateTime> PPAP_PLAN
        {
            get { return _ppap_plan; }
            set
            {
                _ppap_plan = value;
                NotifyPropertyChanged("PPAP_PLAN");
            }
        }

        private Nullable<DateTime> _ppap_actual_dt;
        public Nullable<DateTime> PPAP_ACTUAL_DT
        {
            get { return _ppap_actual_dt; }
            set
            {
                _ppap_actual_dt = value;
                NotifyPropertyChanged("PPAP_ACTUAL_DT");
            }
        }

        private Nullable<DateTime> _psw_date;
        public Nullable<DateTime> PSW_DATE
        {
            get { return _psw_date; }
            set
            {
                _psw_date = value;
                NotifyPropertyChanged("PSW_DATE");
            }
        }

        private Nullable<DateTime> _time_bogauge_plan;
        public Nullable<DateTime> TIME_BOGAUGE_PLAN
        {
            get { return _time_bogauge_plan; }
            set
            {
                _time_bogauge_plan = value;
                NotifyPropertyChanged("TIME_BOGAUGE_PLAN");
            }
        }

        private Nullable<DateTime> _time_bogauge_actual;
        public Nullable<DateTime> TIME_BOGAUGE_ACTUAL
        {
            get { return _time_bogauge_actual; }
            set
            {
                _time_bogauge_actual = value;
                NotifyPropertyChanged("TIME_BOGAUGE_ACTUAL");
            }
        }

        private string _sample_qty = "";
        public string SAMPLE_QTY
        {
            get { return _sample_qty; }
            set
            {               
                _sample_qty = value;
                NotifyPropertyChanged("SAMPLE_QTY");
            }
        }

        private string _remarks;
        public string REMARKS
        {
            get { return _remarks; }
            set
            {
                _remarks = value;
                NotifyPropertyChanged("REMARKS");
            }
        }

        private string _resp;
        public string RESP
        {
            get { return _resp; }
            set
            {
                _resp = value;
                NotifyPropertyChanged("RESP");
            }
        }

        private string _user_full_name;
        public string USER_FULL_NAME
        {
            get { return _user_full_name; }
            set
            {
                _user_full_name = value;
                NotifyPropertyChanged("USER_FULL_NAME");
            }
        }

        private MFM_MAST _mfm_master;
        public MFM_MAST MFM_MASTER
        {
            get { return _mfm_master; }
            set
            {
                _mfm_master = value;
                NotifyPropertyChanged("MFM_MASTER");
            }
        }

    }
}
