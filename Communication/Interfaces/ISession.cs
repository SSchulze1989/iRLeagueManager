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
    public interface ISession : ISessionInfo, INotifyPropertyChanged
    {
        [EqualityCheckProperty]
        new SessionType SessionType { get; set; }

        new DateTime Date { get; set; }
        //new string LocationId { get; }
        //ILocation Location { get; set; }
        TimeSpan Duration { get; set; }
        //IResult Result { get; }
    }
}
