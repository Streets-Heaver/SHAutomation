using System;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Definitions;

namespace SHAutomation.Core.EventHandlers
{
    /// <summary>
    /// Base event handler for structure changed event handlers.
    /// </summary>
    public abstract class StructureChangedEventHandlerBase : ElementEventHandlerBase
    {
        private readonly Action<SHAutomationElement, StructureChangeType, int[]> _callAction;

        protected StructureChangedEventHandlerBase(FrameworkAutomationElementBase frameworkElement, Action<SHAutomationElement, StructureChangeType, int[]> callAction)
            : base(frameworkElement)
        {
            _callAction = callAction;
        }

        protected void HandleStructureChangedEvent(SHAutomationElement sender, StructureChangeType changeType, int[] runtimeId)
        {
            _callAction(sender, changeType, runtimeId);
        }

        /// <inheritdoc />
        protected override void UnregisterEventHandler()
        {
            FrameworkElement.UnregisterStructureChangedEventHandler(this);
        }
    }
}
