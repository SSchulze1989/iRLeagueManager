using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Exceptions
{
    public class ModelInitializeException : Exception
    {
        public ModelInitializeException()
        {
        }

        public ModelInitializeException(string message) : base(message)
        {
        }

        public ModelInitializeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ModelInitializeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
