using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iRLeagueDatabase.Entities.Members;
using iRLeagueDatabase.Entities.Sessions;
using iRLeagueDatabase.Entities.Results;
using iRLeagueManager.Enums;


namespace iRLeagueDatabase.Entities.Reviews
{
    [Serializable]
    public class IncidentReviewEntity : Revision
    {
        [Key]
        public int ReviewId { get; set; }

        public override object MappingId => ReviewId;

        public virtual ResultEntity Result { get; set; }

        //[ForeignKey(nameof(Session))]
        //public int SessionId { get; set; }
        //public virtual SessionBaseEntity Session { get; set; }
        public SessionBaseEntity Session => Result.Session;

        //[ForeignKey(nameof(Author))]
        //public int AuthorId { get; set; }
        public virtual LeagueMemberEntity Author { get; set; }

        public int OnLap { get; set; }
        
        public int Corner { get; set; }
        
        public TimeSpan TimeStamp { get; set; }
        
        public virtual List<LeagueMemberEntity> InvolvedMembers { get; set; }
        
        [InverseProperty(nameof(ReviewCommentEntity.Review))]
        public virtual List<ReviewCommentEntity> Comments { get; set; }

        //[ForeignKey(nameof(MemberAtFault))]
        //public int MeberAtFaultId { get; set; }
        public virtual LeagueMemberEntity MemberAtFault { get; set; }

        public VoteEnum VoteResult { get; set; }

        public VoteState VoteState { get; set; }

        public IncidentReviewEntity()
        {
            InvolvedMembers = new List<LeagueMemberEntity>();
            Comments = new List<ReviewCommentEntity>();
        }

        public IncidentReviewEntity(LeagueMemberEntity author) : this()
        {
            //Author = author;
        }
    }
}
