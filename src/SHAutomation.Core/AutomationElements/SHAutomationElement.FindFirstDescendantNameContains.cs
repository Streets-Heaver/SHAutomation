using System;
using System.Linq;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {

        public ISHAutomationElement FindFirstDescendantNameContains(string name,  bool waitUntilExists = true)
        {
            return FindFirstDescendantNameContains(name, TimeSpan.FromSeconds(20), waitUntilExists);
          
        }
        public ISHAutomationElement FindFirstDescendantNameContains(string name, TimeSpan timeout, bool waitUntilExists = true)
        {
            var elements = FindAllDescendantsNameContains(name, timeout, waitUntilExists);
            return elements.Any() ? elements.First() : null;
        }
    }
}
