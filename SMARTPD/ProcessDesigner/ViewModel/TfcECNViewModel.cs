using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProcessDesigner.Common;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using System.Windows.Media;

namespace ProcessDesigner
{
    public class TfcECNViewModel : ViewModelBase
    {
        private TfcECNBll tfcecnbll;
        private readonly ICommand _onAvailChkFeasibleCommand;
        public ICommand OnAvailChkFeasibleCommand { get { return this._onAvailChkFeasibleCommand; } }
        private readonly ICommand _onAvailChkNotFeasibleCommand;
        public ICommand OnAvailChkNotFeasibleCommand { get { return this._onAvailChkNotFeasibleCommand; } }
        private readonly ICommand mousecombocommand;
        public ICommand MouseComboCommand { get { return this.mousecombocommand; } }
        private readonly ICommand printClickCommand;
        public ICommand PrintClickCommand { get { return this.printClickCommand; } }
        private readonly ICommand saveClickCommand;
        public ICommand SaveClickCommand { get { return this.saveClickCommand; } }

        public TfcECNViewModel(UserInformation _userinformation)
        {
            tfcecnbll = new TfcECNBll(_userinformation);
            TFCMODEL = new TfcModel();
            this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
            this.mousecombocommand = new DelegateCommand(this.MouseClick);
            this.printClickCommand = new DelegateCommand(this.PrintCommand);
            this.saveClickCommand = new DelegateCommand(this.SaveCommand);
            this._onAvailChkFeasibleCommand = new DelegateCommand(this.AvailChkFeasibleCommand);
            this._onAvailChkNotFeasibleCommand = new DelegateCommand(this.AvailChkNotFeasibleCommand);
            LoadCmbDatas();
            SetdropDownItems();
            Clear();
        }

        private void LoadCmbDatas()
        {
            tfcecnbll.GetPartNoDetails(TFCMODEL);
        }

        private void PrintCommand()
        {
            if (!TFCMODEL.PartNo.IsNotNullOrEmpty() || !TFCMODEL.ROUTENO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No and Process No"));
                return;
            }

            else
            {
                DataTable dt;
                GridValues();
                dt = tfcecnbll.GetPrint(TFCMODEL);
                if (dt == null || dt.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {
                    frmReportViewer frmrv = new frmReportViewer(dt, "TFC-ECN");
                    frmrv.ShowDialog();
                }
            }
        }

        private void MouseClick()
        {
            if (SelectedGrid == null || !TFCMODEL.PartNo.IsNotNullOrEmpty()) return;

            if (SelectedGrid["Consideration"].ToString() == "Can the product be manufactured without incurring any unusual:")
            {
                return;
            }
            SelectedGrid.BeginEdit();
            if (SelectedGrid["Yes_No"].ToString() == "")
            {
                SelectedGrid["Yes_No"] = "X";
            }
            else if (SelectedGrid["Yes_No"].ToString() == "X")
            {
                SelectedGrid["Yes_No"] = "";
            }
            SelectedGrid.EndEdit();
        }

        public Boolean IsRecordsUpdated = false;
        private void SaveCommand()
        {
            IsRecordsUpdated = false;
            if (!TFCMODEL.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part Number"));
            }
            else if ((TFCMODEL.FEASIBLEPRODUCT || TFCMODEL.NOTFEASIBLE) == false)
            {
                ShowInformationMessage(PDMsg.NotEmpty("Conclusion"));
            }           
            else
            {
                if (TFCMODEL.FEASIBLEPRODUCT == true)
                {
                    TFCMODEL.CONCLUSION = Convert.ToString("1");
                }
                else if (TFCMODEL.NOTFEASIBLE == true)
                {
                    TFCMODEL.CONCLUSION = Convert.ToString("2");
                }
                GridValues();
                string mode = "";
                bool val = tfcecnbll.AddEditTfc(TFCMODEL, mode);
                IsRecordsUpdated = true;
                if (val == true)
                {
                    ShowInformationMessage(PDMsg.SavedSuccessfully);
                }
                else
                {
                    ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                }
               // Clear();
            }
        }

        public void GridLoad()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Consideration");
            dt.Columns.Add("Yes_No");
            dt.Rows.Add("1. Is Product adequately defined (application requirements, etc.) to enable feasibility evaluation ?", "");
            dt.Rows.Add("2. Can Engineering Performance Specifications be met as written ? ", "");
            dt.Rows.Add("3. Can product be manufactured to tolerances specified on drawing ?", "");
            dt.Rows.Add("4. Is there adequate capacity to produce product?", "");
            dt.Rows.Add("5. Does the design allow the use of efficient material handling techniques ?", "");
            dt.Rows.Add("Can the product be manufactured without incurring any unusual:", "");
            dt.Rows.Add("1. Alternative manufacturing methods ?", "");
            dt.Rows.Add("2. Cost for Capital equipment ?", "");
            dt.Rows.Add("3. Cost for tooling ?", "");
            dt.Rows.Add("4. Alternative manufacturing methods ?", "");
            dt.Rows.Add("7. Is statistical process control required on product ? ", "");
            dt.Rows.Add("8. Is statistical process control presently used on similar products ?", "");
            TFCMODEL.GRIDLOAD = dt.DefaultView;
        }

