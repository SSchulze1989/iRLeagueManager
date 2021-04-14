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
using System.Runtime.CompilerServices;
using System.Windows.Data;

using iRLeagueManager.Models;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.User;
using System.ComponentModel;
using iRLeagueManager.ViewModels.Collections;
using System.Security.Policy;
using iRLeagueManager.Enums;
using iRLeagueManager.Models.Filters;
using iRLeagueDatabase.Enums;
using iRLeagueManager.Extensions;
using System.Windows.Input;
using System.Collections;

namespace iRLeagueManager.ViewModels
{
    public class ScoringViewModel : LeagueContainerModel<ScoringModel>
    {
        protected override ScoringModel Template => new ScoringModel();
        public long? ScoringId => Model?.ScoringId;
        public ScoringViewModel parentScoring;
        public ScoringViewModel ParentScoring { get => parentScoring; set => SetValue(ref parentScoring, value); }
        public ScoringKindEnum ScoringKind { get => Model.ScoringKind; set => Model.ScoringKind = value; }
        public SessionType ScoringSessionType { get => Model.ScoringSessionType; set => Model.ScoringSessionType = value; }
        public ScoringSessionSelectionEnum SessionSelectType { get => Model.SessionSelectType; set => Model.SessionSelectType = value; }
        public string Name { get => Model.Name; set => Model.Name = value; }
        public string Description { get => Model.Description; set => Model.Description = value; }
        public int DropWeeks { get => Model.DropWeeks; set => Model.DropWeeks = value; }
        public int AverageRaceNr { get => Model.AverageRaceNr; set => Model.AverageRaceNr = value; }
        private readonly ObservableViewModelCollection<SessionViewModel, SessionModel> sessions;
        public ICollectionView Sessions
        {
            get
            {
                if (sessions.GetSource() != Model.Sessions)
                {
                    sessions.UpdateSource(Model.Sessions);
                    if (sessions.CollectionView.CanFilter)
                    {
                        sessions.CollectionView.Refresh();
                    }
                }
                return sessions.CollectionView;
            }
        }
        public long SeasonId => Model.SeasonId;
        public SeasonModel Season { get => Model.Season; set => Model.Season = value; }
        public ObservableCollection<ScoringModel.BasePointsValue> BasePoints => Model.BasePoints;
        public ObservableCollection<ScoringModel.BonusPointsValue> BonusPoints => Model.BonusPoints;
        public ObservableCollection<ScoringModel.IncidentPointsValue> IncPenaltyPoints => Model.IncPenaltyPoints;
        public ObservableCollection<MyKeyValuePair<ScoringInfo, double>> MultiScoringResults => Model.MultiScoringResults;
        public bool IsMultiScoring { get => Model.IsMultiScoring; set => Model.IsMultiScoring = value; }
        public int MaxResultsPerGroup { get => Model.MaxResultsPerGroup; set => Model.MaxResultsPerGroup = value; }
        public bool TakeGroupAverage { get => Model.TakeGroupAverage; set => Model.TakeGroupAverage = value; }
        public ScoringInfo ExtScoringSource { get => Model.ExtScoringSource; set => Model.ExtScoringSource = value; }
        public bool TakeResultsFromExtSource { get => Model.TakeResultsFromExtSource; set => Model.TakeResultsFromExtSource = value; }
        public bool ShowResults { get => Model.ShowResults; set => Model.ShowResults = value; }
        public ObservableCollection<long> ResultsFilterOptionIds => Model.ResultsFilterOptionIds;
        public bool UseResultSetTeam { get => Model.UseResultSetTeam; set => Model.UseResultSetTeam = value; }
        public bool UpdateTeamOnRecalculation { get => Model.UpdateTeamOnRecalculation; set => Model.UpdateTeamOnRecalculation = value; }
        public ObservableViewModelCollection<ScoringViewModel, ScoringModel> subSessionScorings;
        public ICollectionView SubSessionScorings
        {
            get
            {
                if (Model != null && subSessionScorings.GetSource() != Model.SubSessionScorings)
                {
                    subSessionScorings.UpdateSource(Model.SubSessionScorings);
                }
                return subSessionScorings.CollectionView;
            }
        }
        public ObservableCollection<MyKeyValuePair<ScoringModel, double>> ScoringWeights => Model.ScoringWeights;
        public bool AccumulateScoring { get => Model.AccumulateScoring; set => Model.AccumulateScoring = value; }
        public AccumulateByOption AccumulateBy { get => Model.AccumulateBy; set => Model.AccumulateBy = value; }
        public AccumulateResultsOption AccumulateResults { get => Model.AccumulateResults; set => Model.AccumulateResults = value; }

