using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.Core.Patterns.Infrastructure;
using StreetsHeaver.SHAutomation.Core.Tools;
using StreetsHeaver.SHAutomation.UIA3.Converters;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class ItemContainerPattern : PatternBase<UIA.IUIAutomationItemContainerPattern>, IItemContainerPattern
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_ItemContainerPatternId, "ItemContainer", AutomationObjectIds.IsItemContainerPatternAvailableProperty);

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
