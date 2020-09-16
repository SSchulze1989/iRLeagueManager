using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using iRLeagueManager.Enums;
using iRLeagueDatabase.DataTransfer.Results;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Reviews;

namespace iRLeagueDatabase.DataTransfer.Sessions
{
    /// <summary>
    /// Base type for league sessions.
    /// </summary>
    [Serializable()]
    [DataContract]
    [KnownType(typeof(SeasonInfoDTO))]
    [KnownType(typeof(RaceSessionDataDTO))]
    public class SessionDataDTO : SessionInfoDTO
    {
        //[DataMember]
        //public int SessionId { get; set; }

        [DataMember]
        public ScheduleInfoDTO Schedule { get; set; }

        [DataMember]
        public ResultInfoDTO SessionResult { get; set; }

        [DataMember]
        public IncidentReviewInfoDTO[] Reviews { get; set; }

        [DataMember]
        /// <summary>
        /// Date of the session.
        /// </summary>
        public DateTime Date { get; set; }

        [DataMember]
        /// <summary>
        /// Id of the track and track-config for the session.
        /// </summary>
        public string LocationId { get; set; }

        [DataMember]
        /// <summary>
        /// Duration of the session. In case of a race with attached qualy, this also includes the times of free practice and qualifiying.
        /// </summary>
        public TimeSpan Duration { get; set; }

        //[DataMember]
        ///// <summary>
        ///// Result of the session
        ///// </summary>
        //public ResultDataDTO SessionResult { get; set; }

        //[DataMember]
        ///// <summary>
        ///// Unique race id for the league
        ///// </summary>
        //public int RaceId { get; set; }

        //[DataMember]
        ///// <summary>
        ///// Number of laps for the race. Set to 0 for time based races.
        ///// </summary>
        //public int Laps { get; set; }

        //[DataMember]
        ///// <summary>
        ///// Length of the free practice. Set to 0:00:00 for no practice or warmup.
        ///// </summary>
        //public TimeSpan PracticeLength { get; set; }

        //[DataMember]
        ///// <summary>
        ///// Length of the attached qualifying. Set to 0:00:00 for no attached qualy.
        ///// </summary>
        //public TimeSpan QualyLength { get; set; }

        //[DataMember]
        ///// <summary>
        ///// Length of the race. If length is not time limited - set to 0:00:00
        ///// </summary>
        //public TimeSpan RaceLength { get; set; }

        //[DataMember]
        ///// <summary>
        ///// Session id from iracing.com service
        ///// </summary>
        //public string IrSessionId { get; set; }

        //[DataMember]
        ///// <summary>
        ///// Link to the iracing.com results page.
        ///// </summary>
        //public string IrResultLink { get; set; }

        //[DataMember]
        ///// <summary>
        ///// Check if session has attached qualifying
        ///// </summary>
        //public bool QualyAttached { get; set; }

        //[DataMember]
        ///// <summary>
        ///// Check if session has attached free-practice or warmup
        ///// </summary>
        //public bool PracticeAttached { get; set; }

        [DataMember]
        public LeagueMemberInfoDTO CreatedBy { get; set; }
        [DataMember]
        public LeagueMemberInfoDTO LastModifiedBy { get; set; }
    }
}
