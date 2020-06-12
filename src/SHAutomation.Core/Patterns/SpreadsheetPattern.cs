using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Patterns.Infrastructure;

namespace SHAutomation.Core.Patterns
{
    public interface ISpreadsheetPattern : IPattern
    {
      SHAutomationElement GetItemByName(string name);
    }
}
