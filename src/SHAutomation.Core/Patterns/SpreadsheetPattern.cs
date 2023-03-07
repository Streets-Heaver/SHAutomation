using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.Patterns.Infrastructure;

namespace StreetsHeaver.SHAutomation.Core.Patterns
{
    public interface ISpreadsheetPattern : IPattern
    {
      SHAutomationElement GetItemByName(string name);
    }
}
