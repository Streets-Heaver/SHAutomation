using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;
using SHAutomation.Core.Tools;
using SHAutomation.UIA3.Converters;
using SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Patterns
{
    public class Text2Pattern : TextPattern, IText2Pattern
    {
        public new static readonly PatternId Pattern = PatternId.Register(AutomationType.UIA3, UIA.UIA_PatternIds.UIA_TextPattern2Id, "Text2", AutomationObjectIds.IsTextPattern2AvailableProperty);

        public Text2Pattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationTextPattern2 nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
            ExtendedNativePattern = nativePattern;
        }

        public UIA.IUIAutomationTextPattern2 ExtendedNativePattern { get; }

        public ITextRange GetCaretRange(out bool isActive)
        {
            var rawIsActive = 0;
            var nativeTextRange = Com.Call(() => ExtendedNativePattern.GetCaretRange(out rawIsActive));
            isActive = rawIsActive != 0;
            return TextRangeConverter.NativeToManaged((UIA3Automation)FrameworkAutomationElement.Automation, nativeTextRange);
        }

        public ITextRange RangeFromAnnotation(SHAutomationElement annotation)
        {
            var nativeInputElement = SHAutomationElementConverter.ToNative(annotation);
            var nativeElement = Com.Call(() => ExtendedNativePattern.RangeFromAnnotation(nativeInputElement));
            return TextRangeConverter.NativeToManaged((UIA3Automation)FrameworkAutomationElement.Automation, nativeElement);
        }
    }
}
