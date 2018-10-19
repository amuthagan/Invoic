using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;

namespace ProcessDesigner.Model
{
    public class ProductInformationModel : ViewModelBase
    {
        public ProductInformationModel()
            : base()
        {
            InitializeMandatoryFields(this);
        }

        private string _part_config_id;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Category is Required")]
        public string PART_CONFIG_ID
        {
            get { return _part_config_id; }
            set
            {
                _part_config_id = value;
                NotifyPropertyChanged("PART_CONFIG_ID");
            }
        }

        private string _part_number;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Part No should not be empty")]
        public string PART_NO
        {
            get { return _part_number; }
            set
            {
                _part_number = value;
                NotifyPropertyChanged("PART_NO");
            }
        }

        private string _part_desc;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Part Description should not be empty")]
        public string PART_DESC
        {
            get { return _part_desc; }
            set
            {
                _part_desc = value;
                NotifyPropertyChanged("PART_DESC");
            }
        }

        private string _sim_to_std_cd;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Similar to Std should not be empty")]
        public string SIM_TO_STD_CD
        {
            get { return _sim_to_std_cd; }
            set
            {
                _sim_to_std_cd = value;
                NotifyPropertyChanged("SIM_TO_STD_CD");
            }
        }

        private string _family;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product family should not be empty")]
        public string FAMILY
        {
            get { return _family; }
            set
            {
                _family = value;
                NotifyPropertyChanged("FAMILY");
            }
        }

        private string _mfg_std;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Manufacturing Standard should not be empty")]
        public string MFG_STD
        {
            get { return _mfg_std; }
            set
            {
                _mfg_std = value;
                NotifyPropertyChanged("MFG_STD");
            }
        }

        private string _thread_cd;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Thread Code should not be empty")]
        public string THREAD_CD
        {
            get { return _thread_cd; }
            set
            {
                _thread_cd = value;
                NotifyPropertyChanged("THREAD_CD");
            }
        }

        private string _dia_cd;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Thread Size should not be empty")]
        public string DIA_CD
        {
            get { return _dia_cd; }
            set
            {
                _dia_cd = value;
                NotifyPropertyChanged("DIA_CD");
            }
        }

        private string _bif_proj;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Bifurcation Project should not be empty")]
        public string BIF_PROJ
        {
            get { return _bif_proj; }
            set
            {
                _bif_proj = value;
                NotifyPropertyChanged("BIF_PROJ");
            }
        }

        private string _bif_forecast;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Forecast Location should not be empty")]
        public string BIF_FORECAST
        {
            get { return _bif_forecast; }
            set
            {
                _bif_forecast = value;
                NotifyPropertyChanged("BIF_FORECAST");
            }
        }

        private string _location;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Current Location2 should not be empty")]
        public string LOCATION
        {
            get { return _location; }
            set
            {
                _location = value;
                NotifyPropertyChanged("LOCATION");
            }
        }

        //new by me
        private string _location_code;
        public string LOCATION_CODE
        {
            get { return _location_code; }
            set
            {
                _location_code = value;
                NotifyPropertyChanged("LOC_CODE");
            }
        }
        //end new

        private string _currentLocation2Code;
        public string CurrentLocation2Code
        {
            get { return _currentLocation2Code; }
            set
            {
                _currentLocation2Code = value;
                NotifyPropertyChanged("CurrentLocation2Code");
            }
        }

        private string _quality;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Quality should not be empty")]
        public string QUALITY
        {
            get { return _quality; }
            set
            {
                _quality = value;
                NotifyPropertyChanged("QUALITY");
            }
        }

        private Nullable<DateTime> _doc_rel_date;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Planned Document Release Date")]
        public Nullable<DateTime> DOC_REL_DATE
        {
            get { return _doc_rel_date; }
            set
            {
                _doc_rel_date = value;
                NotifyPropertyChanged("DOC_REL_DATE");
            }
        }

        private string _heat_treatment_cd;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Heat Treatment should not be empty")]
        public string HEAT_TREATMENT_CD
        {
            get { return _heat_treatment_cd; }
            set
            {
                _heat_treatment_cd = value;
                NotifyPropertyChanged("HEAT_TREATMENT_CD");
            }
        }

