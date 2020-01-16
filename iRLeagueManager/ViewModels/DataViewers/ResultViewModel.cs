using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.ViewModels
{
    public class ResultModel : ContainerModelBase<IResult>, IResult
    {
        [EqualityCheckProperty]
        public uint ResultId => Source.ResultId;

        [EqualityCheckProperty]
        private uint _sessionId => Session.SessionId;

        public SessionModel Session
        {
            get => Season?.Schedules
                .Select(x => x.Sessions)
                .Cast<IEnumerable<SessionModel>>()
                .Aggregate((x, y) => x.Concat(y))
                .SingleOrDefault(x => x.SessionId == Source.Session.SessionId);
            set
            {
                Source.Session = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(IResult.Session));
            }
        }
        ISession IResult.Session { get => Session; set { Source.Session = value; NotifyPropertyChanged(); NotifyPropertyChanged(nameof(Session)); } }

        public SeasonModel Season { get; internal set; }

        private ObservableModelCollection<ResultRowModel, IResultRow> _rawResults;
        public ObservableModelCollection<ResultRowModel, IResultRow> RawResults { get => _rawResults; set { _rawResults = value; NotifyPropertyChanged(); } }

        IEnumerable<IResultRow> IResult.RawResults => Source.RawResults;

        public IEnumerable<IReview> Reviews => Source.Reviews;

        public IEnumerable<IResultRow> FinalResults => Source.FinalResults;

        public ResultModel() : base()
        {
            RawResults = new ObservableModelCollection<ResultRowModel, IResultRow>();
        }
        public ResultModel(IResult source) : base(source)
        {
            RawResults = new ObservableModelCollection<ResultRowModel, IResultRow>(source.RawResults);
        }

        public override void UpdateSource(IResult source)
        {
            base.UpdateSource(source);
            RawResults.UpdateSource(source.RawResults);
        }

        public IReview AddReview(IAdmin admin)
        {
            return Source.AddReview(admin);
        }

        public void RemoveReview(IReview review)
        {
            Source.RemoveReview(review);
        }

        public void SetRawResults(IEnumerable<IResultRow> resultRows)
        {
            Source.SetRawResults(resultRows);
        }

        public object GetSourceObject()
        {
            return Source.GetSourceObject();
        }
    }
}
