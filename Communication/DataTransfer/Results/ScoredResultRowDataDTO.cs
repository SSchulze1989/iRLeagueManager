using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    public class ScoredResultRowDataDTO : ResultRowDataDTO
    {
        [DataMember]
        public long? ScoredResultRowId { get; set; }
        [DataMember]
        public long? ScoringId { get; set; }
        [DataMember]
        public int RacePoints { get; set; }
        [DataMember]
        public int BonusPoints { get; set; }
        [DataMember]
        public int PenaltyPoints { get; set; }
        [DataMember]
        public int FinalPosition { get; set; }
        [DataMember]
        public int FinalPositionChange { get; set; }

        public override object MappingId => ScoredResultRowId;

        public override object[] Keys => new object[] { ScoredResultRowId };
    }
}
