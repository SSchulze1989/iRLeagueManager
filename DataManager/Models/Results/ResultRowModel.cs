using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using iRLeagueManager.Timing;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using iRLeagueManager.Models.Members;

namespace iRLeagueManager.Models.Results
{
    public class ResultRowModel : ModelBase, IResultRow
    {
        public long? ResultRowId { get; }

        private int finalPosition;
        public int FinalPosition { get => finalPosition; set { finalPosition = value; OnPropertyChanged(); } }

        private int startPosition;
        public int StartPosition { get => startPosition; set { startPosition = value; OnPropertyChanged(); } }

        private int finishPosition;
        public int FinishPosition { get => finishPosition; set { finishPosition = value; OnPropertyChanged(); } }

        private LeagueMember member;
        public LeagueMember Member { get => member; set { member = value; OnPropertyChanged(); OnPropertyChanged(nameof(MemberId)); } }
        ILeagueMember IResultRow.Member => Member;

        public long? MemberId { get => Member?.MemberId; }

        private int carNumber;
        public int CarNumber { get => carNumber; set { carNumber = value; OnPropertyChanged(); } }

        private int classId;
        public int ClassId { get => classId; set { classId = value; OnPropertyChanged(); } }

        private string car;
        public string Car { get => car; set { car = value; OnPropertyChanged(); } }

        private string carClass;
        public string CarClass { get => carClass; set { carClass = value; OnPropertyChanged(); } }

        public int completedLaps;
        public int CompletedLaps { get => completedLaps; set { completedLaps = value; OnPropertyChanged(); } }

        private int leadLaps;
        public int LeadLaps { get => leadLaps; set { leadLaps = value; OnPropertyChanged(); } }

        private int fastLapNr;
        public int FastLapNr { get => fastLapNr; set { fastLapNr = value; OnPropertyChanged(); } }

        private int incidents;
        public int Incidents { get => incidents; set { incidents = value; OnPropertyChanged(); } }

        private RaceStatusEnum status;
        public RaceStatusEnum Status { get => status; set { status = value; OnPropertyChanged(); } }

        private int racePoints;
        public int RacePoints { get => racePoints; set { racePoints = value; OnPropertyChanged(); } }

        private int bonusPoints;
        public int BonusPoints { get => bonusPoints; set { bonusPoints = value; OnPropertyChanged(); } }

        private LapTime qualifyingTime = new LapTime();
        public LapTime QualifyingTime { get => qualifyingTime; set { qualifyingTime = value; OnPropertyChanged(); } }

        private LapInterval interval;
        public LapInterval Interval { get => interval; set { interval = value; OnPropertyChanged(); } }

        private LapTime avgLapTime;
        public LapTime AvgLapTime { get => avgLapTime; set { avgLapTime = value; OnPropertyChanged(); } }

        private LapTime fastestLapTime;
        public LapTime FastestLapTime { get => fastestLapTime; set { fastestLapTime = value; OnPropertyChanged(); } }

        private int positionChange;
        public int PositionChange { get => positionChange; set { positionChange = value; OnPropertyChanged(); } }

        public ResultRowModel()
        {
            ResultRowId = null;
        }

        public ResultRowModel(long? resultRowId)
        {
            ResultRowId = resultRowId;
        }
    }
}
