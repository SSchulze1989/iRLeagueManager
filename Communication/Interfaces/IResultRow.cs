using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Timing;
using iRLeagueManager.Enums;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.Interfaces
{
    public interface IResultRow
    {
        //int FinalPosition { get; set; }
        int StartPosition { get; set; }
        int FinishPosition { get; set; }
        [EqualityCheckProperty]
        //int MemberId { get; }
        ILeagueMember Member { get; }
        [EqualityCheckProperty]
        int CarNumber { get; set; }
        int ClassId { get; set; }
        string Car { get; set; }
        string CarClass { get; set; }
        int CompletedLaps { get; set; }
        int LeadLaps { get; set; }
        int FastLapNr { get; set; }
        int Incidents { get; set; }
        RaceStatusEnum Status { get; set; }
        //int RacePoints { get; set; }
        //int BonusPoints { get; set; }
        LapTime QualifyingTime { get; set; }
        LapInterval Interval { get; set; }
        LapTime AvgLapTime { get; set; }
        LapTime FastestLapTime { get; set; }
        //int PositionChange { get; set; }
    }
}
