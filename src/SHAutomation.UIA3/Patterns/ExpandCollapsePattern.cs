using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.Core.Tools;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class ExpandCollapsePattern : ExpandCollapsePatternBase<UIA.IUIAutomationExpandCollapsePattern>
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_ExpandCollapsePatternId, "ExpandCollapse", AutomationObjectIds.IsExpandCollapsePatternAvailableProperty);
        public static readonly PropertyId ExpandCollapseStateProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_ExpandCollapseExpandCollapseStatePropertyId, "ExpandCollapseState");

        public ExpandCollapsePattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationExpandCollapsePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public override void Collapse()
        {
            Com.Call(() => NativePattern.Collapse());
        }

        public override void Expand()
        {
            Com.Call(() => NativePattern.Expand());
        }
    }

    public class ExpandCollapsePatternPropertyIds : IExpandCollapsePatternPropertyIds
    {
        public PropertyId ExpandCollapseState => ExpandCollapsePattern.ExpandCollapseStateProperty;
    }
}
