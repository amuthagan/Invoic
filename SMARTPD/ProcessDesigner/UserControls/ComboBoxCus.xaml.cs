using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using ProcessDesigner.Common;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace ProcessDesigner.UserControls
{
    /// <summary>
    /// Interaction logic for ComboBoxCus.xaml
    /// </summary>
    public partial class ComboBoxCus : UserControl, INotifyPropertyChanged
    {
        private bool blnSetValue = false;
        public event EventHandler SelectedItemChanged;
        public event EventHandler TextBox_LostFocus;

        //public enum MaskType
        //{
        //    Any,
        //    Integer,
        //    Decimal

        //}

        public ComboBoxCus()
        {
            InitializeComponent();
        }

        private void LookUp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            blnSetValue = true;
            TogCombobox.IsChecked = false;
        }

        private string headerText = "LookUp Screen";
        public string HeaderText
        {
            get { return headerText; }
            set
            {
                headerText = value;
                NotifyPropertyChanged("HeaderText");
            }
        }

        private MaskType _mask;
        public MaskType Mask
        {
            get { return _mask; }
            set
            {
                _mask = value;
                TextBoxIntsOnly.SetMask(txtCombobox, value);
            }
        }

        public static DependencyProperty _columns = DependencyProperty.Register("ColumnsHeader", typeof(object), typeof(ComboBoxCus),
        new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public object ColumnsHeader
        {
            get { return (object)GetValue(_columns); }
            set
            {
                SetValue(_columns, value);
            }
        }

        public static DependencyProperty _MaxLength = DependencyProperty.Register("MaxLength", typeof(int), typeof(ComboBoxCus),
         new FrameworkPropertyMetadata(0) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public int MaxLength
        {
            get { return (int)GetValue(_MaxLength); }
            set
            {
                SetValue(_MaxLength, value);
                TextBoxIntsOnly.SetMaximumValue(txtCombobox, value);
            }
        }

        public static DependencyProperty _IsReadOnly = DependencyProperty.Register("IsReadOnly", typeof(Boolean), typeof(ComboBoxCus),
          new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public Boolean IsReadOnly
        {
            get { return (Boolean)GetValue(_IsReadOnly); }
            set
            {
                SetValue(_IsReadOnly, value);
            }
        }

        public static DependencyProperty _IsEditable = DependencyProperty.Register("IsEditable", typeof(Boolean), typeof(ComboBoxCus),
           new FrameworkPropertyMetadata(true) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public Boolean IsEditable
        {
            get { return (Boolean)GetValue(_IsEditable); }
            set
            {
                SetValue(_IsEditable, value);
            }
        }


        public static DependencyProperty _ButtonVisibility = DependencyProperty.Register("ButtonVisibility", typeof(Visibility), typeof(ComboBoxCus),
           new FrameworkPropertyMetadata(Visibility.Visible) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public Visibility ButtonVisibility
        {
            get { return (Visibility)GetValue(_ButtonVisibility); }
            set
            {
                SetValue(_ButtonVisibility, value);
            }
        }


        private String _selectedValuePath;
        public String SelectedValuePath
        {
            get { return _selectedValuePath; }
            set
            {
                _selectedValuePath = value;
            }
        }


        public static DependencyProperty _SelectedValue = DependencyProperty.Register("SelectedValue", typeof(string), typeof(ComboBoxCus),
           new FrameworkPropertyMetadata(String.Empty, TextChanged) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        public string SelectedValue
        {
            get { return (string)GetValue(_SelectedValue); }
            set { SetValue(_SelectedValue, value); }
        }

        public static DependencyProperty _SelectedItem = DependencyProperty.Register("SelectedItem", typeof(DataRow), typeof(ComboBoxCus),
          new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public DataRow SelectedItem
        {
            get { return (DataRow)GetValue(_SelectedItem); }
            set
            {
                SetValue(_SelectedItem, value);
            }
        }


        public static DependencyProperty _DataSource = DependencyProperty.Register("DataSource", typeof(DataView), typeof(ComboBoxCus),
           new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public DataView DataSource
        {
            get { return (DataView)GetValue(_DataSource); }
            set
            {
                SetValue(_DataSource, value);
            }
        }


        private void Popup_Closed(object sender, EventArgs e)
        {
            if (dgLookup.SelectedItem != null && blnSetValue)
            {
                DataRowView drv = (DataRowView)dgLookup.SelectedItem;
                SelectedItem = drv.Row;

                if (_selectedValuePath != null)
                {
                    SelectedValue = SelectedItem[_selectedValuePath].ToString();
                }

                if (this.SelectedItemChanged != null)
                    this.SelectedItemChanged(this, e);
            }

            txtCombobox.Focus();
        }

        protected void txtCombobox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.TextBox_LostFocus != null)
                this.TextBox_LostFocus(this, e);
        }

        private void Popup_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                blnSetValue = true;
                popContent.IsOpen = false;
            }
            else if (e.Key == Key.Escape)
            {
                blnSetValue = false;
                popContent.IsOpen = false;
            }
        }



        public void setDataSource()
        {

            string[] columnNames = (from dc in DataSource.ToTable().Columns.Cast<DataColumn>() select dc.ColumnName).ToArray();

            cmbSearchIn.ItemsSource = columnNames;

            if (columnNames.Length > 0)
            {
                cmbSearchIn.SelectedIndex = 0;
            }

            HeaderText = "LookUp Screen - " + DataSource.Count.ToString() + " Records";

            if (ColumnsHeader != null)
            {
                dgLookup.AutoGenerateColumns = false;
                ObservableCollection<DropdownColumns> dc = new ObservableCollection<DropdownColumns>();
                dc = (ObservableCollection<DropdownColumns>)ColumnsHeader;

                foreach (DropdownColumns item in dc)
                {

                    var column = new DataGridTextColumn
                    {
                        Header = item.ColumnDesc,
                        Binding = new Binding(item.ColumnName)
                    };

                    try
                    {
                        column.Width = int.Parse(item.ColumnWidth.ToString());
                    }
                    catch (Exception e)
                    {
                        column.Width = new DataGridLength(int.Parse(item.ColumnWidth.ToString().Replace("*","")), DataGridLengthUnitType.Star);
                    }
                    dgLookup.Columns.Add(column);
                }
            }
        }

        private void txtSearchValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //string dataType = _dataSource.Columns[cmbSearchIn.Text].DataType.Name.ToString();
                //if (dataType == "String")
                //{
                string sFilter = "Convert(" + cmbSearchIn.Text + ", 'System.String') LIKE ('%" + txtSearchValue.Text + "%')";
                DataSource.RowFilter = sFilter;
                //}

                if (DataSource != null)
                {
                    HeaderText = "LookUp Screen - " + DataSource.Count.ToString() + " Records";
                }
                else
                {
                    HeaderText = "LookUp Screen";
                }


            }
            catch (Exception ex)
            {

            }

        }

        private void cmbSearchIn_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                //string dataType = _dataSource.Columns[cmbSearchIn.Text].DataType.Name.ToString();
                //if (dataType == "String")
                //{
                string sFilter = "Convert(" + cmbSearchIn.Text + ", 'System.String') LIKE ('%" + txtSearchValue.Text + "%')";
                DataSource.RowFilter = sFilter;
                //}
                if (DataSource != null)
                {
                    HeaderText = "LookUp Screen - " + DataSource.Count.ToString() + " Records";
                }
                else
                {
                    HeaderText = "LookUp Screen";
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void dgLookup_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            blnSetValue = true;
            popContent.IsOpen = false;
        }

        private void popContent_Opened(object sender, EventArgs e)
        {
            if (DataSource != null)
            {
                setDataSource();

            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }



}


