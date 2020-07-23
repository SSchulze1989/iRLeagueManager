using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using iRLeagueManager.LeagueDBServiceRef;

using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;
using iRLeagueManager.Data;

namespace iRLeagueManager.Models.Database
{
    public class DatabaseStatusModel : MappableModel, IDatabaseStatus
    {
        private ConnectionStatusEnum databaseConnectionStatus;
        public ConnectionStatusEnum ConnectionStatus { get => databaseConnectionStatus; }

        private Dictionary<Guid, DatabaseStatusEnum> databaseStatus;
        public DatabaseStatusEnum UpdateStatus { get => GetDatabaseStatus(); }

        private string endpointAddress;
        public string EndpointAddress { get => endpointAddress; protected set => SetValue(ref endpointAddress, value); }

        public override long[] ModelId => null;

        public DatabaseStatusModel()
        {
            databaseStatus = new Dictionary<Guid, DatabaseStatusEnum>();
        }

        private DatabaseStatusEnum GetDatabaseStatus()
        {
            DatabaseStatusEnum result = DatabaseStatusEnum.Idle;

            foreach(var status in databaseStatus.Values)
            {
                if (status == DatabaseStatusEnum.Loading && result == DatabaseStatusEnum.Saving)
                {
                    result = DatabaseStatusEnum.Updating;
                }

                if (status > result)
                {
                    result = status;
                }
            }

            return result;
        }

        public void SetConnectionStatus(Guid token, ConnectionStatusEnum status)
        {
            //databaseConnectionStatus = status;
            SetValue(ref databaseConnectionStatus, status, nameof(ConnectionStatus));
        }

        public void SetDatabaseStatus(Guid token, DatabaseStatusEnum status)
        {
            if (databaseStatus.Keys.Contains(token))
            {
                if (status == 0)
                {
                    databaseStatus[token] = status;
                    databaseStatus.Remove(token);
                }
                else
                    databaseStatus[token] = status;
            }
            else
            {
                databaseStatus.Add(token, status);
            }
            OnPropertyChanged(nameof(UpdateStatus));
        }

        public void SetDatabaseStatus(Guid token, DatabaseStatusEnum status, string endpointAddress)
        {
            SetDatabaseStatus(token, status);
            EndpointAddress = endpointAddress;
        }
    }
}
