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
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Results;
using iRLeagueManager.Models;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;
using System.Collections.ObjectModel;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.Models.Reviews
{
    [Serializable]
    public class IncidentReviewModel : IncidentReviewInfo, IHierarchicalModel
    {
        //private ResultModel result;
        //public ResultModel Result { get => result; internal set { result = value; OnPropertyChanged(); } }

        //public SessionModel Session => Result?.Session;
        //public ScheduleModel Schedule => Session?.Schedule;
        //public SeasonModel Season => Schedule?.Season;

        private SessionInfo session;
        public SessionInfo Session { get => session; internal set { SetValue(ref session, value); } }

        private string incidentKind;
        public string IncidentKind { get => incidentKind; set => SetValue(ref incidentKind, value); }

        private string fullDescription;
        public string FullDescription { get => fullDescription; set => SetValue(ref fullDescription, value); }

        private TimeSpan timeStamp;
        public TimeSpan TimeStamp { get => timeStamp; set { SetValue(ref timeStamp, value); } }

        private ObservableCollection<LeagueMember> involvedMembers;
        public ObservableCollection<LeagueMember> InvolvedMembers { get => involvedMembers; set => SetNotifyCollection(ref involvedMembers, value); }

        private ObservableCollection<ReviewCommentModel> comments;
        public ObservableCollection<ReviewCommentModel> Comments { get => comments; set => SetNotifyCollection(ref comments, value); }
        //IEnumerable<IReviewComment> IReview.Comments => Comments;
        //ReadOnlyObservableCollection<IReviewComment> IReview.Comments => new ReadOnlyObservableCollection<IReviewComment>(Comments);
        private ObservableCollection<ReviewVoteModel> acceptedReviewVotes;
        public ObservableCollection<ReviewVoteModel> AcceptedReviewVotes { get => acceptedReviewVotes; set => SetNotifyCollection(ref acceptedReviewVotes, value); }
        //private LeagueMember memberAtFaultResult;        
        //public LeagueMember MemberAtFaultResult { get => memberAtFaultResult; set { SetValue(ref memberAtFaultResult, value); } }

        //private VoteEnum voteResult;
        //public VoteEnum VoteResult { get => voteResult; set { SetValue(ref voteResult, value); } }

        //private VoteState voteState;
        //public VoteState VoteState { get => voteState; set { SetValue(ref voteState, value); } }

        private string resultLongText;
        public string ResultLongText { get => resultLongText; set => SetValue(ref resultLongText, value); }

        public override bool ContainsChanges { get => base.ContainsChanges || AcceptedReviewVotes.Any(x => x.ContainsChanges); protected set => base.ContainsChanges = value; }

        public IncidentReviewModel()
        {
            ReviewId = null;
            InvolvedMembers = new ObservableCollection<LeagueMember>();
            AcceptedReviewVotes = new ObservableCollection<ReviewVoteModel>();
            Comments = new ObservableCollection<ReviewCommentModel>();
        }

        public IncidentReviewModel(UserModel author, SessionInfo session) : base(author)
        {
            ReviewId = null;
            InvolvedMembers = new ObservableCollection<LeagueMember>();
            Comments = new ObservableCollection<ReviewCommentModel>();
            AcceptedReviewVotes = new ObservableCollection<ReviewVoteModel>();

            Session = session;
        }

        //public IncidentReviewModel(ResultModel result) : this ()
        //{
        //    Result = result;
        //}

        public IncidentReviewModel(long? reviewId) : this()
        {
            ReviewId = reviewId;
        }

        //public IncidentReviewModel(LeagueMember author) : this()
        //{
        //    Author = author;
        //}

        internal override void InitializeModel()
        {
            if (!isInitialized)
            {
                foreach (var comment in Comments)
                {
                    comment.InitializeModel();
                }
                foreach (var vote in AcceptedReviewVotes)
                {
                    vote.InitializeModel();
                }
            }
            base.InitializeModel();
        }

        public override void CopyFrom(ModelBase sourceObject)
        {
            base.CopyFrom(sourceObject);

            if (sourceObject is IncidentReviewModel reviewModel)
            {
                InitReset();
                InvolvedMembers = new ObservableCollection<LeagueMember>(reviewModel.InvolvedMembers.ToList());
                Comments = new ObservableCollection<ReviewCommentModel>(reviewModel.Comments.Select(x =>
                    {
                        var comment = new ReviewCommentModel();
                        comment.CopyFrom(x);
                        return comment;
                    }).ToList());
                AcceptedReviewVotes = new ObservableCollection<ReviewVoteModel>(reviewModel.AcceptedReviewVotes.Select(x =>
                {
                    var vote = new ReviewVoteModel();
                    vote.CopyFrom(x);
                    return vote;
                }).ToList());
                if (reviewModel.isInitialized)
                {
                    InitializeModel();
                }
            }
            OnPropertyChanged(null);
        }

        public override void CopyTo(ModelBase targetObject)
        {
            base.CopyTo(targetObject);

            if (targetObject is IncidentReviewModel reviewModel)
            {
                reviewModel.InitReset();
                reviewModel.InvolvedMembers = new ObservableCollection<LeagueMember>(InvolvedMembers.ToList());
                reviewModel.Comments = new ObservableCollection<ReviewCommentModel>(Comments.Select(x =>
                {
                    var comment = new ReviewCommentModel();
                    comment.CopyFrom(x);
                    return comment;
                }).ToList());
                reviewModel.AcceptedReviewVotes = new ObservableCollection<ReviewVoteModel>(AcceptedReviewVotes.Select(x =>
                {
                    var vote = new ReviewVoteModel();
                    vote.CopyFrom(x);
                    return vote;
                }).ToList());
                if (isInitialized)
                {
                    reviewModel.InitializeModel();
                }
            }
        }


        //public ReviewComment AddComment(string text)
        //{
        //    ReviewComment newComment = new ReviewComment()
        //    {
        //        Author = client.ClientMember,
        //        CommentId = client.GetNewCommentId(),
        //        Date = DateTime.Now,
        //        Text = text,
        //    };
        //    Comments.Add(newComment);
        //    return newComment;
        //}

        //public void SetLeagueClient(IRLeagueClient leagueClient)
        //{
        //    client = leagueClient;
        //    Author = client.Admins.SingleOrDefault(x => x.MemberId == Author.MemberId);
        //    InvolvedMembers = InvolvedMembers.Select(x => client.LeagueMembers.Single(y => y.MemberId == x.MemberId)).ToList();
        //    MemberAtFaultResult = client.LeagueMembers.Single(x => x.MemberId == MemberAtFaultResult.MemberId);
        //    Comments.ForEach(x => x.SetLeagueClient(client));
        //}
    }
}
