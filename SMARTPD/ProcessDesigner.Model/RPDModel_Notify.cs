using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
namespace ProcessDesigner.Model
{
    public class RPDModel_Notify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private System.Nullable<System.DateTime> _enqu_recd_on;

        private System.Nullable<System.DateTime> _fr_cs_date;

        private string _prod_desc = "";

        private System.Nullable<decimal> _cust_code = null;



        private string _cust_dwg_no_issue = "";

        private string _export = "";

        private System.Nullable<decimal> _number_off;

        private System.Nullable<decimal> _potential = null;

        private System.Nullable<decimal> _sfl_share = null;

        private string _remarks = "";

        private string _responsibility = "";

        private string _pending = "";

        private string _feasibility = "";

        private string _reject_reason = "";

        private string _loc_code = "";

        private System.Nullable<decimal> _cheese_wt = null;

        private System.Nullable<decimal> _finish_wt = null;

        private System.Nullable<decimal> _finish_code = null;

        private string _suggested_rm;

        private System.Nullable<decimal> _rm_cost = null;

        private System.Nullable<decimal> _final_cost;

        private string _cost_notes;

        private string _processed_by;

        private System.Nullable<System.DateTime> _order_dt;

        private string _print;

        private System.Nullable<decimal> _allot_part_no = null;

        private System.Nullable<System.DateTime> _part_no_req_date = null;

        private string _cust_std_no = "";

        private System.Nullable<System.DateTime> _cust_std_date;

        private string _autopart = "";

        private string _saftypart = "";

        private string _application = "";

        private System.Nullable<decimal> _status = null;

        private System.Nullable<System.DateTime> _customer_need_dt = null;

        private System.Nullable<System.DateTime> _mktg_commited_dt = null;

        private string _ppap_level = "";

        private System.Nullable<decimal> _devl_method = null;

        private System.Nullable<decimal> _ppap_forging = null;

        private System.Nullable<decimal> _ppap_sample = null;

        private System.Nullable<decimal> _packing = null;

        private string _nature_packing;

        private System.Nullable<decimal> _spl_char;

        private string _other_cust_req;

        private System.Nullable<System.DateTime> _atp_date;

        private string _similar_part_no;

        private string _general_remarks;

        private System.Nullable<decimal> _monthly;

        private System.Nullable<System.DateTime> _mktg_commited_date;

        private System.Guid _rowid;

        private System.Nullable<bool> _delete_flag;

        private System.Nullable<System.DateTime> _entered_date;

        private string _entered_by;

        private System.Nullable<System.DateTime> _updated_date;

        private string _updated_by;

        private int _idpk;

        private string _coating_code;

        private System.Nullable<decimal> _realisation;

        private System.Nullable<decimal> _no_of_pcs;



        private string _sFLPART_NO;

        public string SFLPART_NO
        {
            get
            {
                return _sFLPART_NO;
            }
            set
            {
                _sFLPART_NO = value;
                NotifyPropertyChanged("SFLPART_NO");
            }
        }

        private string _pART_NO;
        public string PART_NO
        {
            get
            {
                return _pART_NO;
            }
            set
            {
                _pART_NO = value;
                NotifyPropertyChanged("PART_NO");
            }
        }


        private string _errormessage;
        public string ErrorMessage
        {
            get
            {
                return _errormessage;
            }
            set
            {
                _errormessage = value;
                NotifyPropertyChanged("ErrorMessage");
            }
        }

        public DataRowView SelectedItem { get; set; }
        private DataView _griddata;
        public DataView GridData
        {
            get
            {
                return this._griddata;
            }
            set
            {
                this._griddata = value;
                NotifyPropertyChanged("GridData");
            }
        }

        private DataView dvType = null;
        public DataView DVType
        {
            get { return dvType; }
            set
            {
                dvType = value;
                NotifyPropertyChanged("DVType");
            }
        }


        private int _griddupcount = 0;
        public int GridDupCount
        {
            get
            {
                return _griddupcount;
            }
            set
            {
                _griddupcount = value;
                NotifyPropertyChanged("GridDupCount");
            }
        }

