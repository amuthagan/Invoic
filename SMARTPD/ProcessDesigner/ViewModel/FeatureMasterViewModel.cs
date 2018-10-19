using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using System.Windows.Input;
using ProcessDesigner.BLL;
using System.Windows;
using System.ComponentModel;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.Common;
using System.Data;
using ProcessDesigner.UserControls;
using System.Collections.ObjectModel;

namespace ProcessDesigner.Model
{
    class FeatureMasterViewModel : ViewModelBase
    {
        private FeatureMasterModel _featureMaster;
        private FeatureMasterBll _featureMasterbll;
        private DataTable delCharacteristics = new DataTable();
        private readonly ICommand _onAddCommand;
        public ICommand OnAddCommand { get { return this._onAddCommand; } }

        private readonly ICommand _onEditViewCommand;
        public ICommand OnEditViewCommand { get { return this._onEditViewCommand; } }
        private readonly ICommand _deleteCommandChar;
        public ICommand DeleteCommandChar { get { return this._deleteCommandChar; } }

        private readonly ICommand _onSaveCommand;
        public ICommand OnSaveCommand { get { return this._onSaveCommand; } }

        private readonly ICommand _onCloseCommand;
        public ICommand OnCloseCommand { get { return this._onCloseCommand; } }

        private bool _addButtonIsEnable = false;
        private bool _editButtonIsEnable = true;
        private bool _saveButtonIsEnable = true;
        private bool _deleteButtonIsEnable = true;
        private bool isSaved = false;
        public Action CloseAction { get; set; }

