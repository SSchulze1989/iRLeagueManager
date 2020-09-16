using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Members
{
    [DataContract]
    public class TeamDataDTO : VersionInfoDTO
    {
        [DataMember]
        public long TeamId { get; set; }
        public override object MappingId => TeamId;
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Profile { get; set; }
        [DataMember]
        public string TeamColor { get; set; }
        [DataMember]
        public string TeamHomepage { get; set; }
        [DataMember]
        public long[] MemberIds { get; set; }
        public override object[] Keys => new object[] { TeamId };
    }
}
