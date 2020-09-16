using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using iRLeagueDatabase.DataTransfer.Members;

namespace iRLeagueDatabase.DataTransfer.User
{
    [DataContract]
    public class UserDTO : MappableDTO
    {
        public override object MappingId => UserId;
        public override object[] Keys => new object[] { UserId };

        [DataMember]
        public string UserId { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Firstname { get; set; }
        [DataMember]
        public string Lastname { get; set; }
        [DataMember]
        public long? MemberId { get; set; }
    }
}
