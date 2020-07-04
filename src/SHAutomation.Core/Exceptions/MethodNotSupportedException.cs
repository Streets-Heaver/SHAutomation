using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SHAutomation.Core.Exceptions
{
    [Serializable]
    public class MethodNotSupportedException : SHAutomationException
    {
        public MethodNotSupportedException()
        {
        }

        public MethodNotSupportedException(string message)
            : base(message)
        {
        }

        public MethodNotSupportedException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }

        public MethodNotSupportedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected MethodNotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
