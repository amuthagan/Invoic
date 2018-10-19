using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using ProcessDesigner.BLL;
using System.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.UserControls;
using System.Collections.ObjectModel;
using ProcessDesigner.Common;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.Office.Interop;
using WPF.MDI;
using System.Windows.Forms;
using System.IO;

namespace ProcessDesigner.ViewModel
{
    public class SapExportToPd : ViewModelBase
    {
        public Action CloseAction { get; set; }
        private UserInformation userInformation;
        private SapExportPDBll _bll;
        private readonly ICommand _onUpdateCommand;
        private readonly ICommand _onCancelCommand;
        private readonly ICommand _onOpenExcelCommand;
        private Microsoft.Office.Interop.Excel.Application _excel;
        private Microsoft.Office.Interop.Excel.Workbook _wb;
        private Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
        private Microsoft.Office.Interop.Excel.Range range;

        public ICommand OnUpdateCommand { get { return this._onUpdateCommand; } }
        public ICommand OnCancelCommand { get { return this._onCancelCommand; } }
        public ICommand OnOpenExcelCommand { get { return this._onOpenExcelCommand; } }
        public SapExportToPd(UserInformation userInfo)
        {
            userInformation = new UserInformation();
            userInformation = userInfo;
            this._onUpdateCommand = new DelegateCommand(this.Update);
            this._onCancelCommand = new DelegateCommand(this.Close);
            this._onOpenExcelCommand = new DelegateCommand(this.OpenExcel);
            _bll = new SapExportPDBll(userInformation);
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

        private void OpenExcel()
        {
            SapExport = "";
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "xlsx Files (*.xlsx,*.xls)|*.xlsx;*.xls|All Files (*.*)|*.*";


            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                CmbFilePath = openFileDialog1.FileName;


            }
        }
        private void releaseObject(object obj)
        {

            try
            {

                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);

                obj = null;

            }

            catch (Exception ex)
            {

                obj = null;
                ex.LogException();
                Console.WriteLine("Unable to release the Object " + ex.ToString());

            }

            finally
            {

                GC.Collect();

            }

        }



        private string _cmbFilePath = "";
        public string CmbFilePath
        {
            get { return _cmbFilePath; }
            set
            {
                _cmbFilePath = value;
                NotifyPropertyChanged("CmbFilePath");
            }
        }

        private string _sapExport = "Exporting Sap Data to ProcessDesigner...";
        public string SapExport
        {
            get { return _sapExport; }
            set
            {
                _sapExport = value;
                NotifyPropertyChanged("SapExport");
            }
        }
        private Visibility _sapExportVisible = Visibility.Hidden;
        public Visibility SapExportVisible
        {
            get { return _sapExportVisible; }
            set
            {
                _sapExportVisible = value;
                NotifyPropertyChanged("SapExportVisible");
            }
        }
        private string _cmbTableName = "";
        public string CmbTableName
        {
            get { return _cmbTableName; }
            set
            {
                _cmbTableName = value;
                NotifyPropertyChanged("CmbTableName");
            }
        }
        private void Update()
        {
            int rCnt = 0;
            string str, str2, str3;
            int cCnt = 0;
            SapExport = "Exporting Sap Data to ProcessDesigner...";
            SapExportVisible = Visibility.Visible;
            if (CmbTableName == "")
            {
                ShowInformationMessage(PDMsg.NotEmpty("Updation Name"));
                return;
            }
            else if (CmbFilePath == "")
            {
                ShowInformationMessage(PDMsg.NotEmpty("File Path"));
                return;
            }

            SapExport = "Exporting Sap Data to ProcessDesigner...";
            NotifyPropertyChanged("SapExport");

            _excel = new Microsoft.Office.Interop.Excel.Application();
            _wb = _excel.Workbooks.Open(CmbFilePath);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)_wb.Worksheets.get_Item(1);
            range = xlWorkSheet.UsedRange;
            int preCount = 0;
            switch (CmbTableName)
            {
                case "ROH-UPDATION":
                    preCount = _bll.Update_Remove_SapRoh();
                    for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                    {
                        str = (string)(range.Cells[rCnt, 1] as Microsoft.Office.Interop.Excel.Range).Value2;
                        _bll.Update_Insert_SapRoh(str);
                    }

                    SapExport = preCount + " Rows have been Removed and New Values ('" + _bll.Update_AfterInsertCount_SapRoh() + "') Updated Successfully.";
                    break;
                case "SAP_CC Vs OPRN CC":
                    for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                    {
                        _bll.Update_Insert_Sap_Ccvsoprncc((string)(range.Cells[rCnt, 4] as Microsoft.Office.Interop.Excel.Range).Text, (string)(range.Cells[rCnt, 5] as Microsoft.Office.Interop.Excel.Range).Text, (string)(range.Cells[rCnt, 1] as Microsoft.Office.Interop.Excel.Range).Text);
                    }
                    SapExport = "Processing Completed SuccessFully...";
                    break;
                case "SAP_MATGR Vs PD_OPRN CODE":
                    for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                    {
                        _bll.Update_Insert_Sap_Matgrvspd_Oprncode((string)(range.Cells[rCnt, 3] as Microsoft.Office.Interop.Excel.Range).Text, (string)(range.Cells[rCnt, 1] as Microsoft.Office.Interop.Excel.Range).Text);
                    }
                    SapExport = "Processing Completed SuccessFully...";
                    break;
                case "SAP_MATGR Vs SAP_ROUT_SHORT TEXT":
                    for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                    {
                        _bll.Update_Insert_Sap_Matgrvssap_Routshorttext((string)(range.Cells[rCnt, 2] as Microsoft.Office.Interop.Excel.Range).Text, (string)(range.Cells[rCnt, 1] as Microsoft.Office.Interop.Excel.Range).Text);
                    }
                    SapExport = "Processing Completed SuccessFully...";
                    break;
                case "SAP-NUT-BOLT-STD-SPL-COSTCENRE":
                    for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                    {
                        _bll.Update_Insert_Sap_Nutboltstdsplcostcenre((string)(range.Cells[rCnt, 4] as Microsoft.Office.Interop.Excel.Range).Text, (string)(range.Cells[rCnt, 1] as Microsoft.Office.Interop.Excel.Range).Text);
                    }
                    SapExport = "Processing Completed SuccessFully...";
                    break;
                case "SAPNO vs UNIT-OF-MEASURE-UPDATE":
                    for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                    {
                        _bll.Update_Insert_Sapno_Vs_Unitofmeasureupdate((string)(range.Cells[rCnt, 2] as Microsoft.Office.Interop.Excel.Range).Text, (string)(range.Cells[rCnt, 1] as Microsoft.Office.Interop.Excel.Range).Text);
                    }
                    SapExport = "Processing Completed SuccessFully...";
                    break;
                case "SAP_MATGR Vs CONFIRMATION POINTS":
                    for (rCnt = 1; rCnt <= range.Rows.Count; rCnt++)
                    {
                        _bll.Update_Insert_Sap_Matgrvsconfirmationpoints((string)(range.Cells[rCnt, 3] as Microsoft.Office.Interop.Excel.Range).Text, (string)(range.Cells[rCnt, 1] as Microsoft.Office.Interop.Excel.Range).Text);
                    }
                    SapExport = "Processing Completed SuccessFully...";
                    break;
                default:
                    break;
            }
            _wb.Close(true, null, null);
            _excel.Quit();
            releaseObject(xlWorkSheet);
            releaseObject(_wb);
            releaseObject(_excel);
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return System.Windows.MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

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

    }
}
