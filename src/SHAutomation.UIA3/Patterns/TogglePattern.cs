using SHAutomation.Core;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;
using SHAutomation.Core.Tools;
using SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Patterns
{
    public class TogglePattern : TogglePatternBase<UIA.IUIAutomationTogglePattern>
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_TogglePatternId, "Toggle", AutomationObjectIds.IsTogglePatternAvailableProperty);
        public static readonly PropertyId ToggleStateProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_ToggleToggleStatePropertyId, "ToggleState");

        public TogglePattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationTogglePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public override void Toggle()
        {
            Com.Call(() => NativePattern.Toggle());
        }
    }

    public class TogglePatternPropertyIds : ITogglePatternPropertyIds
    {
        public PropertyId ToggleState => TogglePattern.ToggleStateProperty;
    }
}
