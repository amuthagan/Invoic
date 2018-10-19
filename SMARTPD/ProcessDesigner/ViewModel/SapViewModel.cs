using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using Microsoft.Practices.Prism.Commands;
using System.Data;
using System.Windows.Input;
using ProcessDesigner.Common;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using System.Windows;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;

namespace ProcessDesigner.ViewModel
{
    public class SapViewModel : ViewModelBase
    {
        private readonly ICommand _onRefreshCommand;
        private readonly ICommand _onExportCommand;
        private readonly ICommand _onSaveCommand;
        private readonly ICommand _onCloseCommand;
        public Action CloseAction { get; set; }

        private readonly ICommand _bomEditCommand;
        public ICommand BomEditCommand { get { return this._bomEditCommand; } }

        public ICommand OnRefreshCommand { get { return this._onRefreshCommand; } }
        public ICommand OnExportCommand { get { return this._onExportCommand; } }
        public ICommand OnSaveCommand { get { return this._onSaveCommand; } }
        public ICommand OnCloseCommand { get { return this._onCloseCommand; } }
        private UserInformation _userInformation;

        private readonly ICommand _enterPartNumber;
        public ICommand EnterPartNumberCmb { get { return this._enterPartNumber; } }

        private SapModel _sapModel;
        private PCCSModel _pccsModel;
        private SapBll _sapBll;
        private string sapType = "";
        private string sPlant = "";
        public SapViewModel(UserInformation userInfo, string strType)
        {
            _userInformation = new UserInformation();
            _userInformation = userInfo;
            SapModel = new SapModel();
            _pccsModel = new PCCSModel();
            _sapBll = new SapBll(_userInformation);
            this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
            this.selectChangeComboCommandRoh = new DelegateCommand(this.SelectDataRowRoh);

            _bomEditCommand = new DelegateCommand<DataRowView>(this.BomEdit);
            this._enterPartNumber = new DelegateCommand<string>(this.EnterPartNumber);
            _onRefreshCommand = new DelegateCommand(this.Refresh);
            _onExportCommand = new DelegateCommand(this.Export);
            _onCloseCommand = new DelegateCommand(this.Close);
            _onSaveCommand = new DelegateCommand(this.Save);
            _sapBll.GetPartNoDetails(SapModel);
            _sapBll.GetRohNoDetails(SapModel);
            SetdropDownItems();
            sapType = strType.ToUpper();
            switch (sapType)
            {
                case "FERT":
                    {
                        GrpMFertVisibility = Visibility.Visible;
                        GrpKFertVisibility = Visibility.Collapsed;
                        GrpYFertVisibility = Visibility.Collapsed;
                    }
                    break;
            }
        }
        private void EnterPartNumber(string partNumber)
        {
            try
            {
                SelectDataRowPart();
            }
            catch (Exception)
            {

            }

        }
        private void BomEdit(DataRowView selectedItem)
        {
            try
            {
                if (selectedItem.IsNotNullOrEmpty())
                {
                    frmInputBox inp = new frmInputBox("BOM-Edit", "Unit");
                    inp.ShowDialog();
                    selectedItem.BeginEdit();
                    selectedItem["Component_Quantity"] = inp.Txt_InputBox.Text;
                    selectedItem.EndEdit();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

        }
        private Visibility _keyDateVisible = Visibility.Collapsed;
        public Visibility KeyDateVisible
        {
            get { return _keyDateVisible; }
            set
            {
                this._keyDateVisible = value;
                NotifyPropertyChanged("KeyDateVisible");

            }
        }
        private string[] arrExportHeader = new string[] { };
        private string operCode = "";
        private string _statusMsg = "";
        private void Refresh()
        {

            frmInputBox inp;
            SapModel.HalbDetails = null;
            SapModel.BomDetails = null;
            SapModel.FertKDetails = null;
            SapModel.FertMDetails = null;
            SapModel.FertYDetails = null;
            SapModel.ProductionVersionDetails = null;
            SapModel.RoutingDetails = null;
            if (!SapModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            switch (sapType)
            {
                case "BOM":
                    {
                        if (!_sapBll.IsProcessSheetAvailableDetails(SapModel))
                        {
                            ShowInformationMessage(PDMsg.NoRecordFound);
                            return;
                        }
                    inputInfo:
                        {

                            inp = new frmInputBox("BOM");
                            inp.ShowDialog();
                            if (inp.BlnOk)
                            {
                                SapModel.OperCode = inp.Txt_InputBox.Text;
                                if (SapModel.OperCode.Length == 4 || SapModel.OperCode.Length == 6)
                                {
                                }
                                else
                                {
                                    ShowInformationMessage("Enter Four Or Six Digit Code");
                                    goto inputInfo;
                                }
                            }
                            else
                            {
                                SapModel.OperCode = "";
                                return;
                            }
                            _sapBll.GetBomMasterDetails(SapModel, ref _statusMsg, ref arrExportHeader);
                            if (_statusMsg == PDMsg.NoRecordFound.ToString())
                            {
                                ShowInformationMessage(PDMsg.NoRecordFound);
                            }
                        }
                        if (SapModel.Plant == 1000) KeyDateVisible = Visibility.Hidden; // Changed from Hidden to Collapsed - Jeyan

                    }
                    break;
                case "FERT":
                    {
                        if (!_sapBll.IsProcessSheetAvailableDetails(SapModel))
                        {
                            ShowInformationMessage(PDMsg.NoRecordFound);
                            return;
                        }
                    inputInfo:
                        {
                            inp = new frmInputBox("FERT");
                            inp.ShowDialog();
                            if (inp.BlnOk)
                            {

                                SapModel.OperCode = inp.Txt_InputBox.Text;
                                if (SapModel.OperCode.Length == 4 || SapModel.OperCode.Length == 6)
                                {

                                }
                                else
                                {
                                    ShowInformationMessage("Enter Four Or Six Digit Code");
                                    goto inputInfo;
                                }

                            }
                            else
                            {
                                SapModel.OperCode = "";
                                return;
                            }
                        }
                        _sapBll.GetFertMasterDetails(SapModel, ref _statusMsg, ref arrExportHeader);
                        if (_statusMsg == "M")
                        {
                            GrpMFertVisibility = Visibility.Visible;
                            GrpKFertVisibility = Visibility.Collapsed;
                            GrpYFertVisibility = Visibility.Collapsed;

                        }
                        else if (_statusMsg == "K-O")
                        {
                            GrpMFertVisibility = Visibility.Collapsed;
                            GrpKFertVisibility = Visibility.Visible;
                            GrpYFertVisibility = Visibility.Collapsed;

                        }
                        else if (_statusMsg == "K")
                        {
                            GrpMFertVisibility = Visibility.Collapsed;
                            GrpKFertVisibility = Visibility.Visible;
                            GrpYFertVisibility = Visibility.Collapsed;
                        }
                        else if (_statusMsg == "Y")
                        {
                            GrpMFertVisibility = Visibility.Collapsed;
                            GrpKFertVisibility = Visibility.Collapsed;
                            GrpYFertVisibility = Visibility.Visible;
                        }
                        else
                        {
                            GrpMFertVisibility = Visibility.Visible;
                            GrpKFertVisibility = Visibility.Collapsed;
                            GrpYFertVisibility = Visibility.Collapsed;
                        }
                        if (_statusMsg == PDMsg.NoRecordFound.ToString())
                        {
                            ShowInformationMessage(PDMsg.NoRecordFound);
                        }
                    }
                    break;
                case "HALB":
                    {
                        SapModel.HalbDetails = null;
                        if (!_sapBll.IsProcessSheetAvailableDetails(SapModel))
                        {
                            ShowInformationMessage(PDMsg.NoRecordFound);
                            return;
                        }

                    inputInfoHalb:
                        {
                            inp = new frmInputBox("HALB");
                            inp.Txt_InputBox.Focus();
                            inp.ShowDialog();
                            SapModel.OperCode = inp.Txt_InputBox.Text;
                            if (inp.BlnOk)
                            {
                                if (SapModel.OperCode.Length == 4 || SapModel.OperCode.Length == 6)
                                {
                                }
                                else
                                {

                                    ShowInformationMessage("Enter Four Or Six Digit Code");
                                    goto inputInfoHalb;
                                }


                            }
                            else
                            {
                                SapModel.OperCode = "";
                                return;
                            }
                            _sapBll.GetHalbMasterDetails(SapModel, ref _statusMsg, ref arrExportHeader);
                            NotifyPropertyChanged("SapModel");
                            if (_statusMsg == PDMsg.NoRecordFound.ToString())
                            {
                                ShowInformationMessage(PDMsg.NoRecordFound);
                            }
                        }
                    }
                    break;
                case "ROUTING":
                    if (!_sapBll.IsProcessSheetAvailableDetails(SapModel))
                    {
                        ShowInformationMessage(PDMsg.NoRecordFound);
                        return;
                    }
                inputInfoRout:
                    {
                        inp = new frmInputBox("ROUTING");
                        inp.ShowDialog();
                        if (inp.BlnOk)
                        {

                            SapModel.OperCode = inp.Txt_InputBox.Text;
                            if (SapModel.OperCode.Length == 4 || SapModel.OperCode.Length == 6)
                            {
                            }
                            else
                            {
                                ShowInformationMessage("Enter Four Or Six Digit Code");
                                goto inputInfoRout;
                            }

                        }
                        else
                        {
                            SapModel.OperCode = "";
                            return;
                        }
                        _sapBll.GetRoutingMasterDetails(SapModel, ref _statusMsg, ref arrExportHeader);
                        if (_statusMsg == PDMsg.NoRecordFound.ToString())
                        {
                            ShowInformationMessage(PDMsg.NoRecordFound);
                        }
                    }
                    break;
                case "PRDVERSION":
                    if (!_sapBll.IsProcessSheetAvailableDetails(SapModel))
                    {
                        ShowInformationMessage(PDMsg.NoRecordFound);
                        return;
                    }
                inputInfoPrd:
                    {
                        inp = new frmInputBox("PRODUCTION VERSION");
                        inp.ShowDialog();
                        if (inp.BlnOk)
                        {

                            SapModel.OperCode = inp.Txt_InputBox.Text;
                            if (SapModel.OperCode.Length == 4 || SapModel.OperCode.Length == 6)
                            {
                            }
                            else
                            {
                                ShowInformationMessage("Enter Four Or Six Digit Code");
                                goto inputInfoPrd;
                            }
                        }
                        else
                        {
                            SapModel.OperCode = "";
                            return;
                        }
                        _sapBll.GetPrdVersionMasterDetails(SapModel, ref _statusMsg, ref arrExportHeader);
                        if (_statusMsg == PDMsg.NoRecordFound.ToString())
                        {
                            ShowInformationMessage(PDMsg.NoRecordFound);
                        }
                    }
                    NotifyPropertyChanged("SapModel");
                    break;
                default:
                    break;
            }
            if (SapModel.PartNo.IsNotNullOrEmpty())
            {
                if (sapType == "HALB")
                {
                    _sapBll.GetNoofOperationsHalb(SapModel);
                }
                else
                {
                    _sapBll.GetNoofOperations(SapModel);
                }

            }
        }
        private Visibility _grpMFertVisibility = Visibility.Collapsed;
        public Visibility GrpMFertVisibility
        {
            get { return _grpMFertVisibility; }
            set
            {
                _grpMFertVisibility = value;
                NotifyPropertyChanged("GrpMFertVisibility");
            }
        }
        private Visibility _grpKFertVisibility = Visibility.Collapsed;
        public Visibility GrpKFertVisibility
        {
            get { return _grpKFertVisibility; }
            set
            {
                _grpKFertVisibility = value;
                NotifyPropertyChanged("GrpKFertVisibility");
            }
        }

        private Visibility _grpYFertVisibility = Visibility.Collapsed;
        public Visibility GrpYFertVisibility
        {
            get { return _grpYFertVisibility; }
            set
            {
                _grpYFertVisibility = value;
                NotifyPropertyChanged("GrpYFertVisibility");
            }
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
        private string exportFilePath = "";
        private void Export()
        {
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();

            if (!SapModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            if (!_sapBll.IsProcessSheetAvailableDetails(SapModel))
            {
                ShowInformationMessage(PDMsg.NoRecordFound);
                return;
            }
            sPlant = "";
            sPlant = SapModel.Plant.ToString(); // added by Jeyan

            if (sapType == "BOM")
            {
                if (SapModel.BomDetails.IsNotNullOrEmpty())
                {
                    if (SapModel.BomDetails.Count == 0)
                    {
                        ShowInformationMessage(PDMsg.NoRecordFound);
                        return;
                    }
                }
                else
                {
                    ShowInformationMessage(PDMsg.NoRecordFound);
                    return;
                }


            }
            else if (sapType == "FERT")
            {
                //if (SapModel.FertKDetails.IsNotNullOrEmpty() && SapModel.FertMDetails.IsNotNullOrEmpty() && SapModel.FertYDetails.IsNotNullOrEmpty())
                //{
                if (!SapModel.FertKDetails.IsNotNullOrEmpty() && !SapModel.FertMDetails.IsNotNullOrEmpty() && !SapModel.FertYDetails.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NoRecordFound);
                    return;
                }
                //}
                //else
                //{
                //    ShowInformationMessage(PDMsg.NoRecordFound);
                //    return;
                //}
            }
            else if (sapType == "HALB")
            {
                if (SapModel.HalbDetails.IsNotNullOrEmpty())
                {
                    if (SapModel.HalbDetails.Count == 0)
                    {
                        ShowInformationMessage(PDMsg.NoRecordFound);
                        return;
                    }

                }
                else
                {
                    ShowInformationMessage(PDMsg.NoRecordFound);
                    return;
                }
            }
            else if (sapType == "ROUTING")
            {
                if (SapModel.RoutingDetails.IsNotNullOrEmpty())
                {
                    if (SapModel.RoutingDetails.Count == 0)
                    {
                        ShowInformationMessage(PDMsg.NoRecordFound);
                        return;
                    }
                }
                else
                {
                    ShowInformationMessage(PDMsg.NoRecordFound);
                    return;
                }

            }
            else if (sapType == "PRDVERSION")
            {
                if (SapModel.ProductionVersionDetails.IsNotNullOrEmpty())
                {
                    if (SapModel.ProductionVersionDetails.Count == 0)
                    {
                        ShowInformationMessage(PDMsg.NoRecordFound);
                        return;
                    }
                }
                else
                {
                    ShowInformationMessage(PDMsg.NoRecordFound);
                    return;
                }


            }
            // Set filter options and filter index.
            openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            if (ShowConfirmMessageYesNo("Do You Want To Append The File ") == MessageBoxResult.Yes)
            {
                System.Windows.Forms.DialogResult result = openFileDialog1.ShowDialog();
                exportFilePath = openFileDialog1.FileName;
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    ExportToExcelData();
                    ShowInformationMessage("Exported Successfully");
                }
                // Commnted by Jeyan
                //else if (result == System.Windows.Forms.DialogResult.Cancel)
                //{

                //    if (!File.Exists(exportFilePath))
                //    {
                //        File.Create(exportFilePath);
                //        using (StreamWriter stream = new StreamWriter(exportFilePath, true))
                //        {
                //            // stream.WriteLine("Test1Newline++" + "\t" + "Tab" + Environment.NewLine);

                //        }
                //    }

                //}
            }
            else
            {
                System.Windows.Forms.SaveFileDialog savefile = new System.Windows.Forms.SaveFileDialog();
                // set a default file name
                savefile.FileName = "Sap_" + DateTime.Now.Date.ToString("ddMMyyyy") + ".txt";
                // set filters - this can be done in properties as well
                savefile.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                //StreamWriter outFile = null;
                if (savefile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //using (outFile = File.CreateText(savefile.FileName))
                    //{
                    //    outFile.Close();
                    //}
                    //using (outFile = new StreamWriter(savefile.FileName))
                    //{
                    exportFilePath = savefile.FileName;
                    ExportToExcelData(false);
                    // }
                    ShowInformationMessage("Exported Successfully");
                }
            }
        }

        private void ExportToExcelData(bool append = true)
        {

            int lineCount = 0;
            try
            {
                lineCount = File.ReadAllLines(exportFilePath).Length;
            }
            catch (Exception)
            {

            }


            using (StreamWriter stream = new StreamWriter(exportFilePath, append))
            {
                //if (lineCount == 0) Commented by Jeyan
                if (append == false)
                {
                    for (int j = 0; j < arrExportHeader.Length; j++)
                    {
                        if ((sapType + sPlant + arrExportHeader[j]).ToUpper() != "BOM1000DATUV") // If condition added by Jeyan
                        {
                            stream.Write(arrExportHeader[j] + "\t");
                        }
                        
                    }
                    stream.WriteLine();
                }
                var builder = new StringBuilder();
                switch (sapType)
                {
                    case "BOM":
                        DataTable dtTempBom = new DataTable();
                        dtTempBom = SapModel.BomDetails.ToTable().Copy(); // modified by Jeyan to import last row
                        dtTempBom.Columns.Remove("SeqNo");
                        if ((sapType + sPlant).ToUpper() == "BOM1000") // If condition added by Jeyan
                        {
                            dtTempBom.Columns.Remove("Key_Date");
                        }
                        foreach (DataRowView row in dtTempBom.DefaultView)
                        {
                            for (int n = 0; n < dtTempBom.Columns.Count; n++)
                            {
                                stream.Write(row[n].ToString() + "\t");
                            }
                            stream.WriteLine();
                        }
                        stream.Close();
                        break;
                    case "FERT":
                        {

                            if (_statusMsg == "M")
                            {
                                foreach (DataRowView row in SapModel.FertMDetails)
                                {
                                    for (int n = 0; n < SapModel.FertMDetails.Table.Columns.Count; n++)
                                    {
                                        stream.Write(row[n].ToString() + "\t");
                                    }
                                    stream.WriteLine();
                                }
                                stream.Close();

                            }
                            else if (_statusMsg == "K")
                            {
                                foreach (DataRowView row in SapModel.FertKDetails)
                                {
                                    for (int n = 0; n < SapModel.FertKDetails.Table.Columns.Count; n++)
                                    {
                                        stream.Write(row[n].ToString() + "\t");
                                    }
                                    stream.WriteLine();
                                }
                                stream.Close();
                            }
                            else if (_statusMsg == "Y")
                            {
                                foreach (DataRowView row in SapModel.FertYDetails)
                                {
                                    for (int n = 0; n < SapModel.FertYDetails.Table.Columns.Count; n++)
                                    {
                                        stream.Write(row[n].ToString() + "\t");
                                    }
                                    stream.WriteLine();
                                }
                                stream.Close();
                            }


                        }
                        break;
                    case "HALB":
                        DataTable dtTempHalb = new DataTable();
                        dtTempHalb = SapModel.HalbDetails.ToTable().Copy();
                        dtTempHalb.Columns.Remove("Opn_Desc");
                        dtTempHalb.Columns.Remove("SeqNo");
                        foreach (DataRowView row in dtTempHalb.DefaultView)
                        {
                            for (int n = 0; n < dtTempHalb.Columns.Count; n++)
                            {

                                stream.Write(row[n].ToString() + "\t");
                            }

                            stream.WriteLine();
                        }
                        stream.Close();
                        break;
                    case "ROUTING":
                        foreach (DataRowView row in SapModel.RoutingDetails)
                        {
                            for (int n = 0; n < SapModel.RoutingDetails.Table.Columns.Count; n++)
                            {
                                stream.Write(row[n].ToString() + "\t");
                            }
                            stream.WriteLine();
                        }
                        stream.Close();
                        break;
                    case "PRDVERSION":
                        foreach (DataRowView row in SapModel.ProductionVersionDetails)
                        {
                            for (int n = 0; n < SapModel.ProductionVersionDetails.Table.Columns.Count; n++)
                            {
                                stream.Write(row[n].ToString() + "\t");
                            }
                            stream.WriteLine();
                        }
                        stream.Close();
                        break;
                    default:
                        break;
                }

            }
        }
        private void Save()
        {
            // throw new NotImplementedException();
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
                ex.LogException();
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
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        public SapModel SapModel
        {
            get
            {
                return _sapModel;
            }
            set
            {
                _sapModel = value;
                NotifyPropertyChanged("SapModel");
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

        private readonly ICommand selectChangeComboCommandPartNo;
        public ICommand SelectChangeComboCommandPartNo { get { return this.selectChangeComboCommandPartNo; } }
        private void SelectDataRowPart()
        {
            if (this.SelectedRowPart != null)
            {
                if (SelectedRowPart.IsNotNullOrEmpty())
                {
                    SapModel.PartNo = this.SelectedRowPart["PART_NO"].ToString();
                    SapModel.PartDesc = this.SelectedRowPart["PART_DESC"].ToString();
                    SapModel.NoOfoperations = "";
                }
                if (SapModel.PartNo.IsNotNullOrEmpty())
                {
                    SapModel.HalbDetails = null;
                    SapModel.BomDetails = null;
                    SapModel.FertKDetails = null;
                    SapModel.FertMDetails = null;
                    SapModel.FertYDetails = null;
                    SapModel.ProductionVersionDetails = null;
                    SapModel.RoutingDetails = null;
                    //if (sapType == "HALB")
                    //{
                    //    _sapBll.GetNoofOperationsHalb(SapModel);
                    //}
                    //else
                    //{
                    //    _sapBll.GetNoofOperations(SapModel);
                    //}

                }
                Refresh();
            }

        }
        private DataRowView _selectedrowRoh;
        public DataRowView SelectedRowRoh
        {
            get
            {
                return _selectedrowRoh;
            }

            set
            {
                _selectedrowRoh = value;
            }
        }

        private readonly ICommand selectChangeComboCommandRoh;
        public ICommand SelectChangeComboCommandRoh { get { return this.selectChangeComboCommandRoh; } }
        private void SelectDataRowRoh()
        {
            if (this.SelectedRowRoh != null)
            {

            }

        }

        private string filename = "";
        private string m_exePath = string.Empty;
        private string CreateTextFile(string filename)
        {
            bool fileExsists = false;

            //string filenamebase = "CCCostErrorLog";

            string folderName = "CostUpdate";
            string strFileName = string.Empty;
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!Directory.Exists(m_exePath + "\\" + folderName))
            {
                Directory.CreateDirectory(m_exePath + "\\" + folderName);
            }
            m_exePath = m_exePath + "\\" + folderName;
            fileExsists = File.Exists(m_exePath + "\\" + filename);
            if (fileExsists == false)
            {
                System.IO.FileStream fs = System.IO.File.Create(m_exePath + "\\" + filename);
                fs.Close();
            }
            // filename = filenamebase + "-" + DateTime.Now.Date.ToString("ddMMyyyy") + ".log";

            return m_exePath + "\\" + filename;
        }

        public void LogWrite(string logMessage)
        {

            try
            {
                using (StreamWriter w = File.AppendText(filename))
                {
                    w.WriteLine(logMessage);
                    //  Log(logMessage, w);
                }
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

        private ObservableCollection<DropdownColumns> _dropDownItemsROH;
        public ObservableCollection<DropdownColumns> DropDownItemsROH
        {
            get
            {
                return _dropDownItemsROH;
            }
            set
            {
                this._dropDownItemsROH = value;
                NotifyPropertyChanged("DropDownItemsROH");
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
                DropDownItemsROH = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "ROH", ColumnDesc = "ROH", ColumnWidth = "1*" },
                        };

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
    }
}
