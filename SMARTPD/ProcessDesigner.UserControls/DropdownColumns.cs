using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessDesigner.Model;
using System.Windows;

namespace ProcessDesigner.UserControls
{
    public class DropdownColumns : ViewModelBase
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

        private object _columnWidth = "1*";
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

        private TextAlignment _textAlign = TextAlignment.Left;
        public TextAlignment TextAlign
        {
            get
            {
                return _textAlign;
            }
            set
            {
                _textAlign = value;
                NotifyPropertyChanged("TextAlign");
            }
        }

        private Visibility _columnVisibility = Visibility.Visible;
        public Visibility ColumnVisibility
        {
            get
            {
                return _columnVisibility;
            }
            set
            {
                _columnVisibility = value;
                NotifyPropertyChanged("ColumnVisibility");
            }
        }

        private bool _showInDropdown = true;
        public bool ShowInDropdown
        {
            get
            {
                return _showInDropdown;
            }
            set
            {
                _showInDropdown = value;
                NotifyPropertyChanged("ShowInDropdown");
            }
        }

        private bool _isDefaultSearchColumn;
        public bool IsDefaultSearchColumn
        {
            get
            {
                return _isDefaultSearchColumn;
            }
            set
            {
                _isDefaultSearchColumn = value;
                NotifyPropertyChanged("IsDefaultSearchColumn");
            }
        }
        
    }
}
