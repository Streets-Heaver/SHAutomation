using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.UIA3.Converters;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class DragPattern : DragPatternBase<UIA.IUIAutomationDragPattern>
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_DragPatternId, "Drag", AutomationObjectIds.IsDragPatternAvailableProperty);
        public static readonly PropertyId DropEffectProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_DragDropEffectPropertyId, "DropEffect");
        public static readonly PropertyId DropEffectsProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_DragDropEffectsPropertyId, "DropEffects");
        public static readonly PropertyId IsGrabbedProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_DragIsGrabbedPropertyId, "IsGrabbed");
        public static readonly PropertyId GrabbedItemsProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_DragGrabbedItemsPropertyId, " GrabbedItems").SetConverter(SHAutomationElementConverter.NativeArrayToManaged);
        public static readonly EventId DragCancelEvent = EventId.Register( UIA.UIA_EventIds.UIA_Drag_DragCancelEventId, "DragCancel");
        public static readonly EventId DragCompleteEvent = EventId.Register( UIA.UIA_EventIds.UIA_Drag_DragCompleteEventId, "DragComplete");
        public static readonly EventId DragStartEvent = EventId.Register( UIA.UIA_EventIds.UIA_Drag_DragStartEventId, "DragStart");

        public DragPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationDragPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }
    }

    public class DragPatternPropertyIds : IDragPatternPropertyIds
    {
        public PropertyId DropEffect => DragPattern.DropEffectProperty;
        public PropertyId DropEffects => DragPattern.DropEffectsProperty;
        public PropertyId IsGrabbed => DragPattern.IsGrabbedProperty;
        public PropertyId GrabbedItems => DragPattern.GrabbedItemsProperty;
    }

    public class DragPatternEventIds : IDragPatternEventIds
    {
        public EventId DragCancelEvent => DragPattern.DragCancelEvent;
        public EventId DragCompleteEvent => DragPattern.DragCompleteEvent;
        public EventId DragStartEvent => DragPattern.DragStartEvent;
    }
}
