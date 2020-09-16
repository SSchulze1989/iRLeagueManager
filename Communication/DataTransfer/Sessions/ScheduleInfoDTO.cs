using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Sessions
{
    [DataContract]
    [KnownType(typeof(ScheduleDataDTO))]
    public class ScheduleInfoDTO : VersionInfoDTO, IMappableDTO
    {
        [DataMember]
        public long? ScheduleId { get; set; }

        public override object MappingId => ScheduleId;
        
        [DataMember]
        public string Name { get; set; }

        public override object[] Keys => new object[] { ScheduleId };

        //object IMappableDTO.MappingId => ScheduleId.GetValueOrDefault();
    }
}
