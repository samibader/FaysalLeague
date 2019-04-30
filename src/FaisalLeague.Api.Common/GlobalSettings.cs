
using System;
using System.Collections.Generic;
using System.Text;

namespace FaisalLeague.Api.Common
{
    public static class GlobalSettings
    {
        public static DateTime CURRENT_DATETIME {
            get
            {
                return DateTime.Now;
            }
        }
        public static string DATE_FORMAT = "dd/MM/yyyy";
        public static string DATETIME_FORMAT = "dd/MM/yyyy hh:mm:ss";
        public static string CURRENT_DATETIME_AS_STRING
        {
            get
            {
                return CURRENT_DATETIME.ToString(DATETIME_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
            }

        }
        public static string CURRENT_DATE_AS_STRING = CURRENT_DATETIME.ToString(DATE_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
        public static string ConvertDateTimeToString(DateTime value)
        {
            return value.ToString(DATETIME_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
        }
        public static string ConvertDateToString(DateTime value)
        {
            return value.ToString(DATE_FORMAT, System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}
