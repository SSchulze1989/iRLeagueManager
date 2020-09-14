using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Data
{
    public interface ILeagueDataProvider 
    {
        /// <summary>
        /// Check if league exists in the database and if the current user has access to it.
        /// </summary>
        /// <param name="leagueName"></param>
        /// <returns></returns>
        /// <exception cref="UserNotAuthorizedException">User is not authorized for this league</exception>
        /// <exception cref="LeagueNotFoundException">League name not found in database</exception>
        Task<bool> CheckLeagueExists(string leagueName);
    }
    public class UserNotAuthorizedException : Exception
    {
        public UserNotAuthorizedException() : base() { }

        public UserNotAuthorizedException(string message) : base(message)
        {
        }

        public UserNotAuthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class LeagueNotFoundException : Exception
    {
        public LeagueNotFoundException() : base() { }

        public LeagueNotFoundException(string message) : base(message)
        {
        }

        public LeagueNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
