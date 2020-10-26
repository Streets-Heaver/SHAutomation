using SHAutomation.Core.Definitions;
using SHAutomation.Core.EventHandlers;
using SHAutomation.Core.Identifiers;
using System;

namespace SHAutomation.Core.AutomationElements.Infrastructure
{
    /// <summary>
    /// Interface for methods to subscribe to events on an <see cref="AutomationElement"/>.
    /// </summary>
    public interface ISHAutomationElementEventSubscriber
    {
        /// <summary>
        /// Registers a active text position changed event.
        /// </summary>
        ActiveTextPositionChangedEventHandlerBase RegisterActiveTextPositionChangedEvent(TreeScope treeScope, Action<SHAutomationElement, ITextRange> action);

        /// <summary>
        /// Registers the given automation event.
        /// </summary>
        AutomationEventHandlerBase RegisterAutomationEvent(EventId @event, TreeScope treeScope, Action<SHAutomationElement, EventId> action);

        /// <summary>
        /// Registers a property changed event with the given property.
        /// </summary>
        PropertyChangedEventHandlerBase RegisterPropertyChangedEvent(TreeScope treeScope, Action<SHAutomationElement, PropertyId, object> action, PropertyId[] properties);

        /// <summary>
        /// Registers a structure changed event.
        /// </summary>
        StructureChangedEventHandlerBase RegisterStructureChangedEvent(TreeScope treeScope, Action<SHAutomationElement, StructureChangeType, int[]> action);

        /// <summary>
        /// Registers a notification event.
        /// </summary>
        NotificationEventHandlerBase RegisterNotificationEvent(TreeScope treeScope, Action<SHAutomationElement, NotificationKind, NotificationProcessing, string, string> action);

        /// <summary>
        /// Registers a text edit text changed event.
        /// </summary>
        TextEditTextChangedEventHandlerBase RegisterTextEditTextChangedEventHandler(TreeScope treeScope, TextEditChangeType textEditChangeType, Action<SHAutomationElement, TextEditChangeType, string[]> action);
    }
}
