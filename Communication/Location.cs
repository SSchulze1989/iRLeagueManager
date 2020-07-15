using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using iRLeagueManager.Interfaces;
using System.Windows.Controls;


namespace iRLeagueManager.Locations
{
    [Serializable]
    public class Location : ILocation
    {
        protected TrackConfig config;
        public string LocationId { get => config.Track.TrackId + "-" + config.ConfigId; }
        public string TrackName { get => config.Track.TrackName; }
        public string ConfigName { get => config.ConfigName; }
        public BitmapImage MapImage { get => config.MapImage; }
        public string MapImageSrc { get => config.MapImageSrc; }
        public string FullName
        {
            get
            {
                if (ConfigName != "")
                    return TrackName + " - " + ConfigName;
                else
                    return TrackName;
            }
        }
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
