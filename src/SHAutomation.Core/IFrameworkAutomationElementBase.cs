using System;
using System.Drawing;
using System.Globalization;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.AutomationElements.Infrastructure;
using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.EventHandlers;
using SHAutomation.Core.Identifiers;

namespace SHAutomation.Core
{
    public interface IFrameworkAutomationElementBase
    {
        AutomationProperty<string> AcceleratorKey { get; }
        AutomationProperty<string> AccessKey { get; }
        AutomationProperty<int[]> AnnotationObjects { get; }
        AutomationProperty<int[]> AnnotationTypes { get; }
        AutomationProperty<string> AriaProperties { get; }
        AutomationProperty<string> AriaRole { get; }
        AutomationBase Automation { get; }
        AutomationProperty<string> AutomationId { get; }
        AutomationProperty<Rectangle> BoundingRectangle { get; }
        AutomationProperty<Point> CenterPoint { get; }
        AutomationProperty<string> ClassName { get; }
        AutomationProperty<Point> ClickablePoint { get; }
        AutomationProperty<SHAutomationElement[]> ControllerFor { get; }
        AutomationProperty<ControlType> ControlType { get; }
        AutomationProperty<CultureInfo> Culture { get; }
        AutomationProperty<SHAutomationElement[]> DescribedBy { get; }
        AutomationProperty<int> FillColor { get; }
        AutomationProperty<int> FillType { get; }
        AutomationProperty<SHAutomationElement[]> FlowsFrom { get; }
        AutomationProperty<SHAutomationElement[]> FlowsTo { get; }
        AutomationProperty<string> FrameworkId { get; }
        AutomationProperty<string> FullDescription { get; }
        AutomationProperty<bool> HasKeyboardFocus { get; }
        AutomationProperty<string> HelpText { get; }
        AutomationProperty<bool> IsContentElement { get; }
        AutomationProperty<bool> IsControlElement { get; }
        AutomationProperty<bool> IsDataValidForForm { get; }
        AutomationProperty<bool> IsEnabled { get; }
        AutomationProperty<bool> IsKeyboardFocusable { get; }
        AutomationProperty<bool> IsOffscreen { get; }
        AutomationProperty<bool> IsPassword { get; }
        AutomationProperty<bool> IsPeripheral { get; }
        AutomationProperty<bool> IsRequiredForForm { get; }
        AutomationProperty<string> ItemStatus { get; }
        AutomationProperty<string> ItemType { get; }
        AutomationProperty<SHAutomationElement> LabeledBy { get; }
        AutomationProperty<LandmarkType> LandmarkType { get; }
        AutomationProperty<int> Level { get; }
        AutomationProperty<LiveSetting> LiveSetting { get; }
        AutomationProperty<string> LocalizedControlType { get; }
        AutomationProperty<string> LocalizedLandmarkType { get; }
        AutomationProperty<string> Name { get; }
        AutomationProperty<IntPtr> NativeWindowHandle { get; }
        AutomationProperty<bool> OptimizeForVisualContent { get; }
        AutomationProperty<OrientationType> Orientation { get; }
        AutomationProperty<int[]> OutlineColor { get; }
        AutomationProperty<int[]> OutlineThickness { get; }
        FrameworkAutomationElementBase.IFrameworkPatterns Patterns { get; }
        AutomationProperty<int> PositionInSet { get; }
        AutomationProperty<int> ProcessId { get; }
        FrameworkAutomationElementBase.IProperties Properties { get; }
        ISHAutomationElementPropertyIds PropertyIdLibrary { get; }
        AutomationProperty<string> ProviderDescription { get; }
        AutomationProperty<int> Rotation { get; }
        AutomationProperty<int[]> RuntimeId { get; }
        AutomationProperty<int[]> Size { get; }
        AutomationProperty<int> SizeOfSet { get; }
        AutomationProperty<string> Value { get; }
        AutomationProperty<VisualEffects> VisualEffects { get; }

      SHAutomationElement[] FindAll(TreeScope treeScope, ConditionBase condition);
      SHAutomationElement[] FindAllWithOptions(TreeScope treeScope, ConditionBase condition, TreeTraversalOption traversalOptions,SHAutomationElement root);
      SHAutomationElement FindFirst(TreeScope treeScope, ConditionBase condition);
      SHAutomationElement FindFirstWithOptions(TreeScope treeScope, ConditionBase condition, TreeTraversalOption traversalOptions,SHAutomationElement root);
      SHAutomationElement FindIndexed(TreeScope treeScope, int index, ConditionBase condition);

        Point GetClickablePoint();
        object GetCurrentMetadataValue(PropertyId targetId, int metadataId);
        T GetNativePattern<T>(PatternId pattern);
        object GetPropertyValue(PropertyId property);
        T GetPropertyValue<T>(PropertyId property);
        PatternId[] GetSupportedPatterns();
        PropertyId[] GetSupportedProperties();
     
        AutomationEventHandlerBase RegisterAutomationEvent(EventId @event, TreeScope treeScope, Action<SHAutomationElement, EventId> action);
        NotificationEventHandlerBase RegisterNotificationEvent(TreeScope treeScope, Action<SHAutomationElement, NotificationKind, NotificationProcessing, string, string> action);
        PropertyChangedEventHandlerBase RegisterPropertyChangedEvent(TreeScope treeScope, Action<SHAutomationElement, PropertyId, object> action, PropertyId[] properties);
        StructureChangedEventHandlerBase RegisterStructureChangedEvent(TreeScope treeScope, Action<SHAutomationElement, StructureChangeType, int[]> action);
        TextEditTextChangedEventHandlerBase RegisterTextEditTextChangedEventHandler(TreeScope treeScope, TextEditChangeType textEditChangeType, Action<SHAutomationElement, TextEditChangeType, string[]> action);
        void SetFocus();
        bool TryGetClickablePoint(out Point point);
        bool TryGetNativePattern<T>(PatternId pattern, out T nativePattern);
        bool TryGetPropertyValue(PropertyId property, out object value);
        bool TryGetPropertyValue<T>(PropertyId property, out T value);
        void UnregisterAutomationEventHandler(AutomationEventHandlerBase eventHandler);
        void UnregisterNotificationEventHandler(NotificationEventHandlerBase eventHandler);
        void UnregisterPropertyChangedEventHandler(PropertyChangedEventHandlerBase eventHandler);
        void UnregisterStructureChangedEventHandler(StructureChangedEventHandlerBase eventHandler);
        void UnregisterTextEditTextChangedEventHandler(TextEditTextChangedEventHandlerBase eventHandler);
    }
}