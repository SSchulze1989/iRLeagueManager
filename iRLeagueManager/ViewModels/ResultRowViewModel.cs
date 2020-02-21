using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models.Results;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using iRLeagueManager.Timing;

namespace iRLeagueManager.ViewModels
{
    public class ResultRowViewModel : LeagueContainerModel<ResultRowModel>, IResultRow
    {
        public long ResultRowId => Source.ResultRowId.GetValueOrDefault();
        public int FinalPosition { get => Source.FinalPosition; set => Source.FinalPosition = value; }
        public int StartPosition { get => Source.StartPosition; set => Source.StartPosition = value; }
        public int FinishPosition { get => Source.FinishPosition; set => Source.FinishPosition = value; }

        public ILeagueMember Member => Source.Member;

        public int CarNumber { get => Source.CarNumber; set => Source.CarNumber = value; }
        public int ClassId { get => Source.ClassId; set => Source.ClassId = value; }
        public string Car { get => Source.Car; set => Source.Car = value; }
        public string CarClass { get => Source.CarClass; set => Source.CarClass = value; }
        public int CompletedLaps { get => Source.CompletedLaps; set => Source.CompletedLaps = value; }
        public int LeadLaps { get => Source.LeadLaps; set => Source.LeadLaps = value; }
        public int FastLapNr { get => Source.FastLapNr; set => Source.FastLapNr = value; }
        public int Incidents { get => Source.Incidents; set => Source.Incidents = value; }
        public RaceStatusEnum Status { get => Source.Status; set => Source.Status = value; }
        public int RacePoints { get => Source.RacePoints; set => Source.RacePoints = value; }
        public int BonusPoints { get => Source.BonusPoints; set => Source.BonusPoints = value; }
        public LapTime QualifyingTime { get => Source.QualifyingTime; set => Source.QualifyingTime = value; }
        public LapInterval Interval { get => Source.Interval; set => Source.Interval = value; }
        public LapTime AvgLapTime { get => Source.AvgLapTime; set => Source.AvgLapTime = value; }
        public LapTime FastestLapTime { get => Source.FastestLapTime; set => Source.FastestLapTime = value; }
        public int PositionChange { get => Source.PositionChange; set => Source.PositionChange = value; }
    }
}
