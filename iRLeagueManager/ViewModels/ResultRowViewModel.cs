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

using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using iRLeagueManager.Timing;

namespace iRLeagueManager.ViewModels
{
    public class ResultRowViewModel : LeagueContainerModel<ResultRowModel>, IResultRow
    {
        //public ResultRowModel Model
        //{
        //    get => Source;
        //    set => SetSource(value);
        //}

        protected override ResultRowModel Template => new ResultRowModel();

        public long ResultRowId => Source.ResultRowId.GetValueOrDefault();
        //public int FinalPosition { get => Source.FinalPosition; set => Source.FinalPosition = value; }
        public int StartPosition { get => Source.StartPosition; set => Source.StartPosition = value; }
        public int FinishPosition { get => Source.FinishPosition; set => Source.FinishPosition = value; }

        public LeagueMember Member => Source.Member;
        ILeagueMember IResultRow.Member => Source.Member;

        public int CarNumber { get => Source.CarNumber; set => Source.CarNumber = value; }
        public int ClassId { get => Source.ClassId; set => Source.ClassId = value; }
        public string Car { get => Source.Car; set => Source.Car = value; }
        public string CarClass { get => Source.CarClass; set => Source.CarClass = value; }
        public int CompletedLaps { get => Source.CompletedLaps; set => Source.CompletedLaps = value; }
        public int LeadLaps { get => Source.LeadLaps; set => Source.LeadLaps = value; }
        public int FastLapNr { get => Source.FastLapNr; set => Source.FastLapNr = value; }
        public int Incidents { get => Source.Incidents; set => Source.Incidents = value; }
        public RaceStatusEnum Status { get => Source.Status; set => Source.Status = value; }
        //public int RacePoints { get => Source.RacePoints; set => Source.RacePoints = value; }
        //public int BonusPoints { get => Source.BonusPoints; set => Source.BonusPoints = value; }
        public LapTime QualifyingTime { get => Source.QualifyingTime; set => Source.QualifyingTime = value; }
        public virtual LapInterval Interval { get => Source.Interval; set => Source.Interval = value; }
        public LapTime AvgLapTime { get => Source.AvgLapTime; set => Source.AvgLapTime = value; }
        public LapTime FastestLapTime { get => Source.FastestLapTime; set => Source.FastestLapTime = value; }
        public int PositionChange { get => Source.PositionChange; }
        public TeamModel Team { get => Model.Team; set => Model.Team = value; }

        public int OldIRating => Model.OldIRating;
        public int NewIRating => Model.NewIRating;
        public double OldSafetyRating => Model.OldSafetyRating;
        public double NewSafetyRating => Model.NewSafetyRating;
        public bool Disqualified { get => Model.Disqualified; set => Model.Disqualified = value; }
    }
}
