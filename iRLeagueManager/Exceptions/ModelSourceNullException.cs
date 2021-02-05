using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRLeagueManager.Exceptions
{
    public class ModelSourceNullException : Exception
    {
        public Type SourceType { get; }
        public Type ContainerType { get; }
        public object PreviousModel { get; }

        public ModelSourceNullException(Type sourceType, Type containerType, object previousModel = null) : this(
                $"Error while updating model source: source was null\n" +
                $"\tSource type:\t{sourceType}\n" +
                $"\tContainer type:\t{containerType}\n", sourceType, containerType, previousModel)
        {
        }

        public ModelSourceNullException(string message, Type sourceType, Type containerType, object previousModel = null) : this(message, sourceType, containerType, null, previousModel)
        {
        }

        public ModelSourceNullException(string message, Type sourceType, Type containerType, Exception innerException, object previousModel = null) : base(message, innerException)
        {
            SourceType = sourceType;
            ContainerType = containerType;
            PreviousModel = previousModel;
        }
    }
}
