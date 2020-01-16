using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueManager.Enums;
using LeagueDBService;
using System.Runtime.Serialization;

namespace iRLeagueDatabase.DataTransfer.Reviews
{
    [Serializable]
    [DataContract]
    public class ReviewCommentDataDTO : CommentDataDTO
    {
        [DataMember]
        public VoteEnum Vote { get; set; }
        //[DataMember]
        //public IncidentReviewInfoDTO Review { get; set; }
        [DataMember]
        public LeagueMemberInfoDTO MemberAtFault { get; set; }

        public ReviewCommentDataDTO () { }
    }
}
