using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bing_Rewards.Utilities
{
    public static class TimeUtility
    {
        public static long GetTimeStamp()
        {
            DateTime startTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return (long)(DateTime.Now - startTime).TotalMilliseconds;
        }
    }
}
