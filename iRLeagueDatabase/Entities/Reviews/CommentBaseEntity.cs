using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iRLeagueDatabase.Entities.Members;

namespace iRLeagueDatabase.Entities.Reviews
{
    [Serializable]
    public class CommentBaseEntity : Revision
    {
        [Key]
        public int CommentId { get; set; }

        public override object MappingId => CommentId;

        public DateTime? Date { get; set; }

        //[ForeignKey(nameof(Author))]
        //public int AuthorId { get; set; }
        public LeagueMemberEntity Author { get; set; }

        public string Text { get; set; }

        public CommentBaseEntity()
        {
            Date = DateTime.Now;
        }
    }
}