        public FeatureMasterViewModel(UserInformation userInformation)
        {
            this.FeatureMaster = new FeatureMasterModel();
            _featureMasterbll = new FeatureMasterBll(userInformation);
            this.selectChangeComboCommand = new DelegateCommand(this.SelectDataRow);
            this.selectChangeComboCommandOper = new DelegateCommand(this.SelectDataRowOper);
            this._deleteCommandChar = new DelegateCommand<System.Windows.Controls.DataGrid>(this.DeleteChar);
            this.gridRowEndEditing = new DelegateCommand<Object>(this.GridEditEnding);
            this.gridAddingNewItem = new DelegateCommand(this.GridAddingNew);
            this._onAddCommand = new DelegateCommand(this.Add);
            this._onEditViewCommand = new DelegateCommand(this.Edit);
            this._onSaveCommand = new DelegateCommand<DataView>(this.Save);
            this._onCloseCommand = new DelegateCommand(this.Close);
            this.deleteCommand = new DelegateCommand<DataRowView>(this.DeleteSubmitCommand);
            FeatureMaster.FeatureCode = string.Empty;
            FeatureMaster.Feature = string.Empty;
            FeatureMaster.OperationCode = string.Empty;
            FeatureMaster.Operations = string.Empty;

            SetdropDownItems();
            ButtonEnable = Visibility.Collapsed;
            ButtonVisibleOper = Visibility.Visible;
            FeatureMaster.FeatureCode = _featureMasterbll.GenerateFeatuerCode();
            FeatureMaster.OperationCodeDetails = _featureMasterbll.GetOpertionMaster();
            FeatureMaster.CharacteristicsMasterDetails = _featureMasterbll.GetCharactersiticsMasterGrid(FeatureMaster.OperationCode, FeatureMaster.FeatureCode);
            GetRights();
            //   AddButtonIsEnable = true;
            setRights();
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
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
        private MessageBoxResult ShowWarningMessage(string _showMessage, MessageBoxButton messageBoxButton)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, messageBoxButton, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }
        private void DeleteChar(System.Windows.Controls.DataGrid dgvGridMeasuring)
        {

            try
            {
                if (dgvGridMeasuring != null)
                {
                    if (dgvGridMeasuring.SelectedItems.Count == 0) return;

                    if (MessageBox.Show(dgvGridMeasuring.SelectedItems.Count + "  records will be deleted. Continue? ", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    {
                        FocusButton = true;
                        return;
                    }
                    DataView dvTempMeasuringTech = new DataView();
                    dvTempMeasuringTech = FeatureMaster.CharacteristicsMasterDetails.Table.Copy().DefaultView;
                    foreach (DataRowView item in dgvGridMeasuring.SelectedItems)
                    {
                        dvTempMeasuringTech.RowFilter = "SNO='" + item["SNO"].ToString() + "' and MEASURING_TECHNIQUE='" + item["MEASURING_TECHNIQUE"].ToString() + "'";
                        if (dvTempMeasuringTech.Count > 0)
                        {
                            dvTempMeasuringTech[0].Delete();
                            dvTempMeasuringTech.RowFilter = string.Empty;
                        }
                        dvTempMeasuringTech.RowFilter = string.Empty;
                    }
                    FeatureMaster.CharacteristicsMasterDetails = dvTempMeasuringTech;
                    FeatureMaster.CharacteristicsMasterDetails.Table.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            try
            {
                DataRowView newRow;
                FeatureMaster.CharacteristicsMasterDetails.Table.AcceptChanges();
                if (FeatureMaster.CharacteristicsMasterDetails.Count == 0)
                {
                    DataView charactMast = FeatureMaster.CharacteristicsMasterDetails;
                    newRow = charactMast.AddNew();
                    newRow.BeginEdit();
                    newRow["SNO"] = FeatureMaster.CharacteristicsMasterDetails.Count;
                    newRow.EndEdit();
                    FeatureMaster.CharacteristicsMasterDetails = charactMast;
                }
                if (FeatureMaster.CharacteristicsMasterDetails.Count > 1)
                {
                    if (FeatureMaster.CharacteristicsMasterDetails.IsNotNullOrEmpty())
                    {
                        for (int i = 0; i < FeatureMaster.CharacteristicsMasterDetails.Count; i++)
                        {
                            FeatureMaster.CharacteristicsMasterDetails[i]["SNO"] = i + 1;
                        }
                        FeatureMaster.CharacteristicsMasterDetails.Sort = "SNO ASC";
                        FeatureMaster.CharacteristicsMasterDetails.Table.AcceptChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
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
        private bool _txtReadOnlyOper;
        public bool TxtReadOnlyOper
        {
            get { return _txtReadOnlyOper; }
            set
            {
                this._txtReadOnlyOper = value;
                NotifyPropertyChanged("TxtReadOnlyOper");
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
            ActionPermission.AddNew = false;
            ActionPermission.Delete = false;
            ActionPermission.Edit = false;
            ActionPermission = _featureMasterbll.GetUserRights("FEATURE MASTER");
            if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
            {
                ActionPermission.Save = false;
            }
        }

        private void setRights()
        {
            if (AddButtonIsEnable) AddButtonIsEnable = ActionPermission.AddNew;
            if (EditButtonIsEnable) EditButtonIsEnable = ActionPermission.Edit;
            if (DeleteButtonIsEnable) DeleteButtonIsEnable = ActionPermission.Delete;
            if (SaveButtonIsEnable) SaveButtonIsEnable = ActionPermission.Save;
        }

        public FeatureMasterModel FeatureMaster
        {
            get
            {
                return _featureMaster;
            }
            set
            {
                this._featureMaster = value;
                NotifyPropertyChanged("FeatureMaster");
            }
        }
        private bool _readOnlyFeature;
        public bool ReadOnlyFeature
        {
            get { return _readOnlyFeature; }
            set
            {
                this._readOnlyFeature = value;
                NotifyPropertyChanged("ReadOnlyFeature");
            }
        }
        private readonly ICommand selectChangeComboCommand;
        public ICommand SelectChangeComboCommand { get { return this.selectChangeComboCommand; } }
        private void SelectDataRow()
        {
            if (FeatureMaster.SelectedRowFeat != null)
            {
                FeatureMaster.Feature = FeatureMaster.SelectedRowFeat["FEATURE_DESC"].ToString();
                FeatureMaster.FeatureCode = FeatureMaster.SelectedRowFeat["FEATURE_CODE"].ToString();
                FeatureMaster.CharacteristicsMasterDetails = _featureMasterbll.GetCharactersiticsMasterGrid(FeatureMaster.OperationCode, FeatureMaster.FeatureCode);
                if (FeatureMaster.CharacteristicsMasterDetails.IsNotNullOrEmpty() && EditButtonIsEnable == false)
                {
                    DataView charactMast = FeatureMaster.CharacteristicsMasterDetails;
                    DataRowView newRow = charactMast.AddNew();
                    newRow.BeginEdit();
                    newRow["SNO"] = FeatureMaster.CharacteristicsMasterDetails.Count;
                    newRow.EndEdit();
                    FeatureMaster.CharacteristicsMasterDetails = charactMast;
                }
            }
        }
        private readonly ICommand deleteCommand;
        public ICommand DeleteClickCommand { get { return this.deleteCommand; } }
        private void DeleteSubmitCommand(DataRowView selectedItem)
        {
            DataRowView row = selectedItem;
            if (FeatureMaster.CharacteristicsMasterDetails.Count > 1)
            {
                row.Delete();
            }
            else if (FeatureMaster.CharacteristicsMasterDetails.Count == 1)
            {
                row["SNO"] = FeatureMaster.CharacteristicsMasterDetails.Count;
                row["MEASURING_TECHNIQUE"] = "";
                row["SAMPLE_SIZE"] = "";
                row["SAMPLE_FREQUENCY"] = "";
                row["CONTROL_METHOD"] = "";
                row["REACTION_PLAN"] = "";

            }

        }

        private readonly ICommand selectChangeComboCommandOper;
        public ICommand SelectChangeComboCommandOper { get { return this.selectChangeComboCommandOper; } }
        private void SelectDataRowOper()
        {
            if (FeatureMaster.SelectedRow != null)
            {

                FeatureMaster.Operations = FeatureMaster.SelectedRow["OPER_DESC"].ToString();
                FeatureMaster.OperationCode = FeatureMaster.SelectedRow["OPER_CODE"].ToString();
                if (EditButtonIsEnable == false)
                {
                    FeatureMaster.FeatureCode = "";
                    FeatureMaster.Feature = "";
                }
                FeatureMaster.FeatureCodeDetails = _featureMasterbll.GetFeatureMaster(FeatureMaster.OperationCode);
                FeatureMaster.CharacteristicsMasterDetails = _featureMasterbll.GetCharactersiticsMasterGrid(FeatureMaster.OperationCode, FeatureMaster.FeatureCode);
                delCharacteristics = FeatureMaster.CharacteristicsMasterDetails.ToTable().Copy().Clone();
                if (FeatureMaster.CharacteristicsMasterDetails.Count == 0 && AddButtonIsEnable == false)
                {
                    DataView charactMast = FeatureMaster.CharacteristicsMasterDetails;
                    DataRowView newRow = charactMast.AddNew();
                    newRow.BeginEdit();
                    newRow["SNO"] = FeatureMaster.CharacteristicsMasterDetails.Count;
                    newRow.EndEdit();
                    FeatureMaster.CharacteristicsMasterDetails = charactMast;
                }
            }

        }

        private readonly ICommand gridRowEndEditing;
        public ICommand GridRowEndEditing { get { return this.gridRowEndEditing; } }
        private void GridEditEnding(object selecteditem)
        {
            try
            {
                if (FeatureMaster.CharacteristicsMasterDetails.Count > 0)
                {
                    DataView charactMast = FeatureMaster.CharacteristicsMasterDetails;
                    //DataRowView newRow = charactMast.AddNew();
                    //newRow["SNO"] = FeatureMaster.CharacteristicsMasterDetails.Count;
                    //newRow.EndEdit();
                    charactMast[FeatureMaster.CharacteristicsMasterDetails.Count - 1]["SNO"] = FeatureMaster.CharacteristicsMasterDetails.Count;
                    FeatureMaster.CharacteristicsMasterDetails = charactMast;
                    NotifyPropertyChanged("FeatureMaster");
                    //DataRow newrow = FeatureMaster.CharacteristicsMasterDetails.Table.NewRow();
                    //FeatureMaster.CharacteristicsMasterDetails.Table.Rows.InsertAt(newrow, FeatureMaster.CharacteristicsMasterDetails.Count);

                    if (charactMast[FeatureMaster.CharacteristicsMasterDetails.Count - 1]["MEASURING_TECHNIQUE"].ToString().IsNotNullOrEmpty() || charactMast[FeatureMaster.CharacteristicsMasterDetails.Count - 1]["SAMPLE_SIZE"].ToString().IsNotNullOrEmpty() || charactMast[FeatureMaster.CharacteristicsMasterDetails.Count - 1]["SAMPLE_FREQUENCY"].ToString().IsNotNullOrEmpty() || charactMast[FeatureMaster.CharacteristicsMasterDetails.Count - 1]["CONTROL_METHOD"].ToString().IsNotNullOrEmpty() || charactMast[FeatureMaster.CharacteristicsMasterDetails.Count - 1]["REACTION_PLAN"].ToString().IsNotNullOrEmpty())
                    {
                        DataRowView newRow = charactMast.AddNew();
                        newRow.BeginEdit();
                        newRow["SNO"] = FeatureMaster.CharacteristicsMasterDetails.Count;
                        newRow.EndEdit();
                        FeatureMaster.CharacteristicsMasterDetails = charactMast;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private readonly ICommand gridAddingNewItem;
        public ICommand GridAddingNewItem { get { return this.gridAddingNewItem; } }
        private void GridAddingNew()
        {
            try
            {
                //if (FeatureMaster.CharacteristicsMasterDetails.Count > 0)
                //{
                //    DataView charactMast = FeatureMaster.CharacteristicsMasterDetails;
                //    DataRowView newRow = charactMast.AddNew();
                //    newRow["SNO"] = FeatureMaster.CharacteristicsMasterDetails.Count + 1;
                //    newRow.EndEdit();
                //    FeatureMaster.CharacteristicsMasterDetails = charactMast;
                //}
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
        private Visibility _buttonVisibleOper = Visibility.Collapsed;
        public Visibility ButtonVisibleOper
        {
            get { return _buttonVisibleOper; }
            set
            {
                this._buttonVisibleOper = value;
                NotifyPropertyChanged("ButtonVisibleOper");
            }
        }
        private void Add()
        {
            try
            {
                if (AddButtonIsEnable == false) return;
                if (FeatureMaster.OperationCode.IsNotNullOrEmpty() || FeatureMaster.FeatureCode.IsNotNullOrEmpty() || FeatureMaster.Feature.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save(FeatureMaster.CharacteristicsMasterDetails);
                        return;
                    }
                    else
                    {
                        ButtonEnable = Visibility.Collapsed;
                        ButtonVisibleOper = Visibility.Visible;
                        ResetForm();
                        ReadOnlyFeature = false;
                        AddButtonIsEnable = false;
                        DeleteButtonIsEnable = true;
                        EditButtonIsEnable = true;
                        TxtReadOnlyOper = false;
                        //FeatureMaster.Feature = string.Empty;
                        //FeatureMaster.FeatureCode = string.Empty;
                        setRights();
                        FocusButton = true;
                    }
                }
                else
                {
                    ButtonEnable = Visibility.Collapsed;
                    ButtonVisibleOper = Visibility.Visible;
                    ResetForm();
                    ReadOnlyFeature = false;
                    AddButtonIsEnable = false;
                    DeleteButtonIsEnable = true;
                    EditButtonIsEnable = true;
                    TxtReadOnlyOper = false;
                    //FeatureMaster.Feature = string.Empty;
                    //FeatureMaster.FeatureCode = string.Empty;
                    setRights();
                    FocusButton = true;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void ResetForm()
        {
            FeatureMaster.Feature = "";
            FeatureMaster.FeatureCode = "";
            FeatureMaster.Operations = "";
            FeatureMaster.OperationCode = "";
            FeatureMaster.CharacteristicsMasterDetails = _featureMasterbll.GetCharactersiticsMasterGrid(FeatureMaster.OperationCode, FeatureMaster.FeatureCode);
            FeatureMaster.FeatureCode = _featureMasterbll.GenerateFeatuerCode();
            FeatureMaster.OperationCodeDetails = _featureMasterbll.GetOpertionMaster();
        }
        private void Edit()
        {
            try
            {
                if (EditButtonIsEnable == false) return;
                if (FeatureMaster.OperationCode.IsNotNullOrEmpty() || FeatureMaster.Feature.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save(FeatureMaster.CharacteristicsMasterDetails);
                    }
                    else
                    {
                        ButtonEnable = Visibility.Visible;
                        ButtonVisibleOper = Visibility.Visible;
                        ReadOnlyFeature = true;
                        FeatureMaster.FeatureCodeDetails = _featureMasterbll.GetFeatureMaster(FeatureMaster.OperationCode);
                        FeatureMaster.CharacteristicsMasterDetails = _featureMasterbll.GetCharactersiticsMasterGrid(FeatureMaster.OperationCode, FeatureMaster.FeatureCode);
                        FeatureMaster.FeatureCode = string.Empty;
                        FeatureMaster.Feature = string.Empty;
                        FeatureMaster.OperationCode = string.Empty;
                        AddButtonIsEnable = true;
                        DeleteButtonIsEnable = false;
                        FeatureMaster.Operations = string.Empty;
                        EditButtonIsEnable = false;
                        TxtReadOnlyOper = false;
                        setRights();
                    }
                }
                else
                {
                    ButtonEnable = Visibility.Visible;
                    ButtonVisibleOper = Visibility.Visible;
                    ReadOnlyFeature = true;
                    FeatureMaster.FeatureCodeDetails = _featureMasterbll.GetFeatureMaster(FeatureMaster.OperationCode);
                    FeatureMaster.CharacteristicsMasterDetails = _featureMasterbll.GetCharactersiticsMasterGrid(FeatureMaster.OperationCode, FeatureMaster.FeatureCode);
                    FeatureMaster.FeatureCode = string.Empty;
                    FeatureMaster.Feature = string.Empty;
                    FeatureMaster.OperationCode = string.Empty;
                    AddButtonIsEnable = true;
                    DeleteButtonIsEnable = false;
                    FeatureMaster.Operations = string.Empty;
                    EditButtonIsEnable = false;
                    TxtReadOnlyOper = false;
                    setRights();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private void Save(DataView cmdetails)
        {
            FocusButton = true;
            if (SaveButtonIsEnable == false) return;
            try
            {
                if (cmdetails.IsNotNullOrEmpty())
                    FeatureMaster.CharacteristicsMasterDetails = cmdetails;

                if (FeatureMaster.OperationCode == "0" || FeatureMaster.OperationCode == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Operation Code"));
                    FocusButton = true;
                    Flag = true;
                    //MessageBox.Show("Operation Code Should be Selected", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else if (FeatureMaster.FeatureCode.Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Feature Code"));
                    Flag = true;
                    FocusButton = true;
                    //MessageBox.Show("Feature Code Should be Selected", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else if (FeatureMaster.Feature.Trim() == "")
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Feature Description"));
                    FocusButton = true;
                    Flag = true;
                    //MessageBox.Show("Feature Description Should be Entered", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else if (!_featureMasterbll.IsFeatureDescDuplicate(FeatureMaster.Feature, Convert.ToDecimal(FeatureMaster.OperationCode)) && AddButtonIsEnable == false && EditButtonIsEnable == true)
                {
                    ShowInformationMessage(PDMsg.AlreadyExists("Feature Description"));
                    FocusButton = true;
                    Flag = true;
                    return;
                }
                if (FeatureMaster.CharacteristicsMasterDetails == null)
                {
                    DataRowView newRow = FeatureMaster.CharacteristicsMasterDetails.AddNew();
                    newRow.BeginEdit();
                    newRow["SNO"] = FeatureMaster.CharacteristicsMasterDetails.Count;
                    newRow.EndEdit();
                }
                DataView charactMast = FeatureMaster.CharacteristicsMasterDetails.Table.Copy().DefaultView;
                for (int i = 1; i < charactMast.Count; i++)
                {
                    charactMast.RowFilter = "MEASURING_TECHNIQUE='" + charactMast[i - 1]["MEASURING_TECHNIQUE"].ToValueAsString() + "'";
                    if (charactMast.Count > 1)
                    {
                        ShowInformationMessage(PDMsg.AlreadyExists("MEASURING TECHNIQUE"));
                        charactMast.RowFilter = string.Empty;
                        return;
                    }
                }
                charactMast.RowFilter = string.Empty;
                FeatureMaster.CharacteristicsMasterDetails.RowFilter = string.Empty;
                if (FeatureMaster.CharacteristicsMasterDetails == null)
                {
                    //DataRow newrow = FeatureMaster.CharacteristicsMasterDetails.Table.NewRow();
                    //FeatureMaster.CharacteristicsMasterDetails.Table.Rows.InsertAt(newrow, FeatureMaster.CharacteristicsMasterDetails.Count);
                    DataRowView newRow = FeatureMaster.CharacteristicsMasterDetails.AddNew();
                    newRow.BeginEdit();
                    newRow["SNO"] = FeatureMaster.CharacteristicsMasterDetails.Count;
                    newRow.EndEdit();
                    //MessageBox.Show("Please enter atleast one characteristics details!", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    //return;
                }

                if (FeatureMaster.CharacteristicsMasterDetails == null)
                {
                    DataRowView newRow = FeatureMaster.CharacteristicsMasterDetails.AddNew();
                    newRow.BeginEdit();
                    newRow["SNO"] = FeatureMaster.CharacteristicsMasterDetails.Count;
                    newRow.EndEdit();
                }

                DataView dgvCharactersticFSave = FeatureMaster.CharacteristicsMasterDetails.ToTable().Copy().DefaultView;
                dgvCharactersticFSave.RowFilter = " TRIM(MEASURING_TECHNIQUE)<>'' or TRIM(SAMPLE_SIZE) <>'' or TRIM(SAMPLE_FREQUENCY) <>'' or TRIM(CONTROL_METHOD) <>''  or TRIM(REACTION_PLAN) <> '' ";
                if (dgvCharactersticFSave.Count == 0)
                {
                    MessageBox.Show("Please enter atleast one characteristics details!", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                string typ = "INS";
                Progress.ProcessingText = PDMsg.ProgressUpdateText;
                Progress.Start();
                isSaved = _featureMasterbll.SaveFeatureCodeMaster(FeatureMaster.CharacteristicsMasterDetails, FeatureMaster.OperationCode, FeatureMaster.Operations, FeatureMaster.FeatureCode, FeatureMaster.Feature, ref typ);
                Progress.End();
                if (isSaved)
                {
                    if (AddButtonIsEnable == true)
                    {
                        ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                        //try
                        //{
                        //    foreach (DataRow row in delCharacteristics.Rows)
                        //    {
                        //        _featureMasterbll.DeleteFeatureCharacteristicsDetails(row["OPER_CODE"].ToString().Trim().ToDecimalValue(), row["FEATURE_CODE"].ToString().Trim(), row["SNO"].ToString().Trim().ToDecimalValue());
                        //    }

                        //}
                        //catch (Exception)
                        //{
                        //}
                        Flag = false;
                        delCharacteristics.Clear();
                    }
                    else
                    {
                        ShowInformationMessage(PDMsg.SavedSuccessfully);
                        Flag = false;
                    }

                    //MessageBox.Show("Records Saved successfully", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    if (Flag == false)
                    {
                        ResetForm();
                        AddButtonIsEnable = false;
                        DeleteButtonIsEnable = true;
                        EditButtonIsEnable = true;
                        setRights();
                        FocusButton = true;
                    }
                    else if (Flag == true)
                    {
                        if (AddButtonIsEnable == true)
                        {
                            AddButtonIsEnable = true;
                            EditButtonIsEnable = false;
                            DeleteButtonIsEnable = true;
                            setRights();
                            FocusButton = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();

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
                this._dropDownItems = value;
                NotifyPropertyChanged("DropDownItems");
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
        private void SetdropDownItems()
        {
            try
            {
                DropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "OPER_CODE", ColumnDesc = "Opr. Code", ColumnWidth = 85 },
                            new DropdownColumns() { ColumnName = "OPER_DESC", ColumnDesc = "Opr. Desc", ColumnWidth = "1*" }
                        };
                DropDownItemsFeature = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "FEATURE_CODE", ColumnDesc = "Feature Code", ColumnWidth = 73 },
                            new DropdownColumns() { ColumnName = "FEATURE_DESC", ColumnDesc = "Feature Description", ColumnWidth = "1*", IsDefaultSearchColumn = true }
                        };

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
                //if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //{
                //    Save(FeatureMaster.CharacteristicsMasterDetails);
                //    return;
                //}


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
                //if (FeatureMaster.OperationCode.IsNotNullOrEmpty() || FeatureMaster.FeatureCode.IsNotNullOrEmpty() || FeatureMaster.Feature.IsNotNullOrEmpty())
                //{
                //    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //    {
                //        Save(FeatureMaster.CharacteristicsMasterDetails);
                //        closingev.Cancel = true;
                //        e = closingev;
                //        return;
                //    }
                //}

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

        private bool _flag = false;
        public bool Flag
        {
            get { return _flag; }
            set
            {
                _flag = value;
                NotifyPropertyChanged("Flag");
            }
        }
    }
}
