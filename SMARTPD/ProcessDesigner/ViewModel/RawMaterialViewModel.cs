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
    public class RawMaterialViewMode : ViewModelBase
    {
        private RawMaterial bll = null;
        private DDRM_MAST _activeEntity = null;
        private List<RAW_MATERIAL_CLASS> lstRawMaterialClass = null;
        private DataView oldRawMaterialsSize = null;

        public RawMaterialViewMode(UserInformation userInformation, int entityPrimaryKey, OperationMode operationMode)
        {
            try
            {

                bll = new RawMaterial(userInformation);
                EntityPrimaryKey = entityPrimaryKey;
                MandatoryFields = new RawMaterialModel();
                ActiveEntity = new DDRM_MAST();
                OldActiveEntity = new DDRM_MAST();
                //ActiveEntity.IDPK = EntityPrimaryKey;

                //RawMaterialClass = new RAW_MATERIAL_CLASS();

                DropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "RM_CODE", ColumnDesc = "RM Code", ColumnWidth = 80 },
                            new DropdownColumns() { ColumnName = "RM_DESC", ColumnDesc = "RM Desc", ColumnWidth = "1*" },
                            new DropdownColumns() { ColumnName = "LOC_COST", ColumnDesc = "Cost for Domestic(Per Kg)", ColumnWidth = 90 },
                            new DropdownColumns() { ColumnName = "EXP_COST", ColumnDesc = "Cost for Export(Per Kg)", ColumnWidth = 85 }
                        };
                DropDownItemsLocation = new ObservableCollection<DropdownColumns>()
                {
                            new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Location Code", ColumnWidth = 110 },
                            new DropdownColumns() { ColumnName = "LOCATION", ColumnDesc = "Location Description", ColumnWidth = "1*" }
                };
                //ActionPermission.AddNew = true;
                //ActionPermission.Edit = true;
                //ActionPermission.Save = true;
                //ActionPermission.Close = true;
                //ActionPermission = bll.GetUserRights("RAW MATERIAL MASTER");
                GetRights();
                RawMaterialLocationMaster = bll.GetLocations().ToDataTable<DDLOC_MAST>().DefaultView;
                //RawMaterials = bll.GetRawMaterialsByCode().ToDataTable<DDRM_MAST>().DefaultView;
                lstRawMaterialClass = bll.GetRawMaterialClass();

                this.selectedItemChangedCommand = new DelegateCommand(this.SelectDataRow);
                this.addNewCommand = new DelegateCommand(this.AddNewSubmitCommand);
                this.editCommand = new DelegateCommand(this.EditSubmitCommand);
                this.viewCommand = new DelegateCommand(this.ViewSubmitCommand);
                this.deleteCommand = new DelegateCommand<DataRowView>(this.DeleteSubmitCommand);
                this.saveCommand = new DelegateCommand(this.SaveSubmitCommand);
                this.printCommand = new DelegateCommand(this.PrintSubmitCommand);
                this._closeCommand = new DelegateCommand(this.Close);
                this.textBoxValueChanged = new DelegateCommand(this.TextChanged);
                this.textBoxMIN_DIA_ValueChanged = new DelegateCommand<Object>(this.CheckValueIsNull);

                // private readonly ICommand textBoxMIN_DIA_ValueChanged;

                ActionMode = operationMode;

                switch (ActionMode)
                {
                    case OperationMode.AddNew: AddNewSubmitCommand(); break;
                    case OperationMode.Edit: EditSubmitCommand(); break;
                    //case OperationMode.Delete: DeleteSubmitCommand(); break;
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

        private RawMaterialModel _mandatoryFields = null;
        public RawMaterialModel MandatoryFields
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

        private void copyMandatoryFieldsToEntity(RawMaterialModel mandatoryFields)
        {

            if (!ActiveEntity.IsNotNullOrEmpty() || !mandatoryFields.IsNotNullOrEmpty()) return;


            ActiveEntity.RM_CODE = mandatoryFields.RM_CODE;
            ActiveEntity.RM_DESC = mandatoryFields.RM_DESC;

        }


        private bool _addButtonIsEnable = false;
        private bool _editButtonIsEnable = true;
        private bool _saveButtonIsEnable = true;
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

        private bool _rmCodeIsReadonly = false;
        public bool RMCodeIsReadonly
        {
            get { return _rmCodeIsReadonly; }
            set
            {
                this._rmCodeIsReadonly = value;
                NotifyPropertyChanged("RMCodeIsReadonly");
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
            ActionPermission = bll.GetUserRights("RAW MATERIAL MASTER");
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

        private void ChangeRights()
        {
            if (!ActionPermission.AddNew) ActionMode = OperationMode.Edit;
            //if (!ActionPermission.Edit) ActionMode = OperationMode.View;
            //if (!ActionPermission.View) ActionMode = OperationMode.Close;
        }

        private string _class_description;
        public string CLASS_DESCRIPTION
        {
            get { return _class_description; }
            set
            {
                _class_description = value;
                NotifyPropertyChanged("CLASS_DESCRIPTION");
            }
        }

        private int _entityPrimaryKey = 0;
        public int EntityPrimaryKey
        {
            get { return _entityPrimaryKey; }
            set
            {
                _entityPrimaryKey = value;
                NotifyPropertyChanged("EntityPrimaryKey");
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

        public DDRM_MAST ActiveEntity
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

        private DDRM_MAST _oldactiveEntity = null;
        public DDRM_MAST OldActiveEntity
        {
            get
            {
                return _oldactiveEntity;
            }
            set
            {
                _oldactiveEntity = value;
                NotifyPropertyChanged("OldActiveEntity");
            }
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

        private bool _rmdescisfocused = false;
        public bool RMDescIsFocused
        {
            get { return _rmdescisfocused; }
            set
            {
                _rmdescisfocused = value;
                NotifyPropertyChanged("RMDescIsFocused");
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

        private void ClearAll()
        {
            try
            {
                if (MandatoryFields.IsNotNullOrEmpty())
                {
                    InitializeMandatoryFields(MandatoryFields);
                }

                if (ActiveEntity.IsNotNullOrEmpty())
                {
                    ActiveEntity.RM_CODE = string.Empty;
                    ActiveEntity.RM_DESC = string.Empty;
                    ActiveEntity.LOC_COST = 0.00m;
                    ActiveEntity.EXP_COST = 0.00m;
                }

                if (OldActiveEntity.IsNotNullOrEmpty())
                {
                    OldActiveEntity.RM_CODE = string.Empty;
                    OldActiveEntity.RM_DESC = string.Empty;
                    OldActiveEntity.LOC_COST = 0.00m;
                    OldActiveEntity.EXP_COST = 0.00m;
                }

                CLASS_DESCRIPTION = string.Empty;
                SelectedRow = null;
                RawMaterials = bll.GetRawMaterialsByPrimaryKey().ToDataTable<DDRM_MAST>().DefaultView;
                RawMaterialsSize = bll.GetRawMaterialsSize(new DDRM_MAST() { IDPK = -99999 }).ToDataTable<DDRM_SIZE_MAST>().DefaultView;
                oldRawMaterialsSize = RawMaterialsSize.ToTable().Copy().DefaultView;
                RMCodeIsReadonly = false;
                ChangeRights();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private readonly ICommand textBoxMIN_DIA_ValueChanged;

        public ICommand TextBoxMIN_DIA_ValueChanged { get { return this.textBoxMIN_DIA_ValueChanged; } }
        private void CheckValueIsNull(Object currentCell)
        {
            if (!currentCell.IsNotNullOrEmpty()) return;
            Microsoft.Windows.Controls.DataGridCellInfo cellInfo = (Microsoft.Windows.Controls.DataGridCellInfo)currentCell;
            if (!cellInfo.Column.Header.IsNotNullOrEmpty()) return;
            object control = cellInfo.Column.GetCellContent(cellInfo.Item);
            System.Windows.Controls.TextBox textBox = null;

            switch (cellInfo.Column.SortMemberPath)
            {
                case "RM_DIA_MIN":
                case "RM_DIA_MAX":
                    textBox = control as System.Windows.Controls.TextBox;
                    if (!textBox.Text.IsNumeric() && textBox.Text.ToValueAsString().Length > 0) { ShowInformationMessage("Invalid Number"); textBox.Text = null; }
                    break;
            }
        }


        private readonly ICommand textBoxValueChanged;
        public ICommand TextBoxValueChanged { get { return this.textBoxValueChanged; } }
        private void TextChanged()
        {
            CLASS_DESCRIPTION = getClass(ActiveEntity);
        }

        private string getClass(DDRM_MAST paramEntity)
        {
            copyMandatoryFieldsToEntity(MandatoryFields);

            string sReturnValue = "Class Not Available";

            if (!paramEntity.RM_CODE.IsNotNullOrEmpty()) return "";

            if (paramEntity.RM_CODE.IsNotNullOrEmpty())
            {
                string rm_NewCode = paramEntity.RM_CODE.Substring(0, 1).Trim();

                List<RAW_MATERIAL_CLASS> lstRMClass = null;
                lstRMClass = (from row in lstRawMaterialClass.AsEnumerable()
                              where Convert.ToString(row.CODE) == rm_NewCode.Trim()
                              select row).ToList<RAW_MATERIAL_CLASS>();
                if (!lstRMClass.IsNotNullOrEmpty() || lstRMClass.Count == 0)
                {
                    sReturnValue = "Class Not Available";
                }

                if (lstRMClass.IsNotNullOrEmpty() && lstRMClass.Count > 0)
                {
                    sReturnValue = lstRMClass[0].DESCRIPTION.ToValueAsString();
                }

            }
            return sReturnValue;
        }
        private readonly ICommand selectedItemChangedCommand;
        public ICommand SelectedItemChangedCommand { get { return this.selectedItemChangedCommand; } }
        private void SelectDataRow()
        {
            if (_selectedrow.IsNotNullOrEmpty())
            {

                DataTable dt = bll.GetRawMaterialsByPrimaryKey(new DDRM_MAST() { IDPK = -99999 }).ToDataTable<DDRM_MAST>().Clone();
                dt.ImportRow(_selectedrow.Row);

                List<DDRM_MAST> lstEntity = (from row in dt.AsEnumerable()
                                             select new DDRM_MAST()
                                             {
                                                 IDPK = row.Field<string>("IDPK").ToIntValue(),
                                                 RM_CODE = row.Field<string>("RM_CODE"),
                                                 RM_DESC = row.Field<string>("RM_DESC"),
                                                 LOC_COST = row.Field<string>("LOC_COST").ToDecimalValue(),
                                                 EXP_COST = row.Field<string>("EXP_COST").ToDecimalValue(),
                                                 COST_CENT_CODE = row.Field<string>("COST_CENT_CODE"),
                                                 DDRM_SIZE_MAST = null,
                                                 ROWID = row.Field<string>("ROWID").ToGuidValue(),
                                                 DELETE_FLAG = row.Field<string>("DELETE_FLAG").ToBooleanAsString(),
                                                 ENTERED_BY = row.Field<string>("ENTERED_BY"),
                                                 ENTERED_DATE = row.Field<string>("ENTERED_DATE").ToDateTimeValue(),
                                                 UPDATED_BY = row.Field<string>("UPDATED_BY"),
                                                 UPDATED_DATE = row.Field<string>("UPDATED_DATE").ToDateTimeValue()
                                             }).ToList<DDRM_MAST>();
                DDRM_MAST currentEntity = null;
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0) //&& ActionMode != OperationMode.AddNew
                {
                    currentEntity = lstEntity[0];
                    //ActiveEntity.IDPK = currentEntity.IDPK;
                    //ActiveEntity.RM_CODE = currentEntity.RM_CODE;
                    //ActiveEntity.RM_DESC = currentEntity.RM_DESC;
                    //ActiveEntity.LOC_COST = currentEntity.LOC_COST;
                    //ActiveEntity.EXP_COST = currentEntity.EXP_COST;

                    //MandatoryFields.RM_CODE = currentEntity.RM_CODE;
                    //MandatoryFields.RM_DESC = currentEntity.RM_DESC;

                    //RawMaterialsSize = bll.GetRawMaterialsSize(ActiveEntity).ToDataTable<DDRM_SIZE_MAST>().DefaultView;

                    //if (ActiveEntity.RM_CODE.IsNotNullOrEmpty())
                    //{

                    //    CLASS_DESCRIPTION = getClass(ActiveEntity);
                    //}
                }
                else
                {
                    currentEntity = new DDRM_MAST();
                }
                ClearAll();

                switch (ActionMode)
                {
                    case OperationMode.Edit: RMCodeIsReadonly = true; break;
                }

                ActiveEntity.IDPK = currentEntity.IDPK;
                ActiveEntity.RM_CODE = currentEntity.RM_CODE;
                ActiveEntity.RM_DESC = currentEntity.RM_DESC;
                ActiveEntity.LOC_COST = currentEntity.LOC_COST;
                ActiveEntity.EXP_COST = currentEntity.EXP_COST;

                MandatoryFields.RM_CODE = currentEntity.RM_CODE;
                MandatoryFields.RM_DESC = currentEntity.RM_DESC;

                RawMaterialsSize = bll.GetRawMaterialsSize(ActiveEntity).ToDataTable<DDRM_SIZE_MAST>().DefaultView;


                OldActiveEntity.RM_CODE = currentEntity.RM_CODE;
                OldActiveEntity.RM_DESC = currentEntity.RM_DESC;
                OldActiveEntity.LOC_COST = currentEntity.LOC_COST;
                OldActiveEntity.EXP_COST = currentEntity.EXP_COST;

                oldRawMaterialsSize = RawMaterialsSize.ToTable().Copy().DefaultView;

                if (ActiveEntity.RM_CODE.IsNotNullOrEmpty())
                {
                    CLASS_DESCRIPTION = getClass(ActiveEntity);
                }
            }
        }

        private readonly ICommand addNewCommand;
        public ICommand AddNewClickCommand { get { return this.addNewCommand; } }
        private void AddNewSubmitCommand()
        {
            if (!ActionPermission.AddNew) return;
            RMDescIsFocused = true;
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
            AddButtonIsEnable = false;
            EditButtonIsEnable = true;
            RMCodeIsReadonly = false;
            setRights();
            ActiveEntity = new DDRM_MAST();
            ClearAll();
        }

        private readonly ICommand editCommand;
        public ICommand EditClickCommand { get { return this.editCommand; } }
        private void EditSubmitCommand()
        {
            if (!ActionPermission.Edit) return;
            RMDescIsFocused = true;
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

            ActiveEntity = new DDRM_MAST();
            RawMaterialsSize = bll.GetRawMaterialsSize(ActiveEntity).ToDataTable<DDRM_SIZE_MAST>().DefaultView;
            ClearAll();
            AddButtonIsEnable = true;
            EditButtonIsEnable = false;
            RMCodeIsReadonly = true;
            setRights();

        }

        private readonly ICommand deleteCommand;
        public ICommand DeleteClickCommand { get { return this.deleteCommand; } }
        private void DeleteSubmitCommand(DataRowView selectedItem)
        {
            DataRowView row = selectedItem;
            if (RawMaterialsSize.Count > 1)
            {
                row.Delete();
            }
            else if (RawMaterialsSize.Count == 1)
            {
                row["RM_CODE"] = "";
                row["RM_DIA_MIN"] = "";
                row["RM_DIA_MAX"] = "";
                row["LOC_CODE"] = "";

            }

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
            if (!SaveButtonIsEnable) return;
            copyMandatoryFieldsToEntity(MandatoryFields);
            try
            {
                RMDescIsFocused = true;
                List<DDRM_MAST> lstEntity = null;
                List<DDRM_SIZE_MAST> lstAssociationEntity = null;
                bool isExecuted = false;
                if (ActiveEntity.IsNotNullOrEmpty() && !ActiveEntity.RM_CODE.IsNotNullOrEmpty())
                {
                    Message = PDMsg.NotEmpty("Raw Material Code");
                    return;
                }
                if (ActionMode == OperationMode.AddNew)
                {
                    lstEntity = bll.GetRawMaterialsByCode(ActiveEntity.RM_CODE);
                    if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                    {
                        Message = PDMsg.AlreadyExists("Raw Material Code");
                        return;
                    }
                }

                if (ActiveEntity.IsNotNullOrEmpty() && !ActiveEntity.RM_DESC.IsNotNullOrEmpty())
                {
                    Message = PDMsg.NotEmpty("Raw Material Description");
                    return;
                }
                if (ActionMode == OperationMode.AddNew)
                {
                    lstEntity = bll.GetRawMaterialsByDescription(ActiveEntity.RM_DESC);
                    //if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                    //{
                    //    Message = PDMsg.AlreadyExists("Raw Material Description");
                    //    return;
                    //}
                }

                foreach (DataRow drrow in RawMaterialsSize.ToTable().Rows)
                {
                    if (drrow["LOC_CODE"].ToValueAsString().Trim() != "")
                    {
                        drrow["RM_DIA_MIN"] = (drrow["RM_DIA_MIN"].ToValueAsString().Trim() == "" ? "0" : drrow["RM_DIA_MIN"]);
                        drrow["RM_DIA_MAX"] = (drrow["RM_DIA_MAX"].ToValueAsString().Trim() == "" ? "0" : drrow["RM_DIA_MAX"]);
                    }
                }


                switch (ActionMode)
                {
                    case OperationMode.AddNew:

                        lstEntity = bll.GetRawMaterialsByCode(ActiveEntity.RM_CODE);
                        if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                        {
                            Message = PDMsg.AlreadyExists("Raw Material Code");
                            return;
                        }

                        lstEntity = bll.GetRawMaterialsByDescription(ActiveEntity.RM_DESC);
                        //if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                        //{
                        //    Message = PDMsg.AlreadyExists("Raw Material Description");
                        //    return;
                        //}


                        lstAssociationEntity = (from row in RawMaterialsSize.ToTable().AsEnumerable()
                                                where row.Field<string>("RM_DIA_MIN").ToValueAsString().Trim().Length > 0 ||
                                                    row.Field<string>("RM_DIA_MAX").ToValueAsString().Trim().Length > 0 ||
                                                    row.Field<string>("LOC_CODE").ToValueAsString().Trim().Length > 0
                                                select new DDRM_SIZE_MAST()
                                                {
                                                    RM_CODE = row.Field<string>("RM_CODE").IsNotNullOrEmpty() ?
                                                                                    row.Field<string>("RM_CODE") :
                                                                                    ActiveEntity.RM_CODE,
                                                    RM_DIA_MIN = row.Field<string>("RM_DIA_MIN").ToDecimalValue(),
                                                    RM_DIA_MAX = row.Field<string>("RM_DIA_MAX").ToDecimalValue(),
                                                    LOC_CODE = row.Field<string>("LOC_CODE").IsNotNullOrEmpty() ? row.Field<string>("LOC_CODE") : "0",
                                                    ROWID = Guid.NewGuid(),
                                                    IDPK = row.Field<string>("IDPK").ToIntValue(),
                                                    IDFK = ActiveEntity.IDPK,
                                                }).ToList<DDRM_SIZE_MAST>();

                        foreach (DDRM_SIZE_MAST associationEntity in lstAssociationEntity)
                        {
                            if (!associationEntity.RM_DIA_MIN.IsNotNullOrEmpty())
                            {
                                Message = PDMsg.NotEmpty("Minimum Dia");
                                return;

                            }

                            if (!associationEntity.RM_DIA_MAX.IsNotNullOrEmpty())
                            {
                                Message = PDMsg.NotEmpty("Maximum Dia");
                                return;

                            }

                            if (!associationEntity.RM_DIA_MIN.ToValueAsString().IsNumeric())
                            {
                                Message = "Minimum Dia should not be number";
                                return;

                            }

                            if (!associationEntity.RM_DIA_MAX.ToValueAsString().IsNumeric())
                            {
                                Message = "Maximum Dia should not be number";
                                return;

                            }

                            if (!associationEntity.LOC_CODE.IsNotNullOrEmpty())
                            {
                                Message = PDMsg.NotEmpty("Location");
                                return;

                            }

                            if (associationEntity.RM_DIA_MIN.ToValueAsString().Length > 13)
                            {
                                Message = "Minimum Dia should not be exceeds '12' digits";
                                return;

                            }

                            if (associationEntity.RM_DIA_MAX.ToValueAsString().Length > 13)
                            {
                                Message = "Maximum Dia should not be exceeds '12' digits";
                                return;

                            }

                        }

                        //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                        //Progress.Start();

                        foreach (DDRM_SIZE_MAST associationEntity in ActiveEntity.DDRM_SIZE_MAST)
                        {
                            ActiveEntity.DDRM_SIZE_MAST.Remove(associationEntity);
                        }

                        if (ActiveEntity.IDPK == 0)
                            isExecuted = bll.Insert<DDRM_MAST>(new List<DDRM_MAST>() { ActiveEntity });
                        else
                            isExecuted = bll.Update<DDRM_MAST>(new List<DDRM_MAST>() { ActiveEntity });

                        if (isExecuted)
                        {
                            DDRM_MAST parentEntity = (from row in bll.GetRawMaterialsByCode(ActiveEntity.DeepCopy<DDRM_MAST>().RM_CODE)
                                                      select row).FirstOrDefault<DDRM_MAST>();

                            lstAssociationEntity = (from row in RawMaterialsSize.ToTable().AsEnumerable()
                                                    where row.Field<string>("RM_DIA_MIN").ToValueAsString().Trim().Length > 0 ||
                                                        row.Field<string>("RM_DIA_MAX").ToValueAsString().Trim().Length > 0 ||
                                                        row.Field<string>("LOC_CODE").ToValueAsString().Trim().Length > 0
                                                    select new DDRM_SIZE_MAST()
                                                    {
                                                        RM_CODE = row.Field<string>("RM_CODE").IsNotNullOrEmpty() ?
                                                                                        row.Field<string>("RM_CODE") :
                                                                                        parentEntity.RM_CODE,
                                                        RM_DIA_MIN = row.Field<string>("RM_DIA_MIN").ToDecimalValue(),
                                                        RM_DIA_MAX = row.Field<string>("RM_DIA_MAX").ToDecimalValue(),
                                                        LOC_CODE = row.Field<string>("LOC_CODE").IsNotNullOrEmpty() ? row.Field<string>("LOC_CODE") : "0",
                                                        ROWID = Guid.NewGuid(),
                                                        IDPK = row.Field<string>("IDPK").ToIntValue(),
                                                        IDFK = parentEntity.IDPK,
                                                    }).ToList<DDRM_SIZE_MAST>();

                            //foreach (DDRM_SIZE_MAST associationEntity in lstAssociationEntity)
                            //{
                            //    if (!associationEntity.RM_DIA_MIN.IsNotNullOrEmpty())
                            //    {
                            //        Message = "Minimum Dia should not be empty";
                            //        return;

                            //    }

                            //    if (!associationEntity.RM_DIA_MAX.IsNotNullOrEmpty())
                            //    {
                            //        Message = "Maximum Dia should not be empty";
                            //        return;

                            //    }

                            //    if (!associationEntity.RM_DIA_MIN.ToValueAsString().IsNumeric())
                            //    {
                            //        Message = "Minimum Dia should not be number";
                            //        return;

                            //    }

                            //    if (!associationEntity.RM_DIA_MAX.ToValueAsString().IsNumeric())
                            //    {
                            //        Message = "Maximum Dia should not be number";
                            //        return;

                            //    }

                            //    if (!associationEntity.LOC_CODE.IsNotNullOrEmpty())
                            //    {
                            //        Message = "Location should not be empty";
                            //        return;

                            //    }

                            //    if (associationEntity.RM_DIA_MIN.ToValueAsString().Length > 10)
                            //    {
                            //        Message = "Minimum Dia should not be exceeds '10' digits";
                            //        return;

                            //    }

                            //    if (associationEntity.RM_DIA_MAX.ToValueAsString().Length > 10)
                            //    {
                            //        Message = "Maximum Dia should not be exceeds '10' digits";
                            //        return;

                            //    }

                            //}
                            isExecuted = bll.Insert<DDRM_SIZE_MAST>(lstAssociationEntity);
                            //Progress.End();
                            if (isExecuted)
                            {
                                Message = PDMsg.SavedSuccessfully;
                                //AddButtonIsEnable = false;
                                //EditButtonIsEnable = true;
                                //ClearAll();
                                //setRights();
                            }
                        }
                        break;
                    case OperationMode.Edit:

                        lstEntity = bll.GetRawMaterialsByCode(ActiveEntity.RM_CODE);
                        if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 1)
                        {
                            Message = PDMsg.AlreadyExists("Raw Material Code");
                            return;
                        }

                        string oldDesc = lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0 ? lstEntity[0].RM_DESC : "";

                        lstEntity = bll.GetRawMaterialsByDescription(ActiveEntity.RM_DESC);
                        //if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0 && oldDesc != lstEntity[0].RM_DESC)
                        //{
                        //    Message = PDMsg.AlreadyExists("Raw Material Description");
                        //    return;
                        //}

                        lstAssociationEntity = (from row in RawMaterialsSize.ToTable().AsEnumerable()
                                                where row.Field<string>("RM_DIA_MIN").ToValueAsString().Trim().Length > 0 ||
                                                      row.Field<string>("RM_DIA_MAX").ToValueAsString().Trim().Length > 0 ||
                                                      row.Field<string>("LOC_CODE").ToValueAsString().Trim().Length > 0
                                                select new DDRM_SIZE_MAST()
                                                {
                                                    RM_CODE = ActiveEntity.RM_CODE,
                                                    RM_DIA_MIN = row.Field<string>("RM_DIA_MIN").ToDecimalValue(),
                                                    RM_DIA_MAX = row.Field<string>("RM_DIA_MAX").ToDecimalValue(),
                                                    LOC_CODE = row.Field<string>("LOC_CODE").IsNotNullOrEmpty() ? row.Field<string>("LOC_CODE") : "0",
                                                    IDPK = row.Field<string>("IDPK").ToIntValue(),
                                                    IDFK = ActiveEntity.IDPK,

                                                }).ToList<DDRM_SIZE_MAST>();

                        foreach (DDRM_SIZE_MAST associationEntity in lstAssociationEntity)
                        {
                            if (!associationEntity.RM_DIA_MIN.IsNotNullOrEmpty())
                            {
                                Message = PDMsg.NotEmpty("Minimum Dia");
                                return;

                            }

                            if (!associationEntity.RM_DIA_MAX.IsNotNullOrEmpty())
                            {
                                Message = PDMsg.NotEmpty("Maximum Dia");
                                return;

                            }

                            if (!associationEntity.RM_DIA_MIN.ToValueAsString().IsNumeric())
                            {
                                Message = "Minimum Dia should not be number";
                                return;

                            }

                            if (!associationEntity.RM_DIA_MAX.ToValueAsString().IsNumeric())
                            {
                                Message = "Maximum Dia should not be number";
                                return;

                            }

                            if (!associationEntity.LOC_CODE.IsNotNullOrEmpty())
                            {
                                Message = PDMsg.NotEmpty("Location");
                                return;

                            }

                            if (associationEntity.RM_DIA_MIN.ToValueAsString().Length > 13)
                            {
                                Message = "Minimum Dia should not be exceeds '12' digits";
                                return;

                            }

                            if (associationEntity.RM_DIA_MAX.ToValueAsString().Length > 13)
                            {
                                Message = "Maximum Dia should not be exceeds '12' digits";
                                return;

                            }

                        }

                        foreach (DDRM_SIZE_MAST associationEntity in ActiveEntity.DDRM_SIZE_MAST)
                        {
                            ActiveEntity.DDRM_SIZE_MAST.Remove(associationEntity);
                        }
                        //isExecuted = bll.Delete<DDRM_SIZE_MAST>(lstAssociationEntity);
                        //Progress.ProcessingText = PDMsg.ProgressUpdateText;
                        //Progress.Start();

                        isExecuted = bll.DeleteRMSizeMaster(ActiveEntity);
                        isExecuted = bll.Insert<DDRM_SIZE_MAST>(lstAssociationEntity);
                        isExecuted = bll.Update<DDRM_MAST>(new List<DDRM_MAST>() { ActiveEntity });
                        //Progress.End();
                        if (isExecuted)
                        {
                            Message = PDMsg.UpdatedSuccessfully;
                            ClearAll();
                        }

                        //if (isExecuted)
                        //{
                        //    List<DDRM_SIZE_MAST> lstEntity = (from row in RawMaterialsSize.AsEnumerable()
                        //                                      select new DDRM_SIZE_MAST()
                        //                                      {
                        //                                          RM_CODE = ActiveEntity.RM_CODE,
                        //                                          RM_DIA_MIN = row.RM_DIA_MIN,
                        //                                          RM_DIA_MAX = row.RM_DIA_MAX,
                        //                                          LOC_CODE = row.LOC_CODE.IsNotNullOrEmpty() ? row.LOC_CODE : "0",
                        //                                      }).ToList<DDRM_SIZE_MAST>();

                        //    isExecuted = bll.Update<DDRM_SIZE_MAST>(RawMaterialsSize);
                        //    if (isExecuted)
                        //    {
                        //        Message = "Records saved successfully";
                        //    }
                        //}
                        break;
                    case OperationMode.Delete:
                        break;
                }
                OldActiveEntity.RM_CODE = ActiveEntity.RM_CODE;
                OldActiveEntity.RM_DESC = ActiveEntity.RM_DESC;
                OldActiveEntity.LOC_COST = ActiveEntity.LOC_COST;
                OldActiveEntity.EXP_COST = ActiveEntity.EXP_COST;

                oldRawMaterialsSize = RawMaterialsSize.ToTable().Copy().DefaultView;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            if (RawMaterials.IsNotNullOrEmpty())
            {
                RawMaterials.Table.Rows.Clear();
                RawMaterials.Table.Columns.Clear();
                RawMaterials.Table.Dispose();
                RawMaterials.Dispose();
            }

            RawMaterials = bll.GetRawMaterialsByPrimaryKey().ToDataTable<DDRM_MAST>().DefaultView;
            ActionMode = OperationMode.AddNew;
            AddNewSubmitCommand();

            //ChangeRights();

        }

        #region Close Button Action
        public Action CloseAction { get; set; }
        private readonly ICommand _closeCommand;
        public ICommand CloseClickCommand { get { return this._closeCommand; } }
        private void Close()
        {
            //if (IsChangesMade())
            //{
            //    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            //    {
            //        SaveSubmitCommand();
            //        return;
            //    }
            //}

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

                if (ActiveEntity != null && OldActiveEntity != null)
                {
                    if (MandatoryFields.RM_CODE.ToValueAsString() != OldActiveEntity.RM_CODE.ToValueAsString()) result = true;
                    if (MandatoryFields.RM_DESC.ToValueAsString() != OldActiveEntity.RM_DESC.ToValueAsString()) result = true;
                    if (ActiveEntity.LOC_COST != OldActiveEntity.LOC_COST) result = true;
                    if (ActiveEntity.COST_CENT_CODE != OldActiveEntity.COST_CENT_CODE) result = true;
                }

                if (oldRawMaterialsSize != null && RawMaterialsSize != null && !result)
                {
                    result = !RawMaterialsSize.IsEqual(oldRawMaterialsSize);
                }

                return result;
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
        #endregion

        private DataView _rawMaterialsSize;
        public DataView RawMaterialsSize
        {
            get
            {
                return _rawMaterialsSize;
            }
            set
            {
                _rawMaterialsSize = value;
                NotifyPropertyChanged("RawMaterialsSize");
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
                //if (_selectedrow.IsNotNullOrEmpty())
                //{

                //    DataTable dt = bll.GetRawMaterialsByPrimaryKey(new DDRM_MAST() { IDPK = -99999 }).ToDataTable<DDRM_MAST>().Clone();
                //    dt.ImportRow(_selectedrow.Row);

                //    List<DDRM_MAST> lstEntity = (from row in dt.AsEnumerable()
                //                                 where row.Field<string>("RM_CODE") == (MandatoryFields.RM_CODE.ToValueAsString().Trim().Length == 0 ? row.Field<string>("RM_CODE") : MandatoryFields.RM_CODE.ToValueAsString().Trim())
                //                                 select new DDRM_MAST()
                //                                 {
                //                                     IDPK = row.Field<string>("IDPK").ToIntValue(),
                //                                     RM_CODE = row.Field<string>("RM_CODE"),
                //                                     RM_DESC = row.Field<string>("RM_DESC"),
                //                                     LOC_COST = row.Field<string>("LOC_COST").ToDecimalValue(),
                //                                     EXP_COST = row.Field<string>("EXP_COST").ToDecimalValue(),
                //                                     COST_CENT_CODE = row.Field<string>("COST_CENT_CODE"),
                //                                     DDRM_SIZE_MAST = null,
                //                                     ROWID = row.Field<string>("ROWID").ToGuidValue(),
                //                                     DELETE_FLAG = row.Field<string>("DELETE_FLAG").ToBooleanAsString(),
                //                                     ENTERED_BY = row.Field<string>("ENTERED_BY"),
                //                                     ENTERED_DATE = row.Field<string>("ENTERED_DATE").ToDateTimeValue(),
                //                                     UPDATED_BY = row.Field<string>("UPDATED_BY"),
                //                                     UPDATED_DATE = row.Field<string>("UPDATED_DATE").ToDateTimeValue()
                //                                 }).ToList<DDRM_MAST>();
                //    DDRM_MAST currentEntity = null;
                //    if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0) //&& ActionMode != OperationMode.AddNew
                //    {
                //        currentEntity = lstEntity[0];
                //        //ActiveEntity.IDPK = currentEntity.IDPK;
                //        //ActiveEntity.RM_CODE = currentEntity.RM_CODE;
                //        //ActiveEntity.RM_DESC = currentEntity.RM_DESC;
                //        //ActiveEntity.LOC_COST = currentEntity.LOC_COST;
                //        //ActiveEntity.EXP_COST = currentEntity.EXP_COST;

                //        //MandatoryFields.RM_CODE = currentEntity.RM_CODE;
                //        //MandatoryFields.RM_DESC = currentEntity.RM_DESC;

                //        //RawMaterialsSize = bll.GetRawMaterialsSize(ActiveEntity).ToDataTable<DDRM_SIZE_MAST>().DefaultView;

                //        //if (ActiveEntity.RM_CODE.IsNotNullOrEmpty())
                //        //{

                //        //    CLASS_DESCRIPTION = getClass(ActiveEntity);
                //        //}
                //    }
                //    else
                //    {
                //        currentEntity = new DDRM_MAST();
                //    }
                //    ClearAll();
                //    ActiveEntity.IDPK = currentEntity.IDPK;
                //    ActiveEntity.RM_CODE = currentEntity.RM_CODE;
                //    ActiveEntity.RM_DESC = currentEntity.RM_DESC;
                //    ActiveEntity.LOC_COST = currentEntity.LOC_COST;
                //    ActiveEntity.EXP_COST = currentEntity.EXP_COST;

                //    MandatoryFields.RM_CODE = currentEntity.RM_CODE;
                //    MandatoryFields.RM_DESC = currentEntity.RM_DESC;

                //    RawMaterialsSize = bll.GetRawMaterialsSize(ActiveEntity).ToDataTable<DDRM_SIZE_MAST>().DefaultView;

                //    if (ActiveEntity.RM_CODE.IsNotNullOrEmpty())
                //    {

                //        CLASS_DESCRIPTION = getClass(ActiveEntity);
                //    }
                //}

            }
        }

        private DataView _rawMaterials;
        public DataView RawMaterials
        {
            get
            {
                return _rawMaterials;
            }
            set
            {
                _rawMaterials = value;
                NotifyPropertyChanged("RawMaterials");
            }
        }

        private DataView _rawMaterialLocationMaster;
        public DataView RawMaterialLocationMaster
        {
            get
            {
                return _rawMaterialLocationMaster;
            }
            set
            {
                _rawMaterialLocationMaster = value;
                NotifyPropertyChanged("RawMaterialLocationMaster");
            }
        }

        //private System.Data.DataView _locationMaster;
        //public System.Data.DataView LocationMaster
        //{
        //    get
        //    {
        //        return _locationMaster;
        //    }
        //    set
        //    {
        //        _locationMaster = value;
        //        NotifyPropertyChanged("LocationMaster");
        //    }
        //}


    }
}
