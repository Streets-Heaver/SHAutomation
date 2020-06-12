using SHAutomation.Core;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;
using SHAutomation.UIA3.Converters;
using SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Patterns
{
    public class SelectionPattern : SelectionPatternBase<UIA.IUIAutomationSelectionPattern>
    {
        public static readonly PatternId Pattern = PatternId.Register(AutomationType.UIA3, UIA.UIA_PatternIds.UIA_SelectionPatternId, "Selection", AutomationObjectIds.IsSelectionPatternAvailableProperty);
        public static readonly PropertyId CanSelectMultipleProperty = PropertyId.Register(AutomationType.UIA3, UIA.UIA_PropertyIds.UIA_SelectionCanSelectMultiplePropertyId, "CanSelectMultiple");
        public static readonly PropertyId IsSelectionRequiredProperty = PropertyId.Register(AutomationType.UIA3, UIA.UIA_PropertyIds.UIA_SelectionIsSelectionRequiredPropertyId, "IsSelectionRequired");
        public static readonly PropertyId SelectionProperty = PropertyId.Register(AutomationType.UIA3, UIA.UIA_PropertyIds.UIA_SelectionSelectionPropertyId, "Selection").SetConverter(SHAutomationElementConverter.NativeArrayToManaged);
        public static readonly EventId InvalidatedEvent = EventId.Register(AutomationType.UIA3, UIA.UIA_EventIds.UIA_Selection_InvalidatedEventId, "Invalidated");

        public SelectionPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationSelectionPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }
    }

    public class SelectionPatternPropertyIds : ISelectionPatternPropertyIds
    {
        public PropertyId CanSelectMultiple => SelectionPattern.CanSelectMultipleProperty;
        public PropertyId IsSelectionRequired => SelectionPattern.IsSelectionRequiredProperty;
        public PropertyId Selection => SelectionPattern.SelectionProperty;
    }

    public class SelectionPatternEventIds : ISelectionPatternEventIds
    {
        public EventId InvalidatedEvent => SelectionPattern.InvalidatedEvent;
    }
}
