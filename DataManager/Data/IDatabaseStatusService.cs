using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.Data
{
    public interface IDatabaseStatusService
    {
        DatabaseStatusEnum DatabaseStatus { get; }
        void AddStatusItem(IDatabaseStatus statusItem);

        void RemoveStatusItem(IDatabaseStatus statusItem);

        Task StartRequestWhenReady(Func<Task> func, UpdateKind updateKind, [CallerMemberName] string callName = "");

        Task StartRequestWhenReady<TKey>(TKey key, Func<TKey, Task> func, UpdateKind updateKind, [CallerMemberName] string callName = "");

        Task<TResult> StartRequestWhenReady<TResult>(Func<Task<TResult>> getFunc, UpdateKind updateKind, [CallerMemberName] string callName = "") where TResult : class;

        Task<TResult> StartRequestWhenReady<TResult>(Func<Task<TResult>> getFunc, UpdateKind updateKind, TResult defaultValue, [CallerMemberName] string callName = "");

        Task<TResult> StartRequestWhenReady<TKey, TResult>(TKey key, Func<TKey, Task<TResult>> getFunc, UpdateKind updateKind, [CallerMemberName] string callName = "") where TResult : class;

        Task<TResult> StartRequestWhenReady<TKey, TResult>(TKey key, Func<TKey, Task<TResult>> getFunc, UpdateKind updateKind, TResult defaultValue, [CallerMemberName] string callName = "");

        void EndRequest(Guid token, string callName = "");
    }
}
