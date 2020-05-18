using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;

using iRLeagueManager.Data;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Results;
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

        private LeagueContext LeagueContext => GlobalSettings.LeagueContext;

        //private ObservableCollection<ScheduleInfo> scheduleList;
        //public ObservableCollection<ScheduleInfo> ScheduleList { get => scheduleList; set => SetValue(ref scheduleList, value); }
        private ScheduleVMCollection scheduleList;
        public ScheduleVMCollection ScheduleList { get => scheduleList; set => SetValue(ref scheduleList, value); }

        private ScheduleViewModel selectedSchedule;
        public ScheduleViewModel SelectedSchedule
        {
            get => selectedSchedule;
            set
            {
                if (SetValue(ref selectedSchedule, value))
                {
                    if (selectedSchedule == null || selectedSchedule.Model == null)
                        SessionList = new ObservableCollection<SessionViewModel>(ScheduleList.SelectMany(x => x.Sessions));
                    else
                        SessionList = selectedSchedule.Sessions;
                }
            }
        }
        
        //private ObservableModelCollection<SessionViewModel, SessionModel> sessionList;
        //public ObservableModelCollection<SessionViewModel, SessionModel> SessionList { get => sessionList; set => SetValue(ref sessionList, value); }
        private ObservableCollection<SessionViewModel> sessionList;
        public ObservableCollection<SessionViewModel> SessionList
        {
            get => sessionList;
            set
            {
                if (SetValue(ref sessionList, value))
                {
                    if (SelectedSession == null)
                        SelectedSession = null;
                }
            }
        }

        private SessionViewModel selectedSession;
        public SessionViewModel SelectedSession
        {
            get => selectedSession;
            set
            {
                if (value == null)
                    value = SessionList.Where(x => x.ResultAvailable).LastOrDefault();
                if (SetValue(ref selectedSession, value))
                {
                    _ = LoadResults();
                }
            }
        }

        private ObservableCollection<ResultInfo> resultList;
        public ObservableCollection<ResultInfo> ResultList { get => resultList; set => SetValue(ref resultList, value); }

        private ScoredResultViewModel selectedResult;
        public ScoredResultViewModel SelectedResult { get => selectedResult; set => SetValue(ref selectedResult, value); }

        private ObservableModelCollection<ScoredResultViewModel, ScoredResultModel> currentResults;
        public ObservableModelCollection<ScoredResultViewModel, ScoredResultModel> CurrentResults
        {
            get => currentResults;
            set
            {
                if (SetValue(ref currentResults, value, (t, v) => t.GetSource().Equals(v.GetSource())))
                {
                    OnPropertyChanged(null);
                }
            }
        }

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

        private string statusMsg;
        public string StatusMsg { get => statusMsg; set => SetValue(ref statusMsg, value); }

        public ICommand NextSessionCmd { get; }
        public ICommand PreviousSessionCmd { get; }

        public ResultsPageViewModel() : base()
        {
            ScheduleList = new ScheduleVMCollection();
            resultList = new ObservableCollection<ResultInfo>();
            CurrentResults = new ObservableModelCollection<ScoredResultViewModel, ScoredResultModel>();
            ScoringList = new ObservableModelCollection<ScoringViewModel, ScoringModel>();
            //SessionList = new ObservableModelCollection<SessionViewModel, SessionModel>();
            SessionList = new ObservableCollection<SessionViewModel>();
            SelectedResult = null;
            NextSessionCmd = new RelayCommand(o => SelectNextSession(), o => CanSelectNextSession());
            PreviousSessionCmd = new RelayCommand(o => SelectPreviousSession(), o => CanSelectPreviousSession());
        }

        public async Task Load(iRLeagueManager.Models.SeasonModel season)
        {
            try
            {
                IsLoading = true;
                //await LeagueContext.UpdateMemberList();
                var schedules = await LeagueContext.GetModelsAsync<ScheduleModel>(season.Schedules.Select(x => x.ModelId));
                var scoringsInfo = season.Scorings;

                // Set schedules List
                ScheduleList.UpdateSource(new ScheduleModel[] { null}.Concat(schedules));

                // Set scorings List
                var scoringModels = await LeagueContext.GetModelsAsync<ScoringModel>(scoringsInfo.Select(x => x.ModelId));
                ScoringList.UpdateSource(scoringModels);

                // Set session List
                //var sessionsInfo = ScoringList.SelectMany(x => x.Sessions);
                //var sessionModelIds = sessionsInfo.Select(x => x.ModelId);
                //var sessionModels = await LeagueContext.GetModelsAsync<SessionModel>(sessionModelIds);

                if (SelectedSchedule == null)
                    SessionList = new ObservableCollection<SessionViewModel>(ScheduleList.SelectMany(x => x.Sessions));
                else
                    SessionList = SelectedSchedule.Sessions;

                // Set results List
                //ResultList = new ObservableCollection<ResultInfo>(scoringModels.Select(x => x.Results.AsEnumerable()).Aggregate((x, y) => x.Concat(y)));

                if (SelectedSession == null || !SessionList.Contains(SelectedSession))
                    SelectedSession = SessionList.Where(x => x.ResultAvailable).LastOrDefault();

                //// Load current Result
                //var scoredResultModelIds = new List<long[]>();
                //foreach (var scoring in ScoringList)
                //{
                //    var modelId = new long[] { SelectedSession.SessionId.GetValueOrDefault(), scoring.ScoringId.GetValueOrDefault() };
                //    scoredResultModelIds.Add(modelId);
                //}
                //var scoredResultModels = await LeagueContext.GetModelsAsync<ScoredResultModel>(scoredResultModelIds);
                //CurrentResults.UpdateSource(scoredResultModels);
                //SelectedResult = CurrentResults.FirstOrDefault();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }

            //if (schedules.Count > 0)
            //{
            //    List<Task> waitTasks = new List<Task>();
            //    var scoring = new ScoringViewModel();

            //    waitTasks.Add(scoring.Load(1));
            //    //waitTasks.ForEach(x => x.Start());
            //    await Task.WhenAll(waitTasks);

            //    //var session = schedule.Sessions.OrderBy(x => x.Date).LastOrDefault();
            //    var sessions = scoring.Sessions.Select(async x => { var s = new SessionViewModel(); await s.Load(x.SessionId.GetValueOrDefault()); return s; });
            //    var session = await sessions.Last();

            //    ScoredResultViewModel scoredResult;
            //    if (CurrentResults.Count == 0)
            //    {
            //        scoredResult = new ScoredResultViewModel();
            //        CurrentResults.Add(scoredResult);
            //    }
            //    else
            //    {
            //        scoredResult = CurrentResults.First();
            //    }
            //    await scoredResult.Load(session.SessionId.GetValueOrDefault(), scoring.ScoringId.GetValueOrDefault());
            //}
        }

        private async Task LoadResults()
        {
            if (SelectedSession == null)
            {
                CurrentResults.UpdateSource(new ScoredResultModel[0]);
                SelectedResult = null;
                StatusMsg = "No sessions available!";
                return;
            }
            else if (SelectedSession.Model.SessionResult == null)
            {
                CurrentResults.UpdateSource(new ScoredResultModel[0]);
                SelectedResult = null;
                StatusMsg = "Results not yet available!";
                return;
            }

            try
            {
                IsLoading = true;
                // Load current Result
                var scoredResultModelIds = new List<long[]>();
                foreach (var scoring in ScoringList)
                {
                    var modelId = new long[] { SelectedSession.SessionId.GetValueOrDefault(), scoring.ScoringId.GetValueOrDefault() };
                    scoredResultModelIds.Add(modelId);
                }
                var scoredResultModels = await LeagueContext.GetModelsAsync<ScoredResultModel>(scoredResultModelIds);
                CurrentResults.UpdateSource(scoredResultModels);
                SelectedResult = CurrentResults.FirstOrDefault();
                StatusMsg = "";
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

        public void SelectNextSession()
        {
            var currentSessionIndex = SessionList.IndexOf(SelectedSession);
            if (currentSessionIndex == -1)
                currentSessionIndex = 0;

            currentSessionIndex++;
            if (currentSessionIndex >= SessionList.Count())
                SelectedSession = SessionList.LastOrDefault();
            else
                SelectedSession = SessionList.ElementAt(currentSessionIndex);
        }

        public bool CanSelectNextSession()
        {
            var currentSessionIndex = SessionList.IndexOf(SelectedSession);

            if (currentSessionIndex < SessionList.Count()-1 || currentSessionIndex == -1)
                return true;

            return false;
        }

        public void SelectPreviousSession()
        {
            var currentSessionIndex = SessionList.IndexOf(SelectedSession);
            if (currentSessionIndex == -1)
                currentSessionIndex = SessionList.Count();

            currentSessionIndex--;
            if (currentSessionIndex < 0)
                SelectedSession = SessionList.LastOrDefault();
            else
                SelectedSession = SessionList.ElementAt(currentSessionIndex);
        }

        public bool CanSelectPreviousSession()
        {
            var currentSessionIndex = SessionList.IndexOf(SelectedSession);

            if (currentSessionIndex > 0 || currentSessionIndex == -1)
                return true;

            return false;
        }
    }
}
