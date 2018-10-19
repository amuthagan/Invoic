using System;
using System.Collections.Generic;
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

namespace ProcessDesigner.UserControls
{
    /// <summary>
    /// Interaction logic for DatePickerCus.xaml
    /// </summary>
    public partial class DatePickerCus : UserControl
    {
        private bool blnSetValue = false;
        public DatePickerCus()
        {
            InitializeComponent();
            btnToday.Content = "Today :" + String.Format(DateFormat, DateTime.Today);
        }

        public static readonly DependencyProperty _SelectedDate = DependencyProperty.Register("SelectedDate", typeof(string), typeof(DatePickerCus), new UIPropertyMetadata(String.Empty));
        public string SelectedDate
        {
            get { return (string)GetValue(_SelectedDate); }
            set { SetValue(_SelectedDate, value); }
        }

        private String _dateFormat = "dd/MM/yyyy";
        public String DateFormat
        {
            get { return "{0:" + _dateFormat + "}"; }
            set
            {
                _dateFormat = value;
            }
        }


        private void calForDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            blnSetValue = true;
            SelectDate();
            txtDate.Focus();
            togDate.IsChecked = false;
        }

        private void btnToday_Click(object sender, RoutedEventArgs e)
        {
            SetValue(_SelectedDate, String.Format(DateFormat, DateTime.Today));
            txtDate.Focus();
            togDate.IsChecked = false;
        }

        private void popContent_Opened(object sender, EventArgs e)
        {
            string date = (string)GetValue(_SelectedDate);
            if (date != "" && IsDate(date))
            {
                calForDate.SelectedDate = DateTime.ParseExact(date, _dateFormat, null);
                calForDate.DisplayDate = DateTime.ParseExact(date, _dateFormat, null);
            }

        }


        protected void SelectDate()
        {
            if (blnSetValue)
            {
                SetValue(_SelectedDate, String.Format(DateFormat, calForDate.SelectedDate));
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
                Int32.Parse(value);
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
                SelectDate();
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
                        MessageBox.Show("Invalid date format entered.", "Process Designer");
                        txtDate.Text = "";
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

                    if (month.Length == 0 || Int32.Parse(month) >= 12)
                    {
                        MessageBox.Show("Invalid Month entered.", "Process Designer");
                        txtDate.Text = "";
                        return false;
                    }

                    if (day.Length == 0 || Int32.Parse(day) >= 31)
                    {
                        MessageBox.Show("Invalid date entered.", "Process Designer");
                        txtDate.Text = "";
                        return false;
                    }


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

                    DateTime date;
                    date = DateTime.ParseExact(formattedDate, _dateFormat, null);
                    formattedDate = String.Format(DateFormat, date);

                }
                txtDate.Text = formattedDate;
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Invalid date format entered.", "Process Designer");
                txtDate.Text = "";
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

    }
}
