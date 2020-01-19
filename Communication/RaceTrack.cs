using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace iRLeagueManager.Locations
{
    [Serializable]
    public class RaceTrack
    {
        public int TrackId { get; set; }
        public string TrackName { get; set; }
        public string ShortName => TrackName.Substring(0, Math.Min(20, TrackName.Length));
        public ObservableCollection<TrackConfig> Configs { get; set; }
        //Race track data

        public RaceTrack() { }

        public RaceTrack(int trackId, string trackName)
        {
            TrackId = trackId;
            TrackName = trackName;
            Configs = new ObservableCollection<TrackConfig>();
        }

        public void AddConfig(int configId, string configName, int lengtKm = 0)
        {
            Configs.Add(new TrackConfig(this, configId, configName, lengtKm));
        }
    }
}
