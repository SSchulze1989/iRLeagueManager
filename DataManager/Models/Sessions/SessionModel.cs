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
    public class SessionModel : SessionInfo, ISession //IRaceSession
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

        private ObservableCollection<SessionInfo> subSessions;
        public ObservableCollection<SessionInfo> SubSession { get => subSessions; set => SetNotifyCollection(ref subSessions, value); }

        public SessionModel() : this(0, SessionType.Undefined)
        {
        }

        public SessionModel(long? sessionId, SessionType sessionType) : base(sessionId, sessionType)
        {
            Date = DateTime.Today;
            SessionType = sessionType;
            Reviews = new ObservableCollection<IncidentReviewInfo>();
            SubSession = new ObservableCollection<SessionInfo>();
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
            Reviews = new ObservableCollection<IncidentReviewInfo>();
            SubSession = new ObservableCollection<SessionInfo>();
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
