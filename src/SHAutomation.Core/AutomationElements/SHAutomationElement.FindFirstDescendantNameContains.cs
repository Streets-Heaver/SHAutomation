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
        public ISHAutomationElement FindFirstDescendantNameContains(string name, int timeout = 20000, bool waitUntilExists = true)
        {
            var elements = FindAllDescendantsNameContains(name, timeout, waitUntilExists);
            return elements.Any() ? elements.First() : null;
        }
    }
}
