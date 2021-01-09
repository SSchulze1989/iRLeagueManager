using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    public class SimSessionDetailsDTO : MappableDTO
    {
        [DataMember]
        public long ResultId { get; set; }
        [DataMember]
        public long IRSubsessionId { get; set; }
        [DataMember]
        public long IRSeasonId { get; set; }
        [DataMember]
        public string IRSeasonName { get; set; }
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
        public double KmDistPerLap { get; set; }
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

        public override object MappingId => ResultId;
        public override object[] Keys => new object[] { ResultId };
    }
}