        public MyKeyValuePair<ScoringModel, double> ScoringWeight
        {
            get
            {
                var weight = ParentScoring?.ScoringWeights.SingleOrDefault(x => x.Key == Model);
                if (weight == null)
                {
                    weight = new MyKeyValuePair<ScoringModel, double>(Model, 0);
                    ParentScoring?.ScoringWeights.Add(weight);
                }
                return weight;
            }
        }

        private CollectionViewSource scoringListSource;
        public ICollectionView ScoringList
        {
            get
            {
                var view = scoringListSource.View;
                view.Filter = x => ((ScoringModel)x).ScoringId != this.ScoringId && SubSessionScorings.Contains(x) == false;
                return view;
            }
        }

        private SessionSelectViewModel sessionSelect;
        public SessionSelectViewModel SessionSelect
        {
            get
            {
                if (sessionSelect != null)
                    _ = sessionSelect.LoadSessions(Model?.Sessions);
                return sessionSelect;
            }
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

        private StandingsViewModel standings = new StandingsViewModel();
        public StandingsViewModel Standings
        {
            get
            {
                if (standings == null)
                    standings = new StandingsViewModel();

                //_ = standings.Load(ScoringId.GetValueOrDefault(), SessionSelect.SelectedSession.SessionId);
                //_ = LoadStandings();
                return standings;
            }
        }

        public ScheduleInfo ConnectedSchedule
        {
            get
            {
                var schedule = Season?.Schedules?.SingleOrDefault(x => x.ScheduleId == Model?.ConnectedSchedule?.ScheduleId);
                if (schedule == null)
                    schedule = Model.ConnectedSchedule;
                return schedule;
            }
            set => Model.ConnectedSchedule = value;
        }

        public ICommand AddSessionsCmd { get; }
        public ICommand RemoveSessionsCmd { get; }
        public ICommand AddSubScoringsCmd { get; }
        public ICommand RemoveSubScoringsCmd { get; }

        public ScoringViewModel()
        {
            Model = Template;
            SessionSelect = new SessionSelectViewModel()
            {
                SessionFilter = session => session.ResultAvailable
            };
            sessions = new ObservableViewModelCollection<SessionViewModel, SessionModel>();
            var sessionGroupDescription = new PropertyGroupDescription(nameof(SessionViewModel.ParentSessionName));
            Sessions.GroupDescriptions.Add(sessionGroupDescription);
            subSessionScorings = new ObservableViewModelCollection<ScoringViewModel, ScoringModel>(x => x.ParentScoring = this);
            AddSessionsCmd = new RelayCommand(o =>
            {
                if (o is IList selected)
                {
                    var sessions = selected.OfType<SessionViewModel>().Select(x => x.Model);
                    AddSessions(sessions.ToArray());
                }
                else if (o is SessionViewModel sessionViewModel)
                {
                    AddSessions(sessionViewModel.Model);
                }
            }, o => o is SessionViewModel || (o is IList selected && selected.OfType<SessionViewModel>().Count() > 0));
            RemoveSessionsCmd = new RelayCommand(o =>
            {
                if (o is IList selected)
                {
                    var sessions = selected.OfType<SessionViewModel>().Select(x => x.Model);
                    RemoveSessions(sessions?.ToArray());
                }
                else if (o is SessionViewModel sessionViewModel)
                {
                    RemoveSessions(sessionViewModel.Model);
                }
            }, o => o is SessionViewModel || (o is IList selected && selected.OfType<SessionViewModel>().Count() > 0));
            AddSubScoringsCmd = new RelayCommand(o =>
            {
                if (o is IList selected)
                {
                    var scorings = selected.OfType<ScoringModel>();
                    AddSubScorings(scorings?.ToArray());
                }
                else if (o is ScoringModel scoringModel)
                {
                    AddSubScorings(scoringModel);
                }
            }, o => o is ScoringModel || (o is IList selected && selected.OfType<ScoringModel>().Count() > 0));
            RemoveSubScoringsCmd = new RelayCommand(o =>
            {
                if (o is IList selected)
                {
                    var scorings = selected.OfType<ScoringViewModel>().Select(x => x.Model);
                    RemoveSubScorings(scorings?.ToArray());
                }
                else if (o is ScoringViewModel scoringViewModel)
                {
                    RemoveSubScorings(scoringViewModel.Model);
                }
            }, o => o is ScoringViewModel || (o is IList selected && selected.OfType<ScoringViewModel>().Count() > 0));
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName == nameof(Season) || propertyName == null)
            {
                if (Season != null && ConnectedSchedule != null && Season.Schedules.Any(x => x.ScheduleId == ConnectedSchedule.ScheduleId))
                {
                    //ConnectedSchedule = Season.Schedules.SingleOrDefault(x => x.ScheduleId == ConnectedSchedule.ScheduleId);
                }
            }

            base.OnPropertyChanged(propertyName);
        }

