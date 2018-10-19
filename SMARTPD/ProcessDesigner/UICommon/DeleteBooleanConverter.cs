using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ProcessDesigner.UICommon
{
    class DeleteBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (value == null || value.ToString().Trim().Length == 0 || value == DBNull.Value || value.ToString().Trim().ToUpper() == "0")
                return false ;
            else if (value != null || value.ToString().Trim().Length > 0 || value != DBNull.Value || value.ToString().Trim().ToUpper() == "1")
                return true ;
            else return Binding.DoNothing;

            //if (value == null)
            //{
            //    return false;
            //}
            //switch (value.ToString().ToLower())
            //{
            //    case "1":
            //        return true;
            //    case "0":
            //        return false;

            //    default:
            //        return Binding.DoNothing;
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value == true)
                    return "1";
                else
                    return "0";
            }
            return "0";
        }

        public object ConvertValues(object value, Type targetType, object parameter,
                       System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return false;
            }
            switch (value.ToString().ToLower())
            {
                case "1":
                    return true;
                case "0":
                    return false;
            }
            return false;
        }
    }
}
