using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns.Infrastructure;

namespace StreetsHeaver.SHAutomation.Core.Patterns
{
    public interface IItemContainerPattern : IPattern
    {
      SHAutomationElement FindItemByProperty(SHAutomationElement startAfter, PropertyId property, object value);
    }
}
