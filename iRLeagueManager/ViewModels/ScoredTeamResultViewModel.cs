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
using System.ComponentModel;
using System.Collections;
using System.Windows.Data;
using System.Collections.ObjectModel;

using iRLeagueManager.Models.Results;
using iRLeagueManager.ViewModels.Collections;

namespace iRLeagueManager.ViewModels
{
    public class ScoredTeamResultViewModel : ScoredResultViewModel, IContainerModelBase<ScoredTeamResultModel>
    {
        public new ScoredTeamResultModel Model { get => base.Model as ScoredTeamResultModel; set => base.Model = value; }

        private readonly ObservableViewModelCollection<ScoredTeamResultRowViewModel, ScoredTeamResultRowModel> teamResults;
        public ObservableViewModelCollection<ScoredTeamResultRowViewModel, ScoredTeamResultRowModel> TeamResults
        {
            get
            {
                if (teamResults.GetSource() != Model?.TeamResults)
                    teamResults.UpdateSource(Model?.TeamResults);
                return teamResults;
            }
        }

        public ScoredTeamResultViewModel()
        {
            teamResults = new ObservableViewModelCollection<ScoredTeamResultRowViewModel, ScoredTeamResultRowModel>();
        }

        public bool UpdateSource(ScoredTeamResultModel source)
        {
            return base.UpdateSource(source);
        }

        public override void OnUpdateSource()
        {
            base.OnUpdateSource();
        }

        ScoredTeamResultModel IContainerModelBase<ScoredTeamResultModel>.GetSource()
        {
            return base.GetSource() as ScoredTeamResultModel;
        }
    }
}
