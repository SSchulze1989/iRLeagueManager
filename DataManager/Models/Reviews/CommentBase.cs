using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.ComponentModel;
using iRLeagueManager.Models.Members;
using System.Collections.ObjectModel;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.Models.Reviews
{
    public class CommentModel : CommentInfo, INotifyPropertyChanged
    {
        private CommentInfo replyTo;
        public CommentInfo ReplyTo { get => replyTo; set => SetValue(ref replyTo, value); }

        private DateTime date = DateTime.Now;
        public DateTime Date { get => date; internal set => SetValue(ref date, value); }

        private string text;
        public string Text { get => text; set => SetValue(ref text, value); }

        private ObservableCollection<CommentModel> replies = new ObservableCollection<CommentModel>();
        public ObservableCollection<CommentModel> Replies { get => replies; set => SetNotifyCollection(ref replies, value); }

        public CommentModel() { }

        public CommentModel(long commentId, string authorName) : base(commentId, authorName)
        {
        }

        public CommentModel(UserModel author) : base(author)
        {
        }

        public CommentModel(UserModel author, CommentInfo replyTo) : this(author)
        {
            ReplyTo = replyTo;
        }

        internal override void InitializeModel()
        {
            base.InitializeModel();
        }
    }
}
