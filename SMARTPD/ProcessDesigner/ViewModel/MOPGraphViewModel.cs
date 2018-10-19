using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using System.Data;
using System.Windows;
using ProcessDesigner.Common;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using System.Drawing;
using System.IO;
using System.Windows.Controls;


namespace ProcessDesigner.ViewModel
{
    class MOPGraphViewModel : ViewModelBase
    {
        UserInformation _userInformation;
        WPF.MDI.MdiChild _mdiChild;
        MOPGraphBll _mopGraphBll;
        public MOPGraphViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild)
        {
            try
            {
                _userInformation = userInformation;
                _mdiChild = mdiChild;
                _mopGraphBll = new MOPGraphBll(_userInformation);
                this.refreshGraphCommand = new DelegateCommand(this.RefreshGraph);
                LoadCombo();
                EndDate = DateTime.Now;
                if (DateTime.Now.Month < 4)
                {
                    StartDate = Convert.ToDateTime("01/04/" + (DateTime.Now.Year - 1));
                }
                else
                {
                    StartDate = Convert.ToDateTime("01/04/" + (DateTime.Now.Year));
                }
                GraphType = "FRC";
                RefreshGraph();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

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

        private string _graphType;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Graph type is required")]
        public string GraphType
        {
            get
            {
                return _graphType;
            }
            set
            {
                _graphType = value;
                NotifyPropertyChanged("GraphType");
            }
        }

        private DataView _graphTypeCombo;
        public DataView GraphTypeCombo
        {
            get
            {
                return _graphTypeCombo;
            }
            set
            {
                _graphTypeCombo = value;
                NotifyPropertyChanged("GraphTypeCombo");
            }
        }

        private List<KeyValuePair<string, int>> _graphData;
        public List<KeyValuePair<string, int>> GraphData
        {
            get
            {
                return _graphData;
            }
            set
            {
                _graphData = value;
                NotifyPropertyChanged("GraphData");
            }
        }

        private List<KeyValuePair<string, int>> _graphData1;
        public List<KeyValuePair<string, int>> GraphData1
        {
            get
            {
                return _graphData1;
            }
            set
            {
                _graphData1 = value;
                NotifyPropertyChanged("GraphData1");
            }
        }

        private List<KeyValuePair<string, int>> _graphData2;
        public List<KeyValuePair<string, int>> GraphData2
        {
            get
            {
                return _graphData2;
            }
            set
            {
                _graphData2 = value;
                NotifyPropertyChanged("GraphData2");
            }
        }

        private List<KeyValuePair<string, int>> _graphData3;
        public List<KeyValuePair<string, int>> GraphData3
        {
            get
            {
                return _graphData3;
            }
            set
            {
                _graphData3 = value;
                NotifyPropertyChanged("GraphData3");
            }
        }

        private string _graphTitle;
        public string GraphTitle
        {
            get
            {
                return _graphTitle;
            }
            set
            {
                _graphTitle = value;
                NotifyPropertyChanged("GraphTitle");
            }
        }

        private string _yAxisTitle;
        public string YAxisTitle
        {
            get
            {
                return _yAxisTitle;
            }
            set
            {
                _yAxisTitle = value;
                NotifyPropertyChanged("YAxisTitle");
            }
        }

        private string _xAxisTitle;
        public string XAxisTitle
        {
            get
            {
                return _xAxisTitle;
            }
            set
            {
                _xAxisTitle = value;
                NotifyPropertyChanged("XAxisTitle");
            }
        }



        private string _yAxisTitle1;
        public string YAxisTitle1
        {
            get
            {
                return _yAxisTitle1;
            }
            set
            {
                _yAxisTitle1 = value;
                NotifyPropertyChanged("YAxisTitle1");
            }
        }

        private string _yAxisTitle2;
        public string YAxisTitle2
        {
            get
            {
                return _yAxisTitle2;
            }
            set
            {
                _yAxisTitle2 = value;
                NotifyPropertyChanged("YAxisTitle2");
            }
        }

        private string _yAxisTitle3;
        public string YAxisTitle3
        {
            get
            {
                return _yAxisTitle3;
            }
            set
            {
                _yAxisTitle3 = value;
                NotifyPropertyChanged("YAxisTitle3");
            }
        }

        private string _legendTitle;
        public string LegendTitle
        {
            get
            {
                return _legendTitle;
            }
            set
            {
                _legendTitle = value;
                NotifyPropertyChanged("LegendTitle");
            }
        }


        private string _legendTitle1;
        public string LegendTitle1
        {
            get
            {
                return _legendTitle1;
            }
            set
            {
                _legendTitle1 = value;
                NotifyPropertyChanged("LegendTitle1");
            }
        }

        private string _legendTitle2;
        public string LegendTitle2
        {
            get
            {
                return _legendTitle2;
            }
            set
            {
                _legendTitle2 = value;
                NotifyPropertyChanged("LegendTitle2");
            }
        }

        private string _legendTitle3;
        public string LegendTitle3
        {
            get
            {
                return _legendTitle3;
            }
            set
            {
                _legendTitle3 = value;
                NotifyPropertyChanged("LegendTitle3");
            }
        }


        private Visibility _singleLegend;
        public Visibility SingleLegend
        {
            get
            {
                return _singleLegend;
            }
            set
            {
                _singleLegend = value;
                NotifyPropertyChanged("SingleLegend");
            }
        }

        private Visibility _doubleLegend;
        public Visibility DoubleLegend
        {
            get
            {
                return _doubleLegend;
            }
            set
            {
                _doubleLegend = value;
                NotifyPropertyChanged("DoubleLegend");
            }
        }

        private Visibility _tripleLegend;
        public Visibility TripleLegend
        {
            get
            {
                return _tripleLegend;
            }
            set
            {
                _tripleLegend = value;
                NotifyPropertyChanged("TripleLegend");
            }
        }

        private void LoadCombo()
        {
            DataTable dtCombo = new DataTable();
            dtCombo.Columns.Add("Code");
            dtCombo.Columns.Add("Description");
            try
            {
                InsertCombo(dtCombo, "FRC", "Feasibility Report Count");
                InsertCombo(dtCombo, "DPA", "Development Plan Adherence");
                InsertCombo(dtCombo, "LTA", "Lead Time Adherence");
                InsertCombo(dtCombo, "DE", "Design Effectiveness");
                InsertCombo(dtCombo, "CTS", "Conformance to Specification");
                InsertCombo(dtCombo, "CE", "Cost Effectiveness");
                InsertCombo(dtCombo, "RAC", "Request vs Agreed & Completed");
                InsertCombo(dtCombo, "PGCDR", "Product Group Category For Document Released");
                InsertCombo(dtCombo, "PGCSSP", "Product Group Category For Sample Submitted Products");
                GraphTypeCombo = dtCombo.DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void InsertCombo(DataTable dtCombo, string code, string description)
        {
            DataRow drRow;
            try
            {
                drRow = dtCombo.NewRow();
                drRow["Description"] = description;
                drRow["Code"] = code;
                dtCombo.Rows.Add(drRow);
                dtCombo.AcceptChanges();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private readonly ICommand refreshGraphCommand;
        public ICommand RefreshGraphCommand
        {
            get { return this.refreshGraphCommand; }
        }
        private void RefreshGraph()
        {
            try
            {
                if (GraphType.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Graph Type"));
                    return;
                }

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

                if (GraphData != null)
                {
                    GraphData.Clear();
                }
                if (GraphData1 != null)
                {
                    GraphData1.Clear();
                    NotifyPropertyChanged("GraphData1");
                }
                if (GraphData2 != null)
                {
                    GraphData2.Clear();
                    NotifyPropertyChanged("GraphData2");
                }

                if (GraphData3 != null)
                {
                    GraphData3.Clear();
                    NotifyPropertyChanged("GraphData3");
                }
                if (GraphType == "FRC")
                {
                    GraphData = _mopGraphBll.FeasibilityReportCount(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"));
                    GraphTitle = "Feasibility Reports Completed";
                    YAxisTitle = "No. of Costsheets Completed";
                    XAxisTitle = "Months";
                    SingleLegend = Visibility.Visible;
                    DoubleLegend = Visibility.Collapsed;
                    TripleLegend = Visibility.Collapsed;
                }
                else if (GraphType == "DPA")
                {
                    GraphData = _mopGraphBll.DevelopmentPlanAdherence(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"));
                    GraphTitle = "Documents Completed";
                    YAxisTitle = "No. of Documents Released";
                    XAxisTitle = "Months";
                    SingleLegend = Visibility.Visible;
                    DoubleLegend = Visibility.Collapsed;
                    TripleLegend = Visibility.Collapsed;
                }
                else if (GraphType == "LTA")
                {
                    GraphData = _mopGraphBll.LeadTimeAdherence(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"), "0");
                    GraphData1 = _mopGraphBll.LeadTimeAdherence(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"), "1");
                    GraphTitle = "Design Lead Time";
                    YAxisTitle = "Lead Time";
                    XAxisTitle = "Month";
                    SingleLegend = Visibility.Collapsed;
                    DoubleLegend = Visibility.Visible;
                    TripleLegend = Visibility.Collapsed;
                }
                else if (GraphType == "DE")
                {
                    GraphData = _mopGraphBll.DesignEffectiveness(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"));
                    GraphTitle = "Feasibility Reports Completed";
                    YAxisTitle = "No. of Costsheets Completed";
                    XAxisTitle = "Month";
                    SingleLegend = Visibility.Visible;
                    DoubleLegend = Visibility.Collapsed;
                    TripleLegend = Visibility.Collapsed;
                }
                else if (GraphType == "CTS")
                {
                    GraphData = _mopGraphBll.ConformanceToSpecification(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"));
                    GraphTitle = "Feasibility Reports Completed";
                    YAxisTitle = "No. of Costsheets Completed";
                    XAxisTitle = "Month";
                    SingleLegend = Visibility.Visible;
                    DoubleLegend = Visibility.Collapsed;
                    TripleLegend = Visibility.Collapsed;
                }
                else if (GraphType == "CE")
                {
                    GraphData = _mopGraphBll.CostEffectiveness(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"));
                    GraphTitle = "Feasibility Reports Completed";
                    YAxisTitle = "No. of Costsheets Completed";
                    XAxisTitle = "Month";
                    SingleLegend = Visibility.Visible;
                    DoubleLegend = Visibility.Collapsed;
                    TripleLegend = Visibility.Collapsed;
                }
                else if (GraphType == "RAC")
                {
                    GraphData = _mopGraphBll.RequestVsAgreedAndCompleted(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"));
                    GraphTitle = "Feasibility Reports Completed";
                    YAxisTitle = "No. of Costsheets Completed";
                    XAxisTitle = "Month";
                    SingleLegend = Visibility.Visible;
                    DoubleLegend = Visibility.Collapsed;
                    TripleLegend = Visibility.Collapsed;
                }
                else if (GraphType == "PGCDR")
                {
                    GraphData = _mopGraphBll.PGCForDocumentReleased(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"), "1");
                    GraphData1 = _mopGraphBll.PGCForDocumentReleased(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"), "2");
                    GraphData2 = _mopGraphBll.PGCForDocumentReleased(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"), "3");
                    GraphData3 = _mopGraphBll.PGCForDocumentReleased(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"), "TOTAL");
                    GraphTitle = "Product Group Category For Document Released";
                    XAxisTitle = "Month";
                    YAxisTitle = "Number of Products";
                    LegendTitle = "PG 1";
                    LegendTitle1 = "PG 2";
                    LegendTitle2 = "PG 3";
                    LegendTitle3 = "Total";
                    SingleLegend = Visibility.Collapsed;
                    DoubleLegend = Visibility.Collapsed;
                    TripleLegend = Visibility.Visible;
                }
                else if (GraphType == "PGCSSP")
                {
                    GraphData = _mopGraphBll.PGCForSampleSubmittedProducts(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"), "1");
                    GraphData1 = _mopGraphBll.PGCForSampleSubmittedProducts(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"), "2");
                    GraphData2 = _mopGraphBll.PGCForSampleSubmittedProducts(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"), "3");
                    GraphData3 = _mopGraphBll.PGCForSampleSubmittedProducts(Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy"), Convert.ToDateTime(EndDate).ToString("dd/MM/yyyy"), "TOTAL");
                    GraphTitle = "Product Group Category For Sample Submitted Products";
                    XAxisTitle = "Month";

                    LegendTitle = "PG 1";
                    LegendTitle1 = "PG 2";
                    LegendTitle2 = "PG 3";
                    LegendTitle3 = "Total";
                    SingleLegend = Visibility.Collapsed;
                    DoubleLegend = Visibility.Collapsed;
                    TripleLegend = Visibility.Visible;
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



    }
}
