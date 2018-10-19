using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using System.Windows.Input;
using System.Windows;

namespace ProcessDesigner.ViewModel
{
    class RolesInfoViewModel : ViewModelBase
    {

        private SEC_ROLES_MASTER _secRolesMaster;
        private UserInformation _userInformation;
        private ICommand _onSaveCommand;
        private ICommand _onCancelCommand;
        private UserRoleDet _usrroledet;
        


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userInformation"></param>
        public RolesInfoViewModel(UserInformation userInformation)
        {
            _userInformation= userInformation;
            _secRolesMaster = new SEC_ROLES_MASTER();
            _usrroledet = new UserRoleDet(_userInformation);
        }

        /// <summary>
        /// property for the class SEC_ROLES_MASTER
        /// </summary>
        public SEC_ROLES_MASTER Sec_Roles_Master
        {
            get
            {
                return _secRolesMaster;
            }
            set
            {
                _secRolesMaster = value;
                NotifyPropertyChanged("Sec_Roles_Master");
            }
        }

        /// <summary>
        /// Command to save the role
        /// </summary>
        public ICommand OnSaveCommand
        {
            get
            {
                if(_onSaveCommand == null)
                {
                    _onSaveCommand = new RelayCommand(paam => this.SaveRoles(), null);
                }
                return _onSaveCommand;
            }
        }

        public ICommand OnCancelCommand
        {
            get
            {
                if(_onCancelCommand == null)
                {
                    _onCancelCommand = new RelayCommand(param => this.Cancel(), null);
                }
                return _onCancelCommand;
            }
        }


        public  Action CloseAction { get; set; }


        /// <summary>
        /// This method is used to save the roles;
        /// </summary>
        private void SaveRoles()
        {
            try
            {
                if(Sec_Roles_Master.ROLE_NAME.ToString().Trim()=="")
                {
                    MessageBox.Show("Role name cannot be empty", "Process Designer", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                if (_usrroledet.CheckDuplicate(Sec_Roles_Master.ROLE_NAME) == false)
                {
                    if (_usrroledet.SaveRoles(Sec_Roles_Master) == true)
                    {
                        MessageBox.Show("The Role was saved successfully", "Process Designer", MessageBoxButton.OK, MessageBoxImage.Information);
                        CloseAction();
                    }
                    else
                    {
                        MessageBox.Show("The Role could not be Saved", "Process Designer", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Role already exists", "Process Designer", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
