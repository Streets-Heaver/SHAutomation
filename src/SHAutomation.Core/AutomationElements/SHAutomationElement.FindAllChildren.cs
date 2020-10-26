using SHAutomation.Core.Conditions;
using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        public ISHAutomationElement[] FindAllChildren()
        {
            return FindAllChildren(TimeSpan.FromSeconds(20));
        }
        public ISHAutomationElement[] FindAllChildren(TimeSpan timeout)
        {
            List<SHAutomationElement> elements = new List<SHAutomationElement>();
            bool getElements()
            {
                if(!elements.Any())
                elements = FindAllChildrenBase().Where(x => x.FrameworkAutomationElement != null).ToList();
                return elements.Any();
            }
            getElements();
            if (!elements.Any() && timeout != TimeSpan.Zero)
            {
                SHSpinWait.SpinUntil(() => getElements(), timeout);
            }

            if (elements != null && elements.Any())
            {
                return elements.ToArray();
            }

            return Array.Empty<SHAutomationElement>();
        }
        public ISHAutomationElement[] FindAllChildren(ConditionBase condition, bool waitUntilExists = true)
        {
            return FindAllChildren(condition, TimeSpan.FromSeconds(20), waitUntilExists);
        }
        public ISHAutomationElement[] FindAllChildren(Func<ConditionFactory, ConditionBase> conditionFunc, bool waitUntilExists = true)
        {
            return FindAllChildren(conditionFunc, TimeSpan.FromSeconds(20), waitUntilExists);
        }
        public ISHAutomationElement[] FindAllChildren(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, bool waitUntilExists = true)
        {
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindAllChildren(condition, timeout, waitUntilExists);
        }

        public ISHAutomationElement[] FindAllChildren(ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true)
        {
            List<SHAutomationElement> elements = new List<SHAutomationElement>();
            bool getElements(bool shouldExist)
            {
                if (!elements.Any())
                    elements = FindAllChildrenBase(condition).Where(x => x.FrameworkAutomationElement != null).ToList();
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