        private string _customer_exp;
        public string CUSTOMER_EXP
        {
            get
            {
                return _customer_exp;
            }
            set
            {
                _customer_exp = value;
                NotifyPropertyChanged("CUSTOMER_EXP");
            }

        }

        private string _characteristic;
        public string CHARACTERISTIC
        {
            get
            {
                return _characteristic;
            }
            set
            {
                _characteristic = value;
                NotifyPropertyChanged("CHARACTERISTIC");
            }

        }

        private System.Nullable<decimal> _slno;
        public System.Nullable<decimal> SLNO
        {
            get
            {
                return this._slno;
            }
            set
            {
                _slno = value;
                NotifyPropertyChanged("SLNO");
            }
        }

        private System.Nullable<decimal> _severity;
        public System.Nullable<decimal> SEVERITY
        {
            get
            {
                return this._severity;
            }
            set
            {
                _severity = value;
                NotifyPropertyChanged("SEVERITY");
            }
        }

        private bool _opt_special_no = false;
        public bool Opt_Special_No
        {
            get
            {
                return _opt_special_no;

            }
            set
            {
                _opt_special_no = value;
                NotifyPropertyChanged("Opt_Special_No");
            }
        }


        private bool _opt_special_yes = false;
        public bool Opt_Special_Yes
        {
            get
            {
                return _opt_special_yes;

            }
            set
            {
                _opt_special_yes = value;
                NotifyPropertyChanged("Opt_Special_Yes");
            }
        }

        private bool _opt_stand = false;
        public bool Opt_Stand
        {
            get
            {
                return _opt_stand;
            }
            set
            {
                _opt_stand = value;
                NotifyPropertyChanged("Opt_Stand");
            }
        }


        private bool _opt_special = false;
        public bool Opt_Special
        {
            get
            {
                return _opt_special;

            }
            set
            {
                _opt_special = value;
                NotifyPropertyChanged("Opt_Special");
            }
        }

        private bool _opt_devlp_prelaunch = false;
        public bool Opt_Devlp_Prelaunch
        {
            get
            {
                return _opt_devlp_prelaunch;
            }
            set
            {
                _opt_devlp_prelaunch = value;
                NotifyPropertyChanged("Opt_Devlp_Prelaunch");
            }
        }

        private bool _opt_devlp_proto = false;
        public bool Opt_Devlp_Proto
        {
            get
            {
                return _opt_devlp_proto;
            }
            set
            {
                _opt_devlp_proto = value;
                NotifyPropertyChanged("Opt_Devlp_Proto");
            }
        }

        private bool _opt_production = false;
        public bool Opt_Production
        {
            get
            {
                return _opt_production;
            }
            set
            {
                _opt_production = value;
                NotifyPropertyChanged("Opt_Production");
            }
        }
        private bool _opt_prelaunch = false;
        public bool Opt_PreLaunch
        {
            get
            {
                return _opt_prelaunch;
            }
            set
            {
                _opt_prelaunch = value;
                NotifyPropertyChanged("Opt_PreLaunch");

            }

        }

        private bool _opt_prototype = false;
        public bool Opt_Prototype
        {
            get
            {
                return _opt_prototype;
            }
            set
            {
                _opt_prototype = value;
                NotifyPropertyChanged("Opt_Prototype");
            }
        }

        private bool _safety_yes = false;
        public bool Safety_Yes
        {
            get
            {
                return _safety_yes;
            }
            set
            {
                _safety_yes = value;
                NotifyPropertyChanged("Safety_Yes");
            }
        }

        private bool _safety_no = false;
        public bool Safety_No
        {
            get
            {
                return _safety_no;
            }
            set
            {
                _safety_no = value;
                NotifyPropertyChanged("Safety_No");
            }
        }

        private bool _autopart_yes = false;
        public bool AutoPart_Yes
        {
            get
            {
                return _autopart_yes;
            }
            set
            {
                _autopart_yes = value;
                NotifyPropertyChanged("AutoPart_Yes");
            }
        }

        private bool _autopart_no = false;
        public bool AutoPart_No
        {
            get
            {
                return _autopart_no;
            }
            set
            {
                _autopart_no = value;
                NotifyPropertyChanged("AutoPart_No");
            }
        }

