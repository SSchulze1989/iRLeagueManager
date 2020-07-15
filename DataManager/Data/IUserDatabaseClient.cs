using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace iRLeagueManager.Data
{
    public interface IUserDatabaseClient : IDisposable
    {
        Task<NetworkCredential> AuthenticateUserAsync(string userName, string password);
    }
}
