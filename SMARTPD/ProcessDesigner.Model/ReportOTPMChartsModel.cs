using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class ReportOTPMChartsModel : ViewModelBase
    {

        private List<KeyValuePair<string, decimal>> _chartDataLine1;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public List<KeyValuePair<string, decimal>> ChartDataLine1
        {
            get { return _chartDataLine1; }
            set
            {
                _chartDataLine1 = value;
                NotifyPropertyChanged("ChartDataLine1");
            }
        }

        private List<KeyValuePair<string, decimal>> _chartDataLine2;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public List<KeyValuePair<string, decimal>> ChartDataLine2
        {
            get { return _chartDataLine2; }
            set
            {
                _chartDataLine2 = value;
                NotifyPropertyChanged("ChartDataLine2");
            }
        }

        private List<KeyValuePair<string, decimal>> _chartDataLine3;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public List<KeyValuePair<string, decimal>> ChartDataLine3
        {
            get { return _chartDataLine3; }
            set
            {
                _chartDataLine3 = value;
                NotifyPropertyChanged("ChartDataLine3");
            }
        }

        private string _chartDataLine1Title;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string ChartDataLine1Title
        {
            get { return _chartDataLine1Title; }
            set
            {
                _chartDataLine1Title = value;
                NotifyPropertyChanged("ChartDataLine1Title");
            }
        }

        private string _chartDataLine2Title;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string ChartDataLine2Title
        {
            get { return _chartDataLine2Title; }
            set
            {
                _chartDataLine2Title = value;
                NotifyPropertyChanged("ChartDataLine2Title");
            }
        }

        private string _chartDataLine3Title;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string ChartDataLine3Title
        {
            get { return _chartDataLine3Title; }
            set
            {
                _chartDataLine3Title = value;
                NotifyPropertyChanged("ChartDataLine3Title");
            }
        }

        private string _chartXAxisTitle;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string ChartXAxisTitle
        {
            get { return _chartXAxisTitle; }
            set
            {
                _chartXAxisTitle = value;
                NotifyPropertyChanged("ChartXAxisTitle");
            }
        }

        private string _chartYAxisTitle;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string ChartYAxisTitle
        {
            get { return _chartYAxisTitle; }
            set
            {
                _chartYAxisTitle = value;
                NotifyPropertyChanged("ChartYAxisTitle");
            }
        }

        private int _chartYAxisInterval = 20;
        //[Required(AllowEmptyints = false, ErrorMessage = "Customer is Required")]
        public int ChartYAxisInterval
        {
            get { return _chartYAxisInterval; }
            set
            {
                _chartYAxisInterval = value;
                NotifyPropertyChanged("ChartYAxisInterval");
            }
        }

        private int _chartYAxisMaxValue = 200;
        //[Required(AllowEmptyints = false, ErrorMessage = "Customer is Required")]
        public int ChartYAxisMaxValue
        {
            get { return _chartYAxisMaxValue; }
            set
            {
                _chartYAxisMaxValue = value;
                NotifyPropertyChanged("ChartYAxisMaxValue");
                ChartYAxisInterval = (value % 10 == 0 ? (int)(value / 10) : (int)((value) / 10) + 10);
            }
        }

        private DataView _gridData;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public DataView GridData
        {
            get { return _gridData; }
            set
            {
                _gridData = value;
                NotifyPropertyChanged("GridData");

                if (!GridData.IsNotNullOrEmpty() || GridData.Table.Rows.Count < 3)
                {
                    return;
                }

                List<KeyValuePair<string, decimal>> chartDataLine1 = new List<KeyValuePair<string, decimal>>();
                List<KeyValuePair<string, decimal>> chartDataLine2 = new List<KeyValuePair<string, decimal>>();
                List<KeyValuePair<string, decimal>> chartDataLine3 = new List<KeyValuePair<string, decimal>>();

                ChartYAxisMaxValue = 200;
                decimal maxValue = ChartYAxisMaxValue;

                foreach (DataColumn col in GridData.Table.Columns)
                {
                    if (col.GetHashCode() == GridData.Table.Columns[0].GetHashCode())
                    {
                        ChartXAxisTitle = col.ColumnName;
                        ChartDataLine1Title = Convert.ToString(GridData.Table.Rows[0][col.ColumnName]);
                        ChartDataLine2Title = Convert.ToString(GridData.Table.Rows[1][col.ColumnName]);
                        ChartDataLine3Title = Convert.ToString(GridData.Table.Rows[2][col.ColumnName]);
                        ChartYAxisTitle = ChartDataLine1Title;
                        if (ChartYAxisTitle.Contains("%"))
                        {
                            ChartYAxisTitle = "Percentage(%)";
                            ChartYAxisMaxValue = 100;
                        }

                        continue;
                    }

                    decimal line1Value = GridData.Table.Rows[0][col.ColumnName].ToValueAsString().ToDecimalValue();
                    decimal line2Value = GridData.Table.Rows[1][col.ColumnName].ToValueAsString().ToDecimalValue();
                    decimal line3Value = GridData.Table.Rows[2][col.ColumnName].ToValueAsString().ToDecimalValue();

                    chartDataLine1.Add(new KeyValuePair<string, decimal>(col.ColumnName, line1Value));
                    chartDataLine2.Add(new KeyValuePair<string, decimal>(col.ColumnName, line2Value));
                    chartDataLine3.Add(new KeyValuePair<string, decimal>(col.ColumnName, line3Value));

                    if (Math.Max(Math.Max(line1Value, line2Value), line3Value) > ChartYAxisMaxValue)
                    {
                        maxValue = Math.Max(Math.Max(line1Value, line2Value), line3Value);
                    }
                }

                ChartDataLine1 = chartDataLine1;
                ChartDataLine2 = chartDataLine2;
                ChartDataLine3 = chartDataLine3;
                //ChartYAxisMaxValue = (int)maxValue;
            }
        }

        private string _grid_title;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string GRID_TITLE
        {
            get { return _grid_title; }
            set
            {
                _grid_title = value.IsNotNullOrEmpty() ? value.Trim() : value;
                NotifyPropertyChanged("GRID_TITLE");
            }
        }

        private string _chart_Type;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string CHART_TYPE
        {
            get { return _chart_Type; }
            set
            {
                _chart_Type = value.IsNotNullOrEmpty() ? value.Trim() : value;
                NotifyPropertyChanged("CHART_TYPE");
            }
        }

        private Nullable<DateTime> _end_Date_Of_Working_Year;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Planned Document Release Date")]
        public Nullable<DateTime> END_DATE_OF_WORKING_YEAR
        {
            get { return _end_Date_Of_Working_Year; }
            set
            {
                _end_Date_Of_Working_Year = value;
                NotifyPropertyChanged("END_DATE_OF_WORKING_YEAR");
            }
        }

        private string _chart_type_pg;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string CHART_TYPE_PG
        {
            get { return _chart_type_pg; }
            set
            {
                _chart_type_pg = value.IsNotNullOrEmpty() ? value.Trim() : value;
                NotifyPropertyChanged("CHART_TYPE_PG");
            }
        }

        private string _chartHeader = "Developement Lead time-PG1";
        public string ChartHeader
        {
            get
            {
                return _chartHeader;
            }
            set
            {
                _chartHeader = value;
                NotifyPropertyChanged("ChartHeader");
            }
        }
    }
}
