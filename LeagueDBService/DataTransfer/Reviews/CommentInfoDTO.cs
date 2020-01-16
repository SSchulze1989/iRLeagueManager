using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Reviews
{
    [DataContract]
    [KnownType(typeof(CommentDataDTO))]
    [KnownType(typeof(ReviewCommentDataDTO))]
    public class CommentInfoDTO : VersionInfoDTO, IMappableDTO
    {
        [DataMember]
        public int CommentId { get; set; }

        object IMappableDTO.MappingId => CommentId;
    }
}
