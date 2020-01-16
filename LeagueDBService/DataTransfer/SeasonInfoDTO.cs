using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer
{
    [DataContract]
    [KnownType(typeof(SeasonDataDTO))]
    public class SeasonInfoDTO : VersionInfoDTO, IMappableDTO
    {
        [DataMember]
        public int SeasonId { get; set; }

        [DataMember]
        public string SeasonName { get; set; }

        object IMappableDTO.MappingId => SeasonId;
    }
}