        private string _pg_category;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "PG Category should not be empty")]
        public string PG_CATEGORY
        {
            get { return _pg_category; }
            set
            {
                _pg_category = value;
                NotifyPropertyChanged("PG_CATEGORY");
            }
        }

        private string _keywords;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Keywords should not be empty")]
        public string KEYWORDS
        {
            get { return _keywords; }
            set
            {
                _keywords = value;
                NotifyPropertyChanged("KEYWORDS");
            }
        }

        private string _type;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Type should not be empty")]
        public string TYPE
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyPropertyChanged("TYPE");
            }
        }

        private string _head_style;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Head Form should not be empty")]
        public string HEAD_STYLE
        {
            get { return _head_style; }
            set
            {
                _head_style = value;
                NotifyPropertyChanged("HEAD_STYLE");
            }
        }

        private string _application;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Shank Form should not be empty")]
        public string APPLICATION
        {
            get { return _application; }
            set
            {
                _application = value;
                NotifyPropertyChanged("APPLICATION");
            }
        }

        private string _prd_class_cd;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "End Form should not be empty")]
        public string PRD_CLASS_CD
        {
            get { return _prd_class_cd; }
            set
            {
                _prd_class_cd = value;
                NotifyPropertyChanged("PRD_CLASS_CD");
            }
        }

        private string _prd_grp_cd;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Driving Feature should not be empty")]
        public string PRD_GRP_CD
        {
            get { return _prd_grp_cd; }
            set
            {
                _prd_grp_cd = value;
                NotifyPropertyChanged("PRD_GRP_CD");
            }
        }

        private string _addl_feature;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Additional Feature should not be empty")]
        public string ADDL_FEATURE
        {
            get { return _addl_feature; }
            set
            {
                _addl_feature = value;
                NotifyPropertyChanged("ADDL_FEATURE");
            }
        }

        private string _product_drawing_issue_no;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Product Drawing Issue No should not be empty")]
        public string PRODUCT_DRAWING_ISSUE_NO
        {
            get { return _product_drawing_issue_no; }
            set
            {
                _product_drawing_issue_no = value;
                NotifyPropertyChanged("PRODUCT_DRAWING_ISSUE_NO");
            }
        }
        private string _sequence_drawing_Loc_Code;
        public string SEQUENCE_DRAWING_LOC_CODE
        {
            get { return _sequence_drawing_Loc_Code; }
            set
            {
                _sequence_drawing_Loc_Code = value;
                NotifyPropertyChanged("SEQUENCE_DRAWING_LOC_CODE");
            }
        }
        private Nullable<DateTime> _product_drawing_issue_date;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Product Drawing Issue Date should not be empty")]
        public Nullable<DateTime> PRODUCT_DRAWING_ISSUE_DATE
        {
            get { return _product_drawing_issue_date; }
            set
            {
                _product_drawing_issue_date = value;
                NotifyPropertyChanged("PRODUCT_DRAWING_ISSUE_DATE");
            }
        }

        private string _sequence_drawing_issue_no;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Sequence Drawing Issue No should not be empty")]
        public string SEQUENCE_DRAWING_ISSUE_NO
        {
            get { return _sequence_drawing_issue_no; }
            set
            {
                _sequence_drawing_issue_no = value;
                NotifyPropertyChanged("SEQUENCE_DRAWING_ISSUE_NO");
            }
        }

        private Nullable<DateTime> _sequence_drawing_issue_date;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Sequence Drawing Issue Date should not be empty")]
        public Nullable<DateTime> SEQUENCE_DRAWING_ISSUE_DATE
        {
            get { return _sequence_drawing_issue_date; }
            set
            {
                _sequence_drawing_issue_date = value;
                NotifyPropertyChanged("SEQUENCE_DRAWING_ISSUE_DATE");
            }
        }

        private string _cust_code;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer should not be empty")]
        public string CUST_CODE
        {
            get { return _cust_code; }
            set
            {
                _cust_code = value;
                NotifyPropertyChanged("CUST_CODE");
            }
        }

        private string _finish_desc;
        public string FINISH_DESC
        {
            get { return _finish_desc; }
            set
            {
                _finish_desc = value;
                NotifyPropertyChanged("FINISH_DESC");
            }
        }

        private string _headForm = "Head Form :";
        public string HeadForm
        {
            get { return _headForm; }
            set
            {
                _headForm = value;
                NotifyPropertyChanged("HeadForm");
            }
        }

        private string _shankForm = "Shank Form :";
        public string ShankForm
        {
            get { return _shankForm; }
            set
            {
                _shankForm = value;
                NotifyPropertyChanged("ShankForm");
            }
        }

        private string _endForm = "End Form :";
        public string EndForm
        {
            get { return _endForm; }
            set
            {
                _endForm = value;
                NotifyPropertyChanged("EndForm");
            }
        }

        private int _rowProductGroupCategory = 31;
        public int RowProductGroupCategory
        {
            get { return _rowProductGroupCategory; }
            set
            {
                _rowProductGroupCategory = value;
                NotifyPropertyChanged("RowProductGroupCategory");
            }
        }

        private int _rowSimilarity = 33;
        public int RowSimilarity
        {
            get { return _rowSimilarity; }
            set
            {
                _rowSimilarity = value;
                NotifyPropertyChanged("RowSimilarity");
            }
        }

        //private string _ci_reference;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Part No should not be empty")]
        //public string CI_REFERENCE
        //{
        //    get { return _ci_reference; }
        //    set
        //    {
        //        _ci_reference = value;
        //        NotifyPropertyChanged("CI_REFERENCE");
        //    }
        //}
    }
}
