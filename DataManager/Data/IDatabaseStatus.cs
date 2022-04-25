// MIT License

using System;

namespace iRLeagueManager.Data
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