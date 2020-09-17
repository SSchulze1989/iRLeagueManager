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
    public class StandingsPageViewModel : ViewModelBase
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
