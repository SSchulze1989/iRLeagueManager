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
using iRLeagueManager.Enums;
using iRLeagueDatabase.DataTransfer.Members;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    [KnownType(typeof(ResultInfoDTO))]
    public class ResultRowDataDTO : MappableDTO, IMappableDTO
    {
        [DataMember]
        public long? ResultRowId { get; set; }
        [DataMember]
        public long ResultId { get; set; }
        [DataMember]
        public SimSessionTypeEnum SimSessionType { get; set; }
        [DataMember]
        public int StartPosition { get; set; }
        [DataMember]
        public int FinishPosition { get; set; }
        [DataMember]
        public LeagueMemberInfoDTO Member { get; set; }

        [DataMember]
        public int OldIRating { get; set; }
        [DataMember]
        public int NewIRating { get; set; }
        [DataMember]
        public int SeasonStartIRating { get; set; }
        [DataMember]
        public string License { get; set; }
        [DataMember]
        public double OldSafetyRating { get; set; }
        [DataMember]
        public double NewSafetyRating { get; set; }
        [DataMember]
        public int OldCpi { get; set; }
        [DataMember]
        public int NewCpi { get; set; }
        [DataMember]
        public int ClubId { get; set; }
        [DataMember]
        public string ClubName { get; set; }
        [DataMember]
        public string TeamName { get; set; }
        [DataMember]
        public int CarNumber { get; set; }
        [DataMember]
        public int ClassId { get; set; }
        [DataMember]
        public string Car { get; set; }
        [DataMember]
        public int CarId { get; set; }
        [DataMember]
        public string CarClass { get; set; }
        [DataMember]
        public int CompletedLaps { get; set; }
        [DataMember]
        public double CompletedPct { get; set; }
        [DataMember]
        public int LeadLaps { get; set; }
        [DataMember]
        public int FastLapNr { get; set; }
        [DataMember]
        public int Incidents { get; set; }
        [DataMember]
        public RaceStatusEnum Status { get; set; }
        [DataMember]
        public TimeSpan QualifyingTime { get; set; }
        [DataMember]
        public TimeSpan Interval { get; set; }
        [DataMember]
        public TimeSpan AvgLapTime { get; set; }
        [DataMember]
        public TimeSpan FastestLapTime { get; set; }
        [DataMember]
        public int PositionChange { get; set; }
        [DataMember]
        public string LocationId { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int Division { get; set; }
        [DataMember]
        public int OldLicenseLevel { get; set; }
        [DataMember]
        public int NewLicenseLevel { get; set; }


        public override object MappingId => ResultRowId;

        //public override object[] Keys => new object[] { ResultRowId, ResultId };
        public override object[] Keys => new object[] { ResultRowId };

        public ResultRowDataDTO() { }
    }
}
