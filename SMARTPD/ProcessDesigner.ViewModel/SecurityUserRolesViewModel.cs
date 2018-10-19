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


namespace ProcessDesigner.ViewModel
{
    class SecurityUserRolesViewModel : ViewModelBase
    {
        private ObservableCollection<SEC_ROLES_MASTER> _sec_Roles_Master_List;
        private ObservableCollection<SEC_USER_ROLES> _sec_User_Roles_List;

        


        private SecurityUserRolesDet _securityUserRolesDet;
        private string _userName;
        
        private System.Collections.IList _selectedItemsRoleMaster;
        private System.Collections.IList _selectedItemsUserRole;


        private ICommand _addAllCommand;
        private ICommand _addSelectedCommand;
        private ICommand _removeSelectedCommand;
        private ICommand _removeAllCommand;
        private ICommand _saveCommand;

        private ICommand _selCommand;


        /// <summary>
        /// Constructor for the class SecurityUserRolesViewModel
        /// </summary>
        public SecurityUserRolesViewModel(string username)
        {
            try
            {
                _securityUserRolesDet = new SecurityUserRolesDet();
                _sec_Roles_Master_List = _securityUserRolesDet.GetAvailableRoles(username);
                _sec_User_Roles_List = _securityUserRolesDet.GetAvailableRolesForTheUser(username);
                _userName = username;
            }
            catch (Exception ex)
            {
                throw throw ex;
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

        public object IsSelected{ get; set; }


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

        public ICommand SaveCommand
        {
            get
            {
                if(_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(param => this.SaveRole(), null);
                }
                return _saveCommand;
            }
        }

        /// <summary>
        /// 
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
                MessageBox.Show(ex.Message, "Process Designer");
            }
        }

        public void SelectionChangedUserRole(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                _selectedItemsUserRole = ((System.Windows.Controls.ListBox)sender).SelectedItems;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Process Designer");
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Process Designer");
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
            }
            catch (Exception ex)
            {
                throw throw ex;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Process Designer");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Process Designer");
            }
        }

        /// <summary>
        /// Save the selected user roles
        /// </summary>
        private void SaveRole()
        {
            try
            {
                _securityUserRolesDet.SaveRolesForTheUser(Sec_User_Roles_List, _userName);
                CloseAction();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Process Designer");
            }
        }
    }
}
