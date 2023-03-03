using StreetsHeaver.SHAutomation.Core.AutomationElements;

namespace StreetsHeaver.SHAutomation.Core.Patterns
{
    public interface IText2Pattern : ITextPattern
    {
        ITextRange GetCaretRange(out bool isActive);
        ITextRange RangeFromAnnotation(SHAutomationElement annotation);
    }
}
