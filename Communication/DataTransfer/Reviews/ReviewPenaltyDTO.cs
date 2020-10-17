using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Reviews
{
    [DataContract]
    public class ReviewPenaltyDTO : MappableDTO
    {
        [DataMember]
        public long ResultRowId { get; set; }
        [DataMember]
        public long ReviewId { get; set; }
        [DataMember]
        public int PenaltyPoints { get; set; }
        [DataMember]
        public ReviewVoteDataDTO ReviewVote { get; set; }
        [DataMember]
        public string IncidentNr { get; set; }

        public override object[] Keys => new object[] { ResultRowId, ReviewId };
    }
}
