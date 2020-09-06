using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml;

using iRLeagueDatabase.DataTransfer;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Results;
using iRLeagueDatabase.DataTransfer.Reviews;
using iRLeagueDatabase.DataTransfer.Sessions;
using System.Xml.Serialization;

namespace iRLeagueDatabase.DataTransfer
{
    [DataContract,
        XmlInclude(typeof(SeasonDataDTO)),
        XmlInclude(typeof(SeasonInfoDTO)),
        XmlInclude(typeof(SessionInfoDTO)),
        XmlInclude(typeof(SessionDataDTO)),
        XmlInclude(typeof(RaceSessionDataDTO)),
        XmlInclude(typeof(CommentInfoDTO)),
        XmlInclude(typeof(CommentDataDTO)),
        XmlInclude(typeof(ReviewCommentDataDTO)),
        XmlInclude(typeof(IncidentReviewInfoDTO)),
        XmlInclude(typeof(IncidentReviewDataDTO)),
        XmlInclude(typeof(LeagueMemberInfoDTO)),
        XmlInclude(typeof(LeagueMemberDataDTO)),
        XmlInclude(typeof(ResultInfoDTO)),
        XmlInclude(typeof(ResultDataDTO)),
        XmlInclude(typeof(ScheduleInfoDTO)),
        XmlInclude(typeof(ScheduleDataDTO)),
        XmlInclude(typeof(ScoredResultDataDTO)),
        XmlInclude(typeof(ScoredResultRowDataDTO)),
        XmlInclude(typeof(ScoringInfoDTO)),
        XmlInclude(typeof(ScoringDataDTO)),
        XmlInclude(typeof(ResultRowDataDTO)),
        XmlInclude(typeof(StandingsDataDTO)),
        XmlInclude(typeof(StandingsRowDataDTO)),
        XmlInclude(typeof(AddPenaltyDTO)),
        XmlInclude(typeof(TeamDataDTO)),
        KnownType(typeof(SeasonDataDTO)),
        KnownType(typeof(SeasonInfoDTO)),
        KnownType(typeof(SessionInfoDTO)),
        KnownType(typeof(SessionDataDTO)),
        KnownType(typeof(RaceSessionDataDTO)),
        KnownType(typeof(CommentInfoDTO)),
        KnownType(typeof(CommentDataDTO)),
        KnownType(typeof(ReviewCommentDataDTO)),
        KnownType(typeof(IncidentReviewInfoDTO)),
        KnownType(typeof(IncidentReviewDataDTO)),
        KnownType(typeof(LeagueMemberInfoDTO)),
        KnownType(typeof(LeagueMemberDataDTO)),
        KnownType(typeof(ResultInfoDTO)),
        KnownType(typeof(ResultDataDTO)),
        KnownType(typeof(ScheduleInfoDTO)),
        KnownType(typeof(ScheduleDataDTO)),
        KnownType(typeof(ScoredResultDataDTO)), 
        KnownType(typeof(ScoredResultRowDataDTO)), 
        KnownType(typeof(ScoringInfoDTO)),
        KnownType(typeof(ScoringDataDTO)),
        KnownType(typeof(ScoringTableInfoDTO)),
        KnownType(typeof(ScoringTableDataDTO)),
        KnownType(typeof(ResultRowDataDTO)),
        KnownType(typeof(StandingsDataDTO)),
        KnownType(typeof(StandingsRowDataDTO)),
        KnownType(typeof(AddPenaltyDTO)),
        KnownType(typeof(TeamDataDTO))]
    public abstract class MappableDTO : IMappableDTO
    {
        public bool IsReadOnly { get; set; }
        public virtual object MappingId { get; } = null;
        public abstract object[] Keys { get; }
    }
}
