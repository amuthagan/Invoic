using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using ProcessDesigner.Common;

namespace ProcessDesigner.UICommon
{
    public class DateTimeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value.IsNotNullOrEmpty())
                    return ((DateTime)value).ToFormattedDateAsString();
                return DBNull.Value;
                //DateTime date = (DateTime)value;
                //return new DateTimeOffset(date);
            }
            catch (Exception ex)
            {
                ex.LogException();
                return value;
                //return DateTimeOffset.MinValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string strValue = value.ToString();
                DateTime resultDateTime;
                return DateTime.TryParse(strValue, out resultDateTime) ? resultDateTime : value;

                //DateTimeOffset dto = (DateTimeOffset)value;
                //return dto.DateTime;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return DateTime.MinValue;
            }
        }
    }

    public class DateConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value.IsNotNullOrEmpty())
                {
                    string strValue = value.ToString();
                    DateTime resultDateTime;
                    return DateTime.TryParse(strValue, out resultDateTime)
                        ? (object)resultDateTime.Date
                        : ((DateTime)value).ToFormattedDateAsString();
                }
                return DBNull.Value;
                //DateTime date = (DateTime)value;
                //return new DateTimeOffset(date);
            }
            catch (Exception ex)
            {
                ex.LogException();
                return CorrectDateText(System.Convert.ToString(value).Replace("{0:", "").Replace("}", ""));
                //return DateTimeOffset.MinValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string strValue = value.ToString();
                DateTime resultDateTime;
                return DateTime.TryParse(strValue, out resultDateTime) ? resultDateTime.Date : value;

                //DateTimeOffset dto = (DateTimeOffset)value;
                //return dto.DateTime;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return DateTime.MinValue.Date;
            }
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

        private object CorrectDateText(string txtDate)
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
                    return new DateTime(System.Convert.ToInt32(year), System.Convert.ToInt32(month),
                        System.Convert.ToInt32(day));
                }

                return formattedDate;
            }
            catch (Exception)
            {
                return txtDate;
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

    }

}
