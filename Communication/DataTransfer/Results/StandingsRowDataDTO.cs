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

using iRLeagueDatabase.DataTransfer;
using iRLeagueDatabase.DataTransfer.Members;

namespace iRLeagueDatabase.DataTransfer.Results
{
    public class StandingsRowDataDTO : MappableDTO
    {
        public ScoringInfoDTO Scoring { get; set; }
        public override object MappingId => new long[] { Scoring.ScoringId.GetValueOrDefault(), Member.MemberId.GetValueOrDefault() };
        public override object[] Keys => new object[] { Scoring.ScoringId.GetValueOrDefault(), Member.MemberId.GetValueOrDefault() };
        public int Position { get; set; }
        public int LastPosition { get; set; }
        //[ForeignKey(nameof(Member))]
        //public int MemberId { get; set; }
        public virtual LeagueMemberInfoDTO Member { get; set; }
        public int ClassId { get; set; }
        public string CarClass { get; set; }
        public int RacePoints { get; set; }
        public int RacePointsChange { get; set; }
        public int PenaltyPoints { get; set; }
        public int PenaltyPointsChange { get; set; }
        public int TotalPoints { get; set; }
        public int TotalPointsChange { get; set; }
        public int Races { get; set; }
        public int RacesCounted { get; set; }
        public int DroppedResults { get; set; }
        public int CompletedLaps { get; set; }
        public int CompletedLapsChange { get; set; }
        public int LeadLaps { get; set; }
        public int LeadLapsChange { get; set; }
        public int FastestLaps { get; set; }
        public int FastestLapsChange { get; set; }
        public int PolePositions { get; set; }
        public int PolePositionsChange { get; set; }
        public int Wins { get; set; }
        public int WinsChange { get; set; }
        public int Top3 { get; set; }
        public int Top5 { get; set; }
        public int Top10 { get; set; }
        public int Incidents { get; set; }
        public int IncidentsChange { get; set; }
        public int PositionChange { get; set; }
        public ScoredResultRowDataDTO[] CountedResults { get; set; }
    }
}
