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
        public ISHAutomationElement FindFirst(TreeScope treeScope, ConditionBase condition)
        {
            return FindFirst(treeScope, condition, 20000);
        }
        public ISHAutomationElement FindFirst(TreeScope treeScope, ConditionBase condition, int timeout = 20000, bool waitUntilExists = true)
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
            if (element == null && waitUntilExists && timeout > 0)
            {
                SHSpinWait.SpinUntil(() => getElement(true), timeout);
            }
            else if (element != null && !waitUntilExists && timeout > 0)
            {
                SHSpinWait.SpinUntil(() => getElement(false), timeout);
            }
            return element?.FrameworkAutomationElement != null ? element : null;
        }
    }
}
