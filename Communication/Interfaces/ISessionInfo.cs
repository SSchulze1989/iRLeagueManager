using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using iRLeagueManager.Attributes;
using iRLeagueManager.Enums;

namespace iRLeagueManager.Interfaces
{
    public interface ISessionInfo : INotifyPropertyChanged
    {
        [EqualityCheckProperty]
        long? SessionId { get; }
        [EqualityCheckProperty]
        SessionType SessionType { get; }
        DateTime Date { get; }
        //string LocationId { get; }
    }
}
