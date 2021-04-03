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
using System.Xml;
using System.Xml.Serialization;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;

namespace iRLeagueManager.Models.Sessions
{
    /// <summary>
    /// Session including a race. Can have attached qualy or practice.
    /// </summary>
    [Serializable()]
    public class RaceSessionModel : SessionModel, INotifyPropertyChanged, IRaceSession
    {
        private long raceId;
        /// <summary>
        /// Unique race id for the league
        /// </summary>
        public long RaceId { get => raceId; set => SetValue(ref raceId, value); }

        private int laps;
        /// <summary>
        /// Number of laps for the race. Set to 0 for time based races.
        /// </summary>
        //public int Laps { get => laps; set { laps = value; OnPropertyChanged(); } }
        public int Laps { get => laps; set => SetValue(ref laps, value); } 

        public new DateTime Date
        {
            get => base.Date;
            set
            {
                base.Date = value;
                OnPropertyChanged(nameof(RaceStart));
            }
        }

        public new TimeSpan Duration
        {
            get => base.Duration;
            set
            {
                base.Duration = value;
                OnPropertyChanged(nameof(RaceStart));
            }
        }

        /// <summary>
        /// Xml data of TimeSpan element
        /// </summary>
        private TimeSpan practiceLength;
        /// <summary>
        /// Length of the free practice. Set to 0:00:00 for no practice or warmup.
        /// </summary>
        public TimeSpan PracticeLength
        {
            get => practiceLength;
            set
            {
                if (SetValue(ref practiceLength, value))
                {
                    OnPropertyChanged();
                    SetDuration();
                    OnPropertyChanged(nameof(RaceStart));
                }
                if (practiceLength == TimeSpan.Zero)
                    PracticeAttached = false;
                else
                    PracticeAttached = true;
            }
        }

        /// <summary>
        /// Xml data of TimeSpan element
        /// </summary>
        private TimeSpan qualyLength;
        /// <summary>
        /// Length of the attached qualifying. Set to 0:00:00 for no attached qualy.
        /// </summary>
        public TimeSpan QualyLength
        {
            get => qualyLength;
            set
            {
                if (SetValue(ref qualyLength, value))
                {
                    OnPropertyChanged();
                    SetDuration();
                    OnPropertyChanged(nameof(RaceStart));
                }
                if (qualyLength == TimeSpan.Zero)
                    QualyAttached = false;
                else
                    QualyAttached = true;
            }
        }

        /// <summary>
        /// Xml data of TimeSpan element
        /// </summary>
        private TimeSpan raceLength;
        /// <summary>
        /// Length of the race. If length is not time limited - set to 0:00:00
        /// </summary>
        public TimeSpan RaceLength
        {
            get => raceLength;
            set
            {
                if (SetValue(ref raceLength, value))
                {
                    OnPropertyChanged();
                    SetDuration();
                    OnPropertyChanged(nameof(RaceStart));
                }
            }
        }

        //private TimeSpan raceStart;
        public TimeSpan RaceStart => Date.TimeOfDay + PracticeLength + QualyLength;

        private string irSessionId;
        /// <summary>
        /// Session id from iracing.com service
        /// </summary>
        public string IrSessionId { get => irSessionId; set => SetValue(ref irSessionId, value); }

        private string irResultLink;
        /// <summary>
        /// Link to the iracing.com results page.
        /// </summary>
        public string IrResultLink { get => irResultLink; set => SetValue(ref irResultLink, value); }

        private bool qualyAttached;
        /// <summary>
        /// Check if session has attached qualifying
        /// </summary>
        public bool QualyAttached { get => qualyAttached; set => SetValue(ref qualyAttached, value); }

        private bool practiceAttached;
        /// <summary>
        /// Check if session has attached free-practice or warmup
        /// </summary>
        public bool PracticeAttached { get => practiceAttached; set => SetValue(ref practiceAttached, value); }         

        /// <summary>
        /// Create a new RaceSession object.
        /// </summary>
        //public RaceSession()
        //{
        //    SessionId = 0;
        //    SessionType = SessionType.Race;
        //}

        //public RaceSession(IRaceSession session) : base(session)
        //{
        //    RaceId = session.RaceId;
        //    Laps = session.Laps;
        //    IrResultLink = session.IrResultLink;
        //    IrSessionId = session.IrSessionId;
        //    PracticeLength = session.PracticeLength;
        //    QualyLength = session.QualyLength;
        //    RaceLength = session.RaceLength;
        //    QualyAttached = session.QualyAttached;
        //    PracticeAttached = session.PracticeAttached;
        //}

        public RaceSessionModel(long? sessionId) : base(sessionId, SessionType.Race)
        {
            Date = DateTime.Today;
            //LocationId = "";
            Laps = 0;
            Duration = TimeSpan.FromMinutes(120);
            IrSessionId = "12345678";
            IrResultLink = "https://members.iracing.com/session_id";
        }

        /// <summary>
        /// Create a new RaceSession object.
        /// </summary>
        /// <param name="raceId"></param>
        public RaceSessionModel(long? sessionId, long raceId) : base(sessionId, SessionType.Race)
        {
            RaceId = raceId;
            Date = DateTime.Today;
            //LocationId = "";
            Laps = 0;
            Duration = TimeSpan.FromMinutes(120);
            IrSessionId = "12345678";
            IrResultLink = "https://members.iracing.com/session_id";
        }

        public RaceSessionModel(ScheduleModel schedule) : base(schedule, SessionType.Race)
        {
            Date = DateTime.Today;
            //LocationId = "";
            Laps = 0;
            Duration = TimeSpan.FromMinutes(120);
            IrSessionId = "12345678";
            IrResultLink = "https://members.iracing.com/session_id";
            //if (Schedule.RacesCount == 0)
            //{
            //    RaceId = 1;
            //}
            //else
            //{
            //    var test = Schedule.Sessions.Where(x => x.SessionType == SessionType.Race);
            //    RaceId = Schedule.Sessions.Where(x => x.SessionType == SessionType.Race).Cast<RaceSessionModel>().Select(x => x.RaceId).Max() + 1;
            //}
        }

        private void SetDuration()
        {
            Duration = PracticeLength + QualyLength + RaceLength;
        }

        public new static RaceSessionModel GetTemplate()
        {
            return new RaceSessionModel((int?)null)
            {
                Name = "Test",
                SubSessionNr = 1,
                Date = DateTime.Today.AddHours(19),
                Duration = TimeSpan.FromMinutes(20)
            };
        } 
    }
}
