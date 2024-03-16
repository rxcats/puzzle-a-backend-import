using System;
using System.Globalization;

namespace GameExtensions.Extensions
{
    public static class DateTimeExtensions
    {
        public static int GetWeekOfYear(this DateTime value)
        {
            var dfi = DateTimeFormatInfo.CurrentInfo;

            if (dfi == null)
            {
                return 0;
            }
            
            var cal = dfi.Calendar;
            
            var weekOfYear = cal.GetWeekOfYear(value, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            return int.Parse($"{value.Year}{weekOfYear:D2}");
        }
    }
}