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
        public int? ScheduleId { get; set; }

        object IMappableDTO.MappingId => ScheduleId;
    }
}
