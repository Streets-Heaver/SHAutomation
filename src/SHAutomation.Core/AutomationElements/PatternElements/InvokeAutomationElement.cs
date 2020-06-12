using SHAutomation.Core.Patterns;

namespace SHAutomation.Core.AutomationElements.PatternElements
{
    public class InvokeAutomationElement :SHAutomationElement
    {
        public InvokeAutomationElement(FrameworkAutomationElementBase frameworkAutomationElement) : base(frameworkAutomationElement)
        {
        }

        public IInvokePattern InvokePattern => Patterns.Invoke.Pattern;

        public void Invoke()
        {
            InvokePattern.Invoke();
        }
    }
}
