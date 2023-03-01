using System.Collections.Generic;

namespace SHAutomation.Core.Identifiers
{
    public abstract partial class IdentifierBase
    {
        /// <summary>
        /// Class which capsules all identifiers which can be used for an automation library
        /// </summary>
        private class IdentifiersHolder
        {
            /// <summary>
            /// Dictionary which holds all known properties
            /// </summary>
            public readonly Dictionary<int, PropertyId> PropertyDict = new Dictionary<int, PropertyId>();

            /// <summary>
            /// Dictionary which holds all known events
            /// </summary>
            public readonly Dictionary<int, EventId> EventDict = new Dictionary<int, EventId>();

            /// <summary>
            /// Dictionary which holds all known patterns
            /// </summary>
            public readonly Dictionary<int, PatternId> PatternDict = new Dictionary<int, PatternId>();

            /// <summary>
            /// Dictionary which holds all known text attributes
            /// </summary>
            public readonly Dictionary<int, TextAttributeId> TextAttributeDict = new Dictionary<int, TextAttributeId>();
        }


    }
}
