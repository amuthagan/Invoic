using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Data;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;

namespace ProcessDesigner.Model
{
    class LocationMasterViewModel : ViewModelBase
    {
        private string _locCode = "";
        private string _locName = "";
        private string _errMessage;
        private UserInformation userinformation;
        private BLL.LocationMasterBll oLocMaster;
        private readonly ICommand selectChangeComboCommand;
        private readonly ICommand addClickCommand;
        private readonly ICommand editClickCommand;
        private readonly ICommand updateLocCommand;
        private readonly ICommand deleteClickCommand;
        private RelayCommand _onCloseCommand;




        public LocationMasterViewModel()
        {
            UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            oLocMaster = new LocationMasterBll(userinformation);
            DtDataview = oLocMaster.GetLocationMaster();
            this.selectChangeComboCommand = new DelegateCommand(this.SelectDataRow);
            this.addClickCommand = new DelegateCommand(this.AddSubmitCommand);
            this.editClickCommand = new DelegateCommand(this.EditSubmitCommand);
            this.updateLocCommand = new DelegateCommand(this.CommonFormValUpdtae);
            this.deleteClickCommand = new DelegateCommand(this.DeleteSubmitCommand);
            UserRoleObjName = "LOCATION MASTER";
            ComboBoxMaxLength = 2;
            ActionPermission = oLocMaster.GetUserRights(UserRoleObjName);
            SetdropDownItems();
            FormLoadRights();
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
        private bool isActiveSave = false;
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
        public void FormLoadRights()
        {
            if (ActionPermission.AddNew == true)
            {

                AddEnable = true;
                EditEnable = false;
                SaveEnable = true;
                ButtonEnable = Visibility.Collapsed;
                NextAction = "ADD";
                LocCode = "";
                LocName = "";

                IsVisibilityDelete = Visibility.Visible;
                IsActive = true;
                IsDeleteEnable = false;

                DeleteEnable = false;
                //  TxtDecReadOnly = false;
                TxtReadOnly = false;
                if (ActionPermission.Edit == true)
                {
                    EditEnable = true;
                    SaveEnable = true;
                    AddEnable = false;
                    DeleteEnable = false;

                    if (ActionPermission.AddNew == true)
                    {
                        ButtonEnable = Visibility.Collapsed;
                        NextAction = "ADD";
                    }
                    else
                    {
                        ButtonEnable = Visibility.Visible;
                        NextAction = "EDIT";
                        //  TxtDecReadOnly = true;
                        TxtReadOnly = true;
                        SetMaxLength();
                    }

                }
                else if (ActionPermission.Edit == false)
                {
                    EditEnable = false;
                    DeleteEnable = false;
                    SetdropDownItems();
                    if (ActionPermission.View == true && ActionPermission.AddNew == false && ActionPermission.Edit == false)
                    {
                        SaveEnable = false;
                        ButtonEnable = Visibility.Visible;
                        TxtDecReadOnly = true;
                        TxtReadOnly = true;
                    }
                }

            }
            else if (ActionPermission.AddNew == false)
            {
                // EditEnable = true;
                //SaveEnable = true;
                // AddEnable = false;
                // ButtonEnable = Visibility.Visible;
                LocCode = "";
                LocName = "";
                if (ActionPermission.Edit == true)
                {
                    IsActive = true;
                    IsDeleteEnable = true;
                    EditEnable = true;
                    SaveEnable = true;
                    AddEnable = false;
                    NextAction = "EDIT";
                    DeleteEnable = false;
                    //   TxtDecReadOnly = true;
                    TxtReadOnly = true;
                    ButtonEnable = Visibility.Visible;
                    SetMaxLength();
                }
                else if (ActionPermission.Edit == false)
                {
                    IsActive = true;
                    IsDeleteEnable = false;
                    EditEnable = false;
                    DeleteEnable = false;
                    if (ActionPermission.View == true && ActionPermission.AddNew == false && ActionPermission.Edit == false)
                    {
                        SaveEnable = false;
                        ButtonEnable = Visibility.Visible;
                        TxtDecReadOnly = true;
                        TxtReadOnly = true;
                    }
                }
            }

        }
        private int _combomaxlength;
        public int ComboBoxMaxLength
        {
            get
            {
                return _combomaxlength;
            }
            set
            {
                _combomaxlength = value;
                NotifyPropertyChanged("ComboBoxMaxLength");
            }
        }

        public void SetMaxLength()
        {
            ComboBoxMaxLength = 1;
        }
        public void SetUserRights(string buttoncaption)
        {

            if (buttoncaption == "EDIT")
            {
                if (ActionPermission.AddNew == true)
                {
                    AddEnable = true;
                    EditEnable = false;
                    DeleteEnable = true;
                    IsDeleteEnable = true;
                }
            }
            if (buttoncaption == "ADD")
            {
                if (ActionPermission.Edit == true)
                {
                    AddEnable = false;
                    EditEnable = true;
                    DeleteEnable = false;
                    IsDeleteEnable = false;
                }
            }
        }

        private bool _saveoperation = false;
        public bool SaveEnable
        {
            get { return _saveoperation; }
            set
            {
                _saveoperation = value;
                NotifyPropertyChanged("SaveEnable");
            }
        }

        private OperationMode _actionMode = OperationMode.None;
        public OperationMode ActionMode
        {
            get { return _actionMode; }
            set
            {
                _actionMode = value;
                NotifyPropertyChanged("ActionMode");
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Location Code is Required")]
        public string LocCode
        {
            get
            {
                return this._locCode;
            }
            set
            {
                this._locCode = value;
                NotifyPropertyChanged("LocCode");
            }
        }

        private RolePermission _actionPermission;
        public RolePermission ActionPermission
        {
            get { return _actionPermission; }
            set
            {
                _actionPermission = value;
                NotifyPropertyChanged("ActionPermission");
            }
        }

        private string _userroleobjname;
        public string UserRoleObjName
        {
            get { return this._userroleobjname; }
            set
            {
                this._userroleobjname = value;
                NotifyPropertyChanged("UserRoleObjName");
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Location Name is Required")]
        public string LocName
        {
            get
            {
                return this._locName;
            }
            set
            {
                this._locName = value;
                NotifyPropertyChanged("LocName");
            }
        }

        private DataView _dtDataView;
        public DataView DtDataview
        {
            get
            {
                return this._dtDataView;
            }
            set
            {
                this._dtDataView = value;
                NotifyPropertyChanged("DtDataview");
            }
        }
        string _theSelectedItem = null;
        public string TheSelectedItem
        {
            get { return _theSelectedItem; }
            set { _theSelectedItem = value; } // NotifyPropertyChanged
        }
        private DataRowView _selectedrow;
        public DataRowView SelectedRow
        {
            get
            {
                return _selectedrow;
            }

            set
            {
                _selectedrow = value;
            }
        }
        public ICommand SelectChangeComboCommand { get { return this.selectChangeComboCommand; } }
        private void SelectDataRow()
        {
            if (SelectedRow != null)
            {
                LocCode = (string)(SelectedRow["LOC_CODE"].ToString());
                LocName = (string)SelectedRow["LOCATION"].ToString();
                if (this.SelectedRow["STATUS"].ToString() == "YES")
                {
                    IsActive = true;
                    IsInActive = false;
                }
                else if (this.SelectedRow["STATUS"].ToString() == "NO")
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

        public ICommand AddClickCommand { get { return this.addClickCommand; } }
        private void AddSubmitCommand()
        {
            if (LocCode.IsNotNullOrEmpty() || LocName.IsNotNullOrEmpty())
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    CommonFormValUpdtae();
                    return;
                }
                else
                {
                    NextAction = "ADD";
                    SetUserRights("ADD");
                    // EditEnable = true;
                    //AddEnable = false;
                    LocCode = string.Empty;
                    LocName = string.Empty;
                    DeleteEnable = false;
                    TxtReadOnly = false;
                    IsVisibilityDelete = Visibility.Visible;
                    IsActive = true;
                    IsDeleteEnable = false;

                    ButtonEnable = Visibility.Collapsed;
                    FocusButton = true;
                }
            }
            else
            {
                NextAction = "ADD";
                SetUserRights("ADD");
                // EditEnable = true;
                //AddEnable = false;
                LocCode = string.Empty;
                LocName = string.Empty;
                DeleteEnable = false;
                TxtReadOnly = false;
                IsVisibilityDelete = Visibility.Visible;
                IsActive = true;
                IsDeleteEnable = false;

                ButtonEnable = Visibility.Collapsed;
                FocusButton = true;
            }
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

        private bool _txtdecreadonly = false;
        public bool TxtDecReadOnly
        {
            get { return _txtdecreadonly; }
            set
            {
                _txtdecreadonly = value;
                NotifyPropertyChanged("TxtDecReadOnly");
            }
        }

        private bool _txtreadonly = false;
        public bool TxtReadOnly
        {
            get { return _txtreadonly; }
            set
            {
                _txtreadonly = value;
                NotifyPropertyChanged("TxtReadOnly");
            }
        }

        private string _nextAction = "ADD";
        public string NextAction
        {
            get { return _nextAction; }
            set
            {
                _nextAction = value;
                NotifyPropertyChanged("NextAction");
            }

        }
        private bool _editOpertion = true;
        public bool EditEnable
        {
            get { return _editOpertion; }
            set
            {
                _editOpertion = value;
                NotifyPropertyChanged("EditEnable");
            }
        }
        private bool _addOperation = false;
        public bool AddEnable
        {
            get { return _addOperation; }
            set
            {
                _addOperation = value;
                NotifyPropertyChanged("AddEnable");
            }
        }
        private bool _deleteEnable = false;
        public bool DeleteEnable
        {
            get { return _deleteEnable; }
            set
            {
                _deleteEnable = value;
                NotifyPropertyChanged("DeleteEnable");
            }
        }

        private Visibility _buttonVisible = Visibility.Collapsed;
        public Visibility ButtonEnable
        {
            get { return _buttonVisible; }
            set
            {
                _buttonVisible = value;
                NotifyPropertyChanged("ButtonEnable");
            }
        }
        public ICommand EditClickCommand { get { return this.editClickCommand; } }
        private void EditSubmitCommand()
        {
            if (LocCode.IsNotNullOrEmpty() || LocName.IsNotNullOrEmpty())
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    CommonFormValUpdtae();
                    return;
                }
                else
                {
                    NextAction = "EDIT";
                    SetUserRights("EDIT");
                    //  EditEnable = false;
                    // DeleteEnable = true;
                    //  AddEnable = true;
                    LocCode = string.Empty;
                    LocName = string.Empty;
                    TxtReadOnly = true;
                    FocusButton = true;
                    ButtonEnable = Visibility.Visible;
                    IsVisibilityDelete = Visibility.Visible;
                }
            }
            else
            {
                NextAction = "EDIT";
                SetUserRights("EDIT");
                //  EditEnable = false;
                // DeleteEnable = true;
                //  AddEnable = true;
                LocCode = string.Empty;
                LocName = string.Empty;
                TxtReadOnly = true;
                FocusButton = true;
                ButtonEnable = Visibility.Visible;
                IsVisibilityDelete = Visibility.Visible;
            }
        }

        private void ClearOperMaster()
        {
            LocCode = string.Empty;
            LocName = string.Empty;
            DtDataview = oLocMaster.GetLocationMaster();
            FocusCombo = true;
            FormLoadRights();
        }

        public ICommand UpdateLocCommand { get { return this.updateLocCommand; } }
        public void CommonFormValUpdtae()
        {
            try
            {
                isActiveSave = (IsActive) ? false : true;
                if (ButtonEnable == Visibility.Visible)
                {
                    NextAction = "EDIT";
                }
                else if (ButtonEnable == Visibility.Collapsed)
                {
                    NextAction = "ADD";
                }

                //    NextAction = "EDIT";

                if (String.IsNullOrEmpty(Convert.ToString(LocCode)) || LocCode.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Location Code"));
                    //FocusButton = true;
                    Flag = true;
                    //System.Windows.MessageBox.Show("Location Code should be Entered", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (String.IsNullOrEmpty(LocName) || LocName.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Location Name"));
                    // FocusButton = true;
                    Flag = true;
                    //System.Windows.MessageBox.Show("Location Name should be Entered", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Flag = false;
                    //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                    //Progress.Start();
                    bool val = oLocMaster.AddNewLocationMaster(isActiveSave, LocCode, LocName, NextAction, ref _errMessage);
                    //Progress.End();
                    if (val)
                    {
                        ShowInformationMessage(_errMessage);
                        FocusButton = true;
                        //System.Windows.MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        //MessageBox.Show(_errMessage);
                        ClearOperMaster();
                    }
                    if (val == false)
                    {
                        if ((string)_errMessage != "")
                            ShowInformationMessage(_errMessage);
                        FocusButton = true;
                        //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }

                DtDataview = oLocMaster.GetLocationMaster();
                // FocusButton = true;
                if (Flag == true)
                {
                    if (AddEnable == true)
                    {
                        NextAction = "EDIT";
                        SetUserRights("EDIT");
                        ButtonEnable = Visibility.Visible;
                    }
                    else if (EditEnable == true)
                    {
                        NextAction = "ADD";
                        SetUserRights("ADD");
                        ButtonEnable = Visibility.Collapsed;
                    }
                }
                TxtReadOnly = (NextAction == "EDIT") ? true : false;
                //   LoadTableData();
            }
            catch (Exception ex)
            {
                Progress.End();
                throw ex.LogException();
            }
        }

        public ICommand DeleteClickCommand { get { return this.deleteClickCommand; } }
        private void DeleteSubmitCommand()
        {
            try
            {
                NextAction = "DELETE";

                if (String.IsNullOrEmpty(Convert.ToString(LocCode)))
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Location Code"));
                    FocusButton = true;
                    //MessageBox.Show("Location Code should be Entered", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBoxResult messageBoxResult = ShowWarningMessage(PDMsg.BeforeDelete("Location code"), MessageBoxButton.YesNo);
                    FocusButton = true;
                    //System.Windows.MessageBox.Show("Do you want to delete this Location code?", ApplicationTitle, System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        bool val = oLocMaster.DeleteLocationCode(LocCode, NextAction, ref _errMessage);
                        if (val)
                        {
                            ShowInformationMessage(_errMessage);
                            FocusButton = true;
                            //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearOperMaster();
                        }
                        if (val == false)
                        {
                            if ((string)_errMessage != "")
                                ShowInformationMessage(_errMessage);
                            FocusButton = true;
                            //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
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
                           new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Loc. Code", ColumnWidth = 85 },
                           new DropdownColumns() { ColumnName = "LOCATION", ColumnDesc = "Loc. Name", ColumnWidth = "1*" },
                           new DropdownColumns() { ColumnName = "STATUS", ColumnDesc = "Active?", ColumnWidth = 70, ShowInDropdown = false }
                        };
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
                //if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //{
                //    CommonFormValUpdtae();
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
                //if (LocCode.IsNotNullOrEmpty() || LocName.IsNotNullOrEmpty())
                //{
                //    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //    {
                //        CommonFormValUpdtae();
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

        public Action CloseAction { get; set; }
        public ICommand OnCloseCommand
        {
            get
            {
                if (_onCloseCommand == null)
                {
                    _onCloseCommand = new RelayCommand(param => this.Cancel(), null);
                }
                return _onCloseCommand;
            }
        }

        private bool _focusCombo = false;
        public bool FocusCombo
        {
            get { return _focusCombo; }
            set
            {
                _focusCombo = value;
                NotifyPropertyChanged("FocusCombo");
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
