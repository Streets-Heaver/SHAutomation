using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        public ISHAutomationElement[] FindAllDescendantsNameEndsWith(string name, bool waitUntilExists = true)
        {
            return FindAllDescendantsNameEndsWith(name, TimeSpan.FromSeconds(20), waitUntilExists);
        }

        public ISHAutomationElement[] FindAllDescendantsNameEndsWith(string name, TimeSpan timeout, bool waitUntilExists = true)
        {
            List<ISHAutomationElement> elements = new List<ISHAutomationElement>();
            bool getElements(bool shouldExist)
            {
                if (!elements.Any())
                    elements = FindAllDescendants(timeout: TimeSpan.Zero).Where(x => x.FrameworkAutomationElement != null && x.SupportsName && !string.IsNullOrEmpty(x.Name) && x.Name.EndsWith(name)).ToList();
                return shouldExist ? elements.Any() : !elements.Any();
            }
            getElements(waitUntilExists);
            if (!elements.Any() && waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                SHSpinWait.SpinUntil(() => getElements(true), timeout);
            }
            else if (elements.Any() && !waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                SHSpinWait.SpinUntil(() => getElements(false), timeout);
            }
            return elements.ToArray();
        }
    }
}
