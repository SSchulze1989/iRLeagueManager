using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using iRLeagueManager.Models.Members;
using System.Collections.ObjectModel;

namespace iRLeagueManager.Models.Reviews
{
    public class CommentBase : CommentInfo, INotifyPropertyChanged
    {
        //private SeasonModel season;
        //public virtual SeasonModel Season { get => season; internal set { season = value; OnPropertyChanged(); } }
        private CommentInfo replyTo;
        public CommentInfo ReplyTo { get => replyTo; internal set => SetValue(ref replyTo, value); }

        private DateTime date;
        public DateTime Date { get => date; internal set => SetValue(ref date, value); }

        //private LeagueMember author;
        //public LeagueMember Author { get => author; internal set => SetValue(ref author, value); }
        private UserModel author;
        public UserModel Author { get => author; internal set => SetValue(ref author, value); }

        private string text;
        public string Text { get => text; set => SetValue(ref text, value); }

        private ObservableCollection<CommentBase> replies;
        public ObservableCollection<CommentBase> Replies { get => replies; set => SetNotifyCollection(ref replies, value); }

        public CommentBase() { }

        public CommentBase(long commentId, string authorName) : base(commentId, authorName)
        {
        }

        public CommentBase(UserModel author)
        {
            Author = author;
        }

        public CommentBase(UserModel author, CommentInfo replyTo) : this(author)
        {
            ReplyTo = replyTo;
        }

        internal override void InitializeModel()
        {
            base.InitializeModel();
        }
    }
}
