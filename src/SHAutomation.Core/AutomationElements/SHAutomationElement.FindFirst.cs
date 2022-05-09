using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.StaticClasses;
using System;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        public ISHAutomationElement FindFirst(TreeScope treeScope, ConditionBase condition, bool waitUntilExists = true)
        {
            return FindFirst(treeScope, condition, TimeSpan.FromSeconds(20), waitUntilExists);
        }
        public ISHAutomationElement FindFirst(TreeScope treeScope, ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true)
        {
            ISHAutomationElement element = null;
            Func<ISHAutomationElement> function = () => FindFirstBase(treeScope, condition);
           
            GetElement(function,waitUntilExists, ref element);
            if (element == null && waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                SHSpinWait.SpinUntil(() => GetElement(function, true, ref element), timeout);
            }
            else if (element != null && !waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                SHSpinWait.SpinUntil(() => GetElement(function, false, ref element), timeout);
            }
            return element?.FrameworkAutomationElement != null ? element : null;
        }
    }
}
