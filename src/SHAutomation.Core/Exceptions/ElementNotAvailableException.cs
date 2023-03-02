using System;
using System.Runtime.Serialization;

namespace SHAutomation.Core.Exceptions
{
    [Serializable]
    public class ElementNotAvailableException : SHAutomationException
    {
        public ElementNotAvailableException()
        {
        }

        public ElementNotAvailableException(string message)
            : base(message)
        {
        }

        public ElementNotAvailableException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }

        public ElementNotAvailableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ElementNotAvailableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
