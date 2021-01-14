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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Enums;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Reviews;
using iRLeagueManager.ViewModels.Collections;
using iRLeagueManager.Models.Results;
using System.Diagnostics;
using System.Windows.Input;
using iRLeagueManager.Models;
using System.Windows.Data;
using iRLeagueManager.Converters;
using iRLeagueManager.Extensions;

namespace iRLeagueManager.ViewModels
{
    public class IncidentReviewViewModel : LeagueContainerModel<IncidentReviewModel>
    {
        //public IncidentReviewModel Model { get => Source; set => SetSource(value); }

        public string Description => string.Format("{0} | Lap: {1}\t|\tCorner: {2}\t-\tIncident: {3}\t-\tInvolved: {4}", IncidentNr, Model.OnLap, Model.Corner, "Incident description", (Model.InvolvedMembers.Count() > 0) ? Model.InvolvedMembers.Select(x => x.ShortName).Aggregate((x, y) => x + ", " + y) : "");
        public string OnLap { get => Model.OnLap; set => Model.OnLap = value; }
        public string OnLapSortingString => OnLap.AddLeadingZeroesToNumbers(10);
        public string Corner { get => Model.Corner; set => Model.Corner = value; }
        public string CornerSortingString => Corner.AddLeadingZeroesToNumbers(10);
        public ObservableCollection<LeagueMember> InvolvedMembers => Model.InvolvedMembers;
        public string IncidentKind { get => Model.IncidentKind; set => Model.IncidentKind = value; }
        public string FullDescription { get => Model.FullDescription; set => Model.FullDescription = value; }

        public string IncidentNr { get => Model.IncidentNr; set => Model.IncidentNr = value; }

        public string IncidentNrSortString => IncidentNr.AddLeadingZeroesToNumbers(10);

        private bool forceShowComments;
        public bool ForceShowComments { get => forceShowComments; set => SetValue(ref forceShowComments, value); }

        private bool isExpanded;
        public bool IsExpanded { get => isExpanded; set => SetValue(ref isExpanded, value); }

        private IEnumerable<MyKeyValuePair<ReviewVoteModel, int>> votes;
        public IEnumerable<MyKeyValuePair<ReviewVoteModel, int>> Votes 
        {
            get
            {
                //CalculateVotes();
                return votes;
            }
            set => SetValue(ref votes, value);
        }

        private int votesCount;
        public int VotesCount { get => votesCount; set => SetValue(ref votesCount, value); }
        public int CommentCount => comments.Count;

        public ObservableCollection<ReviewVoteModel> AcceptedVotes => Model?.AcceptedReviewVotes;

        public string ResultLongText { get => Model.ResultLongText; set => Model.ResultLongText = value; }

        public IEnumerable<VoteEnum> VoteEnums => Enum.GetValues(typeof(VoteEnum)).Cast<VoteEnum>();


        private ICollectionView voteCategories;
        public ICollectionView VoteCategories { get => voteCategories; set => SetValue(ref voteCategories, value); }

        private ICollectionView incidentKinds;
        public ICollectionView IncidentKinds { get => incidentKinds; set => SetValue(ref incidentKinds, value); }


        private IEnumerable<MyKeyValuePair<ReviewVoteModel, int>> countAcceptedVotes;
        public IEnumerable<MyKeyValuePair<ReviewVoteModel, int>> CountAcceptedVotes 
        { 
            get
            {
                //CalculateVotes();
                return countAcceptedVotes;
            }
            set => SetValue(ref countAcceptedVotes, value); 
        }


        private readonly ObservableViewModelCollection<ReviewCommentViewModel, ReviewCommentModel> comments;
        public ObservableViewModelCollection<ReviewCommentViewModel, ReviewCommentModel> Comments
        {
            get
            {
                if (comments != null && comments.GetSource() != Model?.Comments)
                {
                    comments.UpdateSource(Model?.Comments);
                    CalculateVotes();
                }
                return comments;
            }
        }

