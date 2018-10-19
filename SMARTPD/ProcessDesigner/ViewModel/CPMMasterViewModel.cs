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
using ProcessDesigner.UserControls;
using System.Collections.ObjectModel;

namespace ProcessDesigner.ViewModel
{
    public class CPMMasterViewModel : ViewModelBase
    {
        public CPMMasterViewModel(UserInformation userInformation)
        {
            this.CPMMaster = new CPMMasterModel();
            this.selectChangeComboCommandDept = new DelegateCommand(this.SelectDataRowDept);
            this.selectChangeComboCommandMem = new DelegateCommand(this.SelectDataRowMem);
            _cpmTitleBll = new CPMMasterBll(userInformation);
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
        private CPMMasterBll _cpmTitleBll;
        private CPMMasterModel _cpmTitleModel;
        private readonly ICommand _onAddCommand;
        private OperationMode _operationmode;
        private bool _addButtonIsEnable = false;
        private bool _editButtonIsEnable = true;
        private bool _saveButtonIsEnable = true;
        private bool _deleteButtonIsEnable = false;
        public ICommand OnAddCommand { get { return this._onAddCommand; } }
        private readonly ICommand _onEditViewCommand;
        public ICommand OnEditViewCommand { get { return this._onEditViewCommand; } }

        private readonly ICommand _onSaveCommand;
        public ICommand OnSaveCommand { get { return this._onSaveCommand; } }

        private ICommand _onCloseCommand;

        private readonly ICommand _onDeleteCommand;
        public ICommand OnDeleteCommand { get { return this._onDeleteCommand; } }
        public CPMMasterModel CPMMaster
        {
            get
            {
                return _cpmTitleModel;
            }
            set
            {
                this._cpmTitleModel = value;
                NotifyPropertyChanged("CPMMaster");
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
        private bool _isCpmReadOnly = false;
        public bool IsCpmReadOnly
        {
            get { return _isCpmReadOnly; }
            set
            {
                this._isCpmReadOnly = value;
                NotifyPropertyChanged("IsCpmReadOnly");
            }
        }
        private DataRowView _selectedrowdept;
        public DataRowView SelectedRowDept
        {
            get
            {
                return _selectedrowdept;
            }

            set
            {
                _selectedrowdept = value;
            }
        }
        private DataRowView _selectedrowmem;
        public DataRowView SelectedRowMem
        {
            get
            {
                return _selectedrowmem;
            }

            set
            {
                this._selectedrowmem = value;
                NotifyPropertyChanged("SelectedRowMem");
            }
        }
        private Visibility _buttonVisible = Visibility.Visible;
        public Visibility ButtonVisibleDept
        {
            get { return _buttonVisible; }
            set
            {
                this._buttonVisible = value;
                NotifyPropertyChanged("ButtonVisibleDept");
            }
        }
        private Visibility _buttonVisibleMember = Visibility.Visible;
        public Visibility ButtonVisibleMember
        {
            get { return _buttonVisibleMember; }
            set
            {
                this._buttonVisibleMember = value;
                NotifyPropertyChanged("ButtonVisibleMember");
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
        private void GetRights()
        {
            ActionPermission = new RolePermission();
            ActionPermission.Save = true;
            ActionPermission.Print = false;
            ActionPermission.View = true;
            ActionPermission.AddNew = false;
            ActionPermission.Delete = false;
            ActionPermission.Edit = false;
            ActionPermission = _cpmTitleBll.GetUserRights("CPM MASTER");
            if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
            {
                ActionPermission.Save = false;
            }
        }

        private void setRights()
        {
            if (AddButtonIsEnable) AddButtonIsEnable = ActionPermission.AddNew;
            //if (EditButtonIsEnable) EditButtonIsEnable = ActionPermission.Edit;
            if (EditButtonIsEnable)
            {
                IsActive = true;
                IsDeleteEnable = false;
                EditButtonIsEnable = ActionPermission.Edit;
            }
            else
            {
                IsActive = true;
                IsDeleteEnable = true;
            }
            if (DeleteButtonIsEnable)
            {
                DeleteButtonIsEnable = ActionPermission.Delete;
                IsDeleteEnable = true;

            }
            if (SaveButtonIsEnable) SaveButtonIsEnable = ActionPermission.Save;
        }

        private readonly ICommand selectChangeComboCommandDept;
        public ICommand SelectChangeComboCommandDept { get { return this.selectChangeComboCommandDept; } }
        private void SelectDataRowDept()
        {
            if (CPMMaster.DEPT.IsNotNullOrEmpty())
            {
                CPMMaster.CPMMemberMasterDetails = _cpmTitleBll.GetCPMMMemberList(CPMMaster.DEPT);
                CPMMaster.SNO = _cpmTitleBll.GenerateSno(CPMMaster.DEPT);
                CPMMaster.MEMBER = "";
            }
        }
        private readonly ICommand selectChangeComboCommandMem;
        public ICommand SelectChangeComboCommandMem { get { return this.selectChangeComboCommandMem; } }
        private void SelectDataRowMem()
        {
            if (this.SelectedRowMem.IsNotNullOrEmpty())
            {
                CPMMaster.MEMBER = this.SelectedRowMem["MEMBER"].ToString();
                CPMMaster.SNO = Convert.ToDecimal(this.SelectedRowMem["SNO"].ToString());
                if (this.SelectedRowMem["STATUS"].ToString() == "YES")
                {
                    IsActive = true;
                    IsInActive = false;
                }
                else if (this.SelectedRowMem["STATUS"].ToString() == "NO")
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
                if (CPMMaster.DEPT.IsNotNullOrEmpty() || CPMMaster.MEMBER.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                    }
                    else
                    {
                        CPMMaster.DEPT = "";
                        CPMMaster.MEMBER = "";
                        CPMMaster.CPMDeptMasterDetails = _cpmTitleBll.GetCPMMasterDeptList();
                        _operationmode = OperationMode.Save;
                        ButtonVisibleMember = Visibility.Collapsed;
                        AddButtonIsEnable = false;
                        DeleteButtonIsEnable = false;
                        EditButtonIsEnable = true;
                        IsCpmReadOnly = false;

                        IsVisibilityDelete = Visibility.Visible;
                        IsActive = true;
                        IsDeleteEnable = false;

                        setRights();
                        if (CPMMaster.CPMDeptMasterDetails.Count > 0)
                        {
                            CPMMaster.DEPT = CPMMaster.CPMDeptMasterDetails[0][0].ToString();
                            CPMMaster.SNO = _cpmTitleBll.GenerateSno(CPMMaster.DEPT);
                        }
                    }
                }
                else
                {
                    CPMMaster.DEPT = "";
                    CPMMaster.MEMBER = "";
                    CPMMaster.CPMDeptMasterDetails = _cpmTitleBll.GetCPMMasterDeptList();
                    _operationmode = OperationMode.Save;
                    ButtonVisibleMember = Visibility.Collapsed;
                    AddButtonIsEnable = false;
                    DeleteButtonIsEnable = false;
                    EditButtonIsEnable = true;
                    IsCpmReadOnly = false;

                    IsVisibilityDelete = Visibility.Visible;
                    IsActive = true;
                    IsDeleteEnable = false;

                    setRights();
                    if (CPMMaster.CPMDeptMasterDetails.Count > 0)
                    {
                        CPMMaster.DEPT = CPMMaster.CPMDeptMasterDetails[0][0].ToString();
                        CPMMaster.SNO = _cpmTitleBll.GenerateSno(CPMMaster.DEPT);
                    }
                }
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
                if (CPMMaster.MEMBER.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                    }
                    else
                    {
                        CPMMaster.CPMMemberMasterDetails = _cpmTitleBll.GetCPMMMemberList(CPMMaster.DEPT);
                        _operationmode = OperationMode.Update;
                        ButtonVisibleMember = Visibility.Visible;
                        CPMMaster.DEPT = "";
                        CPMMaster.MEMBER = "";
                        AddButtonIsEnable = true;
                        DeleteButtonIsEnable = true;
                        EditButtonIsEnable = false;

                        IsVisibilityDelete = Visibility.Visible;

                        // IsCpmReadOnly = true;
                        setRights();
                    }
                }
                else
                {
                    CPMMaster.CPMMemberMasterDetails = _cpmTitleBll.GetCPMMMemberList(CPMMaster.DEPT);
                    _operationmode = OperationMode.Update;
                    ButtonVisibleMember = Visibility.Visible;
                    CPMMaster.DEPT = "";
                    CPMMaster.MEMBER = "";
                    AddButtonIsEnable = true;
                    DeleteButtonIsEnable = true;
                    EditButtonIsEnable = false;

                    IsVisibilityDelete = Visibility.Visible;

                    // IsCpmReadOnly = true;
                    setRights();
                }
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
                if (CPMMaster.DEPT.Trim().Length <= 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Department"));
                    return;
                }
                if (CPMMaster.MEMBER.Trim().Length <= 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Member"));
                    Flag = true;
                    return;
                }
                string typ = "";
                CPMMaster.IsActive = (IsActive) ? false : true;
                //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                //Progress.Start();
                if (_cpmTitleBll.SaveCpmMaster(CPMMaster, CPMMaster.DEPT, CPMMaster.MEMBER, CPMMaster.SNO, ref typ))
                {
                    //Progress.End();
                    if (typ == "UPD")
                        ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                    else
                        ShowInformationMessage(PDMsg.SavedSuccessfully);
                    //CPMMaster.CPMMemberMasterDetails = _cpmTitleBll.GetCPMMMemberList(CPMMaster.DEPT);
                    //AddButtonIsEnable = false;
                    //DeleteButtonIsEnable = false;
                    //EditButtonIsEnable = true;
                    CPMMaster.MEMBER = "";
                    CPMMaster.DEPT = "";
                    CPMMaster.CPMDeptMasterDetails = _cpmTitleBll.GetCPMMasterDeptList();
                    _operationmode = OperationMode.Save;
                    ButtonVisibleMember = Visibility.Collapsed;
                    AddButtonIsEnable = false;
                    DeleteButtonIsEnable = false;
                    EditButtonIsEnable = true;
                    IsCpmReadOnly = false;
                    IsVisibilityDelete = Visibility.Visible;
                    IsActive = true;
                    IsDeleteEnable = false;
                    setRights();
                }
                else
                {
                    Progress.End();
                }
                if (Flag == true)
                {
                    if (AddButtonIsEnable == true)
                    {
                        AddButtonIsEnable = true;
                        DeleteButtonIsEnable = true;
                        EditButtonIsEnable = false;
                    }
                }
            }
            catch (Exception ex)
            {
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
        private void Delete()
        {
            try
            {
                if (CPMMaster.MEMBER.Trim().Length <= 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Member"));
                    return;
                }

                if (CPMMaster.DEPT.Trim().Length <= 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Department"));
                    return;
                }
                MessageBoxResult messageBoxResult = ShowWarningMessage(PDMsg.BeforeDelete("Member"), MessageBoxButton.YesNo);
                //System.Windows.MessageBox.Show("Do you want to delete this Member?", ApplicationTitle, System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (_cpmTitleBll.DeleteCpmTitle(CPMMaster.DEPT, CPMMaster.MEMBER))
                    {
                        ShowInformationMessage(PDMsg.DeletedSuccessfully);
                        CPMMaster.DEPT = "";
                        CPMMaster.MEMBER = "";
                        CPMMaster.CPMDeptMasterDetails = _cpmTitleBll.GetCPMMasterDeptList();
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
                //if (CPMMaster.DEPT.IsNotNullOrEmpty() || CPMMaster.MEMBER.IsNotNullOrEmpty())
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
                            new DropdownColumns() { ColumnName = "SNOT", ColumnDesc = "SNo", ColumnWidth = 55 },
                            new DropdownColumns() { ColumnName = "MEMBER", ColumnDesc = "Member", ColumnWidth = "70*" },
                            new DropdownColumns() { ColumnName = "STATUS", ColumnDesc = "Active?", ColumnWidth = 70, ShowInDropdown = false }
                        };
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        #region Close Button Action
        public Action CloseAction { get; set; }
        private void CloseSubmitCommand()
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

        public ICommand OnCloseCommand
        {
            get
            {
                if (_onCloseCommand == null)
                {
                    _onCloseCommand = new RelayCommand(param => this.Close(), null);
                }
                return _onCloseCommand;
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
        #endregion

    }
}
