using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using iRLeagueManager.Models.Members;

namespace iRLeagueManager.Models.Reviews
{
    [Serializable]
    [XmlInclude(typeof(ReviewCommentModel))]
    public class CommentBase : ModelBase, INotifyPropertyChanged
    {
        private long commentId;
        public long CommentId { get => commentId; internal set { commentId = value; OnPropertyChanged(); } }

        private SeasonModel season;
        public virtual SeasonModel Season { get => season; internal set { season = value; OnPropertyChanged(); } }

        private DateTime date;
        public DateTime Date { get => date; internal set { date = value; OnPropertyChanged(); } }

        private LeagueMember author;
        public LeagueMember Author { get => author; internal set { author = value; OnPropertyChanged(); } }

        private string text;
        public string Text { get => text; set { text = value; OnPropertyChanged(); } }

        public CommentBase() { }

        public CommentBase(long commentId)
        {
            CommentId = commentId;
        }

        public CommentBase(LeagueMember author)
        {
            Author = author;
        }

        internal override void InitializeModel()
        {
            base.InitializeModel();
        }
    }
}
