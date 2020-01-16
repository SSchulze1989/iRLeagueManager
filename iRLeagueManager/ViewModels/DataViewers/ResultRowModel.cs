using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.ViewModels;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using iRLeagueManager.Timing;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.ViewModels
{
    public class ResultRowModel : ContainerModelBase<IResultRow>, IResultRow
    {
        public int FinalPosition { get => Source.FinalPosition; set { Source.FinalPosition = value; NotifyPropertyChanged(); NotifyPropertyChanged(); } } 
        public int StartPosition { get => Source.StartPosition; set { Source.StartPosition = value; NotifyPropertyChanged(); } }
        public int FinishPosition { get => Source.FinishPosition; set { Source.FinishPosition = value; NotifyPropertyChanged(); } }
        [EqualityCheckProperty]
        public uint MemberId { get => Source.MemberId; set { Source.MemberId = value; NotifyPropertyChanged(); } }
        [EqualityCheckProperty]
        public int CarNumber { get => Source.CarNumber; set { Source.CarNumber = value; NotifyPropertyChanged(); } }
        public int ClassId { get => Source.ClassId; set { Source.ClassId = value; NotifyPropertyChanged(); } }
        public string Car { get => Source.Car; set { Source.Car = value; NotifyPropertyChanged(); } }
        public string CarClass { get => Source.CarClass; set { Source.CarClass = value; NotifyPropertyChanged(); } }
        public int CompletedLaps { get => Source.CompletedLaps; set { Source.CompletedLaps = value; NotifyPropertyChanged(); } }
        public int LeadLaps { get => Source.LeadLaps; set { Source.LeadLaps = value; NotifyPropertyChanged(); } }
        public int FastLapNr { get => Source.FastLapNr; set { Source.FastLapNr = value; NotifyPropertyChanged(); } }
        public int Incidents { get => Source.Incidents; set { Source.Incidents = value; NotifyPropertyChanged(); } }
        public RaceStatusEnum Status { get => Source.Status; set { Source.Status = value; NotifyPropertyChanged(); } }
        public int RacePoints { get => Source.RacePoints; set { Source.RacePoints = value; NotifyPropertyChanged(); } }
        public int BonusPoints { get => Source.BonusPoints; set { Source.BonusPoints = value; NotifyPropertyChanged(); } }
        public LapTime QualifyingTime { get => Source.QualifyingTime; set { Source.QualifyingTime = value; NotifyPropertyChanged(); } }
        public LapInterval Interval { get => Source.Interval; set { Source.Interval = value; NotifyPropertyChanged(); } }
        public LapTime AvgLapTime { get => Source.AvgLapTime; set { Source.AvgLapTime = value; NotifyPropertyChanged(); } }
        public LapTime FastestLapTime { get => Source.FastestLapTime; set { Source.FastestLapTime = value; NotifyPropertyChanged(); } }
        public int PositionChange { get => Source.PositionChange; set { Source.PositionChange = value; NotifyPropertyChanged(); } }
    }
}
