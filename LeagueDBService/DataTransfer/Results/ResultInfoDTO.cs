using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using iRLeagueDatabase.DataTransfer.Sessions;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    public class ResultInfoDTO : VersionInfoDTO, IMappableDTO
    {
        [DataMember]
        public int? ResultId { get; set; }
        [DataMember]
        public SessionInfoDTO Session { get; set; }

        object IMappableDTO.MappingId => ResultId;
    }
}
