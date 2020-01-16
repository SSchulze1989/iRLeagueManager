using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using iRLeagueManager.Locations;
using iRLeagueManager.Timing;
using System.Runtime.CompilerServices;

namespace iRLeagueManager.ViewModels
{
    public class SessionViewModel : LeagueContainerModel<SessionModel>
    {
        public SessionModel Model
        {
            get => Source;
            protected set
            {
                if (SetSource(value))
                {
                    OnPropertyChanged(null);
                }
            }
        }

        private LocationCollection Locations { get; } = new LocationCollection();

        private ScheduleViewModel schedule;
        public ScheduleViewModel Schedule { get => schedule; set => SetValue(ref schedule, value); }

        public int? SessionId => Model.SessionId;

        public int? SessionNumber => Schedule?.Sessions.IndexOf(this) + 1;

        public SessionType SessionType { get => Model.SessionType; set => Model.SessionType = value; }
        public DateTime Date { get => Model.Date.Date; set => Model.Date = value.Date.Add(Model.Date.TimeOfDay); }
        public TimeSpan TimeOfDay { get => Model.Date.TimeOfDay; set => Model.Date = Date.Date.Add(value); }
        public TimeComponentVector TimeOfDayComponents { get; }

        public TimeSpan Duration { get => Model.Duration; set => Model.Duration = value; }
        public TimeComponentVector DurationComponents { get; }

        //public RaceTrack Track { get => Model.Location?.GetTrackInfo(); set => Model.Location = new Location(value.Configs.First()); }
        //public IEnumerable<TrackConfig> TrackConfigs { get => Track?.Configs; }
        //public TrackConfig Config { get => Model.Location?.GetConfigInfo(); set => Model.Location = new Location(value); }

        public int TrackId { get => Model.TrackId; set => Model.TrackId = value; }
        public int ConfigId { get => Model.ConfigId; set => Model.ConfigId = value; }
        public int ConfigIndex { get => ConfigId - 1; set => ConfigId = value + 1; }

        public RaceTrack Track => Locations.FirstOrDefault(x => x.GetTrackInfo().TrackId == TrackId)?.GetTrackInfo();
        public IEnumerable<TrackConfig> TrackConfigs => Track?.Configs;
        public TrackConfig Config => Track?.Configs.SingleOrDefault(x => x.ConfigId == ConfigId);

        public int? Laps { get => (Model as RaceSessionModel)?.Laps; set { if (Model is RaceSessionModel race) { race.Laps = value.Value; } } }
        public string IrResultLink { get => (Model as RaceSessionModel)?.IrResultLink; set { if (Model is RaceSessionModel race) { race.IrResultLink = value; } } }
        public string IrSessionId { get => (Model as RaceSessionModel)?.IrSessionId; set { if (Model is RaceSessionModel race) { race.IrSessionId = value; } } }
        public TimeSpan? PracticeLength { get => (Model as RaceSessionModel)?.PracticeLength; set { if (Model is RaceSessionModel race) { race.PracticeLength = value.Value; } } }
        public TimeComponentVector PracticeLenghtComponents { get; }
        public TimeSpan? QualyLength { get => (Model as RaceSessionModel)?.QualyLength; set { if (Model is RaceSessionModel race) { race.QualyLength = value.Value; } } }
        public TimeComponentVector QualyLengthComponents { get; }
        public TimeSpan? RaceLength { get => (Model as RaceSessionModel)?.RaceLength; set { if (Model is RaceSessionModel race) { race.RaceLength = value.Value; } } }
        public TimeComponentVector RaceLengthComponents { get; }
        public bool? QualyAttached { get => (Model as RaceSessionModel)?.QualyAttached; set { if (Model is RaceSessionModel race) { race.QualyAttached = value.Value; } } }
        public bool? PracticeAttached { get => (Model as RaceSessionModel)?.PracticeAttached; set { if (Model is RaceSessionModel race) { race.PracticeAttached = value.Value; } } }

        public int? RaceId => (Model as RaceSessionModel)?.RaceId;

        public SessionViewModel() : base()
        {
            SetSource(RaceSessionModel.GetTemplate());
            TimeOfDayComponents = new TimeComponentVector(() => TimeOfDay, x => TimeOfDay = x);
            DurationComponents = new TimeComponentVector(() => Duration, x => Duration = x);
            PracticeLenghtComponents = new TimeComponentVector(() => PracticeLength.GetValueOrDefault(), x => PracticeLength = x);
            QualyLengthComponents = new TimeComponentVector(() => QualyLength.GetValueOrDefault(), x => QualyLength = x);
            RaceLengthComponents = new TimeComponentVector(() => RaceLength.GetValueOrDefault(), x => RaceLength = x);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Model.LocationId))
            {
                OnPropertyChanged(nameof(Track));
                OnPropertyChanged(nameof(TrackConfigs));
                OnPropertyChanged(nameof(Config));
                OnPropertyChanged(nameof(ConfigIndex));
            }

            if (propertyName == nameof(Date))
            {
                OnPropertyChanged(nameof(TimeOfDay));
                OnPropertyChanged(nameof(TimeOfDayComponents));
            }
            if (propertyName == nameof(Duration))
            {
                OnPropertyChanged(nameof(DurationComponents));
            }
            if (propertyName == nameof(PracticeLength))
            {
                OnPropertyChanged(nameof(PracticeLenghtComponents));
            }
            if (propertyName == nameof(QualyLength))
            {
                OnPropertyChanged(nameof(QualyLengthComponents));
            }
            if (propertyName == nameof(RaceLength))
            {
                OnPropertyChanged(nameof(RaceLengthComponents));
            }
        }
    }
}
