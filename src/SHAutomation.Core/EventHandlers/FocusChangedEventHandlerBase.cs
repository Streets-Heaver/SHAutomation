using System;
using SHAutomation.Core.AutomationElements;

namespace SHAutomation.Core.EventHandlers
{
    /// <summary>
    /// Base event handler for focus changed event handlers.
    /// </summary>
    public abstract class FocusChangedEventHandlerBase : EventHandlerBase
    {
        private readonly Action<SHAutomationElement> _callAction;

        protected FocusChangedEventHandlerBase(AutomationBase automation, Action<SHAutomationElement> callAction)
            : base(automation)
        {
            _callAction = callAction;
        }

        protected void HandleFocusChangedEvent(SHAutomationElement sender)
        {
            _callAction(sender);
        }

        /// <inheritdoc />
        protected override void UnregisterEventHandler()
        {
            Automation.UnregisterFocusChangedEvent(this);
        }
    }
}
