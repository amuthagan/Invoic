using System;
using ProcessDesigner.Model;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using System.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.UserControls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using WPF.MDI;

namespace ProcessDesigner.ViewModel
{
    public class CostSheetSearchViewModel : ViewModelBase
    {

        private DataView dvCostSheetSearch = null;
        private CostSheetSearchModel _costSheetSearchEntity = null;
        private CostSheetSearchBll _costSheetSearchbll;

        private readonly ICommand _onSearchCommand;
        public ICommand OnSearchCommand { get { return this._onSearchCommand; } }

        private readonly ICommand _onPrintCommand;
        public ICommand OnPrintCommand { get { return this._onPrintCommand; } }

        private readonly ICommand _modifyCommand;
        public ICommand ModifyCommand { get { return this._modifyCommand; } }

        private readonly ICommand _onCloseCommand;
        public ICommand OnCloseCommand { get { return this._onCloseCommand; } }


        private readonly ICommand selectChangeComboCommand;
        private ICommand _onCheckChangeCommandShowDomesticOnly;
        private ICommand _onCheckChangeCommandShowExpertOnly;
        private ICommand _onCheckChangeCommandPendingPartNoAllocation;
        private ICommand _onCheckChangeCommandShowPending;
        private ICommand _onCheckChangeCommandShowAll;
        private ICommand _copyDataCommand;
        private ICommand _allotPartNoCommand;
        private ICommand _markAsSubmittedCommand;
        private UserInformation _userInformation;
        private int _entityPrimaryKey;
        private OperationMode _operationMode;
        private WPF.MDI.MdiChild _mdiChild;
        public Action CloseAction { get; set; }
        private string calledfromparentform = "MainWindow";

        public CostSheetSearchViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, int entityPrimaryKey,
            OperationMode operationMode, string calledfromparentform = "MainWindow")
        {
            try
            {
                _userInformation = userInformation;
                _entityPrimaryKey = entityPrimaryKey;
                _operationMode = operationMode;
                _mdiChild = mdiChild;
                this.calledfromparentform = calledfromparentform.IsNotNullOrEmpty() ? calledfromparentform : "MainWindow";

                this.CostSheetSearch = new CostSheetSearchModel();
                _costSheetSearchbll = new CostSheetSearchBll(userInformation);
                this.selectChangeComboCommandCust = new DelegateCommand(this.SelectDataRowCust);
                this.selectChangeComboCommandCustDwg = new DelegateCommand(this.SelectDataRowCustDwg);
                this._onSearchCommand = new DelegateCommand(this.SearchCostSheet);
                this._onPrintCommand = new DelegateCommand(this.PrintCommand);
                this._onCloseCommand = new DelegateCommand(this.CloseCommand);
                this.selectChangeComboCommand = new DelegateCommand(this.SelectDataRow);
                this._modifyCommand = new DelegateCommand<DataRowView>(this.ModifyUsers);
                CostSheetCount = "Cost Sheet: ";
                this.DVCostSheetSearch = _costSheetSearchbll.GetPartNumberDetails();
                LoadFormData();
                _costSheetSearchbll.GetCustNameDetails(CostSheetSearch);
                _costSheetSearchbll.GetCustDwg(CostSheetSearch);
                PrintEnabled = false;
                SetdropDownItems();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void LoadFormData()
        {
            LabelCode = "Part Number* :";
            ComboBoxMaxLength = 10;
            Columns = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "Part Number", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "Part Description", ColumnWidth = "1*" }
                        };

        }

