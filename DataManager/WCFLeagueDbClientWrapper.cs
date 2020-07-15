using iRLeagueManager.Data;
using iRLeagueManager.Enums;
using iRLeagueManager.LeagueDBServiceRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
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

        private ConnectionStatusEnum connectionStatus = ConnectionStatusEnum.Disconnected;
        public ConnectionStatusEnum ConnectionStatus { get => connectionStatus; protected set { connectionStatus = value; OnPropertyChanged(); } }

        public string ServiceAddress => client?.Endpoint.Address.Uri.AbsoluteUri;

        public WCFLeagueDbClientWrapper()
        {
            client = null;
        }

        public WCFLeagueDbClientWrapper(string endpointConfigurationName)
        {
            EndpointConfigurationName = endpointConfigurationName;
            client = null;
        }

        ~WCFLeagueDbClientWrapper()
        {
            Dispose();
        }

        public bool OpenConnection()
        {
            //if (EndpointConfigurationName == "")
            //    client = new LeagueDBServiceClient();
            //else
            //    client = new LeagueDBServiceClient(EndpointConfigurationName);

            //try
            //{
            //    client.Open();
            //}
            //catch
            //{
            //    return false;
            //}
            ConnectionStatus = ConnectionStatusEnum.Connected;
            //OnPropertyChanged(nameof(ConnectionStatus));
            return true;
        }

        public bool CloseConnection()
        {
            //try
            //{
            //    client.Close();
            //    ((IDisposable)client).Dispose();
            //}
            //catch
            //{
            //    return false;
            //}
            //client = null;
            //OnPropertyChanged(nameof(ConnectionStatus));
            ConnectionStatus = ConnectionStatusEnum.Disconnected;
            return true;
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

        //public TTarget[] Get<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO
        //{
        //    return Get(requestIds, typeof(TTarget), userName, password, databaseName).Cast<TTarget>().ToArray();
        //}

        //public MappableDTO[] Get(long[][] requestIds, Type requestType, string userName, string password, string databaseName)
        //{
        //    OnPropertyChanged(nameof(ConnectionStatus));
        //    GETItemsRequestMessage requestMessage = new GETItemsRequestMessage
        //    {
        //        databaseName = databaseName,
        //        userName = userName,
        //        password = password,
        //        requestItemType = requestType.Name,
        //        requestResponse = true,
        //        requestItemIds = requestIds
        //    };

        //    var result = DatabaseGET(requestMessage).items;
        //    OnPropertyChanged(nameof(ConnectionStatus));
        //    return result;
        //}

        public async Task<TTarget[]> GetAsync<TTarget>(long[][] requestIds, string databaseName) where TTarget : MappableDTO
        {
            return (await GetAsync(requestIds, typeof(TTarget), databaseName)).Cast<TTarget>().ToArray();
        }

        public async Task<MappableDTO[]> GetAsync(long[][] requestIds, Type requestType, string databaseName)
        { 
            OnPropertyChanged(nameof(ConnectionStatus));
            GETItemsRequestMessage requestMessage = new GETItemsRequestMessage
            {
                databaseName = databaseName,
                userName = "",
                password = "",
                requestItemType = requestType.Name,
                requestResponse = true,
                requestItemIds = requestIds
            };

            var result = (await DatabaseGETAsync(requestMessage)).items;
            OnPropertyChanged(nameof(ConnectionStatus));
            return result;
        }

        //public TTarget[] Post<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO
        //{
        //    return Post(items, typeof(TTarget), userName, password, databaseName).Cast<TTarget>().ToArray();
        //}

        //public MappableDTO[] Post(MappableDTO[] items, Type requestType, string userName, string password, string databaseName)
        //{
        //    OnPropertyChanged(nameof(ConnectionStatus));
        //    POSTItemsRequestMessage requestMessage = new POSTItemsRequestMessage
        //    {
        //        databaseName = databaseName,
        //        userName = userName,
        //        password = password,
        //        requestItemType = requestType.Name,
        //        requestResponse = true,
        //        items = items,
        //    };

        //    var result = DatabasePOST(requestMessage).items;
        //    OnPropertyChanged(nameof(ConnectionStatus));
        //    return result;
        //}

        public async Task<TTarget[]> PostAsync<TTarget>(TTarget[] items, string databaseName) where TTarget : MappableDTO
        {
            return (await PostAsync(items, typeof(TTarget), databaseName)).Cast<TTarget>().ToArray();
        }

        public async Task<MappableDTO[]> PostAsync(MappableDTO[] items, Type requestType, string databaseName)
        {   
            OnPropertyChanged(nameof(ConnectionStatus));
            POSTItemsRequestMessage requestMessage = new POSTItemsRequestMessage
            {
                databaseName = databaseName,
                userName = "",
                password = "",
                requestItemType = requestType.Name,
                requestResponse = true,
                items = items,
            };

            var result = (await DatabasePOSTAsync(requestMessage)).items;
            OnPropertyChanged(nameof(ConnectionStatus));
            return result;
        }

        //public TTarget[] Put<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO
        //{
        //    OnPropertyChanged(nameof(ConnectionStatus));
        //    PUTItemsRequestMessage requestMessage = new PUTItemsRequestMessage
        //    {
        //        databaseName = databaseName,
        //        userName = userName,
        //        password = password,
        //        requestItemType = typeof(TTarget).Name,
        //        requestResponse = true,
        //        items = items,
        //    };

        //    var result = DatabasePUT(requestMessage).items.Cast<TTarget>().ToArray();
        //    OnPropertyChanged(nameof(ConnectionStatus));
        //    return result;
        //}

        public async Task<TTarget[]> PutAsync<TTarget>(TTarget[] items, string databaseName) where TTarget : MappableDTO
        {
            return (await PutAsync(items, typeof(TTarget), databaseName)).Cast<TTarget>().ToArray();
        }

        public async Task<MappableDTO[]> PutAsync(MappableDTO[] items, Type requestType,  string databaseName)
        {
            OnPropertyChanged(nameof(ConnectionStatus));
            PUTItemsRequestMessage requestMessage = new PUTItemsRequestMessage
            {
                databaseName = databaseName,
                userName = "",
                password = "",
                requestItemType = requestType.Name,
                requestResponse = true,
                items = items,
            };

            var result = (await DatabasePUTAsync(requestMessage)).items;
            OnPropertyChanged(nameof(ConnectionStatus));
            return result;
        }

        //public bool Del<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO
        //{
        //    OnPropertyChanged(nameof(ConnectionStatus));
        //    DELItemsRequestMessage requestMessage = new DELItemsRequestMessage
        //    {
        //        databaseName = databaseName,
        //        userName = userName,
        //        password = password,
        //        requestItemType = typeof(TTarget).Name,
        //        requestResponse = true,
        //        requestItemIds = requestIds
        //    };

        //    var result = DatabaseDEL(requestMessage).success;
        //    OnPropertyChanged(nameof(ConnectionStatus));
        //    return result;
        //}

        public async Task<bool> DelAsync<TTarget>(long[][] requestIds, string databaseName) where TTarget : MappableDTO
        {
            return await DelAsync(requestIds, typeof(TTarget), databaseName);
        }

        public async Task<bool> DelAsync(long[][] requestIds, Type requestType, string databaseName)
        {
            OnPropertyChanged(nameof(ConnectionStatus));
            DELItemsRequestMessage requestMessage = new DELItemsRequestMessage
            {
                databaseName = databaseName,
                userName = "",
                password = "",
                requestItemType = requestType.Name,
                requestResponse = true,
                requestItemIds = requestIds
            };

            var result = (await DatabaseDELAsync(requestMessage)).success;
            OnPropertyChanged(nameof(ConnectionStatus));
            return result;
        }

        public void CalculateScoredResults(long sessionId)
        {
            using (var client = new LeagueDBServiceClient())
            {
                ((ILeagueDBService)client).CalculateScoredResults(sessionId);
            }
        }

        public async Task CalculateScoredResultsAsync(long sessionId)
        {
            using (var client = new LeagueDBServiceClient())
            {
                await ((ILeagueDBService)client).CalculateScoredResultsAsync(sessionId);
            }
        }

        public void CleanUpSessions()
        {
            using (var client = new LeagueDBServiceClient())
            {
                ((ILeagueDBService)client).CleanUpSessions();
            }
        }

        public async Task CleanUpSessionsAsync()
        {
            using (var client = new LeagueDBServiceClient())
            {
                await ((ILeagueDBService)client).CleanUpSessionsAsync();
            }
        }

        public DELItemsResponseMessage DatabaseDEL(DELItemsRequestMessage request)
        {
            using (var client = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)client).DatabaseDEL(request);
            }
        }

        public async Task<DELItemsResponseMessage> DatabaseDELAsync(DELItemsRequestMessage request)
        {
            using (var client = new LeagueDBServiceClient())
            {
                return await ((ILeagueDBService)client).DatabaseDELAsync(request);
            }
        }

        public GETItemsResponseMessage DatabaseGET(GETItemsRequestMessage request)
        {
            using (var client = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)client).DatabaseGET(request);
            }
        }

        public async Task<GETItemsResponseMessage> DatabaseGETAsync(GETItemsRequestMessage request)
        {
            using (var client = new LeagueDBServiceClient())
            {
                client.Open();
                return await ((ILeagueDBService)client).DatabaseGETAsync(request);
            }
        }

        public POSTItemsResponseMessage DatabasePOST(POSTItemsRequestMessage request)
        {
            using (var client = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)client).DatabasePOST(request);
            }
        }

        public async Task<POSTItemsResponseMessage> DatabasePOSTAsync(POSTItemsRequestMessage request)
        {
            using (var client = new LeagueDBServiceClient())
            {
                return await ((ILeagueDBService)client).DatabasePOSTAsync(request);
            }
        }

        public PUTItemsResponseMessage DatabasePUT(PUTItemsRequestMessage request)
        {
            using (var client = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)client).DatabasePUT(request);
            }
        }

        public async Task<PUTItemsResponseMessage> DatabasePUTAsync(PUTItemsRequestMessage request)
        {
            using (var client = new LeagueDBServiceClient())
            {
                client.Open();
                return await ((ILeagueDBService)client).DatabasePUTAsync(request);
            }
        }

        public void Dispose()
        {
        }

        public ResponseMessage MessageTest(RequestMessage request)
        {
            using (var client = new LeagueDBServiceClient())
            {
                return ((ILeagueDBService)client).MessageTest(request);
            }
        }

        public async Task<ResponseMessage> MessageTestAsync(RequestMessage request)
        {
            using (var client = new LeagueDBServiceClient())
            {
                return await ((ILeagueDBService)client).MessageTestAsync(request);
            }
        }

        public void SetDatabaseName(string databaseName)
        {
            using (var client = new LeagueDBServiceClient())
            {
                ((ILeagueDBService)client).SetDatabaseName(databaseName);
            }
        }

        public async Task SetDatabaseNameAsync(string databaseName)
        {
            using (var client = new LeagueDBServiceClient())
            {
                await ((ILeagueDBService)client).SetDatabaseNameAsync(databaseName);
            }
        }

        //public string Test(string name)
        //{
        //    return ((ILeagueDBService)client).Test(name);
        //}

        //public Task<string> TestAsync(string name)
        //{
        //    return ((ILeagueDBService)client).TestAsync(name);
        //}

        //public string TestDB()
        //{
        //    return ((ILeagueDBService)client).TestDB();
        //}

        //public Task<string> TestDBAsync()
        //{
        //    return ((ILeagueDBService)client).TestDBAsync();
        //}

        //public AuthenticationResult AuthenticateUser(string userName, byte[] password, string databaseName)
        //{
        //    using (var client = new LeagueDBServiceClient())
        //    {
        //        return ((ILeagueDBService)client).AuthenticateUser(userName, password, databaseName);
        //    }
        //}

        //public async Task<AuthenticationResult> AuthenticateUserAsync(string userName, byte[] password, string databaseName)
        //{
        //    using (var client = new LeagueDBServiceClient())
        //    {
        //        return await ((ILeagueDBService)client).AuthenticateUserAsync(userName, password, databaseName);
        //    }
        //}

        public LeagueUserDTO RegisterUser(string userName, byte[] password, string databaseName)
        {
            using (var client = new LeagueDBServiceClient())
            {
                throw new NotImplementedException();
            }
        }

        public Task<LeagueUserDTO> RegisterUserAsync(string userName, byte[] password, string databaseName)
        {
            using (var client = new LeagueDBServiceClient())
            {
                throw new NotImplementedException();
            }
        }
    }
}
