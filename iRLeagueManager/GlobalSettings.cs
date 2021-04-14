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
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;

using iRLeagueManager.Data;
//using iRLeagueManager.User;
using iRLeagueManager.Logging;
using iRLeagueManager.Locations;
using iRLeagueManager.Models;

namespace iRLeagueManager
{
    public static class GlobalSettings
    {
        private const bool debugErros = false;
        public static LeagueContext LeagueContext { get; private set; }
        public static ModelCache ModelCache { get; private set; }
        //public static UserContext UserContext { get; private set; }

        public static LocationCollection Locations { get; private set; } = new LocationCollection();

        private const int maxErrors = 10;
        private static int ErrorCount { get; set; }
        private static DateTime LastError { get; set; }
        public static Logger Logger { get; } = new Logger();

        public static void SetGlobalLeagueContext(LeagueContext context)
        {
            LeagueContext = context;
            //ModelManager = new ModelManager(LeagueContext);
        }

        public static void SetLocationCollection(LocationCollection locations)
        {
            Locations = locations;
        }

        //public static void SetGlobalUserContext(UserContext context)
        //{
        //    UserContext = context;
        //}

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
