using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using iRLeagueManager.Enums;

namespace iRLeagueManager.Timing
{
    [Serializable()]
    public class LapTime : IXmlSerializable, ILapTime
    {
        //private DateTime _time { get => DateTime.MinValue + Time; set => Time = value.TimeOfDay; }

        public TimeSpan Time { get; set; }
        public int Minutes => Time.Minutes;
        public int Seconds => Time.Seconds;
        public int Milliseconds => Time.Milliseconds;

        public override string ToString()
        {
            //return _time.ToString("mm:ss,fff", System.Globalization.CultureInfo.GetCultureInfo("de-DE"));
            TimeSpan time = Time;
            string signString = "";

            bool negative = (Time < TimeSpan.Zero) ? true : false;
            if (negative)
            {
                time = Time.Negate();
                signString = "-";
            }

            return signString + (DateTime.MinValue + time).ToString("mm:ss,fff", System.Globalization.CultureInfo.GetCultureInfo("de-DE"));
        }

        public virtual string ToXmlString()
        {
            return XmlConvert.ToString(Time);
        }

        public bool IsValid()
        {
            if (Time > TimeSpan.Zero) return true;
            else return false;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            Time = XmlConvert.ToTimeSpan(reader.ReadElementContentAsString());
            //_time = DateTime.FromBinary(long.Parse(reader.ReadElementContentAsString()));
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlConvert.ToString(Time);
            Console.WriteLine(XmlConvert.ToString(Time));
        }

        public LapTime() { }

        public LapTime(TimeSpan time)
        {
            Time = time;
        }

        public LapTime(ILapTime lapTime)
        {
            Time = lapTime.Time;
        }

        static public LapTime FromXmlString(string xmlString)
        {
            return new LapTime((string.IsNullOrEmpty(xmlString)) ? TimeSpan.Zero : XmlConvert.ToTimeSpan(xmlString));
        }
    }
}
