using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProcessDesigner.Model
{
    public class PSWTitleMasterModel : ViewModelBase
    {
        private string _pswName;
        private string _pswTitle;
        private DataView _dtpswMasterDetails;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is Required")]
        public string PSWName
        {
            get { return _pswName; }
            set
            {
                _pswName = value;
                NotifyPropertyChanged("PSWName");
            }
        }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Title is Required")]
        public string PSWTitle
        {
            get { return _pswTitle; }
            set
            {
                _pswTitle = value;
                NotifyPropertyChanged("PSWTitle");
            }
        }
        private bool? _isActive = null;
        public bool? IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                NotifyPropertyChanged("IsActive");
            }
        }
        public DataView PSWTitleMasterDetails
        {
            get { return _dtpswMasterDetails; }
            set
            {
                _dtpswMasterDetails = value;
                NotifyPropertyChanged("PSWTitleMasterDetails");
            }
        }

    }
}
