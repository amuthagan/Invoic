using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProcessDesigner.Common;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.ViewModel
{
    public class ApplicationViewModel : ViewModelBase 
    {
        private ApplicationBll applicationBll;
        private readonly ICommand _onAddCommand;
        private OperationMode _operationmode;
        public ICommand OnAddCommand { get { return this._onAddCommand; } }

        private readonly ICommand _onEditViewCommand;
        public ICommand OnEditViewCommand { get { return this._onEditViewCommand; } }
        private readonly ICommand _onSaveCommand;
        public ICommand OnSaveCommand { get { return this._onSaveCommand; } }
        private readonly ICommand _onCloseCommand;
        public ICommand OnCloseCommand { get { return this._onCloseCommand; } }
        private readonly ICommand _onDeleteCommand;
        public ICommand OnDeleteCommand { get { return this._onDeleteCommand; } }

        private bool _addButtonIsEnable = false;
        private bool _editButtonIsEnable = true;
        private bool _saveButtonIsEnable = true;
        private bool _deleteButtonIsEnable = false;
        public ApplicationViewModel(UserInformation userInfo)
        {
            this._onAddCommand = new DelegateCommand(this.Add);
            this._onEditViewCommand = new DelegateCommand(this.Edit);
            this._onSaveCommand = new DelegateCommand(this.Save);
            this._onCloseCommand = new DelegateCommand(this.Close);
            this._onDeleteCommand = new DelegateCommand(this.Delete);
            this.selectChangeComboCommandName = new DelegateCommand(this.SelectDataRow);
            ApplicationModel = new ApplicationModel();
            applicationBll = new ApplicationBll(userInfo);
            GetRights();
            AddButtonIsEnable = true;
            FocusButton = true;
            ApplicationFocus = true;
            Add();
            SetdropDownItems();
            setRights();
        }

        private ApplicationModel _application;
        public ApplicationModel ApplicationModel
        {
            get
            {
                return _application;
            }
            set
            {
                _application = value;
                NotifyPropertyChanged("ApplicationModel");
            }
        }

        private void Add()
        {
            try
            {
                //if (AddButtonIsEnable == false) return;
                ApplicationFocus = true;
                ApplicationModel.SNo = applicationBll.GetSerialNo(ApplicationModel);
                if (ApplicationModel.PSWApplication.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                    }
                }
                ApplicationModel.PSWApplication = string.Empty;
                ApplicationModel.PSWApplicationView = applicationBll.GetApplication();
                NotifyPropertyChanged("ApplicationModel");
                _operationmode = OperationMode.Save;
                ButtonVisibleName = Visibility.Collapsed;
                AddButtonIsEnable = false;
                DeleteButtonIsEnable = false;
                EditButtonIsEnable = true;
                IsNameReadonly = false;
                SaveButtonIsEnable = true;
                IsVisibilityDelete = Visibility.Visible;
                Active = true;
                IsDeleteEnable = false;
                setRights();
                FocusButton = true;
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
        }

        private void Edit()
        {
            try
            {
                //if (EditButtonIsEnable == false) return;
                ApplicationFocus = true;
                if (ApplicationModel.PSWApplication.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                    }

                }
                ApplicationModel.PSWApplicationView = applicationBll.GetApplication();
                ApplicationModel.PSWApplication = string.Empty;
                AddButtonIsEnable = true;
                DeleteButtonIsEnable = true;
                EditButtonIsEnable = false;
                _operationmode = OperationMode.Update;
                ButtonVisibleName = Visibility.Visible;
                IsNameReadonly = true;
                IsVisibilityDelete = Visibility.Visible;
                setRights();
                FocusButton = true;
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
        }

        private void Save()
        {
            try
            {
                if (SaveButtonIsEnable == false) return;
                if (!ApplicationModel.PSWApplication.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Application"));
                    FocusButton = true;
                    return;
                }
                else if (!applicationBll.ApplicationAddDuplicate(ApplicationModel))
                {
                    ShowInformationMessage(PDMsg.AlreadyExists("Application"));
                    FocusButton = true;
                    return;
                }
                //else if (!applicationBll.ApplicationEditDuplicate(ApplicationModel) && EditButtonIsEnable == false)
                //{
                //    ShowInformationMessage(PDMsg.AlreadyExists("Application"));
                //    FocusButton = true;
                //    return;
                //}

                string mode = "";
                ApplicationModel.Active = (Active) ? false : true;
                //Progress.Start();
                if (applicationBll.AddEditApplication(ApplicationModel, ref mode))
                {
                    if (mode == "Add")
                    {
                        //Progress.End();
                        ShowInformationMessage(PDMsg.SavedSuccessfully);
                        FocusButton = true;
                    }
                    else
                    {
                        Progress.End();
                        ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                        FocusButton = true;
                    }
                    ApplicationModel.PSWApplication = string.Empty;
                    ApplicationFocus = true;
                    Add();
                }
            }
            catch (Exception e)
            {
                throw e.LogException();
            }
        }

        private readonly ICommand selectChangeComboCommandName;
        public ICommand SelectChangeComboCommandName { get { return this.selectChangeComboCommandName; } }
        private void SelectDataRow()
        {
            if (SelectedRowPsw != null)
            {
                ApplicationModel.PSWApplication = SelectedRowPsw["NewApplication"].ToString();
                ApplicationModel.SNo = SelectedRowPsw["S_"].ToString().ToIntValue();
                if (SelectedRowPsw["STATUS"].ToString() == "YES")
                {
                    Active = true;
                    InActive = false;
                }
                else if (SelectedRowPsw["STATUS"].ToString() == "NO")
                {
                    Active = false;
                    InActive = true;
                }
                else
                {
                    Active = false;
                    InActive = false;
                }
            }
        }

        private bool _isActive = false;
        public bool Active
        {
            get { return _isActive; }
            set
            {
                this._isActive = value;
                NotifyPropertyChanged("Active");

            }
        }

        private bool _inActive = false;
        public bool InActive
        {
            get { return _inActive; }
            set
            {
                this._inActive = value;
                NotifyPropertyChanged("InActive");

            }
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return System.Windows.MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        public Action CloseAction { get; set; }

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
            ActionPermission = applicationBll.GetUserRights("PSW APPLICATION");
            if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
            {
                ActionPermission.Save = false;
            }
        }

        private void setRights()
        {
            if (AddButtonIsEnable) AddButtonIsEnable = ActionPermission.AddNew;
            if (EditButtonIsEnable)
            {
                Active = true;
                IsDeleteEnable = false;
                EditButtonIsEnable = ActionPermission.Edit;
            }
            else
            {
                Active = true;
                IsDeleteEnable = true;
            }
            if (DeleteButtonIsEnable)
            {
                DeleteButtonIsEnable = ActionPermission.Delete;
                 IsDeleteEnable = true;

            }
            if (SaveButtonIsEnable) SaveButtonIsEnable = ActionPermission.Save;
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
                            new DropdownColumns() { ColumnName = "NewApplication", ColumnDesc = "Application", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "STATUS", ColumnDesc = "Active?", ColumnWidth = 70, ShowInDropdown = false }
                        };
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
                if (!ApplicationModel.PSWApplication.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Application"));
                    FocusButton = true;
                    return;
                }
                MessageBoxResult messageBoxResult = ShowWarningMessage(PDMsg.BeforeDelete("Application"), MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    //Progress.Start();
                    if (applicationBll.DeletePswApplication(ApplicationModel.PSWApplication))
                    {
                        //Progress.End();
                        ShowInformationMessage(PDMsg.DeletedSuccessfully);
                        FocusButton = true;
                        ApplicationModel.PSWApplication = string.Empty;
                        ApplicationModel.PSWApplicationView = applicationBll.GetApplication();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private MessageBoxResult ShowWarningMessage(string _showMessage, MessageBoxButton messageBoxButton)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, messageBoxButton, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void Close()
        {
            try
            {
                //if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //{
                //    Save();
                //    return;
                //}

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
                //if (ApplicationModel.PSWApplication.IsNotNullOrEmpty())
                //{
                //    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //    {
                //        Save();
                //        closingev.Cancel = true;
                //        e = closingev;
                //        return;
                //    }
                //}

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

        public bool DeleteButtonIsEnable
        {
            get { return _deleteButtonIsEnable; }
            set
            {
                this._deleteButtonIsEnable = value;
                NotifyPropertyChanged("DeleteButtonIsEnable");
            }
        }

        private bool _applicationFocus = false;
        public bool ApplicationFocus
        {
            get { return _applicationFocus; }
            set
            {
                _applicationFocus = value;
                NotifyPropertyChanged("ApplicationFocus");
            }
        }
    }
}
