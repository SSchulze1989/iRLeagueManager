using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Models.User;

namespace iRLeagueManager.Models.Reviews
{
    public class CommentInfo : MappableModel
    {
        public long? CommentId { get; internal set; }
        public string AuthorName { get; internal set; }
        //public string AuthorUserId { get; internal set; }
        public UserModel Author { get; internal set; }
        public override long[] ModelId => new long[] { CommentId.GetValueOrDefault() };

        public CommentInfo() { }

        public CommentInfo(long? commentId, string authorName)
        {
            CommentId = commentId;
            AuthorName = authorName;
        }

        public CommentInfo(UserModel author)
        {
            Author = author;
        }
    }
}
