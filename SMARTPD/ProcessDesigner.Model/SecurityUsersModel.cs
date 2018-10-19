using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Data;


namespace ProcessDesigner.Model
{
    public class SecurityUsersModel : ViewModelBase
    {
        private string _userName = "";
        private string _fullName = "";
        private string _designation = "";
        private string _password = "";
        private string _mode = "";
        private string _status = "";
        private bool _is_admin = false;
        public DataView Design { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "User Name is Required")]
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyPropertyChanged("UserName");
            }
        }


        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                NotifyPropertyChanged("FullName");
            }
        }

        public string Designation
        {
            get { return _designation; }
            set
            {
                _designation = value;
                NotifyPropertyChanged("Designation");
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is Required")]
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyPropertyChanged("Password");
            }
        }

        public string Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                NotifyPropertyChanged("Mode");
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        public bool IsAdmin
        {
            get { return _is_admin; }
            set
            {
                _is_admin = value;
                NotifyPropertyChanged("IsAdmin");
            }
        }
    }
}
