using System;
using System.Collections.Generic;

namespace StreetsHeaver.SHAutomation.Core.Identifiers
{
    /// <summary>
    /// Base class for wrappers around various identifiers
    /// </summary>
    public abstract partial class IdentifierBase : IEquatable<IdentifierBase>
    {
        private static IdentifiersHolder _idHolder = new IdentifiersHolder();
        /// <summary>
        /// The native id of the identifier
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// A readable name for the identifier
        /// </summary>
        public string Name { get; }

        protected IdentifierBase(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public bool Equals(IdentifierBase other)
        {
            return other != null && Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as IdentifierBase);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return $"{Name} [#{Id}]";
        }

        protected static PropertyId RegisterProperty(int id, string name)
        {
            return Register(id, _idHolder.PropertyDict, () => new PropertyId(id, name));
        }

        protected static EventId RegisterEvent(int id, string name)
        {
            return Register(id, _idHolder.EventDict, () => new EventId(id, name));
        }

        protected static PatternId RegisterPattern(int id, string name, PropertyId availabilityProperty)
        {
            return Register(id, _idHolder.PatternDict, () => new PatternId(id, name, availabilityProperty));
        }

        protected static TextAttributeId RegisterTextAttribute(int id, string name)
        {
            return Register(id, _idHolder.TextAttributeDict, () => new TextAttributeId(id, name));
        }

        protected static PropertyId FindProperty(int id)
        {
            return _idHolder.PropertyDict.ContainsKey(id) ? _idHolder.PropertyDict[id] : new PropertyId(id, string.Format("Property#{0}", id));
        }

        protected static EventId FindEvent(int id)
        {
            return _idHolder.EventDict.ContainsKey(id) ? _idHolder.EventDict[id] : new EventId(id, string.Format("Event#{0}", id));
        }

        protected static PatternId FindPattern(int id)
        {
            return _idHolder.PatternDict.ContainsKey(id) ? _idHolder.PatternDict[id] : new PatternId(id, string.Format("Pattern#{0}", id), null);
        }

        protected static TextAttributeId FindTextAttribute(int id)
        {
            return _idHolder.TextAttributeDict.ContainsKey(id) ? _idHolder.TextAttributeDict[id] : new TextAttributeId(id, string.Format("TextAttribute#{0}", id));
        }

        /// <summary>
        /// Adds the property to the dictionary if it does not exist yet
        /// </summary>
        private static T Register<T>(int commonId, IDictionary<int, T> dict, Func<T> creator)
        {
            if (dict.ContainsKey(commonId))
            {
                return dict[commonId];
            }
            var newIdObject = creator();
            dict[commonId] = newIdObject;
            return newIdObject;
        }




    }
}
