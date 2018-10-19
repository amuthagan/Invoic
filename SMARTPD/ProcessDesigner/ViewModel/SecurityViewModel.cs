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
using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.Common;

namespace ProcessDesigner.Model
{
    public class SecurityViewModel : ViewModelBase
    {
        private RoleUserSecurityModel _security;
        private RoleUserSecurityDet _securitydet;
        private ICommand _onShowPermissionCommand;
        private ICommand _addRoleCommand;
        private ICommand _deleteRoleCommand;
        private ICommand _modifyRoleCommand;
        private ICommand _setRoleUserCommand;
        //private ICommand _onCancelCommand;
        //public Action CloseAction { get; set; }
        private UserInformation _userInformation;
        private UserRoleDet _userRoleDet;
        private readonly ICommand _addCommand;
        public ICommand AddCommand { get { return this._addCommand; } }
        private readonly ICommand _modifyCommand;
        public ICommand ModifyCommand { get { return this._modifyCommand; } }
        private readonly ICommand _deleteCommand;
        public ICommand DeleteCommand { get { return this._deleteCommand; } }


        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="userInformation"></param>
        public SecurityViewModel(UserInformation userInformation)
        {
            _security = new RoleUserSecurityModel();
            _securitydet = new RoleUserSecurityDet(userInformation);
            this._modifyCommand = new DelegateCommand<DataRowView>(this.ModifyUsers);
            this._deleteCommand = new DelegateCommand<DataRowView>(this.DeleteUsers);
            this._addCommand = new DelegateCommand<DataView>(this.AddUsers);
            _userInformation = userInformation;
            _userRoleDet = new UserRoleDet(_userInformation);
            _securitydet.GetUsers(RoleUserSecurities);
            _securitydet.GetRoles(RoleUserSecurities);
            DefaultSelectedItem();
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

        /// <summary>
        /// This is a relay command to modify the selected role in the grid
        /// </summary>
        public ICommand ModifyRoleCommand
        {
            get
            {
                if (_modifyRoleCommand == null)
                {
                    _modifyRoleCommand = new RelayCommand(param => this.ModifyRole(), null);
                }
                return _modifyRoleCommand;
            }
        }

        public ICommand SetRoleUserCommand
        {
            get
            {
                if (_setRoleUserCommand == null)
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
                    permissions.ShowInTaskbar = true;
                    permissions.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
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
                rolesinfo = new frmRolesInfo(_userInformation, null, "I");
                rolesinfo.ShowInTaskbar = false;
                //permissions.Owner = App.Current.MainWindow; 
                rolesinfo.ShowDialog();
                _securitydet.GetRoles(RoleUserSecurities);
                NotifyPropertyChanged("RoleUserSecurities");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// This method is used to delete the selected role
        /// </summary>
        /// 

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        private MessageBoxResult ShowWarningMessage(string _showMessage, MessageBoxButton messageBoxButton)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, messageBoxButton, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void DeleteRole()
        {
            try
            {
                String rolename;
                if (RoleUserSecurities.SelectedRole != null)
                {
                    rolename = RoleUserSecurities.SelectedRole.Row["ROLE_NAME"].ToString().Trim();
                    if (rolename.ToString().ToUpper() == "ADMINISTRATOR")
                    {
                        MessageBox.Show("The role '" + rolename + "' cannot be deleted.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    if (_userRoleDet.CheckUserExistForRole(rolename) == true)
                    {
                        MessageBox.Show("The role '" + rolename + "' cannot be deleted. User exists for the role!", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    if (MessageBox.Show("Do you want to delete the role - " + rolename + "?", "SmartPD", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        _userRoleDet.DeleteRole(rolename);
                        _securitydet.GetRoles(RoleUserSecurities);
                        NotifyPropertyChanged("RoleUserSecurities");
                        ShowInformationMessage(PDMsg.DeletedSuccessfully);
                        //MessageBox.Show("Records Deleted sucessfully", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// This method is used to modify the selected role
        /// </summary>
        private void ModifyRole()
        {
            frmRolesInfo rolesinfo;
            string rolename;
            try
            {
                if (RoleUserSecurities.SelectedRole != null)
                {
                    rolename = RoleUserSecurities.SelectedRole.Row["ROLE_NAME"].ToString().Trim();
                    if (rolename.ToString().ToUpper() == "ADMINISTRATOR")
                    {
                        MessageBox.Show("The role '" + rolename + "' cannot be edited.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    rolesinfo = new frmRolesInfo(_userInformation, RoleUserSecurities.SelectedRole, "U");
                    rolesinfo.ShowInTaskbar = false;
                    //permissions.Owner = App.Current.MainWindow; 
                    rolesinfo.ShowDialog();
                    NotifyPropertyChanged("RoleUserSecurities");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        /// <summary>
        /// This method used to set the role for the selected user
        /// </summary>
        private void SetRoleUser()
        {
            String username;
            frmSecurityUserRoles securityuserroles;
            try
            {
                if (RoleUserSecurities.SelectedUser != null)
                {
                    username = RoleUserSecurities.SelectedUser.Row["USER_NAME"].ToString().Trim();
                    securityuserroles = new frmSecurityUserRoles(_userInformation, username);
                    securityuserroles.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }



        public void DefaultSelectedItem()
        {

            try
            {

                if (RoleUserSecurities.Users != null)
                {
                    if (RoleUserSecurities.Users.Count > 0)
                    {
                        RoleUserSecurities.SelectedUser = RoleUserSecurities.Users[0];
                    }
                }
                if (RoleUserSecurities.Roles != null)
                {
                    if (RoleUserSecurities.Roles.Count > 0)
                    {
                        RoleUserSecurities.SelectedRole = RoleUserSecurities.Roles[0];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Confirm to close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                SecurityUsersBll secuserbll = new SecurityUsersBll(_userInformation);
                WPF.MDI.ClosingEventArgs closingev;
                closingev = (WPF.MDI.ClosingEventArgs)e;
                if (secuserbll.CheckIsAdminAvailable() == false)
                {
                    closingev.Cancel = true;
                    MessageBox.Show("Please select ADMIN rights for any one administrator user", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    e = closingev;
                }
                else
                {
                    if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                    {
                        closingev.Cancel = true;
                        e = closingev;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        public void AddUsers(DataView itemsource)
        {
            try
            {
                if (itemsource != null)
                {
                    itemsource.AllowNew = true;
                    DataRowView selecteditem = itemsource.AddNew();
                    frmSecurityUsers securityusers = new frmSecurityUsers(_userInformation, selecteditem, "I");
                    securityusers.ShowDialog();
                    if (selecteditem.Row["USER_NAME"].ToString() == "")
                    {
                        selecteditem.Delete();
                    }
                    if (RoleUserSecurities.Users.Count > 0)
                    {
                        RoleUserSecurities.SelectedUser = RoleUserSecurities.Users[RoleUserSecurities.Users.Count - 1];
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        public void ModifyUsers(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    frmSecurityUsers securityusers = new frmSecurityUsers(_userInformation, selecteditem, "U");
                    securityusers.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }


        public void DeleteUsers(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    if (selecteditem["IS_ADMIN"].ToBooleanAsString() == true)
                    {
                        ShowInformationMessage("Admin user " + selecteditem.Row["USER_NAME"].ToValueAsString() + " cannot be deleted");
                        return;
                    }
                    MessageBoxResult result = MessageBox.Show("Do you want to delete the User " + selecteditem.Row["USER_NAME"].ToString() + "?", "SmartPD", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        frmSecurityUsers securityusers = new frmSecurityUsers(_userInformation, selecteditem, "D");
                        securityusers = null;
                        selecteditem.Delete();
                        if (RoleUserSecurities.Users.Count > 0)
                        {
                            RoleUserSecurities.SelectedUser = RoleUserSecurities.Users[0];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        public void grdUsersPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DependencyObject src = System.Windows.Media.VisualTreeHelper.GetParent((DependencyObject)e.OriginalSource);
                if (src.GetType() == typeof(System.Windows.Controls.ContentPresenter))
                {

                    if (RoleUserSecurities.SelectedUser != null)
                    {
                        frmSecurityUsers securityusers = new frmSecurityUsers(_userInformation, RoleUserSecurities.SelectedUser, "U");
                        securityusers.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void grdRolePreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DependencyObject src = System.Windows.Media.VisualTreeHelper.GetParent((DependencyObject)e.OriginalSource);
                if (src.GetType() == typeof(System.Windows.Controls.ContentPresenter))
                {
                    ModifyRole();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }


    }
}
