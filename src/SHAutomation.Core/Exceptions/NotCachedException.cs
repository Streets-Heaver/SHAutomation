using System;
using System.Runtime.Serialization;

namespace StreetsHeaver.SHAutomation.Core.Exceptions
{
    [Serializable]
    public class NotCachedException : SHAutomationException
    {
        public NotCachedException()
        {
        }

        public NotCachedException(string message)
            : base(message)
        {
        }

        public NotCachedException(Exception innerException) :
            base(string.Empty, innerException)
        {
        }

        public NotCachedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NotCachedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
