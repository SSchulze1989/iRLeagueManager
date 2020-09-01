using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.User
{
    [DataContract]
    public class UserProfileDTO : UserDTO
    {
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string ProfileText { get; set; }
        [DataMember]
        public string IRacingId { get; set; }
        [DataMember]
        public string DanLisaId { get; set; }
        [DataMember]
        public string DiscordId { get; set; }
    }
}
