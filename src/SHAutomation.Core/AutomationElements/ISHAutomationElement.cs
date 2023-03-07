using StreetsHeaver.SHAutomation.Core.Conditions;
using StreetsHeaver.SHAutomation.Core.Definitions;
using StreetsHeaver.SHAutomation.Core.Enums;
using StreetsHeaver.SHAutomation.Core.EventHandlers;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace StreetsHeaver.SHAutomation.Core.AutomationElements
{
    public interface ISHAutomationElement
    {
        double ActualHeight { get; }
        double ActualWidth { get; }
        AutomationBase Automation { get; }
        string AutomationId { get; }
        Rectangle BoundingRectangle { get; }
        string ClassName { get; }
        Point? ClickablePoint { get; }
        ConditionFactory ConditionFactory { get; }
        ControlType ControlType { get; }
        FrameworkAutomationElementBase FrameworkAutomationElement { get; }
        FrameworkType FrameworkType { get; }
        string HelpText { get; }
        bool IsAvailable { get; }
        bool IsEnabled { get; }
        bool IsOffscreen { get; }
        bool IsOnscreen { get; }
        bool FoundByXPath { get; set; }
        string ItemStatus { get; }
        string Name { get; }
        ISHAutomationElement Parent { get; }
        FrameworkAutomationElementBase.IFrameworkPatterns Patterns { get; }
        int ProcessId { get; }
        string ProcessIdstring { get; }
        FrameworkAutomationElementBase.IProperties Properties { get; }
        bool SupportsAutomationId { get; }
        bool SupportsBoundingRectangle { get; }
        bool SupportsClassName { get; }
        bool SupportsClickablePoint { get; }
        bool SupportsControlType { get; }
        bool SupportsEnabled { get; }
        bool SupportsHelpText { get; }
        bool SupportsName { get; }
        bool SupportsOnscreen { get; }
        bool SupportsTogglePattern { get; }

        Point BottomLeft();
        Point BottomRight();
        Bitmap Capture();
        void CaptureToFile(string filePath);
        Point Centre();
        Point CentreBottom();
        Point CentreLeft();
        Point CentreRight();
        Point CentreTop();
        void Click(int mouseSpeed = 5);
        void Click(MouseAction buttonToPress, int mouseSpeed = 5);
        bool Equals(object obj);
        bool Equals(SHAutomationElement other);
        ISHAutomationElement[] FindAll(TreeScope treeScope, ConditionBase condition, bool waitUntilExists = true);
        ISHAutomationElement[] FindAll(TreeScope treeScope, ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllByXPath(string xPath, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllByXPath(string xPath, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllChildren();
        ISHAutomationElement[] FindAllChildren(ConditionBase condition, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllChildren(ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllChildren(Func<ConditionFactory, ConditionBase> conditionFunc, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllChildren(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllChildren(TimeSpan timeout);
        ISHAutomationElement[] FindAllChildrenBase();
        ISHAutomationElement[] FindAllChildrenBase(ConditionBase condition);
        ISHAutomationElement[] FindAllChildrenBase(Func<ConditionFactory, ConditionBase> conditionFunc);
        ISHAutomationElement[] FindAllDescendants();
        ISHAutomationElement[] FindAllDescendants(ConditionBase condition);
        ISHAutomationElement[] FindAllDescendants(ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendants(Func<ConditionFactory, ConditionBase> conditionFunc, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendants(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendants(TimeSpan timeout);
        ISHAutomationElement[] FindAllDescendantsBase();
        ISHAutomationElement[] FindAllDescendantsBase(ConditionBase condition);
        ISHAutomationElement[] FindAllDescendantsBase(Func<ConditionFactory, ConditionBase> conditionFunc);
        ISHAutomationElement[] FindAllDescendantsNameContains(string name, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendantsNameContains(string name, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendantsNameEndsWith(string name, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendantsNameEndsWith(string name, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendantsNameStartsWith(string name, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendantsNameStartsWith(string name, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllNested(Func<ConditionFactory, IList<ConditionBase>> conditionFunc);
        ISHAutomationElement[] FindAllNested(params ConditionBase[] nestedConditions);
        ISHAutomationElement[] FindAllWithOptions(TreeScope treeScope, ConditionBase condition, TreeTraversalOption traversalOptions, ISHAutomationElement root);
        ISHAutomationElement[] FindAllWithOptions(TreeScope treeScope, ConditionBase condition, TreeTraversalOption traversalOptions, ISHAutomationElement root, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllWithOptionsBase(TreeScope treeScope, ConditionBase condition, TreeTraversalOption traversalOptions, ISHAutomationElement root);
        ISHAutomationElement FindAt(TreeScope treeScope, int index, ConditionBase condition);
        ISHAutomationElement FindChildAt(int index, ConditionBase condition = null);
        ISHAutomationElement FindChildAt(int index, Func<ConditionFactory, ConditionBase> conditionFunc);
        ISHAutomationElement FindFirst(TreeScope treeScope, ConditionBase condition, bool waitUntilExists = true);
        ISHAutomationElement FindFirst(TreeScope treeScope, ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement FindFirstBase(TreeScope treeScope, ConditionBase condition);
        ISHAutomationElement FindFirstByXPath(string xPath);
        ISHAutomationElement FindFirstByXPath(string xPath, TimeSpan timeout);
        ISHAutomationElement FindFirstChild(bool waitUntilExists = true);
        ISHAutomationElement FindFirstChild(ConditionBase condition, bool waitUntilExists = true);
        ISHAutomationElement FindFirstChild(ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement FindFirstChild(Func<ConditionFactory, ConditionBase> conditionFunc);
        ISHAutomationElement FindFirstChild(Func<ConditionFactory, ConditionBase> conditionFunc, bool waitUntilExists = true);
        ISHAutomationElement FindFirstChild(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement FindFirstChild(string automationId);
        ISHAutomationElement FindFirstChild(string automationId, bool waitUntilExists = true);
        ISHAutomationElement FindFirstChild(string automationId, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement FindFirstChild(TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement FindFirstChildBase();
        ISHAutomationElement FindFirstChildBase(ConditionBase condition);
        ISHAutomationElement FindFirstChildBase(string automationId);
        ISHAutomationElement FindFirstDescendant(bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendant(ConditionBase condition, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendant(ConditionBase condition, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendant(Func<ConditionFactory, ConditionBase> conditionFunc, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendant(Func<ConditionFactory, ConditionBase> conditionFunc, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendant(string automationId);
        ISHAutomationElement FindFirstDescendant(string automationId, TimeSpan timeout);
        ISHAutomationElement FindFirstDescendant(TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendantBase();
        ISHAutomationElement FindFirstDescendantBase(ConditionBase condition);
        ISHAutomationElement FindFirstDescendantBase(Func<ConditionFactory, ConditionBase> conditionFunc);
        ISHAutomationElement FindFirstDescendantBase(string automationId);
        ISHAutomationElement FindFirstDescendantNameContains(string name, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendantNameContains(string name, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendantNameEndsWith(string name, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendantNameEndsWith(string name, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendantNameStartsWith(string name, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendantNameStartsWith(string name, TimeSpan timeout, bool waitUntilExists = true);
        ISHAutomationElement FindFirstNested(Func<ConditionFactory, IList<ConditionBase>> conditionFunc);
        ISHAutomationElement FindFirstNested(params ConditionBase[] nestedConditions);
        ISHAutomationElement FindFirstWithOptionsBase(TreeScope treeScope, ConditionBase condition, TreeTraversalOption traversalOptions, ISHAutomationElement root);
        void Focus();
        void FocusNative();
        Point GetClickablePoint();
        object GetCurrentMetadataValue(PropertyId targetId, int metadataId);
        int GetHashCode();
        PatternId[] GetSupportedPatterns();
        PatternId[] GetSupportedPatternsDirect();
        PropertyId[] GetSupportedPropertiesDirect();
        void HoverOver(int mouseSpeed = 5);
        bool IsPatternSupported(PatternId pattern);
        bool IsPatternSupportedDirect(PatternId pattern);
        bool IsPropertySupportedDirect(PropertyId property);
        ActiveTextPositionChangedEventHandlerBase RegisterActiveTextPositionChangedEvent(TreeScope treeScope, Action<SHAutomationElement, ITextRange> action);
        AutomationEventHandlerBase RegisterAutomationEvent(EventId @event, TreeScope treeScope, Action<SHAutomationElement, EventId> action);
        NotificationEventHandlerBase RegisterNotificationEvent(TreeScope treeScope, Action<SHAutomationElement, NotificationKind, NotificationProcessing, string, string> action);
        PropertyChangedEventHandlerBase RegisterPropertyChangedEvent(TreeScope treeScope, Action<SHAutomationElement, PropertyId, object> action, params PropertyId[] properties);
        StructureChangedEventHandlerBase RegisterStructureChangedEvent(TreeScope treeScope, Action<SHAutomationElement, StructureChangeType, int[]> action);
        TextEditTextChangedEventHandlerBase RegisterTextEditTextChangedEventHandler(TreeScope treeScope, TextEditChangeType textEditChangeType, Action<SHAutomationElement, TextEditChangeType, string[]> action);
        void SetForeground();
        Point TopLeft();
        Point TopRight();
        string ToString();
        void TryFocus();
        bool TryGetClickablePoint(out Point point);
        bool WaitUntilPropertyEquals(PropertyId property, bool expected);
        bool WaitUntilPropertyEquals(PropertyId property, bool expected, TimeSpan timeout);
        bool WaitUntilPropertyEquals(PropertyId property, ControlType expected);
        bool WaitUntilPropertyEquals(PropertyId property, ControlType expected, TimeSpan timeout);
        bool WaitUntilPropertyEquals(PropertyId property, CultureInfo expected);
        bool WaitUntilPropertyEquals(PropertyId property, CultureInfo expected, TimeSpan timeout);
        bool WaitUntilPropertyEquals(PropertyId property, double expected);
        bool WaitUntilPropertyEquals(PropertyId property, double expected, TimeSpan timeout);
        bool WaitUntilPropertyEquals(PropertyId property, int expected);
        bool WaitUntilPropertyEquals(PropertyId property, int expected, TimeSpan timeout);
        bool WaitUntilPropertyEquals(PropertyId property, int[] expected);
        bool WaitUntilPropertyEquals(PropertyId property, int[] expected, TimeSpan timeout);
        bool WaitUntilPropertyEquals(PropertyId property, IntPtr expected);
        bool WaitUntilPropertyEquals(PropertyId property, IntPtr expected, TimeSpan timeout);
        bool WaitUntilPropertyEquals(PropertyId property, LiveSetting expected);
        bool WaitUntilPropertyEquals(PropertyId property, LiveSetting expected, TimeSpan timeout);
        bool WaitUntilPropertyEquals(PropertyId property, OrientationType expected);
        bool WaitUntilPropertyEquals(PropertyId property, OrientationType expected, TimeSpan timeout);
        bool WaitUntilPropertyEquals(PropertyId property, Rectangle expected);
        bool WaitUntilPropertyEquals(PropertyId property, Rectangle expected, TimeSpan timeout);
        bool WaitUntilPropertyEquals(PropertyId property, ISHAutomationElement expected);
        bool WaitUntilPropertyEquals(PropertyId property, ISHAutomationElement expected, TimeSpan timeout);
        bool WaitUntilPropertyEquals(PropertyId property, string expected);
        bool WaitUntilPropertyEquals(PropertyId property, string expected, TimeSpan timeout);
        bool WaitUntilPropertyEquals(PropertyId property, VisualEffects expected);
        bool WaitUntilPropertyEquals(PropertyId property, VisualEffects expected, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, bool current);
        bool WaitUntilPropertyNotEquals(PropertyId property, bool current, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, ControlType current);
        bool WaitUntilPropertyNotEquals(PropertyId property, ControlType current, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, CultureInfo current);
        bool WaitUntilPropertyNotEquals(PropertyId property, CultureInfo current, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, double current);
        bool WaitUntilPropertyNotEquals(PropertyId property, double current, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, int current);
        bool WaitUntilPropertyNotEquals(PropertyId property, int current, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, int[] current);
        bool WaitUntilPropertyNotEquals(PropertyId property, int[] current, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, IntPtr current);
        bool WaitUntilPropertyNotEquals(PropertyId property, IntPtr current, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, LiveSetting current);
        bool WaitUntilPropertyNotEquals(PropertyId property, LiveSetting current, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, OrientationType current);
        bool WaitUntilPropertyNotEquals(PropertyId property, OrientationType current, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, Rectangle current);
        bool WaitUntilPropertyNotEquals(PropertyId property, Rectangle current, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, ISHAutomationElement current);
        bool WaitUntilPropertyNotEquals(PropertyId property, ISHAutomationElement current, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, string current);
        bool WaitUntilPropertyNotEquals(PropertyId property, string current, TimeSpan timeout);
        bool WaitUntilPropertyNotEquals(PropertyId property, VisualEffects current);
        bool WaitUntilPropertyNotEquals(PropertyId property, VisualEffects current, TimeSpan timeout);
        object GetCurrentPropertyValue(PropertyId propertyId);
    }
}