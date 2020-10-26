using SHAutomation.Core.Conditions;
using SHAutomation.Core.StaticClasses;
using System;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        public ISHAutomationElement FindFirstDescendant(bool waitUntilExists = true)
        {
            return FindFirstDescendant(TimeSpan.FromSeconds(20), waitUntilExists);
        }
        public ISHAutomationElement FindFirstDescendant(TimeSpan timeout, bool waitUntilExists = true)
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
        public ISHAutomationElement FindFirstDescendant(string automationId)
        {
            Func<ConditionFactory, ConditionBase> conditionFunc = x => x.ByAutomationId(automationId);
            ConditionBase condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindFirstDescendant(conditionFunc, TimeSpan.FromSeconds(20));
        }
        public ISHAutomationElement FindFirstDescendant(string automationId, TimeSpan timeout)
        {
            Func<ConditionFactory, ConditionBase> conditionFunc = x => x.ByAutomationId(automationId);
            ConditionBase condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindFirstDescendant(conditionFunc, timeout);
        }

        public ISHAutomationElement FindFirstDescendant(Func<ConditionFactory, ConditionBase> conditionFunc, bool waitUntilExists = true)
        {
            return FindFirstDescendant(conditionFunc, TimeSpan.FromSeconds(20), waitUntilExists);
        }

        public ISHAutomationElement FindFirstDescendant(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, bool waitUntilExists = true)
        {
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindFirstDescendant(condition, timeout: timeout, waitUntilExists: waitUntilExists);
        }
        public ISHAutomationElement FindFirstDescendant(ConditionBase condition,  bool waitUntilExists = true)
        {
            return FindFirstDescendant(condition, TimeSpan.FromSeconds(20), waitUntilExists);
        }

        public ISHAutomationElement FindFirstDescendant(ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true)
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
            if (element == null && waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                SHSpinWait.SpinUntil(() => getElement(waitUntilExists), timeout);
            }
            else if (element != null && !waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                SHSpinWait.SpinUntil(() => getElement(waitUntilExists), timeout);
            }

            return element?.FrameworkAutomationElement != null ? element : null;
        }

    }
}
