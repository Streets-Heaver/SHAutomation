using System;
using System.Collections.Generic;
using System.Drawing;
using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.EventHandlers;
using SHAutomation.Core.Identifiers;

namespace SHAutomation.Core.AutomationElements
{
    public interface IAutomationElement
    {
        double ActualHeight { get; }
        double ActualWidth { get; }
        AutomationBase Automation { get; }
        string AutomationId { get; }
        AutomationType AutomationType { get; }
        Rectangle BoundingRectangle { get; }
        SHAutomationElement[] CachedChildren { get; }
        SHAutomationElement CachedParent { get; }
        string ClassName { get; }
        ConditionFactory ConditionFactory { get; }
        ControlType ControlType { get; }
        FrameworkAutomationElementBase FrameworkAutomationElement { get; }
        FrameworkType FrameworkType { get; }
        string HelpText { get; }
        bool IsEnabled { get; }
        bool IsOffscreen { get; }
        string ItemStatus { get; }
        string Name { get; }
        FrameworkAutomationElementBase.IFrameworkPatterns Patterns { get; }
        int ProcessId { get; }
        string ProcessIdstring { get; }
        FrameworkAutomationElementBase.IProperties Properties { get; }

        Bitmap Capture();
        void CaptureToFile(string filePath);
        void Click(bool moveMouse = false);
        void DoubleClick(bool moveMouse = false);
        bool Equals(SHAutomationElement other);
        bool Equals(object obj);
        void Focus();
        void FocusNative();
        Point GetClickablePoint();
        object GetCurrentMetadataValue(PropertyId targetId, int metadataId);
        int GetHashCode();
        PatternId[] GetSupportedPatterns();
        PatternId[] GetSupportedPatternsDirect();
        PropertyId[] GetSupportedPropertiesDirect();
        bool IsPatternSupported(PatternId pattern);
        bool IsPatternSupportedDirect(PatternId pattern);
        bool IsPropertySupportedDirect(PropertyId property);
        AutomationEventHandlerBase RegisterAutomationEvent(EventId @event, TreeScope treeScope, Action<SHAutomationElement, EventId> action);
        NotificationEventHandlerBase RegisterNotificationEvent(TreeScope treeScope, Action<SHAutomationElement, NotificationKind, NotificationProcessing, string, string> action);
        PropertyChangedEventHandlerBase RegisterPropertyChangedEvent(TreeScope treeScope, Action<SHAutomationElement, PropertyId, object> action, params PropertyId[] properties);
        StructureChangedEventHandlerBase RegisterStructureChangedEvent(TreeScope treeScope, Action<SHAutomationElement, StructureChangeType, int[]> action);
        TextEditTextChangedEventHandlerBase RegisterTextEditTextChangedEventHandler(TreeScope treeScope, TextEditChangeType textEditChangeType, Action<SHAutomationElement, TextEditChangeType, string[]> action);
        void RightClick(bool moveMouse = false);
        void RightDoubleClick(bool moveMouse = false);
        void SetForeground();
        string ToString();
        void TryFocus();
        bool TryGetClickablePoint(out Point point);
        Point Centre();
        Point CentreLeft();
        Point CentreRight();
        Point CentreTop();
        Point CentreBottom();
        Point TopLeft();
        Point TopRight();
        Point BottomLeft();
        Point BottomRight();
       
    }
}