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
using iRLeagueManager.Enums;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models;

using iRLeagueManager.Interfaces;
using System.Collections.ObjectModel;
using iRLeagueManager.Models.User;

namespace iRLeagueManager.Models.Reviews
{
    [Serializable]
    public class ReviewCommentModel : CommentModel, IReviewComment, INotifyPropertyChanged
    {
        //private IncidentReviewModel review;
        //public IncidentReviewModel Review { get => review; internal set { review = value; OnPropertyChanged(); } }

        //public ScheduleModel Schedule => Review?.Schedule;

        //public override SeasonModel Season => Schedule?.Season;
        private IncidentReviewInfo review;
        public IncidentReviewInfo Review { get => review; internal set => SetValue(ref review, value); }

        private ObservableCollection<ReviewVoteModel> commentReviewVotes = new ObservableCollection<ReviewVoteModel>();
        public ObservableCollection<ReviewVoteModel> CommentReviewVotes { get => commentReviewVotes; set => SetNotifyCollection(ref commentReviewVotes, value); }

        public ReviewCommentModel () { }

        public ReviewCommentModel(long commentId, string authorName) : base(commentId, authorName) 
        {
            Review = review;
        }

        public ReviewCommentModel(UserModel author) : base(author)
        {
        }

        public ReviewCommentModel(UserModel author, IncidentReviewInfo review) : this(author) 
        {
            Review = review;
        }

        internal override void InitializeModel()
        {
            foreach (var vote in CommentReviewVotes)
            {
                vote.InitializeModel();
            }
            base.InitializeModel();
        }

        public override void CopyFrom(ModelBase sourceObject)
        {
            base.CopyFrom(sourceObject);

            if (sourceObject is ReviewCommentModel commentModel)
            {
                InitReset();

                CommentReviewVotes = new ObservableCollection<ReviewVoteModel>(commentModel.CommentReviewVotes.Select(x =>
                {
                    var vote = new ReviewVoteModel();
                    vote.CopyFrom(x);
                    return vote;
                }).ToList());

                if (commentModel.isInitialized)
                {
                    InitializeModel();
                }
            }
        }

        public override void CopyTo(ModelBase targetObject)
        {
            base.CopyTo(targetObject);

            if (targetObject is ReviewCommentModel commentModel)
            {
                commentModel.InitReset();
                commentModel.CommentReviewVotes = new ObservableCollection<ReviewVoteModel>(CommentReviewVotes.Select(x =>
                {
                    var vote = new ReviewVoteModel();
                    vote.CopyFrom(x);
                    return vote;
                }).ToList());
                if (isInitialized)
                {
                    commentModel.InitializeModel();
                }
            }
        }

        //public override void SetLeagueClient(IRLeagueClient leagueClient)
        //{
        //    base.SetLeagueClient(leagueClient);
        //    MemberAtFault = (MemberAtFault != null) ? client.LeagueMembers.Single(x => x.MemberId == MemberAtFault.MemberId) : null;
        //}
    }
}
