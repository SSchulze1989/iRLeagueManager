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
        public long? CommentId { get; set; }
        [DataMember]
        public string AuthorName { get; set; }
        [DataMember]
        public string AuthorUserId { get; set; }

        public override object MappingId => CommentId;

        public override object[] Keys => new object[] { CommentId };

        //object IMappableDTO.MappingId => CommentId;
    }
}
