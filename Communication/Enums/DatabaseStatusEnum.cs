using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Enums
{
    public enum DatabaseStatusEnum
    {
        Idle = 0,
        Loading = 1,
        Saving = 2,
        Updating = 3,
        Error = 99
    }
}
