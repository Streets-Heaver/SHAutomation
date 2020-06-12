using SHAutomation.Core;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;
using SHAutomation.Core.Tools;
using SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Patterns
{
    public class MultipleViewPattern : MultipleViewPatternBase<UIA.IUIAutomationMultipleViewPattern>
    {
        public static readonly PatternId Pattern = PatternId.Register(AutomationType.UIA3, UIA.UIA_PatternIds.UIA_MultipleViewPatternId, "MultipleView", AutomationObjectIds.IsMultipleViewPatternAvailableProperty);
        public static readonly PropertyId CurrentViewProperty = PropertyId.Register(AutomationType.UIA3, UIA.UIA_PropertyIds.UIA_MultipleViewCurrentViewPropertyId, "CurrentView");
        public static readonly PropertyId SupportedViewsProperty = PropertyId.Register(AutomationType.UIA3, UIA.UIA_PropertyIds.UIA_MultipleViewSupportedViewsPropertyId, "SupportedViews");

        public MultipleViewPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationMultipleViewPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public override string GetViewName(int view)
        {
            return Com.Call(() => NativePattern.GetViewName(view));
        }

        public override void SetCurrentView(int view)
        {
            Com.Call(() => NativePattern.SetCurrentView(view));
        }
    }

    public class MultipleViewPatternPropertyIds : IMultipleViewPatternPropertyIds
    {
        public PropertyId CurrentView => MultipleViewPattern.CurrentViewProperty;
        public PropertyId SupportedViews => MultipleViewPattern.SupportedViewsProperty;
    }
}
