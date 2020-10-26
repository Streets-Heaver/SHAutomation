using System;
using System.Linq;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        public ISHAutomationElement FindFirstDescendantNameStartsWith(string name, bool waitUntilExists = true)
        {
            return FindFirstDescendantNameStartsWith(name, TimeSpan.FromSeconds(20), waitUntilExists);
        }
        public ISHAutomationElement FindFirstDescendantNameStartsWith(string name, TimeSpan timeout, bool waitUntilExists = true)
        {
            var elements = FindAllDescendantsNameStartsWith(name, timeout, waitUntilExists);
            return elements.Any() ? elements.First() : null;
        }
    }
}
