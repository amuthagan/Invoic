using Microsoft.Practices.Prism.Commands;
using ProcessDesigner.BLL;
using ProcessDesigner.Common;
using ProcessDesigner.Model;
using ProcessDesigner.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ProcessDesigner.Reports;
using WPF.MDI;
using System.Windows.Controls;
using ProcessDesigner.DAL;

namespace ProcessDesigner.ViewModel
{
    public class ProductInformationViewModel : ViewModelBase
    {
        UserInformation _userInformation = null;
        private const string CONS_RIGHTS_NAME = "PRODUCT MASTER";
        ProductInformation bll = null;
        public string LOCATION_CODE;

        FeasibleReportAndCostSheet bllFRCS = null;
        PartNumberConfiguration bllPartNumberConfiguration = null;
        private ISIR isirbll;
        private PCCSBll _pccsBll;
        private LogViewBll _logviewBll;

        PartSubmissionWarrantModel pm = null;

        WPF.MDI.MdiChild mdiChild = null;
        System.Windows.Window self = null;
        private PartSubmissionWarrantBll partSubmissionBll = null;
        public ProductInformationViewModel(UserInformation userInformation, WPF.MDI.MdiChild mdiChild, int entityPrimaryKey,
            OperationMode operationMode, string title = "Product Master", System.Windows.Window self = null)
        {
            _userInformation = userInformation;
            this.mdiChild = mdiChild;
            this.self = self;
            partSubmissionBll = new PartSubmissionWarrantBll(userInformation);
            bll = new ProductInformation(userInformation);
            bllFRCS = new FeasibleReportAndCostSheet(userInformation);
            bllPartNumberConfiguration = new PartNumberConfiguration(userInformation);
            this._pccsBll = new PCCSBll(userInformation);
            this._logviewBll = new LogViewBll(userInformation);

            pm = new PartSubmissionWarrantModel();
            PARTSUBMISSIONWARRANT = new PartSubmissionWarrantModel();

            Title = title.IsNotNullOrEmpty() ? title : "Product Master";

            EntityPrimaryKey = entityPrimaryKey;

            #region Dropdown Load
            DataTable dtPartNumberConfig = bll.GetPartNumberConfigByPrimaryKey().ToDataTableWithType<PartNumberConfig>();
            if (dtPartNumberConfig.Columns.Contains("Description"))
                dtPartNumberConfig.Columns["Description"].ColumnName = "PART_CONFIG_ID";
            PartNumberConfigDataSource = dtPartNumberConfig.DefaultView;

            PartNumberDataSource = bll.GetEntityByPrimaryKey().ToDataTableWithType<PRD_MAST>().DefaultView;
            //PartNumberDataSource = GetProductMasterDetailsByPartNumberDV();
            DataTable dtManufacturingStandard = bll.GetManufacturingStandardByPrimaryKey().ToDataTable<PRD_MANUFACTURING_STANDARD>();
            if (dtManufacturingStandard.Columns.Contains("Description"))
                dtManufacturingStandard.Columns["Description"].ColumnName = "MFG_STD";
            ManufacturingStandardDataSource = dtManufacturingStandard.DefaultView;

            DataTable dtThreadCode = bll.GetThreadCodeByPrimaryKey().ToDataTable<PRD_THREAD_CODE>();
            if (dtThreadCode.Columns.Contains("Description"))
                dtThreadCode.Columns["Description"].ColumnName = "THREAD_CD";
            ThreadCodeDataSource = dtThreadCode.DefaultView;

            DataTable dtForecastLocation = bll.GetLocationByPrimaryKey().ToDataTable<DDLOC_MAST>();
            if (dtForecastLocation.Columns.Contains("LOCATION"))
                //new
                //if (dtForecastLocation.Columns.Contains("LOC_CODE"))
                //end new
                dtForecastLocation.Columns["LOCATION"].ColumnName = "BIF_FORECAST";
            ForecastLocationDataSource = dtForecastLocation.DefaultView;

            DataTable dtCurrentLocation1 = bll.GetLocationByPrimaryKey().ToDataTable<DDLOC_MAST>();
            if (dtCurrentLocation1.Columns.Contains("LOCATION"))
                //new
                //if (dtCurrentLocation1.Columns.Contains("LOC_CODE"))
                //end new
                dtCurrentLocation1.Columns["LOCATION"].ColumnName = "BIF_PROJ";
            CurrentLocation1DataSource = dtCurrentLocation1.DefaultView;

            DataTable dtCurrentLocation2 = bll.GetLocationByPrimaryKey().ToDataTable<DDLOC_MAST>();
            //if (dtCurrentLocation2.Columns.Contains("LOCATION"))
            //uncomment by me
            dtCurrentLocation2.Columns["LOCATION"].ColumnName = "LOCATION";
            //end uncomment
            CurrentLocation2DataSource = dtCurrentLocation2.DefaultView;

            DataTable dtHeatTreatment = bll.GetHeatTreatmentByPrimaryKey().ToDataTable<HEAT_TREAT_MAST>();
            if (dtHeatTreatment.Columns.Contains("HT_DESC"))
                dtHeatTreatment.Columns["HT_DESC"].ColumnName = "HEAT_TREATMENT_CD";
            HeatTreatmentDataSource = dtHeatTreatment.DefaultView;

            DataTable dtPSWApproved = bll.GetPSWApprovedByPrimaryKey().ToDataTable<PRD_PSW_APPROVED>();
            if (dtPSWApproved.Columns.Contains("Description"))
                dtPSWApproved.Columns["Description"].ColumnName = "PSW_ST";
            PSWApprovedDataSource = dtPSWApproved.DefaultView;
            CIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER_PI>().DefaultView;
            CustomersDataSource = bllFRCS.GetCustomerDetails().ToDataTable<DDCUST_MAST>().DefaultView;

            DataTable dtProductFamily = bll.GetProductFamilyByPrimaryKey().ToDataTable<FASTENERS_MASTER>();
            if (dtProductFamily.Columns.Contains("PRODUCT_CATEGORY"))
                dtProductFamily.Columns["PRODUCT_CATEGORY"].ColumnName = "FAMILY";
            ProductFamilyDataSource = dtProductFamily.DefaultView;

            DataTable dtSimilarity = bll.GetSimilarityByPrimaryKey().ToDataTable<SIM_TO_STD_MAST>();
            if (dtSimilarity.Columns.Contains("STS_DESC"))
                dtSimilarity.Columns["STS_DESC"].ColumnName = "SIM_TO_STD_CD";
            SimilarityDataSource = dtSimilarity.DefaultView;

            DataTable dtPGCategory = bll.GetProductPGCategoryByPrimaryKey().ToDataTable<PG_CATEGORY>();
            if (!dtPGCategory.Columns.Contains("PG_CAT_CODE"))
            {
                dtPGCategory.Columns.Add("PG_CAT_CODE");
            }
            if (dtPGCategory.Columns.Contains("PG_CAT_DESC"))
                dtPGCategory.Columns["PG_CAT_DESC"].ColumnName = "PG_CATEGORY";
            foreach (DataRow item in dtPGCategory.Rows)
            {
                item.BeginEdit();
                item["PG_CAT_CODE"] = "PG" + item["PG_CAT"];
                item.EndEdit();
            }

            ProductPGCategoryDataSource = dtPGCategory.DefaultView;

            loadAllProductFamilyAndSubFamilies();
            #endregion

            //ActiveEntity = new PRD_MAST();
            //MandatoryFields = new ProductInformationModel();
            //ActiveMFMEntity = new MFM_MAST();
            //ActivProcessIssueEntity = new MFM_MAST();
            //ActivProductDrawingIssueEntity = new PRD_DWG_ISSUE();
            //ActivSequenceDrawingIssueEntity = new PRD_DWG_ISSUE();
            //ActiveCIInfoEntity = new DDCI_INFO();

            #region DropdownColumns Settins
            PartNumberConfigDropDownItems = new ObservableCollection<DropdownColumns>()
                        {                                                    
                            new DropdownColumns() { ColumnName = "Location_code", ColumnDesc = "Location Code", ColumnWidth = "80" },
                            new DropdownColumns() { ColumnName = "PART_CONFIG_ID", ColumnDesc = "Category", ColumnWidth = "1*" },                           
                        };

            PartNumberDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "IDPK", ColumnDesc = "ID", ColumnWidth = "90" },
                            new DropdownColumns() { ColumnName = "PART_NO", ColumnDesc = "Part Number", ColumnWidth = 120 },
                            new DropdownColumns() { ColumnName = "PART_DESC", ColumnDesc = "Part Description", ColumnWidth = "1*" },
                            //new DropdownColumns() { ColumnName = "DOC_REL_DATE", ColumnDesc = "Document Release Date", ColumnWidth = "100" },
                            //new DropdownColumns() { ColumnName = "SAMP_SUBMIT_DATE", ColumnDesc = "Sample Submission Date", ColumnWidth = "80" },
                            //new DropdownColumns() { ColumnName = "BIF_PROJ", ColumnDesc = "Projected Location", ColumnWidth = "175" },
                            //new DropdownColumns() { ColumnName = "BIF_FORECAST", ColumnDesc = "Forecast Location", ColumnWidth = "150" },
                            //new DropdownColumns() { ColumnName = "ALLOT_DATE", ColumnDesc = "Allotted Date", ColumnWidth = "175" },
                            //new DropdownColumns() { ColumnName = "ALLOTTED_BY", ColumnDesc = "Allotted By", ColumnWidth = "175" },
                            //new DropdownColumns() { ColumnName = "PSW_ST", ColumnDesc = "PSW Status", ColumnWidth = "175" },
                        };

            ManufacturingStandardDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "CODE", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "MFG_STD", ColumnDesc = "Manufacturing Standard", ColumnWidth = "75*" }
                        };

            ThreadCodeDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "CODE", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "THREAD_CD", ColumnDesc = "Thread Code", ColumnWidth = "75*" }
                        };

            SimilarityDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "STS_CD", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "SIM_TO_STD_CD", ColumnDesc = "Similarity", ColumnWidth = "75*" }
                        };

            ProductFamilyDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "PRD_CODE", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "FAMILY", ColumnDesc = "FAMILY", ColumnWidth = "75*" }
                        };

            ForecastLocationDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Location Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "BIF_FORECAST", ColumnDesc = "Location", ColumnWidth = "75*" }
                        };

            CurrentLocation1DropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Location Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "BIF_PROJ", ColumnDesc = "Location", ColumnWidth = "75*" }
                        };

            CurrentLocation2DropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "LOC_CODE", ColumnDesc = "Location Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "LOCATION", ColumnDesc = "Location", ColumnWidth = "75*" }
                        };

            HeatTreatmentDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           //new DropdownColumns() { ColumnName = "HT_CD", ColumnDesc = "Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "HEAT_TREATMENT_CD", ColumnDesc = "Heat Treatment", ColumnWidth = "75*" }
                        };

            PSWApprovedDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "CODE", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "PSW_ST", ColumnDesc = "PSW Approved", ColumnWidth = "75*" }
                        };

            ProductKeywordsDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "PRD_CODE", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "KEYWORDS", ColumnDesc = "Keywords", ColumnWidth = "75*" }
                        };

            ProductPGCategoryDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            new DropdownColumns() { ColumnName = "PG_CAT_CODE", ColumnDesc = "Code", ColumnWidth = 57 },
                            new DropdownColumns() { ColumnName = "PG_CATEGORY", ColumnDesc = " Product Group Category", ColumnWidth = "1*" }
                        };

            ProductAdditionalFeatureDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "PRD_CODE", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "ADDL_FEATURE", ColumnDesc = "Additional Feature", ColumnWidth = "75*" }
                        };

            ProductDrivingFeatureDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "PRD_CODE", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "PRD_GRP_CD", ColumnDesc = "Driving Feature", ColumnWidth = "75*" }
                        };

            ProductEndFormDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "PRD_CODE", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "PRD_CLASS_CD", ColumnDesc = "End Form", ColumnWidth = "75*" }
                        };

            ProductHeadFormDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "PRD_CODE", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "HEAD_STYLE", ColumnDesc = "Head Form", ColumnWidth = "75*" }
                        };

            ProductShankFormDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "PRD_CODE", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "APPLICATION", ColumnDesc = "Shank Form", ColumnWidth = "75*" }
                        };

            ProductTypeDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "PRD_CODE", ColumnDesc = "Code", ColumnWidth = "25*" },
                            new DropdownColumns() { ColumnName = "TYPE", ColumnDesc = "Type", ColumnWidth = "75*" }
                        };

            CIReferenceDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                            //new DropdownColumns() { ColumnName = "IDPK", ColumnDesc = "Code", ColumnWidth = "90" },
                            new DropdownColumns() { ColumnName = "CI_REFERENCE", ColumnDesc = "CI Reference", ColumnWidth = "90" },
                            //new DropdownColumns() { ColumnName = "FRCS_DATE", ColumnDesc = "FRCS Date", ColumnWidth = "80" },
                            new DropdownColumns() { ColumnName = "CUST_DWG_NO", ColumnDesc = "Customer Drawing Number", ColumnWidth = "100" },
                            new DropdownColumns() { ColumnName = "CUST_CODE", ColumnDesc = "Customer Code", ColumnWidth = "100" },
                            new DropdownColumns() { ColumnName = "FINISH_CODE", ColumnDesc = "Finish Code", ColumnWidth = "80" },
                            new DropdownColumns() { ColumnName = "CUST_DWG_NO_ISSUE", ColumnDesc = "Customer Drawing Issue No.", ColumnWidth = "175" },
                            //new DropdownColumns() { ColumnName = "CUST_DWG_NO_ISSUE", ColumnDesc = "Customer Drawing Issue Date", ColumnWidth = "175" },
                            new DropdownColumns() { ColumnName = "CUST_STD_DATE", ColumnDesc = "Customer STD Date", ColumnWidth = "150" },
                            new DropdownColumns() { ColumnName = "PROD_DESC", ColumnDesc = "Product Description", ColumnWidth = "150" }
                        };

            CustomerDropDownItems = new ObservableCollection<DropdownColumns>()
                        {
                           new DropdownColumns() { ColumnName = "CUST_CODE", ColumnDesc = "Customer Code", ColumnWidth = "25*" },
                           new DropdownColumns() { ColumnName = "CUST_NAME", ColumnDesc = "Customer", ColumnWidth = "75*" }
                        };

            #endregion

            #region Event Init
            this.addNewCommand = new DelegateCommand(this.AddNewSubmitCommand);
            this.editCommand = new DelegateCommand(this.EditSubmitCommand);
            this.viewCommand = new DelegateCommand(this.ViewSubmitCommand);
            this.deleteCommand = new DelegateCommand(this.DeleteSubmitCommand);
            this.saveCommand = new DelegateCommand(this.SaveSubmitCommand);
            this.closeCommand = new DelegateCommand(this.Close);
            this.printCommand = new DelegateCommand(this.PrintSubmitCommand);
            this.seqDwgCommand = new DelegateCommand(this.SeqSubmitDwgCommand);
            this.copyProductInformationCommand = new DelegateCommand(this.copyProductInformationSubmitCommand);

            this.partNumberConfigEndEditCommand = new DelegateCommand(this.partNumberConfigEndEdit);
            this.partNumberConfigSelectedItemChangedCommand = new DelegateCommand(this.partNumberConfigChanged);

            this.partNumberEnterKeyPressedCommand = new DelegateCommand(this.partNumberEnterKeyPressed);
            this.partNumberSelectedItemChangedCommand = new DelegateCommand(this.partNumberChanged);

            this.manufacturingStandardSelectedItemChangedCommand = new DelegateCommand(this.manufacturingStandardChanged);
            this.manufacturingStandardEndEditCommand = new DelegateCommand(this.manufacturingStandardEndEdit);

            this.threadCodeSelectedItemChangedCommand = new DelegateCommand(this.threadCodeChanged);
            this.threadCodeEndEditCommand = new DelegateCommand(this.threadCodeEndEdit);

            this.similaritySelectedItemChangedCommand = new DelegateCommand(this.similarityChanged);
            this.similarityEndEditCommand = new DelegateCommand(this.similarityEndEdit);

            this.productFamilySelectedItemChangedCommand = new DelegateCommand(this.productFamilyChanged);
            this.productFamilyEndEditCommand = new DelegateCommand(this.productFamilyEndEdit);

            this.forecastLocationSelectedItemChangedCommand = new DelegateCommand(this.forecastLocationChanged);
            this.forecastLocationEndEditCommand = new DelegateCommand(this.forecastLocationEndEdit);

            this.currentLocation1SelectedItemChangedCommand = new DelegateCommand(this.currentLocation1Changed);
            this.currentLocation1EndEditCommand = new DelegateCommand(this.currentLocation1EndEdit);

            this.currentLocation2SelectedItemChangedCommand = new DelegateCommand(this.currentLocation2Changed);
            this.currentLocation2EndEditCommand = new DelegateCommand(this.currentLocation2EndEdit);

            this.heatTreatmentSelectedItemChangedCommand = new DelegateCommand(this.heatTreatmentChanged);
            this.heatTreatmentEndEditCommand = new DelegateCommand(this.heatTreatmentEndEdit);

            this.estimatedFinishWeightLostFocusCommand = new DelegateCommand(this.estimatedFinishWeightLostFocus);
            this.finishWeightLostFocusCommand = new DelegateCommand(this.finishWeightLostFocus);

            this.pswApprovedSelectedItemChangedCommand = new DelegateCommand<string>(this.pswApprovedChanged);
            this.pswApprovedEndEditCommand = new DelegateCommand(this.pswApprovedEndEdit);

            this.nosPerPackLostFocusCommand = new DelegateCommand(this.nosPerPackLostFocus);
            this.plannedDocumentReleaseDateOnChangedCommand = new DelegateCommand(this.plannedDocumentReleaseDateOnChanged);

            this.samplesSubmissionDateOnChangedCommand = new DelegateCommand(this.samplesSubmissionDateOnChanged);
            this.samplesSubmissionDateLostFocusCommand = new DelegateCommand(this.samplesSubmissionDateLostFocus);


            this.productKeywordsSelectedItemChangedCommand = new DelegateCommand(this.productKeywordsChanged);
            this.productKeywordsEndEditCommand = new DelegateCommand(this.productKeywordsEndEdit);

            this.productPGCategorySelectedItemChangedCommand = new DelegateCommand(this.productPGCategoryChanged);
            this.productPGCategoryEndEditCommand = new DelegateCommand(this.productPGCategoryEndEdit);

            this.productAdditionalFeatureSelectedItemChangedCommand = new DelegateCommand(this.productAdditionalFeatureChanged);
            this.productAdditionalFeatureEndEditCommand = new DelegateCommand(this.productAdditionalFeatureEndEdit);

            this.productDrivingFeatureSelectedItemChangedCommand = new DelegateCommand(this.productDrivingFeatureChanged);
            this.productDrivingFeatureEndEditCommand = new DelegateCommand(this.productDrivingFeatureEndEdit);

            this.productEndFormSelectedItemChangedCommand = new DelegateCommand(this.productEndFormChanged);
            this.productEndFormEndEditCommand = new DelegateCommand(this.productEndFormEndEdit);

            this.productHeadFormSelectedItemChangedCommand = new DelegateCommand(this.productHeadFormChanged);
            this.productHeadFormEndEditCommand = new DelegateCommand(this.productHeadFormEndEdit);

            this.productShankFormSelectedItemChangedCommand = new DelegateCommand(this.productShankFormChanged);
            this.productShankFormEndEditCommand = new DelegateCommand(this.productShankFormEndEdit);

            this.productTypeSelectedItemChangedCommand = new DelegateCommand(this.productTypeChanged);
            this.productTypeEndEditCommand = new DelegateCommand(this.productTypeEndEdit);

            this._productcirefselectionchangedcommand = new DelegateCommand(this.ProductCIReferenceChanged);

            this.ciReferenceSelectedItemChangedCommand = new DelegateCommand(this.CIReferenceSelectionChanged);
            this.ciReferenceMouseDoubleClickCommand = new DelegateCommand(this.ciReferenceMouseDoubleClick);
            this.customerEndEditCommand = new DelegateCommand(this.customerEndEdit);

            this.releaseDocumentCommand = new DelegateCommand(this.releaseDocumentSubmitCommand);
            this.updateOrderProcessingCommand = new DelegateCommand(this.UpdateOrderProcessing);
            this.createCIReferenceCommand = new DelegateCommand(this.createCIReferenceSubmitCommand);
            this.iSIRClickCommand = new DelegateCommand(this.iSIRReport);
            this.pswClickCommand = new DelegateCommand(this.pswReport);
            this.dimensionalClickCommand = new DelegateCommand<string>(this.dimensionalReport);
            this.materialClickCommand = new DelegateCommand<string>(this.materialReport);
            this.performanceClickCommand = new DelegateCommand(this.perfomanceReport);
            this.checklistClickCommand = new DelegateCommand(this.checklistReport);
            this.initialSampleInspectionReportClickCommand = new DelegateCommand(this.initialReport);

            this.productSearchCommand = new DelegateCommand(this.ProductSearch);
            this.costSheetSearchCommand = new DelegateCommand(this.CostSheetSearch);
            this.toolsSearchCommand = new DelegateCommand(this.ToolsSearch);
            this.controlPlanCommand = new DelegateCommand(this.ControlPlan);
            this.devlptRptCommand = new DelegateCommand(this.DevlptRpt);
            this.mfgRptCommand = new DelegateCommand(this.MfgRpt);
            this.drawingsCommand = new DelegateCommand(this.Drawings);
            this.toolScheduleCommand = new DelegateCommand(this.ToolSchedule);

            this.costSheetCommand = new DelegateCommand(this.CostSheet);
            this._processSheetCommand = new DelegateCommand(this.ProcessSheet);
            this.onCheckChangeCommandCIRef = new DelegateCommand(this.CheckChangeCIRef);

            #endregion

            ActionMode = operationMode;

            ActualPermission = bll.GetUserRights(CONS_RIGHTS_NAME);
            ActionPermission = ActualPermission.DeepCopy<RolePermission>();
            ChangeRights();
        }

        private void loadAllProductFamilyAndSubFamilies(FASTENERS_MASTER paramEntity = null, string productCategory = null)
        {

            DataTable dtKeywords = bll.GetProductKeywordsByPrimaryKey(paramEntity, productCategory).ToDataTable<FASTENERS_MASTER>();
            if (dtKeywords.Columns.Contains("SUBTYPE"))
                dtKeywords.Columns["SUBTYPE"].ColumnName = "KEYWORDS";
            ProductKeywordsDataSource = dtKeywords.DefaultView;

            DataTable dtAdditionalFeature = bll.GetProductAdditionalFeatureByPrimaryKey(paramEntity, productCategory).ToDataTable<FASTENERS_MASTER>();
            if (dtAdditionalFeature.Columns.Contains("SUBTYPE"))
                dtAdditionalFeature.Columns["SUBTYPE"].ColumnName = "ADDL_FEATURE";
            ProductAdditionalFeatureDataSource = dtAdditionalFeature.DefaultView;

            DataTable dtDrivingFeature = bll.GetProductDrivingFeatureByPrimaryKey(paramEntity, productCategory).ToDataTable<FASTENERS_MASTER>();
            if (dtDrivingFeature.Columns.Contains("SUBTYPE"))
                dtDrivingFeature.Columns["SUBTYPE"].ColumnName = "PRD_GRP_CD";
            ProductDrivingFeatureDataSource = dtDrivingFeature.DefaultView;

            string endform = "END FORM";
            if (MandatoryFields != null && MandatoryFields.EndForm == "Feature-II :") endform = "FEATURE - 2";

            DataTable dtEndForm = bll.GetProductEndFormByPrimaryKey(paramEntity, productCategory, endform).ToDataTable<FASTENERS_MASTER>();
            if (dtEndForm.Columns.Contains("SUBTYPE"))
                dtEndForm.Columns["SUBTYPE"].ColumnName = "PRD_CLASS_CD";
            ProductEndFormDataSource = dtEndForm.DefaultView;

            string headform = "HEAD FORMS";
            if (MandatoryFields != null && MandatoryFields.HeadForm == "Bearing Face :") headform = "BEARING FACE";

            DataTable dtHeadForm = bll.GetProductHeadFormByPrimaryKey(paramEntity, productCategory, headform).ToDataTable<FASTENERS_MASTER>();
            if (dtHeadForm.Columns.Contains("SUBTYPE"))
                dtHeadForm.Columns["SUBTYPE"].ColumnName = "HEAD_STYLE";
            ProductHeadFormDataSource = dtHeadForm.DefaultView;

            string shankform = "SHANK FORM";
            if (MandatoryFields != null && MandatoryFields.ShankForm == "Feature-I :") shankform = "FEATURE - 1";
            DataTable dtShankForm = bll.GetProductShankFormByPrimaryKey(paramEntity, productCategory, shankform).ToDataTable<FASTENERS_MASTER>();
            if (dtShankForm.Columns.Contains("SUBTYPE"))
                dtShankForm.Columns["SUBTYPE"].ColumnName = "APPLICATION";
            ProductShankFormDataSource = dtShankForm.DefaultView;

            DataTable dtType = bll.GetProductTypeByPrimaryKey(paramEntity, productCategory).ToDataTable<FASTENERS_MASTER>();
            if (dtType.Columns.Contains("TYPE"))
                dtType.Columns["TYPE"].ColumnName = "TYPE_RENAMED";

            if (dtType.Columns.Contains("SUBTYPE"))
                dtType.Columns["SUBTYPE"].ColumnName = "TYPE";
            ProductTypeDataSource = dtType.DefaultView;

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

        private string _title = "Product Master";
        private string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private int _entityPrimaryKey = 0;
        private int EntityPrimaryKey
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
                switch (_actionMode)
                {
                    case OperationMode.AddNew:
                        PartNumberConfigHasDropDownVisibility = Visibility.Visible;
                        PartNumberHasDropDownVisibility = Visibility.Collapsed;
                        AddModeVisibility = Visibility.Visible;
                        EditModeVisibility = Visibility.Collapsed;

                        ActiveEntity = new PRD_MAST();
                        MandatoryFields = new ProductInformationModel();
                        ActiveMFMEntity = new MFM_MAST();
                        ActivProcessIssueEntity = new MFM_MAST();
                        ActivProductDrawingIssueEntity = new PRD_DWG_ISSUE();
                        //ActivSequenceDrawingIssueEntity = new PRD_DWG_ISSUE();
                        ActiveCIInfoEntity = new DDCI_INFO();

                        ClearAll();
                        ActiveEntity.IDPK = -99999;

                        PartNumberSelectedRow = null;

                        if (PartNumberConfigDataSource.IsNotNullOrEmpty())
                        {
                            //PartNumberConfigDataSource.RowFilter = "IsDefault = " + true.ToValueAsString();
                            //  PartNumberConfigDataSource.RowFilter = "PART_CONFIG_ID = 'Padi Specials-Bolt'";
                            PartNumberConfigDataSource.RowFilter = "Location_code ='MM' and IsObsolete ='False'";
                            if (PartNumberConfigDataSource.Count > 0)
                            {
                                PartNumberConfigSelectedRow = PartNumberConfigDataSource[0];
                                MandatoryFields.PART_CONFIG_ID = PartNumberConfigSelectedRow["PART_CONFIG_ID"].ToValueAsString();
                                CopyMandatoryFieldsToEntity(PartNumberConfigDataSource, ref _partNumberConfigSelectedRow, "ID", "PART_CONFIG_ID", MandatoryFields, ActiveEntity, true, "IN");
                            }
                            PartNumberConfigDataSource.RowFilter = null;
                            PartNoGeneration(PartNumberConfigDataSource);
                        }

                        //MandatoryFields.DOC_REL_DATE = Convert.ToDateTime(bll.ServerDateTime()).AddDays(3);
                        //ActiveEntity.DOC_REL_DATE = MandatoryFields.DOC_REL_DATE; // Commented by Jeyan

                        //MandatoryFields.MFG_STD = "Metric";
                        CopyMandatoryFieldsToEntity(ManufacturingStandardDataSource, ref _manufacturingStandardSelectedRow, "CODE", "MFG_STD", MandatoryFields, ActiveEntity, true);

                        //MandatoryFields.THREAD_CD = "Threaded";
                        CopyMandatoryFieldsToEntity(ThreadCodeDataSource, ref _threadCodeSelectedRow, "CODE", "THREAD_CD", MandatoryFields, ActiveEntity, true);
                        CIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER_PI>().DefaultView;
                        oldEntityDataTable = (new List<PRD_MAST>() { ActiveEntity }).ToDataTable<PRD_MAST>();
                        oldChildEntityDataTable = (new List<DDCI_INFO>() { ActiveCIInfoEntity }).ToDataTable<DDCI_INFO>();
                        IsReadonly = false;
                        break;
                    case OperationMode.Edit:
                        PartNumberConfigHasDropDownVisibility = Visibility.Visible;
                        PartNumberHasDropDownVisibility = Visibility.Visible;
                        AddModeVisibility = Visibility.Collapsed;
                        EditModeVisibility = Visibility.Visible;

                        List<PRD_MAST> lstEntity = bll.GetEntityByPrimaryKey(new PRD_MAST() { IDPK = EntityPrimaryKey });
                        if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                        {
                            ActiveEntity = lstEntity[0];

                            PartNumberDataSource.RowFilter = "PART_NO='" + lstEntity[0].PART_NO.ToValueAsString().FormatEscapeChars() + "'";
                            if (PartNumberDataSource.Count > 0)
                            {
                                ActiveEntity.PART_NO = PartNumberDataSource[0].Row["PART_NO"].ToValueAsString();
                                if (!MandatoryFields.IsNotNullOrEmpty())
                                    MandatoryFields = new ProductInformationModel();
                                MandatoryFields.PART_NO = ActiveEntity.PART_NO;
                                PartNumberSelectedRow = PartNumberDataSource[0];
                                partNumberChanged();
                            }
                            PartNumberDataSource.RowFilter = null;

                        }
                        else
                        {
                            ActiveEntity = new PRD_MAST();
                            MandatoryFields = new ProductInformationModel();
                            ActiveMFMEntity = new MFM_MAST();
                            ActivProcessIssueEntity = new MFM_MAST();
                            ActivProductDrawingIssueEntity = new PRD_DWG_ISSUE();
                            //ActivSequenceDrawingIssueEntity = new PRD_DWG_ISSUE();
                            ActiveCIInfoEntity = new DDCI_INFO();

                            ClearAll();
                            ActiveEntity.IDPK = EntityPrimaryKey;
                        }
                        //EntityPrimaryKey = -99999;

                        if (!ActiveEntity.PART_CONFIG_ID.IsNotNullOrEmpty() || ActiveEntity.PART_CONFIG_ID == 0)
                        {
                            PartNumberConfigDataSource.RowFilter = null;
                            PartNumberConfigSelectedRow = null;
                        }

                        if (PartNumberDataSource.IsNotNullOrEmpty())
                        {
                            PartNumberDataSource.RowFilter = "IDPK = " + ActiveEntity.IDPK;

                            if (PartNumberDataSource.Count > 0)
                            {
                                PartNumberSelectedRow = PartNumberDataSource[0];
                            }
                            else
                            {
                                PartNumberSelectedRow = null;
                            }
                            PartNumberDataSource.RowFilter = null;
                        }
                        else
                        {
                            PartNumberSelectedRow = null;
                        }
                        oldEntityDataTable = (new List<PRD_MAST>() { ActiveEntity }).ToDataTable<PRD_MAST>();
                        if (ActiveCIInfoEntity.IsNotNullOrEmpty())
                            oldChildEntityDataTable = (new List<DDCI_INFO>() { ActiveCIInfoEntity }).ToDataTable<DDCI_INFO>();
                        IsReadonly = true;
                        break;
                    //case OperationMode.Close: ShowInformationMessage("Access Denied");
                    //    CloseSubmitCommand(); break;
                    default: ; break;
                }
                NotifyPropertyChanged("ActionMode");
            }
        }

        private PRD_MAST _activeEntity = null;
        public PRD_MAST ActiveEntity
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

        private DataView _product_ciinfo = null;
        public DataView Product_CIinfo
        {
            get { return _product_ciinfo; }
            set
            {
                _product_ciinfo = value;
                NotifyPropertyChanged("Product_CIinfo");
            }
        }

        private DataRowView _prd_ciref_selectedrow;
        public DataRowView Prd_CIref_SelectedRow
        {
            get
            {
                return _prd_ciref_selectedrow;
            }
            set
            {
                _prd_ciref_selectedrow = value;
                NotifyPropertyChanged("Prd_CIref_SelectedRow");
            }
        }

        private DataRowView _ciReferenceSelectedRow;
        public DataRowView CIReferenceSelectedRow
        {
            get
            {
                return _ciReferenceSelectedRow;
            }
            set
            {
                _ciReferenceSelectedRow = value;
                NotifyPropertyChanged("CIReferenceSelectedRow");
            }
        }

        private void ChangeRights()
        {
            ActionPermission.ReleaseDocument = true;
            ActionPermission.UpdateOrderProcessing = true;
            ActionPermission.CreateCIReference = true;

            if (ActionMode != OperationMode.AddNew)
            {
                //ActionMode = OperationMode.Edit;
                if (ActionMode != OperationMode.Edit)
                {
                    //ActionMode = OperationMode.View;
                    if (!ActualPermission.View) ActionMode = OperationMode.Close;
                    else
                    {
                        //case OperationMode.View:
                        ActionPermission.AddNew = ActualPermission.AddNew;
                        ActionPermission.Edit = ActualPermission.Edit;
                        ActionPermission.View = false;
                        ActionPermission.Delete = ActualPermission.Delete;
                        ActionPermission.Print = ActualPermission.Print;
                        ActionPermission.Save = false;

                        ActionPermission.SimilarPartNumber = ActualPermission.SimilarPartNumber;
                        ActionPermission.ShowRelated = ActualPermission.ShowRelated;
                    }
                }
                else
                {
                    //case OperationMode.Edit:
                    ActionPermission.AddNew = ActualPermission.AddNew;
                    ActionPermission.Edit = false;
                    ActionPermission.View = ActualPermission.View;
                    ActionPermission.Delete = ActualPermission.Delete;
                    ActionPermission.Save = ActualPermission.AddNew || ActualPermission.Edit;
                    ActionPermission.Print = ActualPermission.Print;

                    ActionPermission.SimilarPartNumber = ActualPermission.SimilarPartNumber;
                    ActionPermission.ShowRelated = ActualPermission.ShowRelated;

                }
            }
            else
            {
                //case OperationMode.AddNew:
                ActionPermission.AddNew = false;

                ActionPermission.Edit = ActualPermission.Edit;
                ActionPermission.View = ActualPermission.View;
                ActionPermission.Delete = ActualPermission.Delete;
                ActionPermission.Save = ActualPermission.AddNew || ActualPermission.Edit;
                ActionPermission.Print = false;

                ActionPermission.SimilarPartNumber = false;
                ActionPermission.ShowRelated = false;

                ActionPermission.ReleaseDocument = true;
                ActionPermission.UpdateOrderProcessing = true;
                ActionPermission.CreateCIReference = true;

            }
            ActionPermission.Close = true;
            NotifyPropertyChanged("ActionPermission");
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

        private RolePermission _actualPermission;
        public RolePermission ActualPermission
        {
            get { return _actualPermission; }
            set
            {
                _actualPermission = value;
                NotifyPropertyChanged("ActualPermission");
            }
        }

        private ProductInformationModel _mandatoryFields = null;
        public ProductInformationModel MandatoryFields
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

        #region Close Button Action
        public Action CloseAction { get; set; }
        private RelayCommand _onCancelCommand;
        private void CloseSubmitCommand()
        {
            try
            {
                if (!ActionPermission.Close) return;

                //if (isChangesMade())
                //{
                //    if (ShowWarningMessage(PDMsg.BeforeCloseForm, MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                //    {
                //        SaveSubmitCommand();
                //        return;
                //    }
                //}

                //else
                //{ 
                isclosed = false;
                if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
                {
                    isclosed = true;
                    CloseAction();
                }
                //}

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        bool isclosed = false;
        public void CloseMethod(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isclosed)
                {
                    isclosed = true;
                    WPF.MDI.ClosingEventArgs closingev;
                    closingev = (WPF.MDI.ClosingEventArgs)e;
                    if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                    {
                        isclosed = false;
                        closingev.Cancel = true;
                        e = closingev;
                    }
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
                if (!isclosed)
                {
                    isclosed = true;
                    if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.No)
                    {
                        isclosed = false;
                        e.Cancel = true;
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

        DataTable oldEntityDataTable = null;
        DataTable oldChildEntityDataTable = null;

        private bool isChangesMade()
        {
            //PRD_MAST
            //PRD_CIREF
            //MFM_MAST

            bool bReturnValue = false;
            try
            {
                ActiveEntity.PART_NO = MandatoryFields.PART_NO;
                ActiveEntity.PART_DESC = MandatoryFields.PART_DESC;
                ActiveEntity.QUALITY = MandatoryFields.QUALITY;
                ActiveEntity.DOC_REL_DATE = MandatoryFields.DOC_REL_DATE;
                ActiveEntity.DIA_CD = MandatoryFields.DIA_CD;
                //ActiveCIInfoEntity.CI_REFERENCE = MandatoryFields.CI_REFERENCE.ToValueAsString();

                DataTable activeEntityDataTable = null;
                DataTable activeChildEntityDataTable = null;
                //DataTable activeStandardEntityDataTable = null;
                bool result = true;
                switch (ActionMode)
                {
                    case OperationMode.AddNew:
                        activeEntityDataTable = (new List<PRD_MAST>() { ActiveEntity }).ToDataTable<PRD_MAST>();
                        activeChildEntityDataTable = (new List<DDCI_INFO>() { ActiveCIInfoEntity }).ToDataTable<DDCI_INFO>();

                        break;
                    case OperationMode.Edit:
                        activeEntityDataTable = bll.GetEntityByPrimaryKey(new PRD_MAST() { IDPK = EntityPrimaryKey }).ToDataTable<PRD_MAST>();

                        if (EntityPrimaryKey == -99999 && activeEntityDataTable.IsNotNullOrEmpty() && activeEntityDataTable.Rows.Count == 0)
                        {

                            if (activeEntityDataTable.Columns.Contains("PRD_DWG_ISSUE")) activeEntityDataTable.Columns.Remove("PRD_DWG_ISSUE");
                            if (activeEntityDataTable.Columns.Contains("ROWID")) activeEntityDataTable.Columns.Remove("ROWID");

                            DataRow dataRow = activeEntityDataTable.Rows.Add();
                            dataRow["IDPK"] = EntityPrimaryKey;
                            dataRow.EndEdit();
                        }

                        if (oldEntityDataTable.IsNotNullOrEmpty())
                        {
                            if (oldEntityDataTable.Columns.Contains("PRD_DWG_ISSUE")) oldEntityDataTable.Columns.Remove("PRD_DWG_ISSUE");
                            if (oldEntityDataTable.Columns.Contains("ROWID")) oldEntityDataTable.Columns.Remove("ROWID");
                        }

                        if (oldChildEntityDataTable.IsNotNullOrEmpty())
                        {
                            if (oldChildEntityDataTable.Columns.Contains("DDCOST_PROCESS_DATA")) oldChildEntityDataTable.Columns.Remove("DDCOST_PROCESS_DATA");
                            if (oldChildEntityDataTable.Columns.Contains("ROWID")) oldChildEntityDataTable.Columns.Remove("ROWID");
                            if (oldChildEntityDataTable.Columns.Contains("DDSHAPE_DETAILS")) oldChildEntityDataTable.Columns.Remove("DDSHAPE_DETAILS");
                            if (oldChildEntityDataTable.Columns.Contains("PRD_CIREF")) oldChildEntityDataTable.Columns.Remove("PRD_CIREF");
                        }

                        break;
                }

                result = activeEntityDataTable.IsEqual(oldEntityDataTable);
                if (result)
                {
                    result = activeChildEntityDataTable.IsEqual(oldChildEntityDataTable);
                }

                //if (result)
                //{
                //    result = activeStandardEntityDataTable.IsEqual(originalStandardEntityDataTable);
                //}

                bReturnValue = !result;
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return bReturnValue;
        }

        public ICommand CloseClickCommand
        {
            get
            {


                if (_onCancelCommand == null)
                {
                    _onCancelCommand = new RelayCommand(param => this.CloseSubmitCommand(), null);
                }
                return _onCancelCommand;
            }
        }
        #endregion

        private readonly ICommand addNewCommand;
        public ICommand AddNewClickCommand { get { return this.addNewCommand; } }
        private void AddNewSubmitCommand()
        {
            DefaultControlFocus = true;
            if (!ActionPermission.AddNew) return;
            Progress.Start();
            ActionMode = OperationMode.AddNew;
            ChangeRights();
            Progress.End();
        }

        private readonly ICommand editCommand;
        public ICommand EditClickCommand { get { return this.editCommand; } }
        private void EditSubmitCommand()
        {
            DefaultControlFocus = true;
            if (!ActionPermission.Edit) return;
            EntityPrimaryKey = -99999;
            ActionMode = OperationMode.Edit;
            ChangeRights();
        }
        private readonly ICommand seqDwgCommand;
        public ICommand SeqDwgCommand { get { return this.seqDwgCommand; } }
        private void SeqSubmitDwgCommand()
        {
            if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part Number"));
                return;
            }

            try
            {
                frmSeqDwgIssue frmseqDwgIssue = new frmSeqDwgIssue(_userInformation, MandatoryFields.PART_NO);
                if (frmseqDwgIssue.Vm.DV_PROD_DWG_ISSUE.Count == 0)
                {
                    ShowInformationMessage("Please enter the Issue details for Sequence drawing");
                    return;
                }
                frmseqDwgIssue.dgvProdDwgMast.Focus();
                frmseqDwgIssue.ShowDialog();
                if (frmseqDwgIssue.IsChanged)
                {
                    MandatoryFields.SEQUENCE_DRAWING_ISSUE_NO = frmseqDwgIssue.IssueNo;
                    MandatoryFields.SEQUENCE_DRAWING_ISSUE_DATE = frmseqDwgIssue.IssueDate;
                    MandatoryFields.SEQUENCE_DRAWING_LOC_CODE = frmseqDwgIssue.LocCode;
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }


        }
        private readonly ICommand deleteCommand;
        public ICommand DeleteClickCommand { get { return this.deleteCommand; } }
        private void DeleteSubmitCommand()
        {
            if (!ActionPermission.Delete) return;
            ActionMode = OperationMode.Delete;
            ChangeRights();
        }

        private readonly ICommand viewCommand;
        public ICommand ViewClickCommand { get { return this.viewCommand; } }
        private void ViewSubmitCommand()
        {
            if (!ActionPermission.View) return;
            ActionMode = OperationMode.View;
            ChangeRights();
        }

        private readonly ICommand printCommand;
        public ICommand PrintClickCommand { get { return this.printCommand; } }
        private void PrintSubmitCommand()
        {
            if (!ActionPermission.Print) return;
            ActionMode = OperationMode.Print;

            //DataSet dsReport = new DataSet("FRCS_REPORT");

            //StringBuilder ParentSql = new StringBuilder("SELECT * FROM DDCI_INFO WHERE IDPK ='" + ActiveEntity.IDPK + "'");
            //StringBuilder ChildSql = new StringBuilder("SELECT * FROM DDCOST_PROCESS_DATA WHERE CI_INFO_FK ='" + ActiveEntity.IDPK + "'");

            //List<StringBuilder> sqlList = new List<StringBuilder>() { ParentSql, ChildSql };

            //dsReport = bll.Dal.GetDataSet(sqlList);
            //dsReport.DataSetName = "FRCS_REPORT";

            //if (!dsReport.IsNotNullOrEmpty() || dsReport.Tables.Count < 2) return;

            //DataTable dtDDCI_INFO = dsReport.Tables[0];


            //DataColumn parentColumn = null;
            //DataColumn childColumn = null;
            //ForeignKeyConstraint foreignKeyConstraint = null;

            //if (dtDDCI_INFO.IsNotNullOrEmpty() && dtDDCI_INFO.Rows.Count > 0)
            //{
            //    dtDDCI_INFO.TableName = "DDCI_INFO";
            //    if (!dtDDCI_INFO.Columns.Contains("CUST_NAME")) dtDDCI_INFO.Columns.Add("CUST_NAME");
            //    if (!dtDDCI_INFO.Columns.Contains("RM_DESC")) dtDDCI_INFO.Columns.Add("RM_DESC");
            //    if (!dtDDCI_INFO.Columns.Contains("RM_RATE")) dtDDCI_INFO.Columns.Add("RM_RATE");
            //    if (!dtDDCI_INFO.Columns.Contains("EXPORT_YES_NO")) dtDDCI_INFO.Columns.Add("EXPORT_YES_NO");
            //    if (!dtDDCI_INFO.Columns.Contains("FINISH_DESC")) dtDDCI_INFO.Columns.Add("FINISH_DESC");
            //    if (!dtDDCI_INFO.Columns.Contains("COST_NOTES_TITLE")) dtDDCI_INFO.Columns.Add("COST_NOTES_TITLE");
            //    if (!dtDDCI_INFO.Columns.Contains("SFL_SHARE_INT")) dtDDCI_INFO.Columns.Add("SFL_SHARE_INT");

            //    DataRow dataRow = dtDDCI_INFO.Rows[0];

            //    dataRow["FEASIBILITY"] = dataRow["FEASIBILITY"].ToValueAsString().Trim();

            //    CustomersDataSource.RowFilter = "CUST_CODE='" + dataRow["CUST_CODE"].ToValueAsString() + "'";
            //    if (CustomersDataSource.Count > 0)
            //    {
            //        dataRow["CUST_NAME"] = dataRow["CUST_CODE"].ToValueAsString() + " - " +
            //                               CustomersDataSource[0]["CUST_NAME"].ToValueAsString();

            //        dataRow["SFL_SHARE"] = dataRow["SFL_SHARE"].ToValueAsString().ToIntValue();
            //        dataRow.EndEdit();
            //    }
            //    CustomersDataSource.RowFilter = null;

            //    RawMaterialsDataSource.RowFilter = "RM_CODE='" + dataRow["SUGGESTED_RM"].ToValueAsString() + "'";
            //    if (RawMaterialsDataSource.Count > 0)
            //    {
            //        dataRow["RM_DESC"] = RawMaterialsDataSource[0]["RM_DESC"].ToValueAsString();

            //        dataRow["RM_RATE"] = "@ " + (dataRow["EXPORT"].ToBooleanAsString() ? RawMaterialsDataSource[0]["EXP_COST"].ToValueAsString() :
            //                             RawMaterialsDataSource[0]["LOC_COST"].ToValueAsString()) + " /Kg";

            //        dataRow.EndEdit();
            //    }
            //    RawMaterialsDataSource.RowFilter = null;

            //    FinishDataSource.RowFilter = "FINISH_CODE='" + dataRow["FINISH_CODE"].ToValueAsString() + "'";
            //    if (FinishDataSource.Count > 0)
            //    {
            //        dataRow["FINISH_DESC"] = dataRow["FINISH_CODE"].ToValueAsString() + " - " +
            //                               FinishDataSource[0]["FINISH_DESC"].ToValueAsString();

            //        dataRow["SFL_SHARE_INT"] = dataRow["SFL_SHARE"].ToValueAsString().ToIntValue();
            //        dataRow.EndEdit();
            //    }
            //    FinishDataSource.RowFilter = null;

            //    if (dtDDCI_INFO.PrimaryKey.IsNotNullOrEmpty() && dtDDCI_INFO.PrimaryKey.Length == 0)
            //    {
            //        DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            //        PrimaryKeyColumns[0] = dtDDCI_INFO.Columns["IDPK"];
            //        parentColumn = PrimaryKeyColumns[0];

            //        dtDDCI_INFO.PrimaryKey = PrimaryKeyColumns;

            //    }

            //    dataRow["EXPORT_YES_NO"] = dataRow["EXPORT"].ToBooleanAsString() ? "YES" : "NO";
            //    dataRow["COST_NOTES_TITLE"] = dataRow["FEASIBILITY"].ToBooleanAsString() ? "Cost Notes" :
            //                                  "Enquiry rejected for the following reasons :";

            //    dataRow["COST_NOTES"] = dataRow["FEASIBILITY"].ToBooleanAsString() ? dataRow["COST_NOTES"].ToValueAsString() :
            //                            dataRow["REJECT_REASON"].ToValueAsString();
            //    dataRow.EndEdit();
            //}

            //DataTable dtDDCOST_PROCESS_DATA = dsReport.Tables[1];
            //if (dtDDCOST_PROCESS_DATA.IsNotNullOrEmpty() && dtDDCOST_PROCESS_DATA.Rows.Count == 0)
            //{
            //    DataRow dataRow = dtDDCOST_PROCESS_DATA.Rows.Add();
            //    dataRow["CI_INFO_FK"] = ActiveEntity.IDPK;
            //    dataRow.EndEdit();
            //}
            //dtDDCOST_PROCESS_DATA.TableName = "DDCOST_PROCESS_DATA";

            //if (!dtDDCOST_PROCESS_DATA.Columns.Contains("SNO_INT")) dtDDCOST_PROCESS_DATA.Columns.Add("SNO_INT");

            //foreach (DataRow dataRow in dtDDCOST_PROCESS_DATA.Rows)
            //{
            //    dataRow["SNO_INT"] = dataRow["SNO"].ToValueAsString().ToIntValue();
            //}

            //if (dtDDCOST_PROCESS_DATA.PrimaryKey.IsNotNullOrEmpty() && dtDDCOST_PROCESS_DATA.PrimaryKey.Length == 0)
            //{
            //    if (dtDDCOST_PROCESS_DATA.Columns.Contains("IDPK")) dtDDCOST_PROCESS_DATA.Columns.Remove("IDPK");

            //    dtDDCOST_PROCESS_DATA.Columns["CI_INFO_FK"].ColumnName = "IDPK";
            //    childColumn = dtDDCOST_PROCESS_DATA.Columns["IDPK"];

            //    foreignKeyConstraint = new ForeignKeyConstraint("CPDForeignKeyConstraint", parentColumn, childColumn);
            //    foreignKeyConstraint.DeleteRule = Rule.SetNull;
            //    foreignKeyConstraint.UpdateRule = Rule.Cascade;
            //    foreignKeyConstraint.AcceptRejectRule = AcceptRejectRule.None;

            //    dsReport.Tables["DDCOST_PROCESS_DATA"].Constraints.Add(foreignKeyConstraint);
            //    dsReport.EnforceConstraints = true;

            //    //string path; 
            //    //path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            //    //dsReport.WriteXmlSchema("E:\\" + dsReport.DataSetName + ".xml");

            //}

            //if (!dsReport.IsNotNullOrEmpty() || !dsReport.Tables.IsNotNullOrEmpty() || dsReport.Tables.Count < 0)
            //{
            //    ShowInformationMessage("No Records to Print");
            //    return;
            //}

            //frmReportViewer reportViewer = new frmReportViewer(dsReport, "FRCS");
            //reportViewer.ShowDialog();
        }

        private readonly ICommand saveCommand;
        public ICommand SaveClickCommand { get { return this.saveCommand; } }
        private void SaveSubmitCommand()
        {
            DefaultControlFocus = true;
            if (SaveAllEntity())
            {
                DataTable dtPartNumberConfig = bll.GetPartNumberConfigByPrimaryKey().ToDataTable<PartNumberConfig>();
                if (dtPartNumberConfig.Columns.Contains("Description"))
                    dtPartNumberConfig.Columns["Description"].ColumnName = "PART_CONFIG_ID";
                PartNumberConfigDataSource = dtPartNumberConfig.DefaultView;

                PartNumberDataSource = bll.GetEntityByPrimaryKey().ToDataTableWithType<PRD_MAST>().DefaultView;
                //PartNumberDataSource = GetProductMasterDetailsByPartNumberDV();
                //ActionMode = OperationMode.AddNew;

                if (ActionMode == OperationMode.AddNew)
                {
                    PRD_MAST prdmast = bll.GetEntityByPartNumber(ActiveEntity.PART_NO);
                    if (prdmast != null) EntityPrimaryKey = prdmast.IDPK;
                    ActionMode = OperationMode.Edit;
                    ActiveEntity.ALLOTTED_BY = _userInformation.UserName;
                }

                ChangeRights();
            }
        }

        private bool SaveAllEntity(bool promptSavedMessage = true)
        {
            bool bReturnValue = false;
            if (!ActionPermission.Save) return bReturnValue;

            CopyMandatoryFieldsToEntity(PartNumberConfigDataSource, ref _partNumberConfigSelectedRow, "ID", "PART_CONFIG_ID", MandatoryFields, ActiveEntity, true, "IN");
            CopyMandatoryFieldsToEntity(PartNumberDataSource, ref _partNumberSelectedRow, "PART_NO", "PART_DESC", MandatoryFields, ActiveEntity);
            CopyMandatoryFieldsToEntity(ManufacturingStandardDataSource, ref _manufacturingStandardSelectedRow, "CODE", "MFG_STD", MandatoryFields, ActiveEntity);
            CopyMandatoryFieldsToEntity(ThreadCodeDataSource, ref _threadCodeSelectedRow, "CODE", "THREAD_CD", MandatoryFields, ActiveEntity, true);
            CopyMandatoryFieldsToEntity(SimilarityDataSource, ref _similaritySelectedRow, "STS_CD", "SIM_TO_STD_CD", MandatoryFields, ActiveEntity);
            CopyMandatoryFieldsToEntity(ForecastLocationDataSource, ref _forecastLocationSelectedRow, "LOC_CODE", "BIF_FORECAST", MandatoryFields, ActiveEntity, true);
            CopyMandatoryFieldsToEntity(CurrentLocation1DataSource, ref _currentLocation1SelectedRow, "LOC_CODE", "BIF_PROJ", MandatoryFields, ActiveEntity, true);
            //new
            //CopyMandatoryFieldsToEntity(CurrentLocation1DataSource, ref _currentLocation1SelectedRow, "LOC_CODE", "LOCATION", MandatoryFields, ActiveEntity, true);
            //original
            CopyMandatoryFieldsToEntity(CurrentLocation2DataSource, ref _currentLocation2SelectedRow, "LOC_CODE", "LOCATION", MandatoryFields, ActiveEntity, true);
            //new
            //CopyMandatoryFieldsToEntity(CurrentLocation2DataSource, ref _currentLocation1SelectedRow, "LOC_CODE", "BIF_PROJ", MandatoryFields, ActiveEntity, true);
            //end
            CurrentLocation2DataSource.RowFilter = "LOCATION='" + MandatoryFields.LOCATION.ToValueAsString().Trim().FormatEscapeChars() + "'";
            //new by me
            //CurrentLocation2DataSource.RowFilter = "LOCATION='" + MandatoryFields.CurrentLocation2Code.ToValueAsString().Trim().FormatEscapeChars() + "'";
            if (CurrentLocation2DataSource.Count > 0)
            {
                //ActiveEntity.LOC_CODE = CurrentLocation2DataSource[0]["LOC_CODE"].ToValueAsString();
                ActiveEntity.LOC_CODE = _currentLocation2SelectedRow.Row.ItemArray[0].ToValueAsString();
            }
            CurrentLocation2DataSource.RowFilter = null;

            CopyMandatoryFieldsToEntity(HeatTreatmentDataSource, ref _heatTreatmentSelectedRow, "HT_CD", "HEAT_TREATMENT_CD", MandatoryFields, ActiveEntity, true);

            ActiveEntity.HEAT_TREATMENT_DESC = MandatoryFields.HEAT_TREATMENT_CD;
            if (_heatTreatmentSelectedRow.IsNotNullOrEmpty())
                ActiveEntity.HEAT_TREATMENT_CD = _heatTreatmentSelectedRow["HT_CD"].ToValueAsString();

            if (ActiveEntity.HEAT_TREATMENT_DESC.IsNotNullOrEmpty() && ActiveEntity.HEAT_TREATMENT_DESC.ToValueAsString().Length > 10)
                ActiveEntity.HEAT_TREATMENT_DESC = ActiveEntity.HEAT_TREATMENT_DESC.Substring(0, 10);

            CopyMandatoryFieldsToEntity(PSWApprovedDataSource, ref _pswApprovedSelectedRow, "CODE", "PSW_ST", MandatoryFields, ActiveEntity, true);

            CopyMandatoryFieldsToEntity(ProductPGCategoryDataSource, ref _productPGCategorySelectedRow, "PG_CAT", "PG_CATEGORY", MandatoryFields, ActiveEntity);
            CopyMandatoryFieldsToEntity(ProductKeywordsDataSource, ref _productKeywordsSelectedRow, "PRD_CODE", "KEYWORDS", MandatoryFields, ActiveEntity);
            CopyMandatoryFieldsToEntity(ProductTypeDataSource, ref _productTypeSelectedRow, "PRD_CODE", "TYPE", MandatoryFields, ActiveEntity);
            CopyMandatoryFieldsToEntity(ProductShankFormDataSource, ref _productShankFormSelectedRow, "PRD_CODE", "APPLICATION", MandatoryFields, ActiveEntity);
            CopyMandatoryFieldsToEntity(ProductHeadFormDataSource, ref _productHeadFormSelectedRow, "PRD_CODE", "HEAD_STYLE", MandatoryFields, ActiveEntity);
            CopyMandatoryFieldsToEntity(ProductEndFormDataSource, ref _productEndFormSelectedRow, "PRD_CODE", "PRD_CLASS_CD", MandatoryFields, ActiveEntity);
            CopyMandatoryFieldsToEntity(ProductDrivingFeatureDataSource, ref _productDrivingFeatureSelectedRow, "PRD_CODE", "PRD_GRP_CD", MandatoryFields, ActiveEntity);
            CopyMandatoryFieldsToEntity(ProductAdditionalFeatureDataSource, ref _productAdditionalFeatureSelectedRow, "PRD_CODE", "ADDL_FEATURE", MandatoryFields, ActiveEntity);

            if (Product_CIinfo.Count > 1 && Product_CIinfo[0]["CI_REF"].IsNotNullOrEmpty())
            {
                DataView dv = Product_CIinfo.ToTable().DefaultView;
                dv.RowFilter = "Convert(CURRENT_CIREF, 'System.String') = 'True'";

                if (dv.Count == 0)
                {
                    ShowInformationMessage("Please select a CI reference for the part no.");
                    return bReturnValue;
                }

                if (dv.Count > 1)
                {
                    ShowInformationMessage("Please select any one CI reference");
                    return bReturnValue;
                }
            }


            CopyMandatoryFieldsToEntity(CustomersDataSource, ref _customerSelectedRow, "CUST_CODE", "CUST_NAME", MandatoryFields, ActiveEntity, true, "IN");

            ActiveEntity.PART_NO = MandatoryFields.PART_NO;
            ActiveEntity.PART_DESC = MandatoryFields.PART_DESC;
            ActiveEntity.QUALITY = MandatoryFields.QUALITY;
            ActiveEntity.DOC_REL_DATE = MandatoryFields.DOC_REL_DATE;
            ActiveEntity.DIA_CD = MandatoryFields.DIA_CD;
            ActiveEntity.ALLOTTED_BY = _userInformation.UserName;

            //ActiveCIInfoEntity.CI_REFERENCE = MandatoryFields.CI_REFERENCE.ToValueAsString();

            if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part Number"));
                return bReturnValue;
            }

            //original
            //if (MandatoryFields.PART_NO.Trim().Length != 6 && MandatoryFields.PART_NO.Trim().Length != 9)
            //{
            //    ShowInformationMessage("Please enter 6 or 9 length Part Number");
            //    return bReturnValue;
            //}
            //end

            //new
            if (MandatoryFields.PART_NO.Trim().Length != 6 && MandatoryFields.PART_NO.Trim().Length != 9 && MandatoryFields.PART_NO.Trim().Length != 11)
            {
                ShowInformationMessage("Please enter 6 or 9 or 11 length Part Number");
                return bReturnValue;
            }
            //new end

            if (!MandatoryFields.PART_DESC.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part Description"));
                return bReturnValue;
            }


            foreach (DataRowView drv in Product_CIinfo)
            {
                if (drv["CI_REF"].IsNotNullOrEmpty())
                {
                    ActiveCIInfoEntity.CI_REFERENCE = drv["CI_REF"].ToString();
                    ///Checking for the Existence of releated CIRefeence for this part number
                    List<PRD_CIREF> prd_ciref = null;
                    prd_ciref = bll.GetCIRefernce(new DDCI_INFO() { CI_REFERENCE = ActiveCIInfoEntity.CI_REFERENCE.ToValueAsString() });

                    if (prd_ciref.IsNotNullOrEmpty() && prd_ciref.Count > 0 && ActiveEntity.PART_NO.ToValueAsString().Trim().ToUpper() != prd_ciref[0].PART_NO.ToValueAsString().Trim().ToUpper())
                    {
                        ShowInformationMessage("CI Reference number '" + prd_ciref[0].CI_REF + "' has been allotted to part number '" + prd_ciref[0].PART_NO + "'");
                        return bReturnValue;
                    }

                    FeasibleReportAndCostSheet bllFeasibleReportAndCostSheet = new FeasibleReportAndCostSheet(_userInformation);
                    List<DDCI_INFO> lstCIInfoEntity = bllFeasibleReportAndCostSheet.GetEntitiesByCIReferenceNumber(ActiveCIInfoEntity);

                    if ((!lstCIInfoEntity.IsNotNullOrEmpty() || (lstCIInfoEntity.IsNotNullOrEmpty() && lstCIInfoEntity.Count == 0)) && ActiveCIInfoEntity.CI_REFERENCE.ToValueAsString().IsNotNullOrEmpty())
                    {
                        ShowInformationMessage(PDMsg.DoesNotExists("CI Reference Number"));
                        return bReturnValue;

                        //FeasibleReportAndCostSheet bllFeasibleReportAndCostSheet = new FeasibleReportAndCostSheet(_userInformation);
                        //string outMessage;
                        //if (!bllFeasibleReportAndCostSheet.IsValidCIReferenceNumber(ActiveCIInfoEntity, OperationMode.AddNew, out outMessage) && outMessage.IsNotNullOrEmpty())
                        //{
                        //    ShowInformationMessage(outMessage);
                        //    return bReturnValue;
                        //}
                        //else
                        //{
                        //    ShowInformationMessage(PDMsg.DoesNotExists("CI Reference Number"));
                        //    return bReturnValue;
                        //}
                    }

                    if (ActionMode == OperationMode.AddNew)
                    {
                        ActiveEntity.ALLOTTED_BY = _userInformation.UserName;

                        //Checking for Existence of CIReference in FRCS
                        if (!isDataExists(CIReferenceDataSource.Table.Copy().DefaultView, CIReferenceSelectedRow, ActionMode,
                            "CI_REFERENCE", ActiveCIInfoEntity.CI_REFERENCE, "CI Reference", false, "")) return bReturnValue;
                    }

                    //string message;
                    //if (!bllFRCS.IsValidCIReferenceNumber(ActiveCIInfoEntity, OperationMode.Edit, out message))
                    //{
                    //    ShowInformationMessage(message);
                    //    return bReturnValue;
                    //}
                }
            }

            if (isDataExists(PartNumberDataSource.Table.Copy().DefaultView, PartNumberSelectedRow, ActionMode,
                           "PART_NO", ActiveEntity.PART_NO) && ActionMode == OperationMode.AddNew)
            {
                ShowInformationMessage("Part Number '" + ActiveEntity.PART_NO + "' already exists.");
                return bReturnValue;
            }

            if (!isDataExists(ManufacturingStandardDataSource.Table.Copy().DefaultView, _manufacturingStandardSelectedRow, ActionMode,
                "MFG_STD", MandatoryFields.MFG_STD, "Manufacturing Standard", false, "")) return bReturnValue;

            if (!isDataExists(ThreadCodeDataSource.Table.Copy().DefaultView, _threadCodeSelectedRow, ActionMode,
                "THREAD_CD", MandatoryFields.THREAD_CD, "Thread Code", false, "")) return bReturnValue;

            if (!MandatoryFields.DIA_CD.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Thread Dia Code"));
                return bReturnValue;
            }

            if (!isDataExists(SimilarityDataSource.Table.Copy().DefaultView, _similaritySelectedRow, ActionMode,
                "SIM_TO_STD_CD", MandatoryFields.SIM_TO_STD_CD, "Similarity", false, "")) return bReturnValue;

            if (!isDataExists(ProductFamilyDataSource.Table.Copy().DefaultView, _productFamilySelectedRow, ActionMode,
                "FAMILY", MandatoryFields.FAMILY, "Product Family", false, "")) return bReturnValue;

            if (!isDataExists(ForecastLocationDataSource.Table.Copy().DefaultView, _forecastLocationSelectedRow, ActionMode,
                "BIF_FORECAST", MandatoryFields.BIF_FORECAST, "Forecast Location", false, "")) return bReturnValue;

            if (!isDataExists(CurrentLocation1DataSource.Table.Copy().DefaultView, _currentLocation1SelectedRow, ActionMode,
                "BIF_PROJ", MandatoryFields.BIF_PROJ, "Current Location1", false, "")) return bReturnValue;

            if (!isDataExists(CurrentLocation2DataSource.Table.Copy().DefaultView, _currentLocation2SelectedRow, ActionMode,
                "LOC_CODE", ActiveEntity.LOC_CODE, "Current Location2", true, "")) return bReturnValue;

            if (!MandatoryFields.QUALITY.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Quality"));
                return bReturnValue;
            }

            //if (!MandatoryFields.DOC_REL_DATE.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Planned Document Release Date"));
            //    return bReturnValue;
            //}

            if (ActionMode == OperationMode.Edit && (ActiveEntity.PSW_ST.ToValueAsString().Trim().ToUpper() == "YES" ||
                ActiveEntity.PSW_ST.ToValueAsString().Trim().ToUpper() == "WAIVED") &&
                (ActiveMFMEntity.IsNotNullOrEmpty() && !ActiveMFMEntity.PSW_DATE.IsNotNullOrEmpty()))
            {
                ShowInformationMessage(PDMsg.NormalString("PSW Date should be Entered for PSW Approved option YES or WAIVED"));
                return bReturnValue;
            }

            try
            {

                bool isExecuted = false;
                switch (ActionMode)
                {
                    case OperationMode.AddNew:
                    case OperationMode.Edit:
                        #region Add Operation

                        if (isDataExists(PartNumberDataSource.Table.Copy().DefaultView, PartNumberSelectedRow, ActionMode,
                            "PART_NO", ActiveEntity.PART_NO) && ActionMode == OperationMode.AddNew)
                        {
                            ShowInformationMessage(PDMsg.AlreadyExists("Part Number '" + ActiveEntity.PART_NO + "'"));
                            return bReturnValue;
                        }

                        if (!isDataExists(PartNumberDataSource.Table.Copy().DefaultView, PartNumberSelectedRow, ActionMode,
                            "PART_NO", ActiveEntity.PART_NO) && ActionMode == OperationMode.Edit)
                        {
                            ShowInformationMessage(PDMsg.DoesNotExists("Part Number '" + ActiveEntity.PART_NO + "'"));
                            return bReturnValue;
                        }

                        if (ActionMode == OperationMode.AddNew)
                        {
                            ActiveEntity.ALLOT_DATE = bll.ServerDateTime();
                            ActiveEntity.PSW_ST = "NO";
                        }

                        Progress.ProcessingText = PDMsg.ProgressUpdateText;
                        Progress.Start();

                        isExecuted = bll.Update<PRD_MAST>(new List<PRD_MAST>() { ActiveEntity.DeepCopy<PRD_MAST>() });

                        PRD_MAST parentEntity = bll.GetEntityByPartNumber(ActiveEntity.DeepCopy<PRD_MAST>());

                        if (isExecuted && parentEntity.IsNotNullOrEmpty())
                        {
                            List<PRD_CIREF> prd_ciref = bll.GetCIRefernceByPartNumber(ActiveEntity);
                            bll.DB.PRD_CIREF.DeleteAllOnSubmit(prd_ciref);
                            bll.DB.SubmitChanges();

                            int sno = 1;
                            PRD_CIREF ciref = null;
                            foreach (DataRowView drv in Product_CIinfo)
                            {
                                if (drv["CI_REF"].IsNotNullOrEmpty())
                                {
                                    ciref = new PRD_CIREF();
                                    ciref.PART_NO = ActiveEntity.PART_NO;
                                    ciref.CI_REF = drv["CI_REF"].ToString();
                                    ciref.CIREF_NO_FK = drv["CIREF_NO_FK"].ToString().ToIntValue();
                                    ciref.CURRENT_CIREF = drv["CURRENT_CIREF"].ToBooleanAsString();
                                    ciref.SNO = sno;

                                    isExecuted = bll.Insert<PRD_CIREF>(new List<PRD_CIREF>() { ciref.DeepCopy<PRD_CIREF>() });
                                    sno = sno + 1;
                                }
                            }
                        }

                        if (isExecuted)
                        {
                            isExecuted = UpdateMFMmast();
                        }

                        if (isExecuted && MandatoryFields.SEQUENCE_DRAWING_ISSUE_NO.IsNotNullOrEmpty() && MandatoryFields.SEQUENCE_DRAWING_ISSUE_DATE.IsNotNullOrEmpty())
                        {
                            isExecuted = bll.UpdatePrdDrgIssue(MandatoryFields.PART_NO, MandatoryFields.SEQUENCE_DRAWING_ISSUE_NO.ToDecimalValue(), MandatoryFields.SEQUENCE_DRAWING_LOC_CODE, MandatoryFields.SEQUENCE_DRAWING_ISSUE_DATE);
                        }
                        if (isExecuted)
                        {
                            var a = MandatoryFields.PART_CONFIG_ID;
                            PartNumberConfig partNumberConfig = (from row in bllPartNumberConfiguration.GetEntitiesByID(
                                                                                new PartNumberConfig()
                                                                                {
                                                                                    ID = ActiveEntity.PART_CONFIG_ID.ToValueAsString().ToIntValue()
                                                                                })
                                                                 select row).FirstOrDefault<PartNumberConfig>();

                            if (partNumberConfig.IsNotNullOrEmpty() && PartNumberConfigSelectedRow.IsNotNullOrEmpty())
                            {

                                string prefix = PartNumberConfigSelectedRow["Prefix"].ToValueAsString();
                                string suffix = PartNumberConfigSelectedRow["Suffix"].ToValueAsString();
                                string currentValue = PartNumberConfigSelectedRow["CurrentValue"].ToValueAsString();
                                int beginningNo = PartNumberConfigSelectedRow["BeginningNo"].ToValueAsString().ToIntValue();
                                int endingNo = PartNumberConfigSelectedRow["EndingNo"].ToValueAsString().ToIntValue();
                                if (!suffix.IsNotNullOrEmpty())
                                    suffix = "0";

                                string tmpCurrentValue = ActiveEntity.PART_NO.Replace(prefix, string.Empty);
                                if (currentValue.Length - 1 >= 0)
                                    tmpCurrentValue = tmpCurrentValue.Remove(currentValue.Length - 1);
                                else
                                    tmpCurrentValue = nextCurrentValue.ToValueAsString();

                                if (tmpCurrentValue.IsNumeric())
                                {
                                    partNumberConfig.CurrentValue = Math.Max(tmpCurrentValue.ToValueAsString().ToIntValue(), nextCurrentValue).ToValueAsString();

                                    partNumberConfig.CurrentValue = Math.Max(partNumberConfig.CurrentValue.ToIntValue(), currentValue.ToIntValue()).ToValueAsString();
                                }
                                else
                                {
                                    partNumberConfig.CurrentValue = nextCurrentValue.ToValueAsString();
                                }

                                bllPartNumberConfiguration.Update(new List<PartNumberConfig>() { partNumberConfig });
                            }

                        }
                        Progress.End();
                        if (isExecuted && promptSavedMessage)
                        {
                            ShowInformationMessage(ActionMode == OperationMode.AddNew ? PDMsg.SavedSuccessfully : PDMsg.UpdatedSuccessfully);
                            _logviewBll.SaveLog(MandatoryFields.PART_NO, "ProductMaster");
                            ActiveEntity.DOC_REL_DATE = (DateTime?)null;
                            ActiveEntity.SAMP_SUBMIT_DATE = (DateTime?)null;

                        }
                        bReturnValue = true;
                        #endregion
                        break;
                    case OperationMode.Delete:
                        break;
                }
            }
            catch (Exception ex)
            {
                return bReturnValue;
                throw ex.LogException();
            }

            return bReturnValue;
        }
        private void ClearAll()
        {
            try
            {
                SamplesSubmissionDateReadOnly = false;
                if (MandatoryFields.IsNotNullOrEmpty())
                {
                    InitializeMandatoryFields(MandatoryFields);
                }

                if (ActiveEntity.IsNotNullOrEmpty())
                {
                    ActiveEntity.PART_NO = string.Empty;
                    ActiveEntity.PART_DESC = string.Empty;
                    ActiveEntity.SIM_TO_STD_CD = string.Empty;
                    ActiveEntity.PRD_CLASS_CD = string.Empty;
                    ActiveEntity.MFG_STD = string.Empty;
                    ActiveEntity.ED_CD = string.Empty;
                    ActiveEntity.THREAD_CD = string.Empty;
                    ActiveEntity.DIA_CD = string.Empty;
                    ActiveEntity.QUALITY = string.Empty;
                    ActiveEntity.BIF_PROJ = string.Empty;
                    ActiveEntity.BIF_FORECAST = string.Empty;
                    ActiveEntity.FINISH_WT = null;
                    ActiveEntity.FINISH_WT_EST = null;
                    ActiveEntity.HEAT_TREATMENT_CD = string.Empty;
                    ActiveEntity.HEAT_TREATMENT_DESC = string.Empty;
                    ActiveEntity.PRD_GRP_CD = string.Empty;
                    ActiveEntity.MACHINE_CD = string.Empty;
                    ActiveEntity.ALLOT_DATE = null;
                    ActiveEntity.DOC_REL_DATE = null;
                    ActiveEntity.DOC_REL_REMARKS = string.Empty;
                    ActiveEntity.FAMILY = string.Empty;
                    ActiveEntity.HEAD_STYLE = string.Empty;
                    ActiveEntity.TYPE = string.Empty;
                    ActiveEntity.APPLICATION = string.Empty;
                    ActiveEntity.USER_CD = string.Empty;
                    ActiveEntity.THREAD_CLASS = string.Empty;
                    ActiveEntity.THREAD_STD = string.Empty;
                    ActiveEntity.PG_CATEGORY = string.Empty;
                    ActiveEntity.NOS_PER_PACK = null;
                    ActiveEntity.SAMP_SUBMIT_DATE = null;
                    ActiveEntity.ALLOTTED_BY = string.Empty;
                    ActiveEntity.PSW_ST = string.Empty;
                    ActiveEntity.DOC_REL_TYPE = string.Empty;
                    ActiveEntity.HOLD_ME = null;
                    ActiveEntity.ADDL_FEATURE = string.Empty;
                    ActiveEntity.KEYWORDS = string.Empty;
                    ActiveEntity.CANCEL_STATUS = string.Empty;
                    ActiveEntity.PART_CONFIG_ID = null;
                }
                if (ActiveMFMEntity.IsNotNullOrEmpty())
                {
                    ActiveMFMEntity.PART_NO = string.Empty;
                    ActiveMFMEntity.DOC_REL_DT_PLAN = null;
                    ActiveMFMEntity.DOC_REL_DT_ACTUAL = null;
                    ActiveMFMEntity.TOOLS_READY_DT_PLAN = null;
                    ActiveMFMEntity.TOOLS_READY_ACTUAL_DT = null;
                    ActiveMFMEntity.FORGING_PLAN_DT = null;
                    ActiveMFMEntity.FORGING_ACTUAL_DT = null;
                    ActiveMFMEntity.SECONDARY_PLAN_DT = null;
                    ActiveMFMEntity.SECONDARY_ACTUAL_DT = null;
                    ActiveMFMEntity.HEAT_TREATMENT_PLAN_DT = null;
                    ActiveMFMEntity.HEAT_TREATMENT_ACTUAL = null;
                    ActiveMFMEntity.ISSR_PLAN_DT = null;
                    ActiveMFMEntity.ISSR_ACTUAL_DT = null;
                    ActiveMFMEntity.PPAP_PLAN = null;
                    ActiveMFMEntity.PPAP_ACTUAL_DT = null;
                    ActiveMFMEntity.SAMPLE_QTY = 0;
                    ActiveMFMEntity.REMARKS = string.Empty;
                    ActiveMFMEntity.RESP = string.Empty;
                    ActiveMFMEntity.PSW_DATE = null;
                    ActiveMFMEntity.STATUS = null;
                    ActiveMFMEntity.HOLDME = null;
                    ActiveMFMEntity.TIME_BOGAUGE_PLAN = null;
                    ActiveMFMEntity.TIME_BOGAUGE_ACTUAL = null;
                }
                if (ActivProcessIssueEntity.IsNotNullOrEmpty())
                {
                    ActivProcessIssueEntity.PART_NO = string.Empty;
                    ActivProcessIssueEntity.DOC_REL_DT_PLAN = null;
                    ActivProcessIssueEntity.DOC_REL_DT_ACTUAL = null;
                    ActivProcessIssueEntity.TOOLS_READY_DT_PLAN = null;
                    ActivProcessIssueEntity.TOOLS_READY_ACTUAL_DT = null;
                    ActivProcessIssueEntity.FORGING_PLAN_DT = null;
                    ActivProcessIssueEntity.FORGING_ACTUAL_DT = null;
                    ActivProcessIssueEntity.SECONDARY_PLAN_DT = null;
                    ActivProcessIssueEntity.SECONDARY_ACTUAL_DT = null;
                    ActivProcessIssueEntity.HEAT_TREATMENT_PLAN_DT = null;
                    ActivProcessIssueEntity.HEAT_TREATMENT_ACTUAL = null;
                    ActivProcessIssueEntity.ISSR_PLAN_DT = null;
                    ActivProcessIssueEntity.ISSR_ACTUAL_DT = null;
                    ActivProcessIssueEntity.PPAP_PLAN = null;
                    ActivProcessIssueEntity.PPAP_ACTUAL_DT = null;
                    ActivProcessIssueEntity.SAMPLE_QTY = 0;
                    ActivProcessIssueEntity.REMARKS = string.Empty;
                    ActivProcessIssueEntity.RESP = string.Empty;
                    ActivProcessIssueEntity.PSW_DATE = null;
                    ActivProcessIssueEntity.STATUS = null;
                    ActivProcessIssueEntity.HOLDME = null;
                    ActivProcessIssueEntity.TIME_BOGAUGE_PLAN = null;
                    ActivProcessIssueEntity.TIME_BOGAUGE_ACTUAL = null;

                }
                if (ActivProductDrawingIssueEntity.IsNotNullOrEmpty())
                {
                    ActivProductDrawingIssueEntity.PART_NO = string.Empty;
                    ActivProductDrawingIssueEntity.DWG_TYPE = 0;
                    ActivProductDrawingIssueEntity.ISSUE_NO = 0;
                    ActivProductDrawingIssueEntity.ISSUE_DATE = null;
                    ActivProductDrawingIssueEntity.ISSUE_ALTER = string.Empty;
                    ActivProductDrawingIssueEntity.COMPILED_BY = string.Empty;
                }
                //if (ActivSequenceDrawingIssueEntity.IsNotNullOrEmpty())
                //{
                //    ActivSequenceDrawingIssueEntity.PART_NO = string.Empty;
                //    ActivSequenceDrawingIssueEntity.DWG_TYPE = 0;
                //    ActivSequenceDrawingIssueEntity.ISSUE_NO = 0;
                //    ActivSequenceDrawingIssueEntity.ISSUE_DATE = null;
                //    ActivSequenceDrawingIssueEntity.ISSUE_ALTER = string.Empty;
                //    ActivSequenceDrawingIssueEntity.COMPILED_BY = string.Empty;
                //}

                if (ActiveCIInfoEntity.IsNotNullOrEmpty())
                {
                    ActiveCIInfoEntity.CI_REFERENCE = null;
                    ActiveCIInfoEntity.ENQU_RECD_ON = null;
                    ActiveCIInfoEntity.FR_CS_DATE = null;
                    ActiveCIInfoEntity.PROD_DESC = string.Empty;
                    ActiveCIInfoEntity.CUST_CODE = null;
                    ActiveCIInfoEntity.CUST_DWG_NO = string.Empty;
                    ActiveCIInfoEntity.CUST_DWG_NO_ISSUE = string.Empty;
                    ActiveCIInfoEntity.EXPORT = string.Empty;
                    ActiveCIInfoEntity.NUMBER_OFF = null;
                    ActiveCIInfoEntity.POTENTIAL = null;
                    ActiveCIInfoEntity.SFL_SHARE = null;
                    ActiveCIInfoEntity.REMARKS = string.Empty;
                    ActiveCIInfoEntity.RESPONSIBILITY = null;
                    ActiveCIInfoEntity.PENDING = string.Empty;
                    ActiveCIInfoEntity.FEASIBILITY = string.Empty;
                    ActiveCIInfoEntity.REJECT_REASON = string.Empty;
                    ActiveCIInfoEntity.LOC_CODE = null;
                    ActiveCIInfoEntity.CHEESE_WT = null;
                    ActiveCIInfoEntity.FINISH_WT = null;
                    ActiveCIInfoEntity.FINISH_CODE = null;
                    ActiveCIInfoEntity.COATING_CODE = null;
                    ActiveCIInfoEntity.SUGGESTED_RM = null;
                    ActiveCIInfoEntity.RM_COST = null;
                    ActiveCIInfoEntity.FINAL_COST = null;
                    ActiveCIInfoEntity.COST_NOTES = string.Empty;
                    ActiveCIInfoEntity.PROCESSED_BY = null;
                    ActiveCIInfoEntity.ORDER_DT = null;
                    ActiveCIInfoEntity.PRINT = string.Empty;
                    ActiveCIInfoEntity.ALLOT_PART_NO = null;
                    ActiveCIInfoEntity.PART_NO_REQ_DATE = null;
                    ActiveCIInfoEntity.CUST_STD_NO = string.Empty;
                    ActiveCIInfoEntity.CUST_STD_DATE = null;
                    ActiveCIInfoEntity.AUTOPART = string.Empty;
                    ActiveCIInfoEntity.SAFTYPART = string.Empty;
                    ActiveCIInfoEntity.APPLICATION = string.Empty;
                    ActiveCIInfoEntity.STATUS = null;
                    ActiveCIInfoEntity.CUSTOMER_NEED_DT = null;
                    ActiveCIInfoEntity.MKTG_COMMITED_DT = null;
                    ActiveCIInfoEntity.PPAP_LEVEL = string.Empty;
                    ActiveCIInfoEntity.DEVL_METHOD = null;
                    ActiveCIInfoEntity.PPAP_FORGING = null;
                    ActiveCIInfoEntity.PPAP_SAMPLE = null;
                    ActiveCIInfoEntity.PACKING = null;
                    ActiveCIInfoEntity.NATURE_PACKING = string.Empty;
                    ActiveCIInfoEntity.SPL_CHAR = null;
                    ActiveCIInfoEntity.OTHER_CUST_REQ = string.Empty;
                    ActiveCIInfoEntity.ATP_DATE = null;
                    ActiveCIInfoEntity.SIMILAR_PART_NO = string.Empty;
                    ActiveCIInfoEntity.GENERAL_REMARKS = string.Empty;
                    ActiveCIInfoEntity.MONTHLY = null;
                    ActiveCIInfoEntity.MKTG_COMMITED_DATE = null;
                    ActiveCIInfoEntity.COATING_CODE = string.Empty;
                    ActiveCIInfoEntity.REALISATION = null;
                    ActiveCIInfoEntity.NO_OF_PCS = null;
                    ActiveCIInfoEntity.ZONE_CODE = string.Empty;
                }

                PartNumberConfigDataSource.RowFilter = null;
                PartNumberDataSource.RowFilter = null;
                ManufacturingStandardDataSource.RowFilter = null;
                ThreadCodeDataSource.RowFilter = null;
                SimilarityDataSource.RowFilter = null;
                ProductFamilyDataSource.RowFilter = null;
                ForecastLocationDataSource.RowFilter = null;
                CurrentLocation1DataSource.RowFilter = null;
                CurrentLocation2DataSource.RowFilter = null;
                HeatTreatmentDataSource.RowFilter = null;
                PSWApprovedDataSource.RowFilter = null;
                ProductKeywordsDataSource.RowFilter = null;
                ProductPGCategoryDataSource.RowFilter = null;
                ProductAdditionalFeatureDataSource.RowFilter = null;
                ProductDrivingFeatureDataSource.RowFilter = null;
                ProductEndFormDataSource.RowFilter = null;
                ProductHeadFormDataSource.RowFilter = null;
                ProductShankFormDataSource.RowFilter = null;
                ProductTypeDataSource.RowFilter = null;
                CIReferenceDataSource.RowFilter = null;
                CustomersDataSource.RowFilter = null;

                mdiChild.Title = ApplicationTitle + " - Product Master" + ((ActiveEntity.PART_NO.IsNotNullOrEmpty()) ? " - " + ActiveEntity.PART_NO : "");

                Product_CIinfo = bll.GetCIRefernceByPartNumber(new PRD_MAST { PART_NO = "-1" }).ToDataTableWithType().DefaultView;
                AddNewProductCIRef();

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        #region Part Number Configuration
        private DataView _partNumberConfig = null;
        public DataView PartNumberConfigDataSource
        {
            get
            {
                return _partNumberConfig;
            }
            set
            {
                _partNumberConfig = value;
                NotifyPropertyChanged("PartNumberConfigDataSource");
            }
        }

        private DataRowView _partNumberConfigSelectedRow;
        public DataRowView PartNumberConfigSelectedRow
        {
            get
            {
                return _partNumberConfigSelectedRow;
            }

            set
            {
                _partNumberConfigSelectedRow = value;
            }
        }

        private Visibility _partNumberConfigHasDropDownVisibility = Visibility.Visible;
        public Visibility PartNumberConfigHasDropDownVisibility
        {
            get { return _partNumberConfigHasDropDownVisibility; }
            set
            {
                _partNumberConfigHasDropDownVisibility = value;
                NotifyPropertyChanged("PartNumberConfigHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _partNumberConfigDropDownItems;
        public ObservableCollection<DropdownColumns> PartNumberConfigDropDownItems
        {
            get
            {
                return _partNumberConfigDropDownItems;
            }
            set
            {
                _partNumberConfigDropDownItems = value;
                OnPropertyChanged("PartNumberConfigDropDownItems");
            }
        }

        private readonly ICommand partNumberConfigSelectedItemChangedCommand;
        public ICommand PartNumberConfigSelectedItemChangedCommand { get { return this.partNumberConfigSelectedItemChangedCommand; } }
        private void partNumberConfigChanged()
        {
            ///PartNoGeneration
            //MandatoryFields.DOC_REL_DATE = Convert.ToDateTime(bll.ServerDateTime()).AddDays(3); // Commented by Jeyan

            CopyMandatoryFieldsToEntity(PartNumberConfigDataSource, ref _partNumberConfigSelectedRow, "ID", "PART_CONFIG_ID", MandatoryFields, ActiveEntity, true, "IN");
            PartNoGeneration(PartNumberConfigDataSource);

            string id1 = ActiveEntity.PART_CONFIG_ID.ToValueAsString();

            //isPartNumberConfigSelectionCompleted = false;
            //if (!_partNumberConfigSelectedRow.IsNotNullOrEmpty())
            //{
            //    CostDetailEntities = bll.GetCostDetails(ActiveEntity);
            //    isPartNumberConfigSelectionCompleted = true;
            //    return;
            //}

            //DataTable dt = bll.GetPartNumberConfigNumber(new DDCI_INFO() { IDPK = -99999 }).ToDataTable<V_CI_REFERENCE_NUMBER>().Clone();
            //dt.ImportRow(_partNumberConfigSelectedRow.Row);

            //List<V_CI_REFERENCE_NUMBER> lstEntity = (from row in dt.AsEnumerable()
            //                                         select new V_CI_REFERENCE_NUMBER()
            //                                         {
            //                                             CI_REFERENCE = row.Field<string>("CI_REFERENCE"),
            //                                             FRCS_DATE = row.Field<string>("FRCS_DATE"),
            //                                             CUST_DWG_NO = row.Field<string>("CUST_DWG_NO"),
            //                                             CUST_CODE = row.Field<string>("CUST_CODE").ToDecimalValue(),
            //                                             FINISH_CODE = row.Field<string>("FINISH_CODE"),
            //                                             CUST_DWG_NO_ISSUE = row.Field<string>("CUST_DWG_NO_ISSUE"),
            //                                             CUST_STD_DATE = row.Field<string>("CUST_STD_DATE"),
            //                                             IDPK = row.Field<string>("IDPK").ToIntValue(),
            //                                         }).ToList<V_CI_REFERENCE_NUMBER>();
            //if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
            //{
            //    V_CI_REFERENCE_NUMBER currentEntity = lstEntity[0];
            //    List<DDCI_INFO> lstActiveEntity = bll.GetEntitiesByPrimaryKey(new DDCI_INFO() { IDPK = currentEntity.IDPK });

            //    if (lstActiveEntity.IsNotNullOrEmpty() && lstActiveEntity.Count > 0)
            //    {
            //        ActiveEntity = lstActiveEntity[0].DeepCopy<DDCI_INFO>();

            //        if (MandatoryFields.IsNotNullOrEmpty())
            //        {
            //            MandatoryFields.CI_REFERENCE = ActiveEntity.CI_REFERENCE;
            //            MandatoryFields.CHEESE_WT = ActiveEntity.CHEESE_WT;
            //            MandatoryFields.FINISH_WT = ActiveEntity.FINISH_WT;
            //            MandatoryFields.RM_FACTOR = ActiveEntity.RM_FACTOR;
            //        }

            //        //ActiveEntity.CI_REFERENCE = null;
            //        ActiveEntity.ZONE_CODE = null;
            //        ActiveEntity.CUST_CODE = null;
            //        ActiveEntity.RESPONSIBILITY = null;
            //        ActiveEntity.LOC_CODE = null;
            //        ActiveEntity.FINISH_CODE = null;
            //        ActiveEntity.COATING_CODE = null;
            //        ActiveEntity.SUGGESTED_RM = null;

            //        PartNumberConfigZoneDataSource.RowFilter = "CODE in('" + lstActiveEntity[0].ZONE_CODE.ToValueAsString() + "','"
            //            + (lstActiveEntity[0].CI_REFERENCE.IsNotNullOrEmpty() ? lstActiveEntity[0].CI_REFERENCE.ToValueAsString().Substring(0, 1) : "") + "')";
            //        if (PartNumberConfigZoneDataSource.Count > 0)
            //        {
            //            ActiveEntity.ZONE_CODE = PartNumberConfigZoneDataSource[0].Row["CODE"].ToValueAsString();
            //        }
            //        PartNumberConfigZoneDataSource.RowFilter = null;

            //        //PartNumberConfigDataSource.RowFilter = "CI_REFERENCE='" + lstActiveEntity[0].CI_REFERENCE.ToValueAsString() + "'";
            //        //if (PartNumberConfigDataSource.Count > 0)
            //        //{
            //        //    ActiveEntity.CI_REFERENCE = PartNumberConfigDataSource[0].Row["CI_REFERENCE"].ToValueAsString();
            //        //}
            //        //PartNumberConfigDataSource.RowFilter = null;

            //        CustomerSelectedRow = null;
            //        MandatoryFields.CUST_NAME = string.Empty;
            //        CustomersDataSource.RowFilter = "CUST_CODE='" + lstActiveEntity[0].CUST_CODE.ToValueAsString() + "'";
            //        if (CustomersDataSource.Count > 0)
            //        {
            //            MandatoryFields.CUST_NAME = CustomersDataSource[0]["CUST_NAME"].ToValueAsString();
            //            copyMandatoryFieldsToEntity(MandatoryFields);
            //        }
            //        CustomersDataSource.RowFilter = null;

            //        PreparedByDataSource.RowFilter = "USER_NAME='" + lstActiveEntity[0].RESPONSIBILITY.ToValueAsString() + "'";
            //        if (PreparedByDataSource.Count > 0)
            //        {
            //            ActiveEntity.RESPONSIBILITY = PreparedByDataSource[0].Row["USER_NAME"].ToValueAsString();
            //        }
            //        PreparedByDataSource.RowFilter = null;

            //        PlantDataSource.RowFilter = "LOC_CODE='" + lstActiveEntity[0].LOC_CODE.ToValueAsString() + "'";
            //        if (PlantDataSource.Count > 0)
            //        {
            //            ActiveEntity.LOC_CODE = PlantDataSource[0].Row["LOC_CODE"].ToValueAsString();
            //        }
            //        PlantDataSource.RowFilter = null;

            //        FinishDataSource.RowFilter = "FINISH_CODE='" + lstActiveEntity[0].FINISH_CODE.ToValueAsString() + "'"; ;
            //        if (FinishDataSource.Count > 0)
            //        {
            //            ActiveEntity.FINISH_CODE = FinishDataSource[0].Row["FINISH_CODE"].ToValueAsString();
            //        }
            //        FinishDataSource.RowFilter = null;

            //        TopCoatingDataSource.RowFilter = "COATING_CODE='" + lstActiveEntity[0].COATING_CODE + "'"; ;
            //        if (TopCoatingDataSource.Count > 0)
            //        {
            //            ActiveEntity.COATING_CODE = TopCoatingDataSource[0].Row["COATING_CODE"].ToValueAsString();
            //        }
            //        TopCoatingDataSource.RowFilter = null;

            //        RawMaterialsDataSource.RowFilter = "RM_CODE='" + lstActiveEntity[0].SUGGESTED_RM + "'"; ;
            //        if (RawMaterialsDataSource.Count > 0)
            //        {
            //            ActiveEntity.SUGGESTED_RM = RawMaterialsDataSource[0]["RM_CODE"].ToValueAsString();
            //            RM_DESC = RawMaterialsDataSource[0]["RM_DESC"].ToValueAsString();
            //        }
            //        RawMaterialsDataSource.RowFilter = null;

            //        if (!ActiveEntity.FEASIBILITY.ToBooleanAsString())
            //        {
            //            CostDetailsVisibility = Visibility.Collapsed;
            //            RejectReasonsVisibility = Visibility.Visible;
            //        }
            //        else
            //        {
            //            CostDetailsVisibility = Visibility.Visible;
            //            RejectReasonsVisibility = Visibility.Collapsed;
            //        }

            //    }

            //}

            //CostDetailEntities = bll.GetCostDetails(ActiveEntity);
            //isPartNumberConfigSelectionCompleted = true;
            //copyMandatoryFieldsToEntity(MandatoryFields);

        }

        private readonly ICommand partNumberConfigEndEditCommand;
        public ICommand PartNumberConfigEndEditCommand { get { return this.partNumberConfigEndEditCommand; } }
        private void partNumberConfigEndEdit()
        {
            //partNumberConfigChanged();
            //CopyMandatoryFieldsToEntity(PartNumberConfigDataSource, PartNumberConfigSelectedRow, "ID", "PART_CONFIG_ID", MandatoryFields, ActiveEntity, "IN");

        }

        int nextCurrentValue = 1;
        public bool PartNoGeneration(DataView dataView)
        {
            bool bReturnValue = false;
            try
            {
                if (!ActiveEntity.IsNotNullOrEmpty() || !dataView.IsNotNullOrEmpty()) return bReturnValue;

                if (dataView.Count == 0) return bReturnValue;

                dataView.RowFilter = "CONVERT(Isnull(ID,''), System.String) = '" + ActiveEntity.PART_CONFIG_ID.ToValueAsString().FormatEscapeChars() + "'";
                if (dataView.Count > 0)
                {
                    int partNumberID = dataView[0]["ID"].ToValueAsString().ToIntValue();
                    string partCode = dataView[0]["Code"].ToValueAsString();
                    string partDescription = dataView[0]["PART_CONFIG_ID"].ToValueAsString();

                    string prefix = dataView[0]["Prefix"].ToValueAsString();
                    string suffix = dataView[0]["Suffix"].ToValueAsString();
                    string currentValue = dataView[0]["CurrentValue"].ToValueAsString();
                    int beginningNo = dataView[0]["BeginningNo"].ToValueAsString().ToIntValue();
                    int endingNo = dataView[0]["EndingNo"].ToValueAsString().ToIntValue();
                    int alertMe = dataView[0]["alertMe"].ToValueAsString().ToIntValue();
                    string nextCurrentValuePadding = "0".PadLeft(dataView[0]["BeginningNo"].ToValueAsString().Length, '0');
                    nextCurrentValue = beginningNo;
                    if (currentValue.IsNotNullOrEmpty())
                    {
                        nextCurrentValue = currentValue.ToIntValue() + 1;
                    }
                    if (!nextCurrentValue.IsNotNullOrEmpty() || nextCurrentValue <= 0)
                    {
                        nextCurrentValue = 1;
                    }

                    //since suffix is added in the set up commented the following code
                    if (!suffix.IsNotNullOrEmpty())
                        suffix = "0";
                    ActiveEntity.PART_NO = prefix + nextCurrentValue.ToString(nextCurrentValuePadding) + suffix;
                    while (isDataExists(PartNumberDataSource.Table.Copy().DefaultView, PartNumberSelectedRow, ActionMode,
                           "PART_NO", ActiveEntity.PART_NO) && ActionMode == OperationMode.AddNew)
                    {
                        nextCurrentValue++;
                        ActiveEntity.PART_NO = prefix + nextCurrentValue.ToString(nextCurrentValuePadding) + suffix;
                    }

                    int intCurrentValue = currentValue.ToIntValue();

                    if (intCurrentValue + alertMe >= endingNo && intCurrentValue < endingNo)
                    {
                        ShowInformationMessage("Only '" + (endingNo - intCurrentValue) + "' new part number can be generated automatically.\r\nPlease configure new range using Part Number Configuration");
                    }
                    else if (intCurrentValue + alertMe >= endingNo && intCurrentValue >= endingNo)
                    {
                        if (ShowWarningMessage("The new part number generation for the '- " + partDescription + "' has been reached to the maximum range.\r\n\r\nDo you want to configure new part number now?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {

                            Window win = new Window();
                            win.Title = ApplicationTitle + " - Part Number Configuration";

                            System.Windows.Media.Imaging.IconBitmapDecoder ibd = new System.Windows.Media.Imaging.IconBitmapDecoder(new Uri(@"pack://application:,,/Images/logo.ico", UriKind.RelativeOrAbsolute), System.Windows.Media.Imaging.BitmapCreateOptions.None, System.Windows.Media.Imaging.BitmapCacheOption.Default);

                            win.Icon = ibd.Frames[0];
                            win.ResizeMode = ResizeMode.NoResize;
                            ProcessDesigner.frmPartNumberConfig userControl = new ProcessDesigner.frmPartNumberConfig(_userInformation, win, -999999, OperationMode.AddNew, win.Title);
                            win.Content = userControl;
                            win.Height = userControl.Height + 50;
                            win.Width = userControl.Width + 10;
                            win.ShowInTaskbar = false;
                            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            win.ShowDialog();

                            DataTable dtPartNumberConfig = bll.GetPartNumberConfigByPrimaryKey().ToDataTableWithType<PartNumberConfig>();
                            if (dtPartNumberConfig.Columns.Contains("Description"))
                                dtPartNumberConfig.Columns["Description"].ColumnName = "PART_CONFIG_ID";
                            PartNumberConfigDataSource = dtPartNumberConfig.DefaultView;

                        }
                        else
                        {
                            ActiveEntity.PART_NO = prefix + endingNo.ToString(nextCurrentValuePadding) + suffix;
                        }
                    }
                    //NotifyPropertyChanged("ActiveEntity");

                }

                MandatoryFields.PART_NO = ActiveEntity.PART_NO;
                dataView.RowFilter = null;

                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return bReturnValue;
        }

        #endregion
        private Visibility _addModeVisibility = Visibility.Visible;
        public Visibility AddModeVisibility
        {
            get
            {
                return _addModeVisibility;
            }
            set
            {
                _addModeVisibility = value;
                if (_addModeVisibility == Visibility.Visible)
                {
                    EditModeVisibility = Visibility.Collapsed;
                }
                else if (_addModeVisibility != Visibility.Visible)
                {
                    EditModeVisibility = Visibility.Visible;
                }
                NotifyPropertyChanged("AddModeVisibility");
            }
        }

        private Visibility _editModeVisibility = Visibility.Collapsed;
        public Visibility EditModeVisibility
        {
            get
            {
                return _editModeVisibility;
            }
            set
            {
                _editModeVisibility = value;
                NotifyPropertyChanged("EditModeVisibility");
            }
        }



        #region Part Number
        private DataView _partNumber = null;
        public DataView PartNumberDataSource
        {
            get
            {
                return _partNumber;
            }
            set
            {
                _partNumber = value;
                NotifyPropertyChanged("PartNumberDataSource");
            }
        }

        private DataRowView _partNumberSelectedRow;
        public DataRowView PartNumberSelectedRow
        {
            get
            {
                return _partNumberSelectedRow;
            }

            set
            {
                _partNumberSelectedRow = value;
            }
        }

        private Visibility _partNumberHasDropDownVisibility = Visibility.Visible;
        public Visibility PartNumberHasDropDownVisibility
        {
            get { return _partNumberHasDropDownVisibility; }
            set
            {
                _partNumberHasDropDownVisibility = value;
                NotifyPropertyChanged("PartNumberHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _partNumberDropDownItems;
        public ObservableCollection<DropdownColumns> PartNumberDropDownItems
        {
            get
            {
                return _partNumberDropDownItems;
            }
            set
            {
                _partNumberDropDownItems = value;
                OnPropertyChanged("PartNumberDropDownItems");
            }
        }

        private readonly ICommand partNumberSelectedItemChangedCommand;
        public ICommand PartNumberSelectedItemChangedCommand { get { return this.partNumberSelectedItemChangedCommand; } }
        private void partNumberChanged()
        {
            if (!_partNumberSelectedRow.IsNotNullOrEmpty() || !_partNumberSelectedRow.Row.IsNotNullOrEmpty()) return;

            try
            {

                Progress.Start();

                CIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER_PI>().DefaultView;

                DataTable dt = bll.GetEntityByPrimaryKey(new PRD_MAST() { IDPK = -99999 }).ToDataTableWithType<PRD_MAST>().Clone();
                dt.ImportRow(_partNumberSelectedRow.Row);

                List<PRD_MAST> lstEntity = (from row in dt.AsEnumerable()
                                            select new PRD_MAST()
                                            {
                                                PART_NO = row.Field<string>("PART_NO"),
                                                PART_DESC = row.Field<string>("PART_DESC"),
                                                SIM_TO_STD_CD = row.Field<string>("SIM_TO_STD_CD"),
                                                PRD_CLASS_CD = row.Field<string>("PRD_CLASS_CD"),
                                                MFG_STD = row.Field<string>("MFG_STD"),
                                                ED_CD = row.Field<string>("ED_CD"),
                                                THREAD_CD = row.Field<string>("THREAD_CD"),
                                                DIA_CD = row.Field<string>("DIA_CD"),
                                                QUALITY = row.Field<string>("QUALITY"),
                                                BIF_PROJ = row.Field<string>("BIF_PROJ"),
                                                BIF_FORECAST = row.Field<string>("BIF_FORECAST"),
                                                LOC_CODE = row.Field<string>("LOC_CODE"),
                                                FINISH_WT = row.Field<decimal?>("FINISH_WT"),
                                                FINISH_WT_EST = row.Field<decimal?>("FINISH_WT_EST"),
                                                HEAT_TREATMENT_CD = row.Field<string>("HEAT_TREATMENT_CD"),
                                                HEAT_TREATMENT_DESC = row.Field<string>("HEAT_TREATMENT_DESC"),
                                                PRD_GRP_CD = row.Field<string>("PRD_GRP_CD"),
                                                MACHINE_CD = row.Field<string>("MACHINE_CD"),
                                                ALLOT_DATE = row.Field<DateTime?>("ALLOT_DATE"),
                                                DOC_REL_DATE = row.Field<DateTime?>("DOC_REL_DATE"),
                                                DOC_REL_REMARKS = row.Field<string>("DOC_REL_REMARKS"),
                                                FAMILY = row.Field<string>("FAMILY"),
                                                HEAD_STYLE = row.Field<string>("HEAD_STYLE"),
                                                TYPE = row.Field<string>("TYPE"),
                                                APPLICATION = row.Field<string>("APPLICATION"),
                                                USER_CD = row.Field<string>("USER_CD"),
                                                THREAD_CLASS = row.Field<string>("THREAD_CLASS"),
                                                THREAD_STD = row.Field<string>("THREAD_STD"),
                                                PG_CATEGORY = row.Field<string>("PG_CATEGORY"),
                                                NOS_PER_PACK = row.Field<decimal?>("NOS_PER_PACK"),
                                                SAMP_SUBMIT_DATE = row.Field<DateTime?>("SAMP_SUBMIT_DATE"),
                                                ALLOTTED_BY = row.Field<string>("ALLOTTED_BY"),
                                                PSW_ST = row.Field<string>("PSW_ST"),
                                                DOC_REL_TYPE = row.Field<string>("DOC_REL_TYPE"),
                                                HOLD_ME = row.Field<decimal?>("HOLD_ME"),
                                                ADDL_FEATURE = row.Field<string>("ADDL_FEATURE"),
                                                KEYWORDS = row.Field<string>("KEYWORDS"),
                                                CANCEL_STATUS = row.Field<string>("CANCEL_STATUS"),
                                                IDPK = row.Field<int>("IDPK"),
                                                PART_CONFIG_ID = row.Field<decimal?>("PART_CONFIG_ID"),
                                            }).ToList<PRD_MAST>();
                if (lstEntity.IsNotNullOrEmpty() && lstEntity.Count > 0)
                {
                    List<DEV_REPORT_SUB> lstDevelopmentSubReport = bll.GetDevelopmentSubReportByPartNumber(lstEntity[0]);

                    if (!lstDevelopmentSubReport.IsNotNullOrEmpty() || lstDevelopmentSubReport.Count == 0) //added !
                    {
                        //ShowInformationMessage("Sample Submission Report can be entered only after entering Development Report.");
                        lstEntity[0].SAMP_SUBMIT_DATE = null;
                    }

                    ActiveEntity = lstEntity[0];
                    MandatoryFields.PART_DESC = ActiveEntity.PART_DESC;
                    MandatoryFields.DIA_CD = ActiveEntity.DIA_CD;

                    MandatoryFields.QUALITY = ActiveEntity.QUALITY;
                    MandatoryFields.DOC_REL_DATE = ActiveEntity.DOC_REL_DATE;

                    MandatoryFields.SEQUENCE_DRAWING_ISSUE_NO = null;
                    MandatoryFields.SEQUENCE_DRAWING_ISSUE_DATE = null;
                    MandatoryFields.SEQUENCE_DRAWING_LOC_CODE = null;


                    List<PRD_DWG_ISSUE> lstSequenceDrawingIssueEntities = bll.GetDrawingIssueDetailsByPartNumber(new PRD_DWG_ISSUE()
                    {
                        PART_NO = ActiveEntity.PART_NO,
                        DWG_TYPE = 0
                    });

                    if (lstSequenceDrawingIssueEntities.IsNotNullOrEmpty() && lstSequenceDrawingIssueEntities.Count > 0)
                    {
                        MandatoryFields.PRODUCT_DRAWING_ISSUE_NO = lstSequenceDrawingIssueEntities[0].ISSUE_NO.ToValueAsString();
                        MandatoryFields.PRODUCT_DRAWING_ISSUE_DATE = lstSequenceDrawingIssueEntities[0].ISSUE_DATE;

                    }

                    //lstSequenceDrawingIssueEntities = bll.GetDrawingIssueDetailsByPartNumber(new PRD_DWG_ISSUE()
                    //{
                    //    PART_NO = ActiveEntity.PART_NO,
                    //    DWG_TYPE = 1
                    //});
                    lstSequenceDrawingIssueEntities = bll.GetDrawingIssueDetailsByPartNumberIsActive(new PRD_DWG_ISSUE()
                    {
                        PART_NO = ActiveEntity.PART_NO,
                        DWG_TYPE = 1
                    });
                    if (lstSequenceDrawingIssueEntities.IsNotNullOrEmpty() && lstSequenceDrawingIssueEntities.Count > 0)
                    {
                        MandatoryFields.SEQUENCE_DRAWING_ISSUE_NO = lstSequenceDrawingIssueEntities[0].ISSUE_NO.ToValueAsString();
                        MandatoryFields.SEQUENCE_DRAWING_ISSUE_DATE = lstSequenceDrawingIssueEntities[0].ISSUE_DATE;
                        MandatoryFields.SEQUENCE_DRAWING_LOC_CODE = lstSequenceDrawingIssueEntities[0].Loc_Code;
                    }
                    else
                    {
                        lstSequenceDrawingIssueEntities = bll.GetDrawingIssueDetailsByPartNumber(new PRD_DWG_ISSUE()
                        {
                            PART_NO = ActiveEntity.PART_NO,
                            DWG_TYPE = 1
                        });
                        if (lstSequenceDrawingIssueEntities.IsNotNullOrEmpty() && lstSequenceDrawingIssueEntities.Count > 0)
                        {
                            MandatoryFields.SEQUENCE_DRAWING_ISSUE_NO = lstSequenceDrawingIssueEntities[0].ISSUE_NO.ToValueAsString();
                            MandatoryFields.SEQUENCE_DRAWING_ISSUE_DATE = lstSequenceDrawingIssueEntities[0].ISSUE_DATE;
                            MandatoryFields.SEQUENCE_DRAWING_LOC_CODE = lstSequenceDrawingIssueEntities[0].Loc_Code;
                        }
                    }

                    if (ActiveMFMEntity.IsNotNullOrEmpty())
                    {
                        PWSDateVisibility = Visibility.Collapsed;
                    }

                    if (!ActiveEntity.PSW_ST.IsNotNullOrEmpty()) ActiveEntity.PSW_ST = "NO";

                    if (ActiveEntity.PSW_ST.ToValueAsString().Trim().ToUpper() != "NO")
                    {
                        PWSDateVisibility = Visibility.Visible;
                    }

                    List<MFM_MAST> lstMFMMAST = bll.GetManufacturingMasterByPartNumber(ActiveEntity);
                    if (lstMFMMAST.IsNotNullOrEmpty() && lstMFMMAST.Count > 0)
                    {
                        ActiveMFMEntity = lstMFMMAST[0];
                    }

                    isValidPSWApprovedStatus("NO");
                    CopyEntityToMandatoryFields(PartNumberConfigDataSource, PartNumberConfigSelectedRow, "ID", "PART_CONFIG_ID", MandatoryFields, ActiveEntity);

                    CopyEntityToMandatoryFields(ManufacturingStandardDataSource, ManufacturingStandardSelectedRow, "CODE", "MFG_STD", MandatoryFields, ActiveEntity);

                    CopyEntityToMandatoryFields(ThreadCodeDataSource, ThreadCodeSelectedRow, "CODE", "THREAD_CD", MandatoryFields, ActiveEntity);
                    CopyEntityToMandatoryFields(SimilarityDataSource, SimilaritySelectedRow, "STS_CD", "SIM_TO_STD_CD", MandatoryFields, ActiveEntity);
                    CopyEntityToMandatoryFields(ProductFamilyDataSource, ProductFamilySelectedRow, "PRD_CODE", "FAMILY", MandatoryFields, ActiveEntity);
                    CopyEntityToMandatoryFields(ForecastLocationDataSource, ForecastLocationSelectedRow, "LOC_CODE", "BIF_FORECAST", MandatoryFields, ActiveEntity);
                    CopyEntityToMandatoryFields(CurrentLocation1DataSource, CurrentLocation1SelectedRow, "LOC_CODE", "BIF_PROJ", MandatoryFields, ActiveEntity);
                    //CopyEntityToMandatoryFields(CurrentLocation2DataSource, CurrentLocation2SelectedRow, "LOC_CODE", "CurrentLocation2Description", MandatoryFields, ActiveEntity);
                    //original
                    CopyEntityToMandatoryFields(CurrentLocation2DataSource, CurrentLocation2SelectedRow, "LOC_CODE", "LOC_CODE", MandatoryFields, ActiveEntity);
                    //end original
                    // new by me
                    //CopyEntityToMandatoryFields(CurrentLocation2DataSource, CurrentLocation2SelectedRow, "LOC_CODE", "LOCATION", MandatoryFields, ActiveEntity);
                    //end new
                    CurrentLocation2DataSource.RowFilter = "LOC_CODE='" + ActiveEntity.LOC_CODE.ToValueAsString().Trim().FormatEscapeChars() + "'";
                    if (CurrentLocation2DataSource.Count > 0)
                    {
                        MandatoryFields.LOCATION = CurrentLocation2DataSource[0]["LOCATION"].ToValueAsString();
                        //new
                        //   MandatoryFields.LOCATION_CODE = CurrentLocation2DataSource[0]["LOC_CODE"].ToValueAsString();
                    }
                    CurrentLocation2DataSource.RowFilter = null;

                    CopyEntityToMandatoryFields(HeatTreatmentDataSource, HeatTreatmentSelectedRow, "HT_CD", "HEAT_TREATMENT_CD", MandatoryFields, ActiveEntity);

                    ActiveEntity.HEAT_TREATMENT_DESC = MandatoryFields.HEAT_TREATMENT_CD;

                    CopyEntityToMandatoryFields(PSWApprovedDataSource, PSWApprovedSelectedRow, "CODE", "PSW_ST", MandatoryFields, ActiveEntity);
                    CopyEntityToMandatoryFields(ProductPGCategoryDataSource, ProductPGCategorySelectedRow, "PG_CAT", "PG_CATEGORY", MandatoryFields, ActiveEntity);
                    CopyEntityToMandatoryFields(ProductKeywordsDataSource, ProductKeywordsSelectedRow, "PRD_CODE", "KEYWORDS", MandatoryFields, ActiveEntity);
                    CopyEntityToMandatoryFields(ProductTypeDataSource, ProductTypeSelectedRow, "PRD_CODE", "TYPE", MandatoryFields, ActiveEntity);
                    CopyEntityToMandatoryFields(ProductShankFormDataSource, ProductShankFormSelectedRow, "PRD_CODE", "APPLICATION", MandatoryFields, ActiveEntity);
                    CopyEntityToMandatoryFields(ProductHeadFormDataSource, ProductHeadFormSelectedRow, "PRD_CODE", "HEAD_STYLE", MandatoryFields, ActiveEntity);
                    CopyEntityToMandatoryFields(ProductEndFormDataSource, ProductEndFormSelectedRow, "PRD_CODE", "PRD_CLASS_CD", MandatoryFields, ActiveEntity);
                    CopyEntityToMandatoryFields(ProductDrivingFeatureDataSource, ProductDrivingFeatureSelectedRow, "PRD_CODE", "PRD_GRP_CD", MandatoryFields, ActiveEntity);
                    CopyEntityToMandatoryFields(ProductAdditionalFeatureDataSource, ProductAdditionalFeatureSelectedRow, "PRD_CODE", "ADDL_FEATURE", MandatoryFields, ActiveEntity);


                    List<PRD_CIREF> prd_ciref = bll.GetCIRefernceByPartNumber(ActiveEntity);
                    Product_CIinfo = prd_ciref.ToDataTableWithType().DefaultView;
                    if (Product_CIinfo.IsNotNullOrEmpty() && Product_CIinfo.Count > 0)
                    {
                        foreach (DataRowView drv in Product_CIinfo)
                        {
                            if (drv["CURRENT_CIREF"].ToBooleanAsString())
                            {
                                Prd_CIref_SelectedRow = drv;
                                break;
                            }
                        }

                        if (Prd_CIref_SelectedRow == null) Prd_CIref_SelectedRow = Product_CIinfo[0];

                        ActiveCIInfoEntity = new DDCI_INFO();
                        ActiveCIInfoEntity.CI_REFERENCE = Prd_CIref_SelectedRow["CI_REF"].ToString();
                        CIReferenceChanged();
                    }
                    else
                    {
                        AddNewProductCIRef();
                        CIReferenceSelectedRow = null;
                        MandatoryFields.CUST_CODE = string.Empty;

                        ActiveCIInfoEntity = new DDCI_INFO();
                        ActiveCIInfoEntity.CI_REFERENCE = string.Empty;
                        ActiveCIInfoEntity.REALISATION = null;
                        MandatoryFields.FINISH_DESC = string.Empty;
                    }

                }
                else
                {
                    ActionMode = OperationMode.AddNew;
                    Progress.End();
                    return;
                }
                switch (ActionMode)
                {
                    case OperationMode.Edit:
                        if (isDataExists(PartNumberDataSource.Table.Copy().DefaultView, PartNumberSelectedRow, OperationMode.Edit, "PART_NO",
                            ActiveEntity.PART_NO, "Part Number"))
                        {
                            string sResponsibility;
                            sResponsibility = bll.GetResponsibility(ActiveEntity);
                            if (sResponsibility.IsNotNullOrEmpty())
                                ActiveEntity.ALLOTTED_BY = sResponsibility;
                        }
                        else
                        {
                            ClearAll();
                        }
                        break;
                }
                Progress.End();

                mdiChild.Title = ApplicationTitle + " - Product Master" + ((ActiveEntity.PART_NO.IsNotNullOrEmpty()) ? " - " + ActiveEntity.PART_NO : "");
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }


        private readonly ICommand partNumberEnterKeyPressedCommand;
        public ICommand PartNumberEnterKeyPressedCommand { get { return this.partNumberEnterKeyPressedCommand; } }
        private void partNumberEnterKeyPressed()
        {
            //CopyMandatoryFieldsToEntity(PartNumberDataSource, ref _partNumberSelectedRow, "PART_NO", "PART_DESC", MandatoryFields, ActiveEntity);
            if (PartNumberSelectedRow.IsNotNullOrEmpty())
            {
                partNumberChanged();
            }
            else
            {
                switch (ActionMode)
                {
                    case OperationMode.Edit:
                        ActionMode = OperationMode.Edit;
                        break;
                }
            }


        }

        #endregion

        private void AddNewProductCIRef()
        {
            if (Product_CIinfo != null && (Product_CIinfo.Count == 0 || !CIREFHaveEmptyRow()))
            {
                DataRowView drv = Product_CIinfo.AddNew();
                drv.BeginEdit();
                drv["CURRENT_CIREF"] = false;
                drv.EndEdit();
            }
        }

        private bool CIREFHaveEmptyRow()
        {

            foreach (DataRowView drv in Product_CIinfo)
            {
                if (!drv["CI_REF"].IsNotNullOrEmpty()) return true;
            }
            return false;
        }

        private bool isDataExists(DataView dataView, DataRowView selectedRow, OperationMode operationMode, string fieldName, string fieldValue,
     string showMessage = "", bool ignoreEmptyChecking = true, string appendMessge = "")
        {
            bool bReturnValue = false;
            try
            {
                if (ignoreEmptyChecking && !fieldValue.IsNotNullOrEmpty()) return !bReturnValue;
                selectedRow = dataView.hasValue(fieldName, fieldValue);
                if (!selectedRow.IsNotNullOrEmpty() && showMessage.ToValueAsString().IsNotNullOrEmpty())
                {
                    if (!ignoreEmptyChecking && !fieldValue.IsNotNullOrEmpty())
                        ShowInformationMessage(PDMsg.NotEmpty(showMessage) + appendMessge);
                    else
                        ShowInformationMessage(PDMsg.DoesNotExists(showMessage + "' " + fieldValue.ToValueAsString().ToValueAsString().Trim() + "'") + appendMessge);
                    return bReturnValue;
                }
                bReturnValue = selectedRow.IsNotNullOrEmpty();
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

            return bReturnValue;
        }

        #region Manufacturing Standard
        private DataView _manufacturingStandard = null;
        public DataView ManufacturingStandardDataSource
        {
            get
            {
                return _manufacturingStandard;
            }
            set
            {
                _manufacturingStandard = value;
                NotifyPropertyChanged("ManufacturingStandardDataSource");
            }
        }

        private DataRowView _manufacturingStandardSelectedRow;
        public DataRowView ManufacturingStandardSelectedRow
        {
            get
            {
                return _manufacturingStandardSelectedRow;
            }

            set
            {
                _manufacturingStandardSelectedRow = value;

            }
        }

        private Visibility _manufacturingStandardHasDropDownVisibility = Visibility.Visible;
        public Visibility ManufacturingStandardHasDropDownVisibility
        {
            get { return _manufacturingStandardHasDropDownVisibility; }
            set
            {
                _manufacturingStandardHasDropDownVisibility = value;
                NotifyPropertyChanged("ManufacturingStandardHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _manufacturingStandardDropDownItems;
        public ObservableCollection<DropdownColumns> ManufacturingStandardDropDownItems
        {
            get
            {
                return _manufacturingStandardDropDownItems;
            }
            set
            {
                _manufacturingStandardDropDownItems = value;
                OnPropertyChanged("ManufacturingStandardDropDownItems");
            }
        }

        private readonly ICommand manufacturingStandardSelectedItemChangedCommand;
        public ICommand ManufacturingStandardSelectedItemChangedCommand { get { return this.manufacturingStandardSelectedItemChangedCommand; } }
        private void manufacturingStandardChanged()
        {
            //AssingFieldValue(ManufacturingStandardDataSource, ManufacturingStandardSelectedRow, "CODE", "MFG_STD", MandatoryFields, ActiveEntity);

            //DataView dataView = ManufacturingStandardDataSource;
            //DataRowView selectedRow = ManufacturingStandardSelectedRow;

            //string codeFieldName = "CODE";
            //string descriptionFieldName = "MFG_STD";
            //string Operator = "LIKE";

            //string sFindableValue = MandatoryFields.GetFieldValue(descriptionFieldName).ToValueAsString();

            //if (!selectedRow.IsNotNullOrEmpty() && sFindableValue.IsNotNullOrEmpty())
            //{
            //    dataView.RowFilter = dataView.GetRowFilter(sFindableValue, Operator);
            //    if (dataView.Count > 0)
            //    {
            //        selectedRow = dataView[0];
            //    }
            //    dataView.RowFilter = null;

            //}

            //DataTable dt = dataView.Table.Clone();
            //dt.ImportRow(selectedRow.Row);
            CopyMandatoryFieldsToEntity(ManufacturingStandardDataSource, ref _manufacturingStandardSelectedRow, "CODE", "MFG_STD", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand manufacturingStandardEndEditCommand;
        public ICommand ManufacturingStandardEndEditCommand { get { return this.manufacturingStandardEndEditCommand; } }
        private void manufacturingStandardEndEdit()
        {
            //manufacturingStandardChanged();
            //CopyMandatoryFieldsToEntity(ManufacturingStandardDataSource, ManufacturingStandardSelectedRow, "CODE", "MFG_STD", MandatoryFields, ActiveEntity);
        }
        #endregion

        private void CopyMandatoryFieldsToEntity1(DataView dataView, ref DataRowView selectedRow, string codeFieldName, string descriptionFieldName,
 ProductInformationModel mandatoryFields, PRD_MAST activeEntity, bool clearRowFiler = false, string operatorName = "LIKE")
        {
            try
            {
                if (!dataView.IsNotNullOrEmpty() || !mandatoryFields.IsNotNullOrEmpty() || !codeFieldName.IsNotNullOrEmpty() ||
                    !descriptionFieldName.IsNotNullOrEmpty()) return;
                string sFindableValue = mandatoryFields.GetFieldValue(descriptionFieldName).ToValueAsString().Trim();

                if (clearRowFiler) dataView.RowFilter = null;
                if (dataView.Count == 0 || !sFindableValue.IsNotNullOrEmpty()) return;

                //dataView.RowFilter = dataView.GetRowFilter(sFindableValue, Operator);
                dataView.RowFilter = codeFieldName + " = '" + sFindableValue + "' or " + descriptionFieldName + " = '" + sFindableValue.Replace("'", "") + "'";
                if (dataView.Count > 0)
                {
                    selectedRow = dataView[0];
                    if (dataView.Table.Columns.Contains(descriptionFieldName))
                        mandatoryFields.SetFieldValue(descriptionFieldName, selectedRow[descriptionFieldName].ToValueAsString());

                    if (dataView.Table.Columns.Contains(codeFieldName))
                        activeEntity.SetFieldValue(descriptionFieldName, selectedRow[codeFieldName].ToValueAsString());
                }

                if (clearRowFiler) dataView.RowFilter = null;
            }
            catch (SyntaxErrorException ex)
            {
                ShowInformationMessage(ex.Message); //throw ex.LogException();
            }
        }

        private void CopyMandatoryFieldsToEntity(DataView dataView, ref DataRowView selectedRow, string codeFieldName, string descriptionFieldName,
         ProductInformationModel mandatoryFields, PRD_MAST activeEntity, bool clearRowFiler = false, string operatorName = "LIKE")
        {
            try
            {
                if (!dataView.IsNotNullOrEmpty() || !mandatoryFields.IsNotNullOrEmpty() || !codeFieldName.IsNotNullOrEmpty() ||
                    !descriptionFieldName.IsNotNullOrEmpty()) return;
                object sFindableValue = mandatoryFields.GetFieldValue(descriptionFieldName).ToValueAsString().Trim();
                Console.WriteLine("FindableValue :" + sFindableValue);
                if (clearRowFiler) dataView.RowFilter = null;
                if (dataView.Count == 0 || !sFindableValue.IsNotNullOrEmpty()) return;

                string codeFilterExtention = "'";
                if (dataView.Table.Columns[codeFieldName].DataType.IsNumericType())
                    codeFilterExtention = "";

                string descriptionFilterExtention = "'";
                if (dataView.Table.Columns[descriptionFieldName].DataType.IsNumericType())
                    descriptionFilterExtention = "";

                string filterExpression = "";
                if (dataView.Table.Columns[codeFieldName].DataType.IsNumericType() && sFindableValue.GetType().IsNumericType())
                    filterExpression += codeFieldName + " = " + codeFilterExtention + sFindableValue + codeFilterExtention;

                if (filterExpression.IsNotNullOrEmpty()) filterExpression += " or ";

                if (dataView.Table.Columns[descriptionFieldName].DataType.IsNumericType() == sFindableValue.GetType().IsNumericType())
                    filterExpression += descriptionFieldName + " = " + descriptionFilterExtention + sFindableValue.ToValueAsString().Replace("'", "") + descriptionFilterExtention + "";

                if (filterExpression.ToValueAsString().IsNotNullOrEmpty())
                    dataView.RowFilter = filterExpression;

                if (dataView.Count > 0)
                {
                    //LOCATION_CODE = selectedRow["LOC_CODE"].ToValueAsString(); 
                    selectedRow = dataView[0];
                    //if (!selectedRow.IsNotNullOrEmpty())
                    //{
                    //    selectedRow = dataView[0];
                    //}
                    if (dataView.Table.Columns.Contains(descriptionFieldName))
                    {
                        dataView.SetFieldValue(descriptionFieldName, selectedRow[descriptionFieldName].ToString());
                        mandatoryFields.SetFieldValue(descriptionFieldName, selectedRow[descriptionFieldName].ToValueAsString());
                        //LOCATION_CODE = selectedRow["LOC_CODE"].ToValueAsString();
                    }
                    if (dataView.Table.Columns.Contains(codeFieldName))
                        activeEntity.SetFieldValue(descriptionFieldName, selectedRow[codeFieldName].ToValueAsString());
                }

                if (clearRowFiler) dataView.RowFilter = null;
            }
            catch (SyntaxErrorException ex)
            {
                ShowInformationMessage(ex.Message); //throw ex.LogException();
            }
        }

        private void CopyEntityToMandatoryFields(DataView dataView, DataRowView selectedRow, string codeFieldName, string descriptionFieldName,
 ProductInformationModel mandatoryFields, object activeEntity, string operatorName = "IN")
        {
            if (!dataView.IsNotNullOrEmpty() || !mandatoryFields.IsNotNullOrEmpty() || !codeFieldName.IsNotNullOrEmpty() ||
                !descriptionFieldName.IsNotNullOrEmpty()) return;
            string sFindableValue = activeEntity.GetFieldValue(descriptionFieldName).ToValueAsString().Trim();

            dataView.RowFilter = null;
            if (dataView.Count == 0 || !sFindableValue.IsNotNullOrEmpty()) return;

            //dataView.RowFilter = dataView.GetRowFilter(sFindableValue, Operator);
            dataView.RowFilter = codeFieldName + " = '" + sFindableValue.Replace("'", "''") + "'";
            if (dataView.Count > 0)
            {
                selectedRow = dataView[0];
                if (dataView.Table.Columns.Contains(descriptionFieldName))
                    mandatoryFields.SetFieldValue(descriptionFieldName, selectedRow[descriptionFieldName].ToValueAsString());

                if (dataView.Table.Columns.Contains(codeFieldName))
                    activeEntity.SetFieldValue(descriptionFieldName, selectedRow[codeFieldName].ToValueAsString());
            }
            else
            {
                dataView.RowFilter = descriptionFieldName + " = '" + sFindableValue.Replace("'", "''") + "'";
                if (dataView.Count == 0)
                {
                    //To Show Existing Records those codes does not matches
                    DataRow row = dataView.Table.Rows.Add();
                    row[codeFieldName] = sFindableValue;
                    row[descriptionFieldName] = sFindableValue;
                    row.AcceptChanges();
                    dataView.Table.AcceptChanges();

                    if (dataView.Table.Columns.Contains(descriptionFieldName))
                        mandatoryFields.SetFieldValue(descriptionFieldName, sFindableValue);

                    if (dataView.Table.Columns.Contains(codeFieldName))
                        activeEntity.SetFieldValue(descriptionFieldName, sFindableValue);
                }
            }
            dataView.RowFilter = null;
        }



        #region Thread Code
        private DataView _threadCode = null;
        public DataView ThreadCodeDataSource
        {
            get
            {
                return _threadCode;
            }
            set
            {
                _threadCode = value;
                NotifyPropertyChanged("ThreadCodeDataSource");
            }
        }

        private DataRowView _threadCodeSelectedRow;
        public DataRowView ThreadCodeSelectedRow
        {
            get
            {
                return _threadCodeSelectedRow;
            }

            set
            {
                _threadCodeSelectedRow = value;
            }
        }

        private Visibility _threadCodeHasDropDownVisibility = Visibility.Visible;
        public Visibility ThreadCodeHasDropDownVisibility
        {
            get { return _threadCodeHasDropDownVisibility; }
            set
            {
                _threadCodeHasDropDownVisibility = value;
                NotifyPropertyChanged("ThreadCodeHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _threadCodeDropDownItems;
        public ObservableCollection<DropdownColumns> ThreadCodeDropDownItems
        {
            get
            {
                return _threadCodeDropDownItems;
            }
            set
            {
                _threadCodeDropDownItems = value;
                OnPropertyChanged("ThreadCodeDropDownItems");
            }
        }

        private readonly ICommand threadCodeSelectedItemChangedCommand;
        public ICommand ThreadCodeSelectedItemChangedCommand { get { return this.threadCodeSelectedItemChangedCommand; } }
        private void threadCodeChanged()
        {
            CopyMandatoryFieldsToEntity(ThreadCodeDataSource, ref _threadCodeSelectedRow, "CODE", "THREAD_CD", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand threadCodeEndEditCommand;
        public ICommand ThreadCodeEndEditCommand { get { return this.threadCodeEndEditCommand; } }
        private void threadCodeEndEdit()
        {
            //threadCodeChanged();
            //CopyMandatoryFieldsToEntity(ThreadCodeDataSource, ThreadCodeSelectedRow, "CODE", "THREAD_CD", MandatoryFields, ActiveEntity);
        }
        #endregion

        #region Similarity
        private DataView _similarity = null;
        public DataView SimilarityDataSource
        {
            get
            {
                return _similarity;
            }
            set
            {
                _similarity = value;
                NotifyPropertyChanged("SimilarityDataSource");
            }
        }

        private DataRowView _similaritySelectedRow;
        public DataRowView SimilaritySelectedRow
        {
            get
            {
                return _similaritySelectedRow;
            }

            set
            {
                _similaritySelectedRow = value;
            }
        }

        private Visibility _similarityHasDropDownVisibility = Visibility.Visible;
        public Visibility SimilarityHasDropDownVisibility
        {
            get { return _similarityHasDropDownVisibility; }
            set
            {
                _similarityHasDropDownVisibility = value;
                NotifyPropertyChanged("SimilarityHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _similarityDropDownItems;
        public ObservableCollection<DropdownColumns> SimilarityDropDownItems
        {
            get
            {
                return _similarityDropDownItems;
            }
            set
            {
                _similarityDropDownItems = value;
                OnPropertyChanged("SimilarityDropDownItems");
            }
        }

        private readonly ICommand similaritySelectedItemChangedCommand;
        public ICommand SimilaritySelectedItemChangedCommand { get { return this.similaritySelectedItemChangedCommand; } }
        private void similarityChanged()
        {
            CopyMandatoryFieldsToEntity(SimilarityDataSource, ref _similaritySelectedRow, "STS_CD", "SIM_TO_STD_CD", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand similarityEndEditCommand;
        public ICommand SimilarityEndEditCommand { get { return this.similarityEndEditCommand; } }
        private void similarityEndEdit()
        {
            //similarityChanged();
            //CopyMandatoryFieldsToEntity(SimilarityDataSource, SimilaritySelectedRow, "STS_CD", "SIM_TO_STD_CD", MandatoryFields, ActiveEntity);
        }
        #endregion

        #region Product Family
        private DataView _productFamily = null;
        public DataView ProductFamilyDataSource
        {
            get
            {
                return _productFamily;
            }
            set
            {
                _productFamily = value;
                NotifyPropertyChanged("ProductFamilyDataSource");
            }
        }

        private DataRowView _productFamilySelectedRow;
        public DataRowView ProductFamilySelectedRow
        {
            get
            {
                return _productFamilySelectedRow;
            }

            set
            {
                _productFamilySelectedRow = value;
            }
        }

        private Visibility _productFamilyHasDropDownVisibility = Visibility.Visible;
        public Visibility ProductFamilyHasDropDownVisibility
        {
            get { return _productFamilyHasDropDownVisibility; }
            set
            {
                _productFamilyHasDropDownVisibility = value;
                NotifyPropertyChanged("ProductFamilyHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _productFamilyDropDownItems;
        public ObservableCollection<DropdownColumns> ProductFamilyDropDownItems
        {
            get
            {
                return _productFamilyDropDownItems;
            }
            set
            {
                _productFamilyDropDownItems = value;
                OnPropertyChanged("ProductFamilyDropDownItems");
            }
        }

        private readonly ICommand productFamilySelectedItemChangedCommand;
        public ICommand ProductFamilySelectedItemChangedCommand { get { return this.productFamilySelectedItemChangedCommand; } }
        private void productFamilyChanged()
        {
            CopyMandatoryFieldsToEntity(ProductFamilyDataSource, ref _productFamilySelectedRow, "PRD_CODE", "FAMILY", MandatoryFields, ActiveEntity, true);

            MandatoryFields.HeadForm = "Head Form :";
            MandatoryFields.ShankForm = "Shank Form :";
            MandatoryFields.EndForm = "End Form :";
            InternalThreadedVisibility = Visibility.Visible;

            MandatoryFields.RowProductGroupCategory = 31;
            MandatoryFields.RowSimilarity = 31;




            switch (MandatoryFields.FAMILY.ToValueAsString().ToUpper())
            {
                case "EXTERNAL THREADED":
                    MandatoryFields.HeadForm = "Head Form :";
                    MandatoryFields.ShankForm = "Shank Form :";
                    MandatoryFields.EndForm = "End Form :";
                    InternalThreadedVisibility = Visibility.Visible;

                    MandatoryFields.RowProductGroupCategory = 31;
                    MandatoryFields.RowSimilarity = 33;

                    break;
                case "INTERNAL THREADED":
                    MandatoryFields.HeadForm = "Bearing Face :";
                    MandatoryFields.ShankForm = "Feature-I :";
                    MandatoryFields.EndForm = "Feature-II :";
                    InternalThreadedVisibility = Visibility.Collapsed;

                    MandatoryFields.RowProductGroupCategory = 25;
                    MandatoryFields.RowSimilarity = 27;

                    break;
                default:
                    InternalThreadedVisibility = Visibility.Visible;

                    MandatoryFields.RowProductGroupCategory = 31;
                    MandatoryFields.RowSimilarity = 33;
                    break;
            }

            loadAllProductFamilyAndSubFamilies(null, MandatoryFields.FAMILY.ToValueAsString().Trim());

            ProductTypeDataSource.RowFilter = null;
            ProductAdditionalFeatureDataSource.RowFilter = null;
            ProductDrivingFeatureDataSource.RowFilter = null;
            ProductEndFormDataSource.RowFilter = null;
            ProductHeadFormDataSource.RowFilter = null;
            ProductKeywordsDataSource.RowFilter = null;
            ProductShankFormDataSource.RowFilter = null;

            MandatoryFields.TYPE = string.Empty;
            MandatoryFields.HEAD_STYLE = string.Empty;
            MandatoryFields.APPLICATION = string.Empty;
            MandatoryFields.PRD_CLASS_CD = string.Empty;
            MandatoryFields.PRD_GRP_CD = string.Empty;
            MandatoryFields.ADDL_FEATURE = string.Empty;
            MandatoryFields.KEYWORDS = string.Empty;


            if (ProductFamilyDataSource.IsNotNullOrEmpty() && ProductFamilyDataSource.Count > 0 && MandatoryFields.FAMILY.IsNotNullOrEmpty())
            {
                FilterProductFamily(ProductTypeDataSource, MandatoryFields.FAMILY.ToValueAsString());
                FilterProductFamily(ProductAdditionalFeatureDataSource, MandatoryFields.FAMILY.ToValueAsString());
                FilterProductFamily(ProductDrivingFeatureDataSource, MandatoryFields.FAMILY.ToValueAsString());
                FilterProductFamily(ProductEndFormDataSource, MandatoryFields.FAMILY.ToValueAsString());
                FilterProductFamily(ProductHeadFormDataSource, MandatoryFields.FAMILY.ToValueAsString());
                FilterProductFamily(ProductKeywordsDataSource, MandatoryFields.FAMILY.ToValueAsString());
                FilterProductFamily(ProductShankFormDataSource, MandatoryFields.FAMILY.ToValueAsString());
            }
            ProductFamilyDataSource.RowFilter = null;
        }

        private void FilterProductFamily(DataView dataView, string fieldValue = "", string fieldName = "PRODUCT_CATEGORY")
        {
            dataView.RowFilter = fieldName + " = '" + fieldValue + "'";
        }

        private Visibility _internalThreadedVisibility = Visibility.Visible;
        public Visibility InternalThreadedVisibility
        {
            get { return _internalThreadedVisibility; }
            set
            {
                _internalThreadedVisibility = value;
                NotifyPropertyChanged("InternalThreadedVisibility");
            }
        }

        private readonly ICommand productFamilyEndEditCommand;
        public ICommand ProductFamilyEndEditCommand { get { return this.productFamilyEndEditCommand; } }
        private void productFamilyEndEdit()
        {
            //productFamilyChanged();
        }
        #endregion

        #region Forecast Location
        private DataView _forecastLocation = null;
        public DataView ForecastLocationDataSource
        {
            get
            {
                return _forecastLocation;
            }
            set
            {
                _forecastLocation = value;
                NotifyPropertyChanged("ForecastLocationDataSource");
            }
        }

        private DataRowView _forecastLocationSelectedRow;
        public DataRowView ForecastLocationSelectedRow
        {
            get
            {
                return _forecastLocationSelectedRow;
            }

            set
            {
                _forecastLocationSelectedRow = value;
            }
        }

        private Visibility _forecastLocationHasDropDownVisibility = Visibility.Visible;
        public Visibility ForecastLocationHasDropDownVisibility
        {
            get { return _forecastLocationHasDropDownVisibility; }
            set
            {
                _forecastLocationHasDropDownVisibility = value;
                NotifyPropertyChanged("ForecastLocationHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _forecastLocationDropDownItems;
        public ObservableCollection<DropdownColumns> ForecastLocationDropDownItems
        {
            get
            {
                return _forecastLocationDropDownItems;
            }
            set
            {
                _forecastLocationDropDownItems = value;
                OnPropertyChanged("ForecastLocationDropDownItems");
            }
        }

        private readonly ICommand forecastLocationSelectedItemChangedCommand;
        public ICommand ForecastLocationSelectedItemChangedCommand { get { return this.forecastLocationSelectedItemChangedCommand; } }
        private void forecastLocationChanged()
        {
            CopyMandatoryFieldsToEntity(ForecastLocationDataSource, ref _forecastLocationSelectedRow, "LOC_CODE", "BIF_FORECAST", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand forecastLocationEndEditCommand;
        public ICommand ForecastLocationEndEditCommand { get { return this.forecastLocationEndEditCommand; } }
        private void forecastLocationEndEdit()
        {
            //forecastLocationChanged();
            //CopyMandatoryFieldsToEntity(ForecastLocationDataSource, ForecastLocationSelectedRow, "LOC_CODE", "BIF_FORECAST", MandatoryFields, ActiveEntity);
        }
        #endregion

        #region Current Location1
        private DataView _currentLocation1 = null;
        public DataView CurrentLocation1DataSource
        {
            get
            {
                return _currentLocation1;
            }
            set
            {
                _currentLocation1 = value;
                NotifyPropertyChanged("CurrentLocation1DataSource");
            }
        }

        private DataRowView _currentLocation1SelectedRow;
        public DataRowView CurrentLocation1SelectedRow
        {
            get
            {
                return _currentLocation1SelectedRow;
            }

            set
            {
                _currentLocation1SelectedRow = value;
            }
        }

        private Visibility _currentLocation1HasDropDownVisibility = Visibility.Visible;
        public Visibility CurrentLocation1HasDropDownVisibility
        {
            get { return _currentLocation1HasDropDownVisibility; }
            set
            {
                _currentLocation1HasDropDownVisibility = value;
                NotifyPropertyChanged("CurrentLocation1HasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _currentLocation1DropDownItems;
        public ObservableCollection<DropdownColumns> CurrentLocation1DropDownItems
        {
            get
            {
                return _currentLocation1DropDownItems;
            }
            set
            {
                _currentLocation1DropDownItems = value;
                OnPropertyChanged("CurrentLocation1DropDownItems");
            }
        }

        private readonly ICommand currentLocation1SelectedItemChangedCommand;
        public ICommand CurrentLocation1SelectedItemChangedCommand { get { return this.currentLocation1SelectedItemChangedCommand; } }
        private void currentLocation1Changed()
        {
            CopyMandatoryFieldsToEntity(CurrentLocation1DataSource, ref _currentLocation1SelectedRow, "LOC_CODE", "BIF_PROJ", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand currentLocation1EndEditCommand;
        public ICommand CurrentLocation1EndEditCommand { get { return this.currentLocation1EndEditCommand; } }
        private void currentLocation1EndEdit()
        {
            //currentLocation1Changed();
            //CopyMandatoryFieldsToEntity(CurrentLocation1DataSource, CurrentLocation1SelectedRow, "LOC_CODE", "BIF_PROJ", MandatoryFields, ActiveEntity);
        }
        #endregion

        #region Current Location2
        private DataView _currentLocation2 = null;
        public DataView CurrentLocation2DataSource
        {
            get
            {
                return _currentLocation2;
            }
            set
            {
                _currentLocation2 = value;
                NotifyPropertyChanged("CurrentLocation2DataSource");
            }
        }

        private DataRowView _currentLocation2SelectedRow;
        public DataRowView CurrentLocation2SelectedRow
        {
            get
            {
                return _currentLocation2SelectedRow;
            }

            set
            {
                _currentLocation2SelectedRow = value;
            }
        }

        private Visibility _currentLocation2HasDropDownVisibility = Visibility.Visible;
        public Visibility CurrentLocation2HasDropDownVisibility
        {
            get { return _currentLocation2HasDropDownVisibility; }
            set
            {
                _currentLocation2HasDropDownVisibility = value;
                NotifyPropertyChanged("CurrentLocation2HasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _currentLocation2DropDownItems;
        public ObservableCollection<DropdownColumns> CurrentLocation2DropDownItems
        {
            get
            {
                return _currentLocation2DropDownItems;
            }
            set
            {
                _currentLocation2DropDownItems = value;
                OnPropertyChanged("CurrentLocation2DropDownItems");
            }
        }

        private readonly ICommand currentLocation2SelectedItemChangedCommand;
        public ICommand CurrentLocation2SelectedItemChangedCommand { get { return this.currentLocation2SelectedItemChangedCommand; } }


        private void currentLocation2Changed()
        {
            if (_currentLocation2SelectedRow != null)
            {
                LOCATION_CODE = _currentLocation2SelectedRow.Row.ItemArray[0].ToValueAsString(); //new by me

                //uncomment by me
                CopyMandatoryFieldsToEntity(CurrentLocation2DataSource, ref _currentLocation2SelectedRow, "LOC_CODE", " ", MandatoryFields, ActiveEntity, true);

                //comment by me
                //CopyMandatoryFieldsToEntity(CurrentLocation2DataSource, ref _currentLocation1SelectedRow, "LOC_CODE", "LOCATION", MandatoryFields, ActiveEntity, true);

                CurrentLocation2DataSource.RowFilter = "LOCATION='" + MandatoryFields.LOCATION.ToValueAsString().Trim().FormatEscapeChars() + "'";
                //CurrentLocation2DataSource.RowFilter = "LOC_CODE='" + LOCATION_CODE.ToValueAsString().Trim().FormatEscapeChars() + "'";

                if (CurrentLocation2DataSource.Count > 0)
                {
                    //ActiveEntity.LOC_CODE = CurrentLocation2DataSource[0]["LOC_CODE"].ToValueAsString();
                    //  new
                    foreach (DataRowView i in CurrentLocation2DataSource)
                    {
                        DataRow r = i.Row;
                        if (i.Row.ItemArray[0] == LOCATION_CODE)
                            ActiveEntity.LOC_CODE = LOCATION_CODE;
                        // end new
                    }
                }
                CurrentLocation2DataSource.RowFilter = null;
            }
            else
            {
                ActiveEntity.LOC_CODE = "";
            }

        }



        private readonly ICommand currentLocation2EndEditCommand;
        public ICommand CurrentLocation2EndEditCommand { get { return this.currentLocation2EndEditCommand; } }
        private void currentLocation2EndEdit()
        {

            //currentLocation2Changed();
            //CopyMandatoryFieldsToEntity(CurrentLocation2DataSource, CurrentLocation2SelectedRow, "LOC_CODE", "CurrentLocation2Description", MandatoryFields, ActiveEntity);

            //CopyMandatoryFieldsToEntity(CurrentLocation2DataSource, CurrentLocation2SelectedRow, "LOC_CODE", "CurrentLocation2Code", MandatoryFields, ActiveEntity);



        }
        #endregion

        #region Heat Treatment
        private DataView _heatTreatment = null;
        public DataView HeatTreatmentDataSource
        {
            get
            {
                return _heatTreatment;
            }
            set
            {
                _heatTreatment = value;
                NotifyPropertyChanged("HeatTreatmentDataSource");
            }
        }

        private DataRowView _heatTreatmentSelectedRow;
        public DataRowView HeatTreatmentSelectedRow
        {
            get
            {
                return _heatTreatmentSelectedRow;
            }

            set
            {
                _heatTreatmentSelectedRow = value;
            }
        }

        private Visibility _heatTreatmentHasDropDownVisibility = Visibility.Visible;
        public Visibility HeatTreatmentHasDropDownVisibility
        {
            get { return _heatTreatmentHasDropDownVisibility; }
            set
            {
                _heatTreatmentHasDropDownVisibility = value;
                NotifyPropertyChanged("HeatTreatmentHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _heatTreatmentDropDownItems;
        public ObservableCollection<DropdownColumns> HeatTreatmentDropDownItems
        {
            get
            {
                return _heatTreatmentDropDownItems;
            }
            set
            {
                _heatTreatmentDropDownItems = value;
                OnPropertyChanged("HeatTreatmentDropDownItems");
            }
        }

        private readonly ICommand heatTreatmentSelectedItemChangedCommand;
        public ICommand HeatTreatmentSelectedItemChangedCommand { get { return this.heatTreatmentSelectedItemChangedCommand; } }
        private void heatTreatmentChanged()
        {
            CopyMandatoryFieldsToEntity(HeatTreatmentDataSource, ref _heatTreatmentSelectedRow, "HT_CD", "HEAT_TREATMENT_CD", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand heatTreatmentEndEditCommand;
        public ICommand HeatTreatmentEndEditCommand { get { return this.heatTreatmentEndEditCommand; } }
        private void heatTreatmentEndEdit()
        {
            //heatTreatmentChanged();
            //CopyMandatoryFieldsToEntity(HeatTreatmentDataSource, HeatTreatmentSelectedRow, "HT_CD", "HEAT_TREATMENT_CD", MandatoryFields, ActiveEntity);
        }
        #endregion

        private readonly ICommand estimatedFinishWeightLostFocusCommand = null;
        public ICommand EstimatedFinishWeightLostFocusCommand { get { return this.estimatedFinishWeightLostFocusCommand; } }
        private void estimatedFinishWeightLostFocus()
        {

        }

        private readonly ICommand finishWeightLostFocusCommand = null;
        public ICommand FinishWeightLostFocusCommand { get { return this.finishWeightLostFocusCommand; } }
        private void finishWeightLostFocus()
        {

        }

        private readonly ICommand nosPerPackLostFocusCommand = null;
        public ICommand NosPerPackLostFocusCommand { get { return this.nosPerPackLostFocusCommand; } }
        private void nosPerPackLostFocus()
        {

        }

        #region PSW Approved
        private DataView _pswApproved = null;
        public DataView PSWApprovedDataSource
        {
            get
            {
                return _pswApproved;
            }
            set
            {
                _pswApproved = value;
                NotifyPropertyChanged("PSWApprovedDataSource");
            }
        }

        private DataRowView _pswApprovedSelectedRow;
        public DataRowView PSWApprovedSelectedRow
        {
            get
            {
                return _pswApprovedSelectedRow;
            }

            set
            {
                _pswApprovedSelectedRow = value;
            }
        }

        private Visibility _pswApprovedHasDropDownVisibility = Visibility.Visible;
        public Visibility PSWApprovedHasDropDownVisibility
        {
            get { return _pswApprovedHasDropDownVisibility; }
            set
            {
                _pswApprovedHasDropDownVisibility = value;
                NotifyPropertyChanged("PSWApprovedHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _pswApprovedDropDownItems;
        public ObservableCollection<DropdownColumns> PSWApprovedDropDownItems
        {
            get
            {
                return _pswApprovedDropDownItems;
            }
            set
            {
                _pswApprovedDropDownItems = value;
                OnPropertyChanged("PSWApprovedDropDownItems");
            }
        }

        private readonly ICommand pswApprovedSelectedItemChangedCommand;
        public ICommand PSWApprovedSelectedItemChangedCommand { get { return this.pswApprovedSelectedItemChangedCommand; } }
        private void pswApprovedChanged(string showMessage)
        {

            CopyMandatoryFieldsToEntity(PSWApprovedDataSource, ref _pswApprovedSelectedRow, "CODE", "PSW_ST", MandatoryFields, ActiveEntity, true);
            isValidPSWApprovedStatus(showMessage);

        }

        private readonly ICommand pswApprovedEndEditCommand;
        public ICommand PSWApprovedEndEditCommand { get { return this.pswApprovedEndEditCommand; } }
        private void pswApprovedEndEdit()
        {
            pswApprovedChanged("NO");
        }
        #endregion

        private bool isValidPSWApprovedStatus(string showMessage)
        {
            bool bReturnValue = false;
            try
            {
                if (!ActiveEntity.IsNotNullOrEmpty()) return true;

                switch (ActiveEntity.PSW_ST.ToValueAsString().ToBooleanAsString())
                {
                    case false:
                        if (ActiveMFMEntity.IsNotNullOrEmpty() && ActiveMFMEntity.PSW_DATE.ToValueAsString().IsNotNullOrEmpty())
                        {
                            if (showMessage.ToBooleanAsString()) ShowInformationMessage("PSW Date should be made empty before changing it to NO option");
                            ActiveEntity.PSW_ST = "YES";
                            PWSDateVisibility = Visibility.Visible;
                            //ActiveMFMEntity.PSW_DATE = null;
                            return bReturnValue;
                        }
                        else
                        {
                            PWSDateVisibility = Visibility.Collapsed;
                        }
                        break;
                    default: PWSDateVisibility = Visibility.Visible; break;
                }
                bReturnValue = true;
            }
            catch (Exception ex) { throw ex.LogException(); }
            return bReturnValue;
        }

        private MFM_MAST _activeMFMEntity = null;
        public MFM_MAST ActiveMFMEntity
        {
            get
            {
                return _activeMFMEntity;
            }
            set
            {
                _activeMFMEntity = value;
                NotifyPropertyChanged("ActiveMFMEntity");
            }
        }

        private MFM_MAST _activeProcessIssueEntity = null;
        public MFM_MAST ActivProcessIssueEntity
        {
            get
            {
                return _activeProcessIssueEntity;
            }
            set
            {
                _activeProcessIssueEntity = value;
                NotifyPropertyChanged("ActivProcessIssueEntity");
            }
        }

        private PRD_DWG_ISSUE _activeSequenceDrawingIssueEntity = null;
        public PRD_DWG_ISSUE ActivSequenceDrawingIssueEntity
        {
            get
            {
                return _activeSequenceDrawingIssueEntity;
            }
            set
            {
                _activeSequenceDrawingIssueEntity = value;
                NotifyPropertyChanged("ActivSequenceDrawingIssueEntity");
            }
        }

        private PRD_DWG_ISSUE _activeProductDrawingIssueEntity = null;
        public PRD_DWG_ISSUE ActivProductDrawingIssueEntity
        {
            get
            {
                return _activeProductDrawingIssueEntity;
            }
            set
            {
                _activeProductDrawingIssueEntity = value;
                NotifyPropertyChanged("ActivProductDrawingIssueEntity");
            }
        }

        private DDCI_INFO _activeCIInfoEntity = null;
        public DDCI_INFO ActiveCIInfoEntity
        {
            get
            {
                return _activeCIInfoEntity;
            }
            set
            {
                _activeCIInfoEntity = value;
                NotifyPropertyChanged("ActiveCIInfoEntity");
            }
        }

        private readonly ICommand plannedDocumentReleaseDateOnChangedCommand;
        public ICommand PlannedDocumentReleaseDateOnChangedCommand { get { return this.plannedDocumentReleaseDateOnChangedCommand; } }
        private void plannedDocumentReleaseDateOnChanged()
        {
            switch (ActionMode)
            {
                case OperationMode.AddNew:
                    //ActiveEntity.FR_CS_DATE = ActiveEntity.ENQU_RECD_ON;
                    //MandatoryFields.CI_REFERENCE = bll.CreateCIReferenceNumber(ActiveEntity);
                    //copyMandatoryFieldsToEntity(MandatoryFields);
                    break;
            }
        }

        private readonly ICommand samplesSubmissionDateOnChangedCommand;
        public ICommand SamplesSubmissionDateOnChangedCommand { get { return this.samplesSubmissionDateOnChangedCommand; } }
        private void samplesSubmissionDateOnChanged()
        {
            isValidSamplesSubmissionDate();
        }

        private readonly ICommand samplesSubmissionDateLostFocusCommand = null;
        public ICommand SamplesSubmissionDateLostFocusCommand { get { return this.samplesSubmissionDateLostFocusCommand; } }
        private void samplesSubmissionDateLostFocus()
        {
            isValidSamplesSubmissionDate();
        }

        public bool SamplesSubmissionDateReadOnly = false;

        private bool isValidSamplesSubmissionDate()
        {
            bool bReturnValue = false;
            try
            {
                if (!ActiveEntity.PART_NO.IsNotNullOrEmpty() || !ActiveEntity.SAMP_SUBMIT_DATE.IsNotNullOrEmpty()) return true;
                List<DEV_REPORT_SUB> lstDevelopmentSubReport = bll.GetDevelopmentSubReportByPartNumber(ActiveEntity);

                SamplesSubmissionDateReadOnly = false;
                if (!lstDevelopmentSubReport.IsNotNullOrEmpty() || lstDevelopmentSubReport.Count == 0) //added !
                {
                    ShowInformationMessage("Sample Submission Report can be entered only after entering Development Report.");
                    ActiveEntity.SAMP_SUBMIT_DATE = null;
                    SamplesSubmissionDateReadOnly = true;
                    return bReturnValue;
                }

                List<MFM_MAST> lstManufacturingMaster = (from row in bll.GetManufacturingMasterByPartNumber(ActiveEntity)
                                                         where ((row.DOC_REL_DT_PLAN != null && row.DOC_REL_DT_ACTUAL == null) ||
                                                         (row.TOOLS_READY_DT_PLAN != null && row.TOOLS_READY_ACTUAL_DT == null) ||
                                                         (row.FORGING_PLAN_DT != null && row.FORGING_ACTUAL_DT == null) ||
                                                         (row.SECONDARY_PLAN_DT != null && row.SECONDARY_ACTUAL_DT == null) ||
                                                         (row.HEAT_TREATMENT_PLAN_DT != null && row.HEAT_TREATMENT_ACTUAL == null))
                                                         && row.PART_NO == ActiveEntity.PART_NO
                                                         select row).ToList<MFM_MAST>();

                if (lstManufacturingMaster.IsNotNullOrEmpty() || lstManufacturingMaster.Count == 0)
                {
                    SamplesSubmissionDateReadOnly = false;
                }
                else
                {
                    ShowInformationMessage("Please Fill the Previous MFM Dates");
                    SamplesSubmissionDateReadOnly = true;
                    return bReturnValue;
                }
                bReturnValue = true;
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }


        #region Product PGCategory
        private DataView _productPGCategory = null;
        public DataView ProductPGCategoryDataSource
        {
            get
            {
                return _productPGCategory;
            }
            set
            {
                _productPGCategory = value;
                NotifyPropertyChanged("ProductPGCategoryDataSource");
            }
        }

        private DataRowView _productPGCategorySelectedRow;
        public DataRowView ProductPGCategorySelectedRow
        {
            get
            {
                return _productPGCategorySelectedRow;
            }

            set
            {
                _productPGCategorySelectedRow = value;
            }
        }

        private Visibility _productPGCategoryHasDropDownVisibility = Visibility.Visible;
        public Visibility ProductPGCategoryHasDropDownVisibility
        {
            get { return _productPGCategoryHasDropDownVisibility; }
            set
            {
                _productPGCategoryHasDropDownVisibility = value;
                NotifyPropertyChanged("ProductPGCategoryHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _productPGCategoryDropDownItems;
        public ObservableCollection<DropdownColumns> ProductPGCategoryDropDownItems
        {
            get
            {
                return _productPGCategoryDropDownItems;
            }
            set
            {
                _productPGCategoryDropDownItems = value;
                OnPropertyChanged("ProductPGCategoryDropDownItems");
            }
        }

        private readonly ICommand productPGCategorySelectedItemChangedCommand;
        public ICommand ProductPGCategorySelectedItemChangedCommand { get { return this.productPGCategorySelectedItemChangedCommand; } }
        private void productPGCategoryChanged()
        {
            CopyMandatoryFieldsToEntity(ProductPGCategoryDataSource, ref _productPGCategorySelectedRow, "PG_CAT", "PG_CATEGORY", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand productPGCategoryEndEditCommand;
        public ICommand ProductPGCategoryEndEditCommand { get { return this.productPGCategoryEndEditCommand; } }
        private void productPGCategoryEndEdit()
        {
            //productPGCategoryChanged();
            //CopyMandatoryFieldsToEntity(ProductPGCategoryDataSource, ProductPGCategorySelectedRow, "PG_CAT", "PG_CATEGORY", MandatoryFields, ActiveEntity);
        }
        #endregion

        #region Product Keywords
        private DataView _productKeywords = null;
        public DataView ProductKeywordsDataSource
        {
            get
            {
                return _productKeywords;
            }
            set
            {
                _productKeywords = value;
                NotifyPropertyChanged("ProductKeywordsDataSource");
            }
        }

        private DataRowView _productKeywordsSelectedRow;
        public DataRowView ProductKeywordsSelectedRow
        {
            get
            {
                return _productKeywordsSelectedRow;
            }

            set
            {
                _productKeywordsSelectedRow = value;
            }
        }

        private Visibility _productKeywordsHasDropDownVisibility = Visibility.Visible;
        public Visibility ProductKeywordsHasDropDownVisibility
        {
            get { return _productKeywordsHasDropDownVisibility; }
            set
            {
                _productKeywordsHasDropDownVisibility = value;
                NotifyPropertyChanged("ProductKeywordsHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _productKeywordsDropDownItems;
        public ObservableCollection<DropdownColumns> ProductKeywordsDropDownItems
        {
            get
            {
                return _productKeywordsDropDownItems;
            }
            set
            {
                _productKeywordsDropDownItems = value;
                OnPropertyChanged("ProductKeywordsDropDownItems");
            }
        }

        private readonly ICommand productKeywordsSelectedItemChangedCommand;
        public ICommand ProductKeywordsSelectedItemChangedCommand { get { return this.productKeywordsSelectedItemChangedCommand; } }
        private void productKeywordsChanged()
        {
            CopyMandatoryFieldsToEntity(ProductKeywordsDataSource, ref _productKeywordsSelectedRow, "PRD_CODE", "KEYWORDS", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand productKeywordsEndEditCommand;
        public ICommand ProductKeywordsEndEditCommand { get { return this.productKeywordsEndEditCommand; } }
        private void productKeywordsEndEdit()
        {
            //productKeywordsChanged();
            //CopyMandatoryFieldsToEntity(ProductKeywordsDataSource, ProductKeywordsSelectedRow, "PRD_CODE", "KEYWORDS", MandatoryFields, ActiveEntity);
        }
        #endregion

        #region Product Type
        private DataView _productType = null;
        public DataView ProductTypeDataSource
        {
            get
            {
                return _productType;
            }
            set
            {
                _productType = value;
                NotifyPropertyChanged("ProductTypeDataSource");
            }
        }

        private DataRowView _productTypeSelectedRow;
        public DataRowView ProductTypeSelectedRow
        {
            get
            {
                return _productTypeSelectedRow;
            }

            set
            {
                _productTypeSelectedRow = value;
            }
        }

        private Visibility _productTypeHasDropDownVisibility = Visibility.Visible;
        public Visibility ProductTypeHasDropDownVisibility
        {
            get { return _productTypeHasDropDownVisibility; }
            set
            {
                _productTypeHasDropDownVisibility = value;
                NotifyPropertyChanged("ProductTypeHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _productTypeDropDownItems;
        public ObservableCollection<DropdownColumns> ProductTypeDropDownItems
        {
            get
            {
                return _productTypeDropDownItems;
            }
            set
            {
                _productTypeDropDownItems = value;
                OnPropertyChanged("ProductTypeDropDownItems");
            }
        }

        private readonly ICommand productTypeSelectedItemChangedCommand;
        public ICommand ProductTypeSelectedItemChangedCommand { get { return this.productTypeSelectedItemChangedCommand; } }
        private void productTypeChanged()
        {
            CopyMandatoryFieldsToEntity(ProductTypeDataSource, ref _productTypeSelectedRow, "PRD_CODE", "TYPE", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand productTypeEndEditCommand;
        public ICommand ProductTypeEndEditCommand { get { return this.productTypeEndEditCommand; } }
        private void productTypeEndEdit()
        {
            //productTypeChanged();
            //CopyMandatoryFieldsToEntity(ProductTypeDataSource, ProductTypeSelectedRow, "PRD_CODE", "TYPE", MandatoryFields, ActiveEntity);
        }
        #endregion

        #region Product ShankForm
        private DataView _productShankForm = null;
        public DataView ProductShankFormDataSource
        {
            get
            {
                return _productShankForm;
            }
            set
            {
                _productShankForm = value;
                NotifyPropertyChanged("ProductShankFormDataSource");
            }
        }

        private DataRowView _productShankFormSelectedRow;
        public DataRowView ProductShankFormSelectedRow
        {
            get
            {
                return _productShankFormSelectedRow;
            }

            set
            {
                _productShankFormSelectedRow = value;
            }
        }

        private Visibility _productShankFormHasDropDownVisibility = Visibility.Visible;
        public Visibility ProductShankFormHasDropDownVisibility
        {
            get { return _productShankFormHasDropDownVisibility; }
            set
            {
                _productShankFormHasDropDownVisibility = value;
                NotifyPropertyChanged("ProductShankFormHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _productShankFormDropDownItems;
        public ObservableCollection<DropdownColumns> ProductShankFormDropDownItems
        {
            get
            {
                return _productShankFormDropDownItems;
            }
            set
            {
                _productShankFormDropDownItems = value;
                OnPropertyChanged("ProductShankFormDropDownItems");
            }
        }

        private readonly ICommand productShankFormSelectedItemChangedCommand;
        public ICommand ProductShankFormSelectedItemChangedCommand { get { return this.productShankFormSelectedItemChangedCommand; } }
        private void productShankFormChanged()
        {
            CopyMandatoryFieldsToEntity(ProductShankFormDataSource, ref _productShankFormSelectedRow, "PRD_CODE", "APPLICATION", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand productShankFormEndEditCommand;
        public ICommand ProductShankFormEndEditCommand { get { return this.productShankFormEndEditCommand; } }
        private void productShankFormEndEdit()
        {
            //productShankFormChanged();
            //CopyMandatoryFieldsToEntity(ProductShankFormDataSource, ProductShankFormSelectedRow, "PRD_CODE", "APPLICATION", MandatoryFields, ActiveEntity);
        }
        #endregion

        #region Product HeadForm
        private DataView _productHeadForm = null;
        public DataView ProductHeadFormDataSource
        {
            get
            {
                return _productHeadForm;
            }
            set
            {
                _productHeadForm = value;
                NotifyPropertyChanged("ProductHeadFormDataSource");
            }
        }

        private DataRowView _productHeadFormSelectedRow;
        public DataRowView ProductHeadFormSelectedRow
        {
            get
            {
                return _productHeadFormSelectedRow;
            }

            set
            {
                _productHeadFormSelectedRow = value;
            }
        }

        private Visibility _productHeadFormHasDropDownVisibility = Visibility.Visible;
        public Visibility ProductHeadFormHasDropDownVisibility
        {
            get { return _productHeadFormHasDropDownVisibility; }
            set
            {
                _productHeadFormHasDropDownVisibility = value;
                NotifyPropertyChanged("ProductHeadFormHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _productHeadFormDropDownItems;
        public ObservableCollection<DropdownColumns> ProductHeadFormDropDownItems
        {
            get
            {
                return _productHeadFormDropDownItems;
            }
            set
            {
                _productHeadFormDropDownItems = value;
                OnPropertyChanged("ProductHeadFormDropDownItems");
            }
        }

        private readonly ICommand productHeadFormSelectedItemChangedCommand;
        public ICommand ProductHeadFormSelectedItemChangedCommand { get { return this.productHeadFormSelectedItemChangedCommand; } }
        private void productHeadFormChanged()
        {
            CopyMandatoryFieldsToEntity(ProductHeadFormDataSource, ref _productHeadFormSelectedRow, "PRD_CODE", "HEAD_STYLE", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand productHeadFormEndEditCommand;
        public ICommand ProductHeadFormEndEditCommand { get { return this.productHeadFormEndEditCommand; } }
        private void productHeadFormEndEdit()
        {
            //productHeadFormChanged();
            //CopyMandatoryFieldsToEntity(ProductHeadFormDataSource, ProductHeadFormSelectedRow, "PRD_CODE", "HEAD_STYLE", MandatoryFields, ActiveEntity);
        }
        #endregion

        #region Product EndForm
        private DataView _productEndForm = null;
        public DataView ProductEndFormDataSource
        {
            get
            {
                return _productEndForm;
            }
            set
            {
                _productEndForm = value;
                NotifyPropertyChanged("ProductEndFormDataSource");
            }
        }

        private DataRowView _productEndFormSelectedRow;
        public DataRowView ProductEndFormSelectedRow
        {
            get
            {
                return _productEndFormSelectedRow;
            }

            set
            {
                _productEndFormSelectedRow = value;
            }
        }

        private Visibility _productEndFormHasDropDownVisibility = Visibility.Visible;
        public Visibility ProductEndFormHasDropDownVisibility
        {
            get { return _productEndFormHasDropDownVisibility; }
            set
            {
                _productEndFormHasDropDownVisibility = value;
                NotifyPropertyChanged("ProductEndFormHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _productEndFormDropDownItems;
        public ObservableCollection<DropdownColumns> ProductEndFormDropDownItems
        {
            get
            {
                return _productEndFormDropDownItems;
            }
            set
            {
                _productEndFormDropDownItems = value;
                OnPropertyChanged("ProductEndFormDropDownItems");
            }
        }

        private readonly ICommand productEndFormSelectedItemChangedCommand;
        public ICommand ProductEndFormSelectedItemChangedCommand { get { return this.productEndFormSelectedItemChangedCommand; } }
        private void productEndFormChanged()
        {
            CopyMandatoryFieldsToEntity(ProductEndFormDataSource, ref _productEndFormSelectedRow, "PRD_CODE", "PRD_CLASS_CD", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand productEndFormEndEditCommand;
        public ICommand ProductEndFormEndEditCommand { get { return this.productEndFormEndEditCommand; } }
        private void productEndFormEndEdit()
        {
            //productEndFormChanged();
            //CopyMandatoryFieldsToEntity(ProductEndFormDataSource, ProductEndFormSelectedRow, "PRD_CODE", "PRD_CLASS_CD", MandatoryFields, ActiveEntity);
        }
        #endregion

        #region Product DrivingFeature
        private DataView _productDrivingFeature = null;
        public DataView ProductDrivingFeatureDataSource
        {
            get
            {
                return _productDrivingFeature;
            }
            set
            {
                _productDrivingFeature = value;
                NotifyPropertyChanged("ProductDrivingFeatureDataSource");
            }
        }

        private DataRowView _productDrivingFeatureSelectedRow;
        public DataRowView ProductDrivingFeatureSelectedRow
        {
            get
            {
                return _productDrivingFeatureSelectedRow;
            }

            set
            {
                _productDrivingFeatureSelectedRow = value;
            }
        }

        private Visibility _productDrivingFeatureHasDropDownVisibility = Visibility.Visible;
        public Visibility ProductDrivingFeatureHasDropDownVisibility
        {
            get { return _productDrivingFeatureHasDropDownVisibility; }
            set
            {
                _productDrivingFeatureHasDropDownVisibility = value;
                NotifyPropertyChanged("ProductDrivingFeatureHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _productDrivingFeatureDropDownItems;
        public ObservableCollection<DropdownColumns> ProductDrivingFeatureDropDownItems
        {
            get
            {
                return _productDrivingFeatureDropDownItems;
            }
            set
            {
                _productDrivingFeatureDropDownItems = value;
                OnPropertyChanged("ProductDrivingFeatureDropDownItems");
            }
        }

        private readonly ICommand productDrivingFeatureSelectedItemChangedCommand;
        public ICommand ProductDrivingFeatureSelectedItemChangedCommand { get { return this.productDrivingFeatureSelectedItemChangedCommand; } }
        private void productDrivingFeatureChanged()
        {
            CopyMandatoryFieldsToEntity(ProductDrivingFeatureDataSource, ref _productDrivingFeatureSelectedRow, "PRD_CODE", "PRD_GRP_CD", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand productDrivingFeatureEndEditCommand;
        public ICommand ProductDrivingFeatureEndEditCommand { get { return this.productDrivingFeatureEndEditCommand; } }
        private void productDrivingFeatureEndEdit()
        {
            //productDrivingFeatureChanged();
            //CopyMandatoryFieldsToEntity(ProductDrivingFeatureDataSource, ProductDrivingFeatureSelectedRow, "PRD_CODE", "PRD_GRP_CD", MandatoryFields, ActiveEntity);
        }
        #endregion

        #region Product AdditionalFeature
        private DataView _productAdditionalFeature = null;
        public DataView ProductAdditionalFeatureDataSource
        {
            get
            {
                return _productAdditionalFeature;
            }
            set
            {
                _productAdditionalFeature = value;
                NotifyPropertyChanged("ProductAdditionalFeatureDataSource");
            }
        }

        private DataRowView _productAdditionalFeatureSelectedRow;
        public DataRowView ProductAdditionalFeatureSelectedRow
        {
            get
            {
                return _productAdditionalFeatureSelectedRow;
            }

            set
            {
                _productAdditionalFeatureSelectedRow = value;
            }
        }

        private Visibility _productAdditionalFeatureHasDropDownVisibility = Visibility.Visible;
        public Visibility ProductAdditionalFeatureHasDropDownVisibility
        {
            get { return _productAdditionalFeatureHasDropDownVisibility; }
            set
            {
                _productAdditionalFeatureHasDropDownVisibility = value;
                NotifyPropertyChanged("ProductAdditionalFeatureHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _productAdditionalFeatureDropDownItems;
        public ObservableCollection<DropdownColumns> ProductAdditionalFeatureDropDownItems
        {
            get
            {
                return _productAdditionalFeatureDropDownItems;
            }
            set
            {
                _productAdditionalFeatureDropDownItems = value;
                OnPropertyChanged("ProductAdditionalFeatureDropDownItems");
            }
        }

        private readonly ICommand productAdditionalFeatureSelectedItemChangedCommand;
        public ICommand ProductAdditionalFeatureSelectedItemChangedCommand { get { return this.productAdditionalFeatureSelectedItemChangedCommand; } }
        private void productAdditionalFeatureChanged()
        {
            CopyMandatoryFieldsToEntity(ProductAdditionalFeatureDataSource, ref _productAdditionalFeatureSelectedRow, "PRD_CODE", "ADDL_FEATURE", MandatoryFields, ActiveEntity, true);
        }

        private readonly ICommand productAdditionalFeatureEndEditCommand;
        public ICommand ProductAdditionalFeatureEndEditCommand { get { return this.productAdditionalFeatureEndEditCommand; } }
        private void productAdditionalFeatureEndEdit()
        {
            //productAdditionalFeatureChanged();
            //CopyMandatoryFieldsToEntity(ProductAdditionalFeatureDataSource, ProductAdditionalFeatureSelectedRow, "PRD_CODE", "ADDL_FEATURE", MandatoryFields, ActiveEntity);
        }
        #endregion

        private Visibility pswDateVisibility = Visibility.Visible;
        public Visibility PWSDateVisibility
        {
            get { return pswDateVisibility; }
            set
            {
                pswDateVisibility = value;
                NotifyPropertyChanged("PWSDateVisibility");
            }
        }

        #region CI Reference
        private DataView _ciReference = null;
        public DataView CIReferenceDataSource
        {
            get
            {
                return _ciReference;
            }
            set
            {
                _ciReference = value;
                NotifyPropertyChanged("CIReferenceDataSource");
            }
        }

        private Visibility _ciReferenceHasDropDownVisibility = Visibility.Visible;
        public Visibility CIReferenceHasDropDownVisibility
        {
            get { return _ciReferenceHasDropDownVisibility; }
            set
            {
                _ciReferenceHasDropDownVisibility = value;
                NotifyPropertyChanged("CIReferenceHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _ciReferenceDropDownItems;
        public ObservableCollection<DropdownColumns> CIReferenceDropDownItems
        {
            get
            {
                return _ciReferenceDropDownItems;
            }
            set
            {
                _ciReferenceDropDownItems = value;
                OnPropertyChanged("CIReferenceDropDownItems");
            }
        }



        private readonly ICommand ciReferenceSelectedItemChangedCommand;
        public ICommand CIReferenceSelectedItemChangedCommand { get { return this.ciReferenceSelectedItemChangedCommand; } }
        private void CIReferenceSelectionChanged()
        {
            if (CIReferenceSelectedRow == null) return;

            if (IsDuplicateCIRef(CIReferenceSelectedRow["CI_REFERENCE"].ToString()))
            {
                ShowInformationMessage("Duplicate CI reference has been Entered");
                Prd_CIref_SelectedRow["CI_REF"] = "";
                return;
            }
            Product_CIinfo.Table.AcceptChanges();
            ActiveCIInfoEntity.CI_REFERENCE = CIReferenceSelectedRow["CI_REFERENCE"].ToValueAsString();
            CIReferenceChanged();
        }

        private void CIReferenceChanged()
        {
            if (Prd_CIref_SelectedRow == null) return;


            List<DDCI_INFO> lstActiveCIInfoEntity = bllFRCS.GetEntitiesByCIReferenceNumber(new DDCI_INFO() { CI_REFERENCE = ActiveCIInfoEntity.CI_REFERENCE });

            if (lstActiveCIInfoEntity.IsNotNullOrEmpty() && lstActiveCIInfoEntity.Count == 1)
            {
                ActiveCIInfoEntity = lstActiveCIInfoEntity[0].DeepCopy<DDCI_INFO>();
                Prd_CIref_SelectedRow["CIREF_NO_FK"] = ActiveCIInfoEntity.IDPK;

                CustomersDataSource.RowFilter = null;
                CustomersDataSource.RowFilter = " CUST_CODE = '" + ActiveCIInfoEntity.CUST_CODE.ToValueAsString().FormatEscapeChars() + "'";
                if (CustomersDataSource.Count > 0)
                {
                    CustomerSelectedRow = CustomersDataSource[0];
                    MandatoryFields.CUST_CODE = CustomersDataSource[0]["CUST_NAME"].ToValueAsString();
                }
                else if (ActionMode == OperationMode.Edit && ActiveCIInfoEntity.CUST_CODE.IsNotNullOrEmpty())
                {
                    List<DDCUST_MAST> lstCustomers = bllFRCS.GetCustomerDetails(new DDCUST_MAST() { CUST_CODE = ActiveCIInfoEntity.CUST_CODE.ToValueAsString().ToDecimalValue() });

                    if (lstCustomers.IsNotNullOrEmpty() && lstCustomers.Count > 0)
                    {
                        DataTable dtCustomers = lstCustomers.ToDataTable<DDCUST_MAST>();
                        foreach (DataRow row in dtCustomers.Rows)
                            CustomersDataSource.Table.ImportRow(row);

                        CustomersDataSource.Table.AcceptChanges();
                        CustomersDataSource = CustomersDataSource.Table.DefaultView;

                        CustomersDataSource.RowFilter = " CUST_CODE = '" + ActiveCIInfoEntity.CUST_CODE.ToValueAsString().FormatEscapeChars() + "'";
                        if (CustomersDataSource.Count > 0)
                        {
                            CustomerSelectedRow = CustomersDataSource[0];
                            MandatoryFields.CUST_CODE = CustomersDataSource[0]["CUST_NAME"].ToValueAsString();
                        }
                    }
                    else
                    {
                        MandatoryFields.CUST_CODE = "";
                    }
                }
                CustomersDataSource.RowFilter = null;
                CustomerChanged();

                MandatoryFields.FINISH_DESC = bll.GetFinishDetails(new DDFINISH_MAST() { FINISH_CODE = ActiveCIInfoEntity.FINISH_CODE });
                string topCoatDesc = bll.GetTopCoatDetails(new DDCOATING_MAST() { COATING_CODE = ActiveCIInfoEntity.COATING_CODE });
                MandatoryFields.FINISH_DESC = MandatoryFields.FINISH_DESC.ToValueAsString() + (MandatoryFields.FINISH_DESC.ToValueAsString().Trim().Length > 0 && topCoatDesc.ToValueAsString().Trim().Length > 0 ? " + " : "") + topCoatDesc;
                MandatoryFields.FINISH_DESC = MandatoryFields.FINISH_DESC.ToValueAsString() + (ActiveCIInfoEntity.FINISH_CODE.ToValueAsString().Trim().Length > 0 || ActiveCIInfoEntity.COATING_CODE.ToValueAsString().Trim().Length > 0 ? " (" : "") + ActiveCIInfoEntity.FINISH_CODE.ToValueAsString().Trim() + (ActiveCIInfoEntity.FINISH_CODE.ToValueAsString().Trim().Length > 0 && ActiveCIInfoEntity.COATING_CODE.ToValueAsString().Trim().Length > 0 ? "" : "") + ActiveCIInfoEntity.COATING_CODE.ToValueAsString().Trim() + (ActiveCIInfoEntity.FINISH_CODE.ToValueAsString().Trim().Length > 0 || ActiveCIInfoEntity.COATING_CODE.ToValueAsString().Trim().Length > 0 ? ") " : "");
            }
            else
            {
                ActiveCIInfoEntity = new DDCI_INFO();
                ActiveCIInfoEntity.CI_REFERENCE = Prd_CIref_SelectedRow["CI_REF"].ToString();
                MandatoryFields.CUST_CODE = string.Empty;
                ActiveCIInfoEntity.REALISATION = null;
                MandatoryFields.FINISH_DESC = string.Empty;
            }
            AddNewProductCIRef();

        }

        private readonly ICommand _productcirefselectionchangedcommand;
        public ICommand ProductCIRefSelectionChangedCommand { get { return this._productcirefselectionchangedcommand; } }
        private void ProductCIReferenceChanged()
        {
            if (Prd_CIref_SelectedRow != null)
            {
                ActiveCIInfoEntity.CI_REFERENCE = Prd_CIref_SelectedRow["CI_REF"].ToString();
                CIReferenceChanged();
            }
        }

        public void OnBeginningEditCIRef(object sender, DataGridBeginningEditEventArgs e)
        {
            DataRowView selecteditem = (System.Data.DataRowView)(e.Row.Item);
            string columnName = e.Column.SortMemberPath;

        }

        private static T GetVisualChild<T>(DependencyObject visual) where T : DependencyObject
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

        public void OnCellEditEndingCIRef(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                //if (e.Column.GetType() == typeof(DataGridTemplateColumn))
                //{
                //    var popup = GetVisualChild<System.Windows.Controls.Primitives.Popup>(e.EditingElement);
                //    if (popup != null && popup.IsOpen)
                //    {
                //        e.Cancel = true;
                //        return;
                //    }
                //}

                //Product_CIinfo.Table.AcceptChanges();

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        private readonly ICommand ciReferenceMouseDoubleClickCommand;
        public ICommand CIReferenceClickCommand { get { return this.ciReferenceMouseDoubleClickCommand; } }
        private void ciReferenceMouseDoubleClick()
        {
            if (Prd_CIref_SelectedRow == null) return;

            try
            {

                Progress.Start();

                if (Prd_CIref_SelectedRow["CI_REF"].IsNotNullOrEmpty())
                {
                    List<DDCI_INFO> lstDDCI_INFO = bllFRCS.GetEntitiesByCIReferenceNumber(new DDCI_INFO()
                    {
                        CI_REFERENCE = ActiveCIInfoEntity.CI_REFERENCE.ToValueAsString()
                    });


                    if (lstDDCI_INFO.IsNotNullOrEmpty() && lstDDCI_INFO.Count > 0)
                    {
                        ActiveCIInfoEntity = lstDDCI_INFO[0];

                        CustomersDataSource.RowFilter = null;
                        CustomersDataSource.RowFilter = " CUST_CODE = '" + ActiveCIInfoEntity.CUST_CODE.ToValueAsString().FormatEscapeChars() + "'";
                        if (CustomersDataSource.Count > 0)
                        {
                            CustomerSelectedRow = CustomersDataSource[0];
                            MandatoryFields.CUST_CODE = CustomersDataSource[0]["CUST_NAME"].ToValueAsString();

                        }
                        CustomersDataSource.RowFilter = null;

                    }
                }
                else
                {
                    ActiveCIInfoEntity = new DDCI_INFO();
                    ActiveCIInfoEntity.CI_REFERENCE = Prd_CIref_SelectedRow["CI_REF"].ToString();
                }



                String title = ApplicationTitle + " - " + "Cost Sheet for Part Number : " + MandatoryFields.PART_NO;

                OperationMode ciOperationMode = OperationMode.Edit;

                string message;
                if (!bllFRCS.IsValidCIReferenceNumber(ActiveCIInfoEntity, OperationMode.Edit, out message))
                {
                    if (message != "CI Reference Number should not be empty")
                    {
                        Progress.End();
                        ShowInformationMessage(message);
                        ciOperationMode = OperationMode.Edit;
                        return;
                    }
                    else
                    {
                        ciOperationMode = OperationMode.AddNew;
                        if (!ActiveEntity.PART_NO.IsNotNullOrEmpty())
                            title = ApplicationTitle + " - " + "Feasibility Report and Cost Sheet";
                        ActiveCIInfoEntity.IDPK = -99999;
                    }
                }


                MdiChild frcsChild;

                frcsChild = (MdiChild)MainMDI.GetFormAlreadyOpened(title);
                if (frcsChild != null) MainMDI.Container.Children.Remove(frcsChild);

                frcsChild = new MdiChild();
                ProcessDesigner.frmFRCS frcs = new ProcessDesigner.frmFRCS(_userInformation, frcsChild, ActiveCIInfoEntity.IDPK, ciOperationMode);
                frcsChild.Title = title;
                frcsChild.Content = frcs;
                frcsChild.Height = frcs.Height + 40;
                frcsChild.Width = frcs.Width + 20;
                frcsChild.MinimizeBox = false;
                frcsChild.MaximizeBox = false;
                frcsChild.Resizable = false;
                frcs.Unloaded += frcs_Unloaded;
                MainMDI.Container.Children.Add(frcsChild);

                Progress.End();
            }
            catch (Exception ex)
            {
                Progress.End();
                ex.LogException();
            }

        }

        void frcs_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                frmFRCS frcs = sender as frmFRCS;
                if (frcs != null && frcs.ltbCIReference.SelectedValue.IsNotNullOrEmpty() && Prd_CIref_SelectedRow != null && !Prd_CIref_SelectedRow["CI_REF"].IsNotNullOrEmpty())
                {
                    //Checking for the Existence of releated CIRefeence for this part number
                    List<PRD_CIREF> prd_ciref = null;
                    prd_ciref = bll.GetCIRefernce(new DDCI_INFO() { CI_REFERENCE = frcs.ltbCIReference.SelectedValue });

                    if (prd_ciref.IsNotNullOrEmpty() && prd_ciref.Count > 0 && ActiveEntity.PART_NO.ToValueAsString().Trim().ToUpper() != prd_ciref[0].PART_NO.ToValueAsString().Trim().ToUpper())
                    {
                        ShowInformationMessage("CI Reference number '" + prd_ciref[0].CI_REF + "' has been allotted to part number '" + prd_ciref[0].PART_NO + "'");
                        return;
                    }

                    List<DDCI_INFO> lstDDCI_INFO = bllFRCS.GetEntitiesByCIReferenceNumber(new DDCI_INFO()
                    {
                        CI_REFERENCE = frcs.ltbCIReference.SelectedValue.Trim()
                    });

                    if (lstDDCI_INFO.IsNotNullOrEmpty() && lstDDCI_INFO.Count > 0)
                    {
                        ActiveCIInfoEntity = lstDDCI_INFO[0];
                    }
                    else
                    {
                        ActiveCIInfoEntity = new DDCI_INFO();
                        ActiveCIInfoEntity.CI_REFERENCE = frcs.ltbCIReference.SelectedValue.Trim();
                    }

                    Prd_CIref_SelectedRow.BeginEdit();
                    Prd_CIref_SelectedRow["CI_REF"] = ActiveCIInfoEntity.CI_REFERENCE;
                    Prd_CIref_SelectedRow["PART_NO"] = ActiveEntity.PART_NO;
                    Prd_CIref_SelectedRow["CIREF_NO_FK"] = ActiveCIInfoEntity.IDPK;
                    Prd_CIref_SelectedRow.EndEdit();
                    CIReferenceChanged();

                    //DataTable savedCIReferenceDataTable = (from row in bllFRCS.GetCIReferenceNumber(ActiveCIInfoEntity).AsEnumerable()
                    //                                       select new V_CI_REFERENCE_NUMBER_PI()
                    //                                       {
                    //                                           CI_REFERENCE = row.CI_REFERENCE,
                    //                                           FRCS_DATE = row.FRCS_DATE,
                    //                                           CUST_DWG_NO = row.CUST_DWG_NO,
                    //                                           CUST_CODE = row.CUST_CODE,
                    //                                           FINISH_CODE = row.FINISH_CODE,
                    //                                           CUST_DWG_NO_ISSUE = row.CUST_DWG_NO_ISSUE,
                    //                                           CUST_STD_DATE = row.CUST_STD_DATE,
                    //                                           CI_REFERENCE_IDPK = row.IDPK,
                    //                                           PROD_DESC = row.PROD_DESC
                    //                                       }).ToList<V_CI_REFERENCE_NUMBER_PI>().ToDataTable<V_CI_REFERENCE_NUMBER_PI>();

                    //bll = new ProductInformation(_userInformation);
                    CIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER_PI>().DefaultView;

                    //foreach (DataRow row in savedCIReferenceDataTable.Rows)
                    //{
                    //    CIReferenceDataSource.Table.ImportRow(row);
                    //    CIReferenceDataSource.Table.AcceptChanges();
                    //}

                }
                else
                {
                    bll = new ProductInformation(_userInformation);
                    CIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER_PI>().DefaultView;
                }

            }
            catch (Exception ex)
            {
                ex.LogException();
            }
        }

        public void CIRef_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key != Key.Delete || Prd_CIref_SelectedRow == null || !Prd_CIref_SelectedRow["CI_REF"].IsNotNullOrEmpty()) return;

                ActiveCIInfoEntity.CI_REFERENCE = Prd_CIref_SelectedRow["CI_REF"].ToValueAsString();
                List<PRD_CIREF> prd_ciref = bll.GetCIRefernce(ActiveCIInfoEntity);
                if (prd_ciref.IsNotNullOrEmpty())
                {
                    if (ShowWarningMessage("Do you want to remove the Cost Sheet from this Part Number?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        if (prd_ciref.Count > 0 && bll.Delete<PRD_CIREF>(new List<PRD_CIREF>() { prd_ciref[0] }))
                        {
                            ShowInformationMessage(PDMsg.DeletedSuccessfully);
                            _logviewBll.SaveLog(MandatoryFields.PART_NO, "ProductMaster");
                            MandatoryFields.CUST_CODE = string.Empty;
                            MandatoryFields.FINISH_DESC = string.Empty;
                            ActiveCIInfoEntity = new DDCI_INFO();
                        }
                        Prd_CIref_SelectedRow.Delete();
                        CIReferenceDataSource = bll.GetCIReferenceNumber().ToDataTable<V_CI_REFERENCE_NUMBER_PI>().DefaultView;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        #endregion

        #region Customer
        private DataView _customers = null;
        public DataView CustomersDataSource
        {
            get
            {
                return _customers;
            }
            set
            {
                _customers = value;
                NotifyPropertyChanged("CustomersDataSource");
            }
        }

        private Visibility _customerHasDropDownVisibility = Visibility.Visible;
        public Visibility CustomerHasDropDownVisibility
        {
            get { return _customerHasDropDownVisibility; }
            set
            {
                _customerHasDropDownVisibility = value;
                NotifyPropertyChanged("CustomerHasDropDownVisibility");
            }
        }

        private ObservableCollection<DropdownColumns> _customerDropDownItems;
        public ObservableCollection<DropdownColumns> CustomerDropDownItems
        {
            get
            {
                return _customerDropDownItems;
            }
            set
            {
                _customerDropDownItems = value;
                OnPropertyChanged("CustomerDropDownItems");
            }
        }

        private DataRowView _customerSelectedRow;
        public DataRowView CustomerSelectedRow
        {
            get
            {
                return _customerSelectedRow;
            }

            set
            {
                _customerSelectedRow = value;
            }
        }

        private readonly ICommand customerSelectedItemChangedCommand;
        public ICommand CustomerSelectedItemChangedCommand { get { return this.customerSelectedItemChangedCommand; } }
        private void CustomerChanged()
        {

            CopyMandatoryFieldsToEntity(CustomersDataSource, ref _customerSelectedRow, "CUST_CODE", "CUST_NAME", MandatoryFields, ActiveEntity, true, "IN");
        }


        private readonly ICommand customerEndEditCommand;
        public ICommand CustomerEndEditCommand { get { return this.customerEndEditCommand; } }
        private void customerEndEdit()
        {
            //CustomerChanged();
            //CopyMandatoryFieldsToEntity(CustomersDataSource, CustomerSelectedRow, "CUST_CODE", "CUST_NAME", MandatoryFields, ActiveEntity, "IN");
        }
        #endregion

        private readonly ICommand releaseDocumentCommand;
        public ICommand ReleaseDocumentClickCommand { get { return this.releaseDocumentCommand; } }
        private void releaseDocumentSubmitCommand()
        {
            if (!ActionPermission.ReleaseDocument) return;



            if (!ActiveEntity.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part Number"));
                return;
            }

            if (!ActiveCIInfoEntity.CI_REFERENCE.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("CI Reference"));
                return;
            }

            //if (!ActiveEntity.DOC_REL_DATE.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Planned Document Release Date"));
            //    return;
            //}

            if (Product_CIinfo.Count == 1 && !Product_CIinfo[0]["CI_REF"].IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("CI Reference number") + ".\nYou can select existing CI reference or create new CI reference by double clicking CI reference grid drop down");
                return;
            }

            if (!SaveAllEntity(false)) return;

            if (ActionMode == OperationMode.AddNew)
            {
                foreach (DataRowView drv in Product_CIinfo)
                {
                    if (drv["CI_REF"].IsNotNullOrEmpty())
                    {
                        ActiveCIInfoEntity.CI_REFERENCE = drv["CI_REF"].ToString();

                        //Checking for Existence of CIReference in FRCS
                        if (!isDataExists(CIReferenceDataSource.Table.Copy().DefaultView, CIReferenceSelectedRow, ActionMode,
                            "CI_REFERENCE", ActiveCIInfoEntity.CI_REFERENCE.ToValueAsString(), "Related CI Reference", true, "\r\nDocument cannot be released.")) return;
                    }
                }
            }

            //checking PartNo
            if (!isDataExists(PartNumberDataSource.Table.Copy().DefaultView, PartNumberSelectedRow, OperationMode.Edit,
                "PART_NO", ActiveEntity.PART_NO, "Part Number")) return;

            //Checking for the Existence of releated CIRefeence for this part number
            if (!bll.GetCIRefernceByPartNumber(ActiveEntity).IsNotNullOrEmpty())
            {
                ShowInformationMessage("There is no CI Reference related to this part number.\r\nDocument cannot be released.");
                return;
            }


            List<PROCESS_SHEET> lstProcessSheet = bll.GetProcessSheetByPartNumber(ActiveEntity);
            long processSheetRecordCount = 0;
            if (lstProcessSheet.IsNotNullOrEmpty()) processSheetRecordCount = lstProcessSheet.Count;

            List<PROCESS_SHEET> lstProcessSheetOperation = (from row in lstProcessSheet
                                                            where row.PART_NO.ToUpper() == ActiveEntity.PART_NO.ToUpper() &&
                                                            row.OPN_CD == 2600
                                                            select row).ToList<PROCESS_SHEET>();

            long processSheetOperationCount = 0;
            if (lstProcessSheetOperation.IsNotNullOrEmpty()) processSheetOperationCount = lstProcessSheetOperation.Count;

            if (processSheetRecordCount < 2 || processSheetOperationCount == 0)
            {
                ShowInformationMessage("The Process Sheet is incomplete. \r\nDocument cannot be released");
                return;

            }

            //Checking for Product Drawing
            List<PRD_DWG_ISSUE> lstProductDrawingIssueDetails = bll.GetDrawingIssueDetailsByPartNumber(
                new PRD_DWG_ISSUE() { PART_NO = ActiveEntity.PART_NO, DWG_TYPE = 0 });

            if (!lstProductDrawingIssueDetails.IsNotNullOrEmpty() || lstProductDrawingIssueDetails.Count == 0)
            {
                ShowInformationMessage("The Issue details for the product drawing is incomplete.\r\nDocument cannot be released.");
                return;
            }

            //Check for Sequence Drawing
            List<PRD_DWG_ISSUE> lstSequenceDrawingIssueDetails = bll.GetDrawingIssueDetailsByPartNumber(
                new PRD_DWG_ISSUE() { PART_NO = ActiveEntity.PART_NO, DWG_TYPE = 1 });

            if (!lstSequenceDrawingIssueDetails.IsNotNullOrEmpty() || lstSequenceDrawingIssueDetails.Count == 0)
            {
                ShowInformationMessage("The Issue details for the Sequence drawing is incomplete.\r\nDocument cannot be released.");
                return;
            }

            //Process Issue Details
            List<PROCESS_ISSUE> lstProcessIssueDetails = bll.GetProcessIssueByPartNumber(ActiveEntity);

            if (!lstProcessIssueDetails.IsNotNullOrEmpty() || lstProcessIssueDetails.Count == 0)
            {
                ShowInformationMessage("The Issue details for the Process Sheet is incomplete.\r\nDocument cannot be released.");
                return;
            }

            switch (ActiveEntity.BIF_PROJ.ToValueAsString().ToUpper())
            {
                case "MM":
                case "MN":
                    MessageBoxResult messageBoxResult = ShowWarningMessage("Do you wish to validate Tool Schedule?", MessageBoxButton.YesNoCancel);
                    if (messageBoxResult == MessageBoxResult.Cancel) return;
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        List<PROCESS_SHEET> lstProcessSheetLocation = (from row in lstProcessSheet
                                                                       where row.PART_NO == ActiveEntity.PART_NO &&
                                                                       Convert.ToString(row.OPN_CD).StartsWith("10")
                                                                       select row).ToList<PROCESS_SHEET>();
                        if (lstProcessSheetLocation.IsNotNullOrEmpty() && lstProcessSheetLocation.Count > 0)
                        {
                            if (lstProcessSheetLocation[0].OPN_DESC.ToValueAsString().ToUpper() != "CONVERT COIL TO BAR")
                            {
                                List<TOOL_SCHED_SUB> lstSubToolSchedule = bll.GetSubToolScheduleByPartNumber(ActiveEntity);
                                long subToolScheduleCount = 0;
                                if (lstSubToolSchedule.IsNotNullOrEmpty()) subToolScheduleCount = lstSubToolSchedule.Count;

                                //if (subToolScheduleCount < 2)
                                //{
                                //    ShowInformationMessage("The Tool Schedule is incomplete.\r\nDocument cannot be released.");
                                //    return;
                                //}

                                List<TOOL_SCHED_ISSUE> lstToolScheduleIssue = bll.GetToolScheduleIssueByPartNumber(ActiveEntity);

                                long toolScheduleIssueCount = 0;
                                if (lstToolScheduleIssue.IsNotNullOrEmpty()) toolScheduleIssueCount = lstToolScheduleIssue.Count;

                                if (toolScheduleIssueCount == 0)
                                {
                                    ShowInformationMessage("The Issue details for the Tool Schedule is incomplete.\r\nDocument cannot be released.");
                                    return;
                                }

                            }
                        }
                    }
                    break;
            }

            //Calling Update Order Processing
            UpdateOrderProcessing();

            Window win = new Window();
            win.Title = ApplicationTitle + " - " + "Document Release Date";

            System.Windows.Media.Imaging.IconBitmapDecoder ibd = new System.Windows.Media.Imaging.IconBitmapDecoder(new Uri(@"pack://application:,,/Images/logo.ico", UriKind.RelativeOrAbsolute), System.Windows.Media.Imaging.BitmapCreateOptions.None, System.Windows.Media.Imaging.BitmapCacheOption.Default);


            ProductInformationModel mandatoryFieldsCopy = MandatoryFields.DeepCopy<ProductInformationModel>();

            win.Icon = ibd.Frames[0];
            win.ResizeMode = ResizeMode.NoResize;
            ProcessDesigner.frmProductReleaseDate userControl = new frmProductReleaseDate(_userInformation, win, mandatoryFieldsCopy, ActionMode);
            win.Content = userControl;
            win.Height = userControl.Height + 50;
            win.Width = userControl.Width + 10;
            win.ShowInTaskbar = false;
            win.WindowStyle = WindowStyle.SingleBorderWindow;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();

            if (!mandatoryFieldsCopy.IsNotNullOrEmpty() || !mandatoryFieldsCopy.DOC_REL_DATE.IsNotNullOrEmpty() || mandatoryFieldsCopy.PART_DESC != "OKButtonClicked") return;

            ActiveEntity.DOC_REL_DATE = mandatoryFieldsCopy.DOC_REL_DATE;
            MandatoryFields.DOC_REL_DATE = ActiveEntity.DOC_REL_DATE;

            //if (!ActiveEntity.DOC_REL_DATE.IsNotNullOrEmpty())
            //{
            //    ShowInformationMessage(PDMsg.NotEmpty("Planned Document Release Date"));
            //    return;
            //}

            bll.Update<PRD_MAST>(new List<PRD_MAST>() { ActiveEntity });
            UpdateMFMmast();

            ShowInformationMessage("Document has been Released");
            //ActiveEntity.DOC_REL_DATE = (DateTime?)null;

        }
        private readonly ICommand closeCommand;
        public ICommand CloseCommand { get { return this.closeCommand; } }
        private void Close()
        {
            if (ShowConfirmMessageYesNo(PDMsg.CloseForm) == MessageBoxResult.Yes)
            {
                CloseAction();
            }
        }   //original
        private bool UpdateMFMmast()
        {
            bool bReturnValue = false;
            try
            {

                if (ActiveMFMEntity.IsNotNullOrEmpty())
                {
                    ActiveMFMEntity.PART_NO = ActiveEntity.PART_NO;
                    ActiveMFMEntity.SAMPLE_QTY = 0;
                    ActiveMFMEntity.DOC_REL_DT_PLAN = ActiveEntity.DOC_REL_DATE;
                    if (bll.LoggedOnUserName().IsNotNullOrEmpty())
                        ActiveMFMEntity.RESP = bll.LoggedOnUserName().Trim().Length > 6 ?
                                                bll.LoggedOnUserName().Trim().Substring(0, 5) : bll.LoggedOnUserName().Trim();
                    //if (!ActiveEntity.PSW_ST.ToValueAsString().ToBooleanAsString())
                    //{
                    //    ActiveMFMEntity.PSW_DATE = null;
                    //}
                }

                bReturnValue = bll.Update<MFM_MAST>(new List<MFM_MAST>() { ActiveMFMEntity });
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
            return bReturnValue;
        }

        private readonly ICommand updateOrderProcessingCommand;
        public ICommand UpdateOrderProcessingClickCommand { get { return this.updateOrderProcessingCommand; } }
        private void UpdateOrderProcessing()
        {
            if (!ActionPermission.UpdateOrderProcessing) return;
            if (!ActiveEntity.IsNotNullOrEmpty() || !ActiveEntity.PART_NO.IsNotNullOrEmpty()) return;

            //Dim rsProcess As ADODB.Recordset
            //Dim rsOPSDEVProcess As ADODB.Recordset
            //Dim rsMaxSeqDrawing As ADODB.Recordset
            //Dim rsMaxProdDrawing As ADODB.Recordset
            //Dim rsMaxProcessIssue As ADODB.Recordset
            //Dim rsCustDwg As ADODB.Recordset
            //Dim rsOPSDEVprdDrawing As ADODB.Recordset
            //Dim rsOPSDEVprdMast As ADODB.Recordset
            //Dim rsPrdMast As ADODB.Recordset

            //    Set rsPrdMast = New ADODB.Recordset
            //        rsPrdMast.CursorLocation = adUseClient

            List<V_COST_SHEET_PROCESS> lstCostSheetProcessByPartNumber = bll.GetCostSheetProcessByPartNumber(ActiveEntity);

            //Sql = "select a.part_no,part_desc ,sim_to_std_cd,family," & _
            //      " mfg_std,psw_st,thread_cd,dia_cd,quality,bif_proj, " & _
            //      " bif_forecast,c.cust_code,decode(length(finish_code),3,concat(0,finish_code),finish_code) as finish_code from prd_mast a, " & _
            //      " prd_ciref b,ddci_info c where a.part_no (+) =b.part_no " & _
            //      " and b.ci_ref=c.ci_reference and b.part_no='" & varProductNo & "'"

            //    rsPrdMast.Open Sql, gvarcnn, adOpenForwardOnly, adLockReadOnly

            //    If rsPrdMast.RecordCount = 0 Then
            //        MsgBox "No Entries found. You probably do not have a cost sheet attached.", vbInformation, "SmartPD"
            //        Screen.MousePointer = 0
            //        Exit Sub
            //    End If


            //With rsPrdMast
            //        If !Part_No = "" Or IsNull(!Part_No) Then
            //            MsgBox "Part No cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !part_desc = "" Or IsNull(!part_desc) Then
            //            MsgBox "Part Description cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !sim_to_std_cd = "" Or IsNull(!sim_to_std_cd) Then
            //            MsgBox "Similar to Std cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !Family = "" Or IsNull(!Family) Then
            //            MsgBox "Product family cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !mfg_std = "" Or IsNull(!mfg_std) Then
            //            MsgBox "Manufacturing Std cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !thread_cd = "" Or IsNull(!thread_cd) Then
            //            MsgBox "Thread Code cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !dia_cd = "" Or IsNull(!dia_cd) Then
            //            MsgBox "Thread Size cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !bif_proj = "" Or IsNull(!bif_proj) Then
            //            MsgBox "Bifurcation Project cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //    End With


            //     Set rsOPSDEVprdMast = New ADODB.Recordset
            //     rsOPSDEVprdMast.CursorLocation = adUseClient


            // Sql = " select part_no,part_desc,sim_to_std_cd," & _
            //       " prd_class_cd,mfg_std,thread_cd,dia_cd,bif_proj," & _
            //       " bif_forecast,quality,ed_cd,static_cust_cd,static_fc,upd_dt_time," & _
            //       " user_cd,psw_st from opsdev.prd_mast where part_no= '" & varProductNo & "' "

            //    rsOPSDEVprdMast.Open Sql, OPSDEV, adOpenDynamic, adLockBatchOptimistic

            //    With rsOPSDEVprdMast
            //        If .RecordCount = 0 Then
            //            .AddNew
            //            !Part_No = varProductNo
            //        End If
            //            !part_desc = rsPrdMast!part_desc
            //            !sim_to_std_cd = rsPrdMast!sim_to_std_cd
            //            !prd_class_cd = IIf(Mid(rsPrdMast!Family, 1, 1) = "B", "02", "09")
            //            !mfg_std = rsPrdMast!mfg_std
            //            !ed_cd = 1
            //            If Not IsNull(rsPrdMast!cust_code) Then
            //                !static_cust_cd = rsPrdMast!cust_code
            //            Else
            //                mess = "A Cost Sheet could not be found for this Part No." _
            //                    & "A Customer, Customer Drawing Number Or Finish cannot" _
            //                    & "be attached to this Part No unless a Cost sheet is " _
            //                    & "generated. Please do so before you quit this session."
            //                MsgBox mess, vbInformation, "SmartPD"
            //            End If
            //            If Not IsNull(rsPrdMast!finish_code) Then
            //'                If Len(rsPrdMast!finish_code) = 4 Then
            //'                    If MsgBox("Four Digit Finish Code has been Entered which has not been updated in Online " & Chr(13) & "Do you want to Continue?", vbInformation + vbYesNo + vbDefaultButton2, "SmartPD") = vbNo Then
            //'                        Me.MousePointer = 0
            //'                        Exit Sub
            //'                    End If
            //'                End If
            //                !static_fc = rsPrdMast!finish_code
            //            End If
            //            !thread_cd = rsPrdMast!thread_cd
            //            !dia_cd = rsPrdMast!dia_cd
            //            !bif_proj = rsPrdMast!bif_proj
            //            !bif_forecast = IIf(IsNull(rsPrdMast!bif_forecast), "", rsPrdMast!bif_forecast)
            //            !quality = IIf(IsNull(rsPrdMast!quality), "", rsPrdMast!quality)
            //            !upd_dt_time = GetServerDate
            //            !user_cd = gvarUserName
            //            !psw_st = IIf(IsNull(rsPrdMast!psw_st), "", rsPrdMast!psw_st)
            //            .UpdateBatch

            //    End With

            //    Set rsProcess = New ADODB.Recordset
            //        rsProcess.CursorLocation = adUseClient

            //'Check Valid Process Data being entered by D&D for Forging Operation

            List<V_FORGING_OPERATION_PROCESS> lstForgingOperationProcessByPartNumber = bll.GetForgingOperationProcessByPartNumber(ActiveEntity);

            //Sql = "select c.opn_cd, a.part_no,a.bif_proj,a.heat_treatment_cd,a.prd_grp_cd,a.nos_per_pack,b.rm_cd,b.alt_rm_cd,b.rm_wt," & _
            //      " b.cheese_wt ,finish_wt,finish_wt_est, b.cheese_wt_est, b.wire_rod_cd, b.alt_wire_rod_cd, b.tko_cd, b.ajax_cd,e.machine_cd,wire_size_min,wire_size_max " & _
            //      " from prd_mast a,process_main b,process_sheet c,process_cc d,ddcost_cent_mast e where " & _
            //      " a.part_no = b.part_no And b.part_no = c.part_no And b.route_no = c.route_no And c.part_no = d.part_no  and d.cc_code=e.cost_cent_code " & _
            //      " and c.route_no=d.route_no and c.seq_no=d.seq_no and d.cc_sno=1 " & _
            //      " and c.opn_cd between '1010' and '1110' and a.part_no='" & varProductNo & "'"

            //    rsProcess.Open Sql, gvarcnn, adOpenForwardOnly, adLockReadOnly


            //    If rsProcess.RecordCount = 0 Then
            //        MsgBox "Valid Forging Operation does not exist in the Process Sheet" & Chr(13) & _
            //                "Please complete the information in the Process Sheet before Releasing Part No.", vbInformation, "SmartPD"
            //        Screen.MousePointer = 0
            //        Exit Sub
            //    End If

            //    With rsProcess
            //        If !Part_No = "" Or IsNull(!Part_No) Then
            //            MsgBox "Part No cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !bif_proj = "" Or IsNull(!bif_proj) Then
            //            MsgBox "Project Code cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !prd_grp_cd = "" Or IsNull(!prd_grp_cd) Then
            //            MsgBox "Group Code cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !heat_treatment_cd = "" Or IsNull(!heat_treatment_cd) Then
            //            MsgBox "Heat Treatment Code cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !FINISH_WT = "" Or IsNull(!FINISH_WT) Then
            //            MsgBox "Finish Weight cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !RM_CD = "" Or IsNull(!RM_CD) Then
            //            MsgBox "Wire Spec cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !CHEESE_WT = "" Or IsNull(!CHEESE_WT) Then
            //            MsgBox "Cheese Weight cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !tko_cd = "" Or IsNull(!tko_cd) Then
            //            MsgBox "TKO Code cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !ajax_cd = "" Or IsNull(!ajax_cd) Then
            //            MsgBox "Ajax Code cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //'        If !machine_cd = "" Or IsNull(!machine_cd) Then
            //'            MsgBox "Machine Code cannot be empty", vbInformation, "SmartPD"
            //'            Screen.MousePointer = 0
            //'            Exit Sub
            //'        End If
            //        If !nos_per_pack = "" Or IsNull(!nos_per_pack) Then
            //            MsgBox "Nos Per Pack cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !wire_size_min = "" Or IsNull(!wire_size_min) Then
            //            MsgBox "Wire Size Min cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //        If !wire_size_max = "" Or IsNull(!wire_size_max) Then
            //            MsgBox "Wire Size Max cannot be empty", vbInformation, "SmartPD"
            //            Screen.MousePointer = 0
            //            Exit Sub
            //        End If
            //    End With

            //'Updationg in Prd_proj_mast table in OPSDEV Database

            //    Set rsOPSDEVProcess = New ADODB.Recordset
            //        rsOPSDEVProcess.CursorLocation = adUseClient

            //    Sql = "select part_no,proj_cd,prd_grp_cd,machine_cd,heat_treatment_cd," & _
            //          " finish_wt,finish_wt_est,rm_cd,alt_rm_cd,rm_scrap_cd, " & _
            //          " rm_wt,cheese_wt,cheese_wt_est,wire_size_min,wire_size_max, " & _
            //          " wire_rod_cd,alt_wire_rod_cd,tko_cd,ajax_cd,nos_per_pack, " & _
            //          " user_cd,upd_dt_time from opsdev.prd_proj_mast where part_no='" & varProductNo & "' and proj_cd='" & ltbCurrentLocation1.Value & "' "

            //        rsOPSDEVProcess.Open Sql, OPSDEV, adOpenKeyset, adLockBatchOptimistic

            //        With rsOPSDEVProcess
            //        If .RecordCount = 0 Then
            //            .AddNew
            //            !Part_No = varProductNo
            //        End If
            //            !proj_cd = rsProcess!bif_proj
            //            If Len(rsProcess!prd_grp_cd) > 3 Then
            //                !prd_grp_cd = Mid(rsProcess!prd_grp_cd, 5, 2)
            //            Else
            //                !prd_grp_cd = rsProcess!prd_grp_cd
            //            End If
            //            !machine_cd = IIf(IsNull(rsProcess!machine_cd), " ", rsProcess!machine_cd)
            //            !heat_treatment_cd = rsProcess!heat_treatment_cd
            //            !FINISH_WT = CDbl(rsProcess!FINISH_WT)
            //            If Not IsNull(rsProcess!finish_wt_est) Then !finish_wt_est = Val(rsProcess!finish_wt_est)
            //            !RM_CD = rsProcess!RM_CD & Format$(rsProcess!wire_size_min * 100, "0000")
            //           '' !ALT_RM_CD = rsProcess!ALT_RM_CD & Format$(rsProcess!wire_size_min * 100, "0000")
            //           !alt_wire_rod_cd = rsProcess!alt_wire_rod_cd & Format$(rsProcess!wire_size_min * 100, "0000")
            //            !rm_scrap_cd = "1"
            //            !rm_wt = Val(IIf(IsNull(rsProcess!rm_wt), 0, rsProcess!rm_wt))
            //            !CHEESE_WT = Val(rsProcess!CHEESE_WT)
            //            If Not IsNull(rsProcess!cheese_wt_est) Then !cheese_wt_est = Val(rsProcess!cheese_wt_est)
            //    Dim a As Double

            // If varUpdate = False Then
            //    If Not IsNull(!wire_rod_cd) And !wire_rod_cd <> "" Then
            //            a = Mid(!wire_rod_cd, 6) / 100
            //         WRS = InputBox("Enter the Minimum Rod Size", "Rod Size", a)
            //    Else
            //         WRS = InputBox("Enter the Minimum Rod Size", "Rod Size", IIf(IsNull(rsProcess!wire_size_min), 0, rsProcess!wire_size_min))
            //    End If
            // End If

            //'By TDK
            //'            If Not IsNull(rsProcess!wire_rod_cd) Then !wire_rod_cd = rsProcess!wire_rod_cd
            //'            If Not IsNull(rsProcess!alt_wire_rod_cd) Then !alt_wire_rod_cd = rsProcess!alt_wire_rod_cd

            //            If Not IsNull(rsProcess!wire_rod_cd) Then !wire_rod_cd = rsProcess!wire_rod_cd & Format$(WRS * 100, "0000")
            //            If Not IsNull(rsProcess!alt_wire_rod_cd) Then !ALT_RM_CD = rsProcess!ALT_RM_CD & Format$(WRS * 100, "0000")

            //'TDK
            //            !wire_size_min = IIf(IsNull(rsProcess!wire_size_min), 0, Val(rsProcess!wire_size_min))
            //            !wire_size_max = IIf(IsNull(rsProcess!wire_size_max), 0, Val(rsProcess!wire_size_max))
            //            !tko_cd = rsProcess!tko_cd
            //            !ajax_cd = rsProcess!ajax_cd
            //            !nos_per_pack = Val(rsProcess!nos_per_pack)
            //            !user_cd = gvarUserName
            //            !upd_dt_time = GetServerDate
            //            .UpdateBatch
            //        End With

            //'Update Product Drawing in OPSDEV database

            //    Set rsMaxProdDrawing = New ADODB.Recordset
            //        rsMaxProdDrawing.CursorLocation = adUseClient

            //    Sql = "select max(to_number(issue_no)) as DrwgIssueNo,max(issue_date)as DrwgIssueDate from prd_dwg_issue where part_no='" & varProductNo & "' and dwg_type=0"

            //        rsMaxProdDrawing.Open Sql, gvarcnn, adOpenForwardOnly, adLockReadOnly


            //    Set rsMaxSeqDrawing = New ADODB.Recordset
            //        rsMaxSeqDrawing.CursorLocation = adUseClient

            //    Sql = "select max(to_number(issue_no)) as DrwgSeqNo,max(issue_date) as DrwgSeqDate from prd_dwg_issue where part_no='" & varProductNo & "' and dwg_type=1"


            //        rsMaxSeqDrawing.Open Sql, gvarcnn, adOpenForwardOnly, adLockReadOnly

            //    Set rsCustDwg = New ADODB.Recordset
            //        rsCustDwg.CursorLocation = adUseClient

            //    Sql = "select cust_dwg_no,cust_dwg_no_issue,cust_std_date,cust_std_no,ci_ref from ddci_info a, prd_ciref b where a.ci_reference=b.ci_ref and b.part_no='" & varProductNo & "'"

            //        rsCustDwg.Open Sql, gvarcnn, adOpenForwardOnly, adLockBatchOptimistic


            //    Set rsMaxProcessIssue = New ADODB.Recordset
            //        rsMaxProcessIssue.CursorLocation = adUseClient

            //    Sql = "select max(to_number(issue_no))as  ProcessIssueNo,max(issue_date) as ProcessIssueDate from process_issue i,process_main m where  m.part_no=i.part_no and m.route_no=i.route_no and m.current_proc=1 and m.part_no='" & varProductNo & "'"  'and route_no=1"

            //        rsMaxProcessIssue.Open Sql, gvarcnn, adOpenForwardOnly, adLockReadOnly


            //    Set rsOPSDEVprdDrawing = New ADODB.Recordset
            //        rsOPSDEVprdDrawing.CursorLocation = adUseClient


            //     Sql = " select * from opsdev.product_drawing_mast where part_no='" & varProductNo & "'"

            //        rsOPSDEVprdDrawing.Open Sql, gvarcnn, adOpenDynamic, adLockBatchOptimistic


            //        With rsOPSDEVprdDrawing
            //            If .RecordCount = 0 Then
            //                .AddNew
            //                !Part_No = varProductNo
            //            End If
            //            If rsMaxProdDrawing.RecordCount <> 0 Then
            //                !part_drawing_issue_no = IIf(IsNull(rsMaxProdDrawing!drwgissueno), "", Val(rsMaxProdDrawing!drwgissueno))
            //                !part_drawing_issue_date = IIf(IsNull(rsMaxProdDrawing!drwgissuedate), Null, CDate(rsMaxProdDrawing!drwgissuedate))
            //            End If
            //            If rsMaxSeqDrawing.RecordCount <> 0 Then
            //                !part_drawing_seq_no = IIf(IsNull(rsMaxSeqDrawing!drwgseqno), "", Val(rsMaxSeqDrawing!drwgseqno))
            //                !part_drawing_seq_date = IIf(IsNull(rsMaxSeqDrawing!drwgseqdate), Null, CDate(rsMaxSeqDrawing!drwgseqdate))
            //            End If
            //            If rsMaxProcessIssue.RecordCount <> 0 Then
            //                !process_sheet_issue_no = IIf(IsNull(rsMaxProcessIssue!ProcessIssueno), "", Val(rsMaxProcessIssue!ProcessIssueno))
            //                If Not IsNull(rsMaxProcessIssue!ProcessIssueDate) Then
            //                    !process_sheet_issue_date = IIf(IsNull(rsMaxProcessIssue!ProcessIssueDate), Null, CDate(rsMaxProcessIssue!ProcessIssueDate))
            //                Else
            //                     Screen.MousePointer = 0
            //                    MsgBox "Process Issue Date should be Entered", vbInformation, "SmartPD"
            //                    Exit Sub
            //                End If

            //            End If
            //            If rsCustDwg.RecordCount <> 0 Then
            //                !cust_dwg_no = IIf(IsNull(rsCustDwg!cust_dwg_no), "", rsCustDwg!cust_dwg_no)
            //                !cust_dwg_no_issue = IIf(IsNull(rsCustDwg!cust_dwg_no_issue), "", rsCustDwg!cust_dwg_no_issue)
            //                !CUST_std_NO = IIf(IsNull(rsCustDwg!CUST_std_NO), "", rsCustDwg!CUST_std_NO)
            //                '!cust_std_date = (IIf(IsNull(rsCustDwg!cust_std_date), Null, CDate(rsCustDwg!cust_std_date)))
            //                !cust_std_date = rsCustDwg!cust_std_date
            //            End If
            //                !upd_dt_time = GetServerDate
            //                .UpdateBatch
            //        End With
            // If varUpdate = False Then
            //   MsgBox "Product Master for Order Processing has been updated.", vbInformation, "SmartPD"
            // End If
            //Set rsProcess = Nothing
            //Set rsOPSDEVProcess = Nothing
            //Set rsMaxSeqDrawing = Nothing
            //Set rsMaxProdDrawing = Nothing
            //Set rsMaxProcessIssue = Nothing
            //Set rsCustDwg = Nothing
            //Set rsOPSDEVprdDrawing = Nothing
            //varUpdate = False
            //Screen.MousePointer = 0

            //Exit Sub
            //ErrorTrap:

            //'
            //    MsgBox Err.Description, vbInformation, "SmartPD"


            //    MsgBox "Part No release was Incomplete. Please rectify data and Release Part No again", vbInformation, "SmartPD"
            //    Screen.MousePointer = 0


        }

        private readonly ICommand createCIReferenceCommand;
        public ICommand CreateCIReferenceClickCommand { get { return this.createCIReferenceCommand; } }
        private void createCIReferenceSubmitCommand()
        {
            if (!ActionPermission.CreateCIReference) return;

        }

        private readonly ICommand iSIRClickCommand;
        public ICommand ISIRClickCommand { get { return this.iSIRClickCommand; } }
        private void iSIRReport()
        {
            if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
            }
            else
            {
                ISIRModel im = new ISIRModel();
                im.PART_NO = ActiveEntity.PART_NO;
                frmISIR isir = new frmISIR(_userInformation, im);
                isir.ShowDialog();
            }
        }

        private PartSubmissionWarrantModel _partsubmissionwarrant;
        public PartSubmissionWarrantModel PARTSUBMISSIONWARRANT
        {
            get
            {
                return _partsubmissionwarrant;
            }
            set
            {
                _partsubmissionwarrant = value;
                NotifyPropertyChanged("PARTSUBMISSIONWARRANT");
            }
        }

        private readonly ICommand pswClickCommand;
        public ICommand PSWClickCommand { get { return this.pswClickCommand; } }
        private void pswReport()
        {
            if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
            }
            else
            {
                PartSubmissionWarrantModel pm = new PartSubmissionWarrantModel();
                pm.PART_NO = ActiveEntity.PART_NO;
                frmPartSubmissionWarrant psw = new frmPartSubmissionWarrant(_userInformation, pm);
                psw.ShowDialog();
            }
        }

        private readonly ICommand copyProductInformationCommand;
        public ICommand CopyProductInformationClickCommand { get { return this.copyProductInformationCommand; } }
        private void copyProductInformationSubmitCommand()
        {
            if (!ActionPermission.Copy) return;
            frmCopyStatus frm = new frmCopyStatus("ProductMaster", ActiveEntity.PART_NO, "", "", "", "");
            frm.ShowDialog();
            //Jeyan
            PartNumberDataSource = bll.GetEntityByPrimaryKey().ToDataTableWithType<PRD_MAST>().DefaultView;
            //PartNumberDataSource = GetProductMasterDetailsByPartNumberDV();
        }

        private bool _isReadonly = false;
        public bool IsReadonly
        {
            get { return _isReadonly; }
            set
            {
                this._isReadonly = value;
                NotifyPropertyChanged("IsReadonly");
            }
        }

        private readonly ICommand dimensionalClickCommand;
        public ICommand DimensionalClickCommand { get { return this.dimensionalClickCommand; } }
        private void dimensionalReport(string isBlank)
        {

            if (!ActiveEntity.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            else
            {
                DataTable processData;
                processData = _pccsBll.GetResults(ActiveEntity.PART_NO, "", 1, 70, "DR", isBlank);
                if (processData == null || processData.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {
                    frmReportViewer rv = new frmReportViewer(processData, "Dimensional", ActiveEntity.PART_DESC);
                    rv.ShowDialog();
                }
            }
        }

        private readonly ICommand materialClickCommand;
        public ICommand MaterialClickCommand { get { return this.materialClickCommand; } }
        private void materialReport(string isBlank)
        {
            if (!ActiveEntity.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            else
            {
                DataTable processData;
                processData = _pccsBll.GetResults(ActiveEntity.PART_NO, "", 71, 80, "MTR", isBlank);
                if (processData == null || processData.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {
                    frmReportViewer rv = new frmReportViewer(processData, "Material", ActiveEntity.PART_DESC);
                    rv.ShowDialog();
                }
            }
        }

        private readonly ICommand checklistClickCommand;
        public ICommand CheckListClickCommand { get { return this.checklistClickCommand; } }
        private void checklistReport()
        {
            if (!ActiveEntity.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            else
            {
                PartSubmissionWarrantModel pm = new PartSubmissionWarrantModel();
                pm.PART_NO = ActiveEntity.PART_NO;
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

        private readonly ICommand performanceClickCommand;
        public ICommand PerfomanceClickCommand { get { return this.performanceClickCommand; } }
        private void perfomanceReport()
        {
            if (!ActiveEntity.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            else
            {
                DataTable processData;
                processData = _pccsBll.GetResults(ActiveEntity.PART_NO, "", 81, 90, "PTR");
                if (processData == null || processData.Rows.Count == 0)
                {
                    ShowInformationMessage(PDMsg.NoRecordsPrint);
                }
                else
                {
                    frmReportViewer rv = new frmReportViewer(processData, "Perfomance", ActiveEntity.PART_DESC);
                    rv.ShowDialog();
                }
            }
        }

        private readonly ICommand initialSampleInspectionReportClickCommand;
        public ICommand InitialSampleInspectionReportClickCommand { get { return this.initialSampleInspectionReportClickCommand; } }
        private void initialReport()
        {
            if (!ActiveEntity.PART_NO.IsNotNullOrEmpty())
            {
                ShowInformationMessage(PDMsg.NotEmpty("Part No"));
                return;
            }
            else
            {
                DataTable processData;
                processData = bll.GetInitialInspection(ActiveEntity.PART_NO, 0);
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

        private readonly ICommand productSearchCommand = null;
        public ICommand ProductSearchCommand { get { return this.productSearchCommand; } }
        private void ProductSearch()
        {
            try
            {
                if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
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
                try
                {
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
                if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
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
                if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
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

        private readonly ICommand _processSheetCommand = null;
        public ICommand ProcessSheetCommand { get { return this._processSheetCommand; } }
        private void ProcessSheet()
        {
            try
            {
                if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild psheet = new MdiChild();
                psheet.Title = ApplicationTitle + " - Process Sheet";
                frmProcessSheet processsheet = null;
                if (MainMDI.IsFormAlreadyOpen("Process Sheet - " + MandatoryFields.PART_NO.Trim()) == false)
                {
                    processsheet = new frmProcessSheet(psheet, _userInformation, MandatoryFields.PART_NO, MandatoryFields.PART_DESC);
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
                    psheet = (MdiChild)MainMDI.GetFormAlreadyOpened("Process Sheet - " + MandatoryFields.PART_NO.Trim());
                    processsheet = (frmProcessSheet)psheet.Content;
                    MainMDI.SetMDI(psheet);
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand drawingsCommand = null;
        public ICommand DrawingsCommand { get { return this.drawingsCommand; } }
        private void Drawings()
        {
            try
            {
                if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild drwMaster = new MdiChild();
                drwMaster.Title = ApplicationTitle + " - Drawings";
                ProcessDesigner.frmDrawings drawings = null;
                if (MainMDI.IsFormAlreadyOpen("Drawings - " + MandatoryFields.PART_NO.Trim()) == false)
                {
                    drawings = new ProcessDesigner.frmDrawings(drwMaster, _userInformation, MandatoryFields.PART_NO);
                    drwMaster.Content = drawings;
                    drwMaster.Height = drawings.Height + 40;
                    drwMaster.Width = drawings.Width + 20;
                    drwMaster.MinimizeBox = false;
                    drwMaster.MaximizeBox = false;
                    drwMaster.Resizable = false;
                    if (MainMDI.IsFormAlreadyOpen("Drawings - " + MandatoryFields.PART_NO.Trim()) == false)
                    {
                        MainMDI.Container.Children.Add(drwMaster);
                    }
                    else
                    {
                        drwMaster = new MdiChild();
                        drwMaster = (MdiChild)MainMDI.GetFormAlreadyOpened("Drawings - " + MandatoryFields.PART_NO.Trim());
                        MainMDI.SetMDI(drwMaster);
                    }
                }
                else
                {
                    drwMaster = new MdiChild();
                    drwMaster = (MdiChild)MainMDI.GetFormAlreadyOpened("Drawings  - " + MandatoryFields.PART_NO.Trim());
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
                if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                showDummy();
                MdiChild tschedule = null;
                frmToolSchedule_new toolschedule = null;
                if (MainMDI.IsFormAlreadyOpen("Tool Schedule - " + MandatoryFields.PART_NO.Trim()) == false)
                {
                    tschedule = new MdiChild();
                    tschedule.Title = ApplicationTitle + " - Tool Schedule";
                    toolschedule = new frmToolSchedule_new(_userInformation, tschedule, MandatoryFields.PART_NO);
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
                    tschedule = (MdiChild)MainMDI.GetFormAlreadyOpened("Tool Schedule - " + MandatoryFields.PART_NO.Trim());
                    toolschedule = (frmToolSchedule_new)tschedule.Content;
                    MainMDI.SetMDI(tschedule);
                }
                toolschedule.EditSelectedPartNo(MandatoryFields.PART_NO);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }


        private readonly ICommand controlPlanCommand = null;
        public ICommand ControlPlanCommand { get { return this.controlPlanCommand; } }
        private void ControlPlan()
        {
            try
            {
                if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                //if (!ProcessSheet.ROUTE_NO.IsNotNullOrEmpty())
                //{
                //    MessageBox.Show(PDMsg.NotEmpty("Route No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}
                showDummy();
                MdiChild cplan = new MdiChild();
                cplan.Title = ApplicationTitle + " - Control Plan";

                frmPCCS cplanscreen = null;
                if (MainMDI.IsFormAlreadyOpen("Control Plan - " + MandatoryFields.PART_NO.Trim()) == false)
                {

                    cplanscreen = new frmPCCS(_userInformation, cplan, MandatoryFields.PART_NO);
                    cplan.Content = cplanscreen;
                    cplan.Height = cplanscreen.Height + 40;
                    cplan.Width = cplanscreen.Width + 20;
                    cplan.Resizable = false;
                    cplan.MinimizeBox = true;
                    cplan.MaximizeBox = true;
                    MainMDI.Container.Children.Add(cplan);
                }
                else
                {
                    cplan = new MdiChild();
                    cplan = (MdiChild)MainMDI.GetFormAlreadyOpened("Control Plan - " + MandatoryFields.PART_NO.Trim());
                    cplanscreen = (frmPCCS)cplan.Content;
                    MainMDI.SetMDI(cplan);
                }
                //frmPCCS pccs = new frmPCCS("ProcessSheet", MandatoryFields.PART_NO.Trim(), ProcessSheet.ROUTE_NO.ToString(), "", "", "");
                //pccs.ShowDialog();
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
                if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                showDummy();
                MdiChild devRptmdi = new MdiChild();
                devRptmdi.Title = ApplicationTitle + "Development Report";
                frmDevelopmentReport devReport = null;
                if (MainMDI.IsFormAlreadyOpen("Development Report - " + MandatoryFields.PART_NO.Trim()) == false)
                {
                    devReport = new frmDevelopmentReport(_userInformation, devRptmdi, MandatoryFields.PART_NO);
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
                    devRptmdi = (MdiChild)MainMDI.GetFormAlreadyOpened("Development Report -" + MandatoryFields.PART_NO.Trim());
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
                if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                showDummy();
                MdiChild mfgChild = new MdiChild();
                mfgChild.Title = ApplicationTitle + " - Manufacturing Report";
                frmManufacturingReport mfgReport = null;
                if (MainMDI.IsFormAlreadyOpen(" Manufacturing Report - " + MandatoryFields.PART_NO.Trim()) == false)
                {
                    mfgReport = new frmManufacturingReport(_userInformation, mfgChild, MandatoryFields.PART_NO);
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
                    mfgChild = (MdiChild)MainMDI.GetFormAlreadyOpened("Manufacturing Report -" + MandatoryFields.PART_NO.Trim());
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
                if (!MandatoryFields.PART_NO.IsNotNullOrEmpty())
                {
                    MessageBox.Show(PDMsg.NotEmpty("Part No"), ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }


                if (Prd_CIref_SelectedRow != null && Prd_CIref_SelectedRow["CIREF_NO_FK"].IsNotNullOrEmpty())
                {
                    showDummy();
                    int ci_info_pk = Prd_CIref_SelectedRow["CIREF_NO_FK"].ToString().ToIntValue();
                    MdiChild mdiCostSheet = new MdiChild();
                    ProcessDesigner.frmFRCS userControl = new ProcessDesigner.frmFRCS(_userInformation, mdiCostSheet, ci_info_pk, OperationMode.Edit);
                    mdiCostSheet.Title = ApplicationTitle + " - Cost Sheet Preparation";
                    mdiCostSheet.Content = userControl;
                    mdiCostSheet.Height = userControl.Height + 40;
                    mdiCostSheet.Width = userControl.Width + 20;
                    mdiCostSheet.MinimizeBox = true;
                    mdiCostSheet.MaximizeBox = true;
                    mdiCostSheet.Resizable = false;
                    MainMDI.Container.Children.Add(mdiCostSheet);
                }
                else
                {
                    ShowInformationMessage("CI Reference No should be selected");
                    return;
                }

            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool _defaultControlFocus = true;
        public bool DefaultControlFocus
        {
            get { return _defaultControlFocus; }
            set
            {
                _defaultControlFocus = value;
                NotifyPropertyChanged("DefaultControlFocus");
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

        private readonly ICommand onCheckChangeCommandCIRef;
        public ICommand OnCheckChangeCommandCIRef { get { return this.onCheckChangeCommandCIRef; } }
        private void CheckChangeCIRef()
        {
            Product_CIinfo.Table.AcceptChanges();
            if (Prd_CIref_SelectedRow == null) return;

            if (Prd_CIref_SelectedRow["CURRENT_CIREF"].ToBooleanAsString() && !Prd_CIref_SelectedRow["CI_REF"].IsNotNullOrEmpty())
            {
                Prd_CIref_SelectedRow["CURRENT_CIREF"] = false;
                return;
            }

            if (Prd_CIref_SelectedRow["CURRENT_CIREF"].ToBooleanAsString())
            {
                foreach (DataRowView drv in Product_CIinfo)
                {
                    if (drv["CI_REF"].ToString() != Prd_CIref_SelectedRow["CI_REF"].ToString()) drv["CURRENT_CIREF"] = false;
                }
            }
        }

        private bool IsDuplicateCIRef(string ciref)
        {
            DataView dv = Product_CIinfo.ToTable().DefaultView;
            dv.RowFilter = "CI_REF = '" + ciref + "'";

            if (dv.Count > 1)
            {
                return true;
            }
            return false;
        }

        //Jeyan
        private DataView GetProductMasterDetailsByPartNumberDV()
        {
            DataView dv = null;
            try
            {
                DataTable dt = new DataTable();

                System.Resources.ResourceManager myManager;
                myManager = new System.Resources.ResourceManager(typeof(ProcessDesigner.Properties.Resources));
                string conStr = myManager.GetString("ConnectionString");

                DataAccessLayer dal = new DataAccessLayer(conStr);
                dt = dal.Get_PartNo(0);
                if (dt != null)
                {
                    dv = dt.DefaultView;
                }
                else
                {
                    dv = null;
                }
            }
            catch (Exception ex)
            {
                ex.LogException();
            }

            return dv;
        }
    }
}
