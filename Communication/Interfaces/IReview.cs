using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.Interfaces
{
    public interface IReview : IReviewInfo, INotifyPropertyChanged
    {
        IEnumerable<IReviewComment> Comments { get; }
    }
}
