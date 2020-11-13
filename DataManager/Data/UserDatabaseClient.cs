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

using iRLeagueManager.Models.User;
using iRLeagueDatabase.DataTransfer.User;
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

        public async Task<UserProfileDTO> PutUserAsync(UserProfileDTO user)
        {
            if (user == null)
                return null;

            var requestString = BaseUri.AbsoluteUri + "/User";

            using (var client = new HttpClient(new HttpClientHandler() { Credentials = userCredentials })) 
            {
                var request = await DatabaseStatusService.StartRequestWhenReady(async () => await client.PutAsXmlAsync(requestString, user), Enums.UpdateKind.Saving);

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

        public async Task<bool> ChangePassword(string userName, string oldPw, AddUserDTO user)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(oldPw) || user == null)
            {
                return false;
            }

            var requestString = BaseUri.AbsoluteUri + "/ChangePassword";
            var credentials = new NetworkCredential(userName, oldPw);

            using (var client = new HttpClient(new HttpClientHandler() { Credentials = credentials }))
            {
                var result = await DatabaseStatusService.StartRequestWhenReady(async () => await client.PostAsXmlAsync(requestString, user), Enums.UpdateKind.Saving);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else if (result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UserNotAuthorizedException("Could not change password - old password incorrect");
                }
                else
                {
                    throw new WebException("Could not change password - " + result.StatusCode);
                }
            }
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
