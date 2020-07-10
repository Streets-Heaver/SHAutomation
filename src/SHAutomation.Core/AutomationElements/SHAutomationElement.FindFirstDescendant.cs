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
        public ISHAutomationElement FindFirstDescendant()
        {
            return FindFirstDescendant(20000);
        }
        public ISHAutomationElement FindFirstDescendant(int timeout = 20000, bool waitUntilExists = true)
        {
            ISHAutomationElement element = null;
            bool getElement(bool shouldExist)
            {
                if (element == null)
                {
                    element = FindFirstDescendantBase();
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
        public ISHAutomationElement FindFirstDescendant(string automationId)
        {
            Func<ConditionFactory, ConditionBase> conditionFunc = x => x.ByAutomationId(automationId);
            ConditionBase condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindFirstDescendant(conditionFunc, 20000);
        }
        public ISHAutomationElement FindFirstDescendant(string automationId, int timeout = 20000)
        {
            Func<ConditionFactory, ConditionBase> conditionFunc = x => x.ByAutomationId(automationId);
            ConditionBase condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindFirstDescendant(conditionFunc, timeout);
        }

        public ISHAutomationElement FindFirstDescendant(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            return FindFirstDescendant(conditionFunc, 20000);
        }

        public ISHAutomationElement FindFirstDescendant(Func<ConditionFactory, ConditionBase> conditionFunc, int timeout = 20000, bool waitUntilExists = true)
        {
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindFirstDescendant(condition, timeout: timeout, waitUntilExists: waitUntilExists);
        }

        public ISHAutomationElement FindFirstDescendant(ConditionBase condition, int timeout = 20000, bool waitUntilExists = true)
        {
            SHAutomationElement element = null;
            bool getElement(bool shouldExist)
            {
                if ((shouldExist && element == null) || (!shouldExist && element != null))
                {
                    element = FindFirstDescendantBase(condition);
                }
                return shouldExist ? element?.FrameworkAutomationElement != null : element == null;
            }
            getElement(true);
            if (element == null && waitUntilExists && timeout > 0)
            {
                SHSpinWait.SpinUntil(() => getElement(waitUntilExists), timeout);
            }
            else if (element != null && !waitUntilExists && timeout > 0)
            {
                SHSpinWait.SpinUntil(() => getElement(waitUntilExists), timeout);
            }

            return element?.FrameworkAutomationElement != null ? element : null;
        }

    }
}
