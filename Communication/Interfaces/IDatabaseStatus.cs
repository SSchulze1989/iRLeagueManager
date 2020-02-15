using System;
using System.Collections.Generic;
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

        void SetConnectionStatus(IToken token, ConnectionStatusEnum status);

        void SetDatabaseStatus(IToken token, DatabaseStatusEnum status);
        void SetDatabaseStatus(IToken token, DatabaseStatusEnum status, string endpointAddress);
    }
}
