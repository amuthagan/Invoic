using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.Model;
using ProcessDesigner.BLL;
using System.Windows.Input;
using System.Data;
using ProcessDesigner.Common;
using System.Windows;
using System.ComponentModel;

namespace ProcessDesigner.ViewModel
{
    public class SecurityUsersViewModel : ViewModelBase
    {
        private SecurityUsersModel _securityUsers;
        private readonly ICommand _saveCommand;
        private SecurityUsersBll _securityUserBll;
        public ICommand SaveCommand { get { return this._saveCommand; } }
        private readonly ICommand _closeCommand;
        public ICommand CloseCommand { get { return this._closeCommand; } }
        public Action CloseAction { get; set; }
        private DataRowView _selectedItem = null;
        private bool _userNameIsEnable = false;
        private bool _closed = false;

        public SecurityUsersViewModel(UserInformation userInfo, DataRowView selectedItem, string mode)
        {
            _securityUsers = new SecurityUsersModel();
            _securityUserBll = new SecurityUsersBll(userInfo);
            this._saveCommand = new DelegateCommand(this.Save);
            this._closeCommand = new DelegateCommand(this.Close);
            _selectedItem = selectedItem;
            SecurityUsers.UserName = _selectedItem.Row["USER_NAME"].ToString();
            SecurityUsers.FullName = _selectedItem.Row["FULL_NAME"].ToString();
            SecurityUsers.Designation = _selectedItem.Row["DESIGNATION"].ToString();
            SecurityUsers.Password = _selectedItem.Row["PASSWORD"].ToString();
            SecurityUsers.IsAdmin = _selectedItem.Row["IS_ADMIN"].ToBooleanAsString();
            SecurityUsers.Mode = mode;
            if (mode == "U")
            {
                UserNameIsEnable = true;
                _selectedItem.Row["PASSWORD"] = "!@#$%^&*()";
                SecurityUsers.Password = "!@#$%^&*()";
            }
            else if (mode == "D")
            {
                this.Save();
            }
            _securityUserBll.GetDesignation(SecurityUsers);
            if (_securityUserBll.CheckIsAdminAvailable())
            {
                if (SecurityUsers.IsAdmin == true)
                {
                    AdminVisible = Visibility.Visible;
                }
                else
                {
                    AdminVisible = Visibility.Collapsed;
                }
            }
            else
            {
                if (_securityUserBll.GetUserRole(SecurityUsers.UserName).ToValueAsString().ToUpper() == "ADMINISTRATOR")
                    AdminVisible = Visibility.Visible;
                else
                    AdminVisible = Visibility.Collapsed;
            }
        }

        public SecurityUsersModel SecurityUsers
        {
            get { return _securityUsers; }
            set
            {
                _securityUsers = value;
                NotifyPropertyChanged("SecurityUsers");
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
        public bool UserNameIsEnable
        {
            get { return _userNameIsEnable; }
            set
            {
                _userNameIsEnable = value;
                NotifyPropertyChanged("UserNameIsEnable");
            }
        }

        private Visibility _adminVisible;
        public Visibility AdminVisible
        {
            get { return _adminVisible; }
            set
            {
                _adminVisible = value;
                NotifyPropertyChanged("AdminVisible");
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

        public void Save()
        {
            try
            {
                if (SecurityUsers.Mode == "I" || SecurityUsers.Mode == "U")
                {
                    if (SecurityUsers.UserName.ToString() == "")
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("User Name"));
                        //MessageBox.Show("User Name should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    if (SecurityUsers.Password.ToString() == "")
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Password"));
                        //MessageBox.Show("Password should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    _securityUserBll.UpdateSecurityUsers(SecurityUsers);
                    if (SecurityUsers.Status != "")
                    {
                        _selectedItem.BeginEdit();
                        _selectedItem.Row["USER_NAME"] = SecurityUsers.UserName;
                        _selectedItem.Row["FULL_NAME"] = SecurityUsers.FullName;
                        _selectedItem.Row["DESIGNATION"] = SecurityUsers.Designation;
                        _selectedItem.Row["IS_ADMIN"] = SecurityUsers.IsAdmin;
                        _selectedItem.Row["PASSWORD"] = "!@#$%^&*()";
                        _selectedItem.EndEdit();
                        MessageBox.Show(SecurityUsers.Status, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        if (SecurityUsers.Status != "Given user already exist.")
                        {
                            if (_closed == false)
                            {
                                _closed = true;
                                CloseAction();
                            }
                        }
                        if (_closed == false)
                        {
                            _closed = true;
                            CloseAction();
                        }
                    }


                }
                else if (SecurityUsers.Mode == "D")
                {
                    _securityUserBll.UpdateSecurityUsers(SecurityUsers);
                    if (SecurityUsers.Status != "")
                    {
                        MessageBox.Show(SecurityUsers.Status, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Close()
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
                throw ex.LogException();
            }
        }

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }
    }
}
