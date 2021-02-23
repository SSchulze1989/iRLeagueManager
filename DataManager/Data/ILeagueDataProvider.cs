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

using iRLeagueDatabase.DataTransfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Data
{
    public interface ILeagueDataProvider 
    {
        string LeagueName { get; set; }

        /// <summary>
        /// Get league names from service
        /// </summary>
        /// <returns>Array of league names</returns>
        /// <exception cref="UserNotAuthorizedException">User is not authorized to access service</exception>
        Task<IEnumerable<string>> GetLeagueNames();

        /// <summary>
        /// Check if league exists in the database and if the current user has access to it.
        /// </summary>
        /// <param name="leagueName"></param>
        /// <returns></returns>
        /// <exception cref="UserNotAuthorizedException">User is not authorized for this league</exception>
        /// <exception cref="LeagueNotFoundException">League name not found in database</exception>
        Task<bool> CheckLeagueExists(string leagueName);

        /// <summary>
        /// Get details for a single league from the API
        /// </summary>
        /// <param name="leagueName">Shortname of the league</param>
        /// <returns></returns>
        Task<LeagueDTO> GetLeague(string leagueName);
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
