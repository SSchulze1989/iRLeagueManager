using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Results
{
    [DataContract]
    [KnownType(typeof(ScoringDataDTO))]
    public class ScoringInfoDTO : VersionInfoDTO, IMappableDTO
    {
        [DataMember]
        public long? ScoringId { get; set; }

        public override object MappingId => ScoringId;

        public override object[] Keys => new object[] { ScoringId };

        //object IMappableDTO.MappingId => ScoringId.GetValueOrDefault();
    }
}
