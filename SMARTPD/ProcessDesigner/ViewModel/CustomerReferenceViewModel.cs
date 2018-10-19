using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using System.Windows;
using System.Data;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using System.Windows.Input;
using System.ComponentModel.DataAnnotations;
using ProcessDesigner.DAL;
using System.Windows.Controls;

namespace ProcessDesigner.ViewModel
{
    public class CustomerReferenceViewModel : ViewModelBase
    {
        UserInformation _userInformation;
        private DrawingBll drwBll;
        private LogViewBll _logviewBll;
        private WPF.MDI.MdiChild _mdiChild;
    private string _partNo = "";
        private string _screenType = "";

        private readonly ICommand saveECNOrPCNCommand;
        public ICommand SaveECNOrPCNCommand { get { return this.saveECNOrPCNCommand; } }

        private readonly ICommand addECNOrPCNCommand;
        public ICommand AddECNOrPCNCommand { get { return this.addECNOrPCNCommand; } }

        private readonly ICommand editECNOrPCNCommand;
        public ICommand EditECNOrPCNCommand { get { return this.editECNOrPCNCommand; } }

        private readonly ICommand selectChangeComboPartCommand;
        public ICommand SelectChangeComboPartCommand { get { return this.selectChangeComboPartCommand; } }

        private readonly ICommand lostFocusPartNoCommand;
        public ICommand LostFocusPartNoCommand { get { return this.lostFocusPartNoCommand; } }

        private readonly ICommand printECNOrPCNCommand;
        public ICommand PrintECNOrPCNCommand { get { return this.printECNOrPCNCommand; } }

        public Action CloseAction { get; set; }

