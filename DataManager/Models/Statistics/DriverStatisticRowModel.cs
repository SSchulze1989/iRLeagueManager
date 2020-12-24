using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Statistics
{
    public class DriverStatisticRowModel : MappableModel
    {
        private long statisticSetId;
        public long StatisticSetId { get => statisticSetId; set => SetValue(ref statisticSetId, value); }

        private long memberId;
        public long MemberId { get => memberId; set => SetValue(ref memberId, value); }

        private int startIRating;
        public int StartIRating { get => startIRating; set => SetValue(ref startIRating, value); }

        private int endIRating;
        public int EndIRating { get => endIRating; set => SetValue(ref endIRating, value); }

        private double startSRating;
        public double StartSRating { get => startSRating; set => SetValue(ref startSRating, value); }

        private double endSRating;
        public double EndSRating { get => endSRating; set => SetValue(ref endSRating, value); }

        private long? firstSessionId;
        public long? FirstSessionId { get => firstSessionId; set => SetValue(ref firstSessionId, value); }

        private DateTime? firstSessionDate;
        public DateTime? FirstSessionDate { get => firstSessionDate; set => SetValue(ref firstSessionDate, value); }

        private long? firstRaceId;
        public long? FirstRaceId { get => firstRaceId; set => SetValue(ref firstRaceId, value); }

        private DateTime? firstRaceDate;
        public DateTime? FirstRaceDate { get => firstRaceDate; set => SetValue(ref firstRaceDate, value); }

        private long? firstResultRowId;
        public long? FirstResultRowId { get => firstResultRowId; set => SetValue(ref firstResultRowId, value); }

        private long? lastSessionId;
        public long? LastSessionId { get => lastSessionId; set => SetValue(ref lastSessionId, value); }

        private DateTime? lastSessionDate;
        public DateTime? LastSessionDate { get => lastSessionDate; set => SetValue(ref lastSessionDate, value); }

        private long? lastRaceId;
        public long? LastRaceId { get => lastRaceId; set => SetValue(ref lastRaceId, value); }

        private DateTime? lastRaceDate;
        public DateTime? LastRaceDate { get => lastRaceDate; set => SetValue(ref lastRaceDate, value); }

        private long? lastResultRowId;
        public long? LastResultRowId { get => lastResultRowId; set => SetValue(ref lastResultRowId, value); }

        private int racePoints;
        public int RacePoints { get => racePoints; set => SetValue(ref racePoints, value); }

        private int totalPoints;
        public int TotalPoints { get => totalPoints; set => SetValue(ref totalPoints, value); }

        private int bonusPoints;
        public int BonusPoints { get => bonusPoints; set => SetValue(ref bonusPoints, value); }

        private int races;
        public int Races { get => races; set => SetValue(ref races, value); }

        private int wins;
        public int Wins { get => wins; set => SetValue(ref wins, value); }

        private int poles;
        public int Poles { get => poles; set => SetValue(ref poles, value); }

        private int top3;
        public int Top3 { get => top3; set => SetValue(ref top3, value); }

        private int top5;
        public int Top5 { get => top5; set => SetValue(ref top5, value); }

        private int top10;
        public int Top10 { get => top10; set => SetValue(ref top10, value); }

        private int top15;
        public int Top15 { get => top15; set => SetValue(ref top15, value); }

        private int top20;
        public int Top20 { get => top20; set => SetValue(ref top20, value); }

        private int top25;
        public int Top25 { get => top25; set => SetValue(ref top25, value); }

        private int racesInPoints;
        public int RacesInPoints { get => racesInPoints; set => SetValue(ref racesInPoints, value); }

        private int racesCompleted;
        public int RacesCompleted { get => racesCompleted; set => SetValue(ref racesCompleted, value); }

        private int incidents;
        public int Incidents { get => incidents; set => SetValue(ref incidents, value); }

        private int penaltyPoints;
        public int PenaltyPoints { get => penaltyPoints; set => SetValue(ref penaltyPoints, value); }

        private int fastestLaps;
        public int FastestLaps { get => fastestLaps; set => SetValue(ref fastestLaps, value); }

        private int incidentsUnderInvestigation;
        public int IncidentsUnderInvestigation { get => incidentsUnderInvestigation; set => SetValue(ref incidentsUnderInvestigation, value); }

        private int incidentsWithPenalty;
        public int IncidentsWithPenalty { get => incidentsWithPenalty; set => SetValue(ref incidentsWithPenalty, value); }

        private int leadingLaps;
        public int LeadingLaps { get => leadingLaps; set => SetValue(ref leadingLaps, value); }

        private int completedLaps;
        public int CompletedLaps { get => completedLaps; set => SetValue(ref completedLaps, value); }

        private double drivenKm;
        public double DrivenKm { get => drivenKm; set => SetValue(ref drivenKm, value); }

        private double leadingKm;
        public double LeadingKm { get => leadingKm; set => SetValue(ref leadingKm, value); }

        private double avgFinishPosition;
        public double AvgFinishPosition { get => avgFinishPosition; set => SetValue(ref avgFinishPosition, value); }

        private double avgFinalPosition;
        public double AvgFinalPosition { get => avgFinalPosition; set => SetValue(ref avgFinalPosition, value); }

        private double avgStartPosition;
        public double AvgStartPosition { get => avgStartPosition; set => SetValue(ref avgStartPosition, value); }

        private double avgPointsPerRace;
        public double AvgPointsPerRace { get => avgPointsPerRace; set => SetValue(ref avgPointsPerRace, value); }

        private double avgIncidentsPerRace;
        public double AvgIncidentsPerRace { get => avgIncidentsPerRace; set => SetValue(ref avgIncidentsPerRace, value); }

        private double avgIncidentsPerLap;
        public double AvgIncidentsPerLap { get => avgIncidentsPerLap; set => SetValue(ref avgIncidentsPerLap, value); }

        private double avgIncidentsPerKm;
        public double AvgIncidentsPerKm { get => avgIncidentsPerKm; set => SetValue(ref avgIncidentsPerKm, value); }

        private double avgPenaltyPointsPerRace;
        public double AvgPenaltyPointsPerRace { get => avgPenaltyPointsPerRace; set => SetValue(ref avgPenaltyPointsPerRace, value); }

        private double avgPenaltyPointsPerLap;
        public double AvgPenaltyPointsPerLap { get => avgPenaltyPointsPerLap; set => SetValue(ref avgPenaltyPointsPerLap, value); }

        private double avgPenaltyPointsPerKm;
        public double AvgPenaltyPointsPerKm { get => avgPenaltyPointsPerKm; set => SetValue(ref avgPenaltyPointsPerKm, value); }

        private double avgIRating;
        public double AvgIRating { get => avgIRating; set => SetValue(ref avgIRating, value); }

        private double avgSRating;
        public double AvgSRating { get => avgSRating; set => SetValue(ref avgSRating, value); }

        private double racesCompletedPct;
        public double RacesCompletedPct { get => racesCompletedPct; set => SetValue(ref racesCompletedPct, value); }

        private int bestFinishPosition;
        public int BestFinishPosition { get => bestFinishPosition; set => SetValue(ref bestFinishPosition, value); }

        private int worstFinishPosition;
        public int WorstFinishPosition { get => worstFinishPosition; set => SetValue(ref worstFinishPosition, value); }

        private int firstRaceFinishPosition;
        public int FirstRaceFinishPosition { get => firstRaceFinishPosition; set => SetValue(ref firstRaceFinishPosition, value); }

        private int lastRaceFinishPosition;
        public int LastRaceFinishPosition { get => lastRaceFinishPosition; set => SetValue(ref lastRaceFinishPosition, value); }

        private int bestFinalPosition;
        public int BestFinalPosition { get => bestFinalPosition; set => SetValue(ref bestFinalPosition, value); }

        private int worstFinalPosition;
        public int WorstFinalPosition { get => worstFinalPosition; set => SetValue(ref worstFinalPosition, value); }

        private int firstRaceFinalPosition;
        public int FirstRaceFinalPosition { get => firstRaceFinalPosition; set => SetValue(ref firstRaceFinalPosition, value); }

        private int lastRaceFinalPosition;
        public int LastRaceFinalPosition { get => lastRaceFinalPosition; set => SetValue(ref lastRaceFinalPosition, value); }

        private int bestStartPosition;
        public int BestStartPosition { get => bestStartPosition; set => SetValue(ref bestStartPosition, value); }

        private int worstStartPosition;
        public int WorstStartPosition { get => worstStartPosition; set => SetValue(ref worstStartPosition, value); }

        private int firstRaceStartPosition;
        public int FirstRaceStartPosition { get => firstRaceStartPosition; set => SetValue(ref firstRaceStartPosition, value); }

        private int lastRaceStartPosition;
        public int LastRaceStartPosition { get => lastRaceStartPosition; set => SetValue(ref lastRaceStartPosition, value); }

        private int titles;
        public int Titles { get => titles; set => SetValue(ref titles, value); }

        private int hardChargerAwards;
        public int HardChargerAwards { get => hardChargerAwards; set => SetValue(ref hardChargerAwards, value); }

        private int cleanestDriverAwards;
        public int CleanestDriverAwards { get => cleanestDriverAwards; set => SetValue(ref cleanestDriverAwards, value); }

        public override long[] ModelId => new long[] { StatisticSetId, MemberId };
    }
}
