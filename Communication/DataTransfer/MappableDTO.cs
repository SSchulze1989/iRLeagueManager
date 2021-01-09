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
using System.Runtime.Serialization;
using System.Xml;

using iRLeagueDatabase.DataTransfer;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Results;
using iRLeagueDatabase.DataTransfer.Reviews;
using iRLeagueDatabase.DataTransfer.Sessions;
using System.Xml.Serialization;
using iRLeagueDatabase.DataTransfer.Filters;
using iRLeagueDatabase.DataTransfer.Statistics;
using System.Collections;

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
        KnownType(typeof(ScoredTeamResultDataDTO)),
        KnownType(typeof(ScoredTeamResultRowDataDTO)),
        KnownType(typeof(ScoringInfoDTO)),
        KnownType(typeof(ScoringDataDTO)),
        KnownType(typeof(ScoringTableInfoDTO)),
        KnownType(typeof(ScoringTableDataDTO)),
        KnownType(typeof(ResultRowDataDTO)),
        KnownType(typeof(StandingsDataDTO)),
        KnownType(typeof(StandingsRowDataDTO)),
        KnownType(typeof(AddPenaltyDTO)),
        KnownType(typeof(TeamDataDTO)),
        KnownType(typeof(TeamStandingsDataDTO)),
        KnownType(typeof(TeamStandingsRowDataDTO)),
        KnownType(typeof(VoteCategoryDTO)),
        KnownType(typeof(CustomIncidentDTO)),
        KnownType(typeof(ReviewPenaltyDTO)),
        KnownType(typeof(ResultsFilterOptionDTO)),
        KnownType(typeof(StatisticSetDTO)),
        KnownType(typeof(SeasonStatisticSetDTO)),
        KnownType(typeof(LeagueStatisticSetDTO)),
        KnownType(typeof(ImportedStatisticSetDTO)),
        KnownType(typeof(DriverStatisticDTO)),
        KnownType(typeof(DriverStatisticRowDTO)),
        KnownType(typeof(SimSessionDetailsDTO))]
    public abstract class MappableDTO : IMappableDTO
    {
        public bool IsReadOnly { get; set; }
        public virtual object MappingId { get; } = null;
        public abstract object[] Keys { get; }

        public override string ToString()
        {
            string id;
            if (MappingId?.GetType().IsArray == true)
            {
                id = string.Join(",", ((IEnumerable)MappingId).Cast<object>());
            }
            else
            {
                id = MappingId?.ToString();
            }
            return $"{this.GetType()}/id[{id}]";
        }
    }
}