        public void GridLoadRisk()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("S_NO");
            dt.Columns.Add("Aspects");
            dt.Columns.Add("ImpactofRisk");
            dt.Columns.Add("Remarks");
            dt.Rows.Add("1", "Quality Requirements (Dimension, Metallurgy, Capability)", "", "");
            dt.Rows.Add("2", "Resources (Man, Material, Machine, Method, Measurement, Contingencies)", "", "");
            dt.Rows.Add("3", "Product Cost (Including change in money value)", "", "");
            dt.Rows.Add("4", "Manufacturing Lead Time", "", "");
            dt.Rows.Add("5", "New manufacturing process", "", "");
            dt.Rows.Add("6", "Other customer specific requirements", "", "");
            dt.Rows.Add("7", "Consumption of existing Stock, Stock building", "", "");
            TFCMODEL.GRIDLOADRISK = dt.DefaultView;
        }

        private void AvailChkFeasibleCommand()
        {
            if ((TFCMODEL.FEASIBLEPRODUCT == true && TFCMODEL.NOTFEASIBLE == true))
            {
                TFCMODEL.NOTFEASIBLE = false;
                NotifyPropertyChanged("TFCMODEL");
            }
        }

        private void AvailChkNotFeasibleCommand()
        {
            if ((TFCMODEL.FEASIBLEPRODUCT == true && TFCMODEL.NOTFEASIBLE == true))
            {
                TFCMODEL.FEASIBLEPRODUCT = false;
                NotifyPropertyChanged("TFCMODEL");
            }
        }

        private void SetdropDownItems()
        {
            try
            {
                DropDownItemsPart = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "PART NO", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "DESCRIPTION", ColumnWidth = "1*" }
                        };
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

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

        public TfcModel TFCMODEL
        {
            get
            {
                return tfcmodel;
            }
            set
            {
                tfcmodel = value;
                NotifyPropertyChanged("TFCMODEL");
            }

        }

