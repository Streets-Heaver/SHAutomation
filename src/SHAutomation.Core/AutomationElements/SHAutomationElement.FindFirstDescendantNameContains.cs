using System.Linq;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        public ISHAutomationElement FindFirstDescendantNameContains(string name, int timeout = 20000, bool waitUntilExists = true)
        {
            var elements = FindAllDescendantsNameContains(name, timeout, waitUntilExists);
            return elements.Any() ? elements.First() : null;
        }
    }
}
