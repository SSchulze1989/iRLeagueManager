using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Enums
{
    public enum ConnectionStatusEnum
    {
        Disconnected = 0,
        Connected = 1,
        Connecting = 2,
        DatabaseUnavailable = 3,
        NoConnection = 4,
        ConnectionError = 99
    }
}
