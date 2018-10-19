using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcessDesigner.Model;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;

using System.Data;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Windows;
using ProcessDesigner.UserControls;
using System.Collections.ObjectModel;

namespace ProcessDesigner.ViewModel
{
    public class PartNumberConfigViewModel : ViewModelBase
    {
        private PartNumberConfiguration bll = null;
        const string RIGHTS_NAME = "PartNumberConfig";
        public PartNumberConfigViewModel(UserInformation userInformation, int entityPrimaryKey, OperationMode operationMode)
        {
            try
            {
                bll = new PartNumberConfiguration(userInformation);
                ActiveEntity = new PartNumberConfig();
                MandatoryFields = new PartNumberConfigurationModel();
                OldMandatoryFields = new PartNumberConfigurationModel();
                DropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "Code", ColumnDesc = "Code", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "Location_code", ColumnDesc = "Location", ColumnWidth = 76 },
                            new DropdownColumns() { ColumnName = "Description", ColumnDesc = "Category", ColumnWidth = 125 },
                            new DropdownColumns() { ColumnName = "IsObsolete", ColumnDesc = "Mark as Obsolete", ColumnWidth = 81 },
                            new DropdownColumns() { ColumnName = "Prefix", ColumnDesc = "Prefix", ColumnWidth = 67 },
                            new DropdownColumns() { ColumnName = "BeginningNo", ColumnDesc = "Starting No.", ColumnWidth = 85 },
                            new DropdownColumns() { ColumnName = "EndingNo", ColumnDesc = "Ending No.", ColumnWidth = 74 }
                        };

                LocationdropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Loc. Code", ColumnWidth = 90 },
                            new DropdownColumns() { ColumnName = "LOCATION", ColumnDesc = "Loc. Name", ColumnWidth = "1*" }
                        };

                ActiveEntity.ID = entityPrimaryKey;
                ActionPermission = bll.GetUserRights(RIGHTS_NAME);
                ActionPermission.AddNew = true;
                ActionPermission.Edit = true;
                ActionPermission.Save = true;
                ActionPermission.Close = true;

                LocationCustomDropDownDataSource = bll.GetLocationsByCode().ToDataTable<DDLOC_MAST>().DefaultView;

                this.selectedItemChangedCommand = new DelegateCommand(this.SelectDataRow);
                this.addNewCommand = new DelegateCommand(this.AddNewSubmitCommand);
                this.editCommand = new DelegateCommand(this.EditSubmitCommand);
                this.viewCommand = new DelegateCommand(this.ViewSubmitCommand);
                this.deleteCommand = new DelegateCommand(this.DeleteSubmitCommand);
                this.saveCommand = new DelegateCommand(this.SaveSubmitCommand);
                this.printCommand = new DelegateCommand(this.PrintSubmitCommand);
                this._closeCommand = new DelegateCommand(this.Close);

                ActionMode = operationMode;

