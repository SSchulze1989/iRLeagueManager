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
using System.Collections.ObjectModel;
using System.IO;

namespace iRLeagueManager.Logging
{
    public class Logger 
    {
        private ObservableCollection<LogMessage> messages = new ObservableCollection<LogMessage>();
        public ReadOnlyObservableCollection<LogMessage> Messages => new ReadOnlyObservableCollection<LogMessage>(messages);

        private ObservableCollection<ExceptionLogMessage> errorMessages = new ObservableCollection<ExceptionLogMessage>();
        public ReadOnlyObservableCollection<ExceptionLogMessage> ErrorMessages => new ReadOnlyObservableCollection<ExceptionLogMessage>(errorMessages);
        public string LogFilename { get; private set; }
        public DateTime SessionStart { get; } =  DateTime.Now;

        public Logger()
        {
            //Create log file:
            LogFilename = DateTime.Now.ToString("dd_MM_yyyy-hh_mm_ss") + ".log";
        }

        public void Log(string message, object tag = null)
        {
            var msg = new LogMessage(message, tag);
            Log(msg);
        }

        public void Log(LogMessage msg)
        {
            if (!File.Exists(LogFilename))
            {
                using (var log = File.CreateText(LogFilename))
                {
                    log.WriteLine("## Session startet: " + DateTime.Now.ToString() + " - Log Messages ##");
                }
            }
            messages.Add(msg);
            using (var log = File.AppendText(LogFilename))
            {
                log.WriteLine(msg.Timestamp.ToShortTimeString() + " :: " + msg.Message + " -- attch: " + msg.Tag.ToString());
            }
        }

        public void ErrLog(ExceptionLogMessage e)
        {
            Log(e);
            errorMessages.Add(e);
        }
    }
}
