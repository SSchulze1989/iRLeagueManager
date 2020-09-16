using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueManager.Enums;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Reviews
{
    [Serializable]
    [DataContract]
    public class ReviewCommentDataDTO : CommentDataDTO
    {
        [DataMember]
        public IncidentReviewInfoDTO Review { get; set; }
        [DataMember]
        public ReviewVoteDataDTO[] CommentReviewVotes { get; set; }

        public ReviewCommentDataDTO () { }
    }
}
