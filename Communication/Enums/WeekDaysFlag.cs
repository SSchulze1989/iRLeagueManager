using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Enums
{
    [Flags]
    public enum WeekDaysFlag
    {
        Sunday      =   1<<DayOfWeek.Sunday,
        Monday      =   1<<DayOfWeek.Monday,
        Tuesday     =   1<<DayOfWeek.Tuesday,
        Wednesday   =   1<<DayOfWeek.Wednesday,
        Thursday    =   1<<DayOfWeek.Thursday,
        Friday      =   1<<DayOfWeek.Friday,
        Saturday    =   1<<DayOfWeek.Saturday
    }
}
