using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



namespace ProcessDesigner.Model
{
    public class TapmacModel : ValidatableBindableBase
    {
        private string _cost_cent_code;
        private System.Nullable<decimal> _min_tap_size;
        private System.Nullable<decimal> _max_tap_size;
        private System.Nullable<decimal> _no_of_spindles;
        private System.Nullable<decimal> _push_storke_shaft_speed;
        private System.Nullable<decimal> _motor_power;
        //private System.Guid _ROWID;

        public string COST_CENT_CODE
        {
            get
            {
                return _cost_cent_code;
            }
            set
            {
                _cost_cent_code = value;

            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Min TAP Size is required.")]
        [MinLength(2)]
        [Range(0, Double.MaxValue, ErrorMessage = "Min TAP Size can't be smaller than zero!")]
        public System.Nullable<decimal> MIN_TAP_SIZE
        {
            get
            {
                return _min_tap_size;
            }
            set
            {
                _min_tap_size = value;
                OnPropertyChanged(() => MIN_TAP_SIZE);
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Max TAP Size is required.")]
        [MinLength(2)]
        public System.Nullable<decimal> MAX_TAP_SIZE
        {
            get
            {
                return _max_tap_size;
            }
            set
            {
                _max_tap_size = value;
                OnPropertyChanged(() => MAX_TAP_SIZE);
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Motor Power is required.")]
        public System.Nullable<decimal> MOTOR_POWER
        {
            get
            {
                return _motor_power;
            }
            set
            {
                _motor_power = value;
                OnPropertyChanged(() => MOTOR_POWER);
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "No Of Spindles is required.")]
        public System.Nullable<decimal> NO_OF_SPINDLES
        {
            get
            {
                return _no_of_spindles;
            }
            set
            {
                _no_of_spindles = value;
                OnPropertyChanged(() => NO_OF_SPINDLES);
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "PUSH STORKE SHAFT SPEED is required.")]
        public System.Nullable<decimal> PUSH_STORKE_SHAFT_SPEED
        {
            get
            {
                return _push_storke_shaft_speed;
            }
            set
            {
                _push_storke_shaft_speed = value;
                OnPropertyChanged(() => PUSH_STORKE_SHAFT_SPEED);
            }
        }

    }

    [MetadataTypeAttribute(typeof(DDTAPPING_MACMetaData))]
    public partial class DDTAPPING_MAC : IDataErrorInfo
    {
        #region IDataErrorInfo Members
        private string error = string.Empty;
        private Dictionary<string, string> errors = new Dictionary<string, string>();
        public string Error
        {
            get { return string.Empty; }
        }

        public string this[string columnName]
        {
            get
            {
                string result = null;
                if (columnName == "MIN_TAP_SIZE")
                {
                    if (MIN_TAP_SIZE == null || MIN_TAP_SIZE < 0)
                        result = "Min TAP Size is required.";
                }
                else if (columnName == "MAX_TAP_SIZE")
                {
                    if (MAX_TAP_SIZE == null || MAX_TAP_SIZE < 0)
                        result = "Max TAP Size is required.";
                }
                else if (columnName == "MOTOR_POWER")
                {
                    if (MOTOR_POWER == null || MOTOR_POWER < 0)
                        result = "Moter Power is required.";
                }
                else if (columnName == "NO_OF_SPINDLES")
                {
                    if (NO_OF_SPINDLES == null || NO_OF_SPINDLES < 0)
                        result = "No Of Spindles is required.";
                }
                else if (columnName == "PUSH_STORKE_SHAFT_SPEED")
                {
                    if (PUSH_STORKE_SHAFT_SPEED == null || PUSH_STORKE_SHAFT_SPEED < 0)
                        result = "Push Storke Shaft Speed is required.";
                }
                if (string.IsNullOrEmpty(result))
                {
                    errors.Remove(columnName);
                }
                else
                {
                    errors.Add(columnName, result);
                }

                return result;
            }
        }
        public bool IsValid
        {
            get
            {
                return errors.Count == 0;
            }
        }
        #endregion

    }
    public class DDTAPPING_MACMetaData
    {
        private DDTAPPING_MACMetaData()
        {
        }
        // Apply RequiredAttribute
        [Required(AllowEmptyStrings = false, ErrorMessage = "Min TAP Size is required.")]
        [MinLength(2)]
        [Range(0, Double.MaxValue, ErrorMessage = "Min TAP Size can't be smaller than zero!")]
        public System.Nullable<decimal> MIN_TAP_SIZE { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Max TAP Size is required.")]
        [MinLength(2)]
        public System.Nullable<decimal> MAX_TAP_SIZE { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Motor Power is required.")]
        public System.Nullable<decimal> MOTOR_POWER { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "No Of Spindles is required.")]
        public System.Nullable<decimal> NO_OF_SPINDLES { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "PUSH_STORKE_SHAFT_SPEED is required.")]
        public System.Nullable<decimal> PUSH_STORKE_SHAFT_SPEED { get; set; }
        public string COST_CENT_CODE { get; set; }
        public System.Guid ROWID { get; set; }

    }


}
