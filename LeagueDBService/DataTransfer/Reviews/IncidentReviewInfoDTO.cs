using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Reviews
{
    [DataContract]
    public class IncidentReviewInfoDTO : VersionInfoDTO, IMappableDTO
    {
        [DataMember]
        public int? ReviewId { get; set; }

        object IMappableDTO.MappingId => ReviewId;
    }
}
