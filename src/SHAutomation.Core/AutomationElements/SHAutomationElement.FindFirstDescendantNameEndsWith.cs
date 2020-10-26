using System;
using System.Linq;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        public ISHAutomationElement FindFirstDescendantNameEndsWith(string name,  bool waitUntilExists = true)
        {
            return FindFirstDescendantNameEndsWith(name, TimeSpan.FromSeconds(20), waitUntilExists);
        }
        public ISHAutomationElement FindFirstDescendantNameEndsWith(string name, TimeSpan timeout, bool waitUntilExists = true)
        {
            var elements = FindAllDescendantsNameEndsWith(name, timeout, waitUntilExists);
            return elements.Any() ? elements.First() : null;
        }
    }
}
