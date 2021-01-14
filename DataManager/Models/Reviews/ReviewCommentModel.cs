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
using iRLeagueDatabase.Extensions;

namespace iRLeagueManager.Models.Reviews
{
    [Serializable]
    public class ReviewCommentModel : CommentModel, IReviewComment, INotifyPropertyChanged
    {
        //private IncidentReviewModel review;
        //public IncidentReviewModel Review { get => review; internal set { review = value; OnPropertyChanged(); } }

        //public ScheduleModel Schedule => Review?.Schedule;

        //public override SeasonModel Season => Schedule?.Season;
        //private IncidentReviewInfo review;
        //public IncidentReviewInfo Review { get => review; internal set => SetValue(ref review, value); }
        private long? reviewId;
        public long? ReviewId { get => reviewId; internal set => SetValue(ref reviewId, value); }

        private ObservableCollection<ReviewVoteModel> commentReviewVotes;
        public ObservableCollection<ReviewVoteModel> CommentReviewVotes { get => commentReviewVotes; set => SetNotifyCollection(ref commentReviewVotes, value); }

        public ReviewCommentModel () 
        {
            CommentReviewVotes = new ObservableCollection<ReviewVoteModel>();
        }

        public ReviewCommentModel(long commentId, string authorName) : base(commentId, authorName) 
        {
            CommentReviewVotes = new ObservableCollection<ReviewVoteModel>();
        }

        public ReviewCommentModel(UserModel author) : base(author)
        {
            CommentReviewVotes = new ObservableCollection<ReviewVoteModel>();
        }

        public ReviewCommentModel(UserModel author, IncidentReviewInfo review) : this(author) 
        {
            ReviewId = review.ReviewId;
        }

        internal override void InitializeModel()
        {
            foreach (var vote in CommentReviewVotes)
            {
                vote.InitializeModel();
            }
            base.InitializeModel();
        }

        public override void CopyFrom(ModelBase sourceObject, params string[] excludeProperties)
        {
            base.CopyFrom(sourceObject, excludeProperties.Concat(new string[] { nameof(CommentReviewVotes) }).ToArray());

            if (sourceObject is ReviewCommentModel commentModel)
            {
                InitReset();

                CommentReviewVotes.MapCollection(commentModel.CommentReviewVotes.Select(x =>
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

        public override void CopyTo(ModelBase targetObject, params string[] excludeProperties)
        {
            base.CopyTo(targetObject, excludeProperties.Concat(new string[] { nameof(CommentReviewVotes) }).ToArray());

            if (targetObject is ReviewCommentModel commentModel)
            {
                commentModel.InitReset();
                commentModel.CommentReviewVotes.MapCollection(CommentReviewVotes.Select(x =>
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
