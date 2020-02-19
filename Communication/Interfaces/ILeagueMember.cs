using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.Interfaces
{
    public interface ILeagueMember
    {
        [EqualityCheckProperty]
        long MemberId { get; }
        string Firstname { get; set; }
        string Lastname { get; set; }
        string FullName { get; }
        string IRacingId { get; set; }
        string DanLisaId { get; set; }
        string DiscordId { get; set; }
    }
}
