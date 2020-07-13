using System;
using System.Runtime.Serialization;
using SHAutomation.Core.Identifiers;

namespace SHAutomation.Core.Exceptions
{
    [Serializable]
    public class PropertyNotSupportedException : NotSupportedException
    {
        private const string DefaultMessage = "The requested property is not supported";
        private const string DefaultMessageWithData = "The requested property '{0}' is not supported";

        public PropertyNotSupportedException() : base(DefaultMessage)
        {
        }

        public PropertyNotSupportedException(string message) : base(message)
        {
        }

        public PropertyNotSupportedException(string message, Exception innerException) : base(message, innerException)
        {
        }
        public PropertyNotSupportedException(PropertyId property)
            : base(string.Format(DefaultMessageWithData, property))
        {
            Property = property;
        }

        public PropertyNotSupportedException(string message, PropertyId property)
            : base(message)
        {
            Property = property;
        }

        public PropertyNotSupportedException(PropertyId property, Exception innerException)
            : base(string.Format(DefaultMessageWithData, property), innerException)
        {
            Property = property;
        }

        public PropertyNotSupportedException(string message, PropertyId property, Exception innerException)
            : base(message, innerException)
        {
            Property = property;
        }

        protected PropertyNotSupportedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Property = (PropertyId)info.GetValue("Property", typeof(PropertyId));
        }

        public PropertyId Property { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("Property", Property);
            base.GetObjectData(info, context);
        }

       
    }
}
