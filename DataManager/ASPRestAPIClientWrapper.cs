//using iRLeagueManager.Data;
//using iRLeagueManager.LeagueDBServiceRef;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//using iRLeagueManager.Enums;

////using iRLeagueDatabase.DataTransfer.Messages;

//namespace iRLeagueManager.Data
//{
//    public class ASPRestAPIClientWrapper : NotifyPropertyChangedBase, ILeagueDbServiceClient, IDisposable
//    {
//        private readonly Uri requestUri;
//        private bool disposedValue;

//        private readonly HttpClient client;

//        public ConnectionStatusEnum ConnectionStatus { get; }

//        public bool OpenConnection()
//        {
//            throw new NotImplementedException();
//        }
//        public IClientUser Register(string userName, string password) => throw new NotImplementedException();
//        public ICustomAuthenticationResult Authenticate(string userName, string password) => throw new NotImplementedException();
//        public bool CloseConnection() => throw new NotImplementedException();

//        public string ServiceAddress => requestUri.AbsoluteUri;

//        public ASPRestAPIClientWrapper(Uri uri)
//        {
//            requestUri = uri;
//            client = new HttpClient();
//        }

//        ~ASPRestAPIClientWrapper()
//        {
//            Dispose(false);
//        }


//        private Uri GetRequestUri(Uri controllerUri, long[][] requestIds, Type requestType, string databaseName)
//        {
//            return GetRequestUri(controllerUri, requestIds, requestType.Name, databaseName);
//        }
//        private Uri GetRequestUri(Uri controllerUri, long[][] requestIds, string requestType, string databaseName)
//        {
//            string absoluteUri = controllerUri.AbsoluteUri;
//            List<string> requestIdStrings = new List<string>();

//            string requestIdString = "";

//            if (requestIds != null)
//            {
//                foreach (var itemId in requestIds)
//                {
//                    requestIdStrings.Add(itemId.Select(x => x.ToString()).Aggregate((x, y) => x + "," + y));
//                }
//                requestIdString = requestIdStrings.Select((x, i) => "requestIds[" + i.ToString() + "]=" + x).Aggregate((x, y) => x + "&" + y);
//            }

//            var requestTypeString = "requestType=" + requestType;
//            var databaseNameString = "databaseName=" + databaseName;

//            return new Uri(absoluteUri + "?" + ((requestIdString != "") ? requestIdString + "&": "") + requestTypeString + "&" + databaseNameString);
//        }

//        protected virtual void Dispose(bool disposing)
//        {
//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
//                }

//                client.Dispose();

//                // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
//                // TODO: Große Felder auf NULL setzen
//                disposedValue = true;
//            }
//        }

//        // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
//        // ~ASPRestAPIClientWrapper()
//        // {
//        //     // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
//        //     Dispose(disposing: false);
//        // }

//        public void Dispose()
//        {
//            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in der Methode "Dispose(bool disposing)" ein.
//            Dispose(disposing: true);
//            GC.SuppressFinalize(this);
//        }

//        public TTarget[] Get<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<TTarget[]> GetAsync<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO
//        {
//            var requestString = GetRequestUri(requestUri, requestIds, typeof(TTarget), databaseName);
//            var request = new HttpRequestMessage(HttpMethod.Get, requestString);
//            request.Headers.Add(HttpRequestHeader.Accept.ToString(), "application/xml");

//            var result = await client.SendAsync(request);

//            if (result.IsSuccessStatusCode)
//            {
//                var test = await result.Content.ReadAsStringAsync();
//                var items = await result.Content.ReadAsAsync<MappableDTO[]>();

//                if (items != null)
//                {
//                    var retval = items.Select(x => x as TTarget).ToArray();
//                    return retval;
//                }
//            }

//            throw new WebException("Could not get entries");
//        }

//        public TTarget[] Post<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<TTarget[]> PostAsync<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO
//        {
//            var requestString = GetRequestUri(requestUri, null, typeof(TTarget), databaseName);
//            var result = await client.GetAsync(requestString);

//            if (result.IsSuccessStatusCode)
//            {
//                items = await result.Content.ReadAsAsync<TTarget[]>();

//                if (items != null)
//                    return items;
//            }

//            throw new WebException("Could not post entries");
//        }

//        public TTarget[] Put<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<TTarget[]> PutAsync<TTarget>(TTarget[] items, string userName, string password, string databaseName) where TTarget : MappableDTO
//        {
//            var requestString = GetRequestUri(requestUri, null, typeof(TTarget), databaseName);
//            var result = await client.GetAsync(requestString);

//            if (result.IsSuccessStatusCode)
//            {
//                items = await result.Content.ReadAsAsync<TTarget[]>();

//                if (items != null)
//                    return items;
//            }

//            throw new WebException("Could not post entries");
//        }

//        public bool Del<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<bool> DelAsync<TTarget>(long[][] requestIds, string userName, string password, string databaseName) where TTarget : MappableDTO
//        {
//            var requestString = GetRequestUri(requestUri, requestIds, typeof(TTarget), databaseName);
//            var result = await client.GetAsync(requestString);

//            if (result.IsSuccessStatusCode)
//            {
//                var items = await result.Content.ReadAsAsync<bool>();

//                if (items != null)
//                    return items;
//            }

//            throw new WebException("Could not post entries");
//        }
//    }
//}
