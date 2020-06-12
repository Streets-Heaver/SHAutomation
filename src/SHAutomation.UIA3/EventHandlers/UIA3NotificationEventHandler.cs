using System;
using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.EventHandlers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.EventHandlers
{
    public class UIA3NotificationEventHandler : NotificationEventHandlerBase, UIA.IUIAutomationNotificationEventHandler
    {
        public UIA3NotificationEventHandler(FrameworkAutomationElementBase frameworkElement, Action<SHAutomationElement, NotificationKind, NotificationProcessing, string, string> callAction) : base(frameworkElement, callAction)
        {
        }

        public void HandleNotificationEvent(UIA.IUIAutomationElement sender, UIA.NotificationKind notificationKind,
            UIA.NotificationProcessing notificationProcessing, string displayString, string activityId)
        {
            var frameworkElement = new UIA3FrameworkAutomationElement((UIA3Automation)Automation, sender);
            var senderElement = new SHAutomationElement(frameworkElement);
            HandleNotificationEvent(senderElement, (NotificationKind)notificationKind, (NotificationProcessing)notificationProcessing, displayString, activityId);
        }
    }
}
