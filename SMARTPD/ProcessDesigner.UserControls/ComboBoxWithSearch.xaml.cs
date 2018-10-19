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
using System.ComponentModel;
using System.Collections.ObjectModel;
using ProcessDesigner.Common;
using System.Text.RegularExpressions;


namespace ProcessDesigner.UserControls
{
    /// <summary>
    /// Interaction logic for ComboBoxWithSearch.xaml
    /// </summary>
    public partial class ComboBoxWithSearch : UserControl, INotifyPropertyChanged
    {
        private bool blnSetValue = false;
        public event EventHandler SelectedItemChanged;
        public event EventHandler SelectedItemChanging;
        public event RoutedEventHandler TextBox_LostFocus;
        public event EventHandler TextBox_MouseDoubleClick;
        public event RoutedEventHandler DropdownPreviewKeyDown;
        public event EventHandler EnterKeyPressed;
        public event EventHandler DropDownOpened;
        private string searchText = "";


        public ComboBoxWithSearch()
        {
            InitializeComponent();

        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            // Don't allow focus to leave the textbox if the popup is open            
            if (popContent.IsOpen && txtSearchValue.IsFocused) e.Handled = true;
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

        public static DependencyProperty _IsColumnsSorting = DependencyProperty.Register("IsColumnsSorting", typeof(Boolean), typeof(ComboBoxWithSearch),
       new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public Boolean IsColumnsSorting
        {
            get { return (Boolean)GetValue(_IsColumnsSorting); }
            set
            {
                SetValue(_IsColumnsSorting, value);
            }
        }
        public static DependencyProperty _VisibilityErrorTemp = DependencyProperty.Register("VisibilityErrorTemp", typeof(Visibility), typeof(ComboBoxWithSearch),
           new FrameworkPropertyMetadata(Visibility.Collapsed) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public Visibility VisibilityErrorTemp
        {
            get { return (Visibility)GetValue(_VisibilityErrorTemp); }
            set
            {
                SetValue(_VisibilityErrorTemp, value);
            }
        }
        public static DependencyProperty _CharacterCasingText = DependencyProperty.Register("CharacterCasingText", typeof(CharacterCasing), typeof(ComboBoxWithSearch),
        new FrameworkPropertyMetadata(CharacterCasing.Normal) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public CharacterCasing CharacterCasingText
        {
            get { return (CharacterCasing)GetValue(_CharacterCasingText); }
            set
            {
                SetValue(_CharacterCasingText, value);
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

        public Boolean IsDropDownOpen
        {
            get { return popContent.IsOpen; }
            set { popContent.IsOpen = value; }
        }

        public static DependencyProperty _COLUMNS = DependencyProperty.Register("ColumnsHeader", typeof(object), typeof(ComboBoxWithSearch),
        new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public object ColumnsHeader
        {
            get { return (object)GetValue(_COLUMNS); }
            set
            {
                SetValue(_COLUMNS, value);
            }
        }

        public static DependencyProperty _MaxLength = DependencyProperty.Register("MaxLength", typeof(int), typeof(ComboBoxWithSearch),
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

        public static DependencyProperty _IsReadOnly = DependencyProperty.Register("IsReadOnly", typeof(Boolean), typeof(ComboBoxWithSearch),
          new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public Boolean IsReadOnly
        {
            get { return (Boolean)GetValue(_IsReadOnly); }
            set
            {
                SetValue(_IsReadOnly, value);
            }
        }

        public static DependencyProperty _IsEditable = DependencyProperty.Register("IsEditable", typeof(Boolean), typeof(ComboBoxWithSearch),
           new FrameworkPropertyMetadata(true) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public Boolean IsEditable
        {
            get { return (Boolean)GetValue(_IsEditable); }
            set
            {
                SetValue(_IsEditable, value);
            }
        }


        public static DependencyProperty _ButtonVisibility = DependencyProperty.Register("ButtonVisibility", typeof(Visibility), typeof(ComboBoxWithSearch),
           new FrameworkPropertyMetadata(Visibility.Visible) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public Visibility ButtonVisibility
        {
            get { return (Visibility)GetValue(_ButtonVisibility); }
            set
            {
                SetValue(_ButtonVisibility, value);
            }
        }

        public static DependencyProperty _DropDownWidth = DependencyProperty.Register("DropDownWidth", typeof(int), typeof(ComboBoxWithSearch),
         new FrameworkPropertyMetadata(400) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public int DropDownWidth
        {
            get { return (int)GetValue(_DropDownWidth); }
            set
            {
                SetValue(_DropDownWidth, value);
            }
        }
        public static DependencyProperty _DropDownHeight = DependencyProperty.Register("DropDownHeight", typeof(int), typeof(ComboBoxWithSearch),
         new FrameworkPropertyMetadata(260) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public int DropDownHeight
        {
            get { return (int)GetValue(_DropDownHeight); }
            set
            {
                SetValue(_DropDownHeight, value);
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

        private String _displayValuePath;
        public String DisplayValuePath
        {
            get { return _displayValuePath; }
            set
            {
                _displayValuePath = value;
            }
        }


        public static DependencyProperty _SelectedValue = DependencyProperty.Register("SelectedValue", typeof(string), typeof(ComboBoxWithSearch),
           new FrameworkPropertyMetadata(String.Empty, TextChanged) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public static DependencyProperty _SelectedText = DependencyProperty.Register("SelectedText", typeof(string), typeof(ComboBoxWithSearch),
        new FrameworkPropertyMetadata(String.Empty, TextChanged) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });



        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }
        public string SelectedValue
        {
            get { return (string)GetValue(_SelectedValue); }
            set { SetValue(_SelectedValue, value); }
        }

        public string SelectedText
        {
            get { return (string)GetValue(_SelectedText); }
            set { SetValue(_SelectedText, value); }
        }



        public static DependencyProperty _SelectedItem = DependencyProperty.Register("SelectedItem", typeof(DataRowView), typeof(ComboBoxWithSearch),
          new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public DataRowView SelectedItem
        {
            get { return (DataRowView)GetValue(_SelectedItem); }
            set
            {
                SetValue(_SelectedItem, value);
            }
        }


        public static DependencyProperty _DataSource = DependencyProperty.Register("DataSource", typeof(DataView), typeof(ComboBoxWithSearch),
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
                //DataRowView drv = (DataRowView)dgLookup.SelectedItem;
                //SelectedItem = drv.Row;

                if (_selectedValuePath.IsNotNullOrEmpty() && _displayValuePath.IsNotNullOrEmpty())
                {
                    SelectedValue = SelectedItem[_selectedValuePath].ToString();
                    SelectedText = SelectedItem[_displayValuePath].ToString();
                }
                else if (_selectedValuePath.IsNotNullOrEmpty() && !_displayValuePath.IsNotNullOrEmpty())
                {
                    SelectedValue = SelectedItem[_selectedValuePath].ToString();
                    SelectedText = SelectedItem[_selectedValuePath].ToString();
                }

                blnSetValue = false;

                if (this.SelectedItemChanged != null)
                    this.SelectedItemChanged(this, e);
            }
            else if (blnSetValue == false)
            {
                dgLookup.SelectedItem = null;
            }

            txtCombobox.Focus();
        }

        protected void txtCombobox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.TextBox_LostFocus != null)
                this.TextBox_LostFocus(this, e);
        }


        public static DependencyProperty _FirstAllowZero = DependencyProperty.Register("FirstAllowZero", typeof(Boolean), typeof(ComboBoxWithSearch),
        new FrameworkPropertyMetadata(true) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });



        public Boolean FirstAllowZero
        {
            get { return (Boolean)GetValue(_FirstAllowZero); }
            set
            {
                SetValue(_FirstAllowZero, value);
            }
        }

        protected void txtCombobox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox _this = (sender as TextBox);

            if (FirstAllowZero == false)
            {
                if (e.Text == "0")
                {
                    if (_this.Text.ToString().Length == 0 || _this.SelectionStart == 0)
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }
        }

        private void Popup_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                blnSetValue = true;
                popContent.IsOpen = false;
            }
        }

        private Style textStyle = null;

        public void setDataSource()
        {

            HeaderText = "LookUp Screen - " + DataSource.Count.ToString() + " Entries";

            if (ColumnsHeader != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("DataValue"));
                dt.Columns.Add(new DataColumn("DisplayValue"));

                string defaultSearchColumn = "";
                dgLookup.Columns.Clear();
                dgLookup.AutoGenerateColumns = false;
                ObservableCollection<DropdownColumns> dc = new ObservableCollection<DropdownColumns>();
                dc = (ObservableCollection<DropdownColumns>)ColumnsHeader;

                foreach (DropdownColumns item in dc)
                {
                    textStyle = new Style();
                    textStyle.TargetType = typeof(TextBlock);
                    textStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, item.TextAlign));
                    textStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));

                    if (item.IsDefaultSearchColumn)
                    {
                        defaultSearchColumn = item.ColumnName;
                    }

                    var column = new DataGridTextColumn
                    {
                        Header = item.ColumnDesc,
                        Binding = new Binding(item.ColumnName),
                        Visibility = item.ColumnVisibility,
                        IsReadOnly = true,
                        ElementStyle = textStyle
                    };

                    try
                    {
                        column.Width = double.Parse(item.ColumnWidth.ToString());
                    }
                    catch (Exception)
                    {
                        column.Width = new DataGridLength(double.Parse(item.ColumnWidth.ToString().Replace("*", "")), DataGridLengthUnitType.Star);
                    }
                    dgLookup.Columns.Add(column);
                    dt.Rows.Add(item.ColumnName, item.ColumnDesc);

                }
                cmbSearchIn.SelectedValuePath = "DataValue";
                cmbSearchIn.DisplayMemberPath = "DisplayValue";
                cmbSearchIn.ItemsSource = dt.DefaultView;

                if (defaultSearchColumn != "")
                {
                    cmbSearchIn.SelectedValue = defaultSearchColumn;
                }
                else
                {
                    cmbSearchIn.SelectedIndex = 0;
                }
            }
            else
            {
                string[] columnNames = (from dc in DataSource.ToTable().Columns.Cast<DataColumn>() select dc.ColumnName).ToArray();

                cmbSearchIn.ItemsSource = columnNames;

                if (columnNames.Length > 0)
                {
                    cmbSearchIn.SelectedIndex = 0;
                }
            }
            if (DataSource.Count > 0)
            {
                DataSource.Sort = (cmbSearchIn.SelectedValue.IsNotNullOrEmpty()) ? cmbSearchIn.SelectedValue.ToString() : cmbSearchIn.Text;
            }
            dgLookup.SelectedIndex = 0;
            WaterMark.Text = cmbSearchIn.Text;

        }

        private void txtSearchValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                if (cmbSearchIn.SelectedValue == null)
                {
                    searchText = cmbSearchIn.Text;
                }
                else
                {
                    searchText = cmbSearchIn.SelectedValue.ToString();
                }

                if (DataSource != null)
                {
                    SelectedItem = null;
                    if (txtSearchValue.Text.IsNotNullOrEmpty())
                    {
                        foreach (DataRowView row in DataSource)
                        {
                            if (row[searchText].ToString().ToUpper().StartsWith(txtSearchValue.Text.ToUpper()))
                            {
                                SelectedItem = row;
                                break;
                            }
                        }
                    }

                    if (SelectedItem == null && DataSource.Count > 0) SelectedItem = DataSource[0];
                    HeaderText = "LookUp Screen - " + DataSource.Count.ToString() + " Entries";
                }
                else
                {
                    HeaderText = "LookUp Screen";
                }


                //if (dgLookup.SelectedItem.IsNotNullOrEmpty())
                //    dgLookup.ScrollIntoView(dgLookup.SelectedItem);
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }

        }

        private void cmbSearchIn_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (cmbSearchIn.SelectedValue == null)
                {
                    searchText = cmbSearchIn.Text;
                }
                else
                {
                    searchText = cmbSearchIn.SelectedValue.ToString();
                }

                if (DataSource != null)
                {
                    string sFilter = "Convert(" + searchText + ", 'System.String') LIKE ('" + txtSearchValue.Text + "%')";
                    DataSource.RowFilter = sFilter;
                    DataSource.Sort = (cmbSearchIn.SelectedValue.IsNotNullOrEmpty()) ? cmbSearchIn.SelectedValue.ToString() : cmbSearchIn.Text;
                    HeaderText = "LookUp Screen - " + DataSource.Count.ToString() + " Entries";
                }
                else
                {
                    HeaderText = "LookUp Screen";
                }

                dgLookup.SelectedIndex = 0;

            }
            catch (Exception ex)
            {
                throw ex.LogException();
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
                DataSource.RowFilter = "";
                setDataSource();
                txtSearchValue.Text = "";
                txtSearchValue.Focus();
                SortDataSource();
                dgLookup.SelectedIndex = 0;
                //if (dgLookup.SelectedItem.IsNotNullOrEmpty())
                //    dgLookup.ScrollIntoView(dgLookup.SelectedItem);
            }

            if (this.DropDownOpened != null)
                this.DropDownOpened(this, e);
        }

