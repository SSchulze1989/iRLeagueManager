using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iRLeagueManager.Attributes;
using System.ComponentModel;

namespace iRLeagueManager.Interfaces
{
    public interface IResult : INotifyPropertyChanged
    {
        [EqualityCheckProperty]
        long? ResultId { get; }
        //ISession Session { get; }
        IEnumerable<IResultRow> RawResults { get; }
        IEnumerable<IReviewInfo> Reviews { get; }
        //IEnumerable<IResultRow> FinalResults { get; }

        //IReview AddReview(IAdmin admin);
        //void RemoveReview(IReview review);
        //void SetRawResults(IEnumerable<IResultRow> resultRows);
    }
}
