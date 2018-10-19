using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using ProcessDesigner.UserControls;
using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using AxACCTRLLib;
using System.Collections.Generic;
using WPF.MDI;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Windows.Controls;

namespace ProcessDesigner.ViewModel
{
    public class DrawingViewModel : ViewModelBase
    {
        public System.Windows.Controls.DataGrid DgvProdDwgMast;
        private WindowsFormsHost _drawingHost;
        public AxAcCtrl DrawingControl;

        private WPF.MDI.MdiChild mdiChild;
        System.Windows.Window self = null;

        public DataRowView SelectedItem { get; set; }

        private readonly ICommand dimensionCommand;
        public ICommand OnDimensionCommand { get { return this.dimensionCommand; } }

        private readonly ICommand saveCommand;
        public ICommand OnSaveCommand { get { return this.saveCommand; } }

        private readonly ICommand searchCommand;
        public ICommand OnSearchCommand { get { return this.searchCommand; } }

        private readonly ICommand copyCommand;
        public ICommand OnCopyCommand { get { return this.copyCommand; } }

        private readonly ICommand selectChangeComboPartCommand;
        public ICommand OnselectChangeComboPartCommand { get { return this.selectChangeComboPartCommand; } }


        private readonly ICommand selectChangeComboDrawingCommand;
        public ICommand OnselectChangeComboDrawingCommand { get { return this.selectChangeComboDrawingCommand; } }

        private readonly ICommand onCloseCommand;
        public ICommand OnCloseCommand { get { return this.onCloseCommand; } }


        private readonly ICommand insertImageCommand;
        public ICommand OnInsertImageCommand { get { return this.insertImageCommand; } }

        private readonly ICommand editImageCommand;
        public ICommand OnEditImageCommand { get { return this.editImageCommand; } }

        private readonly ICommand deleteImageCommand;
        public ICommand OnDeleteImageCommand { get { return this.deleteImageCommand; } }

        private readonly ICommand previousCommand;
        public ICommand OnPreviousCommand { get { return this.previousCommand; } }

        private readonly ICommand nextCommand;
        public ICommand OnNextCommand { get { return this.nextCommand; } }

        private readonly ICommand rowEditIssueRevisionsCommand;
        public ICommand RowEditIssueRevisionsCommand { get { return this.rowEditIssueRevisionsCommand; } }

        private readonly ICommand deleteIssueRevisionsCommand;
        public ICommand OnDeleteIssueRevisionsCommand { get { return this.deleteIssueRevisionsCommand; } }

        private readonly ICommand addCommand;
        public ICommand OnAddCommand { get { return this.addCommand; } }

        private readonly ICommand editCommand;
        public ICommand OnEditCommand { get { return this.editCommand; } }
        private readonly ICommand _enterPartNumber;
        public ICommand EnterPartNumberCmb { get { return this._enterPartNumber; } }
        private readonly ICommand showECNCommand;
        public ICommand ShowECNCommand { get { return this.showECNCommand; } }

        private readonly ICommand showPCNCommand;
        public ICommand ShowPCNCommand { get { return this.showPCNCommand; } }

        public string Sort { get; set; }

