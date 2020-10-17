using iRLeagueManager.Models.Members;
using iRLeagueManager.Models.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.ViewModels
{
    public class ReviewPenaltyViewModel : LeagueContainerModel<ReviewPenaltyModel>
    {
        public long ResultRowId => Model.ResultRowId;
        public long ReviewId => Model.ReviewId;
        public int PenaltyPoints => Model.PenaltyPoints;
        public ReviewVoteModel ReviewVote => Model.ReviewVote;
        public VoteCategoryModel VoteCategory => Model.ReviewVote?.VoteCategory;
        public LeagueMember Member => Model.ReviewVote?.MemberAtFault;

        protected override ReviewPenaltyModel Template => new ReviewPenaltyModel();

        public ReviewPenaltyViewModel()
        {

        }
    }
}
