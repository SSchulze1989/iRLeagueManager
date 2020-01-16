using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Attributes;
using System.ComponentModel;

namespace iRLeagueManager.Interfaces
{
    public interface ISeasonInfo : INotifyPropertyChanged
    {
        [EqualityCheckProperty]
        int? SeasonId { get; }
        string SeasonName { get; }

        //int GetSessionCount();
        //ISession GetLastSession();
        //IRaceSession GetLastRace();
    }
}
