using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
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
    /// Interaction logic for frmCharacteristicMaster.xaml
    /// </summary>
    public partial class frmCharacteristicMaster : UserControl
    {
        List<DataRowView> bufferAddedList = new List<DataRowView>();
        private UserInformation _userInformation = new UserInformation();
        private CrossLinkingCharModel _crossLinkingChar = new CrossLinkingCharModel();
        private CrossLinkingCharBll _crossLinkingCharDet;
        private DataSet dsData;
        private DataSet dsDataFeature;
        private DataView charactersticsMasterDatas;
        private DataTable linkedWithMasterDatas;
        private string featureCode = string.Empty;
        private int dgvFeatureRowCount = 0;
        //private string newFeaturCode = string.Empty;
        private string newFeaturDesc = string.Empty;
        private decimal newOperationCode = 1030;
        private List<DataRowView> lstAdded = new List<DataRowView>();
        //WPF.MDI.MdiChild me;
        public frmCharacteristicMaster(UserInformation userInformation, WPF.MDI.MdiChild me)
        {
            InitializeComponent();

            _userInformation = userInformation;
            _crossLinkingCharDet = new CrossLinkingCharBll(_userInformation);
            CrossLinkingViewModel fm = new CrossLinkingViewModel(_userInformation);
            this.DataContext = fm;
            me.Closing += fm.CloseMethod;
            if (fm.CloseAction == null)
                fm.CloseAction = new Action(() => me.Close());
            //  InitForm();

        }

        //    private void InitForm()
        //    {
        //        _crossLinkingChar.Type = "";
        //        _crossLinkingChar.LinkedWith = "";
        //        _crossLinkingChar.ProductType = "";
        //        InitData();
        //        GetRights();
        //        setRights();
        //    }

        //    private void ClearAll()
        //    {
        //        try
        //        {
        //            ResetGrid();
        //            _saveFlag = false;
        //            if (!cmbLinkedWith.Text.IsNotNullOrEmpty()) cmbLinkedWith.SelectedIndex = -1;
        //            if (!cmbProductType.Text.IsNotNullOrEmpty()) cmbProductType.SelectedIndex = -1;
        //            if (!cmbType.Text.IsNotNullOrEmpty()) cmbType.SelectedIndex = -1;
        //            featureCode = "";

        //            newFeaturDesc = "";
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();
        //        }
        //    }
        //    private void ClearAllForType()
        //    {
        //        try
        //        {
        //            ResetGrid();
        //            cmbLinkedWith.SelectedIndex = -1;
        //            cmbProductType.SelectedIndex = -1;
        //            featureCode = "";

        //            newFeaturDesc = "";
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();
        //        }
        //    }

        //    private RolePermission ActionPermission { get; set; }
        //    private void GetRights()
        //    {
        //        ActionPermission = new RolePermission();
        //        ActionPermission.Save = true;
        //        ActionPermission.Print = false;
        //        ActionPermission.View = true;
        //        ActionPermission.AddNew = false;
        //        ActionPermission.Delete = false;
        //        ActionPermission.Edit = false;
        //        ActionPermission = _crossLinkingCharDet.GetUserRights("CHARACTERISTICS GROUPING MASTER");
        //        if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
        //        {
        //            ActionPermission.Save = false;
        //        }
        //    }

        //    private void setRights()
        //    {
        //        if (btnAdd.IsEnabled) btnAdd.IsEnabled = ActionPermission.AddNew;
        //        if (btnEdit.IsEnabled) btnEdit.IsEnabled = ActionPermission.Edit;
        //        if (btnSave.IsEnabled) btnSave.IsEnabled = ActionPermission.Edit;

        //    }
        //    private void NewMode()
        //    {
        //        ClearAll();
        //        btnAdd.IsEnabled = false;
        //        setRights();

        //    }
        //    private void NewModeTypeChanged()
        //    {
        //        ClearAllForType();
        //        btnAdd.IsEnabled = false;
        //        setRights();

        //    }
        //    private void EditMode()
        //    {
        //        btnEdit.IsEnabled = false;
        //        setRights();

        //    }
        //    private void Save()
        //    {
        //        try
        //        {
        //            int d, countPrd = 0;
        //            bool charStatus, prodCatSStatus, typeSStatus = false;
        //            DataView dgvFeaturFSave, dgvCharactersticFSave, dgOperatioFSave, dgvLinkedWitFSave;
        //            cmbType.Focus();
        //            dgvFeaturFSave = ((DataView)dgvFeature.ItemsSource);
        //            dgvCharactersticFSave = (DataView)dgvCharacterstics.ItemsSource;
        //            dgvFeaturFSave = ((DataView)dgvFeature.ItemsSource);
        //            dgOperatioFSave = (DataView)dgOperation.ItemsSource;
        //            dgvLinkedWitFSave = (DataView)dgvLinkedWith.ItemsSource;


        //            if (!cmbType.Text.Trim().IsNotNullOrEmpty())
        //            {
        //                errType.Visibility = Visibility.Visible;
        //                ShowInformationMessage("Type Should not be Empty");
        //                return;
        //            }
        //            else if (!cmbProductType.Text.Trim().IsNotNullOrEmpty())
        //            {
        //                errPrdType.Visibility = Visibility.Visible;
        //                ShowInformationMessage("Product Type Should not be Empty");
        //                return;
        //            }
        //            errType.Visibility = Visibility.Collapsed;
        //            errPrdType.Visibility = Visibility.Collapsed;

        //            countPrd = ((DataView)dgvCharacterstics.ItemsSource).Count;

        //            if (dgvCharactersticFSave.IsNotNullOrEmpty() && dgvCharactersticFSave.Count > 0) dgvCharactersticFSave.RowFilter = "oper_code is null ";

        //            if (countPrd > 0) countPrd = Math.Abs(((DataView)dgvCharacterstics.ItemsSource).Table.Copy().Rows.Count - countPrd);

        //            if (dgvCharactersticFSave.IsNotNullOrEmpty())
        //                charStatus = _crossLinkingCharDet.SaveCharacterMasters(dgvCharactersticFSave, newOperationCode, featureCode, newFeaturDesc, "NEW", countPrd, _crossLinkingChar.Type, _crossLinkingChar.ProductType, _crossLinkingChar.LinkedWith);
        //            dgvCharactersticFSave.RowFilter = string.Empty;
        //            dgvCharactersticFSave.RowFilter = "oper_code is not null ";
        //            _crossLinkingCharDet.UpdateCharacteristicsmaster(dgvCharactersticFSave);
        //            if (dgOperatioFSave.IsNotNullOrEmpty())
        //            {
        //                prodCatSStatus = _crossLinkingCharDet.SaveForginMaster(dgOperatioFSave, featureCode, ((DataView)dgOperation.ItemsSource).Count, _crossLinkingChar.Type, _crossLinkingChar.ProductType, _crossLinkingChar.LinkedWith, "NEW");
        //            }

        //            if (dgvLinkedWitFSave.IsNotNullOrEmpty())
        //            {
        //                typeSStatus = _crossLinkingCharDet.SaveFastnersMaster(dgvLinkedWitFSave, featureCode, ((DataView)dgvLinkedWith.ItemsSource).Count, _crossLinkingChar.Type, _crossLinkingChar.ProductType, _crossLinkingChar.LinkedWith, "NEW");
        //            }
        //            typeSStatus = true;
        //            if (charStatus = prodCatSStatus = typeSStatus == true)
        //            {
        //                foreach (DataRow row in delCharacteristics.Rows)
        //                {
        //                    _crossLinkingCharDet.DeleteFeatureCharacteristicsDetails(row["OPER_CODE"].ToString().Trim().ToDecimalValue(), row["FEATURE_CODE"].ToString().Trim(), row["SNO"].ToString().Trim().ToDecimalValue());
        //                }
        //                foreach (DataRow row in delProductType.Rows)
        //                {
        //                    _crossLinkingCharDet.DeletePrdCategorybasedDetails(row["FEATURE_CODE"].ToString().Trim(), row["PRD_CODE"].ToString().Trim(), row["TYPE"].ToString().Trim(), row["SUB_TYPE"].ToString().Trim(), row["SNO"].ToString().Trim().ToDecimalValue());
        //                }
        //                foreach (DataRow row in delLinkedWith.Rows)
        //                {
        //                    _crossLinkingCharDet.DeleteLnkedWithbasedDetails(row["FEATURE_CODE"].ToString().Trim(), row["PRD_CODE"].ToString().Trim(), row["TYPE"].ToString().Trim(), row["SUB_TYPE"].ToString().Trim(), row["linked_with"].ToString().Trim(), row["SNO"].ToString().Trim().ToDecimalValue());
        //                }
        //                bufferCharctdetails.Rows.Clear();
        //                delCharacteristics.Rows.Clear();
        //                delLinkedWith.Rows.Clear();
        //                delProductType.Rows.Clear();
        //                lstAdded.Clear();
        //                ShowInformationMessage("Records Saved Successfully");
        //            }
        //            _crossLinkingCharDet.GetGridFeature("", "", "");
        //            LoadProductTypeBasedGrid();

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();
        //        }
        //    }
        //    public string ApplicationTitle = "SmartPD";
        //    private MessageBoxResult ShowInformationMessage(string _showMessage)
        //    {
        //        if (_showMessage.IsNotNullOrEmpty())
        //            return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
        //        return MessageBoxResult.None;
        //    }

        //    private MessageBoxResult ShowWarningMessage(string _showMessage, MessageBoxButton messageBoxButton)
        //    {
        //        if (_showMessage.IsNotNullOrEmpty())
        //            return MessageBox.Show(_showMessage, ApplicationTitle, messageBoxButton, MessageBoxImage.Warning);
        //        return MessageBoxResult.None;
        //    }

        //    private void InitData()
        //    {
        //        try
        //        {
        //            dsData = _crossLinkingCharDet.GetTypeCmb();
        //            grdTopCmb.DataContext = _crossLinkingChar;
        //            if (dsData.IsNotNullOrEmpty())
        //            {
        //                cmbType.SelectedValuePath = dsData.Tables[0].Columns[0].ToString();
        //                cmbType.DisplayMemberPath = dsData.Tables[0].Columns[0].ToString();
        //                cmbType.ItemsSource = dsData.Tables[0].DefaultView;
        //            }



        //            DataSet dsFeatureData;
        //            dsFeatureData = _crossLinkingCharDet.GetGridFeature("", "");
        //            if (dsFeatureData.IsNotNullOrEmpty() && dsFeatureData.Tables.Count > 0 && dsFeatureData.Tables[0].DefaultView.Count == 0)
        //            {
        //                dgvFeature.ItemsSource = dsFeatureData.Tables[0].DefaultView;
        //                ((DataView)dgvFeature.ItemsSource).Table.DefaultView.AddNew();
        //            }

        //            ResetGrid();

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();
        //        }
        //    }

        //    private void ResetGrid()
        //    {
        //        charactersticsMasterDatas = _crossLinkingCharDet.GetGridCharacteristicsMaster().Tables[0].DefaultView;
        //        charactersticsMasterDatas.RowFilter = "feature_code='0'";
        //        if (charactersticsMasterDatas.IsNotNullOrEmpty() && charactersticsMasterDatas.Count == 0)
        //        {
        //            dgvCharacterstics.ItemsSource = charactersticsMasterDatas;
        //            //((DataView)dgvCharacterstics.ItemsSource).Table.DefaultView.AddNew();
        //            DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).Table.DefaultView.AddNew();
        //            dr["feature_code"] = featureCode;
        //            dr["feature_desc"] = newFeaturDesc;
        //            lstAdded.Add(dr);
        //        }


        //        DataSet dsDataGrd;
        //        dsDataGrd = _crossLinkingCharDet.GetGridDetailsLinkedWithCross("", "", featureCode);
        //        linkedWithMasterDatas = dsDataGrd.Tables[0];
        //        if (dsDataGrd.IsNotNullOrEmpty() && dsDataGrd.Tables.Count > 0 && dsDataGrd.Tables[0].DefaultView.Count == 0)
        //        {
        //            dgvLinkedWith.ItemsSource = linkedWithMasterDatas.DefaultView;
        //            ((DataView)dgvLinkedWith.ItemsSource).Table.DefaultView.AddNew();
        //        }


        //        dsDataGrd = _crossLinkingCharDet.GetGridDetailsProdCategCross("", "", featureCode);
        //        if (dsDataGrd.IsNotNullOrEmpty() && dsDataGrd.Tables.Count > 0 && dsDataGrd.Tables[0].DefaultView.Count == 0)
        //        {
        //            dgOperation.ItemsSource = dsDataGrd.Tables[0].DefaultView;
        //            ((DataView)dgOperation.ItemsSource).Table.DefaultView.AddNew();
        //        }

        //    }

        //    DataTable dt = null;
        //    private void BindGridProductCategory(string featureCode)
        //    {
        //        try
        //        {

        //            DataSet dsData, dsDataGrd;
        //            dsData = new DataSet();
        //            dsDataGrd = new DataSet();

        //            dsData = _crossLinkingCharDet.GetGridComboProdCategCross(_crossLinkingChar.Type, _crossLinkingChar.ProductType);
        //            if (dsData.IsNotNullOrEmpty())
        //                dgOperation.DataContext = dsData.Tables[0].DefaultView;
        //            // cmbOpMaster.ItemsSource = dsData.Tables[0].DefaultView;
        //            dsDataGrd = _crossLinkingCharDet.GetGridDetailsProdCategCross(_crossLinkingChar.Type, _crossLinkingChar.ProductType, featureCode);
        //            if (dsData.IsNotNullOrEmpty())
        //            {
        //                dt = dsDataGrd.Tables[0];
        //                dgOperation.ItemsSource = dt.DefaultView;
        //            }
        //            if (dsDataGrd.IsNotNullOrEmpty() && dsDataGrd.Tables.Count > 0 && dsDataGrd.Tables[0].DefaultView.Count == 0)
        //            {
        //                ((DataView)dgOperation.ItemsSource).AddNew();
        //            }
        //            else if (dsDataGrd.IsNotNullOrEmpty() && dsDataGrd.Tables.Count > 0 && dsDataGrd.Tables[0].DefaultView.Count > 0)
        //            {
        //                ((DataView)dgOperation.ItemsSource).AddNew();
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();
        //        }

        //    }
        //    DataTable dtLinked = null;
        //    private void BindGridLinkedWith(string featureCode = "")
        //    {
        //        try
        //        {
        //            DataSet dsData, dsDataGrd;
        //            dsData = new DataSet();
        //            dsDataGrd = new DataSet();
        //            if (_crossLinkingChar.LinkedWith.IsNotNullOrEmpty())
        //            {
        //                dsData = _crossLinkingCharDet.GetGridComboLinkedWithCross(_crossLinkingChar.LinkedWith);
        //                if (dsData.IsNotNullOrEmpty()) dgvLinkedWith.DataContext = dsData.Tables[0].DefaultView;
        //                //cmbdgvLinkedWith.ItemsSource = dsData.Tables[0].DefaultView;
        //                dsDataGrd = _crossLinkingCharDet.GetGridDetailsLinkedWithCross(_crossLinkingChar.Type, _crossLinkingChar.ProductType, featureCode);
        //                dtLinked = dsDataGrd.Tables[0];
        //                linkedWithMasterDatas = dtLinked;
        //                dgvLinkedWith.ItemsSource = dtLinked.DefaultView;
        //                if (dsDataGrd.IsNotNullOrEmpty() && dsDataGrd.Tables.Count > 0 && dsDataGrd.Tables[0].DefaultView.Count == 0)
        //                {
        //                    ((DataView)dgvLinkedWith.ItemsSource).AddNew();
        //                }
        //                else if (dsDataGrd.IsNotNullOrEmpty() && dsDataGrd.Tables.Count > 0 && dsDataGrd.Tables[0].DefaultView.Count > 0)
        //                {
        //                    ((DataView)dgvLinkedWith.ItemsSource).AddNew();
        //                }

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();
        //        }

        //    }

        //    private void BindDatas()
        //    {
        //        try
        //        {
        //            DataSet dsData;
        //            dsData = _crossLinkingCharDet.GetGridFeature(_crossLinkingChar.Type, _crossLinkingChar.ProductType);
        //            if (dsData.IsNotNullOrEmpty() && dsData.Tables.Count > 0 && dsData.Tables[0].DefaultView.Count == 0)
        //            {
        //                dgvFeature.ItemsSource = dsData.Tables[0].DefaultView;

        //                ((DataView)dgvFeature.ItemsSource).Table.DefaultView.AddNew();
        //            }
        //            if (dsData.IsNotNullOrEmpty() && dsData.Tables.Count > 0 && dsData.Tables[0].DefaultView.Count > 0)
        //            {
        //                dgvFeature.ItemsSource = dsData.Tables[0].DefaultView;
        //                dgvFeatureRowCount = dgvFeature.Items.Count;
        //                if (dgvFeatureRowCount > 0 && dgvFeature.SelectedIndex == -1)
        //                {
        //                    DataView item = (DataView)dgvFeature.ItemsSource;
        //                    dgvFeature.SelectedItem = item[0];
        //                    dgvFeature.BeginEdit();
        //                    if (dgvFeature.SelectedIndex < dgvFeatureRowCount - 1)
        //                    {

        //                        DataRowView row = (DataRowView)dgvFeature.SelectedItem;
        //                        featureCode = row["feature_code"].ToString().Trim();
        //                        newFeaturDesc = row["Feature"].ToString().Trim();
        //                    }

        //                }
        //                charactersticsMasterDatas = _crossLinkingCharDet.GetGridCharacteristicsMaster().Tables[0].DefaultView;
        //                if (dgvFeature.SelectedIndex == -1)
        //                {
        //                    object item = dgvFeature.Items[0];
        //                    dgvFeature.SelectedItem = item;
        //                    dgvFeature.BeginEdit();
        //                }
        //                charactersticsMasterDatas.RowFilter = "feature_desc='" + newFeaturDesc + "'";
        //                dgvCharacterstics.ItemsSource = charactersticsMasterDatas;
        //                if (charactersticsMasterDatas.IsNotNullOrEmpty() && charactersticsMasterDatas.Count == 0)
        //                {
        //                    DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //                    dr["feature_code"] = featureCode;
        //                    dr["feature_desc"] = newFeaturDesc;
        //                    lstAdded.Add(dr);
        //                }
        //                else if (charactersticsMasterDatas.IsNotNullOrEmpty() && charactersticsMasterDatas.Count > 0)
        //                {
        //                    DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //                    dr["feature_code"] = featureCode;
        //                    dr["feature_desc"] = newFeaturDesc;
        //                    lstAdded.Add(dr);
        //                }
        //                if (dsData.IsNotNullOrEmpty() && dsData.Tables[0].Rows.Count > 0)
        //                {
        //                    ((DataView)dgvFeature.ItemsSource).AddNew();
        //                }
        //                else if (dsData.IsNotNullOrEmpty() && dsData.Tables[0].Rows.Count == 0)
        //                {
        //                    if (((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim() != "")
        //                        ((DataView)dgvFeature.ItemsSource).AddNew();
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();
        //        }
        //    }

        //    private void Window_KeyUp(object sender, KeyEventArgs e)
        //    {

        //    }
        //    private void cmbType_LostFocus(object sender, RoutedEventArgs e)
        //    {

        //    }
        //    private bool _saveFlag = false;
        //    private void btnSave_Click(object sender, RoutedEventArgs e)
        //    {
        //        _saveFlag = true;
        //        Save();
        //        _saveFlag = false;
        //    }

        //    private void cmbProductType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //    {
        //        try
        //        {
        //            if (_crossLinkingChar.ProductType.IsNotNullOrEmpty())
        //            {
        //                errPrdType.Visibility = Visibility.Collapsed;
        //            }
        //            LoadProductTypeBasedGrid();
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();
        //        }
        //    }

        //    private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //    {
        //        try
        //        {
        //            if (_crossLinkingChar.Type.IsNotNullOrEmpty())
        //            {
        //                errType.Visibility = Visibility.Collapsed;
        //            }
        //            NewModeTypeChanged();
        //            grdTopCmb.DataContext = _crossLinkingChar;
        //            dsDataFeature = _crossLinkingCharDet.GetGridFeature("", "", "");
        //            dsData = _crossLinkingCharDet.GetProductTypeCmb(_crossLinkingChar.Type);
        //            if (dsData.IsNotNullOrEmpty())
        //            {
        //                cmbProductType.SelectedValuePath = dsData.Tables[0].Columns[0].ToString();
        //                cmbProductType.DisplayMemberPath = dsData.Tables[0].Columns[0].ToString();
        //                cmbProductType.ItemsSource = dsData.Tables[0].DefaultView;

        //            }
        //            dsData = _crossLinkingCharDet.GetLinkedTypeCmb(_crossLinkingChar.Type);
        //            if (dsData.IsNotNullOrEmpty())
        //            {
        //                cmbLinkedWith.SelectedValuePath = dsData.Tables[0].Columns[0].ToString();
        //                cmbLinkedWith.DisplayMemberPath = dsData.Tables[0].Columns[0].ToString();
        //                cmbLinkedWith.ItemsSource = dsData.Tables[0].DefaultView;

        //            }
        //            if (dsDataFeature.IsNotNullOrEmpty())
        //            {
        //                var dvchar = ((DataView)dgvCharacterstics.ItemsSource);
        //                dgvFeature.ItemsSource = dsDataFeature.Tables[0].DefaultView;
        //                ((DataView)dgvFeature.ItemsSource).Table.DefaultView.AddNew();
        //                charactersticsMasterDatas = _crossLinkingCharDet.GetGridCharacteristicsMaster().Tables[0].DefaultView;
        //                ((DataView)dgvCharacterstics.ItemsSource).Table.AcceptChanges();
        //                if (((DataView)dgvCharacterstics.ItemsSource).Table.Rows.Count <= 0)
        //                {
        //                    charactersticsMasterDatas.RowFilter = "feature_desc='" + newFeaturDesc + "'";
        //                    dgvCharacterstics.ItemsSource = charactersticsMasterDatas;
        //                    dvchar = ((DataView)dgvCharacterstics.ItemsSource);
        //                    if (dvchar.Count > 0)
        //                    {
        //                        if (dvchar[dvchar.Count - 1]["measuring_technique"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_size"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_frequency"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["control_method"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["reaction_plan"].ToString().Trim() != "")
        //                        {
        //                            DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //                            dr["feature_code"] = featureCode;
        //                            dr["feature_desc"] = newFeaturDesc;
        //                            lstAdded.Add(dr);
        //                        }
        //                    }
        //                }
        //                dvchar = ((DataView)dgvCharacterstics.ItemsSource);
        //                if (dvchar.Count == 0)
        //                {
        //                    charactersticsMasterDatas.RowFilter = "feature_desc='" + newFeaturDesc + "'";
        //                    dgvCharacterstics.ItemsSource = charactersticsMasterDatas;
        //                    dvchar = ((DataView)dgvCharacterstics.ItemsSource);
        //                    if (dvchar.Count > 0)
        //                    {
        //                        if (dvchar[dvchar.Count - 1]["measuring_technique"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_size"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_frequency"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["control_method"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["reaction_plan"].ToString().Trim() != "")
        //                        {
        //                            DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //                            dr["feature_code"] = featureCode;
        //                            dr["feature_desc"] = newFeaturDesc;
        //                            lstAdded.Add(dr);
        //                        }
        //                    }
        //                    else if (dvchar.Count == 0)
        //                    {
        //                        DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //                        dr["feature_code"] = featureCode;
        //                        dr["feature_desc"] = newFeaturDesc;
        //                        lstAdded.Add(dr);
        //                    }

        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();
        //        }

        //    }
        //    private void dgvFeature_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //    {
        //        try
        //        {
        //            if (_crossLinkingChar.Type.IsNotNullOrEmpty() && _crossLinkingChar.ProductType.IsNotNullOrEmpty())
        //                LoadDGVFeatureSelectionBasedGrids();

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();
        //        }

        //    }

        //    private void LoadDGVFeatureSelectionBasedGrids()
        //    {

        //        try
        //        {
        //            try
        //            {
        //                // if (dgvFeature.SelectedIndex < dgvFeatureRowCount - 1)
        //                if (dgvFeature.SelectedIndex < dgvFeatureRowCount)
        //                {
        //                    if (((DataRowView)dgvFeature.SelectedItem).IsNotNullOrEmpty())
        //                    {
        //                        DataRowView row = (DataRowView)dgvFeature.SelectedItem;
        //                        featureCode = row["feature_code"].ToString().Trim();
        //                        newFeaturDesc = row["Feature"].ToString().Trim();
        //                    }

        //                }
        //                else
        //                {
        //                    featureCode = _crossLinkingCharDet.GenerateFeatuerCode();
        //                    if (((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim() != "")
        //                    {
        //                        newFeaturDesc = ((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim();
        //                    }
        //                    else if (((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim() == "")
        //                    {
        //                        newFeaturDesc = ((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim();
        //                    }

        //                }
        //            }
        //            catch (Exception)
        //            {
        //                featureCode = "";
        //            }
        //            charactersticsMasterDatas.RowFilter = "feature_desc='" + newFeaturDesc + "'";
        //            dgvCharacterstics.ItemsSource = charactersticsMasterDatas;

        //            //////Buffer   Dont Delete
        //            ////if (charactersticsMasterDatas.Count == 0)
        //            ////{
        //            ////    if (bufferCharctdetails.Rows.Count > 0)
        //            ////    {
        //            ////        dgvCharacterstics.ItemsSource = bufferCharctdetails.DefaultView;
        //            ////    }
        //            ////}
        //            ////else
        //            ////{
        //            ////    DataRow[] drSelect;
        //            ////    DataTable ddcha = new DataTable();
        //            ////    if (delCharacteristics.Rows.Count > 0)
        //            ////    {
        //            ////        drSelect = delCharacteristics.Select("feature_code='" + featureCode + "'");
        //            ////        if (drSelect.Length > 0)
        //            ////        {
        //            ////            ddcha = ((DataView)dgvCharacterstics.ItemsSource).Table.Copy();
        //            ////            foreach (DataRow drRow in drSelect)
        //            ////            {
        //            ////                int i = 0;
        //            ////                while (i < ddcha.Rows.Count)
        //            ////                {
        //            ////                    if (ddcha.Rows[i]["sno"].ToValueAsString().Trim() == drRow["sno"].ToValueAsString().Trim())
        //            ////                    {
        //            ////                        ddcha.Rows.RemoveAt(i);
        //            ////                    }
        //            ////                    else
        //            ////                    {
        //            ////                        i++;
        //            ////                    }
        //            ////                }
        //            ////            }
        //            ////            dgvCharacterstics.ItemsSource = ddcha.DefaultView;
        //            ////        }

        //            ////    }
        //            ////}

        //            ////if (lstAdded.Count > 0)
        //            ////{
        //            ////    try
        //            ////    {

        //            ////        DataTable dtNewList = new DataTable();
        //            ////        bufferAddedList = (from DataRowView rowView in lstAdded
        //            ////                           where rowView.Row.Field<string>("feature_code") == featureCode && rowView.Row.Field<string>("feature_desc") == newFeaturDesc
        //            ////                           select rowView).ToList();
        //            ////        dtNewList = ((DataView)dgvCharacterstics.ItemsSource).Table.Clone();
        //            ////        dtNewList = ((DataView)dgvCharacterstics.ItemsSource).Table.Copy();
        //            ////        foreach (var item in bufferAddedList)
        //            ////        {
        //            ////            dtNewList.ImportRow(item.Row);
        //            ////        }
        //            ////        dgvCharacterstics.ItemsSource = dtNewList.DefaultView;
        //            ////    }
        //            ////    catch (Exception)
        //            ////    {

        //            ////    }
        //            ////}

        //            if (charactersticsMasterDatas.IsNotNullOrEmpty() && dgvCharacterstics.Items.Count == 0)
        //            {
        //                if (((DataRowView)dgvFeature.SelectedItem).IsNotNullOrEmpty() && ((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim().IsNotNullOrEmpty())
        //                {
        //                    var dvchar = ((DataView)dgvCharacterstics.ItemsSource);
        //                    if (dvchar.Count > 0)
        //                    {
        //                        if (dvchar[dvchar.Count - 1]["measuring_technique"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_size"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_frequency"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["control_method"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["reaction_plan"].ToString().Trim() != "")
        //                        {
        //                            DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //                            dr["feature_code"] = featureCode;
        //                            dr["feature_desc"] = newFeaturDesc;
        //                            lstAdded.Add(dr);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //                        dr["feature_code"] = featureCode;
        //                        dr["feature_desc"] = newFeaturDesc;
        //                        lstAdded.Add(dr);

        //                    }

        //                }
        //                else
        //                {
        //                    var dvchar = ((DataView)dgvCharacterstics.ItemsSource);
        //                    if (dvchar.Count > 0)
        //                    {
        //                        if (dvchar[dvchar.Count - 1]["measuring_technique"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_size"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_frequency"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["control_method"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["reaction_plan"].ToString().Trim() != "")
        //                        {
        //                            DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //                            dr["feature_code"] = featureCode;
        //                            dr["feature_desc"] = newFeaturDesc;
        //                            lstAdded.Add(dr);
        //                        }
        //                    }

        //                }

        //            }
        //            else if (charactersticsMasterDatas.IsNotNullOrEmpty() && dgvCharacterstics.Items.Count > 0)
        //            {
        //                var dvchar = ((DataView)dgvCharacterstics.ItemsSource);
        //                if (dvchar.Count > 0)
        //                {
        //                    if (dvchar[dvchar.Count - 1]["measuring_technique"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_size"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_frequency"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["control_method"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["reaction_plan"].ToString().Trim() != "")
        //                    {
        //                        DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //                        dr["feature_code"] = featureCode;
        //                        dr["feature_desc"] = newFeaturDesc;
        //                        lstAdded.Add(dr);
        //                    }
        //                }

        //            }
        //            BindGridProductCategory(featureCode);
        //            BindGridLinkedWith(featureCode);

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();
        //        }

        //    }
        //    private void LoadProductTypeBasedGrid()
        //    {
        //        try
        //        {
        //            dgOperation.Columns[0].Header = _crossLinkingChar.ProductType;
        //            if (_crossLinkingChar.ProductType.IsNotNullOrEmpty() && _crossLinkingChar.Type.IsNotNullOrEmpty())
        //            {
        //                BindDatas();
        //                BindGridProductCategory(featureCode);
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();
        //        }

        //    }

        //    private void cmbLinkedWith_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //    {
        //        try
        //        {
        //            dgvLinkedWith.Columns[0].Header = _crossLinkingChar.LinkedWith;
        //            if (dgvFeature.Items.Count > 0)
        //            {
        //                if (dgvFeature.SelectedIndex == -1)
        //                {
        //                    object item = dgvFeature.Items[0];
        //                    dgvFeature.SelectedItem = item;
        //                    dgvFeature.BeginEdit();
        //                }
        //                if (_crossLinkingChar.ProductType.IsNotNullOrEmpty() && _crossLinkingChar.Type.IsNotNullOrEmpty())
        //                {
        //                    BindGridLinkedWith(featureCode);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex.LogException();

        //        }

        //    }

        //    private void btnClose_Click(object sender, RoutedEventArgs e)
        //    {
        //        this.Close();
        //    }

        //    private void Window_KeyDown(object sender, KeyEventArgs e)
        //    {

        //    }
        //    private void CutContractKeyDown(object sender, KeyEventArgs e)
        //    {
        //        try
        //        {

        //            if (e.Key == Key.F3)
        //            {
        //                NewMode();
        //            }
        //            else if (e.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
        //            {

        //                Save();
        //            }
        //            else if (e.Key == Key.F5)
        //            {
        //                EditMode();
        //            }
        //            else if (e.Key == Key.F9)
        //            {
        //                this.Close();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show(ex.Message, "SmartPD");
        //        }

        //    }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbType.Focus();
            //delCharacteristics = ((DataView)dgvCharacterstics.ItemsSource).Table.Clone();
            //delFeature = ((DataView)dgvFeature.ItemsSource).Table.Clone();
            //delProductType = ((DataView)dgOperation.ItemsSource).Table.Clone();
            //delLinkedWith = ((DataView)dgvLinkedWith.ItemsSource).Table.Clone();
            //errType.Visibility = Visibility.Visible;
            //errPrdType.Visibility = Visibility.Visible;
            //bufferCharctdetails = ((DataView)dgvCharacterstics.ItemsSource).Table.Clone();
        }

        //    private void dgvFeature_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        //    {

        //        DataRowView row = (DataRowView)dgvFeature.SelectedItem;
        //        if (((DataRowView)dgvFeature.SelectedItem).IsNotNullOrEmpty())
        //        {
        //            if (oldFeature.IsNotNullOrEmpty()) row["Feature"] = oldFeature;

        //            featureCode = row["feature_code"].ToString().Trim();
        //            newFeaturDesc = row["Feature"].ToString().Trim();
        //            if (_crossLinkingChar.Type.IsNotNullOrEmpty() && _crossLinkingChar.ProductType.IsNotNullOrEmpty() && newFeaturDesc == "")
        //            {
        //                ShowInformationMessage("Feature is can't be empty!");
        //                dgvFeature.BeginEdit();
        //            }
        //        }
        //        var duplicateRecords = ((DataView)dgvFeature.ItemsSource).ToTable(true, "FEATURE").Rows.Count;
        //        if (((DataView)dgvFeature.ItemsSource).Count != duplicateRecords)
        //        {
        //            // ShowInformationMessage("Duplicate Feature entered");
        //            if (((DataRowView)dgvFeature.SelectedItem).IsNotNullOrEmpty())
        //            {
        //                if (row["feature_code"].ToString().Trim() != "")
        //                {
        //                    featureCode = row["feature_code"].ToString().Trim();
        //                    newFeaturDesc = row["Feature"].ToString().Trim();
        //                }
        //                else
        //                {
        //                    featureCode = _crossLinkingCharDet.GenerateFeatuerCode();
        //                    if (((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim() != "")
        //                        newFeaturDesc = ((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim();
        //                }

        //            }

        //            return;
        //        }
        //        try
        //        {

        //            if (dgvFeature.SelectedIndex < dgvFeatureRowCount - 1)
        //            {
        //                if (((DataRowView)dgvFeature.SelectedItem).IsNotNullOrEmpty())
        //                {
        //                    if (row["feature_code"].ToString().Trim() != "")
        //                    {
        //                        featureCode = row["feature_code"].ToString().Trim();
        //                        newFeaturDesc = row["Feature"].ToString().Trim();
        //                    }
        //                    else
        //                    {
        //                        featureCode = _crossLinkingCharDet.GenerateFeatuerCode();
        //                        if (((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim() != "")
        //                            newFeaturDesc = ((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim();
        //                    }

        //                }

        //            }
        //            else
        //            {
        //                if (featureCode.IsNotNullOrEmpty() && _saveFlag == true)
        //                {
        //                    featureCode = _crossLinkingCharDet.GenerateFeatuerCode();
        //                    if (((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim() != "")
        //                        newFeaturDesc = ((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim();
        //                }
        //                else if (!featureCode.IsNotNullOrEmpty() && _saveFlag == false && row["Feature"].ToString().Trim() != "")
        //                {
        //                    featureCode = _crossLinkingCharDet.GenerateFeatuerCode();
        //                    newFeaturDesc = row["Feature"].ToString().Trim();
        //                }

        //            }
        //        }
        //        catch (Exception)
        //        {
        //            featureCode = _crossLinkingCharDet.GenerateFeatuerCode();
        //            if (((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim() != "")
        //                newFeaturDesc = ((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim();
        //        }

        //    }

        //    private void dgvCharacterstics_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        //    {
        //        rowDeleteChar = true;
        //        DataRowView rowFea = (DataRowView)dgvFeature.SelectedItem;
        //        if (rowFea.IsNotNullOrEmpty())
        //        {
        //            featureCode = rowFea["feature_code"].ToString();
        //            newFeaturDesc = rowFea["feature"].ToString();
        //        }
        //        DataRowView row = (DataRowView)dgvCharacterstics.SelectedItem;
        //        if (((DataRowView)dgvCharacterstics.SelectedItem).IsNotNullOrEmpty())
        //            if ((row["measuring_technique"].ToString().Trim() != "" || row["sample_size"].ToString().Trim() != "" || row["sample_frequency"].ToString().Trim() != "" || row["control_method"].ToString().Trim() != "" || row["reaction_plan"].ToString().Trim() != ""))
        //            {
        //                row["feature_code"] = featureCode;
        //                row["feature_desc"] = newFeaturDesc;

        //                var dvchar = ((DataView)dgvCharacterstics.ItemsSource);
        //                if (dvchar.Count > 0)
        //                {
        //                    bufferCharctdetails = ((DataView)dgvCharacterstics.ItemsSource).Table;
        //                    bufferCharctdetails.DefaultView.AddNew();

        //                    if (dvchar[dvchar.Count - 1]["measuring_technique"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_size"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_frequency"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["control_method"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["reaction_plan"].ToString().Trim() != "")
        //                    {
        //                        //        ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //                        DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //                        dr["feature_code"] = featureCode;
        //                        dr["feature_desc"] = newFeaturDesc;
        //                        // bufferCharctdetails.ImportRow(dr.Row);
        //                        //  dgvCharacterstics.ItemsSource = bufferCharctdetails.DefaultView;
        //                        lstAdded.Add(dr);
        //                    }
        //                   // dgvCharacterstics.Items.Refresh();
        //                }

        //            }
        //    }

        //    private void dgvFeature_MouseLeave(object sender, MouseEventArgs e)
        //    {
        //        if (dgvFeature.IsNotNullOrEmpty() && dgvFeature.Items.Count > 0 && ((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim() != "")
        //            newFeaturDesc = ((DataRowView)dgvFeature.SelectedItem).Row["Feature"].ToString().Trim();

        //    }
        //    private DataTable delCharacteristics = new DataTable();
        //    private DataTable delLinkedWith = new DataTable();
        //    private DataTable delProductType = new DataTable();
        //    private DataTable delFeature = new DataTable();
        //    private DataTable bufferCharctdetails = new DataTable();
        //    private void dgvCharacterstics_KeyDown(object sender, KeyEventArgs e)
        //    {
        //        if (Key.Delete == e.Key)
        //        {
        //            if (rowDeleteChar == false) return;
        //            try
        //            {
        //                MessageBoxResult messageBoxResult = MessageBoxResult.No;
        //                messageBoxResult = MessageBox.Show("Are you sure want to delete the Characteristics Detail?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
        //                if (messageBoxResult == MessageBoxResult.Yes)
        //                {
        //                    if (((DataRowView)dgvCharacterstics.SelectedItem).IsNotNullOrEmpty())
        //                    {
        //                        DataRowView row = (DataRowView)dgvCharacterstics.SelectedItem;
        //                        if (row["OPER_CODE"].ToString().Trim().IsNotNullOrEmpty() && row["FEATURE_CODE"].ToString().Trim().IsNotNullOrEmpty() && row["SNO"].ToString().Trim().IsNotNullOrEmpty())
        //                        {
        //                            delCharacteristics.ImportRow(row.Row);
        //                            row.Delete();
        //                            dgvCharacterstics.Items.Refresh();
        //                            if (((DataView)dgvCharacterstics.ItemsSource).Count == 0)
        //                            {
        //                                //((DataView)dgvCharacterstics.ItemsSource).Table.DefaultView.AddNew();
        //                                DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).Table.DefaultView.AddNew();
        //                                dr["feature_code"] = featureCode;
        //                                dr["feature_desc"] = newFeaturDesc;
        //                                lstAdded.Add(dr);
        //                                //DataRowView rowFeature = (DataRowView)dgvFeature.SelectedItem;
        //                                //rowFeature.Delete();

        //                            }
        //                        }

        //                    }
        //                }
        //            }
        //            catch (Exception)
        //            {

        //            }

        //        }

        //    }

        //    private void dgOperation_KeyDown(object sender, KeyEventArgs e)
        //    {
        //        try
        //        {
        //            if (Key.Delete == e.Key)
        //            {

        //                MessageBoxResult messageBoxResult = MessageBoxResult.No;
        //                messageBoxResult = MessageBox.Show("Are you sure want to delete the Classification?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
        //                if (messageBoxResult == MessageBoxResult.Yes)
        //                {
        //                    if (((DataRowView)dgOperation.SelectedItem).IsNotNullOrEmpty())
        //                    {
        //                        DataRowView row = (DataRowView)dgOperation.SelectedItem;
        //                        if (row["FEATURE_CODE"].ToString().Trim().IsNotNullOrEmpty() && row["PRD_CODE"].ToString().Trim().IsNotNullOrEmpty() && row["TYPE"].ToString().Trim().IsNotNullOrEmpty() && row["SUB_TYPE"].ToString().Trim().IsNotNullOrEmpty() && row["SNO"].ToString().Trim().IsNotNullOrEmpty())
        //                        {
        //                            delProductType.ImportRow(row.Row);
        //                            row.Delete();
        //                            dgOperation.Items.Refresh();
        //                        }

        //                    }
        //                }
        //            }
        //            else if ((Key.Tab == e.Key) || (Key.Enter == e.Key))
        //            {
        //                DataRowView row = (DataRowView)dgOperation.SelectedItem;

        //                DataView dgOperatioFSave = new DataView();
        //                if (((DataRowView)dgOperation.SelectedItem).IsNotNullOrEmpty())
        //                    dgOperatioFSave = (DataView)dgOperation.ItemsSource;
        //                if (dgOperatioFSave.Count > 0)
        //                    if (dgOperatioFSave[dgOperatioFSave.Count - 1]["SUBTYPE"].ToString() != "")
        //                    {
        //                        dgOperation.BeginEdit();
        //                        ((DataView)dgOperation.ItemsSource).Table.DefaultView.AddNew();
        //                        object item = dgOperation.Items[dgOperation.SelectedIndex + 1];
        //                        dgOperation.SelectedItem = item;
        //                        dgOperation.BeginEdit();

        //                    }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ex.LogException();
        //        }
        //    }

        //    private void dgvLinkedWith_KeyDown(object sender, KeyEventArgs e)
        //    {
        //        try
        //        {
        //            if (Key.Delete == e.Key)
        //            {
        //                MessageBoxResult messageBoxResult = MessageBoxResult.No;
        //                messageBoxResult = MessageBox.Show("Are you sure want to delete the Classification?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
        //                if (messageBoxResult == MessageBoxResult.Yes)
        //                {
        //                    if (((DataRowView)dgvLinkedWith.SelectedItem).IsNotNullOrEmpty())
        //                    {
        //                        DataRowView row = (DataRowView)dgvLinkedWith.SelectedItem;
        //                        if (row["FEATURE_CODE"].ToString().Trim().IsNotNullOrEmpty() && row["PRD_CODE"].ToString().Trim().IsNotNullOrEmpty() && row["TYPE"].ToString().Trim().IsNotNullOrEmpty() && row["SUB_TYPE"].ToString().Trim().IsNotNullOrEmpty() && row["SNO"].ToString().Trim().IsNotNullOrEmpty())
        //                        {
        //                            delLinkedWith.ImportRow(row.Row);
        //                            row.Delete();
        //                            dgvLinkedWith.Items.Refresh();
        //                        }
        //                        row.Delete();
        //                    }
        //                }
        //            }
        //            else if ((Key.Tab == e.Key) || (Key.Enter == e.Key))
        //            {
        //                DataRowView row = (DataRowView)dgvLinkedWith.SelectedItem;
        //                DataView dgLinkedWithFSave = new DataView();
        //                if (((DataRowView)dgvLinkedWith.SelectedItem).IsNotNullOrEmpty())
        //                    dgLinkedWithFSave = (DataView)dgvLinkedWith.ItemsSource;
        //                if (dgLinkedWithFSave.Count > 0)
        //                    if (dgLinkedWithFSave[dgLinkedWithFSave.Count - 1]["SUBTYPE"].ToString() != "")
        //                    {
        //                        dgvLinkedWith.BeginEdit();
        //                        ((DataView)dgvLinkedWith.ItemsSource).Table.DefaultView.AddNew();
        //                        object item = dgvLinkedWith.Items[dgvLinkedWith.SelectedIndex + 1];
        //                        dgvLinkedWith.SelectedItem = item;
        //                        dgvLinkedWith.BeginEdit();
        //                    }

        //            }
        //        }
        //        catch (Exception)
        //        {

        //        }
        //    }

        //    private void dgvFeature_KeyDown(object sender, KeyEventArgs e)
        //    {
        //        try
        //        {
        //            var dvchar = ((DataView)dgvFeature.ItemsSource);
        //            if (dvchar.Count > 0)
        //            {
        //                if (dvchar[dvchar.Count - 1]["Feature"].ToString().Trim() != "")
        //                {
        //                    DataRowView dr = ((DataView)dgvFeature.ItemsSource).AddNew();
        //                    //featureCode = _crossLinkingCharDet.GenerateFeatuerCode();
        //                    //dr["feature_code"] = featureCode;
        //                    //dr["Feature"] = newFeaturDesc;

        //                }
        //            }
        //            if (Key.Delete == e.Key)
        //            {
        //                if (((DataView)dgvCharacterstics.ItemsSource).Table.Rows.Count > 0)
        //                {
        //                    foreach (DataRow row in ((DataView)dgvCharacterstics.ItemsSource).Table.Rows)
        //                    {
        //                        delCharacteristics.ImportRow(row);
        //                    }
        //                }
        //            }

        //        }
        //        catch (Exception)
        //        {

        //        }

        //    }

        //    private void dgOperation_CurrentCellChanged(object sender, EventArgs e)
        //    {


        //    }

        //    private void cmbOpMaster_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //    {
        //        DataRowView row = (DataRowView)dgOperation.SelectedItem;

        //        DataView dgOperatioFSave = new DataView();
        //        if (((DataRowView)dgOperation.SelectedItem).IsNotNullOrEmpty())
        //            dgOperatioFSave = (DataView)dgOperation.ItemsSource;
        //        if (dgOperatioFSave.Count > 0)
        //            if (dgOperatioFSave[dgOperatioFSave.Count - 1]["SUBTYPE"].ToString() != "")
        //            {
        //                dgOperation.BeginEdit();
        //                ((DataView)dgOperation.ItemsSource).Table.DefaultView.AddNew();
        //                object item = dgOperation.Items[dgOperation.SelectedIndex + 1];
        //                dgOperation.SelectedItem = item;
        //                dgOperation.BeginEdit();

        //            }

        //    }

        //    private void cmbdgvLinkedWith_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //    {
        //        DataRowView row = (DataRowView)dgvLinkedWith.SelectedItem;
        //        DataView dgLinkedWithFSave = new DataView();
        //        if (((DataRowView)dgvLinkedWith.SelectedItem).IsNotNullOrEmpty())
        //            dgLinkedWithFSave = (DataView)dgvLinkedWith.ItemsSource;
        //        if (dgLinkedWithFSave.Count > 0)
        //            if (dgLinkedWithFSave[dgLinkedWithFSave.Count - 1]["SUBTYPE"].ToString() != "")
        //            {
        //                // ((DataView)dgvLinkedWith.ItemsSource).Table.DefaultView.AddNew();
        //                dgvLinkedWith.BeginEdit();
        //                ((DataView)dgvLinkedWith.ItemsSource).Table.DefaultView.AddNew();
        //                object item = dgvLinkedWith.Items[dgvLinkedWith.SelectedIndex + 1];
        //                dgvLinkedWith.SelectedItem = item;
        //                dgvLinkedWith.BeginEdit();
        //            }

        //    }
        //    private bool rowDeleteChar = true;
        //    private bool rowDeletePrdType = true;
        //    private bool rowDeleteLinkedWith = true;
        //    private void dgvCharacterstics_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        //    {
        //        rowDeleteChar = false;
        //        try
        //        {
        //            //    DataRowView rowFea = (DataRowView)dgvFeature.SelectedItem;
        //            //    if (rowFea.IsNotNullOrEmpty())
        //            //    {
        //            //        featureCode = rowFea["feature_code"].ToString();
        //            //        newFeaturDesc = rowFea["feature"].ToString();
        //            //    }

        //            //    DataRowView row = (DataRowView)dgvCharacterstics.SelectedItem;
        //            //    if (((DataRowView)dgvCharacterstics.SelectedItem).IsNotNullOrEmpty())
        //            //    {
        //            //        row["feature_code"] = featureCode;
        //            //        row["feature_desc"] = newFeaturDesc;
        //            //        row.EndEdit();
        //            //        if ((row["measuring_technique"].ToString().Trim() != "" || row["sample_size"].ToString().Trim() != "" || row["sample_frequency"].ToString().Trim() != "" || row["control_method"].ToString().Trim() != "" || row["reaction_plan"].ToString().Trim() != ""))
        //            //        {
        //            //            var dvchar = ((DataView)dgvCharacterstics.ItemsSource);
        //            //            if (dvchar.Count > 0)
        //            //            {
        //            //                bufferCharctdetails = ((DataView)dgvCharacterstics.ItemsSource).ToTable();
        //            //                if (dvchar[dvchar.Count - 1]["measuring_technique"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_size"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_frequency"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["control_method"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["reaction_plan"].ToString().Trim() != "")
        //            //                {
        //            //                    ((DataView)dgvCharacterstics.ItemsSource).Table.AcceptChanges();
        //            //                    //((DataView)dgvCharacterstics.ItemsSource).Table.DefaultView.AddNew();
        //            //                    DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //            //                    dr["feature_code"] = featureCode;
        //            //                    dr["feature_desc"] = newFeaturDesc;
        //            //                    lstAdded.Add(dr);
        //            //                }

        //            //            }

        //            //        }
        //            //    }
        //        }
        //        catch (Exception ex)
        //        {
        //            ex.LogException();
        //        }
        //    }

        //    private void dgvCharacterstics_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        //    {
        //        rowDeleteChar = true;
        //        //var dvchar = ((DataView)dgvCharacterstics.ItemsSource);
        //        //if (dvchar.Count > 0)
        //        //{
        //        //    if (dvchar[dvchar.Count - 1]["measuring_technique"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_size"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["sample_frequency"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["control_method"].ToString().Trim() != "" || dvchar[dvchar.Count - 1]["reaction_plan"].ToString().Trim() != "")
        //        //    {
        //        //        //((DataView)dgvCharacterstics.ItemsSource).Table.DefaultView.AddNew();
        //        //        DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //        //        dr["feature_code"] = featureCode;
        //        //        dr["feature_desc"] = newFeaturDesc;
        //        //        lstAdded.Add(dr);
        //        //    }

        //        //}
        //        //else if (dvchar.Count == 0)
        //        //{
        //        //    DataRowView dr = ((DataView)dgvCharacterstics.ItemsSource).AddNew();
        //        //    dr["feature_code"] = featureCode;
        //        //    dr["feature_desc"] = newFeaturDesc;
        //        //    lstAdded.Add(dr);
        //        //}
        //    }
        //    private string oldFeature = "";
        //    private string oldprdType = "";
        //    private string oldLinkedWith = "";
        //    private void dgvFeature_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        //    {
        //        var dvchar = ((DataView)dgvFeature.ItemsSource);
        //        if (dvchar[dvchar.Count - 1]["Feature"].ToString().Trim() != "")
        //        {
        //            DataRowView dr = ((DataView)dgvFeature.ItemsSource).AddNew();
        //            //featureCode = _crossLinkingCharDet.GenerateFeatuerCode();
        //            //dr["feature_code"] = featureCode;
        //            //dr["Feature"] = newFeaturDesc;

        //        }


        //        if (((System.Data.DataRowView)(e.Row.Item)).Row["Feature"].ToString().IsNotNullOrEmpty())
        //        {
        //            oldFeature = ((System.Data.DataRowView)(e.Row.Item)).Row["Feature"].ToString();
        //        }
        //        else
        //        {
        //            oldFeature = "";
        //        }
        //    }
        //    private void dgOperation_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        //    {
        //        rowDeletePrdType = false;
        //        dtTab = new DataTable();
        //        dtTab = ((DataView)dgOperation.ItemsSource).ToTable().Copy();
        //        if (((System.Data.DataRowView)(e.Row.Item)).Row["SUBTYPE"].ToString().IsNotNullOrEmpty())
        //        {
        //            oldprdType = ((System.Data.DataRowView)(e.Row.Item)).Row["SUBTYPE"].ToString();
        //        }
        //        else
        //        {
        //            oldprdType = "";
        //        }
        //    }
        //    DataTable dtTab = new DataTable();
        //    private void dgOperation_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        //    {
        //        rowDeletePrdType = true;

        //        string newValue = ((System.Data.DataRowView)(e.Row.Item)).Row["SUBTYPE"].ToString();

        //        if (dtTab.Select("SUBTYPE='" + newValue + "'").Length == 0 && oldprdType != "")
        //        {
        //            DataRowView row = (DataRowView)dgOperation.SelectedItem;
        //            if (row.IsNotNullOrEmpty())
        //            {
        //                row["SUBTYPE"] = oldprdType;
        //            }
        //        }
        //    }

        //    private void dgvLinkedWith_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        //    {
        //        string newValue = ((System.Data.DataRowView)(e.Row.Item)).Row["SUBTYPE"].ToString();

        //        if (dtTab.Select("SUBTYPE='" + newValue + "'").Length == 0)
        //        {
        //            DataRowView row = (DataRowView)dgvLinkedWith.SelectedItem;
        //            if (row.IsNotNullOrEmpty() && oldLinkedWith != "")
        //            {
        //                row["SUBTYPE"] = oldLinkedWith;
        //            }
        //        }

        //    }

        //    private void dgvLinkedWith_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        //    {
        //        dtTab = new DataTable();
        //        dtTab = ((DataView)dgvLinkedWith.ItemsSource).ToTable().Copy();
        //        if (((System.Data.DataRowView)(e.Row.Item)).Row["SUBTYPE"].ToString().IsNotNullOrEmpty())
        //        {
        //            oldLinkedWith = ((System.Data.DataRowView)(e.Row.Item)).Row["SUBTYPE"].ToString();
        //        }
        //        else
        //        {
        //            oldLinkedWith = "";
        //        }
        //    }


    }
}
