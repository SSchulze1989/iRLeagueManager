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

using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace iRLeagueManager.Data
{
    public class DatabaseStatusService : IDatabaseStatusService
    {
        protected List<IDatabaseStatus> StatusArray { get; }
        public Uri BaseUri { get; set; }
        protected Dictionary<Guid, UpdateKind> TaskStatus { get; } = new Dictionary<Guid, UpdateKind>();
        public DatabaseStatusEnum DatabaseStatus { get; private set; }
        private string CurrentUpdateMethod { get; set; }
        private bool IsBusy { get; set; }
        protected Guid Token { get; }

        public DatabaseStatusService()
        {
            Token = Guid.NewGuid();
            StatusArray = new List<IDatabaseStatus>();
        }

        public void AddStatusItem(IDatabaseStatus statusItem)
        {
            if (!StatusArray.Contains(statusItem))
                StatusArray.Add(statusItem);
        }

        public void EndRequest(Guid token, string callName = "")
        {
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

        public void RemoveStatusItem(IDatabaseStatus statusItem)
        {
            if (StatusArray.Contains(statusItem))
                StatusArray.Remove(statusItem);
        }

        protected virtual async Task<bool> StartUpdateWhenReady(UpdateKind updateKind, Guid token, [CallerMemberName] string callName = "")
        {
            if (callName != "")
            {
                CurrentUpdateMethod = callName;
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
            return await Task.FromResult(true);
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
            SetDatabaseStatus(Token, status, BaseUri.AbsoluteUri);
        }

        protected virtual void SetDatabaseStatus(Guid token, DatabaseStatusEnum status, string endpointAddress = "")
        {
            DatabaseStatus = status;
            foreach (var statusItem in StatusArray)
            {
                statusItem.SetDatabaseStatus(token, status, endpointAddress);
            }
        }

        protected virtual bool IsUpdateRunning([CallerMemberName] string callName = "")
        {
            if (IsBusy)
            {
                return true;
            }
            return false;
        }

        public async Task StartRequestWhenReady(Func<Task> func, UpdateKind updateKind, [CallerMemberName] string callName = "")
        {
            //await ClientGetAsync<object, object>(null, x => { func(); return null; }, updateKind, callName: callName);
            await StartRequestWhenReady<object>(null, x => func(), updateKind, callName);
        }

        public async Task StartRequestWhenReady<TKey>(TKey key, Func<TKey, Task> func, UpdateKind updateKind, [CallerMemberName] string callName = "")
        {
            Guid token = Guid.NewGuid();
            try
            {
                if (await StartUpdateWhenReady(updateKind, token, callName))
                    await func(key);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (IsUpdateRunning(callName))
                    EndRequest(token, callName);
            }
        }

        public async Task<TResult> StartRequestWhenReady<TResult>(Func<Task<TResult>> getFunc, UpdateKind updateKind, [CallerMemberName] string callName = "") where TResult : class
        {
            return await StartRequestWhenReady(getFunc, updateKind, null, callName);
        }

        public async Task<TResult> StartRequestWhenReady<TResult>(Func<Task<TResult>> getFunc, UpdateKind updateKind, TResult defaultValue, [CallerMemberName] string callName = "")
        {
            return await StartRequestWhenReady<object, TResult>(null, x => getFunc(), UpdateKind.Loading, defaultValue, callName);
        }

        public async Task<TResult> StartRequestWhenReady<TKey, TResult>(TKey key, Func<TKey, Task<TResult>> getFunc, UpdateKind updateKind, [CallerMemberName] string callName = "") where TResult : class
        {
            return await StartRequestWhenReady(key, getFunc, updateKind, null, callName);
        }

        public async Task<TResult> StartRequestWhenReady<TKey, TResult>(TKey key, Func<TKey, Task<TResult>> getFunc, UpdateKind updateKind, TResult defaultValue, [CallerMemberName] string callName = "")
        {
            Guid token = Guid.NewGuid();
            TResult retVar = defaultValue;
            try
            {
                if (await StartUpdateWhenReady(updateKind, token, callName))
                    retVar = await getFunc(key);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (IsUpdateRunning(callName))
                    EndRequest(token, callName);
            }
            return retVar;
        }
    }
}
