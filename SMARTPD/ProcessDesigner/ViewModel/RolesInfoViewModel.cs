using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using System.Windows.Input;
using System.Windows;
using ProcessDesigner.Model;
using System.Data;
using ProcessDesigner.Common;
using System.ComponentModel.DataAnnotations;
using ProcessDesigner.UserControls;
namespace ProcessDesigner.ViewModel
{
    class RolesInfoViewModel : ViewModelBase
    {

        private SEC_ROLES_MASTER _secRolesMaster;
        private UserInformation _userInformation;
        private ICommand _onSaveCommand;
        private ICommand _onCancelCommand;
        private UserRoleDet _usrroledet;
        private OperationMode _mode;
        private DataRowView selecteditem;
        private bool _closed = false;

        private string _role_name;



        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userInformation"></param>
        public RolesInfoViewModel(UserInformation userInformation, DataRowView selectedItem, string mode)
        {
            try
            {
                _userInformation = userInformation;
                _secRolesMaster = new SEC_ROLES_MASTER();

                if (mode == "U")
                {
                    selecteditem = selectedItem;
                    _secRolesMaster.ROWID = Guid.Parse(selectedItem["ROWID"].ToString());
                    _secRolesMaster.ROLE_NAME = selectedItem["ROLE_NAME"].ToValueAsString();
                    Role_Name = _secRolesMaster.ROLE_NAME;
                    _mode = OperationMode.Edit;
                }
                else
                {
                    _mode = OperationMode.AddNew;
                }
                _usrroledet = new UserRoleDet(_userInformation);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
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
        /// <summary>
        /// property for the class SEC_ROLES_MASTER
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "Cost centre code is Required")]
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

        [Required(AllowEmptyStrings = false, ErrorMessage = "Role name is Required")]
        public string Role_Name
        {
            get
            {
                return _role_name;
            }
            set
            {
                _role_name = value;
                Sec_Roles_Master.ROLE_NAME = value;
                NotifyPropertyChanged("Role_Name");
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
                if (_onSaveCommand == null)
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
                if (_onCancelCommand == null)
                {
                    _onCancelCommand = new RelayCommand(param => this.Cancel(), null);
                }
                return _onCancelCommand;
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


        public Action CloseAction { get; set; }


        /// <summary>
        /// This method is used to save the roles;
        /// </summary>
        private void SaveRoles()
        {
            try
            {
                if (Sec_Roles_Master.ROLE_NAME.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Role name"));
                    //MessageBox.Show("Role name cannot be empty", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                Sec_Roles_Master.ROLE_NAME = Sec_Roles_Master.ROLE_NAME.ToValueAsString().Trim();
                if (_usrroledet.CheckDuplicate(Sec_Roles_Master, _mode) == false)
                {
                    Progress.ProcessingText = PDMsg.ProgressUpdateText;
                    Progress.Start();
                    
                    if (_usrroledet.SaveRoles(Sec_Roles_Master, _mode) == true)
                    {
                        if (selecteditem != null)
                        {
                            selecteditem["ROLE_NAME"] = Sec_Roles_Master.ROLE_NAME.Trim();
                        }
                        Progress.End();
                        if (_mode == OperationMode.AddNew)
                        {
                            ShowInformationMessage(PDMsg.SavedSuccessfully);
                        }
                        else
                        {
                            ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                        }
                        //MessageBox.Show("The Role was saved successfully", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        if (_closed == false)
                        {
                            _closed = true;
                            CloseAction();
                        }
                    }
                    else
                    {
                        Progress.End();
                        MessageBox.Show("The Role could not be Saved", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    ShowInformationMessage(PDMsg.AlreadyExists("Role"));
                    //MessageBox.Show("Role already exists", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


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
