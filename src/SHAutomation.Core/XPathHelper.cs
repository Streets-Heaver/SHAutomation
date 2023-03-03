using StreetsHeaver.SHAutomation.Core.AutomationElements;
using System.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;

namespace StreetsHeaver.SHAutomation.Core
{

    public static class XPathHelper
    {
        private static readonly ConcurrentDictionary<Tuple<SHAutomationElement, SHAutomationElement>, string> _xpathCache = new ConcurrentDictionary<Tuple<SHAutomationElement, SHAutomationElement>, string>();
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
            var cacheKey = Tuple.Create(element, rootElement);
            if (_xpathCache.TryGetValue(cacheKey, out string xpath))
            {
                return xpath;
            }

            try
            {
                var parent = treeWalker.GetParent(element);
                if (parent == null || (rootElement != null && parent.Equals(_callingRoot.Parent)))
                {
                    _callingRoot = null;
                    return string.Empty;
                }

                // Get the index
                var allChildren = parent.FindAllChildren(cf => cf.ByControlType(element.Properties.ControlType));
                var indexInParent = allChildren.ToList().IndexOf(element) + 1; // Index starts with 1
                var currentItemText = $"{element.Properties.ControlType.Value}[{indexInParent}]";

                xpath = $"{GetXPathToElement(parent, treeWalker, rootElement)}/{currentItemText}";
                _xpathCache.TryAdd(cacheKey, xpath);
                return xpath;
            }
            catch (COMException)
            {
                return null;
            }
        }

    }
}
