// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
using System.Runtime.Serialization;
using System.ServiceModel;

namespace iRLeagueManager.Locations
{
    [Serializable]
    [DataContract]
    [XmlSerializerFormat]
    public class TrackConfig
    {
        [XmlIgnore]
        public RaceTrack Track { get; set; }
        [XmlAttribute("config_id")]
        [DataMember(Name = "config_id")]
        public int ConfigId { get; set; }
        [XmlAttribute("config_name")]
        [DataMember(Name = "config_name")]
        public string ConfigName { get; set; }
        [XmlAttribute("length")]
        [DataMember(Name = "length")]
        public double LengthKm { get; set; }
        [XmlAttribute("turns")]
        [DataMember(Name = "turns")]
        public int Turns { get; set; }
        [XmlAttribute("type")]
        [DataMember(Name = "type")]
        public ConfigType ConfigType { get; set; }
        [XmlAttribute("night_lighting")]
        [DataMember(Name = "night_lighting")]
        public bool HasNigtLigthing { get; set; }
        [XmlAttribute("map_img_src")]
        [DataMember(Name = "map_img_src")]
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
            image.UriSource = new Uri(new Uri("pack://application:,,,/"), new Uri(source, UriKind.Relative));
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
