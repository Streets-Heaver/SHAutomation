using StreetsHeaver.SHAutomation.Core.Conditions;
using StreetsHeaver.SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StreetsHeaver.SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        public ISHAutomationElement[] FindAllChildren()
        {
            return FindAllChildren(TimeSpan.FromSeconds(20));
        }
        public ISHAutomationElement[] FindAllChildren(TimeSpan timeout)
        {
            List<ISHAutomationElement> elements = new List<ISHAutomationElement>();
            Func<List<ISHAutomationElement>> function = () => FindAllChildrenBase().Where(x => x.FrameworkAutomationElement != null).ToList();
           
            GetElements(function,true,ref elements);
            if (!elements.Any() && timeout != TimeSpan.Zero)
            {
                Wait.UntilTrue(() => GetElements(function, true, ref elements), timeout);
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
            List<ISHAutomationElement> elements = new List<ISHAutomationElement>();
            Func<List<ISHAutomationElement>> function = () => FindAllChildrenBase(condition).Where(x => x.FrameworkAutomationElement != null).ToList();
           
            GetElements(function,waitUntilExists,ref elements);
            if (!elements.Any() && waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                Wait.UntilTrue(() => GetElements(function,true,ref elements), timeout);
            }
            else if (elements.Any() && !waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                Wait.UntilTrue(() => GetElements(function,false, ref elements), timeout);
            }
            if (elements != null && elements.Any())
            {
                return elements.ToArray();
            }
            return Array.Empty<SHAutomationElement>();
        }
    }
}
