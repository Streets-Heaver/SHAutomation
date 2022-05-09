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
            Func<List<ISHAutomationElement>> function = () => FindAllDescendants(x => x.ByName(name, Definitions.PropertyConditionFlag.MatchSubstring), TimeSpan.Zero).Where(x => x.Name.EndsWith(name)).ToList();
           
            GetElements(function,waitUntilExists, ref elements);
            if (!elements.Any() && waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                SHSpinWait.SpinUntil(() => GetElements(function, true,ref elements), timeout);
            }
            else if (elements.Any() && !waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                SHSpinWait.SpinUntil(() => GetElements(function, false, ref elements), timeout);
            }
            return elements.ToArray();
        }
    }
}
