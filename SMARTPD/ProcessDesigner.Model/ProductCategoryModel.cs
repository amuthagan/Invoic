using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace ProcessDesigner.Model
{
    public class ProductCategoryModel : ViewModelBase
    {
        private string _rowID;
        private string _proCode;
        private string _proCategory;
        private string _mode = "";
        private string _status = "";
        private DataView dvCategory = null;
        private DataView dvType = null;
        private DataView dvSubType = null;
        private DataView dvAllSubType = null;
        private DataView dvAllLinkSubType = null;
        private DataTable dtDeletedRecords = null;



        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Category is Required")]
        public string PRODUCT_CATEGORY
        {
            get { return _proCategory; }
            set
            {
                _proCategory = value;
                NotifyPropertyChanged("PRODUCT_CATEGORY");
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Code is Required")]
        public string PRD_CODE
        {
            get { return _proCode; }
            set
            {
                _proCode = value;
                NotifyPropertyChanged("PRD_CODE");
            }
        }

        public string ROWID
        {
            get { return _rowID; }
            set
            {
                _rowID = value;
                NotifyPropertyChanged("ROWID");
            }
        }

        public string Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                NotifyPropertyChanged("Mode");
            }
        }

        public DataView DVCategory
        {
            get { return dvCategory; }
            set
            {
                dvCategory = value;
                NotifyPropertyChanged("DVCategory");
            }
        }

        public DataView DVType
        {
            get { return dvType; }
            set
            {
                dvType = value;
                NotifyPropertyChanged("DVType");
            }
        }

        public DataView DVSubType
        {
            get { return dvSubType; }
            set
            {
                dvSubType = value;
                NotifyPropertyChanged("DVSubType");
            }
        }


        public DataView DVAllSubType
        {
            get { return dvAllSubType; }
            set
            {
                dvAllSubType = value;
                NotifyPropertyChanged("DVAllSubType");
            }
        }

        public DataView DVAllLinkSubType
        {
            get { return dvAllLinkSubType; }
            set
            {
                dvAllLinkSubType = value;
                NotifyPropertyChanged("DVAllLinkSubType");
            }
        }

        public DataTable DTDeletedRecords
        {
            get { return dtDeletedRecords; }
            set
            {
                dtDeletedRecords = value;
                NotifyPropertyChanged("DTDeletedRecords");
            }
        }

        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                NotifyPropertyChanged("Status");
            }
        }

        private DataRowView _typeselecteditem = null;
        public DataRowView TypeSelectedItem
        {
            get { return _typeselecteditem; }
            set
            {
                _typeselecteditem = value;
                NotifyPropertyChanged("TypeSelectedItem");
            }
        }

        private DataRowView _subtypeselecteditem = null;
        public DataRowView SubTypeSelectedItem
        {
            get { return _subtypeselecteditem; }
            set
            {
                _subtypeselecteditem = value;
                NotifyPropertyChanged("SubTypeSelectedItem");
            }
        }
    }
}
