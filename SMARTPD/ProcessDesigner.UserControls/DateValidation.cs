using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using ProcessDesigner.Common;

namespace ProcessDesigner.UserControls
{
    public class DateValidation
    {
        public DateValidation()
        {
           
        }

        public static bool LeapYear(int year)
        {
            if (year % 4 != 0)
                return true;
            else if ((year % 400) == 0)
                return false;
            else if ((year % 100) == 0)
                return false;
            else
                return true;
        }

        public static bool CheckIsValidDate(string str)
        {
            try
            {
                string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

                if (str.Trim().ToString().Length == 0)
                {
                    return false;
                }

                int sDate = 0;
                int sMonth = 0;
                int sYear = 0;
                string[] dateval = str.Split('/');
                sDate = Convert.ToInt32(dateval[0].ToString());
                sMonth = Convert.ToInt32(dateval[1].ToString());

                if (dateval[2].ToString().Length >= 4)
                {
                    // sYear = Convert.ToInt32(dateval[2].ToString());
                    sYear = Convert.ToInt32(dateval[2].ToString().Substring(0, 4).ToString());
                }


                int retval = 0;
                retval = date_is_valid(sDate, sMonth, sYear);
                if (retval == 0)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ex.LogException();
                return false;
            }

            

        }

        public static int date_is_valid(int day, int month, int year)
        {

            int[] month_length = new int[] { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            if (LeapYear(year))
                month_length[2] = 29;
            if (month < 1 || month > 12)
                return 0;
            else if (day < 1 || day > month_length[month])
                return 0;
            else
                return 1;
        }
    }
}

