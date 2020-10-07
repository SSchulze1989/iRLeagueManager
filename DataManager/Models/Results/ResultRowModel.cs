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
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;
using iRLeagueManager.Timing;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Locations;

using iRLeagueManager.Attributes;

namespace iRLeagueManager.Models.Results
{
    public class ResultRowModel : MappableModel, IResultRow
    {
        [EqualityCheckProperty]
        public long? ResultRowId { get; internal set; }
        [EqualityCheckProperty]
        public long ResultId { get; set; }

        private SimSessionTypeEnum simSessionType;
        public SimSessionTypeEnum SimSessionType { get => simSessionType; set => SetValue(ref simSessionType, value); }

        //public override long[] ModelId => new long[] { ResultRowId.GetValueOrDefault(), ResultId };
        public override long[] ModelId => new long[] { ResultRowId.GetValueOrDefault() };

        //private int finalPosition;
        //public int FinalPosition { get => finalPosition; set { finalPosition = value; OnPropertyChanged(); } }

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

        //private int racePoints;
        //public int RacePoints { get => racePoints; set { racePoints = value; OnPropertyChanged(); } }

        //private int bonusPoints;
        //public int BonusPoints { get => bonusPoints; set { bonusPoints = value; OnPropertyChanged(); } }

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

        //private int positionChange;
        public virtual int PositionChange => StartPosition - FinishPosition;

        public ResultRowModel()
        {
            ResultRowId = null;
        }

        public ResultRowModel(long? resultRowId)
        {
            ResultRowId = resultRowId;
        }

        public static ResultRowModel GetTemplate()
        {
            var template = new ResultRowModel()
            {
                AvgLapTime = new LapTime(TimeSpan.FromMinutes(1.5)),
                Car = "DemoCar",
                CarClass = "Fast cars",
                CarNumber = 281,
                ClassId = 1,
                CompletedLaps = 10,
                FastestLapTime = new LapTime(TimeSpan.FromMinutes(1.4)),
                FastLapNr = 5,
                FinishPosition = 1,
                Incidents = 4,
                Interval = new LapInterval(TimeSpan.Zero),
                LeadLaps = 5,
                Member = LeagueMember.GetTemplate(),
                QualifyingTime = new LapTime(TimeSpan.FromMinutes(1.3)),
                StartPosition = 5
            };
            template.InitializeModel();
            
            return template;
        }
    }
}
