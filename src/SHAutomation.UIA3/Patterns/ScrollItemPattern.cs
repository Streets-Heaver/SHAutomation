using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.Core.Patterns.Infrastructure;
using StreetsHeaver.SHAutomation.Core.Tools;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class ScrollItemPattern : PatternBase<UIA.IUIAutomationScrollItemPattern>, IScrollItemPattern
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_ScrollItemPatternId, "ScrollItem", AutomationObjectIds.IsScrollItemPatternAvailableProperty);

        public ScrollItemPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationScrollItemPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public void ScrollIntoView()
        {
            Com.Call(() => NativePattern.ScrollIntoView());
        }
    }
}
