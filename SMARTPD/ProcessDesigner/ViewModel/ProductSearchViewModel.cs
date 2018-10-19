using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;
using System.Windows.Input;
using System.Collections.ObjectModel;
using ProcessDesigner.UserControls;
using System.Windows.Forms.Integration;
using AxACCTRLLib;
using System.Windows;
using WPF.MDI;


namespace ProcessDesigner.ViewModel
{
    public class ProductSearchViewModel : ViewModelBase
    {
        //private WindowsFormsHost _drawingHost;

        private UserInformation _userInformation = null;
        private ProductSearchBll _productSearchBll = null;
        private DataTable _costCentreData = null;
        private DataTable _familyTypeData = null;
        private DataTable _headFormData = null;
        private DataTable _shankFormData = null;
        private DataTable _endFormData = null;
        private DataTable _drivingFeatureData = null;
        private DataTable _additionalFeatureData = null;
        private DataTable _keywordsData = null;

        private DataTable _applicationCombo = null;



        public Action CloseAction { get; set; }

        public ProductSearchViewModel(UserInformation userInformation)
        {
            try
            {
                this._userInformation = userInformation;
                _productSearchBll = new ProductSearchBll(this._userInformation);
                ProdSearchModel = new ProductSearchModel();
                SetdropDownItems();
                GetAllCombo();
                this.searchCommand = new DelegateCommand(this.Search);
                this.selectionChangedManufacturedCommand = new DelegateCommand(this.FilterCostCentre);
                this.selectionChangedFamilyCommand = new DelegateCommand(this.FilterAll);
                this.closeCommand = new DelegateCommand(this.CloseSubmit);
                this.showProductInformationCommand = new DelegateCommand(this.ShowProductInformation);
                this.printCommand = new DelegateCommand(this.PrintProductSearch);
                this.previewMouseLeftButtonDownCommand = new DelegateCommand(this.ValidateApplicationCombo);
                ProdSearchModel.TotalRecords = "Part Details";
                ProdSearchModel.HeatTreatment = "678WERER@#$%^&$#";
                ProductResult = _productSearchBll.GetProductSearchDetails(ProdSearchModel);
                ProductResult.AddNew();
                ProdSearchModel.HeatTreatment = "";
                NotifyPropertyChanged("ProductResult");
            }
            catch (Exception ex)
            {

            }
        }

        private ProductSearchModel _prodSearchModel = null;
        public ProductSearchModel ProdSearchModel
        {
            get { return _prodSearchModel; }
            set
            {
                _prodSearchModel = value;
                NotifyPropertyChanged("ProdSearchModel");
            }
        }

