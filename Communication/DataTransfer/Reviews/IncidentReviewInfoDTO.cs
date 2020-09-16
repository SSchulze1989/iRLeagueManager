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
        public long? ReviewId { get; set; }
        [DataMember]
        public string AuthorUserId { get; set; }
        [DataMember]
        public string AuthorName { get; set; }

        public override object MappingId => ReviewId;

        public override object[] Keys => new object[] { ReviewId };

        //object IMappableDTO.MappingId => ReviewId;
    }
}
