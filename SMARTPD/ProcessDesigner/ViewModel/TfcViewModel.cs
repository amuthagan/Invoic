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

namespace ProcessDesigner.ViewModel
{
    public class TfcViewModel : ViewModelBase
    {
        private LogViewBll _logviewBll;
        private readonly ICommand _onAvailChkYesCommand;
        public ICommand OnAvailChkYesCommand { get { return this._onAvailChkYesCommand; } }

        private readonly ICommand _onAvailChkNoCommand;
        public ICommand OnAvailChkNoCommand { get { return this._onAvailChkNoCommand; } }

        private readonly ICommand _onAvailChkPtCommand;
        public ICommand OnAvailChkPtCommand { get { return this._onAvailChkPtCommand; } }

        private readonly ICommand _onAvailChkPlCommand;
        public ICommand OnAvailChkPlCommand { get { return this._onAvailChkPlCommand; } }

        private readonly ICommand _onAvailChkPrCommand;
        public ICommand OnAvailChkPrCommand { get { return this._onAvailChkPrCommand; } }

        private readonly ICommand _onAvailChkF1Command;
        public ICommand OnAvailChkF1Command { get { return this._onAvailChkF1Command; } }

        private readonly ICommand _onAvailChkF2Command;
        public ICommand OnAvailChkF2Command { get { return this._onAvailChkF2Command; } }

