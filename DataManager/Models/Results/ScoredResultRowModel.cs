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

using iRLeagueManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Enums;
using iRLeagueManager.Attributes;
using iRLeagueManager.Timing;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Locations;

namespace iRLeagueManager.Models.Results
{
    public class ScoredResultRowModel : MappableModel
    {
        [EqualityCheckProperty]
        public long? ScoredResultRowId { get; internal set; }

        private int racePoints;
        public int RacePoints { get => racePoints; set => SetValue(ref racePoints, value); }

        private int bonusPoints;
        public int BonusPoints { get => bonusPoints; set => SetValue(ref bonusPoints, value); }

        private int penaltyPoints;
        public int PenaltyPoints { get => penaltyPoints; set => SetValue(ref penaltyPoints, value); }

        private int finalPosition;
        public int FinalPosition { get => finalPosition; set => SetValue(ref finalPosition, value); }

        public int TotalPoints => RacePoints + BonusPoints - PenaltyPoints;

        public int PositionChange => StartPosition - FinalPosition;

        public long? ResultRowId { get; internal set; }
        public long ResultId { get; set; }

        //public override long[] ModelId => new long[] { ResultRowId.GetValueOrDefault(), ResultId };

        //private int finalPosition;
        //public int FinalPosition { get => finalPosition; set { finalPosition = value; OnPropertyChanged(); } }

        private int startPosition;
        public int StartPosition { get => startPosition; set { startPosition = value; OnPropertyChanged(); } }

        private int finishPosition;
        public int FinishPosition { get => finishPosition; set { finishPosition = value; OnPropertyChanged(); } }

        private LeagueMember member;
        public LeagueMember Member { get => member; set { member = value; OnPropertyChanged(); OnPropertyChanged(nameof(MemberId)); } }

        private string teamName;
        public string TeamName { get => teamName; set => SetValue(ref teamName, value); }

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

        private LapTime qualifyingTime = new LapTime();
        public LapTime QualifyingTime { get => qualifyingTime; set { qualifyingTime = value; OnPropertyChanged(); } }

        private LapInterval interval;
        public LapInterval Interval { get => interval; set { interval = value; OnPropertyChanged(); } }

        private LapTime avgLapTime;
        public LapTime AvgLapTime { get => avgLapTime; set { avgLapTime = value; OnPropertyChanged(); } }

        private LapTime fastestLapTime;
        public LapTime FastestLapTime { get => fastestLapTime; set { fastestLapTime = value; OnPropertyChanged(); } }

        private Location location;
        public Location Location { get => location; set => SetValue(ref location, value); }

        private DateTime date;
        public DateTime Date { get => date; set => SetValue(ref date, value); }

        public override long[] ModelId => new long[] { ScoredResultRowId.GetValueOrDefault() };

        public ScoredResultRowModel() : base() { }

        public static ScoredResultRowModel GetTemplate()
        {
            var template = new ScoredResultRowModel
            {
                //template.CopyFrom(ResultRowModel.GetTemplate());
                RacePoints = 0,
                BonusPoints = 0,
                PenaltyPoints = 0,
                FinalPosition = 0,
                Member = Members.LeagueMember.GetTemplate()
            };

            return template;
        }
    }
}
