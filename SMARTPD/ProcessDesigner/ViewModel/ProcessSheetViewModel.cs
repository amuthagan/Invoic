using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using ProcessDesigner.BLL;
using System.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.UserControls;
using System.Collections.ObjectModel;
using ProcessDesigner.Common;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using WPF.MDI;
using ProcessDesigner.DAL;
using System.Configuration;


namespace ProcessDesigner.ViewModel
{
    class ProcessSheetViewModel : ViewModelBase
    {
        public Microsoft.Windows.Controls.DataGrid DgrdProcessSheet;
        private bool _shiftTab = false;
        private ProcessSheetModel _processSheet;
        private ProcessSheetBll bll;
        private LogViewBll _logviewBll;
        private CopyProcess copyprocess;
        private UserInformation userInformation;
        private DataRowView partNoSelectedItem = null;
        private readonly ICommand _addCommand;
        public ICommand AddCommand { get { return this._addCommand; } }
        private readonly ICommand _editCommand;
        public ICommand EditCommand { get { return this._editCommand; } }
        private readonly ICommand _saveCommand;
        public ICommand SaveCommand { get { return this._saveCommand; } }
        private readonly ICommand _deleteCommandProcessSheet;
        public ICommand DeleteCommandProcessSheet { get { return this._deleteCommandProcessSheet; } }
        private readonly ICommand _closeCommand;
        public ICommand CloseCommand { get { return this._closeCommand; } }
        private WPF.MDI.MdiChild mdiChild;

        private readonly ICommand _reNumberCommand;
        public ICommand ReNumberCommand { get { return this._reNumberCommand; } }

        private readonly ICommand _onPartNoSelectionChanged;
        public ICommand OnPartNoSelectionChanged { get { return this._onPartNoSelectionChanged; } }
        private readonly ICommand _onProcessNoSelectionChanged;
        public ICommand OnProcessNoSelectionChanged { get { return this._onProcessNoSelectionChanged; } }

        private readonly ICommand _onOperaionSelectionChanged;
        public ICommand OnOperaionSelectionChanged { get { return this._onOperaionSelectionChanged; } }

        private readonly ICommand _onUnitSelectionChanged;
        public ICommand OnUnitSelectionChanged { get { return this._onUnitSelectionChanged; } }

        private readonly ICommand _wireSpecDropDownOpened;
        public ICommand WireSpecDropDownOpened { get { return this._wireSpecDropDownOpened; } }

        private readonly ICommand _altWireSpec1DropDownOpened;
        public ICommand AltWireSpec1DropDownOpened { get { return this._altWireSpec1DropDownOpened; } }

        private readonly ICommand _altWireSpec2DropDownOpened;
        public ICommand AltWireSpec2DropDownOpened { get { return this._altWireSpec2DropDownOpened; } }

        private readonly ICommand _cheeseWtTextChanged;
        public ICommand CheeseWtTextChanged { get { return this._cheeseWtTextChanged; } }

        private readonly ICommand _selectionChangedProcessSheetCommand;
        public ICommand SelectionChangedProcessSheetCommand { get { return this._selectionChangedProcessSheetCommand; } }

        private readonly ICommand _rowEditEndingProcessSheetCommand;
        public ICommand RowEditEndingProcessSheetCommand { get { return this._rowEditEndingProcessSheetCommand; } }

        private readonly ICommand _rowEditEndingProcessIssueCommand;
        public ICommand RowEditEndingProcessIssueCommand { get { return this._rowEditEndingProcessIssueCommand; } }

        private readonly ICommand _rowEditEndingProcessCCCommand;
        public ICommand RowEditEndingProcessCCCommand { get { return this._rowEditEndingProcessCCCommand; } }


        private bool varReNumber = false;
        public Action CloseAction { get; set; }

        private DataView oldDVProcessMainDetails = null;
        private DataView oldDVProcessSheet = null;
        private DataView oldDVProcessIssue = null;
        private DataView oldDVProcessCC = null;

        private DataView newDVProcessMainDetails = null;
        private DataView newDVProcessSheet = null;
        private DataView newDVProcessIssue = null;
        private DataView newDVProcessCC = null;

        public string Sort { get; set; }

        //        public ProcessSheetBll   bllRptProcess;
        public ProcessSheetViewModel(UserInformation userinfo, WPF.MDI.MdiChild me)
        {
            _processSheet = new ProcessSheetModel();
            userInformation = userinfo;
            bll = new ProcessSheetBll(userinfo);
            copyprocess = new CopyProcess(userinfo);
            // bllRptProcess = new ProcessSheetBll(userinfo);
            this._logviewBll = new LogViewBll(userinfo);
            this.mdiChild = me;

            this._onPartNoSelectionChanged = new DelegateCommand(this.PartNoSelectionChanged);
            this._onProcessNoSelectionChanged = new DelegateCommand(this.ProcessNoSelectionChanged);
            this._wireSpecDropDownOpened = new DelegateCommand(this.WireSpec_DropDownOpened);
            this._altWireSpec1DropDownOpened = new DelegateCommand(this.AltWireSpec1_DropDownOpened);
            this._altWireSpec2DropDownOpened = new DelegateCommand(this.AltWireSpec2_DropDownOpened);
            this._cheeseWtTextChanged = new DelegateCommand(this.CheeseWt_TextChanged);

            this._selectionChangedProcessSheetCommand = new DelegateCommand<DataRowView>(this.SelectionChangedProcessSheet);
            this._rowEditEndingProcessSheetCommand = new DelegateCommand<DataRowView>(this.RowEditEndingProcessSheet);

            this._rowEditEndingProcessIssueCommand = new DelegateCommand<DataRowView>(this.RowEditEndingProcessIssue);
            this._rowEditEndingProcessCCCommand = new DelegateCommand<DataRowView>(this.RowEditEndingProcessCC);

            this._deleteCommandProcessSheet = new DelegateCommand<Microsoft.Windows.Controls.DataGrid>(this.DeleteProcessSheet);
            this._deleteCommandProcessIssue = new DelegateCommand<DataGrid>(this.DeleteProcessIssue);
            this._deleteCommandProcessCC = new DelegateCommand(this.DeleteProcessCC);
            this.copyStatusCommand = new DelegateCommand(this.CopyStatus);

            this._onOperaionSelectionChanged = new DelegateCommand(this.OperCode_SelectionChanged);
            this._onUnitSelectionChanged = new DelegateCommand(this.UnitCode_SelectionChanged);

            this._addCommand = new DelegateCommand(this.Add);
            this._editCommand = new DelegateCommand(this.Edit);
            this._saveCommand = new DelegateCommand(this.Save);
            this._closeCommand = new DelegateCommand(this.Close);
            this._reNumberCommand = new DelegateCommand(this.ReNumber);
            this.releaseProcessCommand = new DelegateCommand(this.ReleaseProcess);
            this._printCommand = new DelegateCommand(this.Print);
            this.currenprocessclickcommand = new DelegateCommand(this.CurrenProcessClick);

            this.productSearchCommand = new DelegateCommand(this.ProductSearch);
            this.costSheetSearchCommand = new DelegateCommand(this.CostSheetSearch);
            this.toolsSearchCommand = new DelegateCommand(this.ToolsSearch);

            this.controlPlanCommand = new DelegateCommand(this.ControlPlan);
            this.devlptRptCommand = new DelegateCommand(this.DevlptRpt);
            this.mfgRptCommand = new DelegateCommand(this.MfgRpt);
            this.drawingsCommand = new DelegateCommand(this.Drawings);
            this.toolScheduleCommand = new DelegateCommand(this.ToolSchedule);

            this.costSheetCommand = new DelegateCommand(this.CostSheet);
            this._windowLoadedCommand = new DelegateCommand(this.WindowLoaded);

        }

        public string ProductNo = "";
        public string ProductDesc = "";
        public ProcessSheetViewModel(UserInformation userinfo, string productNo, string productDesc, WPF.MDI.MdiChild me)
        {
            _processSheet = new ProcessSheetModel();
            userInformation = userinfo;
            bll = new ProcessSheetBll(userinfo);
            copyprocess = new CopyProcess(userinfo);
            // bllRptProcess = new ProcessSheetBll(userinfo);
            this._logviewBll = new LogViewBll(userinfo);
            this.mdiChild = me;

            this._onPartNoSelectionChanged = new DelegateCommand(this.PartNoSelectionChanged);
            this._onProcessNoSelectionChanged = new DelegateCommand(this.ProcessNoSelectionChanged);
            this._wireSpecDropDownOpened = new DelegateCommand(this.WireSpec_DropDownOpened);
            this._altWireSpec1DropDownOpened = new DelegateCommand(this.AltWireSpec1_DropDownOpened);
            this._altWireSpec2DropDownOpened = new DelegateCommand(this.AltWireSpec2_DropDownOpened);
            this._cheeseWtTextChanged = new DelegateCommand(this.CheeseWt_TextChanged);

            this._selectionChangedProcessSheetCommand = new DelegateCommand<DataRowView>(this.SelectionChangedProcessSheet);
            this._rowEditEndingProcessSheetCommand = new DelegateCommand<DataRowView>(this.RowEditEndingProcessSheet);

            this._rowEditEndingProcessIssueCommand = new DelegateCommand<DataRowView>(this.RowEditEndingProcessIssue);
            this._rowEditEndingProcessCCCommand = new DelegateCommand<DataRowView>(this.RowEditEndingProcessCC);

            this._deleteCommandProcessSheet = new DelegateCommand<Microsoft.Windows.Controls.DataGrid>(this.DeleteProcessSheet);
            this._deleteCommandProcessIssue = new DelegateCommand<DataGrid>(this.DeleteProcessIssue);
            this._deleteCommandProcessCC = new DelegateCommand(this.DeleteProcessCC);
            this.copyStatusCommand = new DelegateCommand(this.CopyStatus);

            this._onOperaionSelectionChanged = new DelegateCommand(this.OperCode_SelectionChanged);
            this._onUnitSelectionChanged = new DelegateCommand(this.UnitCode_SelectionChanged);

            this._addCommand = new DelegateCommand(this.Add);
            this._editCommand = new DelegateCommand(this.Edit);
            this._saveCommand = new DelegateCommand(this.Save);
            this._closeCommand = new DelegateCommand(this.Close);
            this._reNumberCommand = new DelegateCommand(this.ReNumber);
            this.releaseProcessCommand = new DelegateCommand(this.ReleaseProcess);
            this._printCommand = new DelegateCommand(this.Print);
            this.currenprocessclickcommand = new DelegateCommand(this.CurrenProcessClick);

            this.productSearchCommand = new DelegateCommand(this.ProductSearch);
            this.costSheetSearchCommand = new DelegateCommand(this.CostSheetSearch);
            this.toolsSearchCommand = new DelegateCommand(this.ToolsSearch);

            this.controlPlanCommand = new DelegateCommand(this.ControlPlan);
            this.devlptRptCommand = new DelegateCommand(this.DevlptRpt);
            this.mfgRptCommand = new DelegateCommand(this.MfgRpt);
            this.drawingsCommand = new DelegateCommand(this.Drawings);
            this.toolScheduleCommand = new DelegateCommand(this.ToolSchedule);

            this.costSheetCommand = new DelegateCommand(this.CostSheet);
            this._windowLoadedCommand = new DelegateCommand(this.WindowLoaded);
            ProductNo = productNo;
            ProductDesc = productDesc;
            WindowLoaded();

        }


