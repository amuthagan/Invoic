using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.UserControls;
using ProcessDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ProcessDesigner
{

    /// <summary>
    /// Interaction logic for frmDevelopmentReport.xaml
    /// </summary>
    public partial class frmDevelopmentReport : UserControl
    {
        public frmDevelopmentReport(UserInformation userInformation, WPF.MDI.MdiChild me)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;
            InitializeComponent();
            DevReportViewModel fm = new DevReportViewModel(userInformation, me);
            fm.SsAssumtions = ssAssumtions;
            fm.SsDevReportLog = ssDevReportLog;
            fm.SsShortClosure = ssShortClosure;

            this.DataContext = fm;
            me.Closing += fm.CloseMethod;
            Progress.End();
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => me.Close());
        }
        public frmDevelopmentReport(UserInformation userInformation, WPF.MDI.MdiChild me, string partNo)
        {
            Progress.ProcessingText = PDMsg.Load;
            Progress.Start();
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;
            InitializeComponent();
            DevReportViewModel fm = new DevReportViewModel(userInformation, me, partNo);
            fm.SsAssumtions = ssAssumtions;
            fm.SsDevReportLog = ssDevReportLog;
            fm.SsShortClosure = ssShortClosure;
            this.DataContext = fm;
            me.Closing += fm.CloseMethod;
            Progress.End();
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => me.Close());
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((TextBox)cmbPartNo.FindName("txtCombobox")).Focus();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void ssAssumtions_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = ssAssumtions.CurrentCell.Column.DisplayIndex;
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
                {
                    e.Handled = true;
                    
                    if (columnDisplayIndex == 2)
                    {
                        ssAssumtions.SelectedIndex = ssAssumtions.SelectedIndex - 1;
                        columnDisplayIndex = ssAssumtions.Columns.Count - 1;
                    }
                    else
                    {
                        if (columnDisplayIndex == 1)
                        {
                            columnDisplayIndex = 0;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex - 1;
                        }
                    }
                    System.Windows.Controls.DataGridColumn nextColumn = ssAssumtions.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    ssAssumtions.CurrentCell = new System.Windows.Controls.DataGridCellInfo(ssAssumtions.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    ssAssumtions.BeginEdit();
                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                    if (e.Key == Key.Tab)
                    {

                        if (columnDisplayIndex == 6)
                        {
                            columnDisplayIndex = 2;
                            ssAssumtions.SelectedIndex = ssAssumtions.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = ssAssumtions.SelectedIndex;

                        System.Windows.Controls.DataGridColumn nextColumn = ssAssumtions.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        ssAssumtions.CurrentCell = new System.Windows.Controls.DataGridCellInfo(ssAssumtions.SelectedItem, nextColumn);
                        ssAssumtions.ScrollIntoView(ssAssumtions.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        ssAssumtions.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ssDevReportLog_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                int columnDisplayIndex = ssDevReportLog.CurrentCell.Column.DisplayIndex;
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
                {
                    e.Handled = true;
                    
                    if (columnDisplayIndex == 2)
                    {

                        ssDevReportLog.SelectedIndex = ssDevReportLog.SelectedIndex - 1;
                        columnDisplayIndex = ssDevReportLog.Columns.Count - 1;
                    }
                    else
                    {
                        if (columnDisplayIndex == 1)
                        {
                            columnDisplayIndex = 0;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex - 1;
                        }
                    }
                    Microsoft.Windows.Controls.DataGridColumn nextColumn = ssDevReportLog.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    ssDevReportLog.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(ssDevReportLog.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    ssDevReportLog.BeginEdit();
                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                    if (e.Key == Key.Tab)
                    {

                        if (columnDisplayIndex == 7)
                        {
                            columnDisplayIndex = 2;
                            ssDevReportLog.SelectedIndex = ssDevReportLog.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = ssDevReportLog.SelectedIndex;

                        Microsoft.Windows.Controls.DataGridColumn nextColumn = ssDevReportLog.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        ssDevReportLog.CurrentCell = new Microsoft.Windows.Controls.DataGridCellInfo(ssDevReportLog.SelectedItem, nextColumn);
                        ssDevReportLog.ScrollIntoView(ssDevReportLog.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        ssDevReportLog.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ssShortClosure_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int columnDisplayIndex = ssShortClosure.CurrentCell.Column.DisplayIndex;
                //if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift) || e.Key == Key.Left)
                if ((e.Key == Key.Tab && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift))
                {
                    e.Handled = true;
                    
                    if (columnDisplayIndex == 2)
                    {

                        ssShortClosure.SelectedIndex = ssShortClosure.SelectedIndex - 1;
                        columnDisplayIndex = ssShortClosure.Columns.Count - 1;
                    }
                    else
                    {
                        if (columnDisplayIndex == 1)
                        {
                            columnDisplayIndex = 0;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex - 1;
                        }
                    }
                    System.Windows.Controls.DataGridColumn nextColumn = ssShortClosure.ColumnFromDisplayIndex(columnDisplayIndex);

                    // now telling the grid, that we handled the key down event
                    //e.Handled = true;

                    // setting the current cell (selected, focused)
                    ssShortClosure.CurrentCell = new System.Windows.Controls.DataGridCellInfo(ssShortClosure.SelectedItem, nextColumn);

                    // tell the grid to initialize edit mode for the current cell
                    ssShortClosure.BeginEdit();
                }
                else
                {
                    //if (e.Key == Key.Tab || e.Key == Key.Right)
                    if (e.Key == Key.Tab)
                    {

                        if (columnDisplayIndex == 8)
                        {
                            columnDisplayIndex = 2;
                            ssShortClosure.SelectedIndex = ssShortClosure.SelectedIndex + 1;
                        }
                        else
                        {
                            columnDisplayIndex = columnDisplayIndex + 1;
                        }
                        int selectedIndex = 0;
                        selectedIndex = ssShortClosure.SelectedIndex;

                        System.Windows.Controls.DataGridColumn nextColumn = ssShortClosure.ColumnFromDisplayIndex(columnDisplayIndex);
                        // now telling the grid, that we handled the key down event
                        e.Handled = true;
                        // setting the current cell (selected, focused)
                        ssShortClosure.CurrentCell = new System.Windows.Controls.DataGridCellInfo(ssShortClosure.SelectedItem, nextColumn);
                        ssShortClosure.ScrollIntoView(ssShortClosure.CurrentCell);
                        // tell the grid to initialize edit mode for the current cell
                        ssShortClosure.BeginEdit();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        //private void AddNew_Click(object sender, RoutedEventArgs e)
        //{
        //    Control cntrl = (Control)sender;
        //    string s = sender.GetType().ToString();

        //    //Button button = sender as Button;
        //    //if (button == null)
        //    //{
        //    //    return;
        //    //}

        //    MessageBox.Show(e.Source.ToString());
        //}

        //private void Edit_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}

        //private void Save_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}

        //private void Print_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}

        //private void Close_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}

        //private void Search_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}

        //private void Undo_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}

        //private void ShowRelated_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}

        //private void chkComplaint_Checked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        CheckBox cbComplaint = sender as CheckBox;
        //        if (cbComplaint == null) return;

        //        if (cbComplaint.IsChecked == true)
        //            cbComplaint.Content = "Yes";
        //        else
        //            cbComplaint.Content = "No";
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}

        //private void WindowLoaded(object sender, RoutedEventArgs e)
        //{

        //    try
        //    {
        //        DataTable dtPartNo = devRpt.GetAllPartCodeAndDescription();
        //        ltbPartNo.DataSource = dtPartNo.DefaultView;
        //        ltbPartNo.SelectedValuePath = "PART_NO";
        //        //ltbPartNo.SelectedTextPath = "PART_DESC";

        //        //DataTable dtRunNo = devRpt.GetRunningNumber(ltbPartNo.SelectedValue);
        //        //ltbRunNo.DataSource = dtRunNo;
        //        //ltbRunNo.SelectedValuePath = "PART_NO";
        //        //ltbRunNo.SelectedTextPath = "PART_DESC";

        //        RetrieveDevReport();
        //        AssignGridManager();

        //        if (!String.IsNullOrEmpty(varDevPartNo))
        //        {
        //            frmEdit();
        //            //ltbPartNo.text = varDevPartNo
        //            //lblPartDescription = varDevPartDesc
        //            RetrieveDevReport();
        //            AssignGridManager();
        //        }
        //        else
        //        {
        //            //frmEdit();
        //            //'frmAdd
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }

        //}

        //private void frmEdit()
        //{
        //    //    //    this.EnableAllControl<Button>(Edit);
        //    //    //    this.UnlockAllControl<TextBox>();
        //    //    //    this.ClearAllControl<TextBox>();

        //    //    //    //tbrAction.Buttons(1).Enabled = True
        //    //    //    //tbrAction.Buttons(4).Enabled = True
        //    //    //    //tbrAction.Buttons(2).Enabled = False
        //    //    //    //ltbPartNo.Locked = False
        //    //    //    //ltbRunNo.ButtonEnabled = True

        //    //    //    //ltbPartNo.text = ""
        //    //    //    //lblPartDescription = ""
        //    //    //    //ltbRunNo.text = ""
        //    //    //    //varMode = "Edit"
        //    //    //    //mdiProcessDesigner.sbrDesigner.Panels(4).text = "Edit"

        //}

        ////private void frmAdd()
        ////{
        ////    //    this.EnableAllControl<Button>(AddNew, Print);
        ////    //    this.UnlockAllControl<TextBox>(txtCustComplaint);
        ////    //    varMode = OperationMode.AddNew;
        ////    //    chkComplaint.Content = "Yes / No";

        ////    //    //ltbPartNo.text = ""
        ////    //    //lblPartDescription = ""
        ////    //    //ltbRunNo.text = ""

        ////    //    //tbrAction.Buttons(1).Enabled = False
        ////    //    //tbrAction.Buttons(4).Enabled = False
        ////    //    //tbrAction.Buttons(2).Enabled = True
        ////    //    //tbrAction.Buttons(3).Enabled = True
        ////    //    //ltbPartNo.Locked = False
        ////    //    //ltbRunNo.ButtonEnabled = False
        ////    //    //chkComplaint.Caption = "Yes / No"
        ////    //    //txtCustComplaint.Locked = True
        ////    //    //ssAssumtions.RemoveAll
        ////    //    //ssDevReportvoid.RemoveAll
        ////    //    //ssShortClosure.RemoveAll
        ////    //    frmCancel();
        ////    //    //txtRunDate.text = ""


        ////    //    //'txtRunDate = Format(Date, "dd/mm/yyyy")

        ////    //    //mdiProcessDesigner.sbrDesigner.Panels(4).text = "Add"

        ////}

        ////private void frmCancel()
        ////{
        ////    this.ClearAllControl<TextBox>();
        ////    this.ClearAllControl<CheckBox>();
        ////    //lblTotalDesign = "";
        ////    //lblTotalMfg = "";
        ////}

        ////private void ltbPartNo_AfterSelection()
        ////{
        ////    //If ltbPartNo.text <> "" Then
        ////    //    lblPartDescription = ltbPartNo.Recordset(1).Value
        ////    //    If varMode = "Add" Then
        ////    //        GenerateRunNo
        ////    //        txtRunDate = Format(Date, "dd/mm/yyyy")
        ////    //    End If
        ////    //    RetrieveDevReport
        ////    //    AssignGridManager
        ////    //End If
        ////}

        ////private void ltbPartNo_ButtonClicked() // bool ShowLookup , string msg
        ////{

        ////    //frmCancel();
        ////    //Set ltbPartNo.Recordset = GetRS("select part_no,part_desc from prd_mast ")
        ////    //       ltbPartNo.DisplayField = 0
        ////    //       ltbPartNo.SetColWidth "1500-5000"
        ////    //       lblPartDescription = ""
        ////}

        //private void AssignGridManager()
        //{
        //    try
        //    {
        //        ltbPartNo.SelectedValue = "M29500";
        //        if (ltbPartNo.SelectedValue.IsNotNullOrEmpty())
        //        {
        //            string sPartNumber = ltbPartNo.SelectedValue;
        //            Int16 iRunningNumber;
        //            Int16.TryParse(ltbRunNo.SelectedValue, out iRunningNumber);
        //            DataSet dtDevelopmentReport = devRpt.GetAssumptionsLOGShortClosure(sPartNumber, iRunningNumber);
        //            if (dtDevelopmentReport.IsNotNullOrEmpty())
        //            {
        //                if (dtDevelopmentReport.Tables.Count >= 0 && dtDevelopmentReport.Tables[0].IsNotNullOrEmpty())
        //                {
        //                    if (dtDevelopmentReport.Tables[0].Rows.Count == 0)
        //                    {
        //                        dtDevelopmentReport.Tables[0].Rows.Add();
        //                        dtDevelopmentReport.Tables[0].AcceptChanges();
        //                    }

        //                    ssAssumtions.DataContext = dtDevelopmentReport;
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //    //Set rsDesignAssumptions = New ADODB.Recordset
        //    //    rsDesignAssumptions.CursorLocation = adUseClient

        //    //Sql = "select part_no,dev_run_no,sno,cft_diss,justification,tgr,tgw from " & _
        //    //      "  dev_report_design where part_no ='" & ltbPartNo.text & "' "


        //    //    rsDesignAssumptions.Open Sql, gvarcnn, adOpenKeyset, adLockBatchOptimistic

        //    //    rsDesignAssumptions.Filter = adFilterNone

        //    //    If ltbRunNo.text = "" Then ltbRunNo.text = 1

        //    //    rsDesignAssumptions.Filter = "dev_run_no='" & ltbRunNo.text & "'"

        //    //    Set grdAssumptions.Recordset = rsDesignAssumptions
        //    //    Set grdAssumptions.Grid = ssAssumtions

        //    //    grdAssumptions.ShowFromCol = 2
        //    //    grdAssumptions.AutoNumber = True
        //    //    grdAssumptions.LoadData


        //    //    Set rsDevReportvoid = New ADODB.Recordset
        //    //    rsDevReportvoid.CursorLocation = adUseClient

        //    //Sql = "select part_no,dev_run_no,stage_no,problem_faced,counter_measure," & _
        //    //      " rework_tool_design,rework_tool_mfg,remarks from dev_report_void where " & _
        //    //      " part_no='" & ltbPartNo.text & "'"

        //    //    rsDevReportvoid.Open Sql, gvarcnn, adOpenKeyset, adLockBatchOptimistic

        //    //    rsDevReportvoid.Filter = adFilterNone
        //    //    rsDevReportvoid.Filter = "dev_run_no='" & ltbRunNo.text & "'"

        //    //    Set grdDevReportvoid.Grid = ssDevReportvoid
        //    //    Set grdDevReportvoid.Recordset = rsDevReportvoid

        //    //        grdDevReportvoid.ShowFromCol = 2
        //    //        grdDevReportvoid.AutoNumber = True
        //    //        grdDevReportvoid.LoadData


        //    //Set rsShortClosure = New ADODB.Recordset
        //    //    rsShortClosure.CursorLocation = adUseClient

        //    //Sql = "select part_no,run_no,sno,reason,why,short_action,short_action_date," & _
        //    //      " long_action,long_action_date,trial_date from dev_report_short_closure " & _
        //    //      " where part_no='" & ltbPartNo.text & "'"

        //    //    rsShortClosure.Open Sql, gvarcnn, adOpenKeyset, adLockBatchOptimistic

        //    //    rsShortClosure.Filter = adFilterNone

        //    //    rsShortClosure.Filter = "run_no= '" & ltbRunNo.text & "'"

        //    //    Set grdShortClosure.Grid = ssShortClosure
        //    //    Set grdShortClosure.Recordset = rsShortClosure


        //    //    grdShortClosure.ShowFromCol = 2
        //    //    grdShortClosure.AutoNumber = True
        //    //    grdShortClosure.LoadData

        //    //    ReworkToolCalculation

        //}

        //private void RetrieveDevReport()
        //{
        //    try
        //    {
        //        ltbPartNo.SelectedValue = "001107352";
        //        if (ltbPartNo.SelectedValue.IsNotNullOrEmpty())
        //        {
        //            string sPartNumber = ltbPartNo.SelectedValue;

        //            Int16 iRunningNumber;
        //            Int16.TryParse(ltbRunNo.SelectedValue, out iRunningNumber);

        //            DataSet dtDevelopmentReport = devRpt.RetrieveDevReport(sPartNumber, iRunningNumber);
        //            if (dtDevelopmentReport.IsNotNullOrEmpty() && dtDevelopmentReport.Tables[0].Rows.Count > 0)
        //            {
        //                DataRow rsDevReportMain = dtDevelopmentReport.Tables[0].Rows[0];

        //                ltbRunNo.SelectedValue = rsDevReportMain["dev_run_no"].ToValueAsString();
        //                txtCustComplaint.Text = rsDevReportMain["cust_comp_desc"].ToValueAsString();
        //                //chkComplaint.Content = rsDevReportMain["cust_comp"].ToValueAsString().IsNotNullOrEmpty() ? rsDevReportMain["cust_comp"].ToValueAsString() : "";

        //                chkComplaint.IsChecked = rsDevReportMain["cust_comp"].ToBooleanAsString();
        //                txtCustComplaint.IsReadOnly = (bool)!chkComplaint.IsChecked;
        //                txtRunDate.Text = rsDevReportMain["report_date"].ToDateAsString();
        //                txtDndRep.Text = rsDevReportMain["forge_ddrep"].ToValueAsString();
        //                txtZapRep.Text = rsDevReportMain["forge_zaprep"].ToValueAsString();
        //                txtRecord.Text = rsDevReportMain["Record"].ToValueAsString();
        //                //txtForgeShift.Text = rsDevReportMain["no_forg_sht"].ToValueAsString();

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }
        //}


        ////private void ltbPartNo_KeyPress()//KeyAscii As Integer
        ////{
        ////    //KeyAscii = Asc(UCase(Chr(KeyAscii)))
        ////    //If KeyAscii = 13 Then


        ////    //    If ltbPartNo.text <> "" Then



        ////    //        If varMode = "Add" Then
        ////    //            GenerateRunNo
        ////    //            txtRunDate = Format(Date, "dd/mm/yyyy")
        ////    //        End If
        ////    //        RetrieveDevReport
        ////    //        AssignGridManager
        ////    //    End If
        ////    //    SendKeys "{tab}"
        ////    //End If

        ////}

        ////private void ltbRunNo_AfterSelection()
        ////{

        ////    //If ltbPartNo.text <> "" And ltbRunNo.text <> "" Then

        ////    //        rsDevReportMain.Filter = adFilterNone
        ////    //        rsDevReportMain.Filter = " dev_run_no =" & ltbRunNo.text

        ////    //     If rsDevReportMain.RecordCount <> 0 Then
        ////    //        ltbRunNo.text = rsDevReportMain!dev_run_no
        ////    //        txtCustComplaint = IIf(IsNull(rsDevReportMain!cust_comp_desc), "", rsDevReportMain!cust_comp_desc)
        ////    //        chkComplaint.Caption = IIf(IsNull(rsDevReportMain!cust_comp), "", rsDevReportMain!cust_comp)
        ////    //        chkComplaint.Value = IIf(chkComplaint.Caption = "NO" Or IsNull(rsDevReportMain!cust_comp), 0, 1)
        ////    //        If chkComplaint.Caption = "NO" Then txtCustComplaint.Locked = True Else txtCustComplaint.Locked = False
        ////    //        txtRunDate = IIf(IsNull(rsDevReportMain!report_date), "", rsDevReportMain!report_date)
        ////    //        txtDndRep = IIf(IsNull(rsDevReportMain!forge_ddrep), "", rsDevReportMain!forge_ddrep)
        ////    //        txtZapRep = IIf(IsNull(rsDevReportMain!forge_zaprep), "", rsDevReportMain!forge_zaprep)
        ////    //        txtRecord = IIf(IsNull(rsDevReportMain!Record), "", rsDevReportMain!Record)
        ////    //    End If

        ////    //        rsDesignAssumptions.Filter = adFilterNone
        ////    //        rsDesignAssumptions.Filter = " dev_run_no = " & ltbRunNo.text

        ////    //        Set grdAssumptions.Recordset = rsDesignAssumptions

        ////    //        grdAssumptions.ShowFromCol = 2
        ////    //        grdAssumptions.AutoNumber = True
        ////    //        grdAssumptions.LoadData

        ////    //        rsDevReportvoid.Filter = adFilterNone
        ////    //        rsDevReportvoid.Filter = "dev_run_no=" & ltbRunNo.text
        ////    //        Set grdDevReportvoid.Grid = ssDevReportvoid
        ////    //        Set grdDevReportvoid.Recordset = rsDevReportvoid

        ////    //        grdDevReportvoid.ShowFromCol = 2
        ////    //        grdDevReportvoid.AutoNumber = True
        ////    //        grdDevReportvoid.LoadData

        ////    //        rsShortClosure.Filter = adFilterNone
        ////    //        rsShortClosure.Filter = "run_no= " & ltbRunNo.text

        ////    //        Set grdShortClosure.Grid = ssShortClosure
        ////    //        Set grdShortClosure.Recordset = rsShortClosure

        ////    //        grdShortClosure.ShowFromCol = 2
        ////    //        grdShortClosure.AutoNumber = True
        ////    //        grdShortClosure.LoadData

        ////    //        ReworkToolCalculation
        ////    //End If

        ////}

        ////private void ltbRunNo_ButtonClicked()// bool ShowLookup , string msg
        ////{

        ////    //If Trim(ltbPartNo.text) <> "" Then
        ////    //    Set ltbRunNo.Recordset = GetRS("select dev_run_no from dev_report_main where part_no='" & ltbPartNo.text & "'")
        ////    //Else
        ////    //    MsgBox "Part No should selected first", vbInformation, "SmartPD"
        ////    //End If

        ////}

        ////private void GenerateRunNo()
        ////{
        ////    //Dim rsMaxRunNo As ADODB.Recordset

        ////    //        Set rsMaxRunNo = New ADODB.Recordset
        ////    //            rsMaxRunNo.CursorLocation = adUseClient

        ////    //        Sql = " select max(dev_run_no) as DevRunNo from dev_report_main where part_no='" & ltbPartNo.text & "'"

        ////    //        rsMaxRunNo.Open Sql, gvarcnn, adOpenForwardOnly, adLockReadOnly

        ////    //        If rsMaxRunNo.RecordCount <> 0 Then
        ////    //            If IsNull(rsMaxRunNo.Fields(0).Value) Then
        ////    //                ltbRunNo.text = 1
        ////    //            Else
        ////    //                ltbRunNo.text = rsMaxRunNo!DevRunNo + 1
        ////    //            End If
        ////    //        End If

        ////}

        ////private void ltbRunNo_LostFocus()
        ////{
        ////    //If ltbPartNo.text <> "" And ltbRunNo.text <> "" Then

        ////    //           rsDevReportMain.Filter = adFilterNone
        ////    //           rsDevReportMain.Filter = " dev_run_no ='" & ltbRunNo.text & "'"

        ////    //        If rsDevReportMain.RecordCount <> 0 Then
        ////    //           ltbRunNo.text = rsDevReportMain!dev_run_no
        ////    //           txtCustComplaint = IIf(IsNull(rsDevReportMain!cust_comp_desc), "", rsDevReportMain!cust_comp_desc)
        ////    //           chkComplaint.Caption = IIf(IsNull(rsDevReportMain!cust_comp), "", rsDevReportMain!cust_comp)
        ////    //           chkComplaint.Value = IIf(chkComplaint.Caption = "NO" Or IsNull(rsDevReportMain!cust_comp), 0, 1)
        ////    //           If chkComplaint.Caption = "NO" Then txtCustComplaint.Locked = True Else txtCustComplaint.Locked = False
        ////    //           txtRunDate = IIf(IsNull(rsDevReportMain!report_date), "", rsDevReportMain!report_date)
        ////    //           txtDndRep = IIf(IsNull(rsDevReportMain!forge_ddrep), "", rsDevReportMain!forge_ddrep)
        ////    //           txtZapRep = IIf(IsNull(rsDevReportMain!forge_zaprep), "", rsDevReportMain!forge_zaprep)
        ////    //           txtRecord = IIf(IsNull(rsDevReportMain!Record), "", rsDevReportMain!Record)
        ////    //       End If

        ////    //           rsDevReportMain.Filter = adFilterNone
        ////    //           rsDevReportMain.Filter = " dev_run_no = '" & ltbRunNo.text & "'"

        ////    //           Set grdAssumptions.Recordset = rsDesignAssumptions

        ////    //           grdAssumptions.ShowFromCol = 2
        ////    //           grdAssumptions.AutoNumber = True
        ////    //           grdAssumptions.LoadData

        ////    //           rsDevReportvoid.Filter = adFilterNone
        ////    //           rsDevReportvoid.Filter = "dev_run_no='" & ltbRunNo.text & "'"

        ////    //           Set grdDevReportvoid.Grid = ssDevReportvoid
        ////    //           Set grdDevReportvoid.Recordset = rsDevReportvoid

        ////    //           grdDevReportvoid.ShowFromCol = 2
        ////    //           grdDevReportvoid.AutoNumber = True
        ////    //           grdDevReportvoid.LoadData

        ////    //           rsShortClosure.Filter = adFilterNone
        ////    //           rsShortClosure.Filter = "run_no= '" & ltbRunNo.text & "'"

        ////    //           Set grdShortClosure.Grid = ssShortClosure
        ////    //           Set grdShortClosure.Recordset = rsShortClosure

        ////    //           grdShortClosure.ShowFromCol = 2
        ////    //           grdShortClosure.AutoNumber = True
        ////    //           grdShortClosure.LoadData

        ////    //           ReworkToolCalculation
        ////    //   End If

        ////}

        ////private void ssDevReportvoid_KeyDown()//int KeyCode, int Shift
        ////{

        ////    //'If KeyCode = 9 Then SendKeys "{tab}": }

        ////    //Select Case ssDevReportvoid.Col


        ////    //        Case 3
        ////    //            'If KeyCode = 9 Then SendKeys "{tab}": }
        ////    //            If KeyCode <> 9 Then: fnMdAllowNumber KeyCode
        ////    //            'If Len(Trim(ssDevReportvoid.Columns(3).text)) >= 2 Then KeyAscii = 0: SendKeys "{tab}"
        ////    //        Case 4
        ////    //            If KeyCode <> 9 Then: fnMdAllowNumber KeyCode
        ////    //            'If Len(Trim(ssDevReportvoid.Columns(4).text)) >= 2 Then KeyAscii = 0: SendKeys "{tab}"
        ////    //        End Select


        ////}

        ////private void ssDevReportvoid_RowColChange() //ByVal LastRow As Variant, ByVal LastCol As Integer
        ////{
        ////    //If LastCol = 3 Or LastCol = 4 Then
        ////    //    If Len(ssDevReportvoid.Columns(LastCol).text) <> 0 Then
        ////    //        If Not IsNumeric(ssDevReportvoid.Columns(LastCol).text) Then
        ////    //            MsgBox "Enter a Number", vbInformation, App.ProductName
        ////    //            ssDevReportvoid.SetFocus
        ////    //            ssDevReportvoid.Columns(LastCol).text = ""
        ////    //        Else
        ////    //            If Val(ssDevReportvoid.Columns(LastCol).text) >= 100 Then
        ////    //                MsgBox "The value should be 1 to 99", vbInformation, App.ProductName
        ////    //                ssDevReportvoid.Columns(LastCol).text = ""
        ////    //                ssDevReportvoid.SetFocus
        ////    //            End If
        ////    //        End If
        ////    //    End If
        ////    //End If
        ////}

        ////private void ssShortClosure_BeforeColUpdate()//ByVal ColIndex As Integer, ByVal oldvalue As Variant, Cancel As Integer
        ////{
        ////    //'    Select Case ColIndex
        ////    //'
        ////    //'            Case 4
        ////    //'                    If Not IsDate(ssShortClosure.Columns(4).text) Then
        ////    //'                        MsgBox "Enter Valid Short Action Date", vbInformation, "SmartPD"
        ////    //'                        ssShortClosure.col = ColIndex
        ////    //'                    Else
        ////    //'                        SendKeys "{tab}"
        ////    //'                    End If
        ////    //'            Case 6
        ////    //'                    If Not IsDate(ssShortClosure.Columns(6).text) Then
        ////    //'                        MsgBox "Enter Valid Long Action Date", vbInformation, "SmartPD"
        ////    //'                        Cancel = True
        ////    //'                    Else
        ////    //'                        SendKeys "{tab}"
        ////    //'                    End If
        ////    //'            Case 7
        ////    //'                    If Not IsDate(ssShortClosure.Columns(7).text) Then
        ////    //'                        MsgBox "Enter Valid Trial Date", vbInformation, "SmartPD"
        ////    //'                        Cancel = True
        ////    //'                    Else
        ////    //'                        SendKeys "{tab}"
        ////    //'                    End If
        ////    //'    End Select

        ////}

        ////private void ssShortClosure_RowColChange()//ByVal LastRow As Variant, ByVal LastCol As Integer
        ////{
        ////    //If LastCol = 4 Or LastCol = 6 Or LastCol = 7 Then
        ////    //    If Len(ssShortClosure.Columns(LastCol).text) <> 0 Then
        ////    //    If Not IsDate(ssShortClosure.Columns(LastCol).text) Then
        ////    //        ssShortClosure.Columns(LastCol).text = ""
        ////    //        MsgBox "Enter valid date", vbInformation, App.ProductName
        ////    //         ssShortClosure.Col = LastCol
        ////    //    End If
        ////    //    End If
        ////    //End If
        ////}

        ////private void tbrAction_ButtonClick() //ByVal Button As MSComctlLib.Button
        ////{
        ////    //If Button = "Add F3" Then
        ////    //    frmCancel
        ////    //    frmAdd
        ////    //ElseIf Button = "Edit/View F5" Then
        ////    //    frmCancel
        ////    //    frmEdit
        ////    //ElseIf Button = "Close F9" Then
        ////    //    Unload Me
        ////    //ElseIf Button = "Save F7" Then
        ////    //    frmsave
        ////    //ElseIf Button = "Print F8" Then
        ////    //    frmPrint
        ////    //End If
        ////}

        ////private void frmPrint()
        ////{
        ////    //If ltbPartNo.text <> "" And ltbRunNo.text <> "" Then

        ////    //    With rptDevelopmentReport
        ////    //        .varDevPartNo = ltbPartNo.text
        ////    //        .varRunNo = ltbRunNo.text
        ////    //        .varPartDesc = lblPartDescription
        ////    //        .varReportDate = txtRunDate
        ////    //        .varRecord = txtRecord
        ////    //        .varForgeShift = txtForgeShift
        ////    //        .varTotalDesign = lblTotalDesign
        ////    //        .varTotalMfg = lblTotalMfg
        ////    //       ShowReport rptDevelopmentReport
        ////    //    End With


        ////    //End If


        ////}

        ////private void frmCancel()
        ////{

        ////    //ltbRunNo.text = ""
        ////    //txtDndRep.text = ""
        ////    //txtZapRep.text = ""
        ////    //txtCustComplaint.text = ""
        ////    //chkComplaint.Value = 0
        ////    //chkComplaint.Caption = "Yes / No"
        ////    //txtRunDate.text = ""
        ////    //txtRecord.text = ""
        ////    //lblTotalDesign = ""
        ////    //lblTotalMfg = ""
        ////    //txtForgeShift = ""

        ////    //ssAssumtions.RemoveAll



        ////}

        ////private void frmsave()
        ////{
        ////    //Dim blnSave As Boolean
        ////    //    If Trim(ltbPartNo.text) = "" Then
        ////    //        MsgBox "Part No should be entered", vbInformation, "SmartPD"
        ////    //        }
        ////    //    End If

        ////    //    If Trim(ltbRunNo.text) = "" Then
        ////    //        MsgBox "Run No should be entered", vbInformation, "SmartPD"
        ////    //        }
        ////    //    End If

        ////    //    If chkComplaint.Caption = "Yes" And Trim(txtCustComplaint.text) = "" Then
        ////    //        MsgBox "Complaint Description should be Entered", vbInformation, "SmartPD"
        ////    //        }
        ////    //    End If


        ////    //   'added by mathi
        ////    //    If Len(Trim(txtRunDate.text)) <> 0 Then
        ////    //        If Not IsDate(txtRunDate.text) Then
        ////    //            MsgBox "Enter valid Rundate", vbInformation, App.ProductName
        ////    //            }
        ////    //        End If
        ////    //        Else
        ////    //            MsgBox "Enter RunDate", vbInformation, App.ProductName
        ////    //        }
        ////    //    End If



        ////    //    gvarcnn.BeginTrans
        ////    //    blnSave = False

        ////    //        rsDevReportMain.Filter = adFilterNone
        ////    //        rsDevReportMain.Filter = "part_no='" & ltbPartNo.text & "' AND DEV_RUN_NO = '" & ltbRunNo.text & "'"

        ////    //        With rsDevReportMain
        ////    //            If .RecordCount = 0 Then
        ////    //                .AddNew
        ////    //                !Part_No = ltbPartNo.text
        ////    //                !dev_run_no = ltbRunNo.text
        ////    //            End If
        ////    //            If Trim(txtCustComplaint.text) <> "" Then !cust_comp_desc = Trim(txtCustComplaint.text)
        ////    //            !cust_comp = IIf(chkComplaint.Value = 1, "YES", "NO")
        ////    //            !report_date = CDate(Trim(txtRunDate.text))
        ////    //            !forge_ddrep = Trim(txtDndRep.text)
        ////    //            !forge_zaprep = Trim(txtZapRep.text)
        ////    //            If Trim(txtRecord.text) <> "" Then !Record = Trim(txtRecord.text)
        ////    //            !no_forg_sht = Val(Trim(txtForgeShift.text))
        ////    //            .Update
        ////    //            .UpdateBatch
        ////    //        End With

        ////    //            grdAssumptions.PrimaryKeys(1).Value = ltbPartNo.text
        ////    //            grdAssumptions.PrimaryKeys(2).Value = ltbRunNo.text
        ////    //            grdAssumptions.SaveData
        ////    //            rsDesignAssumptions.UpdateBatch

        ////    //            grdDevReportvoid.PrimaryKeys(1).Value = ltbPartNo.text
        ////    //            grdDevReportvoid.PrimaryKeys(2).Value = ltbRunNo.text
        ////    //            grdDevReportvoid.SaveData
        ////    //            rsDevReportvoid.UpdateBatch

        ////    //            grdShortClosure.PrimaryKeys(1).Value = ltbPartNo.text
        ////    //            grdShortClosure.PrimaryKeys(2).Value = ltbRunNo.text
        ////    //            grdShortClosure.SaveData
        ////    //            rsShortClosure.UpdateBatch

        ////    //        MsgBox "Records Saved sucessfully", vbInformation, "SmartPD"
        ////    //        gvarcnn.CommitTrans
        ////    //        blnSave = True
        ////    //        'MODIFIED BY PREM 27.05.05
        ////    //        RetrieveDevReport
        ////    //        AssignGridManager
        ////}

        ////private void ReworkToolCalculation()
        ////{
        ////    //Dim i As Integer
        ////    //Dim varTotalDesign As Integer
        ////    //Dim varTotalMfg As Integer

        ////    //'        ssDevReportvoid.Update
        ////    //        If ssDevReportvoid.Rows <> 0 Then
        ////    //                ssDevReportvoid.MoveFirst
        ////    //            For i = 1 To ssDevReportvoid.Rows
        ////    //                 varTotalDesign = varTotalDesign + Val(ssDevReportvoid.Columns(3).text)
        ////    //                 varTotalMfg = varTotalMfg + Val(ssDevReportvoid.Columns(4).text)
        ////    //                 ssDevReportvoid.MoveNext
        ////    //            Next
        ////    //        End If

        ////    //        lblTotalDesign = IIf(varTotalDesign = 0, "", varTotalDesign)
        ////    //        lblTotalMfg = IIf(varTotalMfg = 0, "", varTotalMfg)

        ////}

        ////private void tbrAction_ButtonMenuClick()//ByVal ButtonMenu As MSComctlLib.ButtonMenu
        ////{
        ////If ltbPartNo.text = "" Then MsgBox "Part No should be selected", vbInformation, "SmartPD": }

        ////    If RecordCount("part_no", "prd_mast", ltbPartNo.text) = 0 Then
        ////        MsgBox "Part No Does not exists", vbInformation, "SmartPD"
        ////        }
        ////    End If

        ////Select Case ButtonMenu
        ////Case "Product Search"
        ////                   frmProductSearch.Show
        ////       Case "Process Sheet"
        ////                   Unload frmProcessSheet
        ////                   frmProcessSheet.varPMainProductNo = ltbPartNo.text
        ////                   frmProcessSheet.varPMainProductDesc = txtPartDesc
        ////                   frmProcessSheet.Show
        ////       Case "Drawings"
        ////                    Unload frmDrawings
        ////                    frmDrawings.varPMainProductNo = ltbPartNo.text
        ////                    frmDrawings.Show
        ////       Case "Tool Schedule"
        ////                    Unload frmToolSchedule
        ////                    frmToolSchedule.varToolPartno = ltbPartNo.text
        ////                    frmToolSchedule.varToolPartDesc = txtPartDesc
        ////                    frmToolSchedule.Show
        ////       Case "Control Plan"
        ////                    Unload frmPCCS
        ////                    frmPCCS.varPCCSPartNo = ltbPartNo.text
        ////                    frmPCCS.varPCCSPartDesc = txtPartDesc
        ////                    'frmPCCS.ltbRouteNo.text = 1
        ////                    frmPCCS.ssSeqNo.text = 10
        ////                    frmPCCS.Show
        ////       Case "Devlpt Rpt"
        ////                    Unload frmDevelopmentReport
        ////                    frmDevelopmentReport.varDevPartNo = ltbPartNo.text
        ////                    frmDevelopmentReport.varDevPartDesc = txtPartDesc
        ////                    frmDevelopmentReport.Show
        ////'       Case "Cost Sheet"
        ////'                    Unload frmCostSheet
        ////'                If ltbCiReference.text <> "" Then
        ////'                    frmFRCS.varProdMast = ltbPartNo.text
        ////'                    frmFRCS.varCIreference = ltbCiReference.text
        ////'                    frmFRCS.Show
        ////'                Else
        ////'                    MsgBox "CI Reference No should be selected", vbInformation, "SmartPD"
        ////'                End If
        ////        Case "Cost Sheet Search"
        ////                frmCostSheetSearch.Show
        ////        Case "Tools Search"
        ////                frmToolsInfo.Show
        ////   End Select

        ////}

        ////private void ltbPartNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        ////{

        ////}

        ////private void ltbPartNo_DropDownOpened(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        DataTable dt = devRpt.GetAllPartCodeAndDescription();
        ////        ltbPartNo.DataSource = dt;
        ////        ltbPartNo.SelectedValuePath = "part_no";
        ////        ltbPartNo.SelectedTextPath = "part_desc";
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        throw ex.LogException();
        ////    }
        ////}

    }
}




