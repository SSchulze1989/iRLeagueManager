using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Enums;

namespace iRLeagueManager.Interfaces
{
    public interface IDatabaseStatus
    {
        ConnectionStatusEnum ConnectionStatus { get; }
        DatabaseStatusEnum UpdateStatus { get; }

        void SetConnectionStatus(Guid token, ConnectionStatusEnum status);

        void SetDatabaseStatus(Guid token, DatabaseStatusEnum status);
        void SetDatabaseStatus(Guid token, DatabaseStatusEnum status, string endpointAddress);
    }
}
