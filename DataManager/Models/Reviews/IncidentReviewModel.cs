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

        private TimeSpan timeStamp;
        public TimeSpan TimeStamp { get => timeStamp; set { SetValue(ref timeStamp, value); } }

        private ObservableCollection<LeagueMember> involvedMembers;
        public ObservableCollection<LeagueMember> InvolvedMembers { get => involvedMembers; internal set { involvedMembers = value; OnPropertyChanged(); } }

        private ObservableCollection<ReviewCommentModel> comments;
        public ObservableCollection<ReviewCommentModel> Comments { get => comments; internal set { comments = value; OnPropertyChanged(); } }
        IEnumerable<IReviewComment> IReview.Comments => Comments;
        //ReadOnlyObservableCollection<IReviewComment> IReview.Comments => new ReadOnlyObservableCollection<IReviewComment>(Comments);

        private LeagueMember memberAtFaultResult;        
        public LeagueMember MemberAtFaultResult { get => memberAtFaultResult; set { SetValue(ref memberAtFaultResult, value); } }

        private VoteEnum voteResult;
        public VoteEnum VoteResult { get => voteResult; set { SetValue(ref voteResult, value); } }

        private VoteState voteState;
        public VoteState VoteState { get => voteState; set { SetValue(ref voteState, value); } }

        public IncidentReviewModel()
        {
            ReviewId = null;
            InvolvedMembers = new ObservableCollection<LeagueMember>();
            Comments = new ObservableCollection<ReviewCommentModel>();
        }

        //public IncidentReviewModel(ResultModel result) : this ()
        //{
        //    Result = result;
        //}

        public IncidentReviewModel(int? reviewId) : this()
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