        private string _cust_name = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Customer Name")]
        public string CUST_NAME
        {
            get
            {
                return _cust_name;
            }
            set
            {
                _cust_name = value;
                NotifyPropertyChanged("CUST_NAME");
            }
        }

        private string _ci_reference = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the CI Reference No")]
        public string CI_REFERENCE
        {
            get
            {
                return this._ci_reference;
            }
            set
            {
                if ((this._ci_reference != value))
                {

                    this._ci_reference = value;
                    this.NotifyPropertyChanged("CI_REFERENCE");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ENQU_RECD_ON", DbType = "DateTime2(0)")]
        public System.Nullable<System.DateTime> ENQU_RECD_ON
        {
            get
            {
                return this._enqu_recd_on;
            }
            set
            {
                if ((this._enqu_recd_on != value))
                {

                    this._enqu_recd_on = value;
                    this.NotifyPropertyChanged("ENQU_RECD_ON");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FR_CS_DATE", DbType = "DateTime2(0)")]
        public System.Nullable<System.DateTime> FR_CS_DATE
        {
            get
            {
                return this._fr_cs_date;
            }
            set
            {
                if ((this._fr_cs_date != value))
                {

                    this._fr_cs_date = value;
                    this.NotifyPropertyChanged("FR_CS_DATE");

                }
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Part Description")]
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PROD_DESC", DbType = "VarChar(50)")]
        public string PROD_DESC
        {
            get
            {
                return this._prod_desc;
            }
            set
            {
                if ((this._prod_desc != value))
                {

                    this._prod_desc = value;
                    this.NotifyPropertyChanged("PROD_DESC");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CUST_CODE", DbType = "Decimal(10,0)")]
        public System.Nullable<decimal> CUST_CODE
        {
            get
            {
                return this._cust_code;
            }
            set
            {
                if ((this._cust_code != value))
                {

                    this._cust_code = value;
                    this.NotifyPropertyChanged("CUST_CODE");

                }
            }
        }

        private string _cust_dwg_no = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Customer Part No")]
        public string CUST_DWG_NO
        {
            get
            {
                return _cust_dwg_no;
            }
            set
            {

                _cust_dwg_no = value;
                NotifyPropertyChanged("CUST_DWG_NO");
            }
        }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Issue No")]
        public string CUST_DWG_NO_ISSUE
        {
            get
            {
                return this._cust_dwg_no_issue;
            }
            set
            {
                if ((this._cust_dwg_no_issue != value))
                {

                    this._cust_dwg_no_issue = value;
                    this.NotifyPropertyChanged("CUST_DWG_NO_ISSUE");
                }
            }
        }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Customer Part No")] 
        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_EXPORT", DbType = "VarChar(1)")]
        public string EXPORT
        {
            get
            {
                return this._export;
            }
            set
            {
                if ((this._export != value))
                {

                    this._export = value;
                    this.NotifyPropertyChanged("EXPORT");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_NUMBER_OFF", DbType = "Decimal(10,0)")]
        public System.Nullable<decimal> NUMBER_OFF
        {
            get
            {
                return this._number_off;
            }
            set
            {
                if ((this._number_off != value))
                {

                    this._number_off = value;
                    this.NotifyPropertyChanged("NUMBER_OFF");

                }
            }
        }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Annual Req.")]
        public System.Nullable<decimal> POTENTIAL
        {
            get
            {
                return this._potential;
            }
            set
            {
                if ((this._potential != value))
                {

                    this._potential = value;
                    this.NotifyPropertyChanged("POTENTIAL");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SFL_SHARE", DbType = "Decimal(3,0)")]
        public System.Nullable<decimal> SFL_SHARE
        {
            get
            {
                return this._sfl_share;
            }
            set
            {
                if ((this._sfl_share != value))
                {

                    this._sfl_share = value;
                    this.NotifyPropertyChanged("SFL_SHARE");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_REMARKS", DbType = "VarChar(60)")]
        public string REMARKS
        {
            get
            {
                return this._remarks;
            }
            set
            {
                if ((this._remarks != value))
                {

                    this._remarks = value;
                    this.NotifyPropertyChanged("REMARKS");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RESPONSIBILITY", DbType = "VarChar(30)")]
        public string RESPONSIBILITY
        {
            get
            {
                return this._responsibility;
            }
            set
            {
                if ((this._responsibility != value))
                {

                    this._responsibility = value;
                    this.NotifyPropertyChanged("RESPONSIBILITY");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PENDING", DbType = "VarChar(1)")]
        public string PENDING
        {
            get
            {
                return this._pending;
            }
            set
            {
                if ((this._pending != value))
                {

                    this._pending = value;
                    this.NotifyPropertyChanged("PENDING");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FEASIBILITY", DbType = "VarChar(1)")]
        public string FEASIBILITY
        {
            get
            {
                return this._feasibility;
            }
            set
            {
                if ((this._feasibility != value))
                {

                    this._feasibility = value;
                    this.NotifyPropertyChanged("FEASIBILITY");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_REJECT_REASON", DbType = "VarChar(2000)")]
        public string REJECT_REASON
        {
            get
            {
                return this._reject_reason;
            }
            set
            {
                if ((this._reject_reason != value))
                {

                    this._reject_reason = value;
                    this.NotifyPropertyChanged("REJECT_REASON");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_LOC_CODE", DbType = "VarChar(2)")]
        public string LOC_CODE
        {
            get
            {
                return this._loc_code;
            }
            set
            {
                if ((this._loc_code != value))
                {

                    this._loc_code = value;
                    this.NotifyPropertyChanged("LOC_CODE");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_CHEESE_WT", DbType = "Decimal(12,2)")]
        public System.Nullable<decimal> CHEESE_WT
        {
            get
            {
                return this._cheese_wt;
            }
            set
            {
                if ((this._cheese_wt != value))
                {

                    this._cheese_wt = value;
                    this.NotifyPropertyChanged("CHEESE_WT");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FINISH_WT", DbType = "Decimal(12,2)")]
        public System.Nullable<decimal> FINISH_WT
        {
            get
            {
                return this._finish_wt;
            }
            set
            {
                if ((this._finish_wt != value))
                {

                    this._finish_wt = value;
                    this.NotifyPropertyChanged("FINISH_WT");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FINISH_CODE", DbType = "Decimal(4,0)")]
        public System.Nullable<decimal> FINISH_CODE
        {
            get
            {
                return this._finish_code;
            }
            set
            {
                if ((this._finish_code != value))
                {

                    this._finish_code = value;
                    this.NotifyPropertyChanged("FINISH_CODE");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SUGGESTED_RM", DbType = "VarChar(10)")]
        public string SUGGESTED_RM
        {
            get
            {
                return this._suggested_rm;
            }
            set
            {
                if ((this._suggested_rm != value))
                {

                    this._suggested_rm = value;
                    this.NotifyPropertyChanged("SUGGESTED_RM");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_RM_COST", DbType = "Decimal(12,2)")]
        public System.Nullable<decimal> RM_COST
        {
            get
            {
                return this._rm_cost;
            }
            set
            {
                if ((this._rm_cost != value))
                {

                    this._rm_cost = value;
                    this.NotifyPropertyChanged("RM_COST");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_FINAL_COST", DbType = "Decimal(12,2)")]
        public System.Nullable<decimal> FINAL_COST
        {
            get
            {
                return this._final_cost;
            }
            set
            {
                if ((this._final_cost != value))
                {

                    this._final_cost = value;
                    this.NotifyPropertyChanged("FINAL_COST");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_COST_NOTES", DbType = "VarChar(2000)")]
        public string COST_NOTES
        {
            get
            {
                return this._cost_notes;
            }
            set
            {
                if ((this._cost_notes != value))
                {

                    this._cost_notes = value;
                    this.NotifyPropertyChanged("COST_NOTES");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PROCESSED_BY", DbType = "VarChar(20)")]
        public string PROCESSED_BY
        {
            get
            {
                return this._processed_by;
            }
            set
            {
                if ((this._processed_by != value))
                {
                    this._processed_by = value;
                    this.NotifyPropertyChanged("PROCESSED_BY");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ORDER_DT", DbType = "DateTime2(0)")]
        public System.Nullable<System.DateTime> ORDER_DT
        {
            get
            {
                return this._order_dt;
            }
            set
            {
                if ((this._order_dt != value))
                {

                    this._order_dt = value;
                    this.NotifyPropertyChanged("ORDER_DT");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PRINT", DbType = "VarChar(1)")]
        public string PRINT
        {
            get
            {
                return this._print;
            }
            set
            {
                if ((this._print != value))
                {
                    this._print = value;
                    this.NotifyPropertyChanged("PRINT");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ALLOT_PART_NO", DbType = "Decimal(1,0)")]
        public System.Nullable<decimal> ALLOT_PART_NO
        {
            get
            {
                return this._allot_part_no;
            }
            set
            {
                if ((this._allot_part_no != value))
                {

                    this._allot_part_no = value;
                    this.NotifyPropertyChanged("ALLOT_PART_NO");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PART_NO_REQ_DATE", DbType = "DateTime2(0)")]
        public System.Nullable<System.DateTime> PART_NO_REQ_DATE
        {
            get
            {
                return this._part_no_req_date;
            }
            set
            {
                if ((this._part_no_req_date != value))
                {

                    this._part_no_req_date = value;
                    this.NotifyPropertyChanged("PART_NO_REQ_DATE");

                }
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Customer DWG No")]
        public string CUST_STD_NO
        {
            get
            {
                return this._cust_std_no;
            }
            set
            {
                this._cust_std_no = value;
                this.NotifyPropertyChanged("CUST_STD_NO");
            }
        }


        private string _cust_std_date_new = "";
        public string CUST_STD_DATE_NEW
        {
            get
            {
                return _cust_std_date_new;
            }
            set
            {
                _cust_std_date_new = value;
                NotifyPropertyChanged("CUST_STD_DATE_NEW");
            }
        }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Issue Date")]
        public System.Nullable<System.DateTime> CUST_STD_DATE
        {
            get
            {
                return this._cust_std_date;
            }
            set
            {
                if ((this._cust_std_date != value))
                {

                    this._cust_std_date = value;
                    this.NotifyPropertyChanged("CUST_STD_DATE");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_AUTOPART", DbType = "Char(1)")]
        public string AUTOPART
        {
            get
            {
                return this._autopart;
            }
            set
            {
                if ((this._autopart != value))
                {
                    this._autopart = value;
                    this.NotifyPropertyChanged("AUTOPART");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SAFTYPART", DbType = "Char(1)")]
        public string SAFTYPART
        {
            get
            {
                return this._saftypart;
            }
            set
            {
                if ((this._saftypart != value))
                {

                    this._saftypart = value;
                    this.NotifyPropertyChanged("SAFTYPART");

                }
            }
        }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Application of Product")]
        public string APPLICATION
        {
            get
            {
                return this._application;
            }
            set
            {
                if ((this._application != value))
                {

                    this._application = value;
                    this.NotifyPropertyChanged("APPLICATION");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_STATUS", DbType = "Decimal(1,0)")]
        public System.Nullable<decimal> STATUS
        {
            get
            {
                return this._status;
            }
            set
            {
                if ((this._status != value))
                {

                    this._status = value;
                    this.NotifyPropertyChanged("STATUS");

                }
            }
        }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Customer Need Date")]
        public System.Nullable<System.DateTime> CUSTOMER_NEED_DT
        {
            get
            {
                return this._customer_need_dt;
            }
            set
            {
                if ((this._customer_need_dt != value))
                {

                    this._customer_need_dt = value;
                    this.NotifyPropertyChanged("CUSTOMER_NEED_DT");

                }
            }
        }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Marketing Committed Date")]
        public System.Nullable<System.DateTime> MKTG_COMMITED_DT
        {
            get
            {
                return this._mktg_commited_dt;
            }
            set
            {
                if ((this._mktg_commited_dt != value))
                {

                    this._mktg_commited_dt = value;
                    this.NotifyPropertyChanged("MKTG_COMMITED_DT");

                }
            }
        }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the ppap level")]
        public string PPAP_LEVEL
        {
            get
            {
                return this._ppap_level;
            }
            set
            {
                if ((this._ppap_level != value))
                {

                    this._ppap_level = value;
                    this.NotifyPropertyChanged("PPAP_LEVEL");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DEVL_METHOD", DbType = "Decimal(1,0)")]
        public System.Nullable<decimal> DEVL_METHOD
        {
            get
            {
                return this._devl_method;
            }
            set
            {
                if ((this._devl_method != value))
                {

                    this._devl_method = value;
                    this.NotifyPropertyChanged("DEVL_METHOD");
                }
            }
        }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the PPAP Forging Quantity")]
        public System.Nullable<decimal> PPAP_FORGING
        {
            get
            {
                return this._ppap_forging;
            }
            set
            {
                if ((this._ppap_forging != value))
                {

                    this._ppap_forging = value;
                    this.NotifyPropertyChanged("PPAP_FORGING");

                }
            }
        }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the PPAP Sample Quantity")]
        public System.Nullable<decimal> PPAP_SAMPLE
        {
            get
            {
                return this._ppap_sample;
            }
            set
            {
                if ((this._ppap_sample != value))
                {

                    this._ppap_sample = value;
                    this.NotifyPropertyChanged("PPAP_SAMPLE");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_PACKING", DbType = "Decimal(1,0)")]
        public System.Nullable<decimal> PACKING
        {
            get
            {
                return this._packing;
            }
            set
            {
                if ((this._packing != value))
                {

                    this._packing = value;
                    this.NotifyPropertyChanged("PACKING");

                }
            }
        }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Nature of Packing")]
        public string NATURE_PACKING
        {
            get
            {
                return this._nature_packing;
            }
            set
            {
                if ((this._nature_packing != value))
                {

                    this._nature_packing = value;
                    this.NotifyPropertyChanged("NATURE_PACKING");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SPL_CHAR", DbType = "Decimal(1,0)")]
        public System.Nullable<decimal> SPL_CHAR
        {
            get
            {
                return this._spl_char;
            }
            set
            {
                if ((this._spl_char != value))
                {

                    this._spl_char = value;
                    this.NotifyPropertyChanged("SPL_CHAR");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_OTHER_CUST_REQ", DbType = "VarChar(200)")]
        public string OTHER_CUST_REQ
        {
            get
            {
                return this._other_cust_req;
            }
            set
            {
                if ((this._other_cust_req != value))
                {

                    this._other_cust_req = value;
                    this.NotifyPropertyChanged("OTHER_CUST_REQ");

                }
            }
        }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the ATP Date")]
        public System.Nullable<System.DateTime> ATP_DATE
        {
            get
            {
                return this._atp_date;
            }
            set
            {
                if ((this._atp_date != value))
                {

                    this._atp_date = value;
                    this.NotifyPropertyChanged("ATP_DATE");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_SIMILAR_PART_NO", DbType = "VarChar(9)")]
        public string SIMILAR_PART_NO
        {
            get
            {
                return this._similar_part_no;
            }
            set
            {
                if ((this._similar_part_no != value))
                {

                    this._similar_part_no = value;
                    this.NotifyPropertyChanged("SIMILAR_PART_NO");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_GENERAL_REMARKS", DbType = "VarChar(200)")]
        public string GENERAL_REMARKS
        {
            get
            {
                return this._general_remarks;
            }
            set
            {
                if ((this._general_remarks != value))
                {

                    this._general_remarks = value;
                    this.NotifyPropertyChanged("GENERAL_REMARKS");

                }
            }
        }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter the Monthly Req.")]
        public System.Nullable<decimal> MONTHLY
        {
            get
            {
                return this._monthly;
            }
            set
            {
                if ((this._monthly != value))
                {

                    this._monthly = value;
                    this.NotifyPropertyChanged("MONTHLY");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_MKTG_COMMITED_DATE", DbType = "DateTime2(0)")]
        public System.Nullable<System.DateTime> MKTG_COMMITED_DATE
        {
            get
            {
                return this._mktg_commited_date;
            }
            set
            {
                if ((this._mktg_commited_date != value))
                {

                    this._mktg_commited_date = value;
                    this.NotifyPropertyChanged("MKTG_COMMITED_DATE");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ROWID", DbType = "UniqueIdentifier NOT NULL")]
        public System.Guid ROWID
        {
            get
            {
                return this._rowid;
            }
            set
            {
                if ((this._rowid != value))
                {

                    this._rowid = value;
                    this.NotifyPropertyChanged("ROWID");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_DELETE_FLAG", DbType = "Bit")]
        public System.Nullable<bool> DELETE_FLAG
        {
            get
            {
                return this._delete_flag;
            }
            set
            {
                if ((this._delete_flag != value))
                {

                    this._delete_flag = value;
                    this.NotifyPropertyChanged("DELETE_FLAG");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ENTERED_DATE", DbType = "DateTime")]
        public System.Nullable<System.DateTime> ENTERED_DATE
        {
            get
            {
                return this._entered_date;
            }
            set
            {
                if ((this._entered_date != value))
                {

                    this._entered_date = value;
                    this.NotifyPropertyChanged("ENTERED_DATE");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_ENTERED_BY", DbType = "NChar(15)")]
        public string ENTERED_BY
        {
            get
            {
                return this._entered_by;
            }
            set
            {
                if ((this._entered_by != value))
                {

                    this._entered_by = value;
                    this.NotifyPropertyChanged("ENTERED_BY");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UPDATED_DATE", DbType = "DateTime")]
        public System.Nullable<System.DateTime> UPDATED_DATE
        {
            get
            {
                return this._updated_date;
            }
            set
            {
                if ((this._updated_date != value))
                {

                    this._updated_date = value;
                    this.NotifyPropertyChanged("UPDATED_DATE");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_UPDATED_BY", DbType = "NChar(15)")]
        public string UPDATED_BY
        {
            get
            {
                return this._updated_by;
            }
            set
            {
                if ((this._updated_by != value))
                {

                    this._updated_by = value;
                    this.NotifyPropertyChanged("UPDATED_BY");

                }
            }
        }

        public int IDPK
        {
            get
            {
                return this._idpk;
            }
            set
            {
                if ((this._idpk != value))
                {

                    this._idpk = value;
                    this.NotifyPropertyChanged("IDPK");
                }
            }
        }

        private System.Nullable<int> _ciref_no_fk;
        public System.Nullable<int> CIREF_NO_FK
        {
            get
            {
                return this._ciref_no_fk;
            }
            set
            {
                if ((this._ciref_no_fk != value))
                {
                    this._ciref_no_fk = value;
                    this.NotifyPropertyChanged("CIREF_NO_FK");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_COATING_CODE", DbType = "NChar(10)")]
        public string COATING_CODE
        {
            get
            {
                return this._coating_code;
            }
            set
            {
                if ((this._coating_code != value))
                {

                    this._coating_code = value;
                    this.NotifyPropertyChanged("COATING_CODE");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_REALISATION", DbType = "Decimal(10,2)")]
        public System.Nullable<decimal> REALISATION
        {
            get
            {
                return this._realisation;
            }
            set
            {
                if ((this._realisation != value))
                {

                    this._realisation = value;
                    this.NotifyPropertyChanged("REALISATION");

                }
            }
        }

        [global::System.Data.Linq.Mapping.ColumnAttribute(Storage = "_NO_OF_PCS", DbType = "Decimal(10,0)")]
        public System.Nullable<decimal> NO_OF_PCS
        {
            get
            {
                return this._no_of_pcs;
            }
            set
            {
                if ((this._no_of_pcs != value))
                {

                    this._no_of_pcs = value;
                    this.NotifyPropertyChanged("NO_OF_PCS");

                }
            }
        }


        private DataView _rPDModelSearchDetails;
        public DataView RPDModelSearchDetails
        {
            get { return _rPDModelSearchDetails; }
            set
            {
                _rPDModelSearchDetails = value;
                NotifyPropertyChanged("RPDModelSearchDetails");
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
