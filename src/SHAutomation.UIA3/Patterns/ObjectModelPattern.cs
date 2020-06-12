using SHAutomation.Core;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;
using SHAutomation.Core.Patterns.Infrastructure;
using SHAutomation.Core.Tools;
using SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Patterns
{
    public class ObjectModelPattern : PatternBase<UIA.IUIAutomationObjectModelPattern>, IObjectModelPattern
    {
        public static readonly PatternId Pattern = PatternId.Register(AutomationType.UIA3, UIA.UIA_PatternIds.UIA_ObjectModelPatternId, "ObjectModel", AutomationObjectIds.IsObjectModelPatternAvailableProperty);

        public ObjectModelPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationObjectModelPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public object GetUnderlyingObjectModel()
        {
            return Com.Call(() => NativePattern.GetUnderlyingObjectModel());
        }
    }
}
