using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonaDotNetTemplate.Utilities
{
    public class Timestamp
    {
        public static double Now
        {
            get
            {
                var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                return timestamp;
            }
        }
        public static double TimestampDateTime(DateTime? date)
        {
            if (!date.HasValue)
                return 0;
            var timestamp = new DateTimeOffset(date.Value).ToUnixTimeMilliseconds();
            return timestamp;
        }

        public static double TimestampDate(long timestamp)
        {
            DateTime rslt = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).Date;
            var result = new DateTimeOffset(rslt).ToUnixTimeMilliseconds();
            return result;
        }
        public static DateTime ToUTCDatetime(long timestamp)
        {
            DateTime rslt = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).UtcDateTime;
            return rslt;
        }
        public static DateTime ToDatetime(long timestamp)
        {
            DateTime rslt = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
            return rslt;
        }

        public static DateTime ToLocalDateTime(double? timestamp)
        {
            if (!timestamp.HasValue)
                return DateTime.MinValue;

            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds((long)timestamp);
            return dateTimeOffset.LocalDateTime;
        }


        

        public static double MonDay()
        {
            DateTime nowDatetime = DateTime.Today;
            var timeRange = DateTime.Now - DateTime.UtcNow;
            int dayOfWeek = (int)nowDatetime.DayOfWeek;
            DateTime monday = nowDatetime.AddDays(-dayOfWeek + 1);
            double mondayUTC = TimestampDateTime(monday) + timeRange.TotalMilliseconds;
            return Math.Round(mondayUTC);
        }

        public static string ToString(double? timeStamp, string format)
        {
            if (!timeStamp.HasValue)
                return null;

            return ToLocalDateTime(timeStamp).ToString(format);
        }
    }
}
