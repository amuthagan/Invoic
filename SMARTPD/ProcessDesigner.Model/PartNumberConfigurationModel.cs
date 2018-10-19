using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class PartNumberConfigurationModel : ViewModelBase
    {
        public PartNumberConfigurationModel()
            : base()
        {
            InitializeMandatoryFields(this);
        }

        private string _code;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Code should not be empty")]
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                NotifyPropertyChanged("Code");
            }
        }

        private string _description;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description should not be empty")]
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                NotifyPropertyChanged("Description");
            }
        }
        private string _location_code;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Location should not be empty")]
        public string Location_code
        {
            get { return _location_code; }
            set
            {
                _location_code = value;
                NotifyPropertyChanged("Location_code");
            }
        }
        private string _prefix;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Prefix should not be empty")]
        public string Prefix
        {
            get { return _prefix; }
            set
            {
                _prefix = value;
                NotifyPropertyChanged("Prefix");
            }
        }



        private string _beginningNo;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Starting No. should not be empty")]
        [Range(1, int.MaxValue, ErrorMessage = "Starting No. should not be zero!")]
        public string BeginningNo
        {
            get { return _beginningNo; }
            set
            {
                _beginningNo = value;
                NotifyPropertyChanged("BeginningNo");
            }
        }

        private string _endingNo;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Ending No. should not be empty")]
        [Range(1, int.MaxValue, ErrorMessage = "Ending No. should not be zero!")]
        public string EndingNo
        {
            get { return _endingNo; }
            set
            {
                _endingNo = value;
                NotifyPropertyChanged("EndingNo");
            }
        }
    }
}
