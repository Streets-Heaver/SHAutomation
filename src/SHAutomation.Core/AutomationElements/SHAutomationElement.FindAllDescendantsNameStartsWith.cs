using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        public ISHAutomationElement[] FindAllDescendantsNameStartsWith(string name, int timeout = 20000, bool waitUntilExists = true)
        {
            List<ISHAutomationElement> elements = new List<ISHAutomationElement>();
            bool getElements(bool shouldExist)
            {
                if(!elements.Any())
                elements = FindAllDescendants(timeout: 0).Where(x => x.FrameworkAutomationElement != null && x.SupportsName && !string.IsNullOrEmpty(x.Name) && x.Name.StartsWith(name)).ToList();
                return shouldExist ? elements.Any() : !elements.Any();
            }
            getElements(waitUntilExists);
            if (!elements.Any() && waitUntilExists && timeout > 0)
            {
                SHSpinWait.SpinUntil(() => getElements(true), timeout);
            }
            else if (elements.Any() && !waitUntilExists && timeout > 0)
            {
                SHSpinWait.SpinUntil(() => getElements(false), timeout);
            }
            return elements.ToArray();
        }
    }
}
