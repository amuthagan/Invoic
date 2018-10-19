using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.Mvvm;
using ProcessDesigner.Model;
using ProcessDesigner.BLL;
using System.Data;
using System.Windows;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using ProcessDesigner.Common;
using System.Windows.Controls;

namespace ProcessDesigner.ViewModel
{
    class ProductCategoryViewModel : ViewModelBase
    {
        private ProductCategoryModel productCategory;
        private ProductCategoryBll productCategoryBll;
        private DataRowView selectedRow;
        private readonly ICommand _selectionChanged;
        public ICommand OnSelectionChanged { get { return this._selectionChanged; } }
        private readonly ICommand _addCommand;
        public ICommand AddCommand { get { return this._addCommand; } }
        private readonly ICommand _editCommand;
        public ICommand EditCommand { get { return this._editCommand; } }
        private readonly ICommand _saveCommand;
        public ICommand SaveCommand { get { return this._saveCommand; } }
        private readonly ICommand _deleteCommand;
        public ICommand DeleteCommand { get { return this._deleteCommand; } }
        private readonly ICommand _closeCommand;
        public ICommand CloseCommand { get { return this._closeCommand; } }
        private readonly ICommand _rowEditEndingTypeCommand;
        public ICommand RowEditEndingTypeCommand { get { return this._rowEditEndingTypeCommand; } }
        private readonly ICommand _selectionChangedTypeCommand;
        public ICommand SelectionChangedTypeCommand { get { return this._selectionChangedTypeCommand; } }
        private readonly ICommand _rowEditEndingSubTypeCommand;
        public ICommand RowEditEndingSubTypeCommand { get { return this._rowEditEndingSubTypeCommand; } }
        private readonly ICommand _selectionChangedSubTypeCommand;
        public ICommand SelectionChangedSubTypeCommand { get { return this._selectionChangedSubTypeCommand; } }
        private readonly ICommand _rowEditEndingProSubTypeCommand;
        public ICommand RowEditEndingProSubTypeCommand { get { return this._rowEditEndingProSubTypeCommand; } }
        private readonly ICommand _rowEditEndingLinkSubTypeCommand;
        public ICommand RowEditEndingLinkSubTypeCommand { get { return this._rowEditEndingLinkSubTypeCommand; } }
        private bool _addButtonIsEnable = false;
        private bool _editButtonIsEnable = true;
        private bool _saveButtonIsEnable = true;
        private bool _deleteButtonIsEnable = false;
        public Action CloseAction { get; set; }
        private ObservableCollection<DropdownColumns> _dropdownHeaders = null;


        public ProductCategoryViewModel(UserInformation userInformation)
        {
            productCategory = new ProductCategoryModel();
            productCategoryBll = new ProductCategoryBll(userInformation);

            this._selectionChanged = new DelegateCommand(this.SelectionChanged);
            this._addCommand = new DelegateCommand(this.Add);
            this._editCommand = new DelegateCommand(this.Edit);
            this._saveCommand = new DelegateCommand(this.Save);
            this._closeCommand = new DelegateCommand(this.Close);
            this._deleteCommand = new DelegateCommand<DataRowView>(this.DeleteProductType);
            this._rowEditEndingTypeCommand = new DelegateCommand<DataRowView>(this.RowEditEndingType);
            this._rowEditEndingSubTypeCommand = new DelegateCommand<DataRowView>(this.RowEditEndingSubType);
            this._rowEditEndingProSubTypeCommand = new DelegateCommand<DataRowView>(this.RowEditEndingProSubType);
            this._rowEditEndingLinkSubTypeCommand = new DelegateCommand<DataRowView>(this.RowEditEndingLinkSubType);
            this._selectionChangedTypeCommand = new DelegateCommand<DataRowView>(this.SelectionChangedType);
            this._selectionChangedSubTypeCommand = new DelegateCommand<DataRowView>(this.SelectionChangedSubType);
            GetRights();
            AddButtonIsEnable = true;
            Add();
            DropdownHeaders = new ObservableCollection<DropdownColumns>
            {           
               // new DropdownColumns { ColumnName = "PRD_CODE", ColumnDesc = "Product Code", ColumnWidth = 0 },
                new DropdownColumns { ColumnName = "PRODUCT_CATEGORY", ColumnDesc = "Product Category", ColumnWidth = "1*" }
            };
        }

        public ProductCategoryModel ProductCategory
        {
            get { return this.productCategory; }
            set
            {
                this.productCategory = value;
                NotifyPropertyChanged("ProductCategory");
            }
        }
        private bool _isPrdCategoryReadOnly = false;
        public bool IsPrdCategoryReadOnly
        {
            get { return _isPrdCategoryReadOnly; }
            set
            {
                this._isPrdCategoryReadOnly = value;
                NotifyPropertyChanged("IsPrdCategoryReadOnly");
            }
        }

        private bool _isPrdCodeReadOnly = false;
        public bool IsPrdCodeReadOnly
        {
            get { return _isPrdCodeReadOnly; }
            set
            {
                this._isPrdCodeReadOnly = value;
                NotifyPropertyChanged("IsPrdCodeReadOnly");
            }
        }

        public DataRowView SelectedRow
        {
            get { return this.selectedRow; }
            set
            {
                this.selectedRow = value;
            }
        }

