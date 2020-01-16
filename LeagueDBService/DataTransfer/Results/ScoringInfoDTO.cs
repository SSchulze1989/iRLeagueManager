using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Results
{
    public class ScoringInfoDTO : VersionInfoDTO, IMappableDTO
    {
        [DataMember]
        public int ScoringId { get; set; }

        object IMappableDTO.MappingId => ScoringId;
    }
}
