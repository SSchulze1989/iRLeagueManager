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
                IsAuthenticated = true;
                return true;
            }
            else
            {
                UserCredential = new NetworkCredential();
                IsAuthenticated = false;
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
