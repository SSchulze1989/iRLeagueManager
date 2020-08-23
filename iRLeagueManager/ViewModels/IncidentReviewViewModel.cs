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

namespace iRLeagueManager.ViewModels
{
    public class IncidentReviewViewModel : LeagueContainerModel<IncidentReviewModel>
    {
        //public IncidentReviewModel Model { get => Source; set => SetSource(value); }

        public string Description => string.Format("Lap: {0}\t|\tCorner: {1}\t-\tIncident: {2}\t-\tInvolved: {3}", Model.OnLap, Model.Corner, "Incident description", (Model.InvolvedMembers.Count() > 0) ? Model.InvolvedMembers.Select(x => x.ShortName).Aggregate((x, y) => x + ", " + y) : "");
        public int OnLap { get => Model.OnLap; set => Model.OnLap = value; }
        public int Corner { get => Model.Corner; set => Model.Corner = value; }
        public ObservableCollection<LeagueMember> InvolvedMembers => Model.InvolvedMembers;
        public string IncidentKind { get => Model.IncidentKind; set => Model.IncidentKind = value; }
        public string FullDescription { get => Model.FullDescription; set => Model.FullDescription = value; }

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

        public ObservableCollection<ReviewVoteModel> AcceptedVotes => Model?.AcceptedReviewVotes;

        public IEnumerable<VoteEnum> VoteEnums => Enum.GetValues(typeof(VoteEnum)).Cast<VoteEnum>();

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


        private readonly ObservableModelCollection<ReviewCommentViewModel, ReviewCommentModel> comments;
        public ObservableModelCollection<ReviewCommentViewModel, ReviewCommentModel> Comments
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

        private SessionViewModel session;
        public SessionViewModel Session { get => session; set => SetValue(ref session, value); }

        private MemberListViewModel memberList;
        public MemberListViewModel MemberList { get => memberList; set => SetValue(ref memberList, value); }

        public bool CanUserAddComment => Comments.Any(x => x.IsUserAuthor) == false;

        private VoteState voteState;
        public VoteState VoteState { get => voteState; set => SetValue(ref voteState, value); }

        protected override IncidentReviewModel Template => new IncidentReviewModel();
        //...

        public ICommand AddVoteCmd { get; }
        public ICommand DeleteVoteCmd { get; }

        public IncidentReviewViewModel() : base()
        {
            memberList = new MemberListViewModel();
            comments = new ObservableModelCollection<ReviewCommentViewModel, ReviewCommentModel>(x => x.Review = this);
            ((INotifyCollectionChanged)comments).CollectionChanged += OnCommentsCollectionChanged;
            AddVoteCmd = new RelayCommand(o => AddVote(o as ReviewVoteModel), o => Model?.AcceptedReviewVotes != null);
            DeleteVoteCmd = new RelayCommand(o => DeleteVote(o as ReviewVoteModel), o => Model?.AcceptedReviewVotes != null && o is ReviewVoteModel);
        }

        public override void Refresh(string propertyName = "")
        {
            CalculateVotes();
            base.Refresh(propertyName);
        }

        public void Hold()
        {
            int i = 1;
        }

        public void OnCommentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(CanUserAddComment));
            CalculateVotes();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            switch (propertyName)
            {
                case nameof(Comments):
                    OnPropertyChanged(nameof(CanUserAddComment));
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
                    var existingVote = votes.SingleOrDefault(x => x.Key.MemberAtFault.MemberId == currentVote.MemberAtFault?.MemberId && x.Key.Vote == currentVote.Vote);
                    if (existingVote == null)
                    {
                        existingVote = new MyKeyValuePair<ReviewVoteModel, int>(currentVote, 0);
                        votes.Add(existingVote);
                    }
                    existingVote.Value++;
                    votesCount++;
                }
            }
            this.Votes = votes;
            this.VotesCount = votesCount;

            foreach (var currentVote in AcceptedVotes)
            {
                var existingVote = acceptedVotes.SingleOrDefault(x => x.Key.MemberAtFault.MemberId == currentVote.MemberAtFault?.MemberId && x.Key.Vote == currentVote.Vote);
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
            CalculateVotes();
        }

        public async Task LoadMemberListAsync()
        {
            if (Model?.Session == null)
                return;

            try
            {
                IsLoading = true;
                var result = await LeagueContext.GetModelAsync<ResultModel>(Model.Session.SessionId.GetValueOrDefault());
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
    }
}
