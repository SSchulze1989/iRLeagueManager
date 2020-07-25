using iRLeagueManager.Models.User;
using iRLeagueManager.LeagueDBServiceRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Data
{
    public class UserDatabaseClient : IUserDatabaseClient
    {
        public IDatabaseStatusService DatabaseStatusService { get; set; }
        public Uri BaseUri { get; }

        private ICredentials userCredentials;

        public UserDatabaseClient(Uri baseUri)
        {
            BaseUri = baseUri;
            userCredentials = new NetworkCredential();
        }

        public UserDatabaseClient(Uri baseUri, ICredentials credentials) : this(baseUri)
        {
            userCredentials = credentials;
        }

        internal void SetCredentials(ICredentials credentials)
        {
            userCredentials = credentials;
        }

        public async Task<NetworkCredential> AuthenticateUserAsync(string userName, string password)
        {
            var requestString = string.Format(BaseUri.AbsoluteUri + "/User");
            var credentials = new NetworkCredential(userName, password);

            using (var client = new HttpClient(new HttpClientHandler() { Credentials = credentials }))
            {
                var request = await DatabaseStatusService.StartRequestWhenReady(async () => await client.GetAsync(requestString), Enums.UpdateKind.Loading);

                if (request.IsSuccessStatusCode)
                {
                    var result = await request.Content.ReadAsAsync<bool>();
                    return credentials;
                }
            }
            return null;
        }

        public async Task<UserDTO> GetUserAsync(string UserId)
        {
            var requestString = string.Format(BaseUri.AbsoluteUri + "/User?id=" + UserId);

            using (var client = new HttpClient(new HttpClientHandler() { Credentials = userCredentials }))
            {
                var request = await DatabaseStatusService.StartRequestWhenReady(async () => await client.GetAsync(requestString), Enums.UpdateKind.Loading);

                if (request.IsSuccessStatusCode)
                {
                    var result = await request.Content.ReadAsAsync<UserDTO>();
                    return result;
                }
            }
            return null;
        }

        public async Task<UserProfileDTO> AddUserAsync(AddUserDTO user)
        {
            if (user == null)
                return null;

            var requestString = BaseUri.AbsoluteUri + "/User";

            using (var client = new HttpClient())
            {
                var request = await DatabaseStatusService.StartRequestWhenReady(async () => await client.PostAsXmlAsync(requestString, user), Enums.UpdateKind.Saving);

                if (request.IsSuccessStatusCode)
                {
                    var result = await request.Content.ReadAsAsync<UserProfileDTO>();
                    return result;
                }
                else if (request.StatusCode == HttpStatusCode.Conflict)
                {
                    throw new UserExistsException();
                }
            }
            return null;
        }

        #region IDisposable Support
        private bool disposedValue = false; // Dient zur Erkennung redundanter Aufrufe.

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: verwalteten Zustand (verwaltete Objekte) entsorgen.
                }

                // TODO: nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalizer weiter unten überschreiben.
                // TODO: große Felder auf Null setzen.

                disposedValue = true;
            }
        }

        // TODO: Finalizer nur überschreiben, wenn Dispose(bool disposing) weiter oben Code für die Freigabe nicht verwalteter Ressourcen enthält.
        // ~UserDatabaseClient()
        // {
        //   // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
        //   Dispose(false);
        // }

        // Dieser Code wird hinzugefügt, um das Dispose-Muster richtig zu implementieren.
        public void Dispose()
        {
            // Ändern Sie diesen Code nicht. Fügen Sie Bereinigungscode in Dispose(bool disposing) weiter oben ein.
            Dispose(true);
            // TODO: Auskommentierung der folgenden Zeile aufheben, wenn der Finalizer weiter oben überschrieben wird.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public class UserExistsException : Exception
    {
        public UserExistsException() : base("User already in Database") { }
        public UserExistsException(string message) : base(message) { }

        public UserExistsException(string message, Exception innerException) :  base(message, innerException) { }
    }
}
