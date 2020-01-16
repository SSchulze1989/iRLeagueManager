using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using iRLeagueDatabase.DataTransfer.Reviews;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace iRLeagueDatabase.DataTransfer.Members
{
    /// <summary>
    /// This class manages information considering the member's iracing user profile
    /// </summary>
    [DataContract(Name="LeagueMemberDataDTO")]
    [KnownType(typeof(LeagueMemberInfoDTO))]
    public class LeagueMemberDataDTO : LeagueMemberInfoDTO, IMappableDTO
    {
        //[DataMember]
        //public int MemberId { get; set; }
        [DataMember]
        public string Firstname { get; set; }
        [DataMember]
        public string Lastname { get; set; }
        [DataMember]
        public string IRacingId { get; set; }
        [DataMember]
        public string DanLisaId { get; set; }
        [DataMember]
        public string DiscordId { get; set; }

        //[DataMember]
        //public LeagueMemberInfoDTO CreatedBy { get; set; }
        //[DataMember]
        //public LeagueMemberInfoDTO LastModifiedBy { get; set; }

        public LeagueMemberDataDTO() { }
    }
}
