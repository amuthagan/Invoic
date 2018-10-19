using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcessDesigner.Model;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;

using System.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Windows;
using ProcessDesigner.UserControls;
using System.Collections.ObjectModel;
//using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using ProcessDesigner.DAL;

namespace ProcessDesigner.ViewModel
{
    public class OperatorQualityAssuranceViewModel : ViewModelBase
    {
        private OperatorQualityAssurance bll = null;
        const string RIGHTS_NAME = "OperatorQualityAssurance";
        private const string REPORT_NAME = "OPERATOR_QUALITY_ASSURANCE_CHART";
        private const string REPORT_TITLE = "Operator Quality Assurance Chart";

        WPF.MDI.MdiChild mdiChild = null;
        UserInformation _userInformation = null;
        System.Windows.Window self = null;

        public OperatorQualityAssuranceViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, int entityPrimaryKey,
            OperationMode operationMode, string title = "Operator Quality Assurance Chart", System.Windows.Window self = null)
        {
            _userInformation = userInformation;
            this.mdiChild = mdiChild;
            this.self = self;

            bll = new OperatorQualityAssurance(userInformation);
            MandatoryFields = new OperatorQualityAssuranceModel();

            this.printCommand = new DelegateCommand(this.PrintSubmitCommand);
            this.saveCommand = new DelegateCommand(this.saveSubmitCommand);
            this.exportToExcelCommand = new DelegateCommand(this.ExportToExcelSubmitCommand);
            // Jeyan
            //PartNumberDataSource = bll.GetProductMasterDetailsByPartNumber().ToDataTableWithType<PRD_MAST>().DefaultView;
            PartNumberDataSource = GetProductMasterDetailsByPartNumberDV();
            PartNumberDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "Part Number", ColumnWidth = "1*" },
                            //new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "Part Description", ColumnWidth = "100" },
                        };

            this.partNumberEndEditCommand = new DelegateCommand(this.partNumberEndEdit);
            this.partNumberSelectedItemChangedCommand = new DelegateCommand(this.partNumberChanged);

            WorkOrderDataSource = null;
            WorkOrderDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "WORK_ORDER_NO", ColumnDesc = "Work Order", ColumnWidth = "1*" },
                        };

            this._workorderEndEditCommand = new DelegateCommand(this.WorkOrderEndEdit);
            this._workorderSelectedItemChangedCommand = new DelegateCommand(this.WorkOrderChanged);


            SequenceDataSource = null;
            SequenceDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SEQ_NO", ColumnDesc = "Sequence", ColumnWidth = "1*" },
                        };

            this._sequenceEndEditCommand = new DelegateCommand(this.SequenceEndEdit);
            this._sequenceSelectedItemChangedCommand = new DelegateCommand(this.SequenceChanged);

            WireDiaDataSource = null;
            WireDiaDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "TS_ISSUE_ALTER", ColumnDesc = "Wire Dia", ColumnWidth = "1*" },
                        };

            this._wirediaEndEditCommand = new DelegateCommand(this.WireDiaEndEdit);
            this._wirediaSelectedItemChangedCommand = new DelegateCommand(this.WireDiaChanged);

            CostCentreDataSource = null;
            CostCentreDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CC_CODE", ColumnDesc = "Cost Centre", ColumnWidth = "1*" },
                        };

            this._costcentreEndEditCommand = new DelegateCommand(this.CostCentreEndEdit);
            this._costcentreSelectedItemChangedCommand = new DelegateCommand(this.CostCentreChanged);

            DataTable dtShift = new DataTable("SHIFT");
            dtShift.Columns.Add("SHIFT_NO", typeof(string));
            dtShift.Rows.Add("I");
            dtShift.Rows.Add("II");
            dtShift.Rows.Add("III");
            ShiftDataSource = dtShift.DefaultView;
            ShiftDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SHIFT_NO", ColumnDesc = "Shift", ColumnWidth = "1*" },
                        };

            this._shiftEndEditCommand = new DelegateCommand(this.ShiftEndEdit);
            this._shiftSelectedItemChangedCommand = new DelegateCommand(this.ShiftChanged);
            if (ShiftDataSource.IsNotNullOrEmpty() && ShiftDataSource.Table.IsNotNullOrEmpty() && ShiftDataSource.Table.Rows.Count > 0)
            {
                MandatoryFields.SHIFT_NO = ShiftDataSource.Table.Rows[0]["SHIFT_NO"].ToValueAsString();
            }

            MandatoryFields.TODAY_DATE = bll.ServerDateTime();
            PartNumberIsFocused = true;
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return System.Windows.MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        private MessageBoxResult ShowWarningMessage(string _showMessage, MessageBoxButton messageBoxButton)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return System.Windows.MessageBox.Show(_showMessage, ApplicationTitle, messageBoxButton, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        public Action CloseAction { get; set; }
        private RelayCommand _onCancelCommand;
        private void CloseSubmitCommand()
        {
            try
            {

                ActionMode = OperationMode.AddNew;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private OperatorQualityAssuranceModel _mandatoryFields = null;
        public OperatorQualityAssuranceModel MandatoryFields
        {
            get
            {
                return _mandatoryFields;
            }
            set
            {
                _mandatoryFields = value;
                NotifyPropertyChanged("MandatoryFields");
            }
        }

        private OperationMode _actionMode = OperationMode.None;
        public OperationMode ActionMode
        {
            get { return _actionMode; }
            set
            {

            }
        }

        #region Part Number
        private DataView _partNumber = null;
        public DataView PartNumberDataSource
        {
            get
            {
                return _partNumber;
            }
            set
            {
                _partNumber = value;
                NotifyPropertyChanged("PartNumberDataSource");
            }
        }

        private DataRowView _partNumberSelectedRow;
        public DataRowView PartNumberSelectedRow
        {
            get
            {
                return _partNumberSelectedRow;
            }
            set
            {
                _partNumberSelectedRow = value;
            }
        }

        private DataRowView _sNoSelectedItem;
        public DataRowView SNoSelectedItem
        {
            get
            {
                return _sNoSelectedItem;
            }
            set
            {
                _sNoSelectedItem = value;
            }
        }

        private Visibility _partNumberHasDropDownVisibility = Visibility.Visible;
        public Visibility PartNumberHasDropDownVisibility
        {
            get { return _partNumberHasDropDownVisibility; }
            set
            {
                _partNumberHasDropDownVisibility = value;
                NotifyPropertyChanged("PartNumberHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _partNumberDropDownItems;
        public ObservableCollection<DropdownColumns> PartNumberDropDownItems
        {
            get
            {
                return _partNumberDropDownItems;
            }
            set
            {
                _partNumberDropDownItems = value;
                OnPropertyChanged("PartNumberDropDownItems");
            }
        }

        List<PROCESS_SHEET> lstPROCESS_SHEET = null;
        List<WORK_ORDER_MAIN> lstWorkOrderDetails = null;

        private readonly ICommand partNumberSelectedItemChangedCommand;
        public ICommand PartNumberSelectedItemChangedCommand { get { return this.partNumberSelectedItemChangedCommand; } }
        private void partNumberChanged()
        {
            //Clear(); //Jeyan
            if (!MandatoryFields.IsNotNullOrEmpty() || !MandatoryFields.PART_NO.IsNotNullOrEmpty() || !PartNumberDataSource.IsNotNullOrEmpty()) return;

            WorkOrderDataSource = null;
            SequenceDataSource = null;
            WireDiaDataSource = null;
            CostCentreDataSource = null;

            MandatoryFields.SEQ_NO = null;
            MandatoryFields.CC_CODE = null;
            MandatoryFields.TS_ISSUE_ALTER = null;
            MandatoryFields.WORK_ORDER_NO = null;
            MandatoryFields.QUANTITY = null;
            //MandatoryFields.CCF = null;
            //MandatoryFields.NEXT_OPERATION = null;
            MandatoryFields.PART_DESC = null;
            MandatoryFields.RAW_MATERIAL = null;
            MandatoryFields.MACHINE_NAME = null;
            MandatoryFields.OPERATION_CODE = null;
            MandatoryFields.OPERATION_DESC = null;
            MandatoryFields.NEXT_OPERATION_DESC = null;
            MandatoryFields.SEQUENCE_DRAWING_ISSUE_NO = null;
            MandatoryFields.ROUTE_NO = null;
            MandatoryFields.SEQUENCE_DRAWING_ISSUE_DATE = null;
            MandatoryFields.IS_SAVE_ENABLED = true;
            DataView partNumberDataView = PartNumberDataSource.Table.Copy().DefaultView;
            //Clear();
            partNumberDataView.RowFilter = "PART_NO ='" + MandatoryFields.PART_NO + "'";
            if (partNumberDataView.Count == 0)
            {
                partNumberDataView.RowFilter = null;
                return;
            };

            MandatoryFields.PART_DESC = partNumberDataView[0]["PART_DESC"].ToValueAsString();
            partNumberDataView.RowFilter = null;

            List<PROCESS_MAIN> lstPROCESS_MAIN = (from row in bll.GetProcessSheetMainDetailsByPartNumber(new PRD_MAST() { PART_NO = MandatoryFields.PART_NO })
                                                  where Convert.ToBoolean(Convert.ToInt16(row.CURRENT_PROC)) == true && row.PART_NO == MandatoryFields.PART_NO
                                                  select row).ToList<PROCESS_MAIN>();

            MandatoryFields.ROUTE_NO = string.Empty;
            if (lstPROCESS_MAIN.IsNotNullOrEmpty() && lstPROCESS_MAIN.Count > 0)
            {
                MandatoryFields.ROUTE_NO = lstPROCESS_MAIN[0].ROUTE_NO.ToValueAsString();
            }

            List<PRD_DWG_ISSUE> lstSequenceDrawingIssueEntities = bll.GetDrawingIssueDetailsByPartNumber(new PRD_DWG_ISSUE()
            {
                PART_NO = MandatoryFields.PART_NO,
                DWG_TYPE = 1
            });

            DateTime? sequence_drawing_issue_date_max = null;
            if (lstSequenceDrawingIssueEntities.IsNotNullOrEmpty() && lstSequenceDrawingIssueEntities.Count > 0)
            {
                sequence_drawing_issue_date_max = (from row in lstSequenceDrawingIssueEntities.AsEnumerable()
                                                   where row.DWG_TYPE == 1 && row.PART_NO == MandatoryFields.PART_NO
                                                   select row.ISSUE_DATE).Max();
            }

            lstSequenceDrawingIssueEntities = (from row in lstSequenceDrawingIssueEntities.AsEnumerable()
                                               where row.DWG_TYPE == 1 && row.PART_NO == MandatoryFields.PART_NO && row.ISSUE_DATE == sequence_drawing_issue_date_max
                                               orderby row.ISSUE_DATE descending
                                               select row).ToList<PRD_DWG_ISSUE>();

            MandatoryFields.SEQUENCE_DRAWING_ISSUE_NO = string.Empty;
            MandatoryFields.SEQUENCE_DRAWING_ISSUE_DATE = null;
            if (lstSequenceDrawingIssueEntities.IsNotNullOrEmpty() && lstSequenceDrawingIssueEntities.Count > 0)
            {
                MandatoryFields.SEQUENCE_DRAWING_ISSUE_NO = lstSequenceDrawingIssueEntities[0].ISSUE_NO.ToValueAsString();
                MandatoryFields.SEQUENCE_DRAWING_ISSUE_DATE = lstSequenceDrawingIssueEntities[0].ISSUE_DATE;
            }

            lstWorkOrderDetails = bll.GetWorkOrderDetailsByPartNumber(new WORK_ORDER_MAIN() { PART_NO = MandatoryFields.PART_NO });
            WorkOrderDataSource = lstWorkOrderDetails.IsNotNullOrEmpty() ? lstWorkOrderDetails.ToDataTableWithType<WORK_ORDER_MAIN>().DefaultView : null;

            List<OQA_CCF> lstOQA_CCF = bll.GetCCFDetailsByPartNumber(new OQA_CCF()
            {
                PART_NO = MandatoryFields.PART_NO
            });

            MandatoryFields.CCF = string.Empty;
            if (lstOQA_CCF.IsNotNullOrEmpty() && lstOQA_CCF.Count > 0)
            {
                MandatoryFields.CCF = lstOQA_CCF[0].CCF;
            }

            if (!MandatoryFields.ROUTE_NO.IsNotNullOrEmpty())
            {
                MandatoryFields.GridData = null;
                return;
            }

            //Added by Jeyan - By default seq no is 20

            if (SNoSelectedItem == null)
            {
                MandatoryFields.SEQ_NO = "20";
                DataTable dt = new DataTable();
                dt.Columns.Add("SEQ_NO", typeof(string));
                dt.Rows.Add("20");
                SNoSelectedItem = dt.DefaultView[0];
            }

            lstPROCESS_SHEET = bll.GetProcessSheetDetailsByPartNumber(new PROCESS_SHEET() { PART_NO = MandatoryFields.PART_NO, ROUTE_NO = MandatoryFields.ROUTE_NO.ToValueAsString().ToDecimalValue() });
            SequenceDataSource = lstPROCESS_SHEET.IsNotNullOrEmpty() ? lstPROCESS_SHEET.ToDataTableWithType<PROCESS_SHEET>().DefaultView : null;

            string rawmaterial_code = (from row in bll.GetProcessSheetMainDetailsByPartNumber(new PRD_MAST() { PART_NO = MandatoryFields.PART_NO })
                                       where Convert.ToBoolean(Convert.ToInt16(row.CURRENT_PROC)) == true && row.PART_NO == MandatoryFields.PART_NO && row.ROUTE_NO == MandatoryFields.ROUTE_NO.ToDecimalValue()
                                       select row.RM_CD).Min();

            DDRM_MAST ddrm_mast = bll.GetRawMaterialByCode(rawmaterial_code);
            MandatoryFields.RAW_MATERIAL = string.Empty;
            if (ddrm_mast.IsNotNullOrEmpty())
            {
                MandatoryFields.RAW_MATERIAL = ddrm_mast.RM_DESC;
            }
            SeqNumberIsFocused = true;
            SequenceChanged();

        }

        private readonly ICommand partNumberEndEditCommand;
        public ICommand PartNumberEndEditCommand { get { return this.partNumberEndEditCommand; } }
        private void partNumberEndEdit()
        {
            partNumberChanged();
        }

        #endregion

        #region Work Order
        private DataView _workorder = null;
        public DataView WorkOrderDataSource
        {
            get
            {
                return _workorder;
            }
            set
            {
                _workorder = value;
                NotifyPropertyChanged("WorkOrderDataSource");
            }
        }

        private DataRowView _workorderSelectedRow;
        public DataRowView WorkOrderSelectedRow
        {
            get
            {
                return _workorderSelectedRow;
            }

            set
            {
                _workorderSelectedRow = value;
            }
        }

        private Visibility _workorderHasDropDownVisibility = Visibility.Visible;
        public Visibility WorkOrderHasDropDownVisibility
        {
            get { return _workorderHasDropDownVisibility; }
            set
            {
                _workorderHasDropDownVisibility = value;
                NotifyPropertyChanged("WorkOrderHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _workorderDropDownItems;
        public ObservableCollection<DropdownColumns> WorkOrderDropDownItems
        {
            get
            {
                return _workorderDropDownItems;
            }
            set
            {
                _workorderDropDownItems = value;
                OnPropertyChanged("WorkOrderDropDownItems");
            }
        }

        private readonly ICommand _workorderSelectedItemChangedCommand;
        public ICommand WorkOrderSelectedItemChangedCommand { get { return this._workorderSelectedItemChangedCommand; } }
        private void WorkOrderChanged()
        {
            MandatoryFields.IS_SAVE_ENABLED = false;
            MandatoryFields.IS_READ_ONLY_QUANTITY = true;

            if (!MandatoryFields.IsNotNullOrEmpty() || !MandatoryFields.WORK_ORDER_NO.IsNotNullOrEmpty()) return;
            if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                MandatoryFields.WORK_ORDER_NO = string.Empty;
            }
            else
            {
                List<WORK_ORDER_MAIN> lstWORK_ORDER_MAIN = (from row in lstWorkOrderDetails.AsEnumerable()
                                                            where row.PART_NO == MandatoryFields.PART_NO && row.WORK_ORDER_NO == MandatoryFields.WORK_ORDER_NO
                                                            select row).ToList<WORK_ORDER_MAIN>();

                if (lstWORK_ORDER_MAIN.IsNotNullOrEmpty() && lstWORK_ORDER_MAIN.Count > 0)
                {
                    MandatoryFields.QUANTITY = lstWORK_ORDER_MAIN[0].TOTAL_QTY.ToValueAsString();
                    return;
                }
                MandatoryFields.IS_SAVE_ENABLED = true;
                MandatoryFields.IS_READ_ONLY_QUANTITY = false;
            }
        }

        private readonly ICommand _workorderEndEditCommand;
        public ICommand WorkOrderEndEditCommand { get { return this._workorderEndEditCommand; } }
        private void WorkOrderEndEdit()
        {
            WorkOrderChanged();
        }

        #endregion

        #region Sequence
        private DataView _sequence = null;
        public DataView SequenceDataSource
        {
            get
            {
                return _sequence;
            }
            set
            {
                _sequence = value;
                NotifyPropertyChanged("SequenceDataSource");
            }
        }

        private DataRowView _sequenceSelectedRow;
        public DataRowView SequenceSelectedRow
        {
            get
            {
                return _sequenceSelectedRow;
            }

            set
            {
                _sequenceSelectedRow = value;
            }
        }

        private Visibility _sequenceHasDropDownVisibility = Visibility.Visible;
        public Visibility SequenceHasDropDownVisibility
        {
            get { return _sequenceHasDropDownVisibility; }
            set
            {
                _sequenceHasDropDownVisibility = value;
                NotifyPropertyChanged("SequenceHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _sequenceDropDownItems;
        public ObservableCollection<DropdownColumns> SequenceDropDownItems
        {
            get
            {
                return _sequenceDropDownItems;
            }
            set
            {
                _sequenceDropDownItems = value;
                OnPropertyChanged("SequenceDropDownItems");
            }
        }

        private readonly ICommand _sequenceSelectedItemChangedCommand;
        public ICommand SequenceSelectedItemChangedCommand { get { return this._sequenceSelectedItemChangedCommand; } }
        private void SequenceChanged()
        {
            if (SNoSelectedItem == null)
            {
                MandatoryFields.GridData = null;
                MandatoryFields.SEQ_NO = null;
                MandatoryFields.CC_CODE = null;
                MandatoryFields.TS_ISSUE_ALTER = null;
                MandatoryFields.WORK_ORDER_NO = null;
                MandatoryFields.QUANTITY = null;
                //MandatoryFields.CCF = null;
                //MandatoryFields.NEXT_OPERATION = null;
                //MandatoryFields.PART_DESC = null;
                //MandatoryFields.RAW_MATERIAL = null;
                MandatoryFields.MACHINE_NAME = null;
                MandatoryFields.OPERATION_CODE = null;
                MandatoryFields.OPERATION_DESC = null;
                MandatoryFields.NEXT_OPERATION_DESC = null;
                //MandatoryFields.SEQUENCE_DRAWING_ISSUE_NO = null;
                //MandatoryFields.ROUTE_NO = null;
                //MandatoryFields.SEQUENCE_DRAWING_ISSUE_DATE = null;
                return;
            }

            //Commented by Jeyan
            MandatoryFields.SEQ_NO = SNoSelectedItem["SEQ_NO"].ToString();

            if (!MandatoryFields.IsNotNullOrEmpty() || !MandatoryFields.SEQ_NO.IsNotNullOrEmpty() || !lstPROCESS_SHEET.IsNotNullOrEmpty()) return;

            List<PROCESS_SHEET> lstEntity = (from row in lstPROCESS_SHEET.AsEnumerable()
                                             where row.PART_NO == MandatoryFields.PART_NO && row.ROUTE_NO == MandatoryFields.ROUTE_NO.ToDecimalValue() && row.SEQ_NO == MandatoryFields.SEQ_NO.ToDecimalValue()
                                             orderby row.SEQ_NO
                                             select row).ToList<PROCESS_SHEET>();

            MandatoryFields.OPERATION_CODE = string.Empty;
            MandatoryFields.OPERATION_DESC = string.Empty;
            MandatoryFields.CC_CODE = string.Empty;

            MandatoryFields.TS_ISSUE_ALTER = string.Empty;
            if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
            {
                MandatoryFields.OPERATION_CODE = lstEntity[0].OPN_CD.ToValueAsString();
                MandatoryFields.OPERATION_DESC = lstEntity[0].OPN_DESC.ToValueAsString();
            }

            lstEntity = (from row in lstPROCESS_SHEET.AsEnumerable()
                         where row.PART_NO == MandatoryFields.PART_NO && row.ROUTE_NO == MandatoryFields.ROUTE_NO.ToDecimalValue() && row.SEQ_NO > MandatoryFields.SEQ_NO.ToDecimalValue()
                         orderby row.SEQ_NO
                         select row).ToList<PROCESS_SHEET>();

            MandatoryFields.NEXT_OPERATION_DESC = string.Empty;
            if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
            {
                MandatoryFields.NEXT_OPERATION_DESC = lstEntity[0].OPN_DESC.ToValueAsString();
            }

            List<PROCESS_CC> lstPROCESS_CC = bll.GetProcessSheetCCDetailsByPartNumber(new PROCESS_CC() { PART_NO = MandatoryFields.PART_NO, ROUTE_NO = MandatoryFields.ROUTE_NO.ToDecimalValue(), SEQ_NO = MandatoryFields.SEQ_NO.ToDecimalValue() });
            WireDiaDataSource = (lstPROCESS_CC.IsNotNullOrEmpty() ? lstPROCESS_CC.ToDataTableWithType<PROCESS_CC>().DefaultView : null);

            lstPROCESS_CC = (from row in lstPROCESS_CC.AsEnumerable()
                             where row.PART_NO == MandatoryFields.PART_NO && row.ROUTE_NO == MandatoryFields.ROUTE_NO.ToDecimalValue() && row.SEQ_NO == MandatoryFields.SEQ_NO.ToDecimalValue() && row.CC_CODE != null
                             orderby row.SEQ_NO
                             select row).ToList<PROCESS_CC>();
            CostCentreDataSource = (lstPROCESS_CC.IsNotNullOrEmpty() ? lstPROCESS_CC.ToDataTableWithType<PROCESS_CC>().DefaultView : null);

            //Jeyan
            if (WireDiaDataSource.Table.IsNotNullOrEmpty() && CostCentreDataSource.Table.Rows.Count  > 0)
                MandatoryFields.CC_CODE = CostCentreDataSource.Table.Rows[WireDiaDataSource.Table.Rows.Count - 1]["CC_CODE"].ToValueAsString();

            List<PCCS> lstPCCS = bll.GetPCCSDetailsByPartNumber(new PCCS() { PART_NO = MandatoryFields.PART_NO, ROUTE_NO = MandatoryFields.ROUTE_NO.ToDecimalValue(), SEQ_NO = MandatoryFields.SEQ_NO.ToDecimalValue() });
            DataView dv = (lstPCCS.IsNotNullOrEmpty() ? lstPCCS.ToDataTableWithType<PCCS>().DefaultView : null);

            DataTable dt = null;
            if (dv.IsNotNullOrEmpty())
            {
                dt = dv.Table.Copy();
                dt.TableName = "PCCS";
                foreach (DataColumn col in dv.Table.Columns)
                {
                    if (!(col.ColumnName == "SNO" || col.ColumnName == "FEATURE" || col.ColumnName == "SPEC_CHAR" ||
                        col.ColumnName == "CTRL_SPEC_MIN" || col.ColumnName == "CTRL_SPEC_MAX" || col.ColumnName == "GAUGES_USED" ||
                        col.ColumnName == "FREQ_OF_INSP" || col.ColumnName == "SAMPLE_SIZE" || col.ColumnName == "SNO" ||
                        col.ColumnName == "SNO" || col.ColumnName == "SNO" || col.ColumnName == "SNO"))
                    {
                        dt.Columns.Remove(col.ColumnName);
                        dt.AcceptChanges();
                    }
                }

                dt.AcceptChanges();

                foreach (DataColumn col in dv.Table.Columns)
                {
                    switch (col.ColumnName.ToUpper())
                    {
                        case "SNO": dt.Columns[col.ColumnName].SetOrdinal(0); break;
                        case "FEATURE": dt.Columns[col.ColumnName].SetOrdinal(1); break;
                        case "SPEC_CHAR": dt.Columns[col.ColumnName].SetOrdinal(2); break;
                        case "CTRL_SPEC_MIN": dt.Columns[col.ColumnName].SetOrdinal(3); break;
                        case "CTRL_SPEC_MAX": dt.Columns[col.ColumnName].SetOrdinal(4); break;
                        case "GAUGES_USED": dt.Columns[col.ColumnName].SetOrdinal(5); break;
                        case "FREQ_OF_INSP": dt.Columns[col.ColumnName].SetOrdinal(6); break;
                        case "SAMPLE_SIZE": dt.Columns[col.ColumnName].SetOrdinal(7); break;
                    }
                }

            }

            MandatoryFields.GridData = dt.IsNotNullOrEmpty() ? dt.DefaultView : null;
            //CCCodeIsFocused = true;
            CostCentreChanged();
        }

        private readonly ICommand _sequenceEndEditCommand;
        public ICommand SequenceEndEditCommand { get { return this._sequenceEndEditCommand; } }
        private void SequenceEndEdit()
        {
            //SequenceChanged();
        }

        #endregion

        #region Wire Dia
        private DataView _wiredia = null;
        public DataView WireDiaDataSource
        {
            get
            {
                return _wiredia;
            }
            set
            {
                _wiredia = value;
                NotifyPropertyChanged("WireDiaDataSource");
            }
        }

        private DataRowView _wirediaSelectedRow;
        public DataRowView WireDiaSelectedRow
        {
            get
            {
                return _wirediaSelectedRow;
            }

            set
            {
                _wirediaSelectedRow = value;
            }
        }

        private Visibility _wirediaHasDropDownVisibility = Visibility.Visible;
        public Visibility WireDiaHasDropDownVisibility
        {
            get { return _wirediaHasDropDownVisibility; }
            set
            {
                _wirediaHasDropDownVisibility = value;
                NotifyPropertyChanged("WireDiaHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _wirediaDropDownItems;
        public ObservableCollection<DropdownColumns> WireDiaDropDownItems
        {
            get
            {
                return _wirediaDropDownItems;
            }
            set
            {
                _wirediaDropDownItems = value;
                OnPropertyChanged("WireDiaDropDownItems");
            }
        }

        private readonly ICommand _wirediaSelectedItemChangedCommand;
        public ICommand WireDiaSelectedItemChangedCommand { get { return this._wirediaSelectedItemChangedCommand; } }
        private void WireDiaChanged()
        {


        }

        private readonly ICommand _wirediaEndEditCommand;
        public ICommand WireDiaEndEditCommand { get { return this._wirediaEndEditCommand; } }
        private void WireDiaEndEdit()
        {
            //WireDiaChanged();
        }

        #endregion

        #region Cost Centre
        private DataView _costcentre = null;
        public DataView CostCentreDataSource
        {
            get
            {
                return _costcentre;
            }
            set
            {
                _costcentre = value;
                NotifyPropertyChanged("CostCentreDataSource");
            }
        }

        private DataRowView _costcentreSelectedRow;
        public DataRowView CostCentreSelectedRow
        {
            get
            {
                return _costcentreSelectedRow;
            }

            set
            {
                _costcentreSelectedRow = value;
            }
        }

        private Visibility _costcentreHasDropDownVisibility = Visibility.Visible;
        public Visibility CostCentreHasDropDownVisibility
        {
            get { return _costcentreHasDropDownVisibility; }
            set
            {
                _costcentreHasDropDownVisibility = value;
                NotifyPropertyChanged("CostCentreHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _costcentreDropDownItems;
        public ObservableCollection<DropdownColumns> CostCentreDropDownItems
        {
            get
            {
                return _costcentreDropDownItems;
            }
            set
            {
                _costcentreDropDownItems = value;
                OnPropertyChanged("CostCentreDropDownItems");
            }
        }

        private readonly ICommand _costcentreSelectedItemChangedCommand;
        public ICommand CostCentreSelectedItemChangedCommand { get { return this._costcentreSelectedItemChangedCommand; } }
        private void CostCentreChanged()
        {
            if (!MandatoryFields.IsNotNullOrEmpty() || !MandatoryFields.CC_CODE.IsNotNullOrEmpty() || !WireDiaDataSource.IsNotNullOrEmpty() || !WireDiaDataSource.Table.IsNotNullOrEmpty() || WireDiaDataSource.Table.Rows.Count == 0) return;
            MandatoryFields.TS_ISSUE_ALTER = WireDiaDataSource.Table.Rows[WireDiaDataSource.Table.Rows.Count - 1]["TS_ISSUE_ALTER"].ToValueAsString();

            List<DDCOST_CENT_MAST> lstDDCOST_CENT_MAST = bll.GetCostCentreMasterDetailsByCode(new DDCOST_CENT_MAST() { COST_CENT_CODE = MandatoryFields.CC_CODE });
            MandatoryFields.MACHINE_NAME = string.Empty;
            if (lstDDCOST_CENT_MAST.IsNotNullOrEmpty() && lstDDCOST_CENT_MAST.Count > 0)
            {
                MandatoryFields.MACHINE_NAME = lstDDCOST_CENT_MAST[0].COST_CENT_DESC.ToValueAsString();
            }
            //WireDiaIsFocused = true;
        }

        private readonly ICommand _costcentreEndEditCommand;
        public ICommand CostCentreEndEditCommand { get { return this._costcentreEndEditCommand; } }
        private void CostCentreEndEdit()
        {
            //CostCentreChanged();
        }

        #endregion

        #region Shift
        private DataView _shift = null;
        public DataView ShiftDataSource
        {
            get
            {
                return _shift;
            }
            set
            {
                _shift = value;
                NotifyPropertyChanged("ShiftDataSource");
            }
        }

        private DataRowView _shiftSelectedRow;
        public DataRowView ShiftSelectedRow
        {
            get
            {
                return _shiftSelectedRow;
            }

            set
            {
                _shiftSelectedRow = value;
            }
        }

        private Visibility _shiftHasDropDownVisibility = Visibility.Visible;
        public Visibility ShiftHasDropDownVisibility
        {
            get { return _shiftHasDropDownVisibility; }
            set
            {
                _shiftHasDropDownVisibility = value;
                NotifyPropertyChanged("ShiftHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _shiftDropDownItems;
        public ObservableCollection<DropdownColumns> ShiftDropDownItems
        {
            get
            {
                return _shiftDropDownItems;
            }
            set
            {
                _shiftDropDownItems = value;
                OnPropertyChanged("ShiftDropDownItems");
            }
        }

        private readonly ICommand _shiftSelectedItemChangedCommand;
        public ICommand ShiftSelectedItemChangedCommand { get { return this._shiftSelectedItemChangedCommand; } }
        private void ShiftChanged()
        {


        }

        private readonly ICommand _shiftEndEditCommand;
        public ICommand ShiftEndEditCommand { get { return this._shiftEndEditCommand; } }
        private void ShiftEndEdit()
        {
            //ShiftChanged();
        }

        #endregion
        public DataSet DsReport = null;
        private readonly ICommand printCommand;
        public ICommand PrintClickCommand { get { return this.printCommand; } }
        private void PrintSubmitCommand()
        {

            if (!MandatoryFields.IsNotNullOrEmpty()) return;
            if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            else if (!MandatoryFields.SEQ_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Sequence"));
                return;
            }
            else if (!MandatoryFields.CC_CODE.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Cost Centre Code"));
                return;
            }
            //else if (!MandatoryFields.WORK_ORDER_NO.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Work Order Number"));
            //    return;
            //}
            //else if (!MandatoryFields.QUANTITY.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Quantity"));
            //    return;
            //}
            else if (CheckDuplicateSNo()) // Added by Jeyan - to check duplicate Sno
            {
                ShowInformationMessage("S.No should not allow duplicate values");
                return;
            }
            else
            {
                Progress.ProcessingText = PDMsg.ProgressUpdateText;
                Progress.Start();

                if (MandatoryFields.PART_NO.IsNotNullOrEmpty())
                {
                    List<OQA_CCF> lstOQA_CCF = bll.GetCCFDetailsByPartNumber(new OQA_CCF()
                    {
                        PART_NO = MandatoryFields.PART_NO
                    });

                    if (!lstOQA_CCF.IsNotNullOrEmpty() || lstOQA_CCF.Count == 0)
                    {
                        lstOQA_CCF.Add(new OQA_CCF()
                        {
                            PART_NO = MandatoryFields.PART_NO,
                            CCF = MandatoryFields.CCF
                        });
                        if (bll.Update<OQA_CCF>(lstOQA_CCF))
                        {
                            //ShowInformationMessage(PDMsg.SavedSuccessfully);
                        }
                    }
                    else
                    {
                        foreach (OQA_CCF oqa_ccf in lstOQA_CCF)
                        {
                            oqa_ccf.PART_NO = MandatoryFields.PART_NO;
                            oqa_ccf.CCF = MandatoryFields.CCF;
                        }

                        if (bll.Update<OQA_CCF>(lstOQA_CCF))
                        {
                            //ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                        }
                    }
                }

                DsReport = new DataSet(REPORT_NAME + "_REPORT");

                List<OperatorQualityAssuranceModel> lstOperatorQualityAssurance = new List<OperatorQualityAssuranceModel>();
                lstOperatorQualityAssurance.Add(MandatoryFields);

                DataTable dtOperatorQualityAssurance = lstOperatorQualityAssurance.ToDataTableWithType<OperatorQualityAssuranceModel>();

                if (dtOperatorQualityAssurance.IsNotNullOrEmpty())
                {
                    dtOperatorQualityAssurance.TableName = REPORT_NAME;
                    if (dtOperatorQualityAssurance.Columns.Contains("PropertyInfos")) dtOperatorQualityAssurance.Columns.Remove("PropertyInfos");
                    if (dtOperatorQualityAssurance.Columns.Contains("Item")) dtOperatorQualityAssurance.Columns.Remove("Item");
                    if (dtOperatorQualityAssurance.Columns.Contains("Error")) dtOperatorQualityAssurance.Columns.Remove("Error");
                    if (dtOperatorQualityAssurance.Columns.Contains("HasErrors")) dtOperatorQualityAssurance.Columns.Remove("HasErrors");
                    if (dtOperatorQualityAssurance.Columns.Contains("GridData")) dtOperatorQualityAssurance.Columns.Remove("GridData");
                    if (dtOperatorQualityAssurance.Columns.Contains("IS_SAVE_ENABLED")) dtOperatorQualityAssurance.Columns.Remove("IS_SAVE_ENABLED");
                    if (dtOperatorQualityAssurance.Columns.Contains("IS_READ_ONLY_QUANTITY")) dtOperatorQualityAssurance.Columns.Remove("IS_READ_ONLY_QUANTITY");
                }

                //List<PCCS> lstPCCS = new List<PCCS>();
                //if (MandatoryFields.PART_NO.IsNotNullOrEmpty())
                //{
                //    lstPCCS = bll.GetPCCSDetailsByPartNumber(new PCCS() { PART_NO = MandatoryFields.PART_NO, ROUTE_NO = MandatoryFields.ROUTE_NO.ToDecimalValue(), SEQ_NO = MandatoryFields.SEQ_NO.ToDecimalValue() });
                //}

                MandatoryFields.GridData.Table.AcceptChanges();
                DataTable dtPCCS = (MandatoryFields.GridData.IsNotNullOrEmpty() ? MandatoryFields.GridData.ToTable() : null);
                if (dtPCCS.IsNotNullOrEmpty())
                {
                    dtPCCS.TableName = "PCCS";
                    if (dtPCCS.Columns.Contains("PROCESS_SHEET")) dtPCCS.Columns.Remove("PROCESS_SHEET");
                    if (dtPCCS.Columns.Contains("ROWID")) dtPCCS.Columns.Remove("ROWID");
                }
                DsReport.Tables.Add(dtOperatorQualityAssurance);
                DsReport.Tables.Add(dtPCCS);

                DataSet dsReport = DsReport;

                Dictionary<string, string> dictFormulas = new Dictionary<string, string>();
                EXHIBIT_DOC exhibit = (from o in bll.DB.EXHIBIT_DOC
                                       where o.DOC_NAME == "OQA"
                                       select o).FirstOrDefault<EXHIBIT_DOC>();
                if (exhibit != null)
                {
                    dictFormulas.Add("EX_NO", exhibit.EX_NO);
                    dictFormulas.Add("REVISION_NO", exhibit.REVISION_NO);
                }
                else
                {
                    dictFormulas.Add("EX_NO", "");
                    dictFormulas.Add("REVISION_NO", "");
                }

                List<PROCESS_SHEET> lstEntity = (from row in lstPROCESS_SHEET.AsEnumerable()
                                                 where row.PART_NO == MandatoryFields.PART_NO && row.ROUTE_NO == MandatoryFields.ROUTE_NO.ToDecimalValue() && row.SEQ_NO < MandatoryFields.SEQ_NO.ToDecimalValue()
                                                 orderby row.SEQ_NO descending
                                                 select row).ToList<PROCESS_SHEET>();
                
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                {
                    dictFormulas.Add("PRV_OPERATION_DESC", lstEntity[0].OPN_DESC.ToValueAsString());
                }
                else
                {
                    dictFormulas.Add("PRV_OPERATION_DESC", "");
                }

                Progress.End();
                if (dsReport == null || dsReport.Tables[0] == null || dsReport.Tables[0].Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                    return;
                }

                frmReportViewer reportViewer = new frmReportViewer(dsReport, REPORT_NAME, dictFormulas);
                if (!reportViewer.ReadyToShowReport) return;
                reportViewer.ShowDialog();
            }
        }

        private readonly ICommand saveCommand;
        public ICommand SaveClickCommand { get { return this.saveCommand; } }
        private void saveSubmitCommand()
        {
            if (!MandatoryFields.IsNotNullOrEmpty()) return;

            if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part Number"));
                return;
            }
            //else if (CheckDuplicateSNo()) // Added by Jeyan - to check duplicate Sno
            //{
            //    ShowInformationMessage("S.No should not allow duplicate values");
            //    return;
            //}

            //if (!MandatoryFields.WORK_ORDER_NO.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Work Order Number"));
            //    return;
            //}

            //if (!MandatoryFields.QUANTITY.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Quantity"));
            //    return;
            //}
            PartNumberIsFocused = true;
            Progress.ProcessingText = PDMsg.ProgressUpdateText;
            Progress.Start();
            List<WORK_ORDER_MAIN> lstWORK_ORDER_MAIN = bll.GetWorkOrderDetailByPartNumberOrderNumber(new WORK_ORDER_MAIN() { PART_NO = MandatoryFields.PART_NO, WORK_ORDER_NO = MandatoryFields.WORK_ORDER_NO });
            if (!lstWORK_ORDER_MAIN.IsNotNullOrEmpty() || lstWORK_ORDER_MAIN.Count == 0)
            {
                lstWORK_ORDER_MAIN.Add(new WORK_ORDER_MAIN()
                {
                    PART_NO = MandatoryFields.PART_NO,
                    PART_DESC = MandatoryFields.PART_DESC,
                    WORK_ORDER_NO = MandatoryFields.WORK_ORDER_NO,
                    TOTAL_QTY = MandatoryFields.QUANTITY.ToDecimalValue(),
                    CCF = MandatoryFields.CCF
                });
            }
            else
            {
                foreach (WORK_ORDER_MAIN work_order_main in lstWORK_ORDER_MAIN)
                {
                    work_order_main.PART_NO = MandatoryFields.PART_NO;
                    work_order_main.PART_DESC = MandatoryFields.PART_DESC;
                    work_order_main.WORK_ORDER_NO = MandatoryFields.WORK_ORDER_NO;
                    work_order_main.TOTAL_QTY = MandatoryFields.QUANTITY.ToDecimalValue();
                    work_order_main.CCF = MandatoryFields.CCF;
                }
            }

            if (bll.Update<WORK_ORDER_MAIN>(lstWORK_ORDER_MAIN))
            {

                List<OQA_CCF> lstOQA_CCF = bll.GetCCFDetailsByPartNumber(new OQA_CCF() { PART_NO = MandatoryFields.PART_NO });
                if (!lstOQA_CCF.IsNotNullOrEmpty() || lstOQA_CCF.Count == 0)
                {
                    lstOQA_CCF.Add(new OQA_CCF()
                    {
                        PART_NO = MandatoryFields.PART_NO,
                        CCF = MandatoryFields.CCF
                    });
                }
                else
                {
                    foreach (OQA_CCF oqa_ccf in lstOQA_CCF)
                    {
                        oqa_ccf.PART_NO = MandatoryFields.PART_NO;
                        oqa_ccf.CCF = MandatoryFields.CCF;
                    }
                }
                Progress.End();
                if (bll.Update<OQA_CCF>(lstOQA_CCF))
                {
                    ShowInformationMessage(PDMsg.SavedSuccessfully);
                    Clear();
                }
            }


        }

        private void Clear()
        {
            MandatoryFields.PART_NO = string.Empty;
            MandatoryFields.SEQ_NO = string.Empty;
            MandatoryFields.CC_CODE = string.Empty;
            MandatoryFields.TS_ISSUE_ALTER = string.Empty;
            MandatoryFields.TODAY_DATE = DateTime.Now;
            MandatoryFields.WORK_ORDER_NO = string.Empty;
            MandatoryFields.QUANTITY = string.Empty;
            MandatoryFields.CCF = string.Empty;
            DataTable dt = new DataTable();
            if (MandatoryFields.GridData != null)
            {
                MandatoryFields.GridData.Table.Rows.Clear();
            }
            MandatoryFields.OPERATION_CODE = string.Empty;
            MandatoryFields.ROUTE_NO = string.Empty;
            MandatoryFields.PART_DESC = string.Empty;
            MandatoryFields.OPERATION_DESC = string.Empty;
            MandatoryFields.RAW_MATERIAL = string.Empty;
            MandatoryFields.NEXT_OPERATION_DESC = string.Empty;
            MandatoryFields.MACHINE_NAME = string.Empty;
            MandatoryFields.SEQUENCE_DRAWING_ISSUE_NO = string.Empty;
            MandatoryFields.SEQUENCE_DRAWING_ISSUE_DATE = null;
        }

        public string GetReportPath()
        {
            string reportPathNew = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            //  System.Windows.MessageBox.Show(reportPathNew);
            if (Assembly.GetExecutingAssembly().IsDebug() || reportPathNew.Contains("\\bin\\Debug"))
            {
                DirectoryInfo d = new DirectoryInfo(reportPathNew);
                reportPathNew = d.Parent.Parent.FullName;
            }
            return reportPathNew + "\\Reports\\";


        }

        string oldsno = "";
        public void OnBeginningEditOQA(object sender, DataGridBeginningEditEventArgs e)
        {
            DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            string columnName = e.Column.SortMemberPath;
            if (columnName == "SNO")
            {
                oldsno = selecteditem["SNO"].ToString().Trim();
            }
        }

        public void OnCellEditEndingOQA(object sender, DataGridCellEditEndingEventArgs e)
        {
            TextBox tb = e.EditingElement as TextBox;
            DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            string columnName = e.Column.SortMemberPath;
            if (tb != null)
            {
                selecteditem.BeginEdit();
                selecteditem[columnName] = tb.Text;
                selecteditem.EndEdit();
            }
            string sno = selecteditem["SNO"].ToString().Trim();
            if (columnName == "SNO" && sno.IsNotNullOrEmpty())
            {
                //if (IsDuplicateSNo(sno))
                //{
                //    ShowInformationMessage("S.No should not allow duplicate values");
                //    tb.Text = oldsno;
                //    return;
                //}
                IsSorting();
            }
        }

        private bool IsDuplicateSNo(string sNo)
        {
            DataView dv = MandatoryFields.GridData.ToTable().DefaultView;
            dv.RowFilter = "SNO = '" + sNo + "'";
            if (dv.Count > 1)
            {
                return true;
            }
            return false;
        }

        // Added by Jeyan - Check duplicate sno while sava/print/export excel
        private bool CheckDuplicateSNo()
        {
            int iRecordCount = 0;
            string sNo;
            DataTable dt = new DataTable();
            sNo = "";
            dt = MandatoryFields.GridData.Table.Copy();
            iRecordCount = dt.Rows.Count;

            DataView dv = MandatoryFields.GridData.ToTable().DefaultView;
            
            for (int i = 0; i <= iRecordCount - 1; i++)
            {
                sNo = dt.Rows[i]["SNO"].ToString();
                dv.RowFilter = "SNO = '" + sNo + "'";
                if (dv.Count > 1)
                {
                    return true;
                }
            }            
            return false;
        }

        private void IsSorting()
        {
            DataTable dt = new DataTable();
            dt = MandatoryFields.GridData.Table.Copy();
            if (dt.Rows.Count > 1)
            {
                dt.DefaultView.Sort = "SNO Asc";
                MandatoryFields.GridData = dt.DefaultView;
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

        private bool _seqNumberIsFocused = false;
        public bool SeqNumberIsFocused
        {
            get { return _seqNumberIsFocused; }
            set
            {
                _seqNumberIsFocused = value;
                NotifyPropertyChanged("SeqNumberIsFocused");
            }
        }

        private bool _ccCodeIsFocused = false;
        public bool CCCodeIsFocused
        {
            get { return _ccCodeIsFocused; }
            set
            {
                _ccCodeIsFocused = value;
                NotifyPropertyChanged("CCCodeIsFocused");
            }
        }

        private bool _wireDiaIsFocused = false;
        public bool WireDiaIsFocused
        {
            get { return _wireDiaIsFocused; }
            set
            {
                _wireDiaIsFocused = value;
                NotifyPropertyChanged("WireDiaIsFocused");
            }
        }
        private readonly ICommand exportToExcelCommand;
        public ICommand ExportToExcelClickCommand { get { return this.exportToExcelCommand; } }
        private void ExportToExcelSubmitCommand()
        {

            if (!MandatoryFields.GridData.IsNotNullOrEmpty() || MandatoryFields.GridData.Table.Rows.Count == 0)
            {
                ShowInformationMessage(PDMsg.NoRecordExcel);
                return;
            }
            else if (CheckDuplicateSNo()) // Added by Jeyan - to check duplicate Sno
            {
                ShowInformationMessage("S.No should not allow duplicate values");
                return;
            }

            string fileName = "";
            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.DefaultExt = "xlsx";
            saveFileDialog.Filter = "Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx";
            System.IO.DirectoryInfo di = new DirectoryInfo(GetReportPath() + "\\ExportToExcel");
            if (!di.Exists)
            {
                try
                {
                    di.Create();
                }
                catch (Exception)
                {

                }
            }

            saveFileDialog.InitialDirectory = di.FullName;
            saveFileDialog.Title = "Export to Excel";
            if (saveFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            fileName = saveFileDialog.FileName;

            if (!fileName.IsNotNullOrEmpty()) return;
            MandatoryFields.GridData.Table.AcceptChanges();
            DataTable dt = MandatoryFields.GridData.Table.Copy();
            if (dt.IsNotNullOrEmpty())
            {
                foreach (DataColumn col in dt.Columns)
                {
                    switch (col.ColumnName.ToUpper())
                    {
                        case "SNO": dt.Columns[col.ColumnName].ColumnName = "S.No."; break;
                        case "FEATURE": dt.Columns[col.ColumnName].ColumnName = "INSPECT FOR"; break;
                        case "SPEC_CHAR": dt.Columns[col.ColumnName].ColumnName = "_A_"; break;
                        case "CTRL_SPEC_MIN": dt.Columns[col.ColumnName].ColumnName = "SPECIFICATION MIN"; break;
                        case "CTRL_SPEC_MAX": dt.Columns[col.ColumnName].ColumnName = "SPECIFICATION MAX"; break;
                        case "GAUGES_USED": dt.Columns[col.ColumnName].ColumnName = "MEASURING TECH"; break;
                        case "FREQ_OF_INSP": dt.Columns[col.ColumnName].ColumnName = "FREQ"; break;
                        case "SAMPLE_SIZE": dt.Columns[col.ColumnName].ColumnName = "SAMPLE SIZE"; break;
                    }
                }
            }

            if (dt.ExportToExcel(fileName))
            {
                ShowInformationMessage("Succesfully Exported to Excel");
            }
            //if (eXportToExcel(dt,  fileName))
            //{
            //    ShowInformationMessage("Succesfully Exported to Excel");
            //}

        }
        //Jeyan
        private DataView GetProductMasterDetailsByPartNumberDV()
        {
            DataView dv = null;
            try
            {
                DataTable dt = new DataTable();

                System.Resources.ResourceManager myManager;
                myManager = new System.Resources.ResourceManager(typeof(ProcessDesigner.Properties.Resources));
                string conStr = myManager.GetString("ConnectionString");

                DataAccessLayer dal = new DataAccessLayer(conStr);
                dt = dal.Get_PartNo(0);
                if (dt != null)
                {
                    dv = dt.DefaultView;
                }
                else
                {
                    dv = null;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return dv;
        }
        //private bool eXportToExcel(DataTable dt, string fileName)
        //{
        //       //open file
        //       StreamWriter wr = new StreamWriter(fileName);
        //       //write rows to excel file
        //       for (int i = 0; i < (dt.Rows.Count); i++)
        //       {
        //           for (int j = 0; j < dt.Columns.Count; j++)
        //           {
        //               if (dt.Rows[i][j] != null)
        //               {
        //                   wr.Write(Convert.ToString(dt.Rows[i][j]) + "\t");
        //               }
        //               else
        //               {
        //                   wr.Write("\t");
        //               }
        //           }
        //           //go to next line
        //           wr.WriteLine();
        //       }
        //       //close file
        //       wr.Close();

        //       return true;          
        //}
    }
}
