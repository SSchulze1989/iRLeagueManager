using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using iRLeagueDatabase.DataTransfer.Members;

namespace iRLeagueDatabase.DataTransfer.Sessions
{
    [Serializable()]
    [DataContract]
    public class ScheduleDataDTO : ScheduleInfoDTO
    {
        //[DataMember]
        //public int ScheduleId { get; set; }
        //[DataMember]
        //public SeasonInfoDTO Season { get; set; }
        [DataMember]
        public ICollection<SessionDataDTO> Sessions { get; set; } = new List<SessionDataDTO>();

        [DataMember]
        public LeagueMemberInfoDTO CreatedBy { get; set; }
        [DataMember]
        public LeagueMemberInfoDTO LastModifiedBy { get; set; }

        public ScheduleDataDTO() { }
    }
}
