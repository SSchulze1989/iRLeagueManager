// MIT License

// Copyright (c) 2020 Simon Schulze

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using iRLeagueDatabase.DataTransfer;

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
