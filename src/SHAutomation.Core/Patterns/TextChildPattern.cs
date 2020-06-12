using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Patterns.Infrastructure;

namespace SHAutomation.Core.Patterns
{
    public interface ITextChildPattern : IPattern
    {
      SHAutomationElement TextContainer { get; }
        ITextRange TextRange { get; }
    }
}
