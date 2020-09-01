using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Enums;
using iRLeagueDatabase.DataTransfer.Members;

namespace iRLeagueDatabase.DataTransfer.Reviews
{
    public class ReviewVoteDataDTO : MappableDTO
    {
        public long ReviewVoteId { get; set; }
        public VoteEnum Vote { get; set; }
        public LeagueMemberInfoDTO MemberAtFault { get; set; }

        public override object MappingId => ReviewVoteId;
        public override object[] Keys => new object[] { ReviewVoteId };
    }
}
