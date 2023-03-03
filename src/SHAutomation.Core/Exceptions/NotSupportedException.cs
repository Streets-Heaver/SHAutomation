using System;
using System.Runtime.Serialization;

namespace StreetsHeaver.SHAutomation.Core.Exceptions
{
    [Serializable]
    public class NotSupportedException : SHAutomationException
    {
        public NotSupportedException()
        {
        }

        public NotSupportedException(string message)
            : base(message)
        {
        }

        public NotSupportedException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }

        public NotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
