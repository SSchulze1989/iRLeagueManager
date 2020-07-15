using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;

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
        [XmlAttribute("map_img_src")]
        public string MapImageSrc { get; set; }

        private BitmapImage mapImage = null;
        [XmlIgnore]
        public BitmapImage MapImage
        {
            get
            {
                if (MapImageSrc == null)
                    return null;

                if (mapImage == null)
                {
                    var uri = "Graphics/TrackImages/Maps/" + MapImageSrc;
                    mapImage = CreateImageFromPng(uri);
                }
                return mapImage;
            }
        }
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

        private BitmapImage CreateImageFromPng(string source)
        {
            //using (var stream = new FileStream(source, FileMode.Open, FileAccess.Read, FileShare.Read))
            //{
            //    //PngBitmapDecoder decoder = new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            //    //BitmapImage bitmapSource = decoder.Frames[0];

            //    //Image image = new Image()
            //    //{
            //    //    Source = bitmapSource,
            //    //    Stretch = System.Windows.Media.Stretch.None,
            //    //    Margin = new System.Windows.Thickness(10)
            //    //};

            //    BitmapImage image = new BitmapImage();
            //    image.BeginInit();
            //    image.CacheOption = BitmapCacheOption.Default;
            //    image.StreamSource = stream;
            //    image.EndInit();

            //    return image;
            //}

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(source, UriKind.Relative);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();

            return image;
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
