using System;
using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.Core.Tools;
using StreetsHeaver.SHAutomation.Core.WindowsAPI;
using StreetsHeaver.SHAutomation.UIA3.Converters;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class LegacyIAccessiblePattern : LegacyIAccessiblePatternBase<UIA.IUIAutomationLegacyIAccessiblePattern>
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_LegacyIAccessiblePatternId, "LegacyIAccessible", AutomationObjectIds.IsLegacyIAccessiblePatternAvailableProperty);
        public static readonly PropertyId ChildIdProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_LegacyIAccessibleChildIdPropertyId, "ChildId");
        public static readonly PropertyId DefaultActionProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_LegacyIAccessibleDefaultActionPropertyId, "DefaultAction");
        public static readonly PropertyId DescriptionProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_LegacyIAccessibleDescriptionPropertyId, "Description");
        public static readonly PropertyId HelpProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_LegacyIAccessibleHelpPropertyId, "Help");
        public static readonly PropertyId KeyboardShortcutProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_LegacyIAccessibleKeyboardShortcutPropertyId, "KeyboardShortcut");
        public static readonly PropertyId NameProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_LegacyIAccessibleNamePropertyId, "Name");
        public static readonly PropertyId RoleProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_LegacyIAccessibleRolePropertyId, "Role").SetConverter((a, o) => (AccessibilityRole)Convert.ToUInt32(o));
        public static readonly PropertyId SelectionProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_LegacyIAccessibleSelectionPropertyId, "Selection").SetConverter(SHAutomationElementConverter.NativeArrayToManaged);
        public static readonly PropertyId StateProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_LegacyIAccessibleStatePropertyId, "State").SetConverter((a, o) => (AccessibilityState)Convert.ToUInt32(o));
        public static readonly PropertyId ValueProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_LegacyIAccessibleValuePropertyId, "Value");

        public LegacyIAccessiblePattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationLegacyIAccessiblePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public override void DoDefaultAction()
        {
            Com.Call(() => NativePattern.DoDefaultAction());
        }

        public override void Select(int flagsSelect)
        {
            Com.Call(() => NativePattern.Select(flagsSelect));
        }

        public override void SetValue(string value)
        {
            Com.Call(() => NativePattern.SetValue(value));
        }
    }

    public class LegacyIAccessiblePatternPropertyIds : ILegacyIAccessiblePatternPropertyIds
    {
        public PropertyId ChildId => LegacyIAccessiblePattern.ChildIdProperty;
        public PropertyId DefaultAction => LegacyIAccessiblePattern.DefaultActionProperty;
        public PropertyId Description => LegacyIAccessiblePattern.DescriptionProperty;
        public PropertyId Help => LegacyIAccessiblePattern.HelpProperty;
        public PropertyId KeyboardShortcut => LegacyIAccessiblePattern.KeyboardShortcutProperty;
        public PropertyId Name => LegacyIAccessiblePattern.NameProperty;
        public PropertyId Role => LegacyIAccessiblePattern.RoleProperty;
        public PropertyId Selection => LegacyIAccessiblePattern.SelectionProperty;
        public PropertyId State => LegacyIAccessiblePattern.StateProperty;
        public PropertyId Value => LegacyIAccessiblePattern.ValueProperty;
    }
}
