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
using System.Collections.ObjectModel;
using System.Windows.Input;

using iRLeagueManager.Data;
using iRLeagueManager.Models;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Results;
using iRLeagueManager.ViewModels.Collections;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections;

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

        //private LeagueContext LeagueContext => GlobalSettings.LeagueContext;

        //private ObservableCollection<ScheduleInfo> scheduleList;
        //public ObservableCollection<ScheduleInfo> ScheduleList { get => scheduleList; set => SetValue(ref scheduleList, value); }
        private SeasonModel season;

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
                        SessionSelect.SessionList = new ReadOnlyObservableCollection<SessionViewModel>(new ObservableCollection<SessionViewModel>(ScheduleList.SelectMany(x => x.Sessions)));
                    else
                        SessionSelect.SessionList = selectedSchedule.Sessions;
                }
            }
        }

        private SessionSelectViewModel sessionSelect;
        public SessionSelectViewModel SessionSelect
        {
            get => sessionSelect;
            set
            {
                var temp = sessionSelect;
                if (SetValue(ref sessionSelect, value))
                {
                    if (temp != null)
                        temp.PropertyChanged -= OnSessionSelectChanged;
                    if (sessionSelect != null)
                        sessionSelect.PropertyChanged += OnSessionSelectChanged;
                }
            }
        }

        private ObservableCollection<ResultInfo> resultList;
        public ObservableCollection<ResultInfo> ResultList { get => resultList; set => SetValue(ref resultList, value); }

        private ObservableModelCollection<ScoredResultViewModel, ScoredResultModel> currentResults;
        public ICollectionView CurrentResults => currentResults.CollectionView;

        public SessionViewModel SelectedSession { get => SessionSelect?.SelectedSession; set => SessionSelect.SelectedSession = value; }

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

        public ICommand CalculateResultsCmd { get; }

        public ResultsPageViewModel() : base()
        {
            ScheduleList = new ScheduleVMCollection();
            resultList = new ObservableCollection<ResultInfo>();
            currentResults = new ObservableModelCollection<ScoredResultViewModel, ScoredResultModel>(src =>
                {
                    if (src is ScoredTeamResultModel)
                        return new ScoredTeamResultViewModel();
                    else
                        return new ScoredResultViewModel();
                },
                x => x.Scoring = ScoringList?.SingleOrDefault(y => y.ScoringId == x.Model?.Scoring?.ScoringId));
            ScoringList = new ObservableModelCollection<ScoringViewModel, ScoringModel>();
            //SessionList = new ObservableModelCollection<SessionViewModel, SessionModel>();
            SessionSelect = new SessionSelectViewModel()
            {
                SessionFilter = x => x.ResultAvailable
            };
            //SelectedResult = null;
            CalculateResultsCmd = new RelayCommand(o => CalculateResults(SelectedSession), o => SelectedSession != null && SelectedSession.ResultAvailable);
        }

        private void SetCurrentResultsView()
        {
            //var currentResultsViewSource = new CollectionViewSource()
            //{
            //    Source = currentResultsList
            //};

            //CurrentResults = currentResultsViewSource.View;
            //CurrentResults.Filter = x => (x != null && ((x as ScoredResultModel)?.FinalResults?.Count > 0 || (x as ScoredTeamResultModel)?.TeamResults?.Count > 0));
        }

        public async Task Load(iRLeagueManager.Models.SeasonModel season)
        {
            if (season == null)
                return;

            this.season = season;

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

                var lastSelectedSession = SelectedSession;

                if (SelectedSchedule == null)
                    SessionSelect.SessionList = new ReadOnlyObservableCollection<SessionViewModel>(new ObservableCollection<SessionViewModel>(ScheduleList.SelectMany(x => x.Sessions).OrderBy(x => x.Date)));
                else
                    SessionSelect.SessionList = SelectedSchedule.Sessions;

                // Set results List
                //ResultList = new ObservableCollection<ResultInfo>(scoringModels.Select(x => x.Results.AsEnumerable()).Aggregate((x, y) => x.Concat(y)));

                if (lastSelectedSession == null || !SessionSelect.SessionList.Contains(lastSelectedSession))
                    SelectedSession = SessionSelect.SessionList.Where(x => x.ResultAvailable).LastOrDefault();
                else
                    await LoadResults();

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
                OnPropertyChanged(null);
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

        public async Task LoadResults()
        {
            if (SelectedSession == null)
            {
                currentResults.UpdateSource(new ScoredResultModel[0]);
                //SelectedResult = null;
                StatusMsg = "No sessions available!";
                return;
            }
            else if (SelectedSession.Model.SessionResult == null)
            {
                currentResults.UpdateSource(new ScoredResultModel[0]);
                //SelectedResult = null;
                StatusMsg = "Results not yet available!";
                return;
            }

            try
            {
                IsLoading = true;
                // Load current Result
                var scoredResultModelIds = new List<long[]>();
                //SelectedResult = null;
                foreach (var scoring in ScoringList)
                {
                    var modelId = new long[] { SelectedSession.SessionId, scoring.ScoringId.GetValueOrDefault() };
                    scoredResultModelIds.Add(modelId);
                }
                var scoredResultModels = await LeagueContext.GetModelsAsync<ScoredResultModel>(scoredResultModelIds);

                var previousPosition = CurrentResults.CurrentPosition;
                currentResults.UpdateSource(scoredResultModels.Where(x => x != null && (x.FinalResults?.Count > 0 || (x as ScoredTeamResultModel)?.TeamResults?.Count > 0)));
                CurrentResults.MoveCurrentToPosition(previousPosition);
                if (CurrentResults.CurrentItem == null)
                    CurrentResults.MoveCurrentToFirst();
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

        protected async override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == nameof(SelectedSession))
            {
                await LoadResults();
            }
        }

        protected void OnSessionSelectChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e.PropertyName);
        }

        public async void CalculateResults(SessionViewModel session)
        {
            if (session == null)
                return;

            try
            {
                await LeagueContext.ModelContext.CalculateScoredResultsAsync(session.SessionId);
                await LoadResults();
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            OnPropertyChanged(null);
        }

        public override async Task Refresh()
        {
            LeagueContext.ModelManager.ForceExpireModels<ResultModel>();
            LeagueContext.ModelManager.ForceExpireModels<AddPenaltyModel>();
            await Load(season);
            await base.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            // Detach propertychanged events listened to
            if (SessionSelect != null)
                SessionSelect.PropertyChanged -= OnSessionSelectChanged;

            base.Dispose(disposing);
        }
    }
}
