using iRLeagueManager.Models;
using iRLeagueManager.Models.Statistics;
using iRLeagueManager.ViewModels.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ViewModels
{
    public class StatsPageViewModel : ViewModelBase, ISeasonPageViewModel
    {
        private SeasonModel season;

        private readonly ObservableViewModelCollection<StatisticSetViewModel, StatisticSetModel> statisticiSets;
        public ICollectionView StatisticSets => statisticiSets.CollectionView;

        public StatsPageViewModel()
        {
            statisticiSets = new ObservableViewModelCollection<StatisticSetViewModel, StatisticSetModel>(x => StatisticSetViewModel.GetStatisticSetViewModel(x.GetType()));
        }

        public async Task Load(SeasonModel season)
        {
            this.season = season;
            if (season == null)
            {
                return;
            }

            try
            {
                IsLoading = true;
                var seasonStatisticSets = await LeagueContext.GetModelsAsync<SeasonStatisticSetModel>(season.SeasonStatisticSets.Select(x => x.ModelId));
                var leagueStatisticSets = await LeagueContext.GetModelsAsync<LeagueStatisticSetModel>();
                var loadedStatisticSets = seasonStatisticSets.Cast<StatisticSetModel>().Concat(leagueStatisticSets);
                statisticiSets.UpdateSource(loadedStatisticSets);
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
            foreach(var statisticSet in statisticiSets)
            {
                await statisticSet.Refresh();
            }
            await Load(season);
            await base.Refresh();
        }
    }
}
