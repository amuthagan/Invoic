using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using System.Data;
using ProcessDesigner.Common;
using System.Windows;
using WPF.MDI;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
namespace ProcessDesigner.ViewModel
{
    class ToolScheduleViewModel : ViewModelBase
    {
        WPF.MDI.MdiChild _mdiChild;

        //public BHCustCtrl.CustComboBox CmbSubHeadingCombo;
        public ProcessDesigner.UserControls.ComboBoxWithSearch CmbSubHeadingCombo;
        public System.Windows.Controls.DataGrid DgvToolSchedule;
        public System.Windows.Controls.DataGrid DgvAuxTools;
        public System.Windows.Controls.DataGrid DgvToolsScheduleRev;
        public decimal Getcc_sno;
        public decimal GetSub_heading_no;
        public decimal GetSeq_no;
        public string TOOL_DESC_temp;


        string sort = "";
        bool blnExecuteCombo = true;
        bool blnprocesscombo = true;
        bool blnReOrder = false;
        private LogViewBll _logviewBll;
        private readonly ICommand _enterPartNumber;
        public ICommand EnterPartNumberCmb { get { return this._enterPartNumber; } }
        private readonly ICommand _enterRouteNumber;
        public ICommand EnterProcessNumberCmb { get { return this._enterRouteNumber; } }
        private readonly ICommand _enterSeqNumber;
        public ICommand EnterSeqNumberCmb { get { return this._enterSeqNumber; } }
        private readonly ICommand _enterCCNumber;
        public ICommand EnterCCNumberCmb { get { return this._enterCCNumber; } }
        public ToolScheduleViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {
            _mdiChild = mdiChild;
            _userInformation = userInformation;
            _toolSchedModel = new ToolScheduleModel();
            _toolScheduleBll = new ToolScheduleBll(_userInformation);
            this._logviewBll = new LogViewBll(_userInformation);
            this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
            this.selectChangeComboCommand = new DelegateCommand(this.FilterToolScheduleAndSelect);
            this.saveCommand = new DelegateCommand(this.SaveToolSchedule);
            this._enterPartNumber = new DelegateCommand<string>(this.EnterPartNumber);
            this._enterRouteNumber = new DelegateCommand<string>(this.EnterRouteNumber);
            this._enterSeqNumber = new DelegateCommand<string>(this.EnterSeqNumber);
            this._enterCCNumber = new DelegateCommand<string>(this.EnterCCNumber);
            this.rowEditEndingTypeCommand = new DelegateCommand<Object>(this.RowEditEndingType);
            this.cellEditEndingTypeCommand = new DelegateCommand<Object>(this.CellEditEndingType);
            this.selectionChangedToolScheduleCommand = new DelegateCommand<Object>(this.SelectionChangedToolSchedule);
            this.multipleSelectionChangedToolScheduleCommand = new DelegateCommand<object>(this.MultipleSelectionChangedToolSchedule);
            this.multipleSelectionChangedAuxToolScheduleCommand = new DelegateCommand<object>(this.MultipleSelectionChangedAuxToolSchedule);
            this.multipleSelectionChangedToolScheduleIssueCommand = new DelegateCommand<object>(this.MultipleSelectionChangedToolScheduleIssue);
            this.rowEditEndingAuxToolScheduleCommand = new DelegateCommand<Object>(this.RowEditEndingAuxToolSchedule);
            this.rowEditEndingToolScheduleIssueCommand = new DelegateCommand<Object>(this.RowEditEndingToolScheduleIssue);
            this.undoToolScheduleCommand = new DelegateCommand(this.UndoToolSchedule);
            this.rowBeginningEditToolScheduleCommand = new DelegateCommand<Object>(this.RowBeginningEditToolSchedule);
            this.deleteToolScheduleSubCommand = new DelegateCommand(this.MultipleDeleteToolScheduleSub);
            this.deleteToolScheduleAuxCommand = new DelegateCommand(this.MultipleDeleteToolScheduleAux);
            this.deleteToolScheduleIssueCommand = new DelegateCommand(this.MultipleDeleteToolScheduleIssue);
            this.reNumberCommand = new DelegateCommand(this.ReNumber);
            this.copyStatusCommand = new DelegateCommand(this.CopyStatus);
            this.selectChangeComboProcessNoCommand = new DelegateCommand(this.SelectChangeComboProcessNo);
            this.selectChangeComboSeqNoCommand = new DelegateCommand(this.SelectChangeComboSeqNo);
            this.selectChangeComboCostCentreCommand = new DelegateCommand(this.SelectChangeComboCostCentre);
            this.editClickCommand = new DelegateCommand(this.EditClick);
            this.closeCommand = new DelegateCommand(this.Close);
            this.processSheetCommand = new DelegateCommand(this.ProcessSheet);
            this.controlPlanCommand = new DelegateCommand(this.ControlPlan);
            this.drawingsCommand = new DelegateCommand(this.Drawings);
            this.developmenReportCommand = new DelegateCommand(this.DevelopmenReport);
            this.mfgRptCommand = new DelegateCommand(this.MfgRpt);
            this.printToolScheduleCommand = new DelegateCommand(this.PrintToolSchedule);
            this.printAuxToolScheduleCommand = new DelegateCommand(this.PrintAuxToolSchedule);
            this.assignNoteCommand = new DelegateCommand(this.AssignNote);
            this.insertToolScheduleCommand = new DelegateCommand(this.InsertToolSchedule);
            this.showToolInfoCommand = new DelegateCommand(this.ShowToolInfo);
            this.insertNewSubHeadingCommand = new DelegateCommand(this.InsertNewSubHeading);
            this.deleteNewSubHeadingCommand = new DelegateCommand(this.DeleteNewSubHeading);
            LoadCombo();
            ClearAll();
            TextSubHeading = "Sub Heading:";
            GetRights();
        }

        public ToolScheduleViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, string partNo)
        {
            _mdiChild = mdiChild;
            _userInformation = userInformation;
            _toolSchedModel = new ToolScheduleModel();
            _toolScheduleBll = new ToolScheduleBll(_userInformation);
            this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
            this.selectChangeComboCommand = new DelegateCommand(this.FilterToolScheduleAndSelect);
            this.saveCommand = new DelegateCommand(this.SaveToolSchedule);
            this.rowEditEndingTypeCommand = new DelegateCommand<Object>(this.RowEditEndingType);
            this.cellEditEndingTypeCommand = new DelegateCommand<Object>(this.CellEditEndingType);
            this.selectionChangedToolScheduleCommand = new DelegateCommand<Object>(this.SelectionChangedToolSchedule);
            this.rowEditEndingAuxToolScheduleCommand = new DelegateCommand<Object>(this.RowEditEndingAuxToolSchedule);
            this.rowEditEndingToolScheduleIssueCommand = new DelegateCommand<Object>(this.RowEditEndingToolScheduleIssue);
            this.undoToolScheduleCommand = new DelegateCommand(this.UndoToolSchedule);
            this._enterPartNumber = new DelegateCommand<string>(this.EnterPartNumber);
            this._enterRouteNumber = new DelegateCommand<string>(this.EnterRouteNumber);
            this._enterSeqNumber = new DelegateCommand<string>(this.EnterSeqNumber);
            this._enterCCNumber = new DelegateCommand<string>(this.EnterCCNumber);
            this.rowBeginningEditToolScheduleCommand = new DelegateCommand<Object>(this.RowBeginningEditToolSchedule);
            this.deleteToolScheduleSubCommand = new DelegateCommand(this.MultipleDeleteToolScheduleSub);
            this.deleteToolScheduleAuxCommand = new DelegateCommand(this.MultipleDeleteToolScheduleAux);
            this.deleteToolScheduleIssueCommand = new DelegateCommand(this.MultipleDeleteToolScheduleIssue);
            this.reNumberCommand = new DelegateCommand(this.ReNumber);
            this.copyStatusCommand = new DelegateCommand(this.CopyStatus);
            this.selectChangeComboProcessNoCommand = new DelegateCommand(this.SelectChangeComboProcessNo);
            this.selectChangeComboSeqNoCommand = new DelegateCommand(this.SelectChangeComboSeqNo);
            this.selectChangeComboCostCentreCommand = new DelegateCommand(this.SelectChangeComboCostCentre);
            this.editClickCommand = new DelegateCommand(this.EditClick);
            this.closeCommand = new DelegateCommand(this.Close);
            this.processSheetCommand = new DelegateCommand(this.ProcessSheet);
            this.controlPlanCommand = new DelegateCommand(this.ControlPlan);
            this.drawingsCommand = new DelegateCommand(this.Drawings);
            this.developmenReportCommand = new DelegateCommand(this.DevelopmenReport);
            this.mfgRptCommand = new DelegateCommand(this.MfgRpt);
            this.printToolScheduleCommand = new DelegateCommand(this.PrintToolSchedule);
            this.printAuxToolScheduleCommand = new DelegateCommand(this.PrintAuxToolSchedule);
            this.assignNoteCommand = new DelegateCommand(this.AssignNote);
            this.insertToolScheduleCommand = new DelegateCommand(this.InsertToolSchedule);
            this.showToolInfoCommand = new DelegateCommand(this.ShowToolInfo);
            this.insertNewSubHeadingCommand = new DelegateCommand(this.InsertNewSubHeading);
            this.deleteNewSubHeadingCommand = new DelegateCommand(this.DeleteNewSubHeading);
            LoadCombo();
            ClearAll();
            TextSubHeading = "Sub Heading:";
            GetRights();
            EditClick();
        }

        UserInformation _userInformation;

        ToolScheduleBll _toolScheduleBll;

        ToolScheduleModel _toolSchedModel = null;

        public Action CloseAction { get; set; }

        private ToolSchedSubModel _currenttoolscedsub = new ToolSchedSubModel();
        private ToolSchedSubModel _prevtoolscedsub = new ToolSchedSubModel();

        private List<ToolSchedSubModel> _copyToolSchedSub = new List<ToolSchedSubModel>();
        private object _selectedItemsToolSchedSubModel;
        private object _selectedItemsAuxToolSchedSubModel;
        private object _selectedItemsToolScheduleIssue;


        private void EnterPartNumber(string partNumber)
        {
            try
            {
                SelectDataRowPart();
            }
            catch (Exception)
            {

            }

        }
        private void EnterRouteNumber(string partNumber)
        {
            try
            {
                SelectChangeComboProcessNo();
            }
            catch (Exception)
            {

            }

        }
        private void EnterSeqNumber(string partNumber)
        {
            try
            {
                SelectChangeComboSeqNo();
            }
            catch (Exception)
            {

            }

        }
        private void EnterCCNumber(string partNumber)
        {
            try
            {
                SelectChangeComboCostCentre();
            }
            catch (Exception)
            {

            }

        }
        public ToolScheduleModel ToolSchedModel
        {
            get
            { return _toolSchedModel; }
            set
            {
                _toolSchedModel = value;
                NotifyPropertyChanged("ToolSchedModel");
            }
        }

        ComboBoxCus partNoComboBoxCus;

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

