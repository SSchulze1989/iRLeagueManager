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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using iRLeagueDatabase.DataTransfer;
using iRLeagueDatabase.DataTransfer.Members;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    public class StandingsRowDataDTO : MappableDTO
    {
        [DataMember]
        public ScoringInfoDTO Scoring { get; set; }
        public override object MappingId => new long[] { Scoring.ScoringId.GetValueOrDefault(), Member.MemberId.GetValueOrDefault() };
        public override object[] Keys => new object[] { Scoring.ScoringId.GetValueOrDefault(), Member.MemberId.GetValueOrDefault() };
        [DataMember]
        public int Position { get; set; }
        [DataMember]
        public int LastPosition { get; set; }
        //[ForeignKey(nameof(Member))]
        //public int MemberId { get; set; }
        [DataMember]
        public virtual LeagueMemberInfoDTO Member { get; set; }
        [DataMember]
        public int ClassId { get; set; }
        [DataMember]
        public string CarClass { get; set; }
        [DataMember]
        public int RacePoints { get; set; }
        [DataMember]
        public int RacePointsChange { get; set; }
        [DataMember]
        public int PenaltyPoints { get; set; }
        [DataMember]
        public int PenaltyPointsChange { get; set; }
        [DataMember]
        public int TotalPoints { get; set; }
        [DataMember]
        public int TotalPointsChange { get; set; }
        [DataMember]
        public int Races { get; set; }
        [DataMember]
        public int RacesCounted { get; set; }
        [DataMember]
        public int DroppedResults { get; set; }
        [DataMember]
        public int CompletedLaps { get; set; }
        [DataMember]
        public int CompletedLapsChange { get; set; }
        [DataMember]
        public int LeadLaps { get; set; }
        [DataMember]
        public int LeadLapsChange { get; set; }
        [DataMember]
        public int FastestLaps { get; set; }
        [DataMember]
        public int FastestLapsChange { get; set; }
        [DataMember]
        public int PolePositions { get; set; }
        [DataMember]
        public int PolePositionsChange { get; set; }
        [DataMember]
        public int Wins { get; set; }
        [DataMember]
        public int WinsChange { get; set; }
        [DataMember]
        public int Top3 { get; set; }
        [DataMember]
        public int Top5 { get; set; }
        [DataMember]
        public int Top10 { get; set; }
        [DataMember]
        public int Incidents { get; set; }
        [DataMember]
        public int IncidentsChange { get; set; }
        [DataMember]
        public int PositionChange { get; set; }
        [DataMember]
        public ScoredResultRowDataDTO[] CountedResults { get; set; }
    }
}
