using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using ProcessDesigner.Model;
using System.ComponentModel.DataAnnotations;

namespace ProcessDesigner.BLL
{
    public class CrossLinkingCharModel : ViewModelBase
    {
        private string _type = "";
        private string _productType = "";
        private string _linkedWith = "";
        [Required(AllowEmptyStrings = false, ErrorMessage = "Type is Required")]
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                NotifyPropertyChanged("Type");
            }
        }
        private string _featureCode = "";
        public string FeatureCode
        {
            get { return _featureCode; }
            set
            {
                _featureCode = value;
                NotifyPropertyChanged("FeatureCode");
            }
        }
        private string _featureName = "";
        public string FeatureName
        {
            get { return _featureName; }
            set
            {
                _featureName = value;
                NotifyPropertyChanged("FeatureName");
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Type is Required")]
        public string ProductType
        {
            get { return _productType; }
            set
            {
                _productType = value;
                NotifyPropertyChanged("ProductType");
            }
        }
        private string _productTypeH = "SUB TYPE";
        public string ProductTypeH
        {
            get { return _productType; }
            set
            {
                _productType = value;
                NotifyPropertyChanged("ProductTypeH");
            }
        }
        public string LinkedWith
        {
            get { return _linkedWith; }
            set
            {
                _linkedWith = value;
                NotifyPropertyChanged("LinkedWith");
            }
        }
        private string _linkedWithH = "SUB TYPE";
        public string LinkedWithH
        {
            get { return _linkedWithH; }
            set
            {
                _linkedWithH = value;
                NotifyPropertyChanged("LinkedWithH");
            }
        }
        private DataView _dtCmbType;
        public DataView DtCmbType
        {
            get { return _dtCmbType; }
            set
            {
                _dtCmbType = value;
                NotifyPropertyChanged("DtCmbType");
            }
        }
        private DataView _dtCmbPrdType;
        public DataView DtCmbPrdType
        {
            get { return _dtCmbPrdType; }
            set
            {
                _dtCmbPrdType = value;
                NotifyPropertyChanged("DtCmbPrdType");
            }
        }
        private DataView _dtCmbLinkedWith;
        public DataView DtCmbLinkedWith
        {
            get { return _dtCmbLinkedWith; }
            set
            {
                _dtCmbLinkedWith = value;
                NotifyPropertyChanged("DtCmbLinkedWith");
            }
        }

        private DataView _dtFeatureDetails;
        public DataView DtFeatureDetails
        {
            get { return _dtFeatureDetails; }
            set
            {
                _dtFeatureDetails = value;
                NotifyPropertyChanged("DtFeatureDetails");
            }
        }
        private DataView _dtCharacteristicsDetails;
        public DataView DtCharacteristicsDetails
        {
            get { return _dtCharacteristicsDetails; }
            set
            {
                _dtCharacteristicsDetails = value;
                NotifyPropertyChanged("DtCharacteristicsDetails");
            }
        }
        private DataView _dtClassificationPrdType;
        public DataView DtClassificationPrdType
        {
            get { return _dtClassificationPrdType; }
            set
            {
                _dtClassificationPrdType = value;
                NotifyPropertyChanged("DtClassificationPrdType");
            }
        }
        private DataView _dtClassificationCmbPrdType;
        public DataView DtClassificationCmbPrdType
        {
            get { return _dtClassificationCmbPrdType; }
            set
            {
                _dtClassificationCmbPrdType = value;
                NotifyPropertyChanged("DtClassificationCmbPrdType");
            }
        }

        private DataView _dtClassificationCmblinkedWith;
        public DataView DtClassificationCmblinkedWith
        {
            get { return _dtClassificationCmblinkedWith; }
            set
            {
                _dtClassificationCmblinkedWith = value;
                NotifyPropertyChanged("DtClassificationCmblinkedWith");
            }
        }
        private DataView _dtClassificationlinkedWith;
        public DataView DtClassificationlinkedWith
        {
            get { return _dtClassificationlinkedWith; }
            set
            {
                _dtClassificationlinkedWith = value;
                NotifyPropertyChanged("DtClassificationlinkedWith");
            }
        }
        private string _headerLinkedWith;
        public string HeaderLinkedWith
        {
            get { return _headerLinkedWith; }
            set
            {
                _headerLinkedWith = value;
                NotifyPropertyChanged("HeaderLinkedWith");
            }
        }
        private string _headerPrdType;
        public string HeaderPrdType
        {
            get { return _headerPrdType; }
            set
            {
                _headerPrdType = value;
                NotifyPropertyChanged("HeaderPrdType");
            }
        }
    }

}
