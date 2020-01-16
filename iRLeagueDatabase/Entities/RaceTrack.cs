using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iRLeagueDatabase.Entities
{
    [Serializable]
    public class RaceTrack
    {
        public int TrackId { get; set; }
        public string TrackName { get; set; }
        public List<TrackConfig> Configs { get; set; }
        //Race track data

        public RaceTrack() { }

        public RaceTrack(int trackId, string trackName)
        {
            TrackId = trackId;
            TrackName = trackName;
            Configs = new List<TrackConfig>();
        }

        public void AddConfig(int configId, string configName, int lengtKm = 0)
        {
            Configs.Add(new TrackConfig(this, configId, configName, lengtKm));
        }
    }
}
