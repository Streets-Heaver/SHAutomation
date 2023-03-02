using System;
using System.Runtime.Serialization;

namespace SHAutomation.Core.Exceptions
{
    [Serializable]
    public class NoClickablePointException : SHAutomationException
    {
        public NoClickablePointException()
        {
        }

        public NoClickablePointException(string message)
            : base(message)
        {
        }

        public NoClickablePointException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }

        public NoClickablePointException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected NoClickablePointException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
