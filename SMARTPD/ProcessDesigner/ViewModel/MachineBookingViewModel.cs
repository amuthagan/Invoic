using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using System.Windows;

namespace ProcessDesigner.ViewModel
{
    public class MachineBookingViewModel : ViewModelBase
    {
        UserInformation _userInformation;
        MachineBookingBll _machineBookingBll;
        public MachineBookingViewModel(UserInformation userInformation)
        {
            try
            {
                _userInformation = userInformation;
                _machineBookingBll = new MachineBookingBll(_userInformation);
                MACHINEBOOKINGMODEL = new MachineBookingModel();
                this.exportToExcelCommand = new DelegateCommand(this.ExportToExcel);
                this.closeCommand = new DelegateCommand(this.Close);
                this.selectChangeComboCommandPlant = new DelegateCommand(this.SelectDataRowPlant);
                ComboPlant();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public DataView ComboPlant()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Plant");
            dt.Rows.Add("PADI");
            dt.Rows.Add("KPM-BOLT");
            dt.Rows.Add("KPM-NUT");
            dt.Rows.Add("PONDY");
            return MACHINEBOOKINGMODEL.PlantDetails = dt.DefaultView;
        }

        private readonly ICommand exportToExcelCommand;
        public ICommand ExportToExcelCommand { get { return this.exportToExcelCommand; } }
        private void ExportToExcel()
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.DefaultExt = ".xlsx";
                dlg.Filter = "Excel File (*.xls, *.xlsx)|*.xls;*.xlsx";
                dlg.ShowDialog();
                if (!dlg.FileName.IsNotNullOrEmpty()) return;

                DataTable dtMachineName = null;
                DataTable dtMachineBooking = null;
                DataSet ds;
                ds = _machineBookingBll.GetMachineBookingRange(MACHINEBOOKINGMODEL.PLANT, MACHINEBOOKINGMODEL.FROMDATE, MACHINEBOOKINGMODEL.TODATE);
                if (!ds.IsNotNullOrEmpty()) return;

                dtMachineName = ds.Tables["MACHINE_NAME"];
                dtMachineBooking = ds.Tables["MACHINE_BOOKING_DATA"];

                Microsoft.Office.Interop.Excel.Application excel = null;
                Microsoft.Office.Interop.Excel.Workbook workbook = null;
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

                try
                {
                    //dtMachineName.ExportToExcel(dlg.FileName);
                    excel = new Microsoft.Office.Interop.Excel.Application();

                    if (excel.Workbooks.Count == 0) excel.Workbooks.Add();
                    workbook = excel.Workbooks[1];

                    worksheet = excel.ActiveSheet;
                    if (!worksheet.IsNotNullOrEmpty())
                    {
                        excel.Workbooks.Add();
                        worksheet = excel.ActiveSheet;
                    }

                    int intAR = 1;
                    int intAC = 3;
                    worksheet.Columns.AutoFit();
                    Microsoft.Office.Interop.Excel.Range cellRange = null;
                    foreach (DataRow dataRow in dtMachineName.Rows)
                    {
                        cellRange = worksheet.Cells[intAR, intAC];
                        cellRange.Value = dataRow["COLUMN0"].ToValueAsString(); //COST_CENT_CODE
                        intAC++;
                    }

                    int row = 0;
                    int preRow = 2;
                    int rowTotal = 0;
                    int rowTotalMonth = 0;
                    bool pgCount = false;
                    DateTime startDate = Convert.ToDateTime(MACHINEBOOKINGMODEL.FROMDATE);
                    DateTime endDate = Convert.ToDateTime(MACHINEBOOKINGMODEL.TODATE);
                    #region pg loop
                    for (int pg = 1; pg <= 3; pg++)
                    {
                        intAR++;
                        intAC = 1;
                        bool boolDate = false;
                        DateTime minDate = endDate < startDate ? endDate : startDate;
                        DateTime maxDate = endDate > startDate ? endDate : startDate;
                        int total_months = ((Convert.ToDateTime(maxDate).Year - Convert.ToDateTime(minDate).Year) * 12) + Convert.ToDateTime(maxDate).Month - Convert.ToDateTime(minDate).Month;
                        #region monthStart loop
                        for (int monthStart = 1; monthStart <= total_months; monthStart++)
                        {
                            #region intAC loop
                            for (intAC = 3; intAC <= dtMachineName.Rows.Count + 3; intAC++)
                            {
                                cellRange = worksheet.Cells[1, intAC];
                                DataView dvMachineBooking = dtMachineBooking.DefaultView;
                                //dvMachineBooking.RowFilter = "COST_CENT_CODE='" + cellRange.Value + "'  AND PG_CATEGORY ='" + pg + "' AND PPAP_PLAN >= '" + startDate.ToString("dd-MMM-yyyy") + "'  AND  PPAP_PLAN <='" + startDate.AddMonths(1).ToString("dd-MMM-yyyy") + "' ";
                                dvMachineBooking.RowFilter = "COST_CENT_CODE='" + cellRange.Value + "'  AND PG_CATEGORY ='" + pg + "' AND PPAP_PLAN >= #" + startDate.ToString("dd-MMM-yyyy") + "#  AND  PPAP_PLAN <= #" + startDate.AddMonths(1).ToString("dd-MMM-yyyy") + "# ";

                                if (dvMachineBooking.Count > rowTotal)
                                {
                                    rowTotal = dvMachineBooking.Count;
                                    rowTotalMonth = dvMachineBooking.Count;
                                }
                                row = preRow;
                                if (dvMachineBooking.Count > 0)
                                {
                                    DataRowView currentRow = dvMachineBooking[0];
                                    char ch = '\0';
                                    #region i loop
                                    minDate = startDate;
                                    for (int i = 0; i <= dtMachineBooking.Rows.Count; i++)
                                    {
                                        dvMachineBooking.RowFilter = "COST_CENT_CODE='" + cellRange.Value + "'  AND PG_CATEGORY ='" + pg + "' AND PPAP_PLAN >= #" + minDate.ToString("dd-MMM-yyyy") + "#  AND  PPAP_PLAN <= #" + minDate.AddMonths(1).ToString("dd-MMM-yyyy") + "# ";
                                        if (dvMachineBooking.Count > 0)
                                        {
                                            cellRange = worksheet.Cells[row, intAC];
                                            if (!currentRow["DOC_REL_DT_ACTUAL"].ToValueAsString().IsNotNullOrEmpty())
                                            {
                                                cellRange.Interior.Color = System.Drawing.Color.Red;
                                                ch = (char)162;
                                            }
                                            else if (!currentRow["TOOLS_READY_ACTUAL_DT"].ToValueAsString().IsNotNullOrEmpty())
                                            {
                                                cellRange.Interior.Color = System.Drawing.Color.Yellow;
                                                ch = (char)163;
                                            }
                                            else if (!currentRow["FORGING_ACTUAL_DT"].ToValueAsString().IsNotNullOrEmpty())
                                            {
                                                cellRange.Interior.Color = System.Drawing.Color.Blue;
                                                ch = (char)223;
                                            }
                                            else if (!currentRow["SECONDARY_ACTUAL_DT"].ToValueAsString().IsNotNullOrEmpty())
                                            {
                                                cellRange.Interior.Color = System.Drawing.Color.Cyan;
                                                ch = (char)128;
                                            }
                                            else if (currentRow["SAMP_SUBMIT_DATE"].ToValueAsString().IsNotNullOrEmpty())
                                            {
                                                cellRange.Interior.Color = System.Drawing.Color.Green;
                                                ch = (char)181;
                                            }
                                            cellRange.Value = currentRow["PART_NO"].ToValueAsString() + "  " + ch.ToString();
                                            if (!boolDate)
                                            {
                                                cellRange = worksheet.Cells[row, 2];
                                                cellRange.Value = startDate;
                                            }
                                            if (!pgCount)
                                            {
                                                cellRange = worksheet.Cells[row, 1];
                                                cellRange.Value = "PG" + pg.ToString();
                                                pgCount = true;
                                            }
                                            row++;
                                            boolDate = true;
                                            minDate = minDate.AddMonths(1);
                                        }
                                    }
                                    #endregion
                                    preRow = row;
                                }
                            }
                            #endregion
                            //preRow = rowTotalMonth + preRow;
                            boolDate = false;
                            startDate = startDate.AddMonths(1);

                        }
                        #endregion

                        startDate = Convert.ToDateTime(MACHINEBOOKINGMODEL.FROMDATE);
                        preRow = preRow + 1;
                        //int lastRow = dtMachineBooking.Rows.Count + 2;
                        int lastRow = preRow + 1;
                        rowTotal = 0;
                        pgCount = false;
                        if (pg == 3)
                        {
                            cellRange = worksheet.Cells[lastRow, 1];
                            cellRange.Interior.Color = System.Drawing.Color.Red;
                            cellRange = worksheet.Cells[lastRow, 2];
                            cellRange.Value = "Awaiting Document " + ((char)162).ToString();

                            cellRange = worksheet.Cells[lastRow, 4];
                            cellRange.Interior.Color = System.Drawing.Color.Yellow;
                            cellRange = worksheet.Cells[lastRow, 5];
                            cellRange.Value = "Awaiting Tools " + ((char)163).ToString();

                            cellRange = worksheet.Cells[lastRow, 7];
                            cellRange.Interior.Color = System.Drawing.Color.Blue;
                            cellRange = worksheet.Cells[lastRow, 8];
                            cellRange.Value = "Awaiting Forging " + ((char)223).ToString();

                            cellRange = worksheet.Cells[lastRow, 10];
                            cellRange.Interior.Color = System.Drawing.Color.Cyan;
                            cellRange = worksheet.Cells[lastRow, 11];
                            cellRange.Value = "Awaiting Secondary " + ((char)128).ToString();

                            cellRange = worksheet.Cells[lastRow, 13];
                            cellRange.Interior.Color = System.Drawing.Color.Green;
                            cellRange = worksheet.Cells[lastRow, 14];
                            cellRange.Value = "Sample Submitted " + ((char)181).ToString();

                        }


                    }
                    #endregion

                    worksheet.UsedRange.Borders.Color = System.Drawing.Color.Black;
                    worksheet.UsedRange.Cells.Font.Bold = true;
                    worksheet.UsedRange.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                    worksheet.Columns[2].NumberFormat = "MMM-YY";
                    worksheet.Columns.AutoFit();

                    try
                    {
                        excel.DisplayAlerts = false;
                        worksheet.SaveAs(dlg.FileName);
                        ShowInformationMessage("Exported to Excel File Successfully");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
                            + ex.Message);
                    }
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                finally
                {
                    excel.Quit();
                    while (System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet) != 0) { }
                    worksheet = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand closeCommand;
        public ICommand CloseCommand { get { return this.closeCommand; } }
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private readonly ICommand selectChangeComboCommandPlant;
        public ICommand SelectChangeComboCommandPlant { get { return this.selectChangeComboCommandPlant; } }
        private void SelectDataRowPlant()
        {
            if (SelectedRowPlant != null)
            {
                MACHINEBOOKINGMODEL.PLANT = SelectedRowPlant["Plant"].ToString();
            }
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }

        public MachineBookingModel machinebookingmodel { get; set; }
        public MachineBookingModel MACHINEBOOKINGMODEL
        {
            get
            {
                return machinebookingmodel;
            }
            set
            {
                machinebookingmodel = value;
                NotifyPropertyChanged("MACHINEBOOKINGMODEL");
            }
        }

        private DataRowView _selectedrowplant;
        public DataRowView SelectedRowPlant
        {
            get
            {
                return _selectedrowplant;
            }

            set
            {
                _selectedrowplant = value;
            }
        }

        public Action CloseAction { get; set; }
    }
}