        private readonly ICommand _windowLoadedCommand;
        public ICommand WindowLoadedCommand { get { return this._windowLoadedCommand; } }
        private void WindowLoaded()
        {
            try
            {
                StatusMessage.setStatus("Retreiving Process Sheet");

                DropdownHeaders = new ObservableCollection<DropdownColumns>
                 {               
                new DropdownColumns { ColumnName = "PART_NO", ColumnDesc = "Part Number", ColumnWidth = 120 },
                new DropdownColumns { ColumnName = "PART_DESC", ColumnDesc = "Part Description", ColumnWidth = "1*" }
                 };

                DropDownHeaderRM = new ObservableCollection<DropdownColumns>
                 {               
                new DropdownColumns { ColumnName = "RM_CODE", ColumnDesc = "RM Code", ColumnWidth = 79 },
                new DropdownColumns { ColumnName = "RM_DESC", ColumnDesc = "RM Description", ColumnWidth = "1*" }
                 };

                DropDownHeaderPNo = new ObservableCollection<DropdownColumns>
                 {                              
                new DropdownColumns { ColumnName = "ROUTE_NO", ColumnDesc = "Process No", ColumnWidth = "1*" }
                 };

                DropDownHeaderOper = new ObservableCollection<DropdownColumns>
                 {               
                new DropdownColumns { ColumnName = "OPER_CODE", ColumnDesc = "Code", ColumnWidth = 80 },
                new DropdownColumns { ColumnName = "OPER_DESC", ColumnDesc = "Operations", ColumnWidth = "1*" }
                 };
                DropDownHeaderUnit = new ObservableCollection<DropdownColumns>
                 {               
                new DropdownColumns { ColumnName = "UNIT_CODE", ColumnDesc = "Code", ColumnWidth = 80 },
                new DropdownColumns { ColumnName = "UNIT_OF_MEAS", ColumnDesc = "Unit", ColumnWidth = "1*" }
                 };

                DropDownHeaderTrans = new ObservableCollection<DropdownColumns>
                 {               
                new DropdownColumns { ColumnName = "TRANSPORT_CD", ColumnDesc = "Transport CD", ColumnWidth = 110 },
                new DropdownColumns { ColumnName = "TRANSPORT_DESC", ColumnDesc = "Transport Desc", ColumnWidth = "1*" }
                 };

                DropDownHeaderFMEArisk = new ObservableCollection<DropdownColumns>
                 {
                new DropdownColumns { ColumnName = "FMEA_RISK", ColumnDesc = "FMEA Risk", ColumnWidth = "1*" }
                 };

                DropDownHeaderCCMast = new ObservableCollection<DropdownColumns>
                 {               
                new DropdownColumns { ColumnName = "COST_CENT_CODE", ColumnDesc = "CC Code", ColumnWidth = 80 },
                new DropdownColumns { ColumnName = "COST_CENT_DESC", ColumnDesc = "Cost Center", ColumnWidth = "1*" },
                new DropdownColumns { ColumnName = "LOC_CODE", ColumnDesc = "Loc Code", ColumnWidth = 80 }
                 };

                GetRights();
                bll.GetDropDownSource(ProcessSheet);
                bll.GetProductMaster(ProcessSheet);
                bll.GetUnitDetails(ProcessSheet);
                Edit();
                if (ProductNo.IsNotNullOrEmpty())
                {
                    ProcessSheet.PART_NO = ProductNo;
                    ProcessSheet.PART_DESC = ProductDesc;
                    GetPartNoDetails();
                    mdiChild.Title = ApplicationTitle + " - Process Sheet" + ((ProcessSheet.PART_NO.IsNotNullOrEmpty()) ? " - " + ProcessSheet.PART_NO : "");
                }
                setRights();
                PartNoIsFocused = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private ObservableCollection<DropdownColumns> _dropdownHeaders = null;
        public ObservableCollection<DropdownColumns> DropdownHeaders
        {
            get { return this._dropdownHeaders; }
            set
            {
                this._dropdownHeaders = value;
                NotifyPropertyChanged("DropdownHeaders");
            }

        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderRM = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderRM
        {
            get { return this._dropDownHeaderRM; }
            set
            {
                this._dropDownHeaderRM = value;
                NotifyPropertyChanged("DropDownHeaderRM");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderPNo = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderPNo
        {
            get { return this._dropDownHeaderPNo; }
            set
            {
                this._dropDownHeaderPNo = value;
                NotifyPropertyChanged("DropDownHeaderPNo");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderOper = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderOper
        {
            get { return this._dropDownHeaderOper; }
            set
            {
                this._dropDownHeaderOper = value;
                NotifyPropertyChanged("DropDownHeaderOper");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderUnit = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderUnit
        {
            get { return this._dropDownHeaderUnit; }
            set
            {
                this._dropDownHeaderUnit = value;
                NotifyPropertyChanged("DropDownHeaderUnit");
            }
        }

        private ObservableCollection<DropdownColumns> _dropdownheadertrans = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderTrans
        {
            get { return this._dropdownheadertrans; }
            set
            {
                this._dropdownheadertrans = value;
                NotifyPropertyChanged("DropDownHeaderTrans");
            }
        }

        private ObservableCollection<DropdownColumns> _dropdownheaderfmearisk = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderFMEArisk
        {
            get { return this._dropdownheaderfmearisk; }
            set
            {
                this._dropdownheaderfmearisk = value;
                NotifyPropertyChanged("DropDownHeaderFMEArisk");
            }
        }

        private ObservableCollection<DropdownColumns> _dropdownheaderccmast = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderCCMast
        {
            get { return this._dropdownheaderccmast; }
            set
            {
                this._dropdownheaderccmast = value;
                NotifyPropertyChanged("DropDownHeaderCCMast");
            }
        }

        public ProcessSheetModel ProcessSheet
        {
            get { return this._processSheet; }
            set
            {
                this._processSheet = value;
                NotifyPropertyChanged("ProcessSheet");
            }
        }

        public DataRowView PartNoSelectedItem
        {
            get { return this.partNoSelectedItem; }
            set
            {
                this.partNoSelectedItem = value;
                NotifyPropertyChanged("PartNoSelectedItem");
            }
        }

        private DataRowView processNoSelectedItem = null;
        public DataRowView ProcessNoSelectedItem
        {
            get { return this.processNoSelectedItem; }
            set
            {
                this.processNoSelectedItem = value;
                NotifyPropertyChanged("ProcessNoSelectedItem");
            }
        }

        private DataRowView processSheetSelectedItem = null;
        public DataRowView ProcessSheetSelectedItem
        {
            get { return this.processSheetSelectedItem; }
            set
            {
                this.processSheetSelectedItem = value;
                NotifyPropertyChanged("ProcessSheetSelectedItem");
            }
        }

        private DataRowView processIssueSelectedItem = null;
        public DataRowView ProcessIssueSelectedItem
        {
            get { return this.processIssueSelectedItem; }
            set
            {
                this.processIssueSelectedItem = value;
                NotifyPropertyChanged("ProcessIssueSelectedItem");
            }
        }

        private DataRowView processCCSelectedItem = null;
        public DataRowView ProcessCCSelectedItem
        {
            get { return this.processCCSelectedItem; }
            set
            {
                this.processCCSelectedItem = value;
                NotifyPropertyChanged("ProcessCCSelectedItem");
            }
        }

        private bool _partNoIsFocused = false;
        public bool PartNoIsFocused
        {
            get { return _partNoIsFocused; }
            set
            {
                this._partNoIsFocused = value;
                NotifyPropertyChanged("PartNoIsFocused");
            }
        }

        private bool _addButtonIsEnable = false;
        public bool AddButtonIsEnable
        {
            get { return _addButtonIsEnable; }
            set
            {
                this._addButtonIsEnable = value;
                NotifyPropertyChanged("AddButtonIsEnable");
            }
        }

        private bool _editButtonIsEnable = true;
        public bool EditButtonIsEnable
        {
            get { return _editButtonIsEnable; }
            set
            {
                this._editButtonIsEnable = value;
                NotifyPropertyChanged("EditButtonIsEnable");
            }
        }

        private bool _saveButtonIsEnable = true;
        public bool SaveButtonIsEnable
        {
            get { return _saveButtonIsEnable; }
            set
            {
                this._saveButtonIsEnable = value;
                NotifyPropertyChanged("SaveButtonIsEnable");
            }
        }

        private bool _isEnableProcessNo = true;
        public bool IsEnableProcessNo
        {
            get { return _isEnableProcessNo; }
            set
            {
                this._isEnableProcessNo = value;
                NotifyPropertyChanged("IsEnableProcessNo");
            }
        }

        private bool _printButtonIsEnable = true;
        public bool PrintButtonIsEnable
        {
            get { return _printButtonIsEnable; }
            set
            {
                this._printButtonIsEnable = value;
                NotifyPropertyChanged("PrintButtonIsEnable");
            }
        }

        private bool _isEnableProcessDetails = false;
        public bool IsEnableProcessDetails
        {
            get { return _isEnableProcessDetails; }
            set
            {
                this._isEnableProcessDetails = value;
                NotifyPropertyChanged("IsEnableProcessDetails");
            }
        }

        private DataRowView _operCode_SelectedItem = null;
        public DataRowView OperCode_SelectedItem
        {
            get { return this._operCode_SelectedItem; }
            set
            {
                this._operCode_SelectedItem = value;
                NotifyPropertyChanged("OperCode_SelectedItem");
            }
        }

        private DataRowView _unit_SelectedItem = null;
        public DataRowView Unit_SelectedItem
        {
            get { return this._unit_SelectedItem; }
            set
            {
                this._unit_SelectedItem = value;
                NotifyPropertyChanged("Unit_SelectedItem");
            }
        }

        private readonly ICommand copyStatusCommand = null;
        public ICommand CopyStatusCommand { get { return this.copyStatusCommand; } }
        private void CopyStatus()
        {
            try
            {
                if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Process No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                frmCopyStatus copyStatus = new frmCopyStatus("ProcessSheet", ProcessSheet.PART_NO.Trim(), ProcessSheet.ROUTE_NO.ToString(), "", "", "");
                copyStatus.ShowDialog();
                PartNoIsFocused = true;
                PartNoSelectionChanged();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand releaseProcessCommand = null;
        public ICommand ReleaseProcessCommand { get { return this.releaseProcessCommand; } }
        private void ReleaseProcess()
        {
            //try
            //{
            //    if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
            //    {
            //        MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            //        PartNoIsFocused = true;
            //        return;
            //    }

            //    if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
            //    {
            //        MessageBox.Show(PDMsg.NotEmpty("Process No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            //        PartNoIsFocused = true;
            //        return;
            //    }

            //    PROCESS pr = (from o in bll.DB.PROCESS
            //                  where o.PART_NO == ProcessSheet.PART_NO && o.ROUTE_NO == ProcessSheet.ROUTE_NO
            //                  select o).FirstOrDefault<PROCESS>();
            //    if (pr != null)
            //    {
            //        if (MessageBox.Show("This will delete the old process and rewrite the new process. OK to continue?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            //        {
            //            bll.DB.PROCESS.DeleteOnSubmit(pr);
            //            bll.DB.SubmitChanges();
            //        }
            //    }
            //    pr = null;

            //    var q = (from ps in bll.DB.PROCESS_SHEET
            //             join pcc in bll.DB.PROCESS_CC on new { ps.PART_NO, ps.ROUTE_NO, ps.SEQ_NO } equals new { pcc.PART_NO, pcc.ROUTE_NO, pcc.SEQ_NO }
            //             where ps.PART_NO == ProcessSheet.PART_NO && ps.ROUTE_NO == ProcessSheet.ROUTE_NO
            //             select new
            //             {
            //                 ps.PART_NO,
            //                 ps.ROUTE_NO,
            //                 ps.SEQ_NO,
            //                 pcc.CC_SNO,
            //                 ps.OPN_CD,
            //                 pcc.CC_CODE
            //             }).ToList();

            //    foreach (var p in q)
            //    {

            //        PROCESS process = new PROCESS();
            //        try
            //        {
            //            process.PART_NO = p.PART_NO;
            //            process.ROUTE_NO = p.ROUTE_NO;
            //            process.OPN_SEQ = p.SEQ_NO;
            //            process.CC_SNO = p.CC_SNO;
            //            process.OPN_CD = p.OPN_CD.ToString();
            //            process.CC = p.CC_CODE;
            //            process.REP_OPN_FLAG = 1;
            //            process.ROWID = Guid.NewGuid();
            //            bll.DB.PROCESS.InsertOnSubmit(process);
            //            bll.DB.SubmitChanges();
            //            process = null;
            //        }
            //        catch (System.Data.Linq.ChangeConflictException)
            //        {
            //            bll.DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            //        }
            //        catch (Exception ex)
            //        {
            //            bll.DB.PROCESS.DeleteOnSubmit(process);
            //            throw ex.LogException();
            //        }
            //        process = null;
            //    }

            //    ABCON_PROCESS abp = (from o in bll.DB.ABCON_PROCESS
            //                         where o.PART_NO == ProcessSheet.PART_NO && o.ROUTE_NO == ProcessSheet.ROUTE_NO
            //                         select o).FirstOrDefault<ABCON_PROCESS>();
            //    if (abp != null)
            //    {
            //        var qr = (from ps in bll.DB.PROCESS_SHEET
            //                  join pcc in bll.DB.PROCESS_CC on new { ps.PART_NO, ps.ROUTE_NO, ps.SEQ_NO } equals new { pcc.PART_NO, pcc.ROUTE_NO, pcc.SEQ_NO }
            //                  where ps.PART_NO == ProcessSheet.PART_NO && ps.ROUTE_NO == ProcessSheet.ROUTE_NO
            //                  select new
            //                  {
            //                      ps.PART_NO,
            //                      ps.ROUTE_NO,
            //                      ps.SEQ_NO,
            //                      pcc.CC_SNO,
            //                      ps.OPN_CD,
            //                      pcc.CC_CODE
            //                  }).ToList();

            //        foreach (var p in qr)
            //        {

            //            ABCON_PROCESS abcprocess = new ABCON_PROCESS();
            //            try
            //            {
            //                abcprocess.PART_NO = p.PART_NO;
            //                abcprocess.ROUTE_NO = p.ROUTE_NO;
            //                abcprocess.OPN_SEQ = p.SEQ_NO;
            //                abcprocess.CC_SNO = p.CC_SNO;
            //                abcprocess.OPN_CD = p.OPN_CD.ToString();
            //                abcprocess.CC = p.CC_CODE;
            //                abcprocess.ROWID = Guid.NewGuid();
            //                bll.DB.ABCON_PROCESS.InsertOnSubmit(abcprocess);
            //                bll.DB.SubmitChanges();
            //                abcprocess = null;
            //            }
            //            catch (System.Data.Linq.ChangeConflictException)
            //            {
            //                bll.DB.ChangeConflicts.ResolveAll(System.Data.Linq.RefreshMode.KeepChanges);
            //            }
            //            catch (Exception ex)
            //            {
            //                bll.DB.ABCON_PROCESS.DeleteOnSubmit(abcprocess);
            //                throw ex.LogException();
            //            }
            //            abcprocess = null;
            //        }
            //    }
            //    abp = null;

            //    ShowInformationMessage(PDMsg.UpdatedSuccessfully);
            //    PartNoIsFocused = true;
            //    _logviewBll.SaveLog(ProcessSheet.PART_NO, "Process_sheet");

            //}
            //catch (Exception ex)
            //{
            //    throw ex.LogException();
            //}
        }

        private readonly ICommand productSearchCommand = null;
        public ICommand ProductSearchCommand { get { return this.productSearchCommand; } }
        private void ProductSearch()
        {
            try
            {
                if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Process No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                //frmProductSearch productSearch = new frmProductSearch(userInformation);
                //productSearch.Show();
                showDummy();
                MdiChild mdiProductSearch = new MdiChild();
                ProcessDesigner.frmProductSearch productSearch = new frmProductSearch(userInformation, mdiProductSearch);
                mdiProductSearch.Title = ApplicationTitle + " - Product Search";
                mdiProductSearch.Content = productSearch;
                mdiProductSearch.Height = productSearch.Height + 40;
                mdiProductSearch.Width = productSearch.Width + 20;
                mdiProductSearch.MinimizeBox = false;
                mdiProductSearch.MaximizeBox = false;
                mdiProductSearch.Resizable = false;
                if (MainMDI.IsFormAlreadyOpen("Product Search") == false)
                {
                    MainMDI.Container.Children.Add(mdiProductSearch);
                }
                else
                {
                    mdiProductSearch = (MdiChild)MainMDI.GetFormAlreadyOpened("Product Search");
                    MainMDI.SetMDI(mdiProductSearch);
                }
                PartNoIsFocused = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand costSheetSearchCommand = null;
        public ICommand CostSheetSearchCommand { get { return this.costSheetSearchCommand; } }
        private void CostSheetSearch()
        {
            try
            {
                if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Process No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                //MdiChild mdiCostSheetSearch = new MdiChild();
                //ProcessDesigner.frmCostSheetSearch costSheetSearch = new ProcessDesigner.frmCostSheetSearch(userInformation, mdiCostSheetSearch);
                //costSheetSearch.ShowDialog();
                showDummy();
                MdiChild mdiCostSheetSearch = new MdiChild();
                if (MainMDI.IsFormAlreadyOpen("Cost Sheet Search") == false)
                {
                    frmCostSheetSearch frmCostSheetSearch = new frmCostSheetSearch(userInformation, mdiCostSheetSearch);
                    mdiCostSheetSearch.Title = ApplicationTitle + " - Cost Sheet Search";
                    mdiCostSheetSearch.Content = frmCostSheetSearch;
                    mdiCostSheetSearch.Height = frmCostSheetSearch.Height + 40;
                    mdiCostSheetSearch.Width = frmCostSheetSearch.Width + 20;
                    mdiCostSheetSearch.MinimizeBox = false;
                    mdiCostSheetSearch.MaximizeBox = false;
                    mdiCostSheetSearch.Resizable = false;
                    MainMDI.Container.Children.Add(mdiCostSheetSearch);
                }
                else
                {
                    mdiCostSheetSearch = (MdiChild)MainMDI.GetFormAlreadyOpened("Cost Sheet Search");
                    //toolschedule = (frmToolSchedule_new)mdiCostSheetSearch.Content;
                    MainMDI.SetMDI(mdiCostSheetSearch);
                }
                PartNoIsFocused = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand toolsSearchCommand = null;
        public ICommand ToolsSearchCommand { get { return this.toolsSearchCommand; } }
        private void ToolsSearch()
        {
            try
            {
                if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Process No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }
                showDummy();
                MdiChild mdiToolsInfo = new MdiChild();
                if (MainMDI.IsFormAlreadyOpen("Tools Information") == false)
                {

                    ProcessDesigner.frmToolsInfo toolsinfo = new ProcessDesigner.frmToolsInfo(userInformation, mdiToolsInfo);
                    mdiToolsInfo.Title = ApplicationTitle + " - Tools Information";
                    mdiToolsInfo.Content = toolsinfo;
                    mdiToolsInfo.Height = toolsinfo.Height + 23;
                    mdiToolsInfo.Width = toolsinfo.Width + 20;
                    //mdiToolsInfo.MinimizeBox = false;
                    //mdiToolsInfo.MaximizeBox = false;
                    //mdiToolsInfo.Resizable = false;
                    MainMDI.Container.Children.Add(mdiToolsInfo);
                }
                else
                {
                    mdiToolsInfo = (MdiChild)MainMDI.GetFormAlreadyOpened("Tools Information");
                    //toolschedule = (frmToolSchedule_new)mdiCostSheetSearch.Content;
                    MainMDI.SetMDI(mdiToolsInfo);
                }

                //frmCopyStatus copyStatus = new frmCopyStatus("ProcessSheet", ProcessSheet.PART_NO.Trim(), ProcessSheet.ROUTE_NO.ToString(), "", "", "");
                //copyStatus.ShowDialog();
                PartNoIsFocused = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand controlPlanCommand = null;
        public ICommand ControlPlanCommand { get { return this.controlPlanCommand; } }
        private void ControlPlan()
        {
            try
            {
                try
                {
                    if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
                    {
                        MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    //if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                    //{
                    //    MessageBox.Show(PDMsg.NotEmpty("Route No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    //    return;
                    //}
                    showDummy();
                    MdiChild cplan = new MdiChild();
                    cplan.Title = ApplicationTitle + " - Control Plan";

                    frmPCCS cplanscreen = null;
                    if (MainMDI.IsFormAlreadyOpen("Control Plan - " + ProcessSheet.PART_NO.Trim()) == false)
                    {

                        cplanscreen = new frmPCCS(userInformation, cplan, ProcessSheet.PART_NO);
                        cplan.Content = cplanscreen;
                        cplan.Height = cplanscreen.Height + 40;
                        cplan.Width = cplanscreen.Width + 20;
                        cplan.Resizable = false;
                        cplan.MinimizeBox = true;
                        cplan.MaximizeBox = true;
                        MainMDI.Container.Children.Add(cplan);
                    }
                    else
                    {
                        cplan = new MdiChild();
                        cplan = (MdiChild)MainMDI.GetFormAlreadyOpened("Control Plan - " + ProcessSheet.PART_NO.Trim());
                        cplanscreen = (frmPCCS)cplan.Content;
                        MainMDI.SetMDI(cplan);
                    }
                    //frmPCCS pccs = new frmPCCS("ProcessSheet", MandatoryFields.PART_NO.Trim(), ProcessSheet.ROUTE_NO.ToString(), "", "", "");
                    //pccs.ShowDialog();
                }
                catch (Exception ex)
                {
                    throw ex.LogException();
                }

                //frmCopyStatus copyStatus = new frmCopyStatus("ProcessSheet", ProcessSheet.PART_NO.Trim(), ProcessSheet.ROUTE_NO.ToString(), "", "", "");
                //copyStatus.ShowDialog();
                PartNoIsFocused = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand devlptRptCommand = null;
        public ICommand DevlptRptCommand { get { return this.devlptRptCommand; } }
        private void DevlptRpt()
        {
            try
            {
                if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                showDummy();
                MdiChild devRptmdi = new MdiChild();
                devRptmdi.Title = ApplicationTitle + "Development Report";
                frmDevelopmentReport devReport = null;
                if (MainMDI.IsFormAlreadyOpen("Development Report - " + ProcessSheet.PART_NO.Trim()) == false)
                {
                    devReport = new frmDevelopmentReport(userInformation, devRptmdi, ProcessSheet.PART_NO);
                    devRptmdi.Content = devReport;
                    devRptmdi.Height = devReport.Height + 40;
                    devRptmdi.Width = devReport.Width + 20;
                    devRptmdi.Resizable = false;
                    devRptmdi.MinimizeBox = true;
                    devRptmdi.MaximizeBox = true;
                    MainMDI.Container.Children.Add(devRptmdi);
                }
                else
                {
                    devRptmdi = new MdiChild();
                    devRptmdi = (MdiChild)MainMDI.GetFormAlreadyOpened("Development Report -" + ProcessSheet.PART_NO.Trim());
                    devReport = (frmDevelopmentReport)devRptmdi.Content;
                    MainMDI.SetMDI(devRptmdi);
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private readonly ICommand mfgRptCommand = null;
        public ICommand MfgRptCommand { get { return this.mfgRptCommand; } }
        private void MfgRpt()
        {
            try
            {
                if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                showDummy();
                MdiChild mfgChild = new MdiChild();
                mfgChild.Title = ApplicationTitle + " - Manufacturing Report";
                frmManufacturingReport mfgReport = null;
                if (MainMDI.IsFormAlreadyOpen(" Manufacturing Report - " + ProcessSheet.PART_NO.Trim()) == false)
                {
                    mfgReport = new frmManufacturingReport(userInformation, mfgChild, ProcessSheet.PART_NO);
                    mfgChild.Content = mfgReport;
                    mfgChild.Height = mfgReport.Height + 40;
                    mfgChild.Width = mfgReport.Width + 20;
                    mfgChild.Resizable = false;
                    mfgChild.MinimizeBox = true;
                    mfgChild.MaximizeBox = true;
                    MainMDI.Container.Children.Add(mfgChild);
                }
                else
                {
                    mfgChild = new MdiChild();
                    mfgChild = (MdiChild)MainMDI.GetFormAlreadyOpened("Manufacturing Report -" + ProcessSheet.PART_NO.Trim());
                    mfgReport = (frmManufacturingReport)mfgChild.Content;
                    MainMDI.SetMDI(mfgChild);

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand drawingsCommand = null;
        public ICommand DrawingsCommand { get { return this.drawingsCommand; } }
        private void Drawings()
        {
            try
            {
                if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Process No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                //frmCopyStatus copyStatus = new frmCopyStatus("ProcessSheet", ProcessSheet.PART_NO.Trim(), ProcessSheet.ROUTE_NO.ToString(), "", "", "");
                //copyStatus.ShowDialog();
                showDummy();
                MdiChild drwMaster = new MdiChild();
                drwMaster.Title = ApplicationTitle + " - Drawings";
                ProcessDesigner.frmDrawings drawings = new ProcessDesigner.frmDrawings(drwMaster, userInformation, ProcessSheet.PART_NO);
                drwMaster.Content = drawings;
                drwMaster.Height = drawings.Height + 40;
                drwMaster.Width = drawings.Width + 20;
                drwMaster.MinimizeBox = false;
                drwMaster.MaximizeBox = false;
                drwMaster.Resizable = false;
                if (MainMDI.IsFormAlreadyOpen("Drawings") == false)
                {
                    MainMDI.Container.Children.Add(drwMaster);
                }
                else
                {
                    drwMaster = new MdiChild();
                    drwMaster = (MdiChild)MainMDI.GetFormAlreadyOpened("Drawings");
                    MainMDI.SetMDI(drwMaster);
                }


                PartNoIsFocused = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand toolScheduleCommand = null;
        public ICommand ToolScheduleCommand { get { return this.toolScheduleCommand; } }
        private void ToolSchedule()
        {
            try
            {
                if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Process No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                showDummy();
                MdiChild tschedule = null;
                frmToolSchedule_new toolschedule = null;
                if (MainMDI.IsFormAlreadyOpen("Tool Schedule - " + ProcessSheet.PART_NO.Trim()) == false)
                {
                    tschedule = new MdiChild();
                    tschedule.Title = ApplicationTitle + " - Tool Schedule";
                    toolschedule = new frmToolSchedule_new(userInformation, tschedule, ProcessSheet.PART_NO);
                    tschedule.Content = toolschedule;
                    tschedule.Height = toolschedule.Height + 40;
                    tschedule.Width = toolschedule.Width + 20;
                    tschedule.Resizable = false;
                    tschedule.MinimizeBox = true;
                    tschedule.MaximizeBox = true;
                    MainMDI.Container.Children.Add(tschedule);
                }
                else
                {
                    tschedule = new MdiChild();
                    tschedule = (MdiChild)MainMDI.GetFormAlreadyOpened("Tool Schedule - " + ProcessSheet.PART_NO.Trim());
                    toolschedule = (frmToolSchedule_new)tschedule.Content;
                    MainMDI.SetMDI(tschedule);
                }
                toolschedule.EditSelectedPartNo(ProcessSheet.PART_NO);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand costSheetCommand = null;
        public ICommand CostSheetCommand { get { return this.costSheetCommand; } }
        private void CostSheet()
        {
            try
            {
                if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Process No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }
                showDummy();
                frmCostSheet costSheet = new frmCostSheet(userInformation, ProcessSheet.PART_NO, ProcessSheet.ROUTE_NO.ToString().ToIntValue());
                costSheet.ShowDialog();
                PartNoIsFocused = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void PartNoSelectionChanged()
        {
            try
            {
                Sort = "asc";
                if (ProcessSheet.ActionMode == OperationMode.AddNew)
                {
                    ProcessSheet.CURRENT_PROC = true;
                }

                if (ProcessSheet.PART_NO.IsNotNullOrEmpty() && PartNoSelectedItem == null && ProcessSheet.DVProductMaster != null)
                {
                    DataView dv = ProcessSheet.DVProductMaster.ToTable().Copy().DefaultView;
                    dv.RowFilter = "PART_NO = '" + ProcessSheet.PART_NO + "'";

                    if (dv.Count > 0) PartNoSelectedItem = dv[0];
                }

                if (ProcessSheet.PART_NO.IsNotNullOrEmpty() && ProcessSheet.DVProductMaster != null && PartNoSelectedItem != null && ProcessSheet.DVProductMaster.Count > 0)
                {
                    IsEnableProcessDetails = true;
                    ProcessSheet.PART_DESC = PartNoSelectedItem["PART_DESC"].ToString();
                    oldRoutno = 0;
                    bll.GetProcessMain(ProcessSheet);

                    if (ProcessSheet.ActionMode == OperationMode.AddNew)
                    {
                        ProcessSheet.ROUTE_NO = bll.RouteNoGeneration(ProcessSheet);
                        bll.GetProcessSheetDetils(ProcessSheet);
                        ProcessSheet.DVProcessMainDetails.Table.AcceptChanges();
                        ProcessSheet.DVProcessMainDetails.RowFilter = "ROUTE_NO = '" + ProcessSheet.ROUTE_NO + "'";

                        if (ProcessSheet.DVProcessMainDetails.Count == 0)
                        {
                            DataRowView drv = ProcessSheet.DVProcessMainDetails.AddNew();
                            drv.BeginEdit();
                            drv["PART_NO"] = ProcessSheet.PART_NO;
                            drv["ROUTE_NO"] = ProcessSheet.ROUTE_NO;
                            drv.EndEdit();
                        }
                    }
                    else
                    {
                        if (ProcessSheet.DVProcessMain.Count > 0)
                        {
                            ProcessNoSelectedItem = null;
                            foreach (DataRowView drv in ProcessSheet.DVProcessMain)
                            {
                                if (drv["CURRENT_PROC"].ToBooleanAsString())
                                {
                                    ProcessNoSelectedItem = drv;
                                }
                            }

                            if (ProcessNoSelectedItem == null)
                            {
                                ProcessNoSelectedItem = ProcessSheet.DVProcessMain[0];
                            }
                            ProcessSheet.ROUTE_NO = ProcessNoSelectedItem["ROUTE_NO"].ToString().ToIntValue();
                        }
                        else
                        {
                            ProcessSheet.ROUTE_NO = 1;
                        }

                        RetreiveProcessSheet();
                        bll.RetreiveCIRefence(ProcessSheet);
                    }
                    AssignGridManager();

                    oldDVProcessMainDetails = ProcessSheet.DVProcessMainDetails.Table.Copy().DefaultView;
                    oldDVProcessSheet = ProcessSheet.DVProcessSheet.Table.Copy().DefaultView;
                    oldDVProcessIssue = ProcessSheet.DVProcessIssue.Table.Copy().DefaultView;
                    oldDVProcessCC = ProcessSheet.DVProcessCC.Table.Copy().DefaultView;

                    mdiChild.Title = ApplicationTitle + " - Process Sheet" + ((ProcessSheet.PART_NO.IsNotNullOrEmpty()) ? " - " + ProcessSheet.PART_NO : "");
                }
                else
                {
                    ClearValues();
                }



            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void GetPartNoDetails()
        {
            try
            {

                if (ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    IsEnableProcessDetails = true;
                    oldRoutno = 0;
                    bll.GetProcessMain(ProcessSheet);

                    if (ProcessSheet.DVProcessMain.Count > 0)
                    {
                        ProcessNoSelectedItem = null;
                        foreach (DataRowView drv in ProcessSheet.DVProcessMain)
                        {
                            if (drv["CURRENT_PROC"].ToBooleanAsString())
                            {
                                ProcessNoSelectedItem = drv;
                            }
                        }

                        if (ProcessNoSelectedItem == null)
                        {
                            ProcessNoSelectedItem = ProcessSheet.DVProcessMain[0];
                        }
                        ProcessSheet.ROUTE_NO = ProcessNoSelectedItem["ROUTE_NO"].ToString().ToIntValue();
                        bll.RetreiveCIRefence(ProcessSheet);
                    }
                    else
                    {
                        ProcessSheet.ROUTE_NO = 1;
                    }

                    RetreiveProcessSheet();
                    AssignGridManager();
                }
                else
                {
                    ClearValues();
                }

                oldDVProcessMainDetails = ProcessSheet.DVProcessMainDetails.Table.Copy().DefaultView;
                oldDVProcessSheet = ProcessSheet.DVProcessSheet.Table.Copy().DefaultView;
                oldDVProcessIssue = ProcessSheet.DVProcessIssue.Table.Copy().DefaultView;
                oldDVProcessCC = ProcessSheet.DVProcessCC.Table.Copy().DefaultView;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        int oldRoutno = 0;
        private void ProcessNoSelectionChanged()
        {
            try
            {
                if (ProcessSheet.ROUTE_NO != oldRoutno)
                {

                    UpdateMainGridDetail();

                    ApplayFilter();

                    if (ProcessSheet.DVProcessMainDetails.Count > 0)
                    {
                        ProcessSheet.CURRENT_PROC = (ProcessSheet.DVProcessMainDetails[0]["CURRENT_PROC"].ToString() == "1") ? true : false;
                        ProcessSheet.AJAX_CD = ProcessSheet.DVProcessMainDetails[0]["AJAX_CD"].ToString();
                        ProcessSheet.TKO_CD = ProcessSheet.DVProcessMainDetails[0]["TKO_CD"].ToString();
                        ProcessSheet.RM_CD = ProcessSheet.DVProcessMainDetails[0]["WIRE_ROD_CD"].ToString();
                        ProcessSheet.ALT_RM_CD = ProcessSheet.DVProcessMainDetails[0]["ALT_WIRE_ROD_CD"].ToString();
                        ProcessSheet.ALT_RM_CD1 = ProcessSheet.DVProcessMainDetails[0]["ALT_WIRE_ROD_CD1"].ToString();
                        ProcessSheet.WIRE_ROD_CD = ProcessSheet.DVProcessMainDetails[0]["RM_CD"].ToString();
                        ProcessSheet.ALT_WIRE_ROD_CD = ProcessSheet.DVProcessMainDetails[0]["ALT_RM_CD"].ToString();
                        ProcessSheet.ALT_WIRE_ROD_CD1 = ProcessSheet.DVProcessMainDetails[0]["ALT_RM_CD1"].ToString();
                        ProcessSheet.CHEESE_WT = ProcessSheet.DVProcessMainDetails[0]["CHEESE_WT"].ToString().ToDecimalValue();
                        ProcessSheet.CHEESE_WT_EST = ProcessSheet.DVProcessMainDetails[0]["CHEESE_WT_EST"].ToString().ToDecimalValue();
                        ProcessSheet.RM_WT = ProcessSheet.DVProcessMainDetails[0]["RM_WT"].ToString().ToDecimalValue();
                        EditButtonIsEnable = false;
                        AddButtonIsEnable = true;
                        ProcessSheet.ActionMode = OperationMode.Edit;
                        StatusMessage.setStatus("", "Edit");
                        setRights();
                    }
                    else
                    {
                        ClearValues();
                    }
                }
                oldRoutno = ProcessSheet.ROUTE_NO.ToString().ToIntValue();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void WireSpec_DropDownOpened()
        {
            try
            {
                DataView dv = ProcessSheet.AllDVWire.ToTable().DefaultView;

                if (ProcessSheet.RM_CD.IsNotNullOrEmpty())
                {
                    if (ProcessSheet.RM_CD.Substring(0, 1) == "3")
                    {
                        dv.RowFilter = "Convert(RM_CODE, 'System.String') LIKE ('5%') OR Convert(RM_CODE, 'System.String') LIKE ('0%')";
                        ProcessSheet.DVWire = dv.ToTable().DefaultView;
                    }
                    else if (ProcessSheet.RM_CD.Substring(0, 1) == "4")
                    {
                        dv.RowFilter = "Convert(RM_CODE, 'System.String') LIKE ('6%') OR Convert(RM_CODE, 'System.String') LIKE ('0%')";
                        ProcessSheet.DVWire = dv.ToTable().DefaultView;
                    }
                    else if (ProcessSheet.RM_CD.Substring(0, 1) == "1")
                    {
                        dv.RowFilter = "Convert(RM_CODE, 'System.String') LIKE ('1%') OR Convert(RM_CODE, 'System.String') LIKE ('0%')";
                        ProcessSheet.DVWire = dv.ToTable().DefaultView;
                    }
                    else
                    {
                        dv.RowFilter = "";
                        ProcessSheet.DVWire = dv.ToTable().DefaultView;
                    }
                }
                else
                {
                    dv.RowFilter = "";
                    ProcessSheet.DVWire = dv.ToTable().DefaultView;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void AltWireSpec1_DropDownOpened()
        {
            try
            {
                if (ProcessSheet.ALT_RM_CD.Substring(0, 1) == "3")
                {
                    ProcessSheet.DVAltWire1.RowFilter = "Convert(RM_CODE, 'System.String') LIKE ('5%') OR Convert(RM_CODE, 'System.String') LIKE ('0%')";
                }
                else if (ProcessSheet.ALT_RM_CD.Substring(0, 1) == "4")
                {
                    ProcessSheet.DVAltWire1.RowFilter = "Convert(RM_CODE, 'System.String') LIKE ('6%') OR Convert(RM_CODE, 'System.String') LIKE ('0%')";
                }
                else if (ProcessSheet.ALT_RM_CD.Substring(0, 1) == "1")
                {
                    ProcessSheet.DVAltWire1.RowFilter = "Convert(RM_CODE, 'System.String') LIKE ('1%') OR Convert(RM_CODE, 'System.String') LIKE ('0%')";
                }
                else
                {
                    ProcessSheet.DVAltWire1.RowFilter = "";
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void AltWireSpec2_DropDownOpened()
        {
            try
            {
                if (ProcessSheet.ALT_RM_CD1.Substring(0, 1) == "3")
                {
                    ProcessSheet.DVAltWire2.RowFilter = "Convert(RM_CODE, 'System.String') LIKE ('5%') OR Convert(RM_CODE, 'System.String') LIKE ('0%')";
                }
                else if (ProcessSheet.ALT_RM_CD1.Substring(0, 1) == "4")
                {
                    ProcessSheet.DVAltWire2.RowFilter = "Convert(RM_CODE, 'System.String') LIKE ('6%') OR Convert(RM_CODE, 'System.String') LIKE ('0%')";
                }
                else if (ProcessSheet.ALT_RM_CD1.Substring(0, 1) == "1")
                {
                    ProcessSheet.DVAltWire2.RowFilter = "Convert(RM_CODE, 'System.String') LIKE ('1%') OR Convert(RM_CODE, 'System.String') LIKE ('0%')";
                }
                else
                {
                    ProcessSheet.DVAltWire2.RowFilter = "";
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void CheeseWt_TextChanged()
        {
            try
            {
                //original
                //if (ProcessSheet.CHEESE_WT.IsNotNullOrEmpty())
                //{
                //    Nullable<decimal> rm_wei = (ProcessSheet.CHEESE_WT * 1.05m) / 100;
                //    ProcessSheet.CHEESE_WT_EST = rm_wei + ProcessSheet.CHEESE_WT;
                //    ProcessSheet.RM_WT = ProcessSheet.CHEESE_WT + (ProcessSheet.CHEESE_WT * 1.1m) / 100;
                //} // end original

                //new by nandhini
                if (ProcessSheet.CHEESE_WT.IsNotNullOrEmpty())
                {
                    Nullable<decimal> rm_wei = (ProcessSheet.CHEESE_WT * 1.05m);
                    ProcessSheet.CHEESE_WT_EST = rm_wei;
                    ProcessSheet.RM_WT = (ProcessSheet.CHEESE_WT * 1.1m);
                }
                //end new
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void RetreiveProcessSheet()
        {
            bll.GetProcessSheetDetils(ProcessSheet);
            ProcessSheet.DVProcessMainDetails.Table.AcceptChanges();
            ProcessSheet.DVProcessMainDetails.RowFilter = "ROUTE_NO = '" + ProcessSheet.ROUTE_NO + "'";

            if (ProcessSheet.DVProcessMainDetails.Count > 0)
            {
                ProcessSheet.DVProcessMainDetails.Table.AcceptChanges();
                ProcessSheet.DVProcessMainDetails.RowFilter = "ROUTE_NO = '" + Convert.ToInt16(ProcessSheet.DVProcessMainDetails[0]["ROUTE_NO"]) + "'";

                ProcessSheet.ROUTE_NO = Convert.ToInt16(ProcessSheet.DVProcessMainDetails[0]["ROUTE_NO"]);
                ProcessSheet.CURRENT_PROC = (ProcessSheet.DVProcessMainDetails[0]["CURRENT_PROC"].ToString() == "1") ? true : false;
                ProcessSheet.AJAX_CD = ProcessSheet.DVProcessMainDetails[0]["AJAX_CD"].ToString();
                ProcessSheet.TKO_CD = ProcessSheet.DVProcessMainDetails[0]["TKO_CD"].ToString();
                ProcessSheet.RM_CD = ProcessSheet.DVProcessMainDetails[0]["WIRE_ROD_CD"].ToString();
                ProcessSheet.ALT_RM_CD = ProcessSheet.DVProcessMainDetails[0]["ALT_WIRE_ROD_CD"].ToString();
                ProcessSheet.ALT_RM_CD1 = ProcessSheet.DVProcessMainDetails[0]["ALT_WIRE_ROD_CD1"].ToString();
                ProcessSheet.WIRE_ROD_CD = ProcessSheet.DVProcessMainDetails[0]["RM_CD"].ToString();
                ProcessSheet.ALT_WIRE_ROD_CD = ProcessSheet.DVProcessMainDetails[0]["ALT_RM_CD"].ToString();
                ProcessSheet.ALT_WIRE_ROD_CD1 = ProcessSheet.DVProcessMainDetails[0]["ALT_RM_CD1"].ToString();
                ProcessSheet.CHEESE_WT = ProcessSheet.DVProcessMainDetails[0]["CHEESE_WT"].ToString().ToDecimalValue();
                //original
                ProcessSheet.CHEESE_WT_EST = ProcessSheet.DVProcessMainDetails[0]["CHEESE_WT_EST"].ToString().ToDecimalValue(); //uncomment by me
                ProcessSheet.RM_WT = ProcessSheet.DVProcessMainDetails[0]["RM_WT"].ToString().ToDecimalValue();
                //original end
                //new 
                if (ProcessSheet.DVProcessMainDetails[0]["CHEESE_WT_EST"].ToString().ToDecimalValue() != 0)
                {
                    ProcessSheet.CHEESE_WT_EST = ProcessSheet.DVProcessMainDetails[0]["CHEESE_WT_EST"].ToString().ToDecimalValue();
                }
                else { ProcessSheet.CHEESE_WT_EST = ProcessSheet.CHEESE_WT_EST; }
                if (ProcessSheet.DVProcessMainDetails[0]["RM_WT"].ToString().ToDecimalValue() != 0)
                {
                    ProcessSheet.RM_WT = ProcessSheet.DVProcessMainDetails[0]["RM_WT"].ToString().ToDecimalValue();
                }
                else { ProcessSheet.RM_WT = ProcessSheet.RM_WT; }
                //new end 
                //end new
                //new
                //ProcessSheet.CHEESE_WT_EST = ProcessSheet.CHEESE_WT_EST;
                //ProcessSheet.RM_WT = ProcessSheet.RM_WT;
                //new end
                EditButtonIsEnable = false;
                AddButtonIsEnable = true;
                ProcessSheet.ActionMode = OperationMode.Edit;
                StatusMessage.setStatus("", "Edit");
            }
            else
            {
                ProcessSheet.ROUTE_NO = bll.RouteNoGeneration(ProcessSheet);
                ProcessSheet.CURRENT_PROC = true;
                ProcessSheet.AJAX_CD = "";
                ProcessSheet.TKO_CD = "";
                ProcessSheet.RM_CD = "";
                ProcessSheet.ALT_RM_CD = "";
                ProcessSheet.ALT_RM_CD1 = "";
                ProcessSheet.WIRE_ROD_CD = "";
                ProcessSheet.ALT_WIRE_ROD_CD = "";
                ProcessSheet.ALT_WIRE_ROD_CD1 = "";
                ProcessSheet.CHEESE_WT = 0;
                ProcessSheet.CHEESE_WT_EST = 0;
                ProcessSheet.RM_WT = 0;
                EditButtonIsEnable = true;
                AddButtonIsEnable = false;
                ProcessSheet.ActionMode = OperationMode.AddNew;
            }
            setRights();

            if (ProcessSheet.DVProcessMainDetails.Count == 0)
            {
                DataRowView drv = ProcessSheet.DVProcessMainDetails.AddNew();
                drv.BeginEdit();
                drv["PART_NO"] = ProcessSheet.PART_NO;
                drv["ROUTE_NO"] = ProcessSheet.ROUTE_NO;
                drv.EndEdit();
            }

        }

        private void ClearValues()
        {
            IsEnableProcessDetails = false;
            ProcessSheet.PART_NO = "";
            ProcessSheet.PART_DESC = "";
            ProcessSheet.ROUTE_NO = null;
            ProcessSheet.CURRENT_PROC = false;
            ProcessSheet.AJAX_CD = "";
            ProcessSheet.TKO_CD = "";
            ProcessSheet.RM_CD = "";
            ProcessSheet.ALT_RM_CD = "";
            ProcessSheet.ALT_RM_CD1 = "";
            ProcessSheet.WIRE_ROD_CD = "";
            ProcessSheet.ALT_WIRE_ROD_CD = "";
            ProcessSheet.ALT_WIRE_ROD_CD1 = "";
            ProcessSheet.CHEESE_WT = 0;
            ProcessSheet.CHEESE_WT_EST = 0;
            ProcessSheet.RM_WT = 0;
            oldRoutno = 0;

            ProcessSheet.DVProcessMain = null;
            ProcessSheet.DVProcessMainDetails = null;
            ProcessSheet.DVProcessSheet = null;
            ProcessSheet.DVProcessCC = null;
            ProcessSheet.DVProcessIssue = null;

            oldDVProcessMainDetails = null;
            oldDVProcessSheet = null;
            oldDVProcessIssue = null;
            oldDVProcessCC = null;
            PartNoIsFocused = true;
            mdiChild.Title = ApplicationTitle + " - Process Sheet" + ((ProcessSheet.PART_NO.IsNotNullOrEmpty()) ? " - " + ProcessSheet.PART_NO : "");
        }

        private void AssignGridManager()
        {
            try
            {
                bll = new ProcessSheetBll(userInformation);
                if (ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    bll.GetGridDetails(ProcessSheet);

                    ApplayFilter();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private void ApplayFilter()
        {
            try
            {
                if (ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                {
                    ProcessSheet.DVProcessMainDetails.Table.AcceptChanges();
                    ProcessSheet.DVProcessMainDetails.RowFilter = "ROUTE_NO = '" + ProcessSheet.ROUTE_NO + "'";

                    if (ProcessSheet.DVProcessMainDetails.Count == 0)
                    {
                        DataRowView drv = ProcessSheet.DVProcessMainDetails.AddNew();
                        drv.BeginEdit();
                        drv["PART_NO"] = ProcessSheet.PART_NO;
                        drv["ROUTE_NO"] = ProcessSheet.ROUTE_NO;
                        drv.EndEdit();
                    }

                    if (ProcessSheet.DVProcessSheet == null) return;

                    ProcessSheet.DVProcessSheet.Table.AcceptChanges();
                    ProcessSheet.DVProcessSheet.RowFilter = "ROUTE_NO = '" + ProcessSheet.ROUTE_NO + "'";
                    //ProcessSheet.DVProcessSheet.Sort = "SORTCOL";

                    if (ProcessSheet.DVProcessSheet.Count == 0 || !ProcessSheetHaveEmptyRow())
                    {
                        DataRowView drv = ProcessSheet.DVProcessSheet.AddNew();
                        drv.BeginEdit();
                        drv["SORTCOL"] = 9999;
                        drv["PART_NO"] = ProcessSheet.PART_NO;
                        drv["ROUTE_NO"] = ProcessSheet.ROUTE_NO;
                        drv.EndEdit();
                    }

                    if (ProcessSheet.DVProcessSheet.Count > 0) ProcessSheetSelectedItem = ProcessSheet.DVProcessSheet[0];

                    ProcessSheet.DVProcessIssue.Table.AcceptChanges();
                    ProcessSheet.DVProcessIssue.RowFilter = "ROUTE_NO = '" + ProcessSheet.ROUTE_NO + "'";
                    //ProcessSheet.DVProcessIssue.Sort = "SORTCOL";

                    if (ProcessSheet.DVProcessIssue.Count == 0 || !ProcessIssueHaveEmptyRow())
                    {
                        DataRowView drv = ProcessSheet.DVProcessIssue.AddNew();
                        drv.BeginEdit();
                        drv["ISSUE_NONUMERIC"] = 999;
                        drv["SORTCOL"] = 999;
                        drv["PART_NO"] = ProcessSheet.PART_NO;
                        drv["ROUTE_NO"] = ProcessSheet.ROUTE_NO;
                        drv.EndEdit();
                    }

                    if (ProcessSheet.DVProcessIssue.Count > 0) ProcessIssueSelectedItem = ProcessSheet.DVProcessIssue[0];

                    if (ProcessSheetSelectedItem != null && ProcessSheetSelectedItem["SEQ_NO"].IsNotNullOrEmpty())
                    {
                        ProcessSheet.DVProcessCC.Table.AcceptChanges();
                        ProcessSheet.DVProcessCC.RowFilter = "ROUTE_NO = '" + ProcessSheet.ROUTE_NO + "' AND SEQ_NO = '" + ProcessSheetSelectedItem["SEQ_NO"] + "'";

                        if (ProcessSheet.DVProcessCC.Count == 0 || !ProcessCCHaveEmptyRow())
                        {
                            DataRowView drv = ProcessSheet.DVProcessCC.AddNew();
                            drv.BeginEdit();
                            drv["SEQ_NO"] = ProcessSheetSelectedItem["SEQ_NO"].ToString();
                            drv["PART_NO"] = ProcessSheet.PART_NO;
                            drv["ROUTE_NO"] = ProcessSheet.ROUTE_NO;
                            drv.EndEdit();
                        }

                        if (ProcessSheet.DVProcessCC.Count > 0) ProcessCCSelectedItem = ProcessSheet.DVProcessCC[0];
                    }
                    else
                    {
                        ProcessSheet.DVProcessCC.Table.AcceptChanges();
                        ProcessSheet.DVProcessCC.RowFilter = "1 = 2";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Add()
        {
            try
            {
                if (!AddButtonIsEnable) return;
                PartNoIsFocused = true;
                if (IsChangesMade())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        return;
                    }
                }

                AddButtonIsEnable = false;
                EditButtonIsEnable = true;
                ProcessSheet.ActionMode = OperationMode.AddNew;
                StatusMessage.setStatus("", "Add");
                setRights();
                ClearValues();
                bll.GetProductMaster(ProcessSheet);
                PartNoIsFocused = true;
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
                if (!EditButtonIsEnable) return;
                PartNoIsFocused = true;
                if (IsChangesMade())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        return;
                    }
                }

                AddButtonIsEnable = true;
                EditButtonIsEnable = false;
                ProcessSheet.ActionMode = OperationMode.Edit;
                StatusMessage.setStatus("", "Edit");
                setRights();
                ClearValues();
                bll.GetProductMaster(ProcessSheet);

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
                if (!SaveButtonIsEnable) return;

                PartNoIsFocused = true;

                if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Process No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                ProcessSheet.DVProcessSheet.Table.AcceptChanges();
                ProcessSheet.DVProcessCC.Table.AcceptChanges();
                ProcessSheet.DVProcessIssue.Table.AcceptChanges();

                DataView dv = ProcessSheet.DVProcessSheet.Table.Copy().DefaultView;

                dv.RowFilter = "ROUTE_NO = '" + ProcessSheet.ROUTE_NO + "' AND Convert(SEQ_NO, 'System.String') <> ''";

                if (dv.Count == 0)
                {
                    MessageBox.Show(PDMsg.NotEmpty("Process Sheet Details"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }


                if (IsDuplicateCurrentProcess())
                {
                    ShowInformationMessage("Already Current Process has been selected for Process No");
                    ProcessSheet.CURRENT_PROC = false;
                    PartNoIsFocused = true;
                    return;
                }

                bll = new ProcessSheetBll(userInformation);

                Progress.ProcessingText = PDMsg.ProgressUpdateText;
                Progress.Start();

                UpdateProcessSheet();

                //if (IsNeedsToRenumbering())
                //{
                //    varReNumber = true;

                //    dv.Sort = "SORTCOL";
                //    copyprocess = new CopyProcess(userInformation);
                //    int i = 0;
                //    foreach (DataRowView drv in dv)
                //    {
                //        copyprocess.ReNumberProcessSheet(ProcessSheet.PART_NO, ProcessSheet.ROUTE_NO.ToString().ToIntValue(), drv["SEQ_NO"].ToString().ToIntValue(), (1000 + ((i + 1) * 10)), varReNumber);
                //        i++;
                //    }

                //    copyprocess = new CopyProcess(userInformation);
                //    i = 0;
                //    foreach (DataRowView drv in dv)
                //    {
                //        copyprocess.ReNumberProcessSheet(ProcessSheet.PART_NO, ProcessSheet.ROUTE_NO.ToString().ToIntValue(), (1000 + ((i + 1) * 10)), ((i + 1) * 10), varReNumber);
                //        i++;
                //    }

                //    AssignGridManager();
                //}

                string partNo = ProcessSheet.PART_NO;
                int routeNo = ProcessSheet.ROUTE_NO.ToString().ToIntValue();

                bll.GetProcessMain(ProcessSheet);

                oldDVProcessMainDetails = ProcessSheet.DVProcessMainDetails.Table.Copy().DefaultView;
                oldDVProcessSheet = ProcessSheet.DVProcessSheet.Table.Copy().DefaultView;
                oldDVProcessIssue = ProcessSheet.DVProcessIssue.Table.Copy().DefaultView;
                oldDVProcessCC = ProcessSheet.DVProcessCC.Table.Copy().DefaultView;

                if (IsNeedsToRenumbering())
                {
                    //string conStr = ConfigurationManager.ConnectionStrings["ENGGDB"].ConnectionString;
                    System.Resources.ResourceManager myManager;
                    myManager = new System.Resources.ResourceManager(typeof(ProcessDesigner.Properties.Resources));
                    string conStr = myManager.GetString("ConnectionString");

                    DataAccessLayer dal = new DataAccessLayer(conStr);
                    dal.Renumber_Processsheet(partNo, routeNo);


                    AssignGridManager();
                }

                Progress.End();
                if (ProcessSheet.ActionMode == OperationMode.AddNew)
                {
                    MessageBox.Show(PDMsg.SavedSuccessfully, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    _logviewBll.SaveLog(ProcessSheet.PART_NO, "Process_sheet");
                }
                else
                {
                    MessageBox.Show(PDMsg.UpdatedSuccessfully, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    _logviewBll.SaveLog(ProcessSheet.PART_NO, "Process_sheet");
                }
                PartNoIsFocused = true;

                //ClearValues();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool IsNeedsToRenumbering()
        {
            try
            {
                DataView dv = ProcessSheet.DVProcessSheet.Table.Copy().DefaultView;
                dv.RowFilter = "Convert(SEQ_NO, 'System.String') <> '' AND Convert(ROUTE_NO, 'System.String') = '" + ProcessSheet.ROUTE_NO + "'";

                for (int i = 1; i <= dv.Count; i++)
                {
                    if (dv[i - 1].IsNotNullOrEmpty() && i * 10 != dv[i - 1]["SEQ_NO"].ToString().ToDecimalValue()) return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool UpdateProcessSheet()
        {
            try
            {
                UpdateMainGridDetail();
                bll.UpdateProcessSheet(ProcessSheet);
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void UpdateMainGridDetail()
        {
            ProcessSheet.DVProcessMainDetails[0]["CURRENT_PROC"] = (ProcessSheet.CURRENT_PROC) ? 1 : 0;
            ProcessSheet.DVProcessMainDetails[0]["AJAX_CD"] = ProcessSheet.AJAX_CD;
            ProcessSheet.DVProcessMainDetails[0]["TKO_CD"] = ProcessSheet.TKO_CD;
            ProcessSheet.DVProcessMainDetails[0]["RM_CD"] = ProcessSheet.WIRE_ROD_CD;
            ProcessSheet.DVProcessMainDetails[0]["ALT_RM_CD"] = ProcessSheet.ALT_WIRE_ROD_CD;
            ProcessSheet.DVProcessMainDetails[0]["ALT_RM_CD1"] = ProcessSheet.ALT_WIRE_ROD_CD1;
            ProcessSheet.DVProcessMainDetails[0]["WIRE_ROD_CD"] = ProcessSheet.RM_CD;
            ProcessSheet.DVProcessMainDetails[0]["ALT_WIRE_ROD_CD"] = ProcessSheet.ALT_RM_CD;
            ProcessSheet.DVProcessMainDetails[0]["ALT_WIRE_ROD_CD1"] = ProcessSheet.ALT_RM_CD1;
            ProcessSheet.DVProcessMainDetails[0]["CHEESE_WT"] = ProcessSheet.CHEESE_WT;
            ProcessSheet.DVProcessMainDetails[0]["CHEESE_WT_EST"] = ProcessSheet.CHEESE_WT_EST;
            ProcessSheet.DVProcessMainDetails[0]["RM_WT"] = ProcessSheet.RM_WT;
        }

        private bool CheckCurrentProcess()
        {
            PROCESS_MAIN pm = (from o in bll.DB.PROCESS_MAIN
                               where o.PART_NO == ProcessSheet.PART_NO && o.ROUTE_NO != ProcessSheet.ROUTE_NO && o.CURRENT_PROC == 1 && ((Convert.ToBoolean(Convert.ToInt16(o.DELETE_FLAG)) == false) || (o.DELETE_FLAG == null))
                               select o).FirstOrDefault<PROCESS_MAIN>();

            if (pm != null)
            {
                ShowInformationMessage("Already Current Process has been selected for Process No " + pm.ROUTE_NO);
                ProcessSheet.CURRENT_PROC = false;
                PartNoIsFocused = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Close()
        {
            if (IsChangesMade())
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    Save();
                    //new by me
                    CloseAction();
                    return; //old

                }
                //else
                //{
                //    CloseAction();
                //}
            }
            

            if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
            {
                CloseAction();
            }
            StatusMessage.setStatus("Ready", "");
        }

        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsChangesMade())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        return;
                    }
                }

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

        private bool IsChangesMade()
        {
            try
            {
                bool result = true;

                if (oldDVProcessMainDetails != null && oldDVProcessSheet != null && oldDVProcessIssue != null && oldDVProcessCC != null)
                {
                    oldDVProcessMainDetails.RowFilter = "Convert(ROUTE_NO, 'System.String') <> ''";
                    oldDVProcessSheet.RowFilter = "Convert(SEQ_NO, 'System.String') <> ''";
                    oldDVProcessIssue.RowFilter = "Convert(ISSUE_NO, 'System.String') <> ''";
                    oldDVProcessCC.RowFilter = "Convert(COST_CENT_CODE, 'System.String') <> ''";

                    UpdateMainGridDetail();
                    newDVProcessMainDetails = ProcessSheet.DVProcessMainDetails.Table.Copy().DefaultView;
                    newDVProcessSheet = ProcessSheet.DVProcessSheet.Table.Copy().DefaultView;
                    newDVProcessIssue = ProcessSheet.DVProcessIssue.Table.Copy().DefaultView;
                    newDVProcessCC = ProcessSheet.DVProcessCC.Table.Copy().DefaultView;

                    newDVProcessMainDetails.RowFilter = "Convert(ROUTE_NO, 'System.String') <> ''";
                    newDVProcessSheet.RowFilter = "Convert(SEQ_NO, 'System.String') <> ''";
                    newDVProcessIssue.RowFilter = "Convert(ISSUE_NO, 'System.String') <> ''";
                    newDVProcessCC.RowFilter = "Convert(COST_CENT_CODE, 'System.String') <> ''";

                    result = newDVProcessMainDetails.IsEqual(oldDVProcessMainDetails);
                    if (result)
                    {
                        result = newDVProcessSheet.IsEqual(oldDVProcessSheet);
                    }

                    if (result)
                    {
                        result = newDVProcessIssue.IsEqual(oldDVProcessIssue);
                    }

                    if (result)
                    {
                        result = newDVProcessCC.IsEqual(oldDVProcessCC);
                    }
                }

                return !result;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool IsDuplicateSeqNo(string seqNo)
        {
            // ProcessSheet.DVProcessSheet.Table.AcceptChanges();
            DataView dv = ProcessSheet.DVProcessSheet.ToTable().DefaultView;
            dv.RowFilter = "ROUTE_NO = '" + ProcessSheet.ROUTE_NO + "' AND Convert(SEQ_NO, 'System.String') = '" + seqNo + "'";

            if (dv.Count > 1)
            {
                return true;
            }
            return false;
        }

        private bool IsDuplicateCCCode(string ccCode)
        {
            //ProcessSheet.DVProcessCC.Table.AcceptChanges();
            DataView dv = ProcessSheet.DVProcessCC.ToTable().DefaultView;
            dv.RowFilter = "ROUTE_NO = '" + ProcessSheet.ROUTE_NO + "' AND SEQ_NO = '" + ProcessSheetSelectedItem["SEQ_NO"] + "' AND Convert(COST_CENT_CODE, 'System.String') = '" + ccCode + "'";

            if (dv.Count > 1)
            {
                return true;
            }
            return false;
        }

        private bool IsDuplicateIssueNo(string issueNo)
        {
            // By Jeyan Start -> to check the systen to verify issue no duplicate incase if it is 01 or 1
            int issueNumber = 0;
            if (issueNo.Trim().Length > 0)
            {
                issueNumber = Convert.ToInt32(issueNo);
            }
            // By Jeyan End 
            DataView dv = ProcessSheet.DVProcessIssue.ToTable().DefaultView;
            //dv.RowFilter = "ROUTE_NO = '" + ProcessSheet.ROUTE_NO + "' AND Convert(ISSUE_NO, 'System.String') = '" + issueNo + "'"; // Commented by Jeyan Start -> to check the systen to verify issue no duplicate incase if it is 01 or 1
            dv.RowFilter = "ROUTE_NO = '" + ProcessSheet.ROUTE_NO + "' AND Convert(ISSUE_NO, 'System.String') = '" + issueNo + "' OR Convert(ISSUE_NO, 'System.String') = '" + issueNumber + "'";

            if (dv.Count > 1)
            {
                return true;
            }
            return false;
        }

        private bool IsDuplicateCurrentProcess()
        {
            UpdateMainGridDetail();
            DataView dv = ProcessSheet.DVProcessMainDetails.Table.Copy().DefaultView;
            dv.RowFilter = "Convert(CURRENT_PROC, 'System.String') = '1'";

            if (dv.Count > 1)
            {
                return true;
            }
            return false;
        }

        private void SelectionChangedProcessSheet(DataRowView selectedItem)
        {
            try
            {
                if (ProcessSheetSelectedItem != null)
                {
                    if (ProcessSheet.ROUTE_NO.IsNotNullOrEmpty() && ProcessSheetSelectedItem["SEQ_NO"].IsNotNullOrEmpty())
                    {
                        ProcessSheet.DVProcessCC.Table.AcceptChanges();
                        ProcessSheet.DVProcessCC.RowFilter = "ROUTE_NO = '" + ProcessSheet.ROUTE_NO + "' AND SEQ_NO = '" + ProcessSheetSelectedItem["SEQ_NO"] + "'";

                        if (ProcessSheet.DVProcessCC.Count == 0 || !ProcessCCHaveEmptyRow())
                        {
                            DataRowView drv = ProcessSheet.DVProcessCC.AddNew();
                            drv.BeginEdit();
                            drv["SEQ_NO"] = ProcessSheetSelectedItem["SEQ_NO"].ToString();
                            drv["PART_NO"] = ProcessSheet.PART_NO;
                            drv["ROUTE_NO"] = ProcessSheet.ROUTE_NO;
                            drv.EndEdit();
                        }

                        if (ProcessSheet.DVProcessCC.Count > 0) ProcessCCSelectedItem = ProcessSheet.DVProcessCC[0];
                    }
                    else
                    {
                        ProcessSheet.DVProcessCC.Table.AcceptChanges();
                        ProcessSheet.DVProcessCC.RowFilter = "1 = 2";
                    }

                    if (ProcessSheetSelectedItem["OPER_CODE"].IsNotNullOrEmpty())
                    {
                        ProcessSheet.DVCCMaster.RowFilter = "";
                    }
                    else
                    {
                        ProcessSheet.DVCCMaster.RowFilter = "1 = 2";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void RowEditEndingProcessSheet(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    if (selecteditem["OPER_CODE"].IsNotNullOrEmpty())
                    {
                        if (ProcessSheet.DVProcessSheet.Count == 0 || !ProcessSheetHaveEmptyRow())
                        {
                            DataRowView drv = ProcessSheet.DVProcessSheet.AddNew();
                            drv.BeginEdit();
                            drv["SORTCOL"] = 9999;
                            drv["PART_NO"] = ProcessSheet.PART_NO;
                            drv["ROUTE_NO"] = ProcessSheet.ROUTE_NO;
                            drv.EndEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool ProcessSheetHaveEmptyRow()
        {

            foreach (DataRowView drv in ProcessSheet.DVProcessSheet)
            {
                if (!drv["OPER_CODE"].IsNotNullOrEmpty()) return true;
            }
            return false;
        }

        private void OperCode_SelectionChanged()
        {
            try
            {
                if (ProcessSheetSelectedItem != null)
                {
                    ProcessSheetSelectedItem.BeginEdit();
                    if (OperCode_SelectedItem != null)
                    {
                        ProcessSheetSelectedItem["OPN_DESC"] = OperCode_SelectedItem["OPER_DESC"].ToString();
                    }
                    else
                    {
                        ProcessSheetSelectedItem["OPN_DESC"] = "";
                    }
                    ProcessSheetSelectedItem.EndEdit();

                    if (ProcessSheetSelectedItem["OPER_CODE"].IsNotNullOrEmpty())
                    {
                        ProcessSheet.DVCCMaster.RowFilter = "";
                    }
                    else
                    {
                        ProcessSheet.DVCCMaster.RowFilter = "1 = 2";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void UnitCode_SelectionChanged()
        {
            try
            {
                if (ProcessSheetSelectedItem != null)
                {
                    ProcessSheetSelectedItem.BeginEdit();
                    if (Unit_SelectedItem != null)
                    {
                        ProcessSheetSelectedItem["UNIT_OF_MEASURE"] = Unit_SelectedItem["UNIT_OF_MEAS"].ToString();
                    }
                    ProcessSheetSelectedItem.EndEdit();

                    //if (ProcessSheetSelectedItem["OPER_CODE"].IsNotNullOrEmpty())
                    //{
                    //    ProcessSheet.UnitDetails.RowFilter = "";
                    //}
                    //else
                    //{
                    //    ProcessSheet.UnitDetails.RowFilter = "1 = 2";
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void RowEditEndingProcessIssue(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    if (selecteditem["ISSUE_NO"].IsNotNullOrEmpty())
                    {
                        if (ProcessSheet.DVProcessIssue.Count == 0 || !ProcessIssueHaveEmptyRow())
                        {
                            DataRowView drv = ProcessSheet.DVProcessIssue.AddNew();
                            drv.BeginEdit();
                            drv["ISSUE_NONUMERIC"] = 999;
                            drv["SORTCOL"] = 999;
                            drv["PART_NO"] = ProcessSheet.PART_NO;
                            drv["ROUTE_NO"] = ProcessSheet.ROUTE_NO;
                            drv.EndEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool ProcessIssueHaveEmptyRow()
        {

            foreach (DataRowView drv in ProcessSheet.DVProcessIssue)
            {
                if (!drv["ISSUE_NO"].IsNotNullOrEmpty()) return true;
            }
            return false;
        }

        private void RowEditEndingProcessCC(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    if (selecteditem["COST_CENT_CODE"].IsNotNullOrEmpty() && ProcessSheetSelectedItem != null)
                    {
                        if (ProcessSheet.DVProcessCC.Count == 0 || !ProcessCCHaveEmptyRow())
                        {
                            DataRowView drv = ProcessSheet.DVProcessCC.AddNew();
                            drv.BeginEdit();
                            drv["SEQ_NO"] = ProcessSheetSelectedItem["SEQ_NO"].ToString();
                            drv["PART_NO"] = ProcessSheet.PART_NO;
                            drv["ROUTE_NO"] = ProcessSheet.ROUTE_NO;
                            drv.EndEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool ProcessCCHaveEmptyRow()
        {

            foreach (DataRowView drv in ProcessSheet.DVProcessCC)
            {
                if (!drv["COST_CENT_CODE"].IsNotNullOrEmpty()) return true;
            }
            return false;
        }

        string oldsno = "";
        public void OnBeginningEditProcessSheet(object sender, Microsoft.Windows.Controls.DataGridBeginningEditEventArgs e)
        {
            DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            string columnName = e.Column.SortMemberPath;

            string rowID = selecteditem["ROWID"].ToString();

            if (columnName == "SEQ_NO")
            {
                oldsno = selecteditem["SEQ_NO"].ToString().Trim();
            }
        }


        public void OnCellEditEndingProcessSheet(object sender, Microsoft.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            TextBox tb = e.EditingElement as TextBox;
            DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            string columnName = e.Column.SortMemberPath;

            Microsoft.Windows.Controls.DataGrid datagrid = sender as Microsoft.Windows.Controls.DataGrid;

            if (tb != null)
            {
                selecteditem.BeginEdit();
                selecteditem[columnName] = tb.Text;
                selecteditem.EndEdit();
            }

            string seqno = selecteditem["SEQ_NO"].ToString().ToUpper().Trim();
            string rowID = selecteditem["ROWID"].ToString();
            if (columnName == "NET_WEIGHT")
            {
                if (selecteditem["NET_WEIGHT"].ToValueAsString().ToDecimalValue() != 0)
                {
                    if (datagrid.SelectedIndex > -1)
                    {
                        selecteditem.BeginEdit();
                        // selecteditem["GROSS_WEIGHT"] = ProcessSheet.DVProcessSheet[datagrid.SelectedIndex]["NET_WEIGHT"];
                        ProcessSheet.DVProcessSheet[datagrid.SelectedIndex + 1]["GROSS_WEIGHT"] = ProcessSheet.DVProcessSheet[datagrid.SelectedIndex]["NET_WEIGHT"];
                        selecteditem.EndEdit();
                    }
                }
            }
            if (columnName == "SEQ_NO" && seqno.IsNotNullOrEmpty())
            {
                if (!seqno.IsNumeric())
                {
                    ShowInformationMessage("Sequence No should be numeric value");
                    tb.Text = oldsno;
                    _shiftTab = false;
                    return;
                }

                if (seqno.ToDecimalValue() == 0)
                {
                    ShowInformationMessage("Sequence No cannot be zero");
                    tb.Text = oldsno;
                    _shiftTab = false;
                    return;
                }

                if (IsDuplicateSeqNo(seqno))
                {
                    ShowInformationMessage("Duplicate Seq no has been Entered");
                    tb.Text = oldsno;
                    _shiftTab = false;
                    return;
                }

                selecteditem["SORTCOL"] = seqno.ToDecimalValue();

                //if (rowID.IsNotNullOrEmpty() && oldsno.IsNotNullOrEmpty() && seqno != oldsno)
                //{
                //    if (MessageBox.Show("Seq No will be Renumbered permanently. Do you want to Continue?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                //    {
                //        copyprocess = new CopyProcess(userInformation);
                //        //copyprocess.ReNumberProcessSheet(ProcessSheet.PART_NO, ProcessSheet.ROUTE_NO.ToString().ToIntValue(), oldsno.ToDecimalValue(), seqno.ToDecimalValue(), varReNumber);

                //        // if (IsNeedsToRenumbering())
                //        //{
                //            //string conStr = ConfigurationManager.ConnectionStrings["ENGGDB"].ConnectionString;
                //            // Added by Jeyan - Renumber 
                //            System.Resources.ResourceManager myManager;
                //            myManager = new System.Resources.ResourceManager(typeof(ProcessDesigner.Properties.Resources));
                //            string conStr = myManager.GetString("ConnectionString");

                //            DataAccessLayer dal = new DataAccessLayer(conStr);
                //            dal.Renumber_Processsheet(ProcessSheet.PART_NO, ProcessSheet.ROUTE_NO.ToString().ToIntValue());
                //            AssignGridManager();
                //        //}
                //        AssignGridManager();
                //        _shiftTab = false;
                //        return;
                //    }
                //    else
                //    {
                //        tb.Text = oldsno;
                //        _shiftTab = false;
                //        return;

                //    }
                //}


                if (ProcessSheet.DVProcessCC.Count == 0)
                {
                    DataRowView drv = ProcessSheet.DVProcessCC.AddNew();
                    drv.BeginEdit();
                    drv["SEQ_NO"] = seqno;
                    drv["PART_NO"] = ProcessSheet.PART_NO;
                    drv["ROUTE_NO"] = ProcessSheet.ROUTE_NO;
                    drv.EndEdit();
                }

                if (ProcessSheet.DVProcessCC.Count > 0) ProcessCCSelectedItem = ProcessSheet.DVProcessCC[0];
            }
            else if (columnName == "OPER_CODE")
            {
                if (ProcessSheetSelectedItem["OPER_CODE"].IsNotNullOrEmpty() && !ProcessSheetSelectedItem["SEQ_NO"].IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Seq No."));
                    ProcessSheetSelectedItem["OPER_CODE"] = DBNull.Value;
                    ProcessSheetSelectedItem["OPN_DESC"] = "";
                    _shiftTab = false;
                    return;
                }
            }

        }

        string oldCCCode = "";
        public void OnBeginningEditProcessCC(object sender, Microsoft.Windows.Controls.DataGridBeginningEditEventArgs e)
        {
            DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            string columnName = e.Column.SortMemberPath;

            string rowID = selecteditem["ROWID"].ToString();

            if (columnName == "COST_CENT_CODE")
            {
                oldCCCode = selecteditem["COST_CENT_CODE"].ToString().Trim();
            }
        }

        public void OnCellEditEndingProcessCC(object sender, Microsoft.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            string columnName = e.Column.SortMemberPath;
            TextBox tb = e.EditingElement as TextBox;

            //if (tb != null)
            //{
            //    selecteditem.BeginEdit();
            //    selecteditem[columnName] = tb.Text;
            //    selecteditem.EndEdit();
            //}

            string ccCode = selecteditem["COST_CENT_CODE"].ToString().ToUpper().Trim();
            string rowID = selecteditem["ROWID"].ToString();

            if (columnName == "WIRE_SIZE_MIN" && columnName == "WIRE_SIZE_MAX" && columnName == "OUTPUT")
            {
                selecteditem[columnName] = tb.Text;
            }

            if (columnName == "COST_CENT_CODE")
            {

                if (IsDuplicateCCCode(ccCode))
                {
                    ShowInformationMessage("Duplicate Cost Center has been Entered");
                    selecteditem["COST_CENT_CODE"] = oldCCCode;
                    return;
                }
            }
        }

        string oldIssueNo = "";
        public void OnBeginningEditProcessIssue(object sender, DataGridBeginningEditEventArgs e)
        {
            DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            string columnName = e.Column.SortMemberPath;

            string rowID = selecteditem["ROWID"].ToString();

            if (columnName == "ISSUE_NO")
            {
                oldIssueNo = selecteditem["ISSUE_NO"].ToString().Trim();
            }
        }

        public void OnCellEditEndingProcessIssue(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                TextBox tb = e.EditingElement as TextBox;
                DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
                string columnName = e.Column.SortMemberPath;
                string issueNo = selecteditem["ISSUE_NO"].ToString().ToUpper().Trim();
                string rowID = selecteditem["ROWID"].ToString();

                if (columnName == "ISSUE_NO")
                {
                    if (IsDuplicateIssueNo(issueNo))
                    {
                        ShowInformationMessage("Duplicate Issue No has been Entered");
                        //tb.Text = oldIssueNo;
                        tb.Text = "";
                        return;
                    }
                    int output;
                    if (int.TryParse(selecteditem["ISSUE_NO"].ToValueAsString(), out output) == true)
                    {
                        selecteditem["ISSUE_NONUMERIC"] = selecteditem["ISSUE_NO"];
                    }
                    else
                    {
                        selecteditem["ISSUE_NONUMERIC"] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();

            }
        }

        private static T GetVisualChild<T>(DependencyObject visual)
         where T : DependencyObject
        {
            if (visual == null)
                return null;

            var count = System.Windows.Media.VisualTreeHelper.GetChildrenCount(visual);
            for (int i = 0; i < count; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(visual, i);

                var childOfTypeT = child as T ?? GetVisualChild<T>(child);
                if (childOfTypeT != null)
                    return childOfTypeT;
            }

            return null;
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }


        private void DeleteProcessSheet(Microsoft.Windows.Controls.DataGrid dgProcessSheet)
        {
            try
            {
                if (dgProcessSheet != null)
                {
                    if (dgProcessSheet.SelectedItems.Count == 0) return;

                    DataView dvProcessCopy = ProcessSheet.DVProcessSheet.Table.Copy().DefaultView;
                    DataTable dtData = dvProcessCopy.ToTable();
                    dtData.Rows.Clear();

                    dvProcessCopy = dtData.DefaultView;

                    DataRowView dvRow;

                    foreach (DataRowView drv in dgProcessSheet.SelectedItems)
                    {
                        dvRow = dvProcessCopy.AddNew();
                        dvRow["SEQ_NO"] = drv["SEQ_NO"];
                        dvRow["ROUTE_NO"] = drv["ROUTE_NO"];
                        dvRow["OPER_CODE"] = drv["OPER_CODE"];
                    }

                    int deleteCount = dgProcessSheet.SelectedItems.Count;

                    if (MessageBox.Show(dgProcessSheet.SelectedItems.Count + "  record(s) will be deleted.  \nThis will delete related Cost Centres Tool Schedules and PCCS also. Continue? ", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        PartNoIsFocused = true;
                        return;
                    }

                    DataView dvProcess = ProcessSheet.DVProcessSheet.Table.Copy().DefaultView;

                    foreach (DataRowView drv in dvProcessCopy)
                    {
                        if (drv["SEQ_NO"].ToString().Trim() != "" && drv["OPER_CODE"].ToString().Trim() != "")
                        {
                            dvProcess.RowFilter = "SEQ_NO = " + drv["SEQ_NO"].ToString() + " AND ROUTE_NO = " + drv["ROUTE_NO"].ToString();
                            if (dvProcess.Count > 0)
                            {
                                dvProcess.Delete(0);
                            }

                            ProcessSheet.DVProcessCC.RowFilter = "SEQ_NO = " + drv["SEQ_NO"].ToString() + " AND ROUTE_NO = " + drv["ROUTE_NO"].ToString();

                            DataRow[] drs = ProcessSheet.DVProcessCC.Table.Select("SEQ_NO <> " + drv["SEQ_NO"].ToString() + " AND ROUTE_NO = " + drv["ROUTE_NO"].ToString());
                            DataTable dt = ProcessSheet.DVProcessCC.Table.Clone();
                            if (drs.Count() > 0)
                            {
                                foreach (DataRow dr in drs)
                                {
                                    dt.ImportRow(dr);
                                }
                            }
                            ProcessSheet.DVProcessCC = dt.DefaultView;
                            bll.DeleteProcessSheet(drv, ProcessSheet.PART_NO, ProcessSheet.ROUTE_NO.ToString().ToIntValue());
                        }
                    }
                    ProcessSheet.DVProcessSheet = dvProcess;

                    ApplayFilter();
                    MessageBox.Show(deleteCount + "  record(s) deleted.");

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand _deleteCommandProcessCC;
        public ICommand DeleteCommandProcessCC { get { return this._deleteCommandProcessCC; } }
        private void DeleteProcessCC()
        {
            try
            {

                if (ProcessCCSelectedItem == null || ProcessSheet.DVProcessCC[0] == ProcessCCSelectedItem) return;


                if (MessageBox.Show(PDMsg.BeforeDeleteRecord, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                {
                    PartNoIsFocused = true;
                    return;
                }

                if (ProcessCCSelectedItem["ROWID"].ToString().Trim() != "")
                {
                    bll.DeleteProcessCC(ProcessCCSelectedItem, ProcessSheet.PART_NO, ProcessSheet.ROUTE_NO.ToString().ToIntValue());
                }
                ProcessCCSelectedItem.Delete();

                if (ProcessSheet.DVProcessCC.Count == 0 || !ProcessCCHaveEmptyRow())
                {
                    DataRowView drv = ProcessSheet.DVProcessCC.AddNew();
                    drv.BeginEdit();
                    drv["SEQ_NO"] = ProcessSheetSelectedItem["SEQ_NO"].ToString();
                    drv["PART_NO"] = ProcessSheet.PART_NO;
                    drv["ROUTE_NO"] = ProcessSheet.ROUTE_NO;
                    drv.EndEdit();
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand _deleteCommandProcessIssue;
        public ICommand DeleteCommandProcessIssue { get { return this._deleteCommandProcessIssue; } }
        private void DeleteProcessIssue(DataGrid dgProcessIssue)
        {
            try
            {
                if (dgProcessIssue != null)
                {
                    if (dgProcessIssue.SelectedItems.Count == 0) return;

                    DataView dvProcessCopy = ProcessSheet.DVProcessIssue.Table.Copy().DefaultView;
                    DataTable dtData = dvProcessCopy.ToTable();
                    dtData.Rows.Clear();

                    dvProcessCopy = dtData.DefaultView;

                    DataRowView dvRow;

                    foreach (DataRowView drv in dgProcessIssue.SelectedItems)
                    {
                        dvRow = dvProcessCopy.AddNew();
                        dvRow["ISSUE_NO"] = drv["ISSUE_NO"];
                        dvRow["ROUTE_NO"] = drv["ROUTE_NO"];
                    }

                    int deleteCount = dgProcessIssue.SelectedItems.Count;

                    if (MessageBox.Show(deleteCount + " record(s) will be deleted. " + PDMsg.BeforeDeleteRecord, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        PartNoIsFocused = true;
                        return;
                    }



                    DataView dvIssue = ProcessSheet.DVProcessIssue.Table.Copy().DefaultView;

                    foreach (DataRowView drv in dvProcessCopy)
                    {
                        if (drv["ISSUE_NO"].ToString().Trim() != "")
                        {
                            dvIssue.RowFilter = "ISSUE_NO = '" + drv["ISSUE_NO"].ToString() + "' AND ROUTE_NO = " + drv["ROUTE_NO"].ToString();
                            if (dvIssue.Count > 0)
                            {
                                dvIssue.Delete(0);
                            }
                            bll.DeleteProcessIssue(drv, ProcessSheet.PART_NO, ProcessSheet.ROUTE_NO.ToString().ToIntValue());
                        }
                    }
                    dvIssue.RowFilter = ProcessSheet.DVProcessIssue.RowFilter;
                    ProcessSheet.DVProcessIssue = dvIssue;
                    MessageBox.Show(deleteCount + " record(s) deleted. ");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void ReNumber()
        {
            try
            {

                if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Process No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    return;
                }

                DataView dv = ProcessSheet.DVProcessSheet.Table.Copy().DefaultView;
                dv.RowFilter = "Convert(SEQ_NO, 'System.String') <> '' AND Convert(ROUTE_NO, 'System.String') = '" + ProcessSheet.ROUTE_NO + "'";

                if (ProcessSheet.PART_NO.IsNotNullOrEmpty() && ProcessSheet.ROUTE_NO.IsNotNullOrEmpty() && dv.Count > 0)
                {
                    if (MessageBox.Show("Renumbering will make your changes Permanent.  \nDo you want to continue ?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) return;

                    Progress.ProcessingText = PDMsg.ProgressUpdateText;
                    Progress.Start();

                    if (ProcessSheet.CURRENT_PROC && CheckCurrentProcess()) return;

                    varReNumber = true;
                    UpdateProcessSheet();

                    //dv.Sort = "SORTCOL";
                    //copyprocess = new CopyProcess(userInformation);
                    //int i = 0;
                    //foreach (DataRowView drv in dv)
                    //{
                    //    copyprocess.ReNumberProcessSheet(ProcessSheet.PART_NO, ProcessSheet.ROUTE_NO.ToString().ToIntValue(), drv["SEQ_NO"].ToString().ToIntValue(), (1000 + ((i + 1) * 10)), varReNumber);
                    //    i++;
                    //}


                    //copyprocess = new CopyProcess(userInformation);
                    //i = 0;
                    //foreach (DataRowView drv in dv)
                    //{
                    //    copyprocess.ReNumberProcessSheet(ProcessSheet.PART_NO, ProcessSheet.ROUTE_NO.ToString().ToIntValue(), (1000 + ((i + 1) * 10)), ((i + 1) * 10), varReNumber);
                    //    i++;
                    //}
                    if (IsNeedsToRenumbering())
                    {
                        //string conStr = ConfigurationManager.ConnectionStrings["ENGGDB"].ConnectionString;
                        System.Resources.ResourceManager myManager;
                        myManager = new System.Resources.ResourceManager(typeof(ProcessDesigner.Properties.Resources));
                        string conStr = myManager.GetString("ConnectionString");

                        DataAccessLayer dal = new DataAccessLayer(conStr);
                        dal.Renumber_Processsheet(ProcessSheet.PART_NO, ProcessSheet.ROUTE_NO.ToString().ToIntValue());
                    }

                    AssignGridManager();

                    MessageBox.Show("Process Sheet Sequence Number ReNumbered", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    PartNoIsFocused = true;
                    Progress.End();
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand _printCommand;
        public ICommand PrintCommand { get { return this._printCommand; } }
        private void Print()
        {
            if (!ProcessSheet.PART_NO.IsNotNullOrEmpty())
            {
                MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                PartNoIsFocused = true;
                return;
            }
            else
            {
                DataTable processData;
                processData = bll.GetProcessCC(ProcessSheet.PART_NO, ProcessSheet.ROUTE_NO.ToString());
                if (processData == null || processData.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {
                    frmReportViewer rv = new frmReportViewer(processData, "ProcessSheet");
                    rv.ShowDialog();
                }
                PartNoIsFocused = true;

            }
        }

        private readonly ICommand currenprocessclickcommand = null;
        public ICommand CurrenProcessClickCommand { get { return this.currenprocessclickcommand; } }
        private void CurrenProcessClick()
        {
            try
            {
                if (ProcessSheet.CURRENT_PROC && IsDuplicateCurrentProcess())
                {
                    ShowInformationMessage("Already Current Process has been selected for Process No");
                    ProcessSheet.CURRENT_PROC = false;
                    PartNoIsFocused = true;
                    return;
                }
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

        private void GetRights()
        {
            ActionPermission = new RolePermission();
            ActionPermission.Save = true;
            ActionPermission.Print = false;
            ActionPermission.View = true;
            ActionPermission.AddNew = false;
            ActionPermission.Delete = false;
            ActionPermission.Edit = false;
            ActionPermission = bll.GetUserRights("PROCESS SHEET");
            if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
            {
                ActionPermission.Save = false;
            }
        }

        private void setRights()
        {
            if (AddButtonIsEnable) AddButtonIsEnable = ActionPermission.AddNew;
            if (EditButtonIsEnable) EditButtonIsEnable = ActionPermission.Edit;
            if (SaveButtonIsEnable) SaveButtonIsEnable = ActionPermission.Save;
            IsEnableProcessNo = ActionPermission.Edit;
            if (PrintButtonIsEnable) PrintButtonIsEnable = ActionPermission.Print;
        }

        private void showDummy()
        {
            try
            {
                frmDummy dummy = new frmDummy();
                dummy.ShowDialog();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void dgrdProcessIssue_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;

            DataTable dt = ProcessSheet.DVProcessIssue.ToTable().Copy();

            if (Sort == "" || Sort == "asc")
            {
                Sort = "desc";
                dt.Rows[dt.Rows.Count - 1]["ISSUE_NONUMERIC"] = "-999";
                dt.DefaultView.Sort = "ISSUE_NONUMERIC DESC";
            }
            else
            {
                dt.Rows[dt.Rows.Count - 1]["ISSUE_NONUMERIC"] = "999";
                Sort = "asc";
                dt.DefaultView.Sort = "ISSUE_NONUMERIC ASC";
            }

            DataTable dtFinal = dt.DefaultView.ToTable();
            ProcessSheet.DVProcessIssue = (dtFinal != null) ? dtFinal.DefaultView : null;
            ProcessSheet.DVProcessIssue.Sort = "";
        }

        private void ShiftTabProcessSheet()
        {
            try
            {

                int columnDisplayIndex = DgrdProcessSheet.CurrentCell.Column.DisplayIndex;
                if (columnDisplayIndex == 0)
                {

                    DgrdProcessSheet.SelectedIndex = DgrdProcessSheet.SelectedIndex - 1;
                    columnDisplayIndex = DgrdProcessSheet.Columns.Count - 1;
                }
                else
                {
                    if (columnDisplayIndex == 1)
                    {
                        columnDisplayIndex = 0;
                    }
                    else
                    {
                        columnDisplayIndex = columnDisplayIndex - 1;
                    }
                }
                Microsoft.Windows.Controls.DataGridColumn nextColumn = DgrdProcessSheet.ColumnFromDisplayIndex(columnDisplayIndex);

                // now telling the grid, that we handled the key down event
                //e.Handled = true;

                // setting the current cell (selected, focused)
                DgrdProcessSheet.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(DgrdProcessSheet.SelectedItem, nextColumn);

                // tell the grid to initialize edit mode for the current cell
                DgrdProcessSheet.BeginEdit();


            }
            catch (Exception ex)
            {

            }

        }

        public void dgrdProcessSheet_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
                {
                    e.Handled = true;
                    ShiftTabProcessSheet();
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}
