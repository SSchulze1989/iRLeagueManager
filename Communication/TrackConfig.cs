using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iRLeagueManager.Locations
{
    [Serializable]
    public class TrackConfig
    {
        [XmlIgnore]
        public RaceTrack Track { get; set; }
        [XmlAttribute("config_id")]
        public int ConfigId { get; set; }
        [XmlAttribute("config_name")]
        public string ConfigName { get; set; }
        [XmlAttribute("length")]
        public double LengthKm { get; set; }
        [XmlAttribute("turns")]
        public int Turns { get; set; }
        [XmlAttribute("type")]
        public ConfigType ConfigType { get; set; }
        [XmlAttribute("night_lighting")]
        public bool HasNigtLigthing { get; set; }
        [XmlIgnore]
        public string ShortName => ConfigName.Substring(0, Math.Min(12, ConfigName.Length));


        public TrackConfig() { }

        public TrackConfig(RaceTrack track, int configId, string configName, int lengtKm = 0, bool hasNightLighting = false)
        {
            Track = track;
            ConfigId = configId;
            ConfigName = configName;
            LengthKm = lengtKm;
        }
    }

    public enum ConfigType
    {
        ShortTrack,
        Speedway,
        Rallycross,
        RoadCourse,
        DirtOval,
        Unknown
    }
}
