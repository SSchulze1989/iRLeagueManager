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

namespace iRLeagueManager.Models.Reviews
{
    [Serializable]
    public class ReviewCommentModel : CommentBase, IReviewComment, INotifyPropertyChanged
    {
        //private IncidentReviewModel review;
        //public IncidentReviewModel Review { get => review; internal set { review = value; OnPropertyChanged(); } }

        //public ScheduleModel Schedule => Review?.Schedule;

        //public override SeasonModel Season => Schedule?.Season;
        private ObservableCollection<VoteMemberAtFaultModel> votes;
        public ObservableCollection<VoteMemberAtFaultModel> Votes { get => votes; set => SetNotifyCollection(ref votes, value); }

        public ReviewCommentModel () { }

        public ReviewCommentModel(long commentId) : base(commentId) { }

        public ReviewCommentModel(UserModel author) : base(author) { }

        internal override void InitializeModel()
        {
            base.InitializeModel();
        }

        //public override void SetLeagueClient(IRLeagueClient leagueClient)
        //{
        //    base.SetLeagueClient(leagueClient);
        //    MemberAtFault = (MemberAtFault != null) ? client.LeagueMembers.Single(x => x.MemberId == MemberAtFault.MemberId) : null;
        //}
    }
}
