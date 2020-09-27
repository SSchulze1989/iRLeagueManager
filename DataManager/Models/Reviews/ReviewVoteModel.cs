using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Enums;
using iRLeagueManager.Models.Members;

namespace iRLeagueManager.Models.Reviews
{
    public class ReviewVoteModel : MappableModel
    {
        public long ReviewVoteId { get; internal set; }
        public override long[] ModelId => new long[] { ReviewVoteId };

        private VoteEnum vote;
        public VoteEnum Vote { get => vote; set => SetValue(ref vote, value); }

        private LeagueMember memberAtFault;
        public LeagueMember MemberAtFault { get => memberAtFault; set => SetValue(ref memberAtFault, value); }

        private VoteCategoryModel voteCategory;
        public VoteCategoryModel VoteCategory { get => voteCategory; set => SetValue(ref voteCategory, value); }

        public ReviewVoteModel() : base()
        {
        }

        public ReviewVoteModel(long reviewVoteId)
        {
            ReviewVoteId = reviewVoteId;
        }

        public override string ToString()
        {
            return ((VoteCategory != null) ? VoteCategory.Text : Vote.ToString()).ToUpper() + ": " + MemberAtFault?.FullName?.ToString();
        }
    }
}
