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
using System.Windows.Input;
using System.Data;
using System.Windows;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using ProcessDesigner.Model;
using System.ComponentModel.DataAnnotations;
using System.Windows.Controls;



namespace ProcessDesigner.ViewModel
{
    public class OperationMasterViewModel : ViewModelBase
    {
        // private decimal _operCode;
        //private  System.Nullable<decimal> _operCode;
        private string _operCode = "";
        private string _operDesc = "";
        private string _showInCost = "1";
        private string _message;
        //      private string OperErrorMessageDesc;
        //     private string OperErrorMessage;

        private UserInformation userinformation;
        private BLL.OperationMasterBll operModel;
        private BLL.FinishMasterBll ofinishModel;
        private BLL.UnitMasterBll oUnitMaster;
        private BLL.Coatingmaster oCoatingMaster;
        private BLL.CustomerMaset oCustMaster;


        private readonly ICommand updateOperMastCommand;
        private readonly ICommand selectChangeComboCommand;
        private readonly ICommand addClickCommand;
        private readonly ICommand editClickCommand;
        private readonly ICommand deleteClickCommand;
        private readonly ICommand loadCommand;
        private readonly ICommand _onCancelCommand;
        private readonly ICommand alertCommand;

        //   private readonly ICommand keyPressEvent;



        public OperationMasterViewModel(string formName)
        {
            UserInformation user = (UserInformation)App.Current.Properties["userinfo"];
            userinformation = user;
            FormName = formName;
            operModel = new OperationMasterBll(userinformation);
            ofinishModel = new FinishMasterBll(userinformation);
            oUnitMaster = new UnitMasterBll(userinformation);
            oCoatingMaster = new Coatingmaster(userinformation);
            oCustMaster = new CustomerMaset(userinformation);
            LoadTableData();



            this.updateOperMastCommand = new DelegateCommand(this.CommonFormValUpdtae);
            this.selectChangeComboCommand = new DelegateCommand(this.SelectDataRow);
            this.addClickCommand = new DelegateCommand(this.AddSubmitCommand);
            this.editClickCommand = new DelegateCommand(this.EditSubmitCommand);
            this.deleteClickCommand = new DelegateCommand(this.DeleteSubmitCommand);
            this.loadCommand = new DelegateCommand(this.LoadedCommand);
            this.alertCommand = new DelegateCommand(this.KeyPressalertCommand);
            this._onCancelCommand = new DelegateCommand(this.Cancel);
        }

