using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns.Infrastructure;

namespace SHAutomation.Core.Patterns
{
    public interface IItemContainerPattern : IPattern
    {
      SHAutomationElement FindItemByProperty(SHAutomationElement startAfter, PropertyId property, object value);
    }
}
