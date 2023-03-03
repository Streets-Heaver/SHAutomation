using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.Patterns.Infrastructure;

namespace StreetsHeaver.SHAutomation.Core.Patterns
{
    public interface ITextChildPattern : IPattern
    {
      SHAutomationElement TextContainer { get; }
        ITextRange TextRange { get; }
    }
}
