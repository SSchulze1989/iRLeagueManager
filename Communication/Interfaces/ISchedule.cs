using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using iRLeagueManager.Attributes;
using iRLeagueManager.Enums;
using System.Collections.ObjectModel;

namespace iRLeagueManager.Interfaces
{
    public interface ISchedule : IScheduleInfo
    {
        //ISeasonInfo Season { get; }
        [EqualityCheckProperty]
        new string Name { get; set; }

        ObservableCollection<ISession> Sessions { get; }

        //ISession AddSession(SessionType sessionType);
        //void RemoveSession(ISession session);
    }
}
