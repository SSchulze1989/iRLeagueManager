using iRLeagueManager.Data;
using iRLeagueManager.Enums;
using iRLeagueManager.LeagueDBServiceRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Data
{
    public class WCFLeagueDbClientWrapper : NotifyPropertyChangedBase, ILeagueDbServiceClient, ILeagueDBService, IDisposable
    {
        private LeagueDBServiceClient client;

        private string EndpointConfigurationName { get; } = "";

        public CommunicationState State => (client != null) ? client.State : CommunicationState.Closed;

        public ConnectionStatusEnum ConnectionStatus => (client != null) ?  GetConnectionStatus(client.State) : ConnectionStatusEnum.Disconnected;

        public string ServiceAddress => client?.Endpoint.Address.Uri.AbsoluteUri;

        public WCFLeagueDbClientWrapper()
        {
            client = null;
        }

        public WCFLeagueDbClientWrapper(string endpointConfigurationName)
        {
            client = null;
        }

        ~WCFLeagueDbClientWrapper()
        {
            Dispose();
        }

        public bool OpenConnection()
        {
            if (EndpointConfigurationName == "")
                client = new LeagueDBServiceClient();
            else
                client = new LeagueDBServiceClient(EndpointConfigurationName);

            try
            {
                client.Open();
            }
            catch
            {
                return false;
            }
            OnPropertyChanged(nameof(ConnectionStatus));
            return true;
        }

        public bool CloseConnection()
        {
            try
            {
                client.Close();
                ((IDisposable)client).Dispose();
            }
            catch
            {
                return false;
            }
            client = null;
            OnPropertyChanged(nameof(ConnectionStatus));
            return true;
        }

        public ICustomAuthenticationResult Authenticate(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public IClientUser Register(string userName, string password)
        {
            throw new NotImplementedException();
        }

        private ConnectionStatusEnum GetConnectionStatus(CommunicationState communicationState)
        {
            switch (communicationState)
            {
                case CommunicationState.Created:
                    return ConnectionStatusEnum.Disconnected;
                case CommunicationState.Opening:
                    return ConnectionStatusEnum.Connecting;
                case CommunicationState.Opened:
                    return ConnectionStatusEnum.Connected;
                case CommunicationState.Closing:
                    return ConnectionStatusEnum.Connected;
                case CommunicationState.Closed:
                    return ConnectionStatusEnum.Disconnected;
                case CommunicationState.Faulted:
                    return ConnectionStatusEnum.ConnectionError;
                default:
                    return ConnectionStatusEnum.NoConnection;
            }
        }

        public TTarget[] Get<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO
        {
            OnPropertyChanged(nameof(ConnectionStatus));
            GETItemsRequestMessage requestMessage = new GETItemsRequestMessage
            {
                databaseName = databaseName,
                userName = userName,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                requestItemIds = requestIds
            };

            var result = DatabaseGET(requestMessage).items.Cast<TTarget>().ToArray();
            OnPropertyChanged(nameof(ConnectionStatus));
            return result;
        }

        public async Task<TTarget[]> GetAsync<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO
        {
            OnPropertyChanged(nameof(ConnectionStatus));
            GETItemsRequestMessage requestMessage = new GETItemsRequestMessage
            {
                databaseName = databaseName,
                userName = userName,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                requestItemIds = requestIds
            };

            var result = (await DatabaseGETAsync(requestMessage)).items.Cast<TTarget>().ToArray();
            OnPropertyChanged(nameof(ConnectionStatus));
            return result;
        }

        public TTarget[] Post<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO
        {
            POSTItemsRequestMessage requestMessage = new POSTItemsRequestMessage
            {
                databaseName = databaseName,
                userName = userName,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                items = items,
            };

            return DatabasePOST(requestMessage).items.Cast<TTarget>().ToArray();
        }

        public async Task<TTarget[]> PostAsync<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO
        {
            POSTItemsRequestMessage requestMessage = new POSTItemsRequestMessage
            {
                databaseName = databaseName,
                userName = userName,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                items = items,
            };

            return (await DatabasePOSTAsync(requestMessage)).items.Cast<TTarget>().ToArray();
        }

        public TTarget[] Put<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO
        {
            PUTItemsRequestMessage requestMessage = new PUTItemsRequestMessage
            {
                databaseName = databaseName,
                userName = userName,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                items = items,
            };

            return DatabasePUT(requestMessage).items.Cast<TTarget>().ToArray();
        }

        public async Task<TTarget[]> PutAsync<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO
        {
            PUTItemsRequestMessage requestMessage = new PUTItemsRequestMessage
            {
                databaseName = databaseName,
                userName = userName,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                items = items,
            };

            return (await DatabasePUTAsync(requestMessage)).items.Cast<TTarget>().ToArray();
        }
        public bool Del<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO
        {
            DELItemsRequestMessage requestMessage = new DELItemsRequestMessage
            {
                databaseName = databaseName,
                userName = userName,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                requestItemIds = requestIds
            };

            return DatabaseDEL(requestMessage).success;
        }

        public async Task<bool> DelAsync<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO
        {
            DELItemsRequestMessage requestMessage = new DELItemsRequestMessage
            {
                databaseName = databaseName,
                userName = userName,
                password = password,
                requestItemType = typeof(TTarget).Name,
                requestResponse = true,
                requestItemIds = requestIds
            };

            return (await DatabaseDELAsync(requestMessage)).success;
        }

        public void CalculateScoredResults(long sessionId)
        {
            ((ILeagueDBService)client).CalculateScoredResults(sessionId);
        }

        public Task CalculateScoredResultsAsync(long sessionId)
        {
            return ((ILeagueDBService)client).CalculateScoredResultsAsync(sessionId);
        }

        public void CleanUpSessions()
        {
            ((ILeagueDBService)client).CleanUpSessions();
        }

        public Task CleanUpSessionsAsync()
        {
            return ((ILeagueDBService)client).CleanUpSessionsAsync();
        }

        public DELItemsResponseMessage DatabaseDEL(DELItemsRequestMessage request)
        {
            return ((ILeagueDBService)client).DatabaseDEL(request);
        }

        public Task<DELItemsResponseMessage> DatabaseDELAsync(DELItemsRequestMessage request)
        {
            return ((ILeagueDBService)client).DatabaseDELAsync(request);
        }

        public GETItemsResponseMessage DatabaseGET(GETItemsRequestMessage request)
        {
            return ((ILeagueDBService)client).DatabaseGET(request);
        }

        public Task<GETItemsResponseMessage> DatabaseGETAsync(GETItemsRequestMessage request)
        {
            return ((ILeagueDBService)client).DatabaseGETAsync(request);
        }

        public POSTItemsResponseMessage DatabasePOST(POSTItemsRequestMessage request)
        {
            return ((ILeagueDBService)client).DatabasePOST(request);
        }

        public Task<POSTItemsResponseMessage> DatabasePOSTAsync(POSTItemsRequestMessage request)
        {
            return ((ILeagueDBService)client).DatabasePOSTAsync(request);
        }

        public PUTItemsResponseMessage DatabasePUT(PUTItemsRequestMessage request)
        {
            return ((ILeagueDBService)client).DatabasePUT(request);
        }

        public Task<PUTItemsResponseMessage> DatabasePUTAsync(PUTItemsRequestMessage request)
        {
            return ((ILeagueDBService)client).DatabasePUTAsync(request);
        }

        public void Dispose()
        {
            ((IDisposable)client).Dispose();
        }

        public ResponseMessage MessageTest(RequestMessage request)
        {
            return ((ILeagueDBService)client).MessageTest(request);
        }

        public Task<ResponseMessage> MessageTestAsync(RequestMessage request)
        {
            return ((ILeagueDBService)client).MessageTestAsync(request);
        }

        public void SetDatabaseName(string databaseName)
        {
            ((ILeagueDBService)client).SetDatabaseName(databaseName);
        }

        public Task SetDatabaseNameAsync(string databaseName)
        {
            return ((ILeagueDBService)client).SetDatabaseNameAsync(databaseName);
        }

        public string Test(string name)
        {
            return ((ILeagueDBService)client).Test(name);
        }

        public Task<string> TestAsync(string name)
        {
            return ((ILeagueDBService)client).TestAsync(name);
        }

        public string TestDB()
        {
            return ((ILeagueDBService)client).TestDB();
        }

        public Task<string> TestDBAsync()
        {
            return ((ILeagueDBService)client).TestDBAsync();
        }
    }
}
