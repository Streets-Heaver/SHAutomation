using System;
using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.Definitions;

namespace StreetsHeaver.SHAutomation.Core.EventHandlers
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

        protected void HandleTextEditTextChangedEvent(SHAutomationElement sender, TextEditChangeType textEditChangeType, string[] eventstrings)
        {
            _callAction(sender, textEditChangeType, eventstrings);
        }

        /// <inheritdoc />
        protected override void UnregisterEventHandler()
        {
            FrameworkElement.UnregisterTextEditTextChangedEventHandler(this);
        }
    }
}
