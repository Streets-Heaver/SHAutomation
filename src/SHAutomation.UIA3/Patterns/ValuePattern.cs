using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.Core.Tools;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class ValuePattern : ValuePatternBase<UIA.IUIAutomationValuePattern>
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_ValuePatternId, "Value", AutomationObjectIds.IsValuePatternAvailableProperty);
        public static readonly PropertyId IsReadOnlyProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_ValueIsReadOnlyPropertyId, "IsReadOnly");
        public static readonly PropertyId ValueProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_ValueValuePropertyId, "Value");

        public ValuePattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationValuePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        /// <inheritdoc />
        public override void SetValue(string value)
        {
            Com.Call(() => NativePattern.SetValue(value));
        }
    }

    public class ValuePatternPropertyIds : IValuePatternPropertyIds
    {
        public PropertyId IsReadOnly => ValuePattern.IsReadOnlyProperty;

        public PropertyId Value => ValuePattern.ValueProperty;
    }
}
