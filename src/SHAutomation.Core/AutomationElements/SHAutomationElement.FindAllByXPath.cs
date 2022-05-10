using SHAutomation.Core.StaticClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
       
        public ISHAutomationElement[] FindAllByXPath(string xPath, bool waitUntilExists = true)
        {
            return FindAllByXPath(xPath, TimeSpan.FromSeconds(20), waitUntilExists);
        }

        public ISHAutomationElement[] FindAllByXPath(string xPath, TimeSpan timeout, bool waitUntilExists = true)
        {
            List<ISHAutomationElement> elements = new List<ISHAutomationElement>();
            Func<List<ISHAutomationElement>> function = () => FindAllByXPathBase(xPath).Where(x => x.FrameworkAutomationElement != null).ToList();
           
            GetElements(function,waitUntilExists, ref elements);
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
