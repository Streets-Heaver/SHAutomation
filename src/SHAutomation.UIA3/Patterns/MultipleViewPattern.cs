using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.Core.Tools;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class MultipleViewPattern : MultipleViewPatternBase<UIA.IUIAutomationMultipleViewPattern>
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_MultipleViewPatternId, "MultipleView", AutomationObjectIds.IsMultipleViewPatternAvailableProperty);
        public static readonly PropertyId CurrentViewProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_MultipleViewCurrentViewPropertyId, "CurrentView");
        public static readonly PropertyId SupportedViewsProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_MultipleViewSupportedViewsPropertyId, "SupportedViews");

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
