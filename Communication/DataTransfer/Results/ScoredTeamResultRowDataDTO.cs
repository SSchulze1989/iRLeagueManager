using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    public class ScoredTeamResultRowDataDTO : ScoredResultRowDataDTO
    {
        [DataMember]
        public long TeamId { get; set; }
        [DataMember]
        public ScoredResultRowDataDTO[] ScoredResultRows { get; set; }
    }
}
