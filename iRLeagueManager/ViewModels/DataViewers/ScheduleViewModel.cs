using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Attributes;
using iRLeagueManager.Enums;

namespace iRLeagueManager.ViewModels
{
    public class ScheduleModel : ContainerModelBase<ISchedule>, ISchedule
    {
        //[EqualityCheckProperty]
        public uint ScheduleId => Source.ScheduleId;
        //[EqualityCheckProperty]
        public string Name => Source.Name;
        
        public SeasonModel Season { get; internal set; }

        private ObservableModelCollection<SessionModel, ISession> _sessions;
        public ObservableModelCollection<SessionModel, ISession> Sessions { get => _sessions; set { _sessions = value; NotifyPropertyChanged(); } }
        IEnumerable<ISession> ISchedule.Sessions => Source.Sessions;

        private ObservableModelCollection<SessionModel, ISession> _openSessions;
        public ObservableModelCollection<SessionModel, ISession> OpenSessions { get => _openSessions; set { _openSessions = value; NotifyPropertyChanged(); } }

        private ObservableModelCollection<RaceSessionModel, ISession> _raceSessions;
        public ObservableModelCollection<RaceSessionModel, ISession> RaceSessions { get => _raceSessions; set { _raceSessions = value; NotifyPropertyChanged(); } }

        public ObservableCollection<object> Children => new ObservableCollection<object>() { Sessions };
        //new ObservableModelCollection<RaceSessionModel, ISession>(Source.Sessions
        //    .Where(x => x.SessionType == SessionType.Race)
        //    .Select(x => x.GetType().Equals(typeof(RaceSessionModel)) ? (RaceSessionModel)x : new RaceSessionModel((IRaceSession)x)));

        public ScheduleModel() : base()
        {
            _raceSessions = new ObservableModelCollection<RaceSessionModel, ISession>(o => o.Schedule = this);
            _openSessions = new ObservableModelCollection<SessionModel, ISession>(o => o.Schedule = this);
            _sessions = new ObservableModelCollection<SessionModel, ISession>(_openSessions.Concat(_raceSessions), o => o.Schedule = this, updateItemSources: false);
        }

        public ScheduleModel(ISchedule source) : base(source)
        {
            _raceSessions = new ObservableModelCollection<RaceSessionModel, ISession>(source.Sessions
                .Where(x => x.SessionType == SessionType.Race)
                .Cast<IRaceSession>(), o => o.Schedule = this);
            //.Select(x => new RaceSessionModel(x)));
            _openSessions = new ObservableModelCollection<SessionModel, ISession>(source.Sessions
                .Where(x => x.SessionType != SessionType.Race), o => o.Schedule = this);
            _sessions = new ObservableModelCollection<SessionModel, ISession>(_openSessions.Concat(_raceSessions), o => o.Schedule = this, updateItemSources: false);
        }

        public ISession AddSession(SessionType sessionType)
        {
            ISession session = Source.AddSession(sessionType);
            //NotifyPropertyChanged(nameof(Sessions));
            //NotifyPropertyChanged(nameof(RaceSessions));
            RaceSessions.UpdateCollection();
            OpenSessions.UpdateCollection();
            Sessions.UpdateCollection();
            return session;
        }

        public void RemoveSession(ISession session)
        {
            Source.RemoveSession(session);
            //NotifyPropertyChanged(nameof(Sessions));
            //NotifyPropertyChanged(nameof(RaceSessions));
            RaceSessions.UpdateCollection();
            OpenSessions.UpdateCollection();
            Sessions.UpdateCollection();
        }

        public override void UpdateSource(ISchedule source)
        {
            base.UpdateSource(source);
            RaceSessions.UpdateSource(source.Sessions
                .Where(x => x.SessionType == SessionType.Race)
                .Cast<IRaceSession>());
            //.Select(x => new RaceSessionModel(x)));
            OpenSessions.UpdateSource(source.Sessions
                .Where(x => x.SessionType != SessionType.Race));
            Sessions.UpdateSource(_openSessions.Concat(_raceSessions));
        }
    }
}
