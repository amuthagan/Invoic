
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
using System.Windows.Controls;

namespace ProcessDesigner.ViewModel
{
    public class CrossLinkingViewModel : ViewModelBase
    {

        private CrossLinkingCharBll _crossLinkingBll;
        private string maxFeatureCode = "";
        private string newFeatureCode = "";
        private DataTable delCharacteristics = new DataTable();
        private DataTable delLinkedWith = new DataTable();
        private DataTable delProductType = new DataTable();
        private readonly ICommand _addCommand;
        public ICommand AddCommand { get { return this._addCommand; } }
        private readonly ICommand _editCommand;
        public ICommand EditCommand { get { return this._editCommand; } }
        private readonly ICommand _saveCommand;
        public ICommand SaveCommand { get { return this._saveCommand; } }
        private readonly ICommand _deleteCommandChar;
        public ICommand DeleteCommandChar { get { return this._deleteCommandChar; } }
        private readonly ICommand _deleteCommandDeleteFeature;
        public ICommand DeleteCommandFeature { get { return this._deleteCommandDeleteFeature; } }

        private readonly ICommand _deleteCommandPrdType;
        public ICommand DeleteCommandPrdType { get { return this._deleteCommandPrdType; } }
        private readonly ICommand _deleteCommandLnkWith;
        public ICommand DeleteCommandLnkWith { get { return this._deleteCommandLnkWith; } }
        private readonly ICommand _closeCommand;
        public ICommand CloseCommand { get { return this._closeCommand; } }

        private readonly ICommand _rowEditEndingTypeCommand;

        private readonly ICommand _rowEditEndingFeatureCommand;
        public ICommand RowEditEndingFeatureCommand { get { return this._rowEditEndingFeatureCommand; } }

        private readonly ICommand _rowEditEndingCharacteristicsCommand;
        public ICommand RowEditEndingCharacteristicsCommand { get { return this._rowEditEndingCharacteristicsCommand; } }

        private readonly ICommand _rowEditEndingClassificationPrdTypeCommand;
        public ICommand RowEditEndingClassificationPrdTypeCommand { get { return this._rowEditEndingClassificationPrdTypeCommand; } }

        private readonly ICommand _rowBeginEditClassificationPrdTypeCommand;
        public ICommand RowBeginEditClassificationPrdTypeCommand { get { return this._rowBeginEditClassificationPrdTypeCommand; } }

        private readonly ICommand _rowBeginEditCharacteristicsCommand;
        public ICommand RowBeginEditCharacteristicsCommand { get { return this._rowBeginEditCharacteristicsCommand; } }

        private readonly ICommand _rowEditEndingClassificationLinkedWithCommand;
        public ICommand RowEditEndingClassificationLinkedWithCommand { get { return this._rowEditEndingClassificationLinkedWithCommand; } }

        private readonly ICommand _onCloseCommand;
        public ICommand OnCloseCommand { get { return this._onCloseCommand; } }
        private bool _addButtonIsEnable = false;
        private bool _editButtonIsEnable = true;
        private bool _saveButtonIsEnable = true;
        private bool _deleteButtonIsEnable = false;
        private bool isSaved = false;
        public Action CloseAction { get; set; }

        public CrossLinkingViewModel(UserInformation userInformation)
        {
            this._crossLinkingCharModel = new CrossLinkingCharModel();
            _crossLinkingBll = new CrossLinkingCharBll(userInformation);
            this.selectChangeComboCommandType = new DelegateCommand(this.SelectDataRowType);
            this.selectChangeComboCommandPrdType = new DelegateCommand(this.SelectDataRowPrdType);
            this.selectChangeComboCommandLinkedWith = new DelegateCommand(this.SelectDataRowLinkedWith);
            this.selectChangeGrdCommandFeatureDetails = new DelegateCommand(this.SelectDataRowFeatureDetails);
            this.selectChangeGrdCommandCharDetails = new DelegateCommand(this.SelectDataRowCharDetails);
            this.selectChangeGrdCommandPrdTypeDetails = new DelegateCommand(this.SelectDataRowPrdTypeDetails);
            this.selectChangeGrdCommandLinkedWithDetails = new DelegateCommand(this.SelectDataRowLinkedWithDetails);
            this._rowEditEndingFeatureCommand = new DelegateCommand<DataRowView>(this.RowEditEndingFeature);
            this._rowEditEndingCharacteristicsCommand = new DelegateCommand<DataRowView>(this.RowEditEndingCharacteristics);
            this._rowEditEndingClassificationPrdTypeCommand = new DelegateCommand<DataRowView>(this.RowEditEndingClassificationPrdType);
            this._rowBeginEditClassificationPrdTypeCommand = new DelegateCommand<DataRowView>(this.RowBeginEditClassificationPrdType);

            this._rowBeginEditCharacteristicsCommand = new DelegateCommand<DataRowView>(this.RowBeginEditCharacteristics);

            this._rowEditEndingClassificationLinkedWithCommand = new DelegateCommand<DataRowView>(this.RowEditEndingClassificationLinkedWith);
            this._addCommand = new DelegateCommand(this.Add);
            this._editCommand = new DelegateCommand(this.Edit);
            this._saveCommand = new DelegateCommand(this.Save);
            this._deleteCommandDeleteFeature = new DelegateCommand<DataRowView>(this.DeleteFeature);
            this._deleteCommandChar = new DelegateCommand<DataRowView>(this.DeleteChar);
            this._deleteCommandPrdType = new DelegateCommand<DataRowView>(this.DeletePrdType);
            this._deleteCommandLnkWith = new DelegateCommand<DataRowView>(this.DeleteCharLnkWith);
            this._closeCommand = new DelegateCommand(this.Close);
            Add();
            maxFeatureCode = _crossLinkingBll.GenerateFeatuerCode();
            newFeatureCode = maxFeatureCode;
            SetdropDownItems();
        }

