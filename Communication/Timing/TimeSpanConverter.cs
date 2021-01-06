using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Timing
{
    public static class TimeSpanConverter
    {
        public static TimeSpan Convert(long source)
        {
            return TimeSpan.FromTicks(source);
        }

        public static long Convert(TimeSpan source)
        {
            return source.Ticks;
        }
    }
}
