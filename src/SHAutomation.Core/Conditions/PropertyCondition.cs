using StreetsHeaver.SHAutomation.Core.Definitions;
using StreetsHeaver.SHAutomation.Core.Identifiers;

namespace StreetsHeaver.SHAutomation.Core.Conditions
{
    public class PropertyCondition : ConditionBase
    {
        public PropertyCondition(PropertyId property, object value)
            : this(property, value, PropertyConditionFlag.None)
        {
        }

        public PropertyCondition(PropertyId property, object value, PropertyConditionFlag propertyConditionFlags)
        {
            Property = property;
            Value = value;
            PropertyConditionFlags = propertyConditionFlags;
        }

        public PropertyId Property { get; }

        public PropertyConditionFlag PropertyConditionFlags { get; private set; }

        public object Value { get; }

        public override string ToString()
        {
            return $"{Property}: {Value}";
        }
    }
}
