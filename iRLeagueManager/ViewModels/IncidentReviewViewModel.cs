﻿using System;
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

        public ObservableCollection<ReviewVoteModel> AcceptedVotes => Model.AcceptedReviewVotes;

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

        protected override IncidentReviewModel Template => new IncidentReviewModel();
        //...

        public IncidentReviewViewModel() : base()
        {
            memberList = new MemberListViewModel();
            comments = new ObservableModelCollection<ReviewCommentViewModel, ReviewCommentModel>(x => x.Review = this);
        }

        public void Hold()
        {
            int i = 1;
        }

        public void OnCommentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateVotes();
        }

        public void CalculateVotes()
        {
            var votes = new List<MyKeyValuePair<ReviewVoteModel, int>>();
            var acceptedVotes = new List<MyKeyValuePair<ReviewVoteModel, int>>();

            if (comments == null)
                return;

            foreach(var comment in comments)
            {
                foreach(var currentVote in comment.Votes)
                {
                    var existingVote = votes.SingleOrDefault(x => x.Key.MemberAtFault.MemberId == currentVote.MemberAtFault.MemberId && x.Key.Vote == currentVote.Vote);
                    if (existingVote == null)
                    {
                        existingVote = new MyKeyValuePair<ReviewVoteModel, int>(currentVote, 0);
                        votes.Add(existingVote);
                    }
                    existingVote.Value += 1;
                }
            }
            this.votes = votes;

            foreach (var currentVote in AcceptedVotes)
            {
                var existingVote = acceptedVotes.SingleOrDefault(x => x.Key.MemberAtFault.MemberId == currentVote.MemberAtFault.MemberId && x.Key.Vote == currentVote.Vote);
                if (existingVote == null)
                {
                    existingVote = new MyKeyValuePair<ReviewVoteModel, int>(currentVote, 0);
                    acceptedVotes.Add(existingVote);
                }
                existingVote.Value += 1;
            }
            this.countAcceptedVotes = acceptedVotes;
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
                MemberList.SetCollectionViewSourc(members);
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
