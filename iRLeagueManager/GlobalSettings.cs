using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iRLeagueManager.Data;
using iRLeagueManager.User;

namespace iRLeagueManager
{
    public static class GlobalSettings
    {
        public static LeagueContext LeagueContext { get; private set; }
        public static UserContext UserContext { get; private set; }

        public static void SetGlobalLeagueContext(LeagueContext context)
        {
            LeagueContext = context;
        }

        public static void SetGlobalUserContext(UserContext context)
        {
            UserContext = context;
        }

        public static void LogError(Exception e)
        {
            throw e;
        }
    }
}
