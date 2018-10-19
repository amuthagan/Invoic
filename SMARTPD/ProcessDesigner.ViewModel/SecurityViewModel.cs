using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using ProcessDesigner.BLL;
using System.Windows.Input;
using System.Data;
using System.Windows;

namespace ProcessDesigner.ViewModel
{
   public class SecurityViewModel : ViewModelBase
    {
        private RoleUserSecurityModel _security;
        private RoleUserSecurityDet _securitydet;
        private ICommand _onShowPermissionCommand;
        private ICommand _addRoleCommand;
        private ICommand _deleteRoleCommand;
        private ICommand _setRoleUserCommand;
        //private ICommand _onCancelCommand;
        //public Action CloseAction { get; set; }
        private UserInformation _userInformation;
        private UserRoleDet _userRoleDet;



        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="userInformation"></param>
        public SecurityViewModel(UserInformation userInformation)
        {
            _security = new RoleUserSecurityModel();
            _securitydet = new RoleUserSecurityDet(userInformation);
            _userInformation = userInformation;
            _securitydet.GetUsers(RoleUserSecurities);
            _securitydet.GetRoles(RoleUserSecurities);
        }

        /// <summary>
        /// get set Property of the class RoleUserSecurityModel
        /// </summary>
        public RoleUserSecurityModel RoleUserSecurities
        {
            get
            {
                return _security;
            }
            set
            {
                _security = value;
                NotifyPropertyChanged("RoleUserSecurities");
            }
        }

        /// <summary>
        /// This is a relay command to show the permission screen
        /// </summary>
        public ICommand OnShowPermissionCommand
        {
            get
            {
                if (_onShowPermissionCommand == null)
                {
                    _onShowPermissionCommand = new RelayCommand(param => this.ShowPermission(), null);
                }
                return _onShowPermissionCommand;
            }
        }


        /// <summary>
        /// This is a relay command to show the Add role screen
        /// </summary>
        public ICommand AddRoleCommand
        {
            get
            {
                if (_addRoleCommand == null)
                {
                    _addRoleCommand = new RelayCommand(param => this.ShowAddRole(), null);
                }
                return _addRoleCommand;
            }
        }

        /// <summary>
        /// This is a relay command to delete the selected role in the grid
        /// </summary>
        public ICommand DeleteRoleCommand
        {
            get
            {
                if (_deleteRoleCommand == null)
                {
                    _deleteRoleCommand = new RelayCommand(param => this.DeleteRole(), null);
                }
                return _deleteRoleCommand;
            }
        }

        public ICommand SetRoleUserCommand
        {
            get
            {
                if(_setRoleUserCommand==null)
                {
                    _setRoleUserCommand = new RelayCommand(param => this.SetRoleUser(), null);
                }
                return _setRoleUserCommand;
            }
        }



        /// <summary>
        /// This method is used to show the permission screen
        /// </summary>
        private void ShowPermission()
        {
            try
            {
                String rolename;
                if (RoleUserSecurities.SelectedRole != null)
                {
                    rolename = RoleUserSecurities.SelectedRole.Row["ROLE_NAME"].ToString().ToString();
                    ProcessDesigner.frmPermissions permissions = new ProcessDesigner.frmPermissions(_userInformation, rolename);
                    permissions.ShowInTaskbar = false;
                    //permissions.Owner = App.Current.MainWindow; 
                    permissions.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to show the Add Role screen
        /// </summary>
        private void ShowAddRole()
        {
            frmRolesInfo rolesinfo;
            try
            {
                rolesinfo= new frmRolesInfo(_userInformation);
                rolesinfo.ShowInTaskbar = false;
                //permissions.Owner = App.Current.MainWindow; 
                rolesinfo.ShowDialog();
                _securitydet.GetRoles(RoleUserSecurities);
                NotifyPropertyChanged("RoleUserSecurities");
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to delete the selected role
        /// </summary>
        private void DeleteRole()
        {
            try
            {
                String rolename;
                if (RoleUserSecurities.SelectedRole != null)
                {
                    rolename = RoleUserSecurities.SelectedRole.Row["ROLE_NAME"].ToString().Trim();
                    if (MessageBox.Show("Are you sure you want to delete the role - " + rolename + "?", "Process Designer", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        _userRoleDet = new UserRoleDet(_userInformation);
                        _userRoleDet.DeleteRole(rolename);
                        _securitydet.GetRoles(RoleUserSecurities);
                        NotifyPropertyChanged("RoleUserSecurities");
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void SetRoleUser()
        {
            String username;
            try
            {
                if (RoleUserSecurities.SelectedUser != null)
                {
                    username = RoleUserSecurities.SelectedUser.Row["USER_NAME"].ToString().Trim();
                    frmSecurityUserRoles securityuserroles = new frmSecurityUserRoles(_userInformation,username);
                    securityuserroles.ShowDialog();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
