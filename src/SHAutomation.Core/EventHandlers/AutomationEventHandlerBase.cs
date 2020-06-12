using System;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Identifiers;

namespace SHAutomation.Core.EventHandlers
{
    public abstract class AutomationEventHandlerBase : ElementEventHandlerBase
    {
        private readonly Action<SHAutomationElement, EventId> _callAction;

        protected AutomationEventHandlerBase(FrameworkAutomationElementBase frameworkElement, EventId @event, Action<SHAutomationElement, EventId> callAction)
            : base(frameworkElement)
        {
            Event = @event;
            _callAction = callAction;
        }

        public EventId Event { get; }

        protected void HandleAutomationEvent(SHAutomationElement sender, EventId eventId)
        {
            _callAction(sender, eventId);
        }

        /// <inheritdoc />
        protected override void UnregisterEventHandler()
        {
            FrameworkElement.UnregisterAutomationEventHandler(this);
        }
    }
}
