// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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

        private ObservableCollection<CommentModel> replies;
        public ObservableCollection<CommentModel> Replies { get => replies; set => SetNotifyCollection(ref replies, value); }
        
        public CommentModel() { }

        public CommentModel(long commentId, string authorName) : base(commentId, authorName)
        {
            Replies = new ObservableCollection<CommentModel>();
        }

        public CommentModel(UserModel author) : base(author)
        {
            Replies = new ObservableCollection<CommentModel>();
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
