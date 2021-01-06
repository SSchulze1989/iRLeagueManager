using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Statistics
{
    [DataContract]
    public class SeasonStatisticSetDTO : StatisticSetDTO
    {
        [DataMember]
        public long SeasonId { get; set; }
        [DataMember]
        public long? ScoringTableId { get; set; }
        [DataMember]
        public int FinishedRaces { get; set; }
        [DataMember]
        public bool IsSeasonFinished { get; set; }
    }
}
