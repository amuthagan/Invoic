using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using ProcessDesigner.BLL;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;
using ProcessDesigner.Common;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.Model
{
    class SecurityUserRolesViewModel : ViewModelBase
    {
        private ObservableCollection<SEC_ROLES_MASTER> _sec_Roles_Master_List;
        private ObservableCollection<SEC_USER_ROLES> _sec_User_Roles_List;

        private SecurityUserRolesDet _securityUserRolesDet;
        private string _userName;
        private bool _enableAddSelectedRole;
        private bool _enableRemoveSelectedRole;

        private System.Collections.IList _selectedItemsRoleMaster;
        private System.Collections.IList _selectedItemsUserRole;

        private System.Windows.Controls.ListBox _lstRoles;
        private System.Windows.Controls.ListBox _lstUserRoles;

        private ICommand _addAllCommand;
        private ICommand _addSelectedCommand;
        private ICommand _removeSelectedCommand;
        private ICommand _removeAllCommand;
        private ICommand _saveCommand;
        private bool _closed = false;
        private ICommand _cancelCommand;

        /// <summary>
        /// Constructor for the class SecurityUserRolesViewModel
        /// </summary>
        public SecurityUserRolesViewModel(UserInformation userInformation, string username)
        {
            try
            {
                _securityUserRolesDet = new SecurityUserRolesDet(userInformation);
                _sec_Roles_Master_List = _securityUserRolesDet.GetAvailableRoles(username);
                _sec_User_Roles_List = _securityUserRolesDet.GetAvailableRolesForTheUser(username);
                _userName = username;
                EnableOrDisableSelect();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public string User_Name
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                NotifyPropertyChanged("User_Name");
            }
        }

        public bool EnableAddSelectedRole
        {
            get
            {
                return _enableAddSelectedRole;
            }
            set
            {
                _enableAddSelectedRole = value;
                NotifyPropertyChanged("EnableAddSelectedRole");
            }
        }
        public void CloseMethodWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (_closed == false)
                {
                    if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public bool EnableRemoveSelectedRole
        {
            get
            {
                return _enableRemoveSelectedRole;
            }
            set
            {
                _enableRemoveSelectedRole = value;
                NotifyPropertyChanged("EnableRemoveSelectedRole");
            }
        }



        /// <summary>
        /// Property to set and get the list of master roles
        /// </summary>
        public ObservableCollection<SEC_ROLES_MASTER> Sec_Roles_Master_List
        {
            get
            {
                return _sec_Roles_Master_List;
            }
            set
            {
                _sec_Roles_Master_List = value;
                NotifyPropertyChanged("Sec_Roles_Master_List");
            }
        }

        /// <summary>
        /// Property to set and get the list of user roles
        /// </summary>
        public ObservableCollection<SEC_USER_ROLES> Sec_User_Roles_List
        {
            get
            {
                return _sec_User_Roles_List;
            }
            set
            {
                _sec_User_Roles_List = value;
                NotifyPropertyChanged("Sec_User_Roles_List");
            }
        }

        public Action CloseAction { get; set; }

        public object IsSelected { get; set; }

        /// <summary>
        /// Relay command for Add all
        /// </summary>
        public ICommand AddAllCommand
        {
            get
            {
                if (_addAllCommand == null)
                {
                    _addAllCommand = new RelayCommand(param => this.AddAllRoles(), null);
                }
                return _addAllCommand;
            }
        }

        /// <summary>
        /// Relay command for remove all
        /// </summary>
        public ICommand RemoveAllCommand
        {
            get
            {
                if (_removeAllCommand == null)
                {
                    _removeAllCommand = new RelayCommand(param => this.RemoveAllRoles(), null);
                }
                return _removeAllCommand;
            }
        }

        /// <summary>
        /// Relay command for remove
        /// </summary>
        public ICommand RemoveSelectedCommand
        {
            get
            {
                if (_removeSelectedCommand == null)
                {
                    _removeSelectedCommand = new RelayCommand(param => this.RemoveSelectedRoles(), null);
                }
                return _removeSelectedCommand;
            }
        }

        /// <summary>
        /// Relay command for select command
        /// </summary>
        public ICommand AddSelectedCommand
        {
            get
            {
                if (_addSelectedCommand == null)
                {
                    _addSelectedCommand = new RelayCommand(param => this.AddSelectedRoles(), null);
                }
                return _addSelectedCommand;
            }
        }

        /// <summary>
        /// Relay command for save
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(param => this.SaveRole(), null);
                }
                return _saveCommand;
            }
        }


        /// <summary>
        /// relay command for cancel
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                {
                    _cancelCommand = new RelayCommand(param => this.Cancel(), null);
                }
                return _cancelCommand;
            }
        }


        /// <summary>
        /// get the selected master role in the list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SelectionChangedRoleMaster(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                _selectedItemsRoleMaster = ((System.Windows.Controls.ListBox)sender).SelectedItems;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// get the selected user role in the list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void SelectionChangedUserRole(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                _selectedItemsUserRole = ((System.Windows.Controls.ListBox)sender).SelectedItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SmartPD");
                throw ex.LogException();
            }
        }


        /// <summary>
        /// Load method for the current window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadMethod(object sender, EventArgs e)
        {
            try
            {
                _lstRoles = (System.Windows.Controls.ListBox)((System.Windows.Window)sender).FindName("lstRoles");
                _lstUserRoles = (System.Windows.Controls.ListBox)((System.Windows.Window)sender).FindName("lstUserRoles");
                SelectListItem();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Add all the roles
        /// </summary>
        private void AddAllRoles()
        {
            SEC_USER_ROLES secuserroles;
            try
            {
                foreach (SEC_ROLES_MASTER srm in Sec_Roles_Master_List)
                {
                    secuserroles = new SEC_USER_ROLES();
                    secuserroles.ROLE_NAME = srm.ROLE_NAME;
                    secuserroles.USER_NAME = _userName;
                    Sec_User_Roles_List.Add(secuserroles);
                }
                Sec_Roles_Master_List.Clear();
                NotifyPropertyChanged("Sec_User_Roles_List");
                NotifyPropertyChanged("Sec_Roles_Master_List");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Add all the roles
        /// </summary>
        private void RemoveAllRoles()
        {
            SEC_ROLES_MASTER secrolesmaster;
            try
            {
                foreach (SEC_USER_ROLES srm in Sec_User_Roles_List)
                {
                    secrolesmaster = new SEC_ROLES_MASTER();
                    secrolesmaster.ROLE_NAME = srm.ROLE_NAME;
                    Sec_Roles_Master_List.Add(secrolesmaster);
                }
                Sec_User_Roles_List.Clear();
                NotifyPropertyChanged("Sec_User_Roles_List");
                NotifyPropertyChanged("Sec_Roles_Master_List");
                SelectListItem();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        /// <summary>
        /// Remove the selected roles
        /// </summary>
        private void RemoveSelectedRoles()
        {
            SEC_ROLES_MASTER secrolesmaster;
            try
            {
                if (_selectedItemsUserRole != null)
                {
                    while (_selectedItemsUserRole.Count > 0)
                    {
                        SEC_USER_ROLES srm = (SEC_USER_ROLES)_selectedItemsUserRole[0];
                        secrolesmaster = new SEC_ROLES_MASTER();
                        secrolesmaster.ROLE_NAME = srm.ROLE_NAME;
                        Sec_Roles_Master_List.Add(secrolesmaster);
                        Sec_User_Roles_List.Remove(srm);
                    }
                    NotifyPropertyChanged("Sec_User_Roles_List");
                    NotifyPropertyChanged("Sec_Roles_Master_List");
                }
                SelectListItem();
                EnableOrDisableSelect();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        /// <summary>
        /// Add the selected roles
        /// </summary>
        private void AddSelectedRoles()
        {
            SEC_USER_ROLES secuserroles;
            try
            {
                if (_selectedItemsRoleMaster != null)
                {
                    while (_selectedItemsRoleMaster.Count > 0)
                    {
                        SEC_ROLES_MASTER srm = (SEC_ROLES_MASTER)_selectedItemsRoleMaster[0];
                        secuserroles = new SEC_USER_ROLES();
                        secuserroles.ROLE_NAME = srm.ROLE_NAME;
                        secuserroles.USER_NAME = _userName;
                        Sec_Roles_Master_List.Remove(srm);
                        Sec_User_Roles_List.Add(secuserroles);
                    }
                    NotifyPropertyChanged("Sec_User_Roles_List");
                    NotifyPropertyChanged("Sec_Roles_Master_List");
                }
                SelectListItem();
                EnableOrDisableSelect();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SmartPD");
                throw ex.LogException();
            }
        }

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

        /// <summary>
        /// Save the selected user roles
        /// </summary>
        private void SaveRole()
        {
            try
            {
                Progress.ProcessingText = PDMsg.ProgressUpdateText;
                Progress.Start();

                if (_securityUserRolesDet.SaveRolesForTheUser(Sec_User_Roles_List, _userName) == true)
                {
                    Progress.End();
                    ShowInformationMessage(PDMsg.SavedSuccessfully);
                    //MessageBox.Show("All Roles have been Saved Successfully", "SmartPD");
                    CloseAction();
                }
                else
                {
                    Progress.End();
                }
                if (_closed == false)
                {
                    _closed = true;
                    CloseAction();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// close the current view
        /// </summary>
        private void Cancel()
        {
            try
            {
                _closed = false;
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                {
                    _closed = true;
                    CloseAction();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SmartPD");
                throw ex.LogException();
            }
        }

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        /// <summary>
        /// Set the selected item for the list box
        /// </summary>
        private void SelectListItem()
        {
            try
            {
                if (_lstRoles.Items.Count > 0)
                {
                    _lstRoles.SelectedIndex = 0;
                    _selectedItemsRoleMaster = _lstRoles.SelectedItems;
                }

                if (_lstUserRoles.Items.Count > 0)
                {
                    _lstUserRoles.SelectedIndex = 0;
                    _selectedItemsUserRole = _lstUserRoles.SelectedItems;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void EnableOrDisableSelect()
        {
            try
            {
                if (Sec_User_Roles_List.Count > 0)
                {
                    EnableAddSelectedRole = false;
                    EnableRemoveSelectedRole = true;
                }
                else
                {
                    EnableAddSelectedRole = true;
                    EnableRemoveSelectedRole = false;
                }
                NotifyPropertyChanged("EnableAddSelectedRole");
                NotifyPropertyChanged("EnableRemoveSelectedRole");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}
