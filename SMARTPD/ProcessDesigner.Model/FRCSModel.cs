using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Common;

namespace ProcessDesigner.Model
{
    public class FRCSModel : ViewModelBase
    {
        private string _cust_name;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string CUST_NAME
        {
            get { return _cust_name; }
            set
            {
                _cust_name = value.IsNotNullOrEmpty() ? value.Trim() : value;
                NotifyPropertyChanged("CUST_NAME");
            }
        }

        private string _ci_reference;
        [Required(AllowEmptyStrings = false, ErrorMessage = "CI Reference is Required")]
        public string CI_REFERENCE
        {
            get { return _ci_reference; }
            set
            {
                _ci_reference = value.IsNotNullOrEmpty() ? value.Trim() : value;
                NotifyPropertyChanged("CI_REFERENCE");
            }
        }

        private Nullable<decimal> _cheese_wt;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Cheese Weight is Required")]
        public Nullable<decimal> CHEESE_WT
        {
            get { return _cheese_wt; }
            set
            {
                //if (value == 0) value = null;
                _cheese_wt = value;
                NotifyPropertyChanged("CHEESE_WT");
            }
        }

        private Nullable<decimal> _finish_wt;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Finish Weight is Required")]
        public Nullable<decimal> FINISH_WT
        {
            get { return _finish_wt; }
            set
            {
                //if (value == 0) value = null;
                _finish_wt = value;
                NotifyPropertyChanged("FINISH_WT");
            }
        }

        private Nullable<decimal> _rm_factor;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Multiplication Factor is Required")]
        public Nullable<decimal> RM_FACTOR
        {
            get { return _rm_factor; }
            set
            {
                _rm_factor = value;
                NotifyPropertyChanged("RM_FACTOR");
            }
        }

        private string _feasibility;
        public string FEASIBILITY
        {
            get { return _feasibility; }
            set
            {
                _feasibility = value.IsNotNullOrEmpty() ? value.Trim() : value;
                NotifyPropertyChanged("FEASIBILITY");
            }
        }

        public bool _Is_feasibility_can_change = true;
        public bool IS_FEASIBILITY_CAN_CHANGE
        {
            get { return _Is_feasibility_can_change; }
            set
            {
                _Is_feasibility_can_change = value;
                NotifyPropertyChanged("IS_FEASIBILITY_CAN_CHANGE");
            }
        }

        private string _loc_code;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Plant is Required")]
        public string LOC_CODE
        {
            get { return _loc_code; }
            set
            {
                _loc_code = value.IsNotNullOrEmpty() ? value.Trim() : value;
                NotifyPropertyChanged("LOC_CODE");
            }
        }

        private string _rawMaterialName;
        public string RM_DESC
        {
            get
            {
                return _rawMaterialName;
            }
            set
            {
                _rawMaterialName = value;
                NotifyPropertyChanged("RM_DESC");
            }
        }

        private bool _isReadOnlyCI_REFERENCE;
        public bool IsReadOnlyCI_REFERENCE
        {
            get
            {
                return _isReadOnlyCI_REFERENCE;
            }
            set
            {
                _isReadOnlyCI_REFERENCE = value;
                NotifyPropertyChanged("IsReadOnlyCI_REFERENCE");
            }
        }

        private string _number_off;
        public string NUMBER_OFF
        {
            get
            {
                return _number_off;
            }
            set
            {
                _number_off = value;
                NotifyPropertyChanged("NUMBER_OFF");
            }
        }

        private string _sfl_share;
        public string SFL_SHARE
        {
            get
            {
                return _sfl_share;
            }
            set
            {
                _sfl_share = value;
                NotifyPropertyChanged("SFL_SHARE");
            }
        }

        private string _potential;
        public string POTENTIAL
        {
            get
            {
                return _potential;
            }
            set
            {
                _potential = value;
                NotifyPropertyChanged("POTENTIAL");
            }
        }      

    }
}
