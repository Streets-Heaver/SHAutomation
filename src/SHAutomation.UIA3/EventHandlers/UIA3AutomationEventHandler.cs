using System;
using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.EventHandlers;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.EventHandlers
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
            var @event = EventId.Find( eventId);
            HandleAutomationEvent(senderElement, @event);
        }
    }
}
