using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;

namespace iRLeagueManager.Data
{
    public class DbServiceClientBase
    {
        protected List<IDatabaseStatus> StatusArray { get; }

        protected ConnectionStatusEnum ConnectionStatus { get; private set; }

        protected DatabaseStatusEnum UpdateStatus { get; private set; }

        protected DatabaseToken Token { get; }

        private string CurrentUpdateMethod { get; set; }

        private bool IsBusy { get; set; }

        //private List<string> ActiveUpdates { get; } = new List<string>();
        
        public DbServiceClientBase()
        {
            Token = new DatabaseToken();
            StatusArray = new List<IDatabaseStatus>();
        }

        public DbServiceClientBase(IDatabaseStatus status) : this()
        {
            
            StatusArray.Add(status);
        }

        public void AddStatusItem(IDatabaseStatus statusItem)
        {
            if (!StatusArray.Contains(statusItem))
                StatusArray.Add(statusItem);
        }

        public void RemoveStatusItem(IDatabaseStatus statusItem)
        {
            if (StatusArray.Contains(statusItem))
                StatusArray.Remove(statusItem);
        }

        protected async Task StartUpdateWhenReady(UpdateKind updateKind, int timeoutMilliseconds = 60000, [CallerMemberName] string callName = "")
        {
            if (CurrentUpdateMethod != null && IsBusy)
            {
                Task waitTask = new Task(() =>
                {
                    while (IsBusy) { }
                });
                if (await Task.WhenAny(waitTask, Task.Delay(timeoutMilliseconds)) != waitTask)
                {
                    throw new TimeoutException("Database request timed out on " + callName + ". Request was blocked by " + CurrentUpdateMethod + " - the procces was has not finished yet." + "\nTimeout: " + timeoutMilliseconds.ToString() + " ms");
                }
            }

            if (callName != "")
            {
                CurrentUpdateMethod = callName;
                switch (updateKind)
                {
                    case UpdateKind.Loading:
                        //UpdateStatus = DatabaseStatusEnum.Loading;
                        SetDatabaseStatus(Token, DatabaseStatusEnum.Loading);
                        break;
                    case UpdateKind.Saving:
                        //UpdateStatus = DatabaseStatusEnum.Saving;
                        SetDatabaseStatus(Token, DatabaseStatusEnum.Saving);
                        break;
                    case UpdateKind.Updating:
                        SetDatabaseStatus(Token, DatabaseStatusEnum.Updating);
                        break;
                    default:
                        break;
                }
                IsBusy = true;
            }
        }

        protected virtual void SetConnectionStatus(IToken token, ConnectionStatusEnum status)
        {
            ConnectionStatus = status;
            foreach (var statusItem in StatusArray)
            {
                statusItem.SetConnectionStatus(token, status);
            }
        }

        protected virtual void SetDatabaseStatus(IToken token, DatabaseStatusEnum status, string endpointAddress = "")
        {
            UpdateStatus = status;
            foreach (var statusItem in StatusArray)
            {
                statusItem.SetDatabaseStatus(token, status, endpointAddress);
            }
        }

        protected void EndUpdate([CallerMemberName] string callName = "")
        {
            if (CurrentUpdateMethod == callName && IsBusy)
            {
                CurrentUpdateMethod = null;
                //UpdateStatus = DatabaseStatusEnum.Idle;
                SetDatabaseStatus(Token, DatabaseStatusEnum.Idle);
                IsBusy = false;
            }
            else if (IsBusy)
            {
                throw new InvalidOperationException("Database request " + callName + "Can not be finished! Another request is currently active on the database: " + CurrentUpdateMethod);
            }
            else
            {
                throw new InvalidOperationException("Database request " + callName + "Can not be finished! No active request on the database found.");
            }
        }

        public enum UpdateKind
        {
            Loading,
            Saving,
            Updating
        }
    }
}
