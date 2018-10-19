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
    public class CPMMasterModel : ViewModelBase
    {

        private DataView _dtCpmDeptDetails;
        private DataView _dtCpmMemberDetails;
        public DataView CPMDeptMasterDetails
        {
            get { return _dtCpmDeptDetails; }
            set
            {
                _dtCpmDeptDetails = value;
                NotifyPropertyChanged("CPMDeptMasterDetails");
            }
        }
        public DataView CPMMemberMasterDetails
        {
            get { return _dtCpmMemberDetails; }
            set
            {
                _dtCpmMemberDetails = value;
                NotifyPropertyChanged("CPMMemberMasterDetails");
            }
        }

        private decimal _sNo;
        private string _member;
        private string _dept;
        public decimal SNO
        {
            get { return _sNo; }
            set
            {
                _sNo = value;
                NotifyPropertyChanged("SNO");
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Member is Required")]
        public string MEMBER
        {
            get { return _member; }
            set
            {
                _member = value;
                NotifyPropertyChanged("MEMBER");
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "DEPT is Required")]
        public string DEPT
        {
            get { return _dept; }
            set
            {
                _dept = value;
                NotifyPropertyChanged("DEPT");
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
    }
}
