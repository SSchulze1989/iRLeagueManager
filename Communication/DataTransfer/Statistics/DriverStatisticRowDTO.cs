using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Statistics
{
    [DataContract]
    public class DriverStatisticRowDTO : MappableDTO
    {
        [DataMember]
        public long StatisticSetId { get; set; }
        [DataMember]
        public long MemberId { get; set; }
        [DataMember]
        public int StartIRating { get; set; }
        [DataMember]
        public int EndIRating { get; set; }
        [DataMember]
        public double StartSRating { get; set; }
        [DataMember]
        public double EndSRating { get; set; }
        [DataMember]
        public long? FirstSessionId { get; set; }
        [DataMember]
        public DateTime? FirstSessionDate { get; set; }
        [DataMember]
        public long? FirstRaceId { get; set; }
        [DataMember]
        public DateTime? FirstRaceDate { get; set; }
        [DataMember]
        public long? FirstResultRowId { get; set; }
        [DataMember]
        public long? LastSessionId { get; set; }
        [DataMember]
        public DateTime? LastSessionDate { get; set; }
        [DataMember]
        public long? LastRaceId { get; set; }
        [DataMember]
        public DateTime? LastRaceDate { get; set; }
        [DataMember]
        public long? LastResultRowId { get; set; }
        [DataMember]
        public int RacePoints { get; set; }
        [DataMember]
        public int TotalPoints { get; set; }
        [DataMember]
        public int BonusPoints { get; set; }
        [DataMember]
        public int Races { get; set; }
        [DataMember]
        public int Wins { get; set; }
        [DataMember]
        public int Poles { get; set; }
        [DataMember]
        public int Top3 { get; set; }
        [DataMember]
        public int Top5 { get; set; }
        [DataMember]
        public int Top10 { get; set; }
        [DataMember]
        public int Top15 { get; set; }
        [DataMember]
        public int Top20 { get; set; }
        [DataMember]
        public int Top25 { get; set; }
        [DataMember]
        public int RacesInPoints { get; set; }
        [DataMember]
        public int RacesCompleted { get; set; }
        [DataMember]
        public int Incidents { get; set; }
        [DataMember]
        public int PenaltyPoints { get; set; }
        [DataMember]
        public int FastestLaps { get; set; }
        [DataMember]
        public int IncidentsUnderInvestigation { get; set; }
        [DataMember]
        public int IncidentsWithPenalty { get; set; }
        [DataMember]
        public int LeadingLaps { get; set; }
        [DataMember]
        public int CompletedLaps { get; set; }
        [DataMember]
        public int CurrentSeasonPosition { get; set; }
        [DataMember]
        public double DrivenKm { get; set; }
        [DataMember]
        public double LeadingKm { get; set; }
        [DataMember]
        public double AvgFinishPosition { get; set; }
        [DataMember]
        public double AvgFinalPosition { get; set; }
        [DataMember]
        public double AvgStartPosition { get; set; }
        [DataMember]
        public double AvgPointsPerRace { get; set; }
        [DataMember]
        public double AvgIncidentsPerRace { get; set; }
        [DataMember]
        public double AvgIncidentsPerLap { get; set; }
        [DataMember]
        public double AvgIncidentsPerKm { get; set; }
        [DataMember]
        public double AvgPenaltyPointsPerRace { get; set; }
        [DataMember]
        public double AvgPenaltyPointsPerLap { get; set; }
        [DataMember]
        public double AvgPenaltyPointsPerKm { get; set; }
        [DataMember]
        public double AvgIRating { get; set; }
        [DataMember]
        public double AvgSRating { get; set; }
        [DataMember]
        public double RacesCompletedPct { get; set; }
        [DataMember]
        public int BestFinishPosition { get; set; }
        [DataMember]
        public int WorstFinishPosition { get; set; }
        [DataMember]
        public int FirstRaceFinishPosition { get; set; }
        [DataMember]
        public int LastRaceFinishPosition { get; set; }
        [DataMember]
        public int BestFinalPosition { get; set; }
        [DataMember]
        public int WorstFinalPosition { get; set; }
        [DataMember]
        public int FirstRaceFinalPosition { get; set; }
        [DataMember]
        public int LastRaceFinalPosition { get; set; }
        [DataMember]
        public int BestStartPosition { get; set; }
        [DataMember]
        public int WorstStartPosition { get; set; }
        [DataMember]
        public int FirstRaceStartPosition { get; set; }
        [DataMember]
        public int LastRaceStartPosition { get; set; }
        [DataMember]
        public int Titles { get; set; }
        [DataMember]
        public int HardChargerAwards { get; set; }
        [DataMember]
        public int CleanestDriverAwards { get; set; }

        public override object[] Keys => new object[] { StatisticSetId, MemberId };
        public override object MappingId => new { StatisticSetId, MemberId };
    }
}
