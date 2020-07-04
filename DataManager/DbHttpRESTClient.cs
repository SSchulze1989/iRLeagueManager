using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.CompilerServices;

using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;

namespace iRLeagueManager.Data
{
    public class DbHttpRESTClient : DbServiceClientBase
    {

        private string EndpointConfigurationName { get; } = "";

        private string DatabaseName { get; set; }

        public DbHttpRESTClient() : base()
        {
            username = "TestUser";
            password = "12345678";
        }

        public DbHttpRESTClient(IDatabaseStatus status) : base(status)
        {
            username = "TestUser";
            password = "12345678";
            //Status = new DatabaseStatusModel();
        }

        public DbHttpRESTClient(IDatabaseStatus status, string endpointConfigurationName) : this(status)
        {
            EndpointConfigurationName = endpointConfigurationName;
        }

        protected override void SetDatabaseStatus(IToken token, DatabaseStatusEnum status, string endpointAddress = "")
        {
            using (var DbClient = new RESTHttpClient())
            {
                base.SetDatabaseStatus(token, status, DbClient.BaseAddress.AbsoluteUri);
            }
        }

        public async Task ClientCallAsync(Func<Task> func, UpdateKind updateKind, [CallerMemberName] string callName = "")
        {
            //await ClientGetAsync<object, object>(null, x => { func(); return null; }, updateKind, callName: callName);
            await ClientCallAsync<object>(null, x => func(), updateKind, callName);
        }

        public async Task ClientCallAsync<TKey>(TKey key, Func<TKey, Task> func, UpdateKind updateKind, [CallerMemberName] string callName = "")
        {
            IToken token = new RequestToken();
            int timeOutMilliseconds = 10000;
            try
            {
                if (await StartUpdateWhenReady(updateKind, token, timeOutMilliseconds, callName))
                    await func(key);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (IsUpdateRunning(callName))
                    EndUpdate(token, callName);
            }
        }

        public async Task<TResult> ClientGetAsync<TResult>(Func<Task<TResult>> getFunc, UpdateKind updateKind, TResult defaultValue = null, [CallerMemberName] string callName = "") where TResult : class
        {
            return await ClientGetAsync<object, TResult>(null, x => getFunc(), UpdateKind.Loading, defaultValue, callName);
        }

        public async Task<TResult> ClientGetAsync<TKey, TResult>(TKey key, Func<TKey, Task<TResult>> getFunc, UpdateKind updateKind, TResult defaultValue = null, [CallerMemberName] string callName = "") where TResult : class
        {
            int timeOutMilliseconds = 10000;
            IToken token = new RequestToken();
            TResult retVar = defaultValue;
            try
            {
                if (await StartUpdateWhenReady(updateKind, token, timeOutMilliseconds, callName))
                    retVar = await getFunc(key);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (IsUpdateRunning(callName))
                    EndUpdate(token, callName);
            }
            return retVar;
        }
    }
}
