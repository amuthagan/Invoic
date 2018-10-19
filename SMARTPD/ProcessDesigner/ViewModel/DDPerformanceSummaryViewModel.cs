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
using ProcessDesigner.UserControls;


namespace ProcessDesigner.ViewModel
{
    class DDPerformanceSummaryViewModel : ViewModelBase
    {
        enum performanceOption { PartNosAllotted, DocumentsReleased, SamplesSubmitted, AwaitingPartNoAllocation, Dummy };

        WPF.MDI.MdiChild _mdiChild;
        UserInformation _userInformation;
        DDPerformanceBll _ddPerformanceBll;
        performanceOption _performanceOption;
        public DDPerformanceSummaryViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, System.Windows.Controls.DataGrid dgDDPerformance)
        {
            try
            {
                _userInformation = userInformation;
                _mdiChild = mdiChild;
                _ddPerformanceBll = new DDPerformanceBll(_userInformation);
                DDPerformanceSummaryModelReport = new DDPerformanceSummaryModel();
                this.awaitingPartNoAllocationCommand = new DelegateCommand(this.AwaitingPartNoAllocation);
                this.samplesSubmittedCommand = new DelegateCommand(this.SamplesSubmitted);
                this.documentsReleasedCommand = new DelegateCommand(this.DocumentsReleased);
                this.partNosAllottedCommand = new DelegateCommand(this.PartNosAllotted);
                DgDDPerformance = dgDDPerformance;
                _performanceOption = performanceOption.Dummy;
                GetDummyData();
                GetDDPerformanceSummary();
                HeaderDetails = "Result Lists";
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public System.Windows.Controls.DataGrid DgDDPerformance { get; set; }

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

        public System.Windows.Controls.DataGrid DgDDPerformanceSummary { get; set; }


        private DDPerformanceSummaryModel _dDPerformanceSummaryModelReport;

        public DDPerformanceSummaryModel DDPerformanceSummaryModelReport
        {
            get
            {
                return _dDPerformanceSummaryModelReport;
            }
            set
            {
                _dDPerformanceSummaryModelReport = value;
                NotifyPropertyChanged("DDPerformanceSummaryModelReport");
            }
        }

        private readonly ICommand partNosAllottedCommand;
        public ICommand PartNosAllottedCommand { get { return this.partNosAllottedCommand; } }
        private void PartNosAllotted()
        {
            try
            {
                //_perfOption = performanceOption.CostSheetReceived;
                PerformanceResult = _ddPerformanceBll.GetPartNosAllottedSummary();
                HeaderDetails = "Part Nos. Allotted - " + PerformanceResult.Count.ToValueAsString() + (PerformanceResult.Count > 1 ? " Entries" : " Entry") + " Found";
                columnDefinition();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand documentsReleasedCommand;
        public ICommand DocumentsReleasedCommand { get { return this.documentsReleasedCommand; } }
        private void DocumentsReleased()
        {
            try
            {
                //_perfOption = performanceOption.CostSheetReceived;
                PerformanceResult = _ddPerformanceBll.GetDocumentsReleasedSummary();
                HeaderDetails = "Document Released - " + PerformanceResult.Count.ToValueAsString() + (PerformanceResult.Count > 1 ? " Entries" : " Entry") + " Found";
                columnDefinition();
                //NotifyPropertyChanged("PerformanceResult");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand samplesSubmittedCommand;
        public ICommand SamplesSubmittedCommand { get { return this.samplesSubmittedCommand; } }
        private void SamplesSubmitted()
        {
            try
            {
                //_perfOption = performanceOption.CostSheetReceived;
                PerformanceResult = _ddPerformanceBll.GetSamplesSubmittedSummary();
                HeaderDetails = "Samples Submitted - " + PerformanceResult.Count.ToValueAsString() + (PerformanceResult.Count > 1 ? " Entries" : " Entry") + " Found";
                columnDefinition();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand awaitingPartNoAllocationCommand;
        public ICommand AwaitingPartNoAllocationCommand { get { return this.awaitingPartNoAllocationCommand; } }
        private void AwaitingPartNoAllocation()
        {
            try
            {
                //_perfOption = performanceOption.CostSheetReceived;
                PerformanceResult = _ddPerformanceBll.GetAwaitingPartNoAllocationSummary();
                HeaderDetails = "Cost Sheets awaiting Part No Allocation - " + PerformanceResult.Count.ToValueAsString() + (PerformanceResult.Count > 1 ? " Entries" : " Entry") + " Found";
                columnDefinition();
                NotifyPropertyChanged("PerformanceResult");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void GetDummyData()
        {
            try
            {
                //_perfOption = performanceOption.CostSheetReceived;
                PerformanceResult = _ddPerformanceBll.GetDummyData();
                HeaderDetails = "Result Lists";
                columnDefinition();
                NotifyPropertyChanged("PerformanceResult");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void GetDDPerformanceSummary()
        {
            try
            {
                _ddPerformanceBll.GetDDPerformanceSummary(DDPerformanceSummaryModelReport);
                NotifyPropertyChanged("DDPerformanceSummaryModelReport");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void columnDefinition()
        {
            try
            {
                VISIBLE = new ObservableCollection<System.Windows.Visibility>();
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);
                VISIBLE.Add(System.Windows.Visibility.Visible);

                WIDTH = new ObservableCollection<System.Windows.Controls.DataGridLength>();
                WIDTH.Add(new System.Windows.Controls.DataGridLength(20, System.Windows.Controls.DataGridLengthUnitType.Star));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(60, System.Windows.Controls.DataGridLengthUnitType.Star));
                WIDTH.Add(new System.Windows.Controls.DataGridLength(20, System.Windows.Controls.DataGridLengthUnitType.Star));

                HEADER = new ObservableCollection<string>();

                if (_performanceOption == performanceOption.PartNosAllotted || _performanceOption == performanceOption.DocumentsReleased ||
                    _performanceOption == performanceOption.SamplesSubmitted || _performanceOption == performanceOption.Dummy)
                {
                    HEADER.Add("Part No");
                    HEADER.Add("Description");
                    HEADER.Add("Date");
                }
                else if (_performanceOption == performanceOption.AwaitingPartNoAllocation)
                {
                    HEADER.Add("CI Reference");
                    HEADER.Add("Description");
                    HEADER.Add("Requested On");
                }
                else
                {
                    HEADER.Add("Part No");
                    HEADER.Add("Description");
                    HEADER.Add("Date");
                }
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
            System.Windows.Controls.DataGridColumn dc;
            for (ictr = 0; ictr < 3; ictr++)
            {
                dc = DgDDPerformance.Columns[ictr];
                dc.Header = HEADER[ictr];
                dc.Visibility = VISIBLE[ictr];
                dc.Width = WIDTH[ictr];
                //ictr++;
            }
        }


    }
}
