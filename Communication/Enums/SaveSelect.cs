using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Enums
{
    [Flags]
    public enum SaveSelect
    {
        LeagueMembers = 0b00000001,
        Admins = 0b00000010,
        Results = 0b00000100,
        Scorings = 0b00001000,
        Schedules = 0b00010000,
        Seasons = 0b00100000,
        Teams = 0b01000000
    }
}
