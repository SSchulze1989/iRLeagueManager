using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Results
{
    public class SimSessionDetails
    {
        // Detailed information from iracing JSON result
        // Session details
        public long IRSubsessionId { get; set; }
        public long IRSeasonId { get; set; }
        public string IRSeasonName { get; set; }
        public int IRSeasonYear { get; set; }
        public int IRSeasonQuarter { get; set; }
        public int IRRaceWeek { get; set; }
        public long IRSessionId { get; set; }
        public int LicenseCategory { get; set; }
        public string SessionName { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int CornersPerLap { get; set; }
        public double KmDistPerLap { get; set; }
        public int MaxWeeks { get; set; }
        public int EventStrengthOfField { get; set; }
        public long EventAverageLap { get; set; }
        public int EventLapsComplete { get; set; }
        public int NumCautions { get; set; }
        public int NumCautionLaps { get; set; }
        public int NumLeadChanges { get; set; }
        public int TimeOfDay { get; set; }
        public int DamageModel { get; set; }

        // Track details
        public int IRTrackId { get; set; }
        public string TrackName { get; set; }
        public string ConfigName { get; set; }
        public int TrackCategoryId { get; set; }
        public string Category { get; set; }

        // Weather details
        public int WeatherType { get; set; }
        public int TempUnits { get; set; }
        public int TempValue { get; set; }
        public int RelHumidity { get; set; }
        public int Fog { get; set; }
        public int WindDir { get; set; }
        public int WindUnits { get; set; }
        public int Skies { get; set; }
        public int WeatherVarInitial { get; set; }
        public int WeatherVarOngoing { get; set; }
        public DateTime? SimStartUTCTime { get; set; }
        public long SimStartUTCOffset { get; set; }

        // Track state details 
        public bool LeaveMarbles { get; set; }
        public int PracticeRubber { get; set; }
        public int QualifyRubber { get; set; }
        public int WarmupRubber { get; set; }
        public int RaceRubber { get; set; }
        public int PracticeGripCompound { get; set; }
        public int QualifyGripCompund { get; set; }
        public int WarmupGripCompound { get; set; }
        public int RaceGripCompound { get; set; }
    }
}
