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
    public class ManufacReportViewModel : ViewModelBase
    {
        private UserInformation userInformation;

        private ManufacReportBll bll;
        public Action CloseAction { get; set; }
        private readonly ICommand _onAddCommand;
        private readonly ICommand _onEditCommand;
        private readonly ICommand _onSaveCommand;
        private readonly ICommand _onCloseCommand;
        private readonly ICommand _onPrintCommand;

        public ICommand OnAddCommand { get { return this._onAddCommand; } }
        public ICommand OnEditViewCommand { get { return this._onEditCommand; } }
        public ICommand OnSaveCommand { get { return this._onSaveCommand; } }
        public ICommand OnPrintCommand { get { return this._onPrintCommand; } }
        public ICommand OnCloseCommand { get { return this._onCloseCommand; } }

        private readonly ICommand _deleteCommand;
        public ICommand DeleteCommand { get { return this._deleteCommand; } }


        public DataRowView DesignSelectedItem { get; set; }
        public DataRowView DifficultiesSelectedItem { get; set; }
        public DataRowView IssuesSelectedItem { get; set; }
        public DataRowView ProcessSelectedItem { get; set; }
        public DataRowView OutputSelectedItem { get; set; }

        public DataRowView CustomerSelectedRow { get; set; }

        private bool _addButtonIsEnable = true;
        private bool _editButtonIsEnable = true;
        private bool _saveButtonIsEnable = true;
        private bool _printButtonIsEnable = true;

        WPF.MDI.MdiChild mdiChild;

        public ManufacReportViewModel(UserInformation userInfo, WPF.MDI.MdiChild me)
        {
            try
            {
                mdiChild = me;
                userInformation = new UserInformation();
                userInformation = userInfo;
                this.bll = new ManufacReportBll(userInfo);
                this.ManufacReport = new ManufacReportModel();


                this._deleteCommand = new DelegateCommand<DataGrid>(this.Delete);
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
                this.partNoSelectionChangedCommand = new DelegateCommand(this.PartNoSelectionChanged);
                this.devlptRptCommand = new DelegateCommand(this.DevlptRpt);

                SetdropDownItems();
                GetRights();
                Add();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
        private bool formType = false;
        public ManufacReportViewModel(UserInformation userInfo, WPF.MDI.MdiChild me, string partNo)
        {
            try
            {
                mdiChild = me;
                userInformation = new UserInformation();
                userInformation = userInfo;
                this.bll = new ManufacReportBll(userInfo);
                this.ManufacReport = new ManufacReportModel();

                this._deleteCommand = new DelegateCommand<DataGrid>(this.Delete);
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
                this.partNoSelectionChangedCommand = new DelegateCommand(this.PartNoSelectionChanged);
                this.devlptRptCommand = new DelegateCommand(this.DevlptRpt);

                SetdropDownItems();
                GetRights();
                Add();
                ManufacReport.PartNo = partNo;
                formType = true;
                ManufacReport.PartNoDetails.RowFilter = "Part_no='" + partNo + "'";
                if (ManufacReport.PartNoDetails.Count > 0)
                {
                    SelectedRowPart = ManufacReport.PartNoDetails[0];
                    PartNoSelectionChanged();
                }
                ManufacReport.PartNoDetails.RowFilter = string.Empty;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private DataView _customers = null;
        public DataView CustomersDataSource
        {
            get
            {
                return _customers;
            }
            set
            {
                _customers = value;
                NotifyPropertyChanged("CustomersDataSource");
            }
        }

        private DataView _rmbasic = null;
        public DataView RMBasic
        {
            get
            {
                return _rmbasic;
            }
            set
            {
                _rmbasic = value;
                NotifyPropertyChanged("RMBasic");
            }
        }

        private DataView _ccmaster = null;
        public DataView CCMaster
        {
            get
            {
                return _ccmaster;
            }
            set
            {
                _ccmaster = value;
                NotifyPropertyChanged("CCMaster");
            }
        }

        private readonly ICommand productSearchCommand = null;
        public ICommand ProductSearchCommand { get { return this.productSearchCommand; } }
        private void ProductSearch()
        {
            try
            {
                if (!ManufacReport.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

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
                MainMDI.Container.Children.Add(mdiProductSearch);
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
                if (!ManufacReport.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                showDummy();
                MdiChild mdiCostSheetSearch = new MdiChild();
                ProcessDesigner.frmCostSheetSearch costSheetSearch = new ProcessDesigner.frmCostSheetSearch(userInformation, mdiCostSheetSearch);
                mdiCostSheetSearch.Title = ApplicationTitle + " - Cost Sheet Search";
                mdiCostSheetSearch.Content = costSheetSearch;
                mdiCostSheetSearch.Height = costSheetSearch.Height + 40;
                mdiCostSheetSearch.Width = costSheetSearch.Width + 20;
                mdiCostSheetSearch.MinimizeBox = false;
                mdiCostSheetSearch.MaximizeBox = false;
                mdiCostSheetSearch.Resizable = false;
                MainMDI.Container.Children.Add(mdiCostSheetSearch);

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
                if (!ManufacReport.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild mdiToolsInfo = new MdiChild();
                ProcessDesigner.frmToolsInfo toolsinfo = new ProcessDesigner.frmToolsInfo(userInformation, mdiToolsInfo);
                mdiToolsInfo.Title = ApplicationTitle + " - Tools Information";
                mdiToolsInfo.Content = toolsinfo;
                mdiToolsInfo.Height = toolsinfo.Height + 23;
                mdiToolsInfo.Width = toolsinfo.Width + 20;
                MainMDI.Container.Children.Add(mdiToolsInfo);

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
                if (!ManufacReport.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild psheet = new MdiChild();
                psheet.Title = ApplicationTitle + " - Process Sheet";
                frmProcessSheet processsheet = null;
                if (MainMDI.IsFormAlreadyOpen("Process Sheet - " + ManufacReport.PartNo.Trim()) == false)
                {
                    processsheet = new frmProcessSheet(psheet, userInformation, ManufacReport.PartNo, ManufacReport.PartNoDesc);
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
                    psheet = (MdiChild)MainMDI.GetFormAlreadyOpened("Process Sheet - " + ManufacReport.PartNo.Trim());
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
                if (!ManufacReport.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild drwMaster = new MdiChild();
                drwMaster.Title = ApplicationTitle + " - Drawings";
                ProcessDesigner.frmDrawings drawings = null;
                if (MainMDI.IsFormAlreadyOpen("Drawings - " + ManufacReport.PartNo.Trim()) == false)
                {
                    drawings = new ProcessDesigner.frmDrawings(drwMaster, userInformation, ManufacReport.PartNo);
                    drwMaster.Content = drawings;
                    drwMaster.Height = drawings.Height + 40;
                    drwMaster.Width = drawings.Width + 20;
                    drwMaster.MinimizeBox = false;
                    drwMaster.MaximizeBox = false;
                    drwMaster.Resizable = false;
                    if (MainMDI.IsFormAlreadyOpen("Drawings - " + ManufacReport.PartNo.Trim()) == false)
                    {
                        MainMDI.Container.Children.Add(drwMaster);
                    }
                    else
                    {
                        drwMaster = new MdiChild();
                        drwMaster = (MdiChild)MainMDI.GetFormAlreadyOpened("Drawings - " + ManufacReport.PartNo.Trim());
                        MainMDI.SetMDI(drwMaster);
                    }
                }
                else
                {
                    drwMaster = new MdiChild();
                    drwMaster = (MdiChild)MainMDI.GetFormAlreadyOpened("Drawings  - " + ManufacReport.PartNo.Trim());
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
                if (!ManufacReport.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild tschedule = null;
                frmToolSchedule_new toolschedule = null;
                if (MainMDI.IsFormAlreadyOpen("Tool Schedule - " + ManufacReport.PartNo.Trim()) == false)
                {
                    tschedule = new MdiChild();
                    tschedule.Title = ApplicationTitle + " - Tool Schedule";
                    toolschedule = new frmToolSchedule_new(userInformation, tschedule, ManufacReport.PartNo);
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
                    tschedule = (MdiChild)MainMDI.GetFormAlreadyOpened("Tool Schedule - " + ManufacReport.PartNo.Trim());
                    toolschedule = (frmToolSchedule_new)tschedule.Content;
                    MainMDI.SetMDI(tschedule);
                }
                toolschedule.EditSelectedPartNo(ManufacReport.PartNo);
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
                if (!ManufacReport.PartNo.IsNotNullOrEmpty())
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
                if (MainMDI.IsFormAlreadyOpen("Control Plan - " + ManufacReport.PartNo.Trim()) == false)
                {

                    cplanscreen = new frmPCCS(userInformation, cplan, ManufacReport.PartNo);
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
                    cplan = (MdiChild)MainMDI.GetFormAlreadyOpened("Control Plan - " + ManufacReport.PartNo.Trim());
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
                if (!ManufacReport.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                frmCostSheet costSheet = new frmCostSheet(userInformation, ManufacReport.PartNo, -9999);
                costSheet.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand devlptRptCommand = null;
        public ICommand DevlptRptCommand { get { return this.devlptRptCommand; } }
        private void DevlptRpt()
        {
            try
            {
                if (!ManufacReport.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                showDummy();
                MdiChild devRptmdi = new MdiChild();
                devRptmdi.Title = ApplicationTitle + "Development Report";
                frmDevelopmentReport devReport = null;
                if (MainMDI.IsFormAlreadyOpen("Development Report - " + ManufacReport.PartNo.Trim()) == false)
                {
                    devReport = new frmDevelopmentReport(userInformation, devRptmdi, ManufacReport.PartNo);
                    devRptmdi.Content = devReport;
                    devRptmdi.Height = devReport.Height + 40;
                    devRptmdi.Width = devReport.Width + 20;
                    devRptmdi.Resizable = false;
                    devRptmdi.MinimizeBox = true;
                    devRptmdi.MaximizeBox = true;
                    MainMDI.Container.Children.Add(devRptmdi);
                }
                else
                {
                    devRptmdi = new MdiChild();
                    devRptmdi = (MdiChild)MainMDI.GetFormAlreadyOpened("Development Report -" + ManufacReport.PartNo.Trim());
                    devReport = (frmDevelopmentReport)devRptmdi.Content;
                    MainMDI.SetMDI(devRptmdi);
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void Delete(DataGrid dg)
        {
            try
            {

                if (dg.Name == "dgrdDesign")
                {
                    if (DesignSelectedItem != null)
                    {
                        DesignSelectedItem.Delete();
                        AddNewRowDesign();
                    }
                }
                else if (dg.Name == "dgrdDifficulties")
                {
                    if (DifficultiesSelectedItem != null)
                    {
                        DifficultiesSelectedItem.Delete();
                        AddNewRowDifficulties();
                    }
                }
                else if (dg.Name == "dgrdPreQual")
                {
                    if (IssuesSelectedItem != null)
                    {
                        IssuesSelectedItem.Delete();
                        AddNewRowIssues();
                    }
                }
                else if (dg.Name == "dgrdProcess")
                {
                    if (ProcessSelectedItem != null)
                    {
                        ProcessSelectedItem.Delete();
                        AddNewRowProcess();
                    }
                }
                else if (dg.Name == "dgrdOutput")
                {
                    if (OutputSelectedItem != null)
                    {
                        OutputSelectedItem.Delete();
                        AddNewRowOutput();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
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
            ActionPermission = bll.GetUserRights("MANUFACTURING REPORT");
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

        private void Save()
        {
            if (SaveButtonIsEnable == false) return;

            FocusButton = true;
            if (ManufacReport.PartNo == "")
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                FocusButton = true;
                return;
            }

            if (ManufacReport.START_DATE != null && ManufacReport.END_DATE != null)
            {
                DateTime startdt = Convert.ToDateTime(ManufacReport.START_DATE);
                DateTime enddt = Convert.ToDateTime(ManufacReport.END_DATE);

                //if (System.DateTime.ParseExact(startdt.ToString("dd/MM/yyyy hh:mm"), "dd/MM/yyyy hh:mm", null) >= System.DateTime.ParseExact(enddt.ToString("dd/MM/yyyy hh:mm"), "dd/MM/yyyy hh:mm", null))
                // if (System.DateTime.ParseExact(startdt.ToString(), "dd/MM/yyyy", null) >= System.DateTime.ParseExact(enddt.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)) 
                if (startdt.Date > enddt.Date)
                {
                    ShowInformationMessage("Start date should not be greater than End date.");
                    FocusButton = true;
                    return;
                }
            }

            Progress.ProcessingText = PDMsg.ProgressUpdateText;
            Progress.Start();
            bll.SaveManufacturingRpt(ManufacReport);
            Progress.End();
            if (ManufacReport.ActionMode == OperationMode.AddNew)
            {
                MessageBox.Show(PDMsg.SavedSuccessfully, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(PDMsg.UpdatedSuccessfully, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            }
            FocusButton = true;
            //ClearValues();
        }


        private void Print()
        {
            if (ManufacReport.PartNo == "")
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            Progress.Start();
            DataSet dsManufactrpt = bll.GetManufactrptDetails(ManufacReport.PartNo);
            Progress.End();

            if (dsManufactrpt != null)
            {
                if (dsManufactrpt.Tables.Count > 0 && dsManufactrpt.Tables["MainReport"].Rows.Count > 0)
                {
                    //dsManufactrpt.Tables["MainReport"].WriteXmlSchema(@"E:\ManufactMain.xml");
                    //dsManufactrpt.Tables["Design"].WriteXmlSchema(@"E:\ManufactDesign.xml");
                    //dsManufactrpt.Tables["Difficulties"].WriteXmlSchema(@"E:\ManufactDifficulties.xml");
                    //dsManufactrpt.Tables["PreQual"].WriteXmlSchema(@"E:\ManufactPreQual.xml");
                    //dsManufactrpt.Tables["Process"].WriteXmlSchema(@"E:\ManufactProcess.xml");
                    //dsManufactrpt.Tables["Output"].WriteXmlSchema(@"E:\ManufactOutput.xml");

                    frmReportViewer showRpt = new frmReportViewer(dsManufactrpt, "ManufacturePrint");
                    if (!showRpt.ReadyToShowReport) return;
                    showRpt.Show();
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

        }

        private void Edit()
        {
            try
            {
                if (EditButtonIsEnable == false) return;

                if (ManufacReport.PartNo.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        //  return;
                    }
                }

                ButtonEnable = Visibility.Visible;
                AddButtonIsEnable = true;
                EditButtonIsEnable = false;
                PrintButtonIsEnable = true;
                ManufacReport.ActionMode = OperationMode.Edit;
                bll.GetPartNoDetails(ManufacReport);
                CustomersDataSource = bll.GetCustomerDetails().ToDataTable<DDCUST_MAST>().DefaultView;
                RMBasic = bll.GetRawMaterial().ToDataTable<DDRM_MAST>().DefaultView;
                CCMaster = bll.GetCCMaster().ToDataTable<DDCOST_CENT_MAST>().DefaultView;
                setRights();
                ClearValues();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Add()
        {
            try
            {
                if (AddButtonIsEnable == false) return;

                if (ManufacReport.PartNo.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        // return;
                    }
                }
                ButtonEnable = Visibility.Collapsed;
                IsReadOnlyRun = true;
                AddButtonIsEnable = false;
                EditButtonIsEnable = true;
                PrintButtonIsEnable = false;
                ManufacReport.ActionMode = OperationMode.AddNew;
                bll.GetPartNoDetails(ManufacReport);
                CustomersDataSource = bll.GetCustomerDetails().ToDataTable<DDCUST_MAST>().DefaultView;
                RMBasic = bll.GetRawMaterial().ToDataTable<DDRM_MAST>().DefaultView;
                CCMaster = bll.GetCCMaster().ToDataTable<DDCOST_CENT_MAST>().DefaultView;
                setRights();
                ClearValues();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void ClearValues()
        {
            ManufacReport.PartNo = "";
            ManufacReport.PartNoDesc = "";
            ManufacReport.MACHINE = "";
            ManufacReport.CUST_NAME = "";
            ManufacReport.MATERIAL = "";
            ManufacReport.RM_CD = "";
            ManufacReport.WIRE_SIZE = null;
            ManufacReport.ROD_DIA = "";
            ManufacReport.UTS_YP = "";
            ManufacReport.HEAT_NO = null;
            ManufacReport.COATING = "";
            ManufacReport.QTY_PLANNED = "";
            ManufacReport.QTY_FORGED = "";
            ManufacReport.SETTING_SCRAP = "";
            ManufacReport.START_DATE = null;
            ManufacReport.END_DATE = null;
            ManufacReport.DURATION = "";
            ManufacReport.POST_APPROVAL = "";
            ManufacReport.BULK_PRODUCTION = "";
            ManufacReport.PREPARED_DD = "";
            ManufacReport.FORGING = "";
            ManufacReport.TOOL_MANAGEMENT = "";
            ManufacReport.QUALITY_ASSURANCE = "";
            ManufacReport.OTHERS = "";
            ManufacReport.POST_APPR_YES = false;
            ManufacReport.POST_APPR_NO = false;
            ManufacReport.POST_APPR_NA = false;
            ManufacReport.BULK_PROD_YES = false;
            ManufacReport.BULK_PROD_NO = false;
            ManufacReport.BULK_PROD_NA = false;
            ManufacReport.DVDesign = null;
            ManufacReport.DVDifficulties = null;
            ManufacReport.DVOutput = null;
            ManufacReport.DVPreQual = null;
            ManufacReport.DVProcess = null;
            mdiChild.Title = ApplicationTitle + " - Manufacturing Report" + ((ManufacReport.PartNo.IsNotNullOrEmpty()) ? " - " + ManufacReport.PartNo : "");
        }

        private ManufacReportModel _manufacreport;
        public ManufacReportModel ManufacReport
        {
            get
            {
                return _manufacreport;
            }
            set
            {
                this._manufacreport = value;
                NotifyPropertyChanged("ManufacReport");
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

        private ObservableCollection<DropdownColumns> _customerDropDownItems;
        public ObservableCollection<DropdownColumns> CustomerDropDownItems
        {
            get
            {
                return _customerDropDownItems;
            }
            set
            {
                _customerDropDownItems = value;
                OnPropertyChanged("CustomerDropDownItems");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderRM = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderRM
        {
            get { return this._dropDownHeaderRM; }
            set
            {
                this._dropDownHeaderRM = value;
                NotifyPropertyChanged("DropDownHeaderRM");
            }
        }

        private ObservableCollection<DropdownColumns> _dropdownheaderccmast = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderCCMast
        {
            get { return this._dropdownheaderccmast; }
            set
            {
                this._dropdownheaderccmast = value;
                NotifyPropertyChanged("DropDownHeaderCCMast");
            }
        }

        private void SetdropDownItems()
        {
            try
            {
                DropDownItemsPart = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "PART NO", ColumnWidth = 100 },
                            new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "DESCRIPTION", ColumnWidth = "1*" }
                        };

                CustomerDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "CUST_CODE", ColumnDesc = "Customer Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "CUST_NAME", ColumnDesc = "Customer", ColumnWidth = "75*", IsDefaultSearchColumn = true }
                        };

                DropDownHeaderRM = new ObservableCollection<DropdownColumns>
                         {               
                        new DropdownColumns { ColumnName = "RM_CODE", ColumnDesc = "RM Code", ColumnWidth = 100 },
                        new DropdownColumns { ColumnName = "RM_DESC", ColumnDesc = "RM Description", ColumnWidth = "1*" }
                         };

                DropDownHeaderCCMast = new ObservableCollection<DropdownColumns>
                         {               
                        new DropdownColumns { ColumnName = "COST_CENT_CODE", ColumnDesc = "CC Code", ColumnWidth = 80 },
                        new DropdownColumns { ColumnName = "COST_CENT_DESC", ColumnDesc = "Cost Center", ColumnWidth = "1*" }                        
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
        private readonly ICommand partNoSelectionChangedCommand;
        public ICommand PartNoSelectionChangedCommand { get { return this.partNoSelectionChangedCommand; } }
        private void PartNoSelectionChanged()
        {
            if (SelectedRowPart.IsNotNullOrEmpty())
            {
                ManufacReport.PartNo = this.SelectedRowPart["PART_NO"].ToString();
                ManufacReport.PartNoDesc = this.SelectedRowPart["PART_DESC"].ToString();
                bll.GetManufacturingDetails(ManufacReport);
                if (ManufacReport.ActionMode == OperationMode.AddNew)
                {
                    ButtonEnable = Visibility.Collapsed;
                    AddButtonIsEnable = false;
                    EditButtonIsEnable = true;
                    PrintButtonIsEnable = false;
                    setRights();
                }
                else
                {
                    ButtonEnable = Visibility.Visible;
                    AddButtonIsEnable = true;
                    EditButtonIsEnable = false;
                    PrintButtonIsEnable = true;
                    setRights();
                }
                try
                {
                    List<DDCI_INFO> prd_ciref = bll.GetCIRefernceByPartNumber(ManufacReport);

                    DataView dvTemp = new DataView();
                    dvTemp = CustomersDataSource.Table.Copy().DefaultView;
                    CustomersDataSource.RowFilter = "CUST_CODE=" + prd_ciref[0].CUST_CODE;
                    CustomerSelectedRow = CustomersDataSource[0];
                    ManufacReport.CUST_NAME = CustomerSelectedRow["CUST_NAME"].ToValueAsString();

                    dvTemp = bll.GetProcessSheetDetils(ManufacReport);
                    ManufacReport.MATERIAL = dvTemp[0]["WIRE_ROD_CD"].ToValueAsString();

                    //ManufacReport.RM_CD = dvTemp[0]["RM_CD"].ToValueAsString();
                    dvTemp = RMBasic.Table.Copy().DefaultView;
                    dvTemp.RowFilter = "RM_CODE='" + ManufacReport.MATERIAL + "'";
                    ManufacReport.MATERIAL = dvTemp[0]["RM_DESC"].ToValueAsString();

                    dvTemp = bll.GetProcessSheetCCode(ManufacReport);
                    ManufacReport.WIRE_SIZE = dvTemp[0]["WIRE_SIZE_MIN"].ToValueAsString();
                    if (ManufacReport.WIRE_SIZE.IsNotNullOrEmpty())
                    {
                        ManufacReport.WIRE_SIZE = dvTemp[0]["WIRE_SIZE_MIN"].ToValueAsString().Trim() + "/" + dvTemp[0]["WIRE_SIZE_MAX"].ToValueAsString().Trim();
                    }
                    else
                    {
                        ManufacReport.WIRE_SIZE = dvTemp[0]["WIRE_SIZE_MAX"].ToValueAsString().Trim();
                    }

                }
                catch (Exception ex)
                {
                    ex.LogException();
                }

            }
            else
            {
                ClearValues();
            }
            mdiChild.Title = ApplicationTitle + " - Manufacturing Report" + ((ManufacReport.PartNo.IsNotNullOrEmpty()) ? " - " + ManufacReport.PartNo : "");

        }

        public void OnCellEditEndingDesign(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                AddNewRowDesign();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void AddNewRowDesign()
        {
            try
            {
                DataView dv = ManufacReport.DVDesign.ToTable().Copy().DefaultView;
                dv.RowFilter = "Isnull(ASSUMPTIONS,'') = '' AND Isnull(STATUS,'') ='' AND Isnull(REMARKS,'') = ''";
                if (dv.Count == 0)
                {
                    DataRowView drv = ManufacReport.DVDesign.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = ManufacReport.DVDesign.Count;
                    drv.EndEdit();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void OnCellEditEndingDifficulties(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                AddNewRowDifficulties();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void AddNewRowDifficulties()
        {
            try
            {
                DataView dv = ManufacReport.DVDifficulties.ToTable().Copy().DefaultView;
                dv.RowFilter = "Isnull(DIFFICULTIES,'') = '' AND Isnull(ACTION,'') ='' AND Isnull(STATUS,'') = ''";
                if (dv.Count == 0)
                {
                    DataRowView drv = ManufacReport.DVDifficulties.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = ManufacReport.DVDifficulties.Count;
                    drv.EndEdit();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        public void OnCellEditEndingPreQual(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                AddNewRowIssues();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void AddNewRowIssues()
        {
            try
            {
                DataView dv = ManufacReport.DVPreQual.ToTable().Copy().DefaultView;
                dv.RowFilter = "Isnull(ISSUES,'') = ''";
                if (dv.Count == 0)
                {
                    DataRowView drv = ManufacReport.DVPreQual.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = ManufacReport.DVPreQual.Count;
                    drv.EndEdit();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void OnCellEditEndingProcess(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                AddNewRowProcess();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void AddNewRowProcess()
        {
            try
            {
                DataView dv = ManufacReport.DVProcess.ToTable().Copy().DefaultView;
                dv.RowFilter = "Isnull(ISSUES_FACED,'') = '' AND Isnull(ROOT_CAUSE,'') ='' AND Isnull(CORRECTIVE_ACTION,'') = '' AND Isnull(STATUS,'') = ''";
                if (dv.Count == 0)
                {
                    DataRowView drv = ManufacReport.DVProcess.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = ManufacReport.DVProcess.Count;
                    drv.EndEdit();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void OnCellEditEndingOutput(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                AddNewRowOutput();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void AddNewRowOutput()
        {
            try
            {
                DataView dv = ManufacReport.DVOutput.ToTable().Copy().DefaultView;
                dv.RowFilter = "Isnull(REJECTION,'') = '' AND Isnull(REASON,'') ='' AND Isnull(CORRECTIVE_ACTION,'') = '' AND Isnull(STATUS,'') = ''";
                if (dv.Count == 0)
                {
                    DataRowView drv = ManufacReport.DVOutput.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = ManufacReport.DVOutput.Count;
                    drv.EndEdit();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
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

            }
        }
    }
}
