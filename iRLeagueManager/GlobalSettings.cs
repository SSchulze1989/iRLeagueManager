using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

using iRLeagueManager.Data;
using iRLeagueManager.User;
using iRLeagueManager.Logging;
using iRLeagueManager.Locations;

namespace iRLeagueManager
{
    public static class GlobalSettings
    {
        private const bool debugErros = false;
        public static LeagueContext LeagueContext { get; private set; }
        public static ModelManager ModelManager { get; private set; }
        public static UserContext UserContext { get; private set; }

        public static LocationCollection Locations { get; } = new LocationCollection();

        private const int maxErrors = 10;
        private static int ErrorCount { get; set; }
        private static DateTime LastError { get; set; }
        public static Logger Logger { get; } = new Logger();

        public static void SetGlobalLeagueContext(LeagueContext context)
        {
            LeagueContext = context;
            //ModelManager = new ModelManager(LeagueContext);
        }

        public static void SetGlobalUserContext(UserContext context)
        {
            UserContext = context;
        }

        public static void LogError(Exception e)
        {
            if (DateTime.Now - LastError <= TimeSpan.FromSeconds(30))
            {
                ErrorCount += 1;
            }
            else
            {
                ErrorCount = 1;
            }
            LastError = DateTime.Now;
            
            if (ErrorCount >= maxErrors)
            {
                throw e;
            }

            if (debugErros && MessageBox.Show(e.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error, MessageBoxResult.OK) == MessageBoxResult.Cancel)
                throw e;
            else
                Logger.ErrLog(new ExceptionLogMessage(e));
        }
    }
}
