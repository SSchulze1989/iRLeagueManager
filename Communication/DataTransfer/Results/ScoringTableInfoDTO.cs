using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    public class ScoringTableInfoDTO : VersionInfoDTO
    {
        [DataMember]
        public long ScoringTableId { get; set; }
        public override object MappingId => ScoringTableId;
        [DataMember]
        public string Name { get; set; }
        public override object[] Keys => new object[] { ScoringTableId };
    }
}
