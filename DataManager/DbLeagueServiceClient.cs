using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.ServiceModel;

using iRLeagueManager.LeagueDBServiceRef;
using iRLeagueManager.Models.Database;
using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;
using System.Linq.Expressions;
using System.ServiceModel.Configuration;
using System.ComponentModel;
using System.CodeDom;
using System.Runtime.InteropServices;

namespace iRLeagueManager.Data
{
    public sealed class DbLeagueServiceClient : DbServiceClientBase, IDisposable
    {
        private string EndpointConfigurationName { get; } = "";

        private ILeagueDbServiceClient dbClient;
        private bool disposedValue;
        
        private ILeagueDbServiceClient DbClient
        {
            get => dbClient;
            set
            {
                if (dbClient != value)
                {
                    if (dbClient != null)
                        dbClient.PropertyChanged -= ClientPropertyChange;
                    dbClient = value;
                    if (dbClient != null)
                        dbClient.PropertyChanged += ClientPropertyChange;
                }
            }
        }

        private string DatabaseName { get; set; }

        private void ClientPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DbClient.ConnectionStatus))
            {
                UpdateConectionStatus();
            }
        }

        public void SetDbClient(ILeagueDbServiceClient client)
        {
            DbClient = client;
        }

        private ILeagueDbServiceClient GetDbClient()
        {
            return DbClient;
            //return new WCFLeagueDbClientWrapper();
            //return new ASPRestAPIClientWrapper(new Uri("https://localhost:44369/api/Home"));
            //return new ASPRestAPIClientWrapper(new Uri("http://144.91.113.195/iRLeagueRESTService/api/Home"));
        }

        public DbLeagueServiceClient() : base()
        {
            DbClient = new WCFLeagueDbClientWrapper();
            username = "TestUser";
            password = "12345678";
        }

        public DbLeagueServiceClient(ILeagueDbServiceClient client)
        {
            DbClient = client;
        }

        public DbLeagueServiceClient(IDatabaseStatus status) : base(status)
        {
            DbClient = new WCFLeagueDbClientWrapper();
            username = "TestUser";
            password = "12345678";
            //Status = new DatabaseStatusModel();
        }

        public DbLeagueServiceClient(IDatabaseStatus status, string endpointConfigurationName) : this(status)
        {
            EndpointConfigurationName = endpointConfigurationName;
        }

        public DbLeagueServiceClient(ILeagueDbServiceClient client, IDatabaseStatus status) : this(status)
        {
            DbClient = client;
        }

        public DbLeagueServiceClient(ILeagueDbServiceClient client, IDatabaseStatus status, string endpointConfigurationName) : this(status, endpointConfigurationName)
        {
            DbClient = client;
        }

        ~ DbLeagueServiceClient()
        {
            Dispose(false);
        }

        public async Task<AuthenticationResult> AuthenticateUserAsync(string userName, byte[] password)
        {
            if (DbClient != null)
                return await DbClient.AuthenticateUserAsync(userName, password, DatabaseName);
            return new AuthenticationResult() { IsAuthenticated = false, AuthenticatedUser = null, Status = "No Client available" };
        }

        public void UpdateConectionStatus()
        {
            if (DbClient != null)
                SetConnectionStatus(Token, DbClient.ConnectionStatus);
        }

        public bool OpenConnection()
        {
            if (DbClient != null)
                return DbClient.OpenConnection();
            return false;
        }

        public bool CloseConnection()
        {
            if (DbClient != null)
                return DbClient.CloseConnection();
            return false;
        }

        protected override void SetDatabaseStatus(IToken token, DatabaseStatusEnum status, string endpointAddress = "")
        {
            base.SetDatabaseStatus(token, status, DbClient.ServiceAddress);
        }

        public async Task ClientCallAsync(Func<Task> func, UpdateKind updateKind, [CallerMemberName] string callName = "")
        {
            //await ClientGetAsync<object, object>(null, x => { func(); return null; }, updateKind, callName: callName);
            await ClientCallAsync<object>(null, x => func(), updateKind, callName);
        }

        public async Task ClientCallAsync<TKey>(TKey key, Func<TKey, Task> func, UpdateKind updateKind, [CallerMemberName] string callName = "")
        {
            UpdateConectionStatus();
            if (DbClient.ConnectionStatus != ConnectionStatusEnum.Connected)
                return;

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

        public async Task<TResult> ClientGetAsync<TResult>(Func<Task<TResult>> getFunc, UpdateKind updateKind, [CallerMemberName] string callName = "") where TResult : class
        {
            return await ClientGetAsync(getFunc, updateKind, null, callName);
        }

        public async Task<TResult> ClientGetAsync<TResult>(Func<Task<TResult>> getFunc, UpdateKind updateKind, TResult defaultValue, [CallerMemberName] string callName = "")
        {
            return await ClientGetAsync<object, TResult>(null, x => getFunc(), UpdateKind.Loading, defaultValue, callName);
        }

        public async Task<TResult> ClientGetAsync<TKey, TResult>(TKey key, Func<TKey, Task<TResult>> getFunc, UpdateKind updateKind, [CallerMemberName] string callName = "") where TResult : class
        {
            return await ClientGetAsync(key, getFunc, updateKind, null, callName);
        }

        public async Task<TResult> ClientGetAsync<TKey, TResult>(TKey key, Func<TKey, Task<TResult>> getFunc, UpdateKind updateKind, TResult defaultValue, [CallerMemberName] string callName = "")
        {
            UpdateConectionStatus();
            if (DbClient.ConnectionStatus != ConnectionStatusEnum.Connected)
                return defaultValue;

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
                UpdateConectionStatus();
                if (IsUpdateRunning(callName))
                    EndUpdate(token, callName);
            }
            return retVar;
        }

        public void CleanUpSessions()
        {
            ((ILeagueDBService)DbClient).CleanUpSessions();
        }

        public async Task CleanUpSessionsAsync()
        {
            await ClientCallAsync(() => ((ILeagueDBService)DbClient).CleanUpSessionsAsync(), UpdateKind.Saving);
        }

        #region POST
        //public TTarget Post<TTarget>(TTarget item) where TTarget : MappableDTO
        //{
        //    return Post(new TTarget[] { item }).FirstOrDefault();
        //}

        //public TTarget[] Post<TTarget>(TTarget[] items) where TTarget : MappableDTO
        //{
        //    using (var client = GetDbClient())
        //    {
        //        return client.Post<TTarget>(items, username, password, DatabaseName);
        //    }
        //}

        public async Task<TTarget> PostAsync<TTarget>(TTarget item) where TTarget : MappableDTO
        {
            return (await PostAsync(new TTarget[] { item })).FirstOrDefault();
        }

        public async Task<MappableDTO> PostAsync(MappableDTO item, Type type)
        {
            return (await PostAsync(new MappableDTO[] { item }, type)).FirstOrDefault();
        }

        public async Task<TTarget[]> PostAsync<TTarget>(TTarget[] items) where TTarget : MappableDTO
        {
            return (await PostAsync(items, typeof(TTarget))).Cast<TTarget>().ToArray();
        }

        public async Task<MappableDTO[]> PostAsync(MappableDTO[] items, Type type)
        { 
            using (var client = GetDbClient())
            {
                return await ClientGetAsync(async () => (await client.PostAsync(items, type, DatabaseName)), UpdateKind.Saving);
            }
        }
        #endregion

        #region GET
        //public TTarget Get<TTarget>(long[] requestId) where TTarget : MappableDTO
        //{
        //    return Get<TTarget>(new long[][] { requestId }).FirstOrDefault();
        //}

        //public TTarget[] Get<TTarget>(long[][] requestIds = null) where TTarget : MappableDTO
        //{
        //    return DbClient.Get<TTarget>(requestIds, username, password, DatabaseName);
        //}

        public async Task<TTarget> GetAsync<TTarget>(long[] requestId) where TTarget : MappableDTO
        {
            return (await GetAsync<TTarget>(new long[][] { requestId })).FirstOrDefault();
        }

        public async Task<MappableDTO> GetAsync(long[] requestId, Type type)
        {
            return (await GetAsync(new long[][] { requestId }, type)).FirstOrDefault();
        }

        public async Task<TTarget[]> GetAsync<TTarget>(long[][] requestIds = null) where TTarget : MappableDTO
        {
            return (await GetAsync(requestIds, typeof(TTarget))).Cast<TTarget>().ToArray();
        }

        public async Task<MappableDTO[]> GetAsync(long[][] requestIds, Type type)
        {
            return await ClientGetAsync(async () => 
                (await DbClient.GetAsync(requestIds, type, DatabaseName)), UpdateKind.Loading);
        }
        #endregion

        #region PUT
        //public TTarget Put<TTarget>(TTarget item) where TTarget : MappableDTO
        //{
        //    return Put(new TTarget[] { item }).FirstOrDefault();
        //}

        //public TTarget[] Put<TTarget>(TTarget[] items) where TTarget : MappableDTO
        //{
        //    return DbClient.Put(items, username, password, DatabaseName);
        //}

        public async Task<TTarget> PutAsync<TTarget>(TTarget item) where TTarget : MappableDTO
        {
            return (await PutAsync(new TTarget[] { item })).FirstOrDefault();
        }

        public async Task<MappableDTO> PutAsync(MappableDTO item, Type type)
        {
            return (await PutAsync(new MappableDTO[] { item }, type)).FirstOrDefault();
        }

        public async Task<TTarget[]> PutAsync<TTarget>(TTarget[] items) where TTarget : MappableDTO
        {
            return (await PutAsync(items, typeof(TTarget))).Cast<TTarget>().ToArray();
        }

        public async Task<MappableDTO[]> PutAsync(MappableDTO[] items, Type type)
        {
            return await ClientGetAsync(async () => (await DbClient.PutAsync(items, type, DatabaseName)), UpdateKind.Saving);
        }
        #endregion

        #region DEL
        //public bool Del<TTarget>(long[] requestId) where TTarget : MappableDTO
        //{
        //    return Del<TTarget>(new long[][] { requestId });
        //}

        //public bool Del<TTarget>(long[][] requestIds) where TTarget : MappableDTO
        //{
        //    return DbClient.Del<TTarget>(requestIds, username, password, DatabaseName);
        //}

        public async Task<bool> DelAsync<TTarget>(long[] requestId) where TTarget : MappableDTO
        {
            return await DelAsync<TTarget>(new long[][] { requestId });
        }

        public async Task<bool> DelAsync(long[] requestId, Type type)
        {
            return await DelAsync(new long[][] { requestId }, type);
        }

        public async Task<bool> DelAsync<TTarget>(long[][] requestIds) where TTarget : MappableDTO
        {
            return await DelAsync(requestIds, typeof(TTarget));
        }

        public async Task<bool> DelAsync(long[][] requestIds, Type type)
        {
            return (await ClientGetAsync(async () => (await DbClient.DelAsync(requestIds, type, DatabaseName)), UpdateKind.Saving, false));
        }
        #endregion

        public void SetDatabaseName(string databaseName)
        {
            DatabaseName = databaseName;
        }

        public async Task SetDatabaseNameAsync(string databaseName)
        {
            await ClientCallAsync(() => new Task(() => { DatabaseName = databaseName; }), UpdateKind.Saving);
        }

        public void CalculateScoredResults(long sessionId)
        {
            using (var DbClient = GetDbClient())
            {
                throw new NotImplementedException();
                //((ILeagueDBService)DbClient).CalculateScoredResults(sessionId);
            }
        }

        public async Task CalculateScoredResultsAsync(long sessionId)
        {
            using (var DbClient = GetDbClient())
            {
                await ClientCallAsync(sessionId, x => ((ILeagueDBService)DbClient).CalculateScoredResultsAsync(x), UpdateKind.Saving);
            }
        }

        private void SetMessageHeader(RequestMessage message)
        {
            message.userName = username;
            message.password = password;
        }

        public ResponseMessage MessageTest(RequestMessage request)
        {
            return ((ILeagueDBService)DbClient).MessageTest(request);
        }

        public async Task<ResponseMessage> MessageTestAsync(RequestMessage request)
        {
            return await ((ILeagueDBService)DbClient).MessageTestAsync(request);
        }

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
                }

                // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
                // TODO: Große Felder auf NULL setzen
                if (DbClient != null)
                {
                    DbClient.Dispose();
                    DbClient = null;
                }
                disposedValue = true;
            }
        }

        // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
        // ~DbLeagueServiceClient()
        // {
        //     // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
