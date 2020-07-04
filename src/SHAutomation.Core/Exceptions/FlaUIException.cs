using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace SHAutomation.Core.Exceptions
{
    [Serializable]
    public class SHAutomationException : Exception
    {
        public SHAutomationException()
        {
        }

        public SHAutomationException(string message)
            : base(message)
        {
        }

        public SHAutomationException(Exception innerException)
            : base(string.Empty, innerException)
        {
        }

        public SHAutomationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected SHAutomationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
