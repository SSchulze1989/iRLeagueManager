using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Attributes;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace iRLeagueManager.Interfaces
{
    public interface IScoring : IScoringInfo, INotifyPropertyChanged
    {
        int DropWeeks { get; set; }
        int AverageRaceNr { get; set; }
        IScheduleInfo Schedule { get; set; }
        ReadOnlyObservableCollection<IRaceSessionInfo> Races { get; }
    }
}
