using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Attributes;
using iRLeagueManager.Enums;
using System.Collections.ObjectModel;

namespace iRLeagueManager.Interfaces
{
    public interface IScheduleInfo
    {
        [EqualityCheckProperty]
        long? ScheduleId { get; }
        [EqualityCheckProperty]
        string Name { get; }

        int SessionCount { get; }
    }
}
