using StreetsHeaver.SHAutomation.Core.Conditions;
using StreetsHeaver.SHAutomation.Core.StaticClasses;
using System;
using System.Threading;

namespace StreetsHeaver.SHAutomation.Core.AutomationElements
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
            Func<ISHAutomationElement> function = () => FindFirstDescendantBase();

            GetElement(function, waitUntilExists, ref element);
            if (element == null && waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                Wait.UntilTrue(() => GetElement(function, true, ref element), timeout);
            }
            else if (element != null && !waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                Wait.UntilTrue(() => GetElement(function, false, ref element), timeout);
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
        public ISHAutomationElement FindFirstDescendant(ConditionBase condition, bool waitUntilExists = true)
        {
            return FindFirstDescendant(condition, TimeSpan.FromSeconds(20), waitUntilExists);
        }

        public ISHAutomationElement FindFirstDescendant(ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true)
        {
            var token = CancellationToken.None;
            return FindFirstDescendant(condition, timeout, waitUntilExists, ref token);
        }
        public ISHAutomationElement FindFirstDescendant(ConditionBase condition, TimeSpan timeout, bool waitUntilExists, ref CancellationToken cancel)
        {
            ISHAutomationElement element = null;
            Func<ISHAutomationElement> function = () => FindFirstDescendantBase(condition);

            GetElement(function, true, ref element);
            if (element == null && waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                Wait.UntilTrue(() => GetElement(function, true, ref element), timeout, cancel);
            }
            else if (element != null && !waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                Wait.UntilTrue(() => GetElement(function, false, ref element), timeout, cancel);
            }

            return element?.FrameworkAutomationElement != null ? element : null;
        }
    }
}
