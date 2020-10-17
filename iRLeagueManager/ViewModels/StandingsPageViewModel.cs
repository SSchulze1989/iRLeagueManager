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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.ViewModels.Collections;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace iRLeagueManager.ViewModels
{
    public class StandingsPageViewModel : ViewModelBase, ISeasonPageViewModel
    {
        private SeasonModel season;

        private ObservableModelCollection<ScoringTableViewModel, ScoringTableModel> scoringTableList;
        public ICollectionView ScoringTableList
        {
            get => scoringTableList.CollectionView;
        }

        public StandingsPageViewModel()
        {
            scoringTableList = new ObservableModelCollection<ScoringTableViewModel, ScoringTableModel>();
        }

        protected void OnSessionSelectChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public async Task Load(iRLeagueManager.Models.SeasonModel season)
        {
            this.season = season;
            if (season == null)
                return;

            try
            {
                IsLoading = true;
                //var scoringsInfo = season.Scorings;
                var scoringTables = await LeagueContext.UpdateModelsAsync(season.ScoringTables);

                // Set scorings List
                //var scoringModels = await LeagueContext.GetModelsAsync<ScoringModel>(scoringsInfo.Select(x => x.ModelId));
                scoringTableList.UpdateSource(scoringTables);

                //foreach(var scoring in ScoringList)
                //{
                //    _ = scoring.LoadSessions();
                //}
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public override async Task Refresh()
        {
            LeagueContext.ModelManager.ForceExpireModels<StandingsModel>();
            await Load(season);
            var current = ScoringTableList.CurrentItem as ScoringTableViewModel;
            if (current != null)
            {
                await current.LoadStandings();
            }
            await base.Refresh();
        }
    }
}
