using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.Models.Members
{
    /// <summary>
    /// This class manages information considering the member's iracing user profile
    /// </summary>
    [Serializable()]
    public class LeagueMember : MappableModel, ILeagueMember, IAdmin
    {
        public long? MemberId { get; } = null;
        public override long[] ModelId => new long[] { MemberId.GetValueOrDefault() };

        public string Firstname { get; set; } = "Firstname";
        public string Lastname { get; set; } = "Lastname";
        [XmlIgnore]
        public string FullName => Firstname + ' ' + Lastname;
        public string IRacingId { get; set; } = "0";
        public string DanLisaId { get; set; } = "0";
        public string DiscordId { get; set; } = "0";
        public string ShortName => Firstname[0].ToString().ToUpper() + "." + Lastname;

        private long? teamId;
        public long? TeamId { get => teamId; set => SetValue(ref teamId, value); }

        public LeagueMember() { }

        public LeagueMember(long? memberId)
        {
            MemberId = memberId;
        }

        public LeagueMember(long? memberId, string firstname, string lastname, string iRacingId = "", string danLisaId = "", string discordId = "")
        {
            MemberId = memberId;
            Firstname = firstname;
            Lastname = lastname;
            IRacingId = iRacingId;
            DanLisaId = danLisaId;
            DiscordId = discordId;
        }

        public LeagueMember(LeagueMember data)
        {
            MemberId = data.MemberId;
            Firstname = data.Firstname;
            Lastname = data.Lastname;
            IRacingId = data.IRacingId;
            DanLisaId = data.DanLisaId;
            DiscordId = data.DiscordId;
        }

        public static LeagueMember GetTemplate()
        {
            var template = new LeagueMember
            {
                DanLisaId = "danlisa-id",
                DiscordId = "discord-id",
                Firstname = "Template",
                Lastname = "Member",
                IRacingId = "iracing-id"
            };
            template.InitializeModel();

            return template;
        }
        public override string ToString()
        {
            return FullName;
        }
    }
}
