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
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using iRLeagueDatabase.DataTransfer.Sessions;
using iRLeagueDatabase.DataTransfer.Results;
using iRLeagueDatabase.DataTransfer.Reviews;
using System.Runtime.Serialization;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Statistics;

namespace iRLeagueDatabase.DataTransfer
{
    [DataContract]
    public class SeasonDataDTO : SeasonInfoDTO
    {
        public override Type Type => typeof(SeasonDataDTO);

        //[DataMember]
        //public int SeasonId { get; set; }
        [DataMember]
        public bool Finished { get; set; }

        [DataMember]
        public IEnumerable<ScheduleInfoDTO> Schedules { get; set; }

        [DataMember]
        public ICollection<ScoringDataDTO> Scorings { get; set; }

        [DataMember]
        public ICollection<ScoringTableDataDTO> ScoringTables { get; set; }

        [DataMember]
        public IEnumerable<IncidentReviewInfoDTO> Reviews { get; set; }

        [DataMember]
        public IEnumerable<ResultInfoDTO> Results { get; set; }

        [DataMember]
        public long[] SeasonStatisticSetIds { get; set; }

        [DataMember]
        public DateTime SeasonStart { get; set; }

        [DataMember]
        public DateTime SeasonEnd { get; set; }

        [DataMember]
        public VoteCategoryDTO[] VoteCategories { get; set; }

        [DataMember]
        public CustomIncidentDTO[] CustomIncidents { get; set; }

        [DataMember]
        public bool HideCommentsBeforeVoted { get; set; }

        //public ScoringDataDTO MainScoring { get; set; }

        //[DataMember]
        //public LeagueMemberInfoDTO CreatedBy { get; set; }
        //[DataMember]
        //public LeagueMemberInfoDTO LastModifiedBy { get; set; }

        public SeasonDataDTO() { }
    }
}