        private UserInformation _userinformation;
        private DrawingBll drwBll;
        public System.Action CloseAction { get; set; }
        private RawMaterial locBll = null;
        public DrawingViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, int entityPrimaryKey, OperationMode operationMode, System.Windows.Window self = null)
        {
            try
            {
                _userinformation = userInformation;
                DrwModel = new DrawingModel1();
                //MandatoryFields = new DrawingModel1();
                drwBll = new DrawingBll(_userinformation);
                this.mdiChild = mdiChild;
                this.self = self;
                this.dimensionCommand = new DelegateCommand(this.Dimension);
                this.saveCommand = new DelegateCommand(this.SaveCommand);
                this.searchCommand = new DelegateCommand(this.SearchCommand);
                this.copyCommand = new DelegateCommand(this.CopyCommand);
                this.selectChangeComboPartCommand = new DelegateCommand(this.SelectPartDataRow);
                this.selectChangeComboDrawingCommand = new DelegateCommand(this.SelectDrawingDataRow);
                this.onCloseCommand = new DelegateCommand(this.CloseCommand);
                this.insertImageCommand = new DelegateCommand(this.InsertImageCommand);
                this.editImageCommand = new DelegateCommand(this.EditImageCommand);
                this.deleteImageCommand = new DelegateCommand(this.DeleteImageCommand);
                this.previousCommand = new DelegateCommand(this.PreviousCommand);
                this.nextCommand = new DelegateCommand(this.NextCommand);
                this.rowEditIssueRevisionsCommand = new DelegateCommand<Object>(this.RowEditIssueRevisions);
                this.deleteIssueRevisionsCommand = new DelegateCommand<Object>(this.DeleteIssueRevisions);
                this._enterPartNumber = new DelegateCommand<string>(this.EnterPartNumber);
                this.costSheetSearchClickCommand = new DelegateCommand(this.CostSheetSearchClick);
                this.productSearchClickCommand = new DelegateCommand(this.ProductSearchClick);
                this.toolSearchClickCommand = new DelegateCommand(this.ToolsSearchClick);

                this.addCommand = new DelegateCommand(this.Add);
                this.editCommand = new DelegateCommand(this.Edit);

                this.showECNCommand = new DelegateCommand(this.ShowECN);
                this.showPCNCommand = new DelegateCommand(this.ShowPCN);

                DV_PART_MAST = drwBll.GetPartNumberDetails();
                DV_DWG_TYPE_MAST = drwBll.GetDrawingTypeDetails();
                locBll = new RawMaterial(_userinformation);
                LocationMaster = locBll.GetLocations().ToDataTable<DDLOC_MAST>().DefaultView;

                PartColumns = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "PART NUMBER", ColumnWidth = 115 },
                            new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "PART DESC", ColumnWidth = "1*" }
                        };

                DwgColumns = new ObservableCollection<DropdownColumns>()
                        {                            
                            new DropdownColumns() { ColumnName = "DWG_TYPE_DESC", ColumnDesc = "DRAWING TYPE DESC", ColumnWidth = "1*" }
                        };
                DropDownItemsLocation = new ObservableCollection<DropdownColumns>()
                {
                            new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Location Code", ColumnWidth = 110 },
                            new DropdownColumns() { ColumnName = "LOCATION", ColumnDesc = "Location Description", ColumnWidth = "1*" }
                };
                drwBll.GetDrawingModel(DrwModel);
                LoadComboECN();
                Edit();

            }
            catch (Exception exp)
            {
                exp.LogException();
            }
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
        private ObservableCollection<DropdownColumns> _dropDownItemsLocation;
        public ObservableCollection<DropdownColumns> DropDownItemsLocation
        {
            get
            {
                return _dropDownItemsLocation;
            }
            set
            {
                _dropDownItemsLocation = value;
                OnPropertyChanged("DropDownItemsLocation");
            }
        }
        private readonly ICommand costSheetSearchClickCommand;
        public ICommand CostSheetSearchClickCommand { get { return this.costSheetSearchClickCommand; } }
        private void CostSheetSearchClick()
        {
            showDummy();
            MdiChild mdiCostSheetSearch = new MdiChild();
            if (MainMDI.IsFormAlreadyOpen("Cost Sheet Search") == false)
            {
                frmCostSheetSearch frmCostSheetSearch = new frmCostSheetSearch(_userinformation, mdiCostSheetSearch, 1, OperationMode.View, "frmFRCS");
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
            //if (self.IsNotNullOrEmpty())
            //    frmCostSheetSearch.Owner = self;

            //bool? dialogResult = frmCostSheetSearch.ShowDialog();
            //if (dialogResult.ToBooleanAsString())
            //{
            //    //ActionMode = OperationMode.Edit;
            //}

        }
        //private DrawingModel1 _mandatoryFields = null;
        //public DrawingModel1 MandatoryFields
        //{
        //    get
        //    {
        //        return _mandatoryFields;
        //    }
        //    set
        //    {
        //        _mandatoryFields = value;
        //        NotifyPropertyChanged("MandatoryFields");
        //    }
        //}
        private DataView _locationMaster;
        public DataView LocationMaster
        {
            get
            {
                return _locationMaster;
            }
            set
            {
                _locationMaster = value;
                NotifyPropertyChanged("LocationMaster");
            }
        }
        private readonly ICommand productSearchClickCommand;
        public ICommand ProductSearchClickCommand { get { return this.productSearchClickCommand; } }
        private void ProductSearchClick()
        {
            //frmProductSearch frmProductSearch = new frmProductSearch(_userinformation);
            //frmProductWeight.varWeightOption = "F"; //HardCoded value F
            //frmProductWeight.varCIreference = ActiveEntity.CI_REFERENCE;
            //frmProductSearch.ShowDialog();
            try
            {
                showDummy();
                MdiChild mdiProductSearch = new MdiChild();
                ProcessDesigner.frmProductSearch productSearch = new frmProductSearch(_userinformation, mdiProductSearch);
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
            //break;

        }
        private void EnterPartNumber(string partNumber)
        {
            try
            {
                SelectPartDataRow();
            }
            catch (Exception)
            {

            }

        }
        private readonly ICommand toolSearchClickCommand = null;
        public ICommand ToolSearchClickCommand { get { return this.toolSearchClickCommand; } }
        private void ToolsSearchClick()
        {
            try
            {
                showDummy();
                MdiChild mdiToolsInfo = new MdiChild();
                if (MainMDI.IsFormAlreadyOpen("Tools Information") == false)
                {

                    ProcessDesigner.frmToolsInfo toolsinfo = new ProcessDesigner.frmToolsInfo(_userinformation, mdiToolsInfo);
                    mdiToolsInfo.Title = ApplicationTitle + " - Tools Information";
                    mdiToolsInfo.Content = toolsinfo;
                    mdiToolsInfo.Height = toolsinfo.Height + 23;
                    mdiToolsInfo.Width = toolsinfo.Width + 20;
                    mdiToolsInfo.MinimizeBox = false;
                    mdiToolsInfo.MaximizeBox = false;
                    mdiToolsInfo.Resizable = false;
                    MainMDI.Container.Children.Add(mdiToolsInfo);
                }
                else
                {
                    mdiToolsInfo = (MdiChild)MainMDI.GetFormAlreadyOpened("Tools Information");
                    //toolschedule = (frmToolSchedule_new)mdiCostSheetSearch.Content;
                    MainMDI.SetMDI(mdiToolsInfo);
                }
                //frmCopyStatus copyStatus = new frmCopyStatus("ProcessSheet", ProcessSheet.PART_NO.Trim(), ProcessSheet.ROUTE_NO.ToString(), "", "", "");
                //copyStatus.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private DD_PCN _ddPcnEcnModel;
        public DD_PCN DDPCNECNModel
        {
            get { return _ddPcnEcnModel; }
            set
            {
                _ddPcnEcnModel = value;
                NotifyPropertyChanged("DDPCNECNModel");
            }

        }

        private DD_PCN _ddPcnMpsModel;
        public DD_PCN DDPCNMPSModel
        {
            get { return _ddPcnMpsModel; }
            set
            {
                _ddPcnMpsModel = value;
                NotifyPropertyChanged("DDPCNMPSModel");
            }

        }

        private DataView _actualChangeImplementCombo;
        public DataView ActualChangeImplementCombo
        {
            get { return _actualChangeImplementCombo; }
            set
            {
                _actualChangeImplementCombo = value;
                NotifyPropertyChanged("ActualChangeImplementCombo");
            }
        }

        private DataView _proposedImplementCombo;
        public DataView ProposedImplementCombo
        {
            get { return _proposedImplementCombo; }
            set
            {
                _proposedImplementCombo = value;
                NotifyPropertyChanged("ProposedImplementCombo");
            }
        }

        private DataView _compiledByCombo;
        public DataView CompiledByCombo
        {
            get { return _compiledByCombo; }
            set
            {
                _compiledByCombo = value;
                NotifyPropertyChanged("CompiledByCombo");
            }
        }



        private DataView _approvedByCombo;
        public DataView ApprovedByCombo
        {
            get { return _approvedByCombo; }
            set
            {
                _approvedByCombo = value;
                NotifyPropertyChanged("ApprovedByCombo");
            }
        }


        private bool _re_PPAP = false;

        public bool RE_PPAP
        {
            get { return _re_PPAP; }
            set
            {
                _re_PPAP = value;
                _nOT_RE_PPAP = !_re_PPAP;
                NotifyPropertyChanged("RE_PPAP");
                NotifyPropertyChanged("NOT_RE_PPAP");
            }
        }

        private bool _nOT_RE_PPAP = false;

        public bool NOT_RE_PPAP
        {
            get { return _nOT_RE_PPAP; }
            set
            {
                _nOT_RE_PPAP = value;
                _re_PPAP = !_nOT_RE_PPAP;
                NotifyPropertyChanged("RE_PPAP");
                NotifyPropertyChanged("NOT_RE_PPAP");
            }
        }

        private bool _re_PPAP_MPS = false;

        public bool RE_PPAP_MPS
        {
            get { return _re_PPAP_MPS; }
            set
            {
                _re_PPAP_MPS = value;
                _nOT_RE_PPAP_MPS = !_re_PPAP_MPS;
                NotifyPropertyChanged("RE_PPAP_MPS");
                NotifyPropertyChanged("NOT_RE_PPAP_MPS");
            }
        }

        private bool _nOT_RE_PPAP_MPS = false;

        public bool NOT_RE_PPAP_MPS
        {
            get { return _nOT_RE_PPAP_MPS; }
            set
            {
                _nOT_RE_PPAP_MPS = value;
                _re_PPAP_MPS = !_nOT_RE_PPAP_MPS;
                NotifyPropertyChanged("RE_PPAP_MPS");
                NotifyPropertyChanged("NOT_RE_PPAP_MPS");
            }
        }


        private string _eCNReferenceNo;
        public string ECNReferenceNo
        {
            get { return _eCNReferenceNo; }
            set
            {
                _eCNReferenceNo = value;
                NotifyPropertyChanged("ECNReferenceNo");
            }
        }

        private string _mPSReferenceNo;
        public string MPSReferenceNo
        {
            get { return _mPSReferenceNo; }
            set
            {
                _mPSReferenceNo = value;
                NotifyPropertyChanged("MPSReferenceNo");
            }
        }

        private Visibility _hasDropDownVisibility;
        public Visibility HasDropDownVisibility
        {
            get { return _hasDropDownVisibility; }
            set
            {
                _hasDropDownVisibility = value;
                NotifyPropertyChanged("HasDropDownVisibility");
            }
        }

        private bool _addEnable;
        public bool AddEnable
        {
            get { return _addEnable; }
            set
            {
                _addEnable = value;
                NotifyPropertyChanged("AddEnable");
            }
        }

        private bool _editEnable;
        public bool EditEnable
        {
            get { return _editEnable; }
            set
            {
                _editEnable = value;
                NotifyPropertyChanged("EditEnable");
            }
        }

        private bool _saveEnable;
        public bool SaveEnable
        {
            get { return _saveEnable; }
            set
            {
                _saveEnable = value;
                NotifyPropertyChanged("SaveEnable");
            }
        }

        private OperationMode _actionMode = OperationMode.None;
        public OperationMode ActionMode
        {
            get { return _actionMode; }
            set
            {
                _actionMode = value;
                //SetProperty(ref _actionMode, value);
                NotifyPropertyChanged("ActionMode");
            }
        }

        private bool _readOnly;
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
                NotifyPropertyChanged("ReadOnly");
            }
        }

        private void GetDrawingDetails()
        {
            try
            {
                string getPartNumber = DrwModel.PART_NO;
                string getDrwType;
                if (DrwModel.DWG_TYPE_DESC == "Sequence Drawing")
                {
                    getDrwType = "1";
                }
                else if (DrwModel.DWG_TYPE_DESC == "Product Drawing")
                {
                    getDrwType = "0";
                }
                else
                {
                    getDrwType = "-1";
                }
                //                (DrwModel.DWG_TYPE_DESC == "Sequence Drawing") ? "1" : "0";
                ////DV_PROD_DWG_ISSUE = drwBll.GetProductDrawingDetails(getPartNumber, getDrwType);
                ////if (DV_PROD_DWG_ISSUE != null && DV_PROD_DWG_ISSUE.Count > 0)
                ////{
                ////    TOTAL_PAGE_NO = DV_PROD_DWG_ISSUE.Count;
                ////    PAGE_NO = Convert.ToInt32(DV_PROD_DWG_ISSUE.ToTable().Rows[0]["PAGE_NO"].ToString());
                ////    DrwModel.PAGE_NO = PAGE_NO;
                ////    string partnumber = DV_PROD_DWG_ISSUE.ToTable().Rows[0]["PART_NO"].ToString();
                ////    decimal getdrwtype = Convert.ToDecimal(DV_PROD_DWG_ISSUE.ToTable().Rows[0]["DWG_TYPE"]);
                ////    string filepath = DV_PROD_DWG_ISSUE.ToTable().Rows[0]["PRD_DWG"].ToString();
                ////    if (filepath != null)
                ////    {
                ////        DrwModel.FilePath = filepath;
                ////        DrawingControl.Src = filepath;
                ////    }
                ////}
                ////else
                ////{
                ////    PAGE_NO = 0;
                ////    TOTAL_PAGE_NO = 0;
                ////    DrawingControl.Src = null;
                ////}
                DV_PROD_DWG_ISSUE = drwBll.GetRevisionDetails(getPartNumber, getDrwType);

                DataTable dt = new DataTable();

                dt = DV_PROD_DWG_ISSUE.Table.Copy();

                dt.Columns.Add("ISSUE_NONUMERIC", typeof(System.Int32));
                foreach (DataRow dr in dt.Rows)
                {
                    int output;
                    if (int.TryParse(dr["ISSUE_NO"].ToValueAsString(), out output) == true)
                    {
                        dr["ISSUE_NONUMERIC"] = dr["ISSUE_NO"];
                    }
                    else
                    {
                        dr["ISSUE_NONUMERIC"] = 0;
                    }
                }

                dt.Rows[dt.Rows.Count - 1]["ISSUE_NONUMERIC"] = 999;
                dt.DefaultView.Sort = "ISSUE_NONUMERIC";
                DataTable dtFinal = dt.DefaultView.ToTable();
                DV_PROD_DWG_ISSUE = (dtFinal != null) ? dtFinal.DefaultView : null;
                DV_PROD_DWG_ISSUE.Sort = "";
                if (DV_PROD_DWG_ISSUE.Count == 0)
                {
                    DV_PROD_DWG_ISSUE.AddNew();
                }
                oldDV_PROD_DWG_ISSUE = DV_PROD_DWG_ISSUE.ToTable().Copy().DefaultView;
            }
            catch (Exception exp)
            {
                throw exp.LogException();
            }
        }

        private void GetDrawing(int page_no)
        {
            try
            {
                string getPartNumber = DrwModel.PART_NO;
                string getDrwType = (DrwModel.DWG_TYPE_DESC == "Sequence Drawing") ? "1" : "0";
                DataView dv_prod_dwg = drwBll.GetProductDrawing(getPartNumber, getDrwType, page_no);
                if (dv_prod_dwg != null && dv_prod_dwg.Count > 0)
                {
                    PAGE_NO = Convert.ToInt32(dv_prod_dwg.ToTable().Rows[0]["PAGE_NO"].ToString());
                    string partnumber = dv_prod_dwg.ToTable().Rows[0]["PART_NO"].ToString();
                    decimal getdrwtype = Convert.ToDecimal(dv_prod_dwg.ToTable().Rows[0]["DWG_TYPE"]);
                    string filepath = dv_prod_dwg.ToTable().Rows[0]["PRD_DWG"].ToString();
                    if (filepath != null)
                    {
                        DrwModel.FilePath = filepath;
                        if (System.IO.File.Exists(filepath))
                        {
                            DrawingControl.Src = filepath;
                        }
                        else
                        {
                            DrawingControl.Src = null;
                        }
                    }
                }
                else
                {
                    PAGE_NO = 0;
                    PhotoSource = null;
                    TOTAL_PAGE_NO = 0;
                }
            }
            catch (Exception exp)
            {
                throw exp.LogException();
            }
        }

        private void Dimension()
        {

        }

        private void InsertImageCommand()
        {
            DrwModel.Mode = "Add";
            ImageCommand();
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

        private void EditImageCommand()
        {
            DrwModel.Mode = "Edit";
            ImageCommand();
        }

        private void DeleteImageCommand()
        {
            if (string.IsNullOrEmpty(DrwModel.PART_NO) && (string.IsNullOrEmpty(DrwModel.PRD_DWG)))
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part number and Drawing Type"));
                //MessageBox.Show("Please select Part number and Drawing Type.", "SmartPD");
            }
            else
            {
                string getPartNumber = DrwModel.PART_NO;
                string getDwgType = DrwModel.DWG_TYPE_DESC;
                decimal getPageNumber = DrwModel.PAGE_NO;
                string getDrwType = (DrwModel.DWG_TYPE_DESC == "Sequence Drawing") ? "1" : "0";
                bool result = false;
                result = drwBll.DeleteDrawingDetails(DrwModel);
                if (result)
                {
                    MessageBox.Show(DrwModel.Status, "SmartPD");
                    GetDrawingDetails();
                    string networkPath = ConfigurationManager.AppSettings["NetworkPath"];
                    string newFileName = @"" + networkPath + DrwModel.PART_NO + "_" + getDrwType + "_" + DrwModel.PAGE_NO + ".DWG";
                    if (System.IO.File.Exists(newFileName))
                    {
                        System.IO.File.Delete(newFileName);
                    }
                }
            }
        }

        public void CloseCommand()
        {
            try
            {
                if (DrwModel.PART_NO.IsNotNullOrEmpty() && (IsRecordsUpdated || IsChangesMade()))
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        SaveCommand();
                        if (!IsRecordsUpdated) return;
                    }
                }
                
                if (ShowInformationMessageInput(PDMsg.CloseForm) == MessageBoxResult.Yes)
                {
                    CloseAction();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool IsChangesMade()
        {
            try
            {
                PartNumberIsFocused = true;
                bool result = true;

                if (oldDV_PROD_DWG_ISSUE != null)
                {

                    DataView newDV_PROD_DWG_ISSUE = DV_PROD_DWG_ISSUE.Table.Copy().DefaultView;

                    result = newDV_PROD_DWG_ISSUE.IsEqual(oldDV_PROD_DWG_ISSUE);
                }

                return !result;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public Boolean IsRecordsUpdated = false;
        private void SaveCommand()
        {
            try
            {
                PartNumberIsFocused = true;
                IsRecordsUpdated = false;
                if (DrwModel.PART_NO.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage("Part No. Should not be Empty!");
                    return;
                }

                if (DrwModel.DWG_TYPE_DESC.ToValueAsString().Trim() == "")
                {
                    ShowInformationMessage("Drawing Type Should not be Empty!");
                    return;
                }

                if (ValidateDataGrid() == false)
                {
                    return;
                }

                if (DrwModel.PART_NO.ToValueAsString() != "")
                {
                    if (DrwModel.ISSUE_NO.ToString() != "")
                    {
                        //var duplicates = (DrwModel.SelectedItem).DataView.ToTable().AsEnumerable().GroupBy(i => new { ISSUE_NO = i.Field<decimal>("ISSUE_NO") }).Where(g => g.Count() > 1).Select(g => new { g.Key.ISSUE_NO }).ToList();
                        DataView dvCopy = DV_PROD_DWG_ISSUE.ToTable().Copy().DefaultView;
                        dvCopy.RowFilter = "ISSUE_NO is not null";
                        var duplicates = dvCopy.ToTable().AsEnumerable().GroupBy(i => new { ISSUE_NO = i.Field<decimal>("ISSUE_NO") }).Where(g => g.Count() > 1).Select(g => new { g.Key.ISSUE_NO }).ToList();
                        if (duplicates.Count > 0)
                        {
                            ShowInformationMessage("Duplicate Issue No has been Entered");
                            return;
                        }
                        else
                        {
                            bool result = false;
                            result = drwBll.InsertDrawingRevisionDetails(DrwModel, DV_PROD_DWG_ISSUE);
                            IsRecordsUpdated = result;
                            oldDV_PROD_DWG_ISSUE = DV_PROD_DWG_ISSUE.ToTable().Copy().DefaultView;
                            if (DrwModel.Status != "")
                            {
                                PartNumberIsFocused = true;
                                NotifyPropertyChanged("DrwModel");
                                ShowInformationMessage(DrwModel.Status);
                                PartNumberIsFocused = true;
                            }
                        }
                    }
                    //ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                    //ClearAll();
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void RowEditIssueRevisions(Object selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    DrwModel.SelectedItem = (DataRowView)selecteditem;

                    if (DrwModel.SelectedItem["ISSUE_NO"].ToValueAsString().Trim() == "" &&
                        DrwModel.SelectedItem["ISSUE_DATE"].ToValueAsString().Trim() == "" &&
                        DrwModel.SelectedItem["ISSUE_ALTER"].ToValueAsString().Trim() == "" &&
                        DrwModel.SelectedItem["COMPILED_BY"].ToValueAsString().Trim() == "")
                    {
                        return;
                    }

                    if (DrwModel.SelectedItem["ISSUE_NO"].ToValueAsString().Trim() == "")
                    {
                        //ShowInformationMessage(PDMsg.NotEmpty("Revision Issue Number"));
                        //MessageBox.Show("Revision Issue Number should not be empty.", "SmartPD");
                        return;
                    }
                    if (DrwModel.SelectedItem["ISSUE_DATE"].ToValueAsString().Trim() == "")
                    {
                        //ShowInformationMessage(PDMsg.NotEmpty("Revision Issue Date"));
                        //MessageBox.Show("Revision Issue Date should not be empty.", "SmartPD");
                        //return;
                    }
                    if (DrwModel.SelectedItem["ISSUE_ALTER"].ToValueAsString().Trim() == "")
                    {
                        //ShowInformationMessage(PDMsg.NotEmpty("Revision Issue Alteration"));
                        //MessageBox.Show("Revision Issue Alteration should not be empty.", "SmartPD");
                        //return;
                    }
                    //if (DrwModel.SelectedItem["TYPE"].ToString() == "")
                    //{
                    //    MessageBox.Show("Product Type should not be empty.", "SmartPD");
                    //    return;
                    //}

                    //if (DrwModel.Mode == "Add")
                    //{
                    //DrwModel.DVType.AddNew();
                    if (DV_PROD_DWG_ISSUE.ToTable().Rows.Count > 0)
                    {
                        DataRow drRow = DV_PROD_DWG_ISSUE.ToTable().Rows[DV_PROD_DWG_ISSUE.ToTable().Rows.Count - 1];
                        if (drRow["ISSUE_NO"].ToValueAsString().Trim() != "" ||
                        drRow["ISSUE_DATE"].ToValueAsString().Trim() != "" ||
                        drRow["ISSUE_ALTER"].ToValueAsString().Trim() != "" ||
                        drRow["COMPILED_BY"].ToValueAsString().Trim() != "")
                        {
                            DV_PROD_DWG_ISSUE.AddNew();
                            drRow["ISSUE_NONUMERIC"] = 999;
                        }
                    }

                    //}
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool ValidateDataGrid()
        {
            try
            {
                foreach (DataRow drRow in DV_PROD_DWG_ISSUE.ToTable().Rows)
                {
                    if (drRow["ISSUE_NO"].ToValueAsString().Trim() == "" &&
                    drRow["ISSUE_DATE"].ToValueAsString().Trim() == "" &&
                    drRow["ISSUE_ALTER"].ToValueAsString().Trim() == "" &&
                    drRow["COMPILED_BY"].ToValueAsString().Trim() == "")
                    {

                    }
                    else
                    {
                        if (drRow["ISSUE_NO"].ToValueAsString().Trim() == "")
                        {
                            ShowInformationMessage(PDMsg.NotEmpty("Revision Issue Number"));
                            return false;
                        }
                        if (drRow["ISSUE_DATE"].ToValueAsString().Trim() == "")
                        {
                            ShowInformationMessage(PDMsg.NotEmpty("Revision Issue Date"));
                            return false;
                        }
                        if (drRow["ISSUE_ALTER"].ToValueAsString().Trim() == "")
                        {
                            ShowInformationMessage(PDMsg.NotEmpty("Revision Issue Alteration"));
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void DeleteIssueRevisions(Object selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    List<String> lstSelect = new List<String>();
                    bool result = false;
                    if (DV_PROD_DWG_ISSUE.Table.Columns.IndexOf("ID_NUMBER") < 0)
                        DV_PROD_DWG_ISSUE.Table.Columns.Add("ID_NUMBER");
                    for (int ictr = 0; ictr < DV_PROD_DWG_ISSUE.Count; ictr++)
                    {
                        DV_PROD_DWG_ISSUE[ictr]["ID_NUMBER"] = ictr;
                    }
                    foreach (DataRowView drView in DgvProdDwgMast.SelectedItems)
                    {
                        lstSelect.Add(drView["ID_NUMBER"].ToValueAsString());
                    }


                    //MessageBoxResult msgResult = ShowWarningMessage(PDMsg.BeforeDelete("Issue No " + DrwModel.SelectedItem["ISSUE_NO"].ToString()), MessageBoxButton.YesNo);
                    MessageBoxResult msgResult = ShowWarningMessage(lstSelect.Count + " record(s) will be deleted." + PDMsg.BeforeDelete("selected record(s)"), MessageBoxButton.YesNo);
                    //MessageBox.Show("Do you want to delete Issue No " + DrwModel.SelectedItem["ISSUE_NO"].ToString(), "SmartPD", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (msgResult == MessageBoxResult.Yes)
                    {

                        for (int ictr = 0; ictr < lstSelect.Count; ictr++)
                        {
                            DataRowView delView = null;
                            foreach (DataRowView dvView in DV_PROD_DWG_ISSUE)
                            {
                                if (lstSelect[ictr] == dvView["ID_NUMBER"])
                                {
                                    delView = dvView;
                                    break;
                                }
                            }
                            if (delView != null)
                            {
                                DrwModel.SelectedItem = delView;
                                if (DrwModel.SelectedItem["ISSUE_NO"].ToValueAsString().Trim() == "" &&
                                    DrwModel.SelectedItem["ISSUE_DATE"].ToValueAsString().Trim() == "" &&
                                    DrwModel.SelectedItem["ISSUE_ALTER"].ToValueAsString().Trim() == "" &&
                                    DrwModel.SelectedItem["COMPILED_BY"].ToValueAsString().Trim() == "")
                                {

                                }
                                else
                                {
                                    result = drwBll.DeleteIssueDetails(DrwModel);
                                    if (result == true)
                                    {
                                        //ShowInformationMessage(PDMsg.DeletedSuccessfully);
                                        //MessageBox.Show("Record Deleted successfully.", "SmartPD");
                                    }
                                    DrwModel.SelectedItem.Delete();
                                    if (DV_PROD_DWG_ISSUE.Count == 0)
                                    {
                                        DV_PROD_DWG_ISSUE.AddNew();
                                    }
                                }
                            }
                        }
                        if (DV_PROD_DWG_ISSUE.Table.Columns.IndexOf("ID_NUMBER") >= 0)
                            DV_PROD_DWG_ISSUE.Table.Columns.Remove("ID_NUMBER");

                        ShowInformationMessage(lstSelect.Count + " " + PDMsg.DeletedSuccessfully);
                    }
                }
                if (DV_PROD_DWG_ISSUE.Table.Columns.IndexOf("ID_NUMBER") >= 0)
                    DV_PROD_DWG_ISSUE.Table.Columns.Remove("ID_NUMBER");

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void ImageCommand()
        {
            try
            {
                if (string.IsNullOrEmpty(DrwModel.PART_NO) && (string.IsNullOrEmpty(DrwModel.PRD_DWG)))
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Part number and Drawing Type"));
                    //MessageBox.Show("Please select Part number and Drawing Type.", "SmartPD");
                }
                else
                {
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    dlg.DefaultExt = ".dwg";
                    dlg.Filter = "Image Files (*.dwg)|*.dwg;";
                    dlg.ShowDialog();
                    if (dlg.FileName != "")
                    {
                        string getfilename = dlg.SafeFileName;
                        DrwModel.FilePath = dlg.FileName;
                        if (DrwModel.FilePath.ToString().Trim() != "")
                        {
                            var strm = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(DrwModel.FilePath));
                            if (strm.IsNotNullOrEmpty())
                            {
                                string getDrwType = (DrwModel.DWG_TYPE_DESC == "Sequence Drawing") ? "1" : "0";
                                string networkPath = ConfigurationManager.AppSettings["NetworkPath"];
                                int _pagecount = 0;
                                if (DrwModel.Mode == "Add")
                                    _pagecount = TOTAL_PAGE_NO;
                                else
                                    _pagecount = PAGE_NO;
                                string newFileName = @"" + networkPath + DrwModel.PART_NO + "_" + getDrwType + "_" + _pagecount + ".DWG";
                                if (System.IO.File.Exists(newFileName))
                                {
                                    System.IO.File.Delete(newFileName);
                                }
                                if (!System.IO.File.Exists(newFileName))
                                {
                                    System.IO.File.Copy(DrwModel.FilePath, newFileName, true);
                                    DrwModel.FilePath = newFileName;
                                }
                                this.ShoworHidePhotoText = "Hide Photo F8";
                                this.PhotoToolTip = "Hide Photo";
                                this.ShowOrHidePhotoVisibility = Visibility.Visible;
                            }
                        }
                        bool result = drwBll.InsertDrawingDetails(DrwModel);
                        if (result)
                        {
                            MessageBox.Show(DrwModel.Status, "SmartPD");
                            GetDrawingDetails();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void SearchCommand()
        {

        }

        private void CopyCommand()
        {
            //  frmCopyStatus obj = new frmCopyStatus();
            // obj.ShowDialog();
            if (DrwModel.PART_NO.ToValueAsString().Trim() == "")
            {
                ShowInformationMessage("Part No. should be selected!");
                return;
            }
            frmCopyStatus copyStatus = new frmCopyStatus("Drawings", DrwModel.PART_NO.ToString(), "", "", "", "");
            copyStatus.ShowDialog();
        }

        DataView oldDV_PROD_DWG_ISSUE = null;
        private void SelectPartDataRow()
        {
            try
            {
                //DrwModel.DWG_TYPE_DESC = (SelectedRow != null) ? SelectedRow["DWG_TYPE_DESC"].ToString() : string.Empty;
                DrwModel.DWG_TYPE_DESC = "";
                NotifyPropertyChanged("DrwModel");
                PART_DESCRIPTION = (SelectedRow != null) ? SelectedRow["PART_DESC"].ToString() : string.Empty;
                DrwModel.DWG_TYPE_DESC = "Product Drawing";
                GetDrawingDetails();
                mdiChild.Title = ApplicationTitle + " - Drawings" + ((DrwModel.PART_NO.IsNotNullOrEmpty()) ? " - " + DrwModel.PART_NO : "");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private void SelectDrawingDataRow()
        {
            try
            {
                if (!string.IsNullOrEmpty(DrwModel.PART_NO))
                {
                    //DrwModel.DWG_TYPE_DESC = (SelectedRow != null) ? SelectedRow["DWG_TYPE_DESC"].ToString() : string.Empty;
                    GetDrawingDetails();
                }
                else
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Part number"));
                    //MessageBox.Show("Please select Part number.", "SmartPD");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void PreviousCommand()
        {
            if (TOTAL_PAGE_NO > 0)
            {
                int gettotalpage = TOTAL_PAGE_NO;
                int getpageno = Convert.ToInt32(PAGE_NO);
                if ((gettotalpage >= getpageno) && (PAGE_NO != 1))
                {
                    PAGE_NO--;
                    DrwModel.PAGE_NO = PAGE_NO;
                    GetDrawing(PAGE_NO);
                }
            }
        }

        private void NextCommand()
        {
            if (TOTAL_PAGE_NO > 0)
            {
                int gettotalpage = TOTAL_PAGE_NO;
                int getpageno = Convert.ToInt32(PAGE_NO);
                if (gettotalpage > getpageno)
                {
                    PAGE_NO++;
                    DrwModel.PAGE_NO = PAGE_NO;
                    GetDrawing(PAGE_NO);
                }
            }
        }

        #region GetAndSet Properties

        private DrawingModel1 _drawingModel;
        public DrawingModel1 DrwModel
        {
            get
            {
                return _drawingModel;
            }
            set
            {
                _drawingModel = value;
                IsRecordsUpdated = false;
                NotifyPropertyChanged("DrwModel");
            }
        }

        private DataRowView _selectedrow;
        public DataRowView SelectedRow
        {
            get
            {
                return _selectedrow;
            }

            set
            {
                _selectedrow = value;
            }
        }

        private DataView _dVPartMast;
        public DataView DV_PART_MAST
        {
            get { return _dVPartMast; }
            set
            {
                _dVPartMast = value;
                NotifyPropertyChanged("DV_PART_MAST");
            }
        }

        private DataView _dVDwgTypeMast;
        public DataView DV_DWG_TYPE_MAST
        {
            get { return _dVDwgTypeMast; }
            set
            {
                _dVDwgTypeMast = value;
                NotifyPropertyChanged("DV_DWG_TYPE_MAST");
            }
        }

        private DataView _dvProdDwgIssue;
        public DataView DV_PROD_DWG_ISSUE
        {
            get { return _dvProdDwgIssue; }
            set
            {
                _dvProdDwgIssue = value;
                NotifyPropertyChanged("DV_PROD_DWG_ISSUE");
            }
        }

        private string _partDescription;
        public string PART_DESCRIPTION
        {
            get { return _partDescription; }
            set
            {
                _partDescription = value;
                NotifyPropertyChanged("PART_DESCRIPTION");
            }

        }

        private int _pageno;
        public int PAGE_NO
        {
            get { return _pageno; }
            set
            {
                _pageno = value;
                NotifyPropertyChanged("PAGE_NO");
            }
        }

        private int _totalpageno;
        public int TOTAL_PAGE_NO
        {
            get { return _totalpageno; }
            set
            {
                _totalpageno = value;
                NotifyPropertyChanged("TOTAL_PAGE_NO");
            }
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

        private System.Drawing.Bitmap _photoSourceBitMap;
        public System.Drawing.Bitmap PhotoSourceBitMap
        {
            get
            {
                return _photoSourceBitMap;
            }
            set
            {
                _photoSourceBitMap = value;
                NotifyPropertyChanged("PhotoSourceBitMap");
            }
        }

        private string _prddwg;
        public string PRD_DWG
        {
            get { return _prddwg; }
            set
            {
                _prddwg = value;
                NotifyPropertyChanged("PRD_DWG");
            }
        }

        private byte[] _proddrwimage;
        public Byte[] PROD_DRW_IMAGE
        {
            get { return _proddrwimage; }
            set
            {
                _proddrwimage = value;
                NotifyPropertyChanged("PROD_DRW_IMAGE");
            }
        }


        private string _photoToolTip;
        public string PhotoToolTip
        {
            get
            {
                return _photoToolTip;
            }
            set
            {
                _photoToolTip = value;
                NotifyPropertyChanged("PhotoToolTip");
            }
        }

        private Visibility _showOrHidePhotoVisibility;
        public Visibility ShowOrHidePhotoVisibility
        {
            get
            {
                return _showOrHidePhotoVisibility;
            }
            set
            {
                _showOrHidePhotoVisibility = value;
                NotifyPropertyChanged("ShowOrHidePhotoVisibility");
            }
        }

        private string _showorHidePhotoText;
        public string ShoworHidePhotoText
        {
            get
            {
                return _showorHidePhotoText;
            }
            set
            {
                _showorHidePhotoText = value;
                NotifyPropertyChanged("ShoworHidePhotoText");
            }
        }

        private ObservableCollection<DropdownColumns> _partColumns;
        public ObservableCollection<DropdownColumns> PartColumns
        {
            get
            {
                return _partColumns;
            }
            set
            {
                _partColumns = value;
                NotifyPropertyChanged("PartColumns");
            }
        }

        private ObservableCollection<DropdownColumns> _dwgColumns;
        public ObservableCollection<DropdownColumns> DwgColumns
        {
            get
            {
                return _dwgColumns;
            }
            set
            {
                _dwgColumns = value;
                NotifyPropertyChanged("DwgColumns");
            }
        }

        private DDCI_INFO _activeEntity = null;
        public DDCI_INFO ActiveEntity
        {
            get
            {
                return _activeEntity;
            }
            set
            {
                _activeEntity = value;
                NotifyPropertyChanged("ActiveEntity");
            }
        }

        #endregion

        public void LoadMethod1(object sender, RoutedEventArgs e)
        {
            try
            {
                _drawingHost = (System.Windows.Forms.Integration.WindowsFormsHost)((System.Windows.Controls.UserControl)sender).FindName("drawing");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void LoadComboECN()
        {
            try
            {
                ApprovedByCombo = drwBll.GetApprovedByCombo();
                CompiledByCombo = drwBll.GetCompiledByCombo();
                ActualChangeImplementCombo = drwBll.GetActualChangeImplementCombo().DefaultView;
                ProposedImplementCombo = drwBll.GetProposedImplementCombo().DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Get ECN or MPS Details
        /// </summary>
        private void GetECNMPSDetails()
        {
            ////try
            ////{
            ////    string ecnRefNo = "";
            ////    string mpsRefNo = "";
            ////    DDPCNECNModel = drwBll.GetECNMPSDetails(DrwModel.PART_NO, "ECN", ref ecnRefNo);
            ////    DDPCNMPSModel = drwBll.GetECNMPSDetails(DrwModel.PART_NO, "MPS", ref mpsRefNo);
            ////    ECNReferenceNo = ecnRefNo;
            ////    MPSReferenceNo = mpsRefNo;
            ////    if (DDPCNECNModel.PART_NO.ToValueAsString().Trim() != "")
            ////    {
            ////        DDPCNECNModel.PART_NO = DrwModel.PART_NO;
            ////        if (DDPCNECNModel.RE_PPAP == true)
            ////        {
            ////            RE_PPAP = true;
            ////        }
            ////        else
            ////        {
            ////            NOT_RE_PPAP = true;
            ////        }
            ////    }

            ////    if (DDPCNECNModel.PART_NO.ToValueAsString().Trim() != "")
            ////    {
            ////        DDPCNMPSModel.PART_NO = DrwModel.PART_NO;
            ////        if (DDPCNMPSModel.RE_PPAP == true)
            ////        {
            ////            RE_PPAP_MPS = true;
            ////        }
            ////        else
            ////        {
            ////            NOT_RE_PPAP_MPS = true;
            ////        }
            ////    }

            ////    NotifyPropertyChanged("DrwModel");
            ////    NotifyPropertyChanged("DDPCNECNModel");
            ////}
            ////catch (Exception ex)
            ////{
            ////    throw ex.LogException();
            ////}
        }

        private void ClearAll()
        {
            try
            {
                DDPCNECNModel = new DD_PCN();
                DDPCNMPSModel = new DD_PCN();
                DrwModel = new DrawingModel1();
                DrwModel.PART_NO = "";
                DrwModel.DWG_TYPE_DESC = "";
                int icnt = 0;
                DV_PROD_DWG_ISSUE = drwBll.GetRevisionDetails("W#$%^~*&}))", "0");
                if (DV_PROD_DWG_ISSUE != null)
                {
                    //icnt = DV_PROD_DWG_ISSUE.Count;
                    /*
                    for (int ictr = 0; ictr < icnt; ictr++)
                    {
                        DV_PROD_DWG_ISSUE.Delete(0);
                    }
                    DV_PROD_DWG_ISSUE.AddNew();
                     */
                    NotifyPropertyChanged("DV_PROD_DWG_ISSUE");
                }
                /*
                ClearAllFields(DDPCNECNModel);
                ClearAllFields(DDPCNMPSModel);
                RE_PPAP = false;
                NOT_RE_PPAP = true;
                RE_PPAP_MPS = false;
                NOT_RE_PPAP_MPS = true;
                NotifyPropertyChanged("DDPCNECNModel");
                NotifyPropertyChanged("DDPCNMPSModel");
                NotifyPropertyChanged("DrwModel");
                NotifyPropertyChanged("RE_PPAP");
                NotifyPropertyChanged("NOT_RE_PPAP");
                NotifyPropertyChanged("RE_PPAP_MPS");
                NotifyPropertyChanged("NOT_RE_PPAP_MPS");
                */
                PART_DESCRIPTION = "";
                mdiChild.Title = ApplicationTitle + " - Drawings" + ((DrwModel.PART_NO.IsNotNullOrEmpty()) ? " - " + DrwModel.PART_NO : "");
                oldDV_PROD_DWG_ISSUE = null;
                NotifyPropertyChanged("PART_DESCRIPTION");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void ClearAllFields(DD_PCN entity)
        {
            try
            {
                entity.PART_NO = "";
                entity.ACTUAL_CHANGE_IMP = "";
                entity.APPROVED_BY = "";
                entity.CHANGE_EFFECTIVE = "";
                entity.COMPILED_BY = "";
                entity.CONTROL_PLAN = false;
                entity.COST_DESC = "";
                entity.CUST_NAME = "";
                entity.CUST_PART_NO = "";
                entity.DATE_OF_PCN = null;
                entity.DATE_OF_SIGN = null;
                entity.DISPOSITION = "";
                entity.GAUGE_DWG = null;
                entity.INFGG_INITIAL = "";
                entity.INFGG_QTY = "";
                entity.INHEAT_TREATMENT_INITIAL = "";
                entity.INHEAT_TREATMENT_QTY = "";
                entity.INWIP_INITIAL = "";
                entity.INWIP_QTY = "";
                entity.MANUFACTURE_PROCESS = "";
                entity.NATURE_OF_CHANGE = "";
                entity.OTHERS = false;
                entity.PART_DESC = "";
                entity.PFD = false;
                entity.PFMEA = false;
                entity.PRODUCT_CHANGE_NO = "";
                entity.PRODUCT_DWG = false;
                entity.RE_PPAP = false;
                entity.RESON_FOR_CHANGE = "";
                entity.SEQUENCE_DWG = false;
                entity.SFL_DRAW_ISSUE_DATE = null;
                entity.SFL_DRAW_ISSUEDATE1 = null;
                entity.SFL_DRAW_ISSUENO = null;
                entity.SFL_DRAW_ISSUENO1 = null;
                //lstEntity.SNO = ;
                entity.CUST_DWG_NO = "";
                entity.CUST_DWG_NO_ISSUE = "";
                entity.CUST_ISSUE_NO = null;
                entity.ROUTING_TAG = false;
                entity.FINISH_CODE_SAP_DWG = false;
                entity.WORK_INSTRUCTION = false;
                entity.SAP_SEQ_DWG_ISSUE_NO_UPD = false;
                entity.TOOL_DWG = false;
                ECNReferenceNo = "";
                PART_DESCRIPTION = "";
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
                ReadOnly = false;
                ActionMode = OperationMode.AddNew;
                AddEnable = false;
                EditEnable = true;
                SaveEnable = true;
                HasDropDownVisibility = Visibility.Hidden;
                ClearAll();
                PartNumberIsFocused = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void Edit()
        {
            //ClearAll();
            ActionMode = OperationMode.Edit;
            HasDropDownVisibility = Visibility.Visible;
            ReadOnly = true;
            AddEnable = true;
            EditEnable = true;
            SaveEnable = true;
            PartNumberIsFocused = true;
            //if (ActionPermission.Edit == false && ActionPermission.View == true)
            //{
            //    SaveEnable = false;
            //}
            //if (ActionPermission.Edit == false)
            //{
            //    SaveEnable = false;
            //}
            //ClearAll();
        }

        private MessageBoxResult ShowInformationMessageInput(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }


        /// <summary>
        /// show ecn window
        /// </summary>
        private void ShowECN()
        {
            try
            {
                //if (DrwModel.PART_NO.ToValueAsString().Trim() != "")
                //{

                //customerReference.ShowDialog();
                //}
                //else
                //{
                //    ShowInformationMessage("Part No. should be selected!");
                //}
                Progress.ProcessingText = PDMsg.Load;
                Progress.Start();
                MdiChild mdiCustomerReference = new MdiChild();
                //drwMaster.Title = ApplicationTitle + " - Drawings";
                frmCustomerReference customerReference = new frmCustomerReference(_userinformation, DrwModel.PART_NO, "ECN", mdiCustomerReference, PART_DESCRIPTION);
                mdiCustomerReference.Content = customerReference;
                mdiCustomerReference.Height = customerReference.Height + 40;
                mdiCustomerReference.Width = customerReference.Width + 20;
                //mdiCustomerReference.MinimizeBox = false;
                ///mdiCustomerReference.MaximizeBox = false;
                mdiCustomerReference.Resizable = false;
                if (MainMDI.IsFormAlreadyOpen("ECN") == false)
                {
                    MainMDI.Container.Children.Add(mdiCustomerReference);
                }
                else
                {
                    mdiCustomerReference = new MdiChild();
                    mdiCustomerReference = (MdiChild)MainMDI.GetFormAlreadyOpened("ECN");
                    MainMDI.SetMDI(mdiCustomerReference);
                }
                mdiCustomerReference.Position = new Point(mdiCustomerReference.Position.X, mdiCustomerReference.Position.Y + 50);
                MainMDI.Container.InvalidateSize();
                mdiCustomerReference.Position = new Point(mdiCustomerReference.Position.X, mdiCustomerReference.Position.Y - 50);
                Progress.End();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// show pcn window
        /// </summary>
        private void ShowPCN()
        {
            try
            {
                //if (DrwModel.PART_NO.ToValueAsString().Trim() != "")
                //{
                //frmCustomerReference customerReference = new frmCustomerReference(_userinformation, DrwModel.PART_NO, "MPS");
                //customerReference.ShowDialog();
                //}
                //else
                //{
                //    ShowInformationMessage("Part No. should be selected!");
                //}
                Progress.ProcessingText = PDMsg.Load;
                Progress.Start();
                MdiChild mdiCustomerReference = new MdiChild();
                //drwMaster.Title = ApplicationTitle + " - Drawings";
                frmCustomerReference customerReference = new frmCustomerReference(_userinformation, DrwModel.PART_NO, "MPS", mdiCustomerReference, PART_DESCRIPTION);
                mdiCustomerReference.Content = customerReference;
                mdiCustomerReference.Height = customerReference.Height + 40;
                mdiCustomerReference.Width = customerReference.Width + 20;
                //mdiCustomerReference.MinimizeBox = false;
                ///mdiCustomerReference.MaximizeBox = false;
                mdiCustomerReference.Resizable = false;
                if (MainMDI.IsFormAlreadyOpen("PCN") == false)
                {
                    MainMDI.Container.Children.Add(mdiCustomerReference);
                }
                else
                {
                    mdiCustomerReference = new MdiChild();
                    mdiCustomerReference = (MdiChild)MainMDI.GetFormAlreadyOpened("PCN");
                    MainMDI.SetMDI(mdiCustomerReference);
                }
                mdiCustomerReference.Position = new Point(mdiCustomerReference.Position.X, mdiCustomerReference.Position.Y + 50);
                MainMDI.Container.InvalidateSize();
                mdiCustomerReference.Position = new Point(mdiCustomerReference.Position.X, mdiCustomerReference.Position.Y - 50);
                Progress.End();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void EditSelectedPartNo(string partNo)
        {
            try
            {
                DrwModel.PART_NO = partNo;
                foreach (DataRowView drview in DV_PART_MAST)
                {
                    if (drview["PART_NO"].ToValueAsString().Trim() == partNo.Trim())
                    {
                        SelectedRow = drview;
                        SelectPartDataRow();
                        SelectedRow = DV_DWG_TYPE_MAST[0];
                        DrwModel.DWG_TYPE_DESC = "Product Drawing";
                        SelectDrawingDataRow();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void dgvProdDwgMast_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (e.Column.GetType() == typeof(System.Windows.Controls.DataGridTemplateColumn))
                {
                    if (e.Column.Header.ToString().Trim().ToUpper() == "DATE")
                    {
                        var popup = GetVisualChild<System.Windows.Controls.Primitives.Popup>(e.EditingElement);
                        if (popup != null && popup.IsOpen)
                        {
                            e.Cancel = true;
                        }
                    }
                }

                DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
                int output;
                if (int.TryParse(selecteditem["ISSUE_NO"].ToValueAsString(), out output) == true)
                {
                    selecteditem["ISSUE_NONUMERIC"] = selecteditem["ISSUE_NO"];
                }
                else
                {
                    selecteditem["ISSUE_NONUMERIC"] = 0;
                }
            }
            catch (Exception ex)
            {

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

        /// <summary>
        /// Confirm to close the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                WPF.MDI.ClosingEventArgs closingev;
                closingev = (WPF.MDI.ClosingEventArgs)e;

                if (DrwModel.PART_NO.IsNotNullOrEmpty() && (IsRecordsUpdated || IsChangesMade()))
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
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

        public void dgrdProdDwgMast_Sorting(object sender, DataGridSortingEventArgs e)
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
            DataTable dt = DV_PROD_DWG_ISSUE.ToTable().Copy();

            if (Sort == "" || Sort == "asc")
            {
                Sort = "desc";
                dt.Rows[dt.Rows.Count - 1]["ISSUE_NONUMERIC"] = "-999";
                dt.DefaultView.Sort = "ISSUE_NONUMERIC DESC";
            }
            else
            {
                dt.Rows[dt.Rows.Count - 1]["ISSUE_NONUMERIC"] = "999";
                Sort = "asc";
                dt.DefaultView.Sort = "ISSUE_NONUMERIC ASC";
            }

            DataTable dtFinal = dt.DefaultView.ToTable();
            DV_PROD_DWG_ISSUE = (dtFinal != null) ? dtFinal.DefaultView : null;
            DV_PROD_DWG_ISSUE.Sort = "";
        }

    }
}
