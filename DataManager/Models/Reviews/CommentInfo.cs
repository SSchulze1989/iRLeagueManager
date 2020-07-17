using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Models.Reviews
{
    public class CommentInfo : ModelBase
    {
        public long? CommentId { get; internal set; }
        public string AuthorName { get; internal set; }
        public override long[] ModelId => new long[] { CommentId.GetValueOrDefault() };

        public CommentInfo() { }

        public CommentInfo(long? commentId, string authorName)
        {
            CommentId = commentId;
            AuthorName = authorName;
        }
    }
}
