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
    public class ResultsPageViewModel : ViewModelBase, ISeasonPageViewModel
    {
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

        private ObservableViewModelCollection<ScoredResultViewModel, ScoredResultModel> currentResults;
        public ICollectionView CurrentResults => currentResults.CollectionView;

        public SessionViewModel SelectedSession { get => SessionSelect?.SelectedSession; set => SessionSelect.SelectedSession = value; }

        private readonly ObservableViewModelCollection<ScoringViewModel, ScoringModel> scoringList;
        public ICollectionView ScoringList => scoringList.CollectionView;

        public ICommand CalculateResultsCmd { get; }

        public ResultsPageViewModel() : base()
        {
            ScheduleList = new ScheduleVMCollection();
            resultList = new ObservableCollection<ResultInfo>();
            currentResults = new ObservableViewModelCollection<ScoredResultViewModel, ScoredResultModel>(src =>
                {
                    if (src is ScoredTeamResultModel)
                        return new ScoredTeamResultViewModel();
                    else
                        return new ScoredResultViewModel();
                },
                x => x.Scoring = scoringList?.SingleOrDefault(y => y.ScoringId == x.Model?.Scoring?.ScoringId));
            scoringList = new ObservableViewModelCollection<ScoringViewModel, ScoringModel>(x => x.Season = season);
            SessionSelect = new SessionSelectViewModel()
            {
                SessionFilter = x => x.ResultAvailable
            };

            CalculateResultsCmd = new RelayCommand(o => CalculateResults(SelectedSession), o => SelectedSession != null && SelectedSession.ResultAvailable);
        }

        public async Task Load(iRLeagueManager.Models.SeasonModel season)
        {
            if (season == null)
                return;

            this.season = season;

            try
            {
                IsLoading = true;
                
                var schedules = await LeagueContext.GetModelsAsync<ScheduleModel>(season.Schedules.Select(x => x.ModelId));
                var scoringsInfo = season.Scorings;

                // Set schedules List
                ScheduleList.UpdateSource(new ScheduleModel[] { null}.Concat(schedules));

                // Set scorings List
                var scoringModels = await LeagueContext.GetModelsAsync<ScoringModel>(scoringsInfo.Select(x => x.ModelId));
                scoringList.UpdateSource(scoringModels);

                var lastSelectedSession = SelectedSession;

                if (SelectedSchedule == null)
                    SessionSelect.SessionList = new ReadOnlyObservableCollection<SessionViewModel>(new ObservableCollection<SessionViewModel>(ScheduleList.SelectMany(x => x.Sessions).OrderBy(x => x.Date)));
                else
                    SessionSelect.SessionList = SelectedSchedule.Sessions;

                // Set results List
                if (lastSelectedSession == null || !SessionSelect.SessionList.Contains(lastSelectedSession))
                    SelectedSession = SessionSelect.SessionList.Where(x => x.ResultAvailable).LastOrDefault();
                else
                    await LoadResults();

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
                foreach (var scoring in scoringList)
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
            foreach(var result in currentResults)
            {
                await result.Refresh();
            }
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
