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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using iRLeagueManager.Models.Members;
using System.ComponentModel.DataAnnotations;

namespace iRLeagueManager.Models.Results
{
    public class StandingsModel : MappableModel
    {
        //private ScoringInfo scoring;
        //public ScoringInfo Scoring { get => scoring; internal set => SetValue(ref scoring, value); }

        private long scoringTableId;
        public long ScoringTableId { get => scoringTableId; set => SetValue(ref scoringTableId, value); }

        private long? sessionId;
        public long? SessionId { get => sessionId; internal set => SetValue(ref sessionId, value); }

        public override long[] ModelId => new long[] { ScoringTableId, sessionId.GetValueOrDefault() };

        private ObservableCollection<StandingsRowModel> standingsRows;
        public ObservableCollection<StandingsRowModel> StandingsRows { get => standingsRows; internal set => SetValue(ref standingsRows, value); }

        private LeagueMember mostWinsDriver;
        public LeagueMember MostWinsDriver { get => mostWinsDriver; internal set => SetValue(ref mostWinsDriver, value); }

        private LeagueMember mostPolesDriver;
        public LeagueMember MostPolesDriver { get => mostPolesDriver; internal set => SetValue(ref mostPolesDriver, value); }

        private LeagueMember cleanestDriver;
        public LeagueMember CleanestDriver { get => cleanestDriver; internal set => SetValue(ref cleanestDriver, value); }

        private LeagueMember mostPenaltiesDriver;
        public LeagueMember MostPenaltiesDriver { get => mostPenaltiesDriver; internal set => SetValue(ref mostPenaltiesDriver, value); }

        public StandingsModel()
        {
            StandingsRows = new ObservableCollection<StandingsRowModel>();
        }
    }
}
