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
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Models.Sessions;
using iRLeagueManager.Models.Reviews;

namespace iRLeagueManager.Models.Results
{
    public class ResultModel : ResultInfo, IHierarchicalModel
    {
        //private SessionModel session;
        //public SessionModel Session { get => session; set => SetValue(ref session, value);  }
        //ISession IResult.Session => Session;

        //public ScheduleModel Schedule => Session?.Schedule;

        //public SeasonModel Season => Schedule?.Season;

        /// <summary>
        /// Data rows of results table
        /// </summary>
        private ObservableCollection<ResultRowModel> rawResults;
        public ObservableCollection<ResultRowModel> RawResults { get => rawResults; set => SetNotifyCollection(ref rawResults, value); }
        //IEnumerable<IResultRow> IResult.RawResults => RawResults;

        private ObservableCollection<IncidentReviewInfo> reviews;
        public ObservableCollection<IncidentReviewInfo> Reviews { get => reviews; set => SetNotifyCollection(ref reviews, value); }
        //IEnumerable<IReviewInfo> IResult.Reviews => Reviews;

        //IEnumerable<IResultRow> IResult.FinalResults => RawResults;

        public SimSessionDetails SimSessionDetails { get; set; }

        public string Description { get; set; }

        //string IHierarchicalModel.Description => Description;

        IEnumerable<object> IHierarchicalModel.Children => Reviews.Cast<object>();

        //private List<ResultRow> finalResults;
        //public List<ResultRow> FinalResults { get => finalResults; set { finalResults = value; OnPropertyChanged(); } }

        public ResultModel() : base(0)
        {
            ResultId = 0;
            RawResults = new ObservableCollection<ResultRowModel>();
            Reviews = new ObservableCollection<IncidentReviewInfo>();
            //FinalResults = new List<ResultRow>();
        }

        public ResultModel(SessionModel session) : this()
        {
            //Session = session;
            //session.SessionResult = this;
            Session = session;
            ResultId = Session.SessionId;
            session.SessionResult = this;
        }

        public ResultModel(long resultId) : this()
        {
            ResultId = resultId;
        }

        internal override void InitializeModel()
        {
            if (!isInitialized)
            {
                //if (session != null)
                //{
                //    //Reviews = new ObservableCollection<IncidentReviewModel>(Season.Reviews.Where(x => Reviews.Select(y => y.ReviewId).Contains(x.ReviewId)));
                //}
            }
            base.InitializeModel();
        }

        //void IResult.SetRawResults(IEnumerable<IResultRow> resultRows)
        //{
        //    RawResults = resultRows.Cast<ResultRow>().ToList();
        //}

        //public IReview AddReview(IAdmin admin)
        //{
        //    IncidentReview review = new IncidentReview();
        //    Reviews.Add(review);
        //    return review;
        //}

        //public void RemoveReview(IReview review)
        //{
        //    Reviews.Remove(Reviews.Single(x => x.ReviewId == review.ReviewId));
        //}

        //internal List<CommentBase> GetComments()
        //{
        //    List<CommentBase> allComments = new List<CommentBase>();
        //    allComments.AddRange(Reviews.Select(x => x.Comments).Aggregate((x,y) => x.Concat(y).ToList()));
        //    return allComments;
        //}

        //public object GetSourceObject()
        //{
        //    return this;
        //}
    }
}
