using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Attributes;

using iRLeagueManager.Enums;

namespace iRLeagueManager.Interfaces
{
    public interface IAdminData
    {
        [EqualityCheckProperty]
        long? MemberId { get; set; }
        AdminRights Rights { get; set; }
        bool IsOwner { get; set; }
    }
}
