using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace iRLeagueDatabase.Entities
{
    [Serializable]
    public class Location
    {
        protected TrackConfig config;
        public string LocationId { get => config.Track.TrackId + "-" + config.ConfigId; }
        public string TrackName { get => config.Track.TrackName; }
        public string ConfigName { get => config.ConfigName; }
        public string FullName { get => TrackName + " - " + ConfigName; }

        public Location(TrackConfig confg)
        {
            config = confg;
        }

        public RaceTrack GetTrackInfo()
        {
            return config.Track;
        }

        public TrackConfig GetConfigInfo()
        {
            return config;
        }
    }
}
