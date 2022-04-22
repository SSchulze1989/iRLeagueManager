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

using iRLeagueManager.Models.Sessions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations.Builders;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Enums;

namespace iRLeagueManager.Models.Results
{
    public class ScoringTableModel : MappableModel, ICacheableModel
    {
        public long ScoringTableId { get; set; }
        public override long[] ModelId => new long[] { ScoringTableId };

        private ScoringKindEnum scoringKind;
        public ScoringKindEnum ScoringKind { get => scoringKind; set => SetValue(ref scoringKind, value); }

        private DropRacesOption dropRacesOption;
        public DropRacesOption DropRacesOption { get => dropRacesOption; set => SetValue(ref dropRacesOption, value); }

        private int resultsPerRaceCount;
        public int ResultsPerRaceCount { get => resultsPerRaceCount; set => SetValue(ref resultsPerRaceCount, value); }

        private string name;
        public string Name { get => name; set => SetValue(ref name, value); }

        private int dropWeeks;
        public int DropWeeks { get => dropWeeks; set => SetValue(ref dropWeeks, value); }

        private int averageRaceNr;
        public int AverageRaceNr { get => averageRaceNr; set => SetValue(ref averageRaceNr, value); }

        private ObservableCollection<MyKeyValuePair<ScoringInfo, double>> scorings;
        public ObservableCollection<MyKeyValuePair<ScoringInfo, double>> Scorings { get => scorings; set => SetNotifyCollection(ref scorings, value); }

        private ObservableCollection<SessionInfo> sessions;
        public ObservableCollection<SessionInfo> Sessions { get => sessions; set => SetValue(ref sessions, value); }

        private ObservableCollection<long> standingsFilterOptionIds;
        public ObservableCollection<long> StandingsFilterOptionIds { get => standingsFilterOptionIds; set => SetValue(ref standingsFilterOptionIds, value); }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name) == false)
            {
                return Name;
            }
            else
            {
                return base.ToString();
            }
        }

        public ScoringTableModel() : base()
        {
            Scorings = new ObservableCollection<MyKeyValuePair<ScoringInfo, double>>();
            Sessions = new ObservableCollection<SessionInfo>();
            StandingsFilterOptionIds = new ObservableCollection<long>();
        }
    }
}
