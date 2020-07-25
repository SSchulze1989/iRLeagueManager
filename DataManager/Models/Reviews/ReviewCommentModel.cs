using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using iRLeagueManager.Models.Members;
using iRLeagueManager.Enums;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models;

using iRLeagueManager.Interfaces;
using System.Collections.ObjectModel;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.Models.Reviews
{
    [Serializable]
    public class ReviewCommentModel : CommentModel, IReviewComment, INotifyPropertyChanged
    {
        //private IncidentReviewModel review;
        //public IncidentReviewModel Review { get => review; internal set { review = value; OnPropertyChanged(); } }

        //public ScheduleModel Schedule => Review?.Schedule;

        //public override SeasonModel Season => Schedule?.Season;
        private IncidentReviewInfo review;
        public IncidentReviewInfo Review { get => review; internal set => SetValue(ref review, value); }

        private ObservableCollection<ReviewVoteModel> commentReviewVotes;
        public ObservableCollection<ReviewVoteModel> CommentReviewVotes { get => commentReviewVotes; set => SetNotifyCollection(ref commentReviewVotes, value); }

        public ReviewCommentModel () { }

        public ReviewCommentModel(long commentId, string authorName) : base(commentId, authorName) 
        {
            Review = review;
        }

        public ReviewCommentModel(UserModel author) : base(author)
        {
        }

        public ReviewCommentModel(UserModel author, IncidentReviewInfo review) : this(author) 
        {
            Review = review;
        }

        internal override void InitializeModel()
        {
            base.InitializeModel();
        }

        public override void CopyFrom(ModelBase sourceObject)
        {
            base.CopyFrom(sourceObject);

            if (sourceObject is ReviewCommentModel commentModel)
            {
                CommentReviewVotes = new ObservableCollection<ReviewVoteModel>(commentModel.CommentReviewVotes.ToList());
            }
        }

        public override void CopyTo(ModelBase targetObject)
        {
            base.CopyTo(targetObject);

            if (targetObject is ReviewCommentModel commentModel)
            {
                commentModel.CommentReviewVotes = new ObservableCollection<ReviewVoteModel>(CommentReviewVotes.ToList());
            }
        }

        //public override void SetLeagueClient(IRLeagueClient leagueClient)
        //{
        //    base.SetLeagueClient(leagueClient);
        //    MemberAtFault = (MemberAtFault != null) ? client.LeagueMembers.Single(x => x.MemberId == MemberAtFault.MemberId) : null;
        //}
    }
}