        private DataView _productResult = null;
        public DataView ProductResult
        {
            get { return _productResult; }
            set
            {
                _productResult = value;
                NotifyPropertyChanged("ProductResult");
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

        private ObservableCollection<DropdownColumns> _dropDownItemsFamily;
        public ObservableCollection<DropdownColumns> DropDownItemsFamily
        {
            get
            {
                return _dropDownItemsFamily;
            }
            set
            {
                _dropDownItemsFamily = value;
                NotifyPropertyChanged("DropDownItemsFamily");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsFamilyType;
        public ObservableCollection<DropdownColumns> DropDownItemsFamilyType
        {
            get
            {
                return _dropDownItemsFamilyType;
            }
            set
            {
                _dropDownItemsFamilyType = value;
                NotifyPropertyChanged("DropDownItemsFamilyType");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsHeadForm;
        public ObservableCollection<DropdownColumns> DropDownItemsHeadForm
        {
            get
            {
                return _dropDownItemsHeadForm;
            }
            set
            {
                _dropDownItemsHeadForm = value;
                NotifyPropertyChanged("DropDownItemsHeadForm");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsShankForm;
        public ObservableCollection<DropdownColumns> DropDownItemsShankForm
        {
            get
            {
                return _dropDownItemsShankForm;
            }
            set
            {
                _dropDownItemsShankForm = value;
                NotifyPropertyChanged("DropDownItemsShankForm");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsEndForm;
        public ObservableCollection<DropdownColumns> DropDownItemsEndForm
        {
            get
            {
                return _dropDownItemsEndForm;
            }
            set
            {
                _dropDownItemsEndForm = value;
                NotifyPropertyChanged("DropDownItemsEndForm");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsDrivingFeature;
        public ObservableCollection<DropdownColumns> DropDownItemsDrivingFeature
        {
            get
            {
                return _dropDownItemsDrivingFeature;
            }
            set
            {
                _dropDownItemsDrivingFeature = value;
                NotifyPropertyChanged("DropDownItemsDrivingFeature");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsAdditionalFeature;
        public ObservableCollection<DropdownColumns> DropDownItemsAdditionalFeature
        {
            get
            {
                return _dropDownItemsAdditionalFeature;
            }
            set
            {
                _dropDownItemsAdditionalFeature = value;
                NotifyPropertyChanged("DropDownItemsAdditionalFeature");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsKeywords;
        public ObservableCollection<DropdownColumns> DropDownItemsKeywords
        {
            get
            {
                return _dropDownItemsKeywords;
            }
            set
            {
                _dropDownItemsKeywords = value;
                NotifyPropertyChanged("DropDownItemsKeywords");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsProductFamily;
        public ObservableCollection<DropdownColumns> DropDownItemsProductFamily
        {
            get
            {
                return _dropDownItemsProductFamily;
            }
            set
            {
                _dropDownItemsProductFamily = value;
                NotifyPropertyChanged("DropDownItemsProductFamily");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsHeadStyle;
        public ObservableCollection<DropdownColumns> DropDownItemsHeadStyle
        {
            get
            {
                return _dropDownItemsHeadStyle;
            }
            set
            {
                _dropDownItemsHeadStyle = value;
                NotifyPropertyChanged("DropDownItemsHeadStyle");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsProductType;
        public ObservableCollection<DropdownColumns> DropDownItemsProductType
        {
            get
            {
                return _dropDownItemsProductType;
            }
            set
            {
                _dropDownItemsProductType = value;
                NotifyPropertyChanged("DropDownItemsProductType");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsApplication;
        public ObservableCollection<DropdownColumns> DropDownItemsApplication
        {
            get
            {
                return _dropDownItemsApplication;
            }
            set
            {
                _dropDownItemsApplication = value;
                NotifyPropertyChanged("DropDownItemsApplication");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsRMSpec;
        public ObservableCollection<DropdownColumns> DropDownItemsRMSpec
        {
            get
            {
                return _dropDownItemsRMSpec;
            }
            set
            {
                _dropDownItemsRMSpec = value;
                NotifyPropertyChanged("DropDownItemsRMSpec");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsFinish;
        public ObservableCollection<DropdownColumns> DropDownItemsFinish
        {
            get
            {
                return _dropDownItemsFinish;
            }
            set
            {
                _dropDownItemsFinish = value;
                NotifyPropertyChanged("DropDownItemsFinish");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsManufacturedAt;
        public ObservableCollection<DropdownColumns> DropDownItemsManufacturedAt
        {
            get
            {
                return _dropDownItemsManufacturedAt;
            }
            set
            {
                _dropDownItemsManufacturedAt = value;
                NotifyPropertyChanged("DropDownItemsManufacturedAt");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsCostCentre;
        public ObservableCollection<DropdownColumns> DropDownItemsCostCentre
        {
            get
            {
                return _dropDownItemsCostCentre;
            }
            set
            {
                _dropDownItemsCostCentre = value;
                NotifyPropertyChanged("DropDownItemsCostCentre");
            }
        }

        private ObservableCollection<DropdownColumns> _dropDownItemsOperation;
        public ObservableCollection<DropdownColumns> DropDownItemsOperation
        {
            get
            {
                return _dropDownItemsOperation;
            }
            set
            {
                _dropDownItemsOperation = value;
                NotifyPropertyChanged("DropDownItemsOperation");
            }
        }



        private DataRowView _selectedPartRow = null;
        public DataRowView SelectedPartRow
        {
            get
            {
                return _selectedPartRow;
            }
            set
            {
                _selectedPartRow = value;
                NotifyPropertyChanged("SelectedPartRow");
                //ShowDrawing();
            }
        }

        private readonly ICommand searchCommand;
        public ICommand SearchCommand { get { return this.searchCommand; } }

        private readonly ICommand closeCommand;
        public ICommand CloseCommand { get { return this.closeCommand; } }

        private readonly ICommand selectionChangedManufacturedCommand;
        public ICommand SelectionChangedManufacturedCommand { get { return this.selectionChangedManufacturedCommand; } }

        private readonly ICommand selectionChangedFamilyCommand;
        public ICommand SelectionChangedFamilyCommand { get { return this.selectionChangedFamilyCommand; } }


        private readonly ICommand showProductInformationCommand;
        public ICommand ShowProductInformationCommand { get { return this.showProductInformationCommand; } }

        private readonly ICommand printCommand;
        public ICommand PrintCommand { get { return this.printCommand; } }


        private readonly ICommand previewMouseLeftButtonDownCommand;
        public ICommand PreviewMouseLeftButtonDownCommand { get { return this.previewMouseLeftButtonDownCommand; } }

        private readonly ICommand _onPartNoSelectionChanged;
        public ICommand OnPartNoSelectionChanged { get { return this._onPartNoSelectionChanged; } }

        private void CloseSubmit()
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


        private MessageBoxResult ShowConfirmMessageYesNo(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return MessageBoxResult.None;
        }

        private void GetAllCombo()
        {
            try
            {
                ProdSearchModel.RMSpecCombo = _productSearchBll.GetRMSpecCombo().ToDataTable<DDRM_MAST>().DefaultView;
                ProdSearchModel.FinishCombo = _productSearchBll.GetFinishCombo().ToDataTable<DDFINISH_MAST>().DefaultView;
                ProdSearchModel.ManufacturedAtCombo = _productSearchBll.GetLocationCombo().ToDataTable<DDLOC_MAST>().DefaultView;
                ProdSearchModel.CostCentreCombo = _productSearchBll.GetCostCentreCombo("").ToDataTable<DDCOST_CENT_MAST>().DefaultView;
                _costCentreData = ProdSearchModel.CostCentreCombo.Table.Copy();
                _costCentreData.CaseSensitive = false;
                ProdSearchModel.OperationCombo = _productSearchBll.GetOperationCombo().ToDataTable<DDOPER_MAST>().DefaultView;
                ProdSearchModel.CustomerCombo = _productSearchBll.GetCustomerCombo().ToDataTable<DDCUST_MAST>().DefaultView;
                ProdSearchModel.FamilyCombo = _productSearchBll.GetFamilyCombo().ToDataTable<FASTENERS_MASTER>().DefaultView;
                ProdSearchModel.FamilyTypeCombo = _productSearchBll.GetTypeFastenersCombo(ProdSearchModel.Family.ToValueAsString(), "TYPE").ToDataTable<FASTENERS_MASTER>().DefaultView;
                _familyTypeData = ProdSearchModel.FamilyTypeCombo.ToTable().Copy();
                _familyTypeData.CaseSensitive = false;
                ProdSearchModel.HeadFormCombo = _productSearchBll.GetTypeFastenersCombo(ProdSearchModel.Family.ToValueAsString(), "HEAD FORMS").ToDataTable<FASTENERS_MASTER>().DefaultView;
                _headFormData = ProdSearchModel.HeadFormCombo.ToTable().Copy();
                _headFormData.CaseSensitive = false;
                ProdSearchModel.ShankFormCombo = _productSearchBll.GetTypeFastenersCombo(ProdSearchModel.Family.ToValueAsString(), "SHANK FORM").ToDataTable<FASTENERS_MASTER>().DefaultView;
                _shankFormData = ProdSearchModel.ShankFormCombo.ToTable().Copy();
                _shankFormData.CaseSensitive = false;
                ProdSearchModel.EndFormCombo = _productSearchBll.GetTypeFastenersCombo(ProdSearchModel.Family.ToValueAsString(), "END FORM").ToDataTable<FASTENERS_MASTER>().DefaultView;
                _endFormData = ProdSearchModel.EndFormCombo.ToTable().Copy();
                _endFormData.CaseSensitive = false;
                ProdSearchModel.DrivingFeatureCombo = _productSearchBll.GetTypeFastenersCombo(ProdSearchModel.Family.ToValueAsString(), "DRIVING FEATURE").ToDataTable<FASTENERS_MASTER>().DefaultView;
                _drivingFeatureData = ProdSearchModel.DrivingFeatureCombo.ToTable().Copy();
                _drivingFeatureData.CaseSensitive = false;
                ProdSearchModel.AdditionalFeatureCombo = _productSearchBll.GetTypeFastenersCombo(ProdSearchModel.Family.ToValueAsString(), "ADDITIONAL FEATURE").ToDataTable<FASTENERS_MASTER>().DefaultView;
                _additionalFeatureData = ProdSearchModel.AdditionalFeatureCombo.ToTable().Copy();
                _additionalFeatureData.CaseSensitive = false;
                ProdSearchModel.KeywordsCombo = _productSearchBll.GetTypeFastenersCombo(ProdSearchModel.Family.ToValueAsString(), "KEYWORDS").ToDataTable<FASTENERS_MASTER>().DefaultView;
                _keywordsData = ProdSearchModel.KeywordsCombo.ToTable().Copy();
                _keywordsData.CaseSensitive = false;
                ProdSearchModel.ProductFamilyCombo = _productSearchBll.GetProductFamilyCombo().ToDataTable<PRD_CLASS_MAST>().DefaultView;
                ProdSearchModel.HeadStyleCombo = _productSearchBll.GetHeadStyleCombo();
                ProdSearchModel.ProductTypeCombo = _productSearchBll.GetProductTypeCombo();
                ProdSearchModel.ApplicationCombo = _productSearchBll.GetApplicationCombo();
                _applicationCombo = ProdSearchModel.ApplicationCombo.ToTable().Copy();
                ProdSearchModel.PrintEnabled = false;
                ////_pro
                //CategoryCombo = dsData.Tables[0].DefaultView;
                //LocationCombo = dsData.Tables[1].DefaultView;
                //ModuleCombo = dsData.Tables[2].DefaultView;
                //CostCenterCodeCombo = dsData.Tables[3].DefaultView;
                //OperCodeMaster = dsData.Tables[4].DefaultView;
                //UnitCodeMaster = dsData.Tables[5].DefaultView;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        public void Search()
        {
            try
            {

                Progress.ProcessingText = PDMsg.Search;
                Progress.Start();
                ToolInfoViewModel tfvm = new ToolInfoViewModel();
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                if (ProdSearchModel.ToolCode.Length > 0)
                {
                    try
                    {
                        ProdSearchModel.ToolCode = tfvm.Chk_Tool(ProdSearchModel.ToolCode);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                NotifyPropertyChanged("ProdSearchModel");
                ProductResult = _productSearchBll.GetProductSearchDetails(ProdSearchModel);
                if (ProductResult.Count > 0)
                    ProdSearchModel.PrintEnabled = true;
                else
                    ProdSearchModel.PrintEnabled = false;
                ProdSearchModel.TotalRecords = "Part Details (" + ProductResult.Count.ToString() + " Part" + (ProductResult.Count > 1 ? "s" : "") + " Found)";
                if (ProductResult.Count == 0)
                {
                    ProductResult.AddNew();
                }
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                Progress.End();
            }
            catch (Exception ex)
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
                throw ex.LogException();
            }
        }

        private void SetdropDownItems()
        {
            try
            {


                DropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "CUST_CODE", ColumnDesc = "Customer Code", ColumnWidth = "40*" },
                            new DropdownColumns() { ColumnName = "CUST_NAME", ColumnDesc = "Customer Name", ColumnWidth = "60*" },
                        };

                DropDownItemsFamily = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PRODUCT_CATEGORY", ColumnDesc = "Product Category", ColumnWidth = "80*" },
                        };

                DropDownItemsFamilyType = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "Type", ColumnWidth = "100*" },
                        };
                DropDownItemsHeadForm = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "Head Form", ColumnWidth = "100*" },
                        };

                DropDownItemsEndForm = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "End Form", ColumnWidth = "100*" },
                        };

                DropDownItemsDrivingFeature = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "Finish Description", ColumnWidth = "100*" },
                        };

                DropDownItemsAdditionalFeature = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "Additional Feature", ColumnWidth = "100*" },
                        };

                DropDownItemsKeywords = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "Keywords", ColumnWidth = "100*" },
                        };

                DropDownItemsKeywords = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "Keywords", ColumnWidth = "100*" },
                        };

                DropDownItemsProductFamily = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PRD_CLASS_CD", ColumnDesc = "Product Class Code", ColumnWidth = "40*" },
                            new DropdownColumns() { ColumnName = "PRD_CLASS_DESC", ColumnDesc = "Family", ColumnWidth = "60*" },
                        };

                DropDownItemsHeadStyle = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "HEAD_STYLE", ColumnDesc = "Head Style", ColumnWidth = "100*" }
                        };

                DropDownItemsProductType = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "TYPE", ColumnDesc = "Type", ColumnWidth = "100*" }
                        };

                DropDownItemsApplication = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUB_TYPE", ColumnDesc = "Application", ColumnWidth = "100*" }
                        };

                DropDownItemsRMSpec = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "RM_CODE", ColumnDesc = "RM Code", ColumnWidth = "100" },
                            new DropdownColumns() { ColumnName = "RM_DESC", ColumnDesc = "RM Description", ColumnWidth = "1*" }
                        };

                DropDownItemsFinish = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "FINISH_CODE", ColumnDesc = "Finish Code", ColumnWidth = "100" },
                            new DropdownColumns() { ColumnName = "FINISH_DESC", ColumnDesc = "Finish Description", ColumnWidth = "1*" }
                        };

                DropDownItemsManufacturedAt = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Location Code", ColumnWidth = "50*" },
                            new DropdownColumns() { ColumnName = "LOCATION", ColumnDesc = "Location Description", ColumnWidth = "50*" }
                        };

                DropDownItemsCostCentre = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "COST_CENT_CODE", ColumnDesc = "Cost Centre Code", ColumnWidth = "50*" },
                            new DropdownColumns() { ColumnName = "COST_CENT_DESC", ColumnDesc = "Cost Centre Description", ColumnWidth = "50*" }
                        };

                DropDownItemsOperation = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "OPER_CODE", ColumnDesc = "Operation Code", ColumnWidth = "50*" },
                            new DropdownColumns() { ColumnName = "OPER_DESC", ColumnDesc = "Operation Description", ColumnWidth = "50*" }
                        };

                DropDownItemsShankForm = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "Shank Form", ColumnWidth = "100*" },
                        };


                /*
                 
                <toolKit:DataGridTextColumn Width="200" Header="Shank Form" Binding="{Binding SUBTYPE}" /> 
                <toolKit:DataGridTextColumn Width="150"  Header="Operation Code" Binding="{Binding OPER_CODE}" />
                <toolKit:DataGridTextColumn Width="150" Header="Operation Description" Binding="{Binding OPER_DESC}" />
                
                <toolKit:DataGridTextColumn Width="150"  Header="Cost Centre Code" Binding="{Binding COST_CENT_CODE}" />
                <toolKit:DataGridTextColumn Width="160" Header="Cost Centre Description" Binding="{Binding COST_CENT_DESC}" />

                <toolKit:DataGridTextColumn Width="110"  Header="Location Code" Binding="{Binding LOC_CODE}"  />
                <toolKit:DataGridTextColumn Width="150" Header="Location Description" Binding="{Binding LOCATION}" />
                <toolKit:DataGridTextColumn Width="75"  Header="RM Code" Binding="{Binding RM_CODE}" />
                <toolKit:DataGridTextColumn Width="150" Header="RM Description" Binding="{Binding RM_DESC}" />
                */

                //<toolKit:DataGridTextColumn Width="200" Header="Type" Binding="{Binding TYPE}" />


                //<toolKit:DataGridTextColumn Width="200" Header="Head Style" Binding="{Binding HEAD_STYLE}" />

                /*
                DropDownItemsShankForm = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "Shank Form", ColumnWidth = "100*" },
                        };

                DropDownItemsEndForm = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "Shank Form", ColumnWidth = "100*" },
                        };
                DropDownItemsDrivingFeature = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "Finish Description", ColumnWidth = "100*" },
                        };
                DropDownItemsAdditionalFeature = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "Additional Feature", ColumnWidth = "100*" },
                        };
                DropDownItemsKeywords = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "SUBTYPE", ColumnDesc = "keywords", ColumnWidth = "100*" },
                        };
                DropDownItemsProductFamily = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PRD_CLASS_CD", ColumnDesc = "Product Class Code", ColumnWidth = "100" },
                            new DropdownColumns() { ColumnName = "PRD_CLASS_DESC", ColumnDesc = "Family", ColumnWidth = "150" },
                        };
                */




            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        /// <summary>
        /// Filter cost centre based on location(ManufacturedAt)
        /// </summary>
        private void FilterCostCentre()
        {
            try
            {
                if (ProdSearchModel.ManufacturedAtText != "")
                {
                    _costCentreData.DefaultView.RowFilter = "LOC_CODE='" + ProdSearchModel.ManufacturedAt.ToValueAsString().ToUpper() + "'";
                    ProdSearchModel.CostCentreCombo = _costCentreData.DefaultView;
                }
                else
                {
                    _costCentreData.DefaultView.RowFilter = "";
                    ProdSearchModel.CostCentreCombo = _costCentreData.DefaultView;
                }
                NotifyPropertyChanged("ProdSearchModel");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }



        /// <summary>
        /// Filter based on product category
        /// </summary>
        private void FilterAll()
        {
            try
            {
                if (ProdSearchModel.FamilyText.ToValueAsString().Trim() != "")
                {
                    _familyTypeData.DefaultView.RowFilter = "PRODUCT_CATEGORY='" + sqlEncode(ProdSearchModel.FamilyText.ToValueAsString().ToUpper()) + "'";
                    _headFormData.DefaultView.RowFilter = "PRODUCT_CATEGORY='" + sqlEncode(ProdSearchModel.FamilyText.ToValueAsString().ToUpper()) + "'";
                    _shankFormData.DefaultView.RowFilter = "PRODUCT_CATEGORY='" + sqlEncode(ProdSearchModel.FamilyText.ToValueAsString().ToUpper()) + "'";
                    _endFormData.DefaultView.RowFilter = "PRODUCT_CATEGORY='" + sqlEncode(ProdSearchModel.FamilyText.ToValueAsString().ToUpper()) + "'";
                    _drivingFeatureData.DefaultView.RowFilter = "PRODUCT_CATEGORY='" + sqlEncode(ProdSearchModel.FamilyText.ToValueAsString().ToUpper()) + "'";
                    _additionalFeatureData.DefaultView.RowFilter = "PRODUCT_CATEGORY='" + sqlEncode(ProdSearchModel.FamilyText.ToValueAsString().ToUpper()) + "'";
                    _keywordsData.DefaultView.RowFilter = "PRODUCT_CATEGORY='" + sqlEncode(ProdSearchModel.FamilyText.ToValueAsString().ToUpper()) + "'";
                }
                else
                {
                    _familyTypeData.DefaultView.RowFilter = "";
                    _headFormData.DefaultView.RowFilter = "";
                    _shankFormData.DefaultView.RowFilter = "";
                    _endFormData.DefaultView.RowFilter = "";
                    _drivingFeatureData.DefaultView.RowFilter = "";
                    _additionalFeatureData.DefaultView.RowFilter = "";
                    _keywordsData.DefaultView.RowFilter = "";
                }
                ProdSearchModel.FamilyTypeCombo = _familyTypeData.DefaultView;
                ProdSearchModel.HeadFormCombo = _headFormData.DefaultView;
                ProdSearchModel.ShankFormCombo = _shankFormData.DefaultView;
                ProdSearchModel.EndFormCombo = _endFormData.DefaultView;
                ProdSearchModel.DrivingFeatureCombo = _drivingFeatureData.DefaultView;
                ProdSearchModel.AdditionalFeatureCombo = _additionalFeatureData.DefaultView;
                ProdSearchModel.KeywordsCombo = _keywordsData.DefaultView;
                NotifyPropertyChanged("ProdSearchModel");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        /// <summary>
        /// Filter head form based on product category
        /// </summary>
        private void FilterHeadForm()
        {
            try
            {
                if (ProdSearchModel.FamilyText.ToValueAsString().Trim() != "")
                {
                    _headFormData.DefaultView.RowFilter = "PRODUCT_CATEGORY='" + ProdSearchModel.FamilyText.ToValueAsString().ToUpper() + "'";
                    ProdSearchModel.HeadFormCombo = _headFormData.DefaultView;
                }
                else
                {
                    _headFormData.DefaultView.RowFilter = "";
                    ProdSearchModel.HeadFormCombo = _headFormData.DefaultView;
                }
                NotifyPropertyChanged("ProdSearchModel");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void ShowProductInformation()
        {
            try
            {
                //    public frmProductInformation(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, int entityPrimaryKey,
                //OperationMode operationMode, string title = "Feasibility Report and Cost Sheet")

                //return;
                if (SelectedPartRow != null)
                {
                    string part_no = string.Empty;
                    if (SelectedPartRow.DataView.Table.Columns.Contains("PART_NO")) part_no = SelectedPartRow["PART_NO"].ToValueAsString();
                    if (!part_no.IsNotNullOrEmpty()) return;

                    ProductInformation bll = new ProductInformation(_userInformation);
                    PRD_MAST parentEntity = bll.GetEntityByPartNumber(new PRD_MAST() { PART_NO = part_no });

                    //Window win = new Window();
                    //win.Title = ApplicationTitle + " - Prodcut Master";

                    //System.Windows.Media.Imaging.IconBitmapDecoder ibd = new System.Windows.Media.Imaging.IconBitmapDecoder(new Uri(@"pack://application:,,/Images/logo.ico", UriKind.RelativeOrAbsolute), System.Windows.Media.Imaging.BitmapCreateOptions.None, System.Windows.Media.Imaging.BitmapCacheOption.Default);

                    //win.Icon = ibd.Frames[0];
                    //win.ResizeMode = ResizeMode.NoResize;
                    //ProcessDesigner.frmProductInformation userControl = new ProcessDesigner.frmProductInformation(_userInformation, win, parentEntity.IDPK, OperationMode.Edit, win.Title);
                    //win.Content = userControl;
                    //win.Height = userControl.Height + 50;
                    //win.Width = userControl.Width + 10;
                    //win.ShowInTaskbar = false;
                    //win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    //win.ShowDialog();


                    MdiChild frmProductInformationChild = new MdiChild
                    {
                        Title = ApplicationTitle + " - Product Master",
                        MaximizeBox = false,
                        MinimizeBox = false
                    };

                    ProcessDesigner.frmProductInformation productInformation = new ProcessDesigner.frmProductInformation(_userInformation,
                        frmProductInformationChild, parentEntity.IDPK, OperationMode.Edit);
                    frmProductInformationChild.Content = productInformation;
                    frmProductInformationChild.Height = productInformation.Height + 50;
                    frmProductInformationChild.Width = productInformation.Width + 20;
                    MainMDI.Container.Children.Add(frmProductInformationChild);

                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private void PrintProductSearch()
        {
            try
            {
                Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                if (ProductResult != null)
                {
                    Progress.ProcessingText = PDMsg.Load;
                    Progress.Start();
                    frmReportViewer showProductReport = new frmReportViewer(ProductResult.ToTable(), "ProductSearch");
                    if (!showProductReport.ReadyToShowReport) return;
                    showProductReport.ShowDialog();
                    Progress.End();
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private string sqlEncode(string sqlValue)
        {
            return sqlValue.Replace("'", "''");
        }
        //private void ShowDrawing()
        //{
        //    string filename = "";
        //    try
        //    {
        //        if (DrawingControl != null)
        //        {
        //            DrawingControl.Src = "";
        //            if (SelectedPartRow != null)
        //            {
        //                if (SelectedPartRow["PART_NO"].ToValueAsString() != "")
        //                {
        //                    filename = _productSearchBll.ShowDrawing(SelectedPartRow["PART_NO"].ToValueAsString());
        //                    if (filename != "")
        //                    {
        //                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
        //                        DrawingControl.Src = filename;
        //                        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        //        MessageBox.Show("Unable to show Image!", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Information);
        //        throw ex.LogException();
        //    }
        //    Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        //}

        private void ValidateApplicationCombo()
        {
            if (ProdSearchModel.ProductFamilyText.ToValueAsString() != "" && ProdSearchModel.HeadStyleText.Trim() != "" && ProdSearchModel.ProductTypeText.Trim() != "")
            {
                if (_applicationCombo.Rows.Count == 0)
                {
                    ShowInformationMessage("There are no entries in the list!");
                }
            }
            else
            {
                ShowInformationMessage("Product Family, Head Style and Product Type should be selected!");
            }
        }

        private MessageBoxResult ShowInformationMessage(string _showMessage)
        {
            if (_showMessage.IsNotNullOrEmpty())
                return MessageBox.Show(_showMessage, ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
            return MessageBoxResult.None;
        }


    }
}
