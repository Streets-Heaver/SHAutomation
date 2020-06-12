using SHAutomation.Core;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;
using SHAutomation.Core.Patterns.Infrastructure;
using SHAutomation.Core.Tools;
using SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Patterns
{
    public class VirtualizedItemPattern : PatternBase<UIA.IUIAutomationVirtualizedItemPattern>, IVirtualizedItemPattern
    {
        public static readonly PatternId Pattern = PatternId.Register(AutomationType.UIA3, UIA.UIA_PatternIds.UIA_VirtualizedItemPatternId, "VirtualizedItem", AutomationObjectIds.IsVirtualizedItemPatternAvailableProperty);

        public VirtualizedItemPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationVirtualizedItemPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public void Realize()
        {
            Com.Call(() => NativePattern.Realize());
        }
    }
}
