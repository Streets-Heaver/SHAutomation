using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.Core.Patterns.Infrastructure;
using StreetsHeaver.SHAutomation.Core.Tools;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class ObjectModelPattern : PatternBase<UIA.IUIAutomationObjectModelPattern>, IObjectModelPattern
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_ObjectModelPatternId, "ObjectModel", AutomationObjectIds.IsObjectModelPatternAvailableProperty);

        public ObjectModelPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationObjectModelPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public object GetUnderlyingObjectModel()
        {
            return Com.Call(() => NativePattern.GetUnderlyingObjectModel());
        }
    }
}
