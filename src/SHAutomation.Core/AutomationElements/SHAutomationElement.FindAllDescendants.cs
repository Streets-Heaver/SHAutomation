﻿using SHAutomation.Core.Conditions;
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
        public  ISHAutomationElement[] FindAllDescendants()
        {
            return FindAllDescendants(20000);
        }
        public ISHAutomationElement[] FindAllDescendants(int timeout = 20000)
        {
            List<SHAutomationElement> elements = new List<SHAutomationElement>();

            bool getElements()
            {
                if (!elements.Any())
                    elements = FindAllDescendantsBase().Where(x => x.FrameworkAutomationElement != null).ToList();
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

            return new SHAutomationElement[0];
        }
        public  ISHAutomationElement[] FindAllDescendants(ConditionBase condition)
        {
            return FindAllDescendants(condition, 20000);
        }
        public  ISHAutomationElement[] FindAllDescendants(Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            return FindAllDescendants(conditionFunc, 20000);
        }
        public ISHAutomationElement[] FindAllDescendants(Func<ConditionFactory, ConditionBase> conditionFunc, int timeout = 20000, bool waitUntilExists = true)
        {
            var condition = conditionFunc(new ConditionFactory(Automation.PropertyLibrary));
            return FindAllDescendants(condition, timeout, waitUntilExists);
        }

        public ISHAutomationElement[] FindAllDescendants(ConditionBase condition, int timeout = 20000, bool waitUntilExists = true)
        {
            List<SHAutomationElement> elements = new List<SHAutomationElement>();
            bool getElements(bool shouldExist)
            {
                if (!elements.Any())
                    elements = FindAllDescendantsBase(condition).Where(x => x.FrameworkAutomationElement != null).ToList();
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
            return new SHAutomationElement[0];
        }
    }
}
