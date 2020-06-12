using System;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Definitions;

namespace SHAutomation.Core.EventHandlers
{
    /// <summary>
    /// ase event handler for text edit text changed event handlers.
    /// </summary>
    public abstract class TextEditTextChangedEventHandlerBase : ElementEventHandlerBase
    {
        private readonly Action<SHAutomationElement, TextEditChangeType, string[]> _callAction;

        protected TextEditTextChangedEventHandlerBase(FrameworkAutomationElementBase frameworkElement, Action<SHAutomationElement, TextEditChangeType, string[]> callAction) : base(frameworkElement)
        {
            _callAction = callAction;
        }

        protected void HandleTextEditTextChangedEvent(SHAutomationElement sender, TextEditChangeType textEditChangeType, string[] eventStrings)
        {
            _callAction(sender, textEditChangeType, eventStrings);
        }

        /// <inheritdoc />
        protected override void UnregisterEventHandler()
        {
            FrameworkElement.UnregisterTextEditTextChangedEventHandler(this);
        }
    }
}
