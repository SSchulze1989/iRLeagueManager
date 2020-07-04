using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Threading;

using iRLeagueManager.Interfaces;
using iRLeagueManager.Enums;

namespace iRLeagueManager.Data
{
    public class DbServiceClientBase
    {
        protected List<IDatabaseStatus> StatusArray { get; }

        protected Dictionary<IToken, UpdateKind> TaskStatus { get; } = new Dictionary<IToken, UpdateKind>();

        protected ConnectionStatusEnum ConnectionStatus { get; private set; }

        protected DatabaseStatusEnum UpdateStatus { get; private set; }

        protected DatabaseToken Token { get; }

        private string CurrentUpdateMethod { get; set; }

        private Task QueuedTask { get; set; }

        private int StackCount { get; set; }

        private bool IsBusy { get; set; }

        protected string username { get; set; }

        protected string password { get; set; }

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

        protected async Task<bool> StartUpdateWhenReady(UpdateKind updateKind, IToken token, int timeoutMilliseconds = 10000, [CallerMemberName] string callName = "")
        {
            //if (CurrentUpdateMethod != null && IsBusy)
            //{
            //    Task waitTask = new Task(() =>
            //    {
            //        while (IsBusy && QueuedTask.Id == Task.CurrentId) { }
            //    });
            //    QueuedTask = waitTask;
            //    waitTask.Start();
            //    if (await Task.WhenAny(waitTask, Task.Delay(timeoutMilliseconds)) != waitTask)
            //    {
            //        throw new TimeoutException("Database request timed out on " + callName + ". Request was blocked by " + CurrentUpdateMethod + " - the procces was has not finished yet." + "\nTimeout: " + timeoutMilliseconds.ToString() + " ms");
            //    }

            //    if (waitTask != QueuedTask)
            //        return false;
            //    else
            //        QueuedTask = null;
            //}

            if (callName != "")
            {
                CurrentUpdateMethod = callName;
                //switch (updateKind)
                //{
                //    case UpdateKind.Loading:
                //        //UpdateStatus = DatabaseStatusEnum.Loading;
                //        SetDatabaseStatus(Token, DatabaseStatusEnum.Loading);
                //        break;
                //    case UpdateKind.Saving:
                //        //UpdateStatus = DatabaseStatusEnum.Saving;
                //        SetDatabaseStatus(Token, DatabaseStatusEnum.Saving);
                //        break;
                //    case UpdateKind.Updating:
                //        SetDatabaseStatus(Token, DatabaseStatusEnum.Updating);
                //        break;
                //    default:
                //        break;
                //}
                //IsBusy = true;
                if (TaskStatus.ContainsKey(token))
                {
                    TaskStatus[token] = updateKind;
                }
                else
                {
                    TaskStatus.Add(token, updateKind);
                }
                SetDatabaseStatus();
            }
            return true;
        }

        protected virtual void SetConnectionStatus(IToken token, ConnectionStatusEnum status)
        {
            ConnectionStatus = status;
            foreach (var statusItem in StatusArray)
            {
                statusItem.SetConnectionStatus(token, status);
            }
        }

        protected virtual void SetDatabaseStatus()
        {
            DatabaseStatusEnum status = DatabaseStatusEnum.Idle;
            IsBusy = false;

            foreach (var updateKind in TaskStatus.Values)
            {
                IsBusy = true;
                switch (updateKind)
                {
                    case UpdateKind.Loading:
                        //UpdateStatus = DatabaseStatusEnum.Loading;
                        if (status == DatabaseStatusEnum.Idle)
                            status = DatabaseStatusEnum.Loading;
                        else
                            status = DatabaseStatusEnum.Updating;
                        break;
                    case UpdateKind.Saving:
                        //UpdateStatus = DatabaseStatusEnum.Saving;
                        if (status == DatabaseStatusEnum.Idle)
                            status = DatabaseStatusEnum.Saving;
                        else
                            status = DatabaseStatusEnum.Updating;
                        break;
                    case UpdateKind.Updating:
                        status = DatabaseStatusEnum.Updating;
                        break;
                    default:
                        break;
                }
            }
            SetDatabaseStatus(Token, status);
        }

        protected virtual void SetDatabaseStatus(IToken token, DatabaseStatusEnum status, string endpointAddress = "")
        {
            UpdateStatus = status;
            foreach (var statusItem in StatusArray)
            {
                statusItem.SetDatabaseStatus(token, status, endpointAddress);
            }
        }

        protected bool IsUpdateRunning([CallerMemberName] string callName = "")
        {
            if (IsBusy)
            {
                return true;
            }
            return false;
        }

        protected void EndUpdate(IToken token, [CallerMemberName] string callName = "")
        {

            //if (CurrentUpdateMethod == callName && IsBusy)
            //{
            //    CurrentUpdateMethod = null;
            //    //UpdateStatus = DatabaseStatusEnum.Idle;
            //    SetDatabaseStatus(Token, DatabaseStatusEnum.Idle);
            //    IsBusy = false;

            //    if (StackCount > 0)
            //    {
            //        StackCount -= 1;
            //    }
            //}
            //else if (IsBusy)
            //{
            //    throw new InvalidOperationException("Database request " + callName + " can not be finished! Another request is currently active on the database: " + CurrentUpdateMethod);
            //}

            if (TaskStatus.ContainsKey(token))
            {
                TaskStatus.Remove(token);
                SetDatabaseStatus();
            }
            else
            {
                throw new InvalidOperationException("Database request " + callName + " can not be finished! No active request on the database found.");
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
