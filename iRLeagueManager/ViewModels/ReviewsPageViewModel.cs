using iRLeagueManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using iRLeagueManager.Models.Sessions;
using iRLeagueManager.ViewModels.Collections;
using iRLeagueManager.Models.Reviews;

namespace iRLeagueManager.ViewModels
{
    public class ReviewsPageViewModel : ViewModelBase
    {
        private SeasonModel season;

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

        private ObservableModelCollection<IncidentReviewViewModel, IncidentReviewModel> currentReviews;
        public ObservableModelCollection<IncidentReviewViewModel, IncidentReviewModel> CurrentReviews
        {
            get => currentReviews;
            protected set
            {
                if (SetValue(ref currentReviews, value, (t, v) => t.GetSource().Equals(v.GetSource())))
                {
                    //OnPropertyChanged();
                }
            }
        }

        private IncidentReviewViewModel selectedReview;
        public IncidentReviewViewModel SelectedReview { get => selectedReview; set => SetValue(ref selectedReview, value); }

        public ICommand AddReviewCmd { get; }
        public ICommand RemoveReviewCmd { get; }

        public ReviewsPageViewModel()
        {
            SessionSelect = new SessionSelectViewModel()
            {
                SessionFilter = x => x.ResultAvailable
            };
            CurrentReviews = new ObservableModelCollection<IncidentReviewViewModel, IncidentReviewModel>();
            AddReviewCmd = new RelayCommand(async o => await AddReviewAsync(), o => SessionSelect?.SelectedSession != null);
            RemoveReviewCmd = new RelayCommand(async o => await RemoveReviewAsync(), o => SelectedReview != null);
            //RefreshCmd = new RelayCommand(o => { OnPropertyChanged(null); SelectedReview.Hold(); }, o => SelectedReview != null);
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
                //ScheduleList.UpdateSource(new ScheduleModel[] { null }.Concat(schedules));

                // Set session List
                //var sessionsInfo = ScoringList.SelectMany(x => x.Sessions);
                //var sessionModelIds = sessionsInfo.Select(x => x.ModelId);
                //var sessionModels = await LeagueContext.GetModelsAsync<SessionModel>(sessionModelIds);

                var lastSelectedSession = SessionSelect.SelectedSession;

                await SessionSelect.LoadSessions(schedules.SelectMany(x => x.Sessions));

                //if (SelectedSchedule == null)
                //    SessionSelect.SessionList = new ReadOnlyObservableCollection<SessionViewModel>(new ObservableCollection<SessionViewModel>(ScheduleList.SelectMany(x => x.Sessions).OrderBy(x => x.Date)));
                //else
                //    SessionSelect.SessionList = SelectedSchedule.Sessions;

                // Set results List
                //ResultList = new ObservableCollection<ResultInfo>(scoringModels.Select(x => x.Results.AsEnumerable()).Aggregate((x, y) => x.Concat(y)));

                if (lastSelectedSession == null || !SessionSelect.SessionList.Contains(lastSelectedSession))
                    SessionSelect.SelectedSession = SessionSelect.SessionList.Where(x => x.ResultAvailable).LastOrDefault();
                else
                    await LoadReviews();

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
        }

        public async Task LoadReviews()
        {
            if (SessionSelect?.SelectedSession == null)
                return;

            try
            {
                IsLoading = true;
                var reviewList = await LeagueContext.GetModelsAsync<IncidentReviewModel>(SessionSelect.SelectedSession.Reviews.Select(x => x.ReviewId.GetValueOrDefault()));
                CurrentReviews.UpdateSource(reviewList);
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

        public async Task AddReviewAsync()
        {
            if (SessionSelect?.SelectedSession == null)
                return;

            try
            {
                IsLoading = true;
                var newReview = new IncidentReviewModel(LeagueContext.UserManager.CurrentUser, SessionSelect.SelectedSession.Model);
                newReview = await LeagueContext.AddModelAsync(newReview);
                SessionSelect.SelectedSession.Model.Reviews.Add(newReview);
                await LeagueContext.UpdateModelAsync(SessionSelect.SelectedSession.Model);
                await LoadReviews();
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

        public async Task RemoveReviewAsync()
        {
            if (SessionSelect?.SelectedSession == null)
                return;

            var incidentReview = SelectedReview?.Model;

            if (incidentReview == null)
                return;

            try
            {
                IsLoading = true;
                await LeagueContext.DeleteModelAsync<IncidentReviewModel>(incidentReview.ModelId);
                SessionSelect.SelectedSession.Model.Reviews.Remove(incidentReview);
                await LeagueContext.UpdateModelAsync(SessionSelect.SelectedSession.Model);
                await LoadReviews();
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

        protected async void OnSessionSelectChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SessionSelect.SelectedSession))
            {
                await LoadReviews();
                OnPropertyChanged(null);
            }
        }

        public async override void Refresh(string propertyName = "")
        {
            await Load(season);
            base.Refresh(propertyName);
        }
    }
}
