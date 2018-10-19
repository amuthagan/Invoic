using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace ProcessDesigner.Model
{
    public class ProductSearchModel : ViewModelBase
    {
        private string _heatTreatment = "";
        public string HeatTreatment
        {
            get { return _heatTreatment; }
            set
            {
                _heatTreatment = value;
                NotifyPropertyChanged("HeatTreatment");
            }
        }

        private string _quality = "";
        public string Quality
        {
            get { return _quality; }
            set
            {
                _quality = value;
                NotifyPropertyChanged("Quality");
            }
        }

        private string _finish = "";
        public string Finish
        {
            get { return _finish; }
            set
            {
                _finish = value;
                NotifyPropertyChanged("Finish");
            }
        }

        private string _rMSpec = "";
        public string RMSpec
        {
            get { return _rMSpec; }
            set
            {
                _rMSpec = value;
                NotifyPropertyChanged("RMSpec");
            }
        }

        private string _minRMSize = "";
        public string MinRMSize
        {
            get { return _minRMSize; }
            set
            {
                _minRMSize = value;
                NotifyPropertyChanged("MinRMSize");
            }
        }

        private string _maxRMSize = "";
        public string MaxRMSize
        {
            get { return _maxRMSize; }
            set
            {
                _maxRMSize = value;
                NotifyPropertyChanged("MaxRMSize");
            }
        }

        private string _manufacturedAt = "";
        public string ManufacturedAt
        {
            get { return _manufacturedAt; }
            set
            {
                _manufacturedAt = value;
                NotifyPropertyChanged("ManufacturedAt");
            }
        }

        private string _costCentre = "";
        public string CostCentre
        {
            get { return _costCentre; }
            set
            {
                _costCentre = value;
                NotifyPropertyChanged("CostCentre");
            }
        }

        private string _description = "";
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                NotifyPropertyChanged("Description");
            }
        }

        private string _threadSize = "";
        public string ThreadSize
        {
            get { return _threadSize; }
            set
            {
                _threadSize = value;
                NotifyPropertyChanged("ThreadSize");
            }
        }

        private string _threadClass = "";
        public string ThreadClass
        {
            get { return _threadClass; }
            set
            {
                _threadClass = value;
                NotifyPropertyChanged("ThreadClass");
            }
        }

        private string _threadStd = "";
        public string ThreadStd
        {
            get { return _threadStd; }
            set
            {
                _threadStd = value;
                NotifyPropertyChanged("ThreadStd");
            }
        }

        private string _customer = "";
        public string Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                NotifyPropertyChanged("Customer");
            }
        }

        private string _custDrwgNo = "";
        public string CustDrwgNo
        {
            get { return _custDrwgNo; }
            set
            {
                _custDrwgNo = value;
                NotifyPropertyChanged("CustDrwgNo");
            }
        }

        private string _family = "";
        public string Family
        {
            get { return _family; }
            set
            {
                _family = value;
                NotifyPropertyChanged("Family");
            }
        }

        private string _type = "";
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyPropertyChanged("Type");
            }
        }

        private string _headForm = "";
        public string HeadForm
        {
            get { return _headForm; }
            set
            {
                _headForm = value;
                NotifyPropertyChanged("HeadForm");
            }
        }

        private string _shankForm = "";
        public string ShankForm
        {
            get { return _shankForm; }
            set
            {
                _shankForm = value;
                NotifyPropertyChanged("ShankForm");
            }
        }

        private string _endForm = "";
        public string EndForm
        {
            get { return _endForm; }
            set
            {
                _endForm = value;
                NotifyPropertyChanged("EndForm");
            }
        }

        private string _drivingFeature = "";
        public string DrivingFeature
        {
            get { return _drivingFeature; }
            set
            {
                _drivingFeature = value;
                NotifyPropertyChanged("DrivingFeature");
            }
        }

        private string _additionalFeature = "";
        public string AdditionalFeature
        {
            get { return _additionalFeature; }
            set
            {
                _additionalFeature = value;
                NotifyPropertyChanged("AdditionalFeature");
            }
        }

        private string _keyword = "";
        public string Keyword
        {
            get { return _keyword; }
            set
            {
                _keyword = value;
                NotifyPropertyChanged("Keyword");
            }
        }

        private string _productFamily = "";
        public string ProductFamily
        {
            get { return _productFamily; }
            set
            {
                _productFamily = value;
                NotifyPropertyChanged("ProductFamily");
            }
        }

        private string _headStyle = "";
        public string HeadStyle
        {
            get { return _headStyle; }
            set
            {
                _headStyle = value;
                NotifyPropertyChanged("HeadStyle");
            }
        }

        private string _productType = "";
        public string ProductType
        {
            get { return _productType; }
            set
            {
                _productType = value;
                NotifyPropertyChanged("ProductType");
            }
        }

        private string _application = "";
        public string Application
        {
            get { return _application; }
            set
            {
                _application = value;
                NotifyPropertyChanged("Application");
            }
        }

        private string _toolCode = "";
        public string ToolCode
        {
            get { return _toolCode; }
            set
            {
                _toolCode = value;
                NotifyPropertyChanged("ToolCode");
            }
        }

        private string _chkToolCode = "";
        public string ChkToolCode
        {
            get { return _chkToolCode; }
            set
            {
                _chkToolCode = value;
                NotifyPropertyChanged("ChkToolCode");
            }
        }


        private string _toolDescription = "";
        public string ToolDescription
        {
            get { return _toolDescription; }
            set
            {
                _toolDescription = value;
                NotifyPropertyChanged("ToolDescription");
            }
        }

        private bool _specialParts = false;
        public bool SpecialParts
        {
            get { return _specialParts; }
            set
            {
                _specialParts = value;
                NotifyPropertyChanged("SpecialParts");
            }
        }

        private string _finishText = "";
        public string FinishText
        {
            get { return _finishText; }
            set
            {
                _finishText = value;
                NotifyPropertyChanged("FinishText");
            }
        }

        private string _rMSpecText = "";
        public string RMSpecText
        {
            get { return _rMSpecText; }
            set
            {
                _rMSpecText = value;
                NotifyPropertyChanged("RMSpecText");
            }
        }

        private string _manufacturedAtText = "";
        public string ManufacturedAtText
        {
            get { return _manufacturedAtText; }
            set
            {
                _manufacturedAtText = value;
                NotifyPropertyChanged("ManufacturedAtText");
            }
        }

        private string _costCentreText = "";
        public string CostCentreText
        {
            get { return _costCentreText; }
            set
            {
                _costCentreText = value;
                NotifyPropertyChanged("CostCentreText");
            }
        }

        private string _operationText = "";
        public string OperationText
        {
            get { return _operationText; }
            set
            {
                _operationText = value;
                NotifyPropertyChanged("OperationText");
            }
        }

        private string _customerText = "";
        public string CustomerText
        {
            get { return _customerText; }
            set
            {
                _customerText = value;
                NotifyPropertyChanged("CustomerText");
            }
        }

        private string _familyText = "";
        public string FamilyText
        {
            get { return _familyText; }
            set
            {
                _familyText = value;
                NotifyPropertyChanged("FamilyText");
            }
        }


        private string _familyTypeText = "";
        public string FamilyTypeText
        {
            get { return _familyTypeText; }
            set
            {
                _familyTypeText = value;
                NotifyPropertyChanged("FamilyTypeText");
            }
        }

        private string _headFormText = "";
        public string HeadFormText
        {
            get { return _headFormText; }
            set
            {
                _headFormText = value;
                NotifyPropertyChanged("HeadFormText");
            }
        }

        private string _shankFormText = "";
        public string ShankFormText
        {
            get { return _shankFormText; }
            set
            {
                _shankFormText = value;
                NotifyPropertyChanged("ShankFormText");
            }
        }

        private string _endFormText = "";
        public string EndFormText
        {
            get { return _endFormText; }
            set
            {
                _endFormText = value;
                NotifyPropertyChanged("EndFormText");
            }
        }


        private string _drivingFeatureText = "";
        public string DrivingFeatureText
        {
            get { return _drivingFeatureText; }
            set
            {
                _drivingFeatureText = value;
                NotifyPropertyChanged("DrivingFeatureText");
            }
        }

        private string _additionalFeatureText = "";
        public string AdditionalFeatureText
        {
            get { return _additionalFeatureText; }
            set
            {
                _additionalFeatureText = value;
                NotifyPropertyChanged("AdditionalFeatureText");
            }
        }

        private string _keywordText = "";
        public string KeywordText
        {
            get { return _keywordText; }
            set
            {
                _keywordText = value;
                NotifyPropertyChanged("KeywordText");
            }
        }

        private string _productFamilyText = "";
        public string ProductFamilyText
        {
            get { return _productFamilyText; }
            set
            {
                _productFamilyText = value;
                try
                {
                    if (_productFamilyText.Trim() == "" || _headStyleText.Trim() == "" || _productTypeText.Trim() == "")
                    {
                        Application = "";
                        ApplicationText = "";
                    }
                }
                catch (Exception ex)
                {

                }
                NotifyPropertyChanged("ProductFamilyText");
            }
        }

        private string _headStyleText = "";
        public string HeadStyleText
        {
            get { return _headStyleText; }
            set
            {
                _headStyleText = value;
                try
                {
                    if (_productFamilyText.Trim() == "" || _headStyleText.Trim() == "" || _productTypeText.Trim() == "")
                    {
                        Application = "";
                        ApplicationText = "";
                    }
                }
                catch (Exception ex)
                {

                }
                NotifyPropertyChanged("HeadStyleText");
            }
        }

        private string _productTypeText = "";
        public string ProductTypeText
        {
            get { return _productTypeText; }
            set
            {
                _productTypeText = value;
                try
                {
                    if (_productFamilyText.Trim() == "" || _headStyleText.Trim() == "" || _productTypeText.Trim() == "")
                    {
                        Application = "";
                        ApplicationText = "";
                    }
                }
                catch (Exception ex)
                {

                }
                NotifyPropertyChanged("ProductTypeText");
            }
        }

        private string _applicationText = "";
        public string ApplicationText
        {
            get { return _applicationText; }
            set
            {
                _applicationText = value;
                NotifyPropertyChanged("ApplicationText");
            }
        }


        private DataView _rMSpecCombo = null;
        public DataView RMSpecCombo
        {
            get { return _rMSpecCombo; }
            set
            {
                _rMSpecCombo = value;
                NotifyPropertyChanged("RMSpecCombo");
            }
        }

        private DataView _finishCombo;
        public DataView FinishCombo
        {
            get { return _finishCombo; }
            set
            {
                _finishCombo = value;
                NotifyPropertyChanged("FinishCombo");
            }
        }

        private DataView _manufacturedAtCombo = null;
        public DataView ManufacturedAtCombo
        {
            get { return _manufacturedAtCombo; }
            set
            {
                _manufacturedAtCombo = value;
                NotifyPropertyChanged("ManufacturedAtCombo");
            }
        }

        private DataView _costCentreCombo = null;
        public DataView CostCentreCombo
        {
            get { return _costCentreCombo; }
            set
            {
                _costCentreCombo = value;
                NotifyPropertyChanged("ManufacturedAtCombo");
            }
        }


        private string _operation = "";
        public string Operation
        {
            get { return _operation; }
            set
            {
                _operation = value;
                NotifyPropertyChanged("Operation");
            }
        }


        private DataView _operationCombo = null;
        public DataView OperationCombo
        {
            get { return _operationCombo; }
            set
            {
                _operationCombo = value;
                NotifyPropertyChanged("OperationCombo");
            }
        }


        private DataView _customerCombo = null;
        public DataView CustomerCombo
        {
            get { return _customerCombo; }
            set
            {
                _customerCombo = value;
                NotifyPropertyChanged("CustomerCombo");
            }
        }

        private DataView _familyCombo = null;
        public DataView FamilyCombo
        {
            get { return _familyCombo; }
            set
            {
                _familyCombo = value;
                NotifyPropertyChanged("FamilyCombo");
            }
        }

        private DataView _familyTypeCombo = null;
        public DataView FamilyTypeCombo
        {
            get { return _familyTypeCombo; }
            set
            {
                _familyTypeCombo = value;
                NotifyPropertyChanged("FamilyTypeCombo");
            }
        }

        private string _familyType = null;
        public string FamilyType
        {
            get { return _familyType; }
            set
            {
                _familyType = value;
                NotifyPropertyChanged("FamilyType");
            }
        }

        private DataView _headFormCombo = null;
        public DataView HeadFormCombo
        {
            get { return _headFormCombo; }
            set
            {
                _headFormCombo = value;
                NotifyPropertyChanged("HeadFormCombo");
            }
        }

        private DataView _shankFormCombo = null;
        public DataView ShankFormCombo
        {
            get { return _shankFormCombo; }
            set
            {
                _shankFormCombo = value;
                NotifyPropertyChanged("ShankFormCombo");
            }
        }

        private DataView _endFormCombo = null;
        public DataView EndFormCombo
        {
            get { return _endFormCombo; }
            set
            {
                _endFormCombo = value;
                NotifyPropertyChanged("EndFormCombo");
            }
        }

        private DataView _drivingFeatureCombo = null;
        public DataView DrivingFeatureCombo
        {
            get { return _drivingFeatureCombo; }
            set
            {
                _drivingFeatureCombo = value;
                NotifyPropertyChanged("DrivingFeatureCombo");
            }
        }

        private DataView _additionalFeatureCombo = null;
        public DataView AdditionalFeatureCombo
        {
            get { return _additionalFeatureCombo; }
            set
            {
                _additionalFeatureCombo = value;
                NotifyPropertyChanged("AdditionalFeatureCombo");
            }
        }

        private DataView _keywordsCombo = null;
        public DataView KeywordsCombo
        {
            get { return _keywordsCombo; }
            set
            {
                _keywordsCombo = value;
                NotifyPropertyChanged("KeywordsCombo");
            }
        }

        private DataView _productFamilyCombo = null;
        public DataView ProductFamilyCombo
        {
            get { return _productFamilyCombo; }
            set
            {
                _productFamilyCombo = value;
                NotifyPropertyChanged("ProductFamilyCombo");
            }
        }

        private DataView _headStyleCombo = null;
        public DataView HeadStyleCombo
        {
            get { return _headStyleCombo; }
            set
            {
                _headStyleCombo = value;
                NotifyPropertyChanged("HeadStyleCombo");
            }
        }

        private DataView _productTypeCombo = null;
        public DataView ProductTypeCombo
        {
            get { return _productTypeCombo; }
            set
            {
                _productTypeCombo = value;
                NotifyPropertyChanged("ProductTypeCombo");
            }
        }

        private DataView _applicationCombo = null;
        public DataView ApplicationCombo
        {
            get { return _applicationCombo; }
            set
            {
                _applicationCombo = value;
                NotifyPropertyChanged("ApplicationCombo");
            }
        }

        private string _totalRecords;
        public string TotalRecords
        {
            get { return _totalRecords; }
            set
            {
                _totalRecords = value;
                NotifyPropertyChanged("TotalRecords");
            }
        }

        private bool _printEnabled = false;
        public bool PrintEnabled
        {
            get { return _printEnabled; }
            set
            {
                _printEnabled = value;
                NotifyPropertyChanged("PrintEnabled");
            }
        }


    }
}