        //private SessionViewModel session;
        //public SessionViewModel Session { get => session; set => SetValue(ref session, value); }
        public long SessionId => SessionId;

        private MemberListViewModel memberList;
        public MemberListViewModel MemberList { get => memberList; set => SetValue(ref memberList, value); }

        public bool CanUserAddComment => Comments.Any(x => x.IsUserAuthor) == false;
        public bool UserHasVoted => Comments.Any(x => x.IsUserAuthor && x.Votes.Count > 0) == true;

        private VoteState voteState;
        public VoteState VoteState { get => voteState; set => SetValue(ref voteState, value); }

        protected override IncidentReviewModel Template => new IncidentReviewModel();
        //...

        public ICommand AddVoteCmd { get; }
        public ICommand DeleteVoteCmd { get; }

        public IncidentReviewViewModel() : base()
        {
            memberList = new MemberListViewModel();
            comments = new ObservableViewModelCollection<ReviewCommentViewModel, ReviewCommentModel>(x => x.Review = this);
            ((INotifyCollectionChanged)comments).CollectionChanged += OnCommentsCollectionChanged;
            AddVoteCmd = new RelayCommand(o => AddVote(o as ReviewVoteModel), o => Model?.AcceptedReviewVotes != null);
            DeleteVoteCmd = new RelayCommand(o => DeleteVote(o as ReviewVoteModel), o => Model?.AcceptedReviewVotes != null && o is ReviewVoteModel);
            MemberList.CustomFilters.Add(x => InvolvedMembers.Contains(x) == false);
        }

        public override async Task Refresh()
        {
            CalculateVotes();
            try
            {
                IsLoading = true;
                await LoadMemberListAsync();
                var votesCategorieCollection = await LeagueContext.GetModelsAsync<VoteCategoryModel>();
                SetVoteCategoriesView(votesCategorieCollection);
                var incidentKinds = await LeagueContext.GetModelsAsync<CustomIncidentModel>();
                SetIncidentKindsView(incidentKinds);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }
            await base.Refresh();
        }

        private void SetVoteCategoriesView(object source)
        {
            VoteCategories = CollectionViewSource.GetDefaultView(source);
            VoteCategories.SortDescriptions.Add(new SortDescription(nameof(VoteCategoryModel.Index), ListSortDirection.Ascending));
        }

        private void SetIncidentKindsView(object source)
        {
            IncidentKinds = CollectionViewSource.GetDefaultView(source);
            IncidentKinds.SortDescriptions.Add(new SortDescription(nameof(CustomIncidentModel.Index), ListSortDirection.Ascending));
        }

