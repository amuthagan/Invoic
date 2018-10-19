using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using ProcessDesigner.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF.MDI;

namespace ProcessDesigner.ViewModel
{

    public class FeasibleReportAndCostSheetViewModel : ViewModelBase
    {
        UserInformation _userInformation = null;
        private const string CONS_RIGHTS_NAME = "COST SHEET PREPARATION";
        FeasibleReportAndCostSheet bll = null;
        private LogViewBll _logviewBll;
        WPF.MDI.MdiChild mdiChild = null;
        System.Windows.Window self = null;
        bool isCIReferenceSelectionCompleted = false;
        private readonly ICommand _onOperaionSelectionChanged;
        public ICommand OnOperaionSelectionChanged { get { return this._onOperaionSelectionChanged; } }
        public FeasibleReportAndCostSheetViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, int entityPrimaryKey,
            OperationMode operationMode, string title = "Feasibility Report and Cost Sheet", System.Windows.Window self = null)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;

            _userInformation = userInformation;
            this.mdiChild = mdiChild;
            this.self = self;

            bll = new FeasibleReportAndCostSheet(userInformation);
            this._logviewBll = new LogViewBll(userInformation);
            Title = title.IsNotNullOrEmpty() ? title : "Feasibility Report and Cost Sheet";

            EntityPrimaryKey = entityPrimaryKey;
            EnquiryReceivedOn = bll.ServerDateTime();

            //CIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER>().DefaultView;
            //CIReferenceZoneDataSource = bll.GetZoneDetails().ToDataTable<CI_REFERENCE_ZONE>().DefaultView;
            //CustomersDataSource = bll.GetCustomerDetails().ToDataTable<DDCUST_MAST>().DefaultView;
            //PlantDataSource = bll.GetLocationDetails().ToDataTable<DDLOC_MAST>().DefaultView;
            //PreparedByDataSource = bll.GetUserDetails().ToDataTable<SEC_USER_MASTER>().DefaultView;

            //RawMaterialEntities = bll.GetRawMaterialsDetails();
            //RawMaterialsDataSource = RawMaterialEntities.ToDataTable<DDRM_MAST>().DefaultView;
            //FinishDataSource = bll.GetFinishDetails().ToDataTable<DDFINISH_MAST>().DefaultView;
            //TopCoatingDataSource = bll.GetTopCoatingDetails().ToDataTable<DDCOATING_MAST>().DefaultView;

            //CostCentreOutputEntity = bll.GetCostCentreOutputDetails();
            //CostCentreOutputDataSource = CostCentreOutputEntity.ToDataTable<DDCC_OUTPUT>().DefaultView;
            //StandardNotes = bll.GetStandardNotes().ToDataTable<DDSTD_NOTES>().DefaultView;

            //DataTable dt = bll.GetOperationDetails().ToDataTable<DDOPER_MAST>();
            //dt.Columns["OPER_CODE"].ColumnName = "OPERATION_NO";
            //dt.Columns["OPER_DESC"].ColumnName = "OPERATION";
            //OperationDataSource = dt.DefaultView;

            //OperationCostDataSource = bll.GetOperationCostDetails().ToDataTable<V_OPERATION_COST>().DefaultView;
            loadMasters();

            ActiveEntity = new DDCI_INFO();
            ActiveEntity.IS_COMBINED = false;
            MandatoryFields = new FRCSModel();

            //ActiveChildEntity = new DDCOST_PROCESS_DATA();

            #region DropdownColumns Settins
            CiReferenceZoneDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CODE", ColumnDesc = "Zone Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "DESCRIPTION", ColumnDesc = "Zone", ColumnWidth = "75*" }
                        };

            CIReferenceDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CI_REFERENCE", ColumnDesc = "CI Reference", ColumnWidth = "90" },
                            new DropdownColumns() { ColumnName = "FRCS_DATE", ColumnDesc = "FRCS Date", ColumnWidth = "80" },
                            new DropdownColumns() { ColumnName = "CUST_DWG_NO", ColumnDesc = "Drawing No.", ColumnWidth = "100" },
                            new DropdownColumns() { ColumnName = "CUST_CODE", ColumnDesc = "Customer Code", ColumnWidth = "100" },
                            new DropdownColumns() { ColumnName = "FINISH_CODE", ColumnDesc = "Finish Code", ColumnWidth = "80" },
                            new DropdownColumns() { ColumnName = "CUST_DWG_NO_ISSUE", ColumnDesc = "Customer Drawing Issue No.", ColumnWidth = "175" },
                            new DropdownColumns() { ColumnName = "CUST_STD_DATE", ColumnDesc = "Customer STD Date ", ColumnWidth = "150" }
                        };

            CustomerDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "CUST_CODE", ColumnDesc = "Customer Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "CUST_NAME", ColumnDesc = "Customer", ColumnWidth = "75*", IsDefaultSearchColumn = true }
                        };

            PlantDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Location Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "LOCATION", ColumnDesc = "Location", ColumnWidth = "75*" }
                        };

            PreparedByDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "USER_NAME", ColumnDesc = "User Name", ColumnWidth = 95 },
                           new DropdownColumns() { ColumnName = "DESIGNATION", ColumnDesc = "Designation", ColumnWidth = "1*" }
                        };

            RawMaterialDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "RM_CODE", ColumnDesc = "Raw Material Code", ColumnWidth = "57*" },
                           new DropdownColumns() { ColumnName = "RM_DESC", ColumnDesc = "Raw Material", ColumnWidth = "75*", IsDefaultSearchColumn = true },
                           new DropdownColumns() { ColumnName = "LOC_COST", ColumnDesc = "Cost for Domestic(Per Kg.)", ColumnWidth = "75*" },
                           new DropdownColumns() { ColumnName = "EXP_COST", ColumnDesc = "Cost for Export(Per Kg.)", ColumnWidth = "75*" }
                        };

            FinishDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "FINISH_CODE", ColumnDesc = "Finish Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "FINISH_DESC", ColumnDesc = "Finish Description", ColumnWidth = "75*" }
                        };

            TopCoatingDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "COATING_CODE", ColumnDesc = "Coating Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "COATING_DESC", ColumnDesc = "Coating Description", ColumnWidth = "75*" }
                        };

            OperationDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "OPER_CODE", ColumnDesc = "Operation Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "OPER_DESC", ColumnDesc = "Operation Description", ColumnWidth = "75*" },
                           new DropdownColumns() { ColumnName = "OPTIONAL_OPER", ColumnDesc = "Optional Operation", ColumnWidth = "75*" },
                           new DropdownColumns() { ColumnName = "TAG_APPREVIATION", ColumnDesc = "Tag Appreviation", ColumnWidth = "75*" },
                           new DropdownColumns() { ColumnName = "SHOW_IN_COST", ColumnDesc = "Show In Cost", ColumnWidth = "75*" },
                           new DropdownColumns() { ColumnName = "SAP_NO", ColumnDesc = "SAP No.", ColumnWidth = "75*" },
                           new DropdownColumns() { ColumnName = "SHORT_TEXT", ColumnDesc = "Short Text", ColumnWidth = "75*" },
                           new DropdownColumns() { ColumnName = "UNIT_OF_MEASURE", ColumnDesc = "Unit Of Measure", ColumnWidth = "75*" },
                           new DropdownColumns() { ColumnName = "SPECIAL_PROCUREMENT", ColumnDesc = "Special Procurement", ColumnWidth = "75*" },
                        };



            OperationCostDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "COST_CENT_CODE", ColumnDesc = "Cost Centre Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "COST_CENT_DESC", ColumnDesc = "Cost Cent Description", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "SETUP_TIME", ColumnDesc = "Setup Time", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "OUTPUT", ColumnDesc = "Output", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "EFFICIENCY", ColumnDesc = "Efficiency", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Loccation Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "MODULE", ColumnDesc = "Module", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "CATE_CODE", ColumnDesc = "Category Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "PHOTO", ColumnDesc = "Photo", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "MACHINE_CD", ColumnDesc = "Machine Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "CC_ABBR", ColumnDesc = "CC Abbreviations", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "MIN_BATCH_QTY", ColumnDesc = "Min Batch Qty", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "NO_OF_SHIFT", ColumnDesc = "No. Of Shift", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "FORGING_MACHINETYPE", ColumnDesc = "Forging Machine Type", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "TYPE_NUT_BOLT", ColumnDesc = "Type Nut/Bolt", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "SAP_CCCODE", ColumnDesc = "Sap CC Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "SAP_BASE_QUANTITY", ColumnDesc = "Sap Base Quantity", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "MACHINE_NAME", ColumnDesc = "Machine Name", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "OPERATION_NO", ColumnDesc = "Operation Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "UNIT_CODE", ColumnDesc = "Unit Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "FIX_COST", ColumnDesc = "Fix Cost", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "VAR_COST", ColumnDesc = "Variable Cost", ColumnWidth = "25*" },
                        };

            OutputDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "COST_CENT_CODE", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "OUTPUT", ColumnDesc = "Output", ColumnWidth = "25*" },
                        };

            CostCentreOutputDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "COST_CENT_CODE", ColumnDesc = "Cost Centre Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "OUTPUT", ColumnDesc = "Output", ColumnWidth = "25*" },
                        };
            OperationDropDownItem = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "OPERATION_NO", ColumnDesc = "Oper Code", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "OPERATION", ColumnDesc = "Operation", ColumnWidth = "1*" }
                        };

            DropDownItemCCCode = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "COST_CENT_CODE", ColumnDesc = "Code", ColumnWidth = 75 },
                            new DropdownColumns() { ColumnName = "COST_CENT_DESC", ColumnDesc = "Description", ColumnWidth = 150 },
                            new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Location", ColumnWidth = 150 }
                                                    };

            #endregion

            this.ciReferenceEndEditCommand = new DelegateCommand(this.ciReferenceEndEdit);
            this.customerEndEditCommand = new DelegateCommand<Object>(this.customerEndEdit);

            this.enquiryReceivedOnChangedCommand = new DelegateCommand(this.EnquiryReceivedOnChanged);
            this.ciReferenceSelectedItemChangedCommand = new DelegateCommand(this.CIReferenceChanged);
            this._onOperaionSelectionChanged = new DelegateCommand(this.OperCode_SelectionChanged);

            this.selectedItemChangedCommand = new DelegateCommand(this.SelectDataRow);
            this.addNewCommand = new DelegateCommand(this.AddNewSubmitCommand);
            this.editCommand = new DelegateCommand(this.EditSubmitCommand);
            this.viewCommand = new DelegateCommand(this.ViewSubmitCommand);
            this.deleteCommand = new DelegateCommand(this.DeleteSubmitCommand);
            this.saveCommand = new DelegateCommand<string>(this.SaveSubmitCommand);
            this.printCommand = new DelegateCommand(this.PrintSubmitCommand);
            this.refreshCommand = new DelegateCommand(this.RefreshSubmitCommand);
            this._ciReferenceCopyCommand = new DelegateCommand(this.ciReferenceCopyClicked);
            this.standardNoteCommand = new DelegateCommand(this.StandardNoteSubmitCommand);
            this.feasibleIsCheckedCommand = new DelegateCommand(this.FeasibleIsChecked);
            this.feasibleIsUnCheckedCommand = new DelegateCommand(this.FeasibleIsUnChecked);
            this.exportIsCheckedCommand = new DelegateCommand(this.ExportIsChecked);
            this.pendingIsCheckedCommand = new DelegateCommand(this.PendingIsChecked);
            this.combinedIsCheckedCommand = new DelegateCommand(this.combinedIsChecked);
            this.cheeseWeightLostFocusCommand = new DelegateCommand(this.CheeseWeightLostFocus);
            this.finishWeightLostFocusCommand = new DelegateCommand(this.FinishWeightLostFocus);
            this.sflShareLostFocusCommand = new DelegateCommand(this.SFLShareLostFocus);
            this.rmFactorLostFocusCommand = new DelegateCommand(this.RMFactorLostFocus);
            this.finishWeightMouseDoubleClickCommand = new DelegateCommand<string>(this.FinishWeightMouseDoubleClick);
            this.costSheetSearchClickCommand = new DelegateCommand(this.CostSheetSearchClick);
            this.productSearchClickCommand = new DelegateCommand(this.ProductSearchClick);
            this.finishSelectedItemChangedCommand = new DelegateCommand(this.FinishChanged);
            this.rawMaterialSelectedItemChangedCommand = new DelegateCommand(this.RawMaterialChanged);

            this.ciReferenceZoneSelectedItemChangedCommand = new DelegateCommand(this.CIReferenceZoneChanged);
            this.customerSelectedItemChangedCommand = new DelegateCommand(this.CustomerChanged);
            this.plantSelectedItemChangedCommand = new DelegateCommand(this.PlantChanged);
            this.preparedBySelectedItemChangedCommand = new DelegateCommand(this.PreparedByChanged);
            this.topCoatingSelectedItemChangedCommand = new DelegateCommand(this.TopCoatingChanged);

            this._costDetailsBeginningEditCommand = new DelegateCommand<Object>(this.costDetailsBeginningEdit);
            //this._costDetailsCellEndEditCommand = new DelegateCommand<Object>(this.CostDetailsCellEndEdit);
            this._costDetailsDeleteCommand = new DelegateCommand<Object>(this.CostDetailsDeleteRow);
            this._costDetailsInsertCommand = new DelegateCommand<Object>(this.CostDetailsInsertRow);
            this._costDetailsReNumberCommand = new DelegateCommand<Object>(this.CostDetailsReNumberRow);
            this.standatedNotesSaveCommand = new DelegateCommand(this.standatedNotesSaveSubmitCommand);
            this.standatedNotesIncludeCommand = new DelegateCommand(this.standatedNotesIncludeSubmitCommand);
            this.standatedNotesCloseCommand = new DelegateCommand(this.standatedNotesCloseSubmitCommand);

            this.ciReferenceZoneEndEditCommand = new DelegateCommand(this.ciReferenceZoneEndEdit);
            this.costNotesMouseDoubleClickCommand = new DelegateCommand(this.costNotesMouseDoubleClick);
            this.costNotesPreviewMouseDoubleClickCommand = new DelegateCommand(this.costNotesPreviewMouseDoubleClick);
            this.operationCodeSelectedItemChangedCommand = new DelegateCommand(this.operationCodeChanged);

            this.plantEndEditCommand = new DelegateCommand<Object>(this.plantEndEdit);
            this.preparedByEndEditCommand = new DelegateCommand<Object>(this.preparedByEndEdit);
            this.rawMaterialEndEditCommand = new DelegateCommand<Object>(this.rawMaterialEndEdit);
            this.finishEndEditCommand = new DelegateCommand<Object>(this.finishEndEdit);
            this.coatingEndEditCommand = new DelegateCommand<Object>(this.coatingEndEdit);

            this._currentCellChangedCostDetailsCommand = new DelegateCommand<DataRowView>(this.CurrentCellChangedCostDetails);

            ActionMode = operationMode;

            ActualPermission = bll.GetUserRights(CONS_RIGHTS_NAME);
            ActionPermission = ActualPermission.DeepCopy<RolePermission>();
            ChangeRights();


        }

        private DataRowView _ccCode_SelectedItem = null;
        public DataRowView CCCode_SelectedItem
        {
            get { return this._ccCode_SelectedItem; }
            set
            {
                this._ccCode_SelectedItem = value;
                NotifyPropertyChanged("CCCode_SelectedItem");
            }
        }

        private DataRowView _output_SelectedItem = null;
        public DataRowView Output_SelectedItem
        {
            get { return this._output_SelectedItem; }
            set
            {
                this._output_SelectedItem = value;
                NotifyPropertyChanged("Output_SelectedItem");
            }
        }

        private void loadMasters()
        {

            bll = new FeasibleReportAndCostSheet(_userInformation);
            CIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER>().DefaultView;
            CIReferenceZoneDataSource = bll.GetZoneDetails().ToDataTable<CI_REFERENCE_ZONE>().DefaultView;
            CustomersDataSource = bll.GetCustomerDetails().ToDataTable<DDCUST_MAST>().DefaultView;
            PlantDataSource = bll.GetLocationDetails().ToDataTable<DDLOC_MAST>().DefaultView;
            PreparedByDataSource = bll.GetUserDetails().ToDataTable<SEC_USER_MASTER>().DefaultView;

            RawMaterialEntities = bll.GetRawMaterialsDetails();
            RawMaterialsDataSource = RawMaterialEntities.ToDataTable<DDRM_MAST>().DefaultView;
            FinishDataSource = bll.GetFinishDetails().ToDataTable<DDFINISH_MAST>().DefaultView;
            TopCoatingDataSource = bll.GetTopCoatingDetails().ToDataTable<DDCOATING_MAST>().DefaultView;

            CostCentreOutputEntity = bll.GetCostCentreOutputDetails();
            CostCentreOutputDataSource = CostCentreOutputEntity.ToDataTable<DDCC_OUTPUT>().DefaultView;
            DVCostCentreOutput = CostCentreOutputDataSource.ToTable().Copy().DefaultView;
            StandardNotes = bll.GetStandardNotes().ToDataTable<DDSTD_NOTES>().DefaultView;

            DataTable dt = bll.GetOperationDetails().ToDataTable<DDOPER_MAST>();
            dt.Columns["OPER_CODE"].ColumnName = "OPERATION_NO";
            dt.Columns["OPER_DESC"].ColumnName = "OPERATION";
            OperationDataSource = dt.DefaultView;

            OperationCostDataSource = bll.GetOperationCostDetails().ToDataTable<V_OPERATION_COST>().DefaultView;
            DVOperationCost = OperationCostDataSource.ToTable().Copy().DefaultView;

        }

        private void OperCode_SelectionChanged()
        {
            try
            {
                if (CostDetailsSelectedRow.IsNotNullOrEmpty() && OperCode_SelectedItem.IsNotNullOrEmpty())
                {
                    // CostDetailsSelectedRow.BeginEdit();
                    if (OperCode_SelectedItem != null)
                    {
                        CostDetailsSelectedRow["OPERATION"] = OperCode_SelectedItem["OPERATION"].ToValueAsString();
                    }
                    else
                    {
                        CostDetailsSelectedRow["OPERATION"] = "";
                    }
                    // CostDetailsSelectedRow.EndEdit();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
        private string _cheese_wt;
        [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = false, ErrorMessage = "Cheese Weight is Required")]
        public string CHEESE_WT
        {
            get { return _cheese_wt; }
            set
            {
                _cheese_wt = value;
                NotifyPropertyChanged("CHEESE_WT");

                if (MandatoryFields.CHEESE_WT == value.ToNullable<decimal>()) return;

                MandatoryFields.CHEESE_WT = value.ToNullable<decimal>();
                if (MandatoryFields.CHEESE_WT.IsNotNullOrEmpty())
                {
                    ActiveEntity.CHEESE_WT = MandatoryFields.CHEESE_WT;
                    ActiveEntity.FINISH_WT = MandatoryFields.FINISH_WT;
                    WeightCalculation();
                    CostCalculation();
                }
            }
        }

        private string _finish_wt;
        //[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = false, ErrorMessage = "Finish Weight is Required")]
        public string FINISH_WT
        {
            get { return _finish_wt; }
            set
            {
                _finish_wt = value;
                NotifyPropertyChanged("FINISH_WT");
                if (MandatoryFields.FINISH_WT == value.ToNullable<decimal>()) return;

                MandatoryFields.FINISH_WT = value.ToNullable<decimal>();
                if (MandatoryFields.CHEESE_WT.IsNotNullOrEmpty())
                {
                    ActiveEntity.CHEESE_WT = MandatoryFields.CHEESE_WT;
                    ActiveEntity.FINISH_WT = MandatoryFields.FINISH_WT;
                    WeightCalculation();
                    CostCalculation();
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

        public string Message { get; set; }

        private DateTime? _enquiry_received_on;
        private DateTime? EnquiryReceivedOn
        {
            get { return _enquiry_received_on; }
            set
            {
                _enquiry_received_on = value;
                NotifyPropertyChanged("EnquiryReceivedOn");
            }
        }

        private readonly ICommand enquiryReceivedOnChangedCommand;
        public ICommand EnquiryReceivedOnChangedCommand { get { return this.enquiryReceivedOnChangedCommand; } }
        private void EnquiryReceivedOnChanged()
        {
            switch (ActionMode)
            {
                case OperationMode.AddNew:
                    ActiveEntity.FR_CS_DATE = ActiveEntity.ENQU_RECD_ON;
                    string ci_Number = bll.CreateCIReferenceNumber(ActiveEntity);

                    if (ci_Number.IsNotNullOrEmpty())
                    {
                        MandatoryFields.CI_REFERENCE = ci_Number;
                        copyMandatoryFieldsToEntity(MandatoryFields);
                    }
                    break;
            }
        }

        private string _title = "Feasibility Report and Cost Sheet";
        private string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private int _entityPrimaryKey = 0;
        private int EntityPrimaryKey
        {
            get { return _entityPrimaryKey; }
            set
            {
                _entityPrimaryKey = value;
                NotifyPropertyChanged("EntityPrimaryKey");
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
                loadMasters();
                switch (_actionMode)
                {
                    case OperationMode.AddNew:
                        isCIReferenceSelectionCompleted = false;
                        //ClearAll();
                        ZoneVisibility = Visibility.Visible;
                        CIReferenceHasDropDownVisibility = Visibility.Collapsed;
                        CostDetailsVisibility = Visibility.Visible;

                        MandatoryFields = new FRCSModel();
                        ActiveEntity = new DDCI_INFO();
                        ClearAll();
                        ActiveEntity.IDPK = -99999;
                        ActiveEntity.ENQU_RECD_ON = EnquiryReceivedOn;

                        ActiveEntity.FR_CS_DATE = null;
                        ActiveEntity.CUST_STD_DATE = null;
                        CIReferenceSelectedRow = null;
                        isCIReferenceSelectionCompleted = true;
                        MandatoryFields.FEASIBILITY = "1";
                        IS_FEASIBILITY_CAN_CHANGE = true;
                        //MandatoryFields.IsReadOnlyCI_REFERENCE = false;
                        copyMandatoryFieldsToEntity(MandatoryFields);
                        originalEntityDataTable = (new List<DDCI_INFO>() { ActiveEntity }).ToDataTable<DDCI_INFO>();
                        break;
                    case OperationMode.Edit:
                        ZoneVisibility = Visibility.Hidden;
                        CIReferenceHasDropDownVisibility = Visibility.Visible;
                        CostDetailsVisibility = Visibility.Visible;

                        List<DDCI_INFO> lstEntity = bll.GetEntitiesByPrimaryKey(new DDCI_INFO() { IDPK = EntityPrimaryKey });
                        if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                        {
                            ActiveEntity = lstEntity[0];
                            ActiveEntity.FEASIBILITY = "1";

                            CIReferenceDataSource.RowFilter = "CI_REFERENCE='" + lstEntity[0].CI_REFERENCE.ToValueAsString().FormatEscapeChars() + "'";
                            if (CIReferenceDataSource.Count > 0)
                            {
                                ActiveEntity.CI_REFERENCE = CIReferenceDataSource[0].Row["CI_REFERENCE"].ToValueAsString();
                                MandatoryFields.CI_REFERENCE = ActiveEntity.CI_REFERENCE;
                                CIReferenceSelectedRow = CIReferenceDataSource[0];
                                CIReferenceChanged();

                            }
                            CIReferenceDataSource.RowFilter = null;

                        }
                        else
                        {
                            MandatoryFields = new FRCSModel();
                            ActiveEntity = new DDCI_INFO();
                            ClearAll();
                            ActiveEntity.IDPK = EntityPrimaryKey;
                            ActiveEntity.ENQU_RECD_ON = EnquiryReceivedOn;
                            ActiveEntity.FR_CS_DATE = null;
                            ActiveEntity.CUST_STD_DATE = null;
                        }
                        //EntityPrimaryKey = -99999;
                        if (CIReferenceDataSource.IsNotNullOrEmpty())
                        {
                            CIReferenceDataSource.RowFilter = "IDPK = " + ActiveEntity.IDPK;

                            if (CIReferenceDataSource.Count > 0)
                            {
                                CIReferenceSelectedRow = CIReferenceDataSource[0];
                            }
                            else
                            {
                                CIReferenceSelectedRow = null;
                            }
                            CIReferenceDataSource.RowFilter = null;
                        }
                        else
                        {
                            CIReferenceSelectedRow = null;
                        }

                        //MandatoryFields.IsReadOnlyCI_REFERENCE = true;
                        originalEntityDataTable = bll.GetEntitiesByPrimaryKey(ActiveEntity).ToDataTable<DDCI_INFO>();
                        break;
                    //case OperationMode.Close: ShowInformationMessage("Access Denied");
                    //    CloseSubmitCommand(); break;
                    default: ; break;
                }


            }
        }

        private DDCI_INFO _activeEntity = null;
        public DDCI_INFO ActiveEntity
        {
            get
            {
                return _activeEntity;
            }
            set
            {
                _activeEntity = value;
                NotifyPropertyChanged("ActiveEntity");
            }
        }


        //private DDCOST_PROCESS_DATA _activeChildEntity = null;
        //public DDCOST_PROCESS_DATA ActiveChildEntity
        //{
        //    get
        //    {
        //        return _activeChildEntity;
        //    }
        //    set
        //    {
        //        _activeChildEntity = value;
        //        NotifyPropertyChanged("ActiveChildEntity");
        //    }
        //}

        private void ChangeRights()
        {

            if (ActionMode != OperationMode.AddNew)
            {
                ActionMode = OperationMode.Edit;
                if (ActionMode != OperationMode.Edit)
                {
                    ActionMode = OperationMode.View;
                    if (!ActualPermission.View) ActionMode = OperationMode.Close;
                    else
                    {
                        //case OperationMode.View:
                        ActionPermission.AddNew = ActualPermission.AddNew;
                        ActionPermission.Edit = ActualPermission.Edit;
                        ActionPermission.View = false;
                        ActionPermission.Delete = ActualPermission.Delete;
                        ActionPermission.Print = ActualPermission.Print;
                        ActionPermission.Save = false;
                    }
                }
                else
                {
                    //case OperationMode.Edit:
                    ActionPermission.AddNew = ActualPermission.AddNew;
                    ActionPermission.Edit = false;
                    ActionPermission.View = ActualPermission.View;
                    ActionPermission.Delete = ActualPermission.Delete;
                    ActionPermission.Save = ActualPermission.AddNew || ActualPermission.Edit;
                    ActionPermission.Print = ActualPermission.Print;
                }
            }
            else
            {
                //case OperationMode.AddNew:
                ActionPermission.AddNew = false;

                ActionPermission.Edit = ActualPermission.Edit;
                ActionPermission.View = ActualPermission.View;
                ActionPermission.Delete = ActualPermission.Delete;
                ActionPermission.Save = ActualPermission.AddNew || ActualPermission.Edit;
                ActionPermission.Print = false;

            }
            NotifyPropertyChanged("ActionPermission");

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

        private RolePermission _actualPermission;
        public RolePermission ActualPermission
        {
            get { return _actualPermission; }
            set
            {
                _actualPermission = value;
                NotifyPropertyChanged("ActualPermission");
            }
        }


        private DataView _ciReference = null;
        public DataView CIReferenceDataSource
        {
            get
            {
                return _ciReference;
            }
            set
            {
                _ciReference = value;
                NotifyPropertyChanged("CIReferenceDataSource");
            }
        }

        private Visibility _ciReferenceHasDropDownVisibility = Visibility.Visible;
        public Visibility CIReferenceHasDropDownVisibility
        {
            get { return _ciReferenceHasDropDownVisibility; }
            set
            {
                _ciReferenceHasDropDownVisibility = value;
                NotifyPropertyChanged("CIReferenceHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _ciReferenceDropDownItems;
        public ObservableCollection<DropdownColumns> CIReferenceDropDownItems
        {
            get
            {
                return _ciReferenceDropDownItems;
            }
            set
            {
                _ciReferenceDropDownItems = value;
                OnPropertyChanged("CIReferenceDropDownItems");
            }
        }

        private DataRowView _ciReferenceSelectedRow;
        public DataRowView CIReferenceSelectedRow
        {
            get
            {
                return _ciReferenceSelectedRow;
            }

            set
            {
                _ciReferenceSelectedRow = value;
            }
        }

        private readonly ICommand ciReferenceSelectedItemChangedCommand;
        public ICommand CIReferenceSelectedItemChangedCommand { get { return this.ciReferenceSelectedItemChangedCommand; } }
        private void CIReferenceChanged()
        {
            isCIReferenceSelectionCompleted = false;
            if (!_ciReferenceSelectedRow.IsNotNullOrEmpty())
            {
                CostDetailEntities = bll.GetCostDetails(ActiveEntity);
                isCIReferenceSelectionCompleted = true;
                return;
            }

            DataTable dt = bll.GetCIReferenceNumber(new DDCI_INFO() { IDPK = -99999 }).ToDataTable<V_CI_REFERENCE_NUMBER>().Clone();
            dt.ImportRow(_ciReferenceSelectedRow.Row);

            List<V_CI_REFERENCE_NUMBER> lstEntity = (from row in dt.AsEnumerable()
                                                     select new V_CI_REFERENCE_NUMBER()
                                                     {
                                                         CI_REFERENCE = row.Field<string>("CI_REFERENCE"),
                                                         FRCS_DATE = row.Field<string>("FRCS_DATE"),
                                                         CUST_DWG_NO = row.Field<string>("CUST_DWG_NO"),
                                                         CUST_CODE = row.Field<string>("CUST_CODE").ToDecimalValue(),
                                                         FINISH_CODE = row.Field<string>("FINISH_CODE"),
                                                         CUST_DWG_NO_ISSUE = row.Field<string>("CUST_DWG_NO_ISSUE"),
                                                         CUST_STD_DATE = row.Field<string>("CUST_STD_DATE"),
                                                         IDPK = row.Field<string>("IDPK").ToIntValue(),
                                                     }).ToList<V_CI_REFERENCE_NUMBER>();
            if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
            {
                V_CI_REFERENCE_NUMBER currentEntity = lstEntity[0];
                List<DDCI_INFO> lstActiveEntity = bll.GetEntitiesByPrimaryKey(new DDCI_INFO() { IDPK = currentEntity.IDPK });

                if (lstActiveEntity.IsNotNullOrEmpty() && lstActiveEntity.Count > 0)
                {
                    ActiveEntity = lstActiveEntity[0].DeepCopy<DDCI_INFO>();

                    if (MandatoryFields.IsNotNullOrEmpty())
                    {
                        MandatoryFields.CI_REFERENCE = lstActiveEntity[0].CI_REFERENCE;
                        MandatoryFields.CHEESE_WT = lstActiveEntity[0].CHEESE_WT;
                        MandatoryFields.FINISH_WT = lstActiveEntity[0].FINISH_WT;

                        CHEESE_WT = lstActiveEntity[0].CHEESE_WT.ToValueAsString();
                        FINISH_WT = lstActiveEntity[0].FINISH_WT.ToValueAsString();

                        MandatoryFields.RM_FACTOR = (lstActiveEntity[0].RM_FACTOR == 0) ? 1 : lstActiveEntity[0].RM_FACTOR;
                        MandatoryFields.FEASIBILITY = lstActiveEntity[0].FEASIBILITY;
                        IS_FEASIBILITY_CAN_CHANGE = lstActiveEntity[0].FEASIBILITY.ToBooleanAsString();
                        MandatoryFields.LOC_CODE = lstActiveEntity[0].LOC_CODE;

                        MandatoryFields.SFL_SHARE = lstActiveEntity[0].SFL_SHARE.ToValueAsString();
                        MandatoryFields.NUMBER_OFF = lstActiveEntity[0].NUMBER_OFF.ToValueAsString();
                        MandatoryFields.POTENTIAL = lstActiveEntity[0].POTENTIAL.ToValueAsString();

                        if (!MandatoryFields.RM_FACTOR.IsNotNullOrEmpty())
                            MandatoryFields.RM_FACTOR = 1m;
                        NotifyPropertyChanged("MandatoryFields");

                    }

                    //ActiveEntity.CI_REFERENCE = null;
                    ActiveEntity.ZONE_CODE = null;
                    ActiveEntity.RESPONSIBILITY = null;
                    //ActiveEntity.LOC_CODE = null;
                    ActiveEntity.FINISH_CODE = null;
                    ActiveEntity.COATING_CODE = null;
                    ActiveEntity.SUGGESTED_RM = null;

                    if (ActiveEntity.IS_COMBINED == null) ActiveEntity.IS_COMBINED = false;

                    CIReferenceZoneDataSource.RowFilter = "CODE in('" + lstActiveEntity[0].ZONE_CODE.ToValueAsString().FormatEscapeChars() + "','"
                        + (lstActiveEntity[0].CI_REFERENCE.IsNotNullOrEmpty() ? lstActiveEntity[0].CI_REFERENCE.ToValueAsString().Substring(0, 1) : "") + "')";
                    if (CIReferenceZoneDataSource.Count > 0)
                    {
                        ActiveEntity.ZONE_CODE = CIReferenceZoneDataSource[0].Row["CODE"].ToValueAsString();
                    }
                    CIReferenceZoneDataSource.RowFilter = null;

                    //CIReferenceDataSource.RowFilter = "CI_REFERENCE='" + lstActiveEntity[0].CI_REFERENCE.ToValueAsString() + "'";
                    //if (CIReferenceDataSource.Count > 0)
                    //{
                    //    ActiveEntity.CI_REFERENCE = CIReferenceDataSource[0].Row["CI_REFERENCE"].ToValueAsString();
                    //}
                    //CIReferenceDataSource.RowFilter = null;

                    CustomerSelectedRow = null;
                    ActiveEntity.CUST_CODE = lstActiveEntity[0].CUST_CODE;
                    MandatoryFields.CUST_NAME = string.Empty;
                    CustomersDataSource.RowFilter = "CUST_CODE='" + lstActiveEntity[0].CUST_CODE.ToValueAsString().FormatEscapeChars() + "'";
                    if (CustomersDataSource.Count > 0)
                    {
                        MandatoryFields.CUST_NAME = CustomersDataSource[0]["CUST_NAME"].ToValueAsString();
                        copyMandatoryFieldsToEntity(MandatoryFields);
                    }
                    else if (ActionMode == OperationMode.Edit && lstActiveEntity[0].CUST_CODE.IsNotNullOrEmpty())
                    {
                        List<DDCUST_MAST> lstCustomers = (from row in bll.DB.DDCUST_MAST
                                                          where row.CUST_CODE == lstActiveEntity[0].CUST_CODE
                                                          orderby row.CUST_CODE ascending
                                                          select row).ToList<DDCUST_MAST>();

                        if (lstCustomers.IsNotNullOrEmpty() && lstCustomers.Count > 0)
                        {
                            MandatoryFields.CUST_NAME = lstCustomers[0].CUST_NAME.ToValueAsString();
                            copyMandatoryFieldsToEntity(MandatoryFields);
                        }
                    }
                    CustomersDataSource.RowFilter = null;


                    PreparedByDataSource.RowFilter = "USER_NAME='" + lstActiveEntity[0].RESPONSIBILITY.ToValueAsString().FormatEscapeChars() + "'";
                    if (PreparedByDataSource.Count > 0)
                    {
                        ActiveEntity.RESPONSIBILITY = PreparedByDataSource[0].Row["USER_NAME"].ToValueAsString();
                    }
                    else if (ActionMode == OperationMode.Edit && lstActiveEntity[0].RESPONSIBILITY.IsNotNullOrEmpty())
                    {
                        List<SEC_USER_MASTER> lstPreparedBy = (from row in bll.DB.SEC_USER_MASTER
                                                               where row.USER_NAME == lstActiveEntity[0].RESPONSIBILITY
                                                               select row).ToList<SEC_USER_MASTER>();

                        if (lstPreparedBy.IsNotNullOrEmpty() && lstPreparedBy.Count > 0)
                        {
                            ActiveEntity.RESPONSIBILITY = lstPreparedBy[0].USER_NAME;
                        }
                    }

                    PreparedByDataSource.RowFilter = null;

                    PlantDataSource.RowFilter = "LOC_CODE='" + lstActiveEntity[0].LOC_CODE.ToValueAsString().FormatEscapeChars() + "'";
                    if (PlantDataSource.Count > 0)
                    {
                        ActiveEntity.LOC_CODE = PlantDataSource[0].Row["LOC_CODE"].ToValueAsString();
                        MandatoryFields.LOC_CODE = ActiveEntity.LOC_CODE;
                    }
                    else if (ActionMode == OperationMode.Edit && lstActiveEntity[0].LOC_CODE.IsNotNullOrEmpty())
                    {
                        List<DDLOC_MAST> lstPlant = bll.GetLocationDetails(new DDLOC_MAST() { LOC_CODE = lstActiveEntity[0].LOC_CODE.Trim() });

                        if (lstPlant.IsNotNullOrEmpty() && lstPlant.Count > 0)
                        {
                            MandatoryFields.LOC_CODE = lstPlant[0].LOC_CODE.ToValueAsString();
                            copyMandatoryFieldsToEntity(MandatoryFields);
                        }
                    }
                    PlantDataSource.RowFilter = null;

                    ActiveEntity.FINISH_CODE = lstActiveEntity[0].FINISH_CODE;
                    //FinishDataSource.RowFilter = "FINISH_CODE='" + lstActiveEntity[0].FINISH_CODE.ToValueAsString().FormatEscapeChars() + "'";
                    //if (FinishDataSource.Count > 0)
                    //{
                    //    ActiveEntity.FINISH_CODE = FinishDataSource[0].Row["FINISH_CODE"].ToValueAsString();
                    //}
                    //else if (ActionMode == OperationMode.Edit && lstActiveEntity[0].FINISH_CODE.IsNotNullOrEmpty())
                    //{
                    //    List<DDFINISH_MAST> lstFinish = bll.GetFinishDetails(new DDFINISH_MAST() { FINISH_CODE = lstActiveEntity[0].FINISH_CODE.Trim() }, true);

                    //    if (lstFinish.IsNotNullOrEmpty() && lstFinish.Count > 0)
                    //    {
                    //        DataTable dtFinish = lstFinish.ToDataTable<DDFINISH_MAST>();
                    //        foreach (DataRow row in dtFinish.Rows)
                    //            FinishDataSource.Table.ImportRow(row);

                    //        FinishDataSource.Table.AcceptChanges();
                    //        FinishDataSource = FinishDataSource.Table.DefaultView;
                    //        ActiveEntity.FINISH_CODE = lstFinish[0].FINISH_CODE.ToValueAsString();
                    //        copyMandatoryFieldsToEntity(MandatoryFields);

                    //    }
                    //}
                    //FinishDataSource.RowFilter = null;

                    ActiveEntity.COATING_CODE = lstActiveEntity[0].COATING_CODE;
                    //TopCoatingDataSource.RowFilter = "COATING_CODE='" + lstActiveEntity[0].COATING_CODE.FormatEscapeChars() + "'";
                    //if (TopCoatingDataSource.Count > 0)
                    //{
                    //    ActiveEntity.COATING_CODE = TopCoatingDataSource[0].Row["COATING_CODE"].ToValueAsString();
                    //}
                    //else if (ActionMode == OperationMode.Edit && lstActiveEntity[0].COATING_CODE.IsNotNullOrEmpty())
                    //{
                    //    List<DDCOATING_MAST> lstTopCoating = bll.GetTopCoatingDetails(new DDCOATING_MAST() { COATING_CODE = lstActiveEntity[0].COATING_CODE.Trim() }, true);

                    //    if (lstTopCoating.IsNotNullOrEmpty() && lstTopCoating.Count > 0)
                    //    {
                    //        DataTable dtTopCoating = lstTopCoating.ToDataTable<DDCOATING_MAST>();
                    //        foreach (DataRow row in dtTopCoating.Rows)
                    //            TopCoatingDataSource.Table.ImportRow(row);

                    //        TopCoatingDataSource.Table.AcceptChanges();
                    //        TopCoatingDataSource = TopCoatingDataSource.Table.DefaultView;
                    //        ActiveEntity.COATING_CODE = lstTopCoating[0].COATING_CODE.ToValueAsString();
                    //        copyMandatoryFieldsToEntity(MandatoryFields);

                    //    }
                    //}
                    //TopCoatingDataSource.RowFilter = null;

                    RawMaterialsDataSource.RowFilter = "RM_CODE='" + lstActiveEntity[0].SUGGESTED_RM.FormatEscapeChars() + "'";
                    if (RawMaterialsDataSource.Count > 0)
                    {
                        ActiveEntity.SUGGESTED_RM = RawMaterialsDataSource[0]["RM_CODE"].ToValueAsString();
                        MandatoryFields.RM_DESC = RawMaterialsDataSource[0]["RM_DESC"].ToValueAsString();
                    }
                    RawMaterialsDataSource.RowFilter = null;

                    if (!ActiveEntity.FEASIBILITY.ToBooleanAsString())
                    {
                        CostDetailsVisibility = Visibility.Collapsed;
                        RejectReasonsVisibility = Visibility.Visible;
                    }
                    else
                    {
                        CostDetailsVisibility = Visibility.Visible;
                        RejectReasonsVisibility = Visibility.Collapsed;
                    }

                }

            }

            CostDetailEntities = bll.GetCostDetails(ActiveEntity);
            isCIReferenceSelectionCompleted = true;
            copyMandatoryFieldsToEntity(MandatoryFields);
            CostCalculation();
        }

        private DataView _ciReferenceZone = null;
        public DataView CIReferenceZoneDataSource
        {
            get
            {
                return _ciReferenceZone;
            }
            set
            {
                _ciReferenceZone = value;
                NotifyPropertyChanged("CIReferenceZoneDataSource");
            }
        }

        private readonly ICommand ciReferenceEndEditCommand;
        public ICommand CIReferenceEndEditCommand { get { return this.ciReferenceEndEditCommand; } }
        private void ciReferenceEndEdit()
        {
            //if (!verifyCIReferenceNumber()) return;

            //if (CIReferenceDataSource.IsNotNullOrEmpty())
            //{
            //    CIReferenceDataSource.RowFilter = "CI_REFERENCE = '" + ActiveEntity.CI_REFERENCE + "'";

            //    if (CIReferenceDataSource.Count > 0)
            //    {
            //        CIReferenceSelectedRow = CIReferenceDataSource[0];
            //        CIReferenceChanged();
            //    }
            //    else
            //    {
            //        CIReferenceSelectedRow = null;
            //    }
            //    CIReferenceDataSource.RowFilter = null;
            //}
            //else
            //{
            //    CIReferenceSelectedRow = null;
            //}

        }

        private bool verifyCIReferenceNumber()
        {
            bool isCIReferenceNumberVerified = false;
            try
            {
                copyMandatoryFieldsToEntity(MandatoryFields);
                if (!ActiveEntity.IsNotNullOrEmpty()) return isCIReferenceNumberVerified;
                if (!ActiveEntity.CI_REFERENCE.IsNotNullOrEmpty()) return isCIReferenceNumberVerified;
                string message;
                if (!bll.IsValidCIReferenceNumber(ActiveEntity, ActionMode, out message) && !message.IsNotNullOrEmpty()) return isCIReferenceNumberVerified;
                if (message.IsNotNullOrEmpty())
                {
                    List<DDCI_INFO> lstResult = null;
                    switch (ActionMode)
                    {
                        case OperationMode.AddNew:
                            lstResult = (from row in bll.GetEntitiesByCIReferenceNumber(ActiveEntity)
                                         select row).ToList<DDCI_INFO>();

                            if (lstResult.IsNotNullOrEmpty() && lstResult.Count > 0)
                            {
                                ShowInformationMessage(PDMsg.AlreadyExists("CI Reference Number '" + ActiveEntity.CI_REFERENCE + "'") + "\r\nEnter another CI Reference Number");
                                return isCIReferenceNumberVerified;
                            }
                            isCIReferenceNumberVerified = true;
                            break;
                        case OperationMode.Edit:
                            lstResult = (from row in bll.GetEntitiesByCIReferenceNumber(ActiveEntity)
                                         select row).ToList<DDCI_INFO>();

                            if (lstResult.IsNotNullOrEmpty() && lstResult.Count == 0)
                            {
                                ActiveEntity.IDPK = -99999;
                                string tmpCI = ActiveEntity.CI_REFERENCE;
                                ClearAll();
                                MandatoryFields.CI_REFERENCE = tmpCI;
                                copyMandatoryFieldsToEntity(MandatoryFields);
                                CIReferenceSelectedRow = null;
                                ShowInformationMessage(PDMsg.DoesNotExists("CI Reference Number '" + ActiveEntity.CI_REFERENCE + "'") + "\r\nEnter existing CI Reference Number");
                                return isCIReferenceNumberVerified;
                            }
                            isCIReferenceNumberVerified = true;
                            break;
                    }
                }
                else
                {
                    isCIReferenceNumberVerified = true;
                }
                CostCalculation();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return isCIReferenceNumberVerified;
        }

        private readonly ICommand customerEndEditCommand;
        public ICommand CustomerEndEditCommand { get { return this.customerEndEditCommand; } }
        private void customerEndEdit(Object warrnigMessgeOnOff)
        {
            //isValidCustomer(warrnigMessgeOnOff.ToValueAsString());
        }

        private bool isValidCustomer(string warrnigMessgeOnOff = "Y")
        {
            bool bReturnValue = false;
            try
            {
                if (!MandatoryFields.CUST_NAME.ToValueAsString().IsNotNullOrEmpty()) return !bReturnValue;

                CustomersDataSource.RowFilter = "CUST_NAME='" + MandatoryFields.CUST_NAME.ToValueAsString().FormatEscapeChars() + "' OR CUST_CODE='" + MandatoryFields.CUST_NAME.ToValueAsString().FormatEscapeChars() + "'";
                if (CustomersDataSource.Count > 0)
                {
                    CustomerSelectedRow = CustomersDataSource[0];
                    MandatoryFields.CUST_NAME = CustomersDataSource[0]["CUST_NAME"].ToValueAsString();
                    copyMandatoryFieldsToEntity(MandatoryFields);
                }
                else if (warrnigMessgeOnOff.ToBooleanAsString())
                {
                    ShowInformationMessage(PDMsg.DoesNotExists("Customer '" + MandatoryFields.CUST_NAME + "'"));
                    return bReturnValue;
                }
                CustomersDataSource.RowFilter = null;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return bReturnValue;
        }

        private Visibility _ciReferenceZoneHasDropDownVisibility = Visibility.Visible;
        public Visibility CIReferenceZoneHasDropDownVisibility
        {
            get { return Visibility.Visible; }
            set
            {
                _ciReferenceZoneHasDropDownVisibility = value;
                NotifyPropertyChanged("CIReferenceZoneHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _ciReferenceZoneDropDownItems;
        public ObservableCollection<DropdownColumns> CiReferenceZoneDropDownItems
        {
            get
            {
                return _ciReferenceZoneDropDownItems;
            }
            set
            {
                _ciReferenceZoneDropDownItems = value;
                OnPropertyChanged("CiReferenceZoneDropDownItems");
            }
        }

        private DataRowView _ciReferenceZoneSelectedRow;
        public DataRowView CiReferenceZoneSelectedRow
        {
            get
            {
                return _ciReferenceZoneSelectedRow;
            }

            set
            {
                _ciReferenceZoneSelectedRow = value;
            }
        }

        private readonly ICommand ciReferenceZoneSelectedItemChangedCommand;
        public ICommand CIReferenceZoneSelectedItemChangedCommand { get { return this.ciReferenceZoneSelectedItemChangedCommand; } }
        private void CIReferenceZoneChanged()
        {
            if (_ciReferenceZoneSelectedRow.IsNotNullOrEmpty())
            {

                DataTable dt = bll.GetZoneDetails(new CI_REFERENCE_ZONE() { IDPK = -99999 }).ToDataTable<CI_REFERENCE_ZONE>().Clone();
                dt.ImportRow(_ciReferenceZoneSelectedRow.Row);

                List<CI_REFERENCE_ZONE> lstEntity = (from row in dt.AsEnumerable()
                                                     select new CI_REFERENCE_ZONE()
                                                     {
                                                         IDPK = row.Field<string>("IDPK").ToIntValue(),
                                                         CODE = row.Field<string>("CODE"),
                                                         DESCRIPTION = row.Field<string>("DESCRIPTION"),
                                                     }).ToList<CI_REFERENCE_ZONE>();
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                {
                    ActiveEntity.ZONE_CODE = lstEntity[0].CODE;
                    switch (ActionMode)
                    {
                        case OperationMode.AddNew:
                            ActiveEntity.FR_CS_DATE = ActiveEntity.ENQU_RECD_ON;
                            string ci_Number = bll.CreateCIReferenceNumber(ActiveEntity);
                            if (ci_Number.IsNotNullOrEmpty())
                            {

                                MandatoryFields.CI_REFERENCE = ci_Number;
                                copyMandatoryFieldsToEntity(MandatoryFields);
                            }
                            break;
                    }

                }
            }
        }

        private DataView _customers = null;
        public DataView CustomersDataSource
        {
            get
            {
                return _customers;
            }
            set
            {
                _customers = value;
                NotifyPropertyChanged("CustomersDataSource");
            }
        }

        private Visibility _customerHasDropDownVisibility = Visibility.Visible;
        public Visibility CustomerHasDropDownVisibility
        {
            get { return _customerHasDropDownVisibility; }
            set
            {
                _customerHasDropDownVisibility = value;
                NotifyPropertyChanged("CustomerHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _customerDropDownItems;
        public ObservableCollection<DropdownColumns> CustomerDropDownItems
        {
            get
            {
                return _customerDropDownItems;
            }
            set
            {
                _customerDropDownItems = value;
                OnPropertyChanged("CustomerDropDownItems");
            }
        }

        //private string _customerName;
        //public string CUST_NAME
        //{
        //    get
        //    {
        //        return _customerName;
        //    }
        //    set
        //    {
        //        _customerName = value;
        //        NotifyPropertyChanged("CUST_NAME");
        //    }
        //}

        private readonly ICommand customerSelectedItemChangedCommand;
        public ICommand CustomerSelectedItemChangedCommand { get { return this.customerSelectedItemChangedCommand; } }
        private void CustomerChanged()
        {
            if (_customerSelectedRow.IsNotNullOrEmpty())
            {

                DataTable dt = bll.GetCustomerDetails(new DDCUST_MAST() { CUST_CODE = -99999.00m }).ToDataTable<DDCUST_MAST>().Clone();
                dt.ImportRow(_customerSelectedRow.Row);

                List<DDCUST_MAST> lstEntity = (from row in dt.AsEnumerable()
                                               select new DDCUST_MAST()
                                               {
                                                   CUST_CODE = row.Field<string>("CUST_CODE").ToIntValue(),
                                                   CUST_NAME = row.Field<string>("CUST_NAME"),
                                                   ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                                   DELETE_FLAG = row.Field<string>("DELETE_FLAG").ToBooleanAsString(),
                                               }).ToList<DDCUST_MAST>();
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                {
                    MandatoryFields.CUST_NAME = lstEntity[0].CUST_NAME;
                    ActiveEntity.CUST_CODE = lstEntity[0].CUST_CODE;
                }
            }
        }

        private DataRowView _customerSelectedRow;
        public DataRowView CustomerSelectedRow
        {
            get
            {
                return _customerSelectedRow;
            }

            set
            {
                _customerSelectedRow = value;
            }
        }

        private DataView _plantDataSource;
        public DataView PlantDataSource
        {
            get
            {
                return _plantDataSource;
            }
            set
            {
                _plantDataSource = value;
                NotifyPropertyChanged("PlantDataSource");
            }
        }

        private Visibility _plantHasDropDownVisibility = Visibility.Visible;
        public Visibility PlantHasDropDownVisibility
        {
            get { return _plantHasDropDownVisibility; }
            set
            {
                _plantHasDropDownVisibility = value;
                NotifyPropertyChanged("PlantHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _plantDropDownItems;
        public ObservableCollection<DropdownColumns> PlantDropDownItems
        {
            get
            {
                return _plantDropDownItems;
            }
            set
            {
                _plantDropDownItems = value;
                OnPropertyChanged("PlantDropDownItems");
            }
        }

        private DataRowView _plantSelectedRow;
        public DataRowView PlantSelectedRow
        {
            get
            {
                return _plantSelectedRow;
            }

            set
            {
                _plantSelectedRow = value;

            }
        }


        private readonly ICommand plantSelectedItemChangedCommand;
        public ICommand PlantSelectedItemChangedCommand { get { return this.plantSelectedItemChangedCommand; } }
        private void PlantChanged()
        {
            if (_plantSelectedRow.IsNotNullOrEmpty())
            {
                if (OperationCostDataSource.IsNotNullOrEmpty())
                {
                    OperationCostDataSource.RowFilter = null;
                    DVOperationCost = OperationCostDataSource.ToTable().Copy().DefaultView;
                }


                DataTable dt = bll.GetLocationDetails(new DDLOC_MAST() { LOC_CODE = "-99999" }).ToDataTable<DDLOC_MAST>().Clone();
                dt.ImportRow(_plantSelectedRow.Row);

                List<DDLOC_MAST> lstEntity = (from row in dt.AsEnumerable()
                                              select new DDLOC_MAST()
                                              {
                                                  LOC_CODE = row.Field<string>("LOC_CODE"),
                                                  LOCATION = row.Field<string>("LOCATION"),
                                                  ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                                  DELETE_FLAG = row.Field<string>("DELETE_FLAG").ToBooleanAsString()
                                              }).ToList<DDLOC_MAST>();
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                {
                    if (OperationSelectedRow.IsNotNullOrEmpty() && OperationCostDataSource.IsNotNullOrEmpty())
                    {
                        OperationCostDataSource.RowFilter = "OPERATION_NO='" + OperationSelectedRow["OPER_CODE"].ToValueAsString().FormatEscapeChars() + "'";
                        DVOperationCost = OperationCostDataSource.ToTable().Copy().DefaultView;
                    }
                }
            }
        }


        private readonly ICommand plantEndEditCommand;
        public ICommand PlantEndEditCommand { get { return this.plantEndEditCommand; } }
        private void plantEndEdit(Object warrnigMessgeOnOff)
        {
            //isValidPlant(warrnigMessgeOnOff.ToValueAsString());
        }

        private bool isValidPlant(string warrnigMessgeOnOff = "Y")
        {

            bool bReturnValue = false;
            try
            {
                if (!MandatoryFields.LOC_CODE.ToValueAsString().IsNotNullOrEmpty()) return !bReturnValue;

                PlantDataSource.RowFilter = "LOCATION='" + MandatoryFields.LOC_CODE.ToValueAsString().FormatEscapeChars() + "' OR LOC_CODE='" + MandatoryFields.LOC_CODE.ToValueAsString().FormatEscapeChars() + "'";
                if (PlantDataSource.Count > 0)
                {
                    PlantSelectedRow = PlantDataSource[0];
                    MandatoryFields.LOC_CODE = PlantDataSource[0]["LOC_CODE"].ToValueAsString();
                    copyMandatoryFieldsToEntity(MandatoryFields);
                }
                else if (warrnigMessgeOnOff.ToBooleanAsString())
                {
                    PlantDataSource.RowFilter = null;
                    ShowInformationMessage(PDMsg.DoesNotExists("Plant '" + MandatoryFields.LOC_CODE + "'"));
                    return bReturnValue;
                }
                PlantDataSource.RowFilter = null;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return bReturnValue;
        }


        private DataView _preparedBy = null;
        public DataView PreparedByDataSource
        {
            get
            {
                return _preparedBy;
            }
            set
            {
                _preparedBy = value;
                NotifyPropertyChanged("PreparedByDataSource");
            }
        }

        private Visibility _preparedByHasDropDownVisibility = Visibility.Visible;
        public Visibility PreparedByHasDropDownVisibility
        {
            get { return _preparedByHasDropDownVisibility; }
            set
            {
                _preparedByHasDropDownVisibility = value;
                NotifyPropertyChanged("PreparedByHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _preparedByDropDownItems;
        public ObservableCollection<DropdownColumns> PreparedByDropDownItems
        {
            get
            {
                return _preparedByDropDownItems;
            }
            set
            {
                _preparedByDropDownItems = value;
                OnPropertyChanged("PreparedByDropDownItems");
            }
        }

        private DataRowView _preparedBySelectedRow;
        public DataRowView PreparedBySelectedRow
        {
            get
            {
                return _preparedBySelectedRow;
            }

            set
            {
                _preparedBySelectedRow = value;


            }
        }

        private readonly ICommand preparedBySelectedItemChangedCommand;
        public ICommand PreparedBySelectedItemChangedCommand { get { return this.preparedBySelectedItemChangedCommand; } }
        private void PreparedByChanged()
        {
            if (_preparedBySelectedRow.IsNotNullOrEmpty())
            {

                DataTable dt = bll.GetUserDetails(new SEC_USER_MASTER() { USER_NAME = "-99999" }).ToDataTable<SEC_USER_MASTER>().Clone();
                dt.ImportRow(_preparedBySelectedRow.Row);

                List<SEC_USER_MASTER> lstEntity = (from row in dt.AsEnumerable()
                                                   select new SEC_USER_MASTER()
                                                   {
                                                       USER_NAME = row.Field<string>("USER_NAME"),
                                                       FULL_NAME = row.Field<string>("FULL_NAME"),
                                                       DESIGNATION = row.Field<string>("DESIGNATION"),
                                                       ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                                       IS_ADMIN = row.Field<string>("IS_ADMIN").ToBooleanAsString(),
                                                   }).ToList<SEC_USER_MASTER>();
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                {

                }
            }
        }

        private readonly ICommand preparedByEndEditCommand;
        public ICommand PreparedByEndEditCommand { get { return this.preparedByEndEditCommand; } }
        private void preparedByEndEdit(Object warrnigMessgeOnOff)
        {
            //isValidPreparedBy(warrnigMessgeOnOff.ToValueAsString());
        }

        private bool isValidPreparedBy(string warrnigMessgeOnOff = "Y")
        {

            bool bReturnValue = false;
            try
            {
                if (!ActiveEntity.RESPONSIBILITY.ToValueAsString().IsNotNullOrEmpty()) return !bReturnValue;

                //OR FULL_NAME='" + ActiveEntity.RESPONSIBILITY.ToValueAsString() + "'
                PreparedByDataSource.RowFilter = "USER_NAME='" + ActiveEntity.RESPONSIBILITY.ToValueAsString().FormatEscapeChars() + "'";
                if (PreparedByDataSource.Count > 0)
                {
                    PreparedBySelectedRow = PreparedByDataSource[0];
                    ActiveEntity.RESPONSIBILITY = PreparedByDataSource[0]["USER_NAME"].ToValueAsString();
                    copyMandatoryFieldsToEntity(MandatoryFields);
                }
                else if (warrnigMessgeOnOff.ToBooleanAsString())
                {
                    ShowInformationMessage(PDMsg.DoesNotExists("Prepared By '" + ActiveEntity.RESPONSIBILITY + "'"));
                    return bReturnValue;
                }
                PreparedByDataSource.RowFilter = null;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return bReturnValue;
        }

        private DataView _rawMaterials = null;
        public DataView RawMaterialsDataSource
        {
            get
            {
                return _rawMaterials;
            }
            set
            {
                _rawMaterials = value;
                NotifyPropertyChanged("RawMaterialsDataSource");
            }
        }

        private Visibility _rawMaterialHasDropDownVisibility = Visibility.Visible;
        public Visibility RawMaterialHasDropDownVisibility
        {
            get { return _rawMaterialHasDropDownVisibility; }
            set
            {
                _rawMaterialHasDropDownVisibility = value;
                NotifyPropertyChanged("RawMaterialHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _rawMaterialDropDownItems;
        public ObservableCollection<DropdownColumns> RawMaterialDropDownItems
        {
            get
            {
                return _rawMaterialDropDownItems;
            }
            set
            {
                _rawMaterialDropDownItems = value;
                OnPropertyChanged("RawMaterialDropDownItems");
            }
        }

        private DataRowView _rawMaterialSelectedRow;
        public DataRowView RawMaterialSelectedRow
        {
            get
            {
                return _rawMaterialSelectedRow;
            }

            set
            {
                _rawMaterialSelectedRow = value;


            }
        }

        private readonly ICommand rawMaterialSelectedItemChangedCommand;
        public ICommand RawMaterialSelectedItemChangedCommand { get { return this.rawMaterialSelectedItemChangedCommand; } }
        private void RawMaterialChanged()
        {
            if (_rawMaterialSelectedRow.IsNotNullOrEmpty())
            {

                DataTable dt = bll.GetRawMaterialsDetails(new DDRM_MAST() { IDPK = -99999 }).ToDataTable<DDRM_MAST>().Clone();
                dt.ImportRow(_rawMaterialSelectedRow.Row);

                List<DDRM_MAST> lstEntity = (from row in dt.AsEnumerable()
                                             select new DDRM_MAST()
                                             {
                                                 RM_CODE = row.Field<string>("RM_CODE"),
                                                 RM_DESC = row.Field<string>("RM_DESC"),
                                                 LOC_COST = row.Field<string>("LOC_COST").ToDecimalValue(),
                                                 EXP_COST = row.Field<string>("EXP_COST").ToDecimalValue(),
                                                 COST_CENT_CODE = row.Field<string>("COST_CENT_CODE"),
                                                 ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                                 IDPK = row.Field<string>("IDPK").ToIntValue(),
                                             }).ToList<DDRM_MAST>();
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                {
                    MandatoryFields.RM_DESC = lstEntity[0].RM_DESC;
                    ActiveEntity.SUGGESTED_RM = lstEntity[0].RM_CODE;
                    fnRawMaterialProcess();
                }
            }
        }

        private readonly ICommand rawMaterialEndEditCommand;
        public ICommand RawMaterialEndEditCommand { get { return this.rawMaterialEndEditCommand; } }
        private void rawMaterialEndEdit(Object warrnigMessgeOnOff)
        {
            //isValidRawMaterial(warrnigMessgeOnOff.ToValueAsString());
        }

        private bool isValidRawMaterial(string warrnigMessgeOnOff = "Y")
        {

            bool bReturnValue = false;
            try
            {
                if (!MandatoryFields.RM_DESC.ToValueAsString().IsNotNullOrEmpty()) return !bReturnValue;
                RawMaterialsDataSource.RowFilter = "RM_DESC='" + MandatoryFields.RM_DESC.ToValueAsString().FormatEscapeChars() + "' OR RM_CODE='" + MandatoryFields.RM_DESC.ToValueAsString().FormatEscapeChars() + "'";
                if (RawMaterialsDataSource.Count > 0)
                {
                    RawMaterialSelectedRow = RawMaterialsDataSource[0];
                    MandatoryFields.RM_DESC = RawMaterialsDataSource[0]["RM_DESC"].ToValueAsString();
                    ActiveEntity.SUGGESTED_RM = RawMaterialsDataSource[0]["RM_CODE"].ToValueAsString();
                    copyMandatoryFieldsToEntity(MandatoryFields);
                }
                else if (warrnigMessgeOnOff.ToBooleanAsString())
                {
                    ShowInformationMessage(PDMsg.DoesNotExists("Raw Material '" + MandatoryFields.RM_DESC + "'"));
                    return bReturnValue;
                }
                RawMaterialsDataSource.RowFilter = null;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return bReturnValue;
        }

        private DataView _finishDataSource = null;
        public DataView FinishDataSource
        {
            get
            {
                return _finishDataSource;
            }
            set
            {
                _finishDataSource = value;
                NotifyPropertyChanged("FinishDataSource");
            }
        }

        private Visibility _finishHasDropDownVisibility = Visibility.Visible;
        public Visibility FinishHasDropDownVisibility
        {
            get { return _finishHasDropDownVisibility; }
            set
            {
                _finishHasDropDownVisibility = value;
                NotifyPropertyChanged("FinishHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _finishDropDownItems;
        public ObservableCollection<DropdownColumns> FinishDropDownItems
        {
            get
            {
                return _finishDropDownItems;
            }
            set
            {
                _finishDropDownItems = value;
                OnPropertyChanged("FinishDropDownItems");
            }
        }

        private DataRowView _finishSelectedRow;
        public DataRowView FinishSelectedRow
        {
            get
            {
                return _finishSelectedRow;
            }

            set
            {
                _finishSelectedRow = value;


            }
        }

        private readonly ICommand finishSelectedItemChangedCommand;
        public ICommand FinishSelectedItemChangedCommand { get { return this.finishSelectedItemChangedCommand; } }
        private void FinishChanged()
        {
            if (_finishSelectedRow.IsNotNullOrEmpty())
            {
                //if (!(MandatoryFields.CHEESE_WT.ToValueAsString().Length > 0 && MandatoryFields.FINISH_WT.ToValueAsString().Length > 0 && ActiveEntity.FINISH_CODE.IsNotNullOrEmpty()))
                //{
                //    ShowInformationMessage(PDMsg.NotEmpty("Cheese And Finish Weight values"));
                //    return;
                //}

                if (ActiveEntity.FINISH_CODE.IsNotNullOrEmpty())
                {
                    //if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty() && !MandatoryFields.FINISH_WT.IsNotNullOrEmpty())
                    //{
                    //    ShowInformationMessage(PDMsg.NotEmpty("Cheese And Finish Weight values"));
                    //    return;
                    //}

                    if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty())
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Cheese Weight"));
                        return;
                    }

                    //if (!MandatoryFields.FINISH_WT.IsNotNullOrEmpty())
                    //{
                    //    ShowInformationMessage(PDMsg.NotEmpty("Finish Weight"));
                    //    return;
                    //}

                }

                copyMandatoryFieldsToEntity(MandatoryFields);
                DataTable dt = bll.GetFinishDetails(new DDFINISH_MAST() { FINISH_CODE = "-99999" }).ToDataTable<DDFINISH_MAST>().Clone();
                dt.ImportRow(_finishSelectedRow.Row);

                List<DDFINISH_MAST> lstEntity = (from row in dt.AsEnumerable()
                                                 select new DDFINISH_MAST()
                                                 {
                                                     FINISH_CODE = row.Field<string>("FINISH_CODE"),
                                                     FINISH_DESC = row.Field<string>("FINISH_DESC"),
                                                     ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                                     DELETE_FLAG = row.Field<string>("DELETE_FLAG").ToBooleanAsString(),
                                                 }).ToList<DDFINISH_MAST>();
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                {

                    ActiveEntity.FINISH_CODE = lstEntity[0].FINISH_CODE;

                    if (!ActiveEntity.CHEESE_WT.IsNotNullOrEmpty() || !ActiveEntity.FINISH_WT.IsNotNullOrEmpty() &&
                        !ActiveEntity.FINISH_CODE.IsNotNullOrEmpty())
                        return;

                    //if (ActiveEntity.CHEESE_WT > 0 && ActiveEntity.FINISH_WT > 0 && ActiveEntity.FINISH_CODE.IsNotNullOrEmpty())
                    //{
                    //    fnFinishProcess();
                    //}
                    //else
                    //{
                    //    ShowInformationMessage(PDMsg.NotEmpty("Cheese And Finish Weight values"));
                    //}

                    //if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty() && !MandatoryFields.FINISH_WT.IsNotNullOrEmpty())
                    //{
                    //    ShowInformationMessage(PDMsg.NotEmpty("Cheese And Finish Weight values"));
                    //    return;
                    //}

                    if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty())
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Cheese Weight"));
                        return;
                    }

                    //if (!MandatoryFields.FINISH_WT.IsNotNullOrEmpty())
                    //{
                    //    ShowInformationMessage(PDMsg.NotEmpty("Finish Weight"));
                    //    return;
                    //}

                    fnFinishProcess();
                }
            }
        }

        private readonly ICommand finishEndEditCommand;
        public ICommand FinishEndEditCommand { get { return this.finishEndEditCommand; } }
        private void finishEndEdit(Object warrnigMessgeOnOff)
        {
            //isValidFinish(warrnigMessgeOnOff.ToValueAsString());
        }

        private bool isValidFinish(string warrnigMessgeOnOff = "Y")
        {

            bool bReturnValue = false;
            try
            {
                if (!ActiveEntity.FINISH_CODE.ToValueAsString().IsNotNullOrEmpty()) return !bReturnValue;

                FinishDataSource.RowFilter = "FINISH_DESC='" + ActiveEntity.FINISH_CODE.ToValueAsString().FormatEscapeChars() + "' OR FINISH_CODE='" + ActiveEntity.FINISH_CODE.ToValueAsString().FormatEscapeChars() + "'";
                if (FinishDataSource.Count > 0)
                {
                    FinishSelectedRow = FinishDataSource[0];
                    ActiveEntity.FINISH_CODE = FinishDataSource[0]["FINISH_CODE"].ToValueAsString();
                    copyMandatoryFieldsToEntity(MandatoryFields);
                }
                else if (warrnigMessgeOnOff.ToBooleanAsString())
                {
                    ShowInformationMessage(PDMsg.DoesNotExists("Finish '" + ActiveEntity.FINISH_CODE + "'"));
                    return bReturnValue;
                }
                FinishDataSource.RowFilter = null;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return bReturnValue;
        }

        private DataView _topCoatingDataSource = null;
        public DataView TopCoatingDataSource
        {
            get
            {
                return _topCoatingDataSource;
            }
            set
            {
                _topCoatingDataSource = value;
                NotifyPropertyChanged("TopCoatingDataSource");
            }
        }

        private Visibility _topCoatingHasDropDownVisibility = Visibility.Visible;
        public Visibility TopCoatingHasDropDownVisibility
        {
            get { return _topCoatingHasDropDownVisibility; }
            set
            {
                _topCoatingHasDropDownVisibility = value;
                NotifyPropertyChanged("TopCoatingHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _topCoatingDropDownItems;
        public ObservableCollection<DropdownColumns> TopCoatingDropDownItems
        {
            get
            {
                return _topCoatingDropDownItems;
            }
            set
            {
                _topCoatingDropDownItems = value;
                OnPropertyChanged("TopCoatingDropDownItems");
            }
        }

        private DataRowView _topCoatingSelectedRow;
        public DataRowView TopCoatingSelectedRow
        {
            get
            {
                return _topCoatingSelectedRow;
            }

            set
            {
                _topCoatingSelectedRow = value;
            }
        }

        private readonly ICommand topCoatingSelectedItemChangedCommand;
        public ICommand TopCoatingSelectedItemChangedCommand { get { return this.topCoatingSelectedItemChangedCommand; } }
        private void TopCoatingChanged()
        {
            if (_topCoatingSelectedRow.IsNotNullOrEmpty())
            {

                DataTable dt = bll.GetTopCoatingDetails(new DDCOATING_MAST() { COATING_CODE = "-99999" }).ToDataTable<DDCOATING_MAST>().Clone();
                dt.ImportRow(_topCoatingSelectedRow.Row);

                List<DDCOATING_MAST> lstEntity = (from row in dt.AsEnumerable()
                                                  select new DDCOATING_MAST()
                                                  {
                                                      COATING_CODE = row.Field<string>("COATING_CODE"),
                                                      COATING_DESC = row.Field<string>("COATING_DESC"),
                                                      ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                                      DELETE_FLAG = row.Field<string>("DELETE_FLAG").ToBooleanAsString(),
                                                  }).ToList<DDCOATING_MAST>();
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                {

                }
            }
        }

        private readonly ICommand coatingEndEditCommand;
        public ICommand CoatingEndEditCommand { get { return this.coatingEndEditCommand; } }
        private void coatingEndEdit(Object warrnigMessgeOnOff)
        {
            //isValidCoating(warrnigMessgeOnOff.ToValueAsString());
        }

        private bool isValidCoating(string warrnigMessgeOnOff = "Y")
        {

            bool bReturnValue = false;
            try
            {
                if (!ActiveEntity.COATING_CODE.ToValueAsString().IsNotNullOrEmpty()) return !bReturnValue;
                TopCoatingDataSource.RowFilter = "COATING_DESC='" + ActiveEntity.COATING_CODE.ToValueAsString().FormatEscapeChars() + "' OR COATING_CODE='" + ActiveEntity.COATING_CODE.ToValueAsString().FormatEscapeChars() + "'";
                if (TopCoatingDataSource.Count > 0)
                {
                    TopCoatingSelectedRow = TopCoatingDataSource[0];
                    ActiveEntity.COATING_CODE = TopCoatingDataSource[0]["COATING_CODE"].ToValueAsString();
                    copyMandatoryFieldsToEntity(MandatoryFields);
                }
                else if (warrnigMessgeOnOff.ToBooleanAsString())
                {
                    ShowInformationMessage(PDMsg.DoesNotExists("Coating '" + ActiveEntity.COATING_CODE + "'"));
                    return bReturnValue;
                }
                TopCoatingDataSource.RowFilter = null;
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return bReturnValue;
        }

        private DataView _operationDataSource = null;
        public DataView OperationDataSource
        {
            get
            {
                return _operationDataSource;
            }
            set
            {
                _operationDataSource = value;
                NotifyPropertyChanged("OperationDataSource");
            }
        }

        private Visibility _operationHasDropDownVisibility = Visibility.Visible;
        public Visibility OperationHasDropDownVisibility
        {
            get { return _operationHasDropDownVisibility; }
            set
            {
                _operationHasDropDownVisibility = value;
                NotifyPropertyChanged("OperationHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _operationDropDownItems;
        public ObservableCollection<DropdownColumns> OperationDropDownItems
        {
            get
            {
                return _operationDropDownItems;
            }
            set
            {
                _operationDropDownItems = value;
                OnPropertyChanged("OperationDropDownItems");
            }
        }

        private ObservableCollection<DropdownColumns> _outputDropDownItems;
        public ObservableCollection<DropdownColumns> OutputDropDownItems
        {
            get
            {
                return _outputDropDownItems;
            }
            set
            {
                _outputDropDownItems = value;
                OnPropertyChanged("OutputDropDownItems");
            }
        }

        private DataRowView _operationSelectedRow;
        public DataRowView OperationSelectedRow
        {
            get
            {
                return _operationSelectedRow;
            }

            set
            {
                _operationSelectedRow = value;
                if (_operationSelectedRow.IsNotNullOrEmpty())
                {

                    DataTable dt = bll.GetOperationDetails(new DDOPER_MAST() { OPER_CODE = -99999 }).ToDataTable<DDOPER_MAST>().Clone();
                    //dt.ImportRow(_operationSelectedRow.Row);

                    List<DDOPER_MAST> lstEntity = (from row in dt.AsEnumerable()
                                                   select new DDOPER_MAST()
                                                   {
                                                       OPER_CODE = row.Field<string>("OPER_CODE").ToDecimalValue(),
                                                       OPER_DESC = row.Field<string>("OPER_DESC"),
                                                       OPTIONAL_OPER = row.Field<string>("OPTIONAL_OPER"),
                                                       TAG_APPREVIATION = row.Field<string>("TAG_APPREVIATION"),
                                                       SHOW_IN_COST = row.Field<string>("SHOW_IN_COST"),
                                                       SAP_NO = row.Field<string>("SAP_NO").ToDecimalValue(),
                                                       SHORT_TEXT = row.Field<string>("SHORT_TEXT"),
                                                       UNIT_OF_MEASURE = row.Field<string>("UNIT_OF_MEASURE"),
                                                       SPECIAL_PROCUREMENT = row.Field<string>("SPECIAL_PROCUREMENT"),
                                                       ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                                       DELETE_FLAG = row.Field<string>("DELETE_FLAG").ToBooleanAsString(),
                                                   }).ToList<DDOPER_MAST>();
                    if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                    {
                        if (OperationSelectedRow.IsNotNullOrEmpty() && OperationCostDataSource.IsNotNullOrEmpty())
                        {
                            OperationCostDataSource.RowFilter = "OPERATION_NO='" + OperationSelectedRow["OPER_CODE"].ToValueAsString().FormatEscapeChars() + "'";
                            DVOperationCost = OperationCostDataSource.ToTable().Copy().DefaultView;
                        }
                    }
                }

            }
        }

        private readonly ICommand operationCodeSelectedItemChangedCommand;
        public ICommand OperationCodeSelectedItemChangedCommand { get { return this.operationCodeSelectedItemChangedCommand; } }
        private void operationCodeChanged()
        {

        }


        private DataView _operationCostDataSource = null;
        public DataView OperationCostDataSource
        {
            get
            {
                return _operationCostDataSource;
            }
            set
            {
                _operationCostDataSource = value;
                NotifyPropertyChanged("OperationCostDataSource");
            }
        }

        private DataView _dvoperationCost = null;
        public DataView DVOperationCost
        {
            get
            {
                return _dvoperationCost;
            }
            set
            {
                _dvoperationCost = value;
                NotifyPropertyChanged("DVOperationCost");
            }
        }



        private DataView _dvCostCentreOutput = null;
        public DataView DVCostCentreOutput
        {
            get
            {
                return _dvCostCentreOutput;
            }
            set
            {
                _dvCostCentreOutput = value;
                NotifyPropertyChanged("DVCostCentreOutput");
            }
        }

        private Visibility _operationCostHasDropDownVisibility = Visibility.Visible;
        public Visibility OperationCostHasDropDownVisibility
        {
            get { return _operationCostHasDropDownVisibility; }
            set
            {
                _operationCostHasDropDownVisibility = value;
                NotifyPropertyChanged("OperationCostHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _operationCostDropDownItems;
        public ObservableCollection<DropdownColumns> OperationCostDropDownItems
        {
            get
            {
                return _operationCostDropDownItems;
            }
            set
            {
                _operationCostDropDownItems = value;
                OnPropertyChanged("OperationCostDropDownItems");
            }
        }

        private DataRowView _operationCostSelectedRow;
        public DataRowView OperationCostSelectedRow
        {
            get
            {
                return _operationCostSelectedRow;
            }

            set
            {
                _operationCostSelectedRow = value;
                if (_operationCostSelectedRow.IsNotNullOrEmpty())
                {

                    DataTable dt = bll.GetOperationCostDetails(new DDCOST_CENT_MAST() { COST_CENT_CODE = "-99999" }).ToDataTable<V_OPERATION_COST>().Clone();
                    dt.ImportRow(_operationCostSelectedRow.Row);

                    List<V_OPERATION_COST> lstEntity = (from row in dt.AsEnumerable()
                                                        select new V_OPERATION_COST()
                                                        {
                                                            COST_CENT_CODE = row.Field<string>("COST_CENT_CODE"),
                                                            COST_CENT_DESC = row.Field<string>("COST_CENT_DESC"),
                                                            SETUP_TIME = row.Field<string>("SETUP_TIME").ToDecimalValue(),
                                                            OUTPUT = row.Field<string>("OUTPUT").ToDecimalValue(),
                                                            EFFICIENCY = row.Field<string>("EFFICIENCY").ToDecimalValue(),
                                                            LOC_CODE = row.Field<string>("LOC_CODE"),
                                                            MODULE = row.Field<string>("MODULE"),
                                                            CATE_CODE = row.Field<string>("CATE_CODE"),
                                                            //PHOTO = row.Field<string>("PHOTO").ToBytes(),
                                                            MACHINE_CD = row.Field<string>("MACHINE_CD"),
                                                            CC_ABBR = row.Field<string>("CC_ABBR"),
                                                            MIN_BATCH_QTY = row.Field<string>("MIN_BATCH_QTY").ToDecimalValue(),
                                                            NO_OF_SHIFT = row.Field<string>("NO_OF_SHIFT").ToDecimalValue(),
                                                            FORGING_MACHINETYPE = row.Field<string>("FORGING_MACHINETYPE"),
                                                            TYPE_NUT_BOLT = row.Field<string>("TYPE_NUT_BOLT"),
                                                            SAP_CCCODE = row.Field<string>("SAP_CCCODE"),
                                                            SAP_BASE_QUANTITY = row.Field<string>("SAP_BASE_QUANTITY").ToDecimalValue(),
                                                            MACHINE_NAME = row.Field<string>("MACHINE_NAME"),
                                                            OPERATION_NO = row.Field<string>("OPERATION_NO").ToDecimalValue(),
                                                            UNIT_CODE = row.Field<string>("UNIT_CODE"),
                                                            FIX_COST = row.Field<string>("FIX_COST").ToDecimalValue(),
                                                            VAR_COST = row.Field<string>("VAR_COST").ToDecimalValue(),
                                                        }).ToList<V_OPERATION_COST>();
                    if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                    {

                    }
                }

            }
        }

        private DataView _costCentreOutputDataSource = null;
        public DataView CostCentreOutputDataSource
        {
            get
            {
                return _costCentreOutputDataSource;
            }
            set
            {
                _costCentreOutputDataSource = value;
                NotifyPropertyChanged("CostCentreOutputDataSource");
            }
        }

        private Visibility _costCentreOutputHasDropDownVisibility = Visibility.Visible;
        public Visibility CostCentreOutputHasDropDownVisibility
        {
            get { return _costCentreOutputHasDropDownVisibility; }
            set
            {
                _costCentreOutputHasDropDownVisibility = value;
                NotifyPropertyChanged("CostCentreOutputHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _costCentreOutputDropDownItems;
        public ObservableCollection<DropdownColumns> CostCentreOutputDropDownItems
        {
            get
            {
                return _costCentreOutputDropDownItems;
            }
            set
            {
                _costCentreOutputDropDownItems = value;
                OnPropertyChanged("CostCentreOutputDropDownItems");
            }
        }

        private ObservableCollection<DropdownColumns> _operationDropDownItem;
        public ObservableCollection<DropdownColumns> OperationDropDownItem
        {
            get
            {
                return _operationDropDownItem;
            }
            set
            {
                _operationDropDownItem = value;
                OnPropertyChanged("OperationDropDownItem");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemCCCode;
        public ObservableCollection<DropdownColumns> DropDownItemCCCode
        {
            get
            {
                return _dropDownItemCCCode;
            }
            set
            {
                _dropDownItemCCCode = value;
                OnPropertyChanged("DropDownItemCCCode");
            }
        }

        private DataRowView _costCentreOutputSelectedRow;
        public DataRowView CostCentreOutputSelectedRow
        {
            get
            {
                return _costCentreOutputSelectedRow;
            }

            set
            {
                _costCentreOutputSelectedRow = value;
                if (_costCentreOutputSelectedRow.IsNotNullOrEmpty())
                {

                    DataTable dt = bll.GetCostCentreOutputDetails(new DDCOST_CENT_MAST() { COST_CENT_CODE = "-99999" }).ToDataTable<DDCC_OUTPUT>().Clone();
                    dt.ImportRow(_costCentreOutputSelectedRow.Row);

                    List<DDCC_OUTPUT> lstEntity = (from row in dt.AsEnumerable()
                                                   select new DDCC_OUTPUT()
                                                   {
                                                       COST_CENT_CODE = row.Field<string>("COST_CENT_CODE"),
                                                       OUTPUT = row.Field<string>("OUTPUT").ToDecimalValue(),
                                                       ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                                   }).ToList<DDCC_OUTPUT>();
                    if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                    {

                    }
                }

            }
        }

        private List<DDCC_OUTPUT> _costCentreOutputEntity = null;
        public List<DDCC_OUTPUT> CostCentreOutputEntity
        {
            get
            {
                return _costCentreOutputEntity;
            }
            set
            {
                _costCentreOutputEntity = value;
                NotifyPropertyChanged("CostCentreOutputEntity");
            }
        }

        private void ClearAll()
        {
            try
            {

                if (MandatoryFields.IsNotNullOrEmpty())
                {
                    MandatoryFields.CI_REFERENCE = "";
                    MandatoryFields.CUST_NAME = "";
                    MandatoryFields.CHEESE_WT = null;
                    MandatoryFields.FINISH_WT = null;
                    MandatoryFields.RM_FACTOR = 1;
                    //MandatoryFields.RM_FACTOR = 1.05m;
                    MandatoryFields.LOC_CODE = "";
                }

                RejectReasonsVisibility = Visibility.Collapsed;
                if (ActiveEntity.IsNotNullOrEmpty())
                {
                    ActiveEntity.CI_REFERENCE = null;
                    //ActiveEntity.ENQU_RECD_ON = EnquiryReceivedOn();
                    ActiveEntity.FR_CS_DATE = null;
                    ActiveEntity.PROD_DESC = string.Empty;
                    ActiveEntity.CUST_CODE = null;
                    ActiveEntity.CUST_DWG_NO = string.Empty;
                    ActiveEntity.CUST_DWG_NO_ISSUE = string.Empty;
                    ActiveEntity.EXPORT = string.Empty;
                    ActiveEntity.NUMBER_OFF = null;
                    ActiveEntity.POTENTIAL = null;
                    ActiveEntity.SFL_SHARE = null;
                    ActiveEntity.REMARKS = string.Empty;
                    ActiveEntity.RESPONSIBILITY = null;
                    ActiveEntity.PENDING = string.Empty;
                    ActiveEntity.FEASIBILITY = "1";
                    ActiveEntity.REJECT_REASON = string.Empty;
                    ActiveEntity.LOC_CODE = null;
                    ActiveEntity.CHEESE_WT = null;
                    ActiveEntity.FINISH_WT = null;
                    ActiveEntity.FINISH_CODE = null;
                    ActiveEntity.COATING_CODE = null;
                    ActiveEntity.SUGGESTED_RM = null;
                    ActiveEntity.RM_COST = 0.0m;
                    ActiveEntity.FINAL_COST = 0.0m;
                    ActiveEntity.COST_NOTES = string.Empty;
                    ActiveEntity.PROCESSED_BY = null;
                    ActiveEntity.ORDER_DT = null;
                    ActiveEntity.PRINT = string.Empty;
                    ActiveEntity.ALLOT_PART_NO = 0.0m;
                    ActiveEntity.PART_NO_REQ_DATE = null;
                    ActiveEntity.CUST_STD_NO = string.Empty;
                    ActiveEntity.CUST_STD_DATE = null;
                    ActiveEntity.AUTOPART = string.Empty;
                    ActiveEntity.SAFTYPART = string.Empty;
                    ActiveEntity.APPLICATION = string.Empty;
                    ActiveEntity.STATUS = 0.0m;
                    ActiveEntity.CUSTOMER_NEED_DT = null;
                    ActiveEntity.MKTG_COMMITED_DT = null;
                    ActiveEntity.PPAP_LEVEL = string.Empty;
                    ActiveEntity.DEVL_METHOD = 0.0m;
                    ActiveEntity.PPAP_FORGING = 0.0m;
                    ActiveEntity.PPAP_SAMPLE = 0.0m;
                    ActiveEntity.PACKING = null;
                    ActiveEntity.NATURE_PACKING = string.Empty;
                    ActiveEntity.SPL_CHAR = null;
                    ActiveEntity.OTHER_CUST_REQ = string.Empty;
                    ActiveEntity.ATP_DATE = null;
                    ActiveEntity.SIMILAR_PART_NO = string.Empty;
                    ActiveEntity.GENERAL_REMARKS = string.Empty;
                    ActiveEntity.MONTHLY = 0.0m;
                    ActiveEntity.MKTG_COMMITED_DATE = null;
                    ActiveEntity.COATING_CODE = string.Empty;
                    ActiveEntity.REALISATION = 0.0m;
                    ActiveEntity.NO_OF_PCS = ActiveEntity.NO_OF_PCS.ToValueAsString().ToIntValue() <= 0 ? 100 : ActiveEntity.NO_OF_PCS;
                    ActiveEntity.ZONE_CODE = string.Empty;
                    MandatoryFields.CUST_NAME = string.Empty;
                    MandatoryFields.RM_DESC = string.Empty;
                    ActiveEntity.IS_COMBINED = false;

                }

                CHEESE_WT = string.Empty;
                FINISH_WT = string.Empty;

                CostDetailEntities = bll.GetCostDetails(ActiveEntity);
                if (CIReferenceDataSource.IsNotNullOrEmpty()) CIReferenceDataSource.RowFilter = null;
                if (CIReferenceZoneDataSource.IsNotNullOrEmpty()) CIReferenceZoneDataSource.RowFilter = null;
                if (CustomersDataSource.IsNotNullOrEmpty()) CustomersDataSource.RowFilter = null;
                if (PlantDataSource.IsNotNullOrEmpty()) PlantDataSource.RowFilter = null;
                if (PreparedByDataSource.IsNotNullOrEmpty()) PreparedByDataSource.RowFilter = null;

                if (RawMaterialsDataSource.IsNotNullOrEmpty()) RawMaterialsDataSource.RowFilter = null;
                if (FinishDataSource.IsNotNullOrEmpty()) FinishDataSource.RowFilter = null;
                if (TopCoatingDataSource.IsNotNullOrEmpty()) TopCoatingDataSource.RowFilter = null;

                if (OperationDataSource.IsNotNullOrEmpty()) OperationDataSource.RowFilter = null;
                if (OperationCostDataSource.IsNotNullOrEmpty())
                {
                    OperationCostDataSource.RowFilter = null;
                    DVOperationCost = OperationCostDataSource.ToTable().Copy().DefaultView;
                }

                if (CostCentreOutputDataSource.IsNotNullOrEmpty())
                {
                    CostCentreOutputDataSource.RowFilter = null;
                    DVCostCentreOutput = CostCentreOutputDataSource.ToTable().Copy().DefaultView;
                }


            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private void ClearAll2(DDCI_INFO paramEntity)
        {
            try
            {

                //if (MandatoryFields.IsNotNullOrEmpty())
                //{
                //    MandatoryFields.CI_REFERENCE = "";
                //    MandatoryFields.CUST_NAME = "";
                //    MandatoryFields.CHEESE_WT = null;
                //    MandatoryFields.FINISH_WT = null;
                //    //MandatoryFields.RM_FACTOR = null;
                //    MandatoryFields.RM_FACTOR = 1.05m;
                //    MandatoryFields.LOC_CODE = "";
                //}

                //RejectReasonsVisibility = Visibility.Collapsed;
                if (paramEntity.IsNotNullOrEmpty())
                {
                    paramEntity.CI_REFERENCE = null;
                    //paramEntity.ENQU_RECD_ON = EnquiryReceivedOn();
                    paramEntity.FR_CS_DATE = null;
                    paramEntity.PROD_DESC = string.Empty;
                    paramEntity.CUST_CODE = null;
                    paramEntity.CUST_DWG_NO = string.Empty;
                    paramEntity.CUST_DWG_NO_ISSUE = string.Empty;
                    paramEntity.EXPORT = string.Empty;
                    paramEntity.NUMBER_OFF = null;
                    paramEntity.POTENTIAL = null;
                    paramEntity.SFL_SHARE = null;
                    paramEntity.REMARKS = string.Empty;
                    paramEntity.RESPONSIBILITY = null;
                    paramEntity.PENDING = string.Empty;
                    paramEntity.FEASIBILITY = "1";
                    paramEntity.REJECT_REASON = string.Empty;
                    paramEntity.LOC_CODE = null;
                    paramEntity.CHEESE_WT = null;
                    paramEntity.FINISH_WT = null;
                    paramEntity.FINISH_CODE = null;
                    paramEntity.COATING_CODE = null;
                    paramEntity.SUGGESTED_RM = null;
                    paramEntity.RM_COST = 0.0m;
                    paramEntity.FINAL_COST = 0.0m;
                    paramEntity.COST_NOTES = string.Empty;
                    paramEntity.PROCESSED_BY = null;
                    paramEntity.ORDER_DT = null;
                    paramEntity.PRINT = string.Empty;
                    paramEntity.ALLOT_PART_NO = 0.0m;
                    paramEntity.PART_NO_REQ_DATE = null;
                    paramEntity.CUST_STD_NO = string.Empty;
                    paramEntity.CUST_STD_DATE = null;
                    paramEntity.AUTOPART = string.Empty;
                    paramEntity.SAFTYPART = string.Empty;
                    paramEntity.APPLICATION = string.Empty;
                    paramEntity.STATUS = 0.0m;
                    paramEntity.CUSTOMER_NEED_DT = null;
                    paramEntity.MKTG_COMMITED_DT = null;
                    paramEntity.PPAP_LEVEL = string.Empty;
                    paramEntity.DEVL_METHOD = 0.0m;
                    paramEntity.PPAP_FORGING = 0.0m;
                    paramEntity.PPAP_SAMPLE = 0.0m;
                    paramEntity.PACKING = null;
                    paramEntity.NATURE_PACKING = string.Empty;
                    paramEntity.SPL_CHAR = null;
                    paramEntity.OTHER_CUST_REQ = string.Empty;
                    paramEntity.ATP_DATE = null;
                    paramEntity.SIMILAR_PART_NO = string.Empty;
                    paramEntity.GENERAL_REMARKS = string.Empty;
                    paramEntity.MONTHLY = 0.0m;
                    paramEntity.MKTG_COMMITED_DATE = null;
                    paramEntity.COATING_CODE = string.Empty;
                    paramEntity.REALISATION = 0.0m;
                    paramEntity.NO_OF_PCS = paramEntity.NO_OF_PCS.ToValueAsString().ToIntValue() <= 0 ? 100 : paramEntity.NO_OF_PCS;
                    paramEntity.ZONE_CODE = string.Empty;
                    //MandatoryFields.CUST_NAME = string.Empty;
                    //MandatoryFields.RM_DESC = string.Empty;

                }

                //CHEESE_WT = string.Empty;
                //FINISH_WT = string.Empty;

                //CostDetailEntities = bll.GetCostDetails(paramEntity);
                //if (CIReferenceDataSource.IsNotNullOrEmpty()) CIReferenceDataSource.RowFilter = null;
                //if (CIReferenceZoneDataSource.IsNotNullOrEmpty()) CIReferenceZoneDataSource.RowFilter = null;
                //if (CustomersDataSource.IsNotNullOrEmpty()) CustomersDataSource.RowFilter = null;
                //if (PlantDataSource.IsNotNullOrEmpty()) PlantDataSource.RowFilter = null;
                //if (PreparedByDataSource.IsNotNullOrEmpty()) PreparedByDataSource.RowFilter = null;

                //if (RawMaterialsDataSource.IsNotNullOrEmpty()) RawMaterialsDataSource.RowFilter = null;
                //if (FinishDataSource.IsNotNullOrEmpty()) FinishDataSource.RowFilter = null;
                //if (TopCoatingDataSource.IsNotNullOrEmpty()) TopCoatingDataSource.RowFilter = null;

                //if (OperationDataSource.IsNotNullOrEmpty()) OperationDataSource.RowFilter = null;
                //if (OperationCostDataSource.IsNotNullOrEmpty()) OperationCostDataSource.RowFilter = null;
                //if (CostCentreOutputDataSource.IsNotNullOrEmpty()) CostCentreOutputDataSource.RowFilter = null;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private readonly ICommand selectedItemChangedCommand;
        public ICommand SelectedItemChangedCommand { get { return this.selectedItemChangedCommand; } }
        private void SelectDataRow()
        {


        }

        private readonly ICommand addNewCommand;
        public ICommand AddNewClickCommand { get { return this.addNewCommand; } }
        private void AddNewSubmitCommand()
        {

            if (!ActionPermission.AddNew) return;
            Progress.ProcessingText = PDMsg.Load;
            Progress.Start();
            copyMandatoryFieldsToEntity(MandatoryFields);
            DDCI_INFO activeEntityCopy = new DDCI_INFO();
            activeEntityCopy.CI_REFERENCE = ActiveEntity.CI_REFERENCE;
            activeEntityCopy.IDPK = ActiveEntity.IDPK;

            ActiveEntity.LOC_CODE = MandatoryFields.LOC_CODE;
            Boolean isNotFirstTimeLoad = false;

            if (activeEntityDataTable.IsNotNullOrEmpty() && ActionMode == OperationMode.Edit)
                isNotFirstTimeLoad = true;

            if (isNotFirstTimeLoad)
            {
                activeEntityDataTable = (new List<DDCI_INFO>() { ActiveEntity }).ToDataTable<DDCI_INFO>();
                activeChildEntityDataTable = CostDetails.Table.Copy();
                activeStandardEntityDataTable = StandardNotes.Table.Copy();
            }

            DDCI_INFO ddci_info = new DDCI_INFO();
            ClearAll2(ddci_info);
            ddci_info.IDPK = -99999;
            ddci_info.ENQU_RECD_ON = EnquiryReceivedOn;

            ddci_info.FR_CS_DATE = null;
            ddci_info.CUST_STD_DATE = null;
            //CIReferenceSelectedRow = null;
            //isCIReferenceSelectionCompleted = true;
            //MandatoryFields.FEASIBILITY = "1";
            //IS_FEASIBILITY_CAN_CHANGE = true;
            //MandatoryFields.IsReadOnlyCI_REFERENCE = false;
            //copyMandatoryFieldsToEntity(MandatoryFields);
            originalEntityDataTable = (new List<DDCI_INFO>() { ddci_info }).ToDataTable<DDCI_INFO>();
            if (isNotFirstTimeLoad)
            {
                originalEntityDataTable = bll.GetEntitiesByPrimaryKey(activeEntityCopy).ToDataTable<DDCI_INFO>();
                if (originalEntityDataTable.IsNotNullOrEmpty() && originalEntityDataTable.Rows.Count == 0) originalEntityDataTable = activeEntityDataTable;
                originalStandardEntityDataTable = bll.GetStandardNotes().ToDataTable<DDSTD_NOTES>();
                originalChildEntityDataTable = bll.GetCostDetails(activeEntityCopy).ToDataTable<DDCOST_PROCESS_DATA>();
            }
            Progress.End();
            if (isChangesMade(1))
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SaveSubmitCommand("AddNewSubmitCommand");
                    return;
                }
            }

            ActionMode = OperationMode.AddNew;

            ChangeRights();

        }

        private readonly ICommand editCommand;
        public ICommand EditClickCommand { get { return this.editCommand; } }
        private void EditSubmitCommand()
        {
            if (isChangesMade(1))
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SaveSubmitCommand("EditSubmitCommand");
                    return;
                }
            }
            if (!ActionPermission.Edit) return;
            EntityPrimaryKey = -99999;
            ActionMode = OperationMode.Edit;
            ChangeRights();
        }

        private readonly ICommand deleteCommand;
        public ICommand DeleteClickCommand { get { return this.deleteCommand; } }
        private void DeleteSubmitCommand()
        {
            if (!ActionPermission.Delete) return;
            ActionMode = OperationMode.Delete;
            ChangeRights();
        }

        private readonly ICommand viewCommand;
        public ICommand ViewClickCommand { get { return this.viewCommand; } }
        private void ViewSubmitCommand()
        {
            if (!ActionPermission.View) return;
            ActionMode = OperationMode.View;
            ChangeRights();
        }

        private readonly ICommand printCommand;
        public ICommand PrintClickCommand { get { return this.printCommand; } }
        private void PrintSubmitCommand()
        {
            if (!ActionPermission.Print) return;

            if (!ActiveEntity.CI_REFERENCE.ToValueAsString().Trim().IsNotNullOrEmpty() || !MandatoryFields.CI_REFERENCE.ToValueAsString().Trim().IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("CI Reference Number"));
                return;
            }

            copyMandatoryFieldsToEntity(MandatoryFields);
            DDCI_INFO activeEntityCopy = new DDCI_INFO();
            activeEntityCopy.CI_REFERENCE = ActiveEntity.CI_REFERENCE;
            activeEntityCopy.IDPK = ActiveEntity.IDPK;

            ActiveEntity.LOC_CODE = MandatoryFields.LOC_CODE;
            Boolean isNotFirstTimeLoad = false;

            if (activeEntityDataTable.IsNotNullOrEmpty() && ActionMode == OperationMode.Edit)
                isNotFirstTimeLoad = true;

            if (isNotFirstTimeLoad)
            {
                activeEntityDataTable = (new List<DDCI_INFO>() { ActiveEntity }).ToDataTable<DDCI_INFO>();
                activeChildEntityDataTable = CostDetails.Table.Copy();
                activeStandardEntityDataTable = StandardNotes.Table.Copy();
            }

            DDCI_INFO ddci_info = new DDCI_INFO();
            ClearAll2(ddci_info);
            ddci_info.IDPK = -99999;
            ddci_info.ENQU_RECD_ON = EnquiryReceivedOn;

            ddci_info.FR_CS_DATE = null;
            ddci_info.CUST_STD_DATE = null;
            //CIReferenceSelectedRow = null;
            //isCIReferenceSelectionCompleted = true;
            //MandatoryFields.FEASIBILITY = "1";
            //IS_FEASIBILITY_CAN_CHANGE = true;
            //MandatoryFields.IsReadOnlyCI_REFERENCE = false;
            //copyMandatoryFieldsToEntity(MandatoryFields);
            originalEntityDataTable = (new List<DDCI_INFO>() { ddci_info }).ToDataTable<DDCI_INFO>();
            if (isNotFirstTimeLoad)
            {
                originalEntityDataTable = bll.GetEntitiesByPrimaryKey(activeEntityCopy).ToDataTable<DDCI_INFO>();
                if (originalEntityDataTable.IsNotNullOrEmpty() && originalEntityDataTable.Rows.Count == 0) originalEntityDataTable = activeEntityDataTable;
                originalStandardEntityDataTable = bll.GetStandardNotes().ToDataTable<DDSTD_NOTES>();
                originalChildEntityDataTable = bll.GetCostDetails(activeEntityCopy).ToDataTable<DDCOST_PROCESS_DATA>();
            }

            bool hasChanged = false;

            if (isChangesMade(1))
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    activeEntityCopy = ActiveEntity.DeepCopy<DDCI_INFO>();
                    int x = Save("PrintSubmitCommand");
                    if (x != 1) return;
                    bll = new FeasibleReportAndCostSheet(_userInformation);
                    ActiveEntity = activeEntityCopy;
                    hasChanged = true;
                }
            }

            //ActionMode = OperationMode.Print;
            DataSet dsReport = new DataSet("FRCS_REPORT");

            ////select * from DDFINISH_MAST where FINISH_CODE ='9700';
            ////16405

            ////select C.CI_INFO_FK from DDCOST_PROCESS_DATA c where c.ci_reference ='X030726002'
            ////order by sno asc;
            ////16908

            ////select CIREF_NO_FK from PRD_CIREF c where c.CI_REF ='X030726002';
            ////16908

            //UPDATE S SET S.CI_INFO_FK = M.IDPK FROM [dbo].[DDCOST_PROCESS_DATA] AS S 
            //            INNER JOIN [dbo].DDCI_INFO  AS M ON S.CI_REFERENCE = M.CI_REFERENCE WHERE  m.CI_REFERENCE ='X030726002'; 

            //UPDATE S SET S.CIREF_NO_FK = M.IDPK FROM [dbo].[PRD_CIREF] AS S 
            //            INNER JOIN [dbo].DDCI_INFO  AS M ON S.CI_REF = M.CI_REFERENCE WHERE  M.CI_REFERENCE ='X030726002'; 



            StringBuilder parentSql = new StringBuilder("SELECT * FROM DDCI_INFO WHERE IDPK = '" + ActiveEntity.IDPK + "'");
            StringBuilder childSql = new StringBuilder("SELECT * FROM DDCOST_PROCESS_DATA WHERE CI_INFO_FK = '" + ActiveEntity.IDPK + "' ORDER BY SNO ASC");
            StringBuilder partNumberSql = new StringBuilder("SELECT * FROM PRD_CIREF C WHERE C.CIREF_NO_FK = '" + ActiveEntity.IDPK + "'");

            List<StringBuilder> sqlList = new List<StringBuilder>() { parentSql, childSql, partNumberSql };

            dsReport = bll.Dal.GetDataSet(sqlList);
            dsReport.DataSetName = "FRCS_REPORT";

            if (!dsReport.IsNotNullOrEmpty() || dsReport.Tables.Count < 2) return;

            DataTable dtDDCI_INFO = dsReport.Tables[0];
            DataTable dtPartNumber = dsReport.Tables[2].Copy();

            dsReport.Tables.Remove(dsReport.Tables[2]);

            DataColumn parentColumn = null;
            DataColumn childColumn = null;
            ForeignKeyConstraint foreignKeyConstraint = null;

            if (dtDDCI_INFO.IsNotNullOrEmpty() && dtDDCI_INFO.Rows.Count > 0)
            {
                dtDDCI_INFO.TableName = "DDCI_INFO";
                if (!dtDDCI_INFO.Columns.Contains("CUST_NAME")) dtDDCI_INFO.Columns.Add("CUST_NAME");
                if (!dtDDCI_INFO.Columns.Contains("RM_DESC")) dtDDCI_INFO.Columns.Add("RM_DESC");
                if (!dtDDCI_INFO.Columns.Contains("RM_RATE")) dtDDCI_INFO.Columns.Add("RM_RATE");
                if (!dtDDCI_INFO.Columns.Contains("RM_WEIGHT")) dtDDCI_INFO.Columns.Add("RM_WEIGHT");
                if (!dtDDCI_INFO.Columns.Contains("EXPORT_YES_NO")) dtDDCI_INFO.Columns.Add("EXPORT_YES_NO");
                if (!dtDDCI_INFO.Columns.Contains("FINISH_DESC")) dtDDCI_INFO.Columns.Add("FINISH_DESC");
                if (!dtDDCI_INFO.Columns.Contains("COST_NOTES_TITLE")) dtDDCI_INFO.Columns.Add("COST_NOTES_TITLE");
                if (!dtDDCI_INFO.Columns.Contains("SFL_SHARE_INT")) dtDDCI_INFO.Columns.Add("SFL_SHARE_INT");
                if (!dtDDCI_INFO.Columns.Contains("PART_NUMBER")) dtDDCI_INFO.Columns.Add("PART_NUMBER");

                DataRow dataRow = dtDDCI_INFO.Rows[0];

                if (dtPartNumber.IsNotNullOrEmpty() && dtPartNumber.Rows.Count > 0)
                {
                    dataRow["PART_NUMBER"] = dtPartNumber.Rows[0]["PART_NO"].ToValueAsString().Trim();
                }
                dataRow["POTENTIAL"] = Math.Round(dataRow["POTENTIAL"].ToValueAsString().Trim().ToDecimalValue(), 2);

                dataRow["FEASIBILITY"] = dataRow["FEASIBILITY"].ToValueAsString().Trim();

                dataRow["CUST_NAME"] = dataRow["CUST_CODE"].ToValueAsString();
                CustomersDataSource.RowFilter = "CUST_CODE='" + dataRow["CUST_CODE"].ToValueAsString().FormatEscapeChars() + "'";
                if (CustomersDataSource.Count > 0)
                {
                    dataRow["CUST_NAME"] = dataRow["CUST_CODE"].ToValueAsString() + " - " +
                                           CustomersDataSource[0]["CUST_NAME"].ToValueAsString();

                    dataRow["SFL_SHARE"] = dataRow["SFL_SHARE"].ToValueAsString().ToIntValue();
                    dataRow.EndEdit();
                }
                CustomersDataSource.RowFilter = null;

                dataRow["RM_DESC"] = dataRow["SUGGESTED_RM"].ToValueAsString();
                RawMaterialsDataSource.RowFilter = "RM_CODE='" + dataRow["SUGGESTED_RM"].ToValueAsString().FormatEscapeChars() + "'";
                if (RawMaterialsDataSource.Count > 0)
                {
                    dataRow["RM_DESC"] = RawMaterialsDataSource[0]["RM_DESC"].ToValueAsString();

                    dataRow["RM_RATE"] = "@ " + (dataRow["EXPORT"].ToBooleanAsString() ? RawMaterialsDataSource[0]["EXP_COST"].ToValueAsString() :
                                         RawMaterialsDataSource[0]["LOC_COST"].ToValueAsString()) + " /Kg";

                    dataRow.EndEdit();
                }
                RawMaterialsDataSource.RowFilter = null;

                dataRow["FINISH_DESC"] = dataRow["FINISH_CODE"].ToValueAsString();
                FinishDataSource.RowFilter = "FINISH_CODE='" + dataRow["FINISH_CODE"].ToValueAsString().FormatEscapeChars() + "'";
                if (FinishDataSource.Count > 0)
                {
                    //dataRow["FINISH_DESC"] = dataRow["FINISH_CODE"].ToValueAsString() + " - " +
                    //                       FinishDataSource[0]["FINISH_DESC"].ToValueAsString();

                    dataRow["FINISH_DESC"] = FinishDataSource[0]["FINISH_DESC"].ToValueAsString();

                    dataRow["SFL_SHARE_INT"] = dataRow["SFL_SHARE"].ToValueAsString().ToIntValue();
                    dataRow.EndEdit();
                }
                FinishDataSource.RowFilter = null;

                //SelectedValue="{Binding ActiveEntity.COATING_CODE,UpdateSourceTrigger=PropertyChanged,Mode=TwoWay}" 
                //                   DataSource="{Binding TopCoatingDataSource}"

                TopCoatingDataSource.RowFilter = "COATING_CODE='" + dataRow["COATING_CODE"].ToValueAsString().FormatEscapeChars() + "'";
                if (TopCoatingDataSource.Count > 0)
                {
                    dataRow["FINISH_DESC"] = dataRow["FINISH_DESC"].ToValueAsString() + (dataRow["FINISH_DESC"].ToValueAsString().Trim().Length == 0 ? "" : " + ") + TopCoatingDataSource[0]["COATING_DESC"].ToValueAsString();

                    dataRow.EndEdit();
                }
                TopCoatingDataSource.RowFilter = null;

                dataRow["SFL_SHARE_INT"] = dataRow["SFL_SHARE"].ToValueAsString().ToIntValue();

                if (dtDDCI_INFO.PrimaryKey.IsNotNullOrEmpty() && dtDDCI_INFO.PrimaryKey.Length == 0)
                {
                    DataColumn[] primaryKeyColumns = new DataColumn[1];
                    primaryKeyColumns[0] = dtDDCI_INFO.Columns["IDPK"];
                    parentColumn = primaryKeyColumns[0];

                    dtDDCI_INFO.PrimaryKey = primaryKeyColumns;

                }

                dataRow["RM_WEIGHT"] = Math.Round(dataRow["CHEESE_WT"].ToValueAsString().ToDecimalValue() * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue(), 2);

                dataRow["EXPORT_YES_NO"] = dataRow["EXPORT"].ToBooleanAsString() ? "YES" : "NO";
                dataRow["COST_NOTES_TITLE"] = dataRow["FEASIBILITY"].ToBooleanAsString() ? "Cost Notes :" :
                                              "Enquiry rejected for the following reasons :";

                dataRow["COST_NOTES"] = dataRow["FEASIBILITY"].ToBooleanAsString() ? dataRow["COST_NOTES"].ToValueAsString() :
                                        dataRow["REJECT_REASON"].ToValueAsString();
                dataRow.EndEdit();
            }

            DataTable dtDDCOST_PROCESS_DATA = dsReport.Tables[1];
            if (dtDDCOST_PROCESS_DATA.IsNotNullOrEmpty() && dtDDCOST_PROCESS_DATA.Rows.Count == 0)
            {
                DataRow dataRow = dtDDCOST_PROCESS_DATA.Rows.Add();
                dataRow["CI_INFO_FK"] = ActiveEntity.IDPK;
                dataRow.EndEdit();
            }
            dtDDCOST_PROCESS_DATA.TableName = "DDCOST_PROCESS_DATA";

            if (!dtDDCOST_PROCESS_DATA.Columns.Contains("SNO_INT")) dtDDCOST_PROCESS_DATA.Columns.Add("SNO_INT");

            foreach (DataRow dataRow in dtDDCOST_PROCESS_DATA.Rows)
            {
                if (dataRow["SNO"].ToValueAsString().IsNotNullOrEmpty())
                {
                    dataRow["SNO_INT"] = dataRow["SNO"].ToValueAsString().ToIntValue();
                    dataRow.EndEdit();
                }
            }

            if (dtDDCOST_PROCESS_DATA.PrimaryKey.IsNotNullOrEmpty() && dtDDCOST_PROCESS_DATA.PrimaryKey.Length == 0)
            {
                if (dtDDCOST_PROCESS_DATA.Columns.Contains("IDPK")) dtDDCOST_PROCESS_DATA.Columns.Remove("IDPK");

                dtDDCOST_PROCESS_DATA.Columns["CI_INFO_FK"].ColumnName = "IDPK";
                childColumn = dtDDCOST_PROCESS_DATA.Columns["IDPK"];

                foreignKeyConstraint = new ForeignKeyConstraint("CPDForeignKeyConstraint", parentColumn, childColumn);
                foreignKeyConstraint.DeleteRule = Rule.SetNull;
                foreignKeyConstraint.UpdateRule = Rule.Cascade;
                foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;

                dsReport.Tables["DDCOST_PROCESS_DATA"].Constraints.Add(foreignKeyConstraint);
                dsReport.EnforceConstraints = true;

                //string path;
                //path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
                //dsReport.WriteXmlSchema("E:\\" + dsReport.DataSetName + ".xml");

            }

            if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count < 0)
            {
                ShowInformationMessage(PDMsg.NoRecordsPrint);
                return;
            }

            frmReportViewer reportViewer = new frmReportViewer(dsReport, "FRCS");
            if (!reportViewer.ReadyToShowReport) return;
            reportViewer.ShowDialog();

            if (hasChanged == true)
            {
                ActiveEntity = activeEntityCopy;
                EntityPrimaryKey = activeEntityCopy.IDPK;
                ActionMode = OperationMode.Edit;
                ChangeRights();
            }
        }

        private readonly ICommand refreshCommand;
        public ICommand RefreshClickCommand { get { return this.refreshCommand; } }
        private void RefreshSubmitCommand()
        {
            WeightCalculation();
            CostCalculation();
        }

        private readonly ICommand saveCommand;
        public ICommand SaveClickCommand { get { return this.saveCommand; } }
        private void SaveSubmitCommand(string callingfrom = "Save")
        {
            IsDefaultFocused = true;
            if (!ActionPermission.Save) return;
            Save(callingfrom);
        }

        private int Save(string callingfrom = "Save")
        {
            int updatedRecord = 0;
            copyMandatoryFieldsToEntity(MandatoryFields);

            if (!ActiveEntity.CI_REFERENCE.ToValueAsString().Trim().IsNotNullOrEmpty() || !MandatoryFields.CI_REFERENCE.ToValueAsString().Trim().IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("CI Reference Number"));
                return updatedRecord;
            }

            if (!ActiveEntity.CUST_CODE.IsNotNullOrEmpty() || !MandatoryFields.CUST_NAME.ToValueAsString().Trim().IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Customer Name"));
                return updatedRecord;
            }

            if (!ActiveEntity.LOC_CODE.IsNotNullOrEmpty() || !MandatoryFields.LOC_CODE.ToValueAsString().Trim().IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Plant"));
                return updatedRecord;
            }

            //if (!ActiveEntity.FINISH_WT.IsNotNullOrEmpty() || !MandatoryFields.FINISH_WT.ToValueAsString().Trim().IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Finish Weight"));
            //    return;
            //}

            if (!ActiveEntity.CHEESE_WT.IsNotNullOrEmpty() || !MandatoryFields.CHEESE_WT.ToValueAsString().Trim().IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Cheese Weight"));
                return updatedRecord;
            }


            if (!MandatoryFields.RM_FACTOR.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Multiplication Factor"));
                return updatedRecord;
            }

            if (ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue() < 1.00m || ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue() > 1.15m)
            {
                ShowInformationMessage("Multiplication Factor should be between 1.00 and 1.15");
                return updatedRecord;
            }

            if (ActiveEntity.FINISH_WT.ToValueAsString().ToDecimalValue() > ActiveEntity.CHEESE_WT.ToValueAsString().ToDecimalValue() &&
                !ActiveEntity.IS_COMBINED.ToValueAsString().ToBooleanAsString())
            {
                ShowInformationMessage("Finish Weight is greater than Cheese Weight.\r\nPlease make sure it is correct.");
                return updatedRecord;
            }

            ActiveEntity.SFL_SHARE = MandatoryFields.SFL_SHARE.ToDecimalValue();
            if (ActiveEntity.SFL_SHARE.ToValueAsString().ToDecimalValue() > 100)
            {
                ShowInformationMessage("SFL Share Should not exceed 100");
                return updatedRecord;
            }

            if (ActionMode == OperationMode.AddNew)
            {
                if (!isValidCustomer()) return updatedRecord;
                if (!isValidPlant()) return updatedRecord;
                if (!isValidPreparedBy()) return updatedRecord;
                if (!isValidRawMaterial()) return updatedRecord;
                if (!isValidFinish()) return updatedRecord;
                if (!isValidCoating()) return updatedRecord;
                if (!isValidCostDetails()) return updatedRecord;
            }
            else
            {
                if (!isValidCostDetails("N")) return updatedRecord;
            }


            if (!verifyCIReferenceNumber()) return updatedRecord;

            string outMessage;
            if (!bll.IsValidCIReferenceNumber(ActiveEntity, ActionMode, out outMessage) && outMessage.IsNotNullOrEmpty())
            {
                ShowInformationMessage(outMessage);
                return updatedRecord;
            }



            if (ActiveEntity.RM_COST > 9999999999.99m)
            {
                ShowInformationMessage(PDMsg.NotExceeds("Cost for 100 Pcs", "9999999999.99"));
                return updatedRecord;
            }

            if (ActiveEntity.FINAL_COST > 9999999999.99m)
            {
                ShowInformationMessage(PDMsg.NotExceeds("Cost for " + ActiveEntity.NO_OF_PCS + " Pcs", "9999999999.99"));
                return updatedRecord;
            }

            if (ActiveEntity.REALISATION > 99999999.99m)
            {
                ShowInformationMessage(PDMsg.NotExceeds("Realisation", "99999999.99"));
                return updatedRecord;
            }

            try
            {

                List<DDCI_INFO> lstResult = null;
                List<DDCOST_PROCESS_DATA> lstAssociationEntity = null;
                List<DDSTD_NOTES> lstStandardNotes = null;
                List<DDSHAPE_DETAILS> ddshape_details = null;

                bool isExecuted = false;
                switch (ActionMode)
                {
                    case OperationMode.AddNew:
                        #region Add Operation

                        lstResult = (from row in bll.GetEntitiesByCIReferenceNumber(ActiveEntity)
                                     select row).ToList<DDCI_INFO>();

                        if (lstResult.IsNotNullOrEmpty() && lstResult.Count > 0)
                        {
                            ShowInformationMessage("CI Reference Number " + ActiveEntity.CI_REFERENCE + " already exists.\r\nEnter another CI Reference Number");
                            return updatedRecord;
                        }

                        lstResult = (from row in bll.DB.DDCI_INFO
                                     where row.CUST_DWG_NO == ActiveEntity.CUST_DWG_NO && Convert.ToString(row.CUST_DWG_NO).Length > 0
                                     select row).ToList<DDCI_INFO>();

                        if (lstResult.IsNotNullOrEmpty() && lstResult.Count > 0)
                        {

                            //ShowInformationMessage("Customer Drawing Number already exists");
                            ShowInformationMessage(PDMsg.AlreadyExists("Customer Part Number"));
                            return updatedRecord;
                        }

                        lstStandardNotes = (from row in StandardNotesDeletedList.AsEnumerable()
                                            select new DDSTD_NOTES()
                                            {
                                                SNO = Convert.ToDecimal(row.Field<string>("SNO")),
                                                STD_NOTES = row.Field<string>("STD_NOTES"),
                                                ROWID = Guid.NewGuid(),
                                            }).ToList<DDSTD_NOTES>();
                        if (lstStandardNotes.IsNotNullOrEmpty() && lstStandardNotes.Count > 0)
                        {
                            isExecuted = bll.Delete<DDSTD_NOTES>(lstStandardNotes);
                            StandardNotesDeletedList.Rows.Clear();
                        }

                        lstStandardNotes = (from row in StandardNotes.ToTable().AsEnumerable()
                                            select new DDSTD_NOTES()
                                            {
                                                SNO = Convert.ToDecimal(row.Field<string>("SNO")),
                                                STD_NOTES = row.Field<string>("STD_NOTES"),
                                                ROWID = Guid.NewGuid(),
                                            }).ToList<DDSTD_NOTES>();

                        if (lstStandardNotes.IsNotNullOrEmpty() && lstStandardNotes.Count > 0)
                        {
                            var lstRecordCount = (from row in lstStandardNotes
                                                  group row by row.STD_NOTES into grpStandardNotes
                                                  where grpStandardNotes.Count() > 1
                                                  select new { Key = grpStandardNotes.Key, Count = grpStandardNotes.Count() }).ToList<object>();

                            if (lstRecordCount.IsNotNullOrEmpty())
                            {
                                foreach (var item in lstRecordCount)
                                {
                                    ShowInformationMessage("Duplicate Standard Notes '" + item.GetFieldValue("Key").ToValueAsString() + "' has been Entered");
                                    return updatedRecord;
                                }
                            }

                            isExecuted = bll.Update<DDSTD_NOTES>(lstStandardNotes);
                        }

                        foreach (DDCOST_PROCESS_DATA associationEntity in ActiveEntity.DDCOST_PROCESS_DATA)
                        {
                            ActiveEntity.DDCOST_PROCESS_DATA.Remove(associationEntity);
                        }

                        isExecuted = bll.Update<DDCI_INFO>(new List<DDCI_INFO>() { ActiveEntity.DeepCopy<DDCI_INFO>() });

                        DDCI_INFO parentEntity = (from row in bll.GetEntitiesByCIReferenceNumber(ActiveEntity.DeepCopy<DDCI_INFO>())
                                                  select row).FirstOrDefault<DDCI_INFO>();

                        if (isExecuted && parentEntity.IsNotNullOrEmpty())
                        {
                            if (CostDetails.HasNonEmptyCells())
                            {
                                lstAssociationEntity = getAssociationEntity(parentEntity);

                                //lstAssociationEntity = (from row in CostDetails.ToTable().AsEnumerable()
                                //                        where (row.Field<string>("OPERATION_NO").ToValueAsString().Trim().Length > 0 ||
                                //                        row.Field<string>("OPERATION").ToValueAsString().Trim().Length > 0 ||
                                //                        row.Field<string>("COST_CENT_CODE").ToValueAsString().Trim().Length > 0 ||
                                //                        row.Field<string>("OUTPUT").ToValueAsString().Trim().Length > 0)
                                //                        select new DDCOST_PROCESS_DATA()
                                //                        {
                                //                            CI_REFERENCE = parentEntity.CI_REFERENCE,
                                //                            SNO = Convert.ToDecimal(Convert.ToString(row.Field<decimal>("SNO"))),
                                //                            OPERATION_NO = Convert.ToDecimal(row.Field<string>("OPERATION_NO")),
                                //                            OPERATION = row.Field<string>("OPERATION"),
                                //                            COST_CENT_CODE = row.Field<string>("COST_CENT_CODE"),
                                //                            OUTPUT = Convert.ToDecimal(row.Field<string>("OUTPUT")),
                                //                            VAR_COST = Convert.ToDecimal(row.Field<string>("VAR_COST")),
                                //                            FIX_COST = Convert.ToDecimal(row.Field<string>("FIX_COST")),
                                //                            SPL_COST = Convert.ToDecimal(row.Field<string>("SPL_COST")),
                                //                            UNIT_OF_MEASURE = row.Field<string>("UNIT_OF_MEASURE"),
                                //                            TOTAL_COST = Convert.ToDecimal(row.Field<string>("TOTAL_COST")),
                                //                            IDPK = row.Field<string>("IDPK").ToIntValue(),
                                //                            CI_INFO_FK = parentEntity.IDPK,
                                //                            ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                //                        }).ToList<DDCOST_PROCESS_DATA>();
                                if (lstAssociationEntity.IsNotNullOrEmpty())
                                    isExecuted = bll.Update<DDCOST_PROCESS_DATA>(lstAssociationEntity);
                            }
                        }

                        ddshape_details = (from row in bll.GetShapeDetailsByCIReference(parentEntity)
                                           select new DDSHAPE_DETAILS()
                                           {
                                               CI_REFERENCE = row.CI_REFERENCE,
                                               SHAPE_CODE = row.SHAPE_CODE,
                                               WEIGHT_OPTION = row.WEIGHT_OPTION,
                                               HEAD1 = row.HEAD1,
                                               VAL1 = row.VAL1,
                                               HEAD2 = row.HEAD2,
                                               VAL2 = row.VAL2,
                                               HEAD3 = row.HEAD3,
                                               VAL3 = row.VAL3,
                                               VOLUME = row.VOLUME,
                                               SIGN = row.SIGN,
                                               SNO = row.SNO,
                                               ROWID = row.ROWID,
                                               IDPK = row.IDPK,
                                               CIREF_NO_FK = parentEntity.IDPK,
                                           }).ToList<DDSHAPE_DETAILS>();
                        if (isExecuted && ddshape_details.IsNotNullOrEmpty() && ddshape_details.Count > 0)
                        {
                            Progress.ProcessingText = PDMsg.ProgressUpdateText;
                            Progress.Start();

                            isExecuted = bll.Update<DDSHAPE_DETAILS>(ddshape_details);
                            Progress.End();
                        }

                        if (isExecuted)
                        {
                            ShowInformationMessage(PDMsg.SavedSuccessfully);
                            updatedRecord = 1;
                            _logviewBll.SaveLog(MandatoryFields.CI_REFERENCE, "FRCS");
                        }
                        #endregion
                        break;
                    case OperationMode.Edit:
                        #region Update Operation

                        if (lstResult.IsNotNullOrEmpty() && lstResult.Count == 0)
                        {
                            ShowInformationMessage(PDMsg.DoesNotExists("CI Reference"));
                            return updatedRecord;
                        }

                        lstResult = (from row in bll.DB.DDCI_INFO
                                     where row.CUST_DWG_NO == ActiveEntity.CUST_DWG_NO && Convert.ToString(row.CUST_DWG_NO).Length > 0
                                     && row.IDPK != ActiveEntity.IDPK
                                     select row).ToList<DDCI_INFO>();

                        if (lstResult.IsNotNullOrEmpty() && lstResult.Count > 0)
                        {
                            //ShowInformationMessage("Customer Drawing Number already exists");
                            ShowInformationMessage(PDMsg.AlreadyExists("Customer Part Number"));
                            return updatedRecord;
                        }

                        lstStandardNotes = (from row in StandardNotesDeletedList.AsEnumerable()
                                            select new DDSTD_NOTES()
                                            {
                                                SNO = Convert.ToDecimal(row.Field<string>("SNO")),
                                                STD_NOTES = row.Field<string>("STD_NOTES"),
                                                ROWID = Guid.NewGuid(),
                                            }).ToList<DDSTD_NOTES>();
                        if (lstStandardNotes.IsNotNullOrEmpty() && lstStandardNotes.Count > 0)
                        {
                            isExecuted = bll.Delete<DDSTD_NOTES>(lstStandardNotes);
                            StandardNotesDeletedList.Rows.Clear();
                        }

                        lstStandardNotes = (from row in StandardNotes.ToTable().AsEnumerable()
                                            select new DDSTD_NOTES()
                                            {
                                                SNO = Convert.ToDecimal(row.Field<string>("SNO")),
                                                STD_NOTES = row.Field<string>("STD_NOTES"),
                                                ROWID = Guid.NewGuid(),
                                            }).ToList<DDSTD_NOTES>();

                        if (lstStandardNotes.IsNotNullOrEmpty() && lstStandardNotes.Count > 0)
                        {
                            var lstRecordCount = (from row in lstStandardNotes
                                                  group row by row.STD_NOTES into grpStandardNotes
                                                  where grpStandardNotes.Count() > 1
                                                  select new { Key = grpStandardNotes.Key, Count = grpStandardNotes.Count() }).ToList<object>();

                            if (lstRecordCount.IsNotNullOrEmpty())
                            {
                                foreach (var item in lstRecordCount)
                                {
                                    ShowInformationMessage("Duplicate Standard Notes '" + item.GetFieldValue("Key").ToValueAsString() + "' has been Entered");
                                    //StandardNotes.Table.Clear();
                                    return updatedRecord;
                                }
                            }

                            isExecuted = bll.Update<DDSTD_NOTES>(lstStandardNotes);
                        }

                        parentEntity = (from row in bll.GetEntitiesByCIReferenceNumber(ActiveEntity.DeepCopy<DDCI_INFO>())
                                        select row).FirstOrDefault<DDCI_INFO>();
                        if (CostDetails.HasNonEmptyCells() && parentEntity.IsNotNullOrEmpty())
                        {
                            //lstAssociationEntity = (from row in CostDetails.ToTable().AsEnumerable()
                            //                        where row.Field<string>("SNO").ToValueAsString().Trim().Length > 0 &&
                            //                              row.Field<string>("OPERATION_NO").ToValueAsString().Trim().Length > 0 &&
                            //                              row.Field<string>("COST_CENT_CODE").ToValueAsString().Trim().Length > 0
                            //                        select new DDCOST_PROCESS_DATA()
                            //                        {
                            //                            CI_REFERENCE = ParentEntity.CI_REFERENCE,
                            //                            SNO = Convert.ToDecimal(row.Field<string>("S NO")),
                            //                            OPERATION_NO = Convert.ToDecimal(row.Field<string>("OPERATION_NO")),
                            //                            OPERATION = row.Field<string>("OPERATION"),
                            //                            COST_CENT_CODE = row.Field<string>("COST_CENT_CODE"),
                            //                            OUTPUT = Convert.ToDecimal(row.Field<string>("OUTPUT")),
                            //                            VAR_COST = Convert.ToDecimal(row.Field<string>("VAR_COST")),
                            //                            FIX_COST = Convert.ToDecimal(row.Field<string>("FIX_COST")),
                            //                            SPL_COST = Convert.ToDecimal(row.Field<string>("SPL_COST")),
                            //                            UNIT_OF_MEASURE = row.Field<string>("UNIT_OF_MEASURE"),
                            //                            TOTAL_COST = Convert.ToDecimal(row.Field<string>("TOTAL_COST")),
                            //                            IDPK = Convert.ToInt32(row.Field<string>("IDPK")),
                            //                            CI_INFO_FK = ParentEntity.IDPK,
                            //                            ROWID = row.Field<string>("ROWID").ToGuidValue(),
                            //                        }).ToList<DDCOST_PROCESS_DATA>();

                            lstAssociationEntity = getAssociationEntity(parentEntity);

                            //lstAssociationEntity = (from row in CostDetails.ToTable().AsEnumerable()
                            //                        where (row.Field<string>("OPERATION_NO").ToValueAsString().Trim().Length > 0 ||
                            //                        row.Field<string>("OPERATION").ToValueAsString().Trim().Length > 0 ||
                            //                        row.Field<string>("COST_CENT_CODE").ToValueAsString().Trim().Length > 0 ||
                            //                        row.Field<string>("OUTPUT").ToValueAsString().Trim().Length > 0)
                            //                        select new DDCOST_PROCESS_DATA()
                            //                        {
                            //                            CI_REFERENCE = parentEntity.CI_REFERENCE,
                            //                            SNO = Convert.ToDecimal(Convert.ToString(row.Field<decimal>("SNO"))),
                            //                            OPERATION_NO = Convert.ToDecimal(row.Field<string>("OPERATION_NO")),
                            //                            OPERATION = row.Field<string>("OPERATION"),
                            //                            COST_CENT_CODE = row.Field<string>("COST_CENT_CODE"),
                            //                            OUTPUT = Convert.ToDecimal(row.Field<string>("OUTPUT")),
                            //                            VAR_COST = Convert.ToDecimal(row.Field<string>("VAR_COST")),
                            //                            FIX_COST = Convert.ToDecimal(row.Field<string>("FIX_COST")),
                            //                            SPL_COST = Convert.ToDecimal(row.Field<string>("SPL_COST")),
                            //                            UNIT_OF_MEASURE = row.Field<string>("UNIT_OF_MEASURE"),
                            //                            TOTAL_COST = Convert.ToDecimal(row.Field<string>("TOTAL_COST")),
                            //                            IDPK = row.Field<string>("IDPK").ToIntValue(),
                            //                            CI_INFO_FK = parentEntity.IDPK,
                            //                            ROWID = row.Field<string>("ROWID").ToGuidValue(),
                            //                        }).ToList<DDCOST_PROCESS_DATA>();

                            //isExecuted = bll.Delete<DDCOST_PROCESS_DATA>(lstAssociationEntity);
                            if (lstAssociationEntity.IsNotNullOrEmpty() && lstAssociationEntity.Count > 0 && CostDetailEntities.IsNotNullOrEmpty() && CostDetailEntities.Count > 0 && CostDetailEntities.Count != lstAssociationEntity.Count)
                            {
                                isExecuted = bll.Delete<DDCOST_PROCESS_DATA>(CostDetailEntities);
                                isExecuted = bll.Insert<DDCOST_PROCESS_DATA>(lstAssociationEntity);
                            }
                            else if (lstAssociationEntity.IsNotNullOrEmpty() && lstAssociationEntity.Count > 0 && CostDetailEntities.IsNotNullOrEmpty() && CostDetailEntities.Count == 0 && CostDetailEntities.Count != lstAssociationEntity.Count)
                            {
                                isExecuted = bll.Delete<DDCOST_PROCESS_DATA>(CostDetailEntities);
                                isExecuted = bll.Insert<DDCOST_PROCESS_DATA>(lstAssociationEntity);
                            }
                            else if (originalEntityDataTable.IsNotNullOrEmpty() && originalEntityDataTable.Rows.Count > 0 && ((!lstAssociationEntity.IsNotNullOrEmpty() || (lstAssociationEntity.IsNotNullOrEmpty() && lstAssociationEntity.Count == 0)) && CostDetails.IsNotNullOrEmpty() && CostDetails.Table.IsNotNullOrEmpty() && CostDetails.Table.Rows.Count <= 1))
                            {
                                isExecuted = bll.Delete<DDCOST_PROCESS_DATA>(CostDetailEntities);
                            }
                            else if (lstAssociationEntity.IsNotNullOrEmpty() && lstAssociationEntity.Count == 0 && CostDetailEntities.IsNotNullOrEmpty() && CostDetailEntities.Count > 0 && CostDetailEntities.Count != lstAssociationEntity.Count)
                            {
                                isExecuted = bll.Delete<DDCOST_PROCESS_DATA>(CostDetailEntities);
                            }
                            else if (lstAssociationEntity.IsNotNullOrEmpty() && lstAssociationEntity.Count > 0)
                            {
                                isExecuted = bll.Delete<DDCOST_PROCESS_DATA>(CostDetailEntities);
                                isExecuted = bll.Insert<DDCOST_PROCESS_DATA>(lstAssociationEntity);
                            }
                        }
                        if (isExecuted)
                        {
                            foreach (DDCOST_PROCESS_DATA associationEntity in ActiveEntity.DDCOST_PROCESS_DATA)
                            {
                                ActiveEntity.DDCOST_PROCESS_DATA.Remove(associationEntity);
                            }

                            isExecuted = bll.Update<DDCI_INFO>(new List<DDCI_INFO>() { ActiveEntity.DeepCopy<DDCI_INFO>() });

                        }

                        ddshape_details = (from row in bll.GetShapeDetailsByCIReference(parentEntity)
                                           select new DDSHAPE_DETAILS()
                                           {
                                               CI_REFERENCE = row.CI_REFERENCE,
                                               SHAPE_CODE = row.SHAPE_CODE,
                                               WEIGHT_OPTION = row.WEIGHT_OPTION,
                                               HEAD1 = row.HEAD1,
                                               VAL1 = row.VAL1,
                                               HEAD2 = row.HEAD2,
                                               VAL2 = row.VAL2,
                                               HEAD3 = row.HEAD3,
                                               VAL3 = row.VAL3,
                                               VOLUME = row.VOLUME,
                                               SIGN = row.SIGN,
                                               SNO = row.SNO,
                                               ROWID = row.ROWID,
                                               IDPK = row.IDPK,
                                               CIREF_NO_FK = parentEntity.IDPK,
                                           }).ToList<DDSHAPE_DETAILS>();
                        if (isExecuted && ddshape_details.IsNotNullOrEmpty() && ddshape_details.Count > 0)
                        {
                            isExecuted = bll.Update<DDSHAPE_DETAILS>(ddshape_details);
                        }

                        if (isExecuted)
                        {
                            CostDetailEntities = bll.GetCostDetails(ActiveEntity);
                            updatedRecord = 1;
                            ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                            _logviewBll.SaveLog(MandatoryFields.CI_REFERENCE, "FRCS");
                        }

                        #endregion
                        break;
                    case OperationMode.Delete:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            if (callingfrom == "AddNewSubmitCommand")
            {
                bll = new FeasibleReportAndCostSheet(_userInformation);
                ActionMode = OperationMode.AddNew;
                CIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER>().DefaultView;
                StandardNotes = bll.GetStandardNotes().ToDataTable<DDSTD_NOTES>().DefaultView;
                ChangeRights();
                IsDefaultFocused = false;
            }
            return updatedRecord;
        }

        //private List<DDCOST_PROCESS_DATA> getAssociationEntity(DDCI_INFO parentEntity, int callerCode = 0)
        //{
        //    List<DDCOST_PROCESS_DATA> lstDDCOST_PROCESS_DATA = null;
        //    try
        //    {
        //        lstDDCOST_PROCESS_DATA = new List<DDCOST_PROCESS_DATA>();
        //        foreach (DataRow row in CostDetails.Table.Rows)
        //        {
        //            DDCOST_PROCESS_DATA cpd = new DDCOST_PROCESS_DATA();
        //            if (row["SNO"].IsNotNullOrEmpty() && row["OPERATION_NO"].IsNotNullOrEmpty() && row["COST_CENT_CODE"].IsNotNullOrEmpty())
        //            {
        //                cpd.CI_REFERENCE = ActiveEntity.CI_REFERENCE;
        //                cpd.SNO = (callerCode == 1 ? callerCode++ : Convert.ToDecimal(row["SNO"].ToValueAsString()));
        //                cpd.OPERATION_NO = Convert.ToDecimal(row["OPERATION_NO"].ToValueAsString());
        //                cpd.OPERATION = row["OPERATION"].ToValueAsString();
        //                cpd.COST_CENT_CODE = Convert.ToString(row["COST_CENT_CODE"].ToValueAsString());
        //                cpd.OUTPUT = Convert.ToDecimal(row["OUTPUT"].ToValueAsString());
        //                cpd.VAR_COST = Convert.ToDecimal(row["VAR_COST"].ToValueAsString());
        //                cpd.FIX_COST = Convert.ToDecimal(row["FIX_COST"].ToValueAsString());
        //                cpd.SPL_COST = Convert.ToDecimal(row["SPL_COST"].ToValueAsString());
        //                cpd.UNIT_OF_MEASURE = row["UNIT_OF_MEASURE"].ToValueAsString();
        //                cpd.TOTAL_COST = Convert.ToDecimal(row["TOTAL_COST"].ToValueAsString());
        //                cpd.IDPK = row["IDPK"].ToValueAsString().ToIntValue();
        //                cpd.CI_INFO_FK = parentEntity.IDPK;
        //                cpd.PROCESS_CODE = (callerCode == 1 ? callerCode++ : Convert.ToDecimal(Convert.ToString(row["SNO"].ToValueAsString())));
        //                lstDDCOST_PROCESS_DATA.Add(cpd);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //ex.LogException();
        //    }
        //    return lstDDCOST_PROCESS_DATA;
        //}

        private List<DDCOST_PROCESS_DATA> getAssociationEntity(DDCI_INFO parentEntity, int callerCode = 0)
        {
            Console.WriteLine("Begin" + DateTime.Now.ToString("hh:mm:ss.fff"));
            try
            {
                //CostDetails.RowFilter = "CONVERT(Isnull(SNO,''), System.String) = ''";
                //if (CostDetails.Count > 1)
                //{
                //    CostDetails.RowFilter = null;                    
                //}
                CostDetails.RowFilter = null;

                //CostDetails.Table.AcceptChanges();
                //DataView dt = CostDetails;                

                //dt.Table.AcceptChanges();
                //dt.Sort = null;      

                DataView dv = CostDetails.ToTable().Copy().DefaultView;

                DataTable dataTable = dv.ToTable().Copy();
                dataTable.Rows.Clear();
                DataTable source = dv.ToTable().Copy();

                int rowsCount = source.Rows.Count;
                for (int rowIndex = 0; rowIndex <= rowsCount - 1; rowIndex++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    int columnNullValueCount = 0;
                    foreach (DataColumn dataColumn in source.Columns)
                    {
                        if (CostDetails[rowIndex]["SNO"].ToValueAsString().Length == 0 && CostDetails[rowIndex]["OPERATION_NO"].ToValueAsString().Length == 0 && CostDetails[rowIndex]["OPERATION"].ToValueAsString().Length == 0 && CostDetails[rowIndex]["COST_CENT_CODE"].ToValueAsString().Length == 0 && CostDetails[rowIndex]["OUTPUT"].ToValueAsString().Length == 0)
                        {
                            continue;
                        }

                        try
                        {
                            //dataRow[dataColumn.ColumnName] = Convert.ChangeType(CostDetails[rowIndex][dataColumn.ColumnName].ToValueAsString(), dataColumn.DataType);
                            dataRow[dataColumn.ColumnName] = CostDetails[rowIndex][dataColumn.ColumnName];

                            if (!dataRow[dataColumn.ColumnName].IsNotNullOrEmpty())
                                dataRow[dataColumn.ColumnName] = dv[rowIndex][dataColumn.ColumnName];

                            switch (dataColumn.ColumnName)
                            {
                                case "COST_CENT_CODE":
                                case "CCCode":
                                    if (source.Rows[rowIndex]["CCCode"].ToValueAsString().IsNotNullOrEmpty() &&
                                        source.Rows[rowIndex]["COST_CENT_CODE"].ToValueAsString().Length == 0)
                                        dataRow["COST_CENT_CODE"] = dataRow["CCCode"].ToValueAsString();
                                    break;
                                case "OUTPUT":
                                    dataRow[dataColumn.ColumnName] = dataRow[dataColumn.ColumnName].ToValueAsString().ToDecimalValue().ToString();
                                    break;
                                case "VAR_COST":
                                case "FIX_COST":
                                case "SPL_COST":
                                case "TOTAL_COST":
                                    dataRow[dataColumn.ColumnName] = dataRow[dataColumn.ColumnName].ToValueAsString().ToDoubleValue().ToString("0.00");
                                    break;
                                default:
                                    dataRow[dataColumn.ColumnName] = CostDetails[rowIndex][dataColumn.ColumnName];
                                    break;
                            }
                            dataRow.EndEdit();
                            if (Convert.ToString(dataRow[dataColumn.ColumnName]).Length == 0 && (dataColumn.ColumnName == "OPERATION_NO" || dataColumn.ColumnName == "CCCode" || dataColumn.ColumnName == "OUTPUT"))
                            {
                                columnNullValueCount++;
                                Console.WriteLine(dataColumn.ColumnName + " : " + dataColumn.DataType.ToString() + " : " + dataRow[dataColumn.ColumnName] + " : " + dataRow[dataColumn.ColumnName].GetType().ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            dataRow.EndEdit();
                            ex.ShowAndLogException();
                        }
                    }
                    if (columnNullValueCount == 0)
                        dataTable.Rows.Add(dataRow);
                    dataTable.AcceptChanges();

                }

                List<DDCOST_PROCESS_DATA> lstDDCOST_PROCESS_DATA = (from row in dataTable.AsEnumerable()
                                                                    where (row.Field<string>("OPERATION_NO").ToValueAsString().Trim().Length > 0 ||
                                                                    row.Field<string>("OPERATION").ToValueAsString().Trim().Length > 0 ||
                                                                    row.Field<string>("COST_CENT_CODE").ToValueAsString().Trim().Length > 0 ||
                                                                    row.Field<string>("OUTPUT").ToValueAsString().Trim().Length > 0)
                                                                    orderby row.Field<int>("SNO_SEQUENCE") ascending
                                                                    select new DDCOST_PROCESS_DATA()
                                                                    {
                                                                        CI_REFERENCE = ActiveEntity.CI_REFERENCE,
                                                                        SNO = (callerCode == 1 ? callerCode++ : Convert.ToDecimal(Convert.ToString(row.Field<string>("SNO")))),
                                                                        OPERATION_NO = Convert.ToDecimal(row.Field<string>("OPERATION_NO")),
                                                                        OPERATION = row.Field<string>("OPERATION"),
                                                                        COST_CENT_CODE = Convert.ToString(row.Field<string>("COST_CENT_CODE")),
                                                                        OUTPUT = Convert.ToDecimal(row.Field<string>("OUTPUT")),
                                                                        VAR_COST = Convert.ToDecimal(row.Field<string>("VAR_COST")),
                                                                        FIX_COST = Convert.ToDecimal(row.Field<string>("FIX_COST")),
                                                                        SPL_COST = Convert.ToDecimal(row.Field<string>("SPL_COST")),
                                                                        UNIT_OF_MEASURE = row.Field<string>("UNIT_OF_MEASURE"),
                                                                        TOTAL_COST = Convert.ToDecimal(row.Field<string>("TOTAL_COST")),
                                                                        IDPK = row.Field<string>("IDPK").ToIntValue(),
                                                                        CI_INFO_FK = parentEntity.IDPK,
                                                                        PROCESS_CODE = (callerCode == 1 ? callerCode++ : Convert.ToDecimal(Convert.ToString(row.Field<string>("SNO")))),
                                                                        //ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                                                    }).ToList<DDCOST_PROCESS_DATA>();
                Console.WriteLine("End: " + DateTime.Now.ToString("hh:mm:ss.fff"));
                return (from row in lstDDCOST_PROCESS_DATA
                        orderby row.PROCESS_CODE ascending
                        select row).ToList<DDCOST_PROCESS_DATA>();

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return null;
        }
        #region Close Button Action
        DataTable originalEntityDataTable = null;
        DataTable originalChildEntityDataTable = null;
        DataTable originalStandardEntityDataTable = null;

        DataTable activeEntityDataTable = null;
        DataTable activeChildEntityDataTable = null;
        DataTable activeStandardEntityDataTable = null;

        bool isclosed = false;
        public Action CloseAction { get; set; }
        private RelayCommand _onCancelCommand;
        private void CloseSubmitCommand()
        {
            try
            {
                if (!ActionPermission.Close) return;
                StandatedNotesIsFocusedSaveButton = true;

                //if (isChangesMade())
                //{
                //    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //    {
                //        SaveSubmitCommand("CloseSubmitCommand");
                //        return;
                //    }
                //}

                isclosed = false;
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                {
                    isclosed = true;
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
                if (!isclosed)
                {
                    isclosed = true;
                    WPF.MDI.ClosingEventArgs closingev;
                    closingev = (WPF.MDI.ClosingEventArgs)e;
                    if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                    {
                        isclosed = false;
                        closingev.Cancel = true;
                        e = closingev;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void CloseMethodWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (!isclosed)
                {
                    isclosed = true;
                    if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                    {
                        isclosed = false;
                        e.Cancel = true;
                    }
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

        private bool isChangesMade(int processingCode = 0)
        {
            bool bReturnValue = false;
            try
            {
                bool result = true;
                switch (ActionMode)
                {
                    case OperationMode.Print:
                    case OperationMode.AddNew:
                        copyMandatoryFieldsToEntity(MandatoryFields);
                        ActiveEntity.LOC_CODE = MandatoryFields.LOC_CODE;
                        activeEntityDataTable = (new List<DDCI_INFO>() { ActiveEntity }).ToDataTableWithType<DDCI_INFO>();
                        activeChildEntityDataTable = CostDetails.Table.Copy();
                        activeStandardEntityDataTable = StandardNotes.Table.Copy();
                        //if (processingCode == 0)
                        //{
                        //    copyMandatoryFieldsToEntity(MandatoryFields);
                        //    ActiveEntity.LOC_CODE = MandatoryFields.LOC_CODE;
                        //    activeEntityDataTable = (new List<DDCI_INFO>() { ActiveEntity }).ToDataTable<DDCI_INFO>();
                        //    activeChildEntityDataTable = CostDetails.Table.Copy();
                        //    activeStandardEntityDataTable = StandardNotes.Table.Copy();
                        //}
                        break;
                    case OperationMode.Edit:
                        activeEntityDataTable = (new List<DDCI_INFO>() { ActiveEntity }).ToDataTableWithType<DDCI_INFO>();
                        if (processingCode == 0)
                            originalEntityDataTable = bll.GetEntitiesByPrimaryKey(ActiveEntity).ToDataTableWithType<DDCI_INFO>();
                        if (activeEntityDataTable.IsNotNullOrEmpty() && activeEntityDataTable.Rows.Count == 1 &&
                            originalEntityDataTable.IsNotNullOrEmpty() && originalEntityDataTable.Rows.Count == 0)
                        {
                            activeEntityDataTable.Rows.Clear();
                        }
                        activeChildEntityDataTable = CostDetails.Table.Copy();
                        activeStandardEntityDataTable = StandardNotes.Table.Copy();
                        break;
                }

                DDCI_INFO ddci_info = null;
                if (originalEntityDataTable.IsNotNullOrEmpty() && originalEntityDataTable.Rows.Count > 0)
                {
                    DataRow row = originalEntityDataTable.Rows[0];
                    DateTime dt;

                    if (DateTime.TryParse(row["ENQU_RECD_ON"].ToString(), out dt))
                    {
                        row["ENQU_RECD_ON"] = Convert.ToDateTime(row["ENQU_RECD_ON"]);
                    }
                    row.EndEdit();
                    originalEntityDataTable.AcceptChanges();
                }

                if (activeChildEntityDataTable.IsNotNullOrEmpty() && activeChildEntityDataTable.Rows.Count == 1 &&
                        originalChildEntityDataTable.IsNotNullOrEmpty() && originalChildEntityDataTable.Rows.Count == 0)
                {
                    activeChildEntityDataTable.Rows.Clear();
                }

                //if (processingCode == 1)
                //{
                //    DDCI_INFO ddci_info = null;
                //    if (originalEntityDataTable.IsNotNullOrEmpty() && originalEntityDataTable.Rows.Count > 0)
                //    {
                //        DataRow row = originalEntityDataTable.Rows[0];
                //        string enq = Convert.ToString(row["ENQU_RECD_ON"]);
                //        DateTime dt;
                //        if (DateTime.TryParse(enq, out dt))
                //        {
                //            enq = Convert.ToDateTime(enq).ToString();
                //        }
                //        row["ENQU_RECD_ON"] = enq;
                //        row.EndEdit();
                //        originalEntityDataTable.AcceptChanges();
                //    }

                //    if (activeChildEntityDataTable.IsNotNullOrEmpty() && activeChildEntityDataTable.Rows.Count == 1 &&
                //            originalChildEntityDataTable.IsNotNullOrEmpty() && originalChildEntityDataTable.Rows.Count == 0)
                //    {
                //        activeChildEntityDataTable.Rows.Clear();
                //    }
                //}

                result = activeEntityDataTable.IsEqual(originalEntityDataTable);
                if (result)
                {
                    if (activeChildEntityDataTable.IsNotNullOrEmpty() && activeChildEntityDataTable.Rows.Count == 1 && activeChildEntityDataTable.Columns.Contains("SNO"))
                        activeChildEntityDataTable.Columns.Remove("SNO");
                    if (originalChildEntityDataTable.IsNotNullOrEmpty() && originalChildEntityDataTable.Rows.Count == 1 && originalChildEntityDataTable.Columns.Contains("SNO"))
                        originalChildEntityDataTable.Columns.Remove("SNO");
                    result = activeChildEntityDataTable.IsEqual(originalChildEntityDataTable);

                }

                if (result)
                {
                    result = activeStandardEntityDataTable.IsEqual(originalStandardEntityDataTable);
                }

                bReturnValue = !result;
            }
            catch (Exception ex)
            {
                ShowInformationMessage(ex.Message);
            }

            return bReturnValue;
        }

        public ICommand CloseClickCommand
        {
            get
            {


                if (_onCancelCommand == null)
                {
                    _onCancelCommand = new RelayCommand(param => this.CloseSubmitCommand(), null);
                }
                return _onCancelCommand;
            }
        }

        private readonly ICommand standardNoteCommand;
        public ICommand StandardNoteClickCommand { get { return this.standardNoteCommand; } }
        private void StandardNoteSubmitCommand()
        {
            CostDetailsVisibility = Visibility.Collapsed;
            RejectReasonsVisibility = Visibility.Collapsed;
        }

        #endregion

        private readonly ICommand _ciReferenceCopyCommand;
        public ICommand CIReferenceCopyClickCommand { get { return this._ciReferenceCopyCommand; } }
        private void ciReferenceCopyClicked()
        {

            Window win = new Window();

            frmCopyCIReference userControl = new frmCopyCIReference(_userInformation, win, ActiveEntity.DeepCopy<DDCI_INFO>(), OperationMode.AddNew);

            win.Title = "New CI Reference";
            win.Content = userControl;
            win.Height = userControl.Height + 50;
            win.Width = userControl.Width + 10;
            win.ShowInTaskbar = true;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.WindowState = WindowState.Normal;
            win.ResizeMode = ResizeMode.NoResize;
            win.ShowDialog();
            CIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER>().DefaultView;
        }

        //private DataView _ciReferenceNumbers;
        //public DataView CIReferenceNumbers
        //{
        //    get
        //    {
        //        return _ciReferenceNumbers;
        //    }
        //    set
        //    {
        //        _ciReferenceNumbers = value;
        //        NotifyPropertyChanged("CIReferenceNumbers");
        //    }
        //}

        private DataView _costDetails;
        public DataView CostDetails
        {
            get
            {
                return _costDetails;
            }
            set
            {
                _costDetails = value;

                if (_costDetails.IsNotNullOrEmpty())
                {
                    _costDetails.Table.RowDeleting += new DataRowChangeEventHandler(onRowDeletingCostDetails);
                    _costDetails.Table.RowDeleted += new DataRowChangeEventHandler(onRowDeletedCostDetails);
                    _costDetails.Table.RowChanged += new DataRowChangeEventHandler(onRowAddedCostDetails);
                }

                NotifyPropertyChanged("CostDetails");
            }
        }

        private void assignNewSequenceNumberCostDetails(object sender)
        {

            if (CostDetails.IsNotNullOrEmpty())
            {
                //CostDetails.Table.AcceptChanges();
                DataView dataView = CostDetails;
                string fieldName = "SNO";
                ReNumber(ref dataView, fieldName);

                string newFieldName = "SNO_SEQUENCE";
                int rowIndex = 0;
                //int multipleBy = 10;
                int nextNumber = 0;
                int lastRowIndex = 99999999;

                dataView.Sort = null;
                dataView.Sort = newFieldName + " ASC";
                foreach (DataRow row in dataView.Table.Rows)
                {
                    row.BeginEdit();
                    if (!(row["SNO"].ToValueAsString().Length == 0 && row["OPERATION_NO"].ToValueAsString().Length == 0 && row["OPERATION"].ToValueAsString().Length == 0 && row["COST_CENT_CODE"].ToValueAsString().Length == 0 && row["OUTPUT"].ToValueAsString().Length == 0))
                    {
                        row[newFieldName] = ++rowIndex;
                    }
                    else
                    {
                        if (lastRowIndex == nextNumber) { --lastRowIndex; }
                        row[newFieldName] = lastRowIndex--;

                    }

                    //if (nextNumber < dataView.Count && row[fieldName].IsNotNullOrEmpty() &&
                    //row[fieldName].ToValueAsString().IsNotNullOrEmpty() &&
                    //row[fieldName].ToValueAsString().ToIntValue() != CONST_RAW_MATERIAL_PROCESS_SNO &&
                    //row[fieldName].ToValueAsString().ToIntValue() != CONST_DESPATCH_PROCESS_SNO &&
                    //row[fieldName].ToValueAsString().ToIntValue() != CONST_FINISH_PROCESS_SNO &&
                    //!(row[fieldName].ToValueAsString().ToIntValue() >= CONST_MAX_REORDER_SNO) && snoUpdateRequired)
                    //{
                    //    row[fieldName] = (((nextNumber++) % 10) + 1) * multipleBy;
                    //}

                    row.EndEdit();
                    //row.Table.AcceptChanges();
                }

                //CostDetails.Table.AcceptChanges();

                // ReNumber(ref dataView, newFieldName);
                dataView.Sort = null;
                dataView.Sort = newFieldName + " ASC";
            }
        }
        private void onRowAddedCostDetails(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Add)
            {
                assignNewSequenceNumberCostDetails(sender);
            }
        }

        private void onRowDeletedCostDetails(object sender, DataRowChangeEventArgs e)
        {
            if (CostDetails.IsNotNullOrEmpty() && CostDetails.Count == 0 || canAddNewCostDetails())
            {
                CostDetailsInsertRow(null);

                //    //string fieldName = "SNO_SEQUENCE";
                //    //string filter = fieldName + " <> " + CONST_RAW_MATERIAL_PROCESS_SNO + " AND " + fieldName + " <> " + CONST_DESPATCH_PROCESS_SNO + " AND " + fieldName + " <> " + CONST_FINISH_PROCESS_SNO;

                //    //int maxRowNumber = CostDetails.ToTable().Copy().Compute("Max(" + fieldName + ")", filter).ToValueAsString().ToIntValue();

                //    //DataRowView rowView = getEmptyRowCostDetails();
                //    //if (rowView.IsNotNullOrEmpty())
                //    //{
                //    //    rowView[fieldName] = maxRowNumber + 1;
                //    //    rowView.EndEdit();
                //    //}

            }
            //CostDetails.Table.AcceptChanges();
            //CostCalculation();
        }

        private void onRowDeletingCostDetails(object sender, DataRowChangeEventArgs e)
        {


        }

        private DataRowView _costDetailsSelectedRow;
        public DataRowView CostDetailsSelectedRow
        {
            get { return _costDetailsSelectedRow; }
            set
            {

                _costDetailsSelectedRow = value;

                NotifyPropertyChanged("CostDetailsSelectedRow");
            }
        }

        private Visibility _zoneVisibility = Visibility.Visible;
        public Visibility ZoneVisibility
        {
            get
            {
                return _zoneVisibility;
            }
            set
            {
                _zoneVisibility = value;
                NotifyPropertyChanged("ZoneVisibility");
            }
        }

        private Visibility _costDetailsVisibility = Visibility.Visible;
        public Visibility CostDetailsVisibility
        {
            get
            {
                return _costDetailsVisibility;
            }
            set
            {
                _costDetailsVisibility = value;
                if (_costDetailsVisibility == Visibility.Visible)
                {
                    StandatedNotesVisibility = Visibility.Collapsed;
                }
                else if (_costDetailsVisibility != Visibility.Visible)
                {
                    StandatedNotesVisibility = Visibility.Visible;
                }
                NotifyPropertyChanged("CostDetailsVisibility");
            }
        }

        private bool isValidCostDetails(string warrnigMessgeOnOff = "Y")
        {


            bool bReturnValue = false;
            try
            {
                DataView dv = CostDetails.ToTable().Copy().DefaultView;
                dv.RowFilter = "CONVERT(Isnull(SNO,''), System.String) = ''";
                if (dv.Count > 1)
                {
                    dv.RowFilter = null;
                    ShowInformationMessage(PDMsg.NotEmpty("SNO"));
                    return bReturnValue;
                }
                dv.RowFilter = null;

                string cellText = "";
                DataView dataView = null;

                //CostDetails.Table.AcceptChanges();
                DataView dt = CostDetails; //.Table.Copy().DefaultView;
                string fieldName = "SNO";
                ReNumber(ref dt, fieldName);

                //dt.Table.AcceptChanges();

                string newFieldName = "SNO_SEQUENCE";
                dt.Sort = null;
                dt.Sort = newFieldName + " ASC";

                DataTable dataTable = CostDetails.Table.Clone();
                DataTable source = CostDetails.Table;

                int rowsCount = source.Rows.Count;
                for (int rowIndex = 0; rowIndex <= rowsCount - 1; rowIndex++)
                {

                    if (CostDetails[rowIndex]["SNO"].ToValueAsString().Length == 0 && CostDetails[rowIndex]["OPERATION_NO"].ToValueAsString().Length == 0 && CostDetails[rowIndex]["OPERATION"].ToValueAsString().Length == 0 && CostDetails[rowIndex]["COST_CENT_CODE"].ToValueAsString().Length == 0 && CostDetails[rowIndex]["OUTPUT"].ToValueAsString().Length == 0)
                    {
                        continue;
                    }

                    DataRow dataRow = dataTable.NewRow();
                    //int columnNullValueCount = 0;
                    foreach (DataColumn dataColumn in dt.Table.Columns)
                    {
                        dataRow[dataColumn.ColumnName] = CostDetails.Table.Rows[rowIndex][dataColumn.ColumnName];

                        if (!dataRow[dataColumn.ColumnName].IsNotNullOrEmpty())
                            dataRow[dataColumn.ColumnName] = dt[rowIndex][dataColumn.ColumnName];

                        cellText = Convert.ToString(dataRow[dataColumn.ColumnName]);
                        switch (dataColumn.ColumnName)
                        {
                            case "COST_CENT_CODE":
                            case "CCCode":
                                if (dt[rowIndex]["CCCode"].ToValueAsString().IsNotNullOrEmpty() && !dt[rowIndex]["COST_CENT_CODE"].ToValueAsString().IsNotNullOrEmpty()) dataRow["COST_CENT_CODE"] = dt[rowIndex]["CCCode"].ToValueAsString();
                                break;
                            case "OUTPUT":
                                dataRow[dataColumn.ColumnName] = cellText.ToValueAsString().ToDecimalValue().ToString();
                                break;
                            case "VAR_COST":
                            case "FIX_COST":
                            case "SPL_COST":
                            case "TOTAL_COST":
                                dataRow[dataColumn.ColumnName] = cellText.ToValueAsString().ToDoubleValue().ToString("0.00");
                                break;
                        }
                        dataRow.EndEdit();
                        cellText = Convert.ToString(dataRow[dataColumn.ColumnName]);

                        //if (cellText.Length == 0 && (dataColumn.ColumnName == "OPERATION_NO" || dataColumn.ColumnName == "CCCode" || dataColumn.ColumnName == "OUTPUT"))
                        //{
                        //    columnNullValueCount++; break;
                        //}
                        //else
                        //{
                        //    dataView = CostDetails.Table.Copy().DefaultView;
                        //    dataView.RowFilter = dataColumn.ColumnName + " = '" + cellText + "'";
                        //    if (dataView.Count == 0)
                        //    {
                        //        columnNullValueCount++; break;
                        //    }
                        //}

                        switch (dataColumn.ColumnName)
                        {
                            case "SNO":
                                if (cellText.Length == 0)
                                {
                                    ShowInformationMessage(PDMsg.NotEmpty("SNO"));
                                    return bReturnValue;
                                }
                                break;
                            case "OPERATION_NO":
                                if (cellText.Length == 0)
                                {
                                    ShowInformationMessage(PDMsg.NotEmpty("Operation Code"));
                                    return bReturnValue;
                                }
                                else if (warrnigMessgeOnOff.ToBooleanAsString())
                                {
                                    dataView = OperationDataSource;
                                    dataView.RowFilter = dataColumn.ColumnName + " = '" + cellText + "'";
                                    if (dataView.Count == 0)
                                    {
                                        ShowInformationMessage(PDMsg.DoesNotExists("Operation Code '" + cellText + "'"));
                                        return bReturnValue;
                                    }
                                }
                                break;
                            case "COST_CENT_CODE":
                                if (cellText.Length == 0)
                                {
                                    ShowInformationMessage(PDMsg.NotEmpty("CC Code"));
                                    return bReturnValue;
                                }
                                else if (warrnigMessgeOnOff.ToBooleanAsString())
                                {
                                    dataView = OperationCostDataSource;
                                    dataView.RowFilter = dataColumn.ColumnName + " = '" + cellText + "'";
                                    DVOperationCost = OperationCostDataSource.ToTable().Copy().DefaultView;

                                    if (dataView.Count == 0 && !(cellText.Contains("EXP PKG") || cellText.Contains("EXP PKG - K") || cellText.Contains("OE PKG") || cellText.Contains("OE PKG - K")))
                                    {
                                        ShowInformationMessage(PDMsg.DoesNotExists("CC Code '" + cellText + "'"));
                                        return bReturnValue;
                                    }
                                }
                                break;
                            case "OUTPUT":
                                if (cellText.Length == 0)
                                {
                                    ShowInformationMessage(PDMsg.NotEmpty("Output"));
                                    return bReturnValue;
                                }
                                else if (cellText.ToDecimalValue() > 9999999999.99m)
                                {
                                    ShowInformationMessage(PDMsg.NotExceeds("Output", "9999999999.99"));
                                    return bReturnValue;
                                }
                                break;
                            case "VAR_COST":
                                if (cellText.ToDecimalValue() > 9999999999.99m)
                                {
                                    ShowInformationMessage(PDMsg.NotExceeds("Var Cost", "9999999999.99"));
                                    return bReturnValue;
                                }
                                break;
                            case "FIX_COST":
                                if (cellText.ToDecimalValue() > 9999999999.99m)
                                {
                                    ShowInformationMessage(PDMsg.NotExceeds("Fix Cost", "9999999999.99"));
                                    return bReturnValue;
                                }
                                break;
                            case "SPL_COST":
                                if (cellText.ToDecimalValue() > 9999999999.99m)
                                {
                                    ShowInformationMessage(PDMsg.NotExceeds("SPL Cost", "9999999999.99"));
                                    return bReturnValue;
                                }
                                break;
                            case "TOTAL_COST":
                                if (cellText.ToDecimalValue() > 9999999999999.99m)
                                {
                                    ShowInformationMessage(PDMsg.NotExceeds("Operation Cost", "9999999999999.99"));
                                    return bReturnValue;
                                }
                                break;
                        }
                    }

                    //if (columnNullValueCount > 0)
                    //{
                    //    foreach (DataColumn dataColumn in CostDetails.Table.Columns)
                    //    {
                    //        cellText = Convert.ToString(dataRow[dataColumn.ColumnName]);
                    //        dataView = null;
                    //        switch (dataColumn.ColumnName)
                    //        {
                    //            case "OPERATION_NO":
                    //                if (cellText.Length == 0)
                    //                {
                    //                    ShowInformationMessage(PDMsg.NotEmpty("Operation Code"));
                    //                    return bReturnValue;
                    //                }
                    //                else
                    //                {
                    //                    dataView = CostDetails.Table.Copy().DefaultView;
                    //                    dataView.RowFilter = dataColumn.ColumnName + " = '" + cellText + "'";
                    //                    if (dataView.Count == 0)
                    //                    {
                    //                        ShowInformationMessage(PDMsg.DoesNotExists("Operation Code '" + cellText + "'"));
                    //                        return bReturnValue;
                    //                    }
                    //                }
                    //                break;
                    //            case "COST_CENT_CODE":
                    //                if (cellText.Length == 0)
                    //                {
                    //                    ShowInformationMessage(PDMsg.NotEmpty("CC Code"));
                    //                    return bReturnValue;
                    //                }
                    //                else
                    //                {
                    //                    dataView = CostDetails.Table.Copy().DefaultView;
                    //                    dataView.RowFilter = dataColumn.ColumnName + " = '" + cellText + "'";
                    //                    if (dataView.Count == 0)
                    //                    {
                    //                        ShowInformationMessage(PDMsg.DoesNotExists("CC Code '" + cellText + "'"));
                    //                        return bReturnValue;
                    //                    }
                    //                }
                    //                break;
                    //            case "OUTPUT":
                    //                ShowInformationMessage(PDMsg.NotEmpty("Output"));
                    //                return bReturnValue;
                    //        }
                    //    }
                    //}
                    //else
                    dataTable.Rows.Add(dataRow);
                    //dataTable.AcceptChanges();

                }

                bReturnValue = false;

                bReturnValue = isColumnHasValidValue("SNO", 4, "SNO", CostDetails, "Y", ">");
                if (!bReturnValue) return bReturnValue;

                bReturnValue = isColumnHasValidValue("OPERATION_NO", 10, "Operation Code", CostDetails, "Y", ">");
                if (!bReturnValue) return bReturnValue;

                bReturnValue = isColumnHasValidValue("OPERATION", 50, "Operation", CostDetails, "Y", ">");
                if (!bReturnValue) return bReturnValue;

                bReturnValue = isColumnHasValidValue("COST_CENT_CODE", 15, "CC Code", CostDetails, "Y", ">");
                if (!bReturnValue) return bReturnValue;

                bReturnValue = isColumnHasValidValue("OUTPUT", 13, "Output", CostDetails, "Y", ">");
                if (!bReturnValue) return bReturnValue;

                bReturnValue = isColumnHasValidValue("VAR_COST", 13, "Var Cost", CostDetails, "Y", ">");
                if (!bReturnValue) return bReturnValue;

                bReturnValue = isColumnHasValidValue("FIX_COST", 13, "Fixed Cost", CostDetails, "Y", ">");
                if (!bReturnValue) return bReturnValue;

                bReturnValue = isColumnHasValidValue("SPL_COST", 13, "SPL Cost", CostDetails, "Y", ">");
                if (!bReturnValue) return bReturnValue;

                bReturnValue = isColumnHasValidValue("TOTAL_COST", 16, "Total Cost", CostDetails, "Y", ">");
                if (!bReturnValue) return bReturnValue;

                bReturnValue = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return bReturnValue;
        }

        private bool isColumnHasValidValue(string fieldName, int fieldLength, string fieldShortMessage, DataView dataView, string warrnigMessgeOnOff = "Y", string operatorName = ">")
        {
            bool bReturnValue = false;
            try
            {
                if (!dataView.IsNotNullOrEmpty()) return !bReturnValue;
                if (!fieldName.IsNotNullOrEmpty()) throw new ArgumentException(PDMsg.NotEmpty("Field Name"));
                if (!fieldShortMessage.IsNotNullOrEmpty()) throw new ArgumentException(PDMsg.NotEmpty("field Short Message"));
                if (!fieldShortMessage.IsNotNullOrEmpty()) throw new ArgumentException(PDMsg.NotEmpty("Operator"));

                dataView = CostDetails.Table.Copy().DefaultView;
                dataView.RowFilter = "LEN(CONVERT(Isnull(" + fieldName + ",''), System.String)) " + operatorName + " " + fieldLength;
                if (dataView.Count > 0)
                {
                    if (warrnigMessgeOnOff.ToBooleanAsString() && fieldLength > 0) ShowInformationMessage(fieldShortMessage + " '" + dataView[0][fieldName].ToValueAsString().Trim() + "' should be less than or equal to " + fieldLength + " chars'");
                    return bReturnValue;
                }
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return bReturnValue;
        }

        private Visibility _standatedNotesVisibility = Visibility.Collapsed;
        public Visibility StandatedNotesVisibility
        {
            get
            {
                return _standatedNotesVisibility;
            }
            set
            {
                _standatedNotesVisibility = value;
                NotifyPropertyChanged("StandatedNotesVisibility");
            }
        }

        private DataRowView _standardNotesSelectedRow;
        public DataRowView StandardNotesSelectedRow
        {
            get { return _standardNotesSelectedRow; }
            set
            {
                _standardNotesSelectedRow = value;
                NotifyPropertyChanged("StandardNotesSelectedRow");
            }
        }

        private DataView _standardNotes;
        public DataView StandardNotes
        {
            get
            {
                return _standardNotes;
            }
            set
            {
                _standardNotes = value;
                if (_standardNotes.IsNotNullOrEmpty())
                {
                    _standardNotes.Table.RowDeleting += new DataRowChangeEventHandler(onRowDeletingStandardNotes);
                }
                //_standardNotes.ListChanged += new System.ComponentModel.ListChangedEventHandler(OnListChangedStandardNotes);
                NotifyPropertyChanged("StandardNotes");
                StandardNotesDeletedList = _standardNotes.Table.Clone();
                StandardNotesDeletedList.Rows.Clear();

                originalStandardEntityDataTable = StandardNotes.Table.Copy();

            }
        }

        private DataTable _standardNotesDeletedList;
        public DataTable StandardNotesDeletedList
        {
            get
            {
                return _standardNotesDeletedList;
            }
            set
            {
                _standardNotesDeletedList = value;
                NotifyPropertyChanged("StandardNotesDeletedList");
            }
        }

        private void onRowDeletingStandardNotes(object sender, DataRowChangeEventArgs e)
        {
            StandardNotesDeletedList.ImportRow(e.Row);
            StandardNotesDeletedList.AcceptChanges();
        }

        //protected void OnListChangedStandardNotes(object sender, System.ComponentModel.ListChangedEventArgs e)
        //{
        //    Console.WriteLine("ListChanged:");
        //    Console.WriteLine("\t    Type = " + e.ListChangedType);
        //    Console.WriteLine("\tOldIndex = " + e.OldIndex);
        //    Console.WriteLine("\tNewIndex = " + e.NewIndex);
        //    if (e.ListChangedType == System.ComponentModel.ListChangedType.ItemDeleted)
        //    {
        //        var x = e.PropertyDescriptor;
        //        //DataRowView b = (DataRowView)e.PropertyDescriptor;

        //        int rowIndex = Math.Max(e.OldIndex, e.NewIndex);
        //        if (rowIndex < 0) return;

        //        DataTable tmpStandardNotesDataTable = ((DataView)sender).Table;
        //        DataRow row = tmpStandardNotesDataTable.Rows[rowIndex];

        //        if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached)
        //        {
        //            var desRow = StandardNotesDeletedList.NewRow();
        //            var sourceRow = tmpStandardNotesDataTable.Rows[rowIndex];
        //            desRow.ItemArray = sourceRow.ItemArray.Clone() as object[];
        //            StandardNotesDeletedList.Rows.Add(desRow);
        //            StandardNotesDeletedList.AcceptChanges();

        //            //StandardNotesDeletedList.ImportRow(row);
        //            //StandardNotesDeletedList.AcceptChanges();
        //        }

        //        //foreach (DataRow row in tmpStandardNotesDataTable.Rows)
        //        //{
        //        //    if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached)
        //        //    {
        //        //        StandardNotesDeletedList.ImportRow(row);
        //        //    }
        //        //}
        //    }
        //}

        private Visibility _costNotesVisibility = Visibility.Collapsed;
        public Visibility CostNotesVisibility
        {
            get
            {
                return _costNotesVisibility;
            }
            set
            {
                _costNotesVisibility = value;
                NotifyPropertyChanged("CostNotesVisibility");
            }
        }

        private readonly ICommand costNotesMouseDoubleClickCommand = null;
        public ICommand CostNotesMouseDoubleClickCommand { get { return this.costNotesMouseDoubleClickCommand; } }
        private void costNotesMouseDoubleClick()
        {
            CostNotesVisibility = Visibility.Visible;

            CostDetailsVisibility = Visibility.Collapsed;
            RejectReasonsVisibility = Visibility.Collapsed;
            StandatedNotesVisibility = Visibility.Collapsed;

        }

        private readonly ICommand costNotesPreviewMouseDoubleClickCommand = null;
        public ICommand CostNotesPreviewMouseDoubleClickCommand { get { return this.costNotesPreviewMouseDoubleClickCommand; } }
        private void costNotesPreviewMouseDoubleClick()
        {
            CostNotesVisibility = Visibility.Collapsed;

            CostDetailsVisibility = Visibility.Visible;
            RejectReasonsVisibility = Visibility.Collapsed;
            StandatedNotesVisibility = Visibility.Collapsed;
        }

        private readonly ICommand feasibleIsCheckedCommand;
        public ICommand FeasibleIsCheckedCommand { get { return this.feasibleIsCheckedCommand; } }
        private void FeasibleIsChecked()
        {
            MandatoryFields.IS_FEASIBILITY_CAN_CHANGE = true;
            //if (messageBoxResult == MessageBoxResult.No)
            //{
            //    MandatoryFields.FEASIBILITY = "1";
            //    CostDetailsVisibility = Visibility.Visible;
            //    RejectReasonsVisibility = Visibility.Collapsed;
            //}

            ////MessageBoxResult messageBoxResult = MessageBoxResult.No;
            ////if (!ActiveEntity.FEASIBILITY.ToBooleanAsString() &&
            ////    CostDetails.IsNotNullOrEmpty() && CostDetails.HasNonEmptyCells() && isCIReferenceSelectionCompleted)
            ////{
            ////    messageBoxResult = ShowWarningMessage(PDMsg.BeforeDelete("Cost Sheet Details"), MessageBoxButton.YesNo);
            ////    //messageBoxResult = MessageBox.Show("Are you sure want to delete the Cost Sheet Details?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
            ////    if (messageBoxResult == MessageBoxResult.Yes)
            ////    {
            ////        CostDetailEntities = bll.GetCostDetails(new DDCI_INFO() { IDPK = -99999 });
            ////        CostDetailsVisibility = Visibility.Collapsed;
            ////        RejectReasonsVisibility = Visibility.Visible;
            ////        //NotifyPropertyChanged("MandatoryFields");
            ////    }
            ////    else
            ////        MandatoryFields.IS_FEASIBILITY_CAN_CHANGE = false;
            ////}

            ////if (messageBoxResult == MessageBoxResult.No)
            ////{
            ////    //ActiveEntity.FEASIBILITY = "0";
            ////    //NotifyPropertyChanged("ActiveEntity");

            ////    MandatoryFields.FEASIBILITY = "1";
            ////    CostDetailsVisibility = Visibility.Visible;
            ////    RejectReasonsVisibility = Visibility.Collapsed;
            ////    //NotifyPropertyChanged("ActiveEntity");
            ////}
        }

        private readonly ICommand feasibleIsUnCheckedCommand;
        public ICommand FeasibleIsUnCheckedCommand { get { return this.feasibleIsUnCheckedCommand; } }
        private void FeasibleIsUnChecked()
        {
            if (!ActiveEntity.IsNotNullOrEmpty() || !ActiveEntity.FEASIBILITY.IsNotNullOrEmpty()) return;

            MessageBoxResult messageBoxResult = MessageBoxResult.No;
            if (MandatoryFields.IS_FEASIBILITY_CAN_CHANGE == false)
            {
                messageBoxResult = ShowWarningMessage(PDMsg.BeforeDelete("Cost Sheet Details"), MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    CostDetailEntities = bll.GetCostDetails(new DDCI_INFO() { IDPK = -99999 });
                    CostDetailsVisibility = Visibility.Collapsed;
                    RejectReasonsVisibility = Visibility.Visible;
                }
                else
                    MandatoryFields.IS_FEASIBILITY_CAN_CHANGE = true;
            }

        }

        public void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkBox = sender as System.Windows.Controls.CheckBox;
            if (!checkBox.IsNotNullOrEmpty()) return;

            IS_FEASIBILITY_CAN_CHANGE = checkBox.IsChecked.ToBooleanAsString();

            MessageBoxResult messageBoxResult = MessageBoxResult.No;
            if (checkBox.IsChecked == false)
            {
                messageBoxResult = ShowWarningMessage(PDMsg.BeforeDelete("Cost Sheet Details"), MessageBoxButton.YesNo);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    CostDetailEntities = bll.GetCostDetails(new DDCI_INFO() { IDPK = -99999 });
                    CostDetailsVisibility = Visibility.Collapsed;
                    RejectReasonsVisibility = Visibility.Visible;
                }
                else
                {
                    checkBox.IsChecked = true;
                    CostDetailEntities = bll.GetCostDetails(new DDCI_INFO() { IDPK = ActiveEntity.IDPK });
                }
            }
            else
            {
                CostDetailsVisibility = Visibility.Visible;
                RejectReasonsVisibility = Visibility.Collapsed;
                CostDetailEntities = bll.GetCostDetails(new DDCI_INFO() { IDPK = ActiveEntity.IDPK });
            }
        }

        public bool _Is_feasibility_can_change = true;
        public bool IS_FEASIBILITY_CAN_CHANGE
        {
            get { return _Is_feasibility_can_change; }
            set
            {
                _Is_feasibility_can_change = value;
                ActiveEntity.FEASIBILITY = "0";
                if (_Is_feasibility_can_change) ActiveEntity.FEASIBILITY = "1";
                NotifyPropertyChanged("IS_FEASIBILITY_CAN_CHANGE");
            }
        }

        private readonly ICommand pendingIsCheckedCommand;
        public ICommand PendingIsCheckedCommand { get { return this.pendingIsCheckedCommand; } }
        private void PendingIsChecked()
        {
            if (!ActiveEntity.PENDING.ToBooleanAsString())
            {
                ActiveEntity.FR_CS_DATE = bll.ServerDateTime();
                if (ActiveEntity.SUGGESTED_RM.IsNotNullOrEmpty())
                {
                    fnDespatchProcess();
                    CostCalculation();
                }
            }
        }

        const decimal CONST_REALISATION_FACTOR = 1000.0m;

        const int CONST_DESPATCH_PROCESS_SNO = 999;
        const int CONST_DESPATCH_PROCESS_CODE = 2600;
        const string CONST_DESPATCH_PROCESS_DESCRIPTION = "PACK AND DESPATCH";

        const int CONST_RAW_MATERIAL_PROCESS_CODE = 1000;
        const int CONST_RAW_MATERIAL_PROCESS_SNO = 1;

        const string CONST_FINISH_PROCESS_CODE = "2010','2020','2030','2031','2032','2036','2037','2065','2090";
        const int CONST_FINISH_PROCESS_SNO = 888;

        const int CONST_MAX_REORDER_SNO = 10000;

        private readonly ICommand exportIsCheckedCommand;
        public ICommand ExportIsCheckedCommand { get { return this.exportIsCheckedCommand; } }
        private void ExportIsChecked()
        {
            fnDespatchProcess();
            CostCalculation();
        }

        public void RawMaterialCostCalculation()
        {
            if (!isValidRawMaterial()) return;

            copyMandatoryFieldsToEntity(MandatoryFields);
            //////RawMaterialCostCalculation
            if (!ActiveEntity.IsNotNullOrEmpty() || !RawMaterialEntities.IsNotNullOrEmpty() || RawMaterialEntities.Count == 0 ||
                !ActiveEntity.SUGGESTED_RM.IsNotNullOrEmpty()) return;

            List<DDRM_MAST> lstDDRM_MAST = (from row in RawMaterialEntities
                                            where row.RM_CODE == ActiveEntity.SUGGESTED_RM
                                            select row).ToList<DDRM_MAST>();
            if (!lstDDRM_MAST.IsNotNullOrEmpty() || lstDDRM_MAST.Count == 0) return;

            DDRM_MAST activeRMEntity = lstDDRM_MAST[0];
            if (!activeRMEntity.IsNotNullOrEmpty()) return;

            activeRMEntity.LOC_COST = activeRMEntity.LOC_COST.ToValueAsString().ToDecimalValue();
            activeRMEntity.EXP_COST = activeRMEntity.EXP_COST.ToValueAsString().ToDecimalValue();

            ActiveEntity.CHEESE_WT = ActiveEntity.CHEESE_WT.ToValueAsString().ToDecimalValue();

            if (!ActiveEntity.EXPORT.ToBooleanAsString())
            {
                ActiveEntity.RM_COST = activeRMEntity.LOC_COST * ActiveEntity.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
            }
            else
            {
                ActiveEntity.RM_COST = activeRMEntity.EXP_COST * ActiveEntity.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
            }
            ActiveEntity.RM_COST = Math.Round(Convert.ToDecimal(ActiveEntity.RM_COST), 2);
        }

        private void CostCalculation()
        {
            RawMaterialCostCalculation();
            //////CostCalculation
            if (!ActiveEntity.IsNotNullOrEmpty()) return;

            decimal? sumOfCost = 0;

            List<DDCOST_PROCESS_DATA> lstAssociationEntity = getAssociationEntity(ActiveEntity, 1);

            if (lstAssociationEntity.IsNotNullOrEmpty() && lstAssociationEntity.Count > 0)
            {
                sumOfCost = lstAssociationEntity.Where(row => row.CI_REFERENCE == ActiveEntity.CI_REFERENCE).Select(row => row.TOTAL_COST).Sum();
            }
            ActiveEntity.FINAL_COST = Convert.ToDecimal(sumOfCost);

            if (ActiveEntity.RM_COST > 0.0m)
            {
                ActiveEntity.FINAL_COST = ActiveEntity.FINAL_COST + ActiveEntity.RM_COST;
            }

            ActiveEntity.FINAL_COST = Math.Round(Convert.ToDecimal(ActiveEntity.FINAL_COST), 2);

            ActiveEntity.FINISH_WT = Convert.ToDecimal(ActiveEntity.FINISH_WT);
            if (ActiveEntity.FINISH_WT != 0.0m)
            {
                ActiveEntity.REALISATION = (ActiveEntity.FINAL_COST * CONST_REALISATION_FACTOR) / ActiveEntity.FINISH_WT;
            }

            ActiveEntity.REALISATION = Math.Round(Convert.ToDecimal(ActiveEntity.REALISATION), 2);
        }

        private void WeightCalculation()
        {
            switch (ActionMode)
            {
                case OperationMode.AddNew:
                case OperationMode.Edit:
                    if (!CostDetails.IsNotNullOrEmpty() || CostDetails.Count == 0) return;
                    CostDetails.RowFilter = null;

                    foreach (DataRowView dataRowView in CostDetails)
                    {
                        WeightCalculation(dataRowView);
                    }
                    break;
            }
        }

        private void WeightCalculation(DataRowView dataRowView)
        {

            if (!dataRowView.IsNotNullOrEmpty() || CostDetails.Count == 0) return;

            ///////////////Grid Column Index, Name & Mapping Field ///////////////////

            ////0 - SNO         - SNO
            ////1 - Opn Code    - OPERATION_NO
            ////2 - Operation   - OPERATION
            ////3 - CC Code     - COST_CENT_CODE
            ////4 - Output      - OUTPUT
            ////5 - Var Cost    - VAR_COST
            ////6 - Fixed Cost  - FIX_COST
            ////7 - SPL Cost    - SPL_COST
            ////8 - UOM         - UNIT_CODE
            ////9 - Opn Cost    - TOTAL_COST

            CommonProcessCost ccOperation = (from row in bll.GetOperationCostDetails()
                                             where row.COST_CENT_CODE == Convert.ToString(dataRowView["COST_CENT_CODE"]) &&
                                             row.OPERATION_NO == Convert.ToDecimal(dataRowView["OPERATION_NO"])
                                             select new CommonProcessCost()
                                             {
                                                 SNO = (Convert.ToString(dataRowView.Row["SNO"]).Trim().Length > 0 ?
                                                 Convert.ToDecimal(dataRowView.Row["SNO"]) : Convert.ToDecimal(dataRowView.Row["SNO_SEQUENCE"])),

                                                 CODE = Convert.ToString(row.OPERATION_NO),
                                                 DESCRIPTION = Convert.ToString(dataRowView.Row["OPERATION"]),
                                                 UNIT_CODE = Convert.ToString(row.UNIT_CODE),
                                                 //OUTPUT = Convert.ToDecimal(row.OUTPUT),
                                                 OUTPUT = (dataRowView.Row["OUTPUT"].IsNotNullOrEmpty() ?
                                                    Convert.ToDecimal(dataRowView.Row["OUTPUT"]) : 0),
                                                 FIX_COST = Convert.ToDecimal(row.FIX_COST),
                                                 VAR_COST = Convert.ToDecimal(row.VAR_COST),
                                                 SPL_COST = 0.0m,
                                                 CHEESE_WT = Convert.ToDecimal(ActiveEntity.CHEESE_WT),
                                                 FINISH_WT = Convert.ToDecimal(ActiveEntity.FINISH_WT)
                                             }).FirstOrDefault<CommonProcessCost>();

            if (!ccOperation.IsNotNullOrEmpty()) return;
            switch (ccOperation.UNIT_CODE)
            {
                case "1": //Hourly
                    if (ccOperation.OUTPUT == 0)
                    {
                        dataRowView.Row["VAR_COST"] = 0;
                        dataRowView.Row["FIX_COST"] = 0;
                    }
                    else if (ccOperation.OUTPUT > 0)
                    {
                        dataRowView.Row["VAR_COST"] = ccOperation.VAR_COST / ccOperation.OUTPUT * 100;
                        dataRowView.Row["FIX_COST"] = ccOperation.FIX_COST / ccOperation.OUTPUT * 100;
                    }
                    break;
                case "2": //Per Piece
                    dataRowView.Row["VAR_COST"] = ccOperation.VAR_COST * 100;
                    dataRowView.Row["FIX_COST"] = ccOperation.FIX_COST * 100;
                    break;
                case "3": //Kilograms

                    dataRowView.Row["VAR_COST"] = 0.0m;
                    dataRowView.Row["FIX_COST"] = 0.0m;

                    dataRowView.Row["VAR_COST"] = ccOperation.VAR_COST * ccOperation.FINISH_WT;
                    dataRowView.Row["FIX_COST"] = ccOperation.FIX_COST * ccOperation.FINISH_WT;

                    //if (ccOperation.FINISH_WT != 0)
                    //{
                    //    dataRowView.Row["VAR_COST"] = ccOperation.VAR_COST * ccOperation.FINISH_WT;
                    //    dataRowView.Row["FIX_COST"] = ccOperation.FIX_COST * ccOperation.FINISH_WT;
                    //}
                    break;
                case "4": //RM
                    dataRowView.Row["VAR_COST"] = 0.0m;
                    dataRowView.Row["FIX_COST"] = 0.0m;

                    dataRowView.Row["VAR_COST"] = ccOperation.VAR_COST * ccOperation.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    dataRowView.Row["FIX_COST"] = ccOperation.FIX_COST * ccOperation.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();

                    //if (ccOperation.CHEESE_WT != 0)
                    //{
                    //    dataRowView.Row["VAR_COST"] = ccOperation.VAR_COST * ccOperation.CHEESE_WT * CONST_VARIABLE_COST;
                    //    dataRowView.Row["FIX_COST"] = ccOperation.FIX_COST * ccOperation.CHEESE_WT * CONST_FIXED_COST;
                    //}
                    break;
            }

            dataRowView.Row["VAR_COST"] = Math.Round(dataRowView.Row["VAR_COST"].ToValueAsString().ToDecimalValue(), 2);
            dataRowView.Row["FIX_COST"] = Math.Round(dataRowView.Row["FIX_COST"].ToValueAsString().ToDecimalValue(), 2);
            dataRowView.Row["TOTAL_COST"] = Math.Round(dataRowView.Row["VAR_COST"].ToValueAsString().ToDecimalValue() +
                dataRowView.Row["FIX_COST"].ToValueAsString().ToDecimalValue() + dataRowView.Row["SPL_COST"].ToValueAsString().ToDecimalValue(), 2);
            dataRowView.EndEdit();
        }

        private class CommonProcessCost
        {
            public string COST_CENT_CODE = "";
            public decimal SNO = 0;
            public string CODE = "";
            public string DESCRIPTION = "";
            public string UNIT_CODE = "";
            public decimal OUTPUT = 0;
            public decimal FIX_COST = 0.0m;
            public decimal VAR_COST = 0.0m;
            public decimal SPL_COST = 0.0m;
            public decimal CHEESE_WT = 0.0m;
            public decimal FINISH_WT = 0.0m;
            public decimal PROCESS_CODE = 0.0m;
        }

        private void fnDespatchProcess()
        {
            if (!ActiveEntity.IsNotNullOrEmpty() || !isCIReferenceSelectionCompleted) return;
            //if (!ActiveEntity.EXPORT.ToBooleanAsString()) return;

            DataView dv = CostDetails.ToTable().Copy().DefaultView;
            dv.RowFilter = "CONVERT(Isnull(SNO,''), System.String) = ''";
            if (dv.Count > 1)
            {
                dv.RowFilter = null;
                ShowInformationMessage(PDMsg.NotEmpty("SNO"));
                return;
            }
            dv.RowFilter = null;

            //if (!CHEESE_WT.IsNotNullOrEmpty() && !FINISH_WT.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Cheese And Finish Weight values"));
            //    return;
            //}

            //if (!CHEESE_WT.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Cheese Weight"));
            //    return;
            //}

            //if (!FINISH_WT.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Finish Weight"));
            //    return;
            //}

            //if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty() && !MandatoryFields.FINISH_WT.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Cheese And Finish Weight values"));
            //    return;
            //}

            if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Cheese Weight"));
                return;
            }

            //if (!MandatoryFields.FINISH_WT.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Finish Weight"));
            //    return;
            //}

            copyMandatoryFieldsToEntity(MandatoryFields);
            List<DDCOST_CENT_MAST> costCentreDetails = bll.GetCostCentreDetails();
            string pvCOST_CENT_CODE = (from row in costCentreDetails
                                       where row.COST_CENT_CODE == "OE PKG"
                                       select (ActiveEntity.LOC_CODE == "MM" ? (ActiveEntity.EXPORT.ToBooleanAsString() ? "EXP PKG" : "OE PKG")
                                                                             : (ActiveEntity.EXPORT.ToBooleanAsString() ? "EXP PKG - K" : "OE PKG - K"))).SingleOrDefault<string>();
            if (!pvCOST_CENT_CODE.IsNotNullOrEmpty()) return;

            CommonProcessCost process = (from row in bll.DB.V_OPERATION_COST
                                         where row.COST_CENT_CODE == pvCOST_CENT_CODE &&
                                         row.OPERATION_NO == CONST_DESPATCH_PROCESS_CODE &&
                                         row.LOC_CODE == ActiveEntity.LOC_CODE
                                         select new CommonProcessCost()
                                         {
                                             SNO = CONST_DESPATCH_PROCESS_SNO,
                                             PROCESS_CODE = CONST_DESPATCH_PROCESS_SNO,
                                             CODE = Convert.ToString(CONST_DESPATCH_PROCESS_CODE),
                                             DESCRIPTION = CONST_DESPATCH_PROCESS_DESCRIPTION,
                                             UNIT_CODE = row.UNIT_CODE,
                                             OUTPUT = 0,
                                             FIX_COST = Convert.ToDecimal(row.FIX_COST),
                                             VAR_COST = Convert.ToDecimal(row.VAR_COST),
                                             CHEESE_WT = 0.0m
                                         }).FirstOrDefault<CommonProcessCost>();

            if (!process.IsNotNullOrEmpty() || !process.UNIT_CODE.IsNotNullOrEmpty()) return;

            process.COST_CENT_CODE = pvCOST_CENT_CODE;

            decimal pvVARCOST = 0;
            decimal pvFIXCOST = 0;

            process.CHEESE_WT = ActiveEntity.CHEESE_WT.ToValueAsString().ToDecimalValue();
            process.FINISH_WT = ActiveEntity.FINISH_WT.ToValueAsString().ToDecimalValue();

            switch (process.UNIT_CODE)
            {
                case "1": //Hourly
                    if (process.OUTPUT == 0)
                    {
                        pvVARCOST = 0;
                        pvFIXCOST = 0;
                    }
                    else
                    {
                        pvVARCOST = process.VAR_COST / process.OUTPUT * 100;
                        pvFIXCOST = process.FIX_COST / process.OUTPUT * 100;
                    }
                    break;
                case "2": //Per Piece
                    pvVARCOST = process.VAR_COST * 100;
                    pvFIXCOST = process.FIX_COST * 100;
                    break;
                case "3": //Kilograms
                    pvVARCOST = process.VAR_COST * process.FINISH_WT;
                    pvFIXCOST = process.FIX_COST * process.FINISH_WT;

                    //pvVARCOST = process.VAR_COST * process.CHEESE_WT;
                    //pvFIXCOST = process.FIX_COST * process.CHEESE_WT;
                    break;
                case "4": //RM
                    pvVARCOST = process.VAR_COST * process.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    pvFIXCOST = process.FIX_COST * process.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    break;
                default: Message = "Unit Formula is not defined"; break;
            }

            ////Message = "Removing Despatch Process";
            ////CostDetails.RowFilter = "OPERATION_NO='" + CONST_DESPATCH_PROCESS_CODE + "'"; 
            ////if (CostDetails.Count > 0)
            ////{
            ////    DataTable dt = CostDetails.ToTable().Copy();
            ////    DataView CostDetailsTemp = dt.DefaultView;

            ////    CostDetails.RowFilter = null;
            ////    foreach (DataRowView dataRowView in CostDetailsTemp)
            ////    {
            ////        int rowIndex = 0;
            ////        CostDetails.RowFilter = "SNO='" + dataRowView.Row["SNO"] + "'"; 
            ////        if (CostDetails.Count > 0)
            ////        {
            ////            CostDetails.Delete(rowIndex);
            ////            rowIndex++;
            ////        }
            ////        CostDetails.RowFilter = null;
            ////    }

            ////}
            ////CostDetails.RowFilter = null;

            /*********** The Below commented code not implemented here due to unable to location rs!operation_no and only data formating************/
            Message = "Updating Despatch Process";
            CostDetails.RowFilter = "OPERATION_NO IN('" + CONST_DESPATCH_PROCESS_CODE + "')";
            foreach (DataRowView dataRowView in CostDetails)
            {
                dataRowView.BeginEdit();
                dataRowView["VAR_COST"] = dataRowView["VAR_COST"].ToValueAsString().ToDecimalValue();
                dataRowView["VAR_COST"] = dataRowView["FIX_COST"].ToValueAsString().ToDecimalValue();
                dataRowView["TOTAL_COST"] = Math.Round(dataRowView["VAR_COST"].ToValueAsString().ToDecimalValue() +
                                                       dataRowView["FIX_COST"].ToValueAsString().ToDecimalValue(), 2);
                dataRowView.EndEdit();
            }
            CostDetails.RowFilter = null;

            ////ssCostDetails.MoveFirst
            ////For i = 0 To ssCostDetails.Rows - 1
            //// If ssCostDetails.Columns(1).text = rs!operation_no Then
            ////     ssCostDetails.Columns(5).text = Format(pvVARCOST, "###0.#0")
            ////     ssCostDetails.Columns(6).text = Format(pvFIXCOST, "###0.#0")
            ////     ssCostDetails.Columns(7).text = 0
            ////      pvTotCost = pvVARCOST + pvFIXCOST
            ////     ssCostDetails.Columns(9).text = Format(pvTotCost, "###0.#0")
            //// End If
            //// ssCostDetails.MoveNext
            ////Next i

            Message = "Adding Despatch Process";
            DataRowView rowView = null;

            if (!CostDetails.IsNotNullOrEmpty()) return;

            foreach (DataRowView drv in CostDetails)
            {
                if (drv["OPERATION_NO"].ToString() == CONST_DESPATCH_PROCESS_CODE.ToString() && drv["PROCESS_CODE"].ToString() == process.PROCESS_CODE.ToString())
                {
                    rowView = drv;
                    break;
                }
            }

            bool isReNumberRequired = false;

            if (rowView == null)
            {
                if (!CostDetails.HasNonEmptyCells())
                {
                    isReNumberRequired = true;
                    rowView = getEmptyRowCostDetails();
                    if (!rowView.IsNotNullOrEmpty())
                    {
                        rowView = CostDetails.AddNew();
                        rowView.EndEdit();
                    }
                }
                else
                {
                    rowView = CostDetails.AddNew();
                }
            }

            if (!rowView.IsNotNullOrEmpty()) return;

            rowView["CI_REFERENCE"] = ActiveEntity.CI_REFERENCE;
            // Jeyan Added - start
            //DataTable costDetailsTemp = CostDetails.Table.Copy();
            //int maxRowNumber = 0;
            //if (costDetailsTemp.Rows.Count > 0)
            //{
            //    maxRowNumber = costDetailsTemp.Compute("Max(SNO)", null).ToValueAsString().ToIntValue();
            //}
            // Jeyan Added - end
            rowView["SNO"] = process.SNO;
            //rowView["SNO"] = maxRowNumber; // Commented by Jeyan
            rowView["PROCESS_CODE"] = process.PROCESS_CODE;
            rowView["OPERATION_NO"] = process.CODE;
            rowView["OPERATION"] = process.DESCRIPTION;
            rowView["COST_CENT_CODE"] = pvCOST_CENT_CODE;
            rowView["OUTPUT"] = process.OUTPUT;

            rowView["CCCode"] = process.COST_CENT_CODE;
            rowView["CCOutput"] = process.OUTPUT;

            rowView["VAR_COST"] = Math.Round(pvVARCOST, 2);
            rowView["FIX_COST"] = Math.Round(pvFIXCOST, 2);
            rowView["SPL_COST"] = 0.0m;
            rowView["UNIT_OF_MEASURE"] = process.UNIT_CODE;
            rowView["TOTAL_COST"] = Math.Round(pvVARCOST + pvFIXCOST, 2);
            rowView["ROWID"] = Guid.NewGuid();
            rowView["CI_INFO_FK"] = ActiveEntity.IDPK;
            rowView.EndEdit();
            Message = "Despatch Process Completed";
            if (isReNumberRequired)
            {
                DataView dt = CostDetails;
                string fieldName = "SNO";
                ReNumber(ref dt, fieldName);
                rowView["SNO"] = process.SNO; 
                //rowView["SNO"] = maxRowNumber;
                rowView.EndEdit();
            }

            if (canAddNewCostDetails())
            {
                rowView = CostDetails.AddNew();
                rowView.EndEdit();
            }

            CostCalculation();

        }

        private void fnRawMaterialProcess()
        {
            if (!ActiveEntity.IsNotNullOrEmpty() || !isCIReferenceSelectionCompleted) return;

            DataView dv = CostDetails.ToTable().Copy().DefaultView;
            dv.RowFilter = "CONVERT(Isnull(SNO,''), System.String) = ''";
            if (dv.Count > 1)
            {
                dv.RowFilter = null;
                ShowInformationMessage(PDMsg.NotEmpty("SNO"));
                return;
            }
            dv.RowFilter = null;

            //if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty() && !MandatoryFields.FINISH_WT.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Cheese And Finish Weight values"));
            //    return;
            //}

            if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Cheese Weight"));
                return;
            }

            //if (!MandatoryFields.FINISH_WT.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Finish Weight"));
            //    return;
            //}

            if (!ActiveEntity.SUGGESTED_RM.IsNotNullOrEmpty()) return;

            copyMandatoryFieldsToEntity(MandatoryFields);
            CommonProcessCost process = (from row in bll.GetRawMaterialProcessCostDetails()
                                         where row.RM_CODE == ActiveEntity.SUGGESTED_RM &&
                                         row.LOC_CODE == ActiveEntity.LOC_CODE &&
                                         row.OPERATION_NO == CONST_RAW_MATERIAL_PROCESS_CODE
                                         select new CommonProcessCost()
                                         {
                                             SNO = row.SNO,
                                             PROCESS_CODE = CONST_RAW_MATERIAL_PROCESS_SNO,
                                             CODE = Convert.ToString(row.OPERATION_NO),
                                             DESCRIPTION = row.OPERATION_DESCRIPTION,
                                             OUTPUT = row.OUTPUT,
                                             UNIT_CODE = row.UNIT_CODE,
                                             VAR_COST = Convert.ToDecimal(row.VAR_COST),
                                             FIX_COST = Convert.ToDecimal(row.FIX_COST),
                                             COST_CENT_CODE = row.COST_CENT_CODE

                                         }).FirstOrDefault<CommonProcessCost>();
            if (!process.IsNotNullOrEmpty())
            {
                ShowInformationMessage("Selected Raw material has no process");
                return;
            }

            decimal pvVARCOST = 0;
            decimal pvFIXCOST = 0;

            process.CHEESE_WT = ActiveEntity.CHEESE_WT.ToValueAsString().ToDecimalValue();
            process.FINISH_WT = ActiveEntity.FINISH_WT.ToValueAsString().ToDecimalValue();

            switch (process.UNIT_CODE)
            {
                case "1": //Hourly
                    if (process.OUTPUT == 0)
                    {
                        pvVARCOST = 0;
                        pvFIXCOST = 0;
                    }
                    else
                    {
                        pvVARCOST = process.VAR_COST / process.OUTPUT * 100;
                        pvFIXCOST = process.FIX_COST / process.OUTPUT * 100;
                    }
                    break;
                case "2": //Per Piece
                    pvVARCOST = process.VAR_COST * 100;
                    pvFIXCOST = process.FIX_COST * 100;
                    break;
                case "3": //Kilograms
                    pvVARCOST = process.VAR_COST * process.FINISH_WT;
                    pvFIXCOST = process.FIX_COST * process.FINISH_WT;
                    break;
                case "4": //RM
                    pvVARCOST = process.VAR_COST * process.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    pvFIXCOST = process.FIX_COST * process.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    break;
                default: Message = "Unit Formula is not defined"; break;
            }

            //Message = "Removing Raw Material Process";
            //CostDetails.RowFilter = "OPERATION_NO='" + CONST_RAW_MATERIAL_PROCESS_CODE + "'"; 
            //if (CostDetails.Count > 0)
            //{
            //    DataTable dt = CostDetails.ToTable().Copy();
            //    DataView CostDetailsTemp = dt.DefaultView;

            //    CostDetails.RowFilter = null;
            //    foreach (DataRowView dataRowView in CostDetailsTemp)
            //    {
            //        int rowIndex = 0;
            //        CostDetails.RowFilter = "SNO='" + dataRowView.Row["SNO"] + "'"; 
            //        if (CostDetails.Count > 0)
            //        {
            //            CostDetails.Delete(rowIndex);
            //            rowIndex++;
            //        }
            //        CostDetails.RowFilter = null;
            //    }

            //}
            //CostDetails.RowFilter = null;

            /*********** The Below commented code not implemented here due to unable to location rs!operation_no and only data formating************/
            Message = "Updating Raw Material Process";
            CostDetails.RowFilter = "OPERATION_NO IN('" + CONST_RAW_MATERIAL_PROCESS_CODE + "')";
            foreach (DataRowView dataRowView in CostDetails)
            {
                dataRowView.BeginEdit();
                dataRowView["VAR_COST"] = dataRowView["VAR_COST"].ToValueAsString().ToDecimalValue();
                dataRowView["VAR_COST"] = dataRowView["FIX_COST"].ToValueAsString().ToDecimalValue();
                dataRowView["TOTAL_COST"] = Math.Round(dataRowView["VAR_COST"].ToValueAsString().ToDecimalValue() +
                                                       dataRowView["FIX_COST"].ToValueAsString().ToDecimalValue(), 2);
                dataRowView.EndEdit();
            }
            CostDetails.RowFilter = null;

            ////ssCostDetails.MoveFirst
            ////For i = 0 To ssCostDetails.Rows - 1
            //// If ssCostDetails.Columns(1).text = rs!operation_no Then
            ////     ssCostDetails.Columns(5).text = Format(pvVARCOST, "###0.#0")
            ////     ssCostDetails.Columns(6).text = Format(pvFIXCOST, "###0.#0")
            ////     ssCostDetails.Columns(7).text = 0
            ////      pvTotCost = pvVARCOST + pvFIXCOST
            ////     ssCostDetails.Columns(9).text = Format(pvTotCost, "###0.#0")
            //// End If
            //// ssCostDetails.MoveNext
            ////Next i

            Message = "Adding Raw Material Process";

            DataRowView rowView = null;

            if (!CostDetails.IsNotNullOrEmpty()) return;

            foreach (DataRowView drv in CostDetails)
            {
                if (drv["OPERATION_NO"].ToString() == CONST_DESPATCH_PROCESS_CODE.ToString() && drv["PROCESS_CODE"].ToString() == process.PROCESS_CODE.ToString())
                {
                    rowView = drv;
                    break;
                }
            }

            bool isReNumberRequired = false;

            if (rowView == null)
            {
                if (!CostDetails.HasNonEmptyCells())
                {
                    isReNumberRequired = true;
                    rowView = getEmptyRowCostDetails();
                    if (!rowView.IsNotNullOrEmpty())
                    {
                        rowView = CostDetails.AddNew();
                        rowView.EndEdit();
                    }
                }
                else
                {
                    rowView = CostDetails.AddNew();
                }
            }

            if (!rowView.IsNotNullOrEmpty()) return;

            rowView["CI_REFERENCE"] = ActiveEntity.CI_REFERENCE;
            rowView["SNO"] = process.SNO;
            rowView["PROCESS_CODE"] = process.PROCESS_CODE;
            rowView["OPERATION_NO"] = process.CODE;
            rowView["OPERATION"] = process.DESCRIPTION;
            rowView["COST_CENT_CODE"] = process.COST_CENT_CODE;
            rowView["OUTPUT"] = process.OUTPUT;

            rowView["CCCode"] = process.COST_CENT_CODE;
            rowView["CCOutput"] = process.OUTPUT;

            rowView["VAR_COST"] = Math.Round(pvVARCOST, 2);
            rowView["FIX_COST"] = Math.Round(pvFIXCOST, 2);
            rowView["SPL_COST"] = 0.0m;
            rowView["UNIT_OF_MEASURE"] = process.UNIT_CODE;
            rowView["TOTAL_COST"] = Math.Round(pvVARCOST + pvFIXCOST, 2);
            rowView["ROWID"] = Guid.NewGuid();
            rowView["CI_INFO_FK"] = ActiveEntity.IDPK;
            rowView.EndEdit();
            Message = "Raw Material Process Completed";
            if (isReNumberRequired)
            {
                DataView dt = CostDetails;
                string fieldName = "SNO";
                ReNumber(ref dt, fieldName);
                rowView["SNO"] = process.SNO;
                rowView.EndEdit();
            }

            if (canAddNewCostDetails())
            {
                rowView = CostDetails.AddNew();
                rowView.EndEdit();
            }

            CostCalculation();
        }

        private void fnFinishProcess()
        {
            if (!ActiveEntity.IsNotNullOrEmpty() || !isCIReferenceSelectionCompleted) return;
            if (!ActiveEntity.FINISH_CODE.IsNotNullOrEmpty()) return;

            DataView dv = CostDetails.ToTable().Copy().DefaultView;
            dv.RowFilter = "CONVERT(Isnull(SNO,''), System.String) = ''";
            if (dv.Count > 1)
            {
                dv.RowFilter = null;
                ShowInformationMessage(PDMsg.NotEmpty("SNO"));
                return;
            }
            dv.RowFilter = null;

            copyMandatoryFieldsToEntity(MandatoryFields);
            CommonProcessCost process = (from row in bll.GetFinishProcessCostDetails()
                                         where row.FINISH_CODE == ActiveEntity.FINISH_CODE.ToIntValue() &&
                                         row.LOC_CODE == ActiveEntity.LOC_CODE
                                         select new CommonProcessCost()
                                         {
                                             SNO = row.SNO,
                                             PROCESS_CODE = CONST_FINISH_PROCESS_SNO,
                                             CODE = Convert.ToString(row.OPERATION_NO),
                                             DESCRIPTION = row.OPERATION,
                                             OUTPUT = row.OUTPUT,
                                             UNIT_CODE = row.UNIT_CODE,
                                             VAR_COST = Convert.ToDecimal(row.VAR_COST),
                                             FIX_COST = Convert.ToDecimal(row.FIX_COST),
                                             COST_CENT_CODE = row.COST_CENT_CODE
                                         }).FirstOrDefault<CommonProcessCost>();
            if (!process.IsNotNullOrEmpty())
            {
                if (isCIReferenceSelectionCompleted) ShowInformationMessage("Process for this finish is not available at the specified location");
                return;
            }

            decimal pvVARCOST = 0;
            decimal pvFIXCOST = 0;

            process.CHEESE_WT = ActiveEntity.CHEESE_WT.ToValueAsString().ToDecimalValue();
            process.FINISH_WT = ActiveEntity.FINISH_WT.ToValueAsString().ToDecimalValue();

            switch (process.UNIT_CODE)
            {
                case "1": //Hourly
                    if (process.OUTPUT == 0)
                    {
                        pvVARCOST = 0;
                        pvFIXCOST = 0;
                    }
                    else
                    {
                        pvVARCOST = process.VAR_COST / process.OUTPUT * 100;
                        pvFIXCOST = process.FIX_COST / process.OUTPUT * 100;
                    }
                    break;
                case "2": //Per Piece
                    pvVARCOST = process.VAR_COST * 100;
                    pvFIXCOST = process.FIX_COST * 100;
                    break;
                case "3": //Kilograms
                    pvVARCOST = process.VAR_COST * process.FINISH_WT;
                    pvFIXCOST = process.FIX_COST * process.FINISH_WT;
                    break;
                case "4": //RM
                    pvVARCOST = process.VAR_COST * process.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    pvFIXCOST = process.FIX_COST * process.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    break;
                default: Message = "Unit Formula is not defined"; break;
            }

            //Message = "Removing Finish Process";
            //CostDetails.RowFilter = "OPERATION_NO IN('" + CONST_FINISH_PROCESS_CODE + "'"; 
            //if (CostDetails.Count > 0)
            //{
            //    DataTable dt = CostDetails.ToTable().Copy();
            //    DataView CostDetailsTemp = dt.DefaultView;

            //    CostDetails.RowFilter = null;
            //    foreach (DataRowView dataRowView in CostDetailsTemp)
            //    {
            //        int rowIndex = 0;
            //        CostDetails.RowFilter = "SNO='" + dataRowView.Row["SNO"] + "'"; 
            //        if (CostDetails.Count > 0)
            //        {
            //            CostDetails.Delete(rowIndex);
            //            rowIndex++;
            //        }
            //        CostDetails.RowFilter = null;
            //    }

            //}
            //CostDetails.RowFilter = null;

            Message = "Updating Finish Process";

            CostDetails.RowFilter = "OPERATION_NO IN('" + CONST_FINISH_PROCESS_CODE + "')";
            foreach (DataRowView dataRowView in CostDetails)
            {
                dataRowView.BeginEdit();
                dataRowView["VAR_COST"] = dataRowView["VAR_COST"].ToValueAsString().ToDecimalValue();
                dataRowView["VAR_COST"] = dataRowView["FIX_COST"].ToValueAsString().ToDecimalValue();
                dataRowView["TOTAL_COST"] = Math.Round(dataRowView["VAR_COST"].ToValueAsString().ToDecimalValue() +
                                                       dataRowView["FIX_COST"].ToValueAsString().ToDecimalValue(), 2);
                dataRowView.EndEdit();
            }
            CostDetails.RowFilter = null;

            Message = "Adding Finish Process";

            DataRowView rowView = null;
            if (!CostDetails.IsNotNullOrEmpty()) return;
            CostDetails.RowFilter = "OPERATION_NO IN('" + CONST_FINISH_PROCESS_CODE + "') AND PROCESS_CODE='" + process.PROCESS_CODE.ToValueAsString().Trim().FormatEscapeChars() + "'";
            bool isReNumberRequired = false;
            if (CostDetails.Count > 0)
            {
                rowView = CostDetails[0];
            }
            else if (!CostDetails.HasNonEmptyCells())
            {
                isReNumberRequired = true;
                rowView = getEmptyRowCostDetails();
                if (!rowView.IsNotNullOrEmpty())
                {
                    rowView = CostDetails.AddNew();
                    rowView.EndEdit();
                }
            }
            else
            {
                rowView = CostDetails.AddNew();
            }
            CostDetails.RowFilter = null;
            if (!rowView.IsNotNullOrEmpty()) return;
            // Jeyan Added - start
            DataTable costDetailsTemp = CostDetails.Table.Copy();
            int maxRowNumber = 0;
            if (costDetailsTemp.Rows.Count > 0)
            {
                 maxRowNumber = costDetailsTemp.Compute("Max(SNO)", null).ToValueAsString().ToIntValue();
            }
            // Jeyan Added - end
            rowView["CI_REFERENCE"] = ActiveEntity.CI_REFERENCE;
            //rowView["SNO"] = process.SNO; Jeyan commented
            rowView["SNO"] = maxRowNumber + 10;
            rowView["PROCESS_CODE"] = process.PROCESS_CODE;
            rowView["OPERATION_NO"] = process.CODE;
            rowView["OPERATION"] = process.DESCRIPTION;
            rowView["COST_CENT_CODE"] = process.COST_CENT_CODE;
            rowView["OUTPUT"] = process.OUTPUT;

            rowView["CCCode"] = process.COST_CENT_CODE;
            rowView["CCOutput"] = process.OUTPUT;

            rowView["VAR_COST"] = Math.Round(pvVARCOST, 2);
            rowView["FIX_COST"] = Math.Round(pvFIXCOST, 2);
            rowView["SPL_COST"] = 0.0m;
            rowView["UNIT_OF_MEASURE"] = process.UNIT_CODE;
            rowView["TOTAL_COST"] = Math.Round(pvVARCOST + pvFIXCOST, 2);
            rowView["ROWID"] = Guid.NewGuid();
            rowView["CI_INFO_FK"] = ActiveEntity.IDPK;
            rowView.EndEdit();
            Message = "Finish Process Completed";
            if (isReNumberRequired)
            {
                DataView dt = CostDetails;
                string fieldName = "SNO";
                ReNumber(ref dt, fieldName);
                //rowView["SNO"] = process.SNO; Jeyan commented
                rowView["SNO"] = maxRowNumber + 10;
                rowView.EndEdit();
            }

            if (canAddNewCostDetails())
            {
                rowView = CostDetails.AddNew();
                rowView.EndEdit();
            }

            CostCalculation();

        }

        private void fnCostCenterProcess(CommonProcessCost activeChildEntity)
        {
            copyMandatoryFieldsToEntity(MandatoryFields);
            if (!ActiveEntity.IsNotNullOrEmpty() || !activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() ||
                !activeChildEntity.CODE.IsNotNullOrEmpty()) return;

            DataView dv = CostDetails.ToTable().Copy().DefaultView;
            dv.RowFilter = "CONVERT(Isnull(SNO,''), System.String) = ''";
            if (dv.Count > 1)
            {
                dv.RowFilter = null;
                ShowInformationMessage(PDMsg.NotEmpty("SNO"));
                return;
            }
            dv.RowFilter = null;

            //if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty() && !MandatoryFields.FINISH_WT.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Cheese And Finish Weight values"));
            //    return;
            //}

            if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Cheese Weight"));
                return;
            }

            //if (!MandatoryFields.FINISH_WT.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Finish Weight"));
            //    return;
            //}

            if (!ActiveEntity.LOC_CODE.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Plant"));
                return;
            }

            if (!ActiveEntity.LOC_CODE.IsNotNullOrEmpty())
            {
                ShowInformationMessage("Cannot find related Cost Center for this operation");
                return;
            }

            List<DDCC_OUTPUT> lstOutput = null;
            decimal output = 0;

            lstOutput = (from row in CostCentreOutputEntity
                         where row.COST_CENT_CODE == activeChildEntity.COST_CENT_CODE
                         orderby row.OUTPUT ascending
                         select row).ToList<DDCC_OUTPUT>();
            if (lstOutput.IsNotNullOrEmpty() && lstOutput.Count > 0)
            {
                output = lstOutput[0].OUTPUT.ToValueAsString().ToDecimalValue();
            }

            CommonProcessCost process = (from row in bll.GetCostCenterProcessCostDetails()
                                         where row.OPERATION_NO == Convert.ToDecimal(activeChildEntity.CODE) &&
                                         row.LOC_CODE == ActiveEntity.LOC_CODE &&
                                         row.COST_CENT_CODE == activeChildEntity.COST_CENT_CODE
                                         select new CommonProcessCost()
                                         {
                                             SNO = activeChildEntity.SNO.ToValueAsString().ToIntValue(),
                                             PROCESS_CODE = activeChildEntity.SNO.ToValueAsString().ToIntValue(),
                                             CODE = activeChildEntity.CODE,
                                             DESCRIPTION = activeChildEntity.DESCRIPTION,
                                             OUTPUT = output,
                                             UNIT_CODE = row.UNIT_CODE,
                                             VAR_COST = Convert.ToDecimal(row.VAR_COST),
                                             FIX_COST = Convert.ToDecimal(row.FIX_COST),
                                             SPL_COST = Convert.ToDecimal(activeChildEntity.SPL_COST),
                                             COST_CENT_CODE = row.COST_CENT_CODE
                                         }).FirstOrDefault<CommonProcessCost>();
            if (!process.IsNotNullOrEmpty())
            {
                //ShowInformationMessage("Invalid Cost Centre for that Operation");
                return;
            }

            decimal pvVARCOST = 0;
            decimal pvFIXCOST = 0;

            process.CHEESE_WT = ActiveEntity.CHEESE_WT.ToValueAsString().ToDecimalValue();
            process.FINISH_WT = ActiveEntity.FINISH_WT.ToValueAsString().ToDecimalValue();
            switch (process.UNIT_CODE)
            {
                case "1": //Hourly
                    if (process.OUTPUT == 0)
                    {
                        pvVARCOST = 0;
                        pvFIXCOST = 0;
                    }
                    else
                    {
                        pvVARCOST = process.VAR_COST / process.OUTPUT * 100;
                        pvFIXCOST = process.FIX_COST / process.OUTPUT * 100;
                    }
                    break;
                case "2": //Per Piece
                    pvVARCOST = process.VAR_COST * 100;
                    pvFIXCOST = process.FIX_COST * 100;
                    break;
                case "3": //Kilograms
                    pvVARCOST = process.VAR_COST * process.FINISH_WT;
                    pvFIXCOST = process.FIX_COST * process.FINISH_WT;
                    break;
                case "4": //RM
                    pvVARCOST = process.VAR_COST * process.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    pvFIXCOST = process.FIX_COST * process.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    break;
                default: Message = "Unit Formula is not defined"; break;
            }

            Message = "Adding Finish Process";

            DataRowView rowView = null;
            foreach (DataRowView drv in CostDetails)
            {
                if (drv["OPERATION_NO"].ToString() == activeChildEntity.CODE.FormatEscapeChars() && drv["PROCESS_CODE"].ToString() == process.PROCESS_CODE.ToString())
                {
                    rowView = drv;
                    break;
                }
            }

            if (!rowView.IsNotNullOrEmpty()) return;

            rowView["CI_REFERENCE"] = ActiveEntity.CI_REFERENCE;
            rowView["SNO"] = process.SNO;
            rowView["PROCESS_CODE"] = process.PROCESS_CODE;
            rowView["OPERATION_NO"] = process.CODE;
            rowView["OPERATION"] = process.DESCRIPTION;
            rowView["COST_CENT_CODE"] = process.COST_CENT_CODE;
            rowView["OUTPUT"] = process.OUTPUT;

            rowView["CCCode"] = process.COST_CENT_CODE;
            rowView["CCOutput"] = process.OUTPUT;

            rowView["VAR_COST"] = Math.Round(pvVARCOST, 2);
            rowView["FIX_COST"] = Math.Round(pvFIXCOST, 2);
            rowView["SPL_COST"] = 0.0m;
            rowView["UNIT_OF_MEASURE"] = process.UNIT_CODE;
            rowView["TOTAL_COST"] = Math.Round(pvVARCOST + pvFIXCOST + process.SPL_COST, 2);
            rowView["ROWID"] = Guid.NewGuid();
            rowView["CI_INFO_FK"] = ActiveEntity.IDPK;
            rowView.EndEdit();
            Message = "Finish Process Completed";

            CostCalculation();

        }

        private void fnOperationProcess(CommonProcessCost activeChildEntity)
        {
            if (!ActiveEntity.IsNotNullOrEmpty() || !activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() ||
                !activeChildEntity.CODE.IsNotNullOrEmpty()) return;

            DataView dv = CostDetails.ToTable().Copy().DefaultView;
            dv.RowFilter = "CONVERT(Isnull(SNO,''), System.String) = ''";
            if (dv.Count > 1)
            {
                dv.RowFilter = null;
                ShowInformationMessage(PDMsg.NotEmpty("SNO"));
                return;
            }
            dv.RowFilter = null;

            //if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty() && !MandatoryFields.FINISH_WT.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Cheese And Finish Weight values"));
            //    return;
            //}

            if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Cheese Weight"));
                return;
            }

            //if (!MandatoryFields.FINISH_WT.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Finish Weight"));
            //    return;
            //}

            if (!ActiveEntity.LOC_CODE.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Plant"));
                return;
            }

            if (!ActiveEntity.LOC_CODE.IsNotNullOrEmpty())
            {
                ShowInformationMessage("Cannot find related Cost Center for this operation");
                return;
            }

            List<DDCC_OUTPUT> lstOutput = null;
            decimal output = 0;

            lstOutput = (from row in CostCentreOutputEntity
                         where row.COST_CENT_CODE == activeChildEntity.COST_CENT_CODE
                         orderby row.OUTPUT ascending
                         select row).ToList<DDCC_OUTPUT>();
            if (lstOutput.IsNotNullOrEmpty() && lstOutput.Count > 0)
            {
                output = lstOutput[0].OUTPUT.ToValueAsString().ToDecimalValue();
            }

            CommonProcessCost process = (from row in bll.GetCostCenterProcessCostDetails()
                                         where row.OPERATION_NO == Convert.ToDecimal(activeChildEntity.CODE) &&
                                         row.LOC_CODE == ActiveEntity.LOC_CODE
                                         select new CommonProcessCost()
                                         {
                                             SNO = activeChildEntity.SNO.ToValueAsString().ToIntValue(),
                                             PROCESS_CODE = activeChildEntity.SNO.ToValueAsString().ToIntValue(),
                                             CODE = Convert.ToString(activeChildEntity.CODE),
                                             DESCRIPTION = activeChildEntity.DESCRIPTION,
                                             OUTPUT = output,
                                             UNIT_CODE = row.UNIT_CODE,
                                             VAR_COST = Convert.ToDecimal(row.VAR_COST),
                                             FIX_COST = Convert.ToDecimal(row.FIX_COST),
                                             SPL_COST = Convert.ToDecimal(activeChildEntity.SPL_COST),
                                             COST_CENT_CODE = row.COST_CENT_CODE
                                         }).FirstOrDefault<CommonProcessCost>();
            if (!process.IsNotNullOrEmpty())
            {
                ShowInformationMessage("Invalid Cost Centre for that Operation");
                return;
            }

            decimal pvVARCOST = 0;
            decimal pvFIXCOST = 0;

            process.CHEESE_WT = ActiveEntity.CHEESE_WT.ToValueAsString().ToDecimalValue();
            process.FINISH_WT = ActiveEntity.FINISH_WT.ToValueAsString().ToDecimalValue();

            switch (process.UNIT_CODE)
            {
                case "1": //Hourly
                    if (process.OUTPUT == 0)
                    {
                        pvVARCOST = 0;
                        pvFIXCOST = 0;
                    }
                    else
                    {
                        pvVARCOST = process.VAR_COST / process.OUTPUT * 100;
                        pvFIXCOST = process.FIX_COST / process.OUTPUT * 100;
                    }
                    break;
                case "2": //Per Piece
                    pvVARCOST = process.VAR_COST * 100;
                    pvFIXCOST = process.FIX_COST * 100;
                    break;
                case "3": //Kilograms
                    pvVARCOST = process.VAR_COST * process.FINISH_WT;
                    pvFIXCOST = process.FIX_COST * process.FINISH_WT;
                    break;
                case "4": //RM
                    pvVARCOST = process.VAR_COST * process.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    pvFIXCOST = process.FIX_COST * process.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    break;
                default: Message = "Unit Formula is not defined"; break;
            }

            Message = "Adding Finish Process";

            DataRowView rowView = null;
            foreach (DataRowView drv in CostDetails)
            {
                if (drv["OPERATION_NO"].ToString() == activeChildEntity.CODE.FormatEscapeChars() && drv["PROCESS_CODE"].ToString() == process.PROCESS_CODE.ToString())
                {
                    rowView = drv;
                    break;
                }
            }

            if (!rowView.IsNotNullOrEmpty()) return;
            rowView.BeginEdit();
            rowView["CI_REFERENCE"] = ActiveEntity.CI_REFERENCE;
            rowView["SNO"] = process.SNO;
            rowView["PROCESS_CODE"] = process.PROCESS_CODE;
            rowView["OPERATION_NO"] = process.CODE;
            rowView["OPERATION"] = process.DESCRIPTION;
            rowView["COST_CENT_CODE"] = process.COST_CENT_CODE;

            rowView["CCCode"] = process.COST_CENT_CODE;
            rowView["CCOutput"] = process.OUTPUT;

            rowView["OUTPUT"] = process.OUTPUT;
            rowView["VAR_COST"] = Math.Round(pvVARCOST, 2);
            rowView["FIX_COST"] = Math.Round(pvFIXCOST, 2);
            rowView["SPL_COST"] = 0.0m;
            rowView["UNIT_OF_MEASURE"] = process.UNIT_CODE;
            rowView["TOTAL_COST"] = Math.Round(pvVARCOST + pvFIXCOST + process.SPL_COST, 2);
            rowView["ROWID"] = Guid.NewGuid();
            rowView["CI_INFO_FK"] = ActiveEntity.IDPK;
            rowView.EndEdit();
            Message = "Finish Process Completed";

            CostCalculation();

        }

        private void fnUpdateOutput(CommonProcessCost activeChildEntity)
        {
            if (!ActiveEntity.IsNotNullOrEmpty() || !activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() ||
                !activeChildEntity.CODE.IsNotNullOrEmpty() || !CostDetails.IsNotNullOrEmpty() || CostDetails.Count == 0) return;

            DataView dv = CostDetails.ToTable().Copy().DefaultView;
            dv.RowFilter = "CONVERT(Isnull(SNO,''), System.String) = ''";
            if (dv.Count > 1)
            {
                dv.RowFilter = null;
                ShowInformationMessage(PDMsg.NotEmpty("SNO"));
                return;
            }
            dv.RowFilter = null;

            //if (!ActiveEntity.FINISH_WT.IsNotNullOrEmpty())
            //{
            //    ShowMessage = "Finish Weight should be entered";
            //    return;
            //}

            //if (!ActiveEntity.CHEESE_WT.IsNotNullOrEmpty())
            //{
            //    ShowMessage = "Cheese Weight should be entered";
            //    return;
            //}

            //if (!ActiveEntity.LOC_CODE.IsNotNullOrEmpty())
            //{
            //    ShowMessage = "Plant should not be empty for this operation";
            //    return;
            //}

            //if (!ActiveEntity.LOC_CODE.IsNotNullOrEmpty())
            //{
            //    ShowMessage = "Cannot find related Cost Center for this operation";
            //    return;
            //}

            //DDCC_OPER outputOperation = (from row in bll.DB.DDCC_OPER
            //                             where row.COST_CENT_CODE == activeChildEntity.COST_CENT_CODE &&
            //                             row.OPN_CODE == Convert.ToDecimal(activeChildEntity.CODE)
            //                             select row).FirstOrDefault<DDCC_OPER>();
            //if (!outputOperation.IsNotNullOrEmpty()) return;

            CommonProcessCost process = (from row in bll.DB.DDCC_OPER
                                         where row.COST_CENT_CODE == activeChildEntity.COST_CENT_CODE &&
                                         row.OPN_CODE == Convert.ToDecimal(activeChildEntity.CODE)
                                         select new CommonProcessCost()
                                         {
                                             SNO = activeChildEntity.SNO.ToValueAsString().ToIntValue(),
                                             PROCESS_CODE = activeChildEntity.SNO.ToValueAsString().ToIntValue(),
                                             CODE = Convert.ToString(activeChildEntity.CODE),
                                             DESCRIPTION = activeChildEntity.DESCRIPTION,
                                             OUTPUT = Convert.ToDecimal(activeChildEntity.OUTPUT),
                                             UNIT_CODE = row.UNIT_CODE,
                                             VAR_COST = Convert.ToDecimal(row.VAR_COST),
                                             FIX_COST = Convert.ToDecimal(row.FIX_COST),
                                             SPL_COST = Convert.ToDecimal(activeChildEntity.SPL_COST),
                                             COST_CENT_CODE = row.COST_CENT_CODE
                                         }).FirstOrDefault<CommonProcessCost>();
            if (!process.IsNotNullOrEmpty())
            {
                //ShowInformationMessage("Invalid Cost Centre for that Operation");
                return;
            }

            decimal pvVARCOST = 0;
            decimal pvFIXCOST = 0;

            process.CHEESE_WT = ActiveEntity.CHEESE_WT.ToValueAsString().ToDecimalValue();
            process.FINISH_WT = ActiveEntity.FINISH_WT.ToValueAsString().ToDecimalValue();

            switch (process.UNIT_CODE)
            {
                case "1": //Hourly
                    if (process.OUTPUT == 0)
                    {
                        pvVARCOST = 0;
                        pvFIXCOST = 0;
                    }
                    else
                    {
                        pvVARCOST = process.VAR_COST / process.OUTPUT * 100;
                        pvFIXCOST = process.FIX_COST / process.OUTPUT * 100;
                    }
                    break;
                case "2": //Per Piece
                    pvVARCOST = process.VAR_COST * 100;
                    pvFIXCOST = process.FIX_COST * 100;
                    break;
                case "3": //Kilograms
                    pvVARCOST = process.VAR_COST * process.FINISH_WT;
                    pvFIXCOST = process.FIX_COST * process.FINISH_WT;
                    break;
                case "4": //RM
                    pvVARCOST = process.VAR_COST * process.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    pvFIXCOST = process.FIX_COST * process.CHEESE_WT * ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue();
                    break;
                default: Message = "Unit Formula is not defined"; break;
            }

            Message = "Adding Finish Process";
            DataRowView rowView = null;
            foreach (DataRowView drv in CostDetails)
            {
                if (drv["OPERATION_NO"].ToString() == activeChildEntity.CODE.FormatEscapeChars() && drv["PROCESS_CODE"].ToString() == process.PROCESS_CODE.ToString())
                {
                    rowView = drv;
                    break;
                }
            }

            if (rowView.IsNotNullOrEmpty())
            {
                rowView.BeginEdit();
                rowView["CI_REFERENCE"] = ActiveEntity.CI_REFERENCE;
                rowView["SNO"] = process.SNO;
                rowView["PROCESS_CODE"] = process.PROCESS_CODE;
                rowView["OPERATION_NO"] = process.CODE;
                rowView["OPERATION"] = process.DESCRIPTION;
                rowView["COST_CENT_CODE"] = process.COST_CENT_CODE;
                rowView["OUTPUT"] = process.OUTPUT;

                rowView["CCCode"] = process.COST_CENT_CODE;
                rowView["CCOutput"] = process.OUTPUT;

                rowView["VAR_COST"] = Math.Round(pvVARCOST, 2);
                rowView["FIX_COST"] = Math.Round(pvFIXCOST, 2);
                rowView["SPL_COST"] = 0.0m;
                rowView["UNIT_OF_MEASURE"] = process.UNIT_CODE;
                rowView["TOTAL_COST"] = Math.Round(pvVARCOST + pvFIXCOST + process.SPL_COST, 2);
                rowView["ROWID"] = Guid.NewGuid();
                rowView["CI_INFO_FK"] = ActiveEntity.IDPK;
                rowView.EndEdit();
            }

            Message = "Finish Process Completed";

            CostCalculation();

        }

        private void ReNumber(ref DataView dataView, string fieldName, string orderBy = "ASC", int multipleBy = 10, bool isNotManualReOrder = true)
        {
            if (!CostDetails.IsNotNullOrEmpty()) return;
            if (!fieldName.IsNotNullOrEmpty()) fieldName = "SNO";
            if (!orderBy.IsNotNullOrEmpty()) orderBy = "ASC";
            if (multipleBy <= 0) multipleBy = 10;


            DataTable dt = dataView.Table;
            string newFieldName = "SNO_SEQUENCE";

            DataTable costDetailsCopy = CostDetails.Table.Copy();
            if (costDetailsCopy.Rows.Count > 0)
            {
                int maxRowNumber = costDetailsCopy.Compute("Max(" + newFieldName + ")", null).ToValueAsString().ToIntValue();

            }

            //if (!dt.Columns.Contains(newFieldName))
            //{
            //    dt.Columns.Add(newFieldName, typeof(int));
            //}

            int nextNumber = 1000000;

            ////Copy FieldName values into newFieldName, Assing Empty Rows as decreased 10000 no.
            //dataView.RowFilter = null;
            //dataView.Sort = null;
            //foreach (DataRowView rowView in dataView)
            //{
            //    rowView[newFieldName] = rowView[fieldName].ToValueAsString().ToIntValue();
            //    if (!rowView[fieldName].ToValueAsString().IsNotNullOrEmpty())
            //    {
            //        rowView[newFieldName] = --nextNumber;
            //        rowView.EndEdit();
            //    }
            //}
            int lastRowIndex = 99999999;
            int rowIndex = 0;
            nextNumber = 0;
            dataView.Sort = null;
            //dataView.Sort = newFieldName + " ASC";
            dataView.Sort = newFieldName + " ASC";

            foreach (DataRowView rowView in dataView)
            {
                if (!(rowView["SNO"].ToValueAsString().Length == 0 && rowView["OPERATION_NO"].ToValueAsString().Length == 0 && rowView["OPERATION"].ToValueAsString().Length == 0 && rowView["COST_CENT_CODE"].ToValueAsString().Length == 0))
                {
                    rowView["PROCESS_CODE"] = Math.Round(rowView["SNO"].ToValueAsString().ToDoubleValue(), 0);
                }
                else
                {
                    if (lastRowIndex == nextNumber) { --lastRowIndex; }
                    rowView.BeginEdit();
                    rowView["PROCESS_CODE"] = lastRowIndex;
                    rowView.EndEdit();
                }
            }

            dataView.Sort = "PROCESS_CODE ASC";

            foreach (DataRowView rowView in dataView)
            {

                if (nextNumber < dataView.Count && rowView["PROCESS_CODE"].IsNotNullOrEmpty() &&
                    rowView["PROCESS_CODE"].ToValueAsString().IsNotNullOrEmpty() &&
                    (rowView["PROCESS_CODE"].ToValueAsString().ToIntValue() == CONST_RAW_MATERIAL_PROCESS_SNO ||
                    rowView["PROCESS_CODE"].ToValueAsString().ToIntValue() == CONST_DESPATCH_PROCESS_SNO ||
                    rowView["PROCESS_CODE"].ToValueAsString().ToIntValue() == CONST_FINISH_PROCESS_SNO) && isNotManualReOrder)
                {
                    rowView.BeginEdit();
                    rowView[fieldName] = rowView["PROCESS_CODE"].ToValueAsString().ToIntValue();
                    rowView.EndEdit();
                }
                else
                {
                    if (nextNumber < dataView.Count && rowView[fieldName].IsNotNullOrEmpty() &&
                    rowView[fieldName].ToValueAsString().IsNotNullOrEmpty() &&
                    !(rowView[fieldName].ToValueAsString().ToIntValue() >= CONST_MAX_REORDER_SNO &&
                    rowView[fieldName].ToValueAsString().ToIntValue() != 0))
                    {
                        rowView.BeginEdit();
                        nextNumber++;
                        rowView[fieldName] = nextNumber * multipleBy;
                        rowView.EndEdit();
                    }
                }

                if (!(rowView["SNO"].ToValueAsString().Length == 0 && rowView["OPERATION_NO"].ToValueAsString().Length == 0 && rowView["OPERATION"].ToValueAsString().Length == 0 && rowView["COST_CENT_CODE"].ToValueAsString().Length == 0))
                {
                    rowView.BeginEdit();
                    rowView[newFieldName] = ++rowIndex;
                    rowView.EndEdit();
                }
                else
                {
                    if (lastRowIndex == nextNumber) { --lastRowIndex; }
                    rowView.BeginEdit();
                    rowView[newFieldName] = lastRowIndex--;
                    rowView.EndEdit();
                }

            }

            lastRowIndex = 99999999;
            rowIndex = 0;
            nextNumber = 0;

            dataView.Sort = newFieldName + " ASC";

            foreach (DataRowView rowView in dataView)
            {
                if (!(rowView["SNO"].ToValueAsString().Length == 0 && rowView["OPERATION_NO"].ToValueAsString().Length == 0 && rowView["OPERATION"].ToValueAsString().Length == 0 && rowView["COST_CENT_CODE"].ToValueAsString().Length == 0))
                {
                    rowView["PROCESS_CODE"] = Math.Round(rowView["SNO"].ToValueAsString().ToDoubleValue(), 0);
                }
                else
                {
                    if (lastRowIndex == nextNumber) { --lastRowIndex; }
                    rowView.BeginEdit();
                    rowView["PROCESS_CODE"] = lastRowIndex;
                    rowView.EndEdit();
                }
            }

            dataView.Sort = "PROCESS_CODE ASC";

            foreach (DataRowView rowView in dataView)
            {

                if (nextNumber < dataView.Count && rowView["PROCESS_CODE"].IsNotNullOrEmpty() &&
                    rowView["PROCESS_CODE"].ToValueAsString().IsNotNullOrEmpty() &&
                    (rowView["PROCESS_CODE"].ToValueAsString().ToIntValue() == CONST_RAW_MATERIAL_PROCESS_SNO ||
                    rowView["PROCESS_CODE"].ToValueAsString().ToIntValue() == CONST_DESPATCH_PROCESS_SNO ||
                    rowView["PROCESS_CODE"].ToValueAsString().ToIntValue() == CONST_FINISH_PROCESS_SNO) && isNotManualReOrder)
                {
                    rowView.BeginEdit();
                    rowView[fieldName] = rowView["PROCESS_CODE"].ToValueAsString().ToIntValue();
                    rowView.EndEdit();
                }
                else
                {
                    if (nextNumber < dataView.Count && rowView[fieldName].IsNotNullOrEmpty() &&
                    rowView[fieldName].ToValueAsString().IsNotNullOrEmpty() &&
                    !(rowView[fieldName].ToValueAsString().ToIntValue() >= CONST_MAX_REORDER_SNO &&
                    rowView[fieldName].ToValueAsString().ToIntValue() != 0))
                    {
                        rowView.BeginEdit();
                        nextNumber++;
                        rowView[fieldName] = nextNumber * multipleBy;
                        rowView.EndEdit();
                    }
                }

                if (!(rowView["SNO"].ToValueAsString().Length == 0 && rowView["OPERATION_NO"].ToValueAsString().Length == 0 && rowView["OPERATION"].ToValueAsString().Length == 0 && rowView["COST_CENT_CODE"].ToValueAsString().Length == 0))
                {
                    rowView.BeginEdit();
                    rowView[newFieldName] = ++rowIndex;
                    rowView.EndEdit();
                }
                else
                {
                    if (lastRowIndex == nextNumber) { --lastRowIndex; }
                    rowView.BeginEdit();
                    rowView[newFieldName] = lastRowIndex--;
                    rowView.EndEdit();
                }

            }

        }

        //private void ReNumber(ref DataView dataView, string fieldName, string orderBy = "ASC", int multipleBy = 10, bool isNotManualReOrder = true)
        //{
        //    if (!CostDetails.IsNotNullOrEmpty()) return;
        //    if (!fieldName.IsNotNullOrEmpty()) fieldName = "SNO";
        //    if (!orderBy.IsNotNullOrEmpty()) orderBy = "ASC";
        //    if (multipleBy <= 0) multipleBy = 10;


        //    DataTable dt = dataView.Table;
        //    string newFieldName = "SNO_SEQUENCE";

        //    DataTable costDetailsCopy = CostDetails.Table.Copy();
        //    if (costDetailsCopy.Rows.Count > 0)
        //    {
        //        int maxRowNumber = costDetailsCopy.Compute("Max(" + newFieldName + ")", null).ToValueAsString().ToIntValue();

        //    }

        //    //if (!dt.Columns.Contains(newFieldName))
        //    //{
        //    //    dt.Columns.Add(newFieldName, typeof(int));
        //    //}

        //    int nextNumber = 1000000;

        //    ////Copy FieldName values into newFieldName, Assing Empty Rows as decreased 10000 no.
        //    //dataView.RowFilter = null;
        //    //dataView.Sort = null;
        //    //foreach (DataRowView rowView in dataView)
        //    //{
        //    //    rowView[newFieldName] = rowView[fieldName].ToValueAsString().ToIntValue();
        //    //    if (!rowView[fieldName].ToValueAsString().IsNotNullOrEmpty())
        //    //    {
        //    //        rowView[newFieldName] = --nextNumber;
        //    //        rowView.EndEdit();
        //    //    }
        //    //}
        //    int lastRowIndex = 99999999;
        //    int rowIndex = 0;
        //    nextNumber = 0;
        //    dataView.Sort = null;
        //    //dataView.Sort = newFieldName + " ASC";
        //    dataView.Sort = newFieldName + " ASC";

        //    foreach (DataRowView rowView in dataView)
        //    {
        //        //rowView.BeginEdit();
        //        //rowView[fieldName] = (((++nextNumber) % 10) + 1) * multipleBy;
        //        //rowView.EndEdit();

        //        //if (nextNumber < dataView.Count && rowView[fieldName].IsNotNullOrEmpty() &&
        //        //    rowView[fieldName].ToValueAsString().IsNotNullOrEmpty() &&
        //        //    rowView[fieldName].ToValueAsString().ToIntValue() != CONST_RAW_MATERIAL_PROCESS_SNO &&
        //        //    rowView[fieldName].ToValueAsString().ToIntValue() != CONST_DESPATCH_PROCESS_SNO &&
        //        //    rowView[fieldName].ToValueAsString().ToIntValue() != CONST_FINISH_PROCESS_SNO &&
        //        //    !(rowView[fieldName].ToValueAsString().ToIntValue() >= CONST_MAX_REORDER_SNO &&
        //        //    rowView[fieldName].ToValueAsString().ToIntValue() != 0))
        //        //{
        //        //    rowView.BeginEdit();
        //        //    rowView[fieldName] = (((nextNumber++) % 10) + 1) * multipleBy;
        //        //    rowView.EndEdit();
        //        //}


        //        if (nextNumber < dataView.Count && rowView["PROCESS_CODE"].IsNotNullOrEmpty() &&
        //            rowView["PROCESS_CODE"].ToValueAsString().IsNotNullOrEmpty() &&
        //            (rowView["PROCESS_CODE"].ToValueAsString().ToIntValue() == CONST_RAW_MATERIAL_PROCESS_SNO ||
        //            rowView["PROCESS_CODE"].ToValueAsString().ToIntValue() == CONST_DESPATCH_PROCESS_SNO ||
        //            rowView["PROCESS_CODE"].ToValueAsString().ToIntValue() == CONST_FINISH_PROCESS_SNO) && isNotManualReOrder)
        //        {
        //            rowView.BeginEdit();
        //            rowView[fieldName] = rowView["PROCESS_CODE"].ToValueAsString().ToIntValue();
        //            rowView.EndEdit();
        //        }
        //        else
        //        {
        //            if (nextNumber < dataView.Count && rowView[fieldName].IsNotNullOrEmpty() &&
        //            rowView[fieldName].ToValueAsString().IsNotNullOrEmpty() &&
        //            !(rowView[fieldName].ToValueAsString().ToIntValue() >= CONST_MAX_REORDER_SNO &&
        //            rowView[fieldName].ToValueAsString().ToIntValue() != 0))
        //            {
        //                rowView.BeginEdit();
        //                nextNumber++;
        //                rowView[fieldName] = nextNumber * multipleBy;
        //                //rowView[fieldName] = (((nextNumber) % multipleBy) + 1) * multipleBy;
        //                //rowView[newFieldName] = (nextNumber) * multipleBy;
        //                rowView.EndEdit();
        //            }
        //        }

        //        if (!(rowView["SNO"].ToValueAsString().Length == 0 && rowView["OPERATION_NO"].ToValueAsString().Length == 0 && rowView["OPERATION"].ToValueAsString().Length == 0 && rowView["COST_CENT_CODE"].ToValueAsString().Length == 0))
        //        {
        //            rowView.BeginEdit();
        //            rowView[newFieldName] = ++rowIndex;
        //            rowView.EndEdit();
        //        }
        //        else
        //        {
        //            if (lastRowIndex == nextNumber) { --lastRowIndex; }
        //            rowView.BeginEdit();
        //            rowView[newFieldName] = lastRowIndex--;
        //            rowView.EndEdit();
        //        }

        //    }

        //    //nextNumber = 0;
        //    //dataView.Sort = null;
        //    //dataView.Sort = newFieldName + " ASC";
        //    //foreach (DataRowView rowView in dataView)
        //    //{
        //    //    if (nextNumber < dataView.Count && rowView["PROCESS_CODE"].IsNotNullOrEmpty() &&
        //    //        rowView["PROCESS_CODE"].ToValueAsString().IsNotNullOrEmpty() &&
        //    //        (rowView["PROCESS_CODE"].ToValueAsString().ToIntValue() == CONST_RAW_MATERIAL_PROCESS_SNO ||
        //    //        rowView["PROCESS_CODE"].ToValueAsString().ToIntValue() == CONST_DESPATCH_PROCESS_SNO ||
        //    //        rowView["PROCESS_CODE"].ToValueAsString().ToIntValue() == CONST_FINISH_PROCESS_SNO) && isNotManualReOrder)
        //    //    {
        //    //        rowView.BeginEdit();
        //    //        rowView[fieldName] = rowView["PROCESS_CODE"].ToValueAsString().ToIntValue();
        //    //        rowView.EndEdit();
        //    //    }
        //    //    else
        //    //    {
        //    //        if (nextNumber < dataView.Count && rowView[fieldName].IsNotNullOrEmpty() &&
        //    //        rowView[fieldName].ToValueAsString().IsNotNullOrEmpty() &&
        //    //        !(rowView[fieldName].ToValueAsString().ToIntValue() >= CONST_MAX_REORDER_SNO &&
        //    //        rowView[fieldName].ToValueAsString().ToIntValue() != 0))
        //    //        {
        //    //            rowView.BeginEdit();
        //    //            rowView[fieldName] = ++nextNumber * multipleBy;
        //    //            rowView.EndEdit();
        //    //        }
        //    //    }

        //    //}

        //}

        private Visibility _rejectReasonsVisibility = Visibility.Collapsed;
        public Visibility RejectReasonsVisibility
        {
            get
            {
                return _rejectReasonsVisibility;
            }
            set
            {
                _rejectReasonsVisibility = value;
                NotifyPropertyChanged("RejectReasonsVisibility");
            }
        }

        private List<DDRM_MAST> _rawMaterialEntities = null;
        public List<DDRM_MAST> RawMaterialEntities
        {
            get
            {
                return _rawMaterialEntities;
            }
            set
            {
                _rawMaterialEntities = value;
                NotifyPropertyChanged("RawMaterialEntities");
            }
        }

        private List<DDCOST_PROCESS_DATA> _costDetailEntities = null;
        public List<DDCOST_PROCESS_DATA> CostDetailEntities
        {
            get
            {
                return _costDetailEntities;
            }
            set
            {
                _costDetailEntities = value;
                DataTable dt = _costDetailEntities.ToDataTable<DDCOST_PROCESS_DATA>();
                if (dt.Columns.Contains("DDCI_INFO")) dt.Columns.Remove("DDCI_INFO");
                if (!dt.Columns.Contains("CCCode")) dt.Columns.Add("CCCode");
                if (!dt.Columns.Contains("CCOutput")) dt.Columns.Add("CCOutput");
                if (!dt.Columns.Contains("PROCESS_CODE_NEW")) dt.Columns.Add("PROCESS_CODE_NEW", typeof(decimal));

                string newFieldName = "SNO_SEQUENCE";
                if (!dt.Columns.Contains(newFieldName))
                {
                    dt.Columns.Add(newFieldName, typeof(int));
                }
                int rowIndex = 0;
                foreach (DataRow row in dt.Rows)
                {
                    row.BeginEdit();
                    row[newFieldName] = ++rowIndex;
                    row["CCCode"] = row["COST_CENT_CODE"];
                    row["CCOutput"] = row["OUTPUT"];
                    row["PROCESS_CODE_NEW"] = row["PROCESS_CODE"].ToValueAsString().ToDoubleValue();

                    row.EndEdit();
                }

                if (dt.Columns.Contains("PROCESS_CODE"))
                {
                    dt.Columns.Remove("PROCESS_CODE");
                    dt.Columns["PROCESS_CODE_NEW"].ColumnName = "PROCESS_CODE";
                }

                //dt.AcceptChanges();

                //if (dt.Columns.Contains("SNO"))
                //{
                //    string newFieldName = "SNO_INT";
                //    dt.Columns.Add(newFieldName, typeof(decimal));
                //    foreach (DataRow row in dt.Rows)
                //    {
                //        row.BeginEdit();
                //        row[newFieldName] = row["SNO"];
                //        row["CCCode"] = row["COST_CENT_CODE"];
                //        row["CCOutput"] = row["OUTPUT"];

                //        row.EndEdit();
                //    }
                //    dt.Columns.Remove("SNO");
                //    dt.Columns[newFieldName].ColumnName = "SNO";
                //    dt.AcceptChanges();
                //}


                CostDetails = dt.DefaultView;

                if (CostDetails.IsNotNullOrEmpty() && CostDetails.Count == 0 || canAddNewCostDetails())
                {
                    CostDetailsInsertRow(null);
                }
                CostDetailsSelectedRow = CostDetails[0];
                //CostCalculation();
                NotifyPropertyChanged("CostDetailEntities");
                originalChildEntityDataTable = dt.Copy();
            }
        }

        private readonly ICommand cheeseWeightLostFocusCommand = null;
        public ICommand CheeseWeightLostFocusCommand { get { return this.cheeseWeightLostFocusCommand; } }
        private void CheeseWeightLostFocus()
        {
            FinishWeightLostFocus();
        }

        private readonly ICommand finishWeightLostFocusCommand = null;
        public ICommand FinishWeightLostFocusCommand { get { return this.finishWeightLostFocusCommand; } }
        private void FinishWeightLostFocus()
        {
            if (!MandatoryFields.CHEESE_WT.IsNotNullOrEmpty() || !MandatoryFields.FINISH_WT.IsNotNullOrEmpty()) return;

            copyMandatoryFieldsToEntity(MandatoryFields);
            if (ActiveEntity.FINISH_WT.ToValueAsString().ToDecimalValue() > ActiveEntity.CHEESE_WT.ToValueAsString().ToDecimalValue() &&
                !ActiveEntity.IS_COMBINED.ToValueAsString().ToBooleanAsString())
            {
                ShowInformationMessage("Finish Weight is greater than Cheese Weight.\r\nPlease make sure it is correct.");
            }

            WeightCalculation();
            CostCalculation();
        }


        private readonly ICommand finishWeightMouseDoubleClickCommand = null;
        public ICommand FinishWeightMouseDoubleClickCommand { get { return this.finishWeightMouseDoubleClickCommand; } }
        private void FinishWeightMouseDoubleClick(string wtOption)
        {
            if (ActiveEntity.CI_REFERENCE.IsNotNullOrEmpty())
            {
                MdiChild childProductwt;

                childProductwt = (MdiChild)MainMDI.GetFormAlreadyOpened("Product Weight Calculator");
                if (childProductwt != null) MainMDI.Container.Children.Remove(childProductwt);

                childProductwt = new MdiChild();
                frmProductWeight frmProductWeight = new frmProductWeight(childProductwt, _userInformation, ActiveEntity.CI_REFERENCE, wtOption, ActionMode, ActiveEntity.IDPK);
                childProductwt.Title = ApplicationTitle + " - Product Weight Calculator";
                childProductwt.Content = frmProductWeight;
                childProductwt.Height = frmProductWeight.Height + 40;
                childProductwt.Width = frmProductWeight.Width + 20;
                childProductwt.MinimizeBox = false;
                childProductwt.MaximizeBox = false;
                childProductwt.Resizable = false;
                frmProductWeight.Unloaded += childProductwt_Closing;
                MainMDI.Container.Children.Add(childProductwt);

            }
            copyMandatoryFieldsToEntity(MandatoryFields);
        }

        void childProductwt_Closing(object sender, RoutedEventArgs e)
        {
            frmProductWeight frmProductWeight = sender as frmProductWeight;
            if (frmProductWeight != null && frmProductWeight.IsSelected)
            {
                if (frmProductWeight.WTOption == "F")
                {
                    FINISH_WT = frmProductWeight.txtCheeseWeight.Text;
                }
                else if (frmProductWeight.WTOption == "C")
                {
                    CHEESE_WT = frmProductWeight.txtCheeseWeight.Text;
                }
            }
        }


        private readonly ICommand sflShareLostFocusCommand = null;
        public ICommand SFLShareLostFocusCommand { get { return this.sflShareLostFocusCommand; } }
        private void SFLShareLostFocus()
        {
            ActiveEntity.SFL_SHARE = MandatoryFields.SFL_SHARE.ToDecimalValue();
            if (ActiveEntity.SFL_SHARE.ToValueAsString().ToDecimalValue() > 100)
            {
                ShowInformationMessage("SFL Share Should not exceed 100");
                return;
            }

        }

        private readonly ICommand rmFactorLostFocusCommand = null;
        public ICommand RMFactorLostFocusCommand { get { return this.rmFactorLostFocusCommand; } }
        private void RMFactorLostFocus()
        {
            copyMandatoryFieldsToEntity(MandatoryFields);
            if (ActiveEntity.RM_FACTOR.IsNotNullOrEmpty())
            {
                if (ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue() < 1.00m || ActiveEntity.RM_FACTOR.ToValueAsString().ToDecimalValue() > 1.15m)
                {
                    ShowInformationMessage("Multiplication Factor should be between 1.00 and 1.15");
                    MandatoryFields.RM_FACTOR = 1;
                    return;
                }

                FinishWeightLostFocus();
            }
        }

        private readonly ICommand notesMouseDoubleClickCommand = null;
        public ICommand NotesMouseDoubleClickCommand { get { return this.notesMouseDoubleClickCommand; } }
        private void NotesMouseDoubleClick()
        {
            CostDetailsVisibility = Visibility.Collapsed;
            StandatedNotesVisibility = Visibility.Visible;

            //fraCostDetails.Visible = False
            //fraStandard.Visible = True
            //ssStandardNotes.Visible = False
            //cmdSave.Visible = False
            //txtCostNotes.Visible = True
            //txtCostNotes.text = txtNotes.text
            //cmdInclude.Visible = False
            //cmdClose.Visible = False         
        }

        private readonly ICommand costSheetSearchClickCommand;
        public ICommand CostSheetSearchClickCommand { get { return this.costSheetSearchClickCommand; } }
        private void CostSheetSearchClick()
        {
            //frmCostSheetSearch frmCostSheetSearch = new frmCostSheetSearch(_userInformation, mdiChild, ActiveEntity.IDPK, OperationMode.View, "frmFRCS");
            //if (self.IsNotNullOrEmpty())
            //    frmCostSheetSearch.Owner = self;

            //bool? dialogResult = frmCostSheetSearch.ShowDialog();
            //if (dialogResult.ToBooleanAsString())
            //{
            //    //ActionMode = OperationMode.Edit;
            //}

            MdiChild mdiCostSheetSearch = new MdiChild();
            if (MainMDI.IsFormAlreadyOpen("Cost Sheet Search") == false)
            {
                frmCostSheetSearch frmCostSheetSearch = new frmCostSheetSearch(_userInformation, mdiCostSheetSearch, ActiveEntity.IDPK, OperationMode.View, "frmFRCS");
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


        }

        private readonly ICommand productSearchClickCommand;
        public ICommand ProductSearchClickCommand { get { return this.productSearchClickCommand; } }
        private void ProductSearchClick()
        {
            //frmProductSearch frmProductSearch = new frmProductSearch(_userInformation);
            //frmProductSearch.ShowDialog();
            try
            {
                MdiChild mdiProductSearch = new MdiChild();
                ProcessDesigner.frmProductSearch productSearch = new frmProductSearch(_userInformation, mdiProductSearch);
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
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand _costDetailsBeginningEditCommand;
        public ICommand CostDetailsBeginningEditCommand { get { return this._costDetailsBeginningEditCommand; } }

        //private void costDetailsBeginningEdit(Object currentCell)
        //{
        //    try
        //    {
        //        Microsoft.Windows.Controls.DataGridCellInfo cellInfo = (Microsoft.Windows.Controls.DataGridCellInfo)currentCell;
        //        switch (cellInfo.Column.Header.ToValueAsString().ToUpper())
        //        {
        //            case "CC CODE":

        //                string operationCode = CostDetailsSelectedRow["OprCode"].ToValueAsString().Trim();
        //                if (!operationCode.IsNotNullOrEmpty())
        //                    operationCode = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString().Trim();

        //                if (operationCode.IsNotNullOrEmpty())
        //                    OperationCostDataSource.RowFilter = "OPERATION_NO In('" + operationCode + "')";

        //                break;
        //            case "OUTPUT":

        //                string cost_cent_code = CostDetailsSelectedRow["CCCode"].ToValueAsString().Trim();
        //                if (!cost_cent_code.IsNotNullOrEmpty())
        //                    cost_cent_code = CostDetailsSelectedRow["COST_CENT_CODE"].ToValueAsString().Trim();

        //                if (cost_cent_code.IsNotNullOrEmpty())
        //                    CostCentreOutputDataSource.RowFilter = "COST_CENT_CODE In('" + cost_cent_code + "')";

        //                break;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        ex.LogException();
        //    }
        //    //OperationDataSource.RowFilter = null;
        //    //OperationCostDataSource.RowFilter = null;
        //    //CostCentreOutputDataSource.RowFilter = null;

        //    //try
        //    //{
        //    //    if (!currentCell.IsNotNullOrEmpty()) return;
        //    //    CommonProcessCost activeChildEntity = null;
        //    //    //BHCustCtrl.CustComboBox comboBox = null;
        //    //    //System.Windows.Controls.TextBox textBox = null;
        //    //    Microsoft.Windows.Controls.DataGridCellInfo cellInfo = (Microsoft.Windows.Controls.DataGridCellInfo)currentCell;
        //    //    if (!cellInfo.Column.Header.IsNotNullOrEmpty()) return;

        //    //    if (CostDetailsSelectedRow.IsNotNullOrEmpty())
        //    //    {
        //    //        CostDetailsSelectedRow["CI_INFO_FK"] = ActiveEntity.IDPK;
        //    //        //CostDetailsSelectedRow["CI_REFERENCE"] = ActiveEntity.CI_REFERENCE;
        //    //    }

        //    //    object control = cellInfo.Column.GetCellContent(cellInfo.Item);
        //    //    DataView costCenterDataView = null;

        //    //    switch (cellInfo.Column.Header.ToValueAsString().ToUpper())
        //    //    {
        //    //        case "SNO":
        //    //            if (CostDetailsSelectedRow.IsNotNullOrEmpty() && CostDetailsSelectedRow["SNO"].ToValueAsString() == "0")
        //    //            {
        //    //                CostDetailsSelectedRow["SNO"] = string.Empty;
        //    //                CostDetailsSelectedRow.EndEdit();
        //    //            }
        //    //            break;
        //    //        case "OPERATION CODE":
        //    //            if (CostDetailsSelectedRow.IsNotNullOrEmpty())
        //    //            {
        //    //                CostDetailsSelectedRow["OPERATION"] = string.Empty;
        //    //                CostDetailsSelectedRow.EndEdit();
        //    //            }


        //    //            break;
        //    //        case "OPERATION": break;
        //    //        case "CC CODE":

        //    //            activeChildEntity = new CommonProcessCost();

        //    //            if (CostDetailsSelectedRow.IsNotNullOrEmpty())
        //    //            {
        //    //                activeChildEntity.SNO = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDecimalValue();
        //    //                activeChildEntity.CODE = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();
        //    //                if (!CostDetailsSelectedRow["OPERATION_NO"].IsNotNullOrEmpty() && CostDetailsSelectedRow["OprCode"].IsNotNullOrEmpty())
        //    //                    activeChildEntity.CODE = CostDetailsSelectedRow["OprCode"].ToValueAsString();

        //    //                activeChildEntity.DESCRIPTION = CostDetailsSelectedRow["OPERATION"].ToValueAsString();
        //    //            }

        //    //            if (!activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() && CostDetailsSelectedRow["CCCode"].ToValueAsString().IsNotNullOrEmpty()) activeChildEntity.COST_CENT_CODE = CostDetailsSelectedRow["CCCode"].ToValueAsString();

        //    //            if (!activeChildEntity.CODE.IsNotNullOrEmpty() && CostDetailsSelectedRow["OprCode"].ToValueAsString().IsNotNullOrEmpty()) activeChildEntity.CODE = CostDetailsSelectedRow["OprCode"].ToValueAsString();

        //    //            costCenterDataView = OperationCostDataSource;

        //    //            if (activeChildEntity.IsNotNullOrEmpty() && costCenterDataView.IsNotNullOrEmpty())
        //    //                costCenterDataView.RowFilter = "OPERATION_NO In('" + activeChildEntity.CODE.ToValueAsString().Trim() + "')";

        //    //            if (!activeChildEntity.CODE.IsNotNullOrEmpty())
        //    //            {
        //    //                ShowInformationMessage(PDMsg.NotEmpty("Operation Code"));
        //    //                return;
        //    //            }

        //    //            //if (costCenterDataView.Count == 0) costCenterDataView.RowFilter = null;

        //    //            break;
        //    //        case "OUTPUT":

        //    //            activeChildEntity = new CommonProcessCost();

        //    //            if (CostDetailsSelectedRow.IsNotNullOrEmpty())
        //    //            {
        //    //                activeChildEntity.SNO = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDecimalValue();
        //    //                activeChildEntity.CODE = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();
        //    //                activeChildEntity.DESCRIPTION = CostDetailsSelectedRow["OPERATION"].ToValueAsString();
        //    //                activeChildEntity.COST_CENT_CODE = CostDetailsSelectedRow["COST_CENT_CODE"].ToValueAsString();
        //    //            }

        //    //            if (!activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() && CostDetailsSelectedRow["CCCode"].ToValueAsString().IsNotNullOrEmpty()) activeChildEntity.COST_CENT_CODE = CostDetailsSelectedRow["CCCode"].ToValueAsString();

        //    //            if (!activeChildEntity.CODE.IsNotNullOrEmpty() && CostDetailsSelectedRow["OprCode"].ToValueAsString().IsNotNullOrEmpty()) activeChildEntity.CODE = CostDetailsSelectedRow["OprCode"].ToValueAsString();

        //    //            CostCentreOutputDataSource.RowFilter = null;
        //    //            if (activeChildEntity.IsNotNullOrEmpty() && CostCentreOutputDataSource.IsNotNullOrEmpty())
        //    //                CostCentreOutputDataSource.RowFilter = "COST_CENT_CODE In('" + activeChildEntity.COST_CENT_CODE.ToValueAsString().Trim() + "')";
        //    //            //if (CostCentreOutputDataSource.Count == 0) CostCentreOutputDataSource.RowFilter = null;

        //    //            if (CostCentreOutputDataSource.Count == 0)
        //    //            {
        //    //                CostDetailsSelectedRow["OUTPUT"] = "0";
        //    //                CostDetailsSelectedRow["CCOutput"] = CostDetailsSelectedRow["OUTPUT"].ToValueAsString();
        //    //                CostDetailsSelectedRow["OprCode"] = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();

        //    //            }

        //    //            if (CostCentreOutputDataSource.Count == 1)
        //    //            {
        //    //                CostDetailsSelectedRow["OUTPUT"] = CostCentreOutputDataSource[0]["OUTPUT"].ToValueAsString();
        //    //                CostDetailsSelectedRow["CCOutput"] = CostDetailsSelectedRow["OUTPUT"].ToValueAsString();
        //    //                CostDetailsSelectedRow["OprCode"] = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();
        //    //            }

        //    //            if (CostDetailsSelectedRow.IsNotNullOrEmpty()) CostDetailsSelectedRow.EndEdit();

        //    //            break;
        //    //        case "VAR COST":
        //    //            break;
        //    //        case "FIXED COST":
        //    //            break;
        //    //        case "SPL COST":
        //    //            break;
        //    //        case "OPERATION COST":
        //    //            break;
        //    //    }
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw ex.LogException();
        //    //}

        //}
        Microsoft.Windows.Controls.DataGridCellInfo cellInfo;
        private void costDetailsBeginningEdit(Object currentCell)
        {
            OperationDataSource.RowFilter = null;
            OperationCostDataSource.RowFilter = null;
            DVOperationCost = OperationCostDataSource.ToTable().Copy().DefaultView;
            CostCentreOutputDataSource.RowFilter = null;
            DVCostCentreOutput = CostCentreOutputDataSource.ToTable().Copy().DefaultView;

            try
            {
                if (!currentCell.IsNotNullOrEmpty()) return;
                CommonProcessCost activeChildEntity = null;
                //BHCustCtrl.CustComboBox comboBox = null;
                //System.Windows.Controls.TextBox textBox = null;
                cellInfo = (Microsoft.Windows.Controls.DataGridCellInfo)currentCell;
                if (!cellInfo.Column.Header.IsNotNullOrEmpty()) return;

                if (CostDetailsSelectedRow.IsNotNullOrEmpty())
                {
                    CostDetailsSelectedRow["CI_INFO_FK"] = ActiveEntity.IDPK;
                    //CostDetailsSelectedRow["CI_REFERENCE"] = ActiveEntity.CI_REFERENCE;
                }

                object control = cellInfo.Column.GetCellContent(cellInfo.Item);
                DataView costCenterDataView = null;

                switch (cellInfo.Column.Header.ToValueAsString().ToUpper())
                {
                    case "SNO":
                        if (CostDetailsSelectedRow.IsNotNullOrEmpty() && CostDetailsSelectedRow["SNO"].ToValueAsString() == "0")
                        {
                            CostDetailsSelectedRow["SNO"] = string.Empty;
                            CostDetailsSelectedRow.EndEdit();
                        }

                        break;
                    case "OPERATION CODE":
                        if (CostDetailsSelectedRow.IsNotNullOrEmpty())
                        {
                            //CostDetailsSelectedRow["OPERATION"] = string.Empty;
                            // CostDetailsSelectedRow.EndEdit();

                        }


                        break;
                    case "OPERATION": break;
                    case "CC CODE":

                        activeChildEntity = new CommonProcessCost();

                        if (CostDetailsSelectedRow.IsNotNullOrEmpty())
                        {
                            activeChildEntity.SNO = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDecimalValue();
                            activeChildEntity.CODE = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();

                            activeChildEntity.DESCRIPTION = CostDetailsSelectedRow["OPERATION"].ToValueAsString();
                        }

                        if (!activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() && CostDetailsSelectedRow["CCCode"].ToValueAsString().IsNotNullOrEmpty()) activeChildEntity.COST_CENT_CODE = CostDetailsSelectedRow["CCCode"].ToValueAsString();

                        costCenterDataView = OperationCostDataSource;

                        if (activeChildEntity.IsNotNullOrEmpty() && costCenterDataView.IsNotNullOrEmpty())
                        {
                            costCenterDataView.RowFilter = "OPERATION_NO In('" + activeChildEntity.CODE.ToValueAsString().Trim().FormatEscapeChars() + "')";
                            DVOperationCost = OperationCostDataSource.ToTable().Copy().DefaultView;
                        }

                        if (!activeChildEntity.CODE.IsNotNullOrEmpty())
                        {
                            ShowInformationMessage(PDMsg.NotEmpty("Operation Code"));
                            return;
                        }

                        //if (costCenterDataView.Count == 0) costCenterDataView.RowFilter = null;

                        break;
                    case "OUTPUT":

                        activeChildEntity = new CommonProcessCost();

                        if (CostDetailsSelectedRow.IsNotNullOrEmpty())
                        {
                            activeChildEntity.SNO = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDecimalValue();
                            activeChildEntity.CODE = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();
                            activeChildEntity.DESCRIPTION = CostDetailsSelectedRow["OPERATION"].ToValueAsString();
                            activeChildEntity.COST_CENT_CODE = CostDetailsSelectedRow["COST_CENT_CODE"].ToValueAsString();
                        }

                        if (!activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() && CostDetailsSelectedRow["CCCode"].ToValueAsString().IsNotNullOrEmpty()) activeChildEntity.COST_CENT_CODE = CostDetailsSelectedRow["CCCode"].ToValueAsString();


                        CostCentreOutputDataSource.RowFilter = null;
                        if (activeChildEntity.IsNotNullOrEmpty() && CostCentreOutputDataSource.IsNotNullOrEmpty())
                        {
                            CostCentreOutputDataSource.RowFilter = "COST_CENT_CODE In('" + activeChildEntity.COST_CENT_CODE.ToValueAsString().Trim().FormatEscapeChars() + "')";
                            DVCostCentreOutput = CostCentreOutputDataSource.ToTable().Copy().DefaultView;
                        }
                        //if (CostCentreOutputDataSource.Count == 0) CostCentreOutputDataSource.RowFilter = null;

                        if (CostCentreOutputDataSource.Count == 0)
                        {
                            CostDetailsSelectedRow["OUTPUT"] = "0";
                            CostDetailsSelectedRow["CCOutput"] = CostDetailsSelectedRow["OUTPUT"].ToValueAsString();

                        }

                        if (CostCentreOutputDataSource.Count == 1)
                        {
                            CostDetailsSelectedRow["OUTPUT"] = CostCentreOutputDataSource[0]["OUTPUT"].ToValueAsString();
                            CostDetailsSelectedRow["CCOutput"] = CostDetailsSelectedRow["OUTPUT"].ToValueAsString();
                        }

                        if (CostDetailsSelectedRow.IsNotNullOrEmpty()) CostDetailsSelectedRow.EndEdit();

                        break;
                    case "VAR COST":
                        break;
                    case "FIXED COST":
                        break;
                    case "SPL COST":
                        break;
                    case "OPERATION COST":
                        break;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private readonly ICommand _costDetailsCellEndEditCommand;
        public ICommand CostDetailsCellEndEditCommand { get { return this._costDetailsCellEndEditCommand; } }

        //private void CostDetailsCellEndEdit(Object currentCell)
        //{

        //}

        //private void CostDetailsCellEndEdit(Object currentCell)
        //{
        //    try
        //    {
        //        if (!currentCell.IsNotNullOrEmpty()) return;
        //        Microsoft.Windows.Controls.DataGridCellInfo cellInfo = (Microsoft.Windows.Controls.DataGridCellInfo)currentCell;
        //        if (!cellInfo.Column.Header.IsNotNullOrEmpty()) return;

        //        System.Windows.Controls.ContentPresenter contentPresenter = null;
        //        DataRowView currentCellDataRowView = null;
        //        CommonProcessCost activeChildEntity = null;
        //        System.Windows.Controls.TextBox textBox = null;

        //        object control = cellInfo.Column.GetCellContent(cellInfo.Item);

        //        switch (cellInfo.Column.Header.ToValueAsString().ToUpper())
        //        {
        //            case "OPERATION CODE":
        //                contentPresenter = control as System.Windows.Controls.ContentPresenter;
        //                if (contentPresenter.IsNotNullOrEmpty())
        //                    currentCellDataRowView = contentPresenter.Content as DataRowView;
        //                CostDetailsSelectedRow["OprCode"] = currentCellDataRowView["OPERATION_NO"];
        //                CostDetailsSelectedRow.EndEdit();
        //                break;
        //            case "CC CODE":
        //                contentPresenter = control as System.Windows.Controls.ContentPresenter;
        //                if (contentPresenter.IsNotNullOrEmpty())
        //                    currentCellDataRowView = contentPresenter.Content as DataRowView;
        //                CostDetailsSelectedRow["CCCode"] = currentCellDataRowView["COST_CENT_CODE"];
        //                CostDetailsSelectedRow.EndEdit();
        //                break;
        //            case "OUTPUT":

        //                contentPresenter = control as System.Windows.Controls.ContentPresenter;
        //                if (contentPresenter.IsNotNullOrEmpty())
        //                    currentCellDataRowView = contentPresenter.Content as DataRowView;
        //                CostDetailsSelectedRow["CCOutput"] = currentCellDataRowView["OUTPUT"];

        //                string operationCode = CostDetailsSelectedRow["OprCode"].ToValueAsString().Trim();
        //                if (!operationCode.IsNotNullOrEmpty())
        //                    operationCode = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString().Trim();

        //                string cost_cent_code = CostDetailsSelectedRow["CCCode"].ToValueAsString().Trim();
        //                if (!cost_cent_code.IsNotNullOrEmpty())
        //                    cost_cent_code = CostDetailsSelectedRow["COST_CENT_CODE"].ToValueAsString().Trim();

        //                DataView operationCostDataView = OperationCostDataSource.ToTable().Copy().DefaultView;
        //                operationCostDataView.RowFilter = "OPERATION_NO In('" + operationCode + "') AND " +
        //                "COST_CENT_CODE = '" + cost_cent_code + "'";

        //                DataRowView operationCostSelectedRow = null;

        //                if (operationCostDataView.Count == 1)
        //                {
        //                    operationCostSelectedRow = operationCostDataView[0];

        //                    CostDetailsSelectedRow["VAR_COST"] = operationCostSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue();
        //                    CostDetailsSelectedRow["FIX_COST"] = operationCostSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue();
        //                    CostDetailsSelectedRow["SPL_COST"] = operationCostSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue();
        //                    CostDetailsSelectedRow["TOTAL_COST"] = operationCostSelectedRow["TOTAL_COST"].ToValueAsString().ToDecimalValue();

        //                    CostDetailsSelectedRow["TOTAL_COST"] = Math.Round(operationCostSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue() +
        //                                           operationCostSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue() +
        //                                           operationCostSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue(), 2);


        //                    CostDetailsSelectedRow.EndEdit();
        //                }

        //                CostDetailsSelectedRow["VAR_COST"] = CostDetailsSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue();
        //                CostDetailsSelectedRow["FIX_COST"] = CostDetailsSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue();
        //                CostDetailsSelectedRow["SPL_COST"] = CostDetailsSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue();
        //                CostDetailsSelectedRow["TOTAL_COST"] = CostDetailsSelectedRow["TOTAL_COST"].ToValueAsString().ToDecimalValue();

        //                CostDetailsSelectedRow["TOTAL_COST"] = CostDetailsSelectedRow["TOTAL_COST"].ToValueAsString().ToDecimalValue();
        //                CostDetailsSelectedRow.EndEdit();

        //                activeChildEntity = new CommonProcessCost();
        //                activeChildEntity.SNO = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDecimalValue();
        //                activeChildEntity.CODE = operationCode;
        //                activeChildEntity.DESCRIPTION = CostDetailsSelectedRow["OPERATION"].ToValueAsString();
        //                activeChildEntity.COST_CENT_CODE = cost_cent_code;
        //                activeChildEntity.OUTPUT = CostDetailsSelectedRow["CCOutput"].ToValueAsString().ToDecimalValue();

        //                fnUpdateOutput(activeChildEntity);
        //                WeightCalculation(CostDetailsSelectedRow);
        //                CostCalculation();

        //                break;
        //            case "VAR COST":
        //                textBox = control as System.Windows.Controls.TextBox;
        //                if (textBox.IsNotNullOrEmpty() && CostDetailsSelectedRow.IsNotNullOrEmpty())
        //                {
        //                    if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0) { ShowInformationMessage("Invalid Number"); CostDetailsSelectedRow.CancelEdit(); }

        //                    CostDetailsSelectedRow["VAR_COST"] = textBox.Text.ToDecimalValue();
        //                    CostDetailsSelectedRow["TOTAL_COST"] = Math.Round(textBox.Text.ToDecimalValue() +
        //                                                                       CostDetailsSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue() +
        //                                                                       CostDetailsSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue(), 2);
        //                    //CostDetailsSelectedRow.EndEdit();
        //                }
        //                //CostCalculation();
        //                break;
        //            case "FIXED COST": textBox = control as System.Windows.Controls.TextBox;
        //                if (textBox.IsNotNullOrEmpty() && CostDetailsSelectedRow.IsNotNullOrEmpty())
        //                {
        //                    if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0) { ShowInformationMessage("Invalid Number"); CostDetailsSelectedRow.CancelEdit(); }
        //                    CostDetailsSelectedRow["FIX_COST"] = textBox.Text.ToDecimalValue();
        //                    CostDetailsSelectedRow["TOTAL_COST"] = Math.Round(CostDetailsSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue() +
        //                                                                       textBox.Text.ToDecimalValue() +
        //                                                                       CostDetailsSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue(), 2);
        //                    //CostDetailsSelectedRow.EndEdit();
        //                }
        //                //CostCalculation();
        //                break;
        //            case "SPL COST": textBox = control as System.Windows.Controls.TextBox;
        //                if (textBox.IsNotNullOrEmpty() && CostDetailsSelectedRow.IsNotNullOrEmpty())
        //                {
        //                    if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0)
        //                    { ShowInformationMessage("Invalid Number"); CostDetailsSelectedRow.CancelEdit(); }
        //                    CostDetailsSelectedRow["SPL_COST"] = textBox.Text.ToDecimalValue();
        //                    CostDetailsSelectedRow["TOTAL_COST"] = Math.Round(CostDetailsSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue() +
        //                                                                       CostDetailsSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue() +
        //                                                                       textBox.Text.ToDecimalValue(), 2);
        //                    CostDetailsSelectedRow.EndEdit();
        //                }
        //                CostCalculation();
        //                break;
        //            case "OPERATION COST":
        //                textBox = control as System.Windows.Controls.TextBox;
        //                if (textBox.IsNotNullOrEmpty() && CostDetailsSelectedRow.IsNotNullOrEmpty())
        //                {
        //                    if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0) { ShowInformationMessage("Invalid Number"); CostDetailsSelectedRow.CancelEdit(); }

        //                    CostDetailsSelectedRow["TOTAL_COST"] = textBox.Text.ToDecimalValue();
        //                    CostDetailsSelectedRow.EndEdit();
        //                    CostCalculation();
        //                }
        //                break;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.LogException();
        //    }

        //    if (canAddNewCostDetails())
        //        CostDetailsInsertRow(null);
        //}
        private static T GetVisualChild<T>(DependencyObject visual) where T : DependencyObject
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

        public void OnCellEditEndingCostDetails(object sender, Microsoft.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {

            if (e.Column.GetType() == typeof(Microsoft.Windows.Controls.DataGridTemplateColumn))
            {
                var popup = GetVisualChild<System.Windows.Controls.Primitives.Popup>(e.EditingElement);
                if (popup != null && popup.IsOpen)
                {
                    e.Cancel = true;
                    return;
                }
            }

            TextBox textBox = e.EditingElement as TextBox;
            CostDetailsSelectedRow = (System.Data.DataRowView)(e.Row.Item);
            string columnName = e.Column.SortMemberPath.ToUpper();

            CommonProcessCost activeChildEntity = null;
            string currentCellText = null;
            object oldCellValue = null;

            try
            {

                if (CostDetailsSelectedRow.IsNotNullOrEmpty())
                {
                    CostDetailsSelectedRow["CI_INFO_FK"] = ActiveEntity.IDPK;
                    if (CostDetailsSelectedRow.DataView.Table.Columns.Contains(columnName))
                        oldCellValue = CostDetailsSelectedRow[columnName];
                    //CostDetailsSelectedRow["CI_REFERENCE"] = ActiveEntity.CI_REFERENCE;
                }

                switch (columnName)
                {
                    case "SNO":
                        #region SNO Implementation
                        activeChildEntity = new CommonProcessCost();

                        if (textBox.Text.IsNotNullOrEmpty())
                        {
                            CostDetailsSelectedRow.BeginEdit();
                            CostDetailsSelectedRow["SNO"] = textBox.Text.ToDecimalValue();
                            //CostDetailsSelectedRow.EndEdit();

                            DataView costDetailsDataView = CostDetails.ToTable().Copy().DefaultView;
                            costDetailsDataView.RowFilter = "SNO = '" + textBox.Text.Trim().FormatEscapeChars() + "'";
                            if (costDetailsDataView.Count > 1)
                            {
                                ShowInformationMessage("SNo '" + textBox.Text + "' already exists!\r\nEnter New SNo");
                                CostDetailsSelectedRow["SNO"] = oldCellValue.ToValueAsString();
                                //CostDetailsSelectedRow.EndEdit();
                                costDetailsDataView.RowFilter = null;
                                return;
                            }
                            //CostDetailsSelectedRow.EndEdit();
                            costDetailsDataView.RowFilter = null;

                            if (CostDetailsSelectedRow["PROCESS_CODE"].ToValueAsString().ToIntValue() != CONST_RAW_MATERIAL_PROCESS_SNO &&
                               CostDetailsSelectedRow["PROCESS_CODE"].ToValueAsString().ToIntValue() != CONST_DESPATCH_PROCESS_SNO &&
                               CostDetailsSelectedRow["PROCESS_CODE"].ToValueAsString().ToIntValue() != CONST_FINISH_PROCESS_SNO)
                            {
                                CostDetailsSelectedRow["PROCESS_CODE"] = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDoubleValue();
                            }
                            CostDetailsSelectedRow.EndEdit();
                        }
                        break;
                        #endregion
                    case "OPERATION_NO":

                        activeChildEntity = new CommonProcessCost();
                        DataView operationDataView = null;
                        if (OperCode_SelectedItem.IsNotNullOrEmpty())
                        {
                            CostDetailsSelectedRow.BeginEdit();
                            activeChildEntity.SNO = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDecimalValue();

                            currentCellText = OperCode_SelectedItem["OPERATION_NO"].ToValueAsString();
                            activeChildEntity.CODE = currentCellText;

                            operationDataView = OperationDataSource.ToTable().Copy().DefaultView;
                            operationDataView.RowFilter = "OPERATION_NO In('" + activeChildEntity.CODE.ToValueAsString().Trim().FormatEscapeChars() + "')";
                            if (operationDataView.Count > 0)
                            {
                                activeChildEntity.DESCRIPTION = operationDataView[0].Row["OPERATION"].ToValueAsString();
                            }
                            operationDataView.RowFilter = null;

                            //if (!activeChildEntity.CODE.IsNotNullOrEmpty())
                            //{
                            //    ShowInformationMessage(PDMsg.NotEmpty("Operation Code"));
                            //    return;
                            //}


                            if (CostDetailsSelectedRow.IsNotNullOrEmpty())
                            {
                                CostDetailsSelectedRow["OPERATION_NO"] = activeChildEntity.CODE;
                                CostDetailsSelectedRow["OPERATION"] = activeChildEntity.DESCRIPTION;
                            }

                            DataView costCenterDataView = OperationCostDataSource;

                            if (activeChildEntity.IsNotNullOrEmpty() && activeChildEntity.CODE.IsNotNullOrEmpty() &&
                                costCenterDataView.IsNotNullOrEmpty())
                            {
                                costCenterDataView.RowFilter = "OPERATION_NO In('" + activeChildEntity.CODE.ToValueAsString().Trim().FormatEscapeChars() + "')";
                                DVOperationCost = OperationCostDataSource.ToTable().Copy().DefaultView;
                            }
                            //costCenterDataView.RowFilter = "OPERATION_NO In('" + activeChildEntity.CODE.ToValueAsString() + "') AND " +
                            //"LOC_CODE='" + ActiveEntity.LOC_CODE.ToValueAsString() + "'";

                            CostCentreOutputDataSource.RowFilter = null;
                            if (activeChildEntity.IsNotNullOrEmpty() && activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() && CostCentreOutputDataSource.IsNotNullOrEmpty())
                            {
                                CostCentreOutputDataSource.RowFilter = "COST_CENT_CODE In('" + activeChildEntity.COST_CENT_CODE.ToValueAsString().Trim().FormatEscapeChars() + "')";
                                DVCostCentreOutput = CostCentreOutputDataSource.ToTable().Copy().DefaultView;
                            }
                            CostDetailsSelectedRow.EndEdit();
                            fnOperationProcess(activeChildEntity);
                        }
                        break;
                    case "OPERATION": break;
                    case "COST_CENT_CODE":

                        activeChildEntity = new CommonProcessCost();

                        if (CCCode_SelectedItem.IsNotNullOrEmpty())
                        {
                            activeChildEntity.SNO = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDecimalValue();

                            activeChildEntity.CODE = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();
                            activeChildEntity.DESCRIPTION = CostDetailsSelectedRow["OPERATION"].ToValueAsString();

                            currentCellText = CCCode_SelectedItem["COST_CENT_CODE"].ToValueAsString();
                            if (!currentCellText.IsNotNullOrEmpty() && CCCode_SelectedItem["CCCode"].ToValueAsString().IsNotNullOrEmpty()) currentCellText = CCCode_SelectedItem["CCCode"].ToValueAsString();

                            if (!currentCellText.IsNotNullOrEmpty() && CCCode_SelectedItem["CCCode"].ToValueAsString().IsNotNullOrEmpty())
                                currentCellText = CCCode_SelectedItem["CCCode"].ToValueAsString();

                            activeChildEntity.COST_CENT_CODE = currentCellText;

                            if (CostDetailsSelectedRow["COST_CENT_CODE"].ToValueAsString().Trim() != CostDetailsSelectedRow["CCCode"].ToValueAsString().Trim())
                            {
                                CostDetailsSelectedRow["OUTPUT"] = "";
                                CostDetailsSelectedRow["CCOutput"] = "";
                                CostDetailsSelectedRow["VAR_COST"] = "";
                                CostDetailsSelectedRow["FIX_COST"] = "";
                                CostDetailsSelectedRow["SPL_COST"] = "";
                                CostDetailsSelectedRow["TOTAL_COST"] = "";
                                CostDetailsSelectedRow.EndEdit();
                            }

                            if (CostDetailsSelectedRow.IsNotNullOrEmpty())
                            {
                                if (activeChildEntity.SNO != 0)
                                    CostDetailsSelectedRow["SNO"] = activeChildEntity.SNO;
                                CostDetailsSelectedRow["OPERATION_NO"] = activeChildEntity.CODE;
                                CostDetailsSelectedRow["OPERATION"] = activeChildEntity.DESCRIPTION;
                                CostDetailsSelectedRow["COST_CENT_CODE"] = activeChildEntity.COST_CENT_CODE;
                                CostDetailsSelectedRow["CCCode"] = activeChildEntity.COST_CENT_CODE;
                                CostDetailsSelectedRow.EndEdit();
                            }



                            CostCentreOutputDataSource.RowFilter = null;
                            if (activeChildEntity.IsNotNullOrEmpty() && activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() && CostCentreOutputDataSource.IsNotNullOrEmpty())
                            {
                                CostCentreOutputDataSource.RowFilter = "COST_CENT_CODE In('" + activeChildEntity.COST_CENT_CODE.ToValueAsString().Trim().FormatEscapeChars() + "')";
                                DVCostCentreOutput = CostCentreOutputDataSource.ToTable().Copy().DefaultView;
                            }
                            //if (CostCentreOutputDataSource.Count == 0) CostCentreOutputDataSource.RowFilter = null;

                            if (CostCentreOutputDataSource.Count == 0)
                            {
                                CostDetailsSelectedRow["OUTPUT"] = "0";
                                CostDetailsSelectedRow["CCOutput"] = CostDetailsSelectedRow["OUTPUT"].ToValueAsString();
                            }
                            if (CostCentreOutputDataSource.Count == 1)
                            {
                                Output_SelectedItem = CostCentreOutputDataSource[0];
                                CostDetailsSelectedRow["OUTPUT"] = CostCentreOutputDataSource[0]["OUTPUT"].ToValueAsString();
                                CostDetailsSelectedRow["CCOutput"] = CostDetailsSelectedRow["OUTPUT"].ToValueAsString();
                            }
                            if (CostDetailsSelectedRow.IsNotNullOrEmpty()) CostDetailsSelectedRow.EndEdit();

                            fnCostCenterProcess(activeChildEntity);
                            CostCalculation();
                        }
                        break;
                    case "OUTPUT":

                        activeChildEntity = new CommonProcessCost();

                        if (Output_SelectedItem == null)
                        {
                            DataView dv = DVCostCentreOutput.ToTable().Copy().DefaultView;
                            if (dv != null)
                            {
                                dv.RowFilter = "OUTPUT = '" + CostDetailsSelectedRow["OUTPUT"].ToValueAsString().Trim().FormatEscapeChars() + "'";
                                if (dv.Count > 0)
                                {
                                    Output_SelectedItem = dv[0];
                                }
                            }
                        }

                        if (Output_SelectedItem.IsNotNullOrEmpty())
                        {
                            activeChildEntity.SNO = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDecimalValue();
                            activeChildEntity.CODE = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();

                            activeChildEntity.DESCRIPTION = CostDetailsSelectedRow["OPERATION"].ToValueAsString();

                            activeChildEntity.COST_CENT_CODE = CostDetailsSelectedRow["COST_CENT_CODE"].ToValueAsString();

                            if (!activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() && CostDetailsSelectedRow["CCCode"].ToValueAsString().IsNotNullOrEmpty()) activeChildEntity.COST_CENT_CODE = CostDetailsSelectedRow["CCCode"].ToValueAsString();

                            currentCellText = Output_SelectedItem["OUTPUT"].ToValueAsString();
                            if (!currentCellText.IsNotNullOrEmpty() && Output_SelectedItem["CCOutput"].ToValueAsString().IsNotNullOrEmpty()) currentCellText = Output_SelectedItem["CCOutput"].ToValueAsString();

                            activeChildEntity.CODE = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();
                            activeChildEntity.OUTPUT = currentCellText.ToDecimalValue();



                            if (CostDetailsSelectedRow.IsNotNullOrEmpty())
                            {
                                if (activeChildEntity.SNO != 0)
                                    CostDetailsSelectedRow["SNO"] = activeChildEntity.SNO;
                                CostDetailsSelectedRow["OPERATION_NO"] = activeChildEntity.CODE;
                                CostDetailsSelectedRow["OPERATION"] = activeChildEntity.DESCRIPTION;

                                CostDetailsSelectedRow["COST_CENT_CODE"] = activeChildEntity.COST_CENT_CODE;
                                CostDetailsSelectedRow["CCCode"] = activeChildEntity.COST_CENT_CODE;

                                CostDetailsSelectedRow["OUTPUT"] = activeChildEntity.OUTPUT;
                                CostDetailsSelectedRow["CCOutput"] = activeChildEntity.OUTPUT;

                                CostDetailsSelectedRow.EndEdit();

                            }

                            //DataView operationCostDataView = OperationCostDataSource.ToTable().Copy().DefaultView;
                            //operationCostDataView.RowFilter = "OPERATION_NO In('" + activeChildEntity.CODE.ToValueAsString() + "') AND " +
                            //"COST_CENT_CODE = '" + activeChildEntity.COST_CENT_CODE.ToValueAsString() + "' AND " +
                            //"LOC_CODE = '" + ActiveEntity.LOC_CODE.ToValueAsString() + "'";

                            DataView operationCostDataView = OperationCostDataSource.ToTable().Copy().DefaultView;
                            operationCostDataView.RowFilter = "OPERATION_NO In('" + activeChildEntity.CODE.ToValueAsString().Trim().FormatEscapeChars() + "') AND " +
                            "COST_CENT_CODE = '" + activeChildEntity.COST_CENT_CODE.ToValueAsString().Trim().FormatEscapeChars() + "'";

                            DVOperationCost = OperationCostDataSource.ToTable().Copy().DefaultView;
                            DataRowView operationCostSelectedRow = null;

                            if (operationCostDataView.Count == 1)
                            {
                                operationCostSelectedRow = operationCostDataView[0];
                            }
                            else if (operationCostDataView.Count == 0)
                            {
                                operationCostSelectedRow = operationCostDataView.AddNew();
                                if (CostDetailsSelectedRow.IsNotNullOrEmpty() && !CostDetailsSelectedRow["VAR_COST"].ToValueAsString().IsNotNullOrEmpty()) CostDetailsSelectedRow["VAR_COST"] = 0.0m;
                                if (CostDetailsSelectedRow.IsNotNullOrEmpty() && !CostDetailsSelectedRow["FIX_COST"].ToValueAsString().IsNotNullOrEmpty()) CostDetailsSelectedRow["FIX_COST"] = 0.0m;
                                if (CostDetailsSelectedRow.IsNotNullOrEmpty() && !CostDetailsSelectedRow["SPL_COST"].ToValueAsString().IsNotNullOrEmpty()) CostDetailsSelectedRow["SPL_COST"] = 0.0m;
                            }

                            //if (!CostDetailsSelectedRow["VAR_COST"].ToValueAsString().IsNotNullOrEmpty())
                            //    CostDetailsSelectedRow["VAR_COST"] = operationCostSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue();

                            //if (!CostDetailsSelectedRow["FIX_COST"].ToValueAsString().IsNotNullOrEmpty())
                            //    CostDetailsSelectedRow["FIX_COST"] = operationCostSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue();

                            //if (!CostDetailsSelectedRow["SPL_COST"].ToValueAsString().IsNotNullOrEmpty())
                            //    CostDetailsSelectedRow["SPL_COST"] = operationCostSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue();

                            //if (!CostDetailsSelectedRow["TOTAL_COST"].ToValueAsString().IsNotNullOrEmpty())
                            //    CostDetailsSelectedRow["TOTAL_COST"] = operationCostSelectedRow["TOTAL_COST"].ToValueAsString().ToDecimalValue();

                            CostDetailsSelectedRow["VAR_COST"] = operationCostSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue();
                            CostDetailsSelectedRow["FIX_COST"] = operationCostSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue();
                            CostDetailsSelectedRow["SPL_COST"] = operationCostSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue();
                            CostDetailsSelectedRow["TOTAL_COST"] = operationCostSelectedRow["TOTAL_COST"].ToValueAsString().ToDecimalValue();

                            CostDetailsSelectedRow["TOTAL_COST"] = Math.Round(operationCostSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue() +
                                                   operationCostSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue() +
                                                   operationCostSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue(), 2);

                            fnUpdateOutput(activeChildEntity);
                            WeightCalculation(CostDetailsSelectedRow);
                            CostCalculation();
                        }
                        break;
                    case "VAR_COST":
                        if (textBox.IsNotNullOrEmpty() && CostDetailsSelectedRow.IsNotNullOrEmpty())
                        {
                            if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0) { ShowInformationMessage("Invalid Number"); CostDetailsSelectedRow.CancelEdit(); }

                            CostDetailsSelectedRow["VAR_COST"] = textBox.Text.ToDecimalValue();
                            CostDetailsSelectedRow["TOTAL_COST"] = Math.Round(textBox.Text.ToDecimalValue() +
                                                                               CostDetailsSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue() +
                                                                               CostDetailsSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue(), 2);
                            CostDetailsSelectedRow.EndEdit();
                        }
                        CostCalculation();
                        break;
                    case "FIX_COST":
                        if (textBox.IsNotNullOrEmpty() && CostDetailsSelectedRow.IsNotNullOrEmpty())
                        {
                            if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0) { ShowInformationMessage("Invalid Number"); CostDetailsSelectedRow.CancelEdit(); }
                            CostDetailsSelectedRow["FIX_COST"] = textBox.Text.ToDecimalValue();
                            CostDetailsSelectedRow["TOTAL_COST"] = Math.Round(CostDetailsSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue() +
                                                                               textBox.Text.ToDecimalValue() +
                                                                               CostDetailsSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue(), 2);
                            CostDetailsSelectedRow.EndEdit();
                        }
                        CostCalculation();
                        break;
                    case "SPL_COST":
                        if (textBox.IsNotNullOrEmpty() && CostDetailsSelectedRow.IsNotNullOrEmpty())
                        {
                            if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0)
                            { ShowInformationMessage("Invalid Number"); CostDetailsSelectedRow.CancelEdit(); }
                            CostDetailsSelectedRow["SPL_COST"] = textBox.Text.ToDecimalValue();
                            CostDetailsSelectedRow["TOTAL_COST"] = Math.Round(CostDetailsSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue() +
                                                                               CostDetailsSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue() +
                                                                               textBox.Text.ToDecimalValue(), 2);
                            CostDetailsSelectedRow.EndEdit();
                        }
                        CostCalculation();
                        break;
                    case "TOTAL_COST":
                        if (textBox.IsNotNullOrEmpty() && CostDetailsSelectedRow.IsNotNullOrEmpty())
                        {
                            if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0) { ShowInformationMessage("Invalid Number"); CostDetailsSelectedRow.CancelEdit(); }

                            CostDetailsSelectedRow["TOTAL_COST"] = textBox.Text.ToDecimalValue();
                            CostDetailsSelectedRow.EndEdit();
                            CostCalculation();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        //private void CostDetailsCellEndEdit(Object currentCell)
        //{
        //    try
        //    {
        //        if (!currentCell.IsNotNullOrEmpty()) return;

        //        CommonProcessCost activeChildEntity = null;
        //        //BHCustCtrl.CustComboBox comboBox = null;
        //        System.Windows.Controls.TextBox textBox = null;

        //        DataRowView currentCellDataRowView = null;
        //        DataView operationDataView = null;
        //        DataView costCenterDataView = null;
        //        string currentCellText = null;
        //        System.Windows.Controls.ContentPresenter contentPresenter = null;

        //        Microsoft.Windows.Controls.DataGridCellInfo cellInfo = (Microsoft.Windows.Controls.DataGridCellInfo)currentCell;
        //        if (!cellInfo.Column.Header.IsNotNullOrEmpty()) return;

        //        object oldCellValue = null;
        //        if (CostDetailsSelectedRow.IsNotNullOrEmpty())
        //        {
        //            CostDetailsSelectedRow["CI_INFO_FK"] = ActiveEntity.IDPK;
        //            if (CostDetailsSelectedRow.DataView.Table.Columns.Contains(cellInfo.Column.Header.ToValueAsString().ToUpper()))
        //                oldCellValue = CostDetailsSelectedRow[cellInfo.Column.Header.ToValueAsString().ToUpper()];
        //            //CostDetailsSelectedRow["CI_REFERENCE"] = ActiveEntity.CI_REFERENCE;
        //        }

        //        object control = cellInfo.Column.GetCellContent(cellInfo.Item);

        //        switch (cellInfo.Column.Header.ToValueAsString().ToUpper())
        //        {
        //            case "SNO":
        //                #region SNO Implementation
        //                activeChildEntity = new CommonProcessCost();
        //                textBox = control as System.Windows.Controls.TextBox;

        //                //if (textBox.Text.IsNotNullOrEmpty() && textBox.Text.ToIntValue() == CONST_RAW_MATERIAL_PROCESS_SNO &&
        //                //    CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString().ToIntValue() != CONST_RAW_MATERIAL_PROCESS_CODE)
        //                //{
        //                //    ShowInformationMessage("SNo '" + CONST_RAW_MATERIAL_PROCESS_SNO + "' reserved for Raw Material Process.\r\nEnter New SNo");
        //                //    CostDetailsSelectedRow["SNO"] = string.Empty;
        //                //    CostDetailsSelectedRow.EndEdit();
        //                //    return;
        //                //}

        //                //if (textBox.Text.IsNotNullOrEmpty() && textBox.Text.ToIntValue() == CONST_FINISH_PROCESS_SNO &&
        //                //    !CONST_FINISH_PROCESS_CODE.Contains(CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString()))
        //                //{
        //                //    ShowInformationMessage("SNo '" + CONST_FINISH_PROCESS_SNO + "' reserved for Finish Process.\r\nEnter New SNo");
        //                //    CostDetailsSelectedRow["SNO"] = string.Empty;
        //                //    CostDetailsSelectedRow.EndEdit();
        //                //    return;
        //                //}

        //                //if (textBox.Text.IsNotNullOrEmpty() && textBox.Text.ToIntValue() == CONST_DESPATCH_PROCESS_SNO &&
        //                //    CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString().ToIntValue() != CONST_DESPATCH_PROCESS_CODE)
        //                //{
        //                //    ShowInformationMessage("SNo '" + CONST_DESPATCH_PROCESS_SNO + "' reserved for Despatch Process.\r\nEnter New SNo");
        //                //    CostDetailsSelectedRow["SNO"] = string.Empty;
        //                //    CostDetailsSelectedRow.EndEdit();
        //                //    return;
        //                //}

        //                //if (textBox.Text.IsNotNullOrEmpty() && (
        //                //    textBox.Text.ToIntValue() == CONST_RAW_MATERIAL_PROCESS_SNO ||
        //                //    CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString().ToIntValue() == CONST_RAW_MATERIAL_PROCESS_CODE))
        //                //{
        //                //    ShowInformationMessage("SNo '" + CONST_RAW_MATERIAL_PROCESS_SNO + "' reserved for Raw Material Process.\r\nEnter New SNo");
        //                //    textBox.Text = CostDetailsSelectedRow["SNO"].ToValueAsString();
        //                //    CostDetailsSelectedRow.CancelEdit();
        //                //    return;
        //                //}

        //                //if (textBox.Text.IsNotNullOrEmpty() && (
        //                //    textBox.Text.ToIntValue() == CONST_FINISH_PROCESS_SNO &&
        //                //    CONST_FINISH_PROCESS_CODE.Contains(CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString())))
        //                //{
        //                //    ShowInformationMessage("SNo '" + CONST_FINISH_PROCESS_SNO + "' reserved for Finish Process.\r\nEnter New SNo");
        //                //    textBox.Text = CostDetailsSelectedRow["SNO"].ToValueAsString();
        //                //    CostDetailsSelectedRow.CancelEdit();
        //                //    return;
        //                //}

        //                //if (textBox.Text.IsNotNullOrEmpty() && (
        //                //    textBox.Text.ToIntValue() == CONST_DESPATCH_PROCESS_SNO &&
        //                //    CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString().ToIntValue() == CONST_DESPATCH_PROCESS_CODE))
        //                //{
        //                //    ShowInformationMessage("SNo '" + CONST_DESPATCH_PROCESS_SNO + "' reserved for Despatch Process.\r\nEnter New SNo");
        //                //    textBox.Text = CostDetailsSelectedRow["SNO"].ToValueAsString();
        //                //    CostDetailsSelectedRow.CancelEdit();
        //                //    return;
        //                //}


        //                if (textBox.Text.IsNotNullOrEmpty())
        //                {

        //                    CostDetailsSelectedRow["SNO"] = textBox.Text.ToDecimalValue();
        //                    CostDetailsSelectedRow.EndEdit();

        //                    DataView costDetailsDataView = CostDetails.ToTable().Copy().DefaultView;
        //                    costDetailsDataView.RowFilter = "SNO = '" + textBox.Text.Trim().FormatEscapeChars() + "'";
        //                    if (costDetailsDataView.Count > 1)
        //                    {
        //                        ShowInformationMessage("SNo '" + textBox.Text + "' already exists!\r\nEnter New SNo");
        //                        CostDetailsSelectedRow["SNO"] = oldCellValue.ToValueAsString();
        //                        CostDetailsSelectedRow.EndEdit();
        //                        costDetailsDataView.RowFilter = null;
        //                        return;
        //                    }
        //                    CostDetailsSelectedRow.EndEdit();
        //                    costDetailsDataView.RowFilter = null;

        //                    if (CostDetailsSelectedRow["PROCESS_CODE"].ToValueAsString().ToIntValue() != CONST_RAW_MATERIAL_PROCESS_SNO &&
        //                       CostDetailsSelectedRow["PROCESS_CODE"].ToValueAsString().ToIntValue() != CONST_DESPATCH_PROCESS_SNO &&
        //                       CostDetailsSelectedRow["PROCESS_CODE"].ToValueAsString().ToIntValue() != CONST_FINISH_PROCESS_SNO)
        //                    {
        //                        CostDetailsSelectedRow["PROCESS_CODE"] = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDoubleValue();
        //                        CostDetailsSelectedRow.EndEdit();
        //                    }

        //                }
        //                break;
        //                #endregion
        //            case "OPERATION CODE":

        //                activeChildEntity = new CommonProcessCost();
        //                contentPresenter = control as System.Windows.Controls.ContentPresenter;

        //                if (contentPresenter.IsNotNullOrEmpty())
        //                    currentCellDataRowView = contentPresenter.Content as DataRowView;
        //                if (currentCellDataRowView.IsNotNullOrEmpty())
        //                {
        //                    activeChildEntity.SNO = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDecimalValue();

        //                    currentCellText = currentCellDataRowView["OPERATION_NO"].ToValueAsString();
        //                    activeChildEntity.CODE = currentCellText;

        //                    operationDataView = OperationDataSource.ToTable().Copy().DefaultView;
        //                    operationDataView.RowFilter = "OPERATION_NO In('" + activeChildEntity.CODE.ToValueAsString().Trim().FormatEscapeChars() + "')";
        //                    if (operationDataView.Count > 0)
        //                    {
        //                        activeChildEntity.DESCRIPTION = operationDataView[0].Row["OPERATION"].ToValueAsString();
        //                    }
        //                    if (operationDataView.Count == 0)
        //                    {
        //                        //activeChildEntity.CODE = null;
        //                        CostDetailsSelectedRow.CancelEdit();
        //                    }
        //                    operationDataView.RowFilter = null;
        //                }

        //                //if (!activeChildEntity.CODE.IsNotNullOrEmpty())
        //                //{
        //                //    ShowInformationMessage(PDMsg.NotEmpty("Operation Code"));
        //                //    return;
        //                //}


        //                if (CostDetailsSelectedRow.IsNotNullOrEmpty())
        //                {
        //                    if (activeChildEntity.SNO != 0)
        //                        CostDetailsSelectedRow["SNO"] = activeChildEntity.SNO;
        //                    CostDetailsSelectedRow["OPERATION_NO"] = activeChildEntity.CODE;
        //                    CostDetailsSelectedRow["OPERATION"] = activeChildEntity.DESCRIPTION;
        //                    CostDetailsSelectedRow["OprCode"] = activeChildEntity.CODE;
        //                    CostDetailsSelectedRow.EndEdit();
        //                }

        //                costCenterDataView = OperationCostDataSource;

        //                if (activeChildEntity.IsNotNullOrEmpty() && activeChildEntity.CODE.IsNotNullOrEmpty() &&
        //                    costCenterDataView.IsNotNullOrEmpty())
        //                    costCenterDataView.RowFilter = "OPERATION_NO In('" + activeChildEntity.CODE.ToValueAsString().Trim().FormatEscapeChars() + "')";
        //                //costCenterDataView.RowFilter = "OPERATION_NO In('" + activeChildEntity.CODE.ToValueAsString() + "') AND " +
        //                //"LOC_CODE='" + ActiveEntity.LOC_CODE.ToValueAsString() + "'";

        //                CostCentreOutputDataSource.RowFilter = null;
        //                if (activeChildEntity.IsNotNullOrEmpty() && activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() && CostCentreOutputDataSource.IsNotNullOrEmpty())
        //                    CostCentreOutputDataSource.RowFilter = "COST_CENT_CODE In('" + activeChildEntity.COST_CENT_CODE.ToValueAsString().Trim().FormatEscapeChars() + "')";

        //                fnOperationProcess(activeChildEntity);

        //                break;
        //            case "OPERATION": break;
        //            case "CC CODE":

        //                activeChildEntity = new CommonProcessCost();
        //                contentPresenter = control as System.Windows.Controls.ContentPresenter;

        //                if (contentPresenter.IsNotNullOrEmpty())
        //                    currentCellDataRowView = contentPresenter.Content as DataRowView;

        //                if (currentCellDataRowView.IsNotNullOrEmpty())
        //                {
        //                    activeChildEntity.SNO = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDecimalValue();

        //                    activeChildEntity.CODE = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();
        //                    if (!CostDetailsSelectedRow["OPERATION_NO"].IsNotNullOrEmpty() && CostDetailsSelectedRow["OprCode"].IsNotNullOrEmpty())
        //                        activeChildEntity.CODE = CostDetailsSelectedRow["OprCode"].ToValueAsString();

        //                    activeChildEntity.DESCRIPTION = CostDetailsSelectedRow["OPERATION"].ToValueAsString();

        //                    currentCellText = currentCellDataRowView["COST_CENT_CODE"].ToValueAsString();
        //                    if (!currentCellText.IsNotNullOrEmpty() && currentCellDataRowView["CCCode"].ToValueAsString().IsNotNullOrEmpty()) currentCellText = currentCellDataRowView["CCCode"].ToValueAsString();

        //                    if (!currentCellText.IsNotNullOrEmpty() && currentCellDataRowView["CCCode"].ToValueAsString().IsNotNullOrEmpty())
        //                        currentCellText = currentCellDataRowView["CCCode"].ToValueAsString();

        //                    activeChildEntity.COST_CENT_CODE = currentCellText;

        //                }

        //                if (CostDetailsSelectedRow.IsNotNullOrEmpty())
        //                {
        //                    if (activeChildEntity.SNO != 0)
        //                        CostDetailsSelectedRow["SNO"] = activeChildEntity.SNO;
        //                    CostDetailsSelectedRow["OPERATION_NO"] = activeChildEntity.CODE;
        //                    CostDetailsSelectedRow["OPERATION"] = activeChildEntity.DESCRIPTION;
        //                    CostDetailsSelectedRow["COST_CENT_CODE"] = activeChildEntity.COST_CENT_CODE;
        //                    CostDetailsSelectedRow["CCCode"] = activeChildEntity.COST_CENT_CODE;
        //                    CostDetailsSelectedRow.EndEdit();

        //                }

        //                CostCentreOutputDataSource.RowFilter = null;
        //                if (activeChildEntity.IsNotNullOrEmpty() && activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() && CostCentreOutputDataSource.IsNotNullOrEmpty())
        //                    CostCentreOutputDataSource.RowFilter = "COST_CENT_CODE In('" + activeChildEntity.COST_CENT_CODE.ToValueAsString().Trim().FormatEscapeChars() + "')";
        //                //if (CostCentreOutputDataSource.Count == 0) CostCentreOutputDataSource.RowFilter = null;

        //                if (CostCentreOutputDataSource.Count == 0)
        //                {
        //                    CostDetailsSelectedRow["OUTPUT"] = "0";
        //                    CostDetailsSelectedRow["CCOutput"] = CostDetailsSelectedRow["OUTPUT"].ToValueAsString();
        //                    CostDetailsSelectedRow["OprCode"] = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();
        //                }
        //                if (CostCentreOutputDataSource.Count == 1)
        //                {
        //                    CostDetailsSelectedRow["OUTPUT"] = CostCentreOutputDataSource[0]["OUTPUT"].ToValueAsString();
        //                    CostDetailsSelectedRow["CCOutput"] = CostDetailsSelectedRow["OUTPUT"].ToValueAsString();
        //                    CostDetailsSelectedRow["OprCode"] = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();
        //                }
        //                if (CostDetailsSelectedRow.IsNotNullOrEmpty()) CostDetailsSelectedRow.EndEdit();

        //                fnCostCenterProcess(activeChildEntity);
        //                CostCalculation();

        //                break;
        //            case "OUTPUT":

        //                activeChildEntity = new CommonProcessCost();
        //                contentPresenter = control as System.Windows.Controls.ContentPresenter;

        //                if (contentPresenter.IsNotNullOrEmpty())
        //                    currentCellDataRowView = contentPresenter.Content as DataRowView;

        //                if (currentCellDataRowView.IsNotNullOrEmpty())
        //                {
        //                    activeChildEntity.SNO = CostDetailsSelectedRow["SNO"].ToValueAsString().ToDecimalValue();
        //                    activeChildEntity.CODE = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();

        //                    activeChildEntity.DESCRIPTION = CostDetailsSelectedRow["OPERATION"].ToValueAsString();

        //                    activeChildEntity.COST_CENT_CODE = CostDetailsSelectedRow["COST_CENT_CODE"].ToValueAsString();

        //                    if (!activeChildEntity.COST_CENT_CODE.IsNotNullOrEmpty() && CostDetailsSelectedRow["CCCode"].ToValueAsString().IsNotNullOrEmpty()) activeChildEntity.COST_CENT_CODE = CostDetailsSelectedRow["CCCode"].ToValueAsString();

        //                    currentCellText = currentCellDataRowView["OUTPUT"].ToValueAsString();
        //                    if (!currentCellText.IsNotNullOrEmpty() && currentCellDataRowView["CCOutput"].ToValueAsString().IsNotNullOrEmpty()) currentCellText = currentCellDataRowView["CCOutput"].ToValueAsString();

        //                    if (!activeChildEntity.CODE.IsNotNullOrEmpty() && CostDetailsSelectedRow["OprCode"].ToValueAsString().IsNotNullOrEmpty()) activeChildEntity.CODE = CostDetailsSelectedRow["OprCode"].ToValueAsString();

        //                    CostDetailsSelectedRow["OprCode"] = CostDetailsSelectedRow["OPERATION_NO"].ToValueAsString();
        //                    activeChildEntity.OUTPUT = currentCellText.ToDecimalValue();

        //                }

        //                if (CostDetailsSelectedRow.IsNotNullOrEmpty())
        //                {
        //                    if (activeChildEntity.SNO != 0)
        //                        CostDetailsSelectedRow["SNO"] = activeChildEntity.SNO;
        //                    CostDetailsSelectedRow["OPERATION_NO"] = activeChildEntity.CODE;
        //                    CostDetailsSelectedRow["OPERATION"] = activeChildEntity.DESCRIPTION;

        //                    CostDetailsSelectedRow["COST_CENT_CODE"] = activeChildEntity.COST_CENT_CODE;
        //                    CostDetailsSelectedRow["CCCode"] = activeChildEntity.COST_CENT_CODE;

        //                    CostDetailsSelectedRow["OUTPUT"] = activeChildEntity.OUTPUT;
        //                    CostDetailsSelectedRow["CCOutput"] = activeChildEntity.OUTPUT;
        //                    CostDetailsSelectedRow["OprCode"] = activeChildEntity.CODE;

        //                    CostDetailsSelectedRow.EndEdit();

        //                }

        //                //DataView operationCostDataView = OperationCostDataSource.ToTable().Copy().DefaultView;
        //                //operationCostDataView.RowFilter = "OPERATION_NO In('" + activeChildEntity.CODE.ToValueAsString() + "') AND " +
        //                //"COST_CENT_CODE = '" + activeChildEntity.COST_CENT_CODE.ToValueAsString() + "' AND " +
        //                //"LOC_CODE = '" + ActiveEntity.LOC_CODE.ToValueAsString() + "'";

        //                DataView operationCostDataView = OperationCostDataSource.ToTable().Copy().DefaultView;
        //                operationCostDataView.RowFilter = "OPERATION_NO In('" + activeChildEntity.CODE.ToValueAsString().Trim().FormatEscapeChars() + "') AND " +
        //                "COST_CENT_CODE = '" + activeChildEntity.COST_CENT_CODE.ToValueAsString().Trim().FormatEscapeChars() + "'";

        //                DataRowView operationCostSelectedRow = null;

        //                if (operationCostDataView.Count == 1)
        //                {
        //                    operationCostSelectedRow = operationCostDataView[0];
        //                }
        //                else if (operationCostDataView.Count == 0)
        //                {
        //                    operationCostSelectedRow = operationCostDataView.AddNew();
        //                    if (CostDetailsSelectedRow.IsNotNullOrEmpty() && !CostDetailsSelectedRow["VAR_COST"].ToValueAsString().IsNotNullOrEmpty()) CostDetailsSelectedRow["VAR_COST"] = 0.0m;
        //                    if (CostDetailsSelectedRow.IsNotNullOrEmpty() && !CostDetailsSelectedRow["FIX_COST"].ToValueAsString().IsNotNullOrEmpty()) CostDetailsSelectedRow["FIX_COST"] = 0.0m;
        //                    if (CostDetailsSelectedRow.IsNotNullOrEmpty() && !CostDetailsSelectedRow["SPL_COST"].ToValueAsString().IsNotNullOrEmpty()) CostDetailsSelectedRow["SPL_COST"] = 0.0m;
        //                }

        //                //if (!CostDetailsSelectedRow["VAR_COST"].ToValueAsString().IsNotNullOrEmpty())
        //                //    CostDetailsSelectedRow["VAR_COST"] = operationCostSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue();

        //                //if (!CostDetailsSelectedRow["FIX_COST"].ToValueAsString().IsNotNullOrEmpty())
        //                //    CostDetailsSelectedRow["FIX_COST"] = operationCostSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue();

        //                //if (!CostDetailsSelectedRow["SPL_COST"].ToValueAsString().IsNotNullOrEmpty())
        //                //    CostDetailsSelectedRow["SPL_COST"] = operationCostSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue();

        //                //if (!CostDetailsSelectedRow["TOTAL_COST"].ToValueAsString().IsNotNullOrEmpty())
        //                //    CostDetailsSelectedRow["TOTAL_COST"] = operationCostSelectedRow["TOTAL_COST"].ToValueAsString().ToDecimalValue();

        //                CostDetailsSelectedRow["VAR_COST"] = operationCostSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue();
        //                CostDetailsSelectedRow["FIX_COST"] = operationCostSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue();
        //                CostDetailsSelectedRow["SPL_COST"] = operationCostSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue();
        //                CostDetailsSelectedRow["TOTAL_COST"] = operationCostSelectedRow["TOTAL_COST"].ToValueAsString().ToDecimalValue();

        //                CostDetailsSelectedRow["TOTAL_COST"] = Math.Round(operationCostSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue() +
        //                                       operationCostSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue() +
        //                                       operationCostSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue(), 2);

        //                fnUpdateOutput(activeChildEntity);
        //                WeightCalculation(CostDetailsSelectedRow);
        //                CostCalculation();

        //                break;
        //            case "VAR COST":
        //                textBox = control as System.Windows.Controls.TextBox;
        //                if (textBox.IsNotNullOrEmpty() && CostDetailsSelectedRow.IsNotNullOrEmpty())
        //                {
        //                    if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0) { ShowInformationMessage("Invalid Number"); CostDetailsSelectedRow.CancelEdit(); }

        //                    CostDetailsSelectedRow["VAR_COST"] = textBox.Text.ToDecimalValue();
        //                    CostDetailsSelectedRow["TOTAL_COST"] = Math.Round(textBox.Text.ToDecimalValue() +
        //                                                                       CostDetailsSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue() +
        //                                                                       CostDetailsSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue(), 2);
        //                    CostDetailsSelectedRow.EndEdit();
        //                }
        //                CostCalculation();
        //                break;
        //            case "FIXED COST": textBox = control as System.Windows.Controls.TextBox;
        //                if (textBox.IsNotNullOrEmpty() && CostDetailsSelectedRow.IsNotNullOrEmpty())
        //                {
        //                    if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0) { ShowInformationMessage("Invalid Number"); CostDetailsSelectedRow.CancelEdit(); }
        //                    CostDetailsSelectedRow["FIX_COST"] = textBox.Text.ToDecimalValue();
        //                    CostDetailsSelectedRow["TOTAL_COST"] = Math.Round(CostDetailsSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue() +
        //                                                                       textBox.Text.ToDecimalValue() +
        //                                                                       CostDetailsSelectedRow["SPL_COST"].ToValueAsString().ToDecimalValue(), 2);
        //                    CostDetailsSelectedRow.EndEdit();
        //                }
        //                CostCalculation();
        //                break;
        //            case "SPL COST": textBox = control as System.Windows.Controls.TextBox;
        //                if (textBox.IsNotNullOrEmpty() && CostDetailsSelectedRow.IsNotNullOrEmpty())
        //                {
        //                    if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0)
        //                    { ShowInformationMessage("Invalid Number"); CostDetailsSelectedRow.CancelEdit(); }
        //                    CostDetailsSelectedRow["SPL_COST"] = textBox.Text.ToDecimalValue();
        //                    CostDetailsSelectedRow["TOTAL_COST"] = Math.Round(CostDetailsSelectedRow["VAR_COST"].ToValueAsString().ToDecimalValue() +
        //                                                                       CostDetailsSelectedRow["FIX_COST"].ToValueAsString().ToDecimalValue() +
        //                                                                       textBox.Text.ToDecimalValue(), 2);
        //                    CostDetailsSelectedRow.EndEdit();
        //                }
        //                CostCalculation();
        //                break;
        //            case "OPERATION COST":
        //                textBox = control as System.Windows.Controls.TextBox;
        //                if (textBox.IsNotNullOrEmpty() && CostDetailsSelectedRow.IsNotNullOrEmpty())
        //                {
        //                    if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0) { ShowInformationMessage("Invalid Number"); CostDetailsSelectedRow.CancelEdit(); }

        //                    CostDetailsSelectedRow["TOTAL_COST"] = textBox.Text.ToDecimalValue();
        //                    CostDetailsSelectedRow.EndEdit();
        //                    CostCalculation();
        //                }
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }

        //    if (canAddNewCostDetails())
        //        CostDetailsInsertRow(null);
        //}

        private DataRowView getEmptyRowCostDetails()
        {
            DataRowView bReturnValue = null;
            try
            {
                DataView dataView = CostDetails;
                //dataView.RowFilter = "CONVERT(Isnull(SNO,''), System.String) = '' AND " +
                //                     "CONVERT(Isnull(OPERATION_NO,''), System.String) = '' AND " +
                //                     "CONVERT(Isnull(OPERATION,''), System.String) = '' AND " +
                //                     "CONVERT(Isnull(COST_CENT_CODE,''), System.String) = '' AND " +
                //                     "CONVERT(Isnull(OUTPUT,''), System.String) = ''";

                dataView.RowFilter = "CONVERT(Isnull(SNO,''), System.String) = '' AND " +
                                     "CONVERT(Isnull(OPERATION_NO,''), System.String) = '' AND " +
                                     "CONVERT(Isnull(OPERATION,''), System.String) = '' AND " +
                                     "CONVERT(Isnull(COST_CENT_CODE,''), System.String) = ''";

                if (dataView.Count > 0)
                    bReturnValue = dataView[0];
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return bReturnValue;
        }

        private bool canAddNewCostDetails()
        {
            bool bReturnValue = false;
            try
            {

                foreach (DataRowView drv in CostDetails)
                {
                    if (!drv["OPERATION_NO"].IsNotNullOrEmpty() && !drv["OPERATION_NO"].IsNotNullOrEmpty() && !drv["COST_CENT_CODE"].IsNotNullOrEmpty()) return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            return bReturnValue;
        }
        private readonly ICommand _costDetailsDeleteCommand;
        public ICommand CostDetailsDeleteCommand { get { return this._costDetailsDeleteCommand; } }

        private void CostDetailsDeleteRow(Object selectedRow)
        {
            try
            {

                if (!selectedRow.IsNotNullOrEmpty() || selectedRow.ToValueAsString() == "{NewItemPlaceholder}"
                    || !CostDetails.IsNotNullOrEmpty() || CostDetails.Count <= 0) return;

                DataRowView selectedDataRowView = (DataRowView)selectedRow;

                if (ShowWarningMessage(PDMsg.BeforeDeleteRecord, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    //if (CostDetails.Count <= 1)
                    //{
                    //    foreach (DataColumn col in CostDetails.Table.Columns)
                    //    {
                    //        selectedDataRowView[col.ColumnName] = DBNull.Value;
                    //    }

                    //    return;
                    //}

                    int rowIndex = -1;
                    bool isRowFound = false;

                    foreach (DataRowView rowView in CostDetails)
                    {
                        rowIndex++;
                        if (rowView["SNO_SEQUENCE"].ToValueAsString() == selectedDataRowView["SNO_SEQUENCE"].ToValueAsString())
                        {
                            isRowFound = true;
                            break;
                        }
                    }
                    if (isRowFound && rowIndex >= 0)
                    {

                        foreach (DDCOST_PROCESS_DATA associationEntity in ActiveEntity.DDCOST_PROCESS_DATA)
                        {
                            ActiveEntity.DDCOST_PROCESS_DATA.Remove(associationEntity);
                        }
                        DataTable dt = bll.GetCostDetails(new DDCI_INFO() { IDPK = -99999 }).ToDataTable<DDCOST_PROCESS_DATA>().Clone();
                        dt.ImportRow(selectedDataRowView.Row);

                        List<DDCOST_PROCESS_DATA> lstAssociationEntity = null;
                        lstAssociationEntity = (from row in dt.AsEnumerable()
                                                where row.Field<string>("SNO").ToValueAsString().Trim().Length > 0 &&
                                                row.Field<string>("CI_REFERENCE").ToValueAsString().Trim().Length > 0 &&
                                                row.Field<string>("OPERATION_NO").ToValueAsString().Trim().Length > 0 &&
                                                row.Field<string>("COST_CENT_CODE").ToValueAsString().Trim().Length > 0
                                                select new DDCOST_PROCESS_DATA()
                                                {
                                                    CI_REFERENCE = ActiveEntity.CI_REFERENCE,
                                                    SNO = Convert.ToDecimal(row.Field<string>("SNO")),
                                                    OPERATION_NO = Convert.ToDecimal(row.Field<string>("OPERATION_NO")),
                                                    OPERATION = row.Field<string>("OPERATION"),
                                                    COST_CENT_CODE = row.Field<string>("COST_CENT_CODE"),
                                                    OUTPUT = Convert.ToDecimal(row.Field<string>("OUTPUT")),
                                                    VAR_COST = Convert.ToDecimal(row.Field<string>("VAR_COST")),
                                                    FIX_COST = Convert.ToDecimal(row.Field<string>("FIX_COST")),
                                                    SPL_COST = Convert.ToDecimal(row.Field<string>("SPL_COST")),
                                                    UNIT_OF_MEASURE = row.Field<string>("UNIT_OF_MEASURE"),
                                                    TOTAL_COST = Convert.ToDecimal(row.Field<string>("TOTAL_COST")),
                                                    IDPK = row.Field<string>("IDPK").ToIntValue(),
                                                    CI_INFO_FK = ActiveEntity.IDPK,
                                                }).ToList<DDCOST_PROCESS_DATA>();

                        //bool isExecuted = bll.Delete<DDCOST_PROCESS_DATA>(lstAssociationEntity);
                        CostDetails.Delete(rowIndex);
                        //CostDetails.Table.AcceptChanges();
                        CostCalculation();

                        string fieldName = "SNO";
                        DataView dataView = CostDetails;
                        ReNumber(ref dataView, fieldName);

                        dataView.Sort = null;
                        dataView.Sort = "SNO_SEQUENCE ASC";
                    }

                }

                if (CostDetails.IsNotNullOrEmpty() && CostDetails.Count == 0 || canAddNewCostDetails())
                {
                    CostDetailsInsertRow(null);

                    //string fieldName = "SNO_SEQUENCE";
                    //string filter = fieldName + " <> " + CONST_RAW_MATERIAL_PROCESS_SNO + " AND " + fieldName + " <> " + CONST_DESPATCH_PROCESS_SNO + " AND " + fieldName + " <> " + CONST_FINISH_PROCESS_SNO;

                    //int maxRowNumber = CostDetails.ToTable().Copy().Compute("Max(" + fieldName + ")", filter).ToValueAsString().ToIntValue();

                    //DataRowView rowView = getEmptyRowCostDetails();
                    //if (rowView.IsNotNullOrEmpty())
                    //{
                    //    rowView[fieldName] = maxRowNumber + 1;
                    //    rowView.EndEdit();
                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand _currentCellChangedCostDetailsCommand;
        public ICommand CurrentCellChangedCostDetailsCommand { get { return this._currentCellChangedCostDetailsCommand; } }
        private void CurrentCellChangedCostDetails(DataRowView selecteditem)
        {
            if (cellInfo.Column.IsNotNullOrEmpty())
            {
                if (cellInfo.Column.Header.ToValueAsString().ToUpper() == "OPERATION CODE" || cellInfo.Column.Header.ToValueAsString().ToUpper() == "SNO" || cellInfo.Column.Header.ToValueAsString().ToUpper() == "CC CODE" || cellInfo.Column.Header.ToValueAsString().ToUpper() == "OUTPUT")
                {
                    CostDetailsInsertRowWithoutEdit(null);
                }
                else
                {
                    CostDetailsInsertRow(null);
                }
            }
            else
            {
                CostDetailsInsertRow(null);
            }


        }


        private readonly ICommand _costDetailsInsertCommand;
        public ICommand CostDetailsInsertCommand { get { return this._costDetailsInsertCommand; } }

        private void CostDetailsInsertRow(Object selectedRow)
        {
            try
            {

                if (!CostDetails.IsNotNullOrEmpty()) return;

                //DataView dataView = CostDetails.Table.Copy().DefaultView;
                //dataView.RowFilter = "CONVERT(Isnull(OPERATION_NO,''), System.String) = '' AND " +
                //                     "CONVERT(Isnull(OPERATION,''), System.String) = '' AND " +
                //                     "CONVERT(Isnull(COST_CENT_CODE,''), System.String) = '' AND " +
                //                     "CONVERT(Isnull(OUTPUT,''), System.String) = '' AND " +
                //                     "CONVERT(Isnull(SNO,''), System.String) = ''";
                if (!canAddNewCostDetails()) return;


                DataRowView rowView = CostDetails.AddNew();
                rowView.BeginEdit();
                rowView.EndEdit();


                //string fieldName = "SNO";    

                //if (CostDetails.Count > 0)
                //{                    
                //    rowView[fieldName] = DBNull.Value;
                //    if (rowView[fieldName].ToValueAsString().Length > 4) rowView[fieldName] = DBNull.Value;
                //    rowView.EndEdit();
                //}                


                //if (CostDetails.Count - 1 < 0 || (CostDetails[CostDetails.Count - 1][fieldName].IsNotNullOrEmpty() &&
                //    CostDetails[CostDetails.Count - 1][fieldName].ToValueAsString().Length > 0))
                //{
                //    DataView dt = CostDetails;
                //    DataRowView rowView = dt.AddNew();

                //    int maxRowNumber = CONST_MAX_REORDER_SNO;

                //    if (CostDetails.Count > 0 && CostDetails.ToTable().Columns.Contains(fieldName + "_SEQUENCE"))
                //    {
                //        maxRowNumber = CostDetails.ToTable().Copy().Compute("Max(" + fieldName + "_SEQUENCE" + ")", string.Empty).ToValueAsString().ToIntValue();

                //        if (maxRowNumber < CONST_MAX_REORDER_SNO) maxRowNumber = CONST_MAX_REORDER_SNO;
                //        rowView[fieldName] = maxRowNumber + 1;
                //        rowView.EndEdit();
                //    }
                //    else
                //    {
                //        rowView[fieldName] = maxRowNumber;
                //        rowView.EndEdit();
                //    }
                //    ReNumber(ref dt, fieldName);
                //    rowView[fieldName] = string.Empty;
                //    rowView.EndEdit();

                //    if (CostDetails.Count > 0 && CostDetails.ToTable().Columns.Contains(fieldName) && !selectedRow.IsNotNullOrEmpty())
                //    {
                //        maxRowNumber = CostDetails.ToTable().Copy().Compute("Max(" + fieldName + ")", string.Empty).ToValueAsString().ToIntValue();

                //        rowView[fieldName] = ((int)(maxRowNumber / 10) + 1) * 10;
                //        rowView.EndEdit();
                //    }
                //    rowView.EndEdit();
                //    CostDetails.RowFilter = null;
                //}

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private void CostDetailsInsertRowWithoutEdit(Object selectedRow)
        {
            try
            {

                if (!CostDetails.IsNotNullOrEmpty()) return;


                if (!canAddNewCostDetails()) return;

                CostDetails.AddNew();
                // DataRowView rowView = CostDetails.AddNew();
                //rowView.BeginEdit();
                //rowView.EndEdit();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand _costDetailsReNumberCommand;
        public ICommand CostDetailsReNumberCommand { get { return this._costDetailsReNumberCommand; } }

        private void CostDetailsReNumberRow(Object selectedRow)
        {
            try
            {
                //assignNewSequenceNumberCostDetails(CostDetails.Table);

                DataView dt = CostDetails;
                string fieldName = "SNO";
                //ReNumber(ref dt, fieldName, "ASC", 10, false);
                ReNumber(ref dt, fieldName);

                string newFieldName = "SNO_SEQUENCE";
                dt.Sort = null;
                dt.Sort = newFieldName + " ASC";

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private FRCSModel _mandatoryFields = null;
        public FRCSModel MandatoryFields
        {
            get
            {
                return _mandatoryFields;
            }
            set
            {
                _mandatoryFields = value;
                copyMandatoryFieldsToEntity(value);
                NotifyPropertyChanged("MandatoryFields");
            }
        }

        private void copyMandatoryFieldsToEntity(FRCSModel mandatoryFields)
        {

            if (!ActiveEntity.IsNotNullOrEmpty() || !mandatoryFields.IsNotNullOrEmpty()) return;

            //CONST_FIXED_COST = mandatoryFields.RM_FACTOR.ToValueAsString().ToDecimalValue();
            //CONST_VARIABLE_COST = mandatoryFields.RM_FACTOR.ToValueAsString().ToDecimalValue();

            ActiveEntity.CI_REFERENCE = mandatoryFields.CI_REFERENCE;

            CustomerSelectedRow = null;
            //CustomersDataSource.RowFilter = "CUST_NAME='" + mandatoryFields.CUST_NAME.ToValueAsString().FormatEscapeChars() + "'";
            //if (CustomersDataSource.Count > 0)
            //{
            //    ActiveEntity.CUST_CODE = CustomersDataSource[0]["CUST_CODE"].ToValueAsString().ToIntValue();
            //}
            //CustomersDataSource.RowFilter = null;


            ActiveEntity.CHEESE_WT = mandatoryFields.CHEESE_WT;
            ActiveEntity.FINISH_WT = mandatoryFields.FINISH_WT;
            ActiveEntity.RM_FACTOR = mandatoryFields.RM_FACTOR;
            ActiveEntity.FEASIBILITY = IS_FEASIBILITY_CAN_CHANGE ? "1" : "0";
            ActiveEntity.LOC_CODE = mandatoryFields.LOC_CODE;

            if (mandatoryFields.NUMBER_OFF.IsNotNullOrEmpty())
                ActiveEntity.NUMBER_OFF = mandatoryFields.NUMBER_OFF.ToDecimalValue();

            if (mandatoryFields.SFL_SHARE.IsNotNullOrEmpty())
                ActiveEntity.SFL_SHARE = mandatoryFields.SFL_SHARE.ToDecimalValue();

            if (mandatoryFields.POTENTIAL.IsNotNullOrEmpty())
                ActiveEntity.POTENTIAL = mandatoryFields.POTENTIAL.ToDecimalValue();

        }

        private readonly ICommand combinedIsCheckedCommand;
        public ICommand CombinedIsCheckedCommand { get { return this.combinedIsCheckedCommand; } }
        private void combinedIsChecked()
        {
            //if (!ActiveEntity.IS_COMBINED.ToValueAsString().ToBooleanAsString())
            //{
            //    if (ActiveEntity.FINISH_WT.ToValueAsString().ToDecimalValue() > ActiveEntity.CHEESE_WT.ToValueAsString().ToDecimalValue())
            //    {
            //        ShowInformationMessage("Finish Weight is greater than Cheese Weight.\r\nPlease make sure it is correct.");
            //        return;
            //    }
            //}
        }

        private readonly ICommand standatedNotesSaveCommand;
        public ICommand StandatedNotesSaveClickCommand { get { return this.standatedNotesSaveCommand; } }
        private void standatedNotesSaveSubmitCommand()
        {

            try
            {
                StandatedNotesIsFocusedSaveButton = true;
                List<DDSTD_NOTES> lstStandardNotes = null;

                bool isExecuted = false;
                //foreach (DataRowView dataRowView in StandardNotes)
                //{
                //    dataRowView["STD_NOTES"] = dataRowView["STD_NOTES"].ToValueAsString();
                //    dataRowView.EndEdit();
                //}

                lstStandardNotes = (from row in StandardNotesDeletedList.AsEnumerable()
                                    select new DDSTD_NOTES()
                                    {
                                        SNO = Convert.ToDecimal(row.Field<string>("SNO")),
                                        STD_NOTES = row.Field<string>("STD_NOTES"),
                                        ROWID = Guid.NewGuid(),
                                    }).ToList<DDSTD_NOTES>();
                if (lstStandardNotes.IsNotNullOrEmpty() && lstStandardNotes.Count > 0)
                {
                    isExecuted = bll.Delete<DDSTD_NOTES>(lstStandardNotes);
                    StandardNotesDeletedList.Rows.Clear();
                }

                lstStandardNotes = (from row in StandardNotes.ToTable().AsEnumerable()
                                    select new DDSTD_NOTES()
                                    {
                                        SNO = Convert.ToDecimal(row.Field<string>("SNO")),
                                        STD_NOTES = row.Field<string>("STD_NOTES"),
                                        ROWID = Guid.NewGuid(),
                                    }).ToList<DDSTD_NOTES>();

                if (!lstStandardNotes.IsNotNullOrEmpty() || lstStandardNotes.Count == 0) return;

                var lstRecordCount = (from row in lstStandardNotes
                                      group row by row.STD_NOTES into grpStandardNotes
                                      where grpStandardNotes.Count() > 1
                                      select new { Key = grpStandardNotes.Key, Count = grpStandardNotes.Count() }).ToList<object>();

                if (lstRecordCount.IsNotNullOrEmpty())
                {
                    foreach (var item in lstRecordCount)
                    {
                        ShowInformationMessage("Duplicate Standard Notes '" + item.GetFieldValue("Key").ToValueAsString() + "' has been Entered");
                        return;
                    }
                }
                isExecuted = bll.Update<DDSTD_NOTES>(lstStandardNotes);

                if (isExecuted)
                {
                    ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                    _logviewBll.SaveLog(MandatoryFields.CI_REFERENCE, "FRCS");
                }

                StandardNotes = bll.GetStandardNotes().ToDataTable<DDSTD_NOTES>().DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            StandatedNotesIsFocusedSaveButton = false;
        }

        private readonly ICommand standatedNotesIncludeCommand;
        public ICommand StandatedNotesIncludeClickCommand { get { return this.standatedNotesIncludeCommand; } }
        private void standatedNotesIncludeSubmitCommand()
        {

            try
            {
                if (!ActiveEntity.IsNotNullOrEmpty() || !StandardNotesSelectedRow.IsNotNullOrEmpty()) return;
                StandatedNotesIsFocusedSaveButton = true;

                //if (!ActiveEntity.COST_NOTES.Contains(StandardNotesSelectedRow["STD_NOTES"].ToValueAsString().Trim()))
                ActiveEntity.COST_NOTES += (ActiveEntity.COST_NOTES.ToValueAsString().Length > 0 ? "\r\n" : "") +
                    StandardNotesSelectedRow["STD_NOTES"].ToValueAsString();

                ActiveEntity.COST_NOTES = ActiveEntity.COST_NOTES.Substring(0,
                    ActiveEntity.COST_NOTES.ToValueAsString().Length > 1998 ? 1998 : ActiveEntity.COST_NOTES.ToValueAsString().Length).Trim();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            StandatedNotesIsFocusedSaveButton = false;
        }

        private readonly ICommand standatedNotesCloseCommand;
        public ICommand StandatedNotesCloseClickCommand { get { return this.standatedNotesCloseCommand; } }
        private void standatedNotesCloseSubmitCommand()
        {
            StandatedNotesIsFocusedSaveButton = true;
            StandatedNotesIsFocusedSaveButton = false;

            DataTable activeStandardEntityDataTable = null;
            bool result = true;
            activeStandardEntityDataTable = StandardNotes.Table.Copy();
            result = activeStandardEntityDataTable.IsEqual(originalStandardEntityDataTable);
            if (!result)
            {
                if (ShowWarningMessage("Do you want to save Standated Notes", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    standatedNotesSaveSubmitCommand();
                }
            }

            StandardNotes = bll.GetStandardNotes().ToDataTable<DDSTD_NOTES>().DefaultView;

            CostDetailsVisibility = Visibility.Visible;
            RejectReasonsVisibility = Visibility.Collapsed;
            StandatedNotesVisibility = Visibility.Collapsed;
        }

        private void AssingFieldValue(DataView dataView, DataRowView selectedRow, string codeFieldName, string descriptionFieldName,
 FRCSModel mandatoryFields, DDCI_INFO activeEntity, string operatorName = "LIKE")
        {
            if (!dataView.IsNotNullOrEmpty() || !mandatoryFields.IsNotNullOrEmpty() || !codeFieldName.IsNotNullOrEmpty() ||
                !descriptionFieldName.IsNotNullOrEmpty()) return;
            string sFindableValue = mandatoryFields.GetFieldValue(descriptionFieldName).ToValueAsString().Trim();

            dataView.RowFilter = null;
            if (dataView.Count == 0 || !sFindableValue.IsNotNullOrEmpty()) return;

            dataView.RowFilter = dataView.GetRowFilter(sFindableValue, operatorName);
            if (dataView.Count > 0)
            {
                selectedRow = dataView[0];
                if (dataView.Table.Columns.Contains(descriptionFieldName))
                    mandatoryFields.SetFieldValue(descriptionFieldName, selectedRow[descriptionFieldName].ToValueAsString());

                if (dataView.Table.Columns.Contains(codeFieldName))
                    activeEntity.SetFieldValue(descriptionFieldName, selectedRow[codeFieldName].ToValueAsString());
            }

            dataView.RowFilter = null;
        }

        private readonly ICommand ciReferenceZoneEndEditCommand;
        public ICommand CIReferenceZoneEndEditCommand { get { return this.ciReferenceZoneEndEditCommand; } }
        private void ciReferenceZoneEndEdit()
        {
            AssingFieldValue(CIReferenceZoneDataSource, CiReferenceZoneSelectedRow, "CODE", "DESCRIPTION", MandatoryFields, ActiveEntity);
        }

        private bool _standatedNotesIsFocusedSaveButton = false;
        public bool StandatedNotesIsFocusedSaveButton
        {
            get { return _standatedNotesIsFocusedSaveButton; }
            set
            {
                _standatedNotesIsFocusedSaveButton = value;
                NotifyPropertyChanged("StandatedNotesIsFocusedSaveButton");
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

                //if (CostDetailsSelectedRow.IsNotNullOrEmpty() && OperCode_SelectedItem.IsNotNullOrEmpty())
                //{
                //    CostDetailsSelectedRow.BeginEdit();
                //    if (OperCode_SelectedItem != null)
                //    {
                //        CostDetailsSelectedRow["OPERATION"] = OperCode_SelectedItem["OPERATION"].ToValueAsString();
                //    }
                //    else
                //    {
                //        CostDetailsSelectedRow["OPERATION"] = "";
                //    }
                //    CostDetailsSelectedRow.EndEdit();
                //}

            }
        }

        public void TextBoxDateValidation_LostFocus(object sender, RoutedEventArgs e)
        {

            try
            {
                System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)sender;

                if (!string.IsNullOrEmpty(tb.Text.Trim()))
                {
                    if (UserControls.DateValidation.CheckIsValidDate(tb.Text.ToString().Trim()) == false)
                    {
                        ShowInformationMessage(PDMsg.Invalid("Date"));
                        tb.Text = string.Empty;
                        return;
                    }
                    else
                    {
                        //tb.Text = tb.Text.ToDateAsString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }


        }

        private bool _isDefaultFocused = false;
        public bool IsDefaultFocused
        {
            get { return _isDefaultFocused; }
            set
            {
                _isDefaultFocused = value;
                NotifyPropertyChanged("IsDefaultFocused");
            }
        }


    }

}
