using SHAutomation.Core.AutomationElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace SHAutomation.Core.EventHandlers
{
    public abstract class ActiveTextPositionChangedEventHandlerBase : ElementEventHandlerBase
    {
        private readonly Action<SHAutomationElement, ITextRange> _callAction;

        protected ActiveTextPositionChangedEventHandlerBase(FrameworkAutomationElementBase frameworkElement, Action<SHAutomationElement, ITextRange> callAction)
            : base(frameworkElement)
        {
            _callAction = callAction;
        }

        protected void HandleActiveTextPositionChangedEvent(SHAutomationElement sender, ITextRange range)
        {
            _callAction(sender, range);
        }

        /// <inheritdoc />
        protected override void UnregisterEventHandler()
        {
            FrameworkElement.UnregisterActiveTextPositionChangedEventHandler(this);
        }
    }
}
