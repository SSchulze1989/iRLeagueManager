using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Data
{
    public class UserCredentialsManager : IUserCredentialsManager
    {
        private NetworkCredential UserCredential { get; set; }

        private IUserDatabaseClient UserDatabaseClient { get; }
        public bool IsAuthenticated { get; private set; }

        public string AuthenticatedUserName => UserCredential?.UserName;

        public UserCredentialsManager(IUserDatabaseClient client)
        {
            UserDatabaseClient = client;
        }

        public ICredentials GetCredentials()
        {
            return UserCredential;
        }

        public async Task<bool> AuthenticateAsync(string userName, string password)
        {
            var result = await UserDatabaseClient.AuthenticateUserAsync(userName, password);

            if (result != null)
            {
                UserCredential = result;
                return true;
            }
            else
            {
                UserCredential = new NetworkCredential();
                return false;
            }
        }

        public void ClearCredentials()
        {
            UserCredential = new NetworkCredential();
        }

        public NetworkCredential GetCredential(Uri uri, string authType)
        {
            return UserCredential;
        }
    }
}
