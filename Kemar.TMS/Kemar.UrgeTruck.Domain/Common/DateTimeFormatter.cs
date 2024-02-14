using System;

namespace Kemar.UrgeTruck.Domain.Common
{
    public static class DateTimeFormatter
    {
        public static DateTime GetISDTime(DateTime dt)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dt, TimeZoneInfo.Local.Id, "India Standard Time");
        }
    }
}
