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
