using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iRLeagueDatabase.Entities.Members;
using iRLeagueManager.Enums;

namespace iRLeagueDatabase.Entities.Reviews
{
    [Serializable]
    public class ReviewCommentEntity : CommentBaseEntity
    {
        public VoteEnum Vote { get; set; }

        [ForeignKey(nameof(Review))]
        public int? ReviewId { get; set; }
        public IncidentReviewEntity Review { get; set; }

        public LeagueMemberEntity MemberAtFault { get; set; }

        public ReviewCommentEntity () { }
    }
}