        private void SortDataSource()
        {
            try
            {
                if (DataSource.Count > 0)
                {
                    if (DataSource != null)
                    {
                        string sortcoulmname = SelectedValuePath;
                        DataTable dt = DataSource.Table;
                        if (!dt.Columns.Contains("SORT_COLUMN"))
                        {
                            dt.Columns.Add(new DataColumn("SORT_COLUMN", typeof(Double)));
                            dt.Columns.Add(new DataColumn("SORT_COLUMN1"));
                        }
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (IsNumeric(dr[sortcoulmname].ToString()))
                            {
                                dr["SORT_COLUMN"] = dr[sortcoulmname];
                            }
                            else
                            {
                                dr["SORT_COLUMN"] = 9999999999999999999999999.00;
                                dr["SORT_COLUMN1"] = dr[sortcoulmname];
                            }
                        }
                        DataSource.Sort = "SORT_COLUMN, SORT_COLUMN1";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex.LogException();
            }
        }

        private bool IsNumeric(string s)
        {
            float output;
            return float.TryParse(s, out output);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ucComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            string c = e.Key.ToString();
            // if (e.Key == Key.F4 && ButtonVisibility == Visibility.Visible)
            if (ButtonVisibility == Visibility.Visible && IsReadOnly == true)
            {
                Match match = Regex.Match(c, @"[A-Za-z]");
                if (Keyboard.Modifiers != ModifierKeys.Control && c.Length == 1 && match.Length == 1 && popContent.IsOpen == false)
                {
                    popContent.IsOpen = true;
                    return;
                }

                if (!txtSearchValue.IsFocused)
                    txtSearchValue.Focus();

                switch (e.Key)
                {
                    case Key.F4:
                    case Key.D0:
                    case Key.D1:
                    case Key.D2:
                    case Key.D3:
                    case Key.D4:
                    case Key.D5:
                    case Key.D6:
                    case Key.D7:
                    case Key.D8:
                    case Key.D9:
                    case Key.NumPad0:
                    case Key.NumPad1:
                    case Key.NumPad2:
                    case Key.NumPad3:
                    case Key.NumPad4:
                    case Key.NumPad5:
                    case Key.NumPad6:
                    case Key.NumPad7:
                    case Key.NumPad8:
                    case Key.NumPad9:
                        popContent.IsOpen = true;
                        break;
                    case Key.Delete:
                        if (!popContent.IsOpen)
                        {
                            blnSetValue = false;
                            SelectedItem = null;
                            SelectedValue = "";
                            SelectedText = "";
                            if (this.SelectedItemChanged != null)
                                this.SelectedItemChanged(this, e);
                        }
                        break;

                }
            }
            else if (e.Key == Key.Escape)
            {
                blnSetValue = false;
                popContent.IsOpen = false;
            }
            else if (e.Key == Key.Enter)
            {
                try
                {
                    if (!popContent.IsOpen)
                    {
                        SelectedItem = null;
                        if (DataSource != null)
                        {
                            string sFilter = "Convert(" + SelectedValuePath + ", 'System.String') = '" + txtCombobox.Text + "'";
                            DataSource.RowFilter = sFilter;
                            if (DataSource.Count > 0)
                            {
                                SelectedItem = DataSource[0];
                                if (_selectedValuePath != null)
                                {
                                    SelectedValue = SelectedItem[_selectedValuePath].ToString();
                                    SelectedText = SelectedItem[_displayValuePath].ToString();
                                }
                            }
                            DataSource.RowFilter = "";
                        }
                    }
                    else
                    {
                        dgLookup.SelectedItem = SelectedItem;
                        if (SelectedItem != null && _selectedValuePath != null)
                        {
                            SelectedValue = SelectedItem[_selectedValuePath].ToString();
                            SelectedText = SelectedItem[_displayValuePath].ToString();
                        }
                        //if (dgLookup.SelectedItem.IsNotNullOrEmpty())
                        //    dgLookup.ScrollIntoView(dgLookup.SelectedItem);
                    }
                }
                catch (Exception ex)
                {
                    ex.LogException();
                }
                if (this.EnterKeyPressed != null)
                    this.EnterKeyPressed(this, e);

            }

            if (popContent.IsOpen && DataSource != null && DataSource.Count > 0)
            {
                switch (e.Key)
                {
                    case Key.Up:
                        moveUp();
                        break;
                    case Key.Down:
                        moveDown();
                        break;
                    case Key.PageUp:
                        movePageUp();
                        break;
                    case Key.PageDown:
                        movePageDown();
                        break;
                    case Key.Home:
                        dgLookup.SelectedIndex = 0;
                        break;
                    case Key.End:
                        dgLookup.SelectedIndex = dgLookup.Items.Count - 1;
                        break;
                }
            }

            switch (e.Key)
            {
                case Key.Escape:
                    blnSetValue = false;
                    popContent.IsOpen = false;
                    break;
            }

            if (this.DropdownPreviewKeyDown != null)
                this.DropdownPreviewKeyDown(this, e);
        }

        private void moveUp()
        {
            if (cmbSearchIn.IsDropDownOpen == false)
            {
                if (dgLookup.ItemsSource != null && dgLookup.Items.Count > 0)
                {
                    if (dgLookup.Items.Count > 0)
                    {
                        int rowCount = dgLookup.Items.Count;
                        int index = dgLookup.SelectedIndex;

                        if (index == 0)
                        {
                            return;
                        }

                        dgLookup.SelectedIndex = index - 1;
                        //if (dgLookup.SelectedItem.IsNotNullOrEmpty())
                        //    dgLookup.ScrollIntoView(dgLookup.SelectedItem);

                    }
                }
            }
        }

        private void moveDown()
        {
            if (cmbSearchIn.IsDropDownOpen == false)
            {
                if (dgLookup.ItemsSource != null && dgLookup.Items.Count > 0)
                {
                    if (dgLookup.Items.Count > 0)
                    {
                        int rowCount = dgLookup.Items.Count;
                        int index = dgLookup.SelectedIndex;

                        if (index == dgLookup.Items.Count - 1)
                        {
                            return;
                        }

                        dgLookup.SelectedIndex = index + 1;
                        //if (dgLookup.SelectedItem.IsNotNullOrEmpty())
                        //    dgLookup.ScrollIntoView(dgLookup.SelectedItem);
                    }
                }
            }
        }

        private void movePageUp()
        {
            if (cmbSearchIn.IsDropDownOpen == false)
            {
                if (dgLookup.ItemsSource != null && dgLookup.Items.Count > 0)
                {
                    if (dgLookup.Items.Count > 0)
                    {
                        int rowCount = dgLookup.Items.Count;
                        int index = dgLookup.SelectedIndex;

                        if (index == 0)
                        {
                            return;
                        }

                        index = index - 10;

                        if (index < 0)
                        {
                            index = 0;
                        }

                        dgLookup.SelectedIndex = index;
                        //if (dgLookup.SelectedItem.IsNotNullOrEmpty())
                        //    dgLookup.ScrollIntoView(dgLookup.SelectedItem);

                    }
                }
            }
        }

        private void movePageDown()
        {
            if (cmbSearchIn.IsDropDownOpen == false)
            {
                if (dgLookup.ItemsSource != null && dgLookup.Items.Count > 0)
                {
                    if (dgLookup.Items.Count > 0)
                    {
                        int rowCount = dgLookup.Items.Count;
                        int index = dgLookup.SelectedIndex;

                        if (index == dgLookup.Items.Count - 1)
                        {
                            return;
                        }

                        index = index + 10;

                        if (index > dgLookup.Items.Count - 1)
                        {
                            index = dgLookup.Items.Count - 1;
                        }

                        dgLookup.SelectedIndex = index;
                        //if (dgLookup.SelectedItem.IsNotNullOrEmpty())
                        //    dgLookup.ScrollIntoView(dgLookup.SelectedItem);
                    }
                }
            }
        }

        private void dgLookup_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //System.Windows.Shapes.Rectangle rect = e.OriginalSource as System.Windows.Shapes.Rectangle;
            //System.Windows.Documents.Run run = e.OriginalSource as System.Windows.Documents.Run;          
            //if (!IsColumnsSorting)
            //{
            //    if (run != null || (rect != null && (rect.Name == "HeaderBackground" || rect.Name == "HeaderBackgroundGradient" || rect.Name == "VerticalSeparator")))
            //    {
            //        e.Handled = true;
            //    }
            //}            
        }

        private void txtCombobox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (this.TextBox_MouseDoubleClick != null)
                this.TextBox_MouseDoubleClick(this, e);
        }

        private void dgLookup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgLookup.SelectedItem != null)
            {
                if (this.SelectedItemChanging != null)
                    this.SelectedItemChanging(this, e);

                if (dgLookup.SelectedItems.Count > 0)
                    dgLookup.ScrollIntoView(dgLookup.SelectedItems[0]);
                else
                    dgLookup.ScrollIntoView(dgLookup.SelectedItem);
            }

        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popContent.IsOpen = false;
        }


    }



}