        private void TextBox_TextChanged(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (FormName == "OPERMASTER")
            {
                TextBox _this = (sender as TextBox);
                if (e.Text == "0")
                {
                    if (_this.Text.ToString().Length == 0 || _this.SelectionStart == 0)
                    {
                        e.Handled = true;
                        return;
                    }
                }
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

        private MessageBoxResult ShowQuestionMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OKCancel, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        //private string _ErrorMessage;
        //public string OperErrorMessage
        //{
        //    get
        //    {
        //        return _ErrorMessage;
        //    }
        //    set
        //    {
        //        _errMessage = value;
        //        NotifyPropertyChanged("OperErrorMessage");
        //    }
        //}

        //private string _ErrorMessagedesc;
        //public string OperErrorMessageDesc
        //{
        //    get
        //    {
        //        return _ErrorMessagedesc;
        //    }
        //    set
        //    {
        //        _ErrorMessagedesc = value;
        //        NotifyPropertyChanged("OperErrorMessageDesc");
        //    }
        //}

        private Visibility _isVisibilityDelete = Visibility.Visible;
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

        public void SetMaxLength()
        {
            ComboBoxMaxLength = 1;
        }
        public void LoadFormData()
        {

            switch (FormName)
            {
                case "OPERMASTER":
                    // this.OperMaster = operModel.GetOpertionMaster();
                    LabelCode = "Operation Code :";
                    LabelDesc = "Operation Desc :";

                    LabelCodeStatusMsg = "Please enter/select Operation Code";
                    LabelDescStatusMsg = "Please enter Operation Desc";
                    CostVisible = Visibility.Visible;
                    ComboBoxMaxLength = 10;
                    DescTextboxMaxLength = 50;
                    UserRoleObjName = "OPERATION MASTER";
                    //    OperErrorMessage.Append ("Operation Code is Required");
                    //   OperErrorMessageDesc.Append ("Operation Desc is Required");
                    ActionPermission = operModel.GetUserRights(UserRoleObjName);
                    FormLoadRights();
                    Columns = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "OPER_CODE", ColumnDesc = "Opr. Code", ColumnWidth = 85 },
                            new DropdownColumns() { ColumnName = "OPER_DESC", ColumnDesc = "Opr. Desc", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "STATUS", ColumnDesc = "Active?", ColumnWidth = 70, ShowInDropdown = false }
                        };
                    break;
                case "FINISHMASTER":
                    LabelCode = "Finish Code :";
                    LabelDesc = "Finish Desc :";
                    LabelCodeStatusMsg = "Please enter/select Finish Code";
                    LabelDescStatusMsg = "Please enter Finish Desc";
                    ComboBoxMaxLength = 3;
                    DescTextboxMaxLength = 74;
                    UserRoleObjName = "FINISH MASTER";

                    //     OperErrorMessage = "Finish Code is Required";
                    //     OperErrorMessageDesc = "Finish Desc is Required";

                    ActionPermission = ofinishModel.GetUserRights(UserRoleObjName);
                    FormLoadRights();
                    Columns = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "OPER_CODE", ColumnDesc = "Fin. Code", ColumnWidth = 85 },
                            new DropdownColumns() { ColumnName = "OPER_DESC", ColumnDesc = "Fin. Desc", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "STATUS", ColumnDesc = "Active?", ColumnWidth = 70, ShowInDropdown = false }
                            //new DropdownColumns() { ColumnName = "OPER_CODE_SORT", ColumnDesc = "Finish Code", ColumnWidth = 0, ColumnVisibility = Visibility.Collapsed, IsDefaultSearchColumn = true }
                        };
                    break;
                case "UNITMASTER":
                    LabelCode = "Unit Code :";
                    LabelDesc = "Unit of Measurement :";
                    LabelCodeStatusMsg = "Please enter/select Unit Code";
                    LabelDescStatusMsg = "Please enter Unit of Measurement";
                    ComboBoxMaxLength = 5;
                    DescTextboxMaxLength = 9;
                    ComboBoxWidth = 60;
                    UserRoleObjName = "UNIT MASTER";
                    //   OperErrorMessage = "Unit Code is Required";
                    //  OperErrorMessageDesc = "Unit Measurement is Required";
                    ActionPermission = oUnitMaster.GetUserRights(UserRoleObjName);
                    FormLoadRights();
                    Columns = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "OPER_CODE", ColumnDesc = "Unit Code", ColumnWidth = 85 },
                            new DropdownColumns() { ColumnName = "OPER_DESC", ColumnDesc = "Unit of Measurement", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "STATUS", ColumnDesc = "Active?", ColumnWidth = 70, ShowInDropdown = false }
                            //new DropdownColumns() { ColumnName = "OPER_CODE_SORT", ColumnDesc = "Unit Code", ColumnWidth = 0, ColumnVisibility = Visibility.Collapsed, IsDefaultSearchColumn = true }
                        };
                    break;
                case "COATMASTER":
                    LabelCode = "Coating Code :";
                    LabelDesc = "Coating Desc :";
                    LabelCodeStatusMsg = "Please enter/select Coating Code";
                    LabelDescStatusMsg = "Please enter Coating Desc";
                    ComboBoxMaxLength = 3;
                    DescTextboxMaxLength = 74;
                    UserRoleObjName = "COATING MASTER";

                    //   OperErrorMessage = "Coating Code is Required";
                    //   OperErrorMessageDesc = "Coating Desc is Required";

                    ActionPermission = oCoatingMaster.GetUserRights(UserRoleObjName);
                    FormLoadRights();

                    Columns = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "OPER_CODE", ColumnDesc = "Coating Code", ColumnWidth = 105 },
                            new DropdownColumns() { ColumnName = "OPER_DESC", ColumnDesc = "Coating Desc", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "STATUS", ColumnDesc = "Active?", ColumnWidth = 70, ShowInDropdown = false }
                            //new DropdownColumns() { ColumnName = "OPER_CODE_SORT", ColumnDesc = "Coating Code", ColumnWidth = 0, ColumnVisibility = Visibility.Collapsed, IsDefaultSearchColumn = true }
                        };
                    break;
                case "CUSTOMER":
                    LabelCode = "Customer Code :";
                    LabelDesc = "Customer Name :";
                    LabelCodeStatusMsg = "Please enter/select Customer Code";
                    LabelDescStatusMsg = "Please enter Customer Name";
                    ComboBoxMaxLength = 9;
                    DescTextboxMaxLength = 49;
                    ComboBoxWidth = 90;
                    UserRoleObjName = "CUSTOMER MASTER";
                    FirstZero = false;

                    //  OperErrorMessage = "Customer Code is Required";
                    //  OperErrorMessageDesc = "Customer Name is Required";

                    ActionPermission = oCustMaster.GetUserRights(UserRoleObjName);
                    FormLoadRights();
                    Columns = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "OPER_CODE", ColumnDesc = "Cust. Code", ColumnWidth = 90 },
                            new DropdownColumns() { ColumnName = "OPER_DESC", ColumnDesc = "Cust. Name", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "STATUS", ColumnDesc = "Active?", ColumnWidth = 70, ShowInDropdown = false }
                            //new DropdownColumns() { ColumnName = "OPER_CODE_SORT", ColumnDesc = "Customer Code", ColumnWidth = 0, ColumnVisibility = Visibility.Collapsed, IsDefaultSearchColumn = true }
                        };
                    break;

            }
        }

        public void FormLoadRights()
        {
            if (ActionPermission.AddNew == true)
            {

                AddEnable = true;
                EditEnable = false;
                SaveEnable = true;
                DeleteEnable = false;
                ButtonEnable = Visibility.Collapsed;
                NextAction = "ADD";
                // TxtDecReadOnly = false;
                TxtReadOnly = false;
                CheckReadOnly = true;
                IsVisibilityDelete = Visibility.Visible;
                IsActive = true;
                IsDeleteEnable = false;
                OperCode = "";
                OperDesc = "";
                if (ActionPermission.Edit == true)
                {
                    EditEnable = true;
                    SaveEnable = true;
                    AddEnable = false;
                    //DeleteEnable = true;



                    if (ActionPermission.AddNew == true)
                    {
                        ButtonEnable = Visibility.Collapsed;
                        NextAction = "ADD";
                        TxtReadOnly = false;
                    }
                    else
                    {
                        ButtonEnable = Visibility.Visible;
                        TxtReadOnly = true;
                        NextAction = "EDIT";
                        SetMaxLength();
                    }

                }
                else if (ActionPermission.Edit == false)
                {
                    EditEnable = false;
                    DeleteEnable = false;
                    if (ActionPermission.View == true && ActionPermission.AddNew == false && ActionPermission.Edit == false)
                    {
                        SaveEnable = false;
                        ButtonEnable = Visibility.Visible;
                        TxtDecReadOnly = true;
                        TxtReadOnly = true;
                        CheckReadOnly = false;
                    }
                }

            }
            else if (ActionPermission.AddNew == false)
            {
                // EditEnable = true;
                //SaveEnable = true;
                // AddEnable = false;
                // ButtonEnable = Visibility.Visible;
                IsVisibilityDelete = Visibility.Visible;

                DeleteEnable = false;
                if (ActionPermission.Edit == true)
                {
                    IsActive = true;
                    IsDeleteEnable = true;
                    EditEnable = true;
                    SaveEnable = true;
                    AddEnable = false;
                    //DeleteEnable = true;
                    // TxtDecReadOnly = false;
                    TxtReadOnly = true;
                    ButtonEnable = Visibility.Visible;
                    NextAction = "EDIT";
                    CheckReadOnly = true;
                    OperCode = "";
                    OperDesc = "";
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
                        CheckReadOnly = false;
                    }
                }
            }

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


        private bool _checkreadonly = false;
        public bool CheckReadOnly
        {
            get { return _checkreadonly; }
            set
            {
                _checkreadonly = value;
                NotifyPropertyChanged("CheckReadOnly");
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

        public ICommand AlertCommand { get { return this.alertCommand; } }
        private void KeyPressalertCommand()
        {
            // MessageBox.Show("sdfsdfsdfsdsdf");
        }

        public void LoadTableData()
        {
            if (FormName == "OPERMASTER") this.DtDataview = operModel.GetOpertionMaster();
            if (FormName == "FINISHMASTER") this.DtDataview = ofinishModel.GetFinishMaster();
            if (FormName == "UNITMASTER") this.DtDataview = oUnitMaster.GetUnitMaster();
            if (FormName == "COATMASTER") this.DtDataview = oCoatingMaster.GetcoatingnMaster();
            if (FormName == "CUSTOMER") this.DtDataview = oCustMaster.GetCustomerMaster();
            // FillColumNameList();
        }

        public void FillColumNameList()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = DtDataview.Table;
                string colvalue = string.Empty;
                ColumnName = new List<String> { };
                foreach (DataColumn col in dt.Columns)
                {
                    colvalue = col.ColumnName.Replace("_", " ").ToUpper();
                    ColumnName.Add(colvalue.ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
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




        private int _comboboxwidth;
        public int ComboBoxWidth
        {
            get
            {
                return _comboboxwidth;
            }
            set
            {
                _comboboxwidth = value;
                NotifyPropertyChanged("ComboBoxWidth");
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

        private int _descTextboxMaxLength;
        public int DescTextboxMaxLength
        {
            get
            {
                return _descTextboxMaxLength;
            }
            set
            {
                _descTextboxMaxLength = value;
                NotifyPropertyChanged("DescTextboxMaxLength");
            }
        }

        private ObservableCollection<DropdownColumns> _columns;
        public ObservableCollection<DropdownColumns> Columns
        {
            get
            {
                return _columns;
            }
            set
            {
                _columns = value;
                NotifyPropertyChanged("Columns");
            }
        }


        private List<string> _columName;
        public List<string> ColumnName
        {
            get
            {
                return _columName;
            }
            set
            {
                _columName = value;
            }

        }
        private Key _keyPressed;
        public Key KeyPressed
        {
            get
            {
                return _keyPressed;
            }
            set
            {
                _keyPressed = value;
                NotifyPropertyChanged("KeyPressed");
            }
        }


        public bool CanSearchTextBox()
        {
            try
            {
                if (KeyPressed != Key.F6)
                {
                    CommonFormValUpdtae();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        public Action CloseAction { get; set; }

        public DataView OperMaster
        {
            get;
            private set;
        }
        public DataView DtDataview
        {
            get
            {
                return this.OperMaster;
            }
            set
            {
                this.OperMaster = value;
                NotifyPropertyChanged("DtDataview");
            }
        }

        private bool _txtReadOnly;
        public bool TxtReadOnly
        {
            get { return _txtReadOnly; }
            set
            {
                this._txtReadOnly = value;
                NotifyPropertyChanged("TxtReadOnly");
            }
        }


        private Visibility _costvisible = Visibility.Collapsed;
        public Visibility CostVisible
        {
            get { return _costvisible; }
            set
            {
                _costvisible = value;
                NotifyPropertyChanged("CostVisible");
            }
        }
        private string _frmName = "";
        public string FormName
        {
            get { return _frmName; }
            set { _frmName = value; }
        }

        private string _labelCode;

        public String LabelCode
        {
            get { return _labelCode; }
            set
            {
                _labelCode = value;
                NotifyPropertyChanged("LabelCode");
            }

        }
        private string _labelDesc;
        public String LabelDesc
        {
            get { return _labelDesc; }
            set
            {
                _labelDesc = value;
                NotifyPropertyChanged("LabelDesc");
            }

        }

        private string _labelCodeStatusMsg;
        public String LabelCodeStatusMsg
        {
            get { return _labelCodeStatusMsg; }
            set
            {
                _labelCodeStatusMsg = value;
                NotifyPropertyChanged("LabelCodeStatusMsg");
            }

        }
        private string _labelDescStatusMsg;
        public String LabelDescStatusMsg
        {
            get { return _labelDescStatusMsg; }
            set
            {
                _labelDescStatusMsg = value;
                NotifyPropertyChanged("LabelDescStatusMsg");
            }
        }

        private String _nextAction = "ADD";
        public String NextAction
        {
            get { return _nextAction; }
            set
            {
                _nextAction = value;
                NotifyPropertyChanged("NextAction");
            }

        }

        public ICommand OnCloseCommand { get { return this._onCancelCommand; } }
        public void Cancel()
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
                //if (OperCode.IsNotNullOrEmpty() || OperDesc.IsNotNullOrEmpty())
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

        public ICommand LoadCommand { get { return this.loadCommand; } }
        private void LoadedCommand()
        {
            LoadFormData();
        }

        public ICommand EditClickCommand { get { return this.editClickCommand; } }
        private void EditSubmitCommand()
        {
            if (OperCode.IsNotNullOrEmpty() || OperDesc.IsNotNullOrEmpty())
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    CommonFormValUpdtae();
                    return;
                }
                else
                {
                    if (EditEnable == true)
                    {
                        SetUserRights("EDIT");
                        ButtonEnable = Visibility.Visible;
                        TxtReadOnly = true;
                    }

                    NextAction = "EDIT";

                    //  EditEnable = false;
                    //  DeleteEnable = true;
                    //  AddEnable = true;
                    OperCode = "";
                    TxtReadOnly = true;
                    OperDesc = "";
                    ShowInCaset = "0";
                    if (FormName == "FINISHMASTER")
                    {
                        COF = "";
                        SaltSprayRed = "";
                        SaltSprayWhite = "";
                        CoatingThickness = "";
                        CoatingWight = "";
                        ColorAppearance = "";
                    }
                    else if (FormName == "COATMASTER")
                    {
                        COF = "";
                        SaltSprayRed = "";
                        SaltSprayWhite = "";
                        ColorAppearance = "";
                    }
                }
            }
            else
            {
                if (EditEnable == true)
                {
                    SetUserRights("EDIT");
                    ButtonEnable = Visibility.Visible;
                    TxtReadOnly = true;
                }

                NextAction = "EDIT";

                //  EditEnable = false;
                //  DeleteEnable = true;
                //  AddEnable = true;
                OperCode = "";
                TxtReadOnly = true;
                OperDesc = "";
                ShowInCaset = "0";
                if (FormName == "FINISHMASTER")
                {
                    COF = "";
                    SaltSprayRed = "";
                    SaltSprayWhite = "";
                    CoatingThickness = "";
                    CoatingWight = "";
                    ColorAppearance = "";
                }
                else if (FormName == "COATMASTER")
                {
                    COF = "";
                    SaltSprayRed = "";
                    SaltSprayWhite = "";
                    ColorAppearance = "";
                }
            }
            //     FocusCombo = true;
            //ComboBoxMaxLength = 0;

        }
        public ICommand AddClickCommand { get { return this.addClickCommand; } }
        private void AddSubmitCommand()
        {
            if (OperCode.IsNotNullOrEmpty() || OperDesc.IsNotNullOrEmpty())
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    CommonFormValUpdtae();
                    return;
                }
                else
                {

                    NextAction = "ADD";
                    if (AddEnable == true)
                    {
                        SetUserRights("ADD");
                        ButtonEnable = Visibility.Collapsed;
                    }

                    // ComboBoxMaxLength = 10;
                    // EditEnable = true;
                    // AddEnable = false;
                    // DeleteEnable = false;
                    TxtReadOnly = false;
                    OperCode = "";
                    OperDesc = "";
                    ShowInCaset = "1";
                    IsActive = true;
                    if (FormName == "FINISHMASTER")
                    {
                        COF = "";
                        SaltSprayRed = "";
                        SaltSprayWhite = "";
                        CoatingThickness = "";
                        CoatingWight = "";
                        ColorAppearance = "";
                    }
                    else if (FormName == "COATMASTER")
                    {
                        COF = "";
                        SaltSprayRed = "";
                        SaltSprayWhite = "";
                        ColorAppearance = "";
                    }
                }
            }
            else
            {
                NextAction = "ADD";
                if (AddEnable == true)
                {
                    SetUserRights("ADD");
                    ButtonEnable = Visibility.Collapsed;
                }

                // ComboBoxMaxLength = 10;
                // EditEnable = true;
                // AddEnable = false;
                // DeleteEnable = false;
                TxtReadOnly = false;
                OperCode = "";
                IsActive = true;
                OperDesc = "";
                ShowInCaset = "1";
                if (FormName == "FINISHMASTER")
                {
                    COF = "";
                    SaltSprayRed = "";
                    SaltSprayWhite = "";
                    CoatingThickness = "";
                    CoatingWight = "";
                    ColorAppearance = "";
                }
                else if (FormName == "COATMASTER")
                {
                    COF = "";
                    SaltSprayRed = "";
                    SaltSprayWhite = "";
                    ColorAppearance = "";
                }
            }
            // FocusCombo = true;

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

        string _theSelectedItem = null;
        public string TheSelectedItem
        {
            get { return _theSelectedItem; }
            set { _theSelectedItem = value; } // NotifyPropertyChanged
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = " ")]
        public string OperCode
        {
            get
            {
                return this._operCode;
            }
            set
            {
                this._operCode = value;
                //                NotifyPropertyChanged("OperCode");
                NotifyPropertyChanged("OperCode");
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = " ")]
        public string OperDesc
        {
            get
            {
                return this._operDesc;
            }
            set
            {
                this._operDesc = value;
                NotifyPropertyChanged("OperDesc");
                //  NotifyPropertyChanged("OperDesc");
            }
        }
        private string _colorAppearance = "";
        public string ColorAppearance
        {
            get
            {
                return this._colorAppearance;
            }
            set
            {
                this._colorAppearance = value;
                NotifyPropertyChanged("ColorAppearance");
            }
        }

        private string _coatingWight = "";
        public string CoatingWight
        {
            get
            {
                return this._coatingWight;
            }
            set
            {
                this._coatingWight = value;
                NotifyPropertyChanged("CoatingWight");
            }
        }
        private string _coatingThickness = "";
        public string CoatingThickness
        {
            get
            {
                return this._coatingThickness;
            }
            set
            {
                this._coatingThickness = value;
                NotifyPropertyChanged("CoatingThickness");
            }
        }

        private string _saltSprayWhite = "";
        public string SaltSprayWhite
        {
            get
            {
                return this._saltSprayWhite;
            }
            set
            {
                this._saltSprayWhite = value;
                NotifyPropertyChanged("SaltSprayWhite");
            }
        }

        private string _saltSprayRed = "";
        public string SaltSprayRed
        {
            get
            {
                return this._saltSprayRed;
            }
            set
            {
                this._saltSprayRed = value;
                NotifyPropertyChanged("SaltSprayRed");
            }
        }

        private string _cof = "";
        public string COF
        {
            get
            {
                return this._cof;
            }
            set
            {
                this._cof = value;
                NotifyPropertyChanged("COF");
            }
        }

        public string ShowInCaset
        {
            get
            {
                return this._showInCost;
            }
            set
            {
                this._showInCost = value;
                NotifyPropertyChanged("ShowInCaset");
            }
        }
        public string Message
        {
            get
            {
                return this._message;
            }
            set
            {
                this._message = value;
            }
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

        public DataTable DtData { get; set; }

        public ICommand SelectChangeComboCommand { get { return this.selectChangeComboCommand; } }
        private void SelectDataRow()
        {
            if (SelectedRow != null)
            {
                OperCode = SelectedRow["OPER_CODE"].ToString();
                OperDesc = SelectedRow["OPER_DESC"].ToString();
                TxtReadOnly = true;

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

                if (FormName == "OPERMASTER")
                {
                    ShowInCaset = SelectedRow["SHOW_IN_COST"].ToString();
                }
                else if (FormName == "FINISHMASTER")
                {
                    COF = SelectedRow["COF"].ToString();
                    SaltSprayRed = SelectedRow["SALT_SPRAY_RED"].ToString();
                    SaltSprayWhite = SelectedRow["SALT_SPRAY_WHITE"].ToString();
                    CoatingThickness = SelectedRow["COATING_THICKNESS"].ToString();
                    CoatingWight = SelectedRow["COATING_WEIGHT"].ToString();
                    ColorAppearance = SelectedRow["COLORAPP"].ToString();
                }
                else if (FormName == "COATMASTER")
                {
                    COF = SelectedRow["COF"].ToString();
                    SaltSprayRed = SelectedRow["SALT_SPRAY_RED"].ToString();
                    SaltSprayWhite = SelectedRow["SALT_SPRAY_WHITE"].ToString();
                    ColorAppearance = SelectedRow["COLORAPP"].ToString();
                }
            }

        }

        private string _errMessage = "";

        public ICommand UpdateOperMastCommand { get { return this.updateOperMastCommand; } }
        public void CommonFormValUpdtae()
        {
            try
            {
                if (IsActive == false && IsInActive == false) IsActive = true;
                isActiveSave = (IsActive) ? false : true;
                if (FormName == "OPERMASTER") UpdateOperMaster();
                if (FormName == "FINISHMASTER") UpdateFinishMaster();
                if (FormName == "UNITMASTER") UpdateUnitMaster();
                if (FormName == "COATMASTER") UpdatCoatingMaster();
                if (FormName == "CUSTOMER") updateCustomerMaster();
                LoadTableData();
                TxtReadOnly = (NextAction == "EDIT") ? true : false;

                FocusCombo = true;
                // TxtReadOnly = true;
                FocusButton = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void updateCustomerMaster()
        {
            try
            {
                if (ButtonEnable == Visibility.Visible)
                {
                    NextAction = "EDIT";
                }
                else if (ButtonEnable == Visibility.Collapsed)
                {
                    NextAction = "ADD";
                }

                if (String.IsNullOrEmpty(Convert.ToString(OperCode)) || OperCode.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Customer Code"));
                    FocusButton = true;
                    Flag = true;
                    //MessageBox.Show("Customer Code should not be Empty", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (String.IsNullOrEmpty(OperDesc) || OperDesc.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Customer Name"));
                    FocusButton = true;
                    Flag = true;
                    //MessageBox.Show("Customer Name should not be Empty", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    bool val = false;
                    try
                    {
                        Flag = false;
                        OperCode = OperCode.Trim();
                        //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                        //Progress.Start();
                        val = oCustMaster.AddNewCustomerMaster(isActiveSave, Convert.ToDecimal(OperCode), OperDesc, ShowInCaset, NextAction, ref _errMessage);
                        //Progress.End();
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                    }
                    if (val)
                    {
                        ShowInformationMessage(_errMessage);
                        //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        //ClearOperMaster();
                    }
                    if (val == false)
                    {
                        if ((string)_errMessage != "")
                            ShowInformationMessage(_errMessage);
                        //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    FocusButton = true;
                }

                if (Flag == true)
                {
                    if (AddEnable == true)
                    {
                        NextAction = "EDIT";
                        SetUserRights("EDIT");
                        ButtonEnable = Visibility.Visible;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void UpdatCoatingMaster()
        {
            try
            {
                if (ButtonEnable == Visibility.Visible)
                {
                    NextAction = "EDIT";
                }
                else if (ButtonEnable == Visibility.Collapsed)
                {
                    NextAction = "ADD";
                }
                if (String.IsNullOrEmpty(Convert.ToString(OperCode)) || OperCode.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Coating Code"));
                    Flag = true;
                    //MessageBox.Show("Coating Code should be Entered", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (String.IsNullOrEmpty(OperDesc) || OperDesc.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Coating Desc"));
                    Flag = true;
                    //MessageBox.Show("Coating Desc should not be Empty", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    bool val = false;
                    try
                    {
                        Flag = false;
                        //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                        //Progress.Start();
                        val = oCoatingMaster.AddNewCoatingMaster(isActiveSave, Convert.ToString(OperCode.Trim()), OperDesc, ShowInCaset, NextAction, ColorAppearance, SaltSprayWhite, SaltSprayRed, COF, ref _errMessage);
                        //Progress.End();
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                    }
                    if (val)
                    {
                        ShowInformationMessage(_errMessage);
                        //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        //ClearOperMaster();
                    }
                    if (val == false)
                    {
                        if ((string)_errMessage != "")
                            ShowInformationMessage(_errMessage);
                        //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                if (Flag == true)
                {
                    if (AddEnable == true)
                    {
                        NextAction = "EDIT";
                        SetUserRights("EDIT");
                        ButtonEnable = Visibility.Visible;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void UpdateUnitMaster()
        {
            try
            {
                string _localCode;
                if (ButtonEnable == Visibility.Visible)
                {
                    NextAction = "EDIT";
                }
                else if (ButtonEnable == Visibility.Collapsed)
                {
                    NextAction = "ADD";
                }
                _localCode = Convert.ToString(OperCode);
                if (String.IsNullOrEmpty(_localCode) || _localCode.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Unit Code"));
                    Flag = true;
                    //MessageBox.Show("Unit Code should not be Empty", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (String.IsNullOrEmpty(OperDesc) || OperDesc.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Unit of Measurement"));
                    Flag = true;
                    //MessageBox.Show("Unit of Measurement should not be Empty", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    bool val = false;
                    try
                    {
                        Flag = false;
                        //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                        //Progress.Start();

                        val = oUnitMaster.AddNewUnitMaster(isActiveSave, _localCode.Trim(), OperDesc, ShowInCaset, NextAction, ref _errMessage);
                        //Progress.End();
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                    }
                    if (val)
                    {
                        ShowInformationMessage(_errMessage);
                        //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        //ClearOperMaster();
                    }
                    if (val == false)
                    {
                        if ((string)_errMessage != "")
                            ShowInformationMessage(_errMessage);
                        //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

                if (Flag == true)
                {
                    if (AddEnable == true)
                    {
                        NextAction = "EDIT";
                        SetUserRights("EDIT");
                        ButtonEnable = Visibility.Visible;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void UpdateFinishMaster()
        {
            try
            {
                string _localCode;
                if (ButtonEnable == Visibility.Visible)
                {
                    NextAction = "EDIT";
                }
                else if (ButtonEnable == Visibility.Collapsed)
                {
                    NextAction = "ADD";
                }
                _localCode = Convert.ToString(OperCode);
                if (String.IsNullOrEmpty(_localCode) || _localCode.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Finish Code"));
                    Flag = true;
                    //MessageBox.Show("Finish Code should not be Empty", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (String.IsNullOrEmpty(OperDesc) || OperDesc.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Finish Desc"));
                    Flag = true;

                    //MessageBox.Show("Finish Desc should not be Empty", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    try
                    {
                        Flag = false;
                        //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                        //Progress.Start();
                        bool val = ofinishModel.AddNewFinishMaster(isActiveSave, _localCode.Trim(), OperDesc, ShowInCaset, NextAction, ColorAppearance, CoatingWight, CoatingThickness, SaltSprayWhite, SaltSprayRed, COF, ref _errMessage);
                        //Progress.End();
                        if (val)
                        {
                            ShowInformationMessage(_errMessage);
                            //ClearOperMaster();
                        }
                        if (val == false)
                        {
                            if ((string)_errMessage != "")
                                ShowInformationMessage(_errMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.LogException();
                    }

                }

                if (Flag == true)
                {
                    if (AddEnable == true)
                    {
                        NextAction = "EDIT";
                        SetUserRights("EDIT");
                        ButtonEnable = Visibility.Visible;
                    }
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void UpdateOperMaster()
        {
            try
            {

                if (ButtonEnable == Visibility.Visible)
                {
                    NextAction = "EDIT";
                }
                else if (ButtonEnable == Visibility.Collapsed)
                {
                    NextAction = "ADD";
                }
                if (String.IsNullOrEmpty(OperCode) || OperCode.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Operation Code"));
                    Flag = true;
                    // MessageBox.Show("Operation Code should be Entered", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else if (String.IsNullOrEmpty(OperDesc) || OperDesc.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Operation Desc"));
                    Flag = true;
                    //MessageBox.Show("Operation Desc should be Entered", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Flag = false;
                    OperCode = OperCode.Trim();
                    bool val = operModel.AddNewOperationMaster(isActiveSave, Convert.ToDecimal(OperCode), OperDesc, ShowInCaset, NextAction, ref _errMessage);
                    if (val)
                    {
                        ShowInformationMessage(_errMessage);
                        //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        //ClearOperMaster();
                        //ShowInCaset = "1";
                    }
                    if (val == false)
                    {
                        if ((string)_errMessage != "")
                            ShowInformationMessage(_errMessage);
                        //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    }

                }
                //if (AddEnable == true)
                //{
                //    ShowInCaset = "1";
                //}
                if (Flag == true)
                {
                    if (AddEnable == true)
                    {
                        NextAction = "EDIT";
                        SetUserRights("EDIT");
                        ButtonEnable = Visibility.Visible;

                    }
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }



        private void DeleteCustomerMaster()
        {
            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(OperCode)) || OperCode.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Customer Code"));
                    //MessageBox.Show("Customer Code should be Entered", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    OperCode = OperCode.Trim();
                    if (oCustMaster.CheckCodeisEsxists(Convert.ToDecimal(OperCode)) == false)
                    {
                        MessageBox.Show("Customer Code not Exists please entered valid Customer Code", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    MessageBoxResult messageBoxResult = ShowWarningMessage(PDMsg.BeforeDelete("Customer code"), MessageBoxButton.YesNo);
                    //System.Windows.MessageBox.Show("Do you want to delete this Customer code?", ApplicationTitle, System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        bool val = oCustMaster.DeleteCustomerCode(Convert.ToDecimal(OperCode), NextAction, ref _errMessage);
                        if (val)
                        {
                            ShowInformationMessage(_errMessage);
                            //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearOperMaster();
                        }
                        if (val == false)
                        {
                            if ((string)_errMessage != "")
                                ShowInformationMessage(_errMessage);
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

        private void DeleteoperMaster()
        {
            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(OperCode)) || OperCode.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Operation Code"));
                    //MessageBox.Show("Operation Code should be Entered", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    OperCode = OperCode.Trim();
                    if (operModel.CheckOperCodeEsxists(Convert.ToDecimal(OperCode)) == false)
                    {
                        MessageBox.Show("Operation Code not Exists please entered valid Operation Code", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    MessageBoxResult messageBoxResult = ShowWarningMessage(PDMsg.BeforeDelete("Operation code"), MessageBoxButton.YesNo);
                    //System.Windows.MessageBox.Show("Do you want to delete this Operation code?", ApplicationTitle, System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        bool val = operModel.DeletOperCode(Convert.ToDecimal(OperCode), NextAction, ref _errMessage);
                        if (val)
                        {
                            ShowInformationMessage(_errMessage);
                            //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearOperMaster();
                        }
                        if (val == false)
                        {
                            if ((string)_errMessage != "")
                                ShowInformationMessage(_errMessage);
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

        private void DeleteUnitMaster()
        {
            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(OperCode)) || OperCode.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Unit Code"));
                    //MessageBox.Show("Unit Code cannot be empty", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (oUnitMaster.CheckUnitEsxists(Convert.ToString(OperCode.Trim())) == false)
                    {
                        MessageBox.Show("Unit Code not Exists please entered valid Unit Code", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    MessageBoxResult messageBoxResult = ShowWarningMessage(PDMsg.BeforeDelete("Unit code"), MessageBoxButton.YesNo);
                    //System.Windows.MessageBox.Show("Do you want to delete this Unit code?", ApplicationTitle, System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        bool val = oUnitMaster.DeletUnitCode(Convert.ToString(OperCode), NextAction, ref _errMessage);
                        if (val)
                        {
                            ShowInformationMessage(_errMessage);
                            //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearOperMaster();
                        }
                        if (val == false)
                        {
                            if ((string)_errMessage != "")
                                ShowInformationMessage(_errMessage);
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

        private void DeleteFinishMaster()
        {
            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(OperCode)) || OperCode.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Finish Code"));
                    //MessageBox.Show("Finish Code cannot be empty", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (ofinishModel.CheckFinishEsxists(Convert.ToString(OperCode.Trim())) == false)
                    {
                        MessageBox.Show("Finish Code not Exists please entered valid Finish Code", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    MessageBoxResult messageBoxResult = ShowWarningMessage(PDMsg.BeforeDelete("Finish code"), MessageBoxButton.YesNo);
                    //System.Windows.MessageBox.Show("Do you want to delete this Finish code?", ApplicationTitle, System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        bool val = ofinishModel.DeletFinishCode(Convert.ToString(OperCode), NextAction, ref _errMessage);
                        if (val)
                        {
                            ShowInformationMessage(_errMessage);
                            //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearOperMaster();
                        }
                        if (val == false)
                        {
                            if ((string)_errMessage != "")
                                ShowInformationMessage(_errMessage);
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

        private void DeleteCoatingMaster()
        {
            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(OperCode)) || OperCode.Trim().Length == 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Coating Code"));
                    //MessageBox.Show("Coating Code cannot be empty", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    if (oCoatingMaster.CheckCoatingEsxists(Convert.ToString(OperCode.Trim())) == false)
                    {
                        MessageBox.Show("Coating Code not Exists please entered valid Coating Code", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    MessageBoxResult messageBoxResult = ShowWarningMessage(PDMsg.BeforeDelete("Coating code"), MessageBoxButton.YesNo);
                    //System.Windows.MessageBox.Show("Do you want to delete this Coating code?", ApplicationTitle, System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        bool val = oCoatingMaster.DeletcoatingCode(Convert.ToString(OperCode), NextAction, ref _errMessage);
                        if (val)
                        {
                            ShowInformationMessage(_errMessage);
                            //MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearOperMaster();
                        }
                        if (val == false)
                        {
                            if ((string)_errMessage != "")
                                ShowInformationMessage(_errMessage);
                            // MessageBox.Show(_errMessage, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        public ICommand DeleteClickCommand { get { return this.deleteClickCommand; } }
        private void DeleteSubmitCommand()
        {
            NextAction = "DELETE";
            if (FormName == "OPERMASTER")
            {
                if (DeleteEnable == true)
                {
                    DeleteoperMaster();
                }

            }

            if (FormName == "FINISHMASTER")
            {
                if (DeleteEnable == true)
                {
                    DeleteFinishMaster();
                }

            }
            if (FormName == "UNITMASTER")
            {
                if (DeleteEnable == true)
                {
                    DeleteUnitMaster();
                }

            }

            if (FormName == "COATMASTER")
            {
                if (DeleteEnable == true)
                {
                    DeleteCoatingMaster();
                }

            }
            if (FormName == "CUSTOMER")
            {
                if (DeleteEnable == true)
                {
                    DeleteCustomerMaster();
                }
            }

            LoadTableData();
        }

        private void ClearOperMaster()
        {
            //OperCode = 0;
            //   OperCode = "";
            OperCode = "";
            OperDesc = "";
            ShowInCaset = "0";
            FocusCombo = true;
            IsVisibilityDelete = Visibility.Visible;
            IsActive = true;
            IsDeleteEnable = false;
            FormLoadRights();
            if (FormName == "FINISHMASTER" || FormName == "COATMASTER")
            {
                COF = "";
                SaltSprayRed = "";
                SaltSprayWhite = "";
                CoatingThickness = "";
                CoatingWight = "";
                ColorAppearance = "";
            }
        }

        private bool _firstZero = true;
        public bool FirstZero
        {
            get { return _firstZero; }
            set
            {
                _firstZero = value;
                NotifyPropertyChanged("FirstZero");
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



    }
}