        private readonly ICommand _onAvailChkNfCommand;
        public ICommand OnAvailChkNfCommand { get { return this._onAvailChkNfCommand; } }
        private TfcBll tfcbll;
        public TfcViewModel(UserInformation _userinformation)
        {
            tfcbll = new TfcBll(_userinformation);
            this._logviewBll = new LogViewBll(_userinformation);

            TFCMODEL = new TfcModel();

            this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
            this.mousecombocommand = new DelegateCommand(this.MouseClick);
            this.printClickCommand = new DelegateCommand(this.PrintCommand);
            this.saveClickCommand = new DelegateCommand(this.SaveCommand);
            this._onAvailChkYesCommand = new DelegateCommand(this.AvailChkYesCommand);
            this._onAvailChkNoCommand = new DelegateCommand(this.AvailChkNoCommand);
            this._onAvailChkPtCommand = new DelegateCommand(this.AvailChkPtCommand);
            this._onAvailChkPlCommand = new DelegateCommand(this.AvailChkPlCommand);
            this._onAvailChkPrCommand = new DelegateCommand(this.AvailChkPrCommand);
            this._onAvailChkF1Command = new DelegateCommand(this.AvailChkF1Command);
            this._onAvailChkF2Command = new DelegateCommand(this.AvailChkF2Command);
            this._onAvailChkNfCommand = new DelegateCommand(this.AvailChkNfCommand);
            LoadCmbDatas();
            SetdropDownItems();
            Clear();
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

        private void AvailChkYesCommand()
        {
            if (TFCMODEL.AUTOPARTYES == true && TFCMODEL.AUTOPARTNO == true)
            {
                TFCMODEL.AUTOPARTNO = false;
                NotifyPropertyChanged("TFCMODEL");
            }
        }

        private void AvailChkNoCommand()
        {
            if (TFCMODEL.AUTOPARTYES == true && TFCMODEL.AUTOPARTNO == true)
            {
                TFCMODEL.AUTOPARTYES = false;
                NotifyPropertyChanged("TFCMODEL");
            }
        }

        private void AvailChkPtCommand()
        {
            if ((TFCMODEL.PROTOTYPE == true && TFCMODEL.PRODUCTION == true) || (TFCMODEL.PRELAUNCH == true && TFCMODEL.PROTOTYPE == true))
            {
                TFCMODEL.PRODUCTION = false;
                TFCMODEL.PRELAUNCH = false;
                NotifyPropertyChanged("TFCMODEL");
            }
        }

        private void AvailChkPlCommand()
        {
            if ((TFCMODEL.PRELAUNCH == true && TFCMODEL.PRODUCTION == true) || (TFCMODEL.PRELAUNCH == true && TFCMODEL.PROTOTYPE == true))
            {
                TFCMODEL.PRODUCTION = false;
                TFCMODEL.PROTOTYPE = false;
                NotifyPropertyChanged("TFCMODEL");
            }
        }

        private void AvailChkPrCommand()
        {
            if ((TFCMODEL.PRELAUNCH == true && TFCMODEL.PRODUCTION == true) || (TFCMODEL.PRODUCTION == true && TFCMODEL.PROTOTYPE == true))
            {
                TFCMODEL.PROTOTYPE = false;
                TFCMODEL.PRELAUNCH = false;
                NotifyPropertyChanged("TFCMODEL");
            }
        }

        private void AvailChkF1Command()
        {
            if ((TFCMODEL.FEASIBLEPRODUCT == true && TFCMODEL.NOTFEASIBLE == true) || (TFCMODEL.FEASIBLEPRODUCT == true && TFCMODEL.FEASIBLECHANGE == true))
            {
                TFCMODEL.FEASIBLECHANGE = false;
                TFCMODEL.NOTFEASIBLE = false;
                NotifyPropertyChanged("TFCMODEL");
            }
        }

        private void AvailChkF2Command()
        {
            if ((TFCMODEL.FEASIBLECHANGE == true && TFCMODEL.NOTFEASIBLE == true) || (TFCMODEL.FEASIBLECHANGE == true && TFCMODEL.FEASIBLEPRODUCT == true))
            {
                TFCMODEL.FEASIBLEPRODUCT = false;
                TFCMODEL.NOTFEASIBLE = false;
                NotifyPropertyChanged("TFCMODEL");
            }
        }

        private void AvailChkNfCommand()
        {
            if ((TFCMODEL.FEASIBLEPRODUCT == true && TFCMODEL.NOTFEASIBLE == true) || (TFCMODEL.NOTFEASIBLE == true && TFCMODEL.FEASIBLECHANGE == true))
            {
                TFCMODEL.FEASIBLECHANGE = false;
                TFCMODEL.FEASIBLEPRODUCT = false;
                NotifyPropertyChanged("TFCMODEL");
            }
        }

        public void GridLoad()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Consideration");
            dt.Columns.Add("Yes_No");
            dt.Columns.Add("Visible");
            dt.Rows.Add("1. Is Product adequately defined (application requirements, etc.) to enable feasibility evaluation ?", "", "True");
            dt.Rows.Add("2. Can Engineering Performance Specifications be met as written ? ", "", "True");
            dt.Rows.Add("3. Can product be manufactured to tolerances specified on drawing ?", "", "True");
            dt.Rows.Add("4. Can product be manufactured with Cpk's that meet requirements ?", "", "True");
            dt.Rows.Add("5. Is there adequate capacity to produce product?", "", "True");
            dt.Rows.Add("6. Does the design allow the use of efficient material handling techniques ?", "", "True");
            dt.Rows.Add("Can the product be manufactured without incurring any unusual:", "", "True");
            dt.Rows.Add("1. Cost for Capital equipment ?", "", "True");
            dt.Rows.Add("2. Cost for tooling ?", "", "True");
            dt.Rows.Add("3. Alternative manufacturing methods ?", "", "True");
            dt.Rows.Add("7. Is statistical process control required on product ? ", "", "True");
            dt.Rows.Add("8. Is statistical process control presently used on similar products ?", "", "True");
            dt.Rows.Add("Where statistical process control is used on similar products :", "", "True");
            dt.Rows.Add("1. Are the processes in control and stable ?", "", "True");
            dt.Rows.Add("2. Are Cpk's greater than 1.33 ?", "", "True");
            TFCMODEL.GRIDLOAD = dt.DefaultView;
            TFCMODEL.GRIDLOAD.RowFilter = "Visible = 'True'";

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

        private readonly ICommand selectChangeComboCommandPartNo;
        public ICommand SelectChangeComboCommandPartNo { get { return this.selectChangeComboCommandPartNo; } }
        private void SelectDataRowPart()
        {
            Clear();
            if (SelectedRowPart != null)
            {
                TFCMODEL.PartNo = SelectedRowPart["PART_NO"].ToString();
                TFCMODEL.PartDesc = SelectedRowPart["PART_DESC"].ToString();
                tfcbll.GetOtherDetails(TFCMODEL);
                tfcbll.GetDateApplication(TFCMODEL);
                tfcbll.GetAll(TFCMODEL);
                if (TFCMODEL.AUTO_PART == "Y")
                {
                    TFCMODEL.AUTOPARTYES = true;
                    TFCMODEL.AUTOPARTNO = false;
                }
                else if (TFCMODEL.AUTO_PART == "N")
                {
                    TFCMODEL.AUTOPARTYES = false;
                    TFCMODEL.AUTOPARTNO = true;
                }

                if (TFCMODEL.CUST_PROG == "PT")
                {
                    TFCMODEL.PROTOTYPE = true;
                    TFCMODEL.PRELAUNCH = false;
                    TFCMODEL.PRODUCTION = false;
                }
                else if (TFCMODEL.CUST_PROG == "PL")
                {
                    TFCMODEL.PROTOTYPE = false;
                    TFCMODEL.PRELAUNCH = true;
                    TFCMODEL.PRODUCTION = false;
                }
                else if (TFCMODEL.CUST_PROG == "PR")
                {
                    TFCMODEL.PROTOTYPE = false;
                    TFCMODEL.PRELAUNCH = false;
                    TFCMODEL.PRODUCTION = true;
                }


                if (TFCMODEL.CONCLUSION == "1")
                {
                    TFCMODEL.FEASIBLEPRODUCT = true;
                    TFCMODEL.FEASIBLECHANGE = false;
                    TFCMODEL.NOTFEASIBLE = false;
                }
                else if (TFCMODEL.CONCLUSION == "2")
                {
                    TFCMODEL.FEASIBLEPRODUCT = false;
                    TFCMODEL.FEASIBLECHANGE = true;
                    TFCMODEL.NOTFEASIBLE = false;
                }
                else if (TFCMODEL.CONCLUSION == "3")
                {
                    TFCMODEL.FEASIBLEPRODUCT = false;
                    TFCMODEL.FEASIBLECHANGE = false;
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

                TFCMODEL.ROUTENO = "";
                NotifyPropertyChanged("TFCMODEL");
            }
        }

        private readonly ICommand mousecombocommand;
        public ICommand MouseComboCommand { get { return this.mousecombocommand; } }
        private void MouseClick()
        {
            if (SelectedGrid == null || !TFCMODEL.PartNo.IsNotNullOrEmpty()) return;

            string allow = tfcbll.GetRoute(TFCMODEL);
            if (SelectedGrid["Consideration"].ToString() == "Can the product be manufactured without incurring any unusual:")
            {
                return;
            }
            else if (SelectedGrid["Consideration"].ToString() == "Where statistical process control is used on similar products :")
            {
                return;
            }

            if (allow == "NO" && SelectedGrid["Consideration"].ToString() == "8. Is statistical process control presently used on similar products ?")
            {
                ShowInformationMessage("Special character(s) Found For this Part NO ,So User Entry is Denied for This Query!!!");
                return;
            }

            SelectedGrid.BeginEdit();
            if (SelectedGrid["Yes_No"].ToString() == "")
            {
                SelectedGrid["Yes_No"] = "X";
            }
            else
            {
                SelectedGrid["Yes_No"] = "";
            }
            SelectedGrid.EndEdit();

            if (SelectedGrid["Consideration"].ToString() == "8. Is statistical process control presently used on similar products ?")
            {
                if (SelectedGrid["Yes_No"].ToString() == "X")
                {
                    for (int i = 12; i <= 14; i++)
                    {
                        TFCMODEL.GRIDLOAD.Table.Rows[i]["Visible"] = "False";
                    }
                }
                else
                {
                    for (int i = 12; i <= 14; i++)
                    {
                        TFCMODEL.GRIDLOAD.Table.Rows[i]["Visible"] = "True";
                    }
                }
            }
        }

        private void SetdropDownItems()
        {
            try
            {
                DropDownItemsPart = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "PART NO", ColumnWidth = "120" },
                            new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "DESCRIPTION", ColumnWidth = "1*" }
                        };
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void LoadCmbDatas()
        {
            tfcbll.GetPartNoDetails(TFCMODEL);
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

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        private readonly ICommand printClickCommand;
        public ICommand PrintClickCommand { get { return this.printClickCommand; } }
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
                dt = tfcbll.print(TFCMODEL);
                if (dt == null || dt.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {
                    frmReportViewer frmrv = new frmReportViewer(dt, "TFC");
                    frmrv.ShowDialog();
                }
            }
        }
        public Boolean IsRecordsUpdated = false;
        private readonly ICommand saveClickCommand;
        public ICommand SaveClickCommand { get { return this.saveClickCommand; } }
        private void SaveCommand()
        {
            IsRecordsUpdated = false;
            if (!TFCMODEL.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part Number"));
                return;
            }
            else if (TFCMODEL.AUTOPARTYES == false && TFCMODEL.AUTOPARTNO == false)
            {
                ShowInformationMessage(PDMsg.NotEmpty("Auto Part"));
                return;
            }
            else if ((TFCMODEL.PROTOTYPE || TFCMODEL.PRELAUNCH || TFCMODEL.PRODUCTION) == false)
            {
                ShowInformationMessage(PDMsg.NotEmpty("Customer Programme"));
                return;
            }
            else if ((TFCMODEL.FEASIBLECHANGE || TFCMODEL.FEASIBLEPRODUCT || TFCMODEL.NOTFEASIBLE) == false)
            {
                ShowInformationMessage(PDMsg.NotEmpty("Conclusion"));
                return;
            }
            else
            {
                if (TFCMODEL.AUTOPARTYES == true)
                {
                    TFCMODEL.AUTO_PART = "Y";
                }
                else if (TFCMODEL.AUTOPARTNO == true)
                {
                    TFCMODEL.AUTO_PART = "N";
                }

                if (TFCMODEL.PROTOTYPE == true)
                {
                    TFCMODEL.CUST_PROG = "PT";
                }
                else if (TFCMODEL.PRELAUNCH == true)
                {
                    TFCMODEL.CUST_PROG = "PL";
                }
                else if (TFCMODEL.PRODUCTION == true)
                {
                    TFCMODEL.CUST_PROG = "PR";
                }

                if (TFCMODEL.FEASIBLEPRODUCT == true)
                {
                    TFCMODEL.CONCLUSION = Convert.ToString("1");
                }
                else if (TFCMODEL.FEASIBLECHANGE == true)
                {
                    TFCMODEL.CONCLUSION = Convert.ToString("2");
                }
                else if (TFCMODEL.NOTFEASIBLE == true)
                {
                    TFCMODEL.CONCLUSION = Convert.ToString("3");
                }
                GridValues();
                string mode = "";
                bool val = tfcbll.AddEditTfc(TFCMODEL, mode);
                IsRecordsUpdated = true;
                if (val == true)
                {
                    ShowInformationMessage(PDMsg.SavedSuccessfully);
                    _logviewBll.SaveLog(TFCMODEL.PartNo, "TFC");
                }
                else
                {
                    ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                    _logviewBll.SaveLog(TFCMODEL.PartNo, "TFC");
                }
                // Clear();
            }
        }

        public bool GridValues()
        {
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
            string[] arr = new string[20];
            for (int i = 0; i <= 14; i++)
            {
                if (i < 6)
                {
                    if (TFCMODEL.GRIDLOAD.Table.Rows[i]["Yes_No"].ToString() == "X")
                    {
                        arr[i] = "N";
                    }
                    else
                    {
                        arr[i] = "Y";
                    }
                }
                else if (i > 6 && i < 12)
                {
                    if (TFCMODEL.GRIDLOAD.Table.Rows[i]["Yes_No"].ToString() == "X")
                    {
                        arr[i] = "N";
                    }
                    else
                    {
                        arr[i] = "Y";
                    }
                }

                else if (i > 11)
                {
                    if (arr[11] == "Y")
                    {
                        if (TFCMODEL.GRIDLOAD.Table.Rows[i]["Yes_No"].ToString() == "X")
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
            }
            TFCMODEL.Q1 = arr[0];
            TFCMODEL.Q2 = arr[1];
            TFCMODEL.Q3 = arr[2];
            TFCMODEL.Q4 = arr[3];
            TFCMODEL.Q5 = arr[4];
            TFCMODEL.Q6 = arr[5];
            TFCMODEL.Q7 = arr[7];
            TFCMODEL.Q8 = arr[8];
            TFCMODEL.Q9 = arr[9];
            TFCMODEL.Q10 = arr[10];
            TFCMODEL.Q11 = arr[11];
            TFCMODEL.Q12 = arr[13];
            TFCMODEL.Q13 = arr[14];


            return true;
        }

        private void Clear()
        {
            TFCMODEL.PartNo = "";
            TFCMODEL.CUSTOMERNAME = "";
            TFCMODEL.PartDesc = "";
            TFCMODEL.CUSTOMERPARTNO = "";
            TFCMODEL.DATE = "";
            TFCMODEL.APPLICATION = "";
            TFCMODEL.NOTFEASIBLE = false;
            TFCMODEL.ROUTENO = "";
            TFCMODEL.AUTOPARTYES = true;
            TFCMODEL.AUTOPARTNO = false;
            TFCMODEL.PROTOTYPE = false;
            TFCMODEL.PRELAUNCH = true;
            TFCMODEL.PRODUCTION = false;
            TFCMODEL.FEASIBLEPRODUCT = true;
            TFCMODEL.FEASIBLECHANGE = false;
            TFCMODEL.NOTFEASIBLE = false;
            IsRecordsUpdated = false;
            GridLoad();
            GridLoadRisk();
        }

        private void GetGrid()
        {
            if (TFCMODEL.Q1 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[0]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[0]["Yes_No"] = "";
            }

            if (TFCMODEL.Q2 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[1]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[1]["Yes_No"] = "";
            }

            if (TFCMODEL.Q3 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[2]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[2]["Yes_No"] = "";
            }

            if (TFCMODEL.Q4 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[3]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[3]["Yes_No"] = "";
            }

            if (TFCMODEL.Q5 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[4]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[4]["Yes_No"] = "";
            }

            if (TFCMODEL.Q6 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[5]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[5]["Yes_No"] = "";
            }

            if (TFCMODEL.Q7 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[7]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[7]["Yes_No"] = "";
            }

            if (TFCMODEL.Q8 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[8]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[8]["Yes_No"] = "";
            }

            if (TFCMODEL.Q9 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[9]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[9]["Yes_No"] = "";
            }

            if (TFCMODEL.Q10 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[10]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[10]["Yes_No"] = "";
            }

            if (TFCMODEL.Q11 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[11]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[11]["Yes_No"] = "";
            }

            if (TFCMODEL.Q11 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[11]["Yes_No"] = "X";
                TFCMODEL.GRIDLOAD.Table.Rows[12]["Visible"] = "False";
                TFCMODEL.GRIDLOAD.Table.Rows[13]["Visible"] = "False";
                TFCMODEL.GRIDLOAD.Table.Rows[14]["Visible"] = "False";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[11]["Yes_No"] = "";
                TFCMODEL.GRIDLOAD.Table.Rows[12]["Visible"] = "True";
                TFCMODEL.GRIDLOAD.Table.Rows[13]["Visible"] = "True";
                TFCMODEL.GRIDLOAD.Table.Rows[14]["Visible"] = "True";
            }

            if (TFCMODEL.Q12 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[13]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[13]["Yes_No"] = "";
            }

            if (TFCMODEL.Q13 == "N")
            {
                TFCMODEL.GRIDLOAD.Table.Rows[14]["Yes_No"] = "X";
            }
            else
            {
                TFCMODEL.GRIDLOAD.Table.Rows[14]["Yes_No"] = "";
            }
        }

        public Action CloseAction { get; set; }

        public TfcModel tfcmodel { get; set; }

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
                else if (rowView["Consideration"].ToString() == "Where statistical process control is used on similar products :")
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
                    if (dtTable.Rows[i]["Consideration"].ToString() == "Can the product be manufactured without incurring any unusual:" || dtTable.Rows[i]["Consideration"].ToString() == "Where statistical process control is used on similar products :")
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
    }
}