        private void SearchCostSheet()
        {
            try
            {
                Progress.ProcessingText = PDMsg.Search;
                Progress.Start();
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                CostSheetSearch.CostSheetSearchMasterDetails = _costSheetSearchbll.GetCostSheetSearchDetails(CostSheetSearch);
                if (CostSheetSearch.CostSheetSearchMasterDetails != null && CostSheetSearch.CostSheetSearchMasterDetails.Count > 0)
                {
                    PrintEnabled = true;
                    CostSheetCount = CostSheetSearch.CostSheetSearchMasterDetails.Count + " Cost Sheet(s) Found ";
                }
                else
                {
                    PrintEnabled = false;
                    CostSheetCount = "0 Cost Sheet Found";
                }
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                Progress.End();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        public ICommand SelectChangeComboCommand { get { return this.selectChangeComboCommand; } }
        private void SelectDataRow()
        {
            CostSheetSearch.PART_NO = SelectedRow["PART_NO"].ToString();
        }

        private DataRowView _selectedCostSheet;
        public DataRowView SelectedCostSheet
        {
            get { return _selectedCostSheet; }
            set
            {
                _selectedCostSheet = value;
                //SetProperty(ref _actionMode, value);
                NotifyPropertyChanged("SelectedCostSheet");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsPart;
        public ObservableCollection<DropdownColumns> DropDownItemsPart
        {
            get
            {
                return _dropDownItemsPart;
            }
            set
            {
                this._dropDownItemsPart = value;
                NotifyPropertyChanged("DropDownItemsPart");
            }
        }

        private Boolean _printEnabled;
        public Boolean PrintEnabled
        {
            get { return _printEnabled; }
            set
            {
                _printEnabled = value;
                //SetProperty(ref _actionMode, value);
                NotifyPropertyChanged("PrintEnabled");
            }
        }



        #region Close Button Action

        public void CloseCommand()
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

        #endregion

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        public ICommand OnCheckChangeCommandShowDomesticOnly
        {
            get
            {
                if (_onCheckChangeCommandShowDomesticOnly == null)
                {
                    _onCheckChangeCommandShowDomesticOnly = new RelayCommand(param => this.SelectAll(false, "ShowDomesticOnly"), null);
                  //  _onCheckChangeCommandShowExpertOnly = null;
                }
                

                
                return _onCheckChangeCommandShowDomesticOnly;
            }
        }

        public ICommand OnCheckChangeCommandShowExpertOnly
        {
            get
            {
                if (_onCheckChangeCommandShowExpertOnly == null)
                {
                    _onCheckChangeCommandShowExpertOnly = new RelayCommand(param => this.SelectAll(false, "ShowExpertOnly"), null);
                   // _onCheckChangeCommandShowDomesticOnly = null;
                }
                
                return _onCheckChangeCommandShowExpertOnly;
            }
        }

        public ICommand OnCheckChangeCommandPendingPartNoAllocation
        {
            get
            {
                if (_onCheckChangeCommandPendingPartNoAllocation == null)
                {
                    _onCheckChangeCommandPendingPartNoAllocation = new RelayCommand(param => this.SelectAll(false, "PendingPartNoAllocation"), null);
                }
                return _onCheckChangeCommandPendingPartNoAllocation;
            }
        }

        public ICommand OnCheckChangeCommandShowPending
        {
            get
            {
                if (_onCheckChangeCommandShowPending == null)
                {
                    _onCheckChangeCommandShowPending = new RelayCommand(param => this.SelectAll(false, "ShowPending"), null);
                }
                return _onCheckChangeCommandShowPending;
            }
        }

        public ICommand OnCheckChangeCommandShowAll
        {
            get
            {
                if (_onCheckChangeCommandShowAll == null)
                {
                    _onCheckChangeCommandShowAll = new RelayCommand(param => this.SelectAll(false, "ShowAll"), null);
                }
                return _onCheckChangeCommandShowAll;
            }
        }

        public ICommand CopyDataCommand
        {
            get
            {
                if (_copyDataCommand == null)
                {
                    _copyDataCommand = new RelayCommand(param => this.CopyData(), null);
                }
                return _copyDataCommand;
            }
        }

        public ICommand AllotPartNoCommand
        {
            get
            {
                if (_allotPartNoCommand == null)
                {
                    _allotPartNoCommand = new RelayCommand(param => this.AllotPartNo(), null);
                }
                return _allotPartNoCommand;
            }
        }

        public ICommand MarkAsSubmittedCommand
        {
            get
            {
                if (_markAsSubmittedCommand == null)
                {
                    _markAsSubmittedCommand = new RelayCommand(param => this.MarkAsSubmitted(), null);
                }
                return _markAsSubmittedCommand;
            }
        }

        private void SelectAll(bool check, string colname)
        {
            try
            {
                switch (colname)
                {
                    case "ShowDomesticOnly":
                        CostSheetSearch.ChkShowExpertOnly = check;
                        CostSheetSearch.ChkShowAll = check;
                        break;
                    case "ShowExpertOnly":
                        CostSheetSearch.ChkShowDomesticOnly = check;
                        CostSheetSearch.ChkShowAll = check;
                        break;
                    case "PendingPartNoAllocation":
                        CostSheetSearch.ChkShowAll = check;
                        break;
                    case "ShowPending":
                        CostSheetSearch.ChkShowAll = check;
                        break;
                    case "ShowAll":
                        CostSheetSearch.ChkShowExpertOnly = check;
                        CostSheetSearch.ChkShowDomesticOnly = check;
                        CostSheetSearch.ChkPendingPartNoAllocation = check;
                        CostSheetSearch.ChkShowPending = check;
                        CostSheetSearch.CUST_NAME = string.Empty;
                        CostSheetSearch.PROD_DESC = string.Empty;
                        CostSheetSearch.CUST_DWG_NO = string.Empty;
                        CostSheetSearch.PART_NO = string.Empty;
                        break;
                }
                NotifyPropertyChanged("CostSheetSearch");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public int CI_INFO_PK { get; set; }
        public void ModifyUsers(DataRowView selecteditem)
        {
            try
            {

                //if (selecteditem != null && calledfromparentform.IsNotNullOrEmpty() && calledfromparentform == "MainWindow")
                //{
                //    Window win = new Window();
                //    IconBitmapDecoder ibd = new IconBitmapDecoder(new Uri(@"pack://application:,,/Images/logo.ico", UriKind.RelativeOrAbsolute), BitmapCreateOptions.None, BitmapCacheOption.Default);
                //    win.Title = ApplicationTitle + " - " + "Cost Sheet Preparation";
                //    win.Icon = ibd.Frames[0];
                //    win.ResizeMode = ResizeMode.NoResize;
                //    CI_INFO_PK = selecteditem["CI_INFO_PK"].ToValueAsString().ToIntValue();
                //    System.Windows.Controls.UserControl userControl = new ProcessDesigner.frmFRCS(_userInformation, win, CI_INFO_PK, OperationMode.Edit);
                //    win.Content = userControl;
                //    win.Height = userControl.Height + 50;
                //    win.Width = userControl.Width + 10;
                //    win.ShowInTaskbar = false;
                //    win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                //    win.ShowDialog();
                //}
                //else if (selecteditem != null && calledfromparentform.IsNotNullOrEmpty() && calledfromparentform == "frmFRCS")
                //{
                //    CI_INFO_PK = selecteditem["CI_INFO_PK"].ToValueAsString().ToIntValue();
                //    CloseAction();
                //}

                if (selecteditem == null) return;

                MdiChild mdiCostSheet = new MdiChild();
                if (MainMDI.IsFormAlreadyOpen("Cost Sheet Preparation") == true)
                {
                    mdiCostSheet = (MdiChild)MainMDI.GetFormAlreadyOpened("Cost Sheet Preparation");
                    mdiCostSheet.Close();
                }
                CI_INFO_PK = selecteditem["CI_INFO_PK"].ToValueAsString().ToIntValue();
                ProcessDesigner.frmFRCS userControl = new ProcessDesigner.frmFRCS(_userInformation, mdiCostSheet, CI_INFO_PK, OperationMode.Edit);
                //frmCostSheetSearch frmCostSheetSearch = new frmCostSheetSearch(_userInformation, mdiCostSheetSearch, ActiveEntity.IDPK, OperationMode.View, "frmFRCS");
                mdiCostSheet.Title = ApplicationTitle + " - Cost Sheet Preparation";
                mdiCostSheet.Content = userControl;
                mdiCostSheet.Height = userControl.Height + 40;
                mdiCostSheet.Width = userControl.Width + 20;
                mdiCostSheet.MinimizeBox = true;
                mdiCostSheet.MaximizeBox = true;
                mdiCostSheet.Resizable = false;
                MainMDI.Container.Children.Add(mdiCostSheet);
                //}
                //else
                //{
                //    mdiCostSheet = (MdiChild)MainMDI.GetFormAlreadyOpened("Cost Sheet Preparation");
                //    //toolschedule = (frmToolSchedule_new)mdiCostSheetSearch.Content;
                //    MainMDI.SetMDI(mdiCostSheet);
                //}



            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }


        private void PrintCommand()
        {

            Progress.ProcessingText = PDMsg.Load;
            Progress.Start();
            DataTable dtSearchReport = _costSheetSearchbll.GetCostSheetSearchDetails(CostSheetSearch).ToTable();
            ProcessDesigner.frmReportViewer frv = new ProcessDesigner.frmReportViewer(dtSearchReport, "CostSheetSearch");
            if (!frv.ReadyToShowReport) return;
            frv.ShowDialog();
            Progress.End();
        }

        private readonly ICommand selectChangeComboCommandCust;
        public ICommand SelectChangeComboCommandCust { get { return this.selectChangeComboCommandCust; } }
        private void SelectDataRowCust()
        {
            if (SelectedCustName != null)
            {
                CostSheetSearch.CUST_NAME = SelectedCustName["CUST_NAME"].ToString();
            }
        }

        private readonly ICommand selectChangeComboCommandCustDwg;
        public ICommand SelectChangeComboCommandCustDwg { get { return this.selectChangeComboCommandCustDwg; } }
        private void SelectDataRowCustDwg()
        {
            if (SelectedCustDwg != null)
            {
                CostSheetSearch.CUST_DWG_NO = SelectedCustDwg["CUST_DWG_NO"].ToString();
            }
        }

        private DataRowView _selectedcustname;
        public DataRowView SelectedCustName
        {
            get
            {
                return _selectedcustname;
            }

            set
            {
                _selectedcustname = value;
            }
        }

        private DataRowView _selectedcustdwg;
        public DataRowView SelectedCustDwg
        {
            get
            {
                return _selectedcustdwg;
            }

            set
            {
                _selectedcustdwg = value;
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsCust;
        public ObservableCollection<DropdownColumns> DropDownItemsCust
        {
            get
            {
                return _dropDownItemsCust;
            }
            set
            {
                this._dropDownItemsCust = value;
                NotifyPropertyChanged("DropDownItemsCust");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsCustDwg;
        public ObservableCollection<DropdownColumns> DropDownItemsCustDwg
        {
            get
            {
                return _dropDownItemsCustDwg;
            }
            set
            {
                this._dropDownItemsCustDwg = value;
                NotifyPropertyChanged("DropDownItemsCustDwg");
            }
        }

        private void SetdropDownItems()
        {
            try
            {
                DropDownItemsCust = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CUST_NAME", ColumnDesc = "Customer Name", ColumnWidth = "1*" }
                        };
                DropDownItemsCustDwg = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CUST_DWG_NO", ColumnDesc = "Customer Dwg No", ColumnWidth = "1*" }
                        };
            }
            catch (Exception ex)
            {
                throw ex.LogException();
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

        public CostSheetSearchModel CostSheetSearch
        {
            get
            {
                return this._costSheetSearchEntity;
            }
            set
            {
                _costSheetSearchEntity = value;
                NotifyPropertyChanged("CostSheetSearch");
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

        public DataView DVCostSheetSearch
        {
            get
            {
                return this.dvCostSheetSearch;
            }
            set
            {
                this.dvCostSheetSearch = value;
                NotifyPropertyChanged("DVCostSheetSearch");
            }
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

        private string _showInCost;
        public string ShowInCaset
        {
            get
            {
                return this._showInCost;
            }
            set
            {
                _showInCost = value;
                NotifyPropertyChanged("ShowInCaset");
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

        private string _costsheetcount;
        public string CostSheetCount
        {
            get
            {
                return _costsheetcount;
            }
            set
            {
                _costsheetcount = value;
                NotifyPropertyChanged("CostSheetCount");
            }
        }

        private string _partNumber;
        public string PartNumber
        {
            get
            {
                return this._partNumber;
            }
            set
            {
                this._partNumber = value;
                NotifyPropertyChanged("PartNumber");
            }
        }



        private string _message = string.Empty;
        private string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                ShowInformationMessage(_message);
                //System.Windows.MessageBox.Show(_message, "SmartPD", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
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

        private void CopyData()
        {
            try
            {
                bool blnok = false;
                string varOldCIref = "";
                FeasibleReportAndCostSheet frcs;
                Window win = new Window();
                DDCI_INFO activeEntity = new DDCI_INFO();
                DDCI_INFO getEntity = new DDCI_INFO();
                List<DDCI_INFO> lstEntity = new List<DDCI_INFO>();
                if (SelectedCostSheet == null)
                {
                    Message = "CI Reference should be Selected";
                    return;
                }
                getEntity.IDPK = SelectedCostSheet["CI_INFO_PK"].ToValueAsString().ToIntValue();
                frcs = new FeasibleReportAndCostSheet(_userInformation);
                lstEntity = frcs.GetEntitiesByPrimaryKey(getEntity);
                if (lstEntity.Count == 0) { return; }
                activeEntity = lstEntity[0];
                frmCopyCIReference userControl = new frmCopyCIReference(_userInformation, win, activeEntity.DeepCopy<DDCI_INFO>(), OperationMode.AddNew);
                win.Title = "Copy Data";
                win.Content = userControl;
                win.Height = userControl.Height + 50;
                win.Width = userControl.Width + 10;
                win.ShowInTaskbar = true;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.WindowState = WindowState.Normal;
                win.ResizeMode = ResizeMode.NoResize;
                win.ShowDialog();
                if (userControl.Vm.Reload == true)
                {
                    SearchCostSheet();
                    SelectedCostSheet = dvCostSheetSearch[0];
                    NotifyPropertyChanged("SelectedCostSheet");
                }
                //CIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER>().DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void AllotPartNo()
        {
            int idpk = 0;
            DDCI_INFO entity = null;
            bool submit = false;
            try
            {
                if (SelectedCostSheet == null)
                {
                    Message = "CI Reference should be Selected";
                    return;
                }
                idpk = SelectedCostSheet["CI_INFO_PK"].ToValueAsString().ToIntValue();
                entity = (from row in _costSheetSearchbll.DB.DDCI_INFO
                          where (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                          && row.IDPK == idpk
                          select row).FirstOrDefault<DDCI_INFO>();
                if (entity != null)
                {
                    if (entity.ALLOT_PART_NO == null || entity.ALLOT_PART_NO == 0)
                    {
                        entity.ALLOT_PART_NO = 1;
                        entity.PART_NO_REQ_DATE = _costSheetSearchbll.ServerDateTime();
                        submit = true;
                        _costSheetSearchbll.DB.SubmitChanges();
                        Message = "Part No Requested";
                        //entity
                        //entity.PART_NO_REQ_DATE = _cost
                    }
                    else
                    {
                        if (entity.ALLOT_PART_NO == 1)
                        {
                            Message = "Request for Part No allocation already made";
                        }
                        else
                        {
                            Message = "Part No already allocated";
                        }
                    }
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                _costSheetSearchbll.DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    if (entity != null)
                    {
                        _costSheetSearchbll.DB.DDCOST_CENT_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, entity);
                    }
                }
                throw ex.LogException();
            }
        }

        private void MarkAsSubmitted()
        {
            int idpk = 0;
            DDCI_INFO entity = null;
            bool submit = false;
            try
            {
                if (SelectedCostSheet == null)
                {
                    Message = "CI Reference should be Selected";
                    return;
                }
                idpk = SelectedCostSheet["CI_INFO_PK"].ToValueAsString().ToIntValue();
                entity = (from row in _costSheetSearchbll.DB.DDCI_INFO
                          where (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                          && row.IDPK == idpk
                          select row).FirstOrDefault<DDCI_INFO>();
                if (entity != null)
                {
                    //Righ click - Mark as submitted - set 0 for Pending column - 17/12/2015
                    entity.PENDING = "0";
                    submit = true;
                    _costSheetSearchbll.DB.SubmitChanges();
                    Message = "Selected CI Reference has been marked as submitted";
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                _costSheetSearchbll.DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            }
            catch (Exception ex)
            {
                if (submit == true)
                {
                    if (entity != null)
                    {
                        _costSheetSearchbll.DB.DDCOST_CENT_MAST.Context.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, entity);
                    }
                }
                throw ex.LogException();
            }
        }
    }
}
