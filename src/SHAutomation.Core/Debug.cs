using System;
using System.Linq;
using System.Text;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;

namespace SHAutomation.Core
{
    /// <summary>
    /// Provides methods which can help in debugging.
    /// </summary>
    public static class Debug
    {
        private static SHAutomationElement _callingRoot;
        /// <summary>
        /// Gets the XPath to the element until the desktop or the given root element.
        /// Warning: This is quite a heavy operation
        /// </summary>
        public static string GetXPathToElement(SHAutomationElement element,SHAutomationElement rootElement = null)
        {
            _callingRoot = rootElement;
            var treeWalker = element.Automation.TreeWalkerFactory.GetControlViewWalker();
            return GetXPathToElement(element, treeWalker, rootElement);
        }

        private static string GetXPathToElement(SHAutomationElement element, ITreeWalker treeWalker,SHAutomationElement rootElement = null)
        {
            var parent = treeWalker.GetParent(element);
            if (parent == null || (rootElement != null && parent.Equals(_callingRoot.Parent)))
            {
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

        /// <summary>
        /// Prints out various details about the given element (including children).
        /// </summary>
        public static string Details(ISHAutomationElement sHAutomationElement)
        {
            try
            {
                var stringBuilder = new StringBuilder();
                var cr = new CacheRequest
                {
                  SHAutomationElementMode =SHAutomationElementMode.None
                };
                // Add the element properties
                cr.Add(sHAutomationElement.Automation.PropertyLibrary.Element.AutomationId);
                cr.Add(sHAutomationElement.Automation.PropertyLibrary.Element.ControlType);
                cr.Add(sHAutomationElement.Automation.PropertyLibrary.Element.Name);
                cr.Add(sHAutomationElement.Automation.PropertyLibrary.Element.HelpText);
                cr.Add(sHAutomationElement.Automation.PropertyLibrary.Element.BoundingRectangle);
                cr.Add(sHAutomationElement.Automation.PropertyLibrary.Element.ClassName);
                cr.Add(sHAutomationElement.Automation.PropertyLibrary.Element.IsOffscreen);
                cr.Add(sHAutomationElement.Automation.PropertyLibrary.Element.FrameworkId);
                cr.Add(sHAutomationElement.Automation.PropertyLibrary.Element.ProcessId);
                // Add the pattern availability properties
                sHAutomationElement.Automation.PropertyLibrary.PatternAvailability.AllForCurrentFramework.ToList().ForEach(x => cr.Add(x));
                cr.TreeScope = TreeScope.Subtree;
                cr.TreeFilter = TrueCondition.Default;
                // Activate the cache request
                using (cr.Activate())
                {
                    // Re-find the root element with caching activated
                    sHAutomationElement = sHAutomationElement.FindFirst(TreeScope.Element, TrueCondition.Default);
                    Details(stringBuilder, sHAutomationElement, string.Empty);
                }
                return stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to dump info: " + ex);
                return string.Empty;
            }
        }

        private static void Details(StringBuilder stringBuilder,ISHAutomationElement sHAutomationElement, string displayPadding)
        {
            const string indent = "    ";
            WriteDetail(sHAutomationElement, stringBuilder, displayPadding);
            WritePattern(sHAutomationElement, stringBuilder, displayPadding);
            var children = sHAutomationElement.CachedChildren;
            foreach (var child in children)
            {
                Details(stringBuilder, child, displayPadding + indent);
            }
        }

        private static void WriteDetail(ISHAutomationElement sHAutomationElement, StringBuilder stringBuilder, string displayPadding)
        {
            WriteWithPadding(stringBuilder, "AutomationId: " + sHAutomationElement.Properties.AutomationId, displayPadding);
            WriteWithPadding(stringBuilder, "ControlType: " + sHAutomationElement.Properties.ControlType, displayPadding);
            WriteWithPadding(stringBuilder, "Name: " + sHAutomationElement.Properties.Name, displayPadding);
            WriteWithPadding(stringBuilder, "HelpText: " + sHAutomationElement.Properties.HelpText, displayPadding);
            WriteWithPadding(stringBuilder, "Bounding rectangle: " + sHAutomationElement.Properties.BoundingRectangle, displayPadding);
            WriteWithPadding(stringBuilder, "ClassName: " + sHAutomationElement.Properties.ClassName, displayPadding);
            WriteWithPadding(stringBuilder, "IsOffScreen: " + sHAutomationElement.Properties.IsOffscreen, displayPadding);
            WriteWithPadding(stringBuilder, "FrameworkId: " + sHAutomationElement.Properties.FrameworkId, displayPadding);
            WriteWithPadding(stringBuilder, "ProcessId: " + sHAutomationElement.Properties.ProcessId, displayPadding);
        }

        private static void WritePattern(ISHAutomationElement sHAutomationElement, StringBuilder stringBuilder, string displayPadding)
        {
            var availablePatterns = sHAutomationElement.GetSupportedPatterns();
            foreach (var automationPattern in availablePatterns)
            {
                WriteWithPadding(stringBuilder, automationPattern.ToString(), displayPadding);
            }
            stringBuilder.AppendLine();
        }

        private static void WriteWithPadding(StringBuilder stringBuilder, string message, string padding)
        {
            stringBuilder.Append(padding).AppendLine(message);
        }
    }
}
