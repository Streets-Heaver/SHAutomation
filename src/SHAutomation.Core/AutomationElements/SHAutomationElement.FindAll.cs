using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        private SHAutomationElement[] FindAllBase(TreeScope treeScope, ConditionBase condition)
        {
            try
            {
                return FrameworkAutomationElement.FindAll(treeScope, condition);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return Array.Empty<SHAutomationElement>();

            }
        }
        public  ISHAutomationElement[] FindAll(TreeScope treeScope, ConditionBase condition, bool waitUntilExists = true)
        {
            return FindAll(treeScope, condition, TimeSpan.FromSeconds(20), waitUntilExists);
        }
        public ISHAutomationElement[] FindAll(TreeScope treeScope, ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true)
        {
            List<SHAutomationElement> elements = new List<SHAutomationElement>();
            bool getElements(bool shouldExist)
            {
                if (!elements.Any())
                {
                    elements = FindAllBase(treeScope, condition).Where(x => x.FrameworkAutomationElement != null).ToList();
                }
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
            return elements.ToArray();
        }
    }
}
