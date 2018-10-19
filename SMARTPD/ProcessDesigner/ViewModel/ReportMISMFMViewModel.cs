using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using ProcessDesigner.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProcessDesigner.ViewModel
{
    class ReportMISMFMViewModel : ViewModelBase
    {
        UserInformation _userInformation = null;
        private const string CONS_RIGHTS_NAME = "MFM REPORT";
        private const string REPORT_NAME = "MFM_REPORT";
        private const string REPORT_TITLE = "MFM Report";

        ReportMISMFM bll = null;
        WPF.MDI.MdiChild mdiChild = null;

        public ReportMISMFMViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, PRD_MAST productMaster = null, DDCI_INFO customerInfo = null, DDCUST_MAST customerMaster = null, bool refreshOnLoad = false, string title = REPORT_TITLE)
        {

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;

            _userInformation = userInformation;
            this.mdiChild = mdiChild;

            bll = new ReportMISMFM(userInformation);
            MandatoryFields = new ReportMISMFMModel();
            MandatoryFields.GRID_TITLE = REPORT_TITLE;

            CustomersDataSource = bll.GetCustomerDetails().ToDataTable<DDCUST_MAST>().DefaultView;
            this.customerSelectedItemChangedCommand = new DelegateCommand(this.CustomerChanged);
            CustomerDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "CUST_CODE", ColumnDesc = "Customer Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "CUST_NAME", ColumnDesc = "Customer", ColumnWidth = "75*", IsDefaultSearchColumn = true }
                        };

            SFLPartNumbersDataSource = bll.GetPartNumber().ToDataTable<PRD_MAST>().DefaultView;
            this.sflPartNumberSelectedItemChangedCommand = new DelegateCommand(this.SFLPartNumberChanged);
            SFLPartNumberDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "SFL Part Number", ColumnWidth = "1*" },
                        };


            this.printCommand = new DelegateCommand(this.PrintSubmitCommand);
            this.refreshCommand = new DelegateCommand(this.RefreshSubmitCommand);
            this.ExportCommand = new DelegateCommand(this.ExportToExcelData);

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

        private string _title = REPORT_TITLE;
        private string Title
        {
            get { return _title; }
            set
            {
                value = value.IsNotNullOrEmpty() ? value : REPORT_TITLE;
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private readonly ICommand printCommand;
        public ICommand PrintClickCommand { get { return this.printCommand; } }
        private void PrintSubmitCommand()
        {
            DataSet dsReport = null;

            if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count < 0)
            {
                ShowInformationMessage(PDMsg.NoRecordsPrint);
                return;
            }

            frmReportViewer reportViewer = new frmReportViewer(dsReport, "FRCS");
            if (!reportViewer.ReadyToShowReport) return;
            reportViewer.ShowDialog();
        }

        private readonly ICommand refreshCommand;
        public ICommand RefreshClickCommand { get { return this.refreshCommand; } }
        private void RefreshSubmitCommand()
        {
            if (StartDate.ToValueAsString().Trim() == "")
            {
                ShowInformationMessage(PDMsg.NotEmpty("Start Date"));
                return;
            }
            if (EndDate.ToValueAsString().Trim() == "")
            {
                ShowInformationMessage(PDMsg.NotEmpty("End Date"));
                return;
            }
            if (StartDate > EndDate)
            {
                ShowInformationMessage("Start Date is Greater than End Date,Please Check it.!");
                return;
            }
            PRD_MAST productMaster = new PRD_MAST() { PART_NO = MandatoryFields.PART_NO };
            DDCI_INFO customerInfo = null;

            DataSet dsReport = bll.GetAllMFM(Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy"), Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy"), productMaster, customerInfo, DDCUST_MAST);
            MandatoryFields.GRID_TITLE = REPORT_TITLE;
            //for (int i = 0; i < dsReport.Tables[0].Rows.Count; i++)
            //{
            //    string date = Convert.ToDateTime(dsReport.Tables[0].Rows[i]["DOC_REL_DT_PLAN"].ToString()).ToShortDateString();
            //}
            foreach (DataRow dr in dsReport.Tables[0].Rows)
            {
                dr["DOC_REL_DT_PLAN"] = string.IsNullOrEmpty(dr["DOC_REL_DT_PLAN"].ToString()) ? null : DateTime.Parse((dr["DOC_REL_DT_PLAN"].ToString())).ToString("dd/MM/yyyy");
                dr["DOC_REL_DT_ACTUAL"] = string.IsNullOrEmpty(dr["DOC_REL_DT_ACTUAL"].ToString()) ? null : DateTime.Parse((dr["DOC_REL_DT_ACTUAL"].ToString())).ToString("dd/MM/yyyy");
                dr["TOOLS_READY_DT_PLAN"] = string.IsNullOrEmpty(dr["TOOLS_READY_DT_PLAN"].ToString()) ? null : DateTime.Parse((dr["TOOLS_READY_DT_PLAN"].ToString())).ToString("dd/MM/yyyy");
                dr["TOOLS_READY_ACTUAL_DT"] = string.IsNullOrEmpty(dr["TOOLS_READY_ACTUAL_DT"].ToString()) ? null : DateTime.Parse((dr["TOOLS_READY_ACTUAL_DT"].ToString())).ToString("dd/MM/yyyy");
                dr["FORGING_PLAN_DT"] = string.IsNullOrEmpty(dr["FORGING_PLAN_DT"].ToString()) ? null : DateTime.Parse((dr["FORGING_PLAN_DT"].ToString())).ToString("dd/MM/yyyy");
                dr["FORGING_ACTUAL_DT"] = string.IsNullOrEmpty(dr["FORGING_ACTUAL_DT"].ToString()) ? null : DateTime.Parse((dr["FORGING_ACTUAL_DT"].ToString())).ToString("dd/MM/yyyy");
                dr["SECONDARY_PLAN_DT"] = string.IsNullOrEmpty(dr["SECONDARY_PLAN_DT"].ToString()) ? null : DateTime.Parse((dr["SECONDARY_PLAN_DT"].ToString())).ToString("dd/MM/yyyy");
                dr["SECONDARY_ACTUAL_DT"] = string.IsNullOrEmpty(dr["SECONDARY_ACTUAL_DT"].ToString()) ? null : DateTime.Parse((dr["SECONDARY_ACTUAL_DT"].ToString())).ToString("dd/MM/yyyy");
                dr["HEAT_TREATMENT_PLAN_DT"] = string.IsNullOrEmpty(dr["HEAT_TREATMENT_PLAN_DT"].ToString()) ? null : DateTime.Parse((dr["HEAT_TREATMENT_PLAN_DT"].ToString())).ToString("dd/MM/yyyy");
                dr["HEAT_TREATMENT_ACTUAL"] = string.IsNullOrEmpty(dr["HEAT_TREATMENT_ACTUAL"].ToString()) ? null : DateTime.Parse((dr["HEAT_TREATMENT_ACTUAL"].ToString())).ToString("dd/MM/yyyy");
                dr["ISSR_PLAN_DT"] = string.IsNullOrEmpty(dr["ISSR_PLAN_DT"].ToString()) ? null : DateTime.Parse((dr["ISSR_PLAN_DT"].ToString())).ToString("dd/MM/yyyy");
                dr["ISSR_ACTUAL_DT"] = string.IsNullOrEmpty(dr["ISSR_ACTUAL_DT"].ToString()) ? null : DateTime.Parse((dr["ISSR_ACTUAL_DT"].ToString())).ToString("dd/MM/yyyy");
                dr["PPAP_PLAN"] = string.IsNullOrEmpty(dr["PPAP_PLAN"].ToString()) ? null : DateTime.Parse((dr["PPAP_PLAN"].ToString())).ToString("dd/MM/yyyy");
                dr["PPAP_ACTUAL_DT"] = string.IsNullOrEmpty(dr["PPAP_ACTUAL_DT"].ToString()) ? null : DateTime.Parse((dr["PPAP_ACTUAL_DT"].ToString())).ToString("dd/MM/yyyy");
                dr["PSW_DATE"] = string.IsNullOrEmpty(dr["PSW_DATE"].ToString()) ? null : DateTime.Parse((dr["PSW_DATE"].ToString())).ToString("dd/MM/yyyy");
            }
            if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count <= 0)
            {
                return;
            }

            MandatoryFields.GridData = dsReport.Tables[0].DefaultView;
            MandatoryFields.GRID_TITLE = REPORT_TITLE + " - " + MandatoryFields.GridData.Table.Rows.Count + " Entries";
        }

        private Nullable<DateTime> _startDate;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Start date is required")]
        public Nullable<DateTime> StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                NotifyPropertyChanged("StartDate");
            }
        }

        private Nullable<DateTime> _endDate;
        [Required(AllowEmptyStrings = false, ErrorMessage = "End date is required")]
        public Nullable<DateTime> EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                NotifyPropertyChanged("EndDate");
            }
        }
        public readonly ICommand ExportCommand;
        public ICommand ExportToExcelCommand { get { return this.ExportCommand; } }
        private void ExportToExcelData()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".xlsx";
            dlg.Filter = "Excel File (*.xls, *.xlsx)|*.xls;*.xlsx";
            dlg.ShowDialog();
            if (!dlg.FileName.IsNotNullOrEmpty()) return;
            PRD_MAST productMaster = new PRD_MAST() { PART_NO = MandatoryFields.PART_NO };
            DDCI_INFO customerInfo = null;
            DataSet dsReport = bll.GetAllMFM(Convert.ToDateTime(StartDate).ToString("MM/dd/yyyy"), Convert.ToDateTime(EndDate).ToString("MM/dd/yyyy"), productMaster, customerInfo, DDCUST_MAST);
            DataView dv = dsReport.Tables[0].DefaultView;
            DataTable dt = dv.ToTable();
            if (dlg.FileName != "")
            {
                dt.ExportMFMToExcel(dlg.FileName);
                ShowInformationMessage("Successfully Exported to Excel");
            }
        }

        private ReportMISMFMModel _mandatoryFields = null;
        public ReportMISMFMModel MandatoryFields
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

        private DataView _customersDataSource = null;
        public DataView CustomersDataSource
        {
            get
            {
                return _customersDataSource;
            }
            set
            {
                _customersDataSource = value;
                NotifyPropertyChanged("CustomersDataSource");
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
                    DDCUST_MAST = lstEntity[0];
                }
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

        private DataView _sflPartNumbersDataSource = null;
        public DataView SFLPartNumbersDataSource
        {
            get
            {
                return _sflPartNumbersDataSource;
            }
            set
            {
                _sflPartNumbersDataSource = value;
                NotifyPropertyChanged("SFLPartNumbersDataSource");
            }
        }

        private DataRowView _sflPartNumberSelectedRow;
        public DataRowView SFLPartNumberSelectedRow
        {
            get
            {
                return _sflPartNumberSelectedRow;
            }

            set
            {
                _sflPartNumberSelectedRow = value;
            }
        }

        private readonly ICommand sflPartNumberSelectedItemChangedCommand;
        public ICommand SFLPartNumberSelectedItemChangedCommand { get { return this.sflPartNumberSelectedItemChangedCommand; } }
        private void SFLPartNumberChanged()
        {
            if (_sflPartNumberSelectedRow.IsNotNullOrEmpty())
            {
                MandatoryFields.PART_NO = SFLPartNumberSelectedRow.Row["PART_NO"].ToValueAsString();
            }
        }

        private Visibility _sflPartNumberHasDropDownVisibility = Visibility.Visible;
        public Visibility SFLPartNumberHasDropDownVisibility
        {
            get { return _sflPartNumberHasDropDownVisibility; }
            set
            {
                _sflPartNumberHasDropDownVisibility = value;
                NotifyPropertyChanged("SFLPartNumberHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _sflPartNumberDropDownItems;
        public ObservableCollection<DropdownColumns> SFLPartNumberDropDownItems
        {
            get
            {
                return _sflPartNumberDropDownItems;
            }
            set
            {
                _sflPartNumberDropDownItems = value;
                OnPropertyChanged("SFLPartNumberDropDownItems");
            }
        }

        DDCUST_MAST DDCUST_MAST { get; set; }

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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

    }
}
