using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using System.Data;
using System.Windows.Input;

namespace ProcessDesigner.ViewModel
{
    class PermissionViewModel : ViewModelBase
    {

        private SEC_ROLE_OBJECT_PERMISSION _secRoleObjectPermission;
        private PermissionDet _permissionDet;
        //private DataTable _permission;
        private List<SEC_ROLE_OBJECT_PERMISSION> _permission;
        private string _roleName;
        private ICommand _onSaveCommand;
        private ICommand _onCancelCommand;
        

        private SEC_ROLE_OBJECT_PERMISSION _selectedRow;


        public PermissionViewModel(UserInformation userInformation, string roleName)
        {
            _secRoleObjectPermission = new SEC_ROLE_OBJECT_PERMISSION();
            _permissionDet = new PermissionDet(userInformation);
            Permission = _permissionDet.GetRolePermissionList(roleName);
            _roleName = roleName;
        }

        public Action CloseAction { get; set; }

        public SEC_ROLE_OBJECT_PERMISSION SecRoleObjectPermission
        {
            get
            {
                return _secRoleObjectPermission;
            }
            set
            {
                _secRoleObjectPermission = value;
                NotifyPropertyChanged("SecRoleObjectPermission");
            }
        }

        public List<SEC_ROLE_OBJECT_PERMISSION> Permission
        {
            get
            {
                return _permission;
            }
            set
            {
                _permission = value;
                NotifyPropertyChanged("Permission");
            }
        }

        public string Role_Name
        {
            get
            {
                return _roleName;
            }
            set
            {
                _roleName = value;
                NotifyPropertyChanged("Role_Name");
            }
        }

        public SEC_ROLE_OBJECT_PERMISSION SelectedRow 
        { 
            get
            {
                return _selectedRow;
            }
            set
            {
                _selectedRow = value;
                NotifyPropertyChanged("SelectedRow");
            }    
        }


        /// <summary>
        /// This is a relay command to save the rights for the selected role
        /// </summary>
        public ICommand OnSaveCommand
        {
            get
            {
                if (_onSaveCommand == null)
                {
                    _onSaveCommand = new RelayCommand(param => this.SavePermission(), null);
                }
                return _onSaveCommand;
            }
        }

        public ICommand OnCancelCommand
        {
            get
            {
                if (_onCancelCommand == null)
                {
                    _onCancelCommand = new RelayCommand(param => this.Cancel(), null);
                }
                return _onCancelCommand;
            }
        }


        /// <summary>
        /// Save the permission for each role
        /// </summary>
        private void SavePermission()
        {
            try
            {
                _permissionDet.SavePermission(Permission, Role_Name);
                CloseAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Cancel()
        {
            try
            {
                CloseAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
