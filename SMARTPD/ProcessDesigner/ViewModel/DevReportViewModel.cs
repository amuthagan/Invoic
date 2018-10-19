using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Globalization;

namespace ProcessDesigner.ViewModel
{
    public class DevReportViewModel : ViewModelBase
    {
        public System.Windows.Controls.DataGrid SsAssumtions;
        public Microsoft.Windows.Controls.DataGrid SsDevReportLog;
        public System.Windows.Controls.DataGrid SsShortClosure;


        WPF.MDI.MdiChild mdiChild;
        private UserInformation userInformation;
        private DevelopmentReportModel _devModel;
        private DevelopmentReportBll _devBll;
        public Action CloseAction { get; set; }
        private readonly ICommand _onAddCommand;
        private readonly ICommand _onEditCommand;
        private readonly ICommand _onSaveCommand;
        private readonly ICommand _onCloseCommand;
        private readonly ICommand _onPrintCommand;

        private readonly ICommand _rowEditEndingDesignAssumptionCommand;
        public ICommand RowEditEndingDesignAssumptionCommand { get { return this._rowEditEndingDesignAssumptionCommand; } }
        private readonly ICommand _rowEditEndingLogCommand;
        public ICommand RowEditEndingLogCommand { get { return this._rowEditEndingLogCommand; } }
        private readonly ICommand _rowEditEndingShortClosureCommand;
        public ICommand RowEditEndingShortClosureCommand { get { return this._rowEditEndingShortClosureCommand; } }

        public ICommand OnAddCommand { get { return this._onAddCommand; } }
        public ICommand OnEditViewCommand { get { return this._onEditCommand; } }
        public ICommand OnSaveCommand { get { return this._onSaveCommand; } }
        public ICommand OnPrintCommand { get { return this._onPrintCommand; } }
        public ICommand OnCloseCommand { get { return this._onCloseCommand; } }

        private readonly ICommand _deleteCommandAssumtions;
        public ICommand DeleteCommandAssumtions { get { return this._deleteCommandAssumtions; } }
        private readonly ICommand _deleteCommandLog;
        public ICommand DeleteCommandLog { get { return this._deleteCommandLog; } }
        private readonly ICommand _deleteCommandShortClosure;
        public ICommand DeleteCommandShortClosure { get { return this._deleteCommandShortClosure; } }
        private readonly ICommand _enterPartNumber;
        public ICommand EnterPartNumberCmb { get { return this._enterPartNumber; } }

        private DataTable delDesignAssumption = new DataTable();
        private DataTable delLog = new DataTable();
        private DataTable delShortClouser = new DataTable();

        public DataRowView SelectedItemDesignAssumption { get; set; }
        public DataRowView SelectedItemLog { get; set; }
        public DataRowView SelectedItemShortClouser { get; set; }

        private bool _addButtonIsEnable = true;
        private bool _editButtonIsEnable = true;
        private bool _saveButtonIsEnable = true;
        private bool _printButtonIsEnable = true;


        private readonly ICommand _onAvailChkCommand;
        public ICommand OnAvailChkCommand { get { return this._onAvailChkCommand; } }

