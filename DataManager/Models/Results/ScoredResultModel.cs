﻿// MIT License

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
using System.Collections.ObjectModel;

using iRLeagueManager.Attributes;
using iRLeagueManager.Models.Members;

namespace iRLeagueManager.Models.Results
{
    public class ScoredResultModel : ResultModel
    {
        public long? ScoredResultId { get; internal set; }

        private ScoringInfo scoring;
        public ScoringInfo Scoring { get => scoring; set => SetValue(ref scoring, value); }

        public string ScoringName { get; internal set; }

        [EqualityCheckProperty]
        public long? ScoringId => Scoring?.ScoringId;

        private ObservableCollection<ScoredResultRowModel> finalResults;
        public ObservableCollection<ScoredResultRowModel> FinalResults { get => finalResults; set => SetNotifyCollection(ref finalResults, value); }

        private ObservableCollection<LeagueMember> hardChargers;
        public ObservableCollection<LeagueMember> HardChargers { get => hardChargers; set => SetNotifyCollection(ref hardChargers, value); }

        private ObservableCollection<LeagueMember> cleanestDrivers;
        public ObservableCollection<LeagueMember> CleanestDrivers { get => cleanestDrivers; set => SetNotifyCollection(ref cleanestDrivers, value); }

        private LeagueMember fastestLapDriver;
        public LeagueMember FastestLapDriver { get => fastestLapDriver; set => SetValue(ref fastestLapDriver, value); }

        private LeagueMember fastestQualyLapDriver;
        public LeagueMember FastestQualyLapDriver { get => fastestQualyLapDriver; set => SetValue(ref fastestQualyLapDriver, value); }

        private LeagueMember fastestAvgLapDriver;
        public LeagueMember FastestAvgLapDriver { get => fastestAvgLapDriver; set => SetValue(ref fastestAvgLapDriver, value); }

        public override long[] ModelId => new long[] { ResultId.GetValueOrDefault(), ScoringId.GetValueOrDefault() };
        //public override long[] ModelId => new long[] { ScoredResultId.GetValueOrDefault() };

        public  ScoredResultModel() : base()
        {
            FinalResults = new ObservableCollection<ScoredResultRowModel>();
            CleanestDrivers = new ObservableCollection<LeagueMember>();
            HardChargers = new ObservableCollection<LeagueMember>();
        }

        public static ScoredResultModel GetTemplate()
        {
            var template = new ScoredResultModel();
            template.FinalResults.Add(ScoredResultRowModel.GetTemplate());

            return template;
        }
    }
}
