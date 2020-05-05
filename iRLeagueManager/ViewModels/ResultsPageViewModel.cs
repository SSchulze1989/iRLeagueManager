using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.ViewModels.Collections;

namespace iRLeagueManager.ViewModels
{
    public class ResultsPageViewModel : ViewModelBase
    {
        private ScheduleVMCollection scheduleList;
        public ScheduleVMCollection ScheduleList
        {
            get => scheduleList;
            protected set
            {
                if (SetValue(ref scheduleList, value, (t, v) => t.GetSource().Equals(v.GetSource())))
                {
                    OnPropertyChanged(null);
                }
            }
        }
        
        private ObservableCollection<SessionViewModel> sessionList;
        public ObservableCollection<SessionViewModel> SessionList { get => sessionList; set => SetValue(ref sessionList, value); }

        private ObservableCollection<ScoredResultViewModel> currentResults;
        public ObservableCollection<ScoredResultViewModel> CurrentResults { get => currentResults; set => SetValue(ref currentResults, value); }

        private ObservableCollection<ScoringViewModel> scoringList;
        public ObservableCollection<ScoringViewModel> ScoringList { get => scoringList; set => SetValue(ref scoringList, value); }

        public ResultsPageViewModel() : base()
        {
            ScheduleList = new ScheduleVMCollection();
        }

        public async Task Load(iRLeagueManager.Models.SeasonModel season)
        {
            if (season == null || season.Schedules.Count == 0)
            {
                return;
            }

            try
            {
                IsLoading = true;
                var schedules = await GlobalSettings.LeagueContext.GetModelsAsync<ScheduleModel>(season.Schedules.Select(x => x.ScheduleId.GetValueOrDefault()));
                ScheduleList.UpdateSource(schedules);
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
