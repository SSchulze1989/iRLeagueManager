using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using iRLeagueManager.Models.Results;
using System.Collections.ObjectModel;
using iRLeagueManager.Locations;
using iRLeagueManager.Models.Reviews;

namespace iRLeagueManager.Models.Sessions
{
    /// <summary>
    /// Base type for league sessions.
    /// </summary>
    [Serializable()]
    public class SessionModel : SessionInfo, IHierarchicalModel, ISession //IRaceSession
    {
        //private ScheduleModel schedule;
        //public ScheduleModel Schedule { get => schedule; internal set => SetValue(ref schedule, value); }

        //public SeasonModel Season => Schedule?.Season;

        //private int sessionResultId;
        //public ResultModel SessionResult { get => Season.Results.LastOrDefault(x => x.ResultId == sessionResultId); set { sessionResultId = value.ResultId; OnPropertyChanged(); } }
        private ResultInfo sessionResult;
        public ResultInfo SessionResult
        {
            get => sessionResult;
            set {
                if (SetValue(ref sessionResult, value))
                {
                    OnPropertyChanged(nameof(IHierarchicalModel.Children));
                }
            }
        }
        //IResult ISession.Result => SessionResult;

        private ObservableCollection<IncidentReviewInfo> reviews;
        public ObservableCollection<IncidentReviewInfo> Reviews { get => reviews; internal set => SetNotifyCollection(ref reviews, value); }

        private TimeSpan duration;
        /// <summary>
        /// Duration of the session. In case of a race with attached qualy, this also includes the times of free practice and qualifiying.
        /// </summary>
        [XmlIgnore]
        public TimeSpan Duration { get => duration; set => SetValue(ref duration, value); }

        //string IHierarchicalModel.Description => Date.ToShortDateString() + " - " + Location?.ShortName;

        IEnumerable<object> IHierarchicalModel.Children => (SessionResult != null) ? new object[] { SessionResult } : new object[0];

        //private int raceId;
        ///// <summary>
        ///// Unique race id for the league
        ///// </summary>
        //public int RaceId { get => raceId; set { raceId, value); }

        //private int laps;
        ///// <summary>
        ///// Number of laps for the race. Set to 0 for time based races.
        ///// </summary>
        //public int Laps { get => laps; set { laps, value); }

        ///// <summary>
        ///// Xml data of TimeSpan element
        ///// </summary>
        //private TimeSpan practiceLength;
        ///// <summary>
        ///// Length of the free practice. Set to 0:00:00 for no practice or warmup.
        ///// </summary>
        //public TimeSpan PracticeLength { get => practiceLength; set { practiceLength, value); }

        ///// <summary>
        ///// Xml data of TimeSpan element
        ///// </summary>
        //private TimeSpan qualyLength;
        ///// <summary>
        ///// Length of the attached qualifying. Set to 0:00:00 for no attached qualy.
        ///// </summary>
        //public TimeSpan QualyLength { get => qualyLength; set { qualyLength, value); }

        ///// <summary>
        ///// Xml data of TimeSpan element
        ///// </summary>
        //private TimeSpan raceLength;
        ///// <summary>
        ///// Length of the race. If length is not time limited - set to 0:00:00
        ///// </summary>
        //public TimeSpan RaceLength { get => raceLength; set { raceLength, value); }

        //private string irSessionId;
        ///// <summary>
        ///// Session id from iracing.com service
        ///// </summary>
        //public string IrSessionId { get => irSessionId; set { irSessionId, value); }

        //private string irResultLink;
        ///// <summary>
        ///// Link to the iracing.com results page.
        ///// </summary>
        //public string IrResultLink { get => irResultLink; set { irResultLink, value); }

        //private bool qualyAttached;
        ///// <summary>
        ///// Check if session has attached qualifying
        ///// </summary>
        //public bool QualyAttached { get => qualyAttached; set { qualyAttached, value); }

        //private bool practiceAttached;
        ///// <summary>
        ///// Check if session has attached free-practice or warmup
        ///// </summary>
        //public bool PracticeAttached { get => practiceAttached; set { practiceAttached, value); }


        //public XmlTimeSpan Duration { get; set; }

        /// <summary>
        /// Create a new Session object
        /// </summary>
        //public SessionBase()
        //{
        //    SessionType = SessionType.Undefined;
        //}

        public SessionModel(long? sessionId, SessionType sessionType) : base(sessionId, sessionType)
        {
            Date = DateTime.Today;
            SessionType = sessionType;
            //LocationId = "";
            //Laps = 0;
            //Duration = TimeSpan.FromMinutes(120);
            //IrSessionId = "12345678";
            //IrResultLink = "https://members.iracing.com/session_id";
            //RaceId = 0;
        }

        public SessionModel(ScheduleModel schedule, SessionType sessionType) : base(null, sessionType)
        {
            //Schedule = schedule;
            SessionType = sessionType;
            Date = DateTime.Today;
            //LocationId = "";
            //Laps = 0;
            //Duration = TimeSpan.FromMinutes(120);
            //IrSessionId = "12345678";
            //IrResultLink = "https://members.iracing.com/session_id";
            //if (Schedule.RacesCount == 0)
            //{
            //    RaceId = 1;
            //}
            //else
            //{
            //    RaceId = Schedule.Sessions.Where(x => SessionType == SessionType.Race).Select(x => ((RaceSessionModel)x).RaceId).Max() + 1;
            //}
        }


        //public SessionBase(ISession session)
        //{
        //    SessionId = session.SessionId;
        //    SessionType = session.SessionType;
        //    Date = session.Date;
        //    Location = session.Location;
        //    Duration = session.Duration;
        //}

        public static SessionModel GetTemplate()
        {
            return new SessionModel((int?)null, SessionType.Practice);
        }

        internal override void InitializeModel()
        {
            if (!isInitialized)
            {
                //if (Schedule != null)
                //{
                //    //SessionResult = Season.Results.SingleOrDefault(x => x.ResultId == SessionResult.ResultId);
                //    //SessionResult = Season.Results.SingleOrDefault(x => x.Session.SessionId == SessionId);
                //    if (SessionResult != null)
                //    {
                //        //SessionResult.Session = this;
                //        SessionResult.InitializeModel();
                //    }
                //}
                //else
                //{
                //    return;
                //}
            }
            base.InitializeModel();
        }
    }

    ///// <summary>
    ///// Collection of Session types
    ///// </summary>
    //public enum SessionType
    //{
    //    Undefined,
    //    Practice,
    //    Qualifying,
    //    Race
    //}
}
