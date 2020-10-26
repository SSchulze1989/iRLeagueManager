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
using System.Runtime.Serialization;
using iRLeagueDatabase.DataTransfer.Reviews;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Sessions;


namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    [KnownType(typeof(ScoredResultDataDTO))]
    public class ResultDataDTO : ResultInfoDTO, IMappableDTO
    {
        public override Type Type => typeof(ResultDataDTO);

        [DataMember]
        public SeasonInfoDTO Season { get; set; }

        // Detailed information from iracing JSON result
        // Session details
        [DataMember]
        public long IRSubsessionId { get; set; }
        [DataMember]
        public long IRSeasonId { get; set; }
        [DataMember]
        public long IRSeasonName { get; set; }
        [DataMember]
        public int IRSeasonYear { get; set; }
        [DataMember]
        public int IRSeasonQuarter { get; set; }
        [DataMember]
        public int IRRaceWeek { get; set; }
        [DataMember]
        public long IRSessionId { get; set; }
        [DataMember]
        public int LicenseCategory { get; set; }
        [DataMember]
        public string SessionName { get; set; }
        [DataMember]
        public DateTime? StartTime { get; set; }
        [DataMember]
        public DateTime? EndTime { get; set; }
        [DataMember]
        public int CornersPerLap { get; set; }
        [DataMember]
        public int MaxWeeks { get; set; }
        [DataMember]
        public int EventStrengthOfField { get; set; }
        [DataMember]
        public long EventAverageLap { get; set; }
        [DataMember]
        public int EventLapsComplete { get; set; }
        [DataMember]
        public int NumCautions { get; set; }
        [DataMember]
        public int NumCautionLaps { get; set; }
        [DataMember]
        public int NumLeadChanges { get; set; }
        [DataMember]
        public int TimeOfDay { get; set; }
        [DataMember]
        public int DamageModel { get; set; }

        // Track details
        [DataMember]
        public int IRTrackId { get; set; }
        [DataMember]
        public string TrackName { get; set; }
        [DataMember]
        public string ConfigName { get; set; }
        [DataMember]
        public int TrackCategoryId { get; set; }
        [DataMember]
        public string Category { get; set; }

        // Weather details
        [DataMember]
        public int WeatherType { get; set; }
        [DataMember]
        public int TempUnits { get; set; }
        [DataMember]
        public int TempValue { get; set; }
        [DataMember]
        public int RelHumidity { get; set; }
        [DataMember]
        public int Fog { get; set; }
        [DataMember]
        public int WindDir { get; set; }
        [DataMember]
        public int WindUnits { get; set; }
        [DataMember]
        public int Skies { get; set; }
        [DataMember]
        public int WeatherVarInitial { get; set; }
        [DataMember]
        public int WeatherVarOngoing { get; set; }
        [DataMember]
        public DateTime? SimStartUTCTime { get; set; }
        [DataMember]
        public long SimStartUTCOffset { get; set; }

        // Track state details 
        [DataMember]
        public bool LeaveMarbles { get; set; }
        [DataMember]
        public int PracticeRubber { get; set; }
        [DataMember]
        public int QualifyRubber { get; set; }
        [DataMember]
        public int WarmupRubber { get; set; }
        [DataMember]
        public int RaceRubber { get; set; }
        [DataMember]
        public int PracticeGripCompound { get; set; }
        [DataMember]
        public int QualifyGripCompund { get; set; }
        [DataMember]
        public int WarmupGripCompound { get; set; }
        [DataMember]
        public int RaceGripCompound { get; set; }
        [DataMember]
        public IEnumerable<ResultRowDataDTO> RawResults { get; set; }
        [DataMember]
        public IEnumerable<IncidentReviewInfoDTO> Reviews { get; set; }

        [DataMember]
        public LeagueMemberInfoDTO CreatedBy { get; set; }
        [DataMember]
        public LeagueMemberInfoDTO LastModifiedBy { get; set; }

        public ResultDataDTO() {  }
    }
}
