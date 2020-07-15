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
        private ObservableModelCollection<ScoringViewModel, ScoringModel> scoringList;
        public ObservableModelCollection<ScoringViewModel, ScoringModel> ScoringList
        {
            get => scoringList;
            protected set
            {
                if (SetValue(ref scoringList, value, (t, v) => t.GetSource().Equals(v.GetSource())))
                {
                    OnPropertyChanged(null);
                }
            }
        }

        public StandingsPageViewModel()
        {
            ScoringList = new ObservableModelCollection<ScoringViewModel, ScoringModel>();
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
                var scoringsInfo = season.Scorings;

                // Set scorings List
                var scoringModels = await LeagueContext.GetModelsAsync<ScoringModel>(scoringsInfo.Select(x => x.ModelId));
                ScoringList.UpdateSource(scoringModels);

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
