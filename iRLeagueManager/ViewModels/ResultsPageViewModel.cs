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
        //private ScheduleVMCollection scheduleList;
        //public ScheduleVMCollection ScheduleList
        //{
        //    get => scheduleList;
        //    protected set
        //    {
        //        if (SetValue(ref scheduleList, value, (t, v) => t.GetSource().Equals(v.GetSource())))
        //        {
        //            OnPropertyChanged(null);
        //        }
        //    }
        //}

        private ObservableCollection<ScheduleInfo> scheduleList;
        public ObservableCollection<ScheduleInfo> ScheduleList { get => scheduleList; set => SetValue(ref scheduleList, value); }
        
        private ObservableCollection<SessionInfo> sessionList;
        public ObservableCollection<SessionInfo> SessionList { get => sessionList; set => SetValue(ref sessionList, value); }

        private ObservableCollection<ScoredResultViewModel> currentResults;
        public ObservableCollection<ScoredResultViewModel> CurrentResults { get => currentResults; set => SetValue(ref currentResults, value); }

        private ObservableCollection<ScoringViewModel> scoringList;
        public ObservableCollection<ScoringViewModel> ScoringList { get => scoringList; set => SetValue(ref scoringList, value); }

        public ResultsPageViewModel() : base()
        {
            //ScheduleList = new ScheduleVMCollection();
            CurrentResults = new ObservableCollection<ScoredResultViewModel>();
        }

        public async void Load(iRLeagueManager.Models.SeasonModel season)
        {
            var schedules = season.Schedules;
            if (schedules.Count > 0)
            {
                List<Task> waitTasks = new List<Task>();
                var schedule = new ScheduleViewModel();
                var scoring = new ScoringViewModel();
                //waitTasks.Add(schedule.Load(schedules.First().ScheduleId.GetValueOrDefault()));
                waitTasks.Add(scoring.Load(1));
                //waitTasks.ForEach(x => x.Start());
                await Task.WhenAll(waitTasks);

                //var session = schedule.Sessions.OrderBy(x => x.Date).LastOrDefault();
                var sessions = scoring.Sessions.Select(async x => { var s = new SessionViewModel(); await s.Load(x.SessionId.GetValueOrDefault()); return s; });
                var session = await sessions.Last();

                ScoredResultViewModel scoredResult;
                if (CurrentResults.Count == 0)
                {
                    scoredResult = new ScoredResultViewModel();
                    CurrentResults.Add(scoredResult);
                }
                else
                {
                    scoredResult = CurrentResults.First();
                }
                await scoredResult.Load(session.SessionId.GetValueOrDefault(), scoring.ScoringId.GetValueOrDefault());
            }
        }
    }
}