        private readonly ICommand selectChangeComboCommandPartNo;
        public ICommand SelectChangeComboCommandPartNo { get { return this.selectChangeComboCommandPartNo; } }
        private void SelectDataRowPart()
        {
            Clear();
            if (SelectedRowPart != null)
            {
                TFCMODEL.PartNo = SelectedRowPart["PART_NO"].ToString();
                TFCMODEL.PartDesc = SelectedRowPart["PART_DESC"].ToString();
                tfcecnbll.GetOtherDetails(TFCMODEL);
                tfcecnbll.GetIssueNoDate(TFCMODEL);
                tfcecnbll.GetCustomerDwg(TFCMODEL);
                tfcecnbll.GetAll(TFCMODEL);
                if (TFCMODEL.CONCLUSION == "1")
                {
                    TFCMODEL.FEASIBLEPRODUCT = true;
                    TFCMODEL.NOTFEASIBLE = false;
                }
                else if (TFCMODEL.CONCLUSION == "2")
                {
                    TFCMODEL.FEASIBLEPRODUCT = false;
                    TFCMODEL.NOTFEASIBLE = true;
                }
                GetGrid();
                TFCMODEL.GRIDLOADRISK[0]["ImpactofRisk"] = TFCMODEL.IMPACT1;
                TFCMODEL.GRIDLOADRISK[1]["ImpactofRisk"] = TFCMODEL.IMPACT2;
                TFCMODEL.GRIDLOADRISK[2]["ImpactofRisk"] = TFCMODEL.IMPACT3;
                TFCMODEL.GRIDLOADRISK[3]["ImpactofRisk"] = TFCMODEL.IMPACT4;
                TFCMODEL.GRIDLOADRISK[4]["ImpactofRisk"] = TFCMODEL.IMPACT5;
                TFCMODEL.GRIDLOADRISK[5]["ImpactofRisk"] = TFCMODEL.IMPACT6;
                TFCMODEL.GRIDLOADRISK[6]["ImpactofRisk"] = TFCMODEL.IMPACT7;
                TFCMODEL.GRIDLOADRISK[0]["Remarks"] = TFCMODEL.REMARKS1;
                TFCMODEL.GRIDLOADRISK[1]["Remarks"] = TFCMODEL.REMARKS2;
                TFCMODEL.GRIDLOADRISK[2]["Remarks"] = TFCMODEL.REMARKS3;
                TFCMODEL.GRIDLOADRISK[3]["Remarks"] = TFCMODEL.REMARKS4;
                TFCMODEL.GRIDLOADRISK[4]["Remarks"] = TFCMODEL.REMARKS5;
                TFCMODEL.GRIDLOADRISK[5]["Remarks"] = TFCMODEL.REMARKS6;
                TFCMODEL.GRIDLOADRISK[6]["Remarks"] = TFCMODEL.REMARKS7;
                NotifyPropertyChanged("TFCMODEL");
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

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        private void Clear()
        {
            TFCMODEL.PartNo = "";
            TFCMODEL.CUSTOMERNAME = "";
            TFCMODEL.PartDesc = "";
            TFCMODEL.CUSTOMERPARTNO = "";
            TFCMODEL.PRD_ISSUE_NO = "";
            TFCMODEL.CUST_DWG_ISSUE_NO = "";
            TFCMODEL.PRD_ISSUE_DATE = "";
            TFCMODEL.CUST_STD_DATE = "";
            TFCMODEL.FEASIBLEPRODUCT = false;
            TFCMODEL.NOTFEASIBLE = false;
            TFCMODEL.ROUTENO = "";
            IsRecordsUpdated = false;
            GridLoad();
            GridLoadRisk();
        }

        public bool GridValues()
        {
            string[] arr = new string[20];
            for (int i = 0; i <= 12; i++)
            {
                if (i < 5)
                {
                    if (TFCMODEL.GRIDLOAD[i]["Yes_No"].ToString() == "X")
                    {
                        arr[i] = "N";
                    }
                    else
                    {
                        arr[i] = "Y";
                    }
                }
                else if (i > 5 && i < 12)
                {
                    if (TFCMODEL.GRIDLOAD[i]["Yes_No"].ToString() == "X")
                    {
                        arr[i] = "N";
                    }
                    else
                    {
                        arr[i] = "Y";
                    }
                }
                else
                {
                    arr[i] = "";
                }
            }
            TFCMODEL.Q1 = arr[0];
            TFCMODEL.Q2 = arr[1];
            TFCMODEL.Q3 = arr[2];
            TFCMODEL.Q4 = arr[3];
            TFCMODEL.Q5 = arr[4];
            TFCMODEL.Q6 = arr[6];

            TFCMODEL.Q7 = arr[7];
            TFCMODEL.Q8 = arr[8];
            TFCMODEL.Q9 = arr[9];
            TFCMODEL.Q10 = arr[10];
            TFCMODEL.Q11 = arr[11];
            string[] arr1 = new string[20];
            for (int i = 0; i <= 6; i++)
            {
                arr1[i] = TFCMODEL.GRIDLOADRISK[i]["ImpactofRisk"].ToString();
            }
            TFCMODEL.IMPACT1 = arr1[0];
            TFCMODEL.IMPACT2 = arr1[1];
            TFCMODEL.IMPACT3 = arr1[2];
            TFCMODEL.IMPACT4 = arr1[3];
            TFCMODEL.IMPACT5 = arr1[4];
            TFCMODEL.IMPACT6 = arr1[5];
            TFCMODEL.IMPACT7 = arr1[6];
            string[] arr2 = new string[20];
            for (int i = 0; i <= 6; i++)
            {
                arr2[i] = TFCMODEL.GRIDLOADRISK[i]["Remarks"].ToString();
            }
            TFCMODEL.REMARKS1 = arr2[0];
            TFCMODEL.REMARKS2 = arr2[1];
            TFCMODEL.REMARKS3 = arr2[2];
            TFCMODEL.REMARKS4 = arr2[3];
            TFCMODEL.REMARKS5 = arr2[4];
            TFCMODEL.REMARKS6 = arr2[5];
            TFCMODEL.REMARKS7 = arr2[6];
            return true;
        }

        private DataRowView _selectedgrid;
        public DataRowView SelectedGrid
        {
            get
            {
                return _selectedgrid;
            }

            set
            {
                _selectedgrid = value;
            }
        }

        private void GetGrid()
        {
            if (TFCMODEL.Q1 == "N")
            {
                TFCMODEL.GRIDLOAD[0]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD[0]["Yes_No"] = "";
            }
            if (TFCMODEL.Q2 == "N")
            {
                TFCMODEL.GRIDLOAD[1]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD[1]["Yes_No"] = "";
            }
            if (TFCMODEL.Q3 == "N")
            {
                TFCMODEL.GRIDLOAD[2]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD[2]["Yes_No"] = "";
            }
            if (TFCMODEL.Q4 == "N")
            {
                TFCMODEL.GRIDLOAD[3]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD[3]["Yes_No"] = "";
            }
            if (TFCMODEL.Q5 == "N")
            {
                TFCMODEL.GRIDLOAD[4]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD[4]["Yes_No"] = "";
            }
            if (TFCMODEL.Q6 == "N")
            {
                TFCMODEL.GRIDLOAD[6]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD[6]["Yes_No"] = "";
            }
            if (TFCMODEL.Q7 == "N")
            {
                TFCMODEL.GRIDLOAD[7]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD[7]["Yes_No"] = "";
            }
            if (TFCMODEL.Q8 == "N")
            {
                TFCMODEL.GRIDLOAD[8]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD[8]["Yes_No"] = "";
            }
            if (TFCMODEL.Q9 == "N")
            {
                TFCMODEL.GRIDLOAD[9]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD[9]["Yes_No"] = "";
            }
            if (TFCMODEL.Q10 == "N")
            {
                TFCMODEL.GRIDLOAD[10]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD[10]["Yes_No"] = "";
            }
            if (TFCMODEL.Q11 == "N")
            {
                TFCMODEL.GRIDLOAD[11]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD[11]["Yes_No"] = "";
            }
        }

        public object CloseAction { get; set; }

        public TfcModel tfcmodel { get; set; }

        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                WPF.MDI.ClosingEventArgs closingev;
                closingev = (WPF.MDI.ClosingEventArgs)e;

                if (TFCMODEL.PartNo.IsNotNullOrEmpty() && !IsRecordsUpdated)
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        SaveCommand();
                        if (!IsRecordsUpdated)
                        {
                            closingev.Cancel = true;
                            e = closingev;
                            return;
                        }
                    }
                }

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


        private MessageBoxResult ShowWarningMessage(string _showMessage, MessageBoxButton messageBoxButton)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, messageBoxButton, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }
        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        public void dgConsideration_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            try
            {
                DataRowView rowView = e.Row.Item as DataRowView;
                if (rowView["Consideration"].ToString() == "Can the product be manufactured without incurring any unusual:")
                {
                    e.Row.Background = new SolidColorBrush(Colors.LightGray);
                    e.Row.Height = 27;
                }               
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void dgConsideration_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGrid dgConsideration = (DataGrid)sender;
                DataView dvTable = (System.Data.DataView)(dgConsideration.ItemsSource);
                DataTable dtTable = dvTable.ToTable();
                for (int i = 0; i < dtTable.Rows.Count; i++)
                {
                    DataGridRow row = (DataGridRow)dgConsideration.ItemContainerGenerator.ContainerFromIndex(i);
                    if (dtTable.Rows[i]["Consideration"].ToString() == "Can the product be manufactured without incurring any unusual:")
                    {
                        if (row != null)
                        {
                            row.Background = Brushes.LightGray;
                            row.Height = 27;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
    }
}
