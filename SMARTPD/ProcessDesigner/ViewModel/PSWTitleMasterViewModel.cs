using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Data;
using System.Windows;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.ViewModel
{
    class PSWTitleMasterViewModel : ViewModelBase
    {
        private PSWTitleMasterBll _pswTitleBll;
        private PSWTitleMasterModel _pswTitleModel;
        private readonly ICommand _onAddCommand;
        private OperationMode _operationmode;
        public ICommand OnAddCommand { get { return this._onAddCommand; } }

        private readonly ICommand _onEditViewCommand;
        public ICommand OnEditViewCommand { get { return this._onEditViewCommand; } }

        private readonly ICommand _onSaveCommand;
        public ICommand OnSaveCommand { get { return this._onSaveCommand; } }

        private readonly ICommand _onCloseCommand;
        public ICommand OnCloseCommand { get { return this._onCloseCommand; } }
        public Action CloseAction { get; set; }
        private readonly ICommand _onDeleteCommand;
        public ICommand OnDeleteCommand { get { return this._onDeleteCommand; } }
        private bool _addButtonIsEnable = false;
        private bool _editButtonIsEnable = true;
        private bool _saveButtonIsEnable = true;
        private bool _deleteButtonIsEnable = false;
        public PSWTitleMasterViewModel(UserInformation userInformation)
        {
            this.PSWTitleMaster = new PSWTitleMasterModel();
            this.selectChangeComboCommandName = new DelegateCommand(this.SelectDataRow);
            _pswTitleBll = new PSWTitleMasterBll(userInformation);
            this._onAddCommand = new DelegateCommand(this.Add);
            this._onEditViewCommand = new DelegateCommand(this.Edit);
            this._onSaveCommand = new DelegateCommand(this.Save);
            this._onCloseCommand = new DelegateCommand(this.Close);
            this._onDeleteCommand = new DelegateCommand(this.Delete);
            GetRights();
            AddButtonIsEnable = true;
            Add();
            SetdropDownItems();

            setRights();

        }

        private bool _focusButton = false;
        public bool FocusButton
        {
            get { return _focusButton; }
            set
            {
                _focusButton = value;
                NotifyPropertyChanged("FocusButton");
            }
        }


        private bool _focusButtonName = false;
        public bool FocusButtonName
        {
            get { return _focusButtonName; }
            set
            {
                _focusButtonName = value;
                NotifyPropertyChanged("FocusButtonName");
            }
        }

        public PSWTitleMasterModel PSWTitleMaster
        {
            get
            {
                return _pswTitleModel;
            }
            set
            {
                this._pswTitleModel = value;
                NotifyPropertyChanged("PSWTitleMaster");
            }
        }
        private bool _isNameReadOnly = false;
        public bool IsNameReadonly
        {
            get { return _isNameReadOnly; }
            set
            {
                this._isNameReadOnly = value;
                NotifyPropertyChanged("IsNameReadonly");
            }
        }
        public bool AddButtonIsEnable
        {
            get { return _addButtonIsEnable; }
            set
            {
                this._addButtonIsEnable = value;
                NotifyPropertyChanged("AddButtonIsEnable");
            }
        }

        public bool EditButtonIsEnable
        {
            get { return _editButtonIsEnable; }
            set
            {
                this._editButtonIsEnable = value;
                NotifyPropertyChanged("EditButtonIsEnable");
            }
        }

        public bool SaveButtonIsEnable
        {
            get { return _saveButtonIsEnable; }
            set
            {
                this._saveButtonIsEnable = value;
                NotifyPropertyChanged("SaveButtonIsEnable");
            }
        }

        public bool DeleteButtonIsEnable
        {
            get { return _deleteButtonIsEnable; }
            set
            {
                this._deleteButtonIsEnable = value;
                NotifyPropertyChanged("DeleteButtonIsEnable");
            }
        }
        private RolePermission _actionPermission;
        public RolePermission ActionPermission
        {
            get { return _actionPermission; }
            set
            {
                this._actionPermission = value;
                NotifyPropertyChanged("ActionPermission");

            }
        }
        private void GetRights()
        {
            ActionPermission = new RolePermission();
            ActionPermission.Save = true;
            ActionPermission.Print = false;
            ActionPermission.View = true;
            ActionPermission.AddNew = false;
            ActionPermission.Delete = false;
            ActionPermission.Edit = false;
            ActionPermission = _pswTitleBll.GetUserRights("PSW TITLE MASTER");
            if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
            {
                ActionPermission.Save = false;
            }
        }

        private void setRights()
        {
            if (AddButtonIsEnable) AddButtonIsEnable = ActionPermission.AddNew;
            if (EditButtonIsEnable) EditButtonIsEnable = ActionPermission.Edit;
            if (DeleteButtonIsEnable)
            {
                DeleteButtonIsEnable = ActionPermission.Delete;
                IsDeleteEnable = true;

            }
            if (SaveButtonIsEnable) SaveButtonIsEnable = ActionPermission.Save;
        }

        private DataRowView _selectedrowpsw;
        public DataRowView SelectedRowPsw
        {
            get
            {
                return _selectedrowpsw;
            }

            set
            {
                _selectedrowpsw = value;
            }
        }
        private Visibility _buttonVisible = Visibility.Visible;
        public Visibility ButtonVisibleName
        {
            get { return _buttonVisible; }
            set
            {
                this._buttonVisible = value;
                NotifyPropertyChanged("ButtonVisibleName");

            }
        }

        private Visibility _isVisibilityDelete = Visibility.Collapsed;
        public Visibility IsVisibilityDelete
        {
            get { return _isVisibilityDelete; }
            set
            {
                this._isVisibilityDelete = value;
                NotifyPropertyChanged("IsVisibilityDelete");

            }
        }

        private bool _isDeleteEnable = false;
        public bool IsDeleteEnable
        {
            get { return _isDeleteEnable; }
            set
            {
                this._isDeleteEnable = value;
                NotifyPropertyChanged("IsDeleteEnable");

            }
        }

        private bool _isActive = false;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                this._isActive = value;
                NotifyPropertyChanged("IsActive");

            }
        }

        private bool _isInActive = false;
        public bool IsInActive
        {
            get { return _isInActive; }
            set
            {
                this._isInActive = value;
                NotifyPropertyChanged("IsInActive");

            }
        }

        private readonly ICommand selectChangeComboCommandName;
        public ICommand SelectChangeComboCommandName { get { return this.selectChangeComboCommandName; } }
        private void SelectDataRow()
        {
            if (this.SelectedRowPsw != null)
            {
                PSWTitleMaster.PSWName = this.SelectedRowPsw["NAME"].ToString();
                PSWTitleMaster.PSWTitle = this.SelectedRowPsw["TITLE"].ToString();
                if (this.SelectedRowPsw["STATUS"].ToString() == "YES")
                {
                    IsActive = true;
                    IsInActive = false;
                }
                else if (this.SelectedRowPsw["STATUS"].ToString() == "NO")
                {
                    IsActive = false;
                    IsInActive = true;
                }
                else
                {
                    IsActive = false;
                    IsInActive = false;
                }
            }
        }

        private void Add()
        {
            try
            {
                if (AddButtonIsEnable == false) return;
                if (PSWTitleMaster.PSWName.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                    }
                }
                PSWTitleMaster.PSWName = string.Empty;
                PSWTitleMaster.PSWTitle = string.Empty;
                PSWTitleMaster.PSWTitleMasterDetails = _pswTitleBll.GetPswNameList();
                _operationmode = OperationMode.Save;
                ButtonVisibleName = Visibility.Collapsed;
                AddButtonIsEnable = false;
                DeleteButtonIsEnable = false;
                EditButtonIsEnable = true;
                IsNameReadonly = false;
                SaveButtonIsEnable = true;
                IsVisibilityDelete = Visibility.Visible;
                IsActive = true;
                IsDeleteEnable = false;

                setRights();
                FocusButtonName = true;
                FocusButton = false;
                NotifyPropertyChanged("PSWTitleMaster");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private void Edit()
        {
            try
            {
                if (EditButtonIsEnable == false) return;
                if (PSWTitleMaster.PSWName.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                    }
                }
                PSWTitleMaster.PSWTitleMasterDetails = _pswTitleBll.GetPswNameList();
                PSWTitleMaster.PSWName = string.Empty;
                PSWTitleMaster.PSWTitle = string.Empty;
                AddButtonIsEnable = true;
                DeleteButtonIsEnable = true;
                EditButtonIsEnable = false;
                _operationmode = OperationMode.Update;
                ButtonVisibleName = Visibility.Visible;
                IsNameReadonly = true;
                IsVisibilityDelete = Visibility.Visible;
                setRights();
                FocusButtonName = false;
                FocusButton = true;
                NotifyPropertyChanged("PSWTitleMaster");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private void Save()
        {
            try
            {
                if (SaveButtonIsEnable == false) return;
                if (PSWTitleMaster.PSWName.Trim().Length <= 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Name"));
                    FocusButtonName = true;
                    FocusButton = false;
                    return;
                }

                //if (PSWTitleMaster.PSWTitle.Trim().Length <= 0)
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Title"));
                //    return;
                //}
                PSWTitleMaster.IsActive = (IsActive) ? false : true;
                string typ = "";
                //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                //Progress.Start();

                if (_pswTitleBll.SavePswMasterTitle(PSWTitleMaster, PSWTitleMaster.PSWName, PSWTitleMaster.PSWTitle, EditButtonIsEnable, ref typ))
                {
                    //Progress.End();
                    ShowInformationMessage(typ);
                    FocusButtonName = true;
                    //FocusButton = false;
                    //PSWTitleMaster.PSWTitle = string.Empty;
                    //PSWTitleMaster.PSWName = string.Empty;
                    PSWTitleMaster.PSWTitleMasterDetails = _pswTitleBll.GetPswNameList();
                    //_operationmode = OperationMode.Save;
                    //ButtonVisibleName = Visibility.Collapsed;
                    //AddButtonIsEnable = false;
                    //DeleteButtonIsEnable = false;
                    //EditButtonIsEnable = true;
                    //IsNameReadonly = false;
                    //IsVisibilityDelete = Visibility.Visible;
                    //IsActive = true;
                    //IsDeleteEnable = false;
                    //setRights();
                }
                else
                {
                    Progress.End();
                    ShowInformationMessage(typ);

                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private void Delete()
        {
            try
            {
                if (PSWTitleMaster.PSWName.Trim().Length <= 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Name"));
                    FocusButton = true;
                    return;
                }

                if (PSWTitleMaster.PSWTitle.Trim().Length <= 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Title"));
                    FocusButton = true;
                    return;
                }
                MessageBoxResult messageBoxResult = ShowWarningMessage(PDMsg.BeforeDelete("PSW Title"), MessageBoxButton.YesNo);
                //System.Windows.MessageBox.Show("Do you want to delete this PSW Title?", ApplicationTitle, System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (_pswTitleBll.DeletePswTitle(PSWTitleMaster.PSWName, PSWTitleMaster.PSWTitle))
                    {
                        ShowInformationMessage(PDMsg.DeletedSuccessfully);
                        FocusButton = true;
                        PSWTitleMaster.PSWName = string.Empty;
                        PSWTitleMaster.PSWTitle = string.Empty;
                        PSWTitleMaster.PSWTitleMasterDetails = _pswTitleBll.GetPswNameList();
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
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                {
                    CloseAction();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                WPF.MDI.ClosingEventArgs closingev;
                closingev = (WPF.MDI.ClosingEventArgs)e;
                if (PSWTitleMaster.PSWName.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        closingev.Cancel = true;
                        e = closingev;
                        return;
                    }
                }

                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                {
                    closingev.Cancel = true;
                    e = closingev;
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
        private ObservableCollection<DropdownColumns> _dropDownItems;
        public ObservableCollection<DropdownColumns> DropDownItems
        {
            get
            {
                return _dropDownItems;
            }
            set
            {
                this._dropDownItems = value;
                NotifyPropertyChanged("DropDownItems");
            }
        }
        private void SetdropDownItems()
        {
            try
            {
                DropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "NAME", ColumnDesc = "Name", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "TITLE", ColumnDesc = "Title", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "STATUS", ColumnDesc = "Active?", ColumnWidth = 70, ShowInDropdown = false }
                        };
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool _flag = false;
        public bool Flag
        {
            get { return _flag; }
            set
            {
                _flag = value;
                NotifyPropertyChanged("Flag");
            }
        }
    }
}
