using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.ViewModel;

namespace ProcessDesigner.Common
{
    class DropdownColumns : ViewModelBase
    {
        private string _columnName;
        public string ColumnName
        {
            get
            {
                return _columnName;
            }
            set
            {
                _columnName = value;
                NotifyPropertyChanged("ColumnName");
            }
        }

        private string _columnDesc;
        public string ColumnDesc
        {
            get
            {
                return _columnDesc;
            }
            set
            {
                _columnDesc = value;
                NotifyPropertyChanged("ColumnDesc");
            }
        }

        private object _columnWidth;
        public object ColumnWidth
        {
            get
            {
                return _columnWidth;
            }
            set
            {
                _columnWidth = value;
                NotifyPropertyChanged("ColumnWidth");
            }
        }
    }
}
