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

namespace ProcessDesigner.ViewModel
{
    public class OperationMasterViewModel : BindableBase
    //INotifyPropertyChanged 
    //BindableBase
    {
        private decimal _operCode;
        //  private string _operCode;
        private string _operDesc;
        private string _showInCost;
        private string _message;
        private Action _closeAction;

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
        //private readonly ICommand clodeCommand;
        private RelayCommand _onCancelCommand;
        private readonly ICommand keyCommand;

        //  private RelayCommand closeCommand;
        //    public ICommand CloseCommand
        //    {
        //        get
        //{
        //    if(closeCommand == null)
        //    (
        //        closeCommand = new RelayCommand(param => Close(), param => CanClose);
        //    )
        //}
        //    }


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


            //  this.updateOperMastCommand = new DelegateCommand(this.UpdateOperMaster);
            this.updateOperMastCommand = new DelegateCommand(this.CommonFormValUpdtae);
            this.selectChangeComboCommand = new DelegateCommand(this.SelectDataRow);
            this.addClickCommand = new DelegateCommand(this.AddSubmitCommand);
            this.editClickCommand = new DelegateCommand(this.EditSubmitCommand);
            this.deleteClickCommand = new DelegateCommand(this.DeleteSubmitCommand);
            this.loadCommand = new DelegateCommand(this.LoadedCommand);
            this.keyCommand = new DelegateCommand(this.KeyPressCommand);

            // this.clodeCommand = new DelegateCommand(this.CloseCommand);
        }

        public void LoadTableData()
        {
            if (FormName == "OPERMASTER") this.DtDataview = operModel.GetOpertionMaster();
            if (FormName == "FINISHMASTER") this.DtDataview = ofinishModel.GetFinishMaster();
            if (FormName == "UNITMASTER") this.DtDataview = oUnitMaster.GetUnitMaster();
            if (FormName == "COATMASTER") this.DtDataview = oCoatingMaster.GetcoatingnMaster();
            if (FormName == "CUSTOMER") this.DtDataview = oCustMaster.GetCustomerMaster();
        }
        public void LoadFormData()
        {
            switch (FormName)
            {
                case "OPERMASTER":
                    // this.OperMaster = operModel.GetOpertionMaster();
                    LabelCode = "Operation Code :";
                    LabelDesc = "Operation Desc :";
                    CostVisible = Visibility.Visible;
                    //CostVisible.v
                    break;
                case "FINISHMASTER":
                    LabelCode = "Finish Code :";
                    LabelDesc = "Finish Desc :";
                    // CostVisible = Visibility.Collapsed;
                    break;
                case "UNITMASTER":
                    LabelCode = "Unit Code :";
                    LabelDesc = "Unit of Measurement :";
                    break;
                case "COATMASTER":
                    LabelCode = "Coating Code :";
                    LabelDesc = "Coating Desc : ";
                    break;
                case "CUSTOMER":
                    LabelCode = "Customer Code :";
                    LabelDesc = "Customer Name : ";
                    break;

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
                OnPropertyChanged("KeyPressed");
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
                throw ex;
            }
        }
        //bool CanSearchTextBox
        //{
        //    get
        //    {
        //        //if (KeyPressed != Key.Up && KeyPressed != Key.Down && KeyPressed != Key.Left && KeyPressed != Key.Right && MatchSearchList != null)
        //        //    return true;
        //        //else
        //        //    return false;
        //    }
        //}

        public ICommand KeyCommand { get { return this.keyCommand; } }
        private void KeyPressCommand()
        {
            CanSearchTextBox();
        }

        public Action CloseAction { get; set; }

        public DataTable OperMaster
        {
            get;
            private set;
        }
        public DataView DtDataview
        {
            get
            {
                return this.OperMaster.DefaultView;
            }
            set
            {
                this.OperMaster = value.ToTable();
                OnPropertyChanged("DtDataview");
            }
        }