                switch (ActionMode)
                {
                    case OperationMode.AddNew: AddNewSubmitCommand(); break;
                    case OperationMode.Edit: EditSubmitCommand(); break;
                    case OperationMode.Delete: DeleteSubmitCommand(); break;
                    case OperationMode.View: ViewSubmitCommand(); break;
                    case OperationMode.Print: PrintSubmitCommand(); break;
                    case OperationMode.Save: SaveSubmitCommand(); break;
                    //case OperationMode.Close: CloseSubmitCommand(); break;
                }
                GetRights();
                setRights();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }
        private bool _addButtonIsEnable = false;
        private bool _editButtonIsEnable = true;
        private bool _saveButtonIsEnable = true;

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

        public bool AddButtonIsEnable
        {
            get { return _addButtonIsEnable; }
            set
            {
                this._addButtonIsEnable = value;
                NotifyPropertyChanged("AddButtonIsEnable");
            }
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

        private void GetRights()
        {
            ActionPermission = new RolePermission();
            ActionPermission.Save = true;
            ActionPermission.Print = false;
            ActionPermission.View = true;
            ActionPermission.AddNew = false;
            ActionPermission.Delete = false;
            ActionPermission.Edit = false;
            ActionPermission = bll.GetUserRights("PART NUMBER CONFIG");
            if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
            {
                ActionPermission.Save = false;
            }
        }

        private void setRights()
        {
            if (AddButtonIsEnable) AddButtonIsEnable = ActionPermission.AddNew;
            if (EditButtonIsEnable) EditButtonIsEnable = ActionPermission.Edit;
            if (SaveButtonIsEnable) SaveButtonIsEnable = ActionPermission.Save;
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
                //MessageBox.Show(_message, "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private OperationMode _actionMode = OperationMode.None;
        public OperationMode ActionMode
        {
            get { return _actionMode; }
            set
            {
                _actionMode = value;
                NotifyPropertyChanged("ActionMode");
            }
        }

        private PartNumberConfig _activeEntity = null;
        public PartNumberConfig ActiveEntity
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

        private PartNumberConfigurationModel _mandatoryFields = null;
        public PartNumberConfigurationModel MandatoryFields
        {
            get
            {
                return _mandatoryFields;
            }
            set
            {
                _mandatoryFields = value;
                NotifyPropertyChanged("MandatoryFields");
            }
        }

        private PartNumberConfigurationModel _oldmandatoryFields = null;
        public PartNumberConfigurationModel OldMandatoryFields
        {
            get
            {
                return _oldmandatoryFields;
            }
            set
            {
                _oldmandatoryFields = value;
                NotifyPropertyChanged("OldMandatoryFields");
            }
        }

        private void copyMandatoryFieldsToEntity(PartNumberConfigurationModel mandatoryFields)
        {

            if (!ActiveEntity.IsNotNullOrEmpty() || !mandatoryFields.IsNotNullOrEmpty()) return;


            ActiveEntity.Code = mandatoryFields.Code;
            ActiveEntity.Description = mandatoryFields.Description;
            ActiveEntity.Location_code = mandatoryFields.Location_code;
            ActiveEntity.Prefix = mandatoryFields.Prefix;
            ActiveEntity.BeginningNo = mandatoryFields.BeginningNo;
            ActiveEntity.EndingNo = mandatoryFields.EndingNo;

        }

        private void ChangeRights()
        {
            if (!ActionPermission.AddNew) ActionMode = OperationMode.Edit;
            //if (!ActionPermission.Edit) ActionMode = OperationMode.View;
            //if (!ActionPermission.View) ActionMode = OperationMode.Close;
        }

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
                OnPropertyChanged("DropDownItems");
            }
        }

        private DataView _customDropDownDataSource;
        public DataView CustomDropDownDataSource
        {
            get
            {
                return _customDropDownDataSource;
            }
            set
            {
                _customDropDownDataSource = value;
                NotifyPropertyChanged("CustomDropDownDataSource");
            }
        }

        private void ClearAll()
        {
            try
            {
                if (MandatoryFields.IsNotNullOrEmpty())
                {
                    InitializeMandatoryFields(MandatoryFields);
                    InitializeMandatoryFields(OldMandatoryFields);
                }

                if (ActiveEntity.IsNotNullOrEmpty())
                {
                    ActiveEntity.Code = string.Empty;
                    ActiveEntity.Description = string.Empty;
                    ActiveEntity.Prefix = string.Empty;
                    ActiveEntity.BeginningNo = null;
                    ActiveEntity.EndingNo = null;
                    ActiveEntity.Suffix = string.Empty;
                    ActiveEntity.CurrentValue = string.Empty;
                    ActiveEntity.IsObsolete = false;
                    ActiveEntity.DELETE_FLAG = false;
                }

                if (ActionMode != OperationMode.AddNew) IsObsoleteVisible = Visibility.Visible;
                CustomDropDownDataSource = bll.GetEntitiesByID().ToDataTable<PartNumberConfig>().DefaultView;

                SelectedRow = null;
                ChangeRights();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }


        private DataRowView _selectedRow;
        public DataRowView SelectedRow
        {
            get
            {
                return _selectedRow;
            }

            set
            {
                _selectedRow = value;
                NotifyPropertyChanged("SelectedRow");
            }
        }

        private bool _readOnly;
        public bool ReadOnly
        {
            get
            {
                //return _readOnly;
                return false;
            }
            set
            {
                _readOnly = value;
                NotifyPropertyChanged("ReadOnly");
            }
        }


        private readonly ICommand selectedItemChangedCommand;
        public ICommand SelectedItemChangedCommand { get { return this.selectedItemChangedCommand; } }
        private void SelectDataRow()
        {
            if (SelectedRow.IsNotNullOrEmpty())
            {

                DataTable dt = bll.GetEntitiesByCode(ActiveEntity).ToDataTable<PartNumberConfig>().Clone();
                dt.ImportRow(SelectedRow.Row);

                List<PartNumberConfig> lstEntity = (from row in dt.AsEnumerable()
                                                    select new PartNumberConfig()
                                                    {
                                                        ID = row.Field<string>("ID").ToIntValue(),
                                                        Code = row.Field<string>("Code"),
                                                        Description = row.Field<string>("Description"),
                                                        Location_code = row.Field<string>("location_code"),
                                                        Prefix = row.Field<string>("Prefix"),
                                                        BeginningNo = row.Field<string>("BeginningNo"),
                                                        EndingNo = row.Field<string>("EndingNo"),
                                                        IsObsolete = row.Field<string>("IsObsolete").ToBooleanAsString(),
                                                        DELETE_FLAG = row.Field<string>("DELETE_FLAG").ToBooleanAsString(),
                                                        ENTERED_BY = row.Field<string>("ENTERED_BY"),
                                                        ENTERED_DATE = row.Field<string>("ENTERED_DATE").ToDateTimeValue(),
                                                        UPDATED_BY = row.Field<string>("UPDATED_BY"),
                                                        UPDATED_DATE = row.Field<string>("UPDATED_DATE").ToDateTimeValue()
                                                    }).ToList<PartNumberConfig>();
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                {
                    ActiveEntity = lstEntity[0].DeepCopy<PartNumberConfig>();

                    MandatoryFields.Code = ActiveEntity.Code;
                    MandatoryFields.Description = ActiveEntity.Description;
                    MandatoryFields.Location_code = ActiveEntity.Location_code;
                    MandatoryFields.Prefix = ActiveEntity.Prefix;
                    MandatoryFields.BeginningNo = ActiveEntity.BeginningNo.ToValueAsString();
                    MandatoryFields.EndingNo = ActiveEntity.EndingNo.ToValueAsString();

                    OldMandatoryFields.Code = ActiveEntity.Code;
                    OldMandatoryFields.Description = ActiveEntity.Description;
                    OldMandatoryFields.Location_code = ActiveEntity.Location_code;
                    OldMandatoryFields.Prefix = ActiveEntity.Prefix;
                    OldMandatoryFields.BeginningNo = ActiveEntity.BeginningNo.ToValueAsString();
                    OldMandatoryFields.EndingNo = ActiveEntity.EndingNo.ToValueAsString();
                }
            }
        }

        private readonly ICommand addNewCommand;
        public ICommand AddNewClickCommand { get { return this.addNewCommand; } }
        private void AddNewSubmitCommand()
        {
            if (!ActionPermission.AddNew) return;

            if (IsChangesMade())
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SaveSubmitCommand();
                    return;
                }
            }

            ActionMode = OperationMode.AddNew;
            HasDropDownVisibility = Visibility.Collapsed;
            IsObsoleteVisible = Visibility.Hidden;
            ActiveEntity = new PartNumberConfig();
            ClearAll();
            ReadOnly = false;
            AddButtonIsEnable = false;
            EditButtonIsEnable = true;
            setRights();
        }

        private readonly ICommand editCommand;
        public ICommand EditClickCommand { get { return this.editCommand; } }
        private void EditSubmitCommand()
        {
            if (!ActionPermission.Edit) return;

            if (IsChangesMade())
            {
                if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    SaveSubmitCommand();
                    return;
                }
            }

            ActionMode = OperationMode.Edit;
            HasDropDownVisibility = Visibility.Visible;
            IsObsoleteVisible = Visibility.Visible;
            ActiveEntity = new PartNumberConfig();
            ClearAll();
            ReadOnly = true;
            AddButtonIsEnable = true;
            EditButtonIsEnable = false;
            setRights();
        }

        private readonly ICommand deleteCommand;
        public ICommand DeleteClickCommand { get { return this.deleteCommand; } }
        private void DeleteSubmitCommand()
        {
            if (!ActionPermission.Delete) return;
            ActionMode = OperationMode.Delete;
        }

        private readonly ICommand viewCommand;
        public ICommand ViewClickCommand { get { return this.viewCommand; } }
        private void ViewSubmitCommand()
        {
            if (!ActionPermission.View) return;
            ActionMode = OperationMode.View;
        }

        private readonly ICommand printCommand;
        public ICommand PrintClickCommand { get { return this.printCommand; } }
        private void PrintSubmitCommand()
        {
            if (!ActionPermission.Print) return;
            ActionMode = OperationMode.Print;
        }

        private readonly ICommand saveCommand;
        public ICommand SaveClickCommand { get { return this.saveCommand; } }
        private void SaveSubmitCommand()
        {
            bool changed = false;
            IsDefaultFocused = true;
            if (!SaveButtonIsEnable) return;
            copyMandatoryFieldsToEntity(MandatoryFields);
            try
            {
                if (!ActiveEntity.IsNotNullOrEmpty()) return;
                //if (!ActiveEntity.Code.IsNotNullOrEmpty())
                //{
                //    Message = PDMsg.NotEmpty("Code");
                //    return;
                //}

                if (!ActiveEntity.Description.IsNotNullOrEmpty())
                {
                    Message = PDMsg.NotEmpty("Category");
                    return;
                }
                if (!ActiveEntity.Location_code.IsNotNullOrEmpty())
                {
                    Message = PDMsg.NotEmpty("Location");
                    return;
                }
                if (!ActiveEntity.Prefix.IsNotNullOrEmpty())
                {
                    Message = PDMsg.NotEmpty("Prefix");
                    return;
                }
                if (!MandatoryFields.BeginningNo.IsNotNullOrEmpty())
                {
                    Message = PDMsg.NotEmpty("Starting No.");
                    return;
                }

                if (!MandatoryFields.EndingNo.IsNotNullOrEmpty())
                {
                    Message = PDMsg.NotEmpty("Ending No.");
                    return;
                }

                if ((Convert.ToInt64(ActiveEntity.BeginningNo) <= 0))
                {
                    Message = "Starting No. should be greater than zero";
                    return;
                }

                if ((Convert.ToInt64(ActiveEntity.EndingNo) <= 0))
                {
                    Message = "Ending No. should be greater than zero";
                    return;
                }
                if (MandatoryFields.BeginningNo.ToValueAsString().Trim().Length != MandatoryFields.EndingNo.ToValueAsString().Trim().Length)
                {
                    //Message = "Starting No. and Ending No. should be equal length".TrimWhiteSpace();
                    //return;
                }

                if (ActiveEntity.BeginningNo.ToIntValue() > Int32.MaxValue)
                {
                    Message = "Starting No. should be less than " + Int32.MaxValue.ToValueAsString();
                    return;
                }

                if (ActiveEntity.EndingNo.ToIntValue() > Int32.MaxValue)
                {
                    Message = "Ending No. should be less than " + Int32.MaxValue.ToValueAsString();
                    return;
                }

                if (Convert.ToInt64(ActiveEntity.EndingNo) <= Convert.ToInt64(ActiveEntity.BeginningNo))
                {
                    Message = "Ending No. should be greater than Starting No.";
                    return;
                }

                //ActiveEntity.BeginningNo = ActiveEntity.BeginningNo.ToIntValue().ToString("000000");
                //ActiveEntity.EndingNo = ActiveEntity.EndingNo.ToIntValue().ToString("000000");
                MandatoryFields.BeginningNo = ValidateCodeLengthMin(MandatoryFields.BeginningNo, ref changed);
                MandatoryFields.EndingNo = ValidateCodeLengthMax(MandatoryFields.EndingNo, ref changed);
                ActiveEntity.BeginningNo = MandatoryFields.BeginningNo;
                ActiveEntity.EndingNo = MandatoryFields.EndingNo;
                if (Convert.ToInt64(ActiveEntity.EndingNo) <= Convert.ToInt64(ActiveEntity.BeginningNo))
                {
                    Message = "Ending No. should be greater than Starting No.";
                    return;
                }
                if (changed == true)
                {
                    Message = "Part Number should be 6 or 9 digits. \nRefer updated range " + ActiveEntity.BeginningNo + " - " + ActiveEntity.EndingNo;
                }
                List<PartNumberConfig> lstEntity = null;

                bool isExecuted = false;

                switch (ActionMode)
                {
                    case OperationMode.AddNew:

                        ActiveEntity.Code = "PNC" + Convert.ToString((from row in bll.DB.PartNumberConfig
                                                                      select row).ToList<PartNumberConfig>().Count());

                        //lstEntity = bll.GetEntitiesByCode(ActiveEntity);
                        //if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                        //{
                        //    Message = PDMsg.AlreadyExists("Code");
                        //    break;
                        //}

                        lstEntity = bll.GetEntitiesByDescription(ActiveEntity);
                        lstEntity = (from row in lstEntity.AsEnumerable()
                                     where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && Convert.ToBoolean(Convert.ToInt16(row.IsObsolete)) == false
                                     select row).ToList<PartNumberConfig>();
                        if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                        {
                            Message = PDMsg.AlreadyExists("Category");
                            return;
                        }
                        ActiveEntity.IsDefault = false;
                        ActiveEntity.AlertMe = 10;
                        //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                        //Progress.Start();
                        isExecuted = bll.Insert<PartNumberConfig>(new List<PartNumberConfig>() { ActiveEntity });
                        //Progress.End();
                        if (isExecuted)
                        {
                            Message = PDMsg.SavedSuccessfully;
                        }

                        break;
                    case OperationMode.Edit:

                        lstEntity = bll.GetEntitiesByCode(ActiveEntity);
                        if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 1)
                        {
                            Message = PDMsg.AlreadyExists("Code");
                            break;
                        }

                        lstEntity = bll.GetEntitiesByDescription(ActiveEntity);

                        lstEntity = (from row in lstEntity.AsEnumerable()
                                     where Convert.ToBoolean(Convert.ToInt16(row.DELETE_FLAG)) == false && Convert.ToBoolean(Convert.ToInt16(row.IsObsolete)) == false
                                     select row).ToList<PartNumberConfig>();
                        if ((lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 1) || (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0 && lstEntity[0].Description == ActiveEntity.Description && lstEntity[0].ID != ActiveEntity.ID))
                        {
                            Message = PDMsg.AlreadyExists("Category");
                            return;
                        }

                        Progress.ProcessingText = PDMsg.ProgressUpdateText;
                        Progress.Start();

                        isExecuted = bll.Update<PartNumberConfig>(new List<PartNumberConfig>() { ActiveEntity });
                        Progress.End();
                        if (isExecuted)
                        {
                            Message = PDMsg.UpdatedSuccessfully;
                            AddButtonIsEnable = false;
                            EditButtonIsEnable = true;
                            setRights();
                        }

                        break;
                    case OperationMode.Delete:
                        //if (lstRMM.IsNotNullOrEmpty() && lstRMM.Count == 0)
                        //{
                        //    Message = "Raw Material Code does not exists";
                        //    break;
                        //}

                        //isExecuted = bll.Delete<PartNumberConfig>(new List<PartNumberConfig>() { ActiveEntity });
                        //if (isExecuted)
                        //{
                        //    List<DDRM_SIZE_MAST> lstEntity = (from row in RawMaterialsSize.AsEnumerable()
                        //                                      select new DDRM_SIZE_MAST()
                        //                                      {
                        //                                          Code = ActiveEntity.Code,
                        //                                          RM_DIA_MIN = row.RM_DIA_MIN,
                        //                                          RM_DIA_MAX = row.RM_DIA_MAX,
                        //                                          LOC_CODE = row.LOC_CODE.IsNotNullOrEmpty() ? row.LOC_CODE : "0",
                        //                                      }).ToList<DDRM_SIZE_MAST>();

                        //    isExecuted = bll.Delete<DDRM_SIZE_MAST>(lstEntity);
                        //    if (isExecuted)
                        //    {
                        //        Message = "Records deleted successfully";
                        //    }
                        //}
                        break;
                }

                OldMandatoryFields.Code = ActiveEntity.Code;
                OldMandatoryFields.Description = ActiveEntity.Description;
                OldMandatoryFields.Location_code = ActiveEntity.Location_code;
                OldMandatoryFields.Prefix = ActiveEntity.Prefix;
                OldMandatoryFields.BeginningNo = ActiveEntity.BeginningNo.ToValueAsString();
                OldMandatoryFields.EndingNo = ActiveEntity.EndingNo.ToValueAsString();
                ActionMode = OperationMode.AddNew;
                HasDropDownVisibility = Visibility.Collapsed;
                IsObsoleteVisible = Visibility.Hidden;
                ActiveEntity = new PartNumberConfig();
                ClearAll();
                ReadOnly = false;
                AddButtonIsEnable = false;
                EditButtonIsEnable = true;
                setRights();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            CustomDropDownDataSource = bll.GetEntitiesByID().ToDataTable<PartNumberConfig>().DefaultView;
            //ActionMode = OperationMode.AddNew;
            //AddNewSubmitCommand();
            //ChangeRights();

        }

        private string ValidateCodeLengthMin(string sNo, ref bool changed)
        {
            try
            {
                //MandatoryFields.Prefix 
                //if(MandatoryFields.Prefix.Length + )
                string finalSno = "";
                finalSno = sNo;
                if (MandatoryFields.Prefix.Trim().Length + sNo.Trim().Length < 6)
                {
                    finalSno = sNo.PadLeft(5 - MandatoryFields.Prefix.Trim().Length, '0');
                    if (sNo != finalSno)
                    {
                        changed = true;
                    }
                }
                else if (MandatoryFields.Prefix.Trim().Length + sNo.Trim().Length > 6 && MandatoryFields.Prefix.Trim().Length + sNo.Trim().Length < 9)
                {
                    finalSno = sNo.PadLeft(8 - MandatoryFields.Prefix.Trim().Length, '0');
                    if (sNo != finalSno)
                    {
                        changed = true;
                    }
                }
                else if (MandatoryFields.Prefix.Trim().Length + sNo.Trim().Length > 9)
                {
                    finalSno = sNo.Substring(0, 9 - MandatoryFields.Prefix.Trim().Length);
                    changed = true;
                }
                return finalSno;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private string ValidateCodeLengthMax(string sNo, ref bool changed)
        {
            try
            {
                //MandatoryFields.Prefix 
                //if(MandatoryFields.Prefix.Length + )
                string finalSno = "";
                finalSno = sNo;
                if (MandatoryFields.Prefix.Trim().Length + MandatoryFields.BeginningNo.Length == 6)
                {
                    if (MandatoryFields.Prefix.Trim().Length + sNo.Length < 6)
                    {
                        finalSno = sNo.PadLeft(5 - MandatoryFields.Prefix.Trim().Length, '0');
                        if (sNo != finalSno)
                        {
                            changed = true;
                        }
                    }
                    else if (MandatoryFields.Prefix.Trim().Length + sNo.Length == 6)
                    {
                        finalSno = sNo;
                    }
                    else
                    {
                        finalSno = sNo.Substring(0, 6 - MandatoryFields.Prefix.Trim().Length);
                        changed = true;
                    }
                }
                else if (MandatoryFields.Prefix.Trim().Length + MandatoryFields.BeginningNo.Length == 9)
                {
                    if (MandatoryFields.Prefix.Trim().Length + sNo.Length < 9)
                    {
                        finalSno = sNo.PadLeft(8 - MandatoryFields.Prefix.Trim().Length, '0');
                        if (sNo != finalSno)
                        {
                            changed = true;
                        }
                    }
                    else if (MandatoryFields.Prefix.Trim().Length + sNo.Length == 9)
                    {
                        finalSno = sNo;
                    }
                    else
                    {
                        finalSno = sNo.Substring(0, 9 - MandatoryFields.Prefix.Trim().Length);
                        if (sNo != finalSno)
                        {
                            changed = true;
                        }
                    }
                }
                else
                {
                    if (MandatoryFields.Prefix.Trim().Length + sNo.Length < 6)
                    {
                        finalSno = sNo.PadLeft(5 - MandatoryFields.Prefix.Trim().Length, '0');
                        if (sNo != finalSno)
                        {
                            changed = true;
                        }
                    }
                    else if (MandatoryFields.Prefix.Trim().Length + sNo.Length > 6 && MandatoryFields.Prefix.Trim().Length + sNo.Length < 9)
                    {
                        finalSno = sNo.PadLeft(8 - MandatoryFields.Prefix.Trim().Length, '0');
                        if (sNo != finalSno)
                        {
                            changed = true;
                        }
                    }
                    else if (MandatoryFields.Prefix.Trim().Length + sNo.Trim().Length > 9)
                    {
                        finalSno = sNo.Substring(0, 9 - MandatoryFields.Prefix.Trim().Length);
                        changed = true;
                    }
                }
                return finalSno;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private bool _isDefaultFocused = false;
        public bool IsDefaultFocused
        {
            get { return _isDefaultFocused; }
            set
            {
                _isDefaultFocused = value;
                NotifyPropertyChanged("IsDefaultFocused");
            }
        }

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        #region Close Button Action
        public Action CloseAction { get; set; }
        private readonly ICommand _closeCommand;
        public ICommand CloseClickCommand { get { return this._closeCommand; } }
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
                WPF.MDI.ClosingEventArgs closingev;
                closingev = (WPF.MDI.ClosingEventArgs)e;
                if (IsChangesMade())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        SaveSubmitCommand();
                        closingev.Cancel = true;
                        e = closingev;
                        return;
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

        public void CloseMethodWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (IsChangesMade())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        SaveSubmitCommand();
                        return;
                    }
                }

                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                {
                    e.Cancel = true;
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

                if (MandatoryFields != null && OldMandatoryFields != null)
                {
                    if (MandatoryFields.BeginningNo != OldMandatoryFields.BeginningNo) result = true;
                    if (MandatoryFields.Code != OldMandatoryFields.Code) result = true;
                    if (MandatoryFields.Description != OldMandatoryFields.Description) result = true;
                    if (MandatoryFields.Location_code != OldMandatoryFields.Location_code) result = true;
                    if (MandatoryFields.EndingNo != OldMandatoryFields.EndingNo) result = true;
                    if (MandatoryFields.Prefix != OldMandatoryFields.Prefix) result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        #endregion

        #region For Location DropDown"
        private DataView _locationLocationCustomDropDownDataSource;
        public DataView LocationCustomDropDownDataSource
        {
            get
            {
                return _locationLocationCustomDropDownDataSource;
            }
            set
            {
                _locationLocationCustomDropDownDataSource = value;
                NotifyPropertyChanged("LocationCustomDropDownDataSource");
            }
        }

        private DataRow _locationSelectedRow;
        public DataRow LocationSelectedRow
        {
            get
            {
                return _locationSelectedRow;
            }

            set
            {
                _locationSelectedRow = value;
                if (_locationSelectedRow.IsNotNullOrEmpty())
                {

                    DataTable dt = bll.GetLocationsByCode(new DDLOC_MAST() { LOC_CODE = ActiveEntity.Location_code }).ToDataTable<DDLOC_MAST>().Clone();
                    dt.ImportRow(_locationSelectedRow);

                    List<DDLOC_MAST> lstEntity = (from row in dt.AsEnumerable()
                                                  select new DDLOC_MAST()
                                                  {
                                                      LOC_CODE = row.Field<string>("LOC_CODE"),
                                                      LOCATION = row.Field<string>("LOCATION"),
                                                      ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                                      DELETE_FLAG = row.Field<string>("DELETE_FLAG").ToBooleanAsString(),
                                                      ENTERED_BY = row.Field<string>("ENTERED_BY"),
                                                      ENTERED_DATE = row.Field<string>("ENTERED_DATE").ToDateTimeValue(),
                                                      UPDATED_BY = row.Field<string>("UPDATED_BY"),
                                                      UPDATED_DATE = row.Field<string>("UPDATED_DATE").ToDateTimeValue()
                                                  }).ToList<DDLOC_MAST>();
                    if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                    {
                        DDLOC_MAST currentEntity = lstEntity[0];
                        ActiveEntity.Location_code = currentEntity.LOC_CODE;
                    }
                }

            }
        }


        private Visibility _locationHasDropDownVisibility = Visibility.Visible;
        public Visibility LocationHasDropDownVisibility
        {
            get { return _locationHasDropDownVisibility; }
            set
            {
                _locationHasDropDownVisibility = Visibility.Visible;
                NotifyPropertyChanged("LocationHasDropDownVisibility");
            }
        }



        private Visibility _isObsoleteVisible = Visibility.Collapsed;
        public Visibility IsObsoleteVisible
        {
            get { return _isObsoleteVisible; }
            set
            {
                if (ActionMode != OperationMode.AddNew)
                    _isObsoleteVisible = Visibility.Visible;
                else
                    _isObsoleteVisible = value;
                NotifyPropertyChanged("IsObsoleteVisible");
            }
        }

        private ObservableCollection<DropdownColumns> _locationdropDownItems;
        public ObservableCollection<DropdownColumns> LocationdropDownItems
        {
            get
            {
                return _locationdropDownItems;
            }
            set
            {
                _locationdropDownItems = value;
                OnPropertyChanged("LocationdropDownItems");
            }
        }
        #endregion
    }
}
