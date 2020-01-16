using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Attributes;
using iRLeagueManager.Enums;
using iRLeagueManager.Models;

namespace iRLeagueManager.ViewModels
{
    public class SessionModel : ContainerModelBase<ISession>, ISession
    {
        private ScheduleModel _schedule;
        public ScheduleModel Schedule { get => _schedule; set { _schedule = value; NotifyPropertyChanged(); } }

        [EqualityCheckProperty]
        public uint SessionId { get => Source.SessionId; }
        [EqualityCheckProperty]
        public SessionType SessionType { get => Source.SessionType; }

        public virtual DateTime Date { get => Source.Date; set { Source.Date = value; NotifyPropertyChanged(); } }
        public string LocationId
        {
            get => Source.LocationId;
            set
            {
                Source.LocationId = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(AvailableTrackConfigs));
                NotifyPropertyChanged(nameof(TrackConfig));
                NotifyPropertyChanged(nameof(Location));
                NotifyPropertyChanged(nameof(RaceTrack));
            }
        }
        public Location Location
        {
            get => Locations.SingleOrDefault(x => x.LocationId == LocationId);
            set
            {
                LocationId = value?.LocationId;
                //NotifyPropertyChanged();
            }
        }

        public RaceTrack RaceTrack
        {
            get => Location?.GetTrackInfo();
            set
            {
                Location = Locations.FirstOrDefault(x => x.GetTrackInfo().TrackId == value?.TrackId);
                //NotifyPropertyChanged();
            }
        }

        public TrackConfig TrackConfig
        {
            get => Location?.GetConfigInfo();
            set
            {
                if (value != null)
                {
                    Location = Locations.Where(x => x.GetTrackInfo().TrackId == value?.Track.TrackId)?.SingleOrDefault(x => x.GetConfigInfo().ConfigId == value?.ConfigId);
                }
                //NotifyPropertyChanged();
            }
        }

        public LocationCollection Locations { get; } = new LocationCollection();

        public IEnumerable<RaceTrack> TrackCollection => Locations.GetTrackList().OrderBy(x => x.TrackName);

        ILocation ISession.Location { get => Location; set { LocationId = value.LocationId; NotifyPropertyChanged(); } }

        public IEnumerable<TrackConfig> AvailableTrackConfigs => Location?.GetTrackInfo().Configs;

        //public ILocation Location { get => Source.Location; set { Source.Location = value; NotifyPropertyChanged(); } }
        public TimeSpan Duration { get => Source.Duration; set { Source.Duration = value; NotifyPropertyChanged(); } }

        public SessionModel() : base() { }
        public SessionModel(ISession source) : base(source) { }

        public object GetSourceObject()
        {
            return Source.GetSourceObject();
        }
    }
}
