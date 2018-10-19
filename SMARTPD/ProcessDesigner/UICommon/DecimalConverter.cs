using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ProcessDesigner.Common;

namespace ProcessDesigner.UICommon
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                string returnValue = System.Convert.ToDecimal(value).ToString("0.00");
                if (value.IsNotNullOrEmpty())
                    return returnValue;
                return DBNull.Value;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return value;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.IsNotNullOrEmpty() && value.ToValueAsString().IsNumeric())
                return System.Convert.ToDecimal(value);

            if (value is string && (string)value == "")
            {
                return 0;
            }

            return DBNull.Value;
        }

    }
}
