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
    public class SessionInfo : ModelBase, ISessionInfo, INotifyPropertyChanged, IHierarchicalModel
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
