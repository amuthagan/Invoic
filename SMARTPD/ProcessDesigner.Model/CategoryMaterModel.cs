using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessDesigner.Model
{
    public class CategoryMaterModel : ViewModelBase
    {
        private int _catecode;
        public int CateCode
        {
            get { return _catecode; }
            set
            {
                _catecode = value;
                NotifyPropertyChanged("CateCode");
            }
        }

        private string _category;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Category is Required")]
        public string Category
        {
            get { return _category; }
            set
            {
                _category = value;
                NotifyPropertyChanged("Category");
            }
        }

        private bool _active;
        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;
                NotifyPropertyChanged("Active");
            }
        }

        private bool _inactive;
        public bool InActive
        {
            get { return _inactive; }
            set
            {
                _inactive = value;
                NotifyPropertyChanged("InActive");
            }
        }

        private DataView _categoryview;
        public DataView CategoryView
        {
            get { return _categoryview; }
            set
            {
                this._categoryview = value;
                NotifyPropertyChanged("CategoryView");
            }
        }
    }
}
