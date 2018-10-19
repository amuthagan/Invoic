using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class ApplicationModel : ViewModelBase
    {
        private int _sno;
        public int SNo
        {
            get { return _sno; }
            set 
            { 
                _sno = value;
                NotifyPropertyChanged("SNo");
            }
        }

        private string _application;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Application is Required")]
        public string PSWApplication
        {
            get { return _application; }
            set
            {
                _application = value;
                NotifyPropertyChanged("PSWApplication");
            }
        }

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                NotifyPropertyChanged("Active");
            }
        }

        private bool _inactive;
        public bool InActive
        {
            get { return _inactive; }
            set
            {
                _inactive = value;
                NotifyPropertyChanged("InActive");
            }
        }

        private DataView _pswapplicationview;
        public DataView PSWApplicationView
        {
            get { return _pswapplicationview; }
            set
            {
                this._pswapplicationview = value;
                NotifyPropertyChanged("PSWApplication");
            }
        }
    }
}
