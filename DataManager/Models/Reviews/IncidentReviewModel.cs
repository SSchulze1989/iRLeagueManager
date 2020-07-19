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

namespace iRLeagueManager.Models.Reviews
{
    [Serializable]
    public class IncidentReviewModel : IncidentReviewInfo, IReview, IHierarchicalModel
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
        IEnumerable<IReviewComment> IReview.Comments => Comments;
        //ReadOnlyObservableCollection<IReviewComment> IReview.Comments => new ReadOnlyObservableCollection<IReviewComment>(Comments);
        private ObservableCollection<ReviewVoteModel> acceptedReviewVotes;
        public ObservableCollection<ReviewVoteModel> AcceptedReviewVotes { get => acceptedReviewVotes; set => SetNotifyCollection(ref acceptedReviewVotes, value); }
        //private LeagueMember memberAtFaultResult;        
        //public LeagueMember MemberAtFaultResult { get => memberAtFaultResult; set { SetValue(ref memberAtFaultResult, value); } }

        //private VoteEnum voteResult;
        //public VoteEnum VoteResult { get => voteResult; set { SetValue(ref voteResult, value); } }

        //private VoteState voteState;
        //public VoteState VoteState { get => voteState; set { SetValue(ref voteState, value); } }

        public IncidentReviewModel()
        {
            ReviewId = null;
            InvolvedMembers = new ObservableCollection<LeagueMember>();
            Comments = new ObservableCollection<ReviewCommentModel>();
        }

        public IncidentReviewModel(string authorName, SessionInfo session) : this()
        {
            AuthorName = authorName;
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

        public IncidentReviewModel(LeagueMember author) : this()
        {
            Author = author;
        }

        internal override void InitializeModel()
        {
            if (!isInitialized)
            {
                //if (Result != null)
                //{
                //    foreach (var comment in Comments)
                //    {
                //        comment.Review = this;
                //        comment.InitializeModel();
                //    }
                //}
                //else
                //{
                //    return;
                //}
            }
            base.InitializeModel();
        }

        public override void CopyFrom(ModelBase sourceObject)
        {
            base.CopyFrom(sourceObject);

            if (sourceObject is IncidentReviewModel reviewModel)
            {
                InvolvedMembers = new ObservableCollection<LeagueMember>(reviewModel.InvolvedMembers.ToList());
                Comments = new ObservableCollection<ReviewCommentModel>(reviewModel.Comments.ToList());
                AcceptedReviewVotes = new ObservableCollection<ReviewVoteModel>(reviewModel.AcceptedReviewVotes.ToList());
            }
        }

        public override void CopyTo(ModelBase targetObject)
        {
            base.CopyTo(targetObject);

            if (targetObject is IncidentReviewModel reviewModel)
            {
                reviewModel.InvolvedMembers = new ObservableCollection<LeagueMember>(InvolvedMembers.ToList());
                reviewModel.Comments = new ObservableCollection<ReviewCommentModel>(Comments.ToList());
                reviewModel.AcceptedReviewVotes = new ObservableCollection<ReviewVoteModel>(AcceptedReviewVotes.ToList());
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
