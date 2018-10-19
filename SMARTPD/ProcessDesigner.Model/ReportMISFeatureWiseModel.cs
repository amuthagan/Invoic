using ProcessDesigner.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class ReportMISFeatureWiseModel : ViewModelBase
    {

        private string _feature;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string FEATURE
        {
            get { return _feature; }
            set
            {
                _feature = value;
                NotifyPropertyChanged("FEATURE");
            }
        }

        private string _feature1;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string FEATURE1
        {
            get { return _feature1; }
            set
            {
                _feature1 = value;
                NotifyPropertyChanged("FEATURE1");
            }
        }

        private string _feature2;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string FEATURE2
        {
            get { return _feature2; }
            set
            {
                _feature2 = value;
                NotifyPropertyChanged("FEATURE2");
            }
        }

        private string _spec_min;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string SPEC_MIN
        {
            get { return _spec_min; }
            set
            {
                _spec_min = value;
                NotifyPropertyChanged("SPEC_MIN");
            }
        }

        private string _spec_max;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string SPEC_MAX
        {
            get { return _spec_max; }
            set
            {
                _spec_max = value;
                NotifyPropertyChanged("SPEC_MAX");
            }
        }

        private string _part_desc;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string PART_DESC
        {
            get { return _part_desc; }
            set
            {
                _part_desc = value;
                NotifyPropertyChanged("PART_DESC");
            }
        }

        private DataView _gridData;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public DataView GridData
        {
            get { return _gridData; }
            set
            {
                _gridData = value;
                NotifyPropertyChanged("GridData");
            }
        }

        private string _grid_title;
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Customer is Required")]
        public string GRID_TITLE
        {
            get { return _grid_title; }
            set
            {
                _grid_title = value;
                NotifyPropertyChanged("GRID_TITLE");
            }
        }

    }
}
