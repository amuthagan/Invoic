using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class RawMaterialModel : ViewModelBase
    {
        public RawMaterialModel()
            : base()
        {
            InitializeMandatoryFields(this);
        }

        private string _rm_code;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Raw Material Code should not be empty")]
        public string RM_CODE
        {
            get { return _rm_code; }
            set
            {
                _rm_code = value;
                NotifyPropertyChanged("RM_CODE");
            }
        }

        private string _rm_desc;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Raw Material Description should not be empty")]
        public string RM_DESC
        {
            get { return _rm_desc; }
            set
            {
                _rm_desc = value;
                NotifyPropertyChanged("RM_DESC");
            }
        }
    }
}
