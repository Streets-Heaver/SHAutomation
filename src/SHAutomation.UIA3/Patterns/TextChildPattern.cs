using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.Core.Patterns.Infrastructure;
using StreetsHeaver.SHAutomation.Core.Tools;
using StreetsHeaver.SHAutomation.UIA3.Converters;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class TextChildPattern : PatternBase<UIA.IUIAutomationTextChildPattern>, ITextChildPattern
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_TextChildPatternId, "TextChild", AutomationObjectIds.IsTextChildPatternAvailableProperty);

        public TextChildPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationTextChildPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public SHAutomationElement TextContainer
        {
            get
            {
                var nativeElement = Com.Call(() => NativePattern.TextContainer);
                return SHAutomationElementConverter.NativeToManaged((UIA3Automation)FrameworkAutomationElement.Automation, nativeElement);
            }
        }

        public ITextRange TextRange
        {
            get
            {
                var nativeRange = Com.Call(() => NativePattern.TextRange);
                return TextRangeConverter.NativeToManaged((UIA3Automation)FrameworkAutomationElement.Automation, nativeRange);
            }
        }
    }
}
