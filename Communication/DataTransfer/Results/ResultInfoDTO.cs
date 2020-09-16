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
    [KnownType(typeof(ResultDataDTO))]
    [KnownType(typeof(ScoredResultDataDTO))]
    public class ResultInfoDTO : VersionInfoDTO, IMappableDTO
    {
        [DataMember]
        public long? ResultId { get; set; }
        [DataMember]
        public SessionInfoDTO Session { get; set; }

        public override object MappingId => ResultId;

        public override object[] Keys => new object[] { ResultId };
        //object IMappableDTO.MappingId => ResultId;
    }
}
