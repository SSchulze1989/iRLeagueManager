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

using iRLeagueManager.Models.Results;
using iRLeagueManager.ViewModels.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.ViewModels
{
    public class StandingsViewModel : LeagueContainerModel<StandingsModel>
    {
        //ScoringInfo Scoring => Model?.Scoring;
        public bool IsTeamStandings => Model is TeamStandingsModel;
        public IEnumerable<StandingsRowModel> StandingsRows => Model?.StandingsRows.OrderBy(x => -x.TotalPoints);
        public LeagueMember MostWinsDriver => Model?.MostWinsDriver;
        public LeagueMember MostPolesDriver => Model?.MostPolesDriver;
        public LeagueMember CleanestDriver => Model?.CleanestDriver;
        public LeagueMember MostPenaltiesDriver => Model?.MostPenaltiesDriver;

        protected override StandingsModel Template => new StandingsModel();

        public override Task Load(params long[] modelId)
        {
            if (Model == null || Model.ModelId.SequenceEqual(modelId) == false)
                return base.Load(modelId);
            else
                return base.Update();
        }
    }
}
