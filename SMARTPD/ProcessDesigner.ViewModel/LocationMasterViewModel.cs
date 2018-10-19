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
    class LocationMasterViewModel : BindableBase
    {
        private string _locCode;
        private string _locName;
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
            DtDataview=oLocMaster.GetLocationMaster();
            this.selectChangeComboCommand = new DelegateCommand(this.SelectDataRow);
            this.addClickCommand = new DelegateCommand(this.AddSubmitCommand);
            this.editClickCommand = new DelegateCommand(this.EditSubmitCommand);
            this.updateLocCommand = new DelegateCommand(this.CommonFormValUpdtae);
            this.deleteClickCommand = new DelegateCommand(this.DeleteSubmitCommand);
        }

         public string LocCode
         {
             get
             {
                 return this._locCode;
             }
             set
             {
                 SetProperty(ref this._locCode, value);
                 OnPropertyChanged("LocCode");
             }
         }
         public string LocName
         {
             get
             {
                 return this._locName;
             }
             set
             {
                 SetProperty(ref this._locName, value);
                 OnPropertyChanged("LocName");
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
                 OnPropertyChanged("DtDataview");
             }
         }
         string _theSelectedItem = null;
         public string TheSelectedItem
         {
             get { return _theSelectedItem; }
             set { _theSelectedItem = value; } // NotifyPropertyChanged
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
         public ICommand SelectChangeComboCommand { get { return this.selectChangeComboCommand; } }
         private void SelectDataRow()
         {
             LocCode = (string)(SelectedRow["LOC_CODE"].ToString());
             LocName = (string)SelectedRow["LOCATION"].ToString();
         }

         public ICommand AddClickCommand { get { return this.addClickCommand; } }
         private void AddSubmitCommand()
         {
             NextAction = "ADD";
             EditEnable = true;
             AddEnable = false;
             LocCode = string.Empty;
             LocName  = string.Empty;
             DeleteEnable = false;
             ButtonEnable = Visibility.Collapsed;
         }
         private string _nextAction = "ADD";
         public string NextAction
         {
             get { return _nextAction; }
             set
             {
                 _nextAction = value;
                 OnPropertyChanged("NextAction");
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
         public ICommand EditClickCommand { get { return this.editClickCommand; } }
         private void EditSubmitCommand()
         {
             NextAction = "EDIT";
             EditEnable = false;
             DeleteEnable = true;
             AddEnable = true;
             LocCode = string.Empty;
             LocName = string.Empty;
             ButtonEnable = Visibility.Visible;
         }

        private void ClearOperMaster()
        {
            LocCode =string.Empty ;
            LocName=string.Empty ;
        }
        
         public ICommand UpdateLocCommand { get { return this.updateLocCommand; } }
         public void CommonFormValUpdtae()
         {
             try
             {

                 if (String.IsNullOrEmpty(Convert.ToString(LocCode)))
                 {
                     System.Windows.MessageBox.Show("Location Code should be Entered", "Process Designer");
                 }
                 else if (String.IsNullOrEmpty(LocName))
                 {
                     System.Windows.MessageBox.Show("Location Name should be Entered", "Process Designer"); 
                 }
                 else
                 {
                     bool val = oLocMaster.AddNewLocationMaster(LocCode, LocName, NextAction, ref _errMessage);
                     if (val)
                     {
                         System.Windows.MessageBox.Show(_errMessage, "Process Designer");
                         //MessageBox.Show(_errMessage);
                         ClearOperMaster();
                     }
                     if (val == false)
                     {
                         if ((string)_errMessage != "") MessageBox.Show(_errMessage, "Process Designer");
                     }

                 }

                 DtDataview = oLocMaster.GetLocationMaster();
              //   LoadTableData();
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }

         public ICommand DeleteClickCommand { get { return this.deleteClickCommand; } }
         private void DeleteSubmitCommand()
         {
             try
             {
                 if (String.IsNullOrEmpty(Convert.ToString(LocCode)))
                 {
                     MessageBox.Show("Customer Code should be Entered" ,"Process Designer");
                 }
                 else
                 {
                     MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you really want to delete this Customer code ?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
                     if (messageBoxResult == MessageBoxResult.Yes)
                     {
                         bool val = oLocMaster.DeleteLocationCode(LocCode, NextAction, ref _errMessage);
                         if (val)
                         {
                             MessageBox.Show(_errMessage,"Process Designer");
                             ClearOperMaster();
                         }
                         if (val == false)
                         {
                             if ((string)_errMessage != "") MessageBox.Show(_errMessage,"Process Designer");
                         }
                     }
                 }
             }
             catch (Exception ex)
             {
                 throw ex;
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
    }
}
