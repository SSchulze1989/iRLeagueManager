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

using iRLeagueManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Data;

using iRLeagueManager.Models.Sessions;
using iRLeagueManager.ViewModels.Collections;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.Data;
using iRLeagueManager.Extensions;

namespace iRLeagueManager.ViewModels
{
    public class ReviewsPageViewModel : ViewModelBase, ISeasonPageViewModel
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

        private ReviewNavBarViewModel reviewNavBar;
        public ReviewNavBarViewModel ReviewNavBar { get => reviewNavBar; set => SetValue(ref reviewNavBar, value); }

        private ObservableModelCollection<IncidentReviewViewModel, IncidentReviewModel> currentReviews;
        //public ObservableModelCollection<IncidentReviewViewModel, IncidentReviewModel> CurrentReviews
        //{
        //    get => currentReviews;
        //    protected set
        //    {
        //        if (SetValue(ref currentReviews, value, (t, v) => t.GetSource().Equals(v.GetSource())))
        //        {
        //            //OnPropertyChanged();
        //        }
        //    }
        //}
        public ICollectionView CurrentReviews => currentReviews.CollectionView;

        private IncidentReviewViewModel selectedReview;
        public IncidentReviewViewModel SelectedReview { get => selectedReview; set => SetValue(ref selectedReview, value); }

        public ICommand AddReviewCmd { get; }
        public ICommand RemoveReviewCmd { get; }

        private ICollectionView voteCategories;
        public ICollectionView VoteCategories { get => voteCategories; set => SetValue(ref voteCategories, value); }

        public bool HideCommentsBeforeVoted => season.HideCommentsBeforeVoted;

        public ReviewsPageViewModel()
        {
            SessionSelect = new SessionSelectViewModel()
            {
                SessionFilter = x => x.ResultAvailable
            };
            currentReviews = new ObservableModelCollection<IncidentReviewViewModel, IncidentReviewModel>(x => 
                x.Session = SessionSelect?.SessionList.SingleOrDefault(y => y.SessionId == x.Model.Session.SessionId));
            AddReviewCmd = new RelayCommand(async o => await AddReviewAsync(), o => SessionSelect?.SelectedSession != null);
            RemoveReviewCmd = new RelayCommand(async o => await RemoveReviewAsync(o as IncidentReviewModel), o => SelectedReview != null || o is IncidentReviewModel);
            ReviewNavBar = new ReviewNavBarViewModel() { ReviewsPageViewModel = this };
            //RefreshCmd = new RelayCommand(o => { OnPropertyChanged(null); SelectedReview.Hold(); }, o => SelectedReview != null);
            CurrentReviews.CurrentChanged += OnCurrentReviewChange;
            CurrentReviews.SortDescriptions.Add(new SortDescription(nameof(IncidentReviewViewModel.IncidentNrSortString), ListSortDirection.Ascending));
            CurrentReviews.SortDescriptions.Add(new SortDescription(nameof(IncidentReviewViewModel.OnLapSortingString), ListSortDirection.Ascending));
            CurrentReviews.SortDescriptions.Add(new SortDescription(nameof(IncidentReviewViewModel.CornerSortingString), ListSortDirection.Ascending));
        }

        public async Task Load(iRLeagueManager.Models.SeasonModel season)
        {
            if (season == null)
                return;

            this.season = season;

            try
            {
                IsLoading = true;
                VoteCategories = CollectionViewSource.GetDefaultView(season.VoteCategories);
                VoteCategories.SortDescriptions.Add(new SortDescription(nameof(VoteCategoryModel.Index), ListSortDirection.Ascending));
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
                var reviewList = (await LeagueContext.GetModelsAsync<IncidentReviewModel>(SessionSelect.SelectedSession.Reviews.Select(x => x.ReviewId.GetValueOrDefault()))).ToList();
                currentReviews.UpdateSource(reviewList);
                ReviewNavBar.CalculateReviewsStatistics();
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

        public IncidentReviewModel CreateReviewModel()
        {
            return new IncidentReviewModel(LeagueContext.UserManager.CurrentUser, SessionSelect.SelectedSession.Model);
        }

        public async Task<IncidentReviewModel> AddReviewAsync()
        {
            if (SessionSelect?.SelectedSession == null)
                return null;

            return await AddReviewAsync(CreateReviewModel());

            //try
            //{
            //    IsLoading = true;
            //    var newReview = new IncidentReviewModel(LeagueContext.UserManager.CurrentUser, SessionSelect.SelectedSession.Model);
            //    newReview = await LeagueContext.AddModelAsync(newReview);
            //    SessionSelect.SelectedSession.Model.Reviews.Add(newReview);
            //    await LeagueContext.UpdateModelAsync(SessionSelect.SelectedSession.Model);
            //    await LoadReviews();
            //}
            //catch (Exception e)
            //{
            //    GlobalSettings.LogError(e);
            //}
            //finally
            //{
            //    IsLoading = false;
            //}
        }

        public async Task<IncidentReviewModel> AddReviewAsync(IncidentReviewModel reviewModel)
        {
            if (SessionSelect?.SelectedSession == null)
                return null;

            try
            {
                IsLoading = true;
                reviewModel = await LeagueContext.AddModelAsync(reviewModel);
                SessionSelect.SelectedSession.Model.Reviews.Add(reviewModel);
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

            return reviewModel;
        }

        public void OnCurrentReviewChange(object sender, EventArgs e)
        {
            if (CurrentReviews.CurrentItem != null && CurrentReviews.CurrentItem is IncidentReviewViewModel review)
            {
                MoveToReview(review);
            }
        }

        public void MoveToReview(IncidentReviewViewModel review)
        {

        }

        public async Task RemoveReviewAsync(IncidentReviewModel review)
        {
            //if (SessionSelect?.SelectedSession == null)
            //    return;

            if (review == null)
                review = SelectedReview?.Model;

            if (review == null)
                return;

            try
            {
                IsLoading = true;
                await LeagueContext.DeleteModelAsync<IncidentReviewModel>(review.ModelId);
                if (review != null)
                {
                    SessionSelect.SelectedSession.Model.Reviews.Remove(review);
                }
                await LeagueContext.GetModelAsync<SessionModel>(SessionSelect.SelectedSession.Model.ModelId, reload: true);
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

        public async override Task Refresh()
        {
            LeagueContext.ModelManager.ForceExpireModels<SessionModel>();
            LeagueContext.ModelManager.ForceExpireModels<IncidentReviewModel>();
            LeagueContext.ModelManager.ForceExpireModels<ReviewCommentModel>();
            await Load(season);
            CurrentReviews.Refresh();
            await base.Refresh();
        }

        protected override void Dispose(bool disposing)
        {
            if (sessionSelect != null)
            {
                sessionSelect.PropertyChanged -= OnSessionSelectChanged;
            }
            base.Dispose(disposing);
        }
    }
}
