// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Timing
{
    /// <summary>
    /// Set a Timespan Hours, Minutes and Seconds components directly.
    /// All components will notify changes through propertyChanged event.
    /// </summary>
    public class TimeComponentVector : INotifyPropertyChanged
    {
        private readonly Func<TimeSpan> getTime;
        private readonly Action<TimeSpan> setTime;

        private TimeSpan Time => getTime();

        public int Days { get => Time.Days; set { TimeSpanSetDays(value); } }
        public int Hours { get => Time.Hours; set { TimeSpanSetHours(value); } }
        public int Minutes { get => Time.Minutes; set { TimeSpanSetMinutes(value); } }
        public int Seconds { get => Time.Seconds; set { TimeSpanSetSeconds(value);  } }

        public event PropertyChangedEventHandler PropertyChanged;

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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TimeSpanSetDays(int days)
        {
            if (Time.Days != days)
            {
                setTime(Time.Subtract(TimeSpan.FromDays(Time.Days)).Add(TimeSpan.FromDays(days)));
                OnPropertyChanged(nameof(Days));
            }
        }

        private void TimeSpanSetHours(int hours)
        {
            if (Time.Hours != hours)
            {
                setTime(Time.Subtract(TimeSpan.FromHours(Time.Hours)).Add(TimeSpan.FromHours(hours)));
                OnPropertyChanged(nameof(Hours));
            }
        }

        private void TimeSpanSetMinutes(int minutes)
        {
            if (Time.Minutes != minutes)
            {
                setTime(Time.Subtract(TimeSpan.FromMinutes(Time.Minutes)).Add(TimeSpan.FromMinutes(minutes)));
                OnPropertyChanged(nameof(Minutes));
            }
        }

        private void TimeSpanSetSeconds(int seconds)
        {
            if (Time.Seconds != seconds)
            {
                setTime(Time.Subtract(TimeSpan.FromSeconds(Time.Seconds)).Add(TimeSpan.FromSeconds(seconds)));
                OnPropertyChanged(nameof(Seconds));
            }
        }
    }
}
