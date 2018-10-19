using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProcessDesigner.Model
{
    public class ManufacReportModel : ViewModelBase
    {
        private string _partNo = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Part Number is Required")]
        public string PartNo
        {
            get { return _partNo; }
            set
            {
                _partNo = value;
                NotifyPropertyChanged("PartNo");
            }
        }

        private string _partNoDesc = "";
        public string PartNoDesc
        {
            get { return _partNoDesc; }
            set
            {
                _partNoDesc = value;
                NotifyPropertyChanged("PartNoDesc");
            }
        }

        private OperationMode _actionMode = OperationMode.None;
        public OperationMode ActionMode
        {
            get { return _actionMode; }
            set
            {
                _actionMode = value;
                NotifyPropertyChanged("ActionMode");
            }
        }       

        private string _machine = "";
        public string MACHINE
        {
            get { return _machine; }
            set
            {
                _machine = value;
                NotifyPropertyChanged("MACHINE");
            }
        }

        private string _cust_name = "";
        public string CUST_NAME
        {
            get { return _cust_name; }
            set
            {
                _cust_name = value;
                NotifyPropertyChanged("CUST_NAME");
            }
        }

        private string _material = "";
        public string MATERIAL
        {
            get { return _material; }
            set
            {
                _material = value;
                NotifyPropertyChanged("MATERIAL");
            }
        }

        private string _rm_cd = "";
        public string RM_CD
        {
            get { return _rm_cd; }
            set
            {
                _rm_cd = value;
                NotifyPropertyChanged("RM_CD");
            }
        }

        private string _wire_size = null;
        public string WIRE_SIZE
        {
            get { return _wire_size; }
            set
            {
                _wire_size = value;
                NotifyPropertyChanged("WIRE_SIZE");
            }
        }

        private string _rod_dia = "";
        public string ROD_DIA
        {
            get { return _rod_dia; }
            set
            {
                _rod_dia = value;
                NotifyPropertyChanged("ROD_DIA");
            }
        }

        private string _uts_yp = "";
        public string UTS_YP
        {
            get { return _uts_yp; }
            set
            {
                _uts_yp = value;
                NotifyPropertyChanged("UTS_YP");
            }
        }

        private string _heat_no = null;
        public string HEAT_NO
        {
            get { return _heat_no; }
            set
            {
                _heat_no = value;
                NotifyPropertyChanged("HEAT_NO");
            }
        }

        private string _coating = "";
        public string COATING
        {
            get { return _coating; }
            set
            {
                _coating = value;
                NotifyPropertyChanged("COATING");
            }
        }

        private string _qty_planned = "";
        public string QTY_PLANNED
        {
            get { return _qty_planned; }
            set
            {
                _qty_planned = value;
                NotifyPropertyChanged("QTY_PLANNED");
            }
        }

        private string _qty_forged = "";
        public string QTY_FORGED
        {
            get { return _qty_forged; }
            set
            {
                _qty_forged = value;
                NotifyPropertyChanged("QTY_FORGED");
            }
        }

        private string _setting_scrap = "";
        public string SETTING_SCRAP
        {
            get { return _setting_scrap; }
            set
            {
                _setting_scrap = value;
                NotifyPropertyChanged("SETTING_SCRAP");
            }
        }

        private string _duration = "";
        public string DURATION
        {
            get { return _duration; }
            set
            {
                _duration = value;
                NotifyPropertyChanged("DURATION");
            }
        }

        private string _post_approval = "";
        public string POST_APPROVAL
        {
            get { return _post_approval; }
            set
            {
                _post_approval = value;
                NotifyPropertyChanged("POST_APPROVAL");
            }
        }

        private Boolean _post_appr_yes = false;
        public Boolean POST_APPR_YES
        {
            get { return _post_appr_yes; }
            set
            {
                _post_appr_yes = value;
                NotifyPropertyChanged("POST_APPR_YES");
            }
        }

        private Boolean _post_appr_no = false;
        public Boolean POST_APPR_NO
        {
            get { return _post_appr_no; }
            set
            {
                _post_appr_no = value;
                NotifyPropertyChanged("POST_APPR_NO");
            }
        }

        private Boolean _post_appr_na = false;
        public Boolean POST_APPR_NA
        {
            get { return _post_appr_na; }
            set
            {
                _post_appr_na = value;
                NotifyPropertyChanged("POST_APPR_NA");
            }
        }

        private string _bulk_production = "";
        public string BULK_PRODUCTION
        {
            get { return _bulk_production; }
            set
            {
                _bulk_production = value;
                NotifyPropertyChanged("BULK_PRODUCTION");
            }
        }

        private Boolean _bulk_prod_yes = false;
        public Boolean BULK_PROD_YES
        {
            get { return _bulk_prod_yes; }
            set
            {
                _bulk_prod_yes = value;
                NotifyPropertyChanged("BULK_PROD_YES");
            }
        }

        private Boolean _bulk_prod_no = false;
        public Boolean BULK_PROD_NO
        {
            get { return _bulk_prod_no; }
            set
            {
                _bulk_prod_no = value;
                NotifyPropertyChanged("BULK_PROD_NO");
            }
        }

        private Boolean _bulk_prod_na = false;
        public Boolean BULK_PROD_NA
        {
            get { return _bulk_prod_na; }
            set
            {
                _bulk_prod_na = value;
                NotifyPropertyChanged("BULK_PROD_NA");
            }
        }

        private string _prepared_dd = "";
        public string PREPARED_DD
        {
            get { return _prepared_dd; }
            set
            {
                _prepared_dd = value;
                NotifyPropertyChanged("PREPARED_DD");
            }
        }

        private string _forging = "";
        public string FORGING
        {
            get { return _forging; }
            set
            {
                _forging = value;
                NotifyPropertyChanged("FORGING");
            }
        }

        private string _tool_management = "";
        public string TOOL_MANAGEMENT
        {
            get { return _tool_management; }
            set
            {
                _tool_management = value;
                NotifyPropertyChanged("TOOL_MANAGEMENT");
            }
        }

        private string _quality_assurance = "";
        public string QUALITY_ASSURANCE
        {
            get { return _quality_assurance; }
            set
            {
                _quality_assurance = value;
                NotifyPropertyChanged("QUALITY_ASSURANCE");
            }
        }

        private string _others = "";
        public string OTHERS
        {
            get { return _others; }
            set
            {
                _others = value;
                NotifyPropertyChanged("OTHERS");
            }
        }

        private Nullable<DateTime> _start_date = null;
        public Nullable<DateTime> START_DATE
        {
            get { return _start_date; }
            set
            {
                _start_date = value;
                NotifyPropertyChanged("START_DATE");
            }
        }

        private Nullable<DateTime> _end_date = null;
        public Nullable<DateTime> END_DATE
        {
            get { return _end_date; }
            set
            {
                _end_date = value;
                NotifyPropertyChanged("END_DATE");
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

        private DataView _dvDesign;
        public DataView DVDesign
        {
            get { return _dvDesign; }
            set
            {
                _dvDesign = value;
                NotifyPropertyChanged("DVDesign");
            }
        }

        private DataView dvDifficulties;
        public DataView DVDifficulties
        {
            get { return dvDifficulties; }
            set
            {
                dvDifficulties = value;
                NotifyPropertyChanged("DVDifficulties");
            }
        }

        private DataView dvPrequal;
        public DataView DVPreQual
        {
            get { return dvPrequal; }
            set
            {
                dvPrequal = value;
                NotifyPropertyChanged("DVPreQual");
            }
        }

        private DataView dvProcess;
        public DataView DVProcess
        {
            get { return dvProcess; }
            set
            {
                dvProcess = value;
                NotifyPropertyChanged("DVProcess");
            }
        }

        private DataView dvOutput;
        public DataView DVOutput
        {
            get { return dvOutput; }
            set
            {
                dvOutput = value;
                NotifyPropertyChanged("DVOutput");
            }
        }
    }
}
