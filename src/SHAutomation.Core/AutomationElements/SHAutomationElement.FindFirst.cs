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
            SHAutomationElement element = null;
            bool getElement(bool shouldExist)
            {
                if (element == null)
                {
                    element = FindFirstBase(treeScope, condition);
                }
                return shouldExist ? element?.FrameworkAutomationElement != null : element == null;
            }
            getElement(waitUntilExists);
            if (element == null && waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                SHSpinWait.SpinUntil(() => getElement(true), timeout);
            }
            else if (element != null && !waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                SHSpinWait.SpinUntil(() => getElement(false), timeout);
            }
            return element?.FrameworkAutomationElement != null ? element : null;
        }
    }
}
