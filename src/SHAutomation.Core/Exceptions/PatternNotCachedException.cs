using System;
using System.Runtime.Serialization;
using SHAutomation.Core.Identifiers;

namespace SHAutomation.Core.Exceptions
{
    [Serializable]
    public class PatternNotCachedException : NotCachedException
    {
        private const string DefaultMessage = "The requested pattern is not cached";
        private const string DefaultMessageWithData = "The requested pattern '{0}' is not cached";

        public PatternNotCachedException() : base(DefaultMessage)
        {
        }

        public PatternNotCachedException(string message) : base(message)
        {
        }

        public PatternNotCachedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public PatternNotCachedException(PatternId pattern)
            : base(string.Format(DefaultMessageWithData, pattern))
        {
            Pattern = pattern;
        }

        public PatternNotCachedException(string message, PatternId pattern)
            : base(message)
        {
            Pattern = pattern;
        }

        public PatternNotCachedException(PatternId pattern, Exception innerException)
            : base(string.Format(DefaultMessageWithData, pattern), innerException)
        {
            Pattern = pattern;
        }

        public PatternNotCachedException(string message, PatternId pattern, Exception innerException)
            : base(message, innerException)
        {
            Pattern = pattern;
        }

        protected PatternNotCachedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Pattern = (PatternId)info.GetValue("Pattern", typeof(PatternId));
        }

        public PatternId Pattern { get; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }
            info.AddValue("Pattern", Pattern);
            base.GetObjectData(info, context);
        }

        
    }
}
