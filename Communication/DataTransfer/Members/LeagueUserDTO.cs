using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using iRLeagueManager.Enums;

namespace iRLeagueDatabase.DataTransfer.Members
{
    [DataContract]
    public class LeagueUserDTO : MappableDTO
    {
        [DataMember]
        public long AdminId { get; set; }
        public override object MappingId => AdminId;
        public override object[] Keys => new object[] { AdminId };

        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public LeagueMemberInfoDTO Member { get; set; }
        [DataMember]
        public AdminRights AdminRights { get; set; }
        [DataMember]
        public long? TeamId { get; set; }
    }
}
