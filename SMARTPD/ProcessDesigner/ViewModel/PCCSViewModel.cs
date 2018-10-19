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


namespace ProcessDesigner.ViewModel
{
    public class PCCSViewModel : ViewModelBase
    {
        public Microsoft.Windows.Controls.DataGrid DgvPccs;
        private WPF.MDI.MdiChild mdiChild;
        private PCCSBll _pccsBll;
        private PCCSModel _pccsModel;
        private LogViewBll _logviewBll;
        //private OperationMode _operationmode;
        private UserInformation userInformation;
        private readonly ICommand _onReNumberCommand;
        private readonly ICommand _onEditCommand;
        private readonly ICommand _onSaveCommand;
        private readonly ICommand _onCloseCommand;
        private readonly ICommand _onDeleteCommand;
        private readonly ICommand _onCopyCommand;
        private readonly ICommand _onUndoCommand;
        private readonly ICommand _onPrintCommand;
        private readonly ICommand _rowEditEndingPccsCommand;
        public ICommand RowEditEndingPccsCommand { get { return this._rowEditEndingPccsCommand; } }
        private readonly ICommand _rowEditEndingPccsRevCommand;
        public ICommand RowEditEndingPccsRevCommand { get { return this._rowEditEndingPccsRevCommand; } }
        private readonly ICommand _rowBeginEditPccsRevCommand;
        public ICommand RowBeginEditPccsRevCommand { get { return this._rowBeginEditPccsRevCommand; } }

        private readonly ICommand _deleteCommandPccsGrid;
        public ICommand DeleteCommandPccsGrid { get { return this._deleteCommandPccsGrid; } }
        private readonly ICommand _enterPartNumber;
        public ICommand EnterPartNumberCmb { get { return this._enterPartNumber; } }
        private readonly ICommand _enterRouteNumber;
        public ICommand EnterRouteNumberCmb { get { return this._enterRouteNumber; } }
        private readonly ICommand _enterSeqNumber;
        public ICommand EnterSeqNumberCmb { get { return this._enterSeqNumber; } }
        private readonly ICommand _deletePartNumber;
        private ICommand DeletePartNumber { get { return this._deletePartNumber; } }

        private DataTable delPccsGrid = new DataTable();

        public Action CloseAction { get; set; }

        public ICommand OnReNumberCommand { get { return this._onReNumberCommand; } }
        public ICommand OnEditCommand { get { return this._onEditCommand; } }
        public ICommand OnPrintCommand { get { return this._onPrintCommand; } }
        public ICommand OnSaveCommand { get { return this._onSaveCommand; } }
        public ICommand OnCloseCommand { get { return this._onCloseCommand; } }
        public ICommand OnDeleteCommand { get { return this._onDeleteCommand; } }
        public ICommand OnCopyCommand { get { return this._onCopyCommand; } }
        public ICommand OnUndoCommand { get { return this._onUndoCommand; } }

        public DataRowView SelectedItemPccs { get; set; }
        public DataRowView SelectedItemPccsRev { get; set; }

        public string Sort { get; set; }
        //private bool _initLoadFlagRoute = false;
        //private bool _initLoadFlagSeq = false;
        private bool _editButtonIsEnable = true;
        private bool _saveButtonIsEnable = true;
        private bool _deleteButtonIsEnable = false;
        private bool _printButtonIsEnable = true;

