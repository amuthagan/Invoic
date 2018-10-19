using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace ProcessDesigner.Model
{
    public class FeatureMasterModel : ViewModelBase
    {

        private string _operationCode = "";
        private string _featureCode = "";
        private string _operations = "";
        private string _feature = "";
        private DataView _dtOperationCode;
        private DataView _dtFeatureCode;
        private DataView _dtCharacteristicsMaster;


        [Required(AllowEmptyStrings = false, ErrorMessage = "Operation Code is Required")]
        public string OperationCode
        {
            get { return _operationCode; }
            set
            {
                _operationCode = value;
                NotifyPropertyChanged("OperationCode");
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Feature Code is Required")]
        public string FeatureCode
        {
            get { return _featureCode; }
            set
            {
                _featureCode = value;
                NotifyPropertyChanged("FeatureCode");
            }
        }

        public string Operations
        {
            get { return _operations; }
            set
            {
                _operations = value;
                NotifyPropertyChanged("Operations");
            }
        }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Feature Description is Required")]
        public string Feature
        {
            get { return _feature; }
            set
            {
                _feature = value;
                NotifyPropertyChanged("Feature");
            }
        }

        public DataView OperationCodeDetails
        {
            get { return _dtOperationCode; }
            set
            {
                _dtOperationCode = value;
                NotifyPropertyChanged("OperationCodeDetails");
            }
        }
        public DataView FeatureCodeDetails
        {
            get { return _dtFeatureCode; }
            set
            {
                _dtFeatureCode = value;
                NotifyPropertyChanged("FeatureCodeDetails");
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
                NotifyPropertyChanged("SelectedRow");
            }
        }
        private DataRowView _selectedrowfeat;
        public DataRowView SelectedRowFeat
        {
            get
            {
                return _selectedrowfeat;
            }

            set
            {
                _selectedrowfeat = value;
                NotifyPropertyChanged("SelectedRowFeat");
            }
        }
        public DataView CharacteristicsMasterDetails
        {
            get { return _dtCharacteristicsMaster; }
            set
            {
                _dtCharacteristicsMaster = value;
                NotifyPropertyChanged("CharacteristicsMasterDetails");
            }
        }

    }
}
