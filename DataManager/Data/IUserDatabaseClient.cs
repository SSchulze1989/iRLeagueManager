using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using iRLeagueManager.LeagueDBServiceRef;

namespace iRLeagueManager.Data
{
    public interface IUserDatabaseClient : IDisposable
    {
        Task<NetworkCredential> AuthenticateUserAsync(string userName, string password);
        Task<UserDTO> GetUserAsync(string UserId);
    }
}
