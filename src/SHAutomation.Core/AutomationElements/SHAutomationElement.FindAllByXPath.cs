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
            List<SHAutomationElement> elements = new List<SHAutomationElement>();
            bool getElements(bool shouldExist, bool firstRun)
            {
                if (firstRun || elements.Any() != shouldExist)
                    elements = FindAllByXPathBase(xPath).Where(x => x.FrameworkAutomationElement != null).ToList();
                return shouldExist ? elements.Any() : !elements.Any();
            }
            getElements(waitUntilExists,true);
            if (!elements.Any() && waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                SHSpinWait.SpinUntil(() => getElements(true,false), timeout);
            }
            else if (elements.Any() && !waitUntilExists && timeout.TotalMilliseconds > 0)
            {
                SHSpinWait.SpinUntil(() => getElements(false,false), timeout);
            }
            return elements.ToArray();
        }
    }
}
