using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using ProcessDesigner.Common;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using WPF.MDI;
using ProcessDesigner.UserControls;
namespace ProcessDesigner.ViewModel
{


    class DDPerformanceViewModel : ViewModelBase
    {
        enum performanceOption { CostSheetReceived, CostSheetCompleted, PartNumberAllotted, DocumentReleased, SampleSubmitted };

        WPF.MDI.MdiChild _mdiChild;
        UserInformation _userInformation;
        DDPerformanceBll _ddPerformanceBll;
        performanceOption _perfOption;
        string _reportTitle = "";
        public DDPerformanceViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, System.Windows.Controls.DataGrid dgDDPerformance)
        {
            try
            {
                _userInformation = userInformation;
                _mdiChild = mdiChild;
                _ddPerformanceBll = new DDPerformanceBll(_userInformation);
                this.costSheetReceivedCommand = new DelegateCommand(this.costSheetReceived);
                this.costSheetCompletedCommand = new DelegateCommand(this.costSheetCompleted);
                this.partNumbersAllottedCommand = new DelegateCommand(this.partNumbersAllotted);
                this.documentsReleasedCommand = new DelegateCommand(this.documentsReleased);
                this.samplesSubmittedCommand = new DelegateCommand(this.samplesSubmitted);
                this.refreshCommand = new DelegateCommand(this.Refresh);
                this.performanceSummaryCommand = new DelegateCommand(this.PerformanceSummary);
                this.mOPCommand = new DelegateCommand(this.MOPGraph);
                this.showECNCommand = new DelegateCommand(this.ShowECN);
                this.showPCNCommand = new DelegateCommand(this.ShowPCN);
                this.showDesignersCommand = new DelegateCommand(this.ShowDesigners);
                this.printReportCommand = new DelegateCommand(this.PrintReport);
                this.showProductInformationCommand = new DelegateCommand(this.ShowProductInformation);
                StartDate = Convert.ToDateTime("01/" + DateTime.Now.ToString("MM/yyyy"));
                EndDate = DateTime.Now;
                _perfOption = performanceOption.CostSheetReceived;
                DgDDPerformance = dgDDPerformance;
                SetCombo();
                Refresh();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private string _location;
        public string Location
        {
            get { return _location; }
            set
            {
                _location = value;
                NotifyPropertyChanged("Location");
            }
        }

        private ObservableCollection<string> _hEADER;
        public ObservableCollection<string> HEADER
        {
            get { return _hEADER; }
            set
            {
                _hEADER = value;
                NotifyPropertyChanged("HEADER");
            }
        }

        private string _hEADER1;
        public string HEADER1
        {
            get { return _hEADER1; }
            set
            {
                _hEADER1 = value;
                NotifyPropertyChanged("HEADER1");
            }
        }


        private ObservableCollection<System.Windows.Visibility> _vISIBLE;
        public ObservableCollection<System.Windows.Visibility> VISIBLE
        {
            get { return _vISIBLE; }
            set
            {
                _vISIBLE = value;
                NotifyPropertyChanged("VISIBLE");
            }
        }

        private ObservableCollection<System.Windows.Controls.DataGridLength> _wIDTH;
        public ObservableCollection<System.Windows.Controls.DataGridLength> WIDTH
        {
            get { return _wIDTH; }
            set
            {
                _wIDTH = value;
                NotifyPropertyChanged("WIDTH");
            }
        }


        private DataView _performanceResult;
        public DataView PerformanceResult
        {
            get { return _performanceResult; }
            set
            {
                _performanceResult = value;
                NotifyPropertyChanged("PerformanceResult");
            }
        }

        private DataRowView _selectedRow;
        public DataRowView SelectedRow
        {
            get { return _selectedRow; }
            set
            {
                _selectedRow = value;
                NotifyPropertyChanged("SelectedRow");
            }
        }

        private string _locCode;
        public string LocCode
        {
            get
            {
                return _locCode;
            }
            set
            {
                _locCode = value;
                NotifyPropertyChanged("LocCode");
            }
        }

        private Nullable<DateTime> _startDate;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Start date is Required")]
        public Nullable<DateTime> StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                NotifyPropertyChanged("StartDate");
            }
        }