        public void OnCommentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CanUserAddComment));
            OnPropertyChanged(nameof(UserHasVoted));
            OnPropertyChanged(nameof(CommentCount));
            CalculateVotes();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            switch (propertyName)
            {
                case nameof(Comments):
                    OnPropertyChanged(nameof(CanUserAddComment));
                    OnPropertyChanged(nameof(UserHasVoted));
                    break;
            }

            base.OnPropertyChanged(propertyName);
        }

        public void CalculateVotes()
        {
            var votes = new List<MyKeyValuePair<ReviewVoteModel, int>>();
            var acceptedVotes = new List<MyKeyValuePair<ReviewVoteModel, int>>();
            var votesCount = 0;

            if (comments == null)
                return;

            foreach(var comment in comments)
            {
                foreach(var currentVote in comment.Votes)
                {
                    var existingVote = votes.SingleOrDefault(x => x.Key.MemberAtFault?.MemberId == currentVote.MemberAtFault?.MemberId && x.Key.VoteCategory == currentVote.VoteCategory);
                    if (existingVote == null)
                    {
                        existingVote = new MyKeyValuePair<ReviewVoteModel, int>(currentVote, 0);
                        votes.Add(existingVote);
                    }
                    existingVote.Value++;
                }
                if (comment.Votes.Count > 0)
                {
                    votesCount++;
                }
            }
            this.Votes = votes;
            this.VotesCount = votesCount;

            foreach (var currentVote in AcceptedVotes)
            {
                var existingVote = acceptedVotes.SingleOrDefault(x => x.Key.MemberAtFault?.MemberId == currentVote.MemberAtFault?.MemberId && x.Key.VoteCategory == currentVote.VoteCategory);
                if (existingVote == null)
                {
                    existingVote = new MyKeyValuePair<ReviewVoteModel, int>(currentVote, 0);
                    acceptedVotes.Add(existingVote);
                }
                existingVote.Value += 1;
            }
            this.CountAcceptedVotes = acceptedVotes;

            if (AcceptedVotes?.Count() > 0)
            {
                VoteState = VoteState.Closed;
            }
            else if (votes.Count() == 0)
            {
                VoteState = VoteState.NoVote;
            }
            else if (votes.Count() < 2)
            {
                VoteState = VoteState.Open;
            }
            else if (votes.Max(x => x.Value) == votes.Count())
            {
                VoteState = VoteState.Agreed;
            }
            else if (votes.Max(x => x.Value) > votes.Count() / 2)
            {
                VoteState = VoteState.MajorityVote;
            }
            else
            {
                VoteState = VoteState.Conflict;
            }
        }

        public async Task<ReviewCommentModel> AddCommentAsync(ReviewCommentModel comment)
        {
            if (Model == null || comment == null)
                return null;

            try
            {
                IsLoading = true;
                comment = await LeagueContext.AddModelAsync(comment);
                Model.Comments.Add(comment);
            }
            catch (Exception e)
            {
                GlobalSettings.LogError(e);
            }
            finally
            {
                IsLoading = false;
            }

            return comment;
        }

        public async Task DeleteCommentAsync(ReviewCommentModel comment)
        {
            if (Model == null || comment == null)
                return;

            try
            {
                IsLoading = true;
                await LeagueContext.DeleteModelAsync<ReviewCommentModel>(comment.CommentId.GetValueOrDefault());
                Model.Comments.Remove(comment);
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

        public async void AddMember(LeagueMember member)
        {
            if (InvolvedMembers.Contains(member) == false)
            {
                InvolvedMembers.Add(member);
                await MemberList.Refresh();
            }
        }

        public async void RemoveMember(LeagueMember member)
        {
            if (InvolvedMembers.Contains(member))
            {
                InvolvedMembers.Remove(member);
                await memberList.Refresh();
            }
        }

        public void AddVote(ReviewVoteModel vote)
        {
            if (Model == null)
                return;

            if (vote == null)
            {
                vote = new ReviewVoteModel();
            }

            Model.AcceptedReviewVotes.Add(vote);
            CalculateVotes();
        }

        public void DeleteVote(ReviewVoteModel vote)
        {
            if (Model == null)
                return;

            if (Model.AcceptedReviewVotes.Contains(vote))
                Model.AcceptedReviewVotes.Remove(vote);
            CalculateVotes();
        }

        public override void OnUpdateSource()
        {
            base.OnUpdateSource();
            OnPropertyChanged(nameof(AcceptedVotes));
            CalculateVotes();
        }

        public async Task LoadMemberListAsync()
        {
            if (Model == null)
                return;

            try
            {
                IsLoading = true;
                var result = await LeagueContext.GetModelAsync<ResultModel>(Model.SessionId);
                var members = result.RawResults.Select(x => x.Member);
                MemberList.SetCollectionViewSource(members);
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

        public sealed class CustomStringComparer : IComparer<object>
        {
            public int Compare(object a, object b)
            {
                if (a is IncidentReviewViewModel lhs && b is IncidentReviewViewModel rhs)
                {//APPLY ALGORITHM LOGIC HERE
                    var compLap = SafeNativeMethods.StrCmpLogicalW(lhs.OnLap, rhs.OnLap);
                    return compLap;
                }
                return 0;
            }
        }
    }
}
