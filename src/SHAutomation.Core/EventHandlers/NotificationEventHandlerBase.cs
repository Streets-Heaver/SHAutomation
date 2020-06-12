using System;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Definitions;

namespace SHAutomation.Core.EventHandlers
{
    public abstract class NotificationEventHandlerBase : ElementEventHandlerBase
    {
        private readonly Action<SHAutomationElement, NotificationKind, NotificationProcessing, string, string> _callAction;

        protected NotificationEventHandlerBase(FrameworkAutomationElementBase frameworkElement, Action<SHAutomationElement, NotificationKind, NotificationProcessing, string, string> callAction) : base(frameworkElement)
        {
            _callAction = callAction;
        }

        protected void HandleNotificationEvent(SHAutomationElement sender, NotificationKind notificationKind,
            NotificationProcessing notificationProcessing, string displayString, string activityId)
        {
            _callAction(sender, notificationKind, notificationProcessing, displayString, activityId);
        }

        /// <inheritdoc />
        protected override void UnregisterEventHandler()
        {
            FrameworkElement.UnregisterNotificationEventHandler(this);
        }
    }
}
