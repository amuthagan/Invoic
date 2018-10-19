using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using ProcessDesigner.Common;
using System.Globalization;
using System.Threading;

namespace ProcessDesigner.UserControls
{
    /// <summary>
    /// Interaction logic for DatePickerCus.xaml
    /// </summary>
    public partial class DatePickerCus : UserControl
    {
        public event EventHandler SelectedDateChanged;
        public string ApplicationTitle = "SmartPD";

        private bool blnSetValue = false;
        public DatePickerCus()
        {
            InitializeComponent();
        }

        public static DependencyProperty _SelectedDate = DependencyProperty.Register("SelectedDate", typeof(Nullable<DateTime>), typeof(DatePickerCus),
            new FrameworkPropertyMetadata(null) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });


        public Nullable<DateTime> SelectedDate
        {
            get { return (Nullable<DateTime>)GetValue(_SelectedDate); }
            set { SetValue(_SelectedDate, value); }
        }

        public static DependencyProperty _VisibilityErrorTemp = DependencyProperty.Register("VisibilityErrorTempDate", typeof(Visibility), typeof(DatePickerCus),
         new FrameworkPropertyMetadata(Visibility.Collapsed) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        public Visibility VisibilityErrorTempDate
        {
            get { return (Visibility)GetValue(_VisibilityErrorTemp); }
            set
            {
                SetValue(_VisibilityErrorTemp, value);
            }
        }

        private String _dateFormat = "dd/MM/yyyy";
        public String DateFormat
        {

            get { return _dateFormat; }
            set
            {
                _dateFormat = value;
            }
        }

        private String waterMark = "DD/MM/YYYY";
        public String WaterMark
        {
            get { return waterMark; }
            set
            {
                waterMark = value;
            }
        }

        public Boolean IsDropDownOpen
        {
            get { return popContent.IsOpen; }
            set { popContent.IsOpen = value; }
        }

        private void calForDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            blnSetValue = true;
            SelectDate();
            if (this.SelectedDateChanged != null)
                this.SelectedDateChanged(this, e);

            //  txtDate.Focus();
            togDate.IsChecked = false;
        }

        private void btnToday_Click(object sender, RoutedEventArgs e)
        {
            blnSetValue = true;
            calForDate.SelectedDate = DateTime.Now;
            txtDate.Focus();
            togDate.IsChecked = false;
        }

        private void popContent_Opened(object sender, EventArgs e)
        {
            if (SelectedDate == null)
            {
                blnSetValue = false;
                txtDate.Text = "";
                calForDate.DisplayDate = DateTime.Now;
                //calForDate.SelectedDate = DateTime.Now;
            }
            //else
            //{
            //    calForDate.SelectedDate = SelectedDate;
            //    calForDate.DisplayDate = (DateTime)SelectedDate;
            //}

        }

        private void popContent_Closed(object sender, EventArgs e)
        {
            SelectDate();
            txtDate.Focus();
        }

        protected void SelectDate()
        {
            if (blnSetValue)
            {
                SelectedDate = calForDate.SelectedDate;
                txtDate.Text = calForDate.SelectedDate.ToFormattedDateAsString("dd/MM/yyyy");
            }
            else
            {
                SelectedDate = null;
                txtDate.Text = "";
            }
        }

        protected bool IsDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected bool IsNumeric(String value)
        {
            try
            {
                Int64.Parse(value);
                return true;
            }
            catch
            {
                return false;
            }
        }


