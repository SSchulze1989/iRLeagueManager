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

        public Logger()
        {
            //Create log file:
            LogFilename = DateTime.Now.ToString("dd_MM_yyyy-hh_mm_ss") + ".log";
            using (var log = File.CreateText(LogFilename))
            {
                log.WriteLine("## Session startet: " + DateTime.Now.ToString() + " - Log Messages ##");
            }
        }

        public void Log(string message, object tag = null)
        {
            var msg = new LogMessage(message, tag);
            Log(msg);
        }

        public void Log(LogMessage msg)
        {
            messages.Add(msg);
            using (var log = File.AppendText(LogFilename))
            {
                log.WriteLine(msg.Timestamp.ToShortTimeString() + " :: " + msg.Message + " -- attch: " + msg.Tag.ToString());
            }
        }

        public void ErrLog(ExceptionLogMessage e)
        {
            errorMessages.Add(e);
            Log(e);
        }
    }
}
