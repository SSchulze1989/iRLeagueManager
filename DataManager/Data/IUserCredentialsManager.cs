using iRLeagueManager.LeagueDBServiceRef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Data
{
    public interface IUserCredentialsManager : ICredentialsProvider
    {
        bool IsAuthenticated { get; }
        string AuthenticatedUserName { get; }
        Task<bool> AuthenticateAsync(string userName, string password);
        void ClearCredentials();
    }
}
