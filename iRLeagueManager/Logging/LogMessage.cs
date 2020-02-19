using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Logging
{
    public class LogMessage 
    {
        public DateTime Timestamp { get; }
        public string Message { get; }
        public object Tag { get; }

        public LogMessage(string message, object tag = null)
        {
            Timestamp = DateTime.Now;
            Message = message;
            Tag = tag;
        }
    }
}
