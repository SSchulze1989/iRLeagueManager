using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using iRLeagueDatabase.DataTransfer.Members;
using iRLeagueDatabase.DataTransfer.Sessions;
using iRLeagueManager.Enums;
using LeagueDBService;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace iRLeagueDatabase.DataTransfer.Reviews
{
    [Serializable]
    [DataContract]
    public class IncidentReviewDataDTO : IncidentReviewInfoDTO
    {
        public override Type Type => typeof(IncidentReviewDataDTO);

        //[DataMember]
        //public int ReviewId { get; set; }
        //[DataMember]
        //public int SeasonId;
        //[DataMember]
        //public LeagueMemberDTO Author { get; set; }
        [DataMember]
        public SessionInfoDTO Session { get; set; }
        [DataMember]
        public LeagueMemberInfoDTO Author { get; set; }
        [DataMember]
        public int OnLap { get; set; }
        [DataMember]
        public int Corner { get; set; }
        [DataMember]
        public TimeSpan TimeStamp { get; set; }
        [DataMember]
        public List<LeagueMemberInfoDTO> InvolvedMembers { get; set; }
        [DataMember]
        public List<ReviewCommentDataDTO> Comments { get; set; }
        //[DataMember]
        //public LeagueMemberInfoDTO MemberAtFault { get; set; }
        //[DataMember]
        //public VoteEnum VoteResult { get; set; }
        //[DataMember]
        //public VoteState VoteState { get; set; }

        //[DataMember]
        //public LeagueMemberInfoDTO CreatedBy { get; set; }
        //[DataMember]
        //public LeagueMemberInfoDTO LastModifiedBy { get; set; }

        public IncidentReviewDataDTO() { }
    }
}