        private int _tabitemindex = 0;
        public int TabItemIndex
        {
            get { return _tabitemindex; }
            set
            {
                this._tabitemindex = value;
                NotifyPropertyChanged("TabItemIndex");
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

        private string _typecolumheader = "";
        public string TypeColumHeader
        {
            get { return _typecolumheader; }
            set
            {
                this._typecolumheader = value;
                NotifyPropertyChanged("TypeColumHeader");
            }
        }

        private string _subtypecolumheader = "";
        public string SubTypeColumHeader
        {
            get { return _subtypecolumheader; }
            set
            {
                this._subtypecolumheader = value;
                NotifyPropertyChanged("SubTypeColumHeader");
            }
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


        public ObservableCollection<DropdownColumns> DropdownHeaders
        {
            get { return this._dropdownHeaders; }
            set
            {
                this._dropdownHeaders = value;
                NotifyPropertyChanged("DropdownHeaders");
            }

        }

        string oldPrdCode = "";

        DataView dvOldType = null;
        DataView dvOldSubType = null;
        DataView dvOldAllSubType = null;
        DataView dvOldAllLinkSubType = null;

        DataView dvNewType = null;
        DataView dvNewSubType = null;
        DataView dvNewAllSubType = null;
        DataView dvNewAllLinkSubType = null;
        private void SelectionChanged()
        {
            try
            {

                if (ProductCategory.DVCategory != null && SelectedRow != null && ProductCategory.DVCategory.Count > 0)
                {
                    ProductCategory.ROWID = SelectedRow["ROWID"].ToString();
                    ProductCategory.PRD_CODE = SelectedRow["PRD_CODE"].ToString();
                    productCategoryBll.GetProductTypeSubType(ProductCategory);
                    oldCategory = SelectedRow["PRODUCT_CATEGORY"].ToString();
                    oldPrdCode = SelectedRow["PRD_CODE"].ToString();

                    dvOldType = ProductCategory.DVType.Table.Copy().DefaultView;
                    dvOldSubType = ProductCategory.DVSubType.Table.Copy().DefaultView;
                    dvOldAllSubType = ProductCategory.DVAllSubType.Table.Copy().DefaultView;
                    dvOldAllLinkSubType = ProductCategory.DVAllLinkSubType.Table.Copy().DefaultView;

                    cmbSelectionChanged();

                    if (ProductCategory.ROWID != string.Empty)
                    {
                        IsPrdCodeReadOnly = true;
                        IsPrdCategoryReadOnly = ProductTypeExists(ProductCategory.PRODUCT_CATEGORY);
                        if (ProductCategory.Mode == "Add") IsPrdCategoryReadOnly = true;
                    }
                    else
                    {
                        IsPrdCategoryReadOnly = false;
                        if (ProductCategory.Mode == "Edit")
                        {
                            IsPrdCodeReadOnly = true;
                        }
                        else
                        {
                            IsPrdCodeReadOnly = false;
                        }
                    }

                }
                else
                {
                    ProductCategory.ROWID = "";
                    ProductCategory.PRODUCT_CATEGORY = "";
                    ProductCategory.PRD_CODE = "";
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void cmbSelectionChanged()
        {
            if (TabItemIndex == 0)
            {
                if (ProductCategory.DVType.Count > 0)
                {

                    ProductCategory.TypeSelectedItem = ProductCategory.DVType[0];

                    if (ProductCategory.TypeSelectedItem != null && ProductCategory.TypeSelectedItem["PRD_CODE"].ToString() != "" && ProductCategory.TypeSelectedItem["TYPE"].ToString() != "")
                    {
                        GetProductSubType(ProductCategory.TypeSelectedItem);
                    }
                    else
                    {
                        ProductCategory.DVAllSubType.Table.AcceptChanges();
                        ProductCategory.DVAllSubType.RowFilter = "1=2";
                    }

                }
                else
                {
                    ProductCategory.DVAllSubType.Table.AcceptChanges();
                    ProductCategory.DVAllSubType.RowFilter = "1=2";
                }
            }
            else if (TabItemIndex == 1)
            {
                if (ProductCategory.DVSubType.Count > 0)
                {
                    ProductCategory.SubTypeSelectedItem = ProductCategory.DVSubType[0];

                    if (ProductCategory.SubTypeSelectedItem != null && ProductCategory.SubTypeSelectedItem["PRD_CODE"].ToString() != "" && ProductCategory.SubTypeSelectedItem["SUBTYPE"].ToString() != "")
                    {
                        GetLinkedSubType(ProductCategory.SubTypeSelectedItem);
                    }
                    else
                    {
                        ProductCategory.DVAllLinkSubType.Table.AcceptChanges();
                        ProductCategory.DVAllLinkSubType.RowFilter = "1=2";
                    }
                }
                else
                {
                    ProductCategory.DVAllLinkSubType.Table.AcceptChanges();
                    ProductCategory.DVAllLinkSubType.RowFilter = "1=2";
                }
            }
        }

        private void Add()
        {
            try
            {
                if (AddButtonIsEnable == false) return;

                if (IsChangesMade())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        return;
                    }
                }

                AddButtonIsEnable = false;
                DeleteButtonIsEnable = false;
                EditButtonIsEnable = true;
                productCategory.Mode = "Add";
                ProductCategory.PRODUCT_CATEGORY = "";
                ProductCategory.PRD_CODE = Guid.NewGuid().ToValueAsString();
                ProductCategory.ROWID = "";
                setRights();
                productCategoryBll.GetProductCategory(ProductCategory);
                IsPrdCategoryReadOnly = false;
                IsPrdCodeReadOnly = false;
                StatusMessage.setStatus("Ready", ProductCategory.Mode);
                CategoryIsFocused = true;

                dvOldType = null;
                dvOldSubType = null;
                dvOldAllSubType = null;
                dvOldAllLinkSubType = null;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Edit()
        {
            try
            {
                if (EditButtonIsEnable == false) return;

                if (IsChangesMade())
                {
                    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        Save();
                        return;
                    }
                }

                AddButtonIsEnable = true;
                DeleteButtonIsEnable = true;
                EditButtonIsEnable = false;
                setRights();
                productCategory.Mode = "Edit";
                ProductCategory.PRODUCT_CATEGORY = "";
                ProductCategory.PRD_CODE = "";
                ProductCategory.ROWID = "";
                productCategoryBll.GetProductCategory(ProductCategory);
                IsPrdCategoryReadOnly = true;
                IsPrdCodeReadOnly = true;
                StatusMessage.setStatus("Ready", ProductCategory.Mode);

                dvOldType = null;
                dvOldSubType = null;
                dvOldAllSubType = null;
                dvOldAllLinkSubType = null;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void Save()
        {
            try
            {
                if (SaveButtonIsEnable == false) return;
                CategoryIsFocused = true;
                if (ProductCategory.PRD_CODE.ToString().Trim() == string.Empty)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Product Code"));
                    //MessageBox.Show("Product Code should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    CategoryIsFocused = true;
                    return;
                }

                if (ProductCategory.PRODUCT_CATEGORY.ToString().Trim() == string.Empty)
                {
                    ShowInformationMessage(PDMsg.NotEmpty("Product Category"));
                    //MessageBox.Show("Product Category should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    CategoryIsFocused = true;
                    return;
                }
                if (IsDuplicatePrdType(ProductCategory.PRODUCT_CATEGORY.Trim(), "PrdCategory") && AddButtonIsEnable == false)
                {
                    ShowInformationMessage(PDMsg.AlreadyExists("Product Category"));
                    return;
                }
                CategoryIsFocused = true;
                ProductCategory.DVType.Table.AcceptChanges();
                ProductCategory.DVSubType.Table.AcceptChanges();
                ProductCategory.DVAllSubType.Table.AcceptChanges();
                ProductCategory.DVAllLinkSubType.Table.AcceptChanges();
                if (isAllowToSave == false) return;
                if (ProductCategory.PRODUCT_CATEGORY.ToString() != "")
                {

                    bool result = false;
                    Progress.ProcessingText = PDMsg.ProgressUpdateText;
                    Progress.Start();

                    result = productCategoryBll.UpdateProductCategory(ProductCategory);
                    Progress.End();

                    dvOldType = ProductCategory.DVType.Table.Copy().DefaultView;
                    dvOldSubType = ProductCategory.DVSubType.Table.Copy().DefaultView;
                    dvOldAllSubType = ProductCategory.DVAllSubType.Table.Copy().DefaultView;
                    dvOldAllLinkSubType = ProductCategory.DVAllLinkSubType.Table.Copy().DefaultView;

                    //ProductCategory.DVAllSubType.RowFilter = "1=2";
                    //ProductCategory.DVAllLinkSubType.RowFilter = "1=2";
                    if (ProductCategory.Mode == "Add")
                        ShowInformationMessage(PDMsg.SavedSuccessfully);
                    else
                        ShowInformationMessage(PDMsg.UpdatedSuccessfully);
                    //MessageBox.Show("Records Saved Successfully.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                    AddButtonIsEnable = true;
                    Add();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void DeleteProductType(DataRowView selecteditem)
        {
            try
            {
                if (ProductCategory.Mode == "Add") return;

                string productType = "";
                if (selecteditem != null)
                {
                    productType = (selecteditem["TYPE"].IsNotNullOrEmpty() && !selecteditem["SUBTYPE"].IsNotNullOrEmpty()) ? selecteditem["TYPE"].ToString() : selecteditem["SUBTYPE"].ToString();

                    if (selecteditem["TYPE"].IsNotNullOrEmpty() && selecteditem["SUBTYPE"].IsNotNullOrEmpty()) productType = selecteditem["SUBTYPE"].ToString();

                    if (ProductTypeExists(productType))
                    {
                        MessageBox.Show("Selected type " + productType + " used in Production information, and cannot be deleted.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        CategoryIsFocused = true;
                        return;
                    }

                    MessageBoxResult msgResult = MessageBox.Show("Do you want to delete Product " + productType, "SmartPD", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (msgResult == MessageBoxResult.Yes)
                    {
                        if (selecteditem["ROWID"].IsNotNullOrEmpty())
                        {

                            ProductCategory.DTDeletedRecords.ImportRow(selecteditem.Row);
                            if ((selecteditem["TYPE"].IsNotNullOrEmpty() && !selecteditem["SUBTYPE"].IsNotNullOrEmpty()) || (!selecteditem["TYPE"].IsNotNullOrEmpty() && selecteditem["SUBTYPE"].IsNotNullOrEmpty()))
                            {
                                foreach (DataRowView dr in ProductCategory.DVAllSubType)
                                {
                                    if (dr["ROWID"].IsNotNullOrEmpty()) ProductCategory.DTDeletedRecords.ImportRow(dr.Row);

                                    dr.Delete();
                                }
                            }
                        }


                        selecteditem.Delete();
                        ShowInformationMessage(PDMsg.DeletedSuccessfully);

                    }
                    CategoryIsFocused = true;
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private void RowEditEndingType(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    if (selecteditem["PRD_CODE"].ToString() != "" && selecteditem["TYPE"].ToString() != "")
                    {
                        if (ProductCategory.DVType[ProductCategory.DVType.Count - 1]["PRD_CODE"].ToString() != "" && ProductCategory.DVType[ProductCategory.DVType.Count - 1]["TYPE"].ToString() != "")
                        {
                            if (ProductCategory.Mode == "Add")
                            {
                                ///SRS:REQ_PD_71 to 74 – Product code removed from Product category master
                                ///Date: 01/11/2016
                                ///

                                //productCategory.DVType.AddNew();

                                DataRowView drv = productCategory.DVType.AddNew();
                                drv.BeginEdit();
                                drv["PRD_CODE"] = Guid.NewGuid();
                                drv.EndEdit();

                                ///
                                /// 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void RowEditEndingSubType(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null)
                {
                    if (selecteditem["PRD_CODE"].ToString() != "" && selecteditem["SUBTYPE"].ToString() != "")
                    {
                        if (ProductCategory.DVSubType[ProductCategory.DVSubType.Count - 1]["PRD_CODE"].ToString() != "" && ProductCategory.DVSubType[ProductCategory.DVSubType.Count - 1]["SUBTYPE"].ToString() != "")
                        {
                            if (ProductCategory.Mode == "Add")
                            {
                                ///SRS:REQ_PD_71 to 74 – Product code removed from Product category master
                                ///Date: 01/11/2016

                                //productCategory.DVSubType.AddNew();

                                DataRowView drv = productCategory.DVSubType.AddNew();
                                drv.BeginEdit();
                                drv["PRD_CODE"] = Guid.NewGuid();
                                drv.EndEdit();
                                /// End
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void SelectionChangedType(DataRowView selectedItem)
        {
            try
            {
                if (selectedItem != null && selectedItem["PRD_CODE"].ToString() != "" && selectedItem["TYPE"].ToString() != "")
                {
                    GetProductSubType(selectedItem);
                }
                else
                {
                    ProductCategory.DVAllSubType.Table.AcceptChanges();
                    ProductCategory.DVAllSubType.RowFilter = "1=2";
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void GetProductSubType(DataRowView selecteditem)
        {
            if (ProductCategory.DVAllSubType != null)
            {
                DataRowView drv;
                ProductCategory.DVAllSubType.Table.AcceptChanges();
                ProductCategory.DVAllSubType.RowFilter = "PRODUCT_CATEGORY = '" + ProductCategory.PRODUCT_CATEGORY + "' AND TYPE = '" + selecteditem["TYPE"].ToString() + "'";
                NotifyPropertyChanged("ProductCategory");
                if (ProductCategory.DVAllSubType.Count == 0 || ProductCategory.DVAllSubType[ProductCategory.DVAllSubType.Count - 1]["SUBTYPE"].ToString() != "")
                {
                    if (ProductCategory.Mode == "Add")
                    {
                        drv = ProductCategory.DVAllSubType.AddNew();
                        drv.BeginEdit();
                        drv["PRODUCT_CATEGORY"] = ProductCategory.PRODUCT_CATEGORY;
                        drv["PRD_CODE"] = Guid.NewGuid();
                        drv["TYPE"] = selecteditem["TYPE"].ToString();
                        drv.EndEdit();
                    }
                }
            }
        }

        private void SelectionChangedSubType(DataRowView selectedItem)
        {
            try
            {
                if (selectedItem != null && selectedItem["PRD_CODE"].ToString() != "" && selectedItem["SUBTYPE"].ToString() != "")
                {
                    GetLinkedSubType(selectedItem);
                }
                else
                {
                    ProductCategory.DVAllLinkSubType.Table.AcceptChanges();
                    ProductCategory.DVAllLinkSubType.RowFilter = "1=2";
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void GetLinkedSubType(DataRowView selecteditem)
        {
            if (ProductCategory.DVAllLinkSubType != null)
            {
                DataRowView drv;
                ProductCategory.DVAllLinkSubType.Table.AcceptChanges();
                ProductCategory.DVAllLinkSubType.RowFilter = "PRODUCT_CATEGORY = '" + ProductCategory.PRODUCT_CATEGORY + "' AND TYPE = '" + selecteditem["SUBTYPE"].ToString() + "'";
                NotifyPropertyChanged("ProductCategory");
                if (ProductCategory.DVAllLinkSubType.Count == 0 || ProductCategory.DVAllLinkSubType[ProductCategory.DVAllLinkSubType.Count - 1]["SUBTYPE"].ToString() != "")
                {
                    if (ProductCategory.Mode == "Add")
                    {
                        drv = ProductCategory.DVAllLinkSubType.AddNew();
                        drv.BeginEdit();
                        drv["PRODUCT_CATEGORY"] = ProductCategory.PRODUCT_CATEGORY;
                        drv["PRD_CODE"] = Guid.NewGuid();
                        drv["TYPE"] = selecteditem["SUBTYPE"].ToString();
                        drv.EndEdit();
                    }
                }
            }
        }

        private void RowEditEndingProSubType(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null && ProductCategory.TypeSelectedItem != null)
                {
                    if (selecteditem["SUBTYPE"].ToString() != "" && ProductCategory.TypeSelectedItem["TYPE"].ToString() != "")
                    {
                        if (ProductCategory.DVAllSubType[ProductCategory.DVAllSubType.Count - 1]["PRD_CODE"].ToString() != "" && ProductCategory.DVAllSubType[ProductCategory.DVAllSubType.Count - 1]["SUBTYPE"].ToString() != "")
                        {
                            if (ProductCategory.Mode == "Add")
                            {
                                DataRowView drv;
                                drv = ProductCategory.DVAllSubType.AddNew();
                                drv.BeginEdit();
                                drv["PRODUCT_CATEGORY"] = ProductCategory.PRODUCT_CATEGORY;
                                drv["PRD_CODE"] = Guid.NewGuid();
                                drv["TYPE"] = ProductCategory.TypeSelectedItem["TYPE"].ToString();
                                drv.EndEdit();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void RowEditEndingLinkSubType(DataRowView selecteditem)
        {
            try
            {
                if (selecteditem != null && ProductCategory.SubTypeSelectedItem != null)
                {
                    if (selecteditem["SUBTYPE"].ToString() != "" && ProductCategory.SubTypeSelectedItem["SUBTYPE"].ToString() != "")
                    {
                        if (ProductCategory.DVAllLinkSubType[ProductCategory.DVAllLinkSubType.Count - 1]["PRD_CODE"].ToString() != "" && ProductCategory.DVAllLinkSubType[ProductCategory.DVAllLinkSubType.Count - 1]["SUBTYPE"].ToString() != "")
                        {
                            if (ProductCategory.Mode == "Add")
                            {
                                DataRowView drv;
                                drv = ProductCategory.DVAllLinkSubType.AddNew();
                                drv.BeginEdit();
                                drv["PRODUCT_CATEGORY"] = ProductCategory.PRODUCT_CATEGORY;
                                drv["PRD_CODE"] = Guid.NewGuid();
                                drv["TYPE"] = ProductCategory.SubTypeSelectedItem["SUBTYPE"].ToString();
                                drv.EndEdit();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
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

        private void GetRights()
        {
            ActionPermission = new RolePermission();
            ActionPermission.Save = true;
            ActionPermission.Print = false;
            ActionPermission.View = true;
            ActionPermission.AddNew = false;
            ActionPermission.Delete = false;
            ActionPermission.Edit = false;
            ActionPermission = productCategoryBll.GetUserRights("PRODUCT CATEGORY MASTER");
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

        private string oldProductCode = "";
        private string oldProductType = "";
        public void OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            string code = ((System.Data.DataRowView)(e.Row.Item)).Row["PRD_CODE"].ToString();
            string product = ((System.Data.DataRowView)(e.Row.Item)).Row[e.Column.SortMemberPath].ToString();
            string rowID = ((System.Data.DataRowView)(e.Row.Item)).Row["ROWID"].ToString();

            if (e.Column.Header.ToString() == "Product Code" && rowID != "") e.Cancel = true;

            if (e.Column.Header.ToString() == "Product Code") oldProductCode = code;

            if ((e.Column.Header.ToString() == "Type" || e.Column.Header.ToString() == "Sub Type"))
            {
                if (code.Trim() == string.Empty && product.Trim() == string.Empty)
                {
                    e.Cancel = true;
                }

                oldProductType = product;

                if (product.Trim() != string.Empty && rowID != "")
                {
                    StatusMessage.setStatus("Ready", ProductCategory.Mode);
                    if (productCategory.Mode == "Add")
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (ProductTypeExists(product))
                    {
                        e.Cancel = true;
                        if (productCategory.Mode == "Edit")
                        {
                            StatusMessage.setStatus("Selected type " + product + " used in Production information, and cannot be edited.", ProductCategory.Mode);
                        }
                    }

                    if (productCategoryBll.IsSubTypeDuplicate(product))
                    {
                        e.Cancel = true;
                        if (productCategory.Mode == "Edit")
                        {
                            StatusMessage.setStatus("Selected type " + product + " used in Characteristics Grouping Master, and cannot be edited.", ProductCategory.Mode);
                        }
                    }

                }

            }
        }

        private bool ProductTypeExists(string producttype)
        {
            string[] productTypes = new string[] { "EXTERNAL THREADED", "INTERNAL THREADED", "TYPE", "HEAD FORMS", "DRIVING FEATURE", "SHANK FORM", "END FORM",
                                                    "ADDITIONAL FEATURE", "KEYWORDS", "SCREW", "BOLT", "STUD", "NON THREADED", "RIVET", "COMBI SCREW", "SPL FORM / MISCELLAN",
                                                    "SET SCREW", "OTHER FORMS", "NONE", "HEX", "HEX WITH COLLAR / FL", "SQUARE", "SQUARE WITH COLLAR", "BI - HEX",
                                                    "BI - HEX WITH FLANGE", "ROUND / CIRCULAR", "TRUSS", "CHEESE", "FILLISTER", "PAN", "COUNTERSUNK", "RAISED COUNTERSUNK",
                                                    "SIX LOBE", "OVAL", "D HEAD", "DOUBLE D", "FEATURE", "NONE", "HEX - EXTERNAL", "SQUARE - EXTERNAL", "BI-HEX  - EXTERNAL", "SIX LOBE",
                                                    "SLOT", "PHILIPS", "CROSS RECESS", "STRAIGHT KNURL", "BI-HEX SOCKET", "NORMAL", "REDUCED", "WAISTED",
                                                    "INCREASED", "SHOULDER", "SQUARE NECK", "KNURLED", "AS ROLLED", "ROUNDED", "TRUNCATED CONE POINT",
                                                    "SHORT DOG POINT", "LONG DOG POINT", "SHORT DOG PT. WITH R", "SHORT DOG PT. WITH T", "SCRAPE POINT",
                                                    "AS POINTED", "B POINT", "NONE", "HOLE ON HEAD", "HOLE ON THREAD", "HOLE ON SHANK", "AXIAL HOLE ON SHANK",
                                                    "NYLON PATCH", "MICRO ENCAPSULATED", "SERRATED", "WELD FEATURE", "SPHERICAL", "NONE", "GROUND", "CR BOLT",
                                                    "PLACE BOLT", "STUD BOLT", "WHEEL BOLT", "CYL HEAD BOLT", "MAIN BEARING CAP BOL", "WELD SCREW",
                                                    "SELP TAPPER", "COMBI", "STUD", "NON THREADED", "RIVET", "SET SCREW", "BANJO", "TYPE", "BEARING FACE",
                                                    "FEATURE - 1", "FEATURE - 2", "HEX", "ROUND", "BI-HEX", "SQUARE", "MISCELLANEOUS", "BLANK", "BEARING FACE", "PLAIN",
                                                    "FLANGE", "COLLAR", "SPHERICAL", "BEARING FACE", "FLANGED SPHERICAL", "CONICAL", "FLANGED CONICAL", "BLANK",
                                                    "WELD", "NYLOC PREV TORQUE", "METALLIC INSERT PREV TORQUE", "ALL METALLIC PREV TORQUE", "SLOTTED",
                                                    "CASTLE", "INTEGRAL WASHER", "DOME", "WELD", "NYLOC PREV TORQUE", "METALLIC INSERT PREV PREV TORQUE",
                                                    "ALL METALLIC PREV TORQUE", "SLOTTED", "CASTLE", "INTEGRAL WASHER", "DOME", "NYLOCK NUT", "ALL METAL PREV TORQUE NUT",
                                                    "WELD NUT", "DOME NUT", "INTEGRAL WASHERNON THREADED", "SPL KNURLING ON HEAD", "GIMLET", "CHAMFERED", "CONE POINT", "CUP POINT",
                                                    "HEX SOCKET", "SQUARE SOCKET" };

            return productTypes.Contains(producttype);
        }
        private bool isAllowToSave = true;
        public void OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            TextBox tb = e.EditingElement as TextBox;
            DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            string columnName = e.Column.SortMemberPath;
            string prdCode = selecteditem["PRD_CODE"].ToString().ToUpper().Trim();
            string productType = "";
            string rowID = selecteditem["ROWID"].ToString();

            if (columnName == "PRD_CODE") tb.Text = tb.Text.ToUpper();

            if (dg.Name == "dgrdType")
            {
                productType = selecteditem["TYPE"].ToString();
                if (columnName == "TYPE")
                {
                    if (IsDuplicatePrdType(productType.Trim().ToUpper(), "CatType"))
                    {
                        ShowInformationMessage(PDMsg.AlreadyExists("Product Type"));
                        //MessageBox.Show("Product Type should not be allow duplicate.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = oldProductType;
                        isAllowToSave = false;
                        return;
                    }
                    else
                    {
                        isAllowToSave = true;
                    }

                    if (prdCode.Trim() != string.Empty && productType.Trim() == string.Empty)
                    {

                        ShowInformationMessage(PDMsg.NotEmpty("Product Type"));
                        //    isAllowToSave = false;

                        //MessageBox.Show("Product Type should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = oldProductType;
                        return;
                    }

                    if (rowID != "")
                    {
                        if (productType != oldProductType)
                        {
                            ProductCategory.DVAllSubType.Table.AcceptChanges();
                            ProductCategory.DVAllSubType.RowFilter = "PRODUCT_CATEGORY = '" + ProductCategory.PRODUCT_CATEGORY + "' AND TYPE = '" + oldProductType + "'";
                            foreach (DataRowView dr in ProductCategory.DVAllSubType)
                            {
                                dr.BeginEdit();
                                dr["TYPE"] = productType;
                                dr.EndEdit();
                            }
                            ProductCategory.DVAllSubType.Table.AcceptChanges();
                            ProductCategory.DVAllSubType.RowFilter = "PRODUCT_CATEGORY = '" + ProductCategory.PRODUCT_CATEGORY + "' AND TYPE = '" + productType + "'";
                        }
                    }
                    else
                    {

                        if (ProductCategory.DVAllSubType != null && columnName == "TYPE")
                        {
                            if (!productType.Trim().ToUpper().IsNotNullOrEmpty())
                            {
                                foreach (DataRowView dr in ProductCategory.DVAllSubType)
                                {
                                    dr.Delete();
                                }
                                return;
                            }

                            DataRowView drv;

                            ProductCategory.DVAllSubType.Table.AcceptChanges();
                            ProductCategory.DVAllSubType.RowFilter = "PRODUCT_CATEGORY = '" + ProductCategory.PRODUCT_CATEGORY + "' AND TYPE = '" + oldProductType + "'";

                            foreach (DataRowView dr in ProductCategory.DVAllSubType)
                            {
                                dr.BeginEdit();
                                dr["TYPE"] = productType;
                                dr.EndEdit();
                            }

                            ProductCategory.DVAllSubType.Table.AcceptChanges();
                            ProductCategory.DVAllSubType.RowFilter = "PRODUCT_CATEGORY = '" + ProductCategory.PRODUCT_CATEGORY + "' AND TYPE = '" + productType + "'";
                            if (ProductCategory.DVAllSubType.Count == 0 || ProductCategory.DVAllSubType[ProductCategory.DVAllSubType.Count - 1]["SUBTYPE"].ToString() != "")
                            {
                                if (ProductCategory.Mode == "Add")
                                {
                                    drv = ProductCategory.DVAllSubType.AddNew();
                                    drv.BeginEdit();
                                    drv["PRODUCT_CATEGORY"] = ProductCategory.PRODUCT_CATEGORY;
                                    drv["PRD_CODE"] = Guid.NewGuid();
                                    drv["TYPE"] = productType;
                                    drv.EndEdit();
                                }
                            }
                        }
                    }
                }
                else if (columnName == "PRD_CODE")
                {
                    if (prdCode.Trim() == string.Empty && productType.Trim() != string.Empty)
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Product Code"));
                        //MessageBox.Show("Product Code should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = oldProductCode;
                        return;
                    }

                    if (IsDuplicatePrdCode(prdCode, "CatType"))
                    {
                        MessageBox.Show("Product Code should not be allow duplicate.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = "";
                    }
                }

            }
            else if (dg.Name == "dgrdTypeNew")
            {
                productType = selecteditem["SUBTYPE"].ToString();
                if (columnName == "SUBTYPE")
                {
                    if (IsDuplicatePrdType(productType.Trim().ToUpper(), "CatSubType"))
                    {
                        ShowInformationMessage(PDMsg.AlreadyExists("Product Sub Type"));
                        //MessageBox.Show("Product Sub Type should not be allow duplicate.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = oldProductType;
                        return;
                    }
                    if (prdCode.Trim() != string.Empty && productType.Trim() == string.Empty)
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Product Sub Type"));
                        //MessageBox.Show("Product Sub Type should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = oldProductType;
                    }

                }
                else if (columnName == "PRD_CODE")
                {
                    if (prdCode.Trim() == string.Empty && productType.Trim() != string.Empty)
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Product Code"));
                        //MessageBox.Show("Product Code should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = oldProductCode;
                        return;
                    }
                    if (IsDuplicatePrdCode(prdCode, "CatSubType"))
                    {
                        ShowInformationMessage(PDMsg.AlreadyExists("Product Code"));
                        // MessageBox.Show("Product Code should not be allow duplicate.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = "";
                    }
                }
            }
            else if (dg.Name == "dgrdSubType")
            {
                productType = selecteditem["SUBTYPE"].ToString();
                if (columnName == "SUBTYPE")
                {
                    if (IsDuplicatePrdType(productType.Trim().ToUpper(), "LinkType"))
                    {
                        ShowInformationMessage(PDMsg.AlreadyExists("Product Type"));
                        // MessageBox.Show("Product Type should not be allow duplicate.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = oldProductType;
                        return;
                    }

                    if (prdCode.Trim() != string.Empty && productType.Trim() == string.Empty)
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Product Type"));
                        //MessageBox.Show("Product Type should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = oldProductType;
                        return;
                    }
                    if (rowID != "")
                    {
                        if (productType != oldProductType)
                        {
                            ProductCategory.DVAllLinkSubType.Table.AcceptChanges();
                            ProductCategory.DVAllLinkSubType.RowFilter = "PRODUCT_CATEGORY = '" + ProductCategory.PRODUCT_CATEGORY + "' AND TYPE = '" + oldProductType + "'";
                            foreach (DataRowView dr in ProductCategory.DVAllLinkSubType)
                            {
                                dr.BeginEdit();
                                dr["TYPE"] = productType;
                                dr.EndEdit();
                            }
                            ProductCategory.DVAllLinkSubType.Table.AcceptChanges();
                            ProductCategory.DVAllLinkSubType.RowFilter = "PRODUCT_CATEGORY = '" + ProductCategory.PRODUCT_CATEGORY + "' AND TYPE = '" + productType + "'";
                        }
                    }
                    else
                    {
                        if (ProductCategory.DVAllLinkSubType != null)
                        {
                            if (!productType.Trim().ToUpper().IsNotNullOrEmpty())
                            {
                                foreach (DataRowView dr in ProductCategory.DVAllLinkSubType)
                                {
                                    dr.Delete();
                                }
                                return;
                            }

                            DataRowView drv;
                            ProductCategory.DVAllLinkSubType.Table.AcceptChanges();
                            ProductCategory.DVAllLinkSubType.RowFilter = "PRODUCT_CATEGORY = '" + ProductCategory.PRODUCT_CATEGORY + "' AND TYPE = '" + oldProductType + "'";

                            foreach (DataRowView dr in ProductCategory.DVAllLinkSubType)
                            {
                                dr.BeginEdit();
                                dr["TYPE"] = productType;
                                dr.EndEdit();
                            }

                            ProductCategory.DVAllLinkSubType.Table.AcceptChanges();
                            ProductCategory.DVAllLinkSubType.RowFilter = "PRODUCT_CATEGORY = '" + ProductCategory.PRODUCT_CATEGORY + "' AND TYPE = '" + productType + "'";

                            if (ProductCategory.DVAllLinkSubType.Count == 0 || ProductCategory.DVAllLinkSubType[ProductCategory.DVAllLinkSubType.Count - 1]["SUBTYPE"].ToString() != "")
                            {
                                if (ProductCategory.Mode == "Add")
                                {
                                    drv = ProductCategory.DVAllLinkSubType.AddNew();
                                    drv.BeginEdit();
                                    drv["PRODUCT_CATEGORY"] = ProductCategory.PRODUCT_CATEGORY;
                                    drv["PRD_CODE"] = Guid.NewGuid();
                                    drv["TYPE"] = productType;
                                    drv.EndEdit();
                                }
                            }
                        }
                    }
                }
                else if (columnName == "PRD_CODE")
                {
                    if (prdCode.Trim() == string.Empty && productType.Trim() != string.Empty)
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Product Code"));
                        //MessageBox.Show("Product Code should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = oldProductCode;
                        return;
                    }

                    if (IsDuplicatePrdCode(prdCode, "LinkType"))
                    {
                        MessageBox.Show("Product Code should not be allow duplicate.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = "";
                    }
                }
            }
            else if (dg.Name == "dgrdSubTypeNew")
            {
                productType = selecteditem["SUBTYPE"].ToString();
                if (columnName == "SUBTYPE")
                {
                    if (IsDuplicatePrdType(productType.Trim().ToUpper(), "LinkSubType"))
                    {
                        ShowInformationMessage(PDMsg.AlreadyExists("Product Sub Type"));
                        //MessageBox.Show("Product Sub Type should not be allow duplicate.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = oldProductType;
                        return;
                    }

                    if (prdCode.Trim() != string.Empty && productType.Trim() == string.Empty)
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Product Sub Type"));
                        //MessageBox.Show("Product Sub Type should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = oldProductType;
                    }

                }
                else if (columnName == "PRD_CODE")
                {
                    if (prdCode.Trim() == string.Empty && productType.Trim() != string.Empty)
                    {
                        ShowInformationMessage(PDMsg.NotEmpty("Product Code"));
                        //MessageBox.Show("Product Code should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = oldProductCode;
                        return;
                    }

                    if (IsDuplicatePrdCode(prdCode, "LinkSubType"))
                    {
                        MessageBox.Show("Product Code should not be allow duplicate.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                        tb.Text = "";
                    }
                }
            }

        }

        public void CodePreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (ProductCategory.ROWID == "" && ProductCategory.PRD_CODE.Trim() != "" && IsDuplicatePrdCode(ProductCategory.PRD_CODE.Trim(), "PrdCategory"))
            {
                MessageBox.Show("Product Code should not be allow duplicate.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                e.Handled = true;
                ProductCategory.PRD_CODE = "";
            }
        }

        private string oldCategory = "";
        public void PrdCategory_GotFocus(object sender, RoutedEventArgs e)
        {
            oldCategory = ProductCategory.PRODUCT_CATEGORY;
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

        public void CategoryPreviewLostKeyboardFocus(object sender, RoutedEventArgs e)
        {

            if (ProductCategory.PRD_CODE.IsNotNullOrEmpty() && ProductCategory.PRD_CODE.Length <= 10 && !ProductCategory.PRODUCT_CATEGORY.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Product Category"));
                //MessageBox.Show("Product Category should not be empty.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);                
                ProductCategory.PRODUCT_CATEGORY = oldCategory;
            }

            if (ProductCategory.PRODUCT_CATEGORY.IsNotNullOrEmpty())
            {
                if (ProductCategory.PRODUCT_CATEGORY.Trim() != oldCategory)
                {
                    if (IsDuplicatePrdType(ProductCategory.PRODUCT_CATEGORY.Trim(), "PrdCategory"))
                    {
                        if (SaveButtonIsEnable == false)
                        {
                            ShowInformationMessage(PDMsg.AlreadyExists("Product Category"));
                            //MessageBox.Show("Product Category should not be allow duplicate.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
                            ProductCategory.PRODUCT_CATEGORY = oldCategory;
                        }

                    }

                    ProductCategory.DVAllSubType.Table.AcceptChanges();
                    ProductCategory.DVAllSubType.RowFilter = "PRODUCT_CATEGORY = '" + oldCategory + "'";

                    foreach (DataRowView dr in ProductCategory.DVAllSubType)
                    {
                        dr.BeginEdit();
                        dr["PRODUCT_CATEGORY"] = ProductCategory.PRODUCT_CATEGORY;
                        dr.EndEdit();
                    }
                    ProductCategory.DVAllSubType.Table.AcceptChanges();
                    if (ProductCategory.TypeSelectedItem != null) ProductCategory.DVAllSubType.RowFilter = "PRODUCT_CATEGORY = '" + ProductCategory.PRODUCT_CATEGORY + "' AND TYPE = '" + ProductCategory.TypeSelectedItem["TYPE"] + "'";


                    ProductCategory.DVAllLinkSubType.Table.AcceptChanges();
                    ProductCategory.DVAllLinkSubType.RowFilter = "PRODUCT_CATEGORY = '" + oldCategory + "'";

                    foreach (DataRowView dr in ProductCategory.DVAllLinkSubType)
                    {
                        dr.BeginEdit();
                        dr["PRODUCT_CATEGORY"] = ProductCategory.PRODUCT_CATEGORY;
                        dr.EndEdit();
                    }
                    ProductCategory.DVAllLinkSubType.Table.AcceptChanges();
                    if (ProductCategory.SubTypeSelectedItem != null) ProductCategory.DVAllLinkSubType.RowFilter = "PRODUCT_CATEGORY = '" + ProductCategory.PRODUCT_CATEGORY + "' AND TYPE = '" + ProductCategory.SubTypeSelectedItem["SUBTYPE"] + "'";

                }
            }
        }

        public bool IsDuplicatePrdCode(string code, string name)
        {

            if (code.Trim() == string.Empty) return false;

            if (productCategoryBll.CheckCodeIsDuplicate(code.Trim()))
            {
                return true;
            }

            if (name != "PrdCategory" && code.Trim() == ProductCategory.PRD_CODE.Trim()) return true;

            DataView dv = ProductCategory.DVCategory.ToTable().DefaultView;
            dv.RowFilter = "TRIM(PRD_CODE) = '" + code.Trim() + "'";
            if (dv.Count == 1) return true;

            dv = ProductCategory.DVType.ToTable().DefaultView;
            dv.RowFilter = "TRIM(PRD_CODE) = '" + code.Trim() + "'";
            if (name != "CatType")
            {
                if (dv.Count == 1) return true;
            }
            else if (name == "CatType")
            {
                if (dv.Count > 1) return true;
            }



            int cnt = 0;
            foreach (DataRow dr in ProductCategory.DVAllSubType.Table.Rows)
            {
                if (dr["PRD_CODE"].ToString().ToUpper().Trim() == code.Trim().ToUpper())
                {
                    cnt = cnt + 1;
                }
            }

            if (name != "CatSubType")
            {
                if (cnt == 1) return true;
            }
            else if (name == "CatSubType")
            {
                if (cnt > 1) return true;
            }

            dv = ProductCategory.DVSubType.ToTable().DefaultView;
            dv.RowFilter = "TRIM(PRD_CODE) = '" + code.Trim() + "'";
            if (name != "LinkType")
            {
                if (dv.Count == 1) return true;
            }
            else if (name == "LinkType")
            {
                if (dv.Count > 1) return true;
            }

            cnt = 0;
            foreach (DataRow dr in ProductCategory.DVAllLinkSubType.Table.Rows)
            {
                if (dr["PRD_CODE"].ToString().ToUpper().Trim() == code.Trim().ToUpper())
                {
                    cnt = cnt + 1;
                }
            }

            if (name != "LinkSubType")
            {
                if (cnt == 1) return true;
            }
            else if (name == "LinkSubType")
            {
                if (cnt > 1) return true;
            }


            return false;
        }

        public bool IsDuplicatePrdType(string prdType, string name)
        {

            if (prdType.Trim() == string.Empty) return false;

            if (name != "PrdCategory" && prdType.Trim().ToUpper() == ProductCategory.PRODUCT_CATEGORY.Trim().ToUpper()) return true;

            int cnt = 0;
            DataView dv = ProductCategory.DVCategory.ToTable().DefaultView;
            dv.RowFilter = "PRODUCT_CATEGORY = '" + prdType.Trim() + "'";
            if (dv.Count > 1) return true;

            dv = ProductCategory.DVType.ToTable().DefaultView;
            dv.RowFilter = "TYPE = '" + prdType.Trim() + "'";
            cnt = dv.Count;
            if (name == "CatType")
            {
                if (dv.Count > 1) return true;
            }

            dv = ProductCategory.DVAllSubType.ToTable().DefaultView;
            if (ProductCategory.TypeSelectedItem != null) dv.RowFilter = "TYPE = '" + ProductCategory.TypeSelectedItem["TYPE"] + "' AND SUBTYPE = '" + prdType.Trim() + "'";
            if (name == "CatSubType")
            {
                if (dv.Count > 1) return true;
            }

            dv = ProductCategory.DVSubType.ToTable().DefaultView;
            dv.RowFilter = "SUBTYPE = '" + prdType.Trim() + "'";
            if (name == "LinkType")
            {
                if (dv.Count > 1) return true;
            }

            if (cnt == 1 && dv.Count == 1) return true;

            dv = ProductCategory.DVAllLinkSubType.ToTable().DefaultView;
            if (ProductCategory.SubTypeSelectedItem != null) dv.RowFilter = "TYPE = '" + ProductCategory.SubTypeSelectedItem["SUBTYPE"] + "' AND SUBTYPE = '" + prdType.Trim() + "'";
            if (name == "LinkSubType")
            {
                if (dv.Count > 1) return true;
            }

            return false;
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
                //if (IsChangesMade())
                //{
                //    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //    {
                //        Save();
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
                CategoryIsFocused = true;
                bool result = true;

                if (dvOldType != null && dvOldSubType != null && dvOldAllSubType != null && dvOldAllLinkSubType != null)
                {
                    dvOldType.RowFilter = "Convert(TYPE, 'System.String') <> ''";
                    dvOldSubType.RowFilter = "Convert(SUBTYPE, 'System.String') <> ''";
                    dvOldAllSubType.RowFilter = "Convert(SUBTYPE, 'System.String') <> ''";
                    dvOldAllLinkSubType.RowFilter = "Convert(SUBTYPE, 'System.String') <> ''";

                    dvNewType = ProductCategory.DVType.Table.Copy().DefaultView;
                    dvNewSubType = ProductCategory.DVSubType.Table.Copy().DefaultView;
                    dvNewAllSubType = ProductCategory.DVAllSubType.Table.Copy().DefaultView;
                    dvNewAllLinkSubType = ProductCategory.DVAllLinkSubType.Table.Copy().DefaultView;

                    dvNewType.RowFilter = "Convert(TYPE, 'System.String') <> ''";
                    dvNewSubType.RowFilter = "Convert(SUBTYPE, 'System.String') <> ''";
                    dvNewAllSubType.RowFilter = "Convert(SUBTYPE, 'System.String') <> ''";
                    dvNewAllLinkSubType.RowFilter = "Convert(SUBTYPE, 'System.String') <> ''";

                    if (oldCategory != ProductCategory.PRODUCT_CATEGORY) result = false;

                    result = dvNewType.IsEqual(dvOldType);
                    if (result)
                    {
                        result = dvNewSubType.IsEqual(dvOldSubType);
                    }

                    if (result)
                    {
                        result = dvNewAllSubType.IsEqual(dvOldAllSubType);
                    }

                    if (result)
                    {
                        result = dvNewAllLinkSubType.IsEqual(dvOldAllLinkSubType);
                    }
                }

                return !result;
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

    }
}
