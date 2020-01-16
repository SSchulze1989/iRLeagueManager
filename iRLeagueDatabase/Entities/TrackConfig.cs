using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iRLeagueDatabase.Entities
{
    [Serializable]
    public class TrackConfig
    {
        public RaceTrack Track { get; set; }
        [Key]
        public int ConfigId { get; set; }
        public string ConfigName { get; set; }
        public double LengthKm { get; set; }
        public int Turns { get; set; }
        public ConfigType ConfigType { get; set; }
        public bool HasNigtLigthing { get; set; }


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