        private Nullable<DateTime> _endDate;
        [Required(AllowEmptyStrings = false, ErrorMessage = "End date is Required")]
        public Nullable<DateTime> EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                NotifyPropertyChanged("EndDate");
            }
        }



        private string _headerDetails;
        public string HeaderDetails
        {
            get { return _headerDetails; }
            set
            {
                _headerDetails = value;
                NotifyPropertyChanged("HeaderDetails");
            }
        }

        public System.Windows.Controls.DataGrid DgDDPerformance { get; set; }

        public DataView LocationCombo { get; set; }


        private readonly ICommand costSheetReceivedCommand;
        public ICommand CostSheetReceivedCommand { get { return this.costSheetReceivedCommand; } }
        private void costSheetReceived()
        {

            try
            {
                _perfOption = performanceOption.CostSheetReceived;
                Refresh();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand costSheetCompletedCommand;
        public ICommand CostSheetCompletedCommand { get { return this.costSheetCompletedCommand; } }
        private void costSheetCompleted()
        {
            _perfOption = performanceOption.CostSheetCompleted;
            Refresh();
        }

        private readonly ICommand partNumbersAllottedCommand;
        public ICommand PartNumbersAllottedCommand { get { return this.partNumbersAllottedCommand; } }
        private void partNumbersAllotted()
        {
            try
            {
                _perfOption = performanceOption.PartNumberAllotted;
                Refresh();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand documentsReleasedCommand;
        public ICommand DocumentsReleasedCommand { get { return this.documentsReleasedCommand; } }
        private void documentsReleased()
        {
            try
            {
                _perfOption = performanceOption.DocumentReleased;
                Refresh();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand samplesSubmittedCommand;
        public ICommand SamplesSubmittedCommand { get { return this.samplesSubmittedCommand; } }
        private void samplesSubmitted()
        {
            try
            {
                _perfOption = performanceOption.SampleSubmitted;
                Refresh();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand refreshCommand;
        public ICommand RefreshCommand { get { return this.refreshCommand; } }
        private void Refresh()
        {
            string startDate;
            string endDate;
            string message = "";
            Int64 cnt = 0;
            try
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


                startDate = Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy");
                endDate = Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy");

                if (_perfOption == performanceOption.CostSheetReceived)
                {
                    PerformanceResult = _ddPerformanceBll.CostSheetReceived(startDate, endDate, LocCode.ToValueAsString().Trim());
                    columnDefinitionForCostSheet();
                    message = "Cost Sheets Received - ";
                    _reportTitle = "Cost Sheets Received Between " + startDate + " and " + endDate;
                    cnt = PerformanceResult.Count;
                }
                else if (_perfOption == performanceOption.CostSheetCompleted)
                {
                    PerformanceResult = _ddPerformanceBll.CostSheetCompleted(startDate, endDate, LocCode.ToValueAsString().Trim());
                    columnDefinitionForCostSheet();
                    message = "Cost Sheets Completed - ";
                    _reportTitle = "Cost Sheets Completed Between " + startDate + " and " + endDate;
                    cnt = PerformanceResult.Count;
                }
                else if (_perfOption == performanceOption.PartNumberAllotted)
                {
                    PerformanceResult = _ddPerformanceBll.PartNumberAllotted(startDate, endDate, LocCode.ToValueAsString().Trim());
                    columnDefinitionForPartNoAllotted();
                    message = "Part Numbers Allotted - ";
                    _reportTitle = "Part Numbers Allotted Between " + startDate + " and " + endDate;
                    cnt = PerformanceResult.Count;
                }
                else if (_perfOption == performanceOption.DocumentReleased)
                {
                    PerformanceResult = _ddPerformanceBll.DocumentReleased(startDate, endDate, LocCode.ToValueAsString().Trim());
                    columnDefinitionForDocumentsReleased();
                    message = "Documents Released - ";
                    _reportTitle = "Documents Released " + startDate + " and " + endDate;
                    cnt = PerformanceResult.Count;
                }
                else if (_perfOption == performanceOption.SampleSubmitted)
                {
                    PerformanceResult = _ddPerformanceBll.SampleSubmitted(startDate, endDate, LocCode.ToValueAsString().Trim());
                    columnDefinitionForSamplesSubmitted();
                    message = "Samples Submitted - ";
                    _reportTitle = "Samples Submitted " + startDate + " and " + endDate;
                    cnt = PerformanceResult.Count;
                }
                HeaderDetails = message + cnt.ToString() + (cnt > 0 ? " Entries" : " Entry") + " found ";
                NotifyPropertyChanged("HeaderDetails");
                NotifyPropertyChanged("PerformanceResult");
                NotifyPropertyChanged("HEADER1");
                NotifyPropertyChanged("VISIBLE");
                GC.Collect(2);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand performanceSummaryCommand;
        public ICommand PerformanceSummaryCommand { get { return this.performanceSummaryCommand; } }
        private void PerformanceSummary()
        {
            try
            {
                frmDDPerformanceSummary ddPerformanceSummary = new frmDDPerformanceSummary(_userInformation, _mdiChild, ApplicationTitle);
                ddPerformanceSummary.ShowDialog();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private ICommand mOPCommand;
        public ICommand MOPCommand { get { return this.mOPCommand; } }
        private void MOPGraph()
        {
            try
            {
                frmMOPgraph frMOPGraph = new frmMOPgraph(_userInformation, _mdiChild);
                frMOPGraph.ShowDialog();                
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private ICommand showECNCommand;
        public ICommand ShowECNCommand { get { return this.showECNCommand; } }
        private void ShowECN()
        {
            try
            {
                frmECNPCN ecnOrPcn = new frmECNPCN(_userInformation, _mdiChild, "ECN", StartDate, EndDate);
                ecnOrPcn.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private ICommand showPCNCommand;
        public ICommand ShowPCNCommand { get { return this.showPCNCommand; } }
        private void ShowPCN()
        {
            try
            {
                frmECNPCN ecnOrPcn = new frmECNPCN(_userInformation, _mdiChild, "PCN", StartDate, EndDate);
                ecnOrPcn.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsLocation;
        public ObservableCollection<DropdownColumns> DropDownItemsLocation
        {
            get
            {
                return _dropDownItemsLocation;
            }
            set
            {
                this._dropDownItemsLocation = value;
                NotifyPropertyChanged("DropDownItemsLocation");
            }
        }


        private ICommand showDesignersCommand;
        public ICommand ShowDesignersCommand { get { return this.showDesignersCommand; } }
        private void ShowDesigners()
        {
            try
            {
                //showDummy();
                MdiChild designers = new MdiChild();
                designers.Title = ApplicationTitle + " - Reports Designer";
                ProcessDesigner.frmflxReports flxReports = new ProcessDesigner.frmflxReports(_userInformation, designers, StartDate, EndDate);
                designers.Content = flxReports;
                designers.Height = flxReports.Height + 40;
                designers.Width = flxReports.Width + 20;
                designers.MinimizeBox = true;
                designers.MaximizeBox = true;
                designers.Resizable = false;
                if (MainMDI.IsFormAlreadyOpen("Reports Designer") == false)
                {
                    MainMDI.Container.Children.Add(designers);
                }
                else
                {
                    designers = new MdiChild();
                    designers = (MdiChild)MainMDI.GetFormAlreadyOpened("Reports Designer");
                    MainMDI.SetMDI(designers);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private ICommand printReportCommand;
        public ICommand PrintReportCommand { get { return this.printReportCommand; } }
        private void PrintReport()
        {
            try
            {
                DataTable dtData;
                string reportName = "PerformanceReport";
                dtData = PerformanceResult.ToTable().Copy();
                //dtData.Rows.Clear();
                DataSet dsReport = new DataSet();
                //dsReport.DataSetName = "DSREPORTDATA";
                dtData.TableName = "REPORTDATA";
                dsReport.Tables.Add(dtData);
                //dsReport.WriteXml("E:\\" + dsReport.DataSetName + ".xml", XmlWriteMode.WriteSchema);
                Dictionary<string, string> dictFormulas = new Dictionary<string, string>();
                dictFormulas.Add("lblColumn0", HEADER[0]);
                dictFormulas.Add("lblColumn1", HEADER[1]);
                dictFormulas.Add("lblColumn2", HEADER[2]);
                dictFormulas.Add("lblColumn3", HEADER[3]);
                dictFormulas.Add("lblColumn4", HEADER[4]);
                dictFormulas.Add("lblColumn5", HEADER[5]);
                dictFormulas.Add("lblHeader", _reportTitle);
                reportName = (_perfOption == performanceOption.DocumentReleased || _perfOption == performanceOption.SampleSubmitted ? "PerformanceReport1" : "PerformanceReport");
                frmReportViewer reportViewer = new frmReportViewer(dsReport, reportName, CrystalDecisions.Shared.ExportFormatType.NoFormat, dictFormulas);
                if (!reportViewer.ReadyToShowReport) return;
                reportViewer.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void columnDefinitionForCostSheet()
        {
            try
            {
                VISIBLE = new ObservableCollection<System.Windows.Visibility>();
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Collapsed);

                HEADER = new ObservableCollection<string>();
                HEADER.Add("CI Reference");
                HEADER.Add("Description");
                HEADER.Add("Customer Dwg");
                HEADER.Add("Customer");
                HEADER.Add("FR_CS_Date");
                HEADER.Add("");

                WIDTH = new ObservableCollection<System.Windows.Controls.DataGridLength>();
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(200));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(200));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(200));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(0));
                SetGrid();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void columnDefinitionForPartNoAllotted()
        {
            try
            {
                VISIBLE = new ObservableCollection<System.Windows.Visibility>();
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Collapsed);

                HEADER = new ObservableCollection<string>();
                HEADER.Add("Part Number");
                HEADER.Add("Description");
                HEADER.Add("PG Category");
                HEADER.Add("Customer");
                HEADER.Add("Allot Date");
                HEADER.Add("");

                WIDTH = new ObservableCollection<System.Windows.Controls.DataGridLength>();
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(250));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(250));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(0));
                SetGrid();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void columnDefinitionForDocumentsReleased()
        {
            try
            {
                VISIBLE = new ObservableCollection<System.Windows.Visibility>();
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);

                HEADER = new ObservableCollection<string>();
                HEADER.Add("Part Number");
                HEADER.Add("Description");
                HEADER.Add("PG Category");
                HEADER.Add("Customer");
                HEADER.Add("Doc Release Date");
                HEADER.Add("Designer");

                WIDTH = new ObservableCollection<System.Windows.Controls.DataGridLength>();
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(250));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(250));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                SetGrid();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void columnDefinitionForSamplesSubmitted()
        {
            try
            {
                VISIBLE = new ObservableCollection<System.Windows.Visibility>();
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);

                HEADER = new ObservableCollection<string>();
                HEADER.Add("Part Number");
                HEADER.Add("Description");
                HEADER.Add("PG Category");
                HEADER.Add("Customer");
                HEADER.Add("Sample Submitted");
                HEADER.Add("Designer");


                WIDTH = new ObservableCollection<System.Windows.Controls.DataGridLength>();
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(250));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(250));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(100));
                SetGrid();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }



        private void SetGrid()
        {
            int ictr = 0;
            foreach (System.Windows.Controls.DataGridColumn dc in DgDDPerformance.Columns)
            {
                dc.Header = HEADER[ictr];
                dc.Visibility = VISIBLE[ictr];
                dc.Width = WIDTH[ictr];
                ictr++;
            }
        }

        private readonly ICommand showProductInformationCommand;
        public ICommand ShowProductInformationCommand { get { return this.showProductInformationCommand; } }
        private void ShowProductInformation()
        {
            try
            {
                if (SelectedRow != null)
                {
                    if (_perfOption == performanceOption.PartNumberAllotted || _perfOption != performanceOption.DocumentReleased || _perfOption != performanceOption.SampleSubmitted)
                    {
                        string part_no = string.Empty;
                        if (SelectedRow.DataView.Table.Columns.Contains("COLUMN0")) part_no = SelectedRow["COLUMN0"].ToValueAsString();
                        if (!part_no.IsNotNullOrEmpty()) return;

                        ProductInformation bll = new ProductInformation(_userInformation);
                        PRD_MAST parentEntity = bll.GetEntityByPartNumber(new PRD_MAST() { PART_NO = part_no });

                        MdiChild frmProductInformationChild = new MdiChild
                        {
                            Title = ApplicationTitle + " - Product Master - " + part_no,
                            MaximizeBox = false,
                            MinimizeBox = false
                        };

                        ProcessDesigner.frmProductInformation productInformation = new ProcessDesigner.frmProductInformation(_userInformation,
                            frmProductInformationChild, parentEntity.IDPK, OperationMode.Edit);
                        frmProductInformationChild.Content = productInformation;
                        frmProductInformationChild.Height = productInformation.Height + 50;
                        frmProductInformationChild.Width = productInformation.Width + 20;
                        MainMDI.Container.Children.Add(frmProductInformationChild);

                    }
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }


        private void SetCombo()
        {
            try
            {
                LocationCombo = _ddPerformanceBll.GetLocationCombo();
                NotifyPropertyChanged("LocationCombo");
                LocCode = "All";
                Location = "All";
                NotifyPropertyChanged("LocCode");
                DropDownItemsLocation = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Code", ColumnWidth = 75 },
                            new DropdownColumns() { ColumnName = "LOCATION", ColumnDesc = "Location Description", ColumnWidth = "1*" }
                        };
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

    }

}
