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
using System.ComponentModel;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using iRLeagueManager.Attributes;
using iRLeagueManager.Locations;

namespace iRLeagueManager.Models.Sessions
{
    public class SessionInfo : MappableModel, ISessionInfo, INotifyPropertyChanged, IHierarchicalModel
    {
        [EqualityCheckProperty]
        public long? SessionId { get; internal set; }

        public override long[] ModelId => new long[] { SessionId.GetValueOrDefault() };

        private SessionType sessionType;
        /// <summary>
        /// Type of this session. (Practice, Qualifying or Race)
        /// </summary>
        //[XmlIgnore]
        public SessionType SessionType { get => sessionType; set => SetValue(ref sessionType, value); }

        private string name;
        public string Name { get => name; set => SetValue(ref name, value); }

        private DateTime date;
        /// <summary>
        /// Date of the session.
        /// </summary>
        public DateTime Date { get => date; set => SetValue(ref date, value); }

        //private string locationId;
        ///// <summary>
        ///// Id of the track and track-config for the session.
        ///// </summary>
        //public string LocationId { get => locationId; set => SetValue(ref locationId, value); }

        //private Location location;
        //public Location Location
        //{
        //    get => location;
        //    set
        //    {
        //        if (SetValue(ref location, value))
        //        {
        //            OnPropertyChanged(nameof(IHierarchicalModel.Description));
        //        }
        //    }
        //}

        private string locationId;
        public string LocationId
        {
            get => locationId;
            set
            {
                if (SetValue(ref locationId, value)) {
                    OnPropertyChanged(nameof(TrackId));
                    OnPropertyChanged(nameof(ConfigId));
                }
            }
        }

        public int TrackId
        {
            get
            {
                return (int.TryParse(LocationId?.Split('-').First(), out int trackId)) ? trackId : 0;
            }
            set
            {
                LocationId = value.ToString() + '-' + '1';
            }
        }

        public int ConfigId
        {
            get
            {
                return (int.TryParse(LocationId?.Split('-').Last(), out int configId)) ? configId : 0;
            }
            set
            {
                LocationId = LocationId?.Split('-').First() + '-' + value.ToString();
            }
        }

        string IHierarchicalModel.Description => SessionType.ToString() + " - " + Date.ToShortDateString();

        IEnumerable<object> IHierarchicalModel.Children => new object[0];

        public  SessionInfo(long? sessionId, SessionType sessionType)
        {
            SessionId = sessionId;
            SessionType = sessionType;
        }
    }
}