        private UserInformation _userInformation;
        private byte[] _diagram;
        private bool _imageChange;
        private bool formType = false;
        public PCCSViewModel(UserInformation userInfo, WPF.MDI.MdiChild me)
        {
            try
            {
                userInformation = new UserInformation();
                userInformation = userInfo;
                _userInformation = userInfo;
                this._pccsBll = new PCCSBll(userInfo);
                this.PccsModel = new PCCSModel();
                this._logviewBll = new LogViewBll(userInfo);
                this.logmodel = new LogModel();

                this.mdiChild = me;
                this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
                this.selectChangeComboCommandRoute = new DelegateCommand(this.SelectDataRowRoute);
                this.selectChangeComboCommandSequence = new DelegateCommand(this.SelectDataRowSequence);
                //this.cellEditEndingPccsCommand = new DelegateCommand<Object>(this.CellEditEndingPccs);
                this.insertPccsCommand = new DelegateCommand(this.InsertPccs);
                this._onReNumberCommand = new DelegateCommand(this.ReNumber);
                this._onEditCommand = new DelegateCommand(this.Edit);
                this._onPrintCommand = new DelegateCommand(this.Print);
                this._onSaveCommand = new DelegateCommand(this.Save);
                this._onCloseCommand = new DelegateCommand(this.Close);
                this._onDeleteCommand = new DelegateCommand(this.Delete);
                this._onCopyCommand = new DelegateCommand(this.Copy);
                this._onUndoCommand = new DelegateCommand(this.Undo);
                this.controlPlanCommand = new DelegateCommand(this.ControlPlan);
                this.selectionChangedPccsCommand = new DelegateCommand<Object>(this.SelectionChangedPccs);

                this._deleteCommandPccsGrid = new DelegateCommand<DataRowView>(this.DeletePccsGrid);
                this._enterPartNumber = new DelegateCommand<string>(this.EnterPartNumber);
                this._enterRouteNumber = new DelegateCommand<string>(this.EnterRouteNumber);
                this._enterSeqNumber = new DelegateCommand<string>(this.EnterSeqNumber);
                this._deletePartNumber = new DelegateCommand(this.EmptyPartNumber);
                this._rowEditEndingPccsCommand = new DelegateCommand<Object>(this.RowEditEndingPccs);
                this._rowEditEndingPccsRevCommand = new DelegateCommand<Object>(this.RowEditEndingPccsRev);
                this._rowBeginEditPccsRevCommand = new DelegateCommand<Object>(this.RowBeginEditPccsRev);

                this.iSIRClickCommand = new DelegateCommand(this.iSIRReport);
                this.pswClickCommand = new DelegateCommand(this.pswReport);
                this.dimensionalClickCommand = new DelegateCommand(this.dimensionalReport);
                this.materialClickCommand = new DelegateCommand(this.materialReport);
                this.performanceClickCommand = new DelegateCommand(this.perfomanceReport);
                this.checklistClickCommand = new DelegateCommand(this.checklistReport);
                this.initialSampleInspectionReportClickCommand = new DelegateCommand(this.initialReport);
                this._flowChartClickCommand = new DelegateCommand(this.flowChartClick);
                this._showDiagramClickCommand = new DelegateCommand(this.ShowDiagramClick);
                this._hideDiagramClickCommand = new DelegateCommand(this.HideDiagramClick);
                this._generateDiagramClickCommand = new DelegateCommand(this.GenerateDiagramClick);
                this._deleteDiagramClickCommand = new DelegateCommand(this.DeleteDiagramClick);


                this.productSearchCommand = new DelegateCommand(this.ProductSearch);
                this.costSheetSearchCommand = new DelegateCommand(this.CostSheetSearch);
                this.toolsSearchCommand = new DelegateCommand(this.ToolsSearch);
                this.processSheetCommand = new DelegateCommand(this.ProcessSheet);
                this.drawingsCommand = new DelegateCommand(this.Drawings);
                this.devlptRptCommand = new DelegateCommand(this.DevlptRpt);
                this.mfgRptCommand = new DelegateCommand(this.MfgRpt);
                this.toolScheduleCommand = new DelegateCommand(this.ToolSchedule);

                LoadCmbDatas();
                SetdropDownItems();
                GetRights();
                setRights();
                TxtGenEdit = PccsModel.EditGenBtn;
                //PhotoSource = ProcessDesigner.Properties.Resources.Blank_Image.ToBitmapImage();
                ClearImage();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

        }
        private readonly ICommand selectionChangedPccsCommand;
        public ICommand SelectionChangedPccsCommand
        {
            get
            {
                return this.selectionChangedPccsCommand;
            }
        }
        private void SelectionChangedPccs(Object selecteditem)
        {
            try
            {
                //ToolSchedSubModel toolsub = new ToolSchedSubModel();
                //if (selecteditem != null)
                //{
                //    toolsub = (ToolSchedSubModel)selecteditem;
                //    _currenttoolscedsub = toolsub;
                //}
                //else
                //{
                //    _currenttoolscedsub = new ToolSchedSubModel();
                //}
                //FilterAuxToolSchedule(toolsub.TOOL_CODE.ToValueAsString());
                //if (AuxToolScheduleDetails.Count > 0)
                //{
                //    SelectedToolSchedAux = AuxToolScheduleDetails[0];
                //    NotifyPropertyChanged("SelectedToolSchedAux");
                //}
                //if (_currenttoolscedsub.TOOL_CODE.ToValueAsString().Trim() != "")
                //{
                //    AuxEnable = true;
                //}
                //else
                //{
                //    AuxEnable = false;
                //}
                //NotifyPropertyChanged("AuxEnable");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand insertPccsCommand;
        public ICommand InsertPccsCommand { get { return this.insertPccsCommand; } }
        private void InsertPccs()
        {
            try
            {
                if (SelectedRowPccsDetails != null)
                {
                    DataRowView drv = PccsModel.PCCSDetails.AddNew();
                    drv.BeginEdit();
                    drv["SNO"] = (SelectedRowPccsDetails["SNO"].ToString().ToDoubleValue() + (0.5));
                    drv.EndEdit();
                    NotifyPropertyChanged("PccsModel");
                    ReNumber();
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

        }

        private void EmptyPartNumber()
        {
            try
            {
                NewForm();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public PCCSViewModel(UserInformation userInfo, string partNo, WPF.MDI.MdiChild me)
        {
            try
            {
                userInformation = new UserInformation();
                userInformation = userInfo;
                _userInformation = userInfo;
                this._pccsBll = new PCCSBll(userInfo);
                this.PccsModel = new PCCSModel();
                this.mdiChild = me;
                this.selectChangeComboCommandPartNo = new DelegateCommand(this.SelectDataRowPart);
                this.selectChangeComboCommandRoute = new DelegateCommand(this.SelectDataRowRoute);
                this.selectChangeComboCommandSequence = new DelegateCommand(this.SelectDataRowSequence);
                this.selectionChangedPccsCommand = new DelegateCommand<Object>(this.SelectionChangedPccs);

                this._onReNumberCommand = new DelegateCommand(this.ReNumber);
                this._onEditCommand = new DelegateCommand(this.Edit);
                this._onPrintCommand = new DelegateCommand(this.Print);
                this._onSaveCommand = new DelegateCommand(this.Save);
                this._onCloseCommand = new DelegateCommand(this.Close);
                this._onDeleteCommand = new DelegateCommand(this.Delete);
                this._onCopyCommand = new DelegateCommand(this.Copy);
                this._onUndoCommand = new DelegateCommand(this.Undo);
                this.controlPlanCommand = new DelegateCommand(this.ControlPlan);

                this._deleteCommandPccsGrid = new DelegateCommand<DataRowView>(this.DeletePccsGrid);

                this._rowEditEndingPccsCommand = new DelegateCommand<Object>(this.RowEditEndingPccs);
                this._rowEditEndingPccsRevCommand = new DelegateCommand<Object>(this.RowEditEndingPccsRev);
                this._enterPartNumber = new DelegateCommand<string>(this.EnterPartNumber);
                this._enterRouteNumber = new DelegateCommand<string>(this.EnterRouteNumber);
                this._enterSeqNumber = new DelegateCommand<string>(this.EnterSeqNumber);
                this.iSIRClickCommand = new DelegateCommand(this.iSIRReport);
                this.pswClickCommand = new DelegateCommand(this.pswReport);
                this.dimensionalClickCommand = new DelegateCommand(this.dimensionalReport);
                this.materialClickCommand = new DelegateCommand(this.materialReport);
                this.performanceClickCommand = new DelegateCommand(this.perfomanceReport);
                this.checklistClickCommand = new DelegateCommand(this.checklistReport);
                this.initialSampleInspectionReportClickCommand = new DelegateCommand(this.initialReport);
                this._flowChartClickCommand = new DelegateCommand(this.flowChartClick);
                this._showDiagramClickCommand = new DelegateCommand(this.ShowDiagramClick);
                this._hideDiagramClickCommand = new DelegateCommand(this.HideDiagramClick);
                this._generateDiagramClickCommand = new DelegateCommand(this.GenerateDiagramClick);
                this._deleteDiagramClickCommand = new DelegateCommand(this.DeleteDiagramClick);
                this.insertPccsCommand = new DelegateCommand(this.InsertPccs);

                this.productSearchCommand = new DelegateCommand(this.ProductSearch);
                this.costSheetSearchCommand = new DelegateCommand(this.CostSheetSearch);
                this.toolsSearchCommand = new DelegateCommand(this.ToolsSearch);
                this.processSheetCommand = new DelegateCommand(this.ProcessSheet);
                this.drawingsCommand = new DelegateCommand(this.Drawings);
                this.devlptRptCommand = new DelegateCommand(this.DevlptRpt);
                this.mfgRptCommand = new DelegateCommand(this.MfgRpt);
                this.toolScheduleCommand = new DelegateCommand(this.ToolSchedule);
                this._logviewBll = new LogViewBll(userInfo);
                LoadCmbDatas();
                SetdropDownItems();
                GetRights();
                setRights();
                TxtGenEdit = PccsModel.EditGenBtn;
                //PhotoSource = ProcessDesigner.Properties.Resources.Blank_Image.ToBitmapImage();
                ClearImage();
                PccsModel.PartNo = partNo;
                formType = true;
                PccsModel.PartNoDetails.RowFilter = "Part_no='" + partNo + "'";
                SelectedRowPart = PccsModel.PartNoDetails[0];
                SelectDataRowPart();
                PccsModel.PartNoDetails.RowFilter = string.Empty;
                //LoadFromOtherScreens();
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

        }
        //private readonly ICommand cellEditEndingPccsCommand;
        //public ICommand CellEditEndingPccsCommand { get { return this.cellEditEndingPccsCommand; } }
        //private void CellEditEndingPccs(Object selecteditem)
        //{
        //    try
        //    {
        //        if (selecteditem != null)
        //        {
        //            SelectedItemPccs = (DataRowView)selecteditem;

        //            DataView dvFeatureCmb = PccsModel.FeatureCmb.Table.Copy().DefaultView;
        //            DataView dvProcessCmb = PccsModel.Process.Table.Copy().DefaultView;
        //            DataView dvSplCharCmb = PccsModel.SplChar.Table.Copy().DefaultView;
        //            DataView dvControlCmb = PccsModel.Control.Table.Copy().DefaultView;
        //            dvFeatureCmb.RowFilter = "FEATURE='" + SelectedItemPccs["FEATURE"].ToString() + "'";
        //            if (dvFeatureCmb.Count == 0 && (SelectedItemPccs["FEATURE"].ToString().Length > 49)) SelectedItemPccs["FEATURE"] = "";
        //            dvFeatureCmb.RowFilter = string.Empty;

        //            dvProcessCmb.RowFilter = "PROCESS_FEATURE='" + SelectedItemPccs["PROCESS_FEATURE"].ToString() + "'";
        //            if (dvProcessCmb.Count == 0 && (SelectedItemPccs["PROCESS_FEATURE"].ToString().Length > 49)) SelectedItemPccs["PROCESS_FEATURE"] = "";
        //            dvProcessCmb.RowFilter = string.Empty;

        //            dvSplCharCmb.RowFilter = "SPEC_CHAR='" + SelectedItemPccs["SPEC_CHAR"].ToString() + "'";
        //            if (dvSplCharCmb.Count == 0 && (SelectedItemPccs["SPEC_CHAR"].ToString().Length > 9)) SelectedItemPccs["SPEC_CHAR"] = "";
        //            dvSplCharCmb.RowFilter = string.Empty;

        //            dvControlCmb.RowFilter = "CONTROL_METHOD='" + SelectedItemPccs["CONTROL_METHOD"].ToString() + "'";
        //            if (dvControlCmb.Count == 0 && (SelectedItemPccs["CONTROL_METHOD"].ToString().Length > 39)) SelectedItemPccs["CONTROL_METHOD"] = "";
        //            dvControlCmb.RowFilter = string.Empty;

        //            if (SelectedItemPccs.IsNotNullOrEmpty()) SelectedItemPccs["CTRL_SPEC_MIN"] = SelectedItemPccs["SPEC_MIN"].ToString();
        //            if (SelectedItemPccs.IsNotNullOrEmpty()) SelectedItemPccs["CTRL_SPEC_Max"] = SelectedItemPccs["SPEC_MAX"].ToString();

        //            if (SelectedItemPccs["SNO"].ToString() == "") SelectedItemPccs["SNO"] = existingsNo;
        //            if (SelectedItemPccs["FEATURE"].ToString() == "" && (SelectedItemPccs["PART_NO"].ToString() != "" && SelectedItemPccs["ROUTE_NO"].ToString() != "" && SelectedItemPccs["SEQ_NO"].ToString() != "")) SelectedItemPccs["FEATURE"] = existingsFeature;
        //            NotifyPropertyChanged("PccsModel");
        //            if (SelectedItemPccs["FEATURE"].ToString() != "")
        //            {
        //                //if (!SelectedItemPccs["SNO"].ToString().IsNotNullOrEmpty())
        //                //    SelectedItemPccs["SNO"] = PccsModel.PCCSDetails.Count.ToValueAsString();
        //                if (PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["FEATURE"].ToString() != "")
        //                {
        //                    DataRowView drv = PccsModel.PCCSDetails.AddNew();
        //                    drv.BeginEdit();
        //                    drv["SNO"] = PccsModel.PCCSDetails.Count;
        //                    drv.EndEdit();
        //                    NotifyPropertyChanged("PccsModel");
        //                    // ReNumber();
        //                    //SelectedRowPccsDetails = PccsModel.PCCSDetails[0];
        //                    //NotifyPropertyChanged("SelectedRowPccsDetails");
        //                }

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex.LogException();
        //    }

        //}
        public void OnCellEditEndingPccsCommand(object sender, Microsoft.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            TextBox tb = e.EditingElement as TextBox;
            DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            string columnName = e.Column.SortMemberPath;

            if (tb != null)
            {
                //selecteditem.BeginEdit();
                selecteditem[columnName] = tb.Text;
                //selecteditem.EndEdit();
            }
            isFeatureCell = (e.Column.Header.ToString() == "Reaction Plan") ? true : false;
            try
            {
                if (selecteditem != null)
                {
                    SelectedItemPccs = (DataRowView)selecteditem;

                    DataView dvFeatureCmb = PccsModel.FeatureCmb.Table.Copy().DefaultView;
                    DataView dvProcessCmb = PccsModel.Process.Table.Copy().DefaultView;
                    DataView dvSplCharCmb = PccsModel.SplChar.Table.Copy().DefaultView;
                    DataView dvControlCmb = PccsModel.Control.Table.Copy().DefaultView;
                    dvFeatureCmb.RowFilter = "FEATURE='" + SelectedItemPccs["FEATURE"].ToString().Replace("'", "''") + "'";
                    if (dvFeatureCmb.Count == 0 && (SelectedItemPccs["FEATURE"].ToString().Length > 50)) SelectedItemPccs["FEATURE"] = "";
                    dvFeatureCmb.RowFilter = string.Empty;

                    dvProcessCmb.RowFilter = "PROCESS_FEATURE='" + SelectedItemPccs["PROCESS_FEATURE"].ToString().Replace("'", "''") + "'";
                    if (dvProcessCmb.Count == 0 && (SelectedItemPccs["PROCESS_FEATURE"].ToString().Length > 49)) SelectedItemPccs["PROCESS_FEATURE"] = "";
                    dvProcessCmb.RowFilter = string.Empty;

                    dvSplCharCmb.RowFilter = "SPEC_CHAR='" + SelectedItemPccs["SPEC_CHAR"].ToString().Replace("'", "''") + "'";
                    if (dvSplCharCmb.Count == 0 && (SelectedItemPccs["SPEC_CHAR"].ToString().Length > 9)) SelectedItemPccs["SPEC_CHAR"] = "";
                    dvSplCharCmb.RowFilter = string.Empty;

                    dvControlCmb.RowFilter = "CONTROL_METHOD='" + SelectedItemPccs["CONTROL_METHOD"].ToString().Replace("'", "''") + "'";
                    if (dvControlCmb.Count == 0 && (SelectedItemPccs["CONTROL_METHOD"].ToString().Length > 40)) SelectedItemPccs["CONTROL_METHOD"] = "";
                    dvControlCmb.RowFilter = string.Empty;

                    //if (SelectedItemPccs.IsNotNullOrEmpty()) SelectedItemPccs["CTRL_SPEC_MIN"] = SelectedItemPccs["SPEC_MIN"].ToString();
                    //if (SelectedItemPccs.IsNotNullOrEmpty()) SelectedItemPccs["CTRL_SPEC_Max"] = SelectedItemPccs["SPEC_MAX"].ToString();

                    //if ((existingSpecMin != ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MIN"].ToString()) && (e.Column.Header.ToString() == "Spec\nMin") && ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MIN"].ToString().IsNotNullOrEmpty() && !((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MIN"].ToValueAsString().IsNotNullOrEmpty())
                    //{
                    //    ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MIN"] = ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MIN"].ToString();
                    //}
                    //if ((existingSpecMax != ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString()) && (e.Column.Header.ToString() == "Spec\nMax") && ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString().IsNotNullOrEmpty() && !((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"].ToValueAsString().IsNotNullOrEmpty())
                    //{
                    //    ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"] = ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString();
                    //}

                    if ((e.Column.Header.ToString() == "Spec\nMin"))
                    {
                        if (((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MIN"].ToString().IsNotNullOrEmpty() && ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MIN"].ToString() == "")
                        {
                            ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MIN"] = ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MIN"].ToString();
                        }
                        else
                        {
                            // ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MIN"] = "";
                        }

                    }
                    else if ((e.Column.Header.ToString() == "Spec\nMax"))
                    {
                        if (((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString().IsNotNullOrEmpty() && ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"].ToString() == "")
                        {
                            ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"] = ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString();
                        }
                        else
                        {
                            //  ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"] = "";
                        }

                    }

                    if ((e.Column.Header.ToString() == "Ctrl Spec\nMin"))
                    {
                        if ((existingSpecMin != ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MIN"]) && (((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MIN"].ToString() != existingSpecMin))
                        {
                            isctrlSpecMinCopy = false;
                        }
                        else
                        {
                            isctrlSpecMinCopy = true;
                        }
                    }
                    else
                    {
                        isctrlSpecMinCopy = true;
                    }

                    if ((e.Column.Header.ToString() == "Ctrl Spec\nMax"))
                    {
                        if (existingSpecMax != ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"])
                        {
                            isctrlSpecMaxCopy = false;
                        }
                        else
                        {
                            isctrlSpecMaxCopy = true;
                        }
                    }
                    else
                    {
                        isctrlSpecMaxCopy = true;
                    }

                    //if ((existingSpecMax != ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString()) && (e.Column.Header.ToString() == "Spec\nMax") && ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString().IsNotNullOrEmpty() && !((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"].ToValueAsString().IsNotNullOrEmpty())
                    //{
                    //    ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"] = ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString();
                    //}

                    //if (SelectedItemPccs["SNO"].ToString() == "") SelectedItemPccs["SNO"] = existingsNo;
                    //if (SelectedItemPccs["FEATURE"].ToString() == "" && (SelectedItemPccs["PART_NO"].ToString() != "" && SelectedItemPccs["ROUTE_NO"].ToString() != "" && SelectedItemPccs["SEQ_NO"].ToString() != "")) SelectedItemPccs["FEATURE"] = existingsFeature;
                    NotifyPropertyChanged("PccsModel");
                    //if (SelectedItemPccs["FEATURE"].ToString() != "")
                    //{
                    //    //if (!SelectedItemPccs["SNO"].ToString().IsNotNullOrEmpty())
                    //    //    SelectedItemPccs["SNO"] = PccsModel.PCCSDetails.Count.ToValueAsString();
                    //    if (PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["FEATURE"].ToString() != "")
                    //    {
                    //        DataRowView drv = PccsModel.PCCSDetails.AddNew();
                    //        drv.BeginEdit();
                    //        drv["SNO"] = PccsModel.PCCSDetails.Count;
                    //        drv.EndEdit();
                    //        NotifyPropertyChanged("PccsModel");
                    //        // ReNumber();
                    //        //SelectedRowPccsDetails = PccsModel.PCCSDetails[0];
                    //        //NotifyPropertyChanged("SelectedRowPccsDetails");
                    //    }

                    // }

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private readonly ICommand productSearchCommand = null;
        public ICommand ProductSearchCommand { get { return this.productSearchCommand; } }
        private void ProductSearch()
        {
            try
            {
                if (!PccsModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                //if (!MandatoryFields.ROUTE_NO.IsNotNullOrEmpty())
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Route No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}

                //frmProductSearch productSearch = new frmProductSearch(_userInformation);
                //productSearch.Show();
                MdiChild mdiProductSearch = new MdiChild();
                ProcessDesigner.frmProductSearch productSearch = new frmProductSearch(_userInformation, mdiProductSearch);
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
                if (!PccsModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                //if (!MandatoryFields.ROUTE_NO.IsNotNullOrEmpty())
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Route No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}

                //WPF.MDI.MdiChild mdiCostSheetSearch = new WPF.MDI.MdiChild();
                //ProcessDesigner.frmCostSheetSearch costSheetSearch = new ProcessDesigner.frmCostSheetSearch(_userInformation, mdiCostSheetSearch);
                //costSheetSearch.ShowDialog();
                MdiChild mdiCostSheetSearch = new MdiChild();
                if (MainMDI.IsFormAlreadyOpen("Cost Sheet Search") == false)
                {
                    frmCostSheetSearch frmCostSheetSearch = new frmCostSheetSearch(_userInformation, mdiCostSheetSearch);
                    mdiCostSheetSearch.Title = ApplicationTitle + " - Cost Sheet Search";
                    mdiCostSheetSearch.Content = frmCostSheetSearch;
                    mdiCostSheetSearch.Height = frmCostSheetSearch.Height + 40;
                    mdiCostSheetSearch.Width = frmCostSheetSearch.Width + 20;
                    mdiCostSheetSearch.MinimizeBox = false;
                    mdiCostSheetSearch.MaximizeBox = false;
                    mdiCostSheetSearch.Resizable = false;
                    MainMDI.Container.Children.Add(mdiCostSheetSearch);
                }
                else
                {
                    mdiCostSheetSearch = (MdiChild)MainMDI.GetFormAlreadyOpened("Cost Sheet Search");
                    //toolschedule = (frmToolSchedule_new)mdiCostSheetSearch.Content;
                    MainMDI.SetMDI(mdiCostSheetSearch);
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
                if (!PccsModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild mdiToolsInfo = new MdiChild();
                if (MainMDI.IsFormAlreadyOpen("Tools Information") == false)
                {

                    ProcessDesigner.frmToolsInfo toolsinfo = new ProcessDesigner.frmToolsInfo(_userInformation, mdiToolsInfo);
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
                if (!PccsModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild psheet = new MdiChild();
                psheet.Title = ApplicationTitle + " - Process Sheet";
                frmProcessSheet processsheet = null;
                if (MainMDI.IsFormAlreadyOpen("Process Sheet - " + PccsModel.PartNo.Trim()) == false)
                {
                    processsheet = new frmProcessSheet(psheet, _userInformation, PccsModel.PartNo, PccsModel.PartDesc);
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
                    psheet = (MdiChild)MainMDI.GetFormAlreadyOpened("Process Sheet - " + PccsModel.PartNo.Trim());
                    processsheet = (frmProcessSheet)psheet.Content;
                    MainMDI.SetMDI(psheet);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderRoute = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderRoute
        {
            get { return this._dropDownHeaderRoute; }
            set
            {
                this._dropDownHeaderRoute = value;
                NotifyPropertyChanged("DropDownHeaderRoute");
            }
        }

        private readonly ICommand drawingsCommand = null;
        public ICommand DrawingsCommand { get { return this.drawingsCommand; } }
        private void Drawings()
        {
            try
            {
                if (!PccsModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild drwMaster = new MdiChild();
                drwMaster.Title = ApplicationTitle + " - Drawings";
                ProcessDesigner.frmDrawings drawings = null;
                if (MainMDI.IsFormAlreadyOpen("Drawings - " + PccsModel.PartNo.Trim()) == false)
                {
                    drawings = new ProcessDesigner.frmDrawings(drwMaster, _userInformation, PccsModel.PartNo);
                    drwMaster.Content = drawings;
                    drwMaster.Height = drawings.Height + 40;
                    drwMaster.Width = drawings.Width + 20;
                    drwMaster.MinimizeBox = false;
                    drwMaster.MaximizeBox = false;
                    drwMaster.Resizable = false;
                    if (MainMDI.IsFormAlreadyOpen("Drawings - " + PccsModel.PartNo.Trim()) == false)
                    {
                        MainMDI.Container.Children.Add(drwMaster);
                    }
                    else
                    {
                        drwMaster = new MdiChild();
                        drwMaster = (MdiChild)MainMDI.GetFormAlreadyOpened("Drawings - " + PccsModel.PartNo.Trim());
                        MainMDI.SetMDI(drwMaster);
                    }
                }
                else
                {
                    drwMaster = new MdiChild();
                    drwMaster = (MdiChild)MainMDI.GetFormAlreadyOpened("Drawings  - " + PccsModel.PartNo.Trim());
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
                if (!PccsModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild tschedule = null;
                frmToolSchedule_new toolschedule = null;
                if (MainMDI.IsFormAlreadyOpen("Tool Schedule - " + PccsModel.PartNo.Trim()) == false)
                {
                    tschedule = new MdiChild();
                    tschedule.Title = ApplicationTitle + " - Tool Schedule";
                    toolschedule = new frmToolSchedule_new(_userInformation, tschedule, PccsModel.PartNo);
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
                    tschedule = (MdiChild)MainMDI.GetFormAlreadyOpened("Tool Schedule - " + PccsModel.PartNo.Trim());
                    toolschedule = (frmToolSchedule_new)tschedule.Content;
                    MainMDI.SetMDI(tschedule);
                }
                toolschedule.EditSelectedPartNo(PccsModel.PartNo);
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
                if (!PccsModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
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

        private readonly ICommand devlptRptCommand = null;
        public ICommand DevlptRptCommand { get { return this.devlptRptCommand; } }
        private void DevlptRpt()
        {
            try
            {
                if (!PccsModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                showDummy();
                MdiChild devRptmdi = new MdiChild();
                devRptmdi.Title = ApplicationTitle + "Development Report";
                frmDevelopmentReport devReport = null;
                if (MainMDI.IsFormAlreadyOpen("Development Report - " + PccsModel.PartNo.Trim()) == false)
                {
                    devReport = new frmDevelopmentReport(_userInformation, devRptmdi, PccsModel.PartNo);
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
                    devRptmdi = (MdiChild)MainMDI.GetFormAlreadyOpened("Development Report -" + PccsModel.PartNo.Trim());
                    devReport = (frmDevelopmentReport)devRptmdi.Content;
                    MainMDI.SetMDI(devRptmdi);
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand mfgRptCommand = null;
        public ICommand MfgRptCommand { get { return this.mfgRptCommand; } }
        private void MfgRpt()
        {
            try
            {
                if (!PccsModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                showDummy();
                MdiChild mfgChild = new MdiChild();
                mfgChild.Title = ApplicationTitle + " - Manufacturing Report";
                frmManufacturingReport mfgReport = null;
                if (MainMDI.IsFormAlreadyOpen(" Manufacturing Report - " + PccsModel.PartNo.Trim()) == false)
                {
                    mfgReport = new frmManufacturingReport(_userInformation, mfgChild, PccsModel.PartNo);
                    mfgChild.Content = mfgReport;
                    mfgChild.Height = mfgReport.Height + 40;
                    mfgChild.Width = mfgReport.Width + 20;
                    mfgChild.Resizable = false;
                    mfgChild.MinimizeBox = true;
                    mfgChild.MaximizeBox = true;
                    MainMDI.Container.Children.Add(mfgChild);
                }
                else
                {
                    mfgChild = new MdiChild();
                    mfgChild = (MdiChild)MainMDI.GetFormAlreadyOpened("Manufacturing Report -" + PccsModel.PartNo.Trim());
                    mfgReport = (frmManufacturingReport)mfgChild.Content;
                    MainMDI.SetMDI(mfgChild);

                }
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
                if (!PccsModel.PartNo.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                frmCostSheet costSheet = new frmCostSheet(_userInformation, PccsModel.PartNo, -9999);
                costSheet.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
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
        private void EnterRouteNumber(string partNumber)
        {
            try
            {
                SelectDataRowRoute();
            }
            catch (Exception)
            {

            }

        }
        private void EnterSeqNumber(string partNumber)
        {
            try
            {
                SelectDataRowSequence();
            }
            catch (Exception)
            {

            }

        }


        private void DeletePccsGrid(DataRowView selecteditem)
        {
            try
            {
                int icnt = 0;
                List<String> lstSelect = new List<String>();
                if (PccsModel.PCCSDetails.Table.Columns.IndexOf("ID_NUMBER") < 0)
                    PccsModel.PCCSDetails.Table.Columns.Add("ID_NUMBER");
                for (int ictr = 0; ictr < PccsModel.PCCSDetails.Count; ictr++)
                {
                    PccsModel.PCCSDetails[ictr]["ID_NUMBER"] = ictr;
                }
                foreach (DataRowView drView in DgvPccs.SelectedItems)
                {
                    lstSelect.Add(drView["ID_NUMBER"].ToValueAsString());
                }
                MessageBoxResult messageBoxResult = MessageBoxResult.No;
                messageBoxResult = MessageBox.Show(lstSelect.Count + " record(s) will be deleted. Do you want to delete the selected record(s)?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    for (int ictr = 0; ictr <= lstSelect.Count - 1; ictr++)
                    {
                        DataRowView delView = null;
                        foreach (DataRowView dvView in PccsModel.PCCSDetails)
                        {
                            if (lstSelect[ictr] == dvView["ID_NUMBER"])
                            {
                                delView = dvView;
                                break;
                            }
                        }
                        selecteditem = delView;
                        if (selecteditem != null)
                        {

                            DataRowView row = selecteditem;
                            if (row["PART_No"].ToString().Trim().IsNotNullOrEmpty() && row["SEQ_NO"].ToString().Trim().IsNotNullOrEmpty() && row["ROUTE_NO"].ToString().Trim().IsNotNullOrEmpty())
                            {
                                delPccsGrid.ImportRow(row.Row);
                                row.Delete();
                                //PccsModel.PccsDetails.Delete(row);
                                if (PccsModel.PCCSDetails.Count == 0)
                                {
                                    DataRowView drv = PccsModel.PCCSDetails.AddNew();
                                    drv.BeginEdit();
                                    drv["SNO"] = PccsModel.PCCSDetails.Count;
                                    drv.EndEdit();
                                    PccsModel.PCCSDetails.EndInit();
                                }
                            }
                            else
                            {
                                row.Delete();
                                if (PccsModel.PCCSDetails.Count == 0)
                                {
                                    DataRowView drv = PccsModel.PCCSDetails.AddNew();
                                    drv.BeginEdit();
                                    drv["SNO"] = PccsModel.PCCSDetails.Count;
                                    drv.EndEdit();
                                    PccsModel.PCCSDetails.EndInit();
                                }
                            }
                            DgvPccs.CommitEdit();
                        }
                    }
                    PartNumberIsFocused = true;
                    if (PccsModel.PCCSDetails.Table.Columns.IndexOf("ID_NUMBER") >= 0)
                        PccsModel.PCCSDetails.Table.Columns.Remove("ID_NUMBER");

                    MessageBox.Show(lstSelect.Count + " record(s) deleted.");
                }
            }
            catch (Exception ex)
            {
            }
        }
        private string _txtGenEdit = "Generate F5";

        public string TxtGenEdit
        {
            get { return _txtGenEdit; }
            set
            {
                _txtGenEdit = value;
                NotifyPropertyChanged("TxtGenEdit");
            }
        }

        private Visibility _showDiagramVisible = Visibility.Collapsed;
        public Visibility ShowDiagramVisible
        {
            get { return _showDiagramVisible; }
            set
            {
                _showDiagramVisible = value;
                NotifyPropertyChanged("ShowDiagramVisible");
            }
        }

        private Visibility _hideDiagramVisible = Visibility.Collapsed;
        public Visibility HideDiagramVisible
        {
            get { return _hideDiagramVisible; }
            set
            {
                _hideDiagramVisible = value;
                NotifyPropertyChanged("HideDiagramVisible");
            }
        }

        private Visibility _generateDiagramVisible = Visibility.Collapsed;
        public Visibility GenerateDiagramVisible
        {
            get { return _generateDiagramVisible; }
            set
            {
                _generateDiagramVisible = value;
                NotifyPropertyChanged("GenerateDiagramVisible");
            }
        }

        private Visibility _deleteDiagramVisible = Visibility.Collapsed;
        public Visibility DeleteDiagramVisible
        {
            get { return _deleteDiagramVisible; }
            set
            {
                _deleteDiagramVisible = value;
                NotifyPropertyChanged("DeleteDiagramVisible");
            }
        }
        private DataRowView _selectedrowPccsDetails;
        public DataRowView SelectedRowPccsDetails
        {
            get
            {
                return _selectedrowPccsDetails;
            }

            set
            {
                _selectedrowPccsDetails = value;
                NotifyPropertyChanged("SelectedRowPccsDetails");
            }
        }

        private void RowEditEndingPccs(Object selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    SelectedItemPccs = (DataRowView)selecteditem;
                    if (SelectedItemPccs["SNO"].ToString() == "") SelectedItemPccs["SNO"] = existingsNo;
                    if (SelectedItemPccs["FEATURE"].ToString() != "")
                    {

                        if (PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["FEATURE"].ToString() != "")
                        {
                            DataRowView drv = PccsModel.PCCSDetails.AddNew();
                            drv.BeginEdit();
                            drv["SNO"] = PccsModel.PCCSDetails.Count;
                            drv.EndEdit();
                            NotifyPropertyChanged("PccsModel");
                            //PccsModel.PCCSDetails.Table.AcceptChanges();
            
                          //  PccsModel.PCCSDetails.Sort = "sno asc";
                            
                            ReNumber();

                        }

                    }
                    else if ((SelectedItemPccs["FEATURE"].ToString() == "") && (SelectedItemPccs["REACTION_PLAN"].ToString() != "" || SelectedItemPccs["CONTROL_METHOD"].ToString() != "" || SelectedItemPccs["FREQ_OF_INSP"].ToString() != "" || SelectedItemPccs["GAUGES_USED"].ToString() != "" || SelectedItemPccs["DEPT_RESP"].ToString() != "" || SelectedItemPccs["SPEC_CHAR"].ToString() != "" || SelectedItemPccs["CTRL_SPEC_MAX"].ToString() != "" || SelectedItemPccs["CTRL_SPEC_MIN"].ToString() != "" || SelectedItemPccs["SPEC_MAX"].ToString() != "" || SelectedItemPccs["SPEC_MIN"].ToString() != "" || SelectedItemPccs["PROCESS_FEATURE"].ToString() != "" || SelectedItemPccs["ISR_NO"].ToValueAsString().ToDecimalValue() != 0))
                    {
                        if (isFeatureCell)
                            ShowInformationMessage(PDMsg.EnterValid("FEATURE"));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void RowEditEndingPccsRev(Object selecteditem)
        {
            try
            {
                try
                {
                    //var duplicates = PccsModel.PccsRevisionDetails.Table.AsEnumerable().GroupBy(r => r["ISSUE_NO"]).Where(gr => gr.Count() > 1).ToList(); // Commented by jeyan -> system to identify 1 or 01 are duplicate

                    // By Jeyan Start -> system to identify 1 or 01 are duplicate
                    DataTable tempDt = new DataTable();
                    DataRow dr;
                    tempDt.Columns.Add("ISSUE_NO", typeof(int));

                    for (int ind = 0; ind <= PccsModel.PccsRevisionDetails.Table.Rows.Count - 1; ind++)
                    {

                        if (PccsModel.PccsRevisionDetails.Table.Rows[ind].RowState != DataRowState.Deleted)
                        {
                            dr = tempDt.NewRow();
                            dr["ISSUE_NO"] = DBNull.Value.Equals(PccsModel.PccsRevisionDetails.Table.Rows[ind]["ISSUE_NO"]) == true ? 0 : Convert.ToInt32(PccsModel.PccsRevisionDetails.Table.Rows[ind]["ISSUE_NO"]);
                            tempDt.Rows.Add(dr);
                        }
                    }
                    tempDt.AcceptChanges();
                    var duplicates = tempDt.AsEnumerable().GroupBy(r => r["ISSUE_NO"]).Where(gr => gr.Count() > 1).ToList();
                    // By Jeyan End
                    if (duplicates.Any())
                    {
                        ShowInformationMessage(PDMsg.NormalString("Duplicate  Control Plan Issue No has been Entered"));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowInformationMessage(ex.Message);
                }


                if (selecteditem != null)
                {

                    SelectedItemPccsRev = (DataRowView)selecteditem;
                    //if (SelectedItemPccsRev["ISSUE_DATE"].IsNotNullOrEmpty())
                    //{
                    //    if (SelectedItemPccsRev["ISSUE_DATE"].ToValueAsString().tod)
                    //    {

                    //    }
                    //}

                    if (SelectedItemPccsRev["ISSUE_NO"].ToString() != "")
                    {
                        if ((PccsModel.PccsRevisionDetails.Count == 1) && (SelectedItemPccsRev["ISSUE_DATE"].ToString().IsNotNullOrEmpty() || SelectedItemPccsRev["ISSUE_ALTER"].ToString().IsNotNullOrEmpty() || SelectedItemPccsRev["COMPILED_BY"].ToString().IsNotNullOrEmpty()))
                        {

                            DataRowView drv = PccsModel.PccsRevisionDetails.AddNew();
                            drv.BeginEdit();
                            drv["ISSUE_NONUMERIC"] = PccsModel.PccsRevisionDetails.Count;
                            // drv["ISSUE_NO"] = PccsModel.PccsRevisionDetails.Count;
                            drv.EndEdit();
                            // ReNumberIssue();
                        }
                        else if (PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["ISSUE_NO"].ToString() != "" && (PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["ISSUE_DATE"].ToString() != "" || PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["ISSUE_ALTER"].ToString() != "" || PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["COMPILED_BY"].ToString() != ""))
                        {

                            DataRowView drv = PccsModel.PccsRevisionDetails.AddNew();
                            drv.BeginEdit();
                            drv["ISSUE_NONUMERIC"] = PccsModel.PccsRevisionDetails.Count;
                            // drv["ISSUE_NO"] = PccsModel.PccsRevisionDetails.Count;
                            drv.EndEdit();
                            // ReNumberIssue();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }
        private void RowBeginEditPccsRev(Object selecteditem)
        {
            //try
            //{
            //    var duplicates = PccsModel.PccsRevisionDetails.Table.AsEnumerable().GroupBy(r => r["ISSUE_NO"]).Where(gr => gr.Count() > 1).ToList();
            //    if (duplicates.Any())
            //    {
            //        ShowInformationMessage(PDMsg.NormalString("Duplicate  Control Plan Issue No has been Entered"));
            //        return;
            //    }

            //    if (selecteditem != null)
            //    {

            //        SelectedItemPccsRev = (DataRowView)selecteditem;
            //        //if (SelectedItemPccsRev["ISSUE_DATE"].IsNotNullOrEmpty())
            //        //{
            //        //    if (SelectedItemPccsRev["ISSUE_DATE"].ToValueAsString().tod)
            //        //    {

            //        //    }
            //        //}

            if (SelectedItemPccsRev["ISSUE_NO"].ToString() == "")
            {
                if (PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["ISSUE_NO"].ToString() == "" && (PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["ISSUE_DATE"].ToString() != "" || PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["ISSUE_ALTER"].ToString() != "" || PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["COMPILED_BY"].ToString() != ""))
                {
                    DataRowView drv = PccsModel.PccsRevisionDetails.AddNew();
                    drv.BeginEdit();
                    drv["ISSUE_NONUMERIC"] = PccsModel.PccsRevisionDetails.Count;
                    //drv["ISSUE_NO"] = PccsModel.PccsRevisionDetails.Count;
                    drv.EndEdit();
                    ReNumberIssue();
                }
            }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    ex.LogException();
            //}
        }
        private void Print()
        {
            // throw new NotImplementedException();
        }

        private void Undo()
        {
            try
            {
                _pccsBll.GetPccsDetails(PccsModel, PccsModel.PartNo, PccsModel.RouteNo, PccsModel.SeqNo);
                oldDVPCCSDetails = PccsModel.PCCSDetails.ToTable().Copy().DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private void Copy()
        {
            try
            {
                if (PccsModel.PartNo.Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                    return;
                }

                frmCopyStatus copyStatus = new frmCopyStatus("PCCS", PccsModel.PartNo, PccsModel.RouteNo.ToString(), "", "", "");
                copyStatus.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private void Delete()
        {
            try
            {
                if (!PccsModel.PartNo.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                    return;
                }
                else if (!PccsModel.RouteNo.IsNotNullOrEmpty())
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Route No "));
                    return;
                }
                if (PccsModel.PCCSDetails.Count <= 0)
                {
                    ShowInformationMessage("Process Control and Capability Sheet have Zero Rows");
                    return;
                }
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("All record(s) will be deleted. Do you want to continue?", ApplicationTitle, System.Windows.MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (_pccsBll.DeletePccsDetails(PccsModel.PartNo, PccsModel.RouteNo))
                    {
                        ShowInformationMessage(PDMsg.DeletedSuccessfully);
                        _pccsBll.GetPccsDetails(PccsModel, PccsModel.PartNo, PccsModel.RouteNo, PccsModel.SeqNo);
                        oldDVPCCSDetails = PccsModel.PCCSDetails.ToTable().Copy().DefaultView;
                        _logviewBll.SaveLog(PccsModel.PartNo, "ControlPlan");
                    }
                    else
                    {
                        ShowInformationMessage("Make Sure Part No and Route No is selected.");
                    }
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
        private bool IsChangesMade()
        {
            try
            {
                PartNumberIsFocused = true;
                bool result = true;
                bool resultRev = true;

                if (oldDVPCCSDetails != null)
                {

                    DataView newDV_PCCSDetails = PccsModel.PCCSDetails.Table.Copy().DefaultView;
                    if (newDV_PCCSDetails.Table.Columns.Contains("SnoSort"))
                        newDV_PCCSDetails.Table.Columns.Remove("SnoSort");
                    result = newDV_PCCSDetails.IsEqual(oldDVPCCSDetails);

                }
                if (oldDVPCCSRevDetails != null)
                {
                    DataView newDV_PCCSRevDetails = PccsModel.PccsRevisionDetails.Table.Copy().DefaultView;
                    if (newDV_PCCSRevDetails.Table.Columns.Contains("SnoSort"))
                        newDV_PCCSRevDetails.Table.Columns.Remove("SnoSort");

                    resultRev = newDV_PCCSRevDetails.IsEqual(oldDVPCCSRevDetails);
                }
                return !(result && resultRev);

                //return (result || resultRev);
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
                if (PccsModel.PartNo.IsNotNullOrEmpty() && (IsRecordsUpdated || IsChangesMade()))
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        if (!IsRecordsUpdated) return;
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

        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {

                WPF.MDI.ClosingEventArgs closingev;
                closingev = (WPF.MDI.ClosingEventArgs)e;

                if (PccsModel.PartNo.IsNotNullOrEmpty() && (IsRecordsUpdated || IsChangesMade()))
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
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

        private DataTable RemoveDuplicatesRecords(DataTable dt, string typ)
        {

            DataTable dt2 = new DataTable();
            if (typ.ToUpper() == "PCCS")
            {
                var uniqueRows = dt.AsEnumerable()
                       .GroupBy(x => x.Field<string>("FEATURE"))
                       .Select(g => g.First());
                dt2 = uniqueRows.CopyToDataTable();
            }
            else if (typ.ToUpper() == "ISSUE")
            {
                var uniqueRows = dt.AsEnumerable()
                       .GroupBy(x => x.Field<string>("ISSUE_NO"))
                       .Select(g => g.First());
                dt2 = uniqueRows.CopyToDataTable();
            }
            return dt2;

        }

        public Boolean IsRecordsUpdated = false;
        private void Save()
        {
            PartNumberIsFocused = true;
            NotifyPropertyChanged("PccsModel");
            //CellEditEndingPccs(SelectedRowPccsDetails);
            if (!PccsModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            else if (!PccsModel.RouteNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Route No"));
                return;
            }
            else if (!PccsModel.SeqNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Sequence No"));
                return;
            }
            if ((PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["FEATURE"].ToString() == "") && (PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["REACTION_PLAN"].ToString() != "" || PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["CONTROL_METHOD"].ToString() != "" || PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["FREQ_OF_INSP"].ToString() != "" || PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["GAUGES_USED"].ToString() != "" || PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["DEPT_RESP"].ToString() != "" || PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["SPEC_CHAR"].ToString() != "" || PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["CTRL_SPEC_MAX"].ToString() != "" || PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["CTRL_SPEC_MIN"].ToString() != "" || PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["SPEC_MAX"].ToString() != "" || PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["SPEC_MIN"].ToString() != "" || PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["PROCESS_FEATURE"].ToString() != "" || PccsModel.PCCSDetails[PccsModel.PCCSDetails.Count - 1]["ISR_NO"].ToValueAsString().ToDecimalValue() != 0))
            {

                ShowInformationMessage(PDMsg.EnterValid("FEATURE"));
                return;
            }
            IsRecordsUpdated = false;
            DataTable dt = new DataTable();
            if (dt.IsNotNullOrEmpty() || PccsModel.PCCSDetails.Count > 0)
            {
                dt = PccsModel.PCCSDetails.ToTable().Copy();
                if (dt.IsNotNullOrEmpty() || PccsModel.PCCSDetails.Count > 0)
                {
                    dt = RemoveDuplicatesRecords(dt, "PCCS");
                    if (dt.Rows.Count < PccsModel.PCCSDetails.Count)
                    {
                        ShowInformationMessage(PDMsg.NormalString("Duplicate  Characteristics  has been Entered"));
                        return;
                    }
                }
            }
            //ReNumberIssue();
            if (dt.IsNotNullOrEmpty() || PccsModel.PccsRevisionDetails.Count > 0)
            {
                dt = PccsModel.PccsRevisionDetails.ToTable().Copy();
                if (dt.IsNotNullOrEmpty() || PccsModel.PccsRevisionDetails.Count > 0)
                {
                    dt = RemoveDuplicatesRecords(dt, "ISSUE");
                    if (dt.Rows.Count < PccsModel.PccsRevisionDetails.Count)
                    {
                        ShowInformationMessage(PDMsg.NormalString("Duplicate  Control Plan Issue No has been Entered"));
                        return;
                    }
                }
            }

            string typ = "";
            ReNumber();


            Progress.ProcessingText = PDMsg.ProgressUpdateText;
            Progress.Start();

            if (_pccsBll.SavePccs(PccsModel, ref typ))
            {
                //try
                //{
                //    foreach (DataRow row in delPccsGrid.Rows)
                //    {
                //        _pccsBll.DeletePccs(PccsModel.PartNo, PccsModel.RouteNo, Convert.ToDecimal(row["SEQ_NO"].ToValueAsString()), Convert.ToDouble(row["SNO"].ToValueAsString()));
                //    }
                //}
                //catch (Exception ex)
                //{

                //}
                Progress.End();
                if (_imageChange == true)
                {
                    _pccsBll.SaveDiagram(_diagram, PccsModel.PartNo, PccsModel.RouteNo);
                }
                Progress.End();
                IsRecordsUpdated = true;
                if (TxtGenEdit == "Edit F5")
                {
                    ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                    _logviewBll.SaveLog(PccsModel.PartNo, "ControlPlan");
                }
                else
                {
                    ShowInformationMessage(PDMsg.SavedSuccessfully);
                    _logviewBll.SaveLog(PccsModel.PartNo, "ControlPlan");
                }
                Progress.End();
            }
            else
            {
                Progress.End();
            }
            ClearImage();
            PartNumberIsFocused = true;
        }

        private void NewForm()
        {
            delPccsGrid.Clear();
            PccsModel.PartNo = "";
            PccsModel.RouteNo = null;
            PccsModel.SeqNo = "";
            PccsModel.OperDesc = "";
            PccsModel.PartDesc = "";
            this.SelectedRowSequence = null;
            _pccsBll.GetPccsDetails(PccsModel, PccsModel.PartNo, PccsModel.RouteNo);
            oldDVPCCSDetails = PccsModel.PCCSDetails.ToTable().Copy().DefaultView;
            _pccsBll.GetPccsRevisonDetails(PccsModel, PccsModel.PartNo, PccsModel.RouteNo);
            oldDVPCCSRevDetails = PccsModel.PccsRevisionDetails.ToTable().Copy().DefaultView;
        }

        private void Edit()
        {
            if (TxtGenEdit != "Edit F5")
                ShowInformationMessage("Process control and Capability sheet Details Should be Entered");
            else
                TxtGenEdit = "Edit F5";
            if (!PccsModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part Number"));
                return;
            }
            else if (!PccsModel.SeqNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Sequence Number"));
                return;
            }

            frmControlPlanGeneration ctlPlanGen = new frmControlPlanGeneration(userInformation, PccsModel.PartNo, PccsModel.RouteNo.ToValueAsString().ToDecimalValue(), Convert.ToDecimal(PccsModel.SeqNo), PccsModel);
            ctlPlanGen.Show();
        }
        private void LoadCmbDatas()
        {
            oldDVPCCSDetails = null;
            oldDVPCCSRevDetails = null;
            _pccsBll.GetPartNoDetails(PccsModel);
            _pccsBll.GetPccsComboValues(PccsModel);
        }
        private bool _partNumberIsFocused = false;
        public bool PartNumberIsFocused
        {
            get { return _partNumberIsFocused; }
            set
            {
                _partNumberIsFocused = value;
                NotifyPropertyChanged("PartNumberIsFocused");
            }
        }
        private void ReNumber1()
        {
            DataView dvPccs;
            PccsModel.PCCSDetails.Table.AcceptChanges();
            //dvPccs = new DataView(PccsModel.PCCSDetails.ToTable(), "SNO is not null", "SNO Asc", DataViewRowState.CurrentRows);

            dvPccs = PccsModel.PCCSDetails;

            if (!PccsModel.SeqNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Sequence Number"));
                return;
            }
            if (dvPccs.IsNotNullOrEmpty())
            {
                if (dvPccs.Table.Columns.IndexOf("SnoSort") < 0)
                    dvPccs.Table.Columns.Add("SnoSort", typeof(decimal));
                //dvPccs.Table.AcceptChanges();
                for (int i = 0; i < dvPccs.Count; i++)
                {
                    dvPccs[i]["SnoSort"] = dvPccs[i]["SNO"];
                }
                dvPccs.Sort = "SnoSort ASC";
                int j = 0;
                foreach (DataRowView item in dvPccs)
                {
                    //item.BeginEdit();
                    item["SNO"] = j + 1;
                    item["SnoSort"] = item["SNO"];
                    //item.EndEdit();
                    j++;
                }

                //for (int i = 0; i < dvPccs.Count; i++)
                //{
                //    DataRowView rowView = dvPccs[i];

                //    rowView["SNO"] = i + 1;
                //    rowView["SnoSort"] = dvPccs[i]["SNO"];
                //}
                dvPccs.Sort = "SnoSort ASC";
                //dvPccs.Table.AcceptChanges();
                //PccsModel.PCCSDetails = dvPccs;

            }
        }
        private void ReNumber()
        {
            DataView dvPccs;
          //  PccsModel.PCCSDetails.Table.AcceptChanges();
            //dvPccs = new DataView(PccsModel.PCCSDetails.ToTable(), "SNO is not null", "SNO Asc", DataViewRowState.CurrentRows);
            
            dvPccs = PccsModel.PCCSDetails;

            if (!PccsModel.SeqNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Sequence Number"));
                return;
            }
            if (dvPccs.IsNotNullOrEmpty())
            {
                if (dvPccs.Table.Columns.IndexOf("SnoSort") < 0)
                    dvPccs.Table.Columns.Add("SnoSort", typeof(decimal));
                //dvPccs.Table.AcceptChanges();
                for (int i = 0; i < dvPccs.Count; i++)
                {
                    dvPccs[i]["SnoSort"] = dvPccs[i]["SNO"];
                }
                dvPccs.Sort = "SnoSort ASC";
                int j = 0;
                foreach (DataRowView item in dvPccs)
                {
                    //item.BeginEdit();
                    item["SNO"] = j + 1;
                    item["SnoSort"] = item["SNO"];
                    //item.EndEdit();
                    j++;
                }

                //for (int i = 0; i < dvPccs.Count; i++)
                //{
                //    DataRowView rowView = dvPccs[i];

                //    rowView["SNO"] = i + 1;
                //    rowView["SnoSort"] = dvPccs[i]["SNO"];
                //}
               // dvPccs.Sort = "SnoSort ASC";
                //dvPccs.Table.AcceptChanges();
                //PccsModel.PCCSDetails = dvPccs;

            }
        }
        public void OnCellEditEndingPccsIssue(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            try
            {

                var duplicates = PccsModel.PccsRevisionDetails.Table.AsEnumerable().GroupBy(r => r["ISSUE_NO"]).Where(gr => gr.Count() > 1).ToList();
                if (duplicates.Any())
                {
                    ShowInformationMessage(PDMsg.NormalString("Duplicate  Control Plan Issue No has been Entered"));
                    return;
                }

                int output;
                if (int.TryParse(selecteditem["ISSUE_NO"].ToValueAsString(), out output) == true)
                {
                    selecteditem["ISSUE_NONUMERIC"] = selecteditem["ISSUE_NO"];
                }
                else
                {
                    selecteditem["ISSUE_NONUMERIC"] = 0;
                }

                if (selecteditem != null)
                {

                    SelectedItemPccsRev = (DataRowView)selecteditem;
                    //if (SelectedItemPccsRev["ISSUE_DATE"].IsNotNullOrEmpty())
                    //{
                    //    if (SelectedItemPccsRev["ISSUE_DATE"].ToValueAsString().tod)
                    //    {

                    //    }
                    //}

                    if (SelectedItemPccsRev["ISSUE_NO"].ToString() != "")
                    {

                        if (PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["ISSUE_NO"].ToString() != "" && (PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["ISSUE_DATE"].ToString() != "" || PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["ISSUE_ALTER"].ToString() != "" || PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["COMPILED_BY"].ToString() != ""))
                        //if (PccsModel.PccsRevisionDetails[PccsModel.PccsRevisionDetails.Count - 1]["ISSUE_NO"].ToString() != "")
                        {
                            DataRowView drv = PccsModel.PccsRevisionDetails.AddNew();
                            // drv.BeginEdit();
                            //  drv["ISSUE_NO"] = PccsModel.PccsRevisionDetails.Count;
                            drv["ISSUE_NONUMERIC"] = PccsModel.PccsRevisionDetails.Count;
                            // drv.EndEdit();
                            ReNumberIssue();

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            try
            {
                string sno = ((System.Data.DataRowView)(e.Row.Item)).Row["ISSUE_NO"].ToString();
                string columnName = e.Column.SortMemberPath;
                // DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
                System.Windows.Controls.DataGrid datagrid = sender as System.Windows.Controls.DataGrid;


                if (e.Column.GetType() == typeof(System.Windows.Controls.DataGridTemplateColumn))
                {
                    if (columnName == "ISSUE_DATE")
                    {
                        if (datagrid.SelectedIndex > -1)
                        {
                            if (selecteditem["ISSUE_DATE"].ToValueAsString().IsNotNullOrEmpty() && PccsModel.PccsRevisionDetails[datagrid.SelectedIndex - 1]["ISSUE_DATE"].ToString().IsNotNullOrEmpty())
                            {
                                if (Convert.ToDateTime(selecteditem["ISSUE_DATE"]) <= Convert.ToDateTime(PccsModel.PccsRevisionDetails[datagrid.SelectedIndex - 1]["ISSUE_DATE"]))
                                {
                                    ShowInformationMessage(PDMsg.Invalid("Date"));
                                    selecteditem["ISSUE_DATE"] = DBNull.Value;
                                    e.Cancel = true;
                                }
                            }
                        }

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
        private void ReNumberIssueLoad()
        {

            DataView dvPccsRev;
            //dvPccs = new DataView(PccsModel.PCCSDetails.ToTable(), "SNO is not null", "SNO Asc", DataViewRowState.CurrentRows);
            PccsModel.PccsRevisionDetails.Table.AcceptChanges();
            dvPccsRev = PccsModel.PccsRevisionDetails.Table.Copy().DefaultView;

            if (dvPccsRev.IsNotNullOrEmpty())
            {
                if (dvPccsRev.Table.Columns.IndexOf("SnoSort") < 0)
                    dvPccsRev.Table.Columns.Add("SnoSort", typeof(decimal));
                dvPccsRev.Table.AcceptChanges();
                // dvPccsRev.Sort = "ISSUE_DATE ASC";
                for (int i = 0; i < dvPccsRev.Count; i++)
                {
                    dvPccsRev[i]["SnoSort"] = dvPccsRev[i]["ISSUE_NO"];
                    if (dvPccsRev.Count == i + 1)
                    {
                        dvPccsRev[i]["ISSUE_DATE"] = DateTime.Now.Date;
                    }
                }
                //dvPccsRev.Sort = "SnoSort ASC";
                // dvPccsRev.Sort = "ISSUE_DATE ASC";
                int j = 0;
                int prevValue = 0;
                foreach (DataRowView item in dvPccsRev)
                {
                    //       item["ISSUE_NO"] = j + 1;

                    item["SnoSort"] = (!item["ISSUE_NO"].IsNotNullOrEmpty() && item["ISSUE_DATE"].IsNotNullOrEmpty()) ? prevValue + 1 : item["ISSUE_NO"];
                    prevValue = item["ISSUE_NO"].ToValueAsString().ToIntValue();
                    j++;
                }
                for (int i = 0; i < dvPccsRev.Count; i++)
                {
                    if (dvPccsRev.Count == i + 1)
                    {
                        dvPccsRev[i]["ISSUE_DATE"] = DBNull.Value;
                    }
                }
                dvPccsRev.Sort = "SnoSort ASC";
                dvPccsRev.Table.AcceptChanges();
                PccsModel.PccsRevisionDetails = dvPccsRev.Table.Copy().DefaultView;
            }
        }
        private void ReNumberIssue()
        {
            DataView dvPccsRev;
            //dvPccs = new DataView(PccsModel.PCCSDetails.ToTable(), "SNO is not null", "SNO Asc", DataViewRowState.CurrentRows);
            PccsModel.PccsRevisionDetails.Table.AcceptChanges();
            dvPccsRev = PccsModel.PccsRevisionDetails;

            if (dvPccsRev.IsNotNullOrEmpty())
            {
                if (dvPccsRev.Table.Columns.IndexOf("SnoSort") < 0)
                    dvPccsRev.Table.Columns.Add("SnoSort", typeof(decimal));
                dvPccsRev.Table.AcceptChanges();
                // dvPccsRev.Sort = "ISSUE_DATE ASC";
                for (int i = 0; i < dvPccsRev.Count; i++)
                {
                    dvPccsRev[i]["SnoSort"] = dvPccsRev[i]["ISSUE_NO"];
                    if (dvPccsRev.Count == i + 1)
                    {
                        dvPccsRev[i]["ISSUE_DATE"] = DateTime.Now.Date;
                    }
                }
                dvPccsRev.Sort = "ISSUE_NONUMERIC ASC";
                // dvPccsRev.Sort = "ISSUE_DATE ASC";
                int j = 0;
                foreach (DataRowView item in dvPccsRev)
                {
                    //       item["ISSUE_NO"] = j + 1;
                    item["SnoSort"] = item["ISSUE_NO"];
                    j++;
                }
                for (int i = 0; i < dvPccsRev.Count; i++)
                {
                    if (dvPccsRev.Count == i + 1)
                    {
                        dvPccsRev[i]["ISSUE_DATE"] = DBNull.Value;
                    }
                }
                dvPccsRev.Sort = "ISSUE_NONUMERIC ASC";
                dvPccsRev.Table.AcceptChanges();
            }
        }
        string existingsNo = "";
        string existingsFeature = "";
        string existingSpecMin = "";
        string existingSpecMax = "";
        bool isctrlSpecMinCopy = true;
        bool isctrlSpecMaxCopy = true;
        bool isFeatureCell = false;
        public void OnBeginningEdit(object sender, Microsoft.Windows.Controls.DataGridBeginningEditEventArgs e)
        {
            existingSpecMin = "";
            existingSpecMax = "";
            //TextBox tb = e. as TextBox;
            //DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            //string columnName = e.Column.SortMemberPath;

            //if (tb != null)
            //{
            //    //selecteditem.BeginEdit();
            //    selecteditem[columnName] = tb.Text;
            //    //selecteditem.EndEdit();
            //}
            if ((e.Column.Header.ToString() == "Sno") || (e.Column.Header.ToString() == "Product Characteristics"))
            {
                string sno = ((System.Data.DataRowView)(e.Row.Item)).Row["SNO"].ToString();
                string feature = ((System.Data.DataRowView)(e.Row.Item)).Row["FEATURE"].ToString();
                if (e.Column.Header.ToString() == "Sno")
                {
                    existingsNo = sno;
                    // e.Cancel = true;
                }
                if (e.Column.Header.ToString() == "Product Characteristics") existingsFeature = feature;
            }

            //if ((e.Column.Header.ToString() == "Spec\nMax"))
            //{
            //    if (((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MIN"].ToString().IsNotNullOrEmpty() && ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MIN"].ToString() == "")
            //        ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MIN"] = ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MIN"].ToString();
            //}

            //if ((e.Column.Header.ToString() == "Ctrl Spec\nMin"))
            //{
            //    if (((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString().IsNotNullOrEmpty() && ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"].ToString() == "")
            //        ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"] = ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString();
            //}
            //if ((e.Column.Header.ToString() == "Spec\nMin"))
            //{
            //    if (((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MIN"].ToString().IsNotNullOrEmpty() && ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MIN"].ToString() == "")
            //    {
            //        ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MIN"] = ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MIN"].ToString();
            //    }
            //    else
            //    {
            //        // ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MIN"] = "";
            //    }

            //}
            //else if ((e.Column.Header.ToString() == "Spec\nMax"))
            //{
            //    if (((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString().IsNotNullOrEmpty() && ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"].ToString() == "")
            //    {
            //        ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"] = ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString();
            //    }
            //    else
            //    {
            //        //  ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"] = "";
            //    }

            //}

            ////if ((e.Column.Header.ToString() == "Ctrl Spec\nMin"))
            ////{
            ////    existingSpecMin = ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MIN"].ToString();
            ////}
            ////if ((e.Column.Header.ToString() == "Ctrl Spec\nMax"))
            ////{
            ////    existingSpecMax = ((System.Data.DataRowView)(e.Row.Item)).Row["CTRL_SPEC_MAX"].ToString();
            ////}
            //if ((e.Column.Header.ToString() != "Spec&#x0a;Min") && ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MIN"].ToString().IsNotNullOrEmpty())
            //{
            //    existingSpecMin = ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MIN"].ToString();
            //}
            //if ((e.Column.Header.ToString() != "Spec&#x0a;Max") && ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString().IsNotNullOrEmpty())
            //{
            //    existingSpecMax = ((System.Data.DataRowView)(e.Row.Item)).Row["SPEC_MAX"].ToString();
            //}
            NotifyPropertyChanged("PccsModel");
        }
        private readonly ICommand controlPlanCommand;
        public ICommand ControlPlanCommand { get { return this.controlPlanCommand; } }
        private void ControlPlan()
        {
            try
            {
                frmControlPlan controlplan = new frmControlPlan(userInformation, PccsModel.PartNo, PccsModel.RouteNo.ToValueAsString(), PccsModel.SeqNo);
                controlplan.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
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
        public bool EditButtonIsEnable
        {
            get { return _editButtonIsEnable; }
            set
            {
                this._editButtonIsEnable = value;
                NotifyPropertyChanged("EditButtonIsEnable");
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

        public bool DeleteButtonIsEnable
        {
            get { return _deleteButtonIsEnable; }
            set
            {
                this._deleteButtonIsEnable = value;
                NotifyPropertyChanged("DeleteButtonIsEnable");
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

        private void GetRights()
        {
            ActionPermission = new RolePermission();
            ActionPermission.Save = true;
            ActionPermission.Print = false;
            ActionPermission.View = true;
            ActionPermission.Delete = false;
            ActionPermission.Edit = false;
            ActionPermission = _pccsBll.GetUserRights("PCCS");
            if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
            {
                ActionPermission.Save = false;
            }
        }

        private void setRights()
        {
            if (EditButtonIsEnable) EditButtonIsEnable = ActionPermission.Edit;
            if (DeleteButtonIsEnable) DeleteButtonIsEnable = ActionPermission.Delete;
            if (SaveButtonIsEnable) SaveButtonIsEnable = ActionPermission.Save;
            if (SaveButtonIsEnable) SaveButtonIsEnable = ActionPermission.Save;
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
        private ObservableCollection<DropdownColumns> _dropDownItemsFeature;
        public ObservableCollection<DropdownColumns> DropDownItemsFeature
        {
            get
            {
                return _dropDownItemsFeature;
            }
            set
            {
                this._dropDownItemsFeature = value;
                NotifyPropertyChanged("DropDownItemsFeature");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsSpecChar;
        public ObservableCollection<DropdownColumns> DropDownItemsSpecChar
        {
            get
            {
                return _dropDownItemsSpecChar;
            }
            set
            {
                this._dropDownItemsSpecChar = value;
                NotifyPropertyChanged("DropDownItemsSpecChar");
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsControl;
        public ObservableCollection<DropdownColumns> DropDownItemsControl
        {
            get
            {
                return _dropDownItemsControl;
            }
            set
            {
                this._dropDownItemsControl = value;
                NotifyPropertyChanged("DropDownItemsControl");
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsProcessFeature;
        public ObservableCollection<DropdownColumns> DropDownItemsProcessFeature
        {
            get
            {
                return _dropDownItemsProcessFeature;
            }
            set
            {
                this._dropDownItemsProcessFeature = value;
                NotifyPropertyChanged("DropDownItemsProcessFeature");
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsSeq;
        public ObservableCollection<DropdownColumns> DropDownItemsSeq
        {
            get
            {
                return _dropDownItemsSeq;
            }
            set
            {
                this._dropDownItemsSeq = value;
                NotifyPropertyChanged("DropDownItemsSeq");
            }
        }
        private void SetdropDownItems()
        {
            try
            {
                DropDownItemsPart = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "PART NO", ColumnWidth = 120 },
                            new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "DESCRIPTION", ColumnWidth = "1*" }
                        };
                DropDownItemsSeq = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "SEQ_NO", ColumnDesc = "SEQ NO", ColumnWidth = 73 },
                            new DropdownColumns() { ColumnName = "SnoSort", ColumnDesc = "SEQ NO", ColumnWidth = 73 },
                            new DropdownColumns() { ColumnName = "OPN_DESC", ColumnDesc = "DESCRIPTION", ColumnWidth = "1*" }
                        };
                DropDownItemsFeature = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "FEATURE", ColumnDesc = "FEATURE", ColumnWidth = "1*" },
                        };

                DropDownItemsProcessFeature = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PROCESS_FEATURE", ColumnDesc = "PROCESS", ColumnWidth = "1*" },
                        };

                DropDownItemsSpecChar = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SPEC_CHAR", ColumnDesc = "SPECIAL CHAR", ColumnWidth = "1*" },
                        };

                DropDownItemsControl = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CONTROL_METHOD", ColumnDesc = "CONTROL METHOD", ColumnWidth = "1*" },
                        };

                DropDownHeaderRoute = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "ROUTE_NO", ColumnDesc = "Route No", ColumnWidth = "1*" }
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
        DataView oldDVPCCSDetails = null;
        DataView oldDVPCCSRevDetails = null;
        private readonly ICommand selectChangeComboCommandPartNo;
        public ICommand SelectChangeComboCommandPartNo { get { return this.selectChangeComboCommandPartNo; } }
        private void SelectDataRowPart()
        {
            if (this.SelectedRowPart != null)
            {

                PccsModel.EditGenBtn = "Generate F5";
                //  this.SelectedRowSequence = null;
                //SelectDataRowSequence();
                if (SelectedRowPart.IsNotNullOrEmpty())
                {
                    PccsModel.PartNo = this.SelectedRowPart["PART_NO"].ToString();
                    PccsModel.PartDesc = this.SelectedRowPart["PART_DESC"].ToString();
                }

                if (PccsModel.PartNo.IsNotNullOrEmpty())
                {
                    mdiChild.Title = ApplicationTitle + " - Control Plan" + ((PccsModel.PartNo.IsNotNullOrEmpty()) ? " - " + PccsModel.PartNo : "");
                    _pccsBll.GetRouteNoDetailsByPartNo(PccsModel, PccsModel.PartNo);
                    if (_pccsBll.GetCurrentProcessByPartNo(PccsModel) == false)
                    {
                        if (PccsModel.RouteNoDetails.Table.Rows.Count > 0)
                            PccsModel.RouteNo = Convert.ToDecimal(PccsModel.RouteNoDetails.Table.Rows[0]["ROUTE_NO"].ToString());
                    }

                    if (formType == true && PccsModel.RouteNo.IsNotNullOrEmpty())
                    {
                        PccsModel.RouteNoDetails.RowFilter = "ROUTE_NO='" + PccsModel.RouteNo + "'";
                        SelectedRowRoute = PccsModel.RouteNoDetails[0];
                        SelectDataRowRoute();
                        PccsModel.RouteNoDetails.RowFilter = string.Empty;
                        formType = false;
                    }
                    else if (formType == false && PccsModel.RouteNo.IsNotNullOrEmpty())
                    {
                        PccsModel.RouteNoDetails.RowFilter = "ROUTE_NO='" + PccsModel.RouteNo + "'";
                        if (PccsModel.RouteNoDetails.Count > 0)
                        {
                            SelectedRowRoute = PccsModel.RouteNoDetails[0];
                            SelectDataRowRoute();
                        }
                        PccsModel.RouteNoDetails.RowFilter = string.Empty;
                    }
                    IsRecordsUpdated = false;
                    _pccsBll.GetPccsDetails(PccsModel, PccsModel.PartNo, PccsModel.RouteNo, PccsModel.SeqNo);
                    oldDVPCCSDetails = PccsModel.PCCSDetails.ToTable().Copy().DefaultView;
                    delPccsGrid = PccsModel.PCCSDetails.ToTable().Copy().Clone();
                    _pccsBll.GetPccsRevisonDetails(PccsModel, PccsModel.PartNo, PccsModel.RouteNo);
                    oldDVPCCSRevDetails = PccsModel.PccsRevisionDetails.ToTable().Copy().DefaultView;
                    ReNumber();
                    ReNumberIssueLoad();
                    TxtGenEdit = PccsModel.EditGenBtn;
                }

            }
            else
            {
                EmptyPartNumber();
            }
            ClearImage();
        }

        private DataRowView _selectedrowsequence;
        public DataRowView SelectedRowSequence
        {
            get
            {
                return _selectedrowsequence;
            }

            set
            {
                _selectedrowsequence = value;
            }
        }
        DataTable dt = new DataTable();
        private readonly ICommand selectChangeComboCommandSequence;
        public ICommand SelectChangeComboCommandSequence { get { return this.selectChangeComboCommandSequence; } }
        private void SelectDataRowSequence()
        {
            if (this.SelectedRowSequence != null)
            {
                PccsModel.SeqNo = this.SelectedRowSequence["SEQ_NO"].ToString();
                PccsModel.OperDesc = this.SelectedRowSequence["OPN_DESC"].ToString();
                if (PccsModel.SeqNo.IsNotNullOrEmpty())
                {
                    _pccsBll.GetPccsDetails(PccsModel, PccsModel.PartNo, PccsModel.RouteNo, PccsModel.SeqNo);
                    ReNumber();
                    oldDVPCCSDetails = PccsModel.PCCSDetails.ToTable().Copy().DefaultView;
                    TxtGenEdit = PccsModel.EditGenBtn;
                }
            }
            else if (this.SelectedRowSequence == null)
            {
                PccsModel.SeqNo = "";
                PccsModel.OperDesc = "";
            }
            ClearImage();
        }

        private DataRowView _selectedrowroute;
        public DataRowView SelectedRowRoute
        {
            get
            {
                return _selectedrowroute;
            }

            set
            {
                _selectedrowroute = value;
            }
        }
        private readonly ICommand selectChangeComboCommandRoute;
        public ICommand SelectChangeComboCommandRoute { get { return this.selectChangeComboCommandRoute; } }
        private void SelectDataRowRoute()
        {
            if (this.SelectedRowRoute != null)
            {
                PccsModel.RouteNo = this.SelectedRowRoute["ROUTE_NO"].ToString().ToIntValue();
                if (PccsModel.RouteNo.IsNotNullOrEmpty())
                {
                    _pccsBll.GetSequenceNoDetailsByPartNoRouteNo(PccsModel, PccsModel.PartNo, PccsModel.RouteNo.ToValueAsString().ToDecimalValue());
                    _pccsBll.GetPccsDetails(PccsModel, PccsModel.PartNo, PccsModel.RouteNo, PccsModel.SeqNo);
                    ReNumber();
                    oldDVPCCSDetails = PccsModel.PCCSDetails.ToTable().Copy().DefaultView;
                    _pccsBll.GetPccsRevisonDetails(PccsModel, PccsModel.PartNo, PccsModel.RouteNo);
                    oldDVPCCSRevDetails = PccsModel.PccsRevisionDetails.ToTable().Copy().DefaultView;
                    TxtGenEdit = PccsModel.EditGenBtn;
                }
            }
        }

        private readonly ICommand _pccsDetailsDeleteCommand;
        public ICommand PccsDetailsDeleteCommand { get { return this._pccsDetailsDeleteCommand; } }

        private void PccsDetailsDeleteRow(Object selectedRow)
        {
            try
            {
                if (!selectedRow.IsNotNullOrEmpty() || selectedRow.ToValueAsString() == "{NewItemPlaceholder}"
                    || !PccsModel.PCCSDetails.IsNotNullOrEmpty() || PccsModel.PCCSDetails.Count <= 0) return;
                DataRowView selectedDataRowView = (DataRowView)selectedRow;
                if (_pccsBll.DeletePccs(PccsModel.PartNo, PccsModel.RouteNo.ToValueAsString().ToDecimalValue(), Convert.ToDecimal(selectedDataRowView["SNO"].ToValueAsString()), Convert.ToDouble(selectedDataRowView["SNO"].ToValueAsString())))
                {
                    _pccsBll.GetPccsDetails(PccsModel, PccsModel.PartNo, PccsModel.RouteNo, PccsModel.SeqNo);
                    oldDVPCCSDetails = PccsModel.PCCSDetails.ToTable().Copy().DefaultView;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }



        public PCCSModel PccsModel
        {
            get
            {
                return _pccsModel;
            }
            set
            {
                this._pccsModel = value;
                IsRecordsUpdated = false;
                NotifyPropertyChanged("PccsModel");
            }
        }

        private readonly ICommand iSIRClickCommand;
        public ICommand ISIRClickCommand { get { return this.iSIRClickCommand; } }
        private void iSIRReport()
        {
            if (!PccsModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
            }
            else
            {
                ISIRModel im = new ISIRModel();
                im.PART_NO = PccsModel.PartNo;
                frmISIR isir = new frmISIR(userInformation, im);
                isir.ShowDialog();
            }
        }

        private readonly ICommand pswClickCommand;
        public ICommand PSWClickCommand { get { return this.pswClickCommand; } }
        private void pswReport()
        {
            if (!PccsModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
            }
            else
            {
                PartSubmissionWarrantModel pm = new PartSubmissionWarrantModel();
                pm.PART_NO = PccsModel.PartNo;
                frmPartSubmissionWarrant psw = new frmPartSubmissionWarrant(userInformation, pm);
                psw.ShowDialog();
            }
        }

        private readonly ICommand dimensionalClickCommand;
        public ICommand DimensionalClickCommand { get { return this.dimensionalClickCommand; } }
        private void dimensionalReport()
        {
            if (!PccsModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            else
            {
                DataTable processData;
                processData = _pccsBll.GetResults(PccsModel.PartNo, PccsModel.RouteNo.ToString(), 1, 70, "DR");
                if (processData == null || processData.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {
                    frmReportViewer rv = new frmReportViewer(processData, "Dimensional", PccsModel.PartDesc);
                    rv.ShowDialog();
                }
            }
        }

        private readonly ICommand materialClickCommand;
        public ICommand MaterialClickCommand { get { return this.materialClickCommand; } }
        private void materialReport()
        {
            if (!PccsModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            else
            {
                DataTable processData;
                processData = _pccsBll.GetResults(PccsModel.PartNo, PccsModel.RouteNo.ToString(), 71, 80, "MTR");
                if (processData == null || processData.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {
                    frmReportViewer rv = new frmReportViewer(processData, "Material", PccsModel.PartDesc);
                    rv.ShowDialog();
                }
            }
        }

        private readonly ICommand performanceClickCommand;
        public ICommand PerfomanceClickCommand { get { return this.performanceClickCommand; } }
        private void perfomanceReport()
        {
            if (!PccsModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            else
            {
                DataTable processData;
                processData = _pccsBll.GetResults(PccsModel.PartNo, PccsModel.RouteNo.ToString(), 81, 90, "PTR");
                if (processData == null || processData.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {
                    frmReportViewer rv = new frmReportViewer(processData, "Perfomance", PccsModel.PartDesc);
                    rv.ShowDialog();
                }
            }
        }


        private readonly ICommand _showDiagramClickCommand;
        public ICommand ShowDiagramClickCommand { get { return this._showDiagramClickCommand; } }

        private readonly ICommand _hideDiagramClickCommand;
        public ICommand HideDiagramClickCommand { get { return this._hideDiagramClickCommand; } }

        private readonly ICommand _generateDiagramClickCommand;
        public ICommand GenerateDiagramClickCommand { get { return this._generateDiagramClickCommand; } }

        private readonly ICommand _deleteDiagramClickCommand;
        public ICommand DeleteDiagramClickCommand { get { return this._deleteDiagramClickCommand; } }

        public string GetFilePath()
        {
            string filePathNew = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            if (System.Reflection.Assembly.GetExecutingAssembly().IsDebug() || filePathNew.Contains("\\bin\\Debug"))
            {
                System.IO.DirectoryInfo d = new System.IO.DirectoryInfo(filePathNew);
                filePathNew = d.Parent.Parent.FullName;
            }
            string filePath = filePathNew + "\\VSDFiles\\PCCS";

            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(filePath);
            if (!di.Exists)
            {
                try
                {
                    di.Create();
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
            }
            return filePath;
        }

        private System.Windows.Media.Imaging.BitmapImage _photoSource;
        public System.Windows.Media.Imaging.BitmapImage PhotoSource
        {
            get
            {
                return _photoSource;
            }
            set
            {
                _photoSource = value;
                NotifyPropertyChanged("PhotoSource");
            }
        }

        private void ShowDiagramClick()
        {
            //GeneratePFD();
            //GeneratePFDNew();
            GenerateProcessFlowDiagram();
        }

        private void HideDiagramClick()
        {
            try
            {
                ClearImage();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void GenerateDiagramClick()
        {
            try
            {
                //GeneratePFD();
                GenerateProcessFlowDiagram();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void DeleteDiagramClick()
        {
            try
            {
                if (ShowWarningMessage("Do you want to delete the drawing ?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    PhotoSource = ProcessDesigner.Properties.Resources.Blank_Image.ToBitmapImage();
                    _diagram = null;
                    _imageChange = true;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void ParameterMouseClick1()
        {
            try
            {
                //if (!PccsModel.PartNo.IsNotNullOrEmpty() || PccsModel.RouteNo.IsNotNullOrEmpty()) return;

                ProductInformation pi = new ProductInformation(_userInformation);
                List<PROCESS_SHEET> lstPROCESS_SHEET = pi.GetProcessSheetByPartNumber(new PRD_MAST() { PART_NO = PccsModel.PartNo });
                if (!lstPROCESS_SHEET.IsNotNullOrEmpty() || lstPROCESS_SHEET.Count == 0) return;

                List<PROCESS_SHEET> lstProcessSheetByRoutNumber = (from row in lstPROCESS_SHEET
                                                                   where row.PART_NO == PccsModel.PartNo.ToValueAsString().Trim() &&
                                                                   row.ROUTE_NO == PccsModel.RouteNo
                                                                   select row).ToList<PROCESS_SHEET>();

                int recordCount = 0;
                if (lstProcessSheetByRoutNumber.IsNotNullOrEmpty()) recordCount = lstProcessSheetByRoutNumber.Count;

                string pfdFileName = GetFilePath() + "\\FLOW.VSD";
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(pfdFileName);

                if (!fileInfo.Exists)
                {
                    ShowInformationMessage(PDMsg.DoesNotExists(pfdFileName));
                    return;
                };

                fileInfo = SaveFile(fileInfo);

                if (!fileInfo.IsNotNullOrEmpty()) return;
                setImage(fileInfo);

                ////System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                ////System.IO.MemoryStream strm = new System.IO.MemoryStream();

                ////using (System.IO.FileStream file = new System.IO.FileStream(fileInfo.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                ////{
                ////    byte[] bytes = new byte[file.Length];
                ////    file.Read(bytes, 0, (int)file.Length);
                ////    strm.Write(bytes, 0, (int)file.Length);
                ////    file.Close();
                ////    file.Dispose();
                ////}
                ////bitmap.BeginInit();
                ////bitmap.StreamSource = strm;
                ////bitmap.EndInit();
                ////PhotoSource = bitmap;
                //if (parameters != null && ToolAdmin.SelectedParameter != null)
                //{
                //    Point mousePos = Mouse.GetPosition((IInputElement)parameters);
                //    Canvas cv = (Canvas)parameters;

                //    foreach (FrameworkElement parameter in cv.Children)
                //    {
                //        if (parameter is TextBlock)
                //        {
                //            TextBlock tb = (TextBlock)parameter;
                //            if (tb.Name == ToolAdmin.SelectedParameter["PARAMETER_CD"].ToString())
                //            {
                //                tb.SetValue(Canvas.LeftProperty, mousePos.X);
                //                tb.SetValue(Canvas.TopProperty, mousePos.Y);
                //                ToolAdmin.SelectedParameter["X_COORDINATE"] = ConvertPixelsToTwips(mousePos.X);
                //                ToolAdmin.SelectedParameter["Y_COORDINATE"] = ConvertPixelsToTwips(mousePos.Y);
                //            }
                //        }
                //    }
                //}
                //IsFocusedFamilyCode = true;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool setImage(System.IO.FileInfo fileInfo)
        {
            bool breturnValue = false;
            try
            {
                System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                System.IO.MemoryStream strm = new System.IO.MemoryStream();

                using (System.IO.FileStream file = new System.IO.FileStream(fileInfo.FullName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    byte[] bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    strm.Write(bytes, 0, (int)file.Length);
                    file.Close();
                    file.Dispose();
                }
                bitmap.BeginInit();
                bitmap.StreamSource = strm;
                bitmap.EndInit();
                PhotoSource = bitmap;
                breturnValue = true;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return breturnValue;
        }

        private System.IO.FileInfo SaveFile(System.IO.FileInfo fileInfo)
        {
            string exportFileName = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            System.IO.FileInfo exportedFileInfo = new System.IO.FileInfo(fileInfo.DirectoryName + "\\PCCSExport.bmp");

            if (exportedFileInfo.Exists) exportedFileInfo.Delete();

            Microsoft.Office.Interop.Visio.Document doc;
            Microsoft.Office.Interop.Visio.Page page;
            Microsoft.Office.Interop.Visio.InvisibleApp app = null;
            try
            {
                if (fileInfo.Extension.ToUpper() == ".VSD")
                {
                    //if (ToolAdmin.MimeType.ToString().IndexOf("application") >= 0)
                    //{
                    app = new Microsoft.Office.Interop.Visio.InvisibleApp();
                    doc = app.Documents.Open(fileInfo.FullName);
                    page = app.ActivePage;

                    page.Export(exportedFileInfo.FullName);
                    app.Quit();

                    //}
                }
            }
            catch (Exception ex)
            {
                try
                {
                    app.Quit();
                }
                catch (Exception ex1)
                {
                    throw ex1.LogException();
                }
                throw ex.LogException();
            }
            return exportedFileInfo;
        }


        private readonly ICommand checklistClickCommand;
        public ICommand CheckListClickCommand { get { return this.checklistClickCommand; } }
        private void checklistReport()
        {
            if (!PccsModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            else
            {
                PartSubmissionWarrantModel pm = new PartSubmissionWarrantModel();
                pm.PART_NO = PccsModel.PartNo;
                DataTable processData;
                processData = _pccsBll.GetCheckList(pm.PART_NO, "CHKL");
                if (processData == null || processData.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {
                    frmReportViewer rv = new frmReportViewer(processData, "GetCheckList");
                    rv.ShowDialog();
                }
            }
        }

        private readonly ICommand initialSampleInspectionReportClickCommand;
        public ICommand InitialSampleInspectionReportClickCommand { get { return this.initialSampleInspectionReportClickCommand; } }
        private void initialReport()
        {
            if (!PccsModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            else
            {
                DataTable processData;
                processData = _pccsBll.GetInitialInspection(PccsModel.PartNo, PccsModel.PartDesc, PccsModel.RouteNo.ToString(), 0);
                if (processData == null || processData.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {
                    frmReportViewer rv = new frmReportViewer(processData, "Inspection");
                    rv.ShowDialog();
                }

            }
        }

        private void GeneratePFD()
        {
            List<PROCESS_SHEET> lstProcSheet;
            string wmfFilePath = "";
            string imageFilePath = "";
            string filePath = "";
            string fileName = "";
            string targetFileName = "";
            string pCell = "";
            string lCell = "";
            char ch = Convert.ToChar(34);
            Microsoft.Office.Interop.Visio.Document doc;
            Microsoft.Office.Interop.Visio.Page page;
            Microsoft.Office.Interop.Visio.InvisibleApp app = null;
            Microsoft.Office.Interop.Visio.Shape shape;
            try
            {

                filePath = GetFilePath();
                imageFilePath = filePath + "\\temp.bmp";
                wmfFilePath = filePath + "\\temp.wmf";
                if (System.IO.File.Exists(filePath + "\\temp.wmf"))
                {
                    System.IO.File.Delete(filePath + "\\temp.wmf");
                }
                if (System.IO.File.Exists(filePath + "\\stage2.wmf"))
                {
                    System.IO.File.Delete(filePath + "\\stage2.wmf");
                }
                lstProcSheet = (from row in _pccsBll.DB.PROCESS_SHEET
                                where row.PART_NO == _pccsModel.PartNo
                                    && row.ROUTE_NO == _pccsModel.RouteNo
                                select row).ToList<PROCESS_SHEET>();
                if (lstProcSheet.Count < 3)
                {
                    fileName = filePath + "\\F_ass_2.VSD";
                }
                else if (lstProcSheet.Count == 3)
                {
                    fileName = filePath + "\\F_ASS_3.VSD";
                }
                else if (lstProcSheet.Count == 4)
                {
                    fileName = filePath + "\\F_ASS_4.VSD";
                }
                else
                {
                    fileName = filePath + "\\FLOW.VSD";
                }



                if (lstProcSheet.Count > 13)
                {
                    System.Diagnostics.Process[] alreadyRunningProcesses = System.Diagnostics.Process.GetProcessesByName("VISIO");
                    foreach (System.Diagnostics.Process process in alreadyRunningProcesses)
                    {
                        process.Kill();
                        process.Close();
                    }
                    targetFileName = filePath + "\\COPY.VSD";
                    System.IO.File.Copy(fileName, targetFileName, true);

                    wmfFilePath = filePath + "\\temp.wmf";
                    imageFilePath = filePath + "\\temp.bmp";
                    Stage(lstProcSheet, 0, 12, fileName, targetFileName, imageFilePath, wmfFilePath, true);

                    wmfFilePath = filePath + "\\stage2.wmf";
                    imageFilePath = filePath + "\\stage2.bmp";
                    int endIndex;
                    if (lstProcSheet.Count > 25)
                    {
                        endIndex = 25;
                    }
                    else
                    {
                        endIndex = lstProcSheet.Count - 1;
                    }
                    Stage(lstProcSheet, 13, endIndex, fileName, targetFileName, imageFilePath, wmfFilePath, false);
                    if (lstProcSheet.Count > 25)
                    {
                        int diff;
                        diff = (lstProcSheet.Count - 25) + 1;
                        if (lstProcSheet.Count < 3)
                        {
                            fileName = filePath + "\\F_ass_2.VSD";
                        }
                        else if (lstProcSheet.Count == 3)
                        {
                            fileName = filePath + "\\F_ASS_3.VSD";
                        }
                        else if (lstProcSheet.Count == 4)
                        {
                            fileName = filePath + "\\F_ASS_4.VSD";
                        }
                        else
                        {
                            fileName = filePath + "\\FLOW.VSD";
                        }
                        wmfFilePath = filePath + "\\stage3.wmf";
                        imageFilePath = filePath + "\\stage3.bmp";
                        Stage(lstProcSheet, 26, lstProcSheet.Count - 1, fileName, targetFileName, imageFilePath, wmfFilePath, false);
                    }
                }
                else
                {
                    System.Diagnostics.Process[] alreadyRunningProcesses = System.Diagnostics.Process.GetProcessesByName("VISIO");
                    foreach (System.Diagnostics.Process process in alreadyRunningProcesses)
                    {
                        process.Kill();
                        process.Close();
                    }
                    targetFileName = filePath + "\\COPY.VSD";
                    System.IO.File.Copy(fileName, targetFileName, true);
                    app = new Microsoft.Office.Interop.Visio.InvisibleApp();
                    doc = app.Documents.Open(targetFileName);
                    page = app.ActivePage;
                    shape = page.Shapes[1];
                    for (int i = 0; i <= lstProcSheet.Count - 1; i++)
                    {
                        pCell = "Prop.P" + (i + 1).ToString().Trim();
                        lCell = "Prop.L" + (i + 1).ToString().Trim();
                        shape.Cells[pCell].Formula = ch + lstProcSheet[i].SEQ_NO.ToString("000") + ch;
                        if (lstProcSheet[i].TRANSPORT.ToValueAsString().Trim() == "")
                        {
                            ShowInformationMessage("One of the Values was Null. Check Process Sheet");
                        }
                        else
                        {
                            shape.Cells[lCell].Result[32] = Convert.ToInt16(lstProcSheet[i].TRANSPORT);
                        }
                    }
                    page.Export(imageFilePath);
                    page.Export(wmfFilePath);
                    app.Quit();
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(imageFilePath);
                    setImage(fileInfo);
                    System.IO.MemoryStream strm = new System.IO.MemoryStream();
                    using (System.IO.FileStream file = new System.IO.FileStream(wmfFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        _diagram = new byte[file.Length];
                        file.Read(_diagram, 0, (int)file.Length);
                        strm.Write(_diagram, 0, (int)file.Length);
                        file.Close();
                        file.Dispose();
                        //PhotoSource = bitmap;
                    }
                }


                /*
                System.Windows.Media.Imaging.BitmapImage bitmap = new System.Windows.Media.Imaging.BitmapImage();
                byte[] bytes;
                System.IO.MemoryStream strm = new System.IO.MemoryStream();
                using (System.IO.FileStream file = new System.IO.FileStream(imageFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    bytes = new byte[file.Length];
                    file.Read(bytes, 0, (int)file.Length);
                    strm.Write(bytes, 0, (int)file.Length);
                    file.Close();
                    file.Dispose();
                }
                bitmap.BeginInit();
                bitmap.StreamSource = strm;
                bitmap.EndInit();
                strm.Close();
                
                */
                ShowDiagramVisible = Visibility.Collapsed;
                HideDiagramVisible = Visibility.Visible;
                GenerateDiagramVisible = Visibility.Visible;
                DeleteDiagramVisible = Visibility.Visible;
                _imageChange = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void Stage(List<PROCESS_SHEET> lstProcSheet, int startIndex, int endIndex, string fileName, string targetFileName, string imageFilePath, string wmfFilePath, bool assignImage)
        {
            string pCell = "";
            string lCell = "";
            int new1 = 0;
            char ch = Convert.ToChar(34);
            Microsoft.Office.Interop.Visio.Document doc;
            Microsoft.Office.Interop.Visio.Page page;
            Microsoft.Office.Interop.Visio.InvisibleApp app = null;
            Microsoft.Office.Interop.Visio.Shape shape;

            try
            {

                System.Diagnostics.Process[] alreadyRunningProcesses = System.Diagnostics.Process.GetProcessesByName("VISIO");
                foreach (System.Diagnostics.Process process in alreadyRunningProcesses)
                {
                    process.Kill();
                    process.Close();
                }
                //targetFileName = filePath + "\\COPY.VSD";
                System.IO.File.Copy(fileName, targetFileName, true);
                app = new Microsoft.Office.Interop.Visio.InvisibleApp();
                doc = app.Documents.Open(targetFileName);
                page = app.ActivePage;
                shape = page.Shapes[1];
                for (int i = startIndex; i <= endIndex; i++)
                {
                    pCell = "Prop.P" + (new1 + 1).ToString().Trim();
                    lCell = "Prop.L" + (new1 + 1).ToString().Trim();
                    shape.Cells[pCell].Formula = ch + lstProcSheet[i].SEQ_NO.ToString("000") + ch;
                    if (lstProcSheet[i].TRANSPORT.ToValueAsString().Trim() == "")
                    {
                        ShowInformationMessage("One of the Values was Null. Check Process Sheet");
                    }
                    else
                    {
                        shape.Cells[lCell].Result[32] = Convert.ToInt16(lstProcSheet[i].TRANSPORT);
                    }
                    new1 = new1 + 1;
                }
                shape.Cells["Prop.NoOfProcess"].Result[32] = new1;
                page.Export(imageFilePath);
                page.Export(wmfFilePath);
                app.Quit();
                if (assignImage == true)
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(imageFilePath);
                    setImage(fileInfo);
                    System.IO.MemoryStream strm = new System.IO.MemoryStream();
                    using (System.IO.FileStream file = new System.IO.FileStream(wmfFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        _diagram = new byte[file.Length];
                        file.Read(_diagram, 0, (int)file.Length);
                        strm.Write(_diagram, 0, (int)file.Length);
                        file.Close();
                        file.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void StageNew(List<PROCESS_SHEET> lstProcSheet, int startIndex, int endIndex, string fileName, string targetFileName, string imageFilePath, string wmfFilePath, bool assignImage)
        {
            string pCell = "";
            string lCell = "";
            int new1 = 0;
            char ch = Convert.ToChar(34);
            Microsoft.Office.Interop.Visio.Document doc;
            Microsoft.Office.Interop.Visio.Page page;
            Microsoft.Office.Interop.Visio.InvisibleApp app = null;
            Microsoft.Office.Interop.Visio.Shape shape;
            try
            {
                System.Diagnostics.Process[] alreadyRunningProcesses = System.Diagnostics.Process.GetProcessesByName("VISIO");
                foreach (System.Diagnostics.Process process in alreadyRunningProcesses)
                {
                    process.Kill();
                    process.Close();
                }
                //targetFileName = filePath + "\\COPY.VSD";
                System.IO.File.Copy(fileName, targetFileName, true);
                app = new Microsoft.Office.Interop.Visio.InvisibleApp();
                doc = app.Documents.Open(targetFileName);
                page = app.ActivePage;
                shape = page.Shapes[1];
                for (int i = startIndex; i <= endIndex; i++)
                {
                    pCell = "Prop.P" + (new1 + 1).ToString().Trim();
                    lCell = "Prop.L" + (new1 + 1).ToString().Trim();
                    shape.Cells[pCell].Formula = ch + lstProcSheet[i].SEQ_NO.ToString("000") + ch;
                    if (lstProcSheet[i].TRANSPORT.ToValueAsString().Trim() == "")
                    {
                        ShowInformationMessage("One of the Values was Null. Check Process Sheet");
                    }
                    else
                    {
                        shape.Cells[lCell].Result[32] = Convert.ToInt16(lstProcSheet[i].TRANSPORT);
                    }
                    new1 = new1 + 1;
                }
                shape.Cells["Prop.NoOfProcess"].Result[32] = new1;
                page.Export(imageFilePath);
                page.Export(wmfFilePath);
                app.Quit();
                if (assignImage == true)
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(imageFilePath);
                    setImage(fileInfo);
                    System.IO.MemoryStream strm = new System.IO.MemoryStream();
                    using (System.IO.FileStream file = new System.IO.FileStream(wmfFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        _diagram = new byte[file.Length];
                        file.Read(_diagram, 0, (int)file.Length);
                        strm.Write(_diagram, 0, (int)file.Length);
                        file.Close();
                        file.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand _flowChartClickCommand;
        public ICommand FlowChartClickCommand { get { return this._flowChartClickCommand; } }
        private void flowChartClick()
        {
            //if (!_pccsBll.IsNotNullOrEmpty() || !_pccsBll.DB.IsNotNullOrEmpty()) return;

            if (!PccsModel.PartNo.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }

            FlowChartReportViewModel flowChartReportViewModel = new FlowChartReportViewModel(_userInformation);
            flowChartReportViewModel.ShowFlowChart(PccsModel.PartNo, PccsModel.RouteNo.ToValueAsString().ToDecimalValue());

            //List<Model.PROC_FLOW_DGM> lstPROC_FLOW_DGM = (from row in _pccsBll.DB.PROC_FLOW_DGM
            //                                              where row.PART_NO == PccsModel.PartNo
            //                                              select row).ToList<Model.PROC_FLOW_DGM>();

            //Model.PROC_FLOW_DGM proc_flow_dgm = null;

            //System.IO.MemoryStream strm;
            //byte[] bytes = null;

            //if (lstPROC_FLOW_DGM.IsNotNullOrEmpty() && lstPROC_FLOW_DGM.Count > 0)
            //{
            //    proc_flow_dgm = lstPROC_FLOW_DGM[0];
            //    byte[] photosource = proc_flow_dgm.DIAGRAM.ToArray();
            //    string fileType = null;
            //    int offset = GetImageBytesFromOLEField(photosource, ref fileType);

            //    strm = new System.IO.MemoryStream();
            //    strm.Write(photosource, offset, photosource.Length - offset);
            //    string mimeType = getMimeFromFile(photosource);

            //    string displayFile_Name = null;
            //    if (fileType == null && mimeType.Contains("wmf")) fileType = "wmf";
            //    if (fileType == null) fileType = "vsd";
            //    string file_Name = GetFilePath() + "\\TMP_FLOW_CHART_REPORT.VSD";

            //    if (fileType == "vsd")
            //    {
            //        displayFile_Name = GetFilePath() + "\\File_FLOW_CHART_REPORT.bmp";
            //    }
            //    else
            //    {
            //        displayFile_Name = GetFilePath() + "\\File_FLOW_CHART_REPORT." + fileType;
            //    }

            //    System.IO.FileStream fileStream = System.IO.File.Create((fileType != "vsd" ? displayFile_Name : file_Name));
            //    strm.Seek(0, System.IO.SeekOrigin.Begin);
            //    strm.CopyTo(fileStream);
            //    fileStream.Close();
            //    fileStream.Dispose();
            //    strm.Close();
            //    strm.Dispose();


            //    if (SaveFile(fileType, mimeType, file_Name, displayFile_Name) == true)
            //    {
            //        strm = new System.IO.MemoryStream();
            //        using (System.IO.FileStream file = new System.IO.FileStream(displayFile_Name, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            //        {
            //            bytes = new byte[file.Length];
            //            file.Read(bytes, 0, (int)file.Length);
            //            strm.Write(bytes, 0, (int)file.Length);
            //            file.Close();
            //            file.Dispose();
            //        }
            //    }

            //}

            //DataTable dtPROC_FLOW_DGM = (from row in _pccsBll.DB.PROC_FLOW_DGM
            //                             where row.PART_NO == PccsModel.PartNo && row.ROUTE_NO == PccsModel.RouteNo
            //                             select row).ToList<Model.PROC_FLOW_DGM>().ToDataTable<Model.PROC_FLOW_DGM>();
            //if (!dtPROC_FLOW_DGM.IsNotNullOrEmpty() || dtPROC_FLOW_DGM.Rows.Count == 0)
            //{
            //    ShowInformationMessage(PDMsg.NoRecordsPrint);
            //    return;
            //}

            //dtPROC_FLOW_DGM.TableName = "PROC_FLOW_DGM";
            //if (dtPROC_FLOW_DGM.Columns.Contains("DIAGRAM"))
            //{
            //    dtPROC_FLOW_DGM.Columns.Remove("DIAGRAM");
            //    dtPROC_FLOW_DGM.Columns.Add("DIAGRAM", bytes.GetType());
            //}

            //if (dtPROC_FLOW_DGM.Columns.Contains("PROCESS_MAIN"))
            //    dtPROC_FLOW_DGM.Columns.Remove("PROCESS_MAIN");

            //if (dtPROC_FLOW_DGM.Rows.Count > 0)
            //{
            //    dtPROC_FLOW_DGM.Rows[0]["DIAGRAM"] = bytes;
            //    foreach (DataRow row in dtPROC_FLOW_DGM.Rows)
            //    {
            //        row["ROUTE_NO"] = "Process  No. : " + Convert.ToString(row["ROUTE_NO"]).Trim();
            //        row.EndEdit();
            //        dtPROC_FLOW_DGM.AcceptChanges();
            //    }
            //}

            //List<PRD_MAST> lstPRD_MAST = (from pm in _pccsBll.DB.PRD_MAST
            //                              where pm.PART_NO == PccsModel.PartNo
            //                              select pm).ToList<PRD_MAST>();


            //DataTable dtPRD_MAST = new DataTable();
            //if (lstPRD_MAST.IsNotNullOrEmpty())
            //{
            //    dtPRD_MAST = lstPRD_MAST.ToDataTable<PRD_MAST>();
            //    if (dtPRD_MAST.Columns.Contains("PRD_DWG_ISSUE")) dtPRD_MAST.Columns.Remove("PRD_DWG_ISSUE");

            //}

            //if (!dtPRD_MAST.Columns.Contains("PART_NO")) dtPRD_MAST.Columns.Add("PART_NO");
            //if (!dtPRD_MAST.Columns.Contains("LOC_CODE")) dtPRD_MAST.Columns.Add("LOC_CODE");

            //if (dtPRD_MAST.Rows.Count == 0)
            //{
            //    DataRow dataRow = dtPRD_MAST.Rows.Add();
            //    dataRow["PART_NO"] = PccsModel.PartNo;
            //    dataRow["LOC_CODE"] = string.Empty;
            //}

            //List<DDLOC_MAST> lstDDLOC_MAST = (from lm in _pccsBll.DB.DDLOC_MAST
            //                                  join pm in _pccsBll.DB.PRD_MAST on lm.LOC_CODE equals pm.BIF_PROJ
            //                                  where pm.PART_NO == PccsModel.PartNo
            //                                  select lm).ToList<DDLOC_MAST>();

            //DataTable dtDDLOC_MAST = new DataTable();
            //if (lstDDLOC_MAST.IsNotNullOrEmpty())
            //{
            //    dtDDLOC_MAST = lstDDLOC_MAST.ToDataTable<DDLOC_MAST>();
            //}

            //if (!dtDDLOC_MAST.Columns.Contains("LOC_CODE")) dtDDLOC_MAST.Columns.Add("LOC_CODE");
            //if (!dtDDLOC_MAST.Columns.Contains("LOCATION")) dtDDLOC_MAST.Columns.Add("LOCATION");
            //if (dtDDLOC_MAST.Rows.Count == 0)
            //{
            //    DataRow dataRow = dtPRD_MAST.Rows.Add();
            //    dataRow["LOC_CODE"] = string.Empty;
            //}

            //List<PRD_CIREF> lstPRD_CIREF = (from pm in _pccsBll.DB.PRD_CIREF
            //                                where pm.PART_NO == PccsModel.PartNo
            //                                select pm).ToList<PRD_CIREF>();

            //DataTable dtPRD_CIREF = new DataTable();
            //if (lstPRD_CIREF.IsNotNullOrEmpty())
            //{
            //    dtPRD_CIREF = lstPRD_CIREF.ToDataTable<PRD_CIREF>();
            //    if (dtPRD_CIREF.Columns.Contains("DDCI_INFO")) dtPRD_CIREF.Columns.Remove("DDCI_INFO");
            //}

            //if (!dtPRD_CIREF.Columns.Contains("PART_NO")) dtPRD_CIREF.Columns.Add("PART_NO");
            //if (dtPRD_CIREF.Rows.Count == 0)
            //{
            //    DataRow dataRow = dtPRD_CIREF.Rows.Add();
            //    dataRow["PART_NO"] = PccsModel.PartNo;
            //    dataRow["CI_REF"] = string.Empty;
            //}

            //List<DDCI_INFO> lstDDCI_INFO = (from ci in _pccsBll.DB.DDCI_INFO
            //                                join pc in _pccsBll.DB.PRD_CIREF on ci.CI_REFERENCE equals pc.CI_REF
            //                                where pc.PART_NO == PccsModel.PartNo
            //                                select ci).ToList<DDCI_INFO>();

            //DataTable dtDDCI_INFO = new DataTable();
            //if (lstDDCI_INFO.IsNotNullOrEmpty())
            //{
            //    dtDDCI_INFO = lstDDCI_INFO.ToDataTable<DDCI_INFO>();

            //    if (dtDDCI_INFO.Columns.Contains("PRD_CIREF")) dtDDCI_INFO.Columns.Remove("PRD_CIREF");
            //    if (dtDDCI_INFO.Columns.Contains("DDSHAPE_DETAILS")) dtDDCI_INFO.Columns.Remove("DDSHAPE_DETAILS");
            //    if (dtDDCI_INFO.Columns.Contains("DDCOST_PROCESS_DATA")) dtDDCI_INFO.Columns.Remove("DDCOST_PROCESS_DATA");

            //    foreach (DataRow row in dtDDCI_INFO.Rows)
            //    {
            //        row["CUST_DWG_NO_ISSUE"] = Convert.ToString(row["CUST_DWG_NO_ISSUE"]).Trim() + " Dt. " +
            //                                   (Convert.ToString(row["CUST_STD_DATE"]).Trim().Length > 10 ?
            //                                   row["CUST_STD_DATE"].ToValueAsString().Substring(3, 10) : "");

            //        //row["CUST_DWG_NO_ISSUE"] = Convert.ToString(row["CUST_DWG_NO_ISSUE"]).Trim() + " Dt. " +
            //        //                       (row["CUST_STD_DATE"].ToValueAsString());

            //        row.EndEdit();
            //        dtDDCI_INFO.AcceptChanges();
            //    }

            //}

            //if (!dtDDCI_INFO.Columns.Contains("CI_REFERENCE")) dtDDCI_INFO.Columns.Add("CI_REFERENCE");
            //if (!dtDDCI_INFO.Columns.Contains("CUST_CODE")) dtDDCI_INFO.Columns.Add("CUST_CODE");
            //if (dtDDCI_INFO.Rows.Count == 0)
            //{
            //    DataRow dataRow = dtDDCI_INFO.Rows.Add();
            //    dataRow["CI_REFERENCE"] = string.Empty;
            //    dataRow["CUST_CODE"] = string.Empty;
            //}

            //List<DDCUST_MAST> lstDDCUST_MAST = (from ci in _pccsBll.DB.DDCI_INFO
            //                                    join pc in _pccsBll.DB.PRD_CIREF on ci.CI_REFERENCE equals pc.CI_REF
            //                                    join cm in _pccsBll.DB.DDCUST_MAST on ci.CUST_CODE equals cm.CUST_CODE
            //                                    where pc.PART_NO == PccsModel.PartNo
            //                                    select cm).ToList<DDCUST_MAST>();


            //DataTable dtDDCUST_MAST = new DataTable();
            //if (lstDDCUST_MAST.IsNotNullOrEmpty())
            //{
            //    dtDDCUST_MAST = lstDDCUST_MAST.ToDataTable<DDCUST_MAST>();
            //}

            //if (!dtDDCUST_MAST.Columns.Contains("CUST_CODE")) dtDDCUST_MAST.Columns.Add("CUST_CODE");
            //if (!dtDDCUST_MAST.Columns.Contains("CUST_NAME")) dtDDCUST_MAST.Columns.Add("CUST_NAME");

            //if (dtDDCUST_MAST.Rows.Count == 0)
            //{
            //    DataRow dataRow = dtDDCUST_MAST.Rows.Add();
            //    dataRow["CUST_CODE"] = string.Empty;
            //}

            //DataTable dtPROCESS_ISSUE = (from row in _pccsBll.DB.PROCESS_ISSUE
            //                             where row.PART_NO == PccsModel.PartNo && row.ROUTE_NO == PccsModel.RouteNo
            //                             select row).ToList<PROCESS_ISSUE>().ToDataTableWithType<PROCESS_ISSUE>();

            //if (!dtPROCESS_ISSUE.IsNotNullOrEmpty()) dtPROCESS_ISSUE = new DataTable();

            //if (!dtPROCESS_ISSUE.Columns.Contains("PART_NO")) dtPROCESS_ISSUE.Columns.Add("PART_NO");
            //if (!dtPROCESS_ISSUE.Columns.Contains("ROUTE_NO")) dtPROCESS_ISSUE.Columns.Add("ROUTE_NO");

            ////if (!dtPROCESS_ISSUE.Columns.Contains("LOCATION")) dtPROCESS_ISSUE.Columns.Add("LOCATION");

            ////if (!dtPROCESS_ISSUE.Columns.Contains("CUST_CODE")) dtPROCESS_ISSUE.Columns.Add("CUST_CODE");
            ////if (!dtPROCESS_ISSUE.Columns.Contains("CUST_NAME")) dtPROCESS_ISSUE.Columns.Add("CUST_NAME");
            ////if (!dtPROCESS_ISSUE.Columns.Contains("CUST_STD_DATE"))
            ////    dtPROCESS_ISSUE.Columns.Add("CUST_STD_DATE", DateTime.Now.Date.GetType());
            ////if (!dtPROCESS_ISSUE.Columns.Contains("CUST_DWG_NO")) dtPROCESS_ISSUE.Columns.Add("CUST_DWG_NO");
            ////if (!dtPROCESS_ISSUE.Columns.Contains("PART_DESCRIPTION")) dtPROCESS_ISSUE.Columns.Add("PART_DESCRIPTION");
            ////if (!dtPROCESS_ISSUE.Columns.Contains("CUST_DWG_NO_ISSUE")) dtPROCESS_ISSUE.Columns.Add("CUST_DWG_NO_ISSUE");
            ////if (!dtPROCESS_ISSUE.Columns.Contains("FR_CS_DATE"))
            ////    dtPROCESS_ISSUE.Columns.Add("FR_CS_DATE", DateTime.Now.Date.GetType());

            //if (dtPROCESS_ISSUE.Rows.Count == 0)
            //{
            //    DataRow dataRow = dtPROCESS_ISSUE.Rows.Add();
            //    dataRow["PART_NO"] = PccsModel.PartNo;
            //    dataRow["ROUTE_NO"] = PccsModel.RouteNo;

            //    //dataRow["LOCATION"] = ddloc_mast.LOCATION;

            //    //dataRow["CUST_CODE"] = ddci_info.CUST_CODE;
            //    //dataRow["CUST_NAME"] = ddci_info.REMARKS;
            //    //dataRow["CUST_STD_DATE"] = Convert.ToDateTime(ddci_info.CUST_STD_DATE).Date;
            //    //dataRow["CUST_DWG_NO"] = ddci_info.CUST_DWG_NO;
            //    //dataRow["PART_DESCRIPTION"] = ddci_info.PROD_DESC;
            //    //dataRow["CUST_DWG_NO_ISSUE"] = ddci_info.CUST_DWG_NO_ISSUE;
            //    //dataRow["FR_CS_DATE"] = Convert.ToDateTime(ddci_info.FR_CS_DATE).Date;
            //    dataRow.EndEdit();

            //}

            //foreach (DataRow row in dtPROCESS_ISSUE.Rows)
            //{
            //    row["ISSUE_ALTER"] = (Convert.ToString(row["ISSUE_DATE"]).Trim().Length > 10 ?
            //                               row["ISSUE_DATE"].ToValueAsString().Substring(3, 10) : "");
            //    row.EndEdit();
            //    dtPROCESS_ISSUE.AcceptChanges();
            //}

            //if (dtPROCESS_ISSUE.Columns.Contains("PROCESS_MAIN"))
            //    dtPROCESS_ISSUE.Columns.Remove("PROCESS_MAIN");

            //dtPROCESS_ISSUE.TableName = "PROCESS_ISSUE";

            //DataTable dtPROCESS_SHEET = (from row in _pccsBll.DB.PROCESS_SHEET
            //                             where row.PART_NO == PccsModel.PartNo && row.ROUTE_NO == PccsModel.RouteNo
            //                             select row).ToList<PROCESS_SHEET>().ToDataTableWithType<PROCESS_SHEET>();

            //if (!dtPROCESS_SHEET.IsNotNullOrEmpty()) dtPROCESS_SHEET = new DataTable();

            //if (!dtPROCESS_SHEET.Columns.Contains("PART_NO")) dtPROCESS_SHEET.Columns.Add("PART_NO");
            //if (!dtPROCESS_SHEET.Columns.Contains("ROUTE_NO")) dtPROCESS_SHEET.Columns.Add("ROUTE_NO");

            //if (dtPROCESS_SHEET.Rows.Count == 0)
            //{
            //    DataRow dataRow = dtPROCESS_SHEET.Rows.Add();
            //    dataRow["PART_NO"] = PccsModel.PartNo;
            //    dataRow["ROUTE_NO"] = PccsModel.RouteNo;
            //    dataRow.EndEdit();
            //}

            //dtPROCESS_SHEET.TableName = "PROCESS_SHEET";

            //if (dtPROCESS_SHEET.Columns.Contains("PCCS"))
            //    dtPROCESS_SHEET.Columns.Remove("PCCS");

            //if (dtPROCESS_SHEET.Columns.Contains("PROCESS_CC"))
            //    dtPROCESS_SHEET.Columns.Remove("PROCESS_CC");

            //if (dtPROCESS_SHEET.Columns.Contains("PROCESS_MAIN"))
            //    dtPROCESS_SHEET.Columns.Remove("PROCESS_MAIN");

            //DataSet dsReport = new DataSet();
            //dsReport.DataSetName = "FLOW_CHART_REPORT_DATA_SET";

            //dsReport.Tables.Add(dtPROC_FLOW_DGM);
            //dsReport.Tables.Add(dtPROCESS_ISSUE);
            //dsReport.Tables.Add(dtPROCESS_SHEET);
            //dsReport.Tables.Add(dtPRD_MAST);
            //dsReport.Tables.Add(dtDDLOC_MAST);
            //dsReport.Tables.Add(dtPRD_CIREF);
            //dsReport.Tables.Add(dtDDCI_INFO);
            //dsReport.Tables.Add(dtDDCUST_MAST);

            //DataColumn pfdPrimaryKeyColumn = null;
            //DataColumn lmPrimaryKeyColumn = null;
            //DataColumn cmPrimaryKeyColumn = null;
            //DataColumn ciPrimaryKeyColumn = null;

            //DataColumn piChildColumn = null;
            //ForeignKeyConstraint piForeignKeyConstraint = null;

            //DataColumn psChildColumn = null;
            //ForeignKeyConstraint psForeignKeyConstraint = null;

            //DataColumn pcChildColumnPN = null;
            //ForeignKeyConstraint pcForeignKeyConstraintPN = null;
            //DataColumn pcChildColumnCI = null;
            //ForeignKeyConstraint pcForeignKeyConstraintCI = null;

            //DataColumn pmChildColumnPN = null;
            //ForeignKeyConstraint pmForeignKeyConstraintPN = null;
            //DataColumn pmChildColumnLM = null;
            //ForeignKeyConstraint pmForeignKeyConstraintLM = null;

            //DataColumn ciChildColumnCM = null;
            //ForeignKeyConstraint ciForeignKeyConstraintCM = null;

            //if (dtPROC_FLOW_DGM.PrimaryKey.IsNotNullOrEmpty() && dtPROC_FLOW_DGM.PrimaryKey.Length == 0)
            //{
            //    DataColumn[] primaryKeyColumns = new DataColumn[1];
            //    primaryKeyColumns[0] = dtPROC_FLOW_DGM.Columns["PART_NO"];
            //    pfdPrimaryKeyColumn = primaryKeyColumns[0];
            //    dtPROC_FLOW_DGM.PrimaryKey = primaryKeyColumns;

            //    primaryKeyColumns = new DataColumn[1];
            //    primaryKeyColumns[0] = dtDDLOC_MAST.Columns["LOC_CODE"];
            //    lmPrimaryKeyColumn = primaryKeyColumns[0];
            //    dtDDLOC_MAST.PrimaryKey = primaryKeyColumns;

            //    primaryKeyColumns = new DataColumn[1];
            //    primaryKeyColumns[0] = dtDDCUST_MAST.Columns["CUST_CODE"];
            //    cmPrimaryKeyColumn = primaryKeyColumns[0];
            //    dtDDCUST_MAST.PrimaryKey = primaryKeyColumns;

            //    primaryKeyColumns = new DataColumn[1];
            //    primaryKeyColumns[0] = dtDDCI_INFO.Columns["CI_REFERENCE"];
            //    ciPrimaryKeyColumn = primaryKeyColumns[0];
            //    dtDDCI_INFO.PrimaryKey = primaryKeyColumns;

            //    piChildColumn = dtPROCESS_ISSUE.Columns["PART_NO"];
            //    piForeignKeyConstraint = new ForeignKeyConstraint("PIForeignKeyConstraint", pfdPrimaryKeyColumn, piChildColumn);
            //    piForeignKeyConstraint.DeleteRule = Rule.SetNull;
            //    piForeignKeyConstraint.UpdateRule = Rule.Cascade;
            //    piForeignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PROCESS_ISSUE"].Constraints.Add(piForeignKeyConstraint);

            //    psChildColumn = dtPROCESS_SHEET.Columns["PART_NO"];
            //    psForeignKeyConstraint = new ForeignKeyConstraint("PSForeignKeyConstraint", pfdPrimaryKeyColumn, psChildColumn);
            //    psForeignKeyConstraint.DeleteRule = Rule.SetNull;
            //    psForeignKeyConstraint.UpdateRule = Rule.Cascade;
            //    psForeignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PROCESS_SHEET"].Constraints.Add(psForeignKeyConstraint);

            //    pcChildColumnPN = dtPRD_CIREF.Columns["PART_NO"];
            //    pcForeignKeyConstraintPN = new ForeignKeyConstraint("pcForeignKeyConstraint_PART_NO", pfdPrimaryKeyColumn, pcChildColumnPN);
            //    pcForeignKeyConstraintPN.DeleteRule = Rule.SetNull;
            //    pcForeignKeyConstraintPN.UpdateRule = Rule.Cascade;
            //    pcForeignKeyConstraintPN.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PRD_CIREF"].Constraints.Add(pcForeignKeyConstraintPN);

            //    pcChildColumnCI = dtPRD_CIREF.Columns["CI_REF"];
            //    pcForeignKeyConstraintCI = new ForeignKeyConstraint("pcForeignKeyConstraint_CI_REF", ciPrimaryKeyColumn, pcChildColumnCI);
            //    pcForeignKeyConstraintCI.DeleteRule = Rule.SetNull;
            //    pcForeignKeyConstraintCI.UpdateRule = Rule.Cascade;
            //    pcForeignKeyConstraintCI.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PRD_CIREF"].Constraints.Add(pcForeignKeyConstraintCI);

            //    pmChildColumnPN = dtPRD_MAST.Columns["PART_NO"];
            //    pmForeignKeyConstraintPN = new ForeignKeyConstraint("PMForeignKeyConstraint_PART_NO", pfdPrimaryKeyColumn, pmChildColumnPN);
            //    pmForeignKeyConstraintPN.DeleteRule = Rule.SetNull;
            //    pmForeignKeyConstraintPN.UpdateRule = Rule.Cascade;
            //    pmForeignKeyConstraintPN.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PRD_MAST"].Constraints.Add(pmForeignKeyConstraintPN);

            //    pmChildColumnLM = dtPRD_MAST.Columns["LOC_CODE"];
            //    pmForeignKeyConstraintLM = new ForeignKeyConstraint("PMForeignKeyConstraint_LOC_CODE", lmPrimaryKeyColumn, pmChildColumnLM);
            //    pmForeignKeyConstraintLM.DeleteRule = Rule.SetNull;
            //    pmForeignKeyConstraintLM.UpdateRule = Rule.Cascade;
            //    pmForeignKeyConstraintLM.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["PRD_MAST"].Constraints.Add(pmForeignKeyConstraintLM);

            //    ciChildColumnCM = dtDDCI_INFO.Columns["CUST_CODE"];
            //    ciForeignKeyConstraintCM = new ForeignKeyConstraint("ciForeignKeyConstraint_CUST_CODE", cmPrimaryKeyColumn, ciChildColumnCM);
            //    ciForeignKeyConstraintCM.DeleteRule = Rule.SetNull;
            //    ciForeignKeyConstraintCM.UpdateRule = Rule.Cascade;
            //    ciForeignKeyConstraintCM.AcceptRejectRule = AcceptRejectRule.None;
            //    dsReport.Tables["DDCI_INFO"].Constraints.Add(ciForeignKeyConstraintCM);

            //    dsReport.EnforceConstraints = true;

            //}

            ////dsReport.Tables["PROCESS_SHEET"].Rows.Clear();
            ////dsReport.Tables["PROCESS_ISSUE"].Rows.Clear();

            ////dsReport.Tables["PRD_MAST"].Rows.Clear();
            ////dsReport.Tables["PRD_CIREF"].Rows.Clear();
            ////dsReport.Tables["DDCI_INFO"].Rows.Clear();
            ////dsReport.Tables["DDCUST_MAST"].Rows.Clear();
            ////dsReport.Tables["DDLOC_MAST"].Rows.Clear();
            ////dsReport.Tables["PROC_FLOW_DGM"].Rows.Clear();

            ////dsReport.WriteXml("E:\\" + dsReport.DataSetName + ".xml", XmlWriteMode.WriteSchema);

            //if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count < 0)
            //{
            //    ShowInformationMessage(PDMsg.NoRecordsPrint);
            //    return;
            //}

            //frmReportViewer reportViewer = new frmReportViewer(dsReport, "FlowChart");
            //if (!reportViewer.ReadyToShowReport) return;
            //reportViewer.ShowDialog();
        }

        //private int GetImageBytesFromOLEField(byte[] oleFieldBytes, ref string fileType)
        //{
        //    const string BITMAP_ID_BLOCK = "BM";
        //    const string JPG_ID_BLOCK = "\u00FF\u00D8\u00FF";
        //    const string PNG_ID_BLOCK = "\u0089PNG\r\n\u001a\n";
        //    const string GIF_ID_BLOCK = "GIF8";
        //    const string TIFF_ID_BLOCK = "II*\u0000";
        //    const string VSD_ID_BLOCK = "ÐÏà¡±á";

        //    //byte[] imageBytes;

        //    // Get a UTF7 Encoded string version
        //    Encoding u8 = Encoding.UTF7;
        //    string strTemp = u8.GetString(oleFieldBytes);

        //    // Get the first 300 characters from the string
        //    string strVTemp = strTemp.Substring(0, 300);

        //    // Search for the block
        //    int iPos = -1;
        //    if (strVTemp.IndexOf(BITMAP_ID_BLOCK) != -1)
        //    {
        //        iPos = strVTemp.IndexOf(BITMAP_ID_BLOCK);
        //        fileType = "bmp";
        //    }
        //    else if (strVTemp.IndexOf(JPG_ID_BLOCK) != -1)
        //    {
        //        iPos = strVTemp.IndexOf(JPG_ID_BLOCK);
        //        fileType = "bmp";
        //    }
        //    else if (strVTemp.IndexOf(PNG_ID_BLOCK) != -1)
        //    {
        //        iPos = strVTemp.IndexOf(PNG_ID_BLOCK);
        //        fileType = "png";
        //    }
        //    else if (strVTemp.IndexOf(GIF_ID_BLOCK) != -1)
        //    {
        //        iPos = strVTemp.IndexOf(GIF_ID_BLOCK);
        //        fileType = "gif";
        //    }
        //    else if (strVTemp.IndexOf(TIFF_ID_BLOCK) != -1)
        //    {
        //        iPos = strVTemp.IndexOf(TIFF_ID_BLOCK);
        //        fileType = "tiff";
        //    }
        //    else if (strVTemp.IndexOf(VSD_ID_BLOCK) != -1)
        //    {
        //        fileType = "vsd";
        //        iPos = strVTemp.IndexOf(VSD_ID_BLOCK);
        //    }

        //    if (iPos == -1)
        //    {
        //        iPos = 0;
        //    }
        //    return iPos;
        //}

        //public string getMimeFromFile(byte[] byteArray)
        //{

        //    byte[] buffer = new byte[256];
        //    using (System.IO.MemoryStream fs = new System.IO.MemoryStream(byteArray))
        //    {
        //        if (fs.Length >= 256)
        //            fs.Read(buffer, 0, 256);
        //        else
        //            fs.Read(buffer, 0, (int)fs.Length);
        //    }
        //    try
        //    {
        //        System.UInt32 mimetype;
        //        FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
        //        System.IntPtr mimeTypePtr = new IntPtr(mimetype);
        //        string mime = Marshal.PtrToStringUni(mimeTypePtr);
        //        Marshal.FreeCoTaskMem(mimeTypePtr);
        //        return mime;
        //    }
        //    catch (Exception e)
        //    {
        //        e.LogException();
        //        return e.Message;
        //    }
        //}

        //[DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        //private extern static System.UInt32 FindMimeFromData(
        //    System.UInt32 pBC,
        //    [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
        //    [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
        //    System.UInt32 cbSize,
        //    [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
        //    System.UInt32 dwMimeFlags,
        //    out System.UInt32 ppwzMimeOut,
        //    System.UInt32 dwReserverd);

        //private Boolean SaveFile(string fileType, string mimeType, string file_Name, string displayFile_Name)
        //{
        //    Microsoft.Office.Interop.Visio.Document doc;
        //    Microsoft.Office.Interop.Visio.Page page;
        //    Microsoft.Office.Interop.Visio.InvisibleApp app = null;
        //    try
        //    {
        //        if (fileType.ToUpper() == "VSD")
        //        {
        //            if (mimeType.ToString().IndexOf("application") >= 0)
        //            {
        //                app = new Microsoft.Office.Interop.Visio.InvisibleApp();
        //                doc = app.Documents.Open(file_Name);
        //                page = app.ActivePage;
        //                page.Export(displayFile_Name);
        //                app.Quit();

        //            }
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        try
        //        {
        //            app.Quit();
        //            return false;
        //        }
        //        catch (Exception ex1)
        //        {
        //            return false;
        //            throw ex1.LogException();
        //        }
        //        throw ex.LogException();
        //    }
        //}

        private void ClearImage()
        {
            try
            {
                PhotoSource = ProcessDesigner.Properties.Resources.Blank_Image.ToBitmapImage();
                _diagram = null;
                ShowDiagramVisible = Visibility.Visible;
                HideDiagramVisible = Visibility.Collapsed;
                GenerateDiagramVisible = Visibility.Collapsed;
                DeleteDiagramVisible = Visibility.Collapsed;
                _imageChange = false;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void GeneratePFDNew()
        {
            string imageFilePath = "";
            string filePath = "";
            string wmfFilePath = "";
            List<PROCESS_SHEET> lstProcSheet;
            Microsoft.Office.Interop.Visio.Page visioPage;
            Microsoft.Office.Interop.Visio.InvisibleApp visApp;
            Microsoft.Office.Interop.Visio.Documents visioDocs;
            Microsoft.Office.Interop.Visio.Document visioStencil;
            Microsoft.Office.Interop.Visio.Shape visioStart = null;
            Microsoft.Office.Interop.Visio.Shape visioOperationShape;
            Microsoft.Office.Interop.Visio.Shape visioTransportationShape = null;
            Microsoft.Office.Interop.Visio.Shape visioNextTransportationShape = null;
            Microsoft.Office.Interop.Visio.Shape visioOperationShape1;
            Microsoft.Office.Interop.Visio.Shape visioInspectionShape;
            Microsoft.Office.Interop.Visio.Shape visioPrev = null;
            Microsoft.Office.Interop.Visio.Master visioTransportationMaster;
            Microsoft.Office.Interop.Visio.Master visioOperation;
            Microsoft.Office.Interop.Visio.Master visioInspection;
            Microsoft.Office.Interop.Visio.Master visioSmall;
            Microsoft.Office.Interop.Visio.Cell celObj;
            //Dim celObj   As Visio.Cell 

            double iDot = -1.49;
            int minConnector = 0;
            int maxConnector = 0;
            int maxCircleCount = 5;
            int lineCount = 1;
            int circleCount = 0;
            double yPosition = 10.5;
            int itransport = 0;
            double dblsize = -.75;
            double dblconnectDistance = 7;
            double dblxConnectDistance = 5;

            try
            {
                filePath = GetFilePath();
                wmfFilePath = filePath + "\\temp.wmf";
                imageFilePath = filePath + "\\temp.bmp";

                lstProcSheet = (from row in _pccsBll.DB.PROCESS_SHEET
                                where row.PART_NO == _pccsModel.PartNo
                                    && row.ROUTE_NO == _pccsModel.RouteNo
                                select row).ToList<PROCESS_SHEET>();

                maxConnector = lstProcSheet.Count - 1;


                System.Diagnostics.Process[] alreadyRunningProcesses = System.Diagnostics.Process.GetProcessesByName("VISIO");
                foreach (System.Diagnostics.Process process in alreadyRunningProcesses)
                {
                    process.Kill();
                    process.Close();
                }

                visApp = new Microsoft.Office.Interop.Visio.InvisibleApp();

                visApp.Documents.Add(@"");
                visioDocs = visApp.Documents;
                //Microsoft.Office.Interop.Visio.Document visioStencil = visioDocs.OpenEx("Basic Shapes.vss",
                //    (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenDocked);

                visioStencil = visioDocs.OpenEx("TQM Diagram Shapes.vss",
                (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenDocked);

                visioTransportationMaster = visioStencil.Masters.get_ItemU(@"Transportation"); //Arrow
                visioOperation = visioStencil.Masters.get_ItemU(@"Operation"); //Circle
                visioInspection = visioStencil.Masters.get_ItemU(@"Inspection/ measurement"); //Square
                visioSmall = visioStencil.Masters.get_ItemU(@"Inspection/ measurement"); //Square
                visioPage = visApp.ActivePage;


                for (int ictr = minConnector; ictr <= maxConnector; ictr++)
                {
                    double xPosition = ictr;

                    if (ictr == minConnector)
                    {
                        xPosition = 0.5 + ictr;
                    }
                    itransport = lstProcSheet[ictr].TRANSPORT.ToValueAsString().ToIntValue();
                    visioOperationShape = visioPage.Drop(visioOperation, xPosition, yPosition);
                    visioOperationShape.Text = ((ictr + 1) * 10).ToString("D3"); // Jeyan - Format to 3 digit ex: 10->010, 20->020 like wise
                    visioOperationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -.4, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                    visioOperationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -.4, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                    celObj = visioOperationShape.Cells["Char.Size"];
                    celObj.Formula = "=12 pt.";
                    if (visioNextTransportationShape != null)
                    {
                        if (lineCount % 2 != 0 && circleCount % maxCircleCount != 0)
                            visioNextTransportationShape.AutoConnect(visioOperationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirRight);
                        else if (lineCount % 2 == 0 && circleCount % maxCircleCount == 0)
                            visioNextTransportationShape.AutoConnect(visioOperationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown);
                        else if (lineCount % 2 != 0 && circleCount % maxCircleCount == 0)
                            visioNextTransportationShape.AutoConnect(visioOperationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown);
                        else
                            visioNextTransportationShape.AutoConnect(visioOperationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirLeft);
                    }
                    circleCount++;
                    if (circleCount % maxCircleCount == 0) lineCount++;
                    if (ictr != maxConnector)
                    {
                        if (lineCount % 2 != 0 && circleCount % maxCircleCount != 0)
                        {
                            if (itransport == 0)
                            {
                                visioTransportationShape = visioPage.Drop(visioOperation, (xPosition * 2), yPosition);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 1)
                            {
                                visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2), yPosition);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 2)
                            {
                                visioTransportationShape = visioPage.Drop(visioInspection, (xPosition * 2), yPosition);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 3)
                            {
                                visioTransportationShape = visioPage.Drop(visioSmall, (xPosition * 2), yPosition);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirRight);
                            //if (lineCount % 2 != 0)
                            //    visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirRight);
                            //else if (lineCount % 2 == 0 && circleCount % maxCircleCount == 0)
                            //    visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown);
                            //else
                            //    visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirLeft);

                        }
                        else if (lineCount % 2 == 0 && circleCount % maxCircleCount == 0)
                        {
                            if (itransport == 0)
                            {
                                visioTransportationShape = visioPage.Drop(visioOperation, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 1)
                            {
                                visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 2)
                            {
                                visioTransportationShape = visioPage.Drop(visioInspection, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 3)
                            {
                                visioTransportationShape = visioPage.Drop(visioSmall, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            visioTransportationShape.Rotate90();
                            visioTransportationShape.Rotate90();
                            visioTransportationShape.Rotate90();
                            visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown);
                            yPosition = yPosition - 1;
                            //return;
                        }
                        else if (lineCount % 2 != 0 && circleCount % maxCircleCount == 0)
                        {
                            if (itransport == 0)
                            {
                                visioTransportationShape = visioPage.Drop(visioOperation, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 1)
                            {
                                visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 2)
                            {
                                visioTransportationShape = visioPage.Drop(visioInspection, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 3)
                            {
                                visioTransportationShape = visioPage.Drop(visioSmall, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            visioTransportationShape.Rotate90();
                            visioTransportationShape.Rotate90();
                            visioTransportationShape.Rotate90();
                            visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown);
                            yPosition = yPosition - 1;
                        }
                        else
                        {
                            if (itransport == 0)
                            {
                                visioTransportationShape = visioPage.Drop(visioOperation, (xPosition * -2), yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 1)
                            {
                                visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * -2), yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 2)
                            {
                                visioTransportationShape = visioPage.Drop(visioInspection, (xPosition * -2), yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 3)
                            {
                                visioTransportationShape = visioPage.Drop(visioSmall, (xPosition * -2), yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            //visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * -2), yPosition - lineCount + 1);
                            //visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            //visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -.5, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            visioTransportationShape.Rotate90();
                            visioTransportationShape.Rotate90();
                            visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirLeft);
                        }
                    }
                    ////if (ictr > 0)
                    ////{
                    ////    try
                    ////    {
                    ////        string formula1 = "";
                    ////        string formula2 = "";
                    ////        formula1 = visioTransportationShape.CellsU["PinX"].Formula;
                    ////        formula1 = formula1.Replace("in.", "");
                    ////        formula1 = formula1.Replace("cm.", "");

                    ////        formula2 = visioOperationShape.CellsU["PinX"].Formula;
                    ////        formula2 = formula1.Replace("in.", "");
                    ////        formula2 = formula1.Replace("cm.", "");
                    ////        //visioOperationShape.CellsU["PinX"].Formula = (Convert.ToDouble(formula1) - .001).ToString() + " in.";
                    ////        //visioTransportationShape.CellsU["PinX"].Formula = (Convert.ToDouble(formula1) - .001).ToString() + " in.";
                    ////        //visioOperationShape.CellsU["PinX"].Formula = (Convert.ToDouble(formula2) - .05).ToString() + "  in.";
                    ////    }
                    ////    catch (Exception ex)
                    ////    {

                    ////    }
                    ////}
                    visioNextTransportationShape = visioTransportationShape;
                    //visioOperationShape.CellsU["PinY"].Formula = "2";
                }

                visioPage.Export(imageFilePath);
                visioPage.Export(wmfFilePath);
                //visApp.Quit();
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(imageFilePath);
                setImage(fileInfo);
                System.IO.MemoryStream strm = new System.IO.MemoryStream();
                using (System.IO.FileStream file = new System.IO.FileStream(wmfFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    _diagram = new byte[file.Length];
                    file.Read(_diagram, 0, (int)file.Length);
                    strm.Write(_diagram, 0, (int)file.Length);
                    file.Close();
                    file.Dispose();
                    //PhotoSource = bitmap;
                }
                ShowDiagramVisible = Visibility.Collapsed;
                HideDiagramVisible = Visibility.Visible;
                GenerateDiagramVisible = Visibility.Visible;
                DeleteDiagramVisible = Visibility.Visible;
                _imageChange = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void GenerateProcessFlowDiagram()
        {
            string imageFilePath = "";
            string filePath = "";
            string wmfFilePath = "";
            List<PROCESS_SHEET> lstProcSheet;
            Microsoft.Office.Interop.Visio.Page visioPage;
            Microsoft.Office.Interop.Visio.InvisibleApp visApp;
            Microsoft.Office.Interop.Visio.Documents visioDocs;
            Microsoft.Office.Interop.Visio.Document visioStencil;
            Microsoft.Office.Interop.Visio.Shape visioStart = null;
            Microsoft.Office.Interop.Visio.Shape visioOperationShape;
            Microsoft.Office.Interop.Visio.Shape visioTransportationShape = null;
            Microsoft.Office.Interop.Visio.Shape visioNextTransportationShape = null;
            Microsoft.Office.Interop.Visio.Shape visioTransportationShape1 = null;
            Microsoft.Office.Interop.Visio.Shape visioOperationShape1;
            Microsoft.Office.Interop.Visio.Shape visioInspectionShape;
            Microsoft.Office.Interop.Visio.Shape visioPrev = null;
            Microsoft.Office.Interop.Visio.Master visioTransportationMaster;
            Microsoft.Office.Interop.Visio.Master visioOperation;
            Microsoft.Office.Interop.Visio.Master visioInspection;
            Microsoft.Office.Interop.Visio.Master visioSmall;
            Microsoft.Office.Interop.Visio.Cell celObj;
            Microsoft.Office.Interop.Visio.Shape vsoConnectorShape = null;
            //Dim celObj   As Visio.Cell 

            double dblTransport3 = -1.1;
            double iDot = -1.49;
            int minConnector = 0;
            int maxConnector = 0;
            int maxCircleCount = 7;
            double y = 0;
            int lineCount = 1;
            int circleCount = 0;
            double yPosition = 10.5;
            int itransport = 0;
            double dblSquareSize = -.6;
            double dblSmallSquare = -1;
            double dblsize = -.75;
            double dblconnectDistance = 4;
            double dblxConnectDistance = 5;
            double dblcircle = -.5;
            string lineColor = "=RGB(0,0,0)";
            string lineWeight = "=1.25 pt";
            string charSize = "=9 pt.";
            string charStyle = "1";

            System.Diagnostics.Process[] alreadyRunningProcesses = null;
            byte[] _diagram;
            try
            {
                filePath = GetFilePath();
                wmfFilePath = filePath + "\\temp.wmf";
                imageFilePath = filePath + "\\temp.bmp";

                lstProcSheet = (from row in _pccsBll.DB.PROCESS_SHEET
                                where row.PART_NO == PccsModel.PartNo
                                    && row.ROUTE_NO == PccsModel.RouteNo
                                select row).ToList<PROCESS_SHEET>();
                if (!lstProcSheet.IsNotNullOrEmpty()) return;

                alreadyRunningProcesses = System.Diagnostics.Process.GetProcessesByName("VISIO");
                foreach (System.Diagnostics.Process process in alreadyRunningProcesses)
                {
                    process.Kill();
                    process.Close();
                }
                int no_of_loops = ((lstProcSheet.Count - 1) / 14) + 1;
                maxConnector = 13;
                _diagram = null;
                minConnector = 0;
                maxConnector = lstProcSheet.Count - 1;
                visApp = new Microsoft.Office.Interop.Visio.InvisibleApp();
                visApp.Documents.Add(@"");
                visioDocs = visApp.Documents;
                visioStencil = visioDocs.OpenEx("TQM Diagram Shapes.vss",
                (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenDocked);
                visioTransportationMaster = visioStencil.Masters.get_ItemU(@"Transportation"); //Arrow
                visioOperation = visioStencil.Masters.get_ItemU(@"Operation"); //Circle
                visioInspection = visioStencil.Masters.get_ItemU(@"Inspection/ measurement"); //Square
                visioSmall = visioStencil.Masters.get_ItemU(@"Inspection/ measurement"); //Square
                visioPage = visApp.ActivePage;
                double xAxis = 0;
                visioNextTransportationShape = null;
                lineCount = 1;
                circleCount = 0;
                yPosition = 10.5;
                for (int ictr = minConnector; ictr <= maxConnector; ictr++)
                {
                    double xPosition = ictr;

                    if (ictr == minConnector)
                    {
                        xPosition = 0;
                    }
                    itransport = lstProcSheet[ictr].TRANSPORT.ToValueAsString().ToIntValue();
                    if (lstProcSheet[ictr].OPN_CD == 2810 || lstProcSheet[ictr].OPN_CD == 2800 || lstProcSheet[ictr].OPN_CD == 2801)
                    {
                        visioOperationShape = visioPage.Drop(visioInspection, xPosition, yPosition);
                    }
                    else
                    {
                        visioOperationShape = visioPage.Drop(visioOperation, xPosition, yPosition);
                    }
                    visioOperationShape.Text = ((ictr + 1) * 10).ToString("D3"); // Jeyan - Format to 3 digit ex: 10->010, 20->020 like wise
                    visioOperationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblcircle, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                    visioOperationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblcircle, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                    visioOperationShape.Cells["LineColor"].FormulaForce = lineColor;
                    visioOperationShape.Cells["LineWeight"].FormulaForce = lineWeight;
                    celObj = visioOperationShape.Cells["Char.Size"];
                    celObj.Formula = charSize;
                    celObj = visioOperationShape.Cells["Char.Style"];
                    celObj.Formula = charStyle;
                    if (visioNextTransportationShape != null)
                    {
                        visioNextTransportationShape.Cells["LineColor"].FormulaForce = lineColor;
                        visioNextTransportationShape.Cells["LineWeight"].FormulaForce = lineWeight;
                        visioNextTransportationShape.CellsU["LineWeight"].FormulaForce = lineWeight;

                        if (lineCount % 2 != 0 && circleCount % maxCircleCount != 0)
                        {
                            visioNextTransportationShape.AutoConnect(visioOperationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirRight, vsoConnectorShape);
                            xAxis = visioOperationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                            visioOperationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblxConnectDistance);
                            //xAxis = visioNextTransportationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                            //visioNextTransportationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblxConnectDistance);
                        }
                        else if (lineCount % 2 == 0 && circleCount % maxCircleCount == 0)
                        {
                            visioNextTransportationShape.AutoConnect(visioOperationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                            y = visioOperationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                            visioOperationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);
                        }
                        else if (lineCount % 2 != 0 && circleCount % maxCircleCount == 0)
                        {
                            visioNextTransportationShape.AutoConnect(visioOperationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                            y = visioOperationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                            visioOperationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);
                        }
                        else
                        {
                            visioNextTransportationShape.AutoConnect(visioOperationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirLeft, vsoConnectorShape);
                            xAxis = visioOperationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                            visioOperationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis + dblxConnectDistance);
                            //visioNextTransportationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblconnectDistance);
                        }

                    }
                    circleCount++;
                    if (circleCount % maxCircleCount == 0) lineCount++;
                    if (ictr != maxConnector)
                    {
                        if (lineCount % 2 != 0 && circleCount % maxCircleCount != 0)
                        {
                            if (itransport == 0)
                            {
                                visioTransportationShape = visioPage.Drop(visioOperation, (xPosition * 2), yPosition);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                            }
                            else if (itransport == 1)
                            {
                                visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2), yPosition);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 2)
                            {
                                visioTransportationShape = visioPage.Drop(visioInspection, (xPosition * 2), yPosition);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 3)
                            {
                                visioTransportationShape1 = visioPage.Drop(visioTransportationMaster, (xPosition * 2), yPosition);
                                visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                visioOperationShape.AutoConnect(visioTransportationShape1, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirRight, vsoConnectorShape);
                                xAxis = visioTransportationShape1.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioTransportationShape1.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblxConnectDistance);

                                visioTransportationShape = visioPage.Drop(visioSmall, (xPosition * 2), yPosition);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                visioTransportationShape1.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirRight, vsoConnectorShape);
                                xAxis = visioTransportationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioTransportationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblxConnectDistance);
                            }
                            if (itransport == 0 || itransport == 1 || itransport == 2)
                            {
                                visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirRight, vsoConnectorShape);
                                xAxis = visioTransportationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioTransportationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblxConnectDistance);
                            }
                        }
                        else if (lineCount % 2 == 0 && circleCount % maxCircleCount == 0)
                        {
                            if (itransport == 0)
                            {
                                visioTransportationShape = visioPage.Drop(visioOperation, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 1)
                            {
                                visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 2)
                            {
                                visioTransportationShape = visioPage.Drop(visioInspection, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 3)
                            {

                                visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                visioTransportationShape1 = visioPage.Drop(visioSmall, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                visioTransportationShape.Rotate90();
                                visioTransportationShape.Rotate90();
                                visioTransportationShape.Rotate90();

                                visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                yPosition = yPosition - 1;
                                y = visioTransportationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance + 3);

                                visioTransportationShape.AutoConnect(visioTransportationShape1, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                yPosition = yPosition - 1;
                                y = visioTransportationShape1.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioTransportationShape1.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance + 3);

                                visioTransportationShape = visioTransportationShape1;



                                //visioTransportationShape1 = visioPage.Drop(visioTransportationMaster, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                //visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                //visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                //visioTransportationShape1.Rotate90();
                                //visioTransportationShape1.Rotate90();
                                //visioTransportationShape1.Rotate90();
                                //visioOperationShape.AutoConnect(visioTransportationShape1, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                //yPosition = yPosition - 1;
                                //y = visioTransportationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                //visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);


                                //visioTransportationShape = visioPage.Drop(visioSmall, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                //visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                //visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, -1, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                //visioTransportationShape.Rotate90();
                                //visioTransportationShape.Rotate90();
                                //visioTransportationShape.Rotate90();
                                //visioTransportationShape.AutoConnect(visioTransportationShape1, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                //yPosition = yPosition - 1;
                                //y = visioTransportationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                //visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);

                            }

                            if (itransport == 0 || itransport == 1 || itransport == 2)
                            {
                                visioTransportationShape.Rotate90();
                                visioTransportationShape.Rotate90();
                                visioTransportationShape.Rotate90();
                                visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                yPosition = yPosition - 1;
                                y = visioTransportationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);
                            }
                            //return;
                        }
                        else if (lineCount % 2 != 0 && circleCount % maxCircleCount == 0)
                        {
                            if (itransport == 0)
                            {
                                visioTransportationShape = visioPage.Drop(visioOperation, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 1)
                            {
                                visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 2)
                            {
                                visioTransportationShape = visioPage.Drop(visioInspection, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 3)
                            {
                                visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                visioTransportationShape1 = visioPage.Drop(visioSmall, (xPosition * 2) - 1, yPosition - lineCount + 1);
                                visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                visioTransportationShape.Rotate90();
                                visioTransportationShape.Rotate90();
                                visioTransportationShape.Rotate90();
                                visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                yPosition = yPosition - 1;
                                y = visioTransportationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);


                                visioTransportationShape1.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                yPosition = yPosition - 1;
                                y = visioTransportationShape1.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioTransportationShape1.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);
                                visioTransportationShape = visioTransportationShape1;
                            }

                            if (itransport == 0 || itransport == 1 || itransport == 2)
                            {
                                visioTransportationShape.Rotate90();
                                visioTransportationShape.Rotate90();
                                visioTransportationShape.Rotate90();
                                visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirDown, vsoConnectorShape);
                                yPosition = yPosition - 1;
                                y = visioTransportationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + dblconnectDistance);
                            }
                        }
                        else
                        {
                            if (itransport == 0)
                            {
                                visioTransportationShape = visioPage.Drop(visioOperation, (xPosition * -2), yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, iDot, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 1)
                            {
                                visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * -2), yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 2)
                            {
                                visioTransportationShape = visioPage.Drop(visioInspection, (xPosition * -2), yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblsize, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                            }
                            else if (itransport == 3)
                            {
                                visioTransportationShape = visioPage.Drop(visioTransportationMaster, (xPosition * -.5), yPosition - lineCount + 1);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);

                                visioTransportationShape.Rotate90();
                                visioTransportationShape.Rotate90();
                                visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirLeft, vsoConnectorShape);
                                xAxis = visioTransportationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioTransportationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis + dblxConnectDistance + 2);

                                visioTransportationShape1 = visioPage.Drop(visioSmall, (xPosition * -.5), yPosition - lineCount + 1);
                                visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirE, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape1.Resize(Microsoft.Office.Interop.Visio.VisResizeDirection.visResizeDirN, dblTransport3, Microsoft.Office.Interop.Visio.VisUnitCodes.visCentimeters);
                                visioTransportationShape.AutoConnect(visioTransportationShape1, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirLeft, vsoConnectorShape);
                                xAxis = visioTransportationShape1.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioTransportationShape1.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis + dblxConnectDistance + 2);

                                visioTransportationShape = visioTransportationShape1;
                            }

                            if (itransport == 0 || itransport == 1 || itransport == 2)
                            {
                                visioTransportationShape.Rotate90();
                                visioTransportationShape.Rotate90();
                                visioOperationShape.AutoConnect(visioTransportationShape, Microsoft.Office.Interop.Visio.VisAutoConnectDir.visAutoConnectDirLeft, vsoConnectorShape);
                                xAxis = visioTransportationShape.Cells["PinX"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                                visioTransportationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis + dblxConnectDistance);
                            }
                            //visioOperationShape.Cells["PinX"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, xAxis - dblconnectDistance);
                            //double y = visioOperationShape.Cells["PinY"].Result[Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters];
                            //visioTransportationShape.Cells["PinY"].set_Result(Microsoft.Office.Interop.Visio.VisUnitCodes.visMillimeters, y + 5);
                        }
                    }



                    visioNextTransportationShape = visioTransportationShape;
                }
                visioPage.Export(imageFilePath);
                visioPage.Export(wmfFilePath);
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(imageFilePath);
                setImage(fileInfo);
                System.IO.MemoryStream strm = new System.IO.MemoryStream();
                using (System.IO.FileStream file = new System.IO.FileStream(imageFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    _diagram = new byte[file.Length];
                    file.Read(_diagram, 0, (int)file.Length);
                    strm.Write(_diagram, 0, (int)file.Length);
                    file.Close();
                    file.Dispose();
                }

                ShowDiagramVisible = Visibility.Collapsed;
                HideDiagramVisible = Visibility.Visible;
                GenerateDiagramVisible = Visibility.Visible;
                DeleteDiagramVisible = Visibility.Visible;
                _imageChange = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            alreadyRunningProcesses = System.Diagnostics.Process.GetProcessesByName("VISIO");
            foreach (System.Diagnostics.Process process in alreadyRunningProcesses)
            {
                process.Kill();
                process.Close();
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

        private LogModel _log;
        public LogModel logmodel
        {
            get { return _log; }
            set
            {
                this._log = value;
                NotifyPropertyChanged("logmodel");
            }
        }

        public void dgrdPccsIssue_Sorting(object sender, DataGridSortingEventArgs e)
        {
            e.Handled = true;

            if (e.Column.Header.ToString() == "Date")
            {
                return;
            }
            if (e.Column.Header.ToString() == "Alterations")
            {
                return;
            }
            if (e.Column.Header.ToString() == "Intl")
            {
                return;
            }

            DataTable dt = PccsModel.PccsRevisionDetails.ToTable().Copy();

            if (Sort == "" || Sort == "asc")
            {
                Sort = "desc";
                //dt.Rows[dt.Rows.Count - 1]["ISSUE_NONUMERIC"] = "-999";
                dt.DefaultView.Sort = "SnoSort DESC";
            }
            else
            {
                //dt.Rows[dt.Rows.Count - 1]["ISSUE_NONUMERIC"] = "999";
                Sort = "asc";
                dt.DefaultView.Sort = "SnoSort ASC";
            }

            DataTable dtFinal = dt.DefaultView.ToTable();
            PccsModel.PccsRevisionDetails = (dtFinal != null) ? dtFinal.DefaultView : null;
            PccsModel.PccsRevisionDetails.Sort = "";
        }
    }
}
