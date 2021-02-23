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

using iRLeagueManager.Data;
using iRLeagueDatabase.DataTransfer;
using iRLeagueDatabase.DataTransfer.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Enums;
using iRLeagueManager.Interfaces;
using System.Runtime.Serialization;
using iRLeagueManager.Models;

//using iRLeagueDatabase.DataTransfer.Messages;

namespace iRLeagueManager.Data
{
    public class ASPRestAPIClientWrapper : NotifyPropertyChangedBase, IModelDatabase, IModelDataAndActionProvider, IModelDataProvider, IActionProvider, ILeagueDataProvider, IDisposable
    {
        public Uri BaseUri { get; }
        private bool disposedValue;

        public IDatabaseStatusService DatabaseStatusService;

        public string LeagueName { get; set; }

        public string ModelController { get; set; } = "Model";
        public string ActionController { get; set; } = "Action";

        private ICredentials userCredentials;

        public ConnectionStatusEnum ConnectionStatus { get; }

        public bool OpenConnection()
        {
            throw new NotImplementedException();
        }
        public bool CloseConnection() => throw new NotImplementedException();

        public string ServiceAddress => BaseUri.AbsoluteUri;

        public DatabaseStatusEnum DatabaseStatus => (DatabaseStatusService?.DatabaseStatus).GetValueOrDefault();

        public ASPRestAPIClientWrapper(Uri baseUri, string databaseName)
        {
            this.BaseUri = baseUri;
            LeagueName = databaseName;
            userCredentials = new NetworkCredential("TestUser", "testuser");
        }

        public ASPRestAPIClientWrapper(Uri baseUri, string databaseName, ICredentials userCredentials) : this(baseUri, databaseName)
        {
            this.userCredentials = userCredentials;
        }

        ~ASPRestAPIClientWrapper()
        {
            Dispose(false);
        }

        private HttpClient CreateClient()
        {
            var handler = new HttpClientHandler() { Credentials = userCredentials };
            var client = new HttpClient(handler);
            client.DefaultRequestHeaders.Add(HttpRequestHeader.Accept.ToString(), "application/xml");
            client.Timeout = TimeSpan.FromSeconds(60);
            return client;
        }

        private Uri GetRequestUri(string controller, string action, long[][] requestIds, Type requestTypeType)
        {
            return GetRequestUri(controller, action, requestIds, requestTypeType.Name);
        }
        private Uri GetRequestUri(string controller, string action, long[][] requestIds, string requestType)
        {
            string absoluteUri = string.Format("{0}/{1}/{2}", BaseUri.AbsoluteUri, controller, action);
            List<string> requestIdStrings = new List<string>();

            string requestIdString = "";

            if (requestIds != null)
            {
                foreach (var itemId in requestIds)
                {
                    requestIdStrings.Add(itemId.Select(x => x.ToString()).Aggregate((x, y) => x + "," + y));
                }
                requestIdString = requestIdStrings.Select((x, i) => "requestIds[" + i.ToString() + "]=" + x).Aggregate((x, y) => x + "&" + y);
            }

            string requestTypeString = "";
            if (requestType != null)
                requestTypeString = "requestType=" + requestType;

            var databaseNameString = "leagueName=" + LeagueName;

            return new Uri(absoluteUri + "?" + 
                ((requestIdString != "") ? requestIdString + "&" : "") + 
                ((requestTypeString != "") ? requestTypeString + "&" : "") + 
                databaseNameString);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
                }

                // TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer überschreiben
                // TODO: Große Felder auf NULL setzen
                disposedValue = true;
            }
        }

        // // TODO: Finalizer nur überschreiben, wenn "Dispose(bool disposing)" Code für die Freigabe nicht verwalteter Ressourcen enthält
        // ~ASPRestAPIClientWrapper()
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

        public async Task<TTarget[]> GetAsync<TTarget>(long[][] requestIds) where TTarget : MappableDTO
        {
            var result = (await GetAsync(requestIds, typeof(TTarget))).Cast<TTarget>().ToArray();
            return result;
        }

        public async Task<TTarget[]> PostAsync<TTarget>(TTarget[] items) where TTarget : MappableDTO
        {
            return (await PostAsync(items, typeof(TTarget))).Cast<TTarget>().ToArray();
        }

        public async Task<TTarget[]> PutAsync<TTarget>(TTarget[] items) where TTarget : MappableDTO
        {
            var result = (await PutAsync(items, typeof(TTarget))).Cast<TTarget>().ToArray();
            return result;
        }

        public async Task<bool> DelAsync<TTarget>(long[][] requestIds) where TTarget : MappableDTO
        {
            return (await DeleteAsync(requestIds, typeof(TTarget)));
        }

        public async Task<MappableDTO[]> GetAsync(long[][] requestIds, Type requestType)
        {
            var requestString = GetRequestUri(ModelController, "GetArray", requestIds, requestType);
            var request = new HttpRequestMessage(HttpMethod.Get, requestString);

            using (var client = CreateClient())
            {
                var result = await DatabaseStatusService.StartRequestWhenReady(async () => await client.SendAsync(request), UpdateKind.Loading);

                if (result.IsSuccessStatusCode)
                {
                    var test = await result.Content.ReadAsStringAsync();
                    var items = await result.Content.ReadAsAsync<MappableDTO[]>();

                    if (items != null)
                    {
                        var retval = items;
                        return retval;
                    }
                }
                throw new WebException("Could not get entries - " + result.StatusCode.ToString());
            }

            throw new WebException("Could not get entries - unknown Error");
        }

        public async Task<MappableDTO[]> PostAsync(MappableDTO[] items, Type requestType)
        {
            var requestString = GetRequestUri(ModelController, "PostArray", null, requestType);

            using (var client = CreateClient())
            {
                var result = await DatabaseStatusService.StartRequestWhenReady(async () => await client.PostAsXmlAsync(requestString, items), UpdateKind.Saving);

                if (result.IsSuccessStatusCode)
                {
                    items = await result.Content.ReadAsAsync<MappableDTO[]>();

                    if (items != null)
                        return items;
                }
            }

            throw new WebException("Could not post entries");
        }

        public async Task<MappableDTO[]> PutAsync(MappableDTO[] items, Type requestType)
        {
            var requestUri = GetRequestUri(ModelController, "PutArray", null, requestType);

            using (var client = CreateClient())
            {
                var result = await DatabaseStatusService.StartRequestWhenReady(async () => await client.PutAsXmlAsync(requestUri, items), UpdateKind.Updating);

                if (result.IsSuccessStatusCode)
                {
                    items = await result.Content.ReadAsAsync<MappableDTO[]>();

                    if (items != null)
                        return items;
                }
                throw new WebException("Couldn not put entries - " + result.StatusCode);
            }

            throw new WebException("Could not put entries - unknown Error");
        }

        public async Task<bool> DeleteAsync(long[][] requestIds, Type requestType)
        {
            var requestString = GetRequestUri(ModelController, "DeleteArray", requestIds, requestType);

            using (var client = CreateClient())
            {
                var result = await DatabaseStatusService.StartRequestWhenReady(async () => await client.DeleteAsync(requestString), UpdateKind.Saving);

                if (result.IsSuccessStatusCode)
                {
                    var items = await result.Content.ReadAsAsync<bool>();
                    return items;
                }
                throw new WebException("Could not delete entries - " + result.StatusCode);
            }

            throw new WebException("Could not delete entries - unknown Error");
        }

        public async Task CalculateScoredResultsAsync(long sessionId)
        {
            var requestString = GetRequestUri(ActionController, "CalcResult", new long[][] { new long[] { sessionId } }, requestType: null);

            using (var client = CreateClient())
            {
                var result = await DatabaseStatusService.StartRequestWhenReady(async () => await client.PutAsXmlAsync<MappableDTO>(requestString, null), UpdateKind.Loading);
            }
        }

        public void AddDatabaseStatusListener(IDatabaseStatus listener)
        {
            DatabaseStatusService.AddStatusItem(listener);
        }

        public void RemoveDatabaseStatusListener(IDatabaseStatus listener)
        {
            DatabaseStatusService.RemoveStatusItem(listener);
        }

        public async Task<bool> CheckLeagueExists(string leagueName)
        {
            var requestString = $"{BaseUri.AbsoluteUri}/CheckLeague?id={leagueName}";

            using (var client = CreateClient())
            {
                var request = await client.GetAsync(requestString);

                if (request.IsSuccessStatusCode)
                {
                    return true;
                }
                else if (request.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UserNotAuthorizedException("User not authorized for this League");
                }
                else
                {
                    throw new LeagueNotFoundException($"League {leagueName} not found in database");
                }
            }
        }

        public async Task<IEnumerable<string>> GetLeagueNames()
        {
            var requestString = $"{BaseUri.AbsoluteUri}/CheckLeague";

            using (var client = CreateClient())
            {
                var request = await client.GetAsync(requestString);

                if (request.IsSuccessStatusCode)
                {
                    var leagueNames = await request.Content.ReadAsAsync<string[]>();
                    return leagueNames;
                }
                else if (request.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UserNotAuthorizedException("User not authorized for the service");
                }
            }
            return null;
        }

        public async Task<LeagueDTO> GetLeague(string leagueName)
        {
            var requestString = $"{BaseUri.AbsoluteUri}/CheckLeague?id={leagueName}";

            using (var client = CreateClient())
            {
                var request = await client.GetAsync(requestString);

                if (request.IsSuccessStatusCode)
                {
                    var league = await request.Content.ReadAsAsync<LeagueDTO>();
                    return league;
                }
                else if (request.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UserNotAuthorizedException("User not authorized for the service");
                }
                else
                {
                    throw new LeagueNotFoundException($"League {leagueName} not found in database");
                }
            }
        }
    }
}
