using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using iRLeagueManager.Interfaces;


namespace iRLeagueManager.Locations
{
    [Serializable]
    public class Location : ILocation
    {
        protected TrackConfig config;
        public string LocationId { get => config.Track.TrackId + "-" + config.ConfigId; }
        public string TrackName { get => config.Track.TrackName; }
        public string ConfigName { get => config.ConfigName; }
        public string FullName { get => TrackName + " - " + ConfigName; }
        public string ShortName { get => config.Track.ShortName + ((config.ShortName != "") ?  " - " + config.ShortName : ""); }

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
