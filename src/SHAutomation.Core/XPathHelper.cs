using SHAutomation.Core.AutomationElements;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SHAutomation.Core
{

    public static class XPathHelper
    {
        /// <summary>
        /// Gets the XPath to the element until the desktop or the given root element.
        /// Warning: This is quite a heavy operation
        /// </summary>
        public static string GetXPathToElement(SHAutomationElement element, SHAutomationElement rootElement = null)
        {
            var treeWalker = element.Automation.TreeWalkerFactory.GetControlViewWalker();
            return GetXPathToElement(element, treeWalker, rootElement);
        }

        private static string GetXPathToElement(SHAutomationElement element, ITreeWalker treeWalker, SHAutomationElement rootElement = null)
        {
            try
            {
                var allElements = new List<SHAutomationElement>();
                var parent = element;
                while (parent != null && (rootElement == null || !parent.Equals(rootElement)))
                {
                    var parentElement = treeWalker.GetParent(parent);
                    if (parentElement == null || (rootElement != null && parentElement.Equals(rootElement)))
                    {
                        break;
                    }
                    allElements.Add(parentElement);
                    parent = parentElement;
                }
                allElements.Reverse();

                var xpath = "";
                foreach (var item in allElements)
                {
                    var controlType = item.Properties.ControlType.Value;
                    var indexInParent = 1;
                    if (item.Parent != null)
                    {
                        var siblings = item.Parent.FindAllChildren(cf => cf.ByControlType(controlType));
                        if (siblings.Length > 1)
                        {
                            for (int i = 0; i < siblings.Length; i++)
                            {
                                if (siblings[i].Equals(item))
                                {
                                    indexInParent = i + 1;
                                    break;
                                }
                            }
                        }
                    }
                    xpath += $"/{controlType}[{indexInParent}]";
                }
                return xpath;
            }
            catch (COMException)
            {
                return null;
            }
        }

    }
}