        protected async void OnSessionSelectChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SessionSelect.SelectedSession))
                await LoadStandings(); 
        }

        public async Task LoadStandings()
        {
            //await Task.Yield();
            try
            {
                //await standings.Load(ScoringId.GetValueOrDefault());
                if (SessionSelect?.SelectedSession != null)
                {
                    if (SessionSelect.FilteredSessions.Contains(SessionSelect.SelectedSession) == false)
                        SessionSelect.SelectedSession = SessionSelect.FilteredSessions.LastOrDefault();

                    await standings.Load(ScoringId.GetValueOrDefault(), SessionSelect.SelectedSession.SessionId);
                }
                else
                    await standings.Load(ScoringId.GetValueOrDefault(), 0);
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

        public void AddSessions(params SessionModel[] sessions)
        {
            if (sessions == null || sessions.Count() == 0)
            {
                return;
            }

            foreach (var session in sessions)
            {
                if (Model.Sessions.Contains(session) == false)
                {
                    Model.Sessions.Add(session);
                }
            }
        }

        public void RemoveSessions(params SessionModel[] sessions)
        {
            if (sessions == null || sessions.Count() == 0)
            {
                return;
            }

            foreach(var session in sessions)
            {
                Model.Sessions.Remove(session);
            }
        }

        public void AddSubScorings(params ScoringModel[] scorings)
        {
            if (scorings == null || scorings.Count() == 0)
            {
                return;
            }

            foreach(var scoring in scorings)
            {
                if (Model.SubSessionScorings.Contains(scoring) == false)
                {
                    Model.SubSessionScorings.Add(scoring);
                }
            }
        }

        public void RemoveSubScorings(params ScoringModel[] scorings)
        {
            if (scorings == null || scorings.Count() == 0)
            {
                return;
            }

            foreach(var scoring in scorings)
            {
                Model.SubSessionScorings.Remove(scoring);
            }
        }

        public void SetScoringsList(IEnumerable<ScoringModel> scoringList)
        {
            scoringListSource = new CollectionViewSource()
            {
                Source = scoringList
            };
            ScoringList.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (SessionSelect != null)
                SessionSelect.PropertyChanged -= OnSessionSelectChanged;

            base.Dispose(disposing);
        }
    }
}
