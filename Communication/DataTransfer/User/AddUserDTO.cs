using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.User
{
    [DataContract]
    public class AddUserDTO : UserProfileDTO
    {
        [DataMember]
        public string Password { get; set; }
    }
}
