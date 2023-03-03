using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class DropTargetPattern : DropTargetPatternBase<UIA.IUIAutomationDropTargetPattern>
    {
        public static readonly PatternId Pattern = PatternId.Register(UIA.UIA_PatternIds.UIA_DropTargetPatternId, "DropTarget", AutomationObjectIds.IsDropTargetPatternAvailableProperty);
        public static readonly PropertyId DropTargetEffectProperty = PropertyId.Register(UIA.UIA_PropertyIds.UIA_DropTargetDropTargetEffectPropertyId, "DropTargetEffect");
        public static readonly PropertyId DropTargetEffectsProperty = PropertyId.Register(UIA.UIA_PropertyIds.UIA_DropTargetDropTargetEffectsPropertyId, "DropTargetEffects");
        public static readonly EventId DragEnterEvent = EventId.Register(UIA.UIA_EventIds.UIA_DropTarget_DragEnterEventId, "DragEnter");
        public static readonly EventId DragLeaveEvent = EventId.Register(UIA.UIA_EventIds.UIA_DropTarget_DragLeaveEventId, "DragLeave");
        public static readonly EventId DragCompleteEvent = EventId.Register(UIA.UIA_EventIds.UIA_Drag_DragCompleteEventId, "DragComplete");

        public DropTargetPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationDropTargetPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }
    }

    public class DropTargetPatternPropertyIds : IDropTargetPatternPropertyIds
    {
        public PropertyId DropTargetEffect => DropTargetPattern.DropTargetEffectProperty;
        public PropertyId DropTargetEffects => DropTargetPattern.DropTargetEffectsProperty;
    }

    public class DropTargetPatternEventIds : IDropTargetPatternEventIds
    {
        public EventId DragEnterEvent => DropTargetPattern.DragEnterEvent;
        public EventId DragLeaveEvent => DropTargetPattern.DragLeaveEvent;
        public EventId DragCompleteEvent => DropTargetPattern.DragCompleteEvent;
    }
}
