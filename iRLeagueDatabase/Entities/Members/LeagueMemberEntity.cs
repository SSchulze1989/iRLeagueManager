using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iRLeagueDatabase.Entities.Reviews;

namespace iRLeagueDatabase.Entities.Members
{
    /// <summary>
    /// This class manages information considering the member's iracing user profile
    /// </summary>
    public class LeagueMemberEntity : MappableEntity
    {
        [Key]
        public int MemberId { get; set; } = 0;
        public string Firstname { get; set; } = "Firstname";
        public string Lastname { get; set; } = "Lastname";
        public string Fullname => Firstname + ' ' + Lastname;
        public string IRacingId { get; set; } = "0";
        public string DanLisaId { get; set; } = "0";
        public string DiscordId { get; set; } = "0";

        public override object MappingId => MemberId;

        //public virtual List<IncidentReviewEntity> Reviews { get; set; }

        public LeagueMemberEntity() { }

        public LeagueMemberEntity(int memberId, string firstname, string lastname, string iRacingId = "", string danLisaId = "", string discordId = "")
        {
            MemberId = memberId;
            Firstname = firstname;
            Lastname = lastname;
            IRacingId = iRacingId;
            DanLisaId = danLisaId;
            DiscordId = discordId;
        }
    }
}
