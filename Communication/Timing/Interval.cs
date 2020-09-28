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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

namespace iRLeagueManager.Timing
{
    public class LapInterval : LapTime, IInterval
    {
        //private DateTime _time { get => DateTime.MinValue.AddDays(Laps) + Time; set { Time = value.TimeOfDay; Laps = value.DayOfYear; }  }
        //public int Laps { get; set; }
        public int Laps { get => Time.Days; set => Time = Time.Subtract(TimeSpan.FromDays(Time.Days)).Add(TimeSpan.FromDays(value)); }

        public override string ToString()
        {
            if (Laps != 0)
            {
                return Laps.ToString();
            }
            return base.ToString();
        }

        public LapInterval() { }

        public LapInterval(TimeSpan time) : base(time) { }

        public LapInterval(TimeSpan time, int laps) : base(time)
        {
            Laps = laps;
        }

        public LapInterval(ILapTime lapTime, int laps) : base(lapTime)
        {
            Laps = laps;
        }

        public LapInterval(IInterval interval) : base(interval)
        {
            Laps = interval.Laps;
        }

        public override string ToXmlString()
        {
            TimeSpan time = Time.Add(TimeSpan.FromDays(Laps));
            return XmlConvert.ToString(time);
        }

        new static public LapInterval FromXmlString(string xmlString)
        {
            TimeSpan time = (string.IsNullOrEmpty(xmlString)) ? TimeSpan.Zero : XmlConvert.ToTimeSpan(xmlString);
            return new LapInterval(time.Subtract(TimeSpan.FromDays(time.Days)), time.Days);
        }
    }
}
