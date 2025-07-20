using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Utilities
{
    public static class TimestampExtensions
    {
        public static double AddYear(this double time, double year) => time + (year * 31536000 * 1000); //1 year = 31536000 seconds
        public static double AddMinutes(this double time, double minutes) => time + (minutes * 60000);
        public static double AddDay(this double time, int day) => time + (day * 24 * 60 * 60000); //day*hours*minutes*miliseconds

        public static double FirstDayOfLastMonth( this double time)
        {
            var currentDate = Timestamp.ToDatetime((long)time - 1000 * 60 * 60 * 25);
            var lastMonth = currentDate.AddMonths(-1);
            var firstDay = new DateTime(lastMonth.Year, lastMonth.Month, 1);
            return Timestamp.TimestampDateTime(firstDay);
        }
        public static double LastDayOfLastMonth( this double time)
        {
            var currentDate = Timestamp.ToDatetime((long)time);
            var thisMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var lastDay = thisMonth.Date.AddMilliseconds(-1);
            return Timestamp.TimestampDateTime(lastDay);
        }
    }
}
