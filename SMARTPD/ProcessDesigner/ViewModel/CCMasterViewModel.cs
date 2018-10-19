using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using ProcessDesigner.BLL;
using ProcessDesigner.Model;
using System.Data;
using System.Windows;
using ProcessDesigner.Common;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using System.Drawing;
using System.IO;
using System.Windows.Controls;
using Excel;
using ICSharpCode.SharpZipLib;
using System.Reflection;
//using System.Windows.Forms;

namespace ProcessDesigner.ViewModel
{
    class CCMasterViewModel : ViewModelBase
    {
        private CostCenterMasterDet _costCenterMasterDet;
        private UserInformation _userInformation;

        public Action CloseAction { get; set; }

        private Visibility _dropDownVisibility = Visibility.Collapsed;
        public Visibility HasDropDownVisibility
        {
            get { return _dropDownVisibility; }
            set
            {
                _dropDownVisibility = value;
                NotifyPropertyChanged("HasDropDownVisibility");
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


        private bool _optCondition;
        public bool OptCondition
        {
            get { return _optCondition; }
            set
            {
                _optCondition = value;
                //SetProperty(ref _actionMode, value);
                NotifyPropertyChanged("OptCondition");
            }
        }

        private bool _optStandard;
        public bool OptStandard
        {
            get { return _optStandard; }
            set
            {
                _optStandard = value;
                //SetProperty(ref _actionMode, value);
                NotifyPropertyChanged("OptStandard");
            }
        }

        private bool _optCleaning;
        public bool OptCleaning
        {
            get { return _optCleaning; }
            set
            {
                _optCleaning = value;
                NotifyPropertyChanged("OptCleaning");
            }
        }

        private bool _optInspection;
        public bool OptInspection
        {
            get { return _optInspection; }
            set
            {
                _optInspection = value;
                NotifyPropertyChanged("OptLubrication");
            }
        }


        private bool _optLubrication;
        public bool OptLubrication
        {
            get { return _optLubrication; }
            set
            {
                _optLubrication = value;
                NotifyPropertyChanged("OptLubrication");
            }
        }

        private CostCenterMaster _costCenterMaster = null;
        public CostCenterMaster CostCenterMasterModel
        {
            get
            {
                return _costCenterMaster;
            }
            set
            {
                _costCenterMaster = value;
                NotifyPropertyChanged("CostCenterMasterModel");
            }
        }

        private CostCenterMaster _oldcostCenterMaster = null;
        public CostCenterMaster OldCostCenterMasterModel
        {
            get
            {
                return _oldcostCenterMaster;
            }
            set
            {
                _oldcostCenterMaster = value;
                NotifyPropertyChanged("OldCostCenterMasterModel");
            }
        }


        private string _message = string.Empty;
        private string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                ShowInformationMessage(_message);
                //System.Windows.MessageBox.Show(_message, "SmartPD", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
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

        private DataView _dvOutput;
        public DataView DvOutput
        {
            get
            {
                return _dvOutput;
            }
            set
            {
                _dvOutput = value;
                NotifyPropertyChanged("DvOutput");
            }
        }

        private DataView _dvOperation;
        public DataView DvOperation
        {
            get
            {
                return _dvOperation;
            }
            set
            {
                _dvOperation = value;
                NotifyPropertyChanged("DvOperation");
            }
        }

        private DataView _dvOldOutput;
        public DataView DvOldOutput
        {
            get
            {
                return _dvOldOutput;
            }
            set
            {
                _dvOldOutput = value;
                NotifyPropertyChanged("DvOldOutput");
            }
        }

        private DataView _dvOldOperation;
        public DataView DvOldOperation
        {
            get
            {
                return _dvOldOperation;
            }
            set
            {
                _dvOldOperation = value;
                NotifyPropertyChanged("DvOldOperation");
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

        private bool _addEnable;
        public bool AddEnable
        {
            get
            {
                if (ActionPermission.AddNew == false)
                {
                    _addEnable = false;
                }
                return _addEnable;
            }
            set
            {
                if (ActionPermission.AddNew == false)
                {
                    _addEnable = false;
                }
                else
                {
                    _addEnable = value;
                }
                NotifyPropertyChanged("AddEnable");
            }
        }

        private bool _editEnable;
        public bool EditEnable
        {
            get
            {
                return _editEnable;
            }
            set
            {
                _editEnable = value;
                NotifyPropertyChanged("EditEnable");
            }
        }

        private bool _photoEnable;
        public bool PhotoEnable
        {
            get
            {
                return _photoEnable;
            }
            set
            {
                _photoEnable = value;
                NotifyPropertyChanged("PhotoEnable");
            }
        }



        private bool _saveEnable;
        public bool SaveEnable
        {
            get
            {
                if (ActionPermission.Save == false)
                {
                    _saveEnable = false;
                }
                return _saveEnable;
            }
            set
            {
                if (ActionPermission.Save == false)
                {
                    _saveEnable = false;
                }
                else
                {
                    _saveEnable = value;
                }
                NotifyPropertyChanged("SaveEnable");
            }
        }

        private bool _showDetailsEnable;
        public bool ShowDetailsEnable
        {
            get
            {
                return _showDetailsEnable;
            }
            set
            {
                _showDetailsEnable = value;
                NotifyPropertyChanged("ShowDetailsEnable");
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

        private System.IO.FileStream _fileStreamPhoto;
        public System.IO.FileStream FileStreamPhoto
        {
            get
            {
                return _fileStreamPhoto;
            }
            set
            {
                _fileStreamPhoto = value;
                NotifyPropertyChanged("FileStreamPhoto");
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

        private Visibility _standardOptionVisibility;
        public Visibility StandardOptionVisibility
        {
            get
            {
                return _standardOptionVisibility;
            }
            set
            {
                _standardOptionVisibility = value;
                NotifyPropertyChanged("StandardOptionVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItems;
        public ObservableCollection<DropdownColumns> DropDownItems
        {
            get
            {
                return _dropDownItems;
            }
            set
            {
                _dropDownItems = value;
                NotifyPropertyChanged("DropDownItems");
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

        private RolePermission _actionPermission;
        public RolePermission ActionPermission
        {
            get { return _actionPermission; }
            set
            {
                _actionPermission = value;
                NotifyPropertyChanged("ActionPermission");
            }
        }

        private DataRowView _operSelectedRow;
        public DataRowView OperSelectedRow
        {
            get { return _operSelectedRow; }
            set
            {
                _operSelectedRow = value;
                NotifyPropertyChanged("OperSelectedRow");
            }
        }

        private DataRowView _outputSelectedRow;
        public DataRowView OutputSelectedRow
        {
            get { return _outputSelectedRow; }
            set
            {
                _outputSelectedRow = value;
                NotifyPropertyChanged("OutputSelectedRow");
            }
        }


        public DataView OperCodeMaster { get; set; }
        public DataView UnitCodeMaster { get; set; }

        private DataView _costCenterCodeCombo;

        public DataView CostCenterCodeCombo
        {
            get { return _costCenterCodeCombo; }
            set
            {
                _costCenterCodeCombo = value;
                NotifyPropertyChanged("CostCenterCodeCombo");
            }
        }


        public DataView OpMasterCombo { get; set; }
        public DataView CategoryCombo { get; set; }
        public DataView LocationCombo { get; set; }
        public DataView ModuleCombo { get; set; }
        public DataView UnitMasterCombo { get; set; }



        private readonly Common.LogWriter log;

        private readonly ICommand closeCommand;
        public ICommand CloseClickCommand { get { return this.closeCommand; } }

        private readonly ICommand addNewCommand;
        public ICommand AddNewClickCommand { get { return this.addNewCommand; } }

        private readonly ICommand selectedItemChangedCommand;
        public ICommand SelectedItemChangedCommand { get { return this.selectedItemChangedCommand; } }



        private readonly ICommand editCommand;
        public ICommand EditClickCommand { get { return this.editCommand; } }

        private readonly ICommand saveCommand;
        public ICommand SaveClickCommand { get { return this.saveCommand; } }

        private readonly ICommand importCostCommand;
        public ICommand ImportCostClickCommand { get { return this.importCostCommand; } }


        private readonly ICommand showPhotoCommand;
        public ICommand ShowPhotoClickCommand { get { return this.showPhotoCommand; } }

        private readonly ICommand showDetailsCommand;
        public ICommand ShowDetailsClickCommand { get { return this.showDetailsCommand; } }

        private readonly ICommand tPMInfoCommand;
        public ICommand TPMInfoClickCommand { get { return this.tPMInfoCommand; } }

        private readonly ICommand standardCommand;
        public ICommand StandardClickCommand { get { return this.standardCommand; } }

        private readonly ICommand deleteOperationCommand;
        public ICommand DeleteOperationClickCommand { get { return this.deleteOperationCommand; } }

        private readonly ICommand deleteOutputCommand;
        public ICommand DeleteOutputClickCommand { get { return this.deleteOutputCommand; } }

        private readonly ICommand uploadCommand;
        public ICommand UploadClickCommand { get { return this.uploadCommand; } }

        private ObservableCollection<DropdownColumns> _dropDownHeaderCategory = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderCategory
        {
            get { return this._dropDownHeaderCategory; }
            set
            {
                this._dropDownHeaderCategory = value;
                NotifyPropertyChanged("DropDownHeaderCategory");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderLocation = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderLocation
        {
            get { return this._dropDownHeaderLocation; }
            set
            {
                this._dropDownHeaderLocation = value;
                NotifyPropertyChanged("DropDownHeaderLocation");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderModule = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderModule
        {
            get { return this._dropDownHeaderModule; }
            set
            {
                this._dropDownHeaderModule = value;
                NotifyPropertyChanged("DropDownHeaderModule");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderOper = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderOper
        {
            get { return this._dropDownHeaderOper; }
            set
            {
                this._dropDownHeaderOper = value;
                NotifyPropertyChanged("DropDownHeaderOper");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownHeaderUnit = null;
        public ObservableCollection<DropdownColumns> DropDownHeaderUnit
        {
            get { return this._dropDownHeaderUnit; }
            set
            {
                this._dropDownHeaderUnit = value;
                NotifyPropertyChanged("DropDownHeaderUnit");
            }
        }


        private DataRowView _operCode_SelectedItem = null;
        public DataRowView OperCode_SelectedItem
        {
            get { return this._operCode_SelectedItem; }
            set
            {
                this._operCode_SelectedItem = value;
                NotifyPropertyChanged("OperCode_SelectedItem");
                //OperCode_SelectionChanged();
            }
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="userInformation"></param>
        public CCMasterViewModel(UserInformation userInformation)
        {
            try
            {
                _costCenterMaster = new CostCenterMaster();
                _costCenterMasterDet = new CostCenterMasterDet(userInformation);
                this.addNewCommand = new DelegateCommand(this.AddNewSubmitCommand);
                this.selectedItemChangedCommand = new DelegateCommand(this.SelectDataRow);
                this.saveCommand = new DelegateCommand(this.SaveSubmitCommand);
                this.importCostCommand = new DelegateCommand(this.ImportCostCommand);
                this.editCommand = new DelegateCommand(this.EditSubmitCommand);
                this.showPhotoCommand = new DelegateCommand(this.ShowPhotoSubmitCommand);
                this.showDetailsCommand = new DelegateCommand(this.ShowDetailsSubmitCommand);
                this.tPMInfoCommand = new DelegateCommand<string>(this.TMPInfoSubmitCommand);
                this.standardCommand = new DelegateCommand<string>(this.StandardSubmitCommand);
                this.closeCommand = new DelegateCommand(this.Close);
                this.deleteOperationCommand = new DelegateCommand(this.DeleteOperationSubmitCommand);
                this.deleteOutputCommand = new DelegateCommand(this.DeleteOutputSubmitCommand);
                this.uploadCommand = new DelegateCommand(this.UploadSubmitCommand);
                this.rowEditEndingOperationCommand = new DelegateCommand<DataRowView>(this.RowEditEndingOperation);
                this.rowEditEndingOutputCommand = new DelegateCommand<DataRowView>(this.RowEditEndingOutput);
                this._userInformation = userInformation;
                SetdropDownItems();
                GetAllCombo();
                GetGrid();
                ReadOnly = false;
                ShoworHidePhotoText = "Show Photo F8";
                PhotoToolTip = "Show Photo";
                StandardOptionVisibility = Visibility.Collapsed;
                GetRights();
                ClearAll();
                log = new LogWriter();
                if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
                {
                    EditSubmitCommand();
                }
                CostCenterMasterModel.CostCentCode = "";
                CostCenterMasterModel.CostCentDesc = "";

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private DataTable dtExcelData;
        private void ImportCostCommand()
        {
            bool isColumnInvalid = false;
            DataTable dtErrorData = new DataTable();
            dtErrorData.Columns.Add("COST_CENT_CODE");
            dtErrorData.Columns.Add("OPN_CODE");

            string baseFileName = "CCCostErrorLog" + "-" + DateTime.Now.Date.ToString("ddMMyyyy") + ".log";
            filename = CreateTextFile(baseFileName);
            try
            {

                dtExcelData = ImportExcelToDataTable();
                if (dtExcelData.IsNotNullOrEmpty())
                    if (dtExcelData.Rows.Count > 0)
                    {
                        if (dtExcelData.Columns.Contains("COST CENTRE"))
                        {
                            dtExcelData.Columns["COST CENTRE"].ColumnName = "COST_CENT_CODE";
                        }
                        else
                        {
                            isColumnInvalid = true;
                            dtExcelData.Columns.Add("COST_CENT_CODE");
                        }

                        if (dtExcelData.Columns.Contains("OPERATION CODE"))
                        {
                            dtExcelData.Columns["OPERATION CODE"].ColumnName = "OPN_CODE";
                        }
                        else
                        {
                            isColumnInvalid = true;
                            dtExcelData.Columns.Add("OPN_CODE");
                        }

                        if (dtExcelData.Columns.Contains("VARIABLE PRICE"))
                        {
                            dtExcelData.Columns["VARIABLE PRICE"].ColumnName = "VAR_COST";
                        }
                        else
                        {
                            isColumnInvalid = true;
                            dtExcelData.Columns.Add("VAR_COST");
                        }

                        if (dtExcelData.Columns.Contains("FIXED PRICE"))
                        {
                            dtExcelData.Columns["FIXED PRICE"].ColumnName = "FIX_COST";
                        }
                        else
                        {
                            isColumnInvalid = true;
                            dtExcelData.Columns.Add("FIX_COST");
                        }
                        if (isColumnInvalid)
                        {
                            ShowInformationMessage("Invalid Column Name Occured. Column Name Should be" + Environment.NewLine + "COST CENTRE , OPERATION CODE , VARIABLE PRICE , FIXED PRICE");
                            return;
                        }

                        Progress.ProcessingText = "Updating";
                        Progress.Start();

                        string stsMsg = "";
                        LogWrite("-------------------------------------------------------------------------------");
                        LogWrite("Begin Time: " + DateTime.Now + "\t" + _userInformation.UserName);
                        LogWrite("-------------------------------------------------------------------------------");
                        foreach (DataRowView item in dtExcelData.DefaultView)
                        {
                            stsMsg = _costCenterMasterDet.UpdateCCMasterVariableFixedCost(ref dtErrorData, item.Row["COST_CENT_CODE"].ToValueAsString(), item.Row["OPN_CODE"].ToString().ToIntValue(), item.Row["VAR_COST"].ToString().ToDecimalValue(), item.Row["FIX_COST"].ToString().ToDecimalValue());
                            LogWrite(item.Row["COST_CENT_CODE"].ToValueAsString() + "\t" + item.Row["OPN_CODE"].ToString() + "\t" + item.Row["VAR_COST"].ToString() + "\t" + item.Row["FIX_COST"].ToString() + "\t" + stsMsg);
                        }
                        LogWrite("-------------------------------------------------------------------------------");
                        LogWrite("End Time: " + DateTime.Now + "\t" + _userInformation.UserName + "\t" + "Processed Rows Count: " + dtExcelData.Rows.Count);
                        LogWrite("-------------------------------------------------------------------------------");
                        Progress.End();
                        if (dtErrorData.Rows.Count > 0)
                        {
                            ShowInformationMessage("Invalid Data. Refer path" + Environment.NewLine + filename + ". for details" + Environment.NewLine + "Total Records Count: " + dtExcelData.Rows.Count + " Error Records Count: " + dtErrorData.Rows.Count);
                        }
                        else
                        {
                            ShowInformationMessage("Cost updated Successfully");
                        }
                    }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

        }

        private DataTable ImportExcelToDataTable()
        {
            string filePath = string.Empty;
            string fileExt = string.Empty;
            DataTable dtExcelData = new DataTable();
            try
            {
                Microsoft.Win32.OpenFileDialog file = new Microsoft.Win32.OpenFileDialog(); //open dialog to choose file  
                file.Filter = "Excel files (*.xls or .xlsx)|.xls;*.xlsx";
                file.ShowDialog();

                if (file.FileName.IsNotNullOrEmpty()) //if there is a file choosen by the user  
                {
                    filePath = file.FileName; //get the path of the file  
                    fileExt = Path.GetExtension(filePath); //get the file extension  
                    if (fileExt.CompareTo(".xls") == 0 || fileExt.CompareTo(".xlsx") == 0)
                    {

                        //FileStream stream = File.Open(Path.GetFullPath(filePath), FileMode.Open, FileAccess.Read);
                        using (var stream = File.Open(Path.GetFullPath(filePath), FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            IExcelDataReader excelReader = null;
                            switch (fileExt.ToLower())
                            {
                                case ".xls":
                                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                                    break;
                                case ".xlsx":
                                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                                    break;
                            }
                            excelReader.IsFirstRowAsColumnNames = true;
                            DataSet result = excelReader.AsDataSet();
                            dtExcelData = result.Tables[0];
                            stream.Close();
                        }

                    }
                    else
                    {
                        ShowInformationMessage("Please choose .xls or .xlsx file only.");
                    }
                }
            }
            catch (Exception)
            {

            }
            return dtExcelData;
        }



        private bool _changefocus = false;
        public bool ChangeFocus
        {
            get { return _changefocus; }
            set
            {
                _changefocus = value;
                NotifyPropertyChanged("ChangeFocus");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void NewMode()
        {
            try
            {
                ReadOnly = false;
                ActionMode = OperationMode.AddNew;
                _costCenterMaster = new CostCenterMaster();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private bool Validate()
        {
            try
            {
                //if (((TextBox)usrCostCentreCode.FindName("txtCombobox")).Text.ToString().Trim() == "")
                if (_costCenterMaster.CostCentCode == null || _costCenterMaster.CostCentCode.Trim() == "")
                {
                    Message = PDMsg.NotEmpty("Cost centre code");
                    //Message = "Cost centre code should be entered";
                    //((TextBox)usrCostCentreCode.FindName("txtCombobox")).Focus();
                    return false;
                }

                if (_costCenterMaster.CostCentDesc == null || _costCenterMaster.CostCentDesc.Trim() == "")
                {
                    Message = PDMsg.NotEmpty("Cost centre description");
                    //MessageBox.Show("Cost center description should be entered", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    //Message = "Cost centre description should be entered";
                    //tbDescription.Focus();
                    return false;
                }


                if (_costCenterMaster.Efficiency >= 100)
                {
                    //MessageBox.Show("Efficiency should be less than 100", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    Message = "Efficiency should be less than 100";
                    return false;
                    //tbEfficiency.Focus();
                }

                if (CheckMandatoryOperation() == false)
                {
                    return false;
                }

                if (CheckMandatoryOutput() == false)
                {
                    return false;
                }

                if (_costCenterMasterDet.CheckDuplicate(_costCenterMaster.Operation, "opn_code"))
                {
                    Message = "Duplicate Operation Code should not be Entered";
                    return false;
                }



                if (_costCenterMasterDet.CheckDuplicate(_costCenterMaster.Output, "output"))
                {
                    Message = "Duplicate Output should not be Entered";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool CheckMandatoryOperation()
        {
            DataRow drRow;

            for (int i = 0; i < _costCenterMaster.Operation.Rows.Count; i++)
            {
                drRow = _costCenterMaster.Operation.Rows[i];
                if (i == _costCenterMaster.Operation.Rows.Count - 1)
                {
                    if (drRow["OPN_CODE"].ToValueAsString().Trim() == "" && (drRow["FIX_COST"].ToValueAsString().Trim().ToDecimalValue() > 0 || drRow["VAR_COST"].ToValueAsString().Trim().ToDecimalValue() > 0))
                    {
                        Message = "Operation code is mandatory for output details";
                        return false;
                    }

                    if (drRow["UNIT_CODE"].ToValueAsString().Trim() == "" && (drRow["FIX_COST"].ToValueAsString().Trim().ToDecimalValue() > 0 || drRow["VAR_COST"].ToValueAsString().Trim().ToDecimalValue() > 0))
                    {
                        Message = "Unit code is mandatory for output details";
                        return false;
                    }

                }
                else
                {
                    if (drRow["OPN_CODE"].ToValueAsString().Trim() == "")
                    {
                        Message = "Operation code is mandatory for output details";
                        return false;
                    }

                    if (drRow["UNIT_CODE"].ToValueAsString().Trim() == "")
                    {
                        Message = "Unit code is mandatory for output details";
                        return false;
                    }

                }
            }
            return true;
        }

        private bool CheckMandatoryOutput()
        {
            DataRow drRow;

            for (int i = 0; i < _costCenterMaster.Output.Rows.Count; i++)
            {
                drRow = _costCenterMaster.Output.Rows[i];
                if (i != _costCenterMaster.Output.Rows.Count - 1)
                {
                    if (drRow["output"].ToValueAsString().Trim() == "")
                    {
                        Message = "Output code is mandatory for output details";
                        return false;
                    }
                }
            }
            return true;
        }


        /// <summary>
        /// 
        /// </summary>
        private void GetAllCombo()
        {
            DataSet dsData;
            try
            {
                dsData = _costCenterMasterDet.GetAllCombo();
                CategoryCombo = dsData.Tables[0].DefaultView;
                LocationCombo = dsData.Tables[1].DefaultView;
                ModuleCombo = dsData.Tables[2].DefaultView;
                CostCenterCodeCombo = dsData.Tables[3].DefaultView;
                if (CostCenterCodeCombo.Count > 0) CostCenterCodeCombo.Sort = "OPER_CODE_SORT asc";
                OperCodeMaster = dsData.Tables[4].DefaultView;
                UnitCodeMaster = dsData.Tables[5].DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void GetGrid()
        {
            DataSet dsData;
            try
            {
                dsData = _costCenterMasterDet.GetGridDetails(_costCenterMaster.CostCentCode);
                _costCenterMaster.Output = dsData.Tables[0];
                _costCenterMaster.Operation = dsData.Tables[1];
                DvOutput = dsData.Tables[0].DefaultView;
                DvOperation = dsData.Tables[1].DefaultView;
                DvOperation.AddNew();
                DvOperation.EndInit();
                DvOutput.AddNew();
                DvOutput.EndInit();

                DvOldOperation = DvOperation.ToTable().Copy().DefaultView;
                DvOldOutput = DvOutput.ToTable().Copy().DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void ClearAll()
        {
            try
            {
                DataTable dtOperation;
                DataTable dtOutput;
                ShoworHidePhotoText = "Show Photo F8";
                PhotoToolTip = "Show Photo";
                PhotoSource = null;
                ShowOrHidePhotoVisibility = Visibility.Collapsed;
                ActionMode = OperationMode.AddNew;
                ReadOnly = false;
                HasDropDownVisibility = Visibility.Collapsed;
                CostCenterMasterModel = new CostCenterMaster();
                dtOperation = DvOperation.ToTable();
                dtOperation.Rows.Clear();
                DvOperation = dtOperation.DefaultView;
                DvOperation.AddNew();
                dtOutput = DvOutput.ToTable();
                dtOutput.Rows.Clear();
                DvOutput = dtOutput.DefaultView;
                DvOutput.AddNew();
                AddEnable = false;
                PhotoEnable = false;
                ShowDetailsEnable = false;
                EditEnable = true;
                SaveEnable = true;
                ClearOption();
                CostCenterMasterModel.FilePath = "";
                CostCenterMasterModel.CostCentCode = "";
                CostCenterMasterModel.CostCentDesc = "";

                if (ActionPermission.AddNew == false && ActionMode == OperationMode.AddNew)
                {
                    SaveEnable = false;
                }

                OldCostCenterMasterModel = new CostCenterMaster();
                DvOldOperation = DvOperation.ToTable().Copy().DefaultView;
                DvOldOutput = DvOutput.ToTable().Copy().DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void ClearOption()
        {
            OptCondition = false;
            OptStandard = false;
            OptCleaning = false;
            OptLubrication = false;
            OptInspection = false;
            StandardOptionVisibility = Visibility.Collapsed;
            ShowOrHidePhotoVisibility = Visibility.Collapsed;
            PhotoSource = null;
        }

        private void AssignValues()
        {
            try
            {
                DataRow[] drRow;
                ShoworHidePhotoText = "Show Photo F8";
                PhotoToolTip = "Show Photo";
                _costCenterMasterDet.GetCostCenterMaster(ref _costCenterMaster);
                GetGrid();
                ShowDetailsEnable = true;
                ClearOption();
                drRow = CategoryCombo.ToTable().Select("CATE_CODE='" + _costCenterMaster.CateCode + "'");
                if (drRow.Length > 0)
                {
                    _costCenterMaster.Category = drRow[0]["CATEGORY"].ToValueAsString().Trim();
                }

                drRow = LocationCombo.ToTable().Select("LOC_CODE='" + _costCenterMaster.LocCode + "'");
                if (drRow.Length > 0)
                {
                    _costCenterMaster.LocationName = drRow[0]["LOCATION"].ToValueAsString().Trim();
                }

                drRow = ModuleCombo.ToTable().Select("MODULE_CODE='" + _costCenterMaster.Module + "'");
                if (drRow.Length > 0)
                {
                    _costCenterMaster.ModuleName = drRow[0]["MODULE_NAME"].ToValueAsString().Trim();
                }

                OldCostCenterMasterModel.CostCentCode = CostCenterMasterModel.CostCentCode;
                OldCostCenterMasterModel.CostCentDesc = CostCenterMasterModel.CostCentDesc;
                OldCostCenterMasterModel.CateCode = CostCenterMasterModel.CateCode;
                OldCostCenterMasterModel.LocCode = CostCenterMasterModel.LocCode;
                OldCostCenterMasterModel.Module = CostCenterMasterModel.Module;
                OldCostCenterMasterModel.MachineName = CostCenterMasterModel.MachineName;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// This procedure will be executed if the user select the required cost  centre in the user control combo box in edit mode
        /// 
        /// </summary>
        private void SelectDataRow()
        {
            AssignValues();
        }

        private void AddNewSubmitCommand()
        {
            if (IsChangesMade())
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SaveSubmitCommand();
                    return;
                }
            }
            ClearAll();
        }

        /// <summary>
        /// This method will be executed if the user clicks the save button
        /// </summary>
        private void SaveSubmitCommand()
        {
            try
            {
                DataRow drrow;
                int irow;
                ChangeFocus = true;
                if (SaveEnable == false) return;

                if (ActionPermission.AddNew == false && ActionMode == OperationMode.AddNew)
                {
                    return;
                }

                if (ActionPermission.Edit == false && ActionMode == OperationMode.Edit)
                {
                    return;
                }

                CostCenterMasterModel.Output = DvOutput.ToTable().Copy();
                CostCenterMasterModel.Operation = DvOperation.ToTable().Copy();
                if (Validate() == true)
                {

                    if (ActionMode == OperationMode.AddNew)
                    {
                        if (_costCenterMasterDet.CheckDuplicateCode(_costCenterMaster.CostCentCode) > 0)
                        {
                            Message = PDMsg.AlreadyExists("Cost Centre Code");
                            //Message = "Cost Centre Code already exists";
                            return;
                        }
                    }

                    irow = DvOperation.Count - 1;

                    if (irow >= 0)
                    {
                        drrow = DvOperation.ToTable().Copy().Rows[irow];
                        if (drrow["OPN_CODE"].ToValueAsString() == "" || drrow["UNIT_CODE"].ToValueAsString() == "")
                        {
                            DvOperation.Delete(irow);
                        }
                    }

                    irow = DvOutput.Count - 1;

                    if (irow >= 0)
                    {
                        drrow = DvOutput.ToTable().Copy().Rows[irow];
                        if (drrow["OUTPUT"].ToValueAsString() == "")
                        {
                            DvOutput.Delete(irow);
                        }
                    }
                    Progress.ProcessingText = PDMsg.ProgressUpdateText;
                    Progress.Start();
                    CostCenterMasterModel.Operation = DvOperation.ToTable().Copy();
                    CostCenterMasterModel.Output = DvOutput.ToTable().Copy();
                    _costCenterMasterDet.SaveCostCenterMaster(CostCenterMasterModel, ActionMode);
                    if (ActionMode == OperationMode.AddNew)
                    {
                        GetAllCombo();
                    }
                    Progress.End();
                    if (ActionMode == OperationMode.AddNew)
                    {
                        Message = PDMsg.SavedSuccessfully;
                    }
                    else
                    {
                        Message = PDMsg.UpdatedSuccessfully;
                    }

                    log.LogWrite(" Save CCmaster" + Message.ToString());
                    ChangeLookUpRow();
                    ClearAll();

                }
            }
            catch (Exception ex)
            {

                log.LogWrite(" Save CCmaster" + ex.Message.ToString());
                throw ex.LogException();
            }
        }

        private void EditSubmitCommand()
        {
            if (IsChangesMade())
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SaveSubmitCommand();
                    return;
                }
            }
            ClearAll();
            ActionMode = OperationMode.Edit;
            HasDropDownVisibility = Visibility.Visible;
            ReadOnly = true;
            AddEnable = true;
            PhotoEnable = true;
            EditEnable = false;
            SaveEnable = true;
            if (ActionPermission.Edit == false && ActionPermission.View == true)
            {
                SaveEnable = false;
            }
            if (ActionPermission.Edit == false)
            {
                SaveEnable = false;
            }
            ClearOption();
        }

        private void ShowPhotoSubmitCommand()
        {
            try
            {
                if (ShoworHidePhotoText.Contains("Hide"))
                {
                    ShowOrHidePhotoVisibility = Visibility.Collapsed;
                    ShoworHidePhotoText = "Show Photo F8";
                    PhotoToolTip = "Show Photo";
                }
                else
                {
                    if (PhotoSource == null)
                    {
                        if (ShowImage(0) == false)
                        {
                            ShowImage(78);
                        }
                    }
                    else
                    {
                        ShoworHidePhotoText = "Hide Photo F8";
                        PhotoToolTip = "Hide Photo";
                        ShowOrHidePhotoVisibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool ShowImage(int offset)
        {
            try
            {
                System.Windows.Media.Imaging.BitmapImage bitmap;
                string filepath = "";
                System.IO.MemoryStream strm;
                bitmap = new System.Windows.Media.Imaging.BitmapImage();
                strm = _costCenterMasterDet.ShowImage(_costCenterMaster.CostCentCode, offset, ref filepath);
                if (strm.IsNotNullOrEmpty())
                {
                    strm.Seek(0, SeekOrigin.Begin);
                    bitmap.BeginInit();
                    bitmap.StreamSource = strm;
                    bitmap.EndInit();
                    PhotoSource = bitmap;
                    ShoworHidePhotoText = "Hide Photo F8";
                    PhotoToolTip = "Hide Photo";
                    CostCenterMasterModel.FilePath = filepath;
                    ShowOrHidePhotoVisibility = Visibility.Visible;
                }
                else
                {
                    Message = "Cost Centre Photo not Available";
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;

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
        private string applicationTitl = "SmartPD - ";
        private void ShowDetailsSubmitCommand()
        {
            if (ActionMode == OperationMode.AddNew)
            {
                return;
            }
            if (CostCenterMasterModel.CateCode == null)
            {
                return;
            }
            switch (CostCenterMasterModel.CateCode.ToIntValue())
            {
                case 1:
                    frmCircularRolling circularRolling = new frmCircularRolling(_userInformation, _costCenterMaster.CostCentCode);
                    circularRolling.Title = applicationTitl + "Circular Thread Rolling Machine";
                    circularRolling.ShowDialog();
                    break;
                case 2:
                    frmCopyTurningMac copyTurningMac = new frmCopyTurningMac(_userInformation, _costCenterMaster.CostCentCode);
                    copyTurningMac.Title = applicationTitl + "Copy Turning Machine";
                    copyTurningMac.ShowDialog();
                    break;
                case 3:
                    frmFlatRollMac flatRollMac = new frmFlatRollMac(_userInformation, _costCenterMaster.CostCentCode);
                    flatRollMac.Title = applicationTitl + "Flat Thread Rolling Machine";
                    flatRollMac.ShowDialog();
                    break;
                case 4:
                    frmForging forging = new frmForging(_userInformation, _costCenterMaster.CostCentCode);
                    forging.Title = applicationTitl + "Forging Machines - cold forging";
                    forging.ShowDialog();
                    break;
                case 5:
                    forging = new frmForging(_userInformation, _costCenterMaster.CostCentCode);
                    forging.Title = applicationTitl + "Forging Machines - hot forging";
                    forging.ShowDialog();
                    break;
                case 6:
                    frmGrinmac grinMac = new frmGrinmac(_userInformation, _costCenterMaster.CostCentCode);
                    grinMac.Title = applicationTitl + "Grinding Machine";
                    grinMac.ShowDialog();
                    break;
                case 8:
                    frmNylocmac nylocmac = new frmNylocmac(_userInformation, _costCenterMaster.CostCentCode);
                    nylocmac.Title = applicationTitl + "Nyloc Machine";
                    nylocmac.ShowDialog();
                    break;
                case 10:
                    frmPointmac pointmac = new frmPointmac(_userInformation, _costCenterMaster.CostCentCode);
                    pointmac.Title = applicationTitl + "Pointing Machine";
                    pointmac.ShowDialog();
                    break;
                case 7:
                    frmTapmac tapmac = new frmTapmac(_userInformation, _costCenterMaster.CostCentCode);
                    tapmac.Title = applicationTitl + "Tapping Machine";
                    tapmac.ShowDialog();
                    break;
                case 11:
                    frmTrimmac trimmac = new frmTrimmac(_userInformation, _costCenterMaster.CostCentCode);
                    trimmac.Title = applicationTitl + "Trimming Machine";
                    trimmac.ShowDialog();
                    break;
            }
        }


        private void TMPInfoSubmitCommand(string option)
        {
            if (CostCenterMasterModel.CostCentCode != null && CostCenterMasterModel.CostCentCode != "" && ActionMode == OperationMode.Edit)
            {
                if (option.ToUpper() == "STANDARD")
                {
                    StandardOptionVisibility = Visibility.Visible;
                }
                else if (option.ToUpper() == "OPTIMAL")
                {
                    StandardOptionVisibility = Visibility.Collapsed;
                    frmOptimalCondition optimalCondition = new frmOptimalCondition(CostCenterMasterModel.CostCentCode);
                    //   OptCondition = false;
                    optimalCondition.ShowDialog();
                    //       
                }
                else
                {
                    StandardOptionVisibility = Visibility.Collapsed;
                }
            }
        }

        private void StandardSubmitCommand(string categoryId)
        {
            try
            {
                if (CostCenterMasterModel.CostCentCode != null && CostCenterMasterModel.CostCentCode != "" && ActionMode == OperationMode.Edit)
                {
                    frmStandardCondition standardCondition = new frmStandardCondition(_costCenterMaster.CostCentCode, categoryId);
                    standardCondition.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Delete the selected row in operation
        /// </summary>
        private void DeleteOperationSubmitCommand()
        {
            try
            {
                if (OperSelectedRow != null)
                {
                    int index = DvOperation.Table.Rows.IndexOf(OperSelectedRow.Row);
                    if (index > -1)
                    {
                        DvOperation.Delete(index);
                    }
                    if (DvOperation.Count == 0)
                    {
                        DvOperation.AddNew();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Delete the selected row in output
        /// </summary>
        private void DeleteOutputSubmitCommand()
        {
            try
            {
                if (OutputSelectedRow != null)
                {
                    int index = DvOutput.Table.Rows.IndexOf(OutputSelectedRow.Row);
                    if (index > -1)
                    {
                        DvOutput.Delete(index);
                    }
                    if (DvOutput.Count == 0)
                    {
                        DvOutput.AddNew();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Close()
        {
            if (IsChangesMade())
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SaveSubmitCommand();
                    return;
                }
            }

            if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
            {
                CloseAction();
            }
        }

        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (IsChangesMade())
                //{
                //    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //    {
                //        SaveSubmitCommand();
                //        return;
                //    }
                //}

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

        private bool IsChangesMade()
        {
            try
            {
                bool result = false;

                if (CostCenterMasterModel != null && OldCostCenterMasterModel != null)
                {
                    if (OldCostCenterMasterModel.CostCentCode.ToValueAsString() != CostCenterMasterModel.CostCentCode) result = true;
                    if (OldCostCenterMasterModel.CostCentDesc.ToValueAsString() != CostCenterMasterModel.CostCentDesc) result = true;
                    if (OldCostCenterMasterModel.CateCode != CostCenterMasterModel.CateCode) result = true;
                    if (OldCostCenterMasterModel.LocCode != CostCenterMasterModel.LocCode) result = true;
                    if (OldCostCenterMasterModel.Module != CostCenterMasterModel.Module) result = true;
                    if (OldCostCenterMasterModel.MachineName != CostCenterMasterModel.MachineName) result = true;
                    if (OldCostCenterMasterModel.PhotoChanged != CostCenterMasterModel.PhotoChanged) result = true;
                }

                if (DvOldOperation != null && DvOperation != null && !result)
                {
                    result = !DvOperation.IsEqual(DvOldOperation);
                }

                if (DvOldOutput != null && DvOutput != null && !result)
                {
                    result = !DvOutput.IsEqual(DvOldOutput);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void SetdropDownItems()
        {
            try
            {
                DropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "COST_CENT_CODE", ColumnDesc = "CC Code", ColumnWidth = 75.5 },
                            new DropdownColumns() { ColumnName = "COST_CENT_DESC", ColumnDesc = "Description", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Loc.", ColumnWidth = 51.5 },
                            new DropdownColumns() { ColumnName = "EFFICIENCY", ColumnDesc = "Efficiency", ColumnWidth = 83 }
                            //new DropdownColumns() { ColumnName = "OPER_CODE_SORT", ColumnDesc = "Cost Centre Code", ColumnWidth = 0, IsDefaultSearchColumn = true, ColumnVisibility = Visibility.Collapsed }
                        };

                DropDownHeaderCategory = new ObservableCollection<DropdownColumns>
                 {               
                    new DropdownColumns { ColumnName = "CATE_CODE", ColumnDesc = "Code", ColumnWidth = 60 },
                    new DropdownColumns { ColumnName = "CATEGORY", ColumnDesc = "Category Description", ColumnWidth = "1*", IsDefaultSearchColumn = true }
                 };

                DropDownHeaderLocation = new ObservableCollection<DropdownColumns>
                 {               
                    new DropdownColumns { ColumnName = "LOC_CODE", ColumnDesc = "Code", ColumnWidth = 60 },
                    new DropdownColumns { ColumnName = "LOCATION", ColumnDesc = "Location Description", ColumnWidth = "1*", IsDefaultSearchColumn = true }
                 };
                DropDownHeaderModule = new ObservableCollection<DropdownColumns>
                 {               
                    new DropdownColumns { ColumnName = "MODULE_CODE", ColumnDesc = "Code", ColumnWidth = 60 },
                    new DropdownColumns { ColumnName = "MODULE_NAME", ColumnDesc = "Module Description", ColumnWidth = "1*", IsDefaultSearchColumn = true }
                 };

                DropDownHeaderOper = new ObservableCollection<DropdownColumns>
                 {               
                    new DropdownColumns { ColumnName = "OPN_CODE", ColumnDesc = "Oper Code", ColumnWidth = 90 }, 
                    new DropdownColumns { ColumnName = "OPER_DESC", ColumnDesc = "Operations", ColumnWidth = "1*" }            
                 };

                DropDownHeaderUnit = new ObservableCollection<DropdownColumns>
                 {               
                    new DropdownColumns { ColumnName = "UNIT_CODE", ColumnDesc = "Unit Code", ColumnWidth = 100, IsDefaultSearchColumn = true },
                    new DropdownColumns { ColumnName = "UNIT_OF_MEAS", ColumnDesc = "Unit desc", ColumnWidth = "1*" }
                 };



            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void GetRights()
        {
            ActionPermission = new RolePermission();
            ActionPermission.Save = true;
            ActionPermission.Print = false;
            ActionPermission.View = true;
            ActionPermission.AddNew = false;
            ActionPermission.Delete = false;
            ActionPermission.Edit = false;
            ActionPermission = _costCenterMasterDet.GetUserRights("COST CENTER MASTER");
            if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
            {
                ActionPermission.Save = false;
            }
        }

        /// <summary>
        /// Upload Image
        /// </summary>
        private void UploadSubmitCommand()
        {
            System.IO.MemoryStream strm;
            System.Windows.Media.Imaging.BitmapImage bitmap;
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                dlg.DefaultExt = ".bmp";
                dlg.Filter = "Image Files (*.bmp, *.jpg,*.png,*.tif)|*.bmp;*.jpg;*.png;*.tif";
                dlg.ShowDialog();
                if (dlg.FileName != "")
                {
                    CostCenterMasterModel.FilePath = dlg.FileName;
                    if (CostCenterMasterModel.FilePath.ToString().Trim() != "")
                    {
                        System.Drawing.Image image = System.Drawing.Image.FromFile(CostCenterMasterModel.FilePath);
                        strm = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(CostCenterMasterModel.FilePath));
                        bitmap = new System.Windows.Media.Imaging.BitmapImage();

                        if (strm.IsNotNullOrEmpty())
                        {
                            bitmap.BeginInit();
                            bitmap.StreamSource = strm;
                            bitmap.EndInit();
                            CostCenterMasterModel.ImageByte = ConverImageToBinary(CostCenterMasterModel.FilePath);

                            PhotoSource = bitmap;
                            this.ShoworHidePhotoText = "Hide Photo F8";
                            this.PhotoToolTip = "Hide Photo";
                            this.ShowOrHidePhotoVisibility = Visibility.Visible;
                            CostCenterMasterModel.PhotoChanged = true;
                            CostCenterMasterModel.Photo = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(CostCenterMasterModel.FilePath));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        public static byte[] ConverImageToBinary(string convertedImage)
        {
            try
            {
                FileStream fs = new FileStream(convertedImage, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                byte[] image = br.ReadBytes((int)fs.Length);
                fs.Read(image, 0, Convert.ToInt32(fs.Length));
                br.Close();
                fs.Close();
                return image;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        public void FocusTextBoxOnLoad(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.Clipboard.Clear();
                var textbox = sender as TextBox;
                if (textbox == null) return;
                textbox.Focus();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }


        }

        private readonly ICommand rowEditEndingOperationCommand;
        public ICommand RowEditEndingOperationCommand { get { return this.rowEditEndingOperationCommand; } }
        private void RowEditEndingOperation(Object selecteditem)
        {
            return;
            try
            {
                DataRowView drRowView;
                drRowView = (DataRowView)selecteditem;
                if (drRowView != null)
                {
                    if (drRowView["OPN_CODE"].ToValueAsString().Trim() != "" || drRowView["UNIT_CODE"].ToValueAsString().Trim() != "")
                    {
                        if (DvOperation[DvOperation.Count - 1]["OPN_CODE"].ToValueAsString().Trim() != "" || DvOperation[DvOperation.Count - 1]["UNIT_CODE"].ToValueAsString().Trim() != "")
                        {
                            DvOperation.AddNew();
                        }
                    }
                    NotifyPropertyChanged("DvOperation");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private readonly ICommand rowEditEndingOutputCommand;
        public ICommand RowEditEndingOutputCommand { get { return this.rowEditEndingOutputCommand; } }
        private void RowEditEndingOutput(Object selecteditem)
        {
            //return;
            try
            {
                DataRowView drRowView;
                drRowView = (DataRowView)selecteditem;
                if (drRowView != null)
                {
                    if (drRowView["OUTPUT"].ToValueAsString().Trim() != "")
                    {
                        if (DvOutput[DvOutput.Count - 1]["OUTPUT"].ToValueAsString().Trim() != "")
                        {
                            DvOutput.AddNew();
                        }
                    }
                    NotifyPropertyChanged("DvOutput");
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }



        public void OnCellEditEndingOperation(object sender, Microsoft.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {
            try
            {
                DataGrid dg = sender as DataGrid;
                if (e.Column.DisplayIndex == 0 || e.Column.DisplayIndex == 1)
                {
                    //MessageBox.Show(e.Row.ToString());
                    //if (e.Column.DisplayIndex == 0)
                    //{
                    //    ProcessDesigner.UserControls.ComboBoxWithSearch ccOper = (ProcessDesigner.UserControls.ComboBoxWithSearch)e.EditingElement;
                    //    if (ccOper.SelectedText == "" || CheckValueExists("OPN_CODE", ccOper.SelectedValue.Trim().ToUpper(), OperCodeMaster.ToTable()) == false)
                    //    {
                    //        OperSelectedRow["OPN_CODE"] = "";
                    //        ccOper.SelectedValue = null;
                    //        ccOper.SelectedText = "";
                    //    }
                    //    if (ccOper.SelectedValue != null)
                    //    {
                    //        if (e.Row.GetIndex() == DvOperation.Count - 1)
                    //        {
                    //            DvOperation.AddNew();
                    //        }
                    //    }

                    //}
                    //else if (e.Column.DisplayIndex == 1)
                    //{
                    //    ProcessDesigner.UserControls.ComboBoxWithSearch ccUnit = (ProcessDesigner.UserControls.ComboBoxWithSearch)e.EditingElement;
                    //    if (ccUnit.SelectedText == "" || CheckValueExists("UNIT_CODE", ccUnit.SelectedValue.Trim().ToUpper(), UnitCodeMaster.ToTable()) == false)
                    //    {
                    //        OperSelectedRow["UNIT_CODE"] = "";
                    //        ccUnit.SelectedValue = null;
                    //        ccUnit.SelectedText = "";
                    //    }
                    //    if (ccUnit.SelectedValue != null)
                    //    {
                    //        if (e.Row.GetIndex() == DvOperation.Count - 1)
                    //        {
                    //            DvOperation.AddNew();
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    if (DvOperation[DvOperation.Count - 1]["OPN_CODE"].ToValueAsString().Trim() != "" || DvOperation[DvOperation.Count - 1]["UNIT_CODE"].ToValueAsString().Trim() != "")
                    {
                        DvOperation.AddNew();
                    }
                    NotifyPropertyChanged("DvOperation");
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

        }

        private bool CheckValueExists(String columnName, string value, DataTable objData)
        {
            DataRow[] drRow;
            drRow = objData.Select(columnName + "='" + value + "'");
            if (drRow.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ChangeLookUpRow()
        {
            try
            {
                int irow;
                CostCenterCodeCombo.Sort = "COST_CENT_CODE";
                irow = CostCenterCodeCombo.Find(CostCenterMasterModel.CostCentCode);
                if (irow > -1)
                {
                    CostCenterCodeCombo[irow]["COST_CENT_DESC"] = CostCenterMasterModel.CostCentDesc;
                    CostCenterCodeCombo[irow]["LOC_CODE"] = CostCenterMasterModel.LocCode;
                    CostCenterCodeCombo[irow]["EFFICIENCY"] = CostCenterMasterModel.Efficiency;
                }
                NotifyPropertyChanged("CostCenterCodeCombo");
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            //COST_CENT_DESC
            //LOC_CODE
            //EFFICIENCY
        }

        private void SetComboHeader()
        {
        }

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

    }
}
