using SHAutomation.Core.Conditions;
using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        public  ISHAutomationElement[] FindAllDescendants()
        {
            return FindAllDescendants(TimeSpan.FromSeconds(20));
        }
        public ISHAutomationElement[] FindAllDescendants(TimeSpan timeout)
        {
            List<SHAutomationElement> elements = new List<SHAutomationElement>();

            bool getElements()
            {
                if (!elements.Any())
                    elements = FindAllDescendantsBase().Where(x => x.FrameworkAutomationElement != null).ToList();
                return elements.Any();
            }
            getElements();
            if (!elements.Any() && timeout.TotalSeconds != 0)
            {
                SHSpinWait.SpinUntil(() => getElements(), timeout);
            }

            if (elements != null && elements.Any())
            {
                return elements.ToArray();
            }

            return Array.Empty<SHAutomationElement>();
        }
        public  ISHAutomationElement[] FindAllDescendants(ConditionBase condition)
        {
            return FindAllDescendants(condition, TimeSpan.FromSeconds(20));
        }
        public  ISHAutomationElement[] FindAllDescendants(Func<ConditionFactory, ConditionBase> conditionFunc, bool waitUntilExists = true)
        {
            return FindAllDescendants(conditionFunc, TimeSpan.FromSeconds(20), waitUntilExists);
        }
        public ISHAutomationElement[] FindAllDescendants(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, bool waitUntilExists = true)
        {
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindAllDescendants(condition, timeout, waitUntilExists);
        }

        public ISHAutomationElement[] FindAllDescendants(ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true)
        {
            List<SHAutomationElement> elements = new List<SHAutomationElement>();
            bool getElements(bool shouldExist)
            {
                if (!elements.Any())
                    elements = FindAllDescendantsBase(condition).Where(x => x.FrameworkAutomationElement != null).ToList();
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
            if (elements != null && elements.Any())
            {
                return elements.ToArray();
            }
            return Array.Empty<SHAutomationElement>();
        }
    }
}