        private readonly ICommand closeCommand;
        public ICommand CloseCommand { get { return this.closeCommand; } }
        private readonly ICommand _enterPartNumber;
        public ICommand EnterPartNumberCmb { get { return this._enterPartNumber; } }
        /// <summary>
        /// constructor for the class CustomerReferenceViewModel
        /// </summary>
        /// <param name="userInformation"></param>
        /// <param name="partNo"></param>
        /// <param name="screenType"></param>
        public CustomerReferenceViewModel(UserInformation userInformation, string partNo, string screenType, string partDesc)
        {
            try
            {
                _userInformation = userInformation;
                this.saveECNOrPCNCommand = new DelegateCommand(this.SaveECNOrPCN);
                this.addECNOrPCNCommand = new DelegateCommand(this.AddECNOrPCN);
                this.editECNOrPCNCommand = new DelegateCommand(this.EditECNOrPCN);
                this.selectChangeComboPartCommand = new DelegateCommand(this.SelectChangeComboPart);
                this.lostFocusPartNoCommand = new DelegateCommand(this.LostFocusPartNo);
                this.printECNOrPCNCommand = new DelegateCommand(this.PrintECNOrPCN);
                this.closeCommand = new DelegateCommand(this.Close);
                drwBll = new DrawingBll(_userInformation);
                this._logviewBll = new LogViewBll(_userInformation);
                _prod_mast_data_view = drwBll.GetPartNumberDetails();
                LoadComboECN();
                AddECNOrPCN();
                _partNo = partNo;
                this._enterPartNumber = new DelegateCommand<string>(this.EnterPartNumber);
                _screenType = screenType;
                //DDPCNECNModel = new DD_PCN();
                DDPCNECNModel.PART_NO = _partNo;
                DDPCNECNModel.PART_DESC = partDesc;
                GetCustomerDetails();
                //GetDDPCNList();
                ECNOrPCNLabel = (_screenType == "ECN" ? "ECN Sheet No : " : "PCN Sheet No");
                LostFocusPartNo();
                //PCNSheetNo = "";
                SFLDrawIssueNo = "";
                SFLDrawIssueNo1 = "";
                NotifyPropertyChanged("PCNSheetNo");
                NotifyPropertyChanged("SFLDrawIssueNo");
                NotifyPropertyChanged("SFLDrawIssueNo1");
                NotifyPropertyChanged("PartNo");
                EditECNOrPCN();
                SetComboHeader();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private bool _partNumberIsFocused = false;
        public bool PartNumberIsFocused
        {
            get { return _partNumberIsFocused; }
            set
            {
                _partNumberIsFocused = value;
                NotifyPropertyChanged("PartNumberIsFocused");
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Part No. is Required")]
        public string PartNo
        {
            get { return _partNo; }
            set
            {
                _partNo = value;
                NotifyPropertyChanged("PartNo");
            }
        }
        private void EnterPartNumber(string partNumber)
        {
            try
            {
                SelectChangeComboPart();
            }
            catch (Exception)
            {

            }

        }

        private string _sFLDrawIssueNo1;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pro.Drg.Issue No is Required")]
        public string SFLDrawIssueNo1
        {
            get { return _sFLDrawIssueNo1; }
            set
            {
                _sFLDrawIssueNo1 = value;
                NotifyPropertyChanged("SFLDrawIssueNo1");
            }
        }


        private string _sFLDrawIssueNo;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Pro.Seq.Drg.Issue No. is Required")]
        public string SFLDrawIssueNo
        {
            get { return _sFLDrawIssueNo; }
            set
            {
                _sFLDrawIssueNo = value;
                NotifyPropertyChanged("SFLDrawIssueNo");
            }
        }

       


        private string _pCNSheetNo;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Sheet No. is Required")]
        public string PCNSheetNo
        {
            get { return _pCNSheetNo; }
            set
            {
                _pCNSheetNo = value;
                NotifyPropertyChanged("PCNSheetNo");
            }
        }

        private DD_PCN _ddPcnEcnModel;
        public DD_PCN DDPCNECNModel
        {
            get { return _ddPcnEcnModel; }
            set
            {
                _ddPcnEcnModel = value;
                NotifyPropertyChanged("DDPCNECNModel");
            }
        }

        private string _eCNReferenceNo;
        public string ECNReferenceNo
        {
            get { return _eCNReferenceNo; }
            set
            {
                _eCNReferenceNo = value;
                NotifyPropertyChanged("ECNReferenceNo");
            }
        }

        private bool _re_PPAP = false;
        public bool RE_PPAP
        {
            get { return _re_PPAP; }
            set
            {
                _re_PPAP = value;
                _nOT_RE_PPAP = !_re_PPAP;
                NotifyPropertyChanged("RE_PPAP");
                NotifyPropertyChanged("NOT_RE_PPAP");
            }
        }

        private bool _nOT_RE_PPAP = false;

        public bool NOT_RE_PPAP
        {
            get { return _nOT_RE_PPAP; }
            set
            {
                _nOT_RE_PPAP = value;
                _re_PPAP = !_nOT_RE_PPAP;
                NotifyPropertyChanged("RE_PPAP");
                NotifyPropertyChanged("NOT_RE_PPAP");
            }
        }

        private OperationMode _actionMode = OperationMode.None;
        public OperationMode ActionMode
        {
            get { return _actionMode; }
            set
            {
                _actionMode = value;
                //SetProperty(ref _actionMode, value);
                NotifyPropertyChanged("ActionMode");
            }
        }

        private bool _addEnable;
        public bool AddEnable
        {
            get
            {
                return _addEnable;
            }
            set
            {
                _addEnable = value;
                NotifyPropertyChanged("AddEnable");
            }
        }

        private bool _editEnable;
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
        private bool _readOnly;
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
                NotifyPropertyChanged("ReadOnly");
            }
        }

        private Visibility _hasDropDownVisibility;
        public Visibility HasDropDownVisibility
        {
            get { return _hasDropDownVisibility; }
            set
            {
                _hasDropDownVisibility = value;
                NotifyPropertyChanged("HasDropDownVisibility");
            }
        }

        private DataView _dvDD_PCN;
        public DataView DvDD_PCN
        {
            get { return _dvDD_PCN; }
            set
            {
                _dvDD_PCN = value;
                NotifyPropertyChanged("DvDD_PCN");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsPartNo;
        public ObservableCollection<DropdownColumns> DropDownItemsPartNo
        {
            get
            {
                return _dropDownItemsPartNo;
            }
            set
            {
                _dropDownItemsPartNo = value;
                NotifyPropertyChanged("DropDownItemsPartNo");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderCompiledBy = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderCompiledBy
        {
            get { return this._dropDownHeaderCompiledBy; }
            set
            {
                this._dropDownHeaderCompiledBy = value;
                NotifyPropertyChanged("DropDownHeaderCompiledBy");
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

        private DataView _compiledByCombo;
        public DataView CompiledByCombo
        {
            get { return _compiledByCombo; }
            set
            {
                _compiledByCombo = value;
                NotifyPropertyChanged("CompiledByCombo");
            }
        }

        private DataView _approvedByCombo;
        public DataView ApprovedByCombo
        {
            get { return _approvedByCombo; }
            set
            {
                _approvedByCombo = value;
                NotifyPropertyChanged("ApprovedByCombo");
            }
        }

        private string _eCNOrPCNLabel = "ECN";
        public string ECNOrPCNLabel
        {
            get { return _eCNOrPCNLabel; }
            set
            {
                _eCNOrPCNLabel = value;
                NotifyPropertyChanged("ECNOrPCNLabel");
            }
        }

        private DataView _prod_mast_data_view;

        private DataView _dd_pcn_data_view;

        private void GetDDPCNList()
        {
            string eCNPCNDate = "";
            try
            {
                eCNPCNDate = (_screenType == "ECN" ? "ECN Date" : "PCN Date");
                DropDownItemsPartNo = new ObservableCollection<DropdownColumns>()
                {
                    new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "Part No.", ColumnWidth = "30*" },
                    new DropdownColumns() { ColumnName = "PRODUCT_CHANGE_NO", ColumnDesc = "Change Note", ColumnWidth = "40*" },
                    new DropdownColumns() { ColumnName = "SFL_DRAW_ISSUEDATE1", ColumnDesc = eCNPCNDate, ColumnWidth = "30*" },
                };

                //DvDD_PCN = drwBll.GetDDPCNList(_screenType);
                _dd_pcn_data_view = drwBll.GetDDPCNList(_screenType);
                DvDD_PCN = _dd_pcn_data_view;
                NotifyPropertyChanged("DropDownItemsPartNo");
                NotifyPropertyChanged("DvDD_PCN");
                PCNSheetNo = "";
                //SFLDrawIssueNo = DBNull.Value;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Get ECN or PCN Details
        /// </summary>
        private void GetECNMPSDetails()
        {
            try
            {
                string ecnRefNo = "";
                DDPCNECNModel = drwBll.GetECNMPSDetails(DDPCNECNModel.SNO, _screenType, ref ecnRefNo)[0];
                ECNReferenceNo = ecnRefNo;
                //MPSReferenceNo = mpsRefNo;
                //DDPCNECNModel.PART_NO = _partNo;
                if (DDPCNECNModel.PART_NO.ToValueAsString().Trim() != "")
                {
                    if (DDPCNECNModel.RE_PPAP == true)
                    {
                        RE_PPAP = true;
                        NOT_RE_PPAP = false;
                    }
                    else
                    {
                        RE_PPAP = false;
                        NOT_RE_PPAP = true;
                    }
                }
                DDPCNECNModel.CONTROL_PLAN = (DDPCNECNModel.CONTROL_PLAN == null ? false : DDPCNECNModel.CONTROL_PLAN);
                DDPCNECNModel.OTHERS = (DDPCNECNModel.OTHERS == null ? false : DDPCNECNModel.OTHERS);
                DDPCNECNModel.PFD = (DDPCNECNModel.PFD == null ? false : DDPCNECNModel.PFD);
                DDPCNECNModel.PFMEA = (DDPCNECNModel.PFMEA == null ? false : DDPCNECNModel.PFMEA);
                DDPCNECNModel.PRODUCT_DWG = (DDPCNECNModel.PRODUCT_DWG == null ? false : DDPCNECNModel.PRODUCT_DWG);
                DDPCNECNModel.RE_PPAP = (DDPCNECNModel.RE_PPAP == null ? false : DDPCNECNModel.RE_PPAP);
                DDPCNECNModel.SEQUENCE_DWG = (DDPCNECNModel.SEQUENCE_DWG == null ? false : DDPCNECNModel.SEQUENCE_DWG);
                DDPCNECNModel.ROUTING_TAG = (DDPCNECNModel.ROUTING_TAG == null ? false : DDPCNECNModel.ROUTING_TAG);
                DDPCNECNModel.FINISH_CODE_SAP_DWG = (DDPCNECNModel.FINISH_CODE_SAP_DWG == null ? false : DDPCNECNModel.FINISH_CODE_SAP_DWG);
                DDPCNECNModel.WORK_INSTRUCTION = (DDPCNECNModel.WORK_INSTRUCTION == null ? false : DDPCNECNModel.WORK_INSTRUCTION);
                DDPCNECNModel.SAP_SEQ_DWG_ISSUE_NO_UPD = (DDPCNECNModel.SAP_SEQ_DWG_ISSUE_NO_UPD == null ? false : DDPCNECNModel.SAP_SEQ_DWG_ISSUE_NO_UPD);
                DDPCNECNModel.TOOL_DWG = (DDPCNECNModel.TOOL_DWG == null ? false : DDPCNECNModel.TOOL_DWG);
                DDPCNECNModel.GAUGE_DWG = (DDPCNECNModel.GAUGE_DWG == null ? false : DDPCNECNModel.GAUGE_DWG);
                DDPCNECNModel.ALREADY_IN_EFFECT = (DDPCNECNModel.ALREADY_IN_EFFECT == null ? false : DDPCNECNModel.ALREADY_IN_EFFECT);
                DDPCNECNModel.IMMEDIATE = (DDPCNECNModel.IMMEDIATE == null ? false : DDPCNECNModel.IMMEDIATE);
                DDPCNECNModel.FROM_NEXT_HT_CYCLE = (DDPCNECNModel.FROM_NEXT_HT_CYCLE == null ? false : DDPCNECNModel.FROM_NEXT_HT_CYCLE);
                DDPCNECNModel.FROM_NEXT_FORGING = (DDPCNECNModel.FROM_NEXT_FORGING == null ? false : DDPCNECNModel.FROM_NEXT_FORGING);
                PartNo = DDPCNECNModel.PART_NO;
                SFLDrawIssueNo = DDPCNECNModel.SFL_DRAW_ISSUENO.ToValueAsString().Trim();
                SFLDrawIssueNo1 = DDPCNECNModel.SFL_DRAW_ISSUENO1.ToValueAsString().Trim();
                PCNSheetNo = DDPCNECNModel.PRODUCT_CHANGE_NO.ToValueAsString().Trim();
                //NotifyPropertyChanged("DrwModel");
                NotifyPropertyChanged("DDPCNECNModel");
                NotifyPropertyChanged("PartNo");
                NotifyPropertyChanged("SFLDrawIssueNo");
                NotifyPropertyChanged("SFLDrawIssueNo1");
                NotifyPropertyChanged("PCNSheetNo");
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        /// <summary>
        /// Get customer details
        /// </summary>
        private void GetCustomerDetails()
        {
            try
            {
                string ecnRefNo = "";
                //     DDPCNECNModel = new DD_PCN();
                //DDPCNECNModel.PART_NO = _partNo;
                drwBll.GetCustomerDetails(DDPCNECNModel);

                System.Resources.ResourceManager myManager;
                myManager = new System.Resources.ResourceManager(typeof(ProcessDesigner.Properties.Resources));
                string conStr = myManager.GetString("ConnectionString");
                DataTable dt = new DataTable();
                DataAccessLayer dal = new DataAccessLayer(conStr);
               dt = dal.Get_Ecn_Pcn_Details(DDPCNECNModel.PART_NO);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DDPCNECNModel.SFL_DRAW_ISSUENO = DBNull.Value.Equals(dt.Rows[0]["SEQ_ISSUE_NO"]) == true ? 0 : Convert.ToDecimal(dt.Rows[0]["SEQ_ISSUE_NO"]);
                    DDPCNECNModel.SFL_DRAW_ISSUENO1 = DBNull.Value.Equals(dt.Rows[0]["PRD_ISSUE_NO"]) == true ? 0 : Convert.ToDecimal(dt.Rows[0]["PRD_ISSUE_NO"]);
                    if (DBNull.Value.Equals(dt.Rows[0]["SEQ_ISSUE_DATE"]) == true)
                        DDPCNECNModel.SFL_DRAW_ISSUE_DATE = null;
                    else
                        DDPCNECNModel.SFL_DRAW_ISSUE_DATE = Convert.ToDateTime(dt.Rows[0]["SEQ_ISSUE_DATE"]);
                    if (DBNull.Value.Equals(dt.Rows[0]["PRD_ISSUE_DATE"]) == true)
                        DDPCNECNModel.SFL_DRAW_ISSUEDATE1 = null;
                    else
                        DDPCNECNModel.SFL_DRAW_ISSUEDATE1 = Convert.ToDateTime(dt.Rows[0]["PRD_ISSUE_DATE"]);

                    DDPCNECNModel.CUST_DWG_NO = DBNull.Value.Equals(dt.Rows[0]["CUST_DWG_NO"]) == true ? "" : dt.Rows[0]["CUST_DWG_NO"].ToString();
                    DDPCNECNModel.CUST_DWG_NO_ISSUE = DBNull.Value.Equals(dt.Rows[0]["CUST_DWG_NO_ISSUE"]) == true ? "" : dt.Rows[0]["CUST_DWG_NO_ISSUE"].ToString();
                }

                //NotifyPropertyChanged("DrwModel");
                PartNo = DDPCNECNModel.PART_NO;
                SFLDrawIssueNo = DDPCNECNModel.SFL_DRAW_ISSUENO.ToValueAsString().Trim();
                SFLDrawIssueNo1 = DDPCNECNModel.SFL_DRAW_ISSUENO1.ToValueAsString().Trim();
                PCNSheetNo = DDPCNECNModel.PRODUCT_CHANGE_NO.ToValueAsString().Trim();
                //NotifyPropertyChanged("DrwModel");
                NotifyPropertyChanged("DDPCNECNModel");
                NotifyPropertyChanged("PartNo");
                NotifyPropertyChanged("SFLDrawIssueNo");
                NotifyPropertyChanged("SFLDrawIssueNo1");
                NotifyPropertyChanged("PCNSheetNo");

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void ClearAll()
        {
            try
            {
                DDPCNECNModel = new DD_PCN();
                ClearAllFields(DDPCNECNModel);
                PCNSheetNo = "";
                SFLDrawIssueNo = "";
                SFLDrawIssueNo1 = "";
                PartNo = "";
                RE_PPAP = false;
                NOT_RE_PPAP = true;
                NotifyPropertyChanged("PCNSheetNo");
                NotifyPropertyChanged("SFLDrawIssueNo");
                NotifyPropertyChanged("SFLDrawIssueNo1");
                NotifyPropertyChanged("PartNo");
                NotifyPropertyChanged("DDPCNECNModel");
                NotifyPropertyChanged("RE_PPAP");
                NotifyPropertyChanged("NOT_RE_PPAP");
                NotifyPropertyChanged("ReadOnly");
                NotifyPropertyChanged("AddEnable");
                NotifyPropertyChanged("EditEnable");
                NotifyPropertyChanged("SaveEnable");
                NotifyPropertyChanged("HasDropDownVisibility");
                PartNumberIsFocused = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        private void ClearAllFields(DD_PCN entity)
        {
            try
            {
                entity.ECN_REFERENCE_NO = "";
                entity.PART_NO = "";
                entity.ACTUAL_CHANGE_IMP = "";
                entity.APPROVED_BY = "";
                entity.CHANGE_EFFECTIVE = "";
                entity.COMPILED_BY = "";
                entity.CONTROL_PLAN = false;
                entity.COST_DESC = "";
                entity.CUST_NAME = "";
                entity.CUST_PART_NO = "";
                entity.DATE_OF_PCN = null;
                entity.DATE_OF_SIGN = null;
                entity.DISPOSITION = "";
                entity.GAUGE_DWG = null;
                entity.INFGG_INITIAL = "";
                entity.INFGG_QTY = "";
                entity.INHEAT_TREATMENT_INITIAL = "";
                entity.INHEAT_TREATMENT_QTY = "";
                entity.INWIP_INITIAL = "";
                entity.INWIP_QTY = "";
                entity.MANUFACTURE_PROCESS = "";
                entity.NATURE_OF_CHANGE = "";
                entity.OTHERS = false;
                entity.PART_DESC = "";
                entity.PFD = false;
                entity.PFMEA = false;
                entity.PRODUCT_CHANGE_NO = "";
                entity.PRODUCT_DWG = false;
                entity.RE_PPAP = false;
                entity.RESON_FOR_CHANGE = "";
                entity.SEQUENCE_DWG = false;
                entity.SFL_DRAW_ISSUE_DATE = null;
                entity.SFL_DRAW_ISSUEDATE1 = null;
                entity.SFL_DRAW_ISSUENO = null;
                entity.SFL_DRAW_ISSUENO1 = null;
                //lstEntity.SNO = ;
                entity.CUST_DWG_NO = "";
                entity.CUST_DWG_NO_ISSUE = "";
                entity.CUST_ISSUE_NO = null;
                entity.ROUTING_TAG = false;
                entity.FINISH_CODE_SAP_DWG = false;
                entity.WORK_INSTRUCTION = false;
                entity.SAP_SEQ_DWG_ISSUE_NO_UPD = false;
                entity.TOOL_DWG = false;
                ECNReferenceNo = "";
                entity.SNO = 0;
                entity.GAUGE_DWG = false;
                entity.ALREADY_IN_EFFECT = false;
                entity.IMMEDIATE = false;
                entity.FROM_NEXT_HT_CYCLE = false;
                entity.FROM_NEXT_FORGING = false;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SaveECNOrPCN()
        {
            string smessage = "";
            try
            {
                PartNumberIsFocused = true;
                drwBll.IsDefaultSubmitRequired = true;
                //if (DDPCNECNModel.PART_NO.ToValueAsString().Trim() == "")
                if (PartNo.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Part No."));
                    return;
                }

                smessage = (_screenType == "ECN" ? "ECN" : "PCN");
                if (PCNSheetNo.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty(smessage + " Number"));
                    return;
                }
                //if (DDPCNECNModel.SFL_DRAW_ISSUENO.ToValueAsString().Trim() == "")
                if (SFLDrawIssueNo.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Pro.Seq.Drg.Issue No."));
                    return;
                }

                if (SFLDrawIssueNo1.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Pro.Drg.Issue No."));
                    return;
                }

                if (ActionMode == OperationMode.AddNew)
                {
                    LostFocusPartNo();
                }
                //LostFocusPartNo();
                DDPCNECNModel.PART_NO = PartNo;
                DDPCNECNModel.SFL_DRAW_ISSUENO = Convert.ToDecimal(SFLDrawIssueNo);
                DDPCNECNModel.SFL_DRAW_ISSUENO1 = Convert.ToDecimal(SFLDrawIssueNo1);
                DDPCNECNModel.PRODUCT_CHANGE_NO = PCNSheetNo;
                if (DDPCNECNModel.PART_NO.ToValueAsString() != "")
                {
                    if (RE_PPAP == true)
                    {
                        DDPCNECNModel.RE_PPAP = true;
                    }
                    else
                    {
                        DDPCNECNModel.RE_PPAP = false;
                    }
                    if (DDPCNECNModel.PART_NO.ToValueAsString().Trim() != "" &&
                        DDPCNECNModel.PRODUCT_CHANGE_NO.ToValueAsString().Trim() != "" &&
                        DDPCNECNModel.SFL_DRAW_ISSUENO.ToValueAsString().Trim() != "")
                    {
                        Progress.ProcessingText = PDMsg.ProgressUpdateText;
                        Progress.Start();
                        drwBll.SaveECNMPSDetails(DDPCNECNModel, DDPCNECNModel.PART_NO, _screenType, ActionMode);

                        Progress.End();
                    }
                    if (ActionMode == OperationMode.AddNew)
                    {
                        ShowInformationMessage(PDMsg.SavedSuccessfully);
                        _logviewBll.SaveLog(DDPCNECNModel.PART_NO, "ECN/PCN");
                    }
                    else
                    {
                        ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                        _logviewBll.SaveLog(DDPCNECNModel.PART_NO, "ECN/PCN");
                    }
                    if (isPrintClicked != true)
                    {
                        // AddECNOrPCN();
                    }

                }

                try
                {
                    if (ActionMode == OperationMode.AddNew)
                    {
                        string oldPartNo = PartNo;
                        string oldPCNSheetNo = PCNSheetNo;
                        int i = 0;
                        PartNumberIsFocused = true;
                        EditECNOrPCN();
                        foreach (DataRowView drView in DvDD_PCN)
                        {

                            if (drView["PRODUCT_CHANGE_NO"].ToString() == oldPCNSheetNo && drView["PART_NO"].ToString() == oldPartNo)
                            {
                                PartNo = oldPartNo;
                                NotifyPropertyChanged("PartNo");
                                SelectedRow = DvDD_PCN[i];
                                SelectChangeComboPart();
                                //NotifyPropertyChanged("SelectedRow");
                                //GetECNMPSDetails();
                                break;
                            }
                            i = i + 1;
                        }
                    }
                }
                catch (Exception ex1)
                {

                }

                //NotifyPropertyChanged("SelectedRow");


            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void AddECNOrPCN()
        {
            try
            {
                drwBll.IsDefaultSubmitRequired = false;
                ReadOnly = false;
                ActionMode = OperationMode.AddNew;
                AddEnable = false;
                EditEnable = true;
                SaveEnable = true;
                PrintEnable = true;
                HasDropDownVisibility = Visibility.Hidden;
                DropDownItemsPartNo = new ObservableCollection<DropdownColumns>()
                {
                    new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "Part No.", ColumnWidth = "30*" },
                    new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "Part Description", ColumnWidth = "40*" },
                };
                DvDD_PCN = _prod_mast_data_view;
                ClearAll();
                NotifyPropertyChanged("DropDownItemsPartNo");
                NotifyPropertyChanged("DvDD_PCN");
                PartNumberIsFocused = true;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void EditECNOrPCN()
        {
            drwBll.IsDefaultSubmitRequired = false;
            ClearAll();
            string eCNPCNDate = (_screenType == "ECN" ? "ECN Date" : "PCN Date");
            ActionMode = OperationMode.Edit;
            HasDropDownVisibility = Visibility.Visible;
            ReadOnly = true;
            AddEnable = true;
            EditEnable = false;
            SaveEnable = true;
            PrintEnable = true;
            GetDDPCNList();
            NotifyPropertyChanged("DvDD_PCN");
            NotifyPropertyChanged("DropDownItemsPartNo");
            //if (ActionPermission.Edit == false && ActionPermission.View == true)
            //{
            //    SaveEnable = false;
            //}
            //if (ActionPermission.Edit == false)
            //{
            //    SaveEnable = false;
            //}
            ClearAll();
        }

        /// <summary>
        /// use to load the details for the selected part no
        /// </summary>
        private void SelectChangeComboPart()
        {
            try
            {
                drwBll.IsDefaultSubmitRequired = false;
                DDPCNECNModel = new DD_PCN();
                ClearAllFields(DDPCNECNModel);
                DDPCNECNModel.PART_NO = (SelectedRow != null) ? SelectedRow["PART_NO"].ToString() : string.Empty;
                DDPCNECNModel.PART_DESC = (SelectedRow != null) ? SelectedRow["PART_DESC"].ToString() : string.Empty;
                if (ActionMode == OperationMode.AddNew)
                {
                    GetCustomerDetails();
                    LostFocusPartNo();
                }
                else
                {
                    //System.Resources.ResourceManager myManager;
                    //myManager = new System.Resources.ResourceManager(typeof(ProcessDesigner.Properties.Resources));
                    //string conStr = myManager.GetString("ConnectionString");

                    //DataAccessLayer dal = new DataAccessLayer(conStr);
                    //DDPCNECNModel.SNO = dal.Get_Ecn_Pcn_Sel_Sno(DDPCNECNModel.PART_NO, _screenType);
                    DDPCNECNModel.SNO = (SelectedRow != null) ? SelectedRow["SNO"].ToString().ToIntValue() : 0;
                    GetECNMPSDetails();
                }
                NotifyPropertyChanged("DDPCNECNModel");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        /// <summary>
        /// use to load the max number(Sheet No) for the selected part no
        /// </summary>
        private void LostFocusPartNo()
        {
            try
            {
                if (ActionMode == OperationMode.AddNew)
                {
                    //DDPCNECNModel.PRODUCT_CHANGE_NO = drwBll.GetMaxSheetNo(DDPCNECNModel.PART_NO.ToValueAsString().Trim()).ToValueAsString();
                    PCNSheetNo = drwBll.GetMaxSheetNo(DDPCNECNModel.PART_NO.ToValueAsString().Trim(), _screenType).ToValueAsString();
                    NotifyPropertyChanged("PCNSheetNo");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void LoadComboECN()
        {
            try
            {
                ApprovedByCombo = drwBll.GetApprovedByCombo();
                CompiledByCombo = drwBll.GetCompiledByCombo();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        private bool isPrintClicked = false;
        private void PrintECNOrPCN()
        {
            try
            {
                string smessage = "";
                drwBll.IsDefaultSubmitRequired = false;
                if (PartNo.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Part No."));
                    return;
                }
          //  CheckBox chkdraw = (CheckBox)_mdiChild.FindName("chkprddrawing");
          //      if (_screenType=="PCN")
          //       {
          //chkdraw.Visibility = System.Windows.Visibility.Hidden;    
          //       }
                smessage = (_screenType == "ECN" ? "ECN" : "PCN");
                if (PCNSheetNo.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty(smessage + " Number"));
                    return;
                }
                //if (DDPCNECNModel.SFL_DRAW_ISSUENO.ToValueAsString().Trim() == "")
                if (SFLDrawIssueNo.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Pro.Seq.Drg.Issue No."));
                    return;
                }

                if (SFLDrawIssueNo1.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Pro.Drg.Issue No."));
                    return;
                }
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    isPrintClicked = true;
                    SaveECNOrPCN();

                }
                else
                {
                    if (AddEnable)
                    {
                        DDPCNECNModel.SNO = (SelectedRow != null) ? SelectedRow["SNO"].ToString().ToIntValue() : 0;
                        //System.Resources.ResourceManager myManager;
                        //myManager = new System.Resources.ResourceManager(typeof(ProcessDesigner.Properties.Resources));
                        //string conStr = myManager.GetString("ConnectionString");

                        //DataAccessLayer dal = new DataAccessLayer(conStr);
                        //DDPCNECNModel.SNO = dal.Get_Ecn_Pcn_Sel_Sno(DDPCNECNModel.PART_NO, _screenType);
                        GetECNMPSDetails();
                    }

                }
                DDPCNECNModel.SFL_DRAW_ISSUENO = Convert.ToDecimal(SFLDrawIssueNo);
                DDPCNECNModel.SFL_DRAW_ISSUENO1 = Convert.ToDecimal(SFLDrawIssueNo1);
                DDPCNECNModel.PRODUCT_CHANGE_NO = PCNSheetNo;


                DataTable dtData = new DataTable();
                List<DD_PCN> lstDDPCN = new List<DD_PCN>();
                EXHIBIT_DOC entity = new EXHIBIT_DOC();
                string docName = "ECN";
                string formatNo = "";
                string revisionNo = "";
                lstDDPCN.Add(DDPCNECNModel);
                dtData = Essential.ToDataTable(lstDDPCN);

                if (dtData.Rows.Count > 0)
                {
                    foreach (DataColumn dc in dtData.Columns)
                    {
                        if (dtData.Rows[0][dc].ToValueAsString().Trim() == "")
                        {
                            dtData.Rows[0][dc] = "";
                        }
                    }
                    dtData.Rows[0]["SFL_DRAW_ISSUE_DATE"] = ReplaceString(dtData.Rows[0]["SFL_DRAW_ISSUE_DATE"].ToValueAsString());
                    dtData.Rows[0]["SFL_DRAW_ISSUEDATE1"] = ReplaceString(dtData.Rows[0]["SFL_DRAW_ISSUEDATE1"].ToValueAsString());
                    dtData.Rows[0]["DATE_OF_IMPLEMENTATION"] = ReplaceString(dtData.Rows[0]["DATE_OF_IMPLEMENTATION"].ToValueAsString());
                    dtData.Rows[0]["DATE_OF_PCN"] = ReplaceString(dtData.Rows[0]["DATE_OF_PCN"].ToValueAsString());
                    dtData.Rows[0]["INVOICE_DATE"] = ReplaceString(dtData.Rows[0]["INVOICE_DATE"].ToValueAsString());
                    dtData.Rows[0]["SIGNATURE_DATE"] = ReplaceString(dtData.Rows[0]["SIGNATURE_DATE"].ToValueAsString());
                    if (!AddEnable)
                    {
                        dtData.Rows[0]["RE_PPAP"] = RE_PPAP;
                    }
                }

                docName = (_screenType == "ECN" ? "ECN" : "PCN");
                try
                {
                    entity = (from row in drwBll.DB.EXHIBIT_DOC
                              where row.DOC_NAME == docName && (row.DELETE_FLAG == false || row.DELETE_FLAG == null)
                              select row).FirstOrDefault();
                    if (entity != null)
                    {
                        formatNo = entity.EX_NO.ToValueAsString().Trim();
                        revisionNo = entity.REVISION_NO.ToValueAsString().Trim();
                        //revisionNo = entity.
                    }
                }
                catch (Exception ex1)
                {

                }
                frmReportViewer report = new frmReportViewer(dtData, "ECNPCN", _screenType, formatNo, revisionNo);
                report.ShowDialog();
                if (isPrintClicked)
                {
                    //AddECNOrPCN();
                    isPrintClicked = false;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private string ReplaceString(string value)
        {
            try
            {
                if (value.Length > 0)
                {
                    value = value.ToUpper().Replace("} 12:00:00 AM", "").Replace("{0:", "").Trim();
                    value = value.Substring(0, 10);
                }
                return value;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private void Close()
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void SetComboHeader()
        {
            DropDownHeaderCompiledBy = new ObservableCollection<DropdownColumns>
                 {               
                new DropdownColumns { ColumnName = "FULL_NAME", ColumnDesc = "Full Name", ColumnWidth = 100 },
                new DropdownColumns { ColumnName = "DESIGNATION", ColumnDesc = "Designation", ColumnWidth = "1*" }
                 };
        }
    }
}
