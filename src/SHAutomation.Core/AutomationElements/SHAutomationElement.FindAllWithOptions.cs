﻿using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        public ISHAutomationElement[] FindAllWithOptions(TreeScope treeScope, ConditionBase condition, TreeTraversalOption traversalOptions,SHAutomationElement root)
        {
            return FindAllWithOptions(treeScope, condition, traversalOptions, root, TimeSpan.FromSeconds(20));
        }
        public ISHAutomationElement[] FindAllWithOptions(TreeScope treeScope, ConditionBase condition, TreeTraversalOption traversalOptions,SHAutomationElement root, TimeSpan timeout, bool waitUntilExists = true)
        {
            List<SHAutomationElement> elements = new List<SHAutomationElement>();
            bool getElements(bool shouldExist)
            {
                if (!elements.Any())
                    elements = FindAllWithOptionsBase(treeScope, condition, traversalOptions, root).Where(x => x.FrameworkAutomationElement != null).ToList();
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
