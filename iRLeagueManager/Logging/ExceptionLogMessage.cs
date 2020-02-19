using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Logging
{
    public class ExceptionLogMessage : LogMessage
    {
        public Exception Exception => Tag as Exception;

        public ExceptionLogMessage(Exception exception) : base(exception.Message, exception) { }
    }
}
