using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using ProcessDesigner.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace ProcessDesigner
{
    class ReportOTPMChartsViewModel : ViewModelBase
    {
        UserInformation _userInformation = null;
        private const string CONS_RIGHTS_NAME = "OTPM CHARTS REPORT";
        private const string REPORT_NAME = "OTPM-CHARTS_REPORT";
        private const string REPORT_TITLE = "OTPM Charts - Development Lead Time";

        ReportOTPMCharts bll = null;
        WPF.MDI.MdiChild mdiChild = null;
        string monthYearSep = "-";
        string doubleDateSep = " to ";

        public ReportOTPMChartsViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, string chartType = null, int? workingYear = null, int? pgCatogory = null, bool refreshOnLoad = false, string title = REPORT_TITLE)
        {
            _userInformation = userInformation;
            this.mdiChild = mdiChild;

            bll = new ReportOTPMCharts(userInformation);
            MandatoryFields = new ReportOTPMChartsModel();

            this.printCommand = new DelegateCommand(this.PrintSubmitCommand);
            this.refreshCommand = new DelegateCommand(this.RefreshSubmitCommand);

            DataTable dtChartType = new DataTable("ChartType");
            dtChartType.Columns.Add("CHART_TYPE_CODE", typeof(int));
            dtChartType.Columns.Add("CHART_TYPE", typeof(string));

            DataRow row = dtChartType.Rows.Add();
            row["CHART_TYPE_CODE"] = 1;
            row["CHART_TYPE"] = "Development Lead Time - PG";
            row.AcceptChanges();
            dtChartType.AcceptChanges();

            row = dtChartType.Rows.Add();
            row["CHART_TYPE_CODE"] = 2;
            row["CHART_TYPE"] = "Plan-Adherence";
            row.AcceptChanges();
            dtChartType.AcceptChanges();

            row = dtChartType.Rows.Add();
            row["CHART_TYPE_CODE"] = 2;
            row["CHART_TYPE"] = "First-time-right";
            row.AcceptChanges();
            dtChartType.AcceptChanges();
            ChartTypesDataSource = dtChartType.DefaultView;
            ChartTypeSelectedRow = ChartTypesDataSource[0];

            TodayDate = bll.ServerDateTime();

            DataTable dtChartTypePG = new DataTable("ChartTypePG");
            dtChartTypePG.Columns.Add("CHART_TYPE_PG_CODE", typeof(int));
            dtChartTypePG.Columns.Add("CHART_TYPE_PG", typeof(string));

            row = dtChartTypePG.Rows.Add();
            row["CHART_TYPE_PG_CODE"] = 1;
            row["CHART_TYPE_PG"] = "1";
            row.AcceptChanges();
            dtChartTypePG.AcceptChanges();

            row = dtChartTypePG.Rows.Add();
            row["CHART_TYPE_PG_CODE"] = 2;
            row["CHART_TYPE_PG"] = "2";
            row.AcceptChanges();
            dtChartTypePG.AcceptChanges();

            row = dtChartTypePG.Rows.Add();
            row["CHART_TYPE_PG_CODE"] = 2;
            row["CHART_TYPE_PG"] = "3";
            row.AcceptChanges();
            dtChartTypePG.AcceptChanges();
            ChartTypesPGDataSource = dtChartTypePG.DefaultView;
            ChartTypePGSelectedRow = ChartTypesPGDataSource[0];

            this.chartTypeSelectedItemChangedCommand = new DelegateCommand(this.ChartTypeChanged);
            this.endDateOfWorkingYearOnChangedCommand = new DelegateCommand(this.endDateOfWorkingYearOnChanged);
            this.chartTypePGSelectedItemChangedCommand = new DelegateCommand(this.ChartTypePGChanged);
            MandatoryFields.END_DATE_OF_WORKING_YEAR = new DateTime(Convert.ToDateTime(TodayDate).Year + 1, 3, 31);

            ChartTypeChanged();
            ChartTypePGChanged();

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

        private readonly ICommand printCommand;
        public ICommand PrintClickCommand { get { return this.printCommand; } }
        private void PrintSubmitCommand()
        {

            if (!MandatoryFields.GridData.IsNotNullOrEmpty() || MandatoryFields.GridData.Table.Rows.Count == 0)
            {
                ShowInformationMessage(PDMsg.NoRecordsPrint);
                return;
            }

            string fileName = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog();
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


            if (MandatoryFields.GridData.Table.ExportToExcel(fileName))
            {
                ShowInformationMessage("Succesfully Exported to Excel");
            }

        }

        private readonly ICommand refreshCommand;
        public ICommand RefreshClickCommand { get { return this.refreshCommand; } }
        private void RefreshSubmitCommand()
        {

            ChartTypeChanged();
        }

        private ReportOTPMChartsModel _mandatoryFields = null;
        public ReportOTPMChartsModel MandatoryFields
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

        private DataView _chartTypesDataSource = null;
        public DataView ChartTypesDataSource
        {
            get
            {
                return _chartTypesDataSource;
            }
            set
            {
                _chartTypesDataSource = value;
                NotifyPropertyChanged("ChartTypesDataSource");
            }
        }

        private DataRowView _chartTypeSelectedRow;
        public DataRowView ChartTypeSelectedRow
        {
            get
            {
                return _chartTypeSelectedRow;
            }

            set
            {
                _chartTypeSelectedRow = value;
            }
        }

        private readonly ICommand chartTypeSelectedItemChangedCommand = null;
        public ICommand ChartTypeSelectedItemChangedCommand { get { return this.chartTypeSelectedItemChangedCommand; } }
        private void ChartTypeChanged()
        {
            if (!MandatoryFields.END_DATE_OF_WORKING_YEAR.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("End Date of Working Year"));
                return;
            }

            if (_chartTypeSelectedRow.IsNotNullOrEmpty())
            {
                MandatoryFields.ChartHeader = MandatoryFields.CHART_TYPE;
                ChartTypePGHasDropDownVisibility = Visibility.Hidden;
                MandatoryFields.CHART_TYPE = _chartTypeSelectedRow.Row["CHART_TYPE"].ToValueAsString();
                MandatoryFields.GRID_TITLE = MandatoryFields.CHART_TYPE;

                DateTime? start_date = Convert.ToDateTime(MandatoryFields.END_DATE_OF_WORKING_YEAR);
                DateTime? first_month_date = null;
                int count = 0;
                DateTime? minDate = null;
                DateTime? maxDate = null;
                int monthDifference = 0;
                List<DDCOST_PROCESS_DATA> lstPROCESS_DATA = null;

                switch (MandatoryFields.CHART_TYPE)
                {
                    case "Development Lead Time - PG":
                        ChartTypePGHasDropDownVisibility = Visibility.Visible;
                        ChartTypePGChanged();
                        break;
                    case "Plan-Adherence":
                        #region Plan-Adherence
                        //msOTPMChart2.Visible = True
                        //mschartOTPM.Visible = False

                        start_date = Convert.ToDateTime(start_date).AddDays(1);
                        start_date = Convert.ToDateTime(start_date).AddYears(-3);
                        first_month_date = start_date;

                        minDate = start_date < Convert.ToDateTime(MandatoryFields.END_DATE_OF_WORKING_YEAR).AddYears(-1) ? start_date : Convert.ToDateTime(MandatoryFields.END_DATE_OF_WORKING_YEAR).AddYears(-1);
                        maxDate = start_date > Convert.ToDateTime(MandatoryFields.END_DATE_OF_WORKING_YEAR).AddYears(-1) ? start_date : Convert.ToDateTime(MandatoryFields.END_DATE_OF_WORKING_YEAR).AddYears(-1);
                        monthDifference = ((Convert.ToDateTime(maxDate).Year - Convert.ToDateTime(minDate).Year) * 12) + Convert.ToDateTime(maxDate).Month - Convert.ToDateTime(minDate).Month;
                        count = Math.Abs(monthDifference) - 5;

                        start_date = Convert.ToDateTime(start_date).AddMonths(-1);
                        MandatoryFields.ChartHeader = "Target : Aim for 100% development plan Adherence";


                        //case "Plan-Adherence" Start
                        List<MFM_MAST> lstMFM_MAS = (from row in bll.GetAllPlanAdherence()
                                                     where row.PPAP_ACTUAL_DT != null && row.STATUS == null
                                                     select row).ToList<MFM_MAST>();

                        lstPROCESS_DATA = new List<DDCOST_PROCESS_DATA>(); //For store temp Entity
                        for (int i = 1; i < count; i++)
                        {
                            minDate = start_date;
                            decimal? planned = 0;
                            decimal? met = 0;
                            decimal? plan_adherence = 0;
                            if (i <= 4)
                            {
                                maxDate = Convert.ToDateTime(start_date).AddMonths(6).AddDays(-1);
                                start_date = Convert.ToDateTime(start_date).AddMonths(6);
                            }
                            else
                            {
                                maxDate = Convert.ToDateTime(start_date).AddMonths(1).AddDays(-1);
                                start_date = Convert.ToDateTime(start_date).AddMonths(1);
                            }

                            planned = (from row in lstMFM_MAS.AsEnumerable()
                                       where row.PPAP_ACTUAL_DT >= minDate && row.PPAP_ACTUAL_DT <= maxDate
                                       select row).Count();

                            met = (from row in lstMFM_MAS.AsEnumerable()
                                   where row.PPAP_ACTUAL_DT >= minDate && row.PPAP_ACTUAL_DT <= maxDate && row.PPAP_ACTUAL_DT <= row.PPAP_PLAN
                                   select row).Count();
                            if (planned != 0)
                                plan_adherence = Math.Round(Convert.ToDecimal((met / planned) * 100), 1);

                            lstPROCESS_DATA.Add(new DDCOST_PROCESS_DATA()
                            {
                                OPERATION = minDate.ToValueAsString(),
                                COST_CENT_CODE = maxDate.ToValueAsString(),
                                VAR_COST = planned,
                                FIX_COST = met,
                                SPL_COST = plan_adherence
                            });
                        }

                        if (lstPROCESS_DATA.IsNotNullOrEmpty())
                        {
                            DataTable dtProcessData = new DataTable("PGCATOGORY");
                            dtProcessData.Rows.Clear();
                            dtProcessData.Columns.Clear();
                            dtProcessData.Columns.Add("Months", typeof(string));
                            long colCount = 1;
                            start_date = Convert.ToDateTime(first_month_date).AddMonths(-1);
                            foreach (DDCOST_PROCESS_DATA processData in lstPROCESS_DATA)
                            {
                                string colName = "";

                                minDate = start_date;
                                if (colCount <= 4)
                                {
                                    maxDate = Convert.ToDateTime(minDate).AddMonths(6).AddDays(-1);
                                    colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy") + doubleDateSep + Convert.ToDateTime(maxDate).ToString("MMM" + monthYearSep + "yy");
                                    start_date = Convert.ToDateTime(start_date).AddMonths(6);
                                }
                                else
                                {
                                    maxDate = Convert.ToDateTime(minDate).AddMonths(1).AddDays(-1);
                                    colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy");
                                    start_date = Convert.ToDateTime(start_date).AddMonths(1);
                                }

                                if (!dtProcessData.Columns.Contains(colName) && colName.IsNotNullOrEmpty())
                                    dtProcessData.Columns.Add(colName, typeof(decimal));
                                colCount++;
                            }

                            colCount = 1;
                            start_date = Convert.ToDateTime(first_month_date).AddMonths(-1);
                            dtProcessData.Rows.Clear();

                            DataRow rowPlanAdherence = dtProcessData.Rows.Add();
                            DataRow rowPlanned = dtProcessData.Rows.Add();
                            DataRow rowMet = dtProcessData.Rows.Add();

                            rowPlanAdherence["Months"] = "%Plan Adherence";
                            rowPlanned["Months"] = "Planned(Numbers)";
                            rowMet["Months"] = "Met(Numbers)";

                            foreach (DDCOST_PROCESS_DATA processData in lstPROCESS_DATA)
                            {
                                string colName = "";

                                minDate = start_date;
                                if (colCount <= 4)
                                {
                                    maxDate = Convert.ToDateTime(minDate).AddMonths(6).AddDays(-1);
                                    colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy") + doubleDateSep + Convert.ToDateTime(maxDate).ToString("MMM" + monthYearSep + "yy");
                                    start_date = Convert.ToDateTime(start_date).AddMonths(6);
                                }
                                else
                                {
                                    maxDate = Convert.ToDateTime(minDate).AddMonths(1).AddDays(-1);
                                    colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy");
                                    start_date = Convert.ToDateTime(start_date).AddMonths(1);
                                }

                                if (!dtProcessData.Columns.Contains(colName) && colName.IsNotNullOrEmpty())
                                    dtProcessData.Columns.Add(colName, typeof(decimal));

                                rowPlanAdherence[colName] = Math.Round(Convert.ToDecimal(processData.SPL_COST), 2);
                                rowPlanned[colName] = Math.Round(Convert.ToDecimal(processData.VAR_COST), 2);
                                rowMet[colName] = Math.Round(Convert.ToDecimal(processData.FIX_COST), 2);

                                rowPlanAdherence["Months"] = "%Plan Adherence";
                                rowPlanned["Months"] = "Planned(Numbers)";
                                rowMet["Months"] = "Met(Numbers)";

                                rowPlanAdherence.AcceptChanges();
                                rowMet.AcceptChanges();
                                rowPlanned.AcceptChanges();
                                colCount++;
                            }


                            dtProcessData.AcceptChanges();
                            MandatoryFields.GridData = null;
                            MandatoryFields.GridData = dtProcessData.DefaultView;

                        }
                        #endregion

                        break;
                    case "First-time-right":
                        //msOTPMChart2.Visible = True
                        //mschartOTPM.Visible = False

                        start_date = Convert.ToDateTime(start_date).AddDays(1);
                        start_date = Convert.ToDateTime(start_date).AddYears(-3);
                        first_month_date = start_date;

                        minDate = start_date < Convert.ToDateTime(MandatoryFields.END_DATE_OF_WORKING_YEAR).AddYears(-1) ? start_date : Convert.ToDateTime(MandatoryFields.END_DATE_OF_WORKING_YEAR).AddYears(-1);
                        maxDate = start_date > Convert.ToDateTime(MandatoryFields.END_DATE_OF_WORKING_YEAR).AddYears(-1) ? start_date : Convert.ToDateTime(MandatoryFields.END_DATE_OF_WORKING_YEAR).AddYears(-1);
                        monthDifference = ((Convert.ToDateTime(maxDate).Year - Convert.ToDateTime(minDate).Year) * 12) + Convert.ToDateTime(maxDate).Month - Convert.ToDateTime(minDate).Month;
                        count = Math.Abs(monthDifference) - 5;

                        start_date = Convert.ToDateTime(start_date).AddMonths(-1);
                        MandatoryFields.ChartHeader = "Target : Aim for 100%  First time right initial samples";

                        // case "First-time-right" Start
                        List<PRD_MAST> lstPRD_MAST = (from row in bll.GetAllFirstTimeRight()
                                                      select row).ToList<PRD_MAST>();


                        lstPROCESS_DATA = new List<DDCOST_PROCESS_DATA>(); //For store temp Entity
                        for (int i = 1; i < count; i++)
                        {
                            minDate = start_date;
                            decimal? varReceipt = 0;
                            decimal? varRight = 0;
                            decimal? var_tot_right = 0;
                            if (i <= 4)
                            {
                                maxDate = Convert.ToDateTime(start_date).AddMonths(6).AddDays(-1);
                                start_date = Convert.ToDateTime(start_date).AddMonths(6);
                            }
                            else
                            {
                                maxDate = Convert.ToDateTime(start_date).AddMonths(1).AddDays(-1);
                                start_date = Convert.ToDateTime(start_date).AddMonths(1);
                            }

                            varReceipt = (from row in lstPRD_MAST.AsEnumerable()
                                          where (row.SAMP_SUBMIT_DATE >= minDate && row.SAMP_SUBMIT_DATE <= maxDate) && (row.PSW_ST == "YES" || row.PSW_ST == "NO" || row.PSW_ST == "WAIVED")
                                          select row).Count();

                            varRight = (from row in lstPRD_MAST.AsEnumerable()
                                        where (row.SAMP_SUBMIT_DATE >= minDate && row.SAMP_SUBMIT_DATE <= maxDate) && (row.PSW_ST == "YES" || row.PSW_ST == "WAIVED")
                                        select row).Count();

                            if (varReceipt != 0)
                                var_tot_right = Math.Round(Convert.ToDecimal((varRight / varReceipt) * 100), 0);

                            lstPROCESS_DATA.Add(new DDCOST_PROCESS_DATA()
                            {
                                OPERATION = minDate.ToValueAsString(),
                                COST_CENT_CODE = maxDate.ToValueAsString(),
                                VAR_COST = varReceipt,
                                FIX_COST = varRight,
                                SPL_COST = var_tot_right
                            });
                        }

                        if (lstPROCESS_DATA.IsNotNullOrEmpty())
                        {
                            DataTable dtProcessData = new DataTable("PGCATOGORY");
                            dtProcessData.Rows.Clear();
                            dtProcessData.Columns.Clear();
                            dtProcessData.Columns.Add("Months", typeof(string));
                            long colCount = 1;
                            start_date = Convert.ToDateTime(first_month_date).AddMonths(-1);
                            foreach (DDCOST_PROCESS_DATA processData in lstPROCESS_DATA)
                            {
                                string colName = "";

                                minDate = start_date;
                                if (colCount <= 4)
                                {
                                    maxDate = Convert.ToDateTime(minDate).AddMonths(6).AddDays(-1);
                                    colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy") + doubleDateSep + Convert.ToDateTime(maxDate).ToString("MMM" + monthYearSep + "yy");
                                    start_date = Convert.ToDateTime(start_date).AddMonths(6);
                                }
                                else
                                {
                                    maxDate = Convert.ToDateTime(minDate).AddMonths(1).AddDays(-1);
                                    colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy");
                                    start_date = Convert.ToDateTime(start_date).AddMonths(1);
                                }

                                if (!dtProcessData.Columns.Contains(colName) && colName.IsNotNullOrEmpty())
                                    dtProcessData.Columns.Add(colName, typeof(decimal));
                                colCount++;
                            }

                            colCount = 1;
                            start_date = Convert.ToDateTime(first_month_date).AddMonths(-1);
                            dtProcessData.Rows.Clear();

                            DataRow rowPlanAdherence = dtProcessData.Rows.Add();
                            DataRow rowPlanned = dtProcessData.Rows.Add();
                            DataRow rowMet = dtProcessData.Rows.Add();

                            rowPlanAdherence["Months"] = "% First Time Right";
                            rowPlanned["Months"] = "Right(Numbers)";
                            rowMet["Months"] = "Receipt(Numbers)";

                            foreach (DDCOST_PROCESS_DATA processData in lstPROCESS_DATA)
                            {
                                string colName = "";

                                minDate = start_date;
                                if (colCount <= 4)
                                {
                                    maxDate = Convert.ToDateTime(minDate).AddMonths(6).AddDays(-1);
                                    colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy") + doubleDateSep + Convert.ToDateTime(maxDate).ToString("MMM" + monthYearSep + "yy");
                                    start_date = Convert.ToDateTime(start_date).AddMonths(6);
                                }
                                else
                                {
                                    maxDate = Convert.ToDateTime(minDate).AddMonths(1).AddDays(-1);
                                    colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy");
                                    start_date = Convert.ToDateTime(start_date).AddMonths(1);
                                }

                                if (!dtProcessData.Columns.Contains(colName) && colName.IsNotNullOrEmpty())
                                    dtProcessData.Columns.Add(colName, typeof(decimal));

                                rowPlanAdherence[colName] = Math.Round(Convert.ToDecimal(processData.SPL_COST), 0);
                                rowPlanned[colName] = Math.Round(Convert.ToDecimal(processData.FIX_COST), 0);
                                rowMet[colName] = Math.Round(Convert.ToDecimal(processData.VAR_COST), 0);

                                rowPlanAdherence["Months"] = "% First Time Right";
                                rowPlanned["Months"] = "Right(Numbers)";
                                rowMet["Months"] = "Receipt(Numbers)";

                                rowPlanAdherence.AcceptChanges();
                                rowMet.AcceptChanges();
                                rowPlanned.AcceptChanges();
                                colCount++;
                            }


                            dtProcessData.AcceptChanges();
                            MandatoryFields.GridData = null;
                            MandatoryFields.GridData = dtProcessData.DefaultView;

                        }

                        break;

                }
            }
        }

        private Visibility _chartTypeHasDropDownVisibility = Visibility.Visible;
        public Visibility ChartTypeHasDropDownVisibility
        {
            get { return _chartTypeHasDropDownVisibility; }
            set
            {
                _chartTypeHasDropDownVisibility = value;
                NotifyPropertyChanged("ChartTypeHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _chartTypeDropDownItems;
        public ObservableCollection<DropdownColumns> ChartTypeDropDownItems
        {
            get
            {
                return _chartTypeDropDownItems;
            }
            set
            {
                _chartTypeDropDownItems = value;
                OnPropertyChanged("ChartTypeDropDownItems");
            }
        }

        private DateTime? _todayDate;
        private DateTime? TodayDate
        {
            get { return _todayDate; }
            set
            {
                _todayDate = value;
                NotifyPropertyChanged("TodayDate");
            }
        }

        private readonly ICommand endDateOfWorkingYearOnChangedCommand;
        public ICommand EndDateOfWorkingYearOnChangedCommand { get { return this.endDateOfWorkingYearOnChangedCommand; } }
        private void endDateOfWorkingYearOnChanged()
        {
            ChartTypeChanged();
        }

        private DataView _chartTypesPGDataSource = null;
        public DataView ChartTypesPGDataSource
        {
            get
            {
                return _chartTypesPGDataSource;
            }
            set
            {
                _chartTypesPGDataSource = value;
                NotifyPropertyChanged("ChartTypesPGDataSource");
            }
        }

        private DataRowView _chartTypePGSelectedRow;
        public DataRowView ChartTypePGSelectedRow
        {
            get
            {
                return _chartTypePGSelectedRow;
            }

            set
            {
                _chartTypePGSelectedRow = value;
            }
        }

        private readonly ICommand chartTypePGSelectedItemChangedCommand = null;
        public ICommand ChartTypePGSelectedItemChangedCommand { get { return this.chartTypePGSelectedItemChangedCommand; } }
        private void ChartTypePGChanged()
        {
            if (!MandatoryFields.END_DATE_OF_WORKING_YEAR.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("End Date of Working Year"));
                return;
            }

            ChartTitle = "INITIAL CONTROL(IC) - PRODUCTS";
            if (ChartTypePGHasDropDownVisibility != Visibility.Visible) return;
            if (_chartTypePGSelectedRow.IsNotNullOrEmpty())
            {
                MandatoryFields.CHART_TYPE_PG = _chartTypePGSelectedRow.Row["CHART_TYPE_PG"].ToValueAsString();

                switch (MandatoryFields.CHART_TYPE_PG.ToIntValue())
                {
                    case 1:
                        MandatoryFields.ChartHeader = "Developement Lead time-PG1";
                        break;
                    case 2:
                        MandatoryFields.ChartHeader = "Developement Lead time-PG2";
                        break;
                    case 3:
                        MandatoryFields.ChartHeader = "Developement Lead time-PG3";
                        break;
                }

                double leadtime = 0;
                DateTime? start_date = Convert.ToDateTime(MandatoryFields.END_DATE_OF_WORKING_YEAR);
                DateTime? first_month_date = null;
                int count = 0;
                int pgcatogory = MandatoryFields.CHART_TYPE_PG.ToIntValue();

                start_date = Convert.ToDateTime(start_date).AddDays(1);
                start_date = Convert.ToDateTime(start_date).AddYears(-3);
                first_month_date = start_date;

                DateTime? minDate = Convert.ToDateTime(MandatoryFields.END_DATE_OF_WORKING_YEAR).AddYears(-1);
                DateTime? maxDate = TodayDate;
                int monthDifference = ((Convert.ToDateTime(maxDate).Year * 12) + (Convert.ToDateTime(maxDate).Month)) - ((Convert.ToDateTime(minDate).Year * 12) + (Convert.ToDateTime(minDate).Month));
                count = 24 + Math.Abs(monthDifference);

                start_date = Convert.ToDateTime(start_date).AddMonths(-1);

                List<MFM_MAST> lstLeadTime = bll.GetAllLeadTime();
                List<MFM_MAST> lstLead = bll.GetAllLeadTime();

                List<PGCATOGORY> lstPGCatogory = new List<PGCATOGORY>();

                for (int i = 1; i <= count; i++)
                {
                    start_date = Convert.ToDateTime(start_date).AddMonths(1);
                    switch (MandatoryFields.CHART_TYPE_PG.ToIntValue())
                    {
                        case 3:
                            lstLead = (from row in lstLeadTime.AsEnumerable()
                                       where (row.RESP == MandatoryFields.CHART_TYPE_PG || row.RESP == "3F") &&
                                       row.ENTERED_DATE >= start_date && row.ENTERED_DATE <= Convert.ToDateTime(start_date).AddMonths(1).AddDays(-1)
                                       select row).ToList<MFM_MAST>();
                            break;
                        case 2:
                        case 1:
                            lstLead = (from row in lstLeadTime.AsEnumerable()
                                       where (row.RESP == MandatoryFields.CHART_TYPE_PG) &&
                                       row.ENTERED_DATE >= start_date && row.ENTERED_DATE <= Convert.ToDateTime(start_date).AddMonths(1).AddDays(-1)
                                       select row).ToList<MFM_MAST>();
                            break;
                    }

                    leadtime = 0;
                    if (lstLead.IsNotNullOrEmpty() && lstLead.Count > 0)
                    {
                        foreach (MFM_MAST jMFM_MAST in lstLead)
                        {
                            if (!jMFM_MAST.DOC_REL_DT_ACTUAL.IsNotNullOrEmpty() || !jMFM_MAST.PPAP_ACTUAL_DT.IsNotNullOrEmpty())
                            {
                                switch (MandatoryFields.CHART_TYPE_PG.ToIntValue())
                                {
                                    case 1:
                                        leadtime = leadtime + 60;
                                        break;
                                    case 2:
                                        leadtime = leadtime + 90;
                                        break;
                                    case 3:
                                        leadtime = leadtime + 100;
                                        break;
                                }
                            }
                            else
                            {
                                minDate = jMFM_MAST.DOC_REL_DT_ACTUAL < jMFM_MAST.PPAP_ACTUAL_DT ? jMFM_MAST.DOC_REL_DT_ACTUAL : Convert.ToDateTime(jMFM_MAST.PPAP_ACTUAL_DT);
                                maxDate = jMFM_MAST.DOC_REL_DT_ACTUAL > jMFM_MAST.PPAP_ACTUAL_DT ? jMFM_MAST.DOC_REL_DT_ACTUAL : Convert.ToDateTime(jMFM_MAST.PPAP_ACTUAL_DT);
                                TimeSpan dateDifference = (Convert.ToDateTime(maxDate) - Convert.ToDateTime(minDate));

                                switch (MandatoryFields.CHART_TYPE_PG.ToIntValue())
                                {
                                    case 1:
                                        leadtime = leadtime + Math.Min(60, dateDifference.TotalDays);
                                        break;
                                    case 2:
                                        leadtime = leadtime + Math.Min(60, dateDifference.TotalDays);
                                        break;
                                    case 3:
                                        leadtime = leadtime + dateDifference.TotalDays > 150 ? 100 : dateDifference.TotalDays;
                                        break;
                                }
                            }
                        }

                        leadtime = leadtime / lstLead.Count;
                    }

                    lstPGCatogory.Add(new PGCATOGORY() { PGCATOGORY1 = MandatoryFields.CHART_TYPE_PG.ToIntValue(), LEADTIME = Convert.ToDecimal(leadtime), NO_OF_PRODUCTS = lstLead.Count, PG_DATE = start_date });

                }

                List<PGCATOGORY> lstPGCatogoryResult = new List<PGCATOGORY>();

                if (lstPGCatogory.IsNotNullOrEmpty() && lstPGCatogory.Count > 0)
                {
                    for (int i = 1; i <= 4; i++)
                    {

                        minDate = first_month_date;
                        maxDate = Convert.ToDateTime(first_month_date).AddMonths(6).AddDays(-1);

                        decimal? decLeadTime = (from row in lstPGCatogory.AsEnumerable()
                                                where (row.PGCATOGORY1 == Convert.ToDecimal(MandatoryFields.CHART_TYPE_PG)) &&
                                                row.PG_DATE >= minDate && row.PG_DATE <= maxDate
                                                select row.LEADTIME).Sum();

                        decimal? decNo_Of_Products = (from row in lstPGCatogory.AsEnumerable()
                                                      where (row.PGCATOGORY1 == Convert.ToDecimal(MandatoryFields.CHART_TYPE_PG)) &&
                                                      row.PG_DATE >= minDate && row.PG_DATE <= maxDate
                                                      select row.NO_OF_PRODUCTS).Sum();

                        DateTime? decPG_Date = (from row in lstPGCatogory.AsEnumerable()
                                                where (row.PGCATOGORY1 == Convert.ToDecimal(MandatoryFields.CHART_TYPE_PG)) &&
                                                row.PG_DATE >= minDate && row.PG_DATE <= maxDate
                                                select row.PG_DATE).Min();

                        List<PGCATOGORY> lstremove = (from row in lstPGCatogory.AsEnumerable()
                                                      where (row.PGCATOGORY1 == Convert.ToDecimal(MandatoryFields.CHART_TYPE_PG)) &&
                                                      row.PG_DATE >= minDate && row.PG_DATE <= maxDate
                                                      select row).ToList();

                        foreach (PGCATOGORY pg in lstremove)
                        {
                            lstPGCatogory.Remove(pg);
                        }

                        lstPGCatogoryResult.Add(new PGCATOGORY()
                        {
                            PGCATOGORY1 = Convert.ToDecimal(MandatoryFields.CHART_TYPE_PG),
                            LEADTIME = decLeadTime.IsNotNullOrEmpty() ? decLeadTime / 6 : 0,
                            NO_OF_PRODUCTS = decNo_Of_Products.IsNotNullOrEmpty() ? decNo_Of_Products / 6 : 0,
                            PG_DATE = decPG_Date
                        });
                        first_month_date = Convert.ToDateTime(first_month_date).AddMonths(6);
                    }
                }

                foreach (PGCATOGORY itempg in lstPGCatogory)
                {
                    lstPGCatogoryResult.Add(itempg.DeepCopy());
                }

                if (lstPGCatogoryResult.IsNotNullOrEmpty())
                {
                    List<PGCATOGORY> lstResult = (from row in lstPGCatogoryResult.AsEnumerable()
                                                  orderby row.PG_DATE ascending
                                                  select row).Distinct<PGCATOGORY>().ToList<PGCATOGORY>();

                    long colCount = 1;
                    DataTable dtPGCATOGORY = new DataTable("PGCATOGORY");
                    dtPGCATOGORY.Rows.Clear();
                    dtPGCATOGORY.Columns.Clear();
                    dtPGCATOGORY.Columns.Add("Months", typeof(string));
                    foreach (PGCATOGORY pgCategory in lstResult)
                    {
                        if (pgCategory.PG_DATE.IsNotNullOrEmpty())
                        {
                            minDate = pgCategory.PG_DATE;
                            maxDate = Convert.ToDateTime(pgCategory.PG_DATE).AddMonths(6).AddDays(-1);
                            string colName = colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy");
                            if (colCount <= 4)
                            {
                                colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy") + doubleDateSep + Convert.ToDateTime(maxDate).ToString("MMM" + monthYearSep + "yy");
                            }
                            else
                            {
                                colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy");
                            }
                            if (!dtPGCATOGORY.Columns.Contains(colName))
                                dtPGCATOGORY.Columns.Add(colName, typeof(decimal));
                        }
                        colCount++;
                    }

                    colCount = 1;
                    dtPGCATOGORY.Rows.Clear();

                    DataRow rowLeadTime = dtPGCATOGORY.Rows.Add();
                    DataRow rowTarget = dtPGCATOGORY.Rows.Add();
                    DataRow rowNoOfProducts = dtPGCATOGORY.Rows.Add();

                    foreach (PGCATOGORY pgCategory in lstResult)
                    {
                        if (pgCategory.PG_DATE.IsNotNullOrEmpty())
                        {
                            minDate = pgCategory.PG_DATE;
                            maxDate = Convert.ToDateTime(pgCategory.PG_DATE).AddMonths(6).AddDays(-1);
                            string colName = colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy");
                            if (colCount <= 4)
                            {
                                colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy") + doubleDateSep + Convert.ToDateTime(maxDate).ToString("MMM" + monthYearSep + "yy");
                            }
                            else
                            {
                                colName = Convert.ToDateTime(minDate).ToString("MMM" + monthYearSep + "yy");
                            }
                            if (!dtPGCATOGORY.Columns.Contains(colName)) dtPGCATOGORY.Columns.Add(colName, typeof(decimal));

                            rowLeadTime[colName] = Math.Round(Convert.ToDecimal(pgCategory.LEADTIME), 2);
                            rowNoOfProducts[colName] = Math.Round(Convert.ToDecimal(pgCategory.NO_OF_PRODUCTS), 2);

                            switch (MandatoryFields.CHART_TYPE_PG.ToIntValue())
                            {
                                case 1:
                                    rowTarget[colName] = 30;
                                    break;
                                case 2:
                                    rowTarget[colName] = 45;
                                    break;
                                case 3:
                                    rowTarget[colName] = 75;
                                    break;
                            }

                            rowLeadTime["Months"] = "Lead Time in days";
                            rowNoOfProducts["Months"] = "No.of.Products";
                            rowTarget["Months"] = "Target";

                            rowLeadTime.AcceptChanges();
                            rowTarget.AcceptChanges();
                            rowNoOfProducts.AcceptChanges();


                        }
                        colCount++;

                    }
                    dtPGCATOGORY.AcceptChanges();
                    MandatoryFields.GridData = null;
                    MandatoryFields.GridData = dtPGCATOGORY.DefaultView;

                }
                //rst As New ADODB.Recordset, 
                //Dim rsPG As ADODB.Recordset, rsLead As ADODB.Recordset
                ////    pgcatogory = Val(ltbPGcatogory.Value)
                ////    Start_Date = Format(DateAdd("d", 1, ltbfromDate.text), "dd-mmm-yyyy")
                ////    Start_Date = Format(DateAdd("yyyy", -3, Start_Date), "dd-mmm-yyyy")
                ////    first_month_date = Format(Start_Date, "dd-mmm-yyyy")
                ////    Count = 24 + Abs(DateDiff("m", gvarSystemDate, Format(DateAdd("yyyy", -1, ltbfromDate.text), "dd-mmm-yyyy")))
                ////    Start_Date = Format(DateAdd("m", -1, Start_Date), "dd-mmm-yyyy")
                ////    For i = 1 To Count
                ////        Start_Date = Format(DateAdd("m", 1, Start_Date), "dd-mmm-yyyy")

                ////        SqlLeadtime = "select PG_CATEGORY PG,DOC_REL_DT_ACTUAL,PPAP_ACTUAL_DT from mfm_mast a ,"
                ////        SqlLeadtime = SqlLeadtime & " prd_mast b where  b.pg_category in ('1','2','3','3F')"
                ////        SqlLeadtime = SqlLeadtime & " and B.Allot_date is not null and A.DOC_REL_DT_ACTUAL is not null and"
                ////        SqlLeadtime = SqlLeadtime & " samp_submit_date Between '" & Format(Start_Date, "dd-mmm-yyyy") & "'"
                ////        SqlLeadtime = SqlLeadtime & " and '" & Format(DateAdd("m", 1, Start_Date), "dd-mmm-yyyy") & "'"
                ////        SqlLeadtime = SqlLeadtime & " and samp_submit_date is not null and b.part_no = a.part_no(+)"

                ////        Set rsLead = New ADODB.Recordset
                ////        Set rsLead = fnMdOpenRs(SqlLeadtime, adOpenForwardOnly)
                ////        rsLead.Filter = adFilterNone

                ////        If pgcatogory = 3 Then
                ////            rsLead.Filter = " PG= '" & pgcatogory & " ' or PG= '" & "3F" & " '  "
                ////        Else
                ////            rsLead.Filter = " PG= '" & pgcatogory & " ' "
                ////        End If
                ////        If rsLead.RecordCount > 0 Then
                ////            rsLead.MoveFirst
                ////            For j = 0 To rsLead.RecordCount - 1
                ////                If IsNull(rsLead.Fields(1)) Or IsNull(rsLead.Fields(2)) Then
                ////                    Select Case pgcatogory
                ////                    Case 1:   leadtime = leadtime + 60
                ////                    Case 2:   leadtime = leadtime + 90
                ////                    Case 3:   leadtime = leadtime + 100
                ////                    End Select
                ////                Else
                ////                    Select Case pgcatogory
                ////                    Case 1: leadtime = leadtime + IIf(DateDiff("d", rsLead.Fields(1), rsLead.Fields(2)) > 60, 60, DateDiff("d", rsLead.Fields(1), rsLead.Fields(2)))
                ////                    Case 2: leadtime = leadtime + IIf(DateDiff("d", rsLead.Fields(1), rsLead.Fields(2)) > 90, 90, DateDiff("d", rsLead.Fields(1), rsLead.Fields(2)))
                ////                    Case 3: leadtime = leadtime + IIf(DateDiff("d", rsLead.Fields(1), rsLead.Fields(2)) > 150, 100, DateDiff("d", rsLead.Fields(1), rsLead.Fields(2)))
                ////                    End Select
                ////                End If
                ////                rsLead.MoveNext
                ////            Next
                ////            leadtime = leadtime / rsLead.RecordCount
                ////        Else
                ////            leadtime = 0
                ////        End If
                ////        Sql = "insert into pgcatogory values(   '" & pgcatogory & " ' , '" & leadtime & " ','" & rsLead.RecordCount & " ' ,'" & Format(Start_Date, "dd-mmm-yyyy") & " ')"
                ////        gvarcnn.Execute Sql
                ////    Next i
                ////    For i = 1 To 4
                ////        Sql = "select sum(leadtime),sum(no_of_products),min(PG_DATE)  from pgcatogory where  pgcatogory = '" & pgcatogory & "' and PG_DATE between '" & Format(first_month_date, "dd-mmm-yyyy") & "'"
                ////        Sql = Sql & " and '" & Format(DateAdd("m", 5, first_month_date), "dd-mmm-yyyy") & "'"
                ////        Set rst = fnMdOpenRs(Sql, adOpenForwardOnly)
                ////        SqlDelete = " delete from pgcatogory  where  PG_DATE between '" & Format(first_month_date, "dd-mmm-yyyy") & "'"
                ////        SqlDelete = SqlDelete & " and '" & Format(DateAdd("m", 5, first_month_date), "dd-mmm-yyyy") & "'"
                ////        gvarcnn.Execute SqlDelete
                ////        Sql = "insert into pgcatogory values('" & pgcatogory & " ', '" & IIf(IsNull(rst.Fields(0)), 0, rst(0).Value) / 6 & " ','" & IIf(IsNull(rst.Fields(1)), 0, rst(1).Value) / 6 & " ' ,'" & IIf(IsNull(rst(2).Value), Null, Format(rst.Fields(2), "dd-mmm-yyyy")) & " ')"
                ////        gvarcnn.Execute Sql
                ////        first_month_date = Format(DateAdd("m", 6, first_month_date), "dd-mmm-yyyy")
                ////    Next i
                ////    Sql = "select distinct * from pgcatogory order by  pg_date asc "
                ////    Set rst = fnMdOpenRs(Sql, adOpenForwardOnly)
                //    With mschartOTPM
                ////        With flxPGS
                ////            .Cols = rst.RecordCount + 1: .FixedRows = 1: .Rows = 4: .Col = 0: .Row = 0
                ////            .text = "Month": .Row = 1: .Col = 0: .text = "Lead Time"
                ////            .Row = 2: .Col = 0: .text = "Target": .Row = 3: .Col = 0: .text = "No of Prods"
                ////            For i = 1 To rst.RecordCount
                ////                .Col = i
                ////                If i >= 1 And i <= 4 Then .ColWidth(i) = 1200 Else .ColWidth(i) = 600
                ////            Next i
                ////        End With
                ////        rst.MoveFirst
                //        .ChartType = VtChChartType2dLine
                //        .ColumnCount = rst.RecordCount
                //        .RowCount = 3
                //        .Plot.Axis(VtChAxisIdX).AxisTitle.text = "Months"
                ////        .title.text = "INITIAL CONTROL(IC) -PRODUCTS"
                ////        For i = 1 To rst.RecordCount
                //            .Column = i: 
                ////             flxPGS.TextMatrix(1, i) = rst.Fields(1).Value
                ////            flxPGS.TextMatrix(3, i) = rst.Fields(2).Value
                ////            .Row = 1: .RowLabel = "No.of.Products": .Data = rst.Fields(2).Value
                ////            .Row = 2: .RowLabel = "Lead Time in days ": .Data = rst.Fields(1).Value
                ////            .Row = 3: .RowLabel = "Target"
                ////            Select Case pgcatogory
                ////                Case 1: 
                //                      .Data = 30: 
                ////                    lblHead.Caption = "Developement Lead time-PG1 "
                ////                    flxPGS.TextMatrix(2, i) = 30
                ////                Case 2: 
                //                      .Data = 45:  
                ////                    lblHead.Caption = "Developement Lead time-PG2 "
                ////                    flxPGS.TextMatrix(2, i) = 45
                ////                Case 3:
                //                      .Data = 75: 
                ////                     lblHead.Caption = "Developement Lead time-PG3"
                ////                    flxPGS.TextMatrix(2, i) = 75
                ////            End Select
                ////            If Not IsNull(rst.Fields(3).Value) Then
                ////                If i <= 4 Then
                ////                    .ColumnLabel = Format(rst.Fields(3).Value, "mmm-yy") & Format(DateAdd("m", 5, rst.Fields(3).Value), "-mmm-yy")
                ////                    flxPGS.TextMatrix(0, i) = .ColumnLabel
                ////                Else
                ////                    .ColumnLabel = Format(rst.Fields(3).Value, "mmm-yy")
                ////                    flxPGS.TextMatrix(0, i) = .ColumnLabel
                ////                End If
                ////            End If
                ////            rst.MoveNext
                ////        Next i
                //        .Footnote.text = ""
                //         mschartOTPM.Footnote.text = ""
                //    End With
                ////    Sql = "delete from pgcatogory"
                ////    gvarcnn.Execute Sql


            }
        }

        private Visibility _chartTypePGHasDropDownVisibility = Visibility.Visible;
        public Visibility ChartTypePGHasDropDownVisibility
        {
            get { return _chartTypePGHasDropDownVisibility; }
            set
            {
                _chartTypePGHasDropDownVisibility = value;
                NotifyPropertyChanged("ChartTypePGHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _chartTypePGDropDownItems;
        public ObservableCollection<DropdownColumns> ChartTypePGDropDownItems
        {
            get
            {
                return _chartTypePGDropDownItems;
            }
            set
            {
                _chartTypePGDropDownItems = value;
                OnPropertyChanged("ChartTypePGDropDownItems");
            }
        }



        private string _chartTitle;
        public string ChartTitle
        {
            get
            {
                return _chartTitle;
            }
            set
            {
                _chartTitle = value;
                OnPropertyChanged("ChartTitle");
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return System.Windows.MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        //private void cmd_ShowChart_Click()
        //{
        //    MandatoryFields.ChartHeader = MandatoryFields.CHART_TYPE;

        //    switch (MandatoryFields.CHART_TYPE)
        //    {
        //        case "Development Lead Time - PG":
        //            if (!MandatoryFields.CHART_TYPE_PG.IsNotNullOrEmpty())
        //            {
        //                ShowInformationMessage(PDMsg.NotEmpty("PG Chart-type"));
        //                return;
        //            }
        //            //msOTPMChart2.Visible = false;
        //            //mschartOTPM.Visible = true;
        //            ChartTypePGChanged();
        //            break;
        //        case "Plan-Adherence": break;
        //        case "First-time-right": break;
        //    }
        //}

    }
}
