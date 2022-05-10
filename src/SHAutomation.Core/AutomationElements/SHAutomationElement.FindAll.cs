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
        private ISHAutomationElement[] FindAllBase(TreeScope treeScope, ConditionBase condition)
        {
            try
            {
                return FrameworkAutomationElement.FindAll(treeScope, condition);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                return Array.Empty<ISHAutomationElement>();

            }
        }
        public  ISHAutomationElement[] FindAll(TreeScope treeScope, ConditionBase condition, bool waitUntilExists = true)
        {
            return FindAll(treeScope, condition, TimeSpan.FromSeconds(20), waitUntilExists);
        }
        public ISHAutomationElement[] FindAll(TreeScope treeScope, ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true)
        {
            List<ISHAutomationElement> elements = new List<ISHAutomationElement>();
            Func<List<ISHAutomationElement>> function = () => FindAllBase(treeScope, condition).Where(x => x.FrameworkAutomationElement != null).ToList();
         
            GetElements(function,waitUntilExists,ref elements);
            if (!elements.Any() && waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                Wait.UntilTrue(() => GetElements(function,true, ref elements), timeout);
            }
            else if (elements.Any() && !waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                Wait.UntilTrue(() => GetElements(function,false, ref elements), timeout);
            }
            return elements.ToArray();
        }
    }
}
