using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.Models.Members
{
    [Serializable()]
    public class Admin : IAdmin
    {
        public long MemberId { get; set; }
        public AdminRights Rights { get; set; }
        public bool IsOwner => Rights.HasFlag(AdminRights.Owner);

        public Admin(long memberId, AdminRights rights)
        {
            MemberId = memberId;
            Rights = rights;
        }
    }
}

