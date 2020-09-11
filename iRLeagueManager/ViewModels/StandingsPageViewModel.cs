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

namespace iRLeagueManager.ViewModels
{
    public class StandingsPageViewModel : ViewModelBase
    {
        private ObservableModelCollection<ScoringTableViewModel, ScoringTableModel> scoringTableList;
        public ObservableModelCollection<ScoringTableViewModel, ScoringTableModel> ScoringTableList
        {
            get => scoringTableList;
            protected set
            {
                if (SetValue(ref scoringTableList, value, (t, v) => t.GetSource().Equals(v.GetSource())))
                {
                    OnPropertyChanged(null);
                }
            }
        }

        public StandingsPageViewModel()
        {
            ScoringTableList = new ObservableModelCollection<ScoringTableViewModel, ScoringTableModel>();
        }

        protected void OnSessionSelectChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public async Task Load(iRLeagueManager.Models.SeasonModel season)
        {
            try
            {
                IsLoading = true;
                //var scoringsInfo = season.Scorings;
                var scoringTables = await LeagueContext.UpdateModelsAsync(season.ScoringTables);

                // Set scorings List
                //var scoringModels = await LeagueContext.GetModelsAsync<ScoringModel>(scoringsInfo.Select(x => x.ModelId));
                ScoringTableList.UpdateSource(scoringTables);

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
    }
}
