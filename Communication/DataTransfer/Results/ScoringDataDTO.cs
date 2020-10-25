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
using System.Xml.Serialization;
using System.Runtime.Serialization;
using iRLeagueManager.Enums;
using iRLeagueDatabase.DataTransfer.Sessions;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Filters;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    public class ScoringDataDTO : ScoringInfoDTO
    {
        public override Type Type => typeof(ScoringDataDTO);

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public ScoringKindEnum ScoringKind { get; set; }
        [DataMember]
        public DropRacesOption DropRacesOption { get; set; }
        [DataMember]
        public int DropWeeks { get; set; }
        [DataMember]
        public int AverageRaceNr { get; set; }
        [DataMember]
        public int MaxResultsPerGroup { get; set; }
        [DataMember]
        public bool TakeGroupAverage { get; set; }
        [DataMember]
        public virtual SessionInfoDTO[] Sessions { get; set; }
        [DataMember]
        public long SeasonId { get; set; }
        [DataMember]
        public virtual SeasonInfoDTO Season { get; set; }
        [DataMember]
        public string BasePoints { get; set; }
        [DataMember]
        public string BonusPoints { get; set; }
        [DataMember]
        public string IncPenaltyPoints { get; set; }
        [DataMember]
        public virtual ResultInfoDTO[] Results { get; set; }
        [DataMember]
        public LeagueMemberInfoDTO CreatedBy { get; set; }
        [DataMember]
        public LeagueMemberInfoDTO LastModifiedBy { get; set; }
        [DataMember]
        public ScheduleInfoDTO ConnectedSchedule { get; set; }
        [DataMember]
        public ScoringInfoDTO ExtScoringSource { get; set; }
        [DataMember]
        public bool TakeResultsFromExtSource { get; set; }
        [DataMember]
        public long[] ResultsFilterOptionIds { get; set; }

        public ScoringDataDTO() { }
    }
}
