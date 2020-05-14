using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iRLeagueManager.Interfaces;

namespace iRLeagueManager.Data
{
    public class DatabaseToken : IToken
    {
        private readonly int rnd;

        public DatabaseToken()
        {
            var random = new Random();
            rnd = random.Next();
        }
    }

    public class RequestToken : DatabaseToken { }
}
