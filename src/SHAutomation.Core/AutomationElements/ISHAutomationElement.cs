using System;
using System.Drawing;
using System.Globalization;
using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.Enums;
using SHAutomation.Core.Identifiers;

namespace SHAutomation.Core.AutomationElements
{
    public interface ISHAutomationElement : IAutomationElement
    {

        void HoverOver(MouseAction? buttonToPress = null, int mouseSpeed = 5);
        Point? ClickablePoint { get; }
       
        bool IsOnscreen { get; }
     
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

        ISHAutomationElement Parent { get; }

        ISHAutomationElement[] FindAll(TreeScope treeScope, ConditionBase condition);
        ISHAutomationElement[] FindAll(TreeScope treeScope, ConditionBase condition, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllByXPath(string xPath);
        ISHAutomationElement[] FindAllByXPath(string xPath, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllChildren();
        ISHAutomationElement[] FindAllChildren(ConditionBase condition);
        ISHAutomationElement[] FindAllChildren(ConditionBase condition, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllChildren(Func<ConditionFactory, ConditionBase> conditionFunc);
        ISHAutomationElement[] FindAllChildren(Func<ConditionFactory, ConditionBase> conditionFunc, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendants();
        ISHAutomationElement[] FindAllDescendants(int timeout = 20000);
        ISHAutomationElement[] FindAllDescendants(ConditionBase condition);
        ISHAutomationElement[] FindAllDescendants(ConditionBase condition, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendants(Func<ConditionFactory, ConditionBase> conditionFunc);
        ISHAutomationElement[] FindAllDescendants(Func<ConditionFactory, ConditionBase> conditionFunc, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendantsNameContains(string name, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendantsNameEndsWith(string name, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllDescendantsNameStartsWith(string name, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement[] FindAllWithOptions(TreeScope treeScope, ConditionBase condition, TreeTraversalOptions traversalOptions,SHAutomationElement root);
        ISHAutomationElement[] FindAllWithOptions(TreeScope treeScope, ConditionBase condition, TreeTraversalOptions traversalOptions,SHAutomationElement root, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement FindFirst(TreeScope treeScope, ConditionBase condition);
        ISHAutomationElement FindFirst(TreeScope treeScope, ConditionBase condition, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement FindFirstChild();
        ISHAutomationElement FindFirstChild(int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement FindFirstChild(ConditionBase condition);
        ISHAutomationElement FindFirstChild(ConditionBase condition, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement FindFirstChild(Func<ConditionFactory, ConditionBase> conditionFunc);
        ISHAutomationElement FindFirstChild(Func<ConditionFactory, ConditionBase> conditionFunc, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement FindFirstChild(string automationId);
        ISHAutomationElement FindFirstChild(string automationId, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendant();
        ISHAutomationElement FindFirstDescendant(int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendant(ConditionBase condition, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendant(Func<ConditionFactory, ConditionBase> conditionFunc);
        ISHAutomationElement FindFirstDescendant(Func<ConditionFactory, ConditionBase> conditionFunc, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendant(string automationId);
        ISHAutomationElement FindFirstDescendant(string automationId, int timeout = 20000);
        ISHAutomationElement FindFirstDescendantNameContains(string name, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendantNameEndsWith(string name, int timeout = 20000, bool waitUntilExists = true);
        ISHAutomationElement FindFirstDescendantNameStartsWith(string name, int timeout = 20000, bool waitUntilExists = true);
        void WaitUntilPropertyEquals(PropertyId property,SHAutomationElement expected, int timeout = 10000);
        void WaitUntilPropertyEquals(PropertyId property, bool expected, int timeout = 10000);
        void WaitUntilPropertyEquals(PropertyId property, ControlType expected, int timeout = 10000);
        void WaitUntilPropertyEquals(PropertyId property, CultureInfo expected, int timeout = 10000);
        void WaitUntilPropertyEquals(PropertyId property, double expected, int timeout = 10000);
        void WaitUntilPropertyEquals(PropertyId property, int expected, int timeout = 10000);
        void WaitUntilPropertyEquals(PropertyId property, int[] expected, int timeout = 10000);
        void WaitUntilPropertyEquals(PropertyId property, IntPtr expected, int timeout = 10000);
        void WaitUntilPropertyEquals(PropertyId property, LiveSetting expected, int timeout = 10000);
        void WaitUntilPropertyEquals(PropertyId property, OrientationType expected, int timeout = 10000);
        void WaitUntilPropertyEquals(PropertyId property, Rectangle expected, int timeout = 10000);
        void WaitUntilPropertyEquals(PropertyId property, string expected, int timeout = 10000);
        void WaitUntilPropertyEquals(PropertyId property, VisualEffects expected, int timeout = 10000);
    }
}