using SHAutomation.Core;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;
using SHAutomation.Core.Tools;
using SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Patterns
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
