using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;
using SHAutomation.Core.Patterns.Infrastructure;
using SHAutomation.Core.Tools;
using SHAutomation.UIA3.Converters;
using SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Patterns
{
    public class ItemContainerPattern : PatternBase<UIA.IUIAutomationItemContainerPattern>, IItemContainerPattern
    {
        public static readonly PatternId Pattern = PatternId.Register(AutomationType.UIA3, UIA.UIA_PatternIds.UIA_ItemContainerPatternId, "ItemContainer", AutomationObjectIds.IsItemContainerPatternAvailableProperty);

        public ItemContainerPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationItemContainerPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public SHAutomationElement FindItemByProperty(SHAutomationElement startAfter, PropertyId property, object value)
        {
            var foundNativeElement = Com.Call(() =>
                NativePattern.FindItemByProperty(
                    SHAutomationElementConverter.ToNative(startAfter),
                    property?.Id ?? 0, ValueConverter.ToNative(value)));
            return SHAutomationElementConverter.NativeToManaged((UIA3Automation)FrameworkAutomationElement.Automation, foundNativeElement);
        }
    }
}