        public DevReportViewModel(UserInformation userInfo, WPF.MDI.MdiChild me)
        {
            try
            {
                this.mdiChild = me;
                userInformation = new UserInformation();
                userInformation = userInfo;
                this._devBll = new DevelopmentReportBll(userInfo);
                this.DevelopmentReportModel = new DevelopmentReportModel();
                this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
                this.selectChangeComboCommandRunNo = new DelegateCommand(this.SelectDataRowRunNo);

                this._deleteCommandAssumtions = new DelegateCommand<DataRowView>(this.DeleteAssumtions);
                this._deleteCommandLog = new DelegateCommand<DataRowView>(this.DeleteLog);
                this._deleteCommandShortClosure = new DelegateCommand<DataRowView>(this.DeleteShortClosure);
                this._onAvailChkCommand = new DelegateCommand(this.AvailChkCommand);
                this._onAddCommand = new DelegateCommand(this.Add);
                this._onEditCommand = new DelegateCommand(this.Edit);
                this._onPrintCommand = new DelegateCommand(this.Print);
                this._onSaveCommand = new DelegateCommand(this.Save);
                this._onCloseCommand = new DelegateCommand(this.Close);
                this._enterPartNumber = new DelegateCommand<string>(this.EnterPartNumber);
                this.productSearchCommand = new DelegateCommand(this.ProductSearch);
                this.costSheetSearchCommand = new DelegateCommand(this.CostSheetSearch);
                this.toolsSearchCommand = new DelegateCommand(this.ToolsSearch);

                this.processSheetCommand = new DelegateCommand(this.ProcessSheet);
                this.controlPlanCommandm = new DelegateCommand(this.ControlPlanMenu);
                this.drawingsCommand = new DelegateCommand(this.Drawings);
                this.toolScheduleCommand = new DelegateCommand(this.ToolSchedule);

                this._rowEditEndingDesignAssumptionCommand = new DelegateCommand<Object>(this.RowEditEndingDesignAssumption);
                this._rowEditEndingLogCommand = new DelegateCommand<Object>(this.RowEditEndingLog);
                this._rowEditEndingShortClosureCommand = new DelegateCommand<Object>(this.RowEditEndingShortClosure);
                SetdropDownItems();
                GetRights();
                Edit();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
        private bool formType = false;
        public DevReportViewModel(UserInformation userInfo, WPF.MDI.MdiChild me, string partNo)
        {
            try
            {
                this.mdiChild = me;
                userInformation = new UserInformation();
                userInformation = userInfo;
                this._devBll = new DevelopmentReportBll(userInfo);
                this.DevelopmentReportModel = new DevelopmentReportModel();
                this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
                this.selectChangeComboCommandRunNo = new DelegateCommand(this.SelectDataRowRunNo);
                this._enterPartNumber = new DelegateCommand<string>(this.EnterPartNumber);
                this._deleteCommandAssumtions = new DelegateCommand<DataRowView>(this.DeleteAssumtions);
                this._deleteCommandLog = new DelegateCommand<DataRowView>(this.DeleteLog);
                this._deleteCommandShortClosure = new DelegateCommand<DataRowView>(this.DeleteShortClosure);
                this._onAvailChkCommand = new DelegateCommand(this.AvailChkCommand);
                this._onAddCommand = new DelegateCommand(this.Add);
                this._onEditCommand = new DelegateCommand(this.Edit);
                this._onPrintCommand = new DelegateCommand(this.Print);
                this._onSaveCommand = new DelegateCommand(this.Save);
                this._onCloseCommand = new DelegateCommand(this.Close);

                this.productSearchCommand = new DelegateCommand(this.ProductSearch);
                this.costSheetSearchCommand = new DelegateCommand(this.CostSheetSearch);
                this.toolsSearchCommand = new DelegateCommand(this.ToolsSearch);

                this.processSheetCommand = new DelegateCommand(this.ProcessSheet);
                this.controlPlanCommandm = new DelegateCommand(this.ControlPlanMenu);
                this.drawingsCommand = new DelegateCommand(this.Drawings);
                this.toolScheduleCommand = new DelegateCommand(this.ToolSchedule);

                this._rowEditEndingDesignAssumptionCommand = new DelegateCommand<Object>(this.RowEditEndingDesignAssumption);
                this._rowEditEndingLogCommand = new DelegateCommand<Object>(this.RowEditEndingLog);
                this._rowEditEndingShortClosureCommand = new DelegateCommand<Object>(this.RowEditEndingShortClosure);
                SetdropDownItems();
                GetRights();
                Edit();
                DevelopmentReportModel.PartNo = partNo;
                formType = true;
                DevelopmentReportModel.PartNoDetails.RowFilter = "Part_no='" + partNo + "'";
                SelectedRowPart = DevelopmentReportModel.PartNoDetails[0];
                SelectDataRowPart();
                DevelopmentReportModel.PartNoDetails.RowFilter = string.Empty;
            }
            catch (Exception ex)
            {
                ex.LogException();
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
        public void OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            // if (e.Column.Header.ToString() == "S.No" || e.Column.Header.ToString() == "Seq No") e.Cancel = true;
        }
        public void OnBeginningEditLog(object sender, Microsoft.Windows.Controls.DataGridBeginningEditEventArgs e)
        {
            // if (e.Column.Header.ToString() == "S.No" || e.Column.Header.ToString() == "Seq No") e.Cancel = true;
        }
        private readonly ICommand productSearchCommand = null;
        public ICommand ProductSearchCommand { get { return this.productSearchCommand; } }
        private void ProductSearch()
        {
            try
            {
                //if (!DevelopmentReportModel.PartNo.IsNotNullOrEmpty())
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}

                //if (!MandatoryFields.ROUTE_NO.IsNotNullOrEmpty())
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Route No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}

                //frmProductSearch productSearch = new frmProductSearch(userInformation);
                //productSearch.Show();
                showDummy();
                MdiChild mdiProductSearch = new MdiChild();
                ProcessDesigner.frmProductSearch productSearch = new frmProductSearch(userInformation, mdiProductSearch);
                mdiProductSearch.Title = ApplicationTitle + " - Product Search";
                mdiProductSearch.Content = productSearch;
                mdiProductSearch.Height = productSearch.Height + 40;
                mdiProductSearch.Width = productSearch.Width + 20;
                mdiProductSearch.MinimizeBox = false;
                mdiProductSearch.MaximizeBox = false;
                mdiProductSearch.Resizable = false;
                if (MainMDI.IsFormAlreadyOpen("Product Search") == false)
                {
                    MainMDI.Container.Children.Add(mdiProductSearch);
                }
                else
                {
                    mdiProductSearch = (MdiChild)MainMDI.GetFormAlreadyOpened("Product Search");
                    MainMDI.SetMDI(mdiProductSearch);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand costSheetSearchCommand = null;
        public ICommand CostSheetSearchCommand { get { return this.costSheetSearchCommand; } }
        private void CostSheetSearch()
        {
            try
            {
                //if (!DevelopmentReportModel.PartNo.IsNotNullOrEmpty())
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}

                //if (!MandatoryFields.ROUTE_NO.IsNotNullOrEmpty())
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Route No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}

                //WPF.MDI.MdiChild mdiCostSheetSearch = new WPF.MDI.MdiChild();
                //ProcessDesigner.frmCostSheetSearch costSheetSearch = new ProcessDesigner.frmCostSheetSearch(userInformation, mdiCostSheetSearch);
                //costSheetSearch.ShowDialog();
                showDummy();
                MdiChild mdiCostSheetSearch = new MdiChild();
                ProcessDesigner.frmCostSheetSearch costSheetSearch = new ProcessDesigner.frmCostSheetSearch(userInformation, mdiCostSheetSearch);
                mdiCostSheetSearch.Title = ApplicationTitle + " - Cost Sheet Search";
                mdiCostSheetSearch.Content = costSheetSearch;
                mdiCostSheetSearch.Height = mdiCostSheetSearch.Height + 40;
                mdiCostSheetSearch.Width = mdiCostSheetSearch.Width + 20;
                mdiCostSheetSearch.MinimizeBox = false;
                mdiCostSheetSearch.MaximizeBox = false;
                mdiCostSheetSearch.Resizable = false;
                if (MainMDI.IsFormAlreadyOpen("Cost Sheet Search") == false)
                {
                    MainMDI.Container.Children.Add(mdiCostSheetSearch);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private readonly ICommand toolsSearchCommand = null;
        public ICommand ToolsSearchCommand { get { return this.toolsSearchCommand; } }
        private void ToolsSearch()
        {
            try
            {
                //if (!DevelopmentReportModel.PartNo.IsNotNullOrEmpty())
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}
                showDummy();
                MdiChild mdiToolsInfo = new MdiChild();
                if (MainMDI.IsFormAlreadyOpen("Tools Information") == false)
                {

                    ProcessDesigner.frmToolsInfo toolsinfo = new ProcessDesigner.frmToolsInfo(userInformation, mdiToolsInfo);
                    mdiToolsInfo.Title = ApplicationTitle + " - Tools Information";
                    mdiToolsInfo.Content = toolsinfo;
                    mdiToolsInfo.Height = toolsinfo.Height + 23;
                    mdiToolsInfo.Width = toolsinfo.Width + 20;
                    //mdiToolsInfo.MinimizeBox = false;
                    //mdiToolsInfo.MaximizeBox = false;
                    //mdiToolsInfo.Resizable = false;
                    MainMDI.Container.Children.Add(mdiToolsInfo);
                }
                else
                {
                    mdiToolsInfo = (MdiChild)MainMDI.GetFormAlreadyOpened("Tools Information");
                    //toolschedule = (frmToolSchedule_new)mdiCostSheetSearch.Content;
                    MainMDI.SetMDI(mdiToolsInfo);
                }
                //if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Route No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}

                //frmCopyStatus copyStatus = new frmCopyStatus("ProcessSheet", MandatoryFields.PART_NO.Trim(), ProcessSheet.ROUTE_NO.ToString(), "", "", "");
                //copyStatus.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand processSheetCommand = null;
        public ICommand ProcessSheetCommand { get { return this.processSheetCommand; } }
        private void ProcessSheet()
        {
            try
            {
                if (!DevelopmentReportModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild psheet = new MdiChild();
                psheet.Title = ApplicationTitle + " - Process Sheet";
                frmProcessSheet processsheet = null;
                if (MainMDI.IsFormAlreadyOpen("Process Sheet - " + DevelopmentReportModel.PartNo.Trim()) == false)
                {
                    processsheet = new frmProcessSheet(psheet, userInformation, DevelopmentReportModel.PartNo, DevelopmentReportModel.PartNoDesc);
                    psheet.Content = processsheet;
                    psheet.Height = processsheet.Height + 40;
                    psheet.Width = processsheet.Width + 20;
                    psheet.Resizable = false;
                    psheet.MinimizeBox = true;
                    psheet.MaximizeBox = true;
                    MainMDI.Container.Children.Add(psheet);
                }
                else
                {
                    psheet = new MdiChild();
                    psheet = (MdiChild)MainMDI.GetFormAlreadyOpened("Process Sheet - " + DevelopmentReportModel.PartNo.Trim());
                    processsheet = (frmProcessSheet)psheet.Content;
                    MainMDI.SetMDI(psheet);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand drawingsCommand = null;
        public ICommand DrawingsCommand { get { return this.drawingsCommand; } }
        private void Drawings()
        {
            try
            {
                if (!DevelopmentReportModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild drwMaster = new MdiChild();
                drwMaster.Title = ApplicationTitle + " - Drawings";
                ProcessDesigner.frmDrawings drawings = null;
                if (MainMDI.IsFormAlreadyOpen("Drawings - " + DevelopmentReportModel.PartNo.Trim()) == false)
                {
                    drawings = new ProcessDesigner.frmDrawings(drwMaster, userInformation, DevelopmentReportModel.PartNo);
                    drwMaster.Content = drawings;
                    drwMaster.Height = drawings.Height + 40;
                    drwMaster.Width = drawings.Width + 20;
                    drwMaster.MinimizeBox = false;
                    drwMaster.MaximizeBox = false;
                    drwMaster.Resizable = false;
                    if (MainMDI.IsFormAlreadyOpen("Drawings - " + DevelopmentReportModel.PartNo.Trim()) == false)
                    {
                        MainMDI.Container.Children.Add(drwMaster);
                    }
                    else
                    {
                        drwMaster = new MdiChild();
                        drwMaster = (MdiChild)MainMDI.GetFormAlreadyOpened("Drawings - " + DevelopmentReportModel.PartNo.Trim());
                        MainMDI.SetMDI(drwMaster);
                    }
                }
                else
                {
                    drwMaster = new MdiChild();
                    drwMaster = (MdiChild)MainMDI.GetFormAlreadyOpened("Drawings  - " + DevelopmentReportModel.PartNo.Trim());
                    drawings = (frmDrawings)drwMaster.Content;
                    MainMDI.SetMDI(drwMaster);
                }

                //if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Route No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}

                //frmCopyStatus copyStatus = new frmCopyStatus("ProcessSheet", MandatoryFields.PART_NO.Trim(), ProcessSheet.ROUTE_NO.ToString(), "", "", "");
                //copyStatus.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand toolScheduleCommand = null;
        public ICommand ToolScheduleCommand { get { return this.toolScheduleCommand; } }
        private void ToolSchedule()
        {
            try
            {
                if (!DevelopmentReportModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild tschedule = null;
                frmToolSchedule_new toolschedule = null;
                if (MainMDI.IsFormAlreadyOpen("Tool Schedule - " + DevelopmentReportModel.PartNo.Trim()) == false)
                {
                    tschedule = new MdiChild();
                    tschedule.Title = ApplicationTitle + " - Tool Schedule";
                    toolschedule = new frmToolSchedule_new(userInformation, tschedule, DevelopmentReportModel.PartNo);
                    tschedule.Content = toolschedule;
                    tschedule.Height = toolschedule.Height + 40;
                    tschedule.Width = toolschedule.Width + 20;
                    tschedule.Resizable = false;
                    tschedule.MinimizeBox = true;
                    tschedule.MaximizeBox = true;
                    MainMDI.Container.Children.Add(tschedule);
                }
                else
                {
                    tschedule = new MdiChild();
                    tschedule = (MdiChild)MainMDI.GetFormAlreadyOpened("Tool Schedule - " + DevelopmentReportModel.PartNo.Trim());
                    toolschedule = (frmToolSchedule_new)tschedule.Content;
                    MainMDI.SetMDI(tschedule);
                }
                toolschedule.EditSelectedPartNo(DevelopmentReportModel.PartNo);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand controlPlanCommandm = null;
        public ICommand ControlPlanCommandm { get { return this.controlPlanCommandm; } }
        private void ControlPlanMenu()
        {
            try
            {
                if (!DevelopmentReportModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Route No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}
                showDummy();
                MdiChild cplan = new MdiChild();
                cplan.Title = ApplicationTitle + " - Control Plan";

                frmPCCS cplanscreen = null;
                if (MainMDI.IsFormAlreadyOpen("Control Plan - " + DevelopmentReportModel.PartNo.Trim()) == false)
                {

                    cplanscreen = new frmPCCS(userInformation, cplan, DevelopmentReportModel.PartNo);
                    cplan.Content = cplanscreen;
                    cplan.Height = cplanscreen.Height + 40;
                    cplan.Width = cplanscreen.Width + 20;
                    cplan.Resizable = false;
                    cplan.MinimizeBox = true;
                    cplan.MaximizeBox = true;
                    MainMDI.Container.Children.Add(cplan);
                }
                else
                {
                    cplan = new MdiChild();
                    cplan = (MdiChild)MainMDI.GetFormAlreadyOpened("Control Plan - " + DevelopmentReportModel.PartNo.Trim());
                    cplanscreen = (frmPCCS)cplan.Content;
                    MainMDI.SetMDI(cplan);
                }
                //frmPCCS pccs = new frmPCCS("ProcessSheet", MandatoryFields.PART_NO.Trim(), ProcessSheet.ROUTE_NO.ToString(), "", "", "");
                //pccs.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand costSheetCommand = null;
        public ICommand CostSheetCommand { get { return this.costSheetCommand; } }
        private void CostSheet()
        {
            try
            {
                if (!DevelopmentReportModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                frmCostSheet costSheet = new frmCostSheet(userInformation, DevelopmentReportModel.PartNo, -9999);
                costSheet.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void AvailChkCommand()
        {
            if (DevelopmentReportModel.IsDoYouHaveCustComplaint == false)
            {
                DevelopmentReportModel.LabelNatureOfComplaint = "No";
                IsReadonlyComp = true;


            }
            else if (DevelopmentReportModel.IsDoYouHaveCustComplaint == true)
            {
                IsReadonlyComp = false;
                DevelopmentReportModel.LabelNatureOfComplaint = "Yes";
            }


        }

        private void DeleteShortClosure(DataRowView selecteditem)
        {
            try
            {
                List<String> lstSelect = new List<String>();
                DataRowView newRow;

                if (DevelopmentReportModel.ShortClosureDetails.Table.Columns.IndexOf("ID_NUMBERAUTO") < 0)
                    DevelopmentReportModel.ShortClosureDetails.Table.Columns.Add("ID_NUMBERAUTO");
                for (int ictr = 0; ictr < DevelopmentReportModel.ShortClosureDetails.Count; ictr++)
                {
                    DevelopmentReportModel.ShortClosureDetails[ictr]["ID_NUMBERAUTO"] = ictr;
                }
                foreach (DataRowView drView in SsShortClosure.SelectedItems)
                {
                    lstSelect.Add(drView["ID_NUMBERAUTO"].ToValueAsString());
                }

                MessageBoxResult msgResult = ShowWarningMessage(PDMsg.BeforeDelete("selected record(s)"), MessageBoxButton.YesNo);

                if (msgResult == MessageBoxResult.Yes)
                {
                    for (int ictr = 0; ictr < lstSelect.Count; ictr++)
                    {
                        DataRowView delView = null;
                        foreach (DataRowView dvView in DevelopmentReportModel.ShortClosureDetails)
                        {
                            if (lstSelect[ictr] == dvView["ID_NUMBERAUTO"])
                            {
                                delView = dvView;
                                break;
                            }
                        }

                        if (delView != null)
                        {
                            selecteditem = delView;
                            if (selecteditem.IsNotNullOrEmpty() && AddButtonIsEnable == false)
                            {
                                selecteditem.Row.Delete();
                            }
                            else if (selecteditem.IsNotNullOrEmpty() && AddButtonIsEnable == true)
                            {
                                delShortClouser.ImportRow(selecteditem.Row);
                                selecteditem.Row.Delete();
                            }
                        }
                    }
                    #region ShortClosureAddNew

                    if (DevelopmentReportModel.ShortClosureDetails.Count > 0)
                    {
                        if (selecteditem["SNO"].ToString().IsNotNullOrEmpty())
                        {
                            DataRowView drv = DevelopmentReportModel.ShortClosureDetails.AddNew();
                            drv.BeginEdit();
                            drv["SNO"] = DevelopmentReportModel.ShortClosureDetails.Count;
                            drv["PART_NO"] = DevelopmentReportModel.PartNo;
                            drv["RUN_NO"] = DevelopmentReportModel.RunNo;
                            drv.EndEdit();
                        }
                        else if (DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["REASON"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["WHY"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["SHORT_ACTION"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["SHORT_ACTION_DATE"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["LONG_ACTION"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["LONG_ACTION_DATE"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["TRIAL_DATE"].ToString() != "")
                        {
                            DataRowView drv = DevelopmentReportModel.ShortClosureDetails.AddNew();
                            drv.BeginEdit();
                            drv["SNO"] = DevelopmentReportModel.ShortClosureDetails.Count;
                            drv["PART_NO"] = DevelopmentReportModel.PartNo;
                            drv["RUN_NO"] = DevelopmentReportModel.RunNo;
                            drv.EndEdit();
                        }

                    }
                    else if (DevelopmentReportModel.ShortClosureDetails.Count == 0)
                    {
                        DataRowView drv = DevelopmentReportModel.ShortClosureDetails.AddNew();
                        drv.BeginEdit();
                        drv["SNO"] = DevelopmentReportModel.ShortClosureDetails.Count;
                        drv["PART_NO"] = DevelopmentReportModel.PartNo;
                        drv["RUN_NO"] = DevelopmentReportModel.RunNo;
                        drv.EndEdit();
                    }
                    #endregion
                }
                if (DevelopmentReportModel.ShortClosureDetails.Table.Columns.IndexOf("ID_NUMBERAUTO") >= 0)
                    DevelopmentReportModel.ShortClosureDetails.Table.Columns.Remove("ID_NUMBERAUTO");
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void DeleteLog(DataRowView selecteditem)
        {
            try
            {
                List<String> lstSelect = new List<String>();
                DataRowView newRow;

                if (DevelopmentReportModel.LogDetails.Table.Columns.IndexOf("ID_NUMBERAUTO") < 0)
                    DevelopmentReportModel.LogDetails.Table.Columns.Add("ID_NUMBERAUTO");
                for (int ictr = 0; ictr < DevelopmentReportModel.LogDetails.Count; ictr++)
                {
                    DevelopmentReportModel.LogDetails[ictr]["ID_NUMBERAUTO"] = ictr;
                }
                foreach (DataRowView drView in SsDevReportLog.SelectedItems)
                {
                    lstSelect.Add(drView["ID_NUMBERAUTO"].ToValueAsString());
                }

                MessageBoxResult msgResult = ShowWarningMessage(PDMsg.BeforeDelete("selected record(s)"), MessageBoxButton.YesNo);

                if (msgResult == MessageBoxResult.Yes)
                {
                    for (int ictr = 0; ictr < lstSelect.Count; ictr++)
                    {
                        DataRowView delView = null;
                        foreach (DataRowView dvView in DevelopmentReportModel.LogDetails)
                        {
                            if (lstSelect[ictr] == dvView["ID_NUMBERAUTO"])
                            {
                                delView = dvView;
                                break;
                            }
                        }

                        if (delView != null)
                        {
                            selecteditem = delView;
                            if (selecteditem.IsNotNullOrEmpty() && AddButtonIsEnable == false)
                            {
                                selecteditem.Row.Delete();
                            }
                            else if (selecteditem.IsNotNullOrEmpty() && AddButtonIsEnable == true)
                            {
                                delLog.ImportRow(selecteditem.Row);
                                selecteditem.Row.Delete();
                            }
                        }
                    }
                }

                #region LogAddNew

                if (DevelopmentReportModel.LogDetails.Count > 0)
                {
                    if (DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["PROBLEM_FACED"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["COUNTER_MEASURE"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REWORK_TOOL_DESIGN"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REWORK_TOOL_DESIGN"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REMARKS"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REWORK_TOOL_MFG"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REWORK_TOOL_MFG"].ToString() != "")
                    {
                        DataRowView drv = DevelopmentReportModel.LogDetails.AddNew();
                        drv.BeginEdit();
                        drv["STAGE_NO"] = DevelopmentReportModel.LogDetails.Count;
                        drv["PART_NO"] = DevelopmentReportModel.PartNo;
                        drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                        drv.EndEdit();
                    }
                    else if (selecteditem["STAGE_NO"].ToString().IsNotNullOrEmpty())
                    {
                        DataRowView drv = DevelopmentReportModel.LogDetails.AddNew();
                        drv.BeginEdit();
                        drv["STAGE_NO"] = DevelopmentReportModel.LogDetails.Count;
                        drv["PART_NO"] = DevelopmentReportModel.PartNo;
                        drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                        drv.EndEdit();
                    }
                }
                else if (DevelopmentReportModel.LogDetails.Count == 0)
                {
                    DataRowView drv = DevelopmentReportModel.LogDetails.AddNew();
                    drv.BeginEdit();
                    drv["STAGE_NO"] = DevelopmentReportModel.LogDetails.Count;
                    drv["PART_NO"] = DevelopmentReportModel.PartNo;
                    drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                    drv.EndEdit();
                }

                if (DevelopmentReportModel.LogDetails.Table.Columns.IndexOf("ID_NUMBERAUTO") >= 0)
                    DevelopmentReportModel.LogDetails.Table.Columns.Remove("ID_NUMBERAUTO");

                #endregion

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void DeleteAssumtions(DataRowView selecteditem)
        {
            try
            {
                List<String> lstSelect = new List<String>();
                DataRowView newRow;

                if (DevelopmentReportModel.DesignAssumptionDetails.Table.Columns.IndexOf("ID_NUMBERAUTO") < 0)
                    DevelopmentReportModel.DesignAssumptionDetails.Table.Columns.Add("ID_NUMBERAUTO");
                for (int ictr = 0; ictr < DevelopmentReportModel.DesignAssumptionDetails.Count; ictr++)
                {
                    DevelopmentReportModel.DesignAssumptionDetails[ictr]["ID_NUMBERAUTO"] = ictr;
                }
                foreach (DataRowView drView in SsAssumtions.SelectedItems)
                {
                    lstSelect.Add(drView["ID_NUMBERAUTO"].ToValueAsString());
                }

                MessageBoxResult msgResult = ShowWarningMessage(PDMsg.BeforeDelete("selected record(s)"), MessageBoxButton.YesNo);

                if (msgResult == MessageBoxResult.Yes)
                {
                    for (int ictr = 0; ictr < lstSelect.Count; ictr++)
                    {
                        DataRowView delView = null;
                        foreach (DataRowView dvView in DevelopmentReportModel.DesignAssumptionDetails)
                        {
                            if (lstSelect[ictr] == dvView["ID_NUMBERAUTO"])
                            {
                                delView = dvView;
                                break;
                            }
                        }

                        if (delView != null)
                        {
                            selecteditem = delView;
                            if (selecteditem.IsNotNullOrEmpty() && AddButtonIsEnable == false)
                            {
                                selecteditem.Row.Delete();
                            }
                            else if (selecteditem.IsNotNullOrEmpty() && AddButtonIsEnable == true)
                            {
                                delDesignAssumption.ImportRow(selecteditem.Row);
                                selecteditem.Row.Delete();
                            }
                        }
                    }
                    #region DesignAssumptionAddNew
                    if (DevelopmentReportModel.DesignAssumptionDetails.Count == 0)
                    {
                        DataRowView drv = DevelopmentReportModel.DesignAssumptionDetails.AddNew();
                        drv.BeginEdit();
                        drv["SNO"] = DevelopmentReportModel.DesignAssumptionDetails.Count;
                        drv["PART_NO"] = DevelopmentReportModel.PartNo;
                        drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                        drv.EndEdit();
                    }
                    else if (DevelopmentReportModel.DesignAssumptionDetails[DevelopmentReportModel.DesignAssumptionDetails.Count - 1]["CFT_DISS"].ToString() != "" || DevelopmentReportModel.DesignAssumptionDetails[DevelopmentReportModel.DesignAssumptionDetails.Count - 1]["JUSTIFICATION"].ToString() != "" || DevelopmentReportModel.DesignAssumptionDetails[DevelopmentReportModel.DesignAssumptionDetails.Count - 1]["TGR"].ToString() != "" || DevelopmentReportModel.DesignAssumptionDetails[DevelopmentReportModel.DesignAssumptionDetails.Count - 1]["TWG"].ToString() != "")
                    {
                        DataRowView drv = DevelopmentReportModel.DesignAssumptionDetails.AddNew();
                        drv.BeginEdit();
                        drv["SNO"] = DevelopmentReportModel.DesignAssumptionDetails.Count;
                        drv["PART_NO"] = DevelopmentReportModel.PartNo;
                        drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                        drv.EndEdit();
                    }
                }
                if (DevelopmentReportModel.DesignAssumptionDetails.Table.Columns.IndexOf("ID_NUMBERAUTO") >= 0)
                    DevelopmentReportModel.DesignAssumptionDetails.Table.Columns.Remove("ID_NUMBERAUTO");
                    #endregion

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
        private Visibility _buttonVisible = Visibility.Collapsed;
        public Visibility ButtonEnable
        {
            get { return _buttonVisible; }
            set
            {
                this._buttonVisible = value;
                NotifyPropertyChanged("ButtonEnable");
            }
        }

        private RolePermission _actionPermission;
        public RolePermission ActionPermission
        {
            get { return _actionPermission; }
            set
            {
                this._actionPermission = value;
                NotifyPropertyChanged("ActionPermission");

            }
        }
        private bool _isReadOnlyRun = false;
        public bool IsReadOnlyRun
        {
            get { return _isReadOnlyRun; }
            set
            {
                this._isReadOnlyRun = value;
                NotifyPropertyChanged("IsReadOnlyRun");

            }
        }
        private void GetRights()
        {
            ActionPermission = new RolePermission();
            ActionPermission.AddNew = true;
            ActionPermission.Save = true;
            ActionPermission.Print = false;
            ActionPermission.View = true;
            ActionPermission.Delete = false;
            ActionPermission.Edit = false;
            ActionPermission = _devBll.GetUserRights("DEVELOPMENT REPORT");
            if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
            {
                ActionPermission.Save = false;
            }
        }

        private void setRights()
        {
            if (EditButtonIsEnable) EditButtonIsEnable = ActionPermission.Edit;
            if (AddButtonIsEnable) _addButtonIsEnable = ActionPermission.AddNew;
            if (SaveButtonIsEnable) SaveButtonIsEnable = ActionPermission.Save;
            if (PrintButtonIsEnable) PrintButtonIsEnable = ActionPermission.Print;
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
        public bool EditButtonIsEnable
        {
            get { return _editButtonIsEnable; }
            set
            {
                this._editButtonIsEnable = value;
                NotifyPropertyChanged("EditButtonIsEnable");
            }
        }
        public bool AddButtonIsEnable
        {
            get { return _addButtonIsEnable; }
            set
            {
                this._addButtonIsEnable = value;
                NotifyPropertyChanged("AddButtonIsEnable");
            }
        }

        private bool _isReadonlyComp = true;
        public bool IsReadonlyComp
        {
            get { return _isReadonlyComp; }
            set
            {
                this._isReadonlyComp = value;
                NotifyPropertyChanged("IsReadonlyComp");
            }
        }
        public bool SaveButtonIsEnable
        {
            get { return _saveButtonIsEnable; }
            set
            {
                this._saveButtonIsEnable = value;
                NotifyPropertyChanged("SaveButtonIsEnable");
            }
        }
        public bool PrintButtonIsEnable
        {
            get { return _printButtonIsEnable; }
            set
            {
                this._printButtonIsEnable = value;
                NotifyPropertyChanged("PrintButtonIsEnable");
            }
        }

        private void RowEditEndingShortClosure(object selecteditem)
        {
            if (selecteditem != null)
            {
                SelectedItemShortClouser = (DataRowView)selecteditem;
                if (SelectedItemShortClouser["SNO"].ToString().IsNotNullOrEmpty())
                {
                    if (DevelopmentReportModel.ShortClosureDetails.Count == 0)
                    {
                        DataRowView drv = DevelopmentReportModel.ShortClosureDetails.AddNew();
                        drv.BeginEdit();
                        drv["SNO"] = DevelopmentReportModel.ShortClosureDetails.Count;
                        drv["PART_NO"] = DevelopmentReportModel.PartNo;
                        drv["RUN_NO"] = DevelopmentReportModel.RunNo;
                        drv.EndEdit();
                    }
                    else if (DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["REASON"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["WHY"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["SHORT_ACTION"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["SHORT_ACTION_DATE"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["LONG_ACTION"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["LONG_ACTION_DATE"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["TRIAL_DATE"].ToString() != "")
                    {
                        DataRowView drv = DevelopmentReportModel.ShortClosureDetails.AddNew();
                        drv.BeginEdit();
                        drv["SNO"] = DevelopmentReportModel.ShortClosureDetails.Count;
                        drv["PART_NO"] = DevelopmentReportModel.PartNo;
                        drv["RUN_NO"] = DevelopmentReportModel.RunNo;
                        drv.EndEdit();
                    }

                }

            }
        }
        public void OnCellEditEndingLogIssue(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                string columnName = e.Column.SortMemberPath;
                if (e.Column.GetType() == typeof(System.Windows.Controls.DataGridTemplateColumn))
                {
                    if (columnName == "SHORT_ACTION_DATE" || columnName == "LONG_ACTION_DATE" || columnName == "TRIAL_DATE")
                    {
                        var popup = GetVisualChild<System.Windows.Controls.Primitives.Popup>(e.EditingElement);
                        if (popup != null && popup.IsOpen)
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
        private static T GetVisualChild<T>(DependencyObject visual)
       where T : DependencyObject
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

        private void RowEditEndingLog(object selecteditem)
        {
            if (selecteditem != null)
            {
                SelectedItemLog = (DataRowView)selecteditem;
                if (SelectedItemLog["STAGE_NO"].ToString().IsNotNullOrEmpty())
                {
                    if (DevelopmentReportModel.LogDetails.Count == 0)
                    {
                        DataRowView drv = DevelopmentReportModel.LogDetails.AddNew();
                        drv.BeginEdit();
                        drv["STAGE_NO"] = DevelopmentReportModel.LogDetails.Count;
                        drv["PART_NO"] = DevelopmentReportModel.PartNo;
                        drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                        drv.EndEdit();
                    }
                    else if (DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["PROBLEM_FACED"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["COUNTER_MEASURE"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REWORK_TOOL_DESIGN"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REWORK_TOOL_DESIGN"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REMARKS"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REWORK_TOOL_MFG"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REWORK_TOOL_MFG"].ToString() != "")
                    {
                        DataRowView drv = DevelopmentReportModel.LogDetails.AddNew();
                        drv.BeginEdit();
                        drv["STAGE_NO"] = DevelopmentReportModel.LogDetails.Count;
                        drv["PART_NO"] = DevelopmentReportModel.PartNo;
                        drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                        drv.EndEdit();
                    }
                }
                DataTable dtLog = DevelopmentReportModel.LogDetails.ToTable();
                if (DevelopmentReportModel.LogDetails.Count > 0)
                {
                    DevelopmentReportModel.ToolRWDesignTotal = dtLog.Compute("SUM(REWORK_TOOL_DESIGN)", string.Empty).ToValueAsString();
                    DevelopmentReportModel.ToolRWMfgTotal = dtLog.Compute("SUM(REWORK_TOOL_MFG)", string.Empty).ToValueAsString();
                }
                else
                {
                    DevelopmentReportModel.ToolRWDesignTotal = "0";
                    DevelopmentReportModel.ToolRWMfgTotal = "0";
                }
            }
        }

        private void RowEditEndingDesignAssumption(object selecteditem)
        {
            if (selecteditem != null)
            {
                SelectedItemDesignAssumption = (DataRowView)selecteditem;
                if (SelectedItemDesignAssumption["SNO"].ToString().IsNotNullOrEmpty())
                {
                    if (DevelopmentReportModel.DesignAssumptionDetails.Count == 0)
                    {
                        DataRowView drv = DevelopmentReportModel.DesignAssumptionDetails.AddNew();
                        drv.BeginEdit();
                        drv["SNO"] = DevelopmentReportModel.DesignAssumptionDetails.Count;
                        drv["PART_NO"] = DevelopmentReportModel.PartNo;
                        drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                        drv.EndEdit();
                    }
                    else if (DevelopmentReportModel.DesignAssumptionDetails[DevelopmentReportModel.DesignAssumptionDetails.Count - 1]["CFT_DISS"].ToString() != "" || DevelopmentReportModel.DesignAssumptionDetails[DevelopmentReportModel.DesignAssumptionDetails.Count - 1]["JUSTIFICATION"].ToString() != "" || DevelopmentReportModel.DesignAssumptionDetails[DevelopmentReportModel.DesignAssumptionDetails.Count - 1]["TGR"].ToString() != "" || DevelopmentReportModel.DesignAssumptionDetails[DevelopmentReportModel.DesignAssumptionDetails.Count - 1]["TWG"].ToString() != "")
                    {
                        DataRowView drv = DevelopmentReportModel.DesignAssumptionDetails.AddNew();
                        drv.BeginEdit();
                        drv["SNO"] = DevelopmentReportModel.DesignAssumptionDetails.Count;
                        drv["PART_NO"] = DevelopmentReportModel.PartNo;
                        drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                        drv.EndEdit();
                    }

                }


            }
        }
        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
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

        private void Close()
        {
            try
            {
                //if (DevelopmentReportModel.PartNo != "" && DevelopmentReportModel.RunNo != "")
                //    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //    {
                //        Save();
                //    }

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

        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (DevelopmentReportModel.PartNo != "" && DevelopmentReportModel.RunNo != "")
                //    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //    {
                //        Save();
                //    }

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
        private bool isError = false;
        private bool isSaved = false;
        private void Save()
        {
            string typ = "";
            FocusButton = true;
            if (DevelopmentReportModel.PartNo == "")
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                isError = true;
                return;
            }
            else if (DevelopmentReportModel.RunNo == "")
            {
                ShowInformationMessage(PDMsg.NotEmpty("Run No"));
                isError = true;
                return;
            }
            else if (!DevelopmentReportModel.RunDate.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Run Date"));
                isError = true;
                return;
            }
            DevelopmentReportModel.LogDetails.Table.AcceptChanges();
            DevelopmentReportModel.ShortClosureDetails.Table.AcceptChanges();
            DevelopmentReportModel.LogDetails.Table.AcceptChanges();
            Progress.ProcessingText = PDMsg.ProgressUpdateText;
            Progress.Start();
            if (_devBll.SaveDevelopmentRpt(DevelopmentReportModel, ref typ))
            {
                Progress.End();
                if (typ == "UPD")
                {
                    Progress.End();
                    ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                    FocusButton = true;
                    delDesignAssumption.Clear();
                    delLog.Clear();
                    delShortClouser.Clear();
                }
                else
                {
                    Progress.End();
                    ShowInformationMessage(PDMsg.SavedSuccessfully);
                }
                isError = false;
                isSaved = true;
                FocusButton = true;
                // if (EditButtonIsEnable == false)
                // Edit();

                //ButtonEnable = Visibility.Visible;
                //AddButtonIsEnable = true;
                //EditButtonIsEnable = false;
                //PrintButtonIsEnable = true;

                //DevelopmentReportModel.PartNo = "";
                //DevelopmentReportModel.PartNoDesc = "";
                //DevelopmentReportModel.RunNo = "";
                //IsReadOnlyRun = true;
                //DevelopmentReportModel.DADRep = "";
                //DevelopmentReportModel.ZapRep = "";
                //DevelopmentReportModel.RunDate = null;
                //DevelopmentReportModel.ToolRWDesignTotal = "";
                //DevelopmentReportModel.ToolRWMfgTotal = "";
                //DevelopmentReportModel.NoOfForginShift = "";
                //DevelopmentReportModel.NatureOfComplaint = "";
                //DevelopmentReportModel.LabelNatureOfComplaint = "Yes / No";
                //DevelopmentReportModel.IsDoYouHaveCustComplaint = false;
                //DevelopmentReportModel.RecordOfCFTDiscussion = "";
                //_devBll.GetPartNoDetails(DevelopmentReportModel);
                //_devBll.GetTabGridDetails(DevelopmentReportModel);
                setRights();
                FocusButton = true;
            }
            else
            {
                Progress.End();
            }

            Progress.End();
        }
        private DataView _devRptMain = null;
        public DataView DevRptMain
        {
            get { return _devRptMain; }
            set
            {
                _devRptMain = value;
                NotifyPropertyChanged("DevRptMain");
            }
        }

        private DataView _devRptDesignAssu = null;
        public DataView DevRptDesignAssumption
        {
            get { return _devRptDesignAssu; }
            set
            {
                _devRptDesignAssu = value;
                NotifyPropertyChanged("DevRptDesignAssumption");
            }
        }
        private DataView _devRptlog = null;
        public DataView DevRptlog
        {
            get { return _devRptlog; }
            set
            {
                _devRptlog = value;
                NotifyPropertyChanged("DevRptlog");
            }
        }

        private DataView _devRptShortClouser = null;
        public DataView DevRptShortClouser
        {
            get { return _devRptShortClouser; }
            set
            {
                _devRptShortClouser = value;
                NotifyPropertyChanged("DevRptShortClouser");
            }
        }

        private void Print()
        {
            if (DevelopmentReportModel.PartNo == "")
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            Progress.ProcessingText = PDMsg.Load;
            Progress.Start();
            DevRptMain = _devBll.GetTabPrintReportDevMainDetails(DevelopmentReportModel);
            //for (int i = 0; i < DevRptMain.Count; i++)
            //{
            //    //DevRptMain[i]["REPORT_DATE"] = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(DevRptMain[i]["REPORT_DATE"], CultureInfo.InvariantCulture));
            //    Console.WriteLine(String.Format("{0:dd/MM/yyyy}", "01/05/2015"));
            //    DevRptMain[i]["REPORT_DATE"] = String.Format("{0:dd/MM/yyyy}", DevRptMain[i]["REPORT_DATE"]);
            //}

            //foreach (DataRow item in DevRptMain.Table)
            //{
            //    if (item["REPORT_DATE"].IsNotNullOrEmpty())
            //    {
            //        DateTime dt = DateTime.ParseExact(item["REPORT_DATE"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //        Console.WriteLine(item["REPORT_DATE"]);
            //    }
            //}
            DevRptMain.Table.AcceptChanges();
            DevRptDesignAssumption = _devBll.GetTabPrintReportDesignAssumptionDetails(DevelopmentReportModel);
            DevRptlog = _devBll.GetTabPrintReportLogDetails(DevelopmentReportModel);
            DevRptShortClouser = _devBll.GetTabPrintReportShortClouserDetails(DevelopmentReportModel);
            Progress.End();
            if (DevRptMain != null)
            {
                if (DevRptMain.Count > 0)
                {
                    frmReportViewer showControlPlanRpt = new frmReportViewer(DevRptMain.ToTable(), DevRptDesignAssumption.ToTable(), DevRptlog.ToTable(), DevRptShortClouser.ToTable(), "DevelopmentPrint");
                    if (!showControlPlanRpt.ReadyToShowReport) return;
                    showControlPlanRpt.ShowDialog();
                }
                else
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
            }
            else
            {
                ShowInformationMessage(PDMsg.NoRecordsPrint);
            }
            SelectDataRowPart();
        }

        private void Edit()
        {
            try
            {
                if (EditButtonIsEnable == false) return;
                if (DevelopmentReportModel.PartNo.IsNotNullOrEmpty() && DevelopmentReportModel.RunNo.IsNotNullOrEmpty())
                {
                    if (isSaved == false)
                        if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            Save();
                            if (isError == true) return;
                        }
                }
                ButtonEnable = Visibility.Visible;
                AddButtonIsEnable = true;
                EditButtonIsEnable = false;
                PrintButtonIsEnable = true;
                DevelopmentReportModel.PartNo = "";
                DevelopmentReportModel.PartNoDesc = "";
                DevelopmentReportModel.RunNo = "";
                IsReadOnlyRun = true;
                DevelopmentReportModel.DADRep = "";
                DevelopmentReportModel.ZapRep = "";
                DevelopmentReportModel.RunDate = null;
                DevelopmentReportModel.ToolRWDesignTotal = "";
                DevelopmentReportModel.ToolRWMfgTotal = "";
                DevelopmentReportModel.NoOfForginShift = "";
                DevelopmentReportModel.NatureOfComplaint = "";
                DevelopmentReportModel.LabelNatureOfComplaint = "Yes / No";
                DevelopmentReportModel.IsDoYouHaveCustComplaint = false;
                DevelopmentReportModel.RecordOfCFTDiscussion = "";
                _devBll.GetPartNoDetails(DevelopmentReportModel);
                _devBll.GetTabGridDetails(DevelopmentReportModel);
                setRights();
                mdiChild.Title = ApplicationTitle + " - Development Report" + ((DevelopmentReportModel.PartNo.IsNotNullOrEmpty()) ? " - " + DevelopmentReportModel.PartNo : "");
                FocusButton = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void Add()
        {
            try
            {
                if (AddButtonIsEnable == false) return;
                if (DevelopmentReportModel.PartNo.IsNotNullOrEmpty() && DevelopmentReportModel.RunNo.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        if (isError == true) return;
                    }
                }
                ButtonEnable = Visibility.Collapsed;
                IsReadOnlyRun = true;
                AddButtonIsEnable = false;
                EditButtonIsEnable = true;
                PrintButtonIsEnable = false;
                DevelopmentReportModel.RunDate = null;
                DevelopmentReportModel.PartNo = "";
                DevelopmentReportModel.PartNoDesc = "";
                DevelopmentReportModel.RunNo = "";
                DevelopmentReportModel.DADRep = "";
                DevelopmentReportModel.ZapRep = "";
                DevelopmentReportModel.ToolRWDesignTotal = "";
                DevelopmentReportModel.ToolRWMfgTotal = "";
                DevelopmentReportModel.NoOfForginShift = "";
                DevelopmentReportModel.NatureOfComplaint = "";
                DevelopmentReportModel.LabelNatureOfComplaint = "Yes / No";
                DevelopmentReportModel.IsDoYouHaveCustComplaint = false;
                DevelopmentReportModel.RecordOfCFTDiscussion = "";
                _devBll.GetPartNoDetails(DevelopmentReportModel);
                _devBll.GetTabGridDetails(DevelopmentReportModel);
                setRights();
                mdiChild.Title = ApplicationTitle + " - Development Report" + ((DevelopmentReportModel.PartNo.IsNotNullOrEmpty()) ? " - " + DevelopmentReportModel.PartNo : "");
                FocusButton = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
        public DevelopmentReportModel DevelopmentReportModel
        {
            get
            {
                return _devModel;
            }
            set
            {
                this._devModel = value;
                NotifyPropertyChanged("DevelopmentReportModel");
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

        private ObservableCollection<DropdownColumns> _dropDownItemsRun;
        public ObservableCollection<DropdownColumns> DropDownItemsRun
        {
            get
            {
                return _dropDownItemsRun;
            }
            set
            {
                this._dropDownItemsRun = value;
                NotifyPropertyChanged("DropDownItemsRun");
            }
        }
        private void SetdropDownItems()
        {
            try
            {
                DropDownItemsPart = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "PART NO", ColumnWidth = "30*" },
                            new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "DESCRIPTION", ColumnWidth = "70*" }
                        };
                DropDownItemsRun = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "DEV_RUN_NO", ColumnDesc = "RUN NO", ColumnWidth = "1*" },
                                                    };
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private DataRowView _selectedrowunNo;
        public DataRowView SelectedRowRunNo
        {
            get
            {
                return _selectedrowunNo;
            }

            set
            {
                _selectedrowunNo = value;
            }
        }
        private readonly ICommand selectChangeComboCommandPartNo;
        public ICommand SelectChangeComboCommandPartNo { get { return this.selectChangeComboCommandPartNo; } }
        private void SelectDataRowPart()
        {
            DevelopmentReportModel.PartNo = "";
            DevelopmentReportModel.PartNoDesc = "";
            DevelopmentReportModel.RunNo = "";
            IsReadOnlyRun = true;
            DevelopmentReportModel.DADRep = "";
            DevelopmentReportModel.ZapRep = "";
            DevelopmentReportModel.RunDate = null;
            DevelopmentReportModel.ToolRWDesignTotal = "";
            DevelopmentReportModel.ToolRWMfgTotal = "";
            DevelopmentReportModel.NoOfForginShift = "";
            DevelopmentReportModel.NatureOfComplaint = "";
            DevelopmentReportModel.LabelNatureOfComplaint = "Yes / No";
            DevelopmentReportModel.IsDoYouHaveCustComplaint = false;
            DevelopmentReportModel.RecordOfCFTDiscussion = "";
            isSaved = false;
            if (this.SelectedRowPart != null)
            {
                if (SelectedRowPart.IsNotNullOrEmpty())
                {
                    DevelopmentReportModel.PartNo = this.SelectedRowPart["PART_NO"].ToString();
                    DevelopmentReportModel.PartNoDesc = this.SelectedRowPart["PART_DESC"].ToString();
                    if (AddButtonIsEnable == true)
                    {
                        _devBll.GetRunNoDetails(DevelopmentReportModel);
                        _devBll.GetTabGridDetails(DevelopmentReportModel);


                    }
                    else if (AddButtonIsEnable == false)
                    {
                        if (String.IsNullOrEmpty(DevelopmentReportModel.PartNo)) ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                        DevelopmentReportModel.RunNo = _devBll.GenerateRunningNo(DevelopmentReportModel.PartNo);
                    }

                    IsReadonlyComp = (DevelopmentReportModel.IsDoYouHaveCustComplaint == true) ? false : true;

                    if (DevelopmentReportModel.DesignAssumptionDetails.IsNotNullOrEmpty())
                    {
                        DevelopmentReportModel.DesignAssumptionDetails.Table.AcceptChanges();
                        DevelopmentReportModel.DesignAssumptionDetails.RowFilter = "DEV_RUN_NO ='" + DevelopmentReportModel.RunNo + "'";
                    }
                    if (DevelopmentReportModel.LogDetails.IsNotNullOrEmpty())
                    {
                        DevelopmentReportModel.LogDetails.Table.AcceptChanges();
                        DevelopmentReportModel.LogDetails.RowFilter = "DEV_RUN_NO='" + DevelopmentReportModel.RunNo + "'";

                    }
                    if (DevelopmentReportModel.ShortClosureDetails.IsNotNullOrEmpty())
                    {
                        DevelopmentReportModel.ShortClosureDetails.Table.AcceptChanges();
                        DevelopmentReportModel.ShortClosureDetails.RowFilter = "RUN_NO='" + DevelopmentReportModel.RunNo + "'";
                    }
                    CreateNewRows();
                    DataTable dtLog = DevelopmentReportModel.LogDetails.ToTable();
                    if (DevelopmentReportModel.LogDetails.Count > 0)
                    {
                        DevelopmentReportModel.ToolRWDesignTotal = dtLog.Compute("SUM(REWORK_TOOL_DESIGN)", string.Empty).ToValueAsString();
                        DevelopmentReportModel.ToolRWMfgTotal = dtLog.Compute("SUM(REWORK_TOOL_MFG)", string.Empty).ToValueAsString();
                    }
                    else
                    {
                        DevelopmentReportModel.ToolRWDesignTotal = "0";
                        DevelopmentReportModel.ToolRWMfgTotal = "0";
                    }
                }
                if (DevelopmentReportModel.DesignAssumptionDetails.Table.Rows.Count > 0)
                    delDesignAssumption = DevelopmentReportModel.DesignAssumptionDetails.Table.Clone();
                if (DevelopmentReportModel.LogDetails.Table.Rows.Count > 0)
                    delLog = DevelopmentReportModel.LogDetails.Table.Clone();
                if (DevelopmentReportModel.ShortClosureDetails.Table.Rows.Count > 0)
                    delShortClouser = DevelopmentReportModel.ShortClosureDetails.Table.Clone();


            }

            mdiChild.Title = ApplicationTitle + " - Development Report" + ((DevelopmentReportModel.PartNo.IsNotNullOrEmpty()) ? " - " + DevelopmentReportModel.PartNo : "");
        }

        private void CreateNewRows()
        {

            #region DesignAssumptionAddNew
            if (DevelopmentReportModel.DesignAssumptionDetails.IsNotNullOrEmpty())
            {
                if (DevelopmentReportModel.DesignAssumptionDetails.Count == 0)
                {
                    DataRowView drv = DevelopmentReportModel.DesignAssumptionDetails.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = DevelopmentReportModel.DesignAssumptionDetails.Count;
                    drv["PART_NO"] = DevelopmentReportModel.PartNo;
                    drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                    drv.EndEdit();
                }
                else if (DevelopmentReportModel.DesignAssumptionDetails[DevelopmentReportModel.DesignAssumptionDetails.Count - 1]["CFT_DISS"].ToString() != "" || DevelopmentReportModel.DesignAssumptionDetails[DevelopmentReportModel.DesignAssumptionDetails.Count - 1]["JUSTIFICATION"].ToString() != "" || DevelopmentReportModel.DesignAssumptionDetails[DevelopmentReportModel.DesignAssumptionDetails.Count - 1]["TGR"].ToString() != "" || DevelopmentReportModel.DesignAssumptionDetails[DevelopmentReportModel.DesignAssumptionDetails.Count - 1]["TWG"].ToString() != "")
                {
                    DataRowView drv = DevelopmentReportModel.DesignAssumptionDetails.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = DevelopmentReportModel.DesignAssumptionDetails.Count;
                    drv["PART_NO"] = DevelopmentReportModel.PartNo;
                    drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                    drv.EndEdit();
                }

            }

            #endregion

            #region LogDetailsAddNew
            if (DevelopmentReportModel.LogDetails.IsNotNullOrEmpty())
            {
                if (DevelopmentReportModel.LogDetails.Count == 0)
                {
                    DataRowView drv = DevelopmentReportModel.LogDetails.AddNew();
                    drv.BeginEdit();
                    drv["STAGE_NO"] = DevelopmentReportModel.LogDetails.Count;
                    drv["PART_NO"] = DevelopmentReportModel.PartNo;
                    drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                    drv.EndEdit();
                }
                else if (DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["PROBLEM_FACED"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["COUNTER_MEASURE"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REWORK_TOOL_DESIGN"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REWORK_TOOL_MFG"].ToString() != "" || DevelopmentReportModel.LogDetails[DevelopmentReportModel.LogDetails.Count - 1]["REMARKS"].ToString() != "")
                {
                    DataRowView drv = DevelopmentReportModel.LogDetails.AddNew();
                    drv.BeginEdit();
                    drv["STAGE_NO"] = DevelopmentReportModel.LogDetails.Count;
                    drv["PART_NO"] = DevelopmentReportModel.PartNo;
                    drv["DEV_RUN_NO"] = DevelopmentReportModel.RunNo;
                    drv.EndEdit();
                }
            }

            #endregion

            #region ShortClouserAddNew
            if (DevelopmentReportModel.ShortClosureDetails.IsNotNullOrEmpty())
            {
                if (DevelopmentReportModel.ShortClosureDetails.Count == 0)
                {
                    DataRowView drv = DevelopmentReportModel.ShortClosureDetails.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = DevelopmentReportModel.ShortClosureDetails.Count;
                    drv["PART_NO"] = DevelopmentReportModel.PartNo;
                    drv["RUN_NO"] = DevelopmentReportModel.RunNo;
                    drv.EndEdit();
                }
                else if (DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["REASON"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["WHY"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["SHORT_ACTION"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["SHORT_ACTION_DATE"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["LONG_ACTION"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["TRIAL_DATE"].ToString() != "" || DevelopmentReportModel.ShortClosureDetails[DevelopmentReportModel.ShortClosureDetails.Count - 1]["LONG_ACTION_DATE"].ToString() != "")
                {
                    DataRowView drv = DevelopmentReportModel.ShortClosureDetails.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = DevelopmentReportModel.ShortClosureDetails.Count;
                    drv["PART_NO"] = DevelopmentReportModel.PartNo;
                    drv["RUN_NO"] = DevelopmentReportModel.RunNo;
                    drv.EndEdit();
                }
            }

            #endregion

        }
        private readonly ICommand selectChangeComboCommandRunNo;
        public ICommand SelectChangeComboCommandRunNo { get { return this.selectChangeComboCommandRunNo; } }
        private void SelectDataRowRunNo()
        {
            if (this.SelectedRowRunNo != null)
            {
                if (SelectedRowRunNo.IsNotNullOrEmpty())
                {
                    DevelopmentReportModel.RunNo = this.SelectedRowRunNo["DEV_RUN_NO"].ToString();
                    _devBll.GetTabMainFormDetails(DevelopmentReportModel);
                    DevelopmentReportModel.DesignAssumptionDetails.RowFilter = "DEV_RUN_NO ='" + DevelopmentReportModel.RunNo + "'";
                    DevelopmentReportModel.LogDetails.RowFilter = "DEV_RUN_NO='" + DevelopmentReportModel.RunNo + "'";
                    DevelopmentReportModel.ShortClosureDetails.RowFilter = "RUN_NO='" + DevelopmentReportModel.RunNo + "'";

                    CreateNewRows();
                    DataTable dtLog = DevelopmentReportModel.LogDetails.ToTable();
                    if (DevelopmentReportModel.LogDetails.Count > 0)
                    {
                        DevelopmentReportModel.ToolRWDesignTotal = dtLog.Compute("SUM(REWORK_TOOL_DESIGN)", string.Empty).ToValueAsString();
                        DevelopmentReportModel.ToolRWMfgTotal = dtLog.Compute("SUM(REWORK_TOOL_MFG)", string.Empty).ToValueAsString();
                    }
                    else
                    {
                        DevelopmentReportModel.ToolRWDesignTotal = "0";
                        DevelopmentReportModel.ToolRWMfgTotal = "0";
                    }
                }
            }
        }
        private void showDummy()
        {
            try
            {
                frmDummy dummy = new frmDummy();
                dummy.ShowDialog();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
    }
}
