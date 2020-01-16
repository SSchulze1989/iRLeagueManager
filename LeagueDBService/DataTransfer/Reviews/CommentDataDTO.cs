using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer;
using LeagueDBService;

namespace iRLeagueDatabase.DataTransfer.Reviews
{
    [DataContract]   
    [KnownType(typeof(ReviewCommentDataDTO))]
    public class CommentDataDTO : CommentInfoDTO
    {
        public override Type Type => typeof(CommentDataDTO);

        //[DataMember]
        //public int CommentId { get; set; }
        [DataMember]
        public DateTime? Date { get; set; } = null;
        [DataMember]
        public LeagueMemberInfoDTO Author { get; set; }
        [DataMember]
        public string Text { get; set; }

        //[DataMember]
        //public LeagueMemberInfoDTO CreatedBy { get; set; }
        //[DataMember]
        //public LeagueMemberInfoDTO LastModifiedBy { get; set; }

        public CommentDataDTO() { }
    }
}
