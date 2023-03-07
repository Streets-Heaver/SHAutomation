using System;
using System.Runtime.Serialization;

namespace StreetsHeaver.SHAutomation.Core.Exceptions
{
    [Serializable]
    public class ElementNotEnabledException : SHAutomationException
    {
        public ElementNotEnabledException()
        {
        }

        public ElementNotEnabledException(string message)
            : base(message)
        {
        }

        public ElementNotEnabledException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }

        public ElementNotEnabledException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ElementNotEnabledException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
