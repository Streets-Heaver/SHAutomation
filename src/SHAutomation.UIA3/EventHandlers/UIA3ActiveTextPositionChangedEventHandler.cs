using System;
using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.EventHandlers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.EventHandlers
{
    public class UIA3ActiveTextPositionChangedEventHandler : ActiveTextPositionChangedEventHandlerBase, UIA.IUIAutomationActiveTextPositionChangedEventHandler
    {
        public UIA3ActiveTextPositionChangedEventHandler(FrameworkAutomationElementBase frameworkElement, Action<SHAutomationElement, ITextRange> callAction) : base(frameworkElement, callAction)
        {
        }

        public void HandleActiveTextPositionChangedEvent(UIA.IUIAutomationElement sender, UIA.IUIAutomationTextRange range)
        {
            var frameworkElement = new UIA3FrameworkAutomationElement((UIA3Automation)Automation, sender);
            var senderElement = new SHAutomationElement(frameworkElement);
            var rangeElement = new UIA3TextRange((UIA3Automation)Automation, range);
            HandleActiveTextPositionChangedEvent(senderElement, rangeElement);
        }
    }
}