        private void txtDate_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                blnSetValue = true;
            }
            else if (e.Key == Key.Escape)
            {
                blnSetValue = false;
            }
            togDate.IsChecked = false;
        }

        private bool CorrectDateText()
        {
            try
            {
                string formattedDate = txtDate.Text.Replace(".", "/").Replace(" ", "/").Replace("-", "/").Replace("|", "/").Replace("//", "/");
                formattedDate = System.Convert.ToString(formattedDate).Replace("{0:", "").Replace("}", "");

                if (String.IsNullOrEmpty(formattedDate.Trim())) return true;

                string[] dateFormats = _dateFormat.Split('/');
                string[] dateComponents = formattedDate.Split('/');
                string month = "";
                string day = "";
                string year = "";


                if (IsNumeric(formattedDate))
                {
                    if (formattedDate.Length == 8)
                    {
                        //if (Int32.Parse(formattedDate.Substring(4, 4)) < 1900)
                        //{
                        //    MessageBox.Show("Invalid Year entered.", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                        //    txtDate.Text = "";
                        //    SelectedDate = null;
                        //    return false;
                        //}

                        DateTime date;
                        date = DateTime.ParseExact(formattedDate, _dateFormat.Replace("/", ""), null);
                        formattedDate = String.Format(DateFormat, date);
                    }
                    else
                    {
                        MessageBox.Show("Invalid date entered.", "SmartPD", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtDate.Text = "";
                        SelectedDate = null;
                        return false;
                    }
                }


                if (dateComponents.Length > 1)
                {
                    for (int i = 0; i < dateComponents.Length; i++)
                    {
                        switch (dateFormats[i].ToString().ToUpper())
                        {
                            case "MM":
                                month = dateComponents[i].Trim();
                                break;
                            case "DD":
                                day = dateComponents[i].Trim();
                                break;
                            case "YYYY":
                                year = dateComponents[i].Trim();
                                break;
                        }
                    }

                    // We require a two-digit month. If there is only one digit, add a leading zero:
                    if (month.Length == 1)
                    {
                        month = "0" + month;
                    }

                    // We require a two-digit day. If there is only one digit, add a leading zero:

                    if (day.Length == 1)
                    {
                        day = "0" + day;
                    }

                    // two digits denoting the current century as leading numerals:
                    if (year.Length == 2)
                    {
                        year = "20" + year;
                    }
                    else if (year.Length == 0)
                    {
                        year = DateTime.Now.Year.ToString();
                    }

                    if (month.Length == 0 || Int32.Parse(month) > 12)
                    {
                        MessageBox.Show("Invalid Month entered.", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtDate.Text = "";
                        SelectedDate = null;
                        return false;
                    }

                    if (day.Length == 0 || Int32.Parse(day) > 31)
                    {
                        MessageBox.Show("Invalid date entered.", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                        txtDate.Text = "";
                        SelectedDate = null;
                        return false;
                    }

                    //if (year.Length == 4 && Int32.Parse(year) < 1900)
                    //{
                    //    MessageBox.Show("Invalid Year entered.", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                    //    txtDate.Text = "";
                    //    SelectedDate = null;
                    //    return false;
                    //}


                    formattedDate = "";
                    // Put the date back together again with proper delimiters, and 
                    for (int i = 0; i < dateComponents.Length; i++)
                    {
                        switch (dateFormats[i].ToString().ToUpper())
                        {
                            case "MM":
                                formattedDate = formattedDate + month + "/";
                                break;
                            case "DD":
                                formattedDate = formattedDate + day + "/";
                                break;
                            case "YYYY":
                                formattedDate = formattedDate + year;
                                break;
                        }
                    }

                }

                SelectedDate = DateTime.ParseExact(formattedDate, _dateFormat, null);
                txtDate.Text = formattedDate;
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid date entered.", ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
                txtDate.Text = "";
                SelectedDate = null;
                return false;
            }
        }



        private void txtDate_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (CorrectDateText() == false)
            {
                e.Handled = true;
            }
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[0-9/, -]"); //regex that matches allowed text
            return regex.IsMatch(text);
        }


        private void txtDate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsTextAllowed(e.Text))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!txtDate.Text.Trim().IsNotNullOrEmpty() || !IsTextAllowed(txtDate.Text))
            {
                txtDate.Text = "";
                blnSetValue = false;
                SelectedDate = null;
            }
        }

        private void CusDatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = DateFormat;
            ci.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = ci;

            //if (!txtDate.Text.IsNotNullOrEmpty())
            //{
            //    txtDate.Text = "";
            //}

            BindingExpression binding = this.GetBindingExpression(DatePickerCus._SelectedDate);
            if (binding != null && binding.DataItem != null)
            {
                if (binding.DataItem.GetType() == typeof(DataRowView) && binding.ParentBinding.Path.Path.Split('.').Length == 1)
                {
                    DataRowView drv = (DataRowView)binding.DataItem;
                    string cellValue = Convert.ToString(drv[binding.ParentBinding.Path.Path]);
                    if (cellValue.Length > 0)
                    {
                        txtDate.Text = GetCorrectDateText(cellValue);
                        binding.UpdateSource();
                    }
                }
                else
                {
                    binding.UpdateSource();
                    object dt = GetValue<object>(binding, binding.DataItem);
                    if (dt != null)
                    {
                        txtDate.Text = dt.ToFormattedDateAsString();
                        binding.UpdateSource();
                    }
                }


            }
            btnToday.Content = "Today : " + String.Format(DateFormat, DateTime.Today);
        }

        public T GetValue<T>(BindingExpression expression, object dataItem)
        {
            if (expression == null || dataItem == null)
            {
                return default(T);
            }

            string bindingPath = expression.ParentBinding.Path.Path;
            string[] properties = bindingPath.Split('.');

            object currentObject = dataItem;
            Type currentType = null;

            for (int i = 0; i < properties.Length; i++)
            {
                currentType = currentObject.GetType();
                System.Reflection.PropertyInfo property = currentType.GetProperty(properties[i]);
                if (property == null)
                {
                    currentObject = null;
                    break;
                    if (currentObject.GetType() == typeof(DataRowView) && properties.Length == 1)
                    {
                        DataRowView drv = (DataRowView)currentObject;
                        currentObject = drv[bindingPath];
                        break;
                    }


                }
                currentObject = property.GetValue(currentObject, null);
                if (currentObject == null)
                {
                    break;
                }
            }

            return (T)currentObject;
        }

        private string GetCorrectDateText(string txtDate)
        {
            try
            {
                string formattedDate = txtDate.Replace(".", "/").Replace(" ", "/").Replace("-", "/").Replace("|", "/").Replace("//", "/");

                if (String.IsNullOrEmpty(formattedDate.Trim())) return txtDate;

                string[] dateFormats = _dateFormat.Split('/');
                string[] dateComponents = formattedDate.Split('/');
                string month = "";
                string day = "";
                string year = "";


                if (IsNumeric(formattedDate))
                {
                    if (formattedDate.Length == 8)
                    {
                        DateTime date;
                        date = DateTime.ParseExact(formattedDate, _dateFormat.Replace("/", ""), null);
                        formattedDate = String.Format(DateFormat, date);
                    }
                    else
                    {
                        return txtDate;
                    }
                }


                if (dateComponents.Length > 1)
                {
                    for (int i = 0; i < dateComponents.Length; i++)
                    {
                        if (i > dateFormats.Length - 1) break;
                        switch (dateFormats[i].ToString().ToUpper())
                        {
                            case "MM":
                                month = dateComponents[i].Trim();
                                break;
                            case "DD":
                                day = dateComponents[i].Trim();
                                break;
                            case "YYYY":
                                year = dateComponents[i].Trim();
                                break;
                        }
                    }

                    if (Int32.Parse(month) > 12 && Int32.Parse(day) < 12)
                    {
                        string tmp = day;
                        day = month;
                        month = tmp;
                    }

                    // We require a two-digit month. If there is only one digit, add a leading zero:
                    if (month.Length == 1)
                    {
                        month = "0" + month;
                    }

                    // We require a two-digit day. If there is only one digit, add a leading zero:

                    if (day.Length == 1)
                    {
                        day = "0" + day;
                    }

                    // two digits denoting the current century as leading numerals:
                    if (year.Length == 2)
                    {
                        year = "20" + year;
                    }
                    else if (year.Length == 0)
                    {
                        year = DateTime.Now.Year.ToString();
                    }

                    //if (month.Length == 0 || Int32.Parse(month) > 12)
                    //{
                    //    return txtDate;
                    //}

                    //if (day.Length == 0 || Int32.Parse(day) > 31)
                    //{
                    //    return txtDate;
                    //}

                    formattedDate = "";
                    // Put the date back together again with proper delimiters, and 
                    for (int i = 0; i < dateComponents.Length; i++)
                    {
                        if (i > dateFormats.Length - 1) break;
                        switch (dateFormats[i].ToString().ToUpper())
                        {
                            case "MM":
                                formattedDate = formattedDate + month + "/";
                                break;
                            case "DD":
                                formattedDate = formattedDate + day + "/";
                                break;
                            case "YYYY":
                                formattedDate = formattedDate + year;
                                break;
                        }
                    }
                }

                return formattedDate;
            }
            catch (Exception)
            {
                return txtDate;
            }
        }

    }
}
