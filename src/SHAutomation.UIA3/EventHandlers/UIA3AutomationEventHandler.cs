using System;
using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.EventHandlers;
using SHAutomation.Core.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.EventHandlers
{
    public class UIA3AutomationEventHandler : AutomationEventHandlerBase, UIA.IUIAutomationEventHandler
    {
        public UIA3AutomationEventHandler(FrameworkAutomationElementBase frameworkElement, EventId @event, Action<SHAutomationElement, EventId> callAction) : base(frameworkElement, @event, callAction)
        {
        }

        public void HandleAutomationEvent(UIA.IUIAutomationElement sender, int eventId)
        {
            var frameworkElement = new UIA3FrameworkAutomationElement((UIA3Automation)Automation, sender);
            var senderElement = new SHAutomationElement(frameworkElement);
            var @event = EventId.Find(AutomationType.UIA3, eventId);
            HandleAutomationEvent(senderElement, @event);
        }
    }
}
