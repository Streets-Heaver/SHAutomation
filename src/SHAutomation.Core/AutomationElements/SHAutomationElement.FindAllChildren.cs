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
        public ISHAutomationElement[] FindAllChildren()
        {
            return FindAllChildren(20000);
        }
        public ISHAutomationElement[] FindAllChildren(int timeout = 20000)
        {
            List<SHAutomationElement> elements = new List<SHAutomationElement>();
            bool getElements()
            {
                if(!elements.Any())
                elements = FindAllChildrenBase().Where(x => x.FrameworkAutomationElement != null).ToList();
                return elements.Any();
            }
            getElements();
            if (!elements.Any() && timeout != 0)
            {
                SHSpinWait.SpinUntil(() => getElements(), timeout);
            }

            if (elements != null && elements.Any())
            {
                return elements.ToArray();
            }

            return Array.Empty<SHAutomationElement>();
        }
        public ISHAutomationElement[] FindAllChildren(ConditionBase condition)
        {
            return FindAllChildren(condition, 20000);
        }
        public ISHAutomationElement[] FindAllChildren(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            return FindAllChildren(conditionFunc, 20000);
        }
        public ISHAutomationElement[] FindAllChildren(Func<ConditionFactory, ConditionBase> conditionFunc, int timeout = 20000, bool waitUntilExists = true)
        {
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindAllChildren(condition, timeout, waitUntilExists);
        }

        public ISHAutomationElement[] FindAllChildren(ConditionBase condition, int timeout = 20000, bool waitUntilExists = true)
        {
            List<SHAutomationElement> elements = new List<SHAutomationElement>();
            bool getElements(bool shouldExist)
            {
                if (!elements.Any())
                    elements = FindAllChildrenBase(condition).Where(x => x.FrameworkAutomationElement != null).ToList();
                return shouldExist ? elements.Any() : !elements.Any();
            }
            getElements(waitUntilExists);
            if (!elements.Any() && waitUntilExists && timeout > 0)
            {
                SHSpinWait.SpinUntil(() => getElements(true), timeout);
            }
            else if (elements.Any() && !waitUntilExists && timeout > 0)
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