        private void RowBeginEditCharacteristics(DataRowView selecteditem)
        {
            if (selecteditem != null)
            {
                if (SelectedRowFeatureDetails.IsNotNullOrEmpty())
                    if (SelectedRowFeatureDetails["feature"].ToString().IsNotNullOrEmpty())
                    {
                        selecteditem.BeginEdit();
                        selecteditem["feature_desc"] = SelectedRowFeatureDetails["feature"].ToString();
                        selecteditem.EndEdit();
                    }
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
        private void RowBeginEditClassificationPrdType(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    DataRowView drv;
                    if (selecteditem["feature_code"].IsNotNullOrEmpty())
                    {
                        CrossLinkingCharModel.FeatureCode = selecteditem["feature_code"].ToString();
                        if (CrossLinkingCharModel.DtClassificationPrdType.Count > 0)
                        {
                            if (CrossLinkingCharModel.DtClassificationPrdType[CrossLinkingCharModel.DtClassificationPrdType.Count - 1]["SUBTYPE"].ToString() != "")
                            {
                                drv = CrossLinkingCharModel.DtClassificationPrdType.AddNew();
                                drv.BeginEdit();
                                drv["feature_code"] = selecteditem["feature_code"].ToString();
                                drv["type"] = selecteditem["type"].ToString();
                                drv["sub_type"] = selecteditem["sub_type"].ToString();
                                drv.EndEdit();
                            }
                        }
                        else if (CrossLinkingCharModel.DtClassificationPrdType.Count == 0)
                        {
                            drv = CrossLinkingCharModel.DtClassificationPrdType.AddNew();
                            drv.BeginEdit();
                            drv["feature_code"] = selecteditem["feature_code"].ToString();
                            drv["type"] = selecteditem["type"].ToString();
                            drv["sub_type"] = selecteditem["sub_type"].ToString();
                            drv.EndEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void DeleteCharLnkWith(DataRowView selecteditem)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBoxResult.No;
                messageBoxResult = MessageBox.Show("Are you sure want to delete the Classification?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (selecteditem.IsNotNullOrEmpty())
                    {
                        DataRowView row = selecteditem;

                        if (row["FEATURE_CODE"].ToString().Trim().IsNotNullOrEmpty() && row["PRD_CODE"].ToString().Trim().IsNotNullOrEmpty() && row["TYPE"].ToString().Trim().IsNotNullOrEmpty() && row["SUB_TYPE"].ToString().Trim().IsNotNullOrEmpty() && row["SNO"].ToString().Trim().IsNotNullOrEmpty())
                        {
                            delLinkedWith.ImportRow(row.Row);
                            row.Delete();
                            if (CrossLinkingCharModel.DtClassificationlinkedWith.Count == 0)
                            {
                                DataRowView drv;
                                drv = CrossLinkingCharModel.DtClassificationlinkedWith.AddNew();
                                drv.BeginEdit();
                                drv["feature_code"] = CrossLinkingCharModel.FeatureCode;
                                drv["type"] = CrossLinkingCharModel.Type;
                                drv["sub_type"] = CrossLinkingCharModel.ProductType;
                                drv["linked_with"] = CrossLinkingCharModel.LinkedWith;
                                drv.EndEdit();
                            }

                        }
                        row.Delete();
                    }
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private void DeletePrdType(DataRowView selecteditem)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBoxResult.No;
                messageBoxResult = MessageBox.Show("Are you sure want to delete the Classification?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (selecteditem.IsNotNullOrEmpty())
                    {
                        DataRowView row = selecteditem;
                        DataRowView drv;
                        if (row["FEATURE_CODE"].ToString().Trim().IsNotNullOrEmpty() && row["PRD_CODE"].ToString().Trim().IsNotNullOrEmpty() && row["TYPE"].ToString().Trim().IsNotNullOrEmpty() && row["SUB_TYPE"].ToString().Trim().IsNotNullOrEmpty() && row["SNO"].ToString().Trim().IsNotNullOrEmpty())
                        {
                            delProductType.ImportRow(row.Row);
                            row.Delete();
                            if (CrossLinkingCharModel.DtClassificationPrdType.Count == 0)
                            {
                                drv = CrossLinkingCharModel.DtClassificationPrdType.AddNew();
                                drv.BeginEdit();
                                drv["feature_code"] = CrossLinkingCharModel.FeatureCode;
                                drv["type"] = CrossLinkingCharModel.Type;
                                drv["sub_type"] = CrossLinkingCharModel.ProductType;
                                drv.EndEdit();
                            }
                        }
                        else if (!row["sno"].ToString().Trim().IsNotNullOrEmpty())
                        {
                            row.Delete();
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                ex.LogException();
            }
        }
        private void DeleteFeature(DataRowView selecteditem)
        {
            try
            {
                DataRowView row = selecteditem;
                if (CrossLinkingCharModel.DtFeatureDetails.Count > 1)
                {
                    row.Delete();
                }
                else if (CrossLinkingCharModel.DtFeatureDetails.Count == 1)
                {
                    row["feature_code"] = "";
                    row["Feature"] = "";
                }
            }
            catch (Exception)
            {

            }
        }
        private void DeleteChar(DataRowView selecteditem)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBoxResult.No;
                messageBoxResult = MessageBox.Show("Are you sure want to delete the Characteristics Detail?", ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (selecteditem.IsNotNullOrEmpty())
                    {
                        DataRowView row = selecteditem;
                        DataRowView drv;
                        if (row["OPER_CODE"].ToString().Trim().IsNotNullOrEmpty() && row["FEATURE_CODE"].ToString().Trim().IsNotNullOrEmpty() && row["SNO"].ToString().Trim().IsNotNullOrEmpty())
                        {
                            delCharacteristics.ImportRow(row.Row);
                            row.Delete();
                            if (CrossLinkingCharModel.DtCharacteristicsDetails.Count == 0)
                            {
                                drv = CrossLinkingCharModel.DtCharacteristicsDetails.AddNew();
                                drv.BeginEdit();
                                drv["feature_code"] = SelectedRowFeatureDetails["feature_code"];
                                drv["feature_desc"] = SelectedRowFeatureDetails["feature"];
                                drv["oper_code"] = (CrossLinkingCharModel.DtCharacteristicsDetails[0]["oper_code"].ToString().Trim());
                                drv.EndEdit();

                            }
                        }
                        else
                        {
                            if (row["OPER_CODE"].ToString().Trim() == "0")
                            {
                                row.Delete();
                                if (CrossLinkingCharModel.DtCharacteristicsDetails.Count > 0)
                                {
                                    if (SelectedRowFeatureDetails["feature_code"].ToString() != "" && (CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["measuring_technique"].ToString().Trim() != "" || CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["sample_size"].ToString().Trim() != "" || CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["sample_frequency"].ToString().Trim() != "" || CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["control_method"].ToString().Trim() != "" || CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["reaction_plan"].ToString().Trim() != ""))
                                    {
                                        if (CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["feature_code"].ToString() != "")
                                        {
                                            drv = CrossLinkingCharModel.DtCharacteristicsDetails.AddNew();
                                            drv.BeginEdit();
                                            drv["feature_code"] = CrossLinkingCharModel.FeatureCode.Trim();
                                            drv["oper_code"] = 0;
                                            drv.EndEdit();
                                        }
                                    }
                                }
                                else if (CrossLinkingCharModel.DtCharacteristicsDetails.Count == 0)
                                {
                                    drv = CrossLinkingCharModel.DtCharacteristicsDetails.AddNew();
                                    drv.BeginEdit();
                                    drv["feature_code"] = CrossLinkingCharModel.FeatureCode.Trim();
                                    drv["oper_code"] = 0;
                                    drv.EndEdit();
                                }


                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private DataRowView _featureSelectedItem = null;
        public DataRowView FeatureSelectedItem
        {
            get { return _featureSelectedItem; }
            set
            {
                _featureSelectedItem = value;
                NotifyPropertyChanged("FeatureSelectedItem");
            }
        }

        private DataRowView _charSelectedItem = null;
        public DataRowView CharacterSelectedItem
        {
            get { return _charSelectedItem; }
            set
            {
                _charSelectedItem = value;
                NotifyPropertyChanged("CharacterSelectedItem");
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

        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void Save()
        {
            string featuCode = "";
            if (SaveButtonIsEnable == false) return;
            if (!CrossLinkingCharModel.Type.ToString().Trim().IsNotNullOrEmpty())
            {
                ShowInformationMessage("Type Should not be Empty");
                CategoryIsFocused = true;
                return;
            }
            else if (!CrossLinkingCharModel.ProductType.ToString().Trim().IsNotNullOrEmpty())
            {
                ShowInformationMessage("Product Type Should not be Empty");
                CategoryIsFocused = true;
                return;
            }
            CategoryIsFocused = true;
            if (SelectedRowFeatureDetails.IsNotNullOrEmpty())
            {
                featuCode = SelectedRowFeatureDetails["feature_code"].ToString();
            }
            Progress.ProcessingText = PDMsg.ProgressUpdateText;
            Progress.Start();
            try
            {
                CrossLinkingCharModel.DtCharacteristicsDetails.Table.AcceptChanges();
                //CrossLinkingCharModel.DtCharacteristicsDetails.RowFilter = string.Empty;
                CrossLinkingCharModel.DtCharacteristicsDetails.RowFilter = "oper_code = 0";
                for (int i = 0; i < CrossLinkingCharModel.DtCharacteristicsDetails.Count; i++)
                {
                    CrossLinkingCharModel.DtFeatureDetails.RowFilter = "feature_code='" + CrossLinkingCharModel.DtCharacteristicsDetails[i]["feature_code"] + "'";
                    if (CrossLinkingCharModel.DtFeatureDetails.Count > 0 && CrossLinkingCharModel.DtFeatureDetails[0]["Feature"].ToString().IsNotNullOrEmpty())
                        CrossLinkingCharModel.DtCharacteristicsDetails[i]["feature_desc"] = CrossLinkingCharModel.DtFeatureDetails[0]["Feature"];
                }
                CrossLinkingCharModel.DtCharacteristicsDetails.Table.AcceptChanges();
                CrossLinkingCharModel.DtCharacteristicsDetails.RowFilter = "oper_code = 0 and feature_desc <> ''";
                _crossLinkingBll.SaveCharacterMasters(CrossLinkingCharModel.DtCharacteristicsDetails, CrossLinkingCharModel.Type, CrossLinkingCharModel.ProductType, CrossLinkingCharModel.LinkedWith);

            }
            catch (Exception ex)
            {
                ex.LogException();

                if (SelectedRowFeatureDetails.IsNotNullOrEmpty())
                {
                    CrossLinkingCharModel.DtCharacteristicsDetails.RowFilter = "feature_code='" + SelectedRowFeatureDetails["feature_code"].ToString() + "'";
                    CrossLinkingCharModel.DtClassificationPrdType.Table.AcceptChanges();
                }

            }
            try
            {

                CrossLinkingCharModel.DtCharacteristicsDetails.RowFilter = "oper_code <> 0";
                _crossLinkingBll.UpdateCharacteristicsmaster(CrossLinkingCharModel.DtCharacteristicsDetails);
                CrossLinkingCharModel.DtCharacteristicsDetails.Table.AcceptChanges();
            }
            catch (Exception ex)
            {
                ex.LogException();
                CrossLinkingCharModel.DtCharacteristicsDetails.RowFilter = "feature_code='" + SelectedRowFeatureDetails["feature_code"].ToString() + "'";
                CrossLinkingCharModel.DtClassificationPrdType.Table.AcceptChanges();
            }

            try
            {
                CrossLinkingCharModel.DtClassificationPrdType.RowFilter = "SUBTYPE <> '' and sno is null";
                if (CrossLinkingCharModel.DtClassificationPrdType.IsNotNullOrEmpty())
                {
                    if (CrossLinkingCharModel.DtClassificationPrdType.Count > 0)
                        _crossLinkingBll.SaveForginMaster(CrossLinkingCharModel.DtClassificationPrdType, CrossLinkingCharModel.Type, CrossLinkingCharModel.ProductType, CrossLinkingCharModel.LinkedWith);

                    CrossLinkingCharModel.DtClassificationPrdType.RowFilter = "SUBTYPE <> '' and sno is not null and feature_code='" + featuCode + "'";

                    if (CrossLinkingCharModel.DtClassificationPrdType.Count > 0)
                        _crossLinkingBll.UpdateForginMaster(CrossLinkingCharModel.DtClassificationPrdType, CrossLinkingCharModel.Type, CrossLinkingCharModel.ProductType, CrossLinkingCharModel.LinkedWith);

                }
                CrossLinkingCharModel.DtClassificationPrdType.RowFilter = "feature_code='" + featuCode + "'";

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
            try
            {

                CrossLinkingCharModel.DtClassificationlinkedWith.RowFilter = "SUBTYPE <> '' and sno is null";
                if (CrossLinkingCharModel.DtClassificationlinkedWith.IsNotNullOrEmpty())
                {
                    if (CrossLinkingCharModel.DtClassificationlinkedWith.Count > 0)
                        _crossLinkingBll.SaveFastnersMaster(CrossLinkingCharModel.DtClassificationlinkedWith, CrossLinkingCharModel.Type, CrossLinkingCharModel.ProductType, CrossLinkingCharModel.LinkedWith);
                    CrossLinkingCharModel.DtClassificationlinkedWith.RowFilter = "SUBTYPE <> '' and sno is not null and feature_code='" + featuCode + "'";
                    if (CrossLinkingCharModel.DtClassificationlinkedWith.Count > 0)
                        _crossLinkingBll.UpdateFastnersMaster(CrossLinkingCharModel.DtClassificationlinkedWith, CrossLinkingCharModel.Type, CrossLinkingCharModel.ProductType, CrossLinkingCharModel.LinkedWith);

                }
                CrossLinkingCharModel.DtClassificationlinkedWith.RowFilter = "feature_code='" + featuCode + "'";

            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            try
            {
                foreach (DataRow row in delCharacteristics.Rows)
                {
                    _crossLinkingBll.DeleteFeatureCharacteristicsDetails(row["OPER_CODE"].ToString().Trim().ToDecimalValue(), row["FEATURE_CODE"].ToString().Trim(), row["SNO"].ToString().Trim().ToDecimalValue());
                }
                foreach (DataRow row in delProductType.Rows)
                {
                    _crossLinkingBll.DeletePrdCategorybasedDetails(row["FEATURE_CODE"].ToString().Trim(), row["PRD_CODE"].ToString().Trim(), row["TYPE"].ToString().Trim(), row["SUB_TYPE"].ToString().Trim(), row["SNO"].ToString().Trim().ToDecimalValue());
                }
                foreach (DataRow row in delLinkedWith.Rows)
                {
                    _crossLinkingBll.DeleteLnkedWithbasedDetails(row["FEATURE_CODE"].ToString().Trim(), row["PRD_CODE"].ToString().Trim(), row["TYPE"].ToString().Trim(), row["SUB_TYPE"].ToString().Trim(), row["linked_with"].ToString().Trim(), row["SNO"].ToString().Trim().ToDecimalValue());
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }


            ////Clear
            //if (CrossLinkingCharModel.DtFeatureDetails.Count > 0)
            //{
            //    SelectedRowFeatureDetails = CrossLinkingCharModel.DtFeatureDetails[0];

            //}
            //// _crossLinkingBll.GetGridFeature(CrossLinkingCharModel);
            //_crossLinkingBll.GetGridCharacteristicsMaster(CrossLinkingCharModel);
            //CrossLinkingCharModel.DtCharacteristicsDetails.Table.AcceptChanges();
            //CrossLinkingCharModel.DtCharacteristicsDetails.RowFilter = "feature_code='" + SelectedRowFeatureDetails["feature_code"].ToString() + "'";

            //_crossLinkingBll.GetGridDetailsProdCategCross(CrossLinkingCharModel);
            //CrossLinkingCharModel.DtClassificationPrdType.Table.AcceptChanges();
            //CrossLinkingCharModel.DtClassificationPrdType.RowFilter = "feature_code='" + SelectedRowFeatureDetails["feature_code"].ToString() + "'";

            //_crossLinkingBll.GetGridDetailsLinkedWithCross(CrossLinkingCharModel);
            //CrossLinkingCharModel.DtClassificationlinkedWith.Table.AcceptChanges();
            //CrossLinkingCharModel.DtClassificationlinkedWith.RowFilter = "feature_code='" + SelectedRowFeatureDetails["feature_code"].ToString() + "'";
            Add();
            Progress.End();
            ShowInformationMessage(PDMsg.SavedSuccessfully);

        }

        private void Edit()
        {
            //throw new NotImplementedException();
        }
        private bool _categoryisfocused = false;
        public bool CategoryIsFocused
        {
            get { return _categoryisfocused; }
            set
            {
                _categoryisfocused = value;
                NotifyPropertyChanged("CategoryIsFocused");
            }
        }
        private void Add()
        {
            try
            {
                _crossLinkingBll.GetTypeCmb(CrossLinkingCharModel);
                CrossLinkingCharModel.Type = string.Empty;
                CrossLinkingCharModel.LinkedWith = string.Empty;
                CrossLinkingCharModel.ProductType = string.Empty;
                _crossLinkingBll.GetGridFeature(CrossLinkingCharModel);
                _crossLinkingBll.GetGridCharacteristicsMaster(CrossLinkingCharModel);
                _crossLinkingBll.GetGridDetailsProdCategCross(CrossLinkingCharModel);
                _crossLinkingBll.GetGridDetailsLinkedWithCross(CrossLinkingCharModel);
                if (CrossLinkingCharModel.DtCharacteristicsDetails.Count > 0)
                {
                    CrossLinkingCharModel.DtCharacteristicsDetails.Table.AcceptChanges();
                    CrossLinkingCharModel.DtCharacteristicsDetails.RowFilter = "feature_code='0'";
                }
                if (CrossLinkingCharModel.DtClassificationPrdType.Count > 0)
                {
                    CrossLinkingCharModel.DtClassificationPrdType.Table.AcceptChanges();
                    CrossLinkingCharModel.DtClassificationPrdType.RowFilter = "type='" + CrossLinkingCharModel.Type + "' and sub_type='" + CrossLinkingCharModel.ProductType + "' and Feature_Code='" + CrossLinkingCharModel.FeatureCode + "' ";
                }
                if (CrossLinkingCharModel.DtClassificationlinkedWith.Count > 0)
                {
                    CrossLinkingCharModel.DtClassificationlinkedWith.Table.AcceptChanges();
                    CrossLinkingCharModel.DtClassificationlinkedWith.RowFilter = "type='" + CrossLinkingCharModel.Type + "' and sub_type='" + CrossLinkingCharModel.ProductType + "' and Feature_Code='" + CrossLinkingCharModel.FeatureCode + "' ";
                }
                GetRights();
                AddButtonIsEnable = true;
                setRights();
                if (CrossLinkingCharModel.ProductTypeH.ToString().ToUpper() != "SUBTYPE")
                    CrossLinkingCharModel.ProductTypeH = "SUBTYPE";
                if (CrossLinkingCharModel.LinkedWithH.ToString().ToUpper() != "SUBTYPE")
                    CrossLinkingCharModel.LinkedWithH = "SUBTYPE";
                CategoryIsFocused = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void RowEditEndingFeature(DataRowView selecteditem)
        {
            try
            {
                //CrossLinkingCharModel.FeatureName = selecteditem["feature"].ToString();
                if (selecteditem != null)
                {
                    CrossLinkingCharModel.DtFeatureDetails.Table.AcceptChanges();
                    if (selecteditem["feature"].ToString().IsNotNullOrEmpty() && CrossLinkingCharModel.DtFeatureDetails.Count > 1)
                    {
                        CrossLinkingCharModel.DtFeatureDetails.RowFilter = "feature='" + selecteditem["feature"].ToString() + "' ";
                        if (CrossLinkingCharModel.DtFeatureDetails.Count > 1)
                        {
                            CrossLinkingCharModel.DtFeatureDetails.RowFilter = string.Empty;
                            ShowInformationMessage(PDMsg.AlreadyExists("Feature Name"));
                            SelectedRowFeatureDetails = selecteditem;
                            selecteditem.BeginEdit();
                            selecteditem["feature"] = "";
                            selecteditem.EndEdit();
                            return;
                        }
                        CrossLinkingCharModel.DtFeatureDetails.RowFilter = string.Empty;
                    }
                    DataRowView drv;
                    if (selecteditem["feature"].ToString() != "")
                    {
                        if (CrossLinkingCharModel.DtFeatureDetails[CrossLinkingCharModel.DtFeatureDetails.Count - 1]["feature"].ToString() != "")
                        {
                            drv = CrossLinkingCharModel.DtFeatureDetails.AddNew();
                            drv.BeginEdit();
                            newFeatureCode = "F" + (Convert.ToInt32(newFeatureCode.Replace("F", "").Trim()) + 1).ToString("0000");
                            drv["feature_code"] = newFeatureCode;
                            drv.EndEdit();
                            CrossLinkingCharModel.FeatureCode = newFeatureCode;
                            CrossLinkingCharModel.FeatureName = selecteditem["feature"].ToString();

                        }
                    }
                    else if (selecteditem["feature"].ToString() == "")
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Feature"));
                        selecteditem["feature"] = oldFeatureNam;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private string oldFeatureNam = "";
        private void RowEditEndingCharacteristics(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    selecteditem.Row.BeginEdit();
                    selecteditem.Row["feature_desc"] = CrossLinkingCharModel.FeatureName;
                    selecteditem.EndEdit();
                    DataRowView drv;
                    if (selecteditem["feature_code"].ToString() != "" && (CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["measuring_technique"].ToString().Trim() != "" || CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["sample_size"].ToString().Trim() != "" || CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["sample_frequency"].ToString().Trim() != "" || CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["control_method"].ToString().Trim() != "" || CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["reaction_plan"].ToString().Trim() != ""))
                    {
                        if (CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["feature_code"].ToString() != "")
                        {
                            drv = CrossLinkingCharModel.DtCharacteristicsDetails.AddNew();
                            drv.BeginEdit();
                            drv["feature_code"] = SelectedRowFeatureDetails["feature_code"];
                            drv["feature_desc"] = SelectedRowFeatureDetails["feature"];
                            drv["oper_code"] = (CrossLinkingCharModel.DtCharacteristicsDetails[0]["oper_code"].ToString().Trim());
                            drv.EndEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private void RowEditEndingClassificationPrdType(DataRowView selecteditem)
        {
            try
            {
                DataView dvTemp = CrossLinkingCharModel.DtClassificationCmbPrdType.Table.Clone().DefaultView;
                if (selecteditem != null)
                {
                    DataRowView drv;
                    if (selecteditem["feature_code"].IsNotNullOrEmpty())
                    {
                        CrossLinkingCharModel.DtClassificationCmbPrdType.Table.AcceptChanges();
                        CrossLinkingCharModel.FeatureCode = selecteditem["feature_code"].ToString();
                        //CrossLinkingCharModel.DtClassificationCmbPrdType.RowFilter = "SUBTYPE='" + selecteditem["SUBTYPE"].ToString() + "'";
                        //if (CrossLinkingCharModel.DtClassificationCmbPrdType.Count == 0)
                        //{
                        //    selecteditem["SUBTYPE"] = "";
                        //}
                        // CrossLinkingCharModel.DtClassificationCmbPrdType.Table.AcceptChanges();
                        // CrossLinkingCharModel.DtClassificationCmbPrdType.RowFilter = string.Empty;
                        dvTemp = CrossLinkingCharModel.DtClassificationCmbPrdType.Table.Copy().DefaultView;

                        dvTemp.RowFilter = "SUBTYPE='" + selecteditem["SUBTYPE"].ToString() + "'";
                        if (dvTemp.Count == 0)
                        {
                            selecteditem["SUBTYPE"] = "";
                        }
                        dvTemp.Table.AcceptChanges();
                        dvTemp.RowFilter = string.Empty;
                        if (CrossLinkingCharModel.DtClassificationPrdType.Count > 0)
                        {
                            if (CrossLinkingCharModel.DtClassificationPrdType[CrossLinkingCharModel.DtClassificationPrdType.Count - 1]["SUBTYPE"].ToString() != "")
                            {
                                drv = CrossLinkingCharModel.DtClassificationPrdType.AddNew();
                                drv.BeginEdit();
                                drv["feature_code"] = selecteditem["feature_code"].ToString();
                                drv["type"] = selecteditem["type"].ToString();
                                drv["sub_type"] = selecteditem["sub_type"].ToString();
                                drv.EndEdit();
                            }
                        }
                        else if (CrossLinkingCharModel.DtClassificationPrdType.Count == 0)
                        {
                            drv = CrossLinkingCharModel.DtClassificationPrdType.AddNew();
                            drv.BeginEdit();
                            drv["feature_code"] = selecteditem["feature_code"].ToString();
                            drv["type"] = selecteditem["type"].ToString();
                            drv["sub_type"] = selecteditem["sub_type"].ToString();
                            drv.EndEdit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private void RowEditEndingClassificationLinkedWith(DataRowView selecteditem)
        {
            DataView dvLink = CrossLinkingCharModel.DtClassificationCmblinkedWith.Table.Clone().DefaultView;
            try
            {
                if (selecteditem != null)
                {
                    DataRowView drv;
                    if (selecteditem["feature_code"].IsNotNullOrEmpty())
                    {
                        dvLink = CrossLinkingCharModel.DtClassificationCmblinkedWith.Table.Copy().DefaultView;
                        CrossLinkingCharModel.FeatureCode = selecteditem["feature_code"].ToString();
                        dvLink.RowFilter = "SUBTYPE='" + selecteditem["SUBTYPE"].ToString() + "'";
                        if (dvLink.Count == 0)
                        {
                            selecteditem["SUBTYPE"] = "";
                        }
                        dvLink.RowFilter = string.Empty;

                        if (CrossLinkingCharModel.DtClassificationlinkedWith.Count > 0)
                        {
                            if (CrossLinkingCharModel.DtClassificationlinkedWith[CrossLinkingCharModel.DtClassificationlinkedWith.Count - 1]["SUBTYPE"].ToString() != "")
                            {
                                drv = CrossLinkingCharModel.DtClassificationlinkedWith.AddNew();
                                drv.BeginEdit();
                                drv["feature_code"] = selecteditem["feature_code"].ToString();
                                drv["type"] = selecteditem["type"].ToString();
                                drv["sub_type"] = selecteditem["sub_type"].ToString();
                                drv["linked_with"] = selecteditem["linked_with"].ToString();
                                drv.EndEdit();
                            }
                        }
                        else if (CrossLinkingCharModel.DtClassificationlinkedWith.Count == 0)
                        {
                            drv = CrossLinkingCharModel.DtClassificationlinkedWith.AddNew();
                            drv.BeginEdit();
                            drv["feature_code"] = selecteditem["feature_code"].ToString();
                            drv["type"] = selecteditem["type"].ToString();
                            drv["sub_type"] = selecteditem["sub_type"].ToString();
                            drv["linked_with"] = selecteditem["linked_with"].ToString();
                            drv.EndEdit();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        //Button
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
            ActionPermission = _crossLinkingBll.GetUserRights("CHARACTERISTICS GROUPING MASTER");
            if (ActionPermission.AddNew == false && ActionPermission.Edit == false)
            {
                ActionPermission.Save = false;
            }
        }

        private void setRights()
        {
            if (AddButtonIsEnable) AddButtonIsEnable = ActionPermission.AddNew;
            if (EditButtonIsEnable) EditButtonIsEnable = ActionPermission.Edit;
            if (SaveButtonIsEnable) SaveButtonIsEnable = ActionPermission.Edit;

        }
        private CrossLinkingCharModel _crossLinkingCharModel;
        public CrossLinkingCharModel CrossLinkingCharModel
        {
            get
            {
                return _crossLinkingCharModel;
            }
            set
            {
                this._crossLinkingCharModel = value;
                NotifyPropertyChanged("CrossLinkingCharModel");
            }
        }

        private DataRowView _selectedrowType;
        public DataRowView SelectedRowType
        {
            get
            {
                return _selectedrowType;
            }

            set
            {
                _selectedrowType = value;
                NotifyPropertyChanged("SelectedRowType");
            }
        }

        private readonly ICommand selectChangeComboCommandType;
        public ICommand SelectChangeComboCommandType { get { return this.selectChangeComboCommandType; } }
        private void SelectDataRowType()
        {
            if (SelectedRowType != null)
            {
                CrossLinkingCharModel.Type = SelectedRowType["PRODUCT_CATEGORY"].ToString();
                if (CrossLinkingCharModel.Type.IsNotNullOrEmpty())
                {
                    CrossLinkingCharModel.ProductType = "";
                    CrossLinkingCharModel.LinkedWith = "";

                    _crossLinkingBll.GetProductTypeCmb(CrossLinkingCharModel);
                    _crossLinkingBll.GetLinkedTypeCmb(CrossLinkingCharModel);
                    _crossLinkingBll.GetGridFeature(CrossLinkingCharModel);
                    _crossLinkingBll.GetGridCharacteristicsMaster(CrossLinkingCharModel);
                    CrossLinkingCharModel.DtCharacteristicsDetails.Table.AcceptChanges();
                    CrossLinkingCharModel.DtCharacteristicsDetails.RowFilter = "feature_code=''";

                    _crossLinkingBll.GetGridDetailsProdCategCross(CrossLinkingCharModel);
                    CrossLinkingCharModel.DtClassificationPrdType.Table.AcceptChanges();
                    CrossLinkingCharModel.DtClassificationPrdType.RowFilter = "type='' and sub_type='' and Feature_Code='' ";

                    _crossLinkingBll.GetGridDetailsLinkedWithCross(CrossLinkingCharModel);
                    CrossLinkingCharModel.DtClassificationlinkedWith.Table.AcceptChanges();
                    CrossLinkingCharModel.DtClassificationlinkedWith.RowFilter = "type='' and sub_type='' and Feature_Code='' ";
                }
            }
        }


        //Prd Type
        private DataRowView _selectedrowPrdType;
        public DataRowView SelectedRowPrdType
        {
            get
            {
                return _selectedrowPrdType;
            }

            set
            {
                _selectedrowPrdType = value;
                NotifyPropertyChanged("SelectedRowPrdType");
            }
        }

        private readonly ICommand selectChangeComboCommandPrdType;
        public ICommand SelectChangeComboCommandPrdType { get { return this.selectChangeComboCommandPrdType; } }
        private void SelectDataRowPrdType()
        {
            if (SelectedRowPrdType != null)
            {
                CrossLinkingCharModel.ProductType = SelectedRowPrdType["TYPE"].ToString();
                if (SelectedRowPrdType["TYPE"].ToString().IsNotNullOrEmpty())
                    CrossLinkingCharModel.ProductTypeH = SelectedRowPrdType["TYPE"].ToString();
                if (CrossLinkingCharModel.Type.IsNotNullOrEmpty() && CrossLinkingCharModel.ProductType.IsNotNullOrEmpty())
                {
                    _crossLinkingBll.GetGridFeature(CrossLinkingCharModel);
                    if (CrossLinkingCharModel.DtFeatureDetails.Count > 0)
                    {
                        SelectedRowFeatureDetails = CrossLinkingCharModel.DtFeatureDetails[0];

                    }
                    _crossLinkingBll.GetGridComboProdCategCross(CrossLinkingCharModel);
                    _crossLinkingBll.GetGridComboLinkedWithCross(CrossLinkingCharModel);

                    delCharacteristics = CrossLinkingCharModel.DtCharacteristicsDetails.ToTable().Copy().Clone();
                    delLinkedWith = CrossLinkingCharModel.DtClassificationlinkedWith.ToTable().Copy().Clone();
                    delProductType = CrossLinkingCharModel.DtClassificationPrdType.ToTable().Copy().Clone();

                }
            }
        }
        //Linked With
        private DataRowView _selectedrowLinkedWith;
        public DataRowView SelectedRowLinkedWith
        {
            get
            {
                return _selectedrowLinkedWith;
            }

            set
            {
                _selectedrowLinkedWith = value;
                NotifyPropertyChanged("SelectedRowLinkedWith");
            }
        }

        private readonly ICommand selectChangeComboCommandLinkedWith;
        public ICommand SelectChangeComboCommandLinkedWith { get { return this.selectChangeComboCommandLinkedWith; } }
        private void SelectDataRowLinkedWith()
        {
            if (SelectedRowLinkedWith != null)
            {
                CrossLinkingCharModel.LinkedWith = SelectedRowLinkedWith["SUBTYPE"].ToString();
                if (SelectedRowLinkedWith["SUBTYPE"].ToString().IsNotNullOrEmpty())
                    CrossLinkingCharModel.LinkedWithH = SelectedRowLinkedWith["SUBTYPE"].ToString();
                // if (CrossLinkingCharModel.LinkedWith.Trim().ToString().ToUpper() != "SUBTYPE") CrossLinkingCharModel.HeaderLinkedWith = CrossLinkingCharModel.LinkedWith;
                if (SelectedRowLinkedWith != null)
                {
                    if (CrossLinkingCharModel.Type.IsNotNullOrEmpty() && CrossLinkingCharModel.ProductType.IsNotNullOrEmpty() && CrossLinkingCharModel.LinkedWith.IsNotNullOrEmpty())
                    {
                        _crossLinkingBll.GetGridComboLinkedWithCross(CrossLinkingCharModel);
                        _crossLinkingBll.GetGridDetailsLinkedWithCross(CrossLinkingCharModel);
                    }
                    if (CrossLinkingCharModel.DtClassificationlinkedWith.Count > 0)
                    {
                        CrossLinkingCharModel.DtClassificationlinkedWith.Table.AcceptChanges();
                        CrossLinkingCharModel.DtClassificationlinkedWith.RowFilter = "type='" + CrossLinkingCharModel.Type + "' and sub_type='" + CrossLinkingCharModel.ProductType + "' and Feature_Code='" + CrossLinkingCharModel.FeatureCode + "' and Linked_with='" + CrossLinkingCharModel.LinkedWith + "' ";
                        if (CrossLinkingCharModel.DtClassificationlinkedWith.Count > 0)
                        {
                            if (CrossLinkingCharModel.DtClassificationlinkedWith[CrossLinkingCharModel.DtClassificationlinkedWith.Count - 1]["SUBTYPE"].ToString() != "")
                            {
                                DataRowView drv = CrossLinkingCharModel.DtClassificationlinkedWith.AddNew();
                                drv.BeginEdit();
                                drv["feature_code"] = CrossLinkingCharModel.FeatureCode;
                                drv["type"] = CrossLinkingCharModel.Type;
                                drv["sub_type"] = CrossLinkingCharModel.ProductType;
                                drv["Linked_with"] = CrossLinkingCharModel.LinkedWith;
                                drv.EndEdit();
                            }
                        }
                        else if (CrossLinkingCharModel.DtClassificationlinkedWith.Count == 0)
                        {
                            DataRowView drv = CrossLinkingCharModel.DtClassificationlinkedWith.AddNew();
                            drv.BeginEdit();
                            drv["feature_code"] = CrossLinkingCharModel.FeatureCode;
                            drv["type"] = CrossLinkingCharModel.Type;
                            drv["sub_type"] = CrossLinkingCharModel.ProductType;
                            drv["Linked_with"] = CrossLinkingCharModel.LinkedWith;
                            drv.EndEdit();

                        }
                        CrossLinkingCharModel.DtClassificationlinkedWith.RowFilter = "type='" + CrossLinkingCharModel.Type + "' and sub_type='" + CrossLinkingCharModel.ProductType + "' and Feature_Code='" + CrossLinkingCharModel.FeatureCode + "' and Linked_with='" + CrossLinkingCharModel.LinkedWith + "' ";
                    }
                }
            }
        }
        public void OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            string featuNm = ((System.Data.DataRowView)(e.Row.Item)).Row["feature"].ToString();
            if (e.Column.Header.ToString() == "Feature") oldFeatureNam = featuNm;

        }
        public void OnBeginningEditCharacter(object sender, DataGridBeginningEditEventArgs e)
        {

        }
        public void OnBeginningEditDtPrdType(object sender, DataGridBeginningEditEventArgs e)
        {
            //try
            //{
            //    if (selecteditem != null)
            //    {
            //        DataRowView drv;
            //        if (selecteditem["feature_code"].ToString() != "")
            //        {
            //            if (_crossLinkingCharModel.DtClassificationPrdType[_crossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["feature_code"].ToString() != "")
            //            {
            //                drv = _crossLinkingCharModel.DtClassificationPrdType.AddNew();
            //                drv.BeginEdit();
            //                drv["feature_code"] = _crossLinkingCharModel.FeatureCode;
            //                drv.EndEdit();
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex.LogException();
            //}
        }
        public void OnBeginningEditDtLinkedWith(object sender, DataGridBeginningEditEventArgs e)
        {
            //try
            //{
            //    if (selecteditem != null)
            //    {
            //        DataRowView drv;
            //        if (selecteditem["feature_code"].ToString() != "")
            //        {
            //            if (_crossLinkingCharModel.DtClassificationlinkedWith[_crossLinkingCharModel.DtClassificationlinkedWith.Count - 1]["feature_code"].ToString() != "")
            //            {
            //                drv = _crossLinkingCharModel.DtClassificationlinkedWith.AddNew();
            //                drv.BeginEdit();
            //                drv["feature_code"] = _crossLinkingCharModel.FeatureCode;
            //                drv.EndEdit();
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex.LogException();
            //}
        }
        //Feature Details
        private DataRowView _selectedrowFeatureDetails;
        public DataRowView SelectedRowFeatureDetails
        {
            get
            {
                return _selectedrowFeatureDetails;
            }

            set
            {
                _selectedrowFeatureDetails = value;
                NotifyPropertyChanged("SelectedRowFeatureDetails");
            }
        }

        private readonly ICommand selectChangeGrdCommandFeatureDetails;
        public ICommand SelectChangeGrdCommandFeatureDetails { get { return this.selectChangeGrdCommandFeatureDetails; } }
        private void SelectDataRowFeatureDetails()
        {
            if (SelectedRowFeatureDetails != null)
            {
                CrossLinkingCharModel.DtCharacteristicsDetails.Table.AcceptChanges();
                CrossLinkingCharModel.DtCharacteristicsDetails.RowFilter = "feature_code='" + SelectedRowFeatureDetails["feature_code"].ToString() + "'";
                CrossLinkingCharModel.FeatureCode = SelectedRowFeatureDetails["feature_code"].ToString();
                CrossLinkingCharModel.FeatureName = SelectedRowFeatureDetails["feature"].ToString();
                if (CrossLinkingCharModel.FeatureCode.IsNotNullOrEmpty())
                {
                    if (CrossLinkingCharModel.DtCharacteristicsDetails.Count > 0)
                    {
                        if (SelectedRowFeatureDetails["feature_code"].ToString() != "" && (CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["measuring_technique"].ToString().Trim() != "" || CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["sample_size"].ToString().Trim() != "" || CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["sample_frequency"].ToString().Trim() != "" || CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["control_method"].ToString().Trim() != "" || CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["reaction_plan"].ToString().Trim() != ""))
                        {
                            if (CrossLinkingCharModel.DtCharacteristicsDetails[CrossLinkingCharModel.DtCharacteristicsDetails.Count - 1]["feature_code"].ToString() != "")
                            {
                                DataRowView drv = CrossLinkingCharModel.DtCharacteristicsDetails.AddNew();
                                drv.BeginEdit();
                                drv["feature_code"] = CrossLinkingCharModel.FeatureCode.Trim();
                                drv["oper_code"] = 0;
                                drv.EndEdit();
                            }
                        }
                    }
                    else if (CrossLinkingCharModel.DtCharacteristicsDetails.Count == 0)
                    {
                        DataRowView drv = CrossLinkingCharModel.DtCharacteristicsDetails.AddNew();
                        drv.BeginEdit();
                        drv["feature_code"] = CrossLinkingCharModel.FeatureCode.Trim();
                        drv["oper_code"] = 0;
                        drv.EndEdit();
                    }

                }
                if (CrossLinkingCharModel.Type.IsNotNullOrEmpty() && CrossLinkingCharModel.ProductType.IsNotNullOrEmpty())
                {
                    //_crossLinkingBll.GetGridDetailsProdCategCross(CrossLinkingCharModel);

                    CrossLinkingCharModel.DtClassificationPrdType.Table.AcceptChanges();
                    CrossLinkingCharModel.DtClassificationPrdType.RowFilter = "type='" + CrossLinkingCharModel.Type + "' and sub_type='" + CrossLinkingCharModel.ProductType + "' and Feature_Code='" + CrossLinkingCharModel.FeatureCode + "' ";
                    if (CrossLinkingCharModel.DtClassificationPrdType.Count > 0)
                    {
                        if (CrossLinkingCharModel.DtClassificationPrdType[CrossLinkingCharModel.DtClassificationPrdType.Count - 1]["SUBTYPE"].ToString() != "")
                        {
                            DataRowView drv = CrossLinkingCharModel.DtClassificationPrdType.AddNew();
                            drv.BeginEdit();
                            drv["feature_code"] = CrossLinkingCharModel.FeatureCode;
                            drv["type"] = CrossLinkingCharModel.Type;
                            drv["sub_type"] = CrossLinkingCharModel.ProductType;
                            drv.EndEdit();
                        }
                    }
                    else if (CrossLinkingCharModel.DtClassificationPrdType.Count == 0)
                    {
                        DataRowView drv = CrossLinkingCharModel.DtClassificationPrdType.AddNew();
                        drv.BeginEdit();
                        drv["feature_code"] = CrossLinkingCharModel.FeatureCode;
                        drv["type"] = CrossLinkingCharModel.Type;
                        drv["sub_type"] = CrossLinkingCharModel.ProductType;
                        drv.EndEdit();
                    }
                }
                if (CrossLinkingCharModel.Type.IsNotNullOrEmpty() && CrossLinkingCharModel.ProductType.IsNotNullOrEmpty() && CrossLinkingCharModel.LinkedWith.IsNotNullOrEmpty())
                {
                    CrossLinkingCharModel.DtClassificationlinkedWith.Table.AcceptChanges();
                    CrossLinkingCharModel.DtClassificationlinkedWith.RowFilter = "type='" + CrossLinkingCharModel.Type + "' and sub_type='" + CrossLinkingCharModel.ProductType + "' and Feature_Code='" + CrossLinkingCharModel.FeatureCode + "' ";
                    if (CrossLinkingCharModel.DtClassificationlinkedWith.Count > 0)
                    {
                        if (CrossLinkingCharModel.DtClassificationlinkedWith[CrossLinkingCharModel.DtClassificationlinkedWith.Count - 1]["SUBTYPE"].ToString() != "")
                        {
                            DataRowView drv = CrossLinkingCharModel.DtClassificationlinkedWith.AddNew();
                            drv.BeginEdit();
                            drv["feature_code"] = CrossLinkingCharModel.FeatureCode;
                            drv["type"] = CrossLinkingCharModel.Type;
                            drv["sub_type"] = CrossLinkingCharModel.ProductType;
                            drv["linked_with"] = CrossLinkingCharModel.LinkedWith;
                            drv.EndEdit();
                        }
                        else if (CrossLinkingCharModel.DtClassificationlinkedWith.Count == 1 && CrossLinkingCharModel.DtClassificationlinkedWith[CrossLinkingCharModel.DtClassificationlinkedWith.Count - 1]["SNO"].ToString() == "")
                        {
                            DataRowView drv = CrossLinkingCharModel.DtClassificationlinkedWith.AddNew();
                            drv.BeginEdit();
                            drv["feature_code"] = CrossLinkingCharModel.FeatureCode;
                            drv["type"] = CrossLinkingCharModel.Type;
                            drv["sub_type"] = CrossLinkingCharModel.ProductType;
                            drv["linked_with"] = CrossLinkingCharModel.LinkedWith;
                            drv.EndEdit();
                        }
                    }
                    else if (CrossLinkingCharModel.DtClassificationlinkedWith.Count == 0)
                    {
                        DataRowView drv = CrossLinkingCharModel.DtClassificationlinkedWith.AddNew();
                        drv.BeginEdit();
                        drv["feature_code"] = CrossLinkingCharModel.FeatureCode;
                        drv["type"] = CrossLinkingCharModel.Type;
                        drv["sub_type"] = CrossLinkingCharModel.ProductType;
                        drv["linked_with"] = CrossLinkingCharModel.LinkedWith;
                        drv.EndEdit();

                    }
                }

            }
        }
        //Characteristics Details
        private DataRowView _selectedrowCharDetails;
        public DataRowView SelectedRowCharDetails
        {
            get
            {
                return _selectedrowCharDetails;
            }

            set
            {
                _selectedrowCharDetails = value;
                NotifyPropertyChanged("SelectedRowCharDetails");
            }
        }

        private readonly ICommand selectChangeGrdCommandCharDetails;
        public ICommand SelectChangeGrdCommandCharDetails { get { return this.selectChangeGrdCommandCharDetails; } }
        private void SelectDataRowCharDetails()
        {
            if (SelectedRowCharDetails != null)
            {

            }
        }

        //Classification PrdType Details
        private DataRowView _selectedrowPrdTypeDetails;
        public DataRowView SelectedRowPrdTypeDetails
        {
            get
            {
                return _selectedrowPrdTypeDetails;
            }

            set
            {
                _selectedrowPrdTypeDetails = value;
                NotifyPropertyChanged("SelectedRowPrdTypeDetails");
            }
        }

        private readonly ICommand selectChangeGrdCommandPrdTypeDetails;
        public ICommand SelectChangeGrdCommandPrdTypeDetails { get { return this.selectChangeGrdCommandPrdTypeDetails; } }
        private void SelectDataRowPrdTypeDetails()
        {
            if (SelectedRowPrdTypeDetails != null)
            {

            }
        }
        //Classification LinkedWith Details
        private DataRowView _selectedrowLinkedWithDetails;
        public DataRowView SelectedRowLinkedWithDetails
        {
            get
            {
                return _selectedrowLinkedWithDetails;
            }

            set
            {
                _selectedrowLinkedWithDetails = value;
                NotifyPropertyChanged("SelectedRowLinkedWithDetails");
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsPrdType;
        public ObservableCollection<DropdownColumns> DropDownItemsPrdType
        {
            get
            {
                return _dropDownItemsPrdType;
            }
            set
            {
                this._dropDownItemsPrdType = value;
                NotifyPropertyChanged("DropDownItemsPrdType");
            }
        }
        private ObservableCollection<DropdownColumns> _dropDownItemsLinkedWith;
        public ObservableCollection<DropdownColumns> DropDownItemsLinkedWith
        {
            get
            {
                return _dropDownItemsLinkedWith;
            }
            set
            {
                this._dropDownItemsLinkedWith = value;
                NotifyPropertyChanged("DropDownItemsLinkedWith");
            }
        }
        private void SetdropDownItems()
        {
            try
            {
                DropDownItemsPrdType = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "SUB TYPE", ColumnWidth = "1*" }
                        };
                DropDownItemsLinkedWith = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "SUB TYPE", ColumnWidth = "1*" }
                        };
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }
        private readonly ICommand selectChangeGrdCommandLinkedWithDetails;
        public ICommand SelectChangeGrdCommandLinkedWithDetails { get { return this.selectChangeGrdCommandLinkedWithDetails; } }
        private void SelectDataRowLinkedWithDetails()
        {
            if (SelectedRowLinkedWithDetails != null)
            {

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
    }
}
