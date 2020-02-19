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

namespace iRLeagueManager.Models.Reviews
{
    [Serializable]
    public class ReviewCommentModel : CommentBase, IReviewComment, INotifyPropertyChanged
    {
        private IncidentReviewModel review;
        public IncidentReviewModel Review { get => review; internal set { review = value; OnPropertyChanged(); } }

        //public ScheduleModel Schedule => Review?.Schedule;

        //public override SeasonModel Season => Schedule?.Season;

        private VoteEnum vote;
        public VoteEnum Vote { get => vote; set { vote = value; OnPropertyChanged(); } }

        private LeagueMember memberAtFault;
        public LeagueMember MemberAtFault { get => memberAtFault; set { memberAtFault = value; OnPropertyChanged(); } }

        public ReviewCommentModel () { }

        public ReviewCommentModel(long commentId) : base(commentId) { }

        public ReviewCommentModel(LeagueMember author) : base(author) { }

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
