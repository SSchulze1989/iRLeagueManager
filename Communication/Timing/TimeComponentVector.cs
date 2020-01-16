using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Timing
{
    /// <summary>
    /// Set a Timespan Hours, Minutes and Seconds components directly
    /// </summary>
    public class TimeComponentVector
    {
        private readonly Func<TimeSpan> getTime;
        private readonly Action<TimeSpan> setTime;

        private TimeSpan Time => getTime();

        public int Hours { get => Time.Hours; set { TimeSpanSetHours(value); } }
        public int Minutes { get => Time.Minutes; set { TimeSpanSetMinutes(value); } }
        public int Seconds { get => Time.Seconds; set { TimeSpanSetSeconds(value);  } }

        /// <summary>
        /// Create a new component vector
        /// </summary>
        /// <param name="getTimeFunc">Get function for target timespan</param>
        /// <param name="setTimeFunc">Set function for target timespan</param>
        public TimeComponentVector(Func<TimeSpan> getTimeFunc, Action<TimeSpan> setTimeFunc)
        {
            getTime = getTimeFunc;
            setTime = setTimeFunc;
            if (getTime() == null)
            {
                setTime(TimeSpan.Zero);
            }
        }

        private void TimeSpanSetHours(int hours)
        {
            setTime(Time.Subtract(TimeSpan.FromHours(Time.Hours)).Add(TimeSpan.FromHours(hours)));
        }

        private void TimeSpanSetMinutes(int minutes)
        {
            setTime(Time.Subtract(TimeSpan.FromMinutes(Time.Minutes)).Add(TimeSpan.FromMinutes(minutes)));
        }

        private void TimeSpanSetSeconds(int seconds)
        {
            setTime(Time.Subtract(TimeSpan.FromSeconds(Time.Seconds)).Add(TimeSpan.FromSeconds(seconds)));
        }
    }
}
