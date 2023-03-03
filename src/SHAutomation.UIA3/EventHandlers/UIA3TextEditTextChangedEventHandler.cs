using System;
using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.Definitions;
using StreetsHeaver.SHAutomation.Core.EventHandlers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.EventHandlers
{
    /// <summary>
    /// UIA3 implementation of a text edit text changed event handler.
    /// </summary>
    public class UIA3TextEditTextChangedEventHandler : TextEditTextChangedEventHandlerBase, UIA.IUIAutomationTextEditTextChangedEventHandler
    {
        public UIA3TextEditTextChangedEventHandler(FrameworkAutomationElementBase frameworkElement, Action<SHAutomationElement, TextEditChangeType, string[]> callAction) : base(frameworkElement, callAction)
        {
        }

        public void HandleTextEditTextChangedEvent(UIA.IUIAutomationElement sender, UIA.TextEditChangeType textEditChangeType, string[] eventstrings)
        {
            var frameworkElement = new UIA3FrameworkAutomationElement((UIA3Automation)Automation, sender);
            var senderElement = new SHAutomationElement(frameworkElement);
            HandleTextEditTextChangedEvent(senderElement, (TextEditChangeType)textEditChangeType, eventstrings);
        }

    }
}