        private Visibility _CostVisible = Visibility.Collapsed;
        public Visibility CostVisible
        {
            get { return _CostVisible; }
            set
            {
                _CostVisible = value;
                OnPropertyChanged("CostVisible");
            }
        }
        private string _frmName;
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
                OnPropertyChanged("LabelCode");
            }

        }
        private string _labelDesc;
        public String LabelDesc
        {
            get { return _labelDesc; }
            set
            {
                _labelDesc = value;
                OnPropertyChanged("LabelDesc");
            }

        }

        private String _nextAction = "ADD";
        public String NextAction
        {
            get { return _nextAction; }
            set
            {
                _nextAction = value;
                OnPropertyChanged("NextAction");
            }

        }

        private void Cancel()
        {
            try
            {
                CloseAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ICommand OnCloseCommand
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

        public ICommand LoadCommand { get { return this.loadCommand; } }
        private void LoadedCommand()
        {
            LoadFormData();
        }

        public ICommand EditClickCommand { get { return this.editClickCommand; } }
        private void EditSubmitCommand()
        {
            NextAction = "EDIT";
            EditEnable = false;
            DeleteEnable = true;
            AddEnable = true;
            ButtonEnable = Visibility.Visible;
        }
        public ICommand AddClickCommand { get { return this.addClickCommand; } }
        private void AddSubmitCommand()
        {
            NextAction = "ADD";
            EditEnable = true;
            AddEnable = false;
            DeleteEnable = false;
            //OperCode = 0;
            OperDesc = "";
            ShowInCaset = "0";
            ButtonEnable = Visibility.Collapsed;
        }
        private Visibility _buttonVisible = Visibility.Collapsed;
        public Visibility ButtonEnable
        {
            get { return _buttonVisible; }
            set
            {
                _buttonVisible = value;
                OnPropertyChanged("ButtonEnable");
            }
        }

        private bool _deleteEnable = false;
        public bool DeleteEnable
        {
            get { return _deleteEnable; }
            set
            {
                _deleteEnable = value;
                OnPropertyChanged("DeleteEnable");
            }
        }

        private bool _addOperation = false;
        public bool AddEnable
        {
            get { return _addOperation; }
            set
            {
                _addOperation = value;
                OnPropertyChanged("AddEnable");
            }
        }
        private bool _editOpertion = true;
        public bool EditEnable
        {
            get { return _editOpertion; }
            set
            {
                _editOpertion = value;
                OnPropertyChanged("EditEnable");
            }
        }

        string _theSelectedItem = null;
        public string TheSelectedItem
        {
            get { return _theSelectedItem; }
            set { _theSelectedItem = value; } // NotifyPropertyChanged
        }

        //public string OperCode
        //{
        //    get
        //    {
        //        return this._operCode;
        //    }
        //    set
        //    {
        //        SetProperty(ref this._operCode, value);
        //        OnPropertyChanged("OperCode");
        //    }
        //}

        public decimal OperCode
        {
            get
            {
                return this._operCode;
            }
            set
            {
                SetProperty(ref this._operCode, value);
                OnPropertyChanged("OperCode");
            }
        }

        public string OperDesc
        {
            get
            {
                return this._operDesc;
            }
            set
            {
                SetProperty(ref this._operDesc, value);
                OnPropertyChanged("OperDesc");
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
                SetProperty(ref this._showInCost, value);
                OnPropertyChanged("ShowInCaset");
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
                SetProperty(ref this._message, value);
            }
        }
        private DataRow _SelectedRow;
        public DataRow SelectedRow
        {
            get
            {
                return _SelectedRow;
            }

            set
            {
                _SelectedRow = value;
            }
        }
        //public List<DDOPER_MAST> OperMaster { get; private set; }


        public DataTable DtData { get; set; }

        public ICommand SelectChangeComboCommand { get { return this.selectChangeComboCommand; } }
        private void SelectDataRow()
        {
            OperCode = decimal.Parse(SelectedRow["OPER_CODE"].ToString());
            OperDesc = SelectedRow["OPER_DESC"].ToString();
            if (FormName == "OPERMASTER") ShowInCaset = SelectedRow["SHOW_IN_COST"].ToString();
        }

        private string _errMessage;

        public ICommand UpdateOperMastCommand { get { return this.updateOperMastCommand; } }
        public void CommonFormValUpdtae()
        {
            try
            {
                if (FormName == "OPERMASTER") UpdateOperMaster();
                if (FormName == "FINISHMASTER") UpdateFinishMaster();
                if (FormName == "UNITMASTER") UpdateUnitMaster();
                if (FormName == "COATMASTER") UpdatCoatingMaster();
                if (FormName == "CUSTOMER") updateCustomerMaster();
                LoadTableData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void updateCustomerMaster()
        {
            if (String.IsNullOrEmpty(Convert.ToString(OperCode)))
            {
                MessageBox.Show("Customer Code should not be Empty", "Process Designer");
            }
            else if (String.IsNullOrEmpty(OperDesc))
            {
                MessageBox.Show("Customer Name should not be Empty", "Process Designer");
            }
            else
            {
                bool val = oCustMaster.AddNewCustomerMaster(OperCode, OperDesc, ShowInCaset, NextAction, ref _errMessage);
                if (val)
                {
                    MessageBox.Show(_errMessage, "Process Designer");
                    ClearOperMaster();
                }
                if (val == false)
                {
                    if ((string)_errMessage != "") MessageBox.Show(_errMessage, "Process Designer");
                }

            }
        }


        private void UpdatCoatingMaster()
        {
            if (String.IsNullOrEmpty(Convert.ToString(OperCode)))
            {
                MessageBox.Show("Coating Code should be Entered", "Process Designer");
            }
            else if (String.IsNullOrEmpty(OperDesc))
            {
                MessageBox.Show("Coating Desc should not be Empty", "Process Designer");
            }
            else
            {
                bool val = oCoatingMaster.AddNewCoatingMaster(Convert.ToString(OperCode), OperDesc, ShowInCaset, NextAction, ref _errMessage);
                if (val)
                {
                    MessageBox.Show(_errMessage, "Process Designer");
                    ClearOperMaster();
                }
                if (val == false)
                {
                    if ((string)_errMessage != "") MessageBox.Show(_errMessage, "Process Designer");
                }

            }
        }

        private void UpdateUnitMaster()
        {
            string _localCode;
            _localCode = Convert.ToString(OperCode);
            if (String.IsNullOrEmpty(_localCode))
            {
                MessageBox.Show("Unit Code should not be Empty", "Process Designer");
            }
            else if (String.IsNullOrEmpty(OperDesc))
            {
                MessageBox.Show("Unit of Measurement should not be Empty", "Process Designer");
            }
            else
            {
                bool val = oUnitMaster.AddNewUnitMaster(_localCode, OperDesc, ShowInCaset, NextAction, ref _errMessage);
                if (val)
                {
                    MessageBox.Show(_errMessage, "Process Designer");
                    ClearOperMaster();
                }
                if (val == false)
                {
                    if ((string)_errMessage != "") MessageBox.Show(_errMessage, "Process Designer");
                }

            }
        }

        private void UpdateFinishMaster()
        {
            string _localCode;
            _localCode = Convert.ToString(OperCode);
            if (String.IsNullOrEmpty(_localCode))
            {
                MessageBox.Show("Finish Code should not be Empty", "Process Designer");
            }
            else if (String.IsNullOrEmpty(OperDesc))
            {
                MessageBox.Show("Finish Desc should not be Empty", "Process Designer");
            }
            else
            {
                bool val = ofinishModel.AddNewFinishMaster(_localCode, OperDesc, ShowInCaset, NextAction, ref _errMessage);
                if (val)
                {
                    MessageBox.Show(_errMessage, "Process Designer");
                    ClearOperMaster();
                }
                if (val == false)
                {
                    if ((string)_errMessage != "") MessageBox.Show(_errMessage, "Process Designer");
                }

            }
        }

        private void UpdateOperMaster()
        {
            if (String.IsNullOrEmpty(Convert.ToString(OperCode)))
            {
                // System.Windows.MessageBox.Show("Do you really want to delete this Customer code ?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                MessageBox.Show("Operation Code should be Entered", "Process Designer");
            }
            else if (String.IsNullOrEmpty(OperDesc))
            {
                MessageBox.Show("Operation Desc should be Entered", "Process Designer");
            }
            else
            {
                bool val = operModel.AddNewOperationMaster(OperCode, OperDesc, ShowInCaset, NextAction, ref _errMessage);
                if (val)
                {
                    MessageBox.Show(_errMessage, "Process Designer");
                    ClearOperMaster();
                }
                if (val == false)
                {
                    if ((string)_errMessage != "") MessageBox.Show(_errMessage, "Process Designer");
                }

            }
        }



        private void DeleteCustomerMaster()
        {
            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(OperCode)))
                {
                    MessageBox.Show("Customer Code should be Entered", "Process Designer");
                }
                else
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you really want to delete this Customer code ?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        bool val = oCustMaster.DeleteCustomerCode(OperCode, NextAction, ref _errMessage);
                        if (val)
                        {
                            MessageBox.Show(_errMessage, "Process Designer");
                            ClearOperMaster();
                        }
                        if (val == false)
                        {
                            if ((string)_errMessage != "") MessageBox.Show(_errMessage, "Process Designer");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void DeleteoperMaster()
        {
            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(OperCode)))
                {
                    MessageBox.Show("Operation Code should be Entered", "Process Designer");
                }
                else
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you really want to delete this Operation code ?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        bool val = operModel.DeletOperCode(OperCode, NextAction, ref _errMessage);
                        if (val)
                        {
                            MessageBox.Show(_errMessage, "Process Designer");
                            ClearOperMaster();
                        }
                        if (val == false)
                        {
                            if ((string)_errMessage != "") MessageBox.Show(_errMessage, "Process Designer");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void DeleteUnitMaster()
        {
            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(OperCode)))
                {
                    MessageBox.Show("Unit Code cannot be empty", "Process Designer");
                }
                else
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you really want to delete this Unit code ?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        bool val = oUnitMaster.DeletUnitCode(Convert.ToString(OperCode), NextAction, ref _errMessage);
                        if (val)
                        {
                            MessageBox.Show(_errMessage, "Process Designer");
                            ClearOperMaster();
                        }
                        if (val == false)
                        {
                            if ((string)_errMessage != "") MessageBox.Show(_errMessage, "Process Designer");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void DeleteFinishMaster()
        {
            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(OperCode)))
                {
                    MessageBox.Show("Operation Desc should be Entered", "Process Designer");
                }
                else
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you really want to delete this Finish code ?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        bool val = ofinishModel.DeletFinishCode(Convert.ToString(OperCode), NextAction, ref _errMessage);
                        if (val)
                        {
                            MessageBox.Show(_errMessage, "Process Designer");
                            ClearOperMaster();
                        }
                        if (val == false)
                        {
                            if ((string)_errMessage != "") MessageBox.Show(_errMessage, "Process Designer");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void DeleteCoatingMaster()
        {
            try
            {
                if (String.IsNullOrEmpty(Convert.ToString(OperCode)))
                {
                    MessageBox.Show("Coating Code cannot be empty", "Process Designer");
                }
                else
                {
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you really want to delete this Coating code ?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        bool val = oCoatingMaster.DeletcoatingCode(Convert.ToString(OperCode), NextAction, ref _errMessage);
                        if (val)
                        {
                            MessageBox.Show(_errMessage, "Process Designer");
                            ClearOperMaster();
                        }
                        if (val == false)
                        {
                            if ((string)_errMessage != "") MessageBox.Show(_errMessage, "Process Designer");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public ICommand DeleteClickCommand { get { return this.deleteClickCommand; } }
        private void DeleteSubmitCommand()
        {
            NextAction = "DELETE";
            if (FormName == "OPERMASTER") DeleteoperMaster();
            if (FormName == "FINISHMASTER") DeleteFinishMaster();
            if (FormName == "UNITMASTER") DeleteUnitMaster();
            if (FormName == "COATMASTER") DeleteCoatingMaster();
            if (FormName == "CUSTOMER") DeleteCustomerMaster();
            LoadTableData();
        }

        private void ClearOperMaster()
        {
            OperCode = 0;
            OperDesc = "";
            ShowInCaset = "0";
        }
    }
}

