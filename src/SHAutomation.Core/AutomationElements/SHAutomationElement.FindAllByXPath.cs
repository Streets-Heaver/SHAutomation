using SHAutomation.Core.StaticClasses;
using System.Collections.Generic;
using System.Linq;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement
    {
        public ISHAutomationElement[] FindAllByXPath(string xPath)
        {
            return FindAllByXPath(xPath, timeout: 20000);
        }
        public ISHAutomationElement[] FindAllByXPath(string xPath, int timeout = 20000, bool waitUntilExists = true)
        {
            List<SHAutomationElement> elements = new List<SHAutomationElement>();
            bool getElements(bool shouldExist)
            {
                if (!elements.Any())
                    elements = FindAllByXPathBase(xPath).Where(x => x.FrameworkAutomationElement != null).ToList();
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
            return elements.ToArray();
        }
    }
}
