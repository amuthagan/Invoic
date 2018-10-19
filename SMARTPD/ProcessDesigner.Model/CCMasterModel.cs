using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace ProcessDesigner.Model
{

    public class CostCenterMaster : ViewModelBase
    {

        private string _costCentCode;
        private string _cateCode;
        private string _locCode;
        private string _module;
        private Nullable<double> _efficiency;
        private MemoryStream _photo;
        private string _costCentDesc;
        private string _machineName;
        private bool _photoChanged;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Cost centre code is Required")]
        public string CostCentCode
        {
            get { return _costCentCode; }
            set
            {
                _costCentCode = value;              
                NotifyPropertyChanged("CostCentCode");
            }
        }

        public string CateCode
        {
            get { return _cateCode; }
            set
            {
                _cateCode = value;
                NotifyPropertyChanged("CateCode");
            }
        }


        private string _category;
        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                NotifyPropertyChanged("Category");
            }
        }
        

        public string LocCode
        {
            get { return _locCode; }
            set
            {
                _locCode = value;
                NotifyPropertyChanged("LocCode");
            }
        }

        private string _locationName;
        public string LocationName
        {
            get { return _locationName; }
            set
            {
                _locationName = value;
                NotifyPropertyChanged("LocationName");
            }
        }



        public string Module
        {
            get { return _module; }
            set
            {
                _module = value;
                NotifyPropertyChanged("Module");
            }
        }

        private string _moduleName;
        public string ModuleName
        {
            get { return _moduleName; }
            set
            {
                _moduleName = value;
                NotifyPropertyChanged("ModuleName");
            }
        }


        public Nullable<double> Efficiency
        {
            get { return _efficiency; }
            set
            {
                _efficiency = value;
                NotifyPropertyChanged("Efficiency");
            }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Cost centre description is Required")]
        public string CostCentDesc
        {
            get { return _costCentDesc; }
            set
            {
                _costCentDesc = value;               
                NotifyPropertyChanged("CostCentDesc");
            }
        }


        public string MachineName
        {
            get { return _machineName; }
            set
            {
                _machineName = value;
                NotifyPropertyChanged("MachineName");
            }
        }

        public MemoryStream Photo
        {
            get { return _photo; }
            set
            {
                _photo = value;
                NotifyPropertyChanged("Photo");
            }
        }

        public bool PhotoChanged
        {
            get { return _photoChanged; }
            set
            {
                _photoChanged = value;
                NotifyPropertyChanged("PhotoChanged");
            }
        }

        private byte[] _imageByte;

        public Byte[] ImageByte
        {
            get { return _imageByte; }
            set
            {
                _imageByte = value;
                NotifyPropertyChanged("ImageByte");
            }
        }

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                NotifyPropertyChanged("FilePath");
            }
        }



        public DataTable Output { get; set; }
        public DataTable Operation { get; set; }
       
    }
}
