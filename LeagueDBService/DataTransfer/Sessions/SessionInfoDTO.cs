using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using iRLeagueManager.Enums;

namespace iRLeagueDatabase.DataTransfer.Sessions
{
    [DataContract]
    [KnownType(typeof(VersionInfoDTO))]
    //[KnownType(typeof(SessionDataDTO))]
    public class SessionInfoDTO : VersionInfoDTO, IMappableDTO
    {
        [DataMember]
        public int? SessionId { get; set; }

        [DataMember]
        /// <summary>
        /// Type of this session. (Practice, Qualifying or Race)
        /// </summary>
        public SessionType SessionType { get; set; }

        object IMappableDTO.MappingId => SessionId;
    }
}
