using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Enums;
using iRLeagueManager.Models.Members;

namespace iRLeagueManager.Models.Reviews
{
    public class VoteMemberAtFaultModel : ModelBase
    {
        public long? VoteMemberAtFaultId { get; internal set; }
        public override long[] ModelId => new long[] { VoteMemberAtFaultId.GetValueOrDefault() };

        private VoteEnum vote;
        public VoteEnum Vote { get => vote; set => SetValue(ref vote, value); }

        private LeagueMember memberAtFault;
        public LeagueMember MemberAtFault { get => memberAtFault; set => SetValue(ref memberAtFault, value); }

        public VoteMemberAtFaultModel() : base()
        { }

        public VoteMemberAtFaultModel(long? voteMemberAtFaultId)
        {
            VoteMemberAtFaultId = voteMemberAtFaultId;
        }
    }
}
