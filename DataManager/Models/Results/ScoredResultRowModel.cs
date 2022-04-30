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
using iRLeagueManager.Models.Reviews;

namespace iRLeagueManager.Models.Results
{
    public class ScoredResultRowModel : ResultRowModel
    {
        [EqualityCheckProperty]
        public long? ScoredResultRowId { get; internal set; }

        //private int racePoints;
        //public int RacePoints { get => racePoints; set => SetValue(ref racePoints, value); }

        private int bonusPoints;
        public int BonusPoints { get => bonusPoints; set => SetValue(ref bonusPoints, value); }

        private int penaltyPoints;
        public int PenaltyPoints { get => penaltyPoints; set => SetValue(ref penaltyPoints, value); }

        private int finalPosition;
        public int FinalPosition { get => finalPosition; set => SetValue(ref finalPosition, value); }

        private int totalPoints;
        public int TotalPoints { get => totalPoints; set => SetValue(ref totalPoints, value); }

        private TimeSpan penaltyTime;
        public TimeSpan PenaltyTime { get => penaltyTime; set => SetValue(ref penaltyTime, value); }

        public new LapInterval Interval => base.Interval.Add(PenaltyTime);

        public override int PositionChange => StartPosition - FinalPosition;

        private DateTime date;
        public DateTime Date { get => date; set => SetValue(ref date, value); }

        private List<ReviewPenaltyModel> reviewPenalties;
        public List<ReviewPenaltyModel> ReviewPenalties { get => reviewPenalties; internal set => SetValue(ref reviewPenalties, value); }

        public override long[] ModelId => new long[] { ScoredResultRowId.GetValueOrDefault() };

        private bool isDroppedResult;
        public bool IsDroppedResult { get => isDroppedResult; set => SetValue(ref isDroppedResult, value); }

        public ScoredResultRowModel() : base() { }

        public new static ScoredResultRowModel GetTemplate()
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
