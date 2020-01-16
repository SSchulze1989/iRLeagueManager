using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using iRLeagueManager.Attributes;

namespace iRLeagueManager.Interfaces
{
    public interface IScoringInfo : INotifyPropertyChanged
    {
        [EqualityCheckProperty]
        int ScoringNr { get; }
    }
}
