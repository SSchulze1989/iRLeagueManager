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

        public UserDatabaseClient(Uri baseUri)
        {
            BaseUri = baseUri;
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
}
