using System;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Identifiers;

namespace SHAutomation.Core.EventHandlers
{
    /// <summary>
    /// Base event handler for property changed event handlers.
    /// </summary>
    public abstract class PropertyChangedEventHandlerBase : ElementEventHandlerBase
    {
        private readonly Action<SHAutomationElement, PropertyId, object> _callAction;

        protected PropertyChangedEventHandlerBase(FrameworkAutomationElementBase frameworkElement, Action<SHAutomationElement, PropertyId, object> callAction)
            : base(frameworkElement)
        {
            _callAction = callAction;
        }

        protected void HandlePropertyChangedEvent(SHAutomationElement sender, PropertyId propertyId, object newValue)
        {
            _callAction(sender, propertyId, newValue);
        }

        /// <inheritdoc />
        protected override void UnregisterEventHandler()
        {
            FrameworkElement.UnregisterPropertyChangedEventHandler(this);
        }
    }
}