        private ObservableCollection<DropdownColumns> _dropDownItemsProcessNo;
        public ObservableCollection<DropdownColumns> DropDownItemsProcessNo
        {
            get
            {
                return _dropDownItemsProcessNo;
            }
            set
            {
                this._dropDownItemsProcessNo = value;
                NotifyPropertyChanged("DropDownItemsProcessNo");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsSeqNo;
        public ObservableCollection<DropdownColumns> DropDownItemsSeqNo
        {
            get
            {
                return _dropDownItemsSeqNo;
            }
            set
            {
                this._dropDownItemsSeqNo = value;
                NotifyPropertyChanged("DropDownItemsSeqNo");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsCostCentre;
        public ObservableCollection<DropdownColumns> DropDownItemsCostCentre
        {
            get
            {
                return _dropDownItemsCostCentre;
            }
            set
            {
                this._dropDownItemsCostCentre = value;
                NotifyPropertyChanged("DropDownItemsCostCentre");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsSubHeading;
        public ObservableCollection<DropdownColumns> DropDownItemsSubHeading
        {
            get
            {
                return _dropDownItemsSubHeading;
            }
            set
            {
                this._dropDownItemsSubHeading = value;
                NotifyPropertyChanged("DropDownItemsSubHeading");
            }
        }


        private DataRowView _selectedrowpart;
        public DataRowView SelectedRowPart
        {
            get
            {
                return _selectedrowpart;
            }

            set
            {
                _selectedrowpart = value;
            }
        }

        private DataRowView _selectedrowprocessno;
        public DataRowView SelectedRowProcessNo
        {
            get
            {
                return _selectedrowprocessno;
            }

            set
            {
                _selectedrowprocessno = value;
            }
        }


        private DataRowView _selectedrowSeqNo;
        public DataRowView SelectedRowSeqNo
        {
            get
            {
                return _selectedrowSeqNo;
            }
            set
            {
                _selectedrowSeqNo = value;
                NotifyPropertyChanged("SelectedRowSeqNo");
            }
        }


        private object _selectedrowsubheading;
        public object SelectedRowSubHeading
        {
            get
            {
                return _selectedrowsubheading;
            }
            set
            {
                _selectedrowsubheading = value;
            }
        }

        private List<AUX_TOOL_SCHED> _auxtoolScheduleDetails;
        public List<AUX_TOOL_SCHED> AuxToolScheduleDetails
        {
            get
            {
                return _auxtoolScheduleDetails;
            }

            set
            {
                _auxtoolScheduleDetails = value;
                NotifyPropertyChanged("AuxToolScheduleDetails");
            }
        }

        private List<AUX_TOOL_SCHED> _auxtoolScheduleDetailsAll;
        public List<AUX_TOOL_SCHED> AuxToolScheduleDetailsAll
        {
            get
            {
                return _auxtoolScheduleDetailsAll;
            }

            set
            {
                _auxtoolScheduleDetailsAll = value;
                NotifyPropertyChanged("AuxToolScheduleDetailsAll");
            }
        }

        private List<TOOL_SCHED_MAIN> _toolScheduleMainAll;
        public List<TOOL_SCHED_MAIN> ToolScheduleMainAll
        {
            get
            {
                return _toolScheduleMainAll;
            }

            set
            {
                _toolScheduleMainAll = value;
                NotifyPropertyChanged("ToolScheduleMainAll");
            }
        }


        private List<ToolSchedSubModel> _toolScheduleDetails;
        public List<ToolSchedSubModel> ToolScheduleDetails
        {
            get
            {
                return _toolScheduleDetails;
            }

            set
            {
                _toolScheduleDetails = value;
            }
        }

        private List<ToolSchedSubModel> _toolScheduleDetailsAll;
        public List<ToolSchedSubModel> ToolScheduleDetailsAll
        {
            get
            {
                return _toolScheduleDetailsAll;
            }

            set
            {
                _toolScheduleDetailsAll = value;
            }
        }

        private List<TOOL_SCHED_ISSUE> _toolScheduleRevision;
        public List<TOOL_SCHED_ISSUE> ToolScheduleRevision
        {
            get
            {
                return _toolScheduleRevision;
            }

            set
            {
                _toolScheduleRevision = value;
            }
        }

        private List<TOOL_SCHED_ISSUE> _toolScheduleRevisionAll;
        public List<TOOL_SCHED_ISSUE> ToolScheduleRevisionAll
        {
            get
            {
                return _toolScheduleRevisionAll;
            }
            set
            {
                _toolScheduleRevisionAll = value;
            }
        }

        private ToolSchedSubModel _selectedToolSchedSub;
        public ToolSchedSubModel SelectedToolSchedSub
        {
            get
            {
                return _selectedToolSchedSub;
            }
            set
            {
                _selectedToolSchedSub = value;
            }
        }

        private AUX_TOOL_SCHED _selectedToolSchedAux;
        public AUX_TOOL_SCHED SelectedToolSchedAux
        {
            get
            {
                return _selectedToolSchedAux;
            }
            set
            {
                _selectedToolSchedAux = value;
            }
        }

        private TOOL_SCHED_ISSUE _selectedToolSchedIssue;
        public TOOL_SCHED_ISSUE SelectedToolSchedIssue
        {
            get
            {
                return _selectedToolSchedIssue;
            }
            set
            {
                _selectedToolSchedIssue = value;
            }
        }

        private List<ToolSchedSubModel> _deleteToolScheduleSubAll;
        public List<ToolSchedSubModel> DeleteToolScheduleSubAll
        {
            get
            {
                return _deleteToolScheduleSubAll;
            }
            set
            {
                _deleteToolScheduleSubAll = value;
                NotifyPropertyChanged("DeleteToolScheduleSubAll");
            }
        }

        private List<AUX_TOOL_SCHED> _deleteToolScheduleAuxAll;
        public List<AUX_TOOL_SCHED> DeleteToolScheduleAuxAll
        {
            get
            {
                return _deleteToolScheduleAuxAll;
            }
            set
            {
                _deleteToolScheduleAuxAll = value;
                NotifyPropertyChanged("DeleteToolScheduleAuxAll");
            }
        }

        private List<TOOL_SCHED_ISSUE> _deleteToolScheduleIssueAll;
        public List<TOOL_SCHED_ISSUE> DeleteToolScheduleIssueAll
        {
            get
            {
                return _deleteToolScheduleIssueAll;
            }
            set
            {
                _deleteToolScheduleIssueAll = value;
                NotifyPropertyChanged("DeleteToolScheduleIssueAll");
            }
        }

        private DataView _sequenceNoCombo;
        public DataView SequenceNoCombo
        {
            get
            {
                return _sequenceNoCombo;
            }
            set
            {
                _sequenceNoCombo = value;
                NotifyPropertyChanged("SequenceNoCombo");
            }
        }

        private DataView _costCentreCombo;
        public DataView CostCentreCombo
        {
            get
            {
                return _costCentreCombo;
            }
            set
            {
                _costCentreCombo = value;
                NotifyPropertyChanged("CostCentreCombo");
            }
        }

        private DataView _subHeadingCombo;
        public DataView SubHeadingCombo
        {
            get
            {
                return _subHeadingCombo;
            }
            set
            {
                _subHeadingCombo = value;
                NotifyPropertyChanged("SubHeadingCombo");
            }
        }


        private DataView _partNoCombo;
        public DataView PartNoCombo
        {
            get
            {
                return _partNoCombo;
            }
            set
            {
                _partNoCombo = value;
                NotifyPropertyChanged("PartNoCombo");
            }
        }

        private DataView _processNoCombo;
        public DataView ProcessNoCombo
        {
            get
            {
                return _processNoCombo;
            }
            set
            {
                _processNoCombo = value;
                NotifyPropertyChanged("ProcessNoCombo");
            }
        }

        private string _textSubHeading;
        public string TextSubHeading
        {
            get
            {
                return _textSubHeading;
            }
            set
            {
                _textSubHeading = value;
                NotifyPropertyChanged("TextSubHeading");
            }
        }

        private string _textCostCenter = "Cost Center : ";
        public string TextCostCenter
        {
            get
            {
                return _textCostCenter;
            }
            set
            {
                _textCostCenter = value;
                NotifyPropertyChanged("TextCostCenter");
            }
        }

        private object _selectedRowCostCentre;
        public object SelectedRowCostCentre
        {
            get
            {
                return _selectedRowCostCentre;
            }
            set
            {
                _selectedRowCostCentre = value;
                //NotifyPropertyChanged("SelectedRowCostCentre");
            }
        }


        private bool _auxEnable;
        public bool AuxEnable
        {
            get
            {
                return _auxEnable;
            }
            set
            {
                _auxEnable = value;
                NotifyPropertyChanged("AuxEnable");
            }

        }

        private bool _saveEnable;
        public bool SaveEnable
        {
            get
            {
                return _saveEnable;
            }
            set
            {
                _saveEnable = value;
                NotifyPropertyChanged("SaveEnable");
            }
        }

        private bool _editEnable = false;
        public bool EditEnable
        {
            get
            {
                return _editEnable;
            }
            set
            {
                _editEnable = value;
                NotifyPropertyChanged("EditEnable");
            }
        }

        private bool _printEnable;
        public bool PrintEnable
        {
            get
            {
                return _printEnable;
            }
            set
            {
                _printEnable = value;
                NotifyPropertyChanged("PrintEnable");
            }
        }


        private bool _partNoIsFocused;

        public bool PartNoIsFocused
        {
            get
            {
                return _partNoIsFocused;
            }
            set
            {
                _partNoIsFocused = value;
                NotifyPropertyChanged("PartNoIsFocused");
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

        private void LoadCombo()
        {
            PartNoCombo = _toolScheduleBll.GetPartNoDetails(ToolSchedModel);
            SetdropDownItems();
        }

        private void SetdropDownItems()
        {
            try
            {
                DropDownItemsPart = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "Part No", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "Description", ColumnWidth = "1*" }
                        };

                DropDownItemsProcessNo = new ObservableCollection<DropdownColumns>()
                {
                    new DropdownColumns() { ColumnName = "ROUTE_NO", ColumnDesc = "Process No", ColumnWidth = "1*" }
                };

                DropDownItemsSeqNo = new ObservableCollection<DropdownColumns>()
                {
                    new DropdownColumns() { ColumnName = "SEQ_NO", ColumnDesc = "Seq No", ColumnWidth = "75" },
                    new DropdownColumns() { ColumnName = "OPN_DESC", ColumnDesc = "Description", ColumnWidth = "150" }
                };

                DropDownItemsCostCentre = new ObservableCollection<DropdownColumns>()
                {
                    new DropdownColumns() { ColumnName = "CC_SNO", ColumnDesc = "Cost Center", ColumnWidth = "130" },
                    new DropdownColumns() { ColumnName = "CC_CODE", ColumnDesc = "CC Code", ColumnWidth = "150" }
                };

                DropDownItemsSubHeading = new ObservableCollection<DropdownColumns>()
                {
                    new DropdownColumns() { ColumnName = "SUB_HEADING_NO", ColumnDesc = "Sub Heading No", ColumnWidth = "125" },
                    new DropdownColumns() { ColumnName = "SUB_HEADING", ColumnDesc = "Sub Heading", ColumnWidth = "150" }
                };


            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }



        private readonly ICommand selectChangeComboCommandPartNo;
        public ICommand SelectChangeComboCommandPartNo { get { return this.selectChangeComboCommandPartNo; } }
        private void SelectDataRowPart()
        {
            blnExecuteCombo = false;
            ToolScheduleModel temptsm = new ToolScheduleModel();
            List<TOOL_SCHED_MAIN> lsttoolmain = new List<TOOL_SCHED_MAIN>();
            try
            {
                _copyToolSchedSub = new List<ToolSchedSubModel>();
                if (this.SelectedRowPart != null)
                {
                    ToolSchedModel.RouteNo = "";
                    ToolSchedModel.SeqNo = "";
                    ToolSchedModel.CCSno = "";
                    ToolSchedModel.CCCode = "";
                    ToolSchedModel.SubHeadingNo = "";
                    ToolSchedModel.SubHeading = "";
                    ToolSchedModel.TopNote = "";
                    ToolSchedModel.BotNote = "";
                    ClearDataView(ProcessNoCombo);
                    ClearDataView(SequenceNoCombo);
                    ClearDataView(CostCentreCombo);
                    ClearDataView(SubHeadingCombo);
                    ToolSchedModel.PartNo = this.SelectedRowPart["PART_NO"].ToString();
                    ToolSchedModel.PartDescription = this.SelectedRowPart["PART_DESC"].ToString();
                    if (ToolSchedModel.PartNo != null)
                    {

                        ToolScheduleDetails = new List<ToolSchedSubModel>();
                        ToolScheduleDetailsAll = new List<ToolSchedSubModel>();
                        ToolScheduleRevision = new List<TOOL_SCHED_ISSUE>();
                        ToolScheduleRevisionAll = new List<TOOL_SCHED_ISSUE>();
                        AuxToolScheduleDetails = new List<AUX_TOOL_SCHED>();
                        AuxToolScheduleDetailsAll = new List<AUX_TOOL_SCHED>();

                        DeleteToolScheduleAuxAll = new List<AUX_TOOL_SCHED>();
                        DeleteToolScheduleIssueAll = new List<TOOL_SCHED_ISSUE>();
                        DeleteToolScheduleSubAll = new List<ToolSchedSubModel>();
                        ProcessNoCombo = _toolScheduleBll.GetProcessNo(ToolSchedModel);
                        ToolScheduleMainAll = _toolScheduleBll.GetToolScheduleMainList(ToolSchedModel);
                        ToolScheduleDetailsAll = _toolScheduleBll.GetToolScheduleSub(ToolSchedModel.PartNo);
                        ToolSchedModel.RouteNo = _toolScheduleBll.GetCurrentProcessNo(ToolSchedModel);

                        if (!ToolSchedModel.RouteNo.IsNotNullOrEmpty() && ProcessNoCombo.Count > 0) ToolSchedModel.RouteNo = ProcessNoCombo[0]["ROUTE_NO"].ToString();

                        if (ToolSchedModel.RouteNo.IsNotNullOrEmpty())
                        {
                            blnExecuteCombo = true;
                            SelectChangeComboProcessNo();
                        }

                        ToolScheduleRevisionAll = _toolScheduleBll.GetAuxToolScheduleIssue(ToolSchedModel);
                        FilterToolSchedule();
                        FilterToolScheduleIssue();
                        AuxToolScheduleDetailsAll = _toolScheduleBll.GetAuxToolSchedule(ToolSchedModel, ToolScheduleDetailsAll);
                        AddToolScheduleMain();
                    }
                }
                NotifyPropertyChanged("ToolSchedModel");
                NotifyPropertyChanged("ToolScheduleDetails");
                NotifyPropertyChanged("ToolScheduleRevision");
                SelectTheGridRow();
                _mdiChild.Title = ApplicationTitle + " - Tool Schedule" + ((ToolSchedModel.PartNo.IsNotNullOrEmpty()) ? " - " + ToolSchedModel.PartNo : "");
            }
            catch (Exception ex)
            {
                //throw ex.LogException()
            }
            blnExecuteCombo = true;
        }

        private readonly ICommand selectChangeComboCommand;
        public ICommand SelectChangeComboCommand { get { return this.selectChangeComboCommand; } }
        private void FilterToolScheduleAndSelect()
        {
            try
            {
                FilterToolSchedule();
                SelectTheGridRow();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void FilterToolSchedule()
        {
            try
            {
                AddBlankRowToolSchedDetails();
                if (CmbSubHeadingCombo.SelectedValue != null)
                {
                    ToolSchedModel.SubHeadingNo = CmbSubHeadingCombo.SelectedValue.ToValueAsString();
                    TextSubHeading = "Sub Heading:(" + ToolSchedModel.SubHeadingNo + ")";

                    //new
                    //GetSub_heading_no = Convert.ToDecimal(ToolSchedModel.SubHeadingNo); //new comment by me
                }
                else
                {
                    //TextSubHeading = "Sub Heading:(0)";
                    //ToolSchedModel.SubHeadingNo = "0";
                }
                ToolScheduleDetails = (from row in ToolScheduleDetailsAll
                                       where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
                                       row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
                                       row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue() &&
                                       row.SUB_HEADING_NO == ToolSchedModel.SubHeadingNo.ToDecimalValue()
                                       //orderby row.SNO ?? Int64.MaxValue 
                                       //new by me
                                       //&& row.SNO != null 
                                       //orderby row.SNO ascending
                                       orderby (row.SNO.ToDecimalValue() == 0 ? Int64.MaxValue : row.SNO.ToDecimalValue())
                                       //end new
                                       select row).ToList<ToolSchedSubModel>();

                //new
                //Getcc_sno = ToolScheduleDetails[0].CC_SNO;  //comment by me
                //GetSub_heading_no = ToolScheduleDetails[0].SUB_HEADING_NO;
                //GetSub_heading_no = Convert.ToDecimal(ToolSchedModel.SubHeadingNo);
                //end new

                FilterToolScheduleIssue();
                //NotifyPropertyChanged("ToolSchedModel");
                NotifyPropertyChanged("ToolScheduleDetails");
                NotifyPropertyChanged("TextSubHeading");
                AddToolScheduleMain();
                SetNote();
                NotifyPropertyChanged("ToolSchedModel");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void AddBlankRowToolSchedDetails()
        {
            List<ToolSchedSubModel> lstCheck = new List<ToolSchedSubModel>();
            ToolSchedSubModel toolschedsub = new ToolSchedSubModel();
            lstCheck = (from row in ToolScheduleDetailsAll
                        where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
                        row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
                        row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue() &&
                        row.SUB_HEADING_NO == ToolSchedModel.SubHeadingNo.ToDecimalValue()
                        && (row.IDPK == 0)
                        orderby row.SNO
                        //orderby row.SNO ascending //new by me
                        select row).ToList<ToolSchedSubModel>();
            if (lstCheck.Count == 0)
            {
                toolschedsub.PART_NO = ToolSchedModel.PartNo;
                toolschedsub.ROUTE_NO = ToolSchedModel.RouteNo.ToDecimalValue();
                toolschedsub.SEQ_NO = ToolSchedModel.SeqNo.ToDecimalValue();
                toolschedsub.CC_SNO = ToolSchedModel.CCSno.ToDecimalValue();
                toolschedsub.SUB_HEADING_NO = ToolSchedModel.SubHeadingNo.ToDecimalValue();
                toolschedsub.TOOL_CODE = "";
                toolschedsub.TOOL_CODE_END = "";
                toolschedsub.TOOL_DESC = "";
                toolschedsub.REMARKS = "";
                toolschedsub.IDPK = 0;
                ToolScheduleDetailsAll.Add(toolschedsub);
            }
        }

        private void FilterToolScheduleIssue()
        {
            try
            {
                AddBlankRowToolSchedDetailsIssue();
                ToolScheduleRevision = (from row in ToolScheduleRevisionAll
                                        where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
                                        row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
                                        row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue()
                                        select row).ToList<TOOL_SCHED_ISSUE>();

                //NotifyPropertyChanged("ToolSchedModel");
                NotifyPropertyChanged("ToolScheduleRevision");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void AddBlankRowToolSchedDetailsIssue()
        {

            List<TOOL_SCHED_ISSUE> lstCheck = new List<TOOL_SCHED_ISSUE>();
            TOOL_SCHED_ISSUE toolschedissue = new TOOL_SCHED_ISSUE();
            try
            {
                lstCheck = (from row in ToolScheduleRevisionAll
                            where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
                            row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
                            row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue() &&
                            row.IDPK == 0
                            select row).ToList<TOOL_SCHED_ISSUE>();
                if (lstCheck.Count == 0)
                {
                    toolschedissue.PART_NO = ToolSchedModel.PartNo;
                    toolschedissue.ROUTE_NO = ToolSchedModel.RouteNo.ToDecimalValue();
                    toolschedissue.SEQ_NO = ToolSchedModel.SeqNo.ToDecimalValue();
                    toolschedissue.CC_SNO = ToolSchedModel.CCSno.ToDecimalValue();
                    toolschedissue.TS_ISSUE_NO = "";
                    toolschedissue.IDPK = 0;
                    toolschedissue.ISSUE_NO_NEW = 999;
                    ToolScheduleRevisionAll.Add(toolschedissue);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void FilterAuxToolSchedule(string madeFor)
        {
            try
            {
                AddBlankRowAuxToolSchedDetails(madeFor);
                AuxToolScheduleDetails = (from row in AuxToolScheduleDetailsAll
                                          where row.MADE_FOR == madeFor
                                          select row).ToList<AUX_TOOL_SCHED>();
                NotifyPropertyChanged("AuxToolScheduleDetails");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void AddBlankRowAuxToolSchedDetails(string madeFor)
        {

            List<AUX_TOOL_SCHED> lstCheck = new List<AUX_TOOL_SCHED>();
            AUX_TOOL_SCHED auxtoolsched = new AUX_TOOL_SCHED();
            try
            {
                lstCheck = (from row in AuxToolScheduleDetailsAll
                            where row.MADE_FOR == madeFor &&
                            row.IDPK == 0
                            select row).ToList<AUX_TOOL_SCHED>();
                if (lstCheck.Count == 0)
                {
                    auxtoolsched.CATEGORY = "";
                    auxtoolsched.MADE_FOR = madeFor;
                    auxtoolsched.TEMPLATE_CD = "";
                    auxtoolsched.TOOL_CODE = "";
                    auxtoolsched.TOOL_DESC = "";

                    auxtoolsched.IDPK = 0;
                    AuxToolScheduleDetailsAll.Add(auxtoolsched);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand saveCommand;
        public ICommand SaveClickCommand { get { return this.saveCommand; } }

        public Boolean IsRecordsUpdated = false;
        private void SaveToolSchedule()
        {
            NotifyPropertyChanged("ToolScheduleDetails");
            NotifyPropertyChanged("SelectedToolSchedSub");
            NotifyPropertyChanged("ToolSchedModel");
            NotifyPropertyChanged("SelectedToolSchedIssue");
            // NotifyPropertyChanged("DeleteToolScheduleSubAll");
            // ReNumber();
            if (SaveEnable == false) return;
            try
            {
                if (SelectedToolSchedSub != null)
                {
                    RowEditEndingType(SelectedToolSchedSub);
                }
                if (SelectedToolSchedAux != null)
                {
                    RowEditEndingAuxToolSchedule(SelectedToolSchedAux);
                }
                if (SelectedToolSchedIssue != null)
                {
                    RowEditEndingToolScheduleIssue(SelectedToolSchedIssue);
                }
                CmbSubHeadingCombo.Focus();

                if (Validate() == true)
                {
                    if (CmbSubHeadingCombo.SelectedItem == null)
                    {
                        ToolSchedModel.SubHeading = CmbSubHeadingCombo.SelectedText;
                        if (ToolSchedModel.SubHeadingNo == "")
                        {
                            ToolSchedModel.SubHeadingNo = "0";
                        }
                    }
                    else
                    {
                        ToolSchedModel.SubHeading = CmbSubHeadingCombo.SelectedText;
                        ToolSchedModel.SubHeadingNo = CmbSubHeadingCombo.SelectedValue.ToValueAsString().Trim();
                    }
                    Progress.ProcessingText = PDMsg.ProgressUpdateText;
                    Progress.Start();
                    AddToolScheduleMain();
                    UpdateToolScheduleMain();
                    string headNo = "";
                    //string headNo = null; //new
                    string subHeading = "";
                    headNo = ToolSchedModel.SubHeadingNo;
                    subHeading = ToolSchedModel.SubHeading;
                    //new
                    _toolScheduleBll.setvalue(Getcc_sno);
                    //new new
                    if (GetSub_heading_no != 0)
                    {
                        _toolScheduleBll.setsubheadingno(GetSub_heading_no);
                    }
                    //end new
                    if (_toolScheduleBll.Save(DeleteToolScheduleSubAll, ToolSchedModel, AuxToolScheduleDetailsAll, ToolScheduleDetailsAll, ToolScheduleRevisionAll, DeleteToolScheduleAuxAll, DeleteToolScheduleIssueAll, ToolScheduleMainAll) == true)
                    {
                        ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                        _logviewBll.SaveLog(ToolSchedModel.PartNo, "ToolSchedule");
                        EditClick();
                        if (!blnExecuteCombo)
                            SelectDataRowPart();
                        ToolSchedModel.SubHeadingNo = headNo;
                        ToolSchedModel.SubHeading = subHeading;
                        FilterToolScheduleAndSelect();
                    }
                    //new
                    GetSub_heading_no = 0;
                    CmbSubHeadingCombo.SelectedItem = null;
                    Progress.End();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand rowEditEndingTypeCommand;
        public ICommand RowEditEndingTypeCommand { get { return this.rowEditEndingTypeCommand; } }
        private void RowEditEndingType(Object selecteditem)
        {
            //return;
            try
            {
                ToolSchedSubModel toolsub = new ToolSchedSubModel();
                toolsub = (ToolSchedSubModel)selecteditem;
                /*
                SNO
                TOOL_CODE
                TOOL_CODE_END
                TOOL_DESC
                CATEGORY
                QTY
                REMARKS
                  */
                if (toolsub.TOOL_CODE.ToValueAsString() != "" || toolsub.SNO.ToValueAsString() != "" || toolsub.TOOL_CODE.ToValueAsString() != ""
                    || toolsub.TOOL_CODE_END.ToValueAsString() != "" || toolsub.TOOL_DESC.ToValueAsString() != "" || toolsub.CATEGORY.ToValueAsString() != ""
                    || toolsub.QTY.ToValueAsString() != "" || toolsub.REMARKS.ToValueAsString() != "")
                {
                    if (toolsub.IDPK == 0)
                    {
                        toolsub.IDPK = -1 * ToolScheduleDetailsAll.Count;
                        FilterToolSchedule();
                    }
                }
                FilterAuxToolSchedule(toolsub.TOOL_CODE.ToValueAsString());
                if (AuxToolScheduleDetails.Count > 0)
                {
                    SelectedToolSchedAux = AuxToolScheduleDetails[0];
                    NotifyPropertyChanged("SelectedToolSchedAux");
                }
                if (_currenttoolscedsub.TOOL_CODE.ToValueAsString().Trim() != "")
                {
                    AuxEnable = true;
                }
                else
                {
                    AuxEnable = false;
                }
                NotifyPropertyChanged("AuxEnable");
                NotifyPropertyChanged("ToolScheduleDetails");
                //DgvToolSchedule.Focus();
                //DgvToolSchedule.BeginEdit();

                try
                {
                    DataGridCell cell = GetCell(DgvToolSchedule.SelectedIndex, DgvToolSchedule.CurrentCell.Column.DisplayIndex);
                    //if (cell != null)
                    //{
                    //cell.Focus();
                    //cell.Visibility = Visibility.Hidden;
                    //cell.
                    //DgvToolSchedule.SelectedIndex = DgvToolSchedule.Items.Count - 1;
                    //DgvToolSchedule.SelectedIndex = 0;
                    //System.Windows.Controls.DataGridColumn nextcolumn = DgvToolSchedule.ColumnFromDisplayIndex(1);
                    //DgvToolSchedule.CurrentCell = new System.Windows.Controls.DataGridCellInfo(DgvToolSchedule.SelectedItem, nextcolumn);
                    //DgvToolSchedule.Focus();
                    // DgvToolSchedule.BeginEdit();
                    //}
                }
                catch (Exception ex1)
                {

                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand cellEditEndingTypeCommand;
        public ICommand CellEditEndingTypeCommand { get { return this.cellEditEndingTypeCommand; } }
        private void CellEditEndingType(Object selecteditem)
        {
            try
            {
                string convertToolCode = "";
                ToolSchedSubModel toolsub = new ToolSchedSubModel();
                toolsub = (ToolSchedSubModel)selecteditem;
                if (toolsub.TOOL_CODE.Length > 0)
                {
                    try
                    {
                        if (_prevtoolscedsub.TOOL_CODE != toolsub.TOOL_CODE.Trim())
                        {
                            //ToolInfoViewModel tfvm = new ToolInfoViewModel();
                            convertToolCode = Chk_Tool(toolsub.TOOL_CODE);
                            if (convertToolCode.ToValueAsString().Trim() != "")
                            {
                                toolsub.TOOL_CODE = convertToolCode;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    AuxEnable = true;
                }
                else
                {
                    AuxEnable = false;
                }
                NotifyPropertyChanged("AuxEnable");
                /*
                SNO
                TOOL_CODE
                TOOL_CODE_END
                TOOL_DESC
                CATEGORY
                QTY
                REMARKS
                  */
                if (toolsub.TOOL_CODE.ToValueAsString() != "" || toolsub.SNO.ToValueAsString() != "" || toolsub.TOOL_CODE.ToValueAsString() != ""
                    || toolsub.TOOL_CODE_END.ToValueAsString() != "" || toolsub.TOOL_DESC.ToValueAsString() != "" || toolsub.CATEGORY.ToValueAsString() != ""
                    || toolsub.QTY.ToValueAsString() != "" || toolsub.REMARKS.ToValueAsString() != "")
                {
                    if (toolsub.IDPK == 0)
                    {
                        //toolsub.IDPK = -1 * ToolScheduleDetailsAll.Count;
                        //FilterToolSchedule();
                        //AddBlankRowToolSchedDetails();
                    }
                }
                if (toolsub.TOOL_CODE.ToValueAsString().Trim() != _prevtoolscedsub.TOOL_CODE.ToValueAsString().Trim() || toolsub.SNO.ToValueAsString().Trim() != _prevtoolscedsub.SNO.ToValueAsString().Trim() ||
     toolsub.TOOL_CODE_END.ToValueAsString().Trim() != _prevtoolscedsub.TOOL_CODE_END.ToValueAsString().Trim() || toolsub.TOOL_DESC.ToValueAsString().Trim() != _prevtoolscedsub.TOOL_DESC.ToValueAsString().Trim() || toolsub.CATEGORY.ToValueAsString().Trim() != _prevtoolscedsub.CATEGORY.ToValueAsString().Trim() ||
     toolsub.QTY.ToValueAsString().Trim() != _prevtoolscedsub.QTY.ToValueAsString().Trim() || toolsub.REMARKS.ToValueAsString().Trim() != _prevtoolscedsub.REMARKS.ToValueAsString().Trim())
                {
                    if (_prevtoolscedsub.IDPK == 0)
                    {
                        _prevtoolscedsub.IDPK = -1 * ToolScheduleDetailsAll.Count;
                    }
                    _copyToolSchedSub.Add(_prevtoolscedsub);
                }
                //NotifyPropertyChanged("ToolScheduleDetails");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand selectionChangedToolScheduleCommand;
        public ICommand SelectionChangedToolScheduleCommand
        {
            get
            {
                return this.selectionChangedToolScheduleCommand;
            }
        }
        private void SelectionChangedToolSchedule(Object selecteditem)
        {
            try
            {
                ToolSchedSubModel toolsub = new ToolSchedSubModel();
                if (selecteditem != null)
                {
                    toolsub = (ToolSchedSubModel)selecteditem;
                    _currenttoolscedsub = toolsub;
                }
                else
                {
                    _currenttoolscedsub = new ToolSchedSubModel();
                }
                FilterAuxToolSchedule(toolsub.TOOL_CODE.ToValueAsString());
                if (AuxToolScheduleDetails.Count > 0)
                {
                    SelectedToolSchedAux = AuxToolScheduleDetails[0];
                    NotifyPropertyChanged("SelectedToolSchedAux");
                }
                if (_currenttoolscedsub.TOOL_CODE.ToValueAsString().Trim() != "")
                {
                    AuxEnable = true;
                }
                else
                {
                    AuxEnable = false;
                }
                NotifyPropertyChanged("AuxEnable");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand multipleSelectionChangedToolScheduleCommand;
        public ICommand MultipleSelectionChangedToolScheduleCommand { get { return this.multipleSelectionChangedToolScheduleCommand; } }
        private void MultipleSelectionChangedToolSchedule(object selectedItems)
        {
            try
            {
                //_selectedItemsToolSchedSubModel = new List<ToolSchedSubModel>();
                _selectedItemsToolSchedSubModel = selectedItems;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand multipleSelectionChangedAuxToolScheduleCommand;
        public ICommand MultipleSelectionChangedAuxToolScheduleCommand { get { return this.multipleSelectionChangedAuxToolScheduleCommand; } }
        private void MultipleSelectionChangedAuxToolSchedule(object selectedItems)
        {
            try
            {
                //_selectedItemsToolSchedSubModel = new List<ToolSchedSubModel>();
                _selectedItemsAuxToolSchedSubModel = selectedItems;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand multipleSelectionChangedToolScheduleIssueCommand;
        public ICommand MultipleSelectionChangedToolScheduleIssueCommand { get { return this.multipleSelectionChangedToolScheduleIssueCommand; } }
        private void MultipleSelectionChangedToolScheduleIssue(object selectedItems)
        {
            try
            {
                //_selectedItemsToolSchedSubModel = new List<ToolSchedSubModel>();
                _selectedItemsToolScheduleIssue = selectedItems;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand rowEditEndingAuxToolScheduleCommand;
        public ICommand RowEditEndingAuxToolScheduleCommand { get { return this.rowEditEndingAuxToolScheduleCommand; } }
        private void RowEditEndingAuxToolSchedule(Object selecteditem)
        {
            try
            {
                AUX_TOOL_SCHED auxtoolsub = new AUX_TOOL_SCHED();
                auxtoolsub = (AUX_TOOL_SCHED)selecteditem;
                if (auxtoolsub.IDPK == 0 && (auxtoolsub.TOOL_CODE.ToValueAsString().Trim() != ""
                            || auxtoolsub.CATEGORY.ToValueAsString() != ""
                            || auxtoolsub.TEMPLATE_CD.ToValueAsString() != ""
                            || auxtoolsub.TOOL_DESC.ToValueAsString() != ""))
                {
                    auxtoolsub.IDPK = -1;
                    FilterAuxToolSchedule(_currenttoolscedsub.TOOL_CODE.ToValueAsString());
                    DataGridCell cell = GetCell(0, 0, DgvAuxTools);
                    if (cell != null)
                    {
                        //cell.Focus();
                        //cell.Visibility = Visibility.Hidden;
                        //cell.
                        //DgvToolSchedule.SelectedIndex = DgvToolSchedule.Items.Count - 1;
                        //DgvToolSchedule.SelectedIndex = 0;
                        //System.Windows.Controls.DataGridColumn nextcolumn = DgvToolSchedule.ColumnFromDisplayIndex(1);
                        //DgvToolSchedule.CurrentCell = new System.Windows.Controls.DataGridCellInfo(DgvToolSchedule.SelectedItem, nextcolumn);
                        DgvAuxTools.Focus();
                        DgvAuxTools.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand rowEditEndingToolScheduleIssueCommand;
        public ICommand RowEditEndingToolScheduleIssueCommand { get { return this.rowEditEndingToolScheduleIssueCommand; } }
        private void RowEditEndingToolScheduleIssue(Object selecteditem)
        {
            try
            {
                TOOL_SCHED_ISSUE toolsubissue = new TOOL_SCHED_ISSUE();
                toolsubissue = (TOOL_SCHED_ISSUE)selecteditem;
                if (toolsubissue.IDPK == 0 && (toolsubissue.TS_ISSUE_NO.ToValueAsString().Trim() != ""
                            || toolsubissue.TS_ISSUE_DATE.ToValueAsString() != ""
                            || toolsubissue.TS_ISSUE_ALTER.ToValueAsString() != ""
                            || toolsubissue.TS_COMPILED_BY.ToValueAsString() != ""))
                {
                    toolsubissue.IDPK = -1;
                    FilterToolScheduleIssue();
                    DataGridCell cell = GetCell(0, 0, DgvToolsScheduleRev);
                    if (cell != null)
                    {
                        //cell.Focus();
                        //cell.Visibility = Visibility.Hidden;
                        //cell.
                        //DgvToolSchedule.SelectedIndex = DgvToolSchedule.Items.Count - 1;
                        //DgvToolSchedule.SelectedIndex = 0;
                        //System.Windows.Controls.DataGridColumn nextcolumn = DgvToolSchedule.ColumnFromDisplayIndex(1);
                        //DgvToolSchedule.CurrentCell = new System.Windows.Controls.DataGridCellInfo(DgvToolSchedule.SelectedItem, nextcolumn);
                        DgvToolsScheduleRev.Focus();
                        DgvToolsScheduleRev.BeginEdit();
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand undoToolScheduleCommand;
        public ICommand UndoToolScheduleCommand { get { return this.undoToolScheduleCommand; } }
        private void UndoToolSchedule()
        {
            try
            {
                CmbSubHeadingCombo.Focus();
                List<ToolSchedSubModel> tss = new List<ToolSchedSubModel>();
                ToolSchedSubModel copytss = new ToolSchedSubModel();
                if (_copyToolSchedSub.Count > 0)
                {
                    tss = (from row in ToolScheduleDetailsAll
                           where row.IDPK == _copyToolSchedSub[_copyToolSchedSub.Count - 1].IDPK
                           select row).ToList<ToolSchedSubModel>();

                    copytss = _copyToolSchedSub[_copyToolSchedSub.Count - 1];
                    //tss[0]= _copyToolSchedSub[_copyToolSchedSub.Count - 1].DeepCopy<ToolSchedSubModel>();
                    if (tss.Count > 0)
                    {
                        tss[0].SNO = copytss.SNO;
                        tss[0].TOOL_CODE = copytss.TOOL_CODE;
                        tss[0].TOOL_CODE_END = copytss.TOOL_CODE_END;
                        tss[0].TOOL_DESC = copytss.TOOL_DESC;
                        tss[0].CATEGORY = copytss.CATEGORY;
                        tss[0].QTY = copytss.QTY;
                        tss[0].REMARKS = copytss.REMARKS;
                        //tss[0].SNO = _copyToolSchedSub[_copyToolSchedSub.Count]
                        //tss[0] = _copyToolSchedSub[_copyToolSchedSub.Count - 1];
                        _copyToolSchedSub.RemoveAt(_copyToolSchedSub.Count - 1);
                        FilterToolSchedule();
                    }
                    else
                    {
                        if (copytss.Delete == 1)
                        {
                            ToolSchedSubModel tssm = new ToolSchedSubModel();
                            tssm.CATEGORY = copytss.CATEGORY;
                            tssm.CC_SNO = copytss.CC_SNO;
                            tssm.IDPK = copytss.IDPK;
                            tssm.PART_NO = copytss.PART_NO;
                            tssm.QTY = copytss.QTY;
                            tssm.REMARKS = copytss.REMARKS;
                            tssm.ROUTE_NO = copytss.ROUTE_NO;
                            tssm.ROWID = copytss.ROWID;
                            tssm.SEQ_NO = copytss.SEQ_NO;
                            tssm.SNO = copytss.SNO;
                            tssm.SUB_HEADING_NO = copytss.SUB_HEADING_NO;
                            tssm.TOOL_CODE = copytss.TOOL_CODE;
                            tssm.TOOL_CODE_END = copytss.TOOL_CODE_END;
                            tssm.TOOL_DESC = copytss.TOOL_DESC;
                            ToolScheduleDetailsAll.Insert(ToolScheduleDetailsAll.Count - 1, tssm);
                            _copyToolSchedSub.RemoveAt(_copyToolSchedSub.Count - 1);
                            int ictr = 0;
                            while (ictr <= DeleteToolScheduleSubAll.Count - 1)
                            {
                                if (DeleteToolScheduleSubAll[ictr].IDPK == copytss.IDPK)
                                {
                                    DeleteToolScheduleSubAll.RemoveAt(ictr);
                                }
                                else
                                {
                                    ictr = ictr + 1;
                                }
                            }
                            FilterToolSchedule();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand rowBeginningEditToolScheduleCommand;
        public ICommand RowBeginningEditToolScheduleCommand { get { return this.rowBeginningEditToolScheduleCommand; } }
        private void RowBeginningEditToolSchedule(Object selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    _prevtoolscedsub = ((ToolSchedSubModel)(selecteditem)).DeepCopy();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand reNumberCommand;
        public ICommand ReNumberCommand { get { return this.reNumberCommand; } }
        //original
        private void ReNumber()
        {
            int ictr = 1;
            decimal decPreviousNo = 0;
            decimal nextRoundValue = 1;
            int c;
            int cnt;
            int i = 0;

            decimal intPreviousNo;
            decimal intPreviousRoundValue;
            decimal intCurrentValue = 1;
            ToolSchedSubModel tss = new ToolSchedSubModel();
            //decimal 
            try
            {
                ToolScheduleDetails = (from row in ToolScheduleDetailsAll
                                       where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
                                       row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
                                       row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue() &&
                                       row.SUB_HEADING_NO == ToolSchedModel.SubHeadingNo.ToDecimalValue() &&
                                       row.SNO != null
                                       orderby (row.SNO.ToDecimalValue() == 0 ? Int64.MaxValue : row.SNO.ToDecimalValue())
                                       select row).ToList<ToolSchedSubModel>();

                /*
                if(ToolScheduleDetails[ictr+1].SNO == )
                if (decPreviousNo == tss.SNO)
                {
                    tss.SNO = previousRoundValue + Convert.ToDecimal(0.001);
                    previousRoundValue = Convert.ToDecimal(tss.SNO);
                }
                else
                {
                    tss.SNO = ictr;
                    ictr = ictr + 1;
                    previousRoundValue = ictr;
                }
                decPreviousNo = Convert.ToDecimal(tss.SNO);
                */


                c = ToolScheduleDetails.Count;
                cnt = 1;
                intPreviousNo = 0;
                intPreviousRoundValue = 0;
                if (c == 0) { c = -1; }
                i = 0;
                ////for (i = c - 1; i >= 0; i--)
                ////{
                ////    intCurrentValue = Convert.ToDecimal(ToolScheduleDetails[i].SNO);
                ////    if (cnt == 1 || (intCurrentValue != Math.Round(intPreviousNo)) || Convert.ToDecimal(intCurrentValue) == 0)
                ////    {
                ////        ToolScheduleDetails[i].SNO = (1000 - (cnt * 10)).ToValueAsString();
                ////        intPreviousRoundValue = 1000 - (cnt * 10);
                ////        cnt = cnt + 1;
                ////    }
                ////    else
                ////    {
                ////        intPreviousRoundValue = intPreviousRoundValue - Convert.ToDecimal(0.001);
                ////        ToolScheduleDetails[i].SNO = intPreviousRoundValue.ToValueAsString();
                ////    }
                ////    intPreviousNo = intCurrentValue;
                ////}

                intPreviousNo = 0;
                intPreviousRoundValue = 0;
                i = 0;
                for (i = 0; i < ToolScheduleDetails.Count; i++)
                {
                    intCurrentValue = Convert.ToDecimal(ToolScheduleDetails[i].SNO);

                    if (i == 0 || (Math.Truncate(intCurrentValue) != Math.Truncate(intPreviousNo)))
                    {
                        ToolScheduleDetails[i].SNO = (Math.Truncate(intPreviousRoundValue) + 1).ToString();
                        intPreviousRoundValue = (Math.Truncate(intPreviousRoundValue) + 1);
                        intPreviousNo = intCurrentValue;
                    }

                    else
                    {
                        intPreviousRoundValue = intPreviousRoundValue + Convert.ToDecimal(0.001);  //original
                        ToolScheduleDetails[i].SNO = intPreviousRoundValue.ToString();
                    }

                    //.Update
                    //.MoveFirst
                    //If i < c - 1 Then .Move i + 1
                }
                ToolScheduleDetailsAll = (from row in ToolScheduleDetailsAll
                                          orderby (row.SNO.ToDecimalValue() == 0 ? Int64.MaxValue : row.SNO.ToDecimalValue())
                                          select row).ToList<ToolSchedSubModel>();


                ToolScheduleDetails = (from row in ToolScheduleDetailsAll
                                       where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
                                       row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
                                       row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue() &&
                                       row.SUB_HEADING_NO == ToolSchedModel.SubHeadingNo.ToDecimalValue()
                                       select row).ToList<ToolSchedSubModel>();
                AddBlankRowToolSchedDetails();
                NotifyPropertyChanged("ToolScheduleDetails");
            }
            catch (Exception ex)
            {
                //throw ex.LogException();
            }
        }

        ////new by nandhu
        //private void ReNumber()
        //{
        //    int ictr = 1;
        //    decimal decPreviousNo = 0;
        //    decimal nextRoundValue = 1;
        //    int c;
        //    int cnt;
        //    int i = 0;

        //    decimal intPreviousNo;
        //    decimal intPreviousRoundValue;
        //    decimal intCurrentValue = 1;
        //    ToolSchedSubModel tss = new ToolSchedSubModel();
        //    //decimal 
        //    try
        //    {
        //        ToolScheduleDetails = (from row in ToolScheduleDetailsAll
        //                               where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
        //                               row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
        //                               row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue() &&
        //                               row.SUB_HEADING_NO == ToolSchedModel.SubHeadingNo.ToDecimalValue() &&
        //                               row.SNO != null
        //                               orderby (row.SNO.ToDecimalValue() == 0 ? Int64.MaxValue : row.SNO.ToDecimalValue())
        //                               select row).ToList<ToolSchedSubModel>();

        //        /*
        //        if(ToolScheduleDetails[ictr+1].SNO == )
        //        if (decPreviousNo == tss.SNO)
        //        {
        //            tss.SNO = previousRoundValue + Convert.ToDecimal(0.001);
        //            previousRoundValue = Convert.ToDecimal(tss.SNO);
        //        }
        //        else
        //        {
        //            tss.SNO = ictr;
        //            ictr = ictr + 1;
        //            previousRoundValue = ictr;
        //        }
        //        decPreviousNo = Convert.ToDecimal(tss.SNO);
        //        */


        //        c = ToolScheduleDetails.Count;
        //        cnt = 1;
        //        intPreviousNo = 0;
        //        intPreviousRoundValue = 0;
        //        if (c == 0) { c = -1; }
        //        i = 0;
        //        ////for (i = c - 1; i >= 0; i--)
        //        ////{
        //        ////    intCurrentValue = Convert.ToDecimal(ToolScheduleDetails[i].SNO);
        //        ////    if (cnt == 1 || (intCurrentValue != Math.Round(intPreviousNo)) || Convert.ToDecimal(intCurrentValue) == 0)
        //        ////    {
        //        ////        ToolScheduleDetails[i].SNO = (1000 - (cnt * 10)).ToValueAsString();
        //        ////        intPreviousRoundValue = 1000 - (cnt * 10);
        //        ////        cnt = cnt + 1;
        //        ////    }
        //        ////    else
        //        ////    {
        //        ////        intPreviousRoundValue = intPreviousRoundValue - Convert.ToDecimal(0.001);
        //        ////        ToolScheduleDetails[i].SNO = intPreviousRoundValue.ToValueAsString();
        //        ////    }
        //        ////    intPreviousNo = intCurrentValue;
        //        ////}

        //        intPreviousNo = 0;
        //        intPreviousRoundValue = 0;
        //        i = 0;
        //        for (i = 0; i < ToolScheduleDetails.Count; i++)
        //        {
        //            intCurrentValue = Convert.ToDecimal(ToolScheduleDetails[i].SNO);

        //            if (i == 0 || (Math.Truncate(intCurrentValue) != Math.Truncate(intPreviousNo))) //original 
        //            //if (i == 0 || (Math.Truncate(intCurrentValue) != Math.Truncate(intPreviousNo)) && ((!ToolScheduleDetails[i].TOOL_DESC.Contains("consists"))))  //new
        //            {
        //                ToolScheduleDetails[i].SNO = (Math.Truncate(intPreviousRoundValue) + 1).ToString();
        //                intPreviousRoundValue = (Math.Truncate(intPreviousRoundValue) + 1);
        //                intPreviousNo = intCurrentValue;
        //            }
        //            //original
        //            //else
        //            //{
        //            //    intPreviousRoundValue = intPreviousRoundValue + Convert.ToDecimal(0.001);  //original
        //            //    ToolScheduleDetails[i].SNO = intPreviousRoundValue.ToString();
        //            //}
        //            //end original

        //            //newby nandhu
        //            else if ((ToolScheduleDetails[i].TOOL_DESC.Contains("consists")) || (TOOL_DESC_temp.Contains("consists")))
        //            //else if (TOOL_DESC_temp.Contains("consists"))

        //            {
        //                intPreviousRoundValue = intPreviousRoundValue + Convert.ToDecimal(0.001);
        //                ToolScheduleDetails[i].SNO = intPreviousRoundValue.ToString();
        //            }
        //            else
        //            {
        //                intPreviousRoundValue = intPreviousRoundValue + Convert.ToInt32(1);
        //                ToolScheduleDetails[i].SNO = intPreviousRoundValue.ToString();
        //            }
        //            //end new nandhu

        //            //.Update
        //            //.MoveFirst
        //            //If i < c - 1 Then .Move i + 1
        //        }
        //        ToolScheduleDetailsAll = (from row in ToolScheduleDetailsAll
        //                                  orderby (row.SNO.ToDecimalValue() == 0 ? Int64.MaxValue : row.SNO.ToDecimalValue())
        //                                  select row).ToList<ToolSchedSubModel>();


        //        ToolScheduleDetails = (from row in ToolScheduleDetailsAll
        //                               where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
        //                               row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
        //                               row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue() &&
        //                               row.SUB_HEADING_NO == ToolSchedModel.SubHeadingNo.ToDecimalValue()
        //                               select row).ToList<ToolSchedSubModel>();
        //        AddBlankRowToolSchedDetails();
        //        NotifyPropertyChanged("ToolScheduleDetails");
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw ex.LogException();
        //    }
        //}
        //new by nandhu

        private void ReNumberAll()
        {
            int ictr = 1;
            decimal decPreviousNo = 0;
            decimal nextRoundValue = 1;
            int c;
            int cnt;
            int i = 0;

            decimal intPreviousNo;
            decimal intPreviousRoundValue;
            decimal intCurrentValue = 1;
            ToolSchedSubModel tss = new ToolSchedSubModel();
            //decimal 
            try
            {
                foreach (TOOL_SCHED_MAIN tsm in ToolScheduleMainAll)
                {
                    ToolScheduleDetails = (from row in ToolScheduleDetailsAll
                                           where row.ROUTE_NO == tsm.ROUTE_NO &&
                                           row.SEQ_NO == tsm.SEQ_NO &&
                                           row.CC_SNO == tsm.CC_SNO &&
                                           row.SUB_HEADING_NO == tsm.SUB_HEADING_NO &&
                                           row.SNO != null
                                           orderby (row.SNO.ToDecimalValue() == 0 ? Int64.MaxValue : row.SNO.ToDecimalValue())
                                           select row).ToList<ToolSchedSubModel>();

                    if (ToolScheduleDetails.Count > 0)
                    {
                        c = ToolScheduleDetails.Count;
                        cnt = 1;
                        intPreviousNo = 0;
                        intPreviousRoundValue = 0;
                        if (c == 0) { c = -1; }
                        i = 0;
                        intPreviousNo = 0;
                        intPreviousRoundValue = 0;
                        i = 0;
                        for (i = 0; i < ToolScheduleDetails.Count; i++)
                        {
                            intCurrentValue = Convert.ToDecimal(ToolScheduleDetails[i].SNO);

                            if (i == 0 || (Math.Truncate(intCurrentValue) != Math.Truncate(intPreviousNo)))
                            {
                                ToolScheduleDetails[i].SNO = (Math.Truncate(intPreviousRoundValue) + 1).ToString();
                                intPreviousRoundValue = (Math.Truncate(intPreviousRoundValue) + 1);
                                intPreviousNo = intCurrentValue;
                            }
                            else
                            {
                                intPreviousRoundValue = intPreviousRoundValue + Convert.ToDecimal(0.001);
                                ToolScheduleDetails[i].SNO = intPreviousRoundValue.ToString();
                            }
                        }
                        ToolScheduleDetailsAll = (from row in ToolScheduleDetailsAll
                                                  orderby (row.SNO.ToDecimalValue() == 0 ? Int64.MaxValue : row.SNO.ToDecimalValue())
                                                  select row).ToList<ToolSchedSubModel>();


                        ToolScheduleDetails = (from row in ToolScheduleDetailsAll
                                               where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
                                               row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
                                               row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue() &&
                                               row.SUB_HEADING_NO == ToolSchedModel.SubHeadingNo.ToDecimalValue()
                                               select row).ToList<ToolSchedSubModel>();
                        AddBlankRowToolSchedDetails();
                        NotifyPropertyChanged("ToolScheduleDetails");
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex.LogException();
            }
        }

        private readonly ICommand copyStatusCommand;
        public ICommand CopyStatusCommand { get { return this.copyStatusCommand; } }
        private void CopyStatus()
        {
            try
            {
                if (ToolSchedModel.PartNo.ToValueAsString().Trim() != "")
                {
                    frmCopyStatus copyStatus = new frmCopyStatus("ToolSchedule", ToolSchedModel.PartNo.ToValueAsString().Trim(), "", "", "", "");
                    copyStatus.ShowDialog();
                }
                else
                {
                    ShowInformationMessage("Please select the Part No.!");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand deleteToolScheduleSubCommand;
        public ICommand DeleteToolScheduleSubCommand { get { return this.deleteToolScheduleSubCommand; } }
        private void DeleteToolScheduleSub()
        {
            try
            {
                if (SelectedToolSchedSub != null)
                {
                    //Bug Id : 693
                    if (SelectedToolSchedSub.TOOL_CODE.ToValueAsString().Trim() == "" && SelectedToolSchedSub.SNO.ToValueAsString().Trim() == "" && SelectedToolSchedSub.TOOL_CODE.ToValueAsString().Trim() == ""
                        && SelectedToolSchedSub.TOOL_CODE_END.ToValueAsString().Trim() == "" && SelectedToolSchedSub.TOOL_DESC.ToValueAsString().Trim() == "" && SelectedToolSchedSub.CATEGORY.ToValueAsString().Trim() == ""
                        && SelectedToolSchedSub.QTY.ToValueAsString().Trim() == "" && SelectedToolSchedSub.REMARKS.ToValueAsString().Trim() == "")
                    {
                        if (ToolScheduleDetails.Count > 0)
                        {
                            if (SelectedToolSchedSub.IDPK == ToolScheduleDetails[ToolScheduleDetails.Count - 1].IDPK)
                            {
                                return;
                            }
                        }
                    }
                    if (ShowQuestionMessage(PDMsg.SelectDeleteRecord) == MessageBoxResult.OK)
                    {
                        if (SelectedToolSchedSub.IDPK > 0)
                        {
                            //_copyToolSchedSub
                            DeleteToolScheduleSubAll.Add(SelectedToolSchedSub);
                            //int ictr = 0;
                            //while (ictr <= _copyToolSchedSub.Count - 1)
                            //{
                            //    if (_copyToolSchedSub[ictr].IDPK == SelectedToolSchedSub.IDPK)
                            //    {
                            //        _copyToolSchedSub.RemoveAt(ictr);
                            //    }
                            //    else
                            //    {
                            //        ictr = ictr + 1;
                            //    }
                            //}
                        }
                        ToolSchedSubModel tsm = new ToolSchedSubModel();
                        tsm.CATEGORY = SelectedToolSchedSub.CATEGORY;
                        tsm.CC_SNO = SelectedToolSchedSub.CC_SNO;
                        tsm.IDPK = SelectedToolSchedSub.IDPK;
                        tsm.PART_NO = SelectedToolSchedSub.PART_NO;
                        tsm.QTY = SelectedToolSchedSub.QTY;
                        tsm.REMARKS = SelectedToolSchedSub.REMARKS;
                        tsm.ROUTE_NO = SelectedToolSchedSub.ROUTE_NO;
                        tsm.ROWID = SelectedToolSchedSub.ROWID;
                        tsm.SEQ_NO = SelectedToolSchedSub.SEQ_NO;
                        tsm.SNO = SelectedToolSchedSub.SNO;
                        tsm.SUB_HEADING_NO = SelectedToolSchedSub.SUB_HEADING_NO;
                        tsm.TOOL_CODE = SelectedToolSchedSub.TOOL_CODE;
                        tsm.TOOL_CODE_END = SelectedToolSchedSub.TOOL_CODE_END;
                        tsm.TOOL_DESC = SelectedToolSchedSub.TOOL_DESC;
                        tsm.Delete = 1;
                        _copyToolSchedSub.Add(tsm);
                        ToolScheduleDetailsAll.Remove(SelectedToolSchedSub);
                        FilterToolSchedule();
                        if (ToolScheduleDetails != null)
                        {
                            if (ToolScheduleDetails.Count > 0)
                            {
                                SelectedToolSchedSub = ToolScheduleDetails[0];
                                NotifyPropertyChanged("SelectedToolSchedSub");
                                SelectionChangedToolSchedule(SelectedToolSchedSub);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void MultipleDeleteToolScheduleSub()
        {
            try
            {
                List<ToolSchedSubModel> lstTssm;
                int label = 0;
                if (_selectedItemsToolSchedSubModel != null)
                {
                    //Bug Id : 707
                    lstTssm = new List<ToolSchedSubModel>() { };
                    //System.Windows.Controls.sele objs = _selectedItemsToolSchedSubModel;

                    System.Collections.IList items = (System.Collections.IList)_selectedItemsToolSchedSubModel;
                    IEnumerable<ToolSchedSubModel> collection = items.Cast<ToolSchedSubModel>();

                    lstTssm = collection.ToList<ToolSchedSubModel>();
                    if (lstTssm.Count == 0) { return; }
                    if (ShowQuestionMessage(lstTssm.Count + " record(s) will be deleted." + PDMsg.SelectDeleteRecord) == MessageBoxResult.Cancel)
                    {
                        return;
                    }

                    foreach (ToolSchedSubModel tssm in lstTssm)
                    {
                        //ToolSchedSubModel tssm = (ToolSchedSubModel)obj;
                        if (tssm.TOOL_CODE.ToValueAsString().Trim() == "" && tssm.SNO.ToValueAsString().Trim() == "" && tssm.TOOL_CODE.ToValueAsString().Trim() == ""
                            && tssm.TOOL_CODE_END.ToValueAsString().Trim() == "" && tssm.TOOL_DESC.ToValueAsString().Trim() == "" && tssm.CATEGORY.ToValueAsString().Trim() == ""
                            && tssm.QTY.ToValueAsString().Trim() == "" && tssm.REMARKS.ToValueAsString().Trim() == "")
                        {
                            if (ToolScheduleDetails.Count > 0)
                            {
                                if (tssm.IDPK == ToolScheduleDetails[ToolScheduleDetails.Count - 1].IDPK)
                                {
                                    goto NextSchedule;
                                }
                            }
                        }
                        if (tssm.IDPK > 0)
                        {
                            DeleteToolScheduleSubAll.Add(tssm);
                            //int ictr = 0;
                            //while (ictr <= _copyToolSchedSub.Count - 1)
                            //{
                            //    if (_copyToolSchedSub[ictr].IDPK == tssm.IDPK)
                            //    {
                            //        _copyToolSchedSub.RemoveAt(ictr);
                            //    }
                            //    else
                            //    {
                            //        ictr = ictr + 1;
                            //    }
                        }

                        ToolSchedSubModel tsmdel = new ToolSchedSubModel();
                        tsmdel.CATEGORY = tssm.CATEGORY;
                        tsmdel.CC_SNO = tssm.CC_SNO;
                        tsmdel.IDPK = tssm.IDPK;
                        tsmdel.PART_NO = tssm.PART_NO;
                        tsmdel.QTY = tssm.QTY;
                        tsmdel.REMARKS = tssm.REMARKS;
                        tsmdel.ROUTE_NO = tssm.ROUTE_NO;
                        tsmdel.ROWID = tssm.ROWID;
                        tsmdel.SEQ_NO = tssm.SEQ_NO;
                        tsmdel.SNO = tssm.SNO;
                        tsmdel.SUB_HEADING_NO = tssm.SUB_HEADING_NO;
                        tsmdel.TOOL_CODE = tssm.TOOL_CODE;
                        tsmdel.TOOL_CODE_END = tssm.TOOL_CODE_END;
                        tsmdel.TOOL_DESC = tssm.TOOL_DESC;
                        tsmdel.Delete = 1;
                        _copyToolSchedSub.Add(tsmdel);
                        ToolScheduleDetailsAll.Remove(tssm);
                    NextSchedule:
                        label = label + 1;
                    }
                    FilterToolSchedule();
                    if (ToolScheduleDetails != null)
                    {
                        if (ToolScheduleDetails.Count > 0)
                        {
                            SelectedToolSchedSub = ToolScheduleDetails[0];
                            NotifyPropertyChanged("SelectedToolSchedSub");
                            SelectionChangedToolSchedule(SelectedToolSchedSub);
                        }
                    }
                    MessageBox.Show(lstTssm.Count + " record(s) deleted.");
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }






        private readonly ICommand deleteToolScheduleAuxCommand;
        public ICommand DeleteToolScheduleAuxCommand { get { return this.deleteToolScheduleAuxCommand; } }
        private void DeleteToolScheduleAux()
        {
            try
            {
                if (SelectedToolSchedAux != null)
                {
                    //Bug Id : 693
                    if (SelectedToolSchedAux.TOOL_CODE.ToValueAsString().Trim() == ""
                            && SelectedToolSchedAux.CATEGORY.ToValueAsString().Trim() == ""
                            && SelectedToolSchedAux.TEMPLATE_CD.ToValueAsString().Trim() == ""
                            && SelectedToolSchedAux.TOOL_DESC.ToValueAsString() == "")
                    {
                        if (AuxToolScheduleDetails.Count > 0)
                        {
                            if (SelectedToolSchedAux.IDPK == AuxToolScheduleDetails[AuxToolScheduleDetails.Count - 1].IDPK)
                            {
                                return;
                            }
                        }
                    }

                    if (ShowQuestionMessage(PDMsg.SelectDeleteRecord) == MessageBoxResult.OK)
                    {
                        if (SelectedToolSchedAux.IDPK > 0)
                        {
                            DeleteToolScheduleAuxAll.Add(SelectedToolSchedAux);
                        }
                        AuxToolScheduleDetailsAll.Remove(SelectedToolSchedAux);
                        if (_currenttoolscedsub != null)
                        {
                            FilterAuxToolSchedule(_currenttoolscedsub.TOOL_CODE);
                        }
                        else
                        {
                            FilterAuxToolSchedule("");
                        }
                        if (AuxToolScheduleDetails.Count > 0)
                        {
                            SelectedToolSchedAux = AuxToolScheduleDetails[0];
                            NotifyPropertyChanged("SelectedToolSchedAux");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void MultipleDeleteToolScheduleAux()
        {
            try
            {
                List<AUX_TOOL_SCHED> lstAts;
                int label = 0;
                if (_selectedItemsAuxToolSchedSubModel != null)
                {
                    lstAts = new List<AUX_TOOL_SCHED>() { };
                    System.Collections.IList items = (System.Collections.IList)_selectedItemsAuxToolSchedSubModel;
                    IEnumerable<AUX_TOOL_SCHED> collection = items.Cast<AUX_TOOL_SCHED>();

                    lstAts = collection.ToList<AUX_TOOL_SCHED>();
                    if (lstAts.Count == 0) { return; }
                    if (ShowQuestionMessage(lstAts.Count + " record(s) will be deleted." + PDMsg.SelectDeleteRecord) == MessageBoxResult.Cancel)
                    {
                        return;
                    }


                    foreach (AUX_TOOL_SCHED ats in lstAts)
                    {

                        //Bug Id : 707
                        if (ats.TOOL_CODE.ToValueAsString().Trim() == ""
                                && ats.CATEGORY.ToValueAsString().Trim() == ""
                                && ats.TEMPLATE_CD.ToValueAsString().Trim() == ""
                                && ats.TOOL_DESC.ToValueAsString() == "")
                        {
                            if (AuxToolScheduleDetails.Count > 0)
                            {
                                if (ats.IDPK == AuxToolScheduleDetails[AuxToolScheduleDetails.Count - 1].IDPK)
                                {
                                    goto NextRecord;
                                }
                            }
                        }
                        if (ats.IDPK > 0)
                        {
                            DeleteToolScheduleAuxAll.Add(ats);
                        }
                        AuxToolScheduleDetailsAll.Remove(ats);
                    NextRecord:
                        label = label + 1;
                    }
                    if (_currenttoolscedsub != null)
                    {
                        FilterAuxToolSchedule(_currenttoolscedsub.TOOL_CODE);
                    }
                    else
                    {
                        FilterAuxToolSchedule("");
                    }
                    if (AuxToolScheduleDetails.Count > 0)
                    {
                        SelectedToolSchedAux = AuxToolScheduleDetails[0];
                        NotifyPropertyChanged("SelectedToolSchedAux");
                    }
                    MessageBox.Show(lstAts.Count + " record(s) deleted.");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand deleteToolScheduleIssueCommand;
        public ICommand DeleteToolScheduleIssueCommand { get { return this.deleteToolScheduleIssueCommand; } }
        private void DeleteToolScheduleIssue()
        {
            try
            {
                if (SelectedToolSchedIssue != null)
                {
                    //Bug Id : 693
                    if (SelectedToolSchedIssue.TS_ISSUE_NO.ToValueAsString().Trim() == ""
                    && SelectedToolSchedIssue.TS_ISSUE_DATE.ToValueAsString() == ""
                    && SelectedToolSchedIssue.TS_ISSUE_ALTER.ToValueAsString() == ""
                    && SelectedToolSchedIssue.TS_COMPILED_BY.ToValueAsString() == "")
                    {
                        if (SelectedToolSchedIssue.IDPK == ToolScheduleRevision[ToolScheduleRevision.Count - 1].IDPK)
                        {
                            return;
                        }
                    }

                    if (ShowQuestionMessage(PDMsg.SelectDeleteRecord) == MessageBoxResult.OK)
                    {
                        if (SelectedToolSchedIssue.IDPK > 0)
                        {
                            DeleteToolScheduleIssueAll.Add(SelectedToolSchedIssue);
                        }
                        ToolScheduleRevisionAll.Remove(SelectedToolSchedIssue);
                        FilterToolScheduleIssue();
                        if (ToolScheduleRevision != null)
                        {
                            if (ToolScheduleRevision.Count > 0)
                            {
                                SelectedToolSchedIssue = ToolScheduleRevision[0];
                                NotifyPropertyChanged("SelectedToolSchedIssue");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void MultipleDeleteToolScheduleIssue()
        {
            try
            {
                List<TOOL_SCHED_ISSUE> lstTsi;
                int label = 0;
                if (_selectedItemsToolScheduleIssue != null)
                {
                    //Bug Id : 707
                    lstTsi = new List<TOOL_SCHED_ISSUE>() { };
                    System.Collections.IList items = (System.Collections.IList)_selectedItemsToolScheduleIssue;
                    IEnumerable<TOOL_SCHED_ISSUE> collection = items.Cast<TOOL_SCHED_ISSUE>();
                    lstTsi = collection.ToList<TOOL_SCHED_ISSUE>();
                    if (lstTsi.Count == 0) { return; }
                    if (ShowQuestionMessage(lstTsi.Count + " record(s) will be deleted." + PDMsg.SelectDeleteRecord) == MessageBoxResult.Cancel)
                    {
                        return;
                    }

                    foreach (TOOL_SCHED_ISSUE tsi in lstTsi)
                    {
                        if (tsi.TS_ISSUE_NO.ToValueAsString().Trim() == ""
                        && tsi.TS_ISSUE_DATE.ToValueAsString() == ""
                        && tsi.TS_ISSUE_ALTER.ToValueAsString() == ""
                        && tsi.TS_COMPILED_BY.ToValueAsString() == "")
                        {
                            if (tsi.IDPK == ToolScheduleRevision[ToolScheduleRevision.Count - 1].IDPK)
                            {
                                goto NextRecord;
                            }
                        }

                        if (tsi.IDPK > 0)
                        {
                            DeleteToolScheduleIssueAll.Add(tsi);
                        }
                        ToolScheduleRevisionAll.Remove(tsi);
                    NextRecord:
                        label = label + 1;
                    }
                    FilterToolScheduleIssue();
                    if (ToolScheduleRevision != null)
                    {
                        if (ToolScheduleRevision.Count > 0)
                        {
                            SelectedToolSchedIssue = ToolScheduleRevision[0];
                            NotifyPropertyChanged("SelectedToolSchedIssue");
                        }
                    }
                    MessageBox.Show(lstTsi.Count + " record(s) deleted.");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand editClickCommand;
        public ICommand EditClickCommand { get { return this.editClickCommand; } }
        private void EditClick()
        {
            try
            {
                ((System.Windows.Controls.TextBox)partNoComboBoxCus.FindName("txtCombobox")).Focus();
                //ToolSchedModel = new ToolScheduleModel();
                //ClearAll();
            }
            catch (Exception ex)
            {
                //throw ex.LogException();
            }
        }


        private bool Validate()
        {
            try
            {
                if (!_toolSchedModel.PartNo.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                    FocusPartNo();
                    return false;
                }
                else if (!_toolSchedModel.RouteNo.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Process No"));
                    return false;
                }
                else if (!_toolSchedModel.SeqNo.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Sequence No"));
                    return false;
                }
                else if (!_toolSchedModel.CCSno.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Cost Center"));
                    return false;
                }
                else if (CmbSubHeadingCombo.SelectedText.Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Sub Heading"));
                    return false;
                }

                if (CheckBlankSno() == false)
                {
                    return false;
                }
                if (CheckBlankToolCode() == false)
                {
                    return false;
                }
                if (CheckDuplicateToolCode() == false)
                {
                    return false;
                }
                if (CheckDuplicateSno() == false)
                {
                    return false;
                }
                if (CheckBlankAuxToolCode() == false)
                {
                    return false;
                }
                if (CheckDuplicateAuxTooNo() == false)
                {
                    return false;
                }
                if (CheckBlankIssueNo() == false)
                {
                    return false;
                }
                if (CheckDuplicateIssueNo() == false)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return true;
                throw ex.LogException();
            }
            return true;
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }


        private void ClearAll()
        {
            try
            {
                CostCentreCombo = new DataView();
                SequenceNoCombo = new DataView();
                SubHeadingCombo = new DataView();
                ProcessNoCombo = new DataView();
                ToolSchedModel.PartNo = "";
                ToolSchedModel.RouteNo = "";
                ToolSchedModel.SeqNo = "";
                ToolSchedModel.CCSno = "";
                ToolSchedModel.CCCode = "";
                ToolSchedModel.SubHeadingNo = "";
                ToolSchedModel.SubHeading = "";
                ToolSchedModel.BotNote = "";
                ToolSchedModel.TopNote = "";
                ToolSchedModel.PartDescription = "";
                if (CmbSubHeadingCombo != null)
                {
                    FilterToolScheduleAndSelect();
                }
                NotifyPropertyChanged("ToolSchedModel");
                _mdiChild.Title = ApplicationTitle + " - Tool Schedule" + ((ToolSchedModel.PartNo.IsNotNullOrEmpty()) ? " - " + ToolSchedModel.PartNo : "");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }



        private readonly ICommand assignNoteCommand;
        public ICommand AssignNoteCommand { get { return this.assignNoteCommand; } }
        private void AssignNote()
        {
            TOOL_SCHED_MAIN entity = new TOOL_SCHED_MAIN();
            try
            {
                if (ToolSchedModel.RouteNo.ToValueAsString().Trim() != "" &&
                   ToolSchedModel.SeqNo.ToValueAsString().Trim() != "" &&
                    ToolSchedModel.CCSno.ToValueAsString().Trim() != "" &&
                    ToolSchedModel.SubHeadingNo.ToValueAsString().Trim() != "" &&
                    ToolSchedModel.PartNo.ToValueAsString().Trim() != "")
                {
                    entity = (from row in ToolScheduleMainAll
                              where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
                             row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
                             row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue() &&
                             row.SUB_HEADING_NO == ToolSchedModel.SubHeadingNo.ToDecimalValue() &&
                             row.PART_NO == ToolSchedModel.PartNo.ToValueAsString()
                              select row).FirstOrDefault();
                    if (entity != null)
                    {
                        entity.TOP_NOTE = ToolSchedModel.TopNote.ToValueAsString().Trim();
                        entity.BOT_NOTE = ToolSchedModel.BotNote.ToValueAsString().Trim();
                    }
                    else
                    {
                        entity.TOP_NOTE = "";
                        entity.BOT_NOTE = "";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void SetNote()
        {
            TOOL_SCHED_MAIN entity = new TOOL_SCHED_MAIN();
            try
            {
                entity = (from row in ToolScheduleMainAll
                          where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
                         row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
                         row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue() &&
                         row.SUB_HEADING_NO == ToolSchedModel.SubHeadingNo.ToDecimalValue() &&
                         row.PART_NO == ToolSchedModel.PartNo
                          select row).FirstOrDefault();
                if (entity != null)
                {
                    ToolSchedModel.TopNote = "";
                    ToolSchedModel.BotNote = "";
                    ToolSchedModel.TopNote = entity.TOP_NOTE.ToValueAsString().Trim();
                    ToolSchedModel.BotNote = entity.BOT_NOTE.ToValueAsString().Trim();
                }
                else
                {
                    ToolSchedModel.TopNote = "";
                    ToolSchedModel.BotNote = "";
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        //private void AssignSubHeading() // original
        public void AssignSubHeading()
        {

            //List<TOOL_SCHED_MAIN> ToolScheduleMainAll;
            //TOOL_SCHED_MAIN toolSchd;
            //if (CmbSubHeadingCombo.SelectedValue != null)
            //if(!IsNotNullOrEmpty(CmbSubHeadingCombo.SelectedValue))
            //new
            if (GetSub_heading_no != 0)  //new
            {
                _toolScheduleBll.setsubheadingno(GetSub_heading_no);
            }

            //_toolScheduleBll.setvalue(Getcc_sno);
            //end new
            SubHeadingCombo = _toolScheduleBll.GetToolSubHeading(ToolSchedModel);

            if (SubHeadingCombo.Count > 0)
            {
                if (ToolScheduleDetailsAll.Count == 0) //new
                {
                    ToolSchedModel.SubHeadingNo = SubHeadingCombo.Table.Rows[0]["SUB_HEADING_NO"].ToValueAsString();
                    ToolSchedModel.SubHeading = SubHeadingCombo.Table.Rows[0]["SUB_HEADING"].ToValueAsString();   //original
                }
                //new by nandhini
                //ToolSchedModel.SubHeadingNo = ToolScheduleMainAll[0].SUB_HEADING_NO.ToString();
                //ToolSchedModel.SubHeading = ToolScheduleMainAll[0].SUB_HEADING.ToString();
                //end new
                //new
                else if (ToolScheduleDetailsAll.Count > 0)  //new
                {  //new


                    foreach (TOOL_SCHED_MAIN toolSchd in ToolScheduleMainAll) //original
                    //foreach (toolSchd in ToolScheduleMainAll)
                    {
                        //if (ToolScheduleDetailsAll[0].SUB_HEADING_NO.ToString() == toolSchd.SUB_HEADING_NO.ToString() && ToolScheduleDetailsAll[0].SEQ_NO == GetSeq_no)  //original
                        if (ToolScheduleDetailsAll[0].SUB_HEADING_NO.ToString() == toolSchd.SUB_HEADING_NO.ToString() && toolSchd.SEQ_NO == GetSeq_no && ToolScheduleDetailsAll[0].ROUTE_NO == toolSchd.ROUTE_NO)
                        {
                            ToolSchedModel.SubHeadingNo = toolSchd.SUB_HEADING_NO.ToString();
                            ToolSchedModel.SubHeading = toolSchd.SUB_HEADING.ToString();
                            break;
                        }
                    }
                }  //new

                //end new

                TextSubHeading = "Sub Heading:(" + ToolSchedModel.SubHeadingNo + ")";
            }
            else
            {
                TextSubHeading = "Sub Heading:(1)";
                ToolSchedModel.SubHeadingNo = "1";
                ToolSchedModel.SubHeading = "";
                CmbSubHeadingCombo.SelectedText = "";
            }
            GetSub_heading_no = 0;

            NotifyPropertyChanged("TextSubHeading");
            NotifyPropertyChanged("SubHeadingCombo");
            AddToolScheduleMain();
            SetNote();
            NotifyPropertyChanged("ToolSchedModel");
        }

        public void LoadMethod(object sender, EventArgs e)
        {
            try
            {
                partNoComboBoxCus = ((ComboBoxCus)((System.Windows.Controls.UserControl)sender).FindName("cmbPartNo"));
                FocusPartNo();
            }
            catch (Exception ex)
            {

            }
        }


        private readonly ICommand selectChangeComboSeqNoCommand;
        public ICommand SelectChangeComboSeqNoCommand { get { return this.selectChangeComboSeqNoCommand; } }
        private void SelectChangeComboSeqNo()
        {
            if (blnExecuteCombo == false) return;
            try
            {
                blnExecuteCombo = false;
                LoadComboAndGridFromProcess(SelectedRowSeqNo);
                LoadComboAndGridFromSeqNo();

                SelectTheGridRow();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                blnExecuteCombo = true;
            }
        }

        private readonly ICommand selectChangeComboCostCentreCommand;
        public ICommand SelectChangeComboCostCentreCommand { get { return this.selectChangeComboCostCentreCommand; } }
        private void SelectChangeComboCostCentre()
        {
            if (blnExecuteCombo == false) return;
            try
            {
                blnExecuteCombo = false;
                LoadComboAndGridFromCostCentre();
                SelectTheGridRow();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                blnExecuteCombo = true;
            }
        }

        private readonly ICommand selectChangeComboProcessNoCommand;
        public ICommand SelectChangeComboProcessNoCommand { get { return this.selectChangeComboProcessNoCommand; } }
        private void SelectChangeComboProcessNo()
        {
            if (blnExecuteCombo == false) return;
            try
            {
                blnExecuteCombo = false;
                SequenceNoCombo = new DataView();
                //DataView TmpSequenceNoCombo = new DataView();
                SequenceNoCombo = _toolScheduleBll.GetSeqNo(ToolSchedModel);
                //SequenceNoCombo.Sort = "SEQ_NO ASC";
                int tenthSeqIndex = 0;
                int twenthSeqIndex = 0;
                // added by Jeyan
                for (int ind = 0; ind <= SequenceNoCombo.ToTable().Rows.Count - 1; ind++)
                {
                    if (SequenceNoCombo.Table.Rows[ind]["SEQ_NO"].ToString() == "10")
                    {
                        tenthSeqIndex = ind;
                    }
                    else if (SequenceNoCombo.Table.Rows[ind]["SEQ_NO"].ToString() == "20")
                    {
                        twenthSeqIndex = ind;
                    }
                }
                if (SequenceNoCombo.Count > 0)
                {
                    //TmpSequenceNoCombo = SequenceNoCombo;
                    //TmpSequenceNoCombo.RowFilter = ("SEQ_NO = 20 and OPN_DESC like 'FORGE%'");
                    DataRow[] tmbDtRow = SequenceNoCombo.ToTable().Select("SEQ_NO = '20' and TRIM(OPN_DESC) like 'FORGE%'");
                    //DataRow dr = null;
                    if (tmbDtRow.Length >= 1)
                    {
                        //dr = tmbDtRow[0];
                        //SelectedRowSeqNo = SequenceNoCombo.ToTable().DefaultView[SequenceNoCombo.ToTable().Rows.IndexOf(dr)];
                        SelectedRowSeqNo = SequenceNoCombo[twenthSeqIndex];
                    }
                    else
                    {
                        //tmbDtRow = null;
                        //tmbDtRow = SequenceNoCombo.ToTable().Select("SEQ_NO = '10'");

                        //if (tmbDtRow.Length >= 1)
                        //{
                        // dr = tmbDtRow[0];
                        //SelectedRowSeqNo = SequenceNoCombo.ToTable().DefaultView[SequenceNoCombo.ToTable().Rows.IndexOf(dr)];
                        SelectedRowSeqNo = SequenceNoCombo[tenthSeqIndex];
                        //}
                        //else
                        //{
                        //    SelectedRowSeqNo = SequenceNoCombo[0];
                        //}
                    }
                    // Commented by Jeyan
                    //if (SequenceNoCombo.Count > 1)
                    //{
                    //    SelectedRowSeqNo = SequenceNoCombo[1];
                    //}
                    //else
                    //{
                    //    SelectedRowSeqNo = SequenceNoCombo[0];
                    //}

                }
                LoadComboAndGridFromProcess(SelectedRowSeqNo);
                SelectTheGridRow();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                blnExecuteCombo = true;
            }
        }



        private void FocusPartNo()
        {
            try
            {
                ((System.Windows.Controls.TextBox)partNoComboBoxCus.FindName("txtCombobox")).Focus();
            }
            catch (Exception ex)
            {

            }
        }

        private void AddToolScheduleMain()
        {
            bool add = false;
            try
            {
                if (ToolSchedModel.CCSno.ToValueAsString() != "" &&
                    ToolSchedModel.PartNo != "" &&
                    ToolSchedModel.RouteNo != "" &&
                    ToolSchedModel.SeqNo != "" &&
                    ToolSchedModel.CCSno != "" &&
                    ToolSchedModel.SubHeadingNo != "")
                {

                    TOOL_SCHED_MAIN tsm = new TOOL_SCHED_MAIN();

                    tsm = (from row in ToolScheduleMainAll
                           where row.PART_NO == ToolSchedModel.PartNo
                            && row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue()
                            && row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue()
                            && row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue()
                            && row.SUB_HEADING_NO == ToolSchedModel.SubHeadingNo.ToDecimalValue()
                           select row).FirstOrDefault();
                    if (tsm == null)
                    {
                        add = true;
                        tsm = new TOOL_SCHED_MAIN();
                        tsm.PART_NO = ToolSchedModel.PartNo.ToValueAsString();
                        tsm.ROUTE_NO = ToolSchedModel.RouteNo.ToDecimalValue();
                        tsm.SEQ_NO = ToolSchedModel.SeqNo.ToDecimalValue();
                        tsm.CC_SNO = ToolSchedModel.CCSno.ToDecimalValue();
                        tsm.SUB_HEADING_NO = ToolSchedModel.SubHeadingNo.ToDecimalValue();
                        tsm.SUB_HEADING = ToolSchedModel.SubHeading.ToValueAsString().Trim();
                        tsm.TOP_NOTE = ToolSchedModel.TopNote.ToValueAsString().Trim();
                        tsm.BOT_NOTE = ToolSchedModel.BotNote.ToValueAsString().Trim();
                        ToolScheduleMainAll.Add(tsm);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void UpdateToolScheduleMain()
        {
            bool add = false;
            try
            {
                if (ToolSchedModel.CCSno.ToValueAsString() != "" &&
                    ToolSchedModel.PartNo != "" &&
                    ToolSchedModel.RouteNo != "" &&
                    ToolSchedModel.SeqNo != "" &&
                    ToolSchedModel.CCSno != "" &&
                    ToolSchedModel.SubHeadingNo != "")
                {

                    TOOL_SCHED_MAIN tsm = new TOOL_SCHED_MAIN();

                    tsm = (from row in ToolScheduleMainAll
                           where row.PART_NO == ToolSchedModel.PartNo
                            && row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue()
                            && row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue()
                            && row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue()
                            && row.SUB_HEADING_NO == ToolSchedModel.SubHeadingNo.ToDecimalValue()
                           select row).FirstOrDefault();
                    if (tsm != null)
                    {
                        tsm.SUB_HEADING = ToolSchedModel.SubHeading.ToValueAsString();
                        tsm.TOP_NOTE = ToolSchedModel.TopNote.ToValueAsString();
                        tsm.BOT_NOTE = ToolSchedModel.BotNote.ToValueAsString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }



        private bool CheckDuplicateToolCode()
        {
            try
            {
                var result = ToolScheduleDetails.Where(x => x.TOOL_CODE.ToValueAsString().Trim() != "").GroupBy(x => new { x.PART_NO, x.ROUTE_NO, x.SEQ_NO, x.CC_SNO, x.SUB_HEADING_NO, x.TOOL_CODE })
                   .Select(g => new { g.Key.PART_NO, g.Key.ROUTE_NO, g.Key.SEQ_NO, g.Key.CC_SNO, g.Key.SUB_HEADING_NO, g.Key.TOOL_CODE, Count = g.Count() }).Where(x => x.Count > 1);

                if (result.Count() > 0)
                {
                    ShowInformationMessage("Duplicate Tool Schedule Code has been entered!");
                    return false;
                }

                //var result1 = ToolScheduleDetailsAll.Where(x => x.TOOL_CODE.ToValueAsString().Trim() != "").GroupBy(x => new { x.PART_NO, x.ROUTE_NO, x.SEQ_NO, x.CC_SNO, x.SUB_HEADING_NO, x.TOOL_CODE })
                //    .Select(g => new { g.Key.PART_NO, g.Key.ROUTE_NO, g.Key.SEQ_NO, g.Key.CC_SNO, g.Key.SUB_HEADING_NO, g.Key.TOOL_CODE, Count = g.Count() }).Where(x => x.Count > 1);

                List<ToolSchedSubModel> result1 = ToolScheduleDetailsAll.Where(x => x.TOOL_CODE.ToValueAsString().Trim() != "").GroupBy(x => new { x.PART_NO, x.ROUTE_NO, x.SEQ_NO, x.CC_SNO, x.SUB_HEADING_NO, x.TOOL_CODE })
                    .Select(g => new ToolSchedSubModel() { PART_NO = g.Key.PART_NO, ROUTE_NO = g.Key.ROUTE_NO, SEQ_NO = g.Key.SEQ_NO, CC_SNO = g.Key.CC_SNO, SUB_HEADING_NO = g.Key.SUB_HEADING_NO, TOOL_CODE = g.Key.TOOL_CODE, IDPK = g.Count() }).Where(x => x.IDPK > 1).ToList<ToolSchedSubModel>();
                if (result1.Count > 0)
                {
                    ShowInformationMessage("Duplicate Tool Schedule Code has been entered!");
                    setDefaultComboForBlankTool(result1[0].ROUTE_NO.ToValueAsString().Trim(), result1[0].SEQ_NO.ToValueAsString().Trim(), result1[0].CC_SNO.ToValueAsString().Trim(), result1[0].SUB_HEADING_NO.ToValueAsString());
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool CheckDuplicateSno()
        {
            try
            {
                var result = ToolScheduleDetailsAll.Where(x => x.TOOL_CODE.ToValueAsString().Trim() != "" && x.ROUTE_NO.ToString() == ToolSchedModel.RouteNo && x.SEQ_NO.ToString() == ToolSchedModel.SeqNo && x.SUB_HEADING_NO.ToString() == ToolSchedModel.SubHeadingNo).GroupBy(x => new { x.PART_NO, x.ROUTE_NO, x.SEQ_NO, x.CC_SNO, x.SUB_HEADING_NO, x.SNO })
                   .Select(g => new { g.Key.PART_NO, g.Key.ROUTE_NO, g.Key.SEQ_NO, g.Key.CC_SNO, g.Key.SUB_HEADING_NO, Count = g.Count() }).Where(x => x.Count > 1);

                if (result.Count() > 0)
                {
                    //ShowInformationMessage("Duplicate Tool Schedule Sno has been entered!");
                    if (ShowQuestionMessage("Duplicate Tool Schedule Sno has been Entered , Do you want to Reorder ?") == MessageBoxResult.OK)
                        ReNumberAll();
                    else
                        return false;
                }
                //else
                //{
                //    ReNumberAll();
                //}
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool CheckBlankSno()
        {
            try
            {
                List<ToolSchedSubModel> lsttool;
                ToolSchedSubModel tssm;
                lsttool = new List<ToolSchedSubModel>();

                lsttool = (from row in ToolScheduleDetails
                           where row.SNO.ToValueAsString().Trim() == "" && (row.IDPK > 0 || row.IDPK < 0)
                           select row).ToList<ToolSchedSubModel>();
                if (lsttool.Count > 0)
                {
                    ShowInformationMessage("Blank Sno Not Allowed!");
                    return false;
                }

                lsttool = (from row in ToolScheduleDetailsAll
                           where row.SNO.ToValueAsString().Trim() == "" && (row.IDPK > 0 || row.IDPK < 0)
                           select row).ToList<ToolSchedSubModel>();
                if (lsttool.Count > 0)
                {
                    ShowInformationMessage("Blank Sno Not Allowed!");
                    tssm = lsttool[0];
                    setDefaultComboForBlankTool(tssm.ROUTE_NO.ToValueAsString().Trim(), tssm.SEQ_NO.ToValueAsString().Trim(), tssm.CC_SNO.ToValueAsString().Trim(), tssm.SUB_HEADING_NO.ToValueAsString());
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                //throw ex.LogException();
                return false;
            }
        }


        private bool CheckBlankIssueNo()
        {
            try
            {
                List<TOOL_SCHED_ISSUE> lstissue;
                TOOL_SCHED_ISSUE tsrm;
                lstissue = new List<TOOL_SCHED_ISSUE>();

                lstissue = (from row in ToolScheduleRevision
                            where row.TS_ISSUE_NO.ToValueAsString().Trim() == "" && (row.IDPK > 0 || row.IDPK < 0)
                            select row).ToList<TOOL_SCHED_ISSUE>();
                if (lstissue.Count > 0)
                {
                    ShowInformationMessage("Blank Tool Schedule Issue No Not Allowed!");
                    return false;
                }

                lstissue = (from row in ToolScheduleRevisionAll
                            where row.TS_ISSUE_NO.ToValueAsString().Trim() == "" && (row.IDPK > 0 || row.IDPK < 0)
                            select row).ToList<TOOL_SCHED_ISSUE>();
                if (lstissue.Count > 0)
                {
                    ShowInformationMessage("Blank Tool Schedule Issue No Not Allowed!");
                    tsrm = lstissue[0];
                    setDefaultComboForBlankToolIssue(tsrm.ROUTE_NO.ToValueAsString().Trim(), tsrm.SEQ_NO.ToValueAsString().Trim(), tsrm.CC_SNO.ToValueAsString(), "");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private bool CheckDuplicateIssueNo()
        {
            try
            {
                //var result = ToolScheduleRevision.Where(x => x.TS_ISSUE_NO.ToValueAsString().Trim() != "").GroupBy(x => new { x.PART_NO, x.ROUTE_NO, x.SEQ_NO, x.CC_SNO, x.TS_ISSUE_NO })
                //   .Select(g => new { g.Key.PART_NO, g.Key.ROUTE_NO, g.Key.SEQ_NO, g.Key.CC_SNO, g.Key.TS_ISSUE_NO, Count = g.Count() }).Where(x => x.Count > 1);

                //if (result.Count() > 0)
                //{
                //    ShowInformationMessage("Duplicate Tool Schedule Issue No has been Entered!");
                //    return false;
                //}

                DataTable tempDt = new DataTable();
                DataRow dr;
                tempDt.Columns.Add("ISSUE_NO", typeof(int));
                for (int ind = 0; ind <= ToolScheduleRevision.Count - 1; ind++)
                {
                    dr = tempDt.NewRow();
                    dr["ISSUE_NO"] = DBNull.Value.Equals(ToolScheduleRevision[ind].TS_ISSUE_NO) == true ? 0 : Convert.ToInt32(ToolScheduleRevision[ind].TS_ISSUE_NO);
                    tempDt.Rows.Add(dr);
                }
                tempDt.AcceptChanges();
                var duplicates = tempDt.AsEnumerable().GroupBy(r => r["ISSUE_NO"]).Where(gr => gr.Count() > 1).ToList();

                if (duplicates.Any())
                {
                    ShowInformationMessage("Duplicate Tool Schedule Issue No has been Entered!");
                    return false;
                }

                //var result1 = ToolScheduleRevisionAll.Where(x => x.TS_ISSUE_NO.ToValueAsString().Trim() != "").GroupBy(x => new { x.PART_NO, x.ROUTE_NO, x.SEQ_NO, x.CC_SNO, x.TS_ISSUE_NO })
                //    .Select(g => new { g.Key.PART_NO, g.Key.ROUTE_NO, g.Key.SEQ_NO, g.Key.CC_SNO, g.Key.TS_ISSUE_NO, Count = g.Count() }).Where(x => x.Count > 1);

                List<TOOL_SCHED_ISSUE> result1 = ToolScheduleRevisionAll.Where(x => x.TS_ISSUE_NO.ToValueAsString().Trim() != "").GroupBy(x => new { x.PART_NO, x.ROUTE_NO, x.SEQ_NO, x.CC_SNO, x.TS_ISSUE_NO })
    .Select(g => new TOOL_SCHED_ISSUE() { PART_NO = g.Key.PART_NO, ROUTE_NO = g.Key.ROUTE_NO, SEQ_NO = g.Key.SEQ_NO, CC_SNO = g.Key.CC_SNO, TS_ISSUE_NO = g.Key.TS_ISSUE_NO, IDPK = g.Count() }).Where(x => x.IDPK > 1).ToList<TOOL_SCHED_ISSUE>();

                if (result1.IsNotNullOrEmpty() && result1.Count > 0)
                {
                    ShowInformationMessage("Duplicate Tool Schedule Issue No has been Entered!");
                    setDefaultComboForBlankToolIssue(result1[0].ROUTE_NO.ToValueAsString().Trim(), result1[0].SEQ_NO.ToValueAsString().Trim(), result1[0].CC_SNO.ToValueAsString().Trim(), "");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool CheckDuplicateAuxTooNo()
        {
            try
            {
                var result = AuxToolScheduleDetailsAll.Where(x => x.TOOL_CODE != "").GroupBy(x => new { x.TOOL_CODE, x.MADE_FOR })
                   .Select(g => new { g.Key.TOOL_CODE, g.Key.MADE_FOR, Count = g.Count() }).Where(x => x.Count > 1);
                if (result.Count() > 0)
                {
                    ShowInformationMessage("Duplicate Aux Tool Code has been Entered!");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }





        private bool CheckBlankToolCode()
        {

            List<ToolSchedSubModel> lsttool;

            lsttool = (from row in ToolScheduleDetails
                       where row.TOOL_CODE.Trim() == "" && (row.IDPK > 0 || row.IDPK < 0)
                       select row).ToList<ToolSchedSubModel>();

            if (lsttool.Count > 0)
            {
                ShowInformationMessage("Blank Tool Code Not Allowed!");
                return false;
            }


            lsttool = (from row in ToolScheduleDetailsAll
                       where row.TOOL_CODE.Trim() == "" && (row.IDPK > 0 || row.IDPK < 0)
                       select row).ToList<ToolSchedSubModel>();

            ToolSchedSubModel tssm;
            if (lsttool.Count > 0)
            {
                ShowInformationMessage("Blank Tool Code Not Allowed!");
                tssm = lsttool[0];
                setDefaultComboForBlankTool(tssm.ROUTE_NO.ToValueAsString().Trim(), tssm.SEQ_NO.ToValueAsString().Trim(), tssm.CC_SNO.ToValueAsString().Trim(), tssm.SUB_HEADING_NO.ToValueAsString().Trim());
                //ToolSchedModel.RouteNo = tssm.ROUTE_NO.ToValueAsString().Trim();
                //ToolSchedModel.SeqNo = tssm.SEQ_NO.ToValueAsString().Trim();
                //ToolSchedModel.CCSno = tssm.CC_SNO.ToValueAsString().Trim();
                //ToolSchedModel.SubHeadingNo = tssm.SUB_HEADING_NO.ToValueAsString();
                //AssignSubHeading();
                //CmbSubHeadingCombo.SelectedValue = ToolSchedModel.SubHeadingNo;
                //FilterToolScheduleAndSelect();
                //NotifyPropertyChanged("ToolSchedModel");
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckBlankAuxToolCode()
        {
            List<AUX_TOOL_SCHED> lsttool;
            lsttool = (from row in AuxToolScheduleDetails
                       where row.TOOL_CODE == "" && (row.IDPK > 0 || row.IDPK < 0)
                       select row).ToList<AUX_TOOL_SCHED>();
            if (lsttool.Count > 0)
            {
                ShowInformationMessage("Blank Aux Tool Code Not Allowed!");
                return false;
            }

            lsttool = (from row in AuxToolScheduleDetailsAll
                       where row.TOOL_CODE == "" && (row.IDPK > 0 || row.IDPK < 0)
                       select row).ToList<AUX_TOOL_SCHED>();

            if (lsttool.Count > 0)
            {
                ShowInformationMessage("Blank Aux Tool Code Not Allowed!");
                ToolSchedSubModel tss;
                tss = new ToolSchedSubModel();
                tss = (from row in ToolScheduleDetailsAll
                       where row.TOOL_CODE == lsttool[0].MADE_FOR
                       select row).FirstOrDefault<ToolSchedSubModel>();
                if (tss != null)
                {
                    setDefaultComboForBlankTool(tss.ROUTE_NO.ToValueAsString().Trim(), tss.SEQ_NO.ToValueAsString().Trim(), tss.CC_SNO.ToValueAsString().Trim(), tss.SUB_HEADING_NO.ToValueAsString().Trim());
                    lsttool = (from row in AuxToolScheduleDetailsAll
                               where row.TOOL_CODE.Trim() == "" && (row.IDPK > 0 || row.IDPK < 0)
                               select row).ToList<AUX_TOOL_SCHED>();
                    if (lsttool.Count > 0)
                    {
                        tss = (from row in ToolScheduleDetails
                               where row.TOOL_CODE == lsttool[0].MADE_FOR.ToValueAsString().Trim()
                               select row).FirstOrDefault<ToolSchedSubModel>();
                        if (tss != null)
                        {
                            SelectedToolSchedSub = tss;
                            NotifyPropertyChanged("SelectedToolSchedSub");
                            SelectionChangedToolSchedule(SelectedToolSchedSub);
                        }
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }


        private readonly ICommand closeCommand;
        public ICommand CloseCommand { get { return this.closeCommand; } }
        private void Close()
        {
            if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
            {
                CloseAction();
            }
        }   //original
        //new
        //private void Close()
        //{
        //    try
        //    {
        //        if (_toolSchedModel.PartNo.IsNotNullOrEmpty() && (IsRecordsUpdated))
        //        {
        //            if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
        //            {
        //                SaveToolSchedule();
        //                if (!IsRecordsUpdated) return;
        //            }
        //        }
        //        if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
        //        {
        //            CloseAction();
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }

        //   
        //}
        //end new

        private readonly ICommand printToolScheduleCommand;
        public ICommand PrintToolScheduleCommand { get { return this.printToolScheduleCommand; } }
        private void PrintToolSchedule()
        {
            if (!_toolSchedModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                FocusPartNo();
                return;
            }
            else if (!_toolSchedModel.RouteNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Process No"));
                FocusPartNo();
                return;
            }
            else if (!_toolSchedModel.SeqNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Sequence No"));
                FocusPartNo();
                return;
            }
            else if (!_toolSchedModel.CCSno.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Cost Center"));
                FocusPartNo();
                return;
            }
            else if (CmbSubHeadingCombo.SelectedText.Trim() == "")
            {
                ShowInformationMessage(PDMsg.NotEmpty("Sub Heading"));
                FocusPartNo();
                return;
            }

            DataTable dtData;
            try
            {
                dtData = _toolScheduleBll.GetToolScheduleReport(ToolSchedModel.PartNo, ToolSchedModel.SeqNo, ToolSchedModel.RouteNo, ToolSchedModel.CCSno);
                if (dtData.Rows.Count > 0)
                {
                    try
                    {
                        foreach (DataRow drRow in dtData.Rows)
                        {
                            if (ToolScheduleRevision.Count() > 0)
                            {
                                drRow["SNO"] = drRow["SNO"].ToValueAsString().Replace(".000", "");
                                decimal temp = 0;
                                try
                                {
                                    if (Decimal.TryParse(drRow["SNO"].ToValueAsString(), out temp))
                                    {
                                        if (temp.ToValueAsString().Contains("."))
                                        {
                                            drRow["SNO"] = "";
                                        }
                                    }
                                    Console.WriteLine(temp);
                                }
                                catch (Exception ex)
                                {
                                    ex.LogException();
                                }


                                TOOL_SCHED_ISSUE tss;
                                tss = ToolScheduleRevision[ToolScheduleRevision.Count - 1];
                                if (tss.TS_ISSUE_NO.Trim() == "" && tss.TS_ISSUE_ALTER.ToValueAsString().Trim() == "" && tss.TS_COMPILED_BY.ToValueAsString() == "" && tss.TS_ISSUE_DATE.ToValueAsString().Trim() == "")
                                {
                                    if (ToolScheduleRevision.Count > 1)
                                        tss = ToolScheduleRevision[ToolScheduleRevision.Count - 2];
                                    else
                                        tss = ToolScheduleRevision[ToolScheduleRevision.Count - 1];
                                }
                                drRow["ISSUE_NO"] = tss.TS_ISSUE_NO;
                                drRow["ALTER"] = tss.TS_ISSUE_ALTER.ToValueAsString().Trim();
                                drRow["ISSUE_DATE"] = Convert.ToDateTime(tss.TS_ISSUE_DATE).ToString("dd/MM/yyyy");
                                if (drRow["ISSUE_DATE"].ToValueAsString().Trim() == "01/01/0001")
                                {
                                    drRow["ISSUE_DATE"] = "";
                                }
                                drRow["INITIAL"] = tss.TS_COMPILED_BY;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    frmReportViewer reportViewer = new frmReportViewer(dtData, "ToolSchedule");
                    reportViewer.ShowDialog();
                }
                else
                {
                    ShowInformationMessage("No data available for printing");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand printAuxToolScheduleCommand;
        public ICommand PrintAuxToolScheduleCommand { get { return this.printAuxToolScheduleCommand; } }
        private void PrintAuxToolSchedule()
        {
            if (!_toolSchedModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                FocusPartNo();
                return;
            }
            else if (!_toolSchedModel.RouteNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Process No"));
                FocusPartNo();
                return;
            }
            else if (!_toolSchedModel.SeqNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Sequence No"));
                FocusPartNo();
                return;
            }
            else if (!_toolSchedModel.CCSno.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Cost Center"));
                FocusPartNo();
                return;
            }
            else if (CmbSubHeadingCombo.SelectedText.Trim() == "")
            {
                ShowInformationMessage(PDMsg.NotEmpty("Sub Heading"));
                FocusPartNo();
                return;
            }

            DataTable dtData;
            try
            {
                dtData = _toolScheduleBll.GetAuxToolScheduleReport(ToolSchedModel.PartNo, ToolSchedModel.SeqNo, ToolSchedModel.RouteNo, ToolSchedModel.CCSno);
                if (dtData.Rows.Count > 0)
                {
                    try
                    {
                        foreach (DataRow drRow in dtData.Rows)
                        {
                            if (ToolScheduleRevision.Count() > 0)
                            {
                                TOOL_SCHED_ISSUE tss;
                                tss = ToolScheduleRevision[ToolScheduleRevision.Count - 1];
                                if (tss.TS_ISSUE_NO.Trim() == "" && tss.TS_ISSUE_ALTER.ToValueAsString().Trim() == "" && tss.TS_COMPILED_BY.ToValueAsString() == "" && tss.TS_ISSUE_DATE.ToValueAsString().Trim() == "")
                                {
                                    if (ToolScheduleRevision.Count > 1)
                                        tss = ToolScheduleRevision[ToolScheduleRevision.Count - 2];
                                    else
                                        tss = ToolScheduleRevision[ToolScheduleRevision.Count - 1];
                                }
                                drRow["ISSUE_NO"] = tss.TS_ISSUE_NO;
                                drRow["ALTER"] = tss.TS_ISSUE_ALTER.ToValueAsString().Trim();
                                drRow["ISSUE_DATE"] = Convert.ToDateTime(tss.TS_ISSUE_DATE).ToString("dd/MM/yyyy");
                                if (drRow["ISSUE_DATE"].ToValueAsString().Trim() == "01/01/0001")
                                {
                                    drRow["ISSUE_DATE"] = "";
                                }
                                drRow["INITIAL"] = tss.TS_COMPILED_BY;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    frmReportViewer reportViewer = new frmReportViewer(dtData, "AuxToolSchedule");
                    reportViewer.ShowDialog();
                }
                else
                {
                    ShowInformationMessage("No data available for printing");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand processSheetCommand;
        public ICommand ProcessSheetCommand { get { return this.processSheetCommand; } }
        private void ProcessSheet()
        {
            try
            {
                if (!ToolSchedModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild psheet = new MdiChild();
                psheet.Title = ApplicationTitle + " - Process Sheet";
                frmProcessSheet processsheet = null;
                if (MainMDI.IsFormAlreadyOpen("Process Sheet - " + ToolSchedModel.PartNo.Trim()) == false)
                {
                    processsheet = new frmProcessSheet(psheet, _userInformation, ToolSchedModel.PartNo, ToolSchedModel.PartDescription);
                    psheet.Content = processsheet;
                    psheet.Height = processsheet.Height + 40;
                    psheet.Width = processsheet.Width + 20;
                    psheet.Resizable = false;
                    psheet.MinimizeBox = true;
                    psheet.MaximizeBox = true;
                    MainMDI.Container.Children.Add(psheet);
                }
                else
                {
                    psheet = new MdiChild();
                    psheet = (MdiChild)MainMDI.GetFormAlreadyOpened("Process Sheet - " + ToolSchedModel.PartNo.Trim());
                    processsheet = (frmProcessSheet)psheet.Content;
                    MainMDI.SetMDI(psheet);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand controlPlanCommand;
        public ICommand ControlPlanCommand { get { return this.controlPlanCommand; } }
        private void ControlPlan()
        {
            try
            {
                if (!ToolSchedModel.PartNo.IsNotNullOrEmpty())
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
                if (MainMDI.IsFormAlreadyOpen("Control Plan - " + ToolSchedModel.PartNo.Trim()) == false)
                {

                    cplanscreen = new frmPCCS(_userInformation, cplan, ToolSchedModel.PartNo);
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
                    cplan = (MdiChild)MainMDI.GetFormAlreadyOpened("Control Plan - " + ToolSchedModel.PartNo.Trim());
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
        }

        private readonly ICommand drawingsCommand;
        public ICommand DrawingsCommand { get { return this.drawingsCommand; } }
        private void Drawings()
        {
            try
            {
                if (!ToolSchedModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild drwMaster = new MdiChild();
                drwMaster.Title = ApplicationTitle + " - Drawings";
                ProcessDesigner.frmDrawings drawings = null;
                if (MainMDI.IsFormAlreadyOpen("Drawings - " + ToolSchedModel.PartNo.Trim()) == false)
                {
                    drawings = new ProcessDesigner.frmDrawings(drwMaster, _userInformation, ToolSchedModel.PartNo);
                    drwMaster.Content = drawings;
                    drwMaster.Height = drawings.Height + 40;
                    drwMaster.Width = drawings.Width + 20;
                    drwMaster.MinimizeBox = false;
                    drwMaster.MaximizeBox = false;
                    drwMaster.Resizable = false;
                    if (MainMDI.IsFormAlreadyOpen("Drawings - " + ToolSchedModel.PartNo.Trim()) == false)
                    {
                        MainMDI.Container.Children.Add(drwMaster);
                    }
                    else
                    {
                        drwMaster = new MdiChild();
                        drwMaster = (MdiChild)MainMDI.GetFormAlreadyOpened("Drawings - " + ToolSchedModel.PartNo.Trim());
                        MainMDI.SetMDI(drwMaster);
                    }
                }
                else
                {
                    drwMaster = new MdiChild();
                    drwMaster = (MdiChild)MainMDI.GetFormAlreadyOpened("Drawings  - " + ToolSchedModel.PartNo.Trim());
                    drawings = (frmDrawings)drwMaster.Content;
                    MainMDI.SetMDI(drwMaster);
                }

                //if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Route No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}

                //frmCopyStatus copyStatus = new frmCopyStatus("ProcessSheet", MandatoryFields.PART_NO.Trim(), ProcessSheet.ROUTE_NO.ToString(), "", "", "");
                //copyStatus.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand developmenReportCommand;
        public ICommand DevelopmenReportCommand { get { return this.developmenReportCommand; } }
        private void DevelopmenReport()
        {
            try
            {
                if (!ToolSchedModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                showDummy();
                MdiChild devRptmdi = new MdiChild();
                devRptmdi.Title = ApplicationTitle + "Development Report";
                frmDevelopmentReport devReport = null;
                if (MainMDI.IsFormAlreadyOpen("Development Report - " + ToolSchedModel.PartNo.Trim()) == false)
                {
                    devReport = new frmDevelopmentReport(_userInformation, devRptmdi, ToolSchedModel.PartNo);
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
                    devRptmdi = (MdiChild)MainMDI.GetFormAlreadyOpened("Development Report -" + ToolSchedModel.PartNo.Trim());
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
                if (!ToolSchedModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                showDummy();
                MdiChild mfgChild = new MdiChild();
                mfgChild.Title = ApplicationTitle + " - Manufacturing Report";
                frmManufacturingReport mfgReport = null;
                if (MainMDI.IsFormAlreadyOpen(" Manufacturing Report - " + ToolSchedModel.PartNo.Trim()) == false)
                {
                    mfgReport = new frmManufacturingReport(_userInformation, mfgChild, ToolSchedModel.PartNo);
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
                    mfgChild = (MdiChild)MainMDI.GetFormAlreadyOpened("Manufacturing Report -" + ToolSchedModel.PartNo.Trim());
                    mfgReport = (frmManufacturingReport)mfgChild.Content;
                    MainMDI.SetMDI(mfgChild);

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand showToolInfoCommand;
        public ICommand ShowToolInfoCommand { get { return this.showToolInfoCommand; } }
        private void ShowToolInfo()
        {
            int cnt = 0;
            try
            {
                if (_currenttoolscedsub != null)
                {
                    if (_currenttoolscedsub.TOOL_CODE.ToValueAsString() != "")
                    {
                        cnt = _toolScheduleBll.ValidateToolInfo(_currenttoolscedsub.TOOL_CODE.ToValueAsString());
                        if (cnt == 0)
                        {
                            ShowInformationMessage("Tool Information Does not exist!");
                        }
                        else
                        {
                            frmToolsQuickView toolQuickView = new frmToolsQuickView(_userInformation, _currenttoolscedsub.TOOL_CODE);
                            toolQuickView.ShowDialog();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void FilterAllCombo()
        {
            CostCentreCombo = new DataView();
            CostCentreCombo = _toolScheduleBll.GetCostCentre(ToolSchedModel);

            AssignSubHeading();
            if (CmbSubHeadingCombo.DataSource.Count > 0)
            {
                CmbSubHeadingCombo.SelectedItem = CmbSubHeadingCombo.DataSource[0];
            }
        }

        private void LoadComboAndGridFromProcess(DataRowView selectedSeqNo)
        {
            _copyToolSchedSub = new List<ToolSchedSubModel>();
            SequenceNoCombo = _toolScheduleBll.GetSeqNo(ToolSchedModel);

            // Added by Jeyan
            if (SequenceNoCombo.Count > 1)
            {
                ToolSchedModel.SeqNo = selectedSeqNo[0].ToValueAsString();
                //new by nandhini
                //ToolSchedModel.SeqNo = ToolScheduleMainAll[0].SEQ_NO.ToString();


                GetSeq_no = ToolSchedModel.SeqNo.ToDecimalValue();  //new comment by me
                //ToolSchedModel.ROWID = 

                //end new

            }
            else if (SequenceNoCombo.Count > 0)
            {
                ToolSchedModel.SeqNo = selectedSeqNo[0].ToValueAsString();
            }
            else
            {
                ToolSchedModel.SeqNo = "";
            }
            // Commented by Jeyan
            //if (SequenceNoCombo.Count > 1)
            //{
            //    ToolSchedModel.SeqNo = SequenceNoCombo[1][0].ToValueAsString();
            //}
            //else if (SequenceNoCombo.Count > 0)
            //{
            //    ToolSchedModel.SeqNo = SequenceNoCombo[0][0].ToValueAsString();
            //}
            //else
            //{
            //    ToolSchedModel.SeqNo = "";
            //}
            CostCentreCombo = _toolScheduleBll.GetCostCentre(ToolSchedModel);
            if (CostCentreCombo.Count > 0)
            {
                ToolSchedModel.CCSno = CostCentreCombo[0][0].ToValueAsString();
                ToolSchedModel.CCCode = CostCentreCombo[0]["CC_CODE"].ToValueAsString();
                //new by nandhini
                //DataRow[] dr = CostCentreCombo.ToTable().Select("cc_sno = '" + ToolScheduleMainAll[0].CC_SNO.ToString() + "'");
                //if (dr.Count() > 0)
                //{
                //    ToolSchedModel.CCSno = dr[0]["CC_SNO"].ToString();
                //    ToolSchedModel.CCCode = dr[0]["CC_CODE"].ToString();
                //}
                //else
                //{
                //    ToolSchedModel.CCSno = CostCentreCombo[0][0].ToValueAsString();
                //    ToolSchedModel.CCCode = CostCentreCombo[0]["CC_CODE"].ToValueAsString();
                //}                
                //end new
                TextCostCenter = "Cost Center :(" + ToolSchedModel.CCSno + ") ";
            }
            else
            {
                ToolSchedModel.CCSno = "";
                ToolSchedModel.CCCode = "";
                TextCostCenter = "Cost Center :(1) ";
            }

            AssignSubHeading();
            FilterToolSchedule();
            AddToolScheduleMain();
            SetNote();

        }

        private void LoadComboAndGridFromSeqNo()
        {
            _copyToolSchedSub = new List<ToolSchedSubModel>();
            CostCentreCombo = _toolScheduleBll.GetCostCentre(ToolSchedModel);
            if (CostCentreCombo.Count > 0)
            {
                ToolSchedModel.CCSno = CostCentreCombo[0]["CC_SNO"].ToValueAsString();
                ToolSchedModel.CCCode = CostCentreCombo[0]["CC_CODE"].ToValueAsString();
                TextCostCenter = "Cost Center :(" + ToolSchedModel.CCSno + ") ";
            }
            else
            {
                ToolSchedModel.CCSno = "";
                ToolSchedModel.CCCode = "";
                TextCostCenter = "Cost Center :(1) ";
            }
            AssignSubHeading();
            FilterToolSchedule();
            AddToolScheduleMain();
            SetNote();
        }

        private void LoadComboAndGridFromCostCentre()
        {
            _copyToolSchedSub = new List<ToolSchedSubModel>();
            AssignSubHeading();
            FilterToolSchedule();
            AddToolScheduleMain();
            SetNote();
            TextCostCenter = "Cost Center :(" + ToolSchedModel.CCSno + ") ";
        }

        private readonly ICommand insertToolScheduleCommand;
        public ICommand InsertToolScheduleCommand { get { return this.insertToolScheduleCommand; } }
        private void InsertToolSchedule()
        {
            int ipos = 0;
            try
            {
                if (SelectedToolSchedSub != null)
                {
                    if (SelectedToolSchedSub.SNO.ToValueAsString() != "")
                    {
                        ToolSchedSubModel toolschedsub = new ToolSchedSubModel();

                        toolschedsub.PART_NO = ToolSchedModel.PartNo;
                        toolschedsub.ROUTE_NO = ToolSchedModel.RouteNo.ToDecimalValue();
                        toolschedsub.SEQ_NO = ToolSchedModel.SeqNo.ToDecimalValue();
                        toolschedsub.CC_SNO = ToolSchedModel.CCSno.ToDecimalValue();
                        toolschedsub.SNO = SelectedToolSchedSub.SNO;
                        toolschedsub.SUB_HEADING_NO = ToolSchedModel.SubHeadingNo.ToDecimalValue();
                        toolschedsub.TOOL_CODE = "Insert Tool";
                        toolschedsub.TOOL_CODE_END = "";
                        toolschedsub.TOOL_DESC = "";  //original
                        //new 
                        TOOL_DESC_temp = SelectedToolSchedSub.TOOL_DESC;
                        //toolschedsub.TOOL_DESC = SelectedToolSchedSub.TOOL_DESC;
                        //end new
                        toolschedsub.REMARKS = "";
                        toolschedsub.IDPK = -1 * ToolScheduleDetailsAll.Count;
                        ipos = ToolScheduleDetailsAll.IndexOf(SelectedToolSchedSub) + 1;
                        ToolScheduleDetailsAll.Insert(ipos, toolschedsub);
                        ReNumber();
                        NotifyPropertyChanged("SelectedToolSchedSub");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            finally
            {
                blnExecuteCombo = true;
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
            ActionPermission = _toolScheduleBll.GetUserRights("TOOL SCHEDULE");
            //if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
            //{
            //    ActionPermission.Save = false;
            //}
            SetRights();
        }

        private void SetRights()
        {
            EditEnable = false;
            SaveEnable = false;
            if (ActionPermission.Edit == true)
            {
                SaveEnable = true;
            }
            else if (ActionPermission.AddNew == true)
            {
                SaveEnable = true;
            }

            PrintEnable = ActionPermission.Print;
        }

        /// <summary>
        /// Confirm to close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                WPF.MDI.ClosingEventArgs closingev;
                closingev = (WPF.MDI.ClosingEventArgs)e;
                if (ShowConfirmMessageYesNo("Do you want to close the Form?") == MessageBoxResult.No)
                {
                    closingev.Cancel = true;
                    //MessageBox.Show("Please select ADMIN rights for any one administrator user", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    e = closingev;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void SelectTheGridRow()
        {
            //Bug Id : 704
            if (ToolScheduleDetails != null)
            {
                if (ToolScheduleDetails.Count > 0)
                {
                    SelectedToolSchedSub = ToolScheduleDetails[0];
                    NotifyPropertyChanged("SelectedToolSchedSub");
                    SelectionChangedToolSchedule(SelectedToolSchedSub);
                }
            }

            if (ToolScheduleRevision != null)
            {
                if (ToolScheduleRevision.Count > 0)
                {
                    SelectedToolSchedIssue = ToolScheduleRevision[0];
                    NotifyPropertyChanged("SelectedToolSchedIssue");
                }
            }
        }


        private readonly ICommand insertNewSubHeadingCommand;
        public ICommand InsertNewSubHeadingCommand { get { return this.insertNewSubHeadingCommand; } }
        public void InsertNewSubHeading()
        {
            decimal varStart;
            decimal maxSSNo;
            TOOL_SCHED_MAIN tsm;
            ToolScheduleModel tsmnew;
            try
            {
                if (ShowConfirmMessageYesNo("New Record will be  Permanently saved in Database.\n Do you want to Continue?") == MessageBoxResult.Yes)
                {
                    varStart = Convert.ToInt16(ToolSchedModel.SubHeadingNo) + 1;
                    TOOL_SCHED_MAIN tss = new TOOL_SCHED_MAIN();
                    tss = (from row in _toolScheduleBll.DB.TOOL_SCHED_MAIN
                           where row.PART_NO == ToolSchedModel.PartNo.ToValueAsString().Trim()
                           && row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue()
                           && row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue()
                           && row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue()
                           orderby row.SUB_HEADING_NO descending
                           select row).FirstOrDefault<TOOL_SCHED_MAIN>();
                    if (tss != null)
                    {
                        maxSSNo = tss.SUB_HEADING_NO;
                    }
                    else
                    {
                        maxSSNo = 0;
                        varStart = 1;
                        TextSubHeading = "Sub Heading:(1)";
                        ToolSchedModel.SubHeadingNo = "1";
                    }
                    if (maxSSNo >= varStart)
                    {
                        for (decimal ictr = maxSSNo; ictr >= varStart; ictr--)
                        {
                            _toolScheduleBll.CopyToolScheduleMain(ToolSchedModel, ictr, ictr + 1);
                            _toolScheduleBll.CopyToolScheduleSub(ToolSchedModel, ictr, ictr + 1);
                            _toolScheduleBll.DeleteToolScheduleMain(ToolSchedModel, ictr);
                        }
                    }
                    tsmnew = new ToolScheduleModel();
                    tsmnew.RouteNo = ToolSchedModel.RouteNo;
                    tsmnew.SeqNo = ToolSchedModel.SeqNo;
                    tsmnew.SubHeading = "";
                    tsmnew.CCSno = ToolSchedModel.CCSno;
                    tsmnew.SubHeadingNo = varStart.ToValueAsString();
                    _toolScheduleBll.CopyToolScheduleMain(ToolSchedModel, varStart, varStart);
                    ToolSchedModel.RouteNo = tsmnew.RouteNo;
                    ToolSchedModel.SeqNo = tsmnew.SeqNo;
                    ToolSchedModel.CCSno = tsmnew.CCSno;
                    ToolSchedModel.SubHeadingNo = varStart.ToValueAsString();
                    ToolScheduleMainAll = _toolScheduleBll.GetToolScheduleMainList(ToolSchedModel);
                    ToolScheduleDetailsAll = _toolScheduleBll.GetToolScheduleSub(ToolSchedModel.PartNo);
                    AssignSubHeading();
                    CmbSubHeadingCombo.SelectedValue = tsmnew.SubHeadingNo;
                    ToolSchedModel.SubHeading = "";
                    CmbSubHeadingCombo.SelectedText = "";
                    FilterToolScheduleAndSelect();
                    NotifyPropertyChanged("ToolSchedModel");
                }


                /*
                            If MsgBox("New Record will be  Permanently saved in Database." & Chr(13) & "Do you want to Continue?", vbYesNo + vbInformation, "Process Designer") = vbYes Then
                    Me.MousePointer = 11
                    varStart = Val(ltbSubHeading.Value) + 1
                    Set rs = New ADODB.Recordset
                    Sql = "SELECT MAX(SUB_HEADING_NO) FROM TOOL_SCHED_SUB WHERE PART_NO ='" & ltbPartNo.text & "' AND ROUTE_NO = '"
                    Sql = Sql & ltbRouteNo.text & "' AND SEQ_NO = '" & ltbSeqNo.text & "' AND CC_SNO = '" & LtbCostCentre.Value & "'"
                    Set rs = fnMdOpenRs(Sql, adOpenStatic)
                    If rs.RecordCount > 0 Then
                        If Not IsNull(rs(0).Value) Then
                            maxSSNo = rs(0).Value
                        Else
                            If rsToolSchedule.RecordCount > 0 Then
                                rsToolSchedule.Sort = "SUB_HEADING_NO"
                                rsToolSchedule.MoveLast
                                maxSSNo = rsToolSchedule("SUB_HEADING_NO").Value
                            Else
                                maxSSNo = 0: lblSubHeading.Caption = 1: ltbSubHeading.Value = 1
                            End If
                        End If
                    End If
                    If maxSSNo >= varStart Then
                        For i = maxSSNo To varStart Step -1
                            CopyToolSchedule "tool_sched_main", ltbPartNo.text, ltbPartNo.text, ltbRouteNo.text, ltbRouteNo.text, ltbSeqNo.text, ltbSeqNo.text, LtbCostCentre.Value, LtbCostCentre.Value, i, i + 1
                            Sql = "update tool_sched_sub set sub_heading_no='" & i + 1 & "' where part_no='" & ltbPartNo.text & "' and route_no='" & ltbRouteNo.text & "' and seq_no='" & ltbSeqNo.text & "' and cc_sno='" & LtbCostCentre.Value & "' and sub_heading_no='" & i & "'"
                            gvarcnn.Execute Sql
                            DeleteRecord "tool_sched_main", ltbPartNo.text, ltbRouteNo.text, ltbSeqNo.text, LtbCostCentre.Value, i
                        Next i
                    End If
                    RetrieveToolSchedule ltbPartNo.text
                    AssignGridManager ltbPartNo.text
                    InsertToolScheduleMain Val(varStart)
                    ssToolSchedule.RemoveAll
                    lblSubHeading.Caption = varStart
                    ltbSubHeading.text = ""
                    ltbSubHeading.Value = varStart
                    rsToolSchedMain.Sort = "sub_heading_no"
                    rsToolSchedMain.Filter = adFilterNone
                    rsToolSchedMain.Filter = "route_no='" & ltbRouteNo.text & "' and seq_no='" & ltbSeqNo.text & "' and cc_sno ='" & LtbCostCentre.Value & "'"
                    Me.MousePointer = 0
                End If
                */
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand deleteNewSubHeadingCommand;
        public ICommand DeleteNewSubHeadingCommand { get { return this.deleteNewSubHeadingCommand; } }
        public void DeleteNewSubHeading()
        {
            decimal varEnd;
            decimal varStart;
            decimal maxSSNo;
            TOOL_SCHED_MAIN tsm;
            ToolScheduleModel tsmnew;
            try
            {
                if (ShowConfirmMessageYesNo("Are you sure you want to delete this Sub Heading " + ToolSchedModel.SubHeading + " Completely ? \n Changes will be made permanently.") == MessageBoxResult.Yes)
                {
                    varStart = Convert.ToInt16(ToolSchedModel.SubHeadingNo) + 1;
                    TOOL_SCHED_MAIN tss = new TOOL_SCHED_MAIN();
                    tss = (from row in _toolScheduleBll.DB.TOOL_SCHED_MAIN
                           where row.PART_NO == ToolSchedModel.PartNo.ToValueAsString().Trim()
                           && row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue()
                           && row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue()
                           && row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue()
                           orderby row.SUB_HEADING_NO descending
                           select row).FirstOrDefault<TOOL_SCHED_MAIN>();
                    if (tss != null)
                    {
                        maxSSNo = tss.SUB_HEADING_NO;
                    }
                    else
                    {
                        maxSSNo = 0;
                        //TextSubHeading = "Sub Heading:(1)";
                        //ToolSchedModel.SubHeadingNo = "1";
                    }
                    _toolScheduleBll.DeleteToolScheduleMain(ToolSchedModel, ToolSchedModel.SubHeadingNo.ToDecimalValue());
                    if (maxSSNo > 1)
                    {
                        varEnd = ToolSchedModel.SubHeadingNo.ToDecimalValue() + 1;
                        if (maxSSNo >= varStart)
                        {
                            for (decimal ictr = varEnd; ictr <= maxSSNo; ictr++)
                            {
                                _toolScheduleBll.CopyToolScheduleMain(ToolSchedModel, ictr, ictr - 1);
                                _toolScheduleBll.CopyToolScheduleSub(ToolSchedModel, ictr, ictr - 1);
                                _toolScheduleBll.DeleteToolScheduleMain(ToolSchedModel, ictr);
                            }
                        }
                    }
                    else
                    {
                        //ToolSchedModel.SubHeadingNo = "";
                        //ToolSchedModel.SubHeading = "";
                        //ToolSchedModel.CCSno = "";
                    }

                    tsmnew = new ToolScheduleModel();
                    tsmnew.RouteNo = ToolSchedModel.RouteNo;
                    tsmnew.SeqNo = ToolSchedModel.SeqNo;
                    tsmnew.SubHeading = ToolSchedModel.SubHeading;
                    tsmnew.CCSno = ToolSchedModel.CCSno;
                    tsmnew.SubHeadingNo = varStart.ToValueAsString();
                    ToolScheduleMainAll = _toolScheduleBll.GetToolScheduleMainList(ToolSchedModel);
                    ToolScheduleDetailsAll = _toolScheduleBll.GetToolScheduleSub(ToolSchedModel.PartNo);
                    ToolSchedModel.RouteNo = tsmnew.RouteNo;
                    ToolSchedModel.SeqNo = tsmnew.SeqNo;
                    ToolSchedModel.CCSno = tsmnew.CCSno;
                    ToolSchedModel.SubHeadingNo = varStart.ToValueAsString();
                    AssignSubHeading();
                    if (CmbSubHeadingCombo.DataSource.Count > 0)
                    {
                        CmbSubHeadingCombo.SelectedItem = CmbSubHeadingCombo.DataSource[0];
                    }
                    FilterToolScheduleAndSelect();
                    NotifyPropertyChanged("ToolSchedModel");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private ToolScheduleModel CopyToolScheduleModel(ToolScheduleModel tsm)
        {
            ToolScheduleModel entity = null;
            try
            {
                entity = new ToolScheduleModel();
                entity.PartNo = tsm.PartNo;
                entity.RouteNo = tsm.RouteNo;
                entity.SeqNo = tsm.SeqNo;
                entity.CCSno = tsm.CCSno;
                entity.SubHeadingNo = tsm.SubHeadingNo;
                entity.SubHeading = tsm.SubHeading;
                entity.BotNote = tsm.BotNote;
                entity.TopNote = tsm.TopNote;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return entity;
        }

        public void EditSelectedPartNo(string partNo)
        {
            try
            {

                ToolSchedModel.PartNo = partNo;
                foreach (DataRowView drview in PartNoCombo)
                {
                    if (drview["PART_NO"].ToValueAsString().Trim() == partNo.Trim())
                    {
                        SelectedRowPart = drview;
                        SelectDataRowPart();
                        AssignSubHeading();
                        CmbSubHeadingCombo.SelectedValue = ToolSchedModel.SubHeadingNo;
                        FilterToolScheduleAndSelect();
                    }
                }
            }
            catch (Exception ex)
            {
                //throw ex.LogException();
            }
        }

        //Bug Id : 746
        public void dgvToolsScheduleRev_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (e.Column.GetType() == typeof(System.Windows.Controls.DataGridTemplateColumn))
                {
                    if (e.Column.Header.ToString().Trim().ToUpper() == "DATE")
                    {
                        var popup = GetVisualChild<System.Windows.Controls.Primitives.Popup>(e.EditingElement);
                        if (popup != null && popup.IsOpen)
                        {
                            e.Cancel = true;
                        }
                    }
                }

                TextBox tb = e.EditingElement as TextBox;
                string columnName = e.Column.SortMemberPath;

                if (columnName == "TS_ISSUE_NO")
                {
                    if (!CheckDuplicateIssueNo())
                    {
                        tb.Text = "";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

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

        private void setDefaultComboForBlankTool(string routeNo, string seqNo, string ccSno, string subHeadingNo)
        {
            try
            {
                ToolSchedModel.RouteNo = routeNo;
                ToolSchedModel.SeqNo = seqNo;
                ToolSchedModel.CCSno = ccSno;
                ToolSchedModel.SubHeadingNo = subHeadingNo;
                //AssignSubHeading();
                if (CmbSubHeadingCombo.DataSource.Count > 0)
                    CmbSubHeadingCombo.SelectedValue = subHeadingNo;
                FilterToolScheduleAndSelect();
                NotifyPropertyChanged("ToolSchedModel");
            }
            catch (Exception ex)
            {

            }
        }

        private void setDefaultComboForBlankToolIssue(string routeNo, string seqNo, string ccSno, string subHeadingNo)
        {
            try
            {
                ToolSchedModel.RouteNo = routeNo;
                ToolSchedModel.SeqNo = seqNo;
                ToolSchedModel.CCSno = ccSno;
                if (CmbSubHeadingCombo.DataSource.Count > 0)
                    CmbSubHeadingCombo.SelectedItem = CmbSubHeadingCombo.DataSource[0];
                else
                    AssignSubHeading();
                FilterToolScheduleAndSelect();
                NotifyPropertyChanged("ToolSchedModel");
            }
            catch (Exception ex)
            {

            }
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

            }
        }

        private string AssignCostCentre(DataView dvData, string ccSno)
        {
            string ccode = "";
            if (dvData.Count > 0)
            {
                DataTable dtData = dvData.ToTable();
                DataRow[] drRow;
                drRow = dtData.Select("CC_SNO='" + ccSno + "'");
                if (drRow.Length > 0)
                {
                    ccode = drRow[0]["CC_CODE"].ToValueAsString();
                }
            }
            return ccode;
        }

        private void ClearDataView(DataView dvData)
        {
            try
            {
                if (dvData != null)
                {
                    if (dvData.Table != null)
                    {
                        dvData.Table.Clear();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void OnCellEditEndingAuxToolSchedule(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            try
            {
                TextBox tb = e.EditingElement as TextBox;
                string columnName = e.Column.SortMemberPath;

                if (columnName == "TOOL_CODE")
                {
                    if (!CheckDuplicateAuxTooNo())
                    {
                        tb.Text = "";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();

            }
        }

        public void dgvToolsScheduleRev_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;

            foreach (TOOL_SCHED_ISSUE source in ToolScheduleRevisionAll)
            {
                int output;
                if (source.TS_ISSUE_NO.Trim() == "")
                {
                    source.ISSUE_NO_NEW = 999;
                }
                else
                {
                    if (int.TryParse(source.TS_ISSUE_NO.ToValueAsString(), out output) == true)
                    {
                        source.ISSUE_NO_NEW = source.TS_ISSUE_NO.ToIntValue();
                    }
                    else
                    {
                        source.ISSUE_NO_NEW = 0;
                    }
                }
            }


            if (sort == "" || sort == "asc")
            {
                ToolScheduleRevision = (from row in ToolScheduleRevisionAll
                                        where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
                                        row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
                                        row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue()
                                        select row).ToList<TOOL_SCHED_ISSUE>().OrderByDescending(x => (x.ISSUE_NO_NEW == 999 ? -999 : x.ISSUE_NO_NEW)).ToList<TOOL_SCHED_ISSUE>();
                sort = "desc";
            }
            else
            {
                ToolScheduleRevision = (from row in ToolScheduleRevisionAll
                                        where row.ROUTE_NO == ToolSchedModel.RouteNo.ToDecimalValue() &&
                                        row.SEQ_NO == ToolSchedModel.SeqNo.ToDecimalValue() &&
                                        row.CC_SNO == ToolSchedModel.CCSno.ToDecimalValue()
                                        select row).ToList<TOOL_SCHED_ISSUE>().OrderBy(x => x.ISSUE_NO_NEW).ToList<TOOL_SCHED_ISSUE>();
                sort = "asc";
            }
            NotifyPropertyChanged("ToolScheduleRevision");
        }

        public DataGridCell GetCell(int row, int column)
        {
            DataGridRow rowContainer = GetRow(row);

            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);

                // try to get the cell but it may possibly be virtualized
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                if (cell == null)
                {
                    // now try to bring into view and retreive the cell
                    //DgvToolSchedule.ScrollIntoView(rowContainer, DgvToolSchedule.Columns[column]);
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                }
                return cell;
            }
            return null;
        }

        public DataGridCell GetCell(int row, int column, System.Windows.Controls.DataGrid grid)
        {
            DataGridRow rowContainer = GetRow(row);

            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);

                // try to get the cell but it may possibly be virtualized
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                //if (cell == null)
                //{
                // now try to bring into view and retreive the cell
                grid.ScrollIntoView(rowContainer, grid.Columns[column]);
                cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                //}
                return cell;
            }
            return null;
        }


        static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        public DataGridRow GetRow(int index)
        {
            DataGridRow row = (DataGridRow)DgvToolSchedule.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // may be virtualized, bring into view and try again
                DgvToolSchedule.ScrollIntoView(DgvToolSchedule.Items[index]);
                row = (DataGridRow)DgvToolSchedule.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        public DataGridRow GetRow(int index, System.Windows.Controls.DataGrid grid)
        {
            DataGridRow row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            if (row == null)
            {
                // may be virtualized, bring into view and try again
                grid.ScrollIntoView(grid.Items[index]);
                row = (DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(index);
            }
            return row;
        }

        public string Chk_Toolstr = string.Empty;
        public string Chk_Tool(string tool_code)
        {

            string pToolCode = null;
            int pLength = 0;
            string prefix = null;
            int start = 0;
            string toolcode;
            int length = 0;


            pToolCode = tool_code.Replace(" ", "");

            //PToolCode = Rem_Spc(Tool_Code);
            pLength = pToolCode.ToString().Length;
            if (pToolCode.Substring(0, 1) == "E")
            {
                toolcode = pToolCode.Substring(2);
                prefix = pToolCode.Substring(0, 2) + " ";
            }
            else
            {
                toolcode = pToolCode;
                prefix = "";
            }

            length = toolcode.ToString().Length;

            if (string.IsNullOrEmpty(prefix))
            {
                start = 0;
            }
            else
            {
                start = 1;
            }

            //int i = 0;        
            //string firstChar = string.Empty;
            //for (i = start; i < length; i++)
            //{
            //    firstChar = pToolCode.Substring(i, 1).ToString();

            //    int n;
            //    bool isNumeric = int.TryParse(firstChar, out n);


            //    if (isNumeric == false)
            //    {
            //        Chk_Toolstr = "";
            //        return Chk_Toolstr;
            //    }
            //}


            switch (length)
            {
                case 15:
                    Chk_Toolstr = prefix + toolcode.Substring(0, 6) + " " + toolcode.Substring(6, 1) + " " + toolcode.Substring(7, 2) + " " + toolcode.Substring(9, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(4, toolcode.Length));
                    //Chk_Tool =  Prefix & Left$(toolcode, 6) & " " & Mid$(toolcode, 7, 1) & " " & Mid$(toolcode, 8, 2) & " " & Mid$(toolcode, 10, 2) & " " & Right$(toolcode, 4)
                    break;
                case 12:
                    Chk_Toolstr = prefix + toolcode.Substring(0, 3) + " " + toolcode.Substring(3, 1) + " " + toolcode.Substring(4, 2) + " " + toolcode.Substring(6, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(4, toolcode.Length));
                    break;
                case 11:
                    if (toolcode.Substring(0, 1) == "0")
                    {
                        Chk_Toolstr = prefix + toolcode.Substring(0, 3) + " " + toolcode.Substring(3, 1) + " " + toolcode.Substring(4, 2) + " " + toolcode.Substring(6, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(3, toolcode.Length));
                        //    Chk_Toolstr = Prefix + Strings.Left(toolcode, 3) + " " + Strings.Mid(toolcode, 4, 1) + " " + Strings.Mid(toolcode, 5, 2) + " " + Strings.Mid(toolcode, 7, 2) + " " + Strings.Right(toolcode, 3);
                    }
                    else
                    {
                        Chk_Toolstr = prefix + toolcode.Substring(0, 2) + " " + toolcode.Substring(2, 1) + " " + toolcode.Substring(3, 2) + " " + toolcode.Substring(5, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(4, toolcode.Length));
                        //Chk_Toolstr = Prefix + Strings.Left(toolcode, 2) + " " + Strings.Mid(toolcode, 3, 1) + " " + Strings.Mid(toolcode, 4, 2) + " " + Strings.Mid(toolcode, 6, 2) + " " + Strings.Right(toolcode, 4);
                    }
                    break;
                case 10:
                    Chk_Toolstr = prefix + toolcode.Substring(0, 3) + " " + toolcode.Substring(3, 1) + " " + toolcode.Substring(4, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(4, toolcode.Length));
                    break;
                case 9:
                    Chk_Toolstr = prefix + toolcode.Substring(0, 2) + " " + toolcode.Substring(2, 1) + " " + toolcode.Substring(3, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(4, toolcode.Length));
                    break;
                case 8:
                    Chk_Toolstr = prefix + toolcode.Substring(0, 2) + " " + toolcode.Substring(2, 1) + " " + toolcode.Substring(3, 2) + " " + toolcode.Substring(toolcode.Length - Math.Min(3, toolcode.Length));
                    break;
                default:
                    Chk_Toolstr = "";
                    break;
            }
            return Chk_Toolstr;
        }
    }

}
