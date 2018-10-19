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
using WPF.MDI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ProcessDesigner.ViewModel
{
    public class CFTMeetingReportViewModel : ViewModelBase
    {
        private CFTMeetingBll _bll;
        private readonly ICommand _onSaveCommand;
        private readonly ICommand _onPrintCommand;
        private readonly ICommand _onCloseCommand;
        public ICommand OnPrintCommand { get { return this._onPrintCommand; } }
        public ICommand OnSaveCommand { get { return this._onSaveCommand; } }
        public ICommand OnCloseCommand { get { return this._onCloseCommand; } }
        public Action CloseAction { get; set; }
        private readonly ICommand _enterPartNumber;
        public ICommand EnterPartNumberCmb { get { return this._enterPartNumber; } }

        public CFTMeetingReportViewModel(UserInformation userInformation)
        {
            _bll = new CFTMeetingBll(userInformation);
            CFTMeetingModel = new CFTMeetingModel();
            this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
            this.selectChangeComboCommandCtm1 = new DelegateCommand(this.SelectDataRowCtm1);
            this.selectChangeComboCommandCtm2 = new DelegateCommand(this.SelectDataRowCtm2);
            this.selectChangeComboCommandCtm3 = new DelegateCommand(this.SelectDataRowCtm3);
            this.selectChangeComboCommandCtm4 = new DelegateCommand(this.SelectDataRowCtm4);
            this.selectChangeComboCommandCtm5 = new DelegateCommand(this.SelectDataRowCtm5);
            this.selectChangeComboCommandCtm6 = new DelegateCommand(this.SelectDataRowCtm6);
            this.selectChangeComboCommandCtm7 = new DelegateCommand(this.SelectDataRowCtm7);
            this.safetyIsCheckedCommand = new DelegateCommand<string>(this.SafetyIsChecked);
            this._enterPartNumber = new DelegateCommand<string>(this.EnterPartNumber);
            this.protoIsCheckedCommand = new DelegateCommand<string>(this.ProtoIsChecked);
            this._onPrintCommand = new DelegateCommand(this.Print);
            this._onSaveCommand = new DelegateCommand(this.Save);
            this._onCloseCommand = new DelegateCommand(this.Close);
            InitLoad();
            SetdropDownItems();
            FocusButton = true;
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

        private bool _focusButton = false;
        public bool FocusButton
        {
            get { return _focusButton; }
            set
            {
                _focusButton = value;
                NotifyPropertyChanged("FocusButton");
            }
        }

        private bool _isEnabledPrint = false;
        public bool IsEnabledPrint
        {
            get { return _isEnabledPrint; }
            set
            {
                _isEnabledPrint = value;
                NotifyPropertyChanged("IsEnabledPrint");
            }
        }

        private readonly ICommand safetyIsCheckedCommand;
        public ICommand SafetyIsCheckedCommand { get { return this.safetyIsCheckedCommand; } }
        private void SafetyIsChecked(string checkedSafety)
        {
            if (checkedSafety == "YC")
            {
                CFTMeetingModel.ChkSPYesIsChecked = true;
                CFTMeetingModel.ChkSPNoIsChecked = false;
            }
            else if (checkedSafety == "YU")
            {
                CFTMeetingModel.ChkSPYesIsChecked = false;
                CFTMeetingModel.ChkSPNoIsChecked = true;
            }
            else if (checkedSafety == "NC")
            {
                CFTMeetingModel.ChkSPYesIsChecked = false;
                CFTMeetingModel.ChkSPNoIsChecked = true;
            }
            else if (checkedSafety == "NU")
            {
                CFTMeetingModel.ChkSPYesIsChecked = true;
                CFTMeetingModel.ChkSPNoIsChecked = false;
            }
            CFTMeetingModel.IsSafetyPart = (CFTMeetingModel.ChkSPYesIsChecked) ? CFTMeetingModel.ChkSPYesIsChecked.ToValueAsString() : false.ToValueAsString();
        }
        private readonly ICommand protoIsCheckedCommand;
        public ICommand ProtoIsCheckedCommand { get { return this.protoIsCheckedCommand; } }
        private void ProtoIsChecked(string checkedProto)
        {
            if (checkedProto == "YC")
            {
                CFTMeetingModel.ChkPTYesIsChecked = true;
                CFTMeetingModel.ChkPTNoIsChecked = false;

            }
            else if (checkedProto == "YU")
            {
                CFTMeetingModel.ChkPTYesIsChecked = false;
                CFTMeetingModel.ChkPTNoIsChecked = true;
            }
            else if (checkedProto == "NC")
            {
                CFTMeetingModel.ChkPTYesIsChecked = false;
                CFTMeetingModel.ChkPTNoIsChecked = true;
            }
            else if (checkedProto == "NU")
            {
                CFTMeetingModel.ChkPTYesIsChecked = true;
                CFTMeetingModel.ChkPTNoIsChecked = false;
            }
            CFTMeetingModel.IsProtoType = (CFTMeetingModel.ChkPTYesIsChecked) ? CFTMeetingModel.ChkPTYesIsChecked.ToValueAsString() : false.ToValueAsString();
        }
        private void InitLoad()
        {
            CFTMeetingModel = new CFTMeetingModel();
            _bll.GetCPRptCPM(CFTMeetingModel);
            LoadTimeChart();
            _bll.GetPartNoDetails(CFTMeetingModel);
            CFTMeetingModel.CFTMeetingIssueNo = "";
            CFTMeetingModel.CFTMeetingDate = null;
            CFTMeetingModel.PPAPSampleQty = "";


        }
        public void OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            int iCurrentRow;
            iCurrentRow = e.Row.GetIndex() + 1;
            if ((iCurrentRow <= 11 && iCurrentRow >= 5) || iCurrentRow == 12 || iCurrentRow == 14 || iCurrentRow == 16 || iCurrentRow == 17)
            {
                if (e.Column.Header.ToString() == "Planned End Date") e.Cancel = false;
                if (e.Column.Header.ToString() == "Revised Date") e.Cancel = false;
                if (e.Column.Header.ToString() == "Planned Start Date") e.Cancel = false;
                if (e.Column.Header.ToString() == "Actual Finish Date") e.Cancel = false;
            }
            else
            {
                if (e.Column.Header.ToString() == "Planned End Date") e.Cancel = true;
                if (e.Column.Header.ToString() == "Revised Date") e.Cancel = true;
                if (e.Column.Header.ToString() == "Planned Start Date") e.Cancel = true;
                if (e.Column.Header.ToString() == "Actual Finish Date") e.Cancel = true;
            }

            //  ((System.Data.DataRowView)(e.Row.Item))

        }
        public void OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;

            if (e.Column.GetType() == typeof(DataGridTemplateColumn))
            {
                var popup = GetVisualChild<System.Windows.Controls.Primitives.Popup>(e.EditingElement);
                if (popup != null && popup.IsOpen)
                {
                    e.Cancel = true;
                    return;
                }
            }
            TextBox tb = e.EditingElement as TextBox;
            DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            string columnName = e.Column.SortMemberPath;
            if (selecteditem["Planned_End_Date"].ToString().Length > 0 && selecteditem["Planned_Start_Date"].ToString().Length > 0)
            {

                if (Convert.ToDateTime(selecteditem["Planned_End_Date"]).Date <= Convert.ToDateTime(selecteditem["Planned_Start_Date"]).Date)
                {
                    switch (selecteditem["Activity"].ToString())
                    {
                        case "Document Release":
                            ShowInformationMessage("Document Release End Date should be greater than Start Date");
                            break;
                        case "Tool Manufacturing":
                            ShowInformationMessage("Tool Manufacturing End Date should be greater than Start Date");
                            break;
                        case "B/OTools (if any)":
                            ShowInformationMessage("B/O Tools End Date should be greater than Start Date");
                            break;
                        case "RM":
                            ShowInformationMessage("RM End Date should be greater than Start Date");
                            break;
                        case "Forging":
                            ShowInformationMessage("Forging End Date should be greater than Start Date");
                            break;
                        case "Secondary":
                            ShowInformationMessage("Secondary End Date should be greater than Start Date");
                            break;
                        case "Heat/Surface Treatment":
                            ShowInformationMessage("Heat Surface Treatment End Date should be greater than Start Date");
                            break;
                        case "PPAP":
                            ShowInformationMessage("PPAP End Date should be greater than Start Date");
                            break;

                        default:
                            break;
                    }
                    return;
                }
            }
        }
        private static T GetVisualChild<T>(DependencyObject visual) where T : DependencyObject
        {
            if (visual == null)
                return null;

            var count = System.Windows.Media.VisualTreeHelper.GetChildrenCount(visual);
            for (int i = 0; i < count; i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(visual, i);

                var childOfTypeT = child as T ?? GetVisualChild<T>(child);
                if (childOfTypeT != null)
                    return childOfTypeT;
            }

            return null;
        }
        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                WPF.MDI.ClosingEventArgs closingev;
                closingev = (WPF.MDI.ClosingEventArgs)e;

                if (CFTMeetingModel.PartNo.IsNotNullOrEmpty() && !IsEnabledPrint)
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        if (!IsEnabledPrint)
                        {
                            closingev.Cancel = true;
                            e = closingev;
                            return;
                        }
                    }
                }

                if (ShowConfirmMessageYesNo("Do you want to close the Form?") == MessageBoxResult.No)
                {
                    closingev.Cancel = true;
                    //MessageBox.Show("Please select ADMIN rights for any one administrator user", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    e = closingev;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Close()
        {
            try
            {
                if (CFTMeetingModel.PartNo.IsNotNullOrEmpty() && !IsEnabledPrint)
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        if (!IsEnabledPrint) return;
                    }
                }

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
        private void Save()
        {
            try
            {
                string status = "";
                FocusButton = true;
                if (CFTMeetingModel.PartNo.Trim().Length <= 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("PartNo"));
                    FocusButton = true;
                    return;
                }

                if (CFTMeetingModel.PPAPSampleQty.Trim().Length <= 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("SampleQty"));
                    return;
                }
                else if (CFTMeetingModel.CFTMeetingIssueNo.Trim().Length <= 0)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Issue Number"));
                    return;
                }
                else if (CFTMeetingModel.CFTMeetingDate == null)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("CFT Meeting Date"));
                    return;
                }
                Progress.ProcessingText = PDMsg.ProgressUpdateText;
                Progress.Start();
                if (_bll.SaveCFTMeeting(CFTMeetingModel, ref status))
                {
                    Progress.End();
                    // ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                    //InitLoad();
                }

                IsEnabledPrint = true;
                if (status == "INS")
                {
                    ShowInformationMessage(PDMsg.SavedSuccessfully);
                }
                else
                {
                    ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                }
                Progress.End();
            }
            catch (Exception ex)
            {
                Progress.End();
                ex.LogException();
            }
            FocusButton = true;
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
        private void Print()
        {
            if (!IsEnabledPrint) return;

            DataSet dsCFT = new DataSet();
            List<CFTMeetingModel> cftDt = new List<CFTMeetingModel>();
            if (CFTMeetingModel.PartNo.Trim().Length <= 0)
            {
                ShowInformationMessage(PDMsg.NotEmpty("PartNo"));
                return;
            }

            if (CFTMeetingModel.CFTMeetingIssueNo.Trim().Length <= 0)
            {
                ShowInformationMessage(PDMsg.NotEmpty("Issue Number"));
                return;
            }
            else if (CFTMeetingModel.CFTMeetingDate == null)
            {
                ShowInformationMessage(PDMsg.NotEmpty("CFT Meeting Date"));
                return;
            }
            //if (!CFTMeetingModel.PartNo.IsNotNullOrEmpty() && !CFTMeetingModel.CFTMeetingDate.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NoRecordsPrint);
            //    return;
            //}
            Progress.Start();
            if (CFTMeetingModel.PartNo.IsNotNullOrEmpty() && CFTMeetingModel.CFTMeetingDate.IsNotNullOrEmpty() && CFTMeetingModel.CFTMeetingIssueNo.IsNotNullOrEmpty())
            {
                try
                {

                    cftDt.Add(CFTMeetingModel);
                    DataTable dtTemp = new DataTable();

                    if (cftDt.ToDataTableWithType<CFTMeetingModel>().Rows.Count > 0)
                    {
                        dtTemp = cftDt.ToDataTableWithType<CFTMeetingModel>().Copy();
                        dtTemp.Columns.Add("Part_No");
                        for (int i = 0; i < dtTemp.Rows.Count; i++)
                        {
                            dtTemp.Rows[i]["Part_No"] = CFTMeetingModel.PartNo;
                        }
                        dsCFT.Tables.Add(dtTemp);
                        dtTemp.Columns.Remove("Part_No");
                    }

                    if (CFTMeetingModel.DtMidGrid.ToTable().Rows.Count > 0)
                    {
                        CFTMeetingModel.DtMidGrid.Table.Columns.Add("Part_No");
                        for (int i = 0; i < CFTMeetingModel.DtMidGrid.Table.Rows.Count; i++)
                        {
                            CFTMeetingModel.DtMidGrid.Table.Rows[i]["Part_No"] = CFTMeetingModel.PartNo;
                        }
                        dsCFT.Tables.Add(CFTMeetingModel.DtMidGrid.ToTable());
                        CFTMeetingModel.DtMidGrid.Table.Columns.Remove("Part_No");
                    }
                    if (CFTMeetingModel.DtSpecialCharacteristics.IsNotNullOrEmpty())
                    {
                        CFTMeetingModel.DtSpecialCharacteristics.Table.Columns.Add("Part_No");
                        for (int i = 0; i < CFTMeetingModel.DtSpecialCharacteristics.Table.Rows.Count; i++)
                        {
                            CFTMeetingModel.DtSpecialCharacteristics.Table.Rows[i]["Part_No"] = CFTMeetingModel.PartNo;
                        }
                        dsCFT.Tables.Add(CFTMeetingModel.DtSpecialCharacteristics.ToTable());
                        CFTMeetingModel.DtSpecialCharacteristics.Table.Columns.Remove("Part_No");
                    }
                    if (CFTMeetingModel.DtTimigChart.IsNotNullOrEmpty())
                    {
                        CFTMeetingModel.DtTimigChart.Table.Columns.Add("Part_No");
                        for (int i = 0; i < CFTMeetingModel.DtTimigChart.Table.Rows.Count; i++)
                        {
                            CFTMeetingModel.DtTimigChart.Table.Rows[i]["Part_No"] = CFTMeetingModel.PartNo;
                        }
                        dsCFT.Tables.Add(CFTMeetingModel.DtTimigChart.ToTable());
                        CFTMeetingModel.DtTimigChart.Table.Columns.Remove("Part_No");
                    }
                    if (_bll.ExihibitNoDetails().IsNotNullOrEmpty())
                    {
                        dsCFT.Tables.Add(_bll.ExihibitNoDetails());

                    }

                    //foreach (DataTable dt in dsCFT.Tables)
                    //{
                    //    dt.Rows.Clear();
                    //    dt.AcceptChanges();
                    //}
                    //dsCFT.WriteXmlSchema("E:\\" + dsCFT.DataSetName + ".xml");

                    frmReportViewer reportViewer = new frmReportViewer(dsCFT, "CFTMeet");
                    ReportDocument myReportDocument = new ReportDocument();
                    myReportDocument.PrintOptions.PaperSize = PaperSize.PaperA4;                    
                    Progress.End();
                    if (!reportViewer.ReadyToShowReport) return;
                    reportViewer.ShowDialog();

                }
                catch (Exception ex)
                {
                    Progress.End();
                    ex.LogException();
                }
            }
            Progress.End();
            FocusButton = true;
        }

        // Key Ctm1
        private readonly ICommand selectChangeComboCommandCtm1;
        public ICommand SelectChangeComboCommandCtm1 { get { return this.selectChangeComboCommandCtm1; } }
        private void SelectDataRowCtm1()
        {
            if (this.SelectedRowCtm1 != null)
            {
                CFTMeetingModel.Ctm1 = SelectedRowCtm1["core_team_member1"].ToString();
            }
            else
            {
                CFTMeetingModel.Ctm1 = "";
            }
        }

        private DataRowView _selectedRowCtm1;
        public DataRowView SelectedRowCtm1
        {
            get
            {
                return _selectedRowCtm1;
            }
            set
            {
                _selectedRowCtm1 = value;
            }
        }

        //End Key Ctm1

        // Key Ctm2
        private readonly ICommand selectChangeComboCommandCtm2;
        public ICommand SelectChangeComboCommandCtm2 { get { return this.selectChangeComboCommandCtm2; } }
        private void SelectDataRowCtm2()
        {
            if (this.SelectedRowCtm2 != null)
            {
                CFTMeetingModel.Ctm2 = SelectedRowCtm2["core_team_member2"].ToString();
            }
            else
            {
                CFTMeetingModel.Ctm2 = "";
            }
        }

        private DataRowView _selectedRowCtm2;
        public DataRowView SelectedRowCtm2
        {
            get
            {
                return _selectedRowCtm2;
            }
            set
            {
                _selectedRowCtm2 = value;
            }
        }

        //End Key Ctm2

        // Key Ctm3
        private readonly ICommand selectChangeComboCommandCtm3;
        public ICommand SelectChangeComboCommandCtm3 { get { return this.selectChangeComboCommandCtm3; } }
        private void SelectDataRowCtm3()
        {
            if (this.SelectedRowCtm3 != null)
            {
                CFTMeetingModel.Ctm3 = SelectedRowCtm3["core_team_member3"].ToString();
            }
            else
            {
                CFTMeetingModel.Ctm3 = "";
            }
        }

        private DataRowView _selectedRowCtm3;
        public DataRowView SelectedRowCtm3
        {
            get
            {
                return _selectedRowCtm3;
            }
            set
            {
                _selectedRowCtm3 = value;
            }
        }

        //End Key Ctm3

        // Key Ctm4
        private readonly ICommand selectChangeComboCommandCtm4;
        public ICommand SelectChangeComboCommandCtm4 { get { return this.selectChangeComboCommandCtm4; } }
        private void SelectDataRowCtm4()
        {
            if (this.SelectedRowCtm4 != null)
            {
                CFTMeetingModel.Ctm4 = SelectedRowCtm4["core_team_member4"].ToString();
            }
            else
            {
                CFTMeetingModel.Ctm4 = "";
            }
        }

        private DataRowView _selectedRowCtm4;
        public DataRowView SelectedRowCtm4
        {
            get
            {
                return _selectedRowCtm4;
            }
            set
            {
                _selectedRowCtm4 = value;
            }
        }

        //End Key Ctm4

        // Key Ctm5
        private readonly ICommand selectChangeComboCommandCtm5;
        public ICommand SelectChangeComboCommandCtm5 { get { return this.selectChangeComboCommandCtm5; } }
        private void SelectDataRowCtm5()
        {
            if (this.SelectedRowCtm5 != null)
            {
                CFTMeetingModel.Ctm5 = SelectedRowCtm5["core_team_member5"].ToString();
            }
            else
            {
                CFTMeetingModel.Ctm5 = "";
            }
        }

        private DataRowView _selectedRowCtm5;
        public DataRowView SelectedRowCtm5
        {
            get
            {
                return _selectedRowCtm5;
            }
            set
            {
                _selectedRowCtm5 = value;
            }
        }

        //End Key Ctm5

        // Key Ctm6
        private readonly ICommand selectChangeComboCommandCtm6;
        public ICommand SelectChangeComboCommandCtm6 { get { return this.selectChangeComboCommandCtm6; } }
        private void SelectDataRowCtm6()
        {
            if (this.SelectedRowCtm6 != null)
            {
                CFTMeetingModel.Ctm6 = SelectedRowCtm6["core_team_member6"].ToString();
            }
            else
            {
                CFTMeetingModel.Ctm6 = "";
            }
        }

        private DataRowView _selectedRowCtm6;
        public DataRowView SelectedRowCtm6
        {
            get
            {
                return _selectedRowCtm6;
            }
            set
            {
                _selectedRowCtm6 = value;
            }
        }

        //End Key Ctm6

        // Key Ctm7
        private readonly ICommand selectChangeComboCommandCtm7;
        public ICommand SelectChangeComboCommandCtm7 { get { return this.selectChangeComboCommandCtm7; } }
        private void SelectDataRowCtm7()
        {
            if (this.SelectedRowCtm7 != null)
            {
                CFTMeetingModel.Ctm7 = SelectedRowCtm7["core_team_member7"].ToString();
            }
            else
            {
                CFTMeetingModel.Ctm7 = "";
            }
        }

        private DataRowView _selectedRowCtm7;
        public DataRowView SelectedRowCtm7
        {
            get
            {
                return _selectedRowCtm7;
            }
            set
            {
                _selectedRowCtm7 = value;
            }
        }

        //End Key Ctm7

        private void LoadTimeChart()
        {
            DataTable dtTime = new DataTable();

            CFTMeetingModel.DtTimigChart = null;
            dtTime.Columns.Add("Phase");
            dtTime.Columns.Add("Activity");
            DataColumn planned_Start_Date, planned_End_Date, revised_Date, actual_Finish_Date;
            planned_Start_Date = new DataColumn("Planned_Start_Date", typeof(DateTime));
            planned_Start_Date.AllowDBNull = true;

            planned_End_Date = new DataColumn("Planned_End_Date", typeof(DateTime));
            planned_End_Date.AllowDBNull = true;

            revised_Date = new DataColumn("Revised_Date", typeof(DateTime));
            revised_Date.AllowDBNull = true;

            actual_Finish_Date = new DataColumn("Actual_Finish_Date", typeof(DateTime));
            actual_Finish_Date.AllowDBNull = true;

            dtTime.Columns.Add(planned_Start_Date);
            dtTime.Columns.Add(planned_End_Date);
            dtTime.Columns.Add(revised_Date);
            dtTime.Columns.Add(actual_Finish_Date);

            //dtTime.Columns.Add("Planned_Start_Date");
            //dtTime.Columns.Add("Planned_End_Date");
            //dtTime.Columns.Add("Revised_Date");
            //dtTime.Columns.Add("Actual_Finish_Date");

            CFTMeetingModel.DtTimigChart = dtTime.DefaultView;

            for (int i = 0; i < 18; i++)
            {
                CFTMeetingModel.DtTimigChart.AddNew();
            }
            //Phase I
            CFTMeetingModel.DtTimigChart[0][0] = "I";
            CFTMeetingModel.DtTimigChart[0][1] = "Plan & Define Program.";
            //CFTMeetingModel.DtTimigChart[0][2] = "CFT Meeting date as above";

            //Phase II
            CFTMeetingModel.DtTimigChart[1][0] = "II";
            CFTMeetingModel.DtTimigChart[1][1] = "Product Design & devpt.";
            CFTMeetingModel.DtTimigChart[2][1] = "TFC";
            //CFTMeetingModel.DtTimigChart[1][2] = "CFT Meeting date as above";

            //Phase III
            CFTMeetingModel.DtTimigChart[3][0] = "III";
            CFTMeetingModel.DtTimigChart[3][1] = "Process Design & Devpt.";
            CFTMeetingModel.DtTimigChart[4][1] = "Document Release";
            CFTMeetingModel.DtTimigChart[5][1] = "B/Ogauge (if any)";
            CFTMeetingModel.DtTimigChart[6][1] = "Tool Manufacturing";
            CFTMeetingModel.DtTimigChart[7][1] = "B/OTools (if any)";
            CFTMeetingModel.DtTimigChart[8][1] = "RM";
            CFTMeetingModel.DtTimigChart[9][1] = "Forging";
            CFTMeetingModel.DtTimigChart[10][1] = "Secondary";
            CFTMeetingModel.DtTimigChart[11][1] = "Heat/Surface Treatment";

            //Phase IV
            CFTMeetingModel.DtTimigChart[12][0] = "IV";
            CFTMeetingModel.DtTimigChart[12][1] = "Product & Process Validation";
            CFTMeetingModel.DtTimigChart[13][1] = "PPAP";

            //Phase V
            CFTMeetingModel.DtTimigChart[14][0] = "V";
            CFTMeetingModel.DtTimigChart[14][1] = "Customer Feedback";
            CFTMeetingModel.DtTimigChart[15][1] = "Corrective Action";

            //Phase VI
            CFTMeetingModel.DtTimigChart[16][0] = "VI";
            CFTMeetingModel.DtTimigChart[16][1] = "Production";

        }
        private ObservableCollection<DropdownColumns> _dropDownItemsCTM1;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM1
        {
            get
            {
                return _dropDownItemsCTM1;
            }
            set
            {
                this._dropDownItemsCTM1 = value;
                NotifyPropertyChanged("DropDownItemsCTM1");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsCTM2;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM2
        {
            get
            {
                return _dropDownItemsCTM2;
            }
            set
            {
                this._dropDownItemsCTM2 = value;
                NotifyPropertyChanged("DropDownItemsCTM2");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsCTM3;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM3
        {
            get
            {
                return _dropDownItemsCTM3;
            }
            set
            {
                this._dropDownItemsCTM3 = value;
                NotifyPropertyChanged("DropDownItemsCTM3");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsCTM4;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM4
        {
            get
            {
                return _dropDownItemsCTM4;
            }
            set
            {
                this._dropDownItemsCTM4 = value;
                NotifyPropertyChanged("DropDownItemsCTM4");
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsCTM5;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM5
        {
            get
            {
                return _dropDownItemsCTM5;
            }
            set
            {
                this._dropDownItemsCTM5 = value;
                NotifyPropertyChanged("DropDownItemsCTM5");
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsCTM6;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM6
        {
            get
            {
                return _dropDownItemsCTM6;
            }
            set
            {
                this._dropDownItemsCTM6 = value;
                NotifyPropertyChanged("DropDownItemsCTM6");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsCTM7;
        public ObservableCollection<DropdownColumns> DropDownItemsCTM7
        {
            get
            {
                return _dropDownItemsCTM7;
            }
            set
            {
                this._dropDownItemsCTM7 = value;
                NotifyPropertyChanged("DropDownItemsCTM7");
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
        private void SetdropDownItems()
        {
            try
            {
                DropDownItemsPart = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "PART NO", ColumnWidth = "22*" },
                            new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "DESCRIPTION", ColumnWidth = "78*" }
                        };

                DropDownItemsCTM1 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER1", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM2 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER2", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM3 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER3", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM4 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER4", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM5 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER5", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM6 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER6", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };
                DropDownItemsCTM7 = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CORE_TEAM_MEMBER7", ColumnDesc = "Member", ColumnWidth = "1*" }
                        };

            }
            catch (Exception ex)
            {
                throw ex.LogException();
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
        public ICommand SelectChangeComboCommandPartNo { get { return selectChangeComboCommandPartNo; } }
        private void SelectDataRowPart()
        {
            IsEnabledPrint = false;
            string statusMsg = "";
            CFTMeetingModel.Ctm1 = "";
            CFTMeetingModel.Ctm2 = "";
            CFTMeetingModel.Ctm3 = "";
            CFTMeetingModel.Ctm4 = "";
            CFTMeetingModel.Ctm5 = "";
            CFTMeetingModel.Ctm6 = "";
            CFTMeetingModel.Ctm7 = "";
            CFTMeetingModel.Application = "";
            CFTMeetingModel.IsSafetyPart = "";

            CFTMeetingModel.CustPartName = "";

            CFTMeetingModel.PG = "";
            CFTMeetingModel.PartDesc = "";
            CFTMeetingModel.RMSpec = "";
            CFTMeetingModel.PackingSpec = "";
            CFTMeetingModel.CheesWt = "";
            CFTMeetingModel.FinishWt = "";
            CFTMeetingModel.DtSpecialCharacteristics = null;
            CFTMeetingModel.DtMidGrid = null;
            CFTMeetingModel.DtTimigChart = null;
            CFTMeetingModel.PPAPSampleQty = "300";
            LoadTimeChart();
            if (this.SelectedRowPart != null)
            {
                CFTMeetingModel.PartDesc = SelectedRowPart["PART_DESC"].ToValueAsString();
                _bll.GetRecordsByPartNo(CFTMeetingModel, ref statusMsg);
                NotifyPropertyChanged("CFTMeetingModel");
                if (statusMsg.IsNotNullOrEmpty()) ShowInformationMessage(statusMsg);
            }
        }
        public void CheckAll(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private CFTMeetingModel _cftMeetingModel;
        public CFTMeetingModel CFTMeetingModel
        {
            get { return _cftMeetingModel; }
            set
            {
                _cftMeetingModel = value;
                NotifyPropertyChanged("CFTMeetingModel");
            }
        }
    }

}
