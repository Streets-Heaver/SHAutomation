using System;
using System.Linq;
using System.Text;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;

namespace SHAutomation.Core
{

    public static class XPathHelper
    {
        private static SHAutomationElement _callingRoot;
        /// <summary>
        /// Gets the XPath to the element until the desktop or the given root element.
        /// Warning: This is quite a heavy operation
        /// </summary>
        public static string GetXPathToElement(SHAutomationElement element, SHAutomationElement rootElement = null)
        {
            _callingRoot = rootElement;
            var treeWalker = element.Automation.TreeWalkerFactory.GetControlViewWalker();
            return GetXPathToElement(element, treeWalker, rootElement);
        }

        private static string GetXPathToElement(SHAutomationElement element, ITreeWalker treeWalker, SHAutomationElement rootElement = null)
        {
            var parent = treeWalker.GetParent(element);
            if (parent == null || (rootElement != null && parent.Equals(_callingRoot.Parent)))
            {
                _callingRoot = null;
                return string.Empty;
            }
            // Get the index
            var allChildren = parent.FindAllChildren(cf => cf.ByControlType(element.Properties.ControlType));
            var currentItemText = $"{element.Properties.ControlType.Value}";
            if (allChildren.Length > 1)
            {
                // There is more than one matching child, find out the index
                var indexInParent = 1; // Index starts with 1
                foreach (var child in allChildren)
                {
                    if (child.Equals(element))
                    {
                        break;
                    }
                    indexInParent++;
                }
                currentItemText += $"[{indexInParent}]";
            }
            return $"{GetXPathToElement(parent, treeWalker, rootElement)}/{currentItemText}";
        }


    }
}
