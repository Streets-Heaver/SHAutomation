using SHAutomation.Core;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;
using SHAutomation.UIA3.Converters;
using SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Patterns
{
    public class TableItemPattern : TableItemPatternBase<UIA.IUIAutomationTableItemPattern>
    {
        public static readonly PatternId Pattern = PatternId.Register(AutomationType.UIA3, UIA.UIA_PatternIds.UIA_TableItemPatternId, "TableItem", AutomationObjectIds.IsTableItemPatternAvailableProperty);
        public static readonly PropertyId ColumnHeaderItemsProperty = PropertyId.Register(AutomationType.UIA3, UIA.UIA_PropertyIds.UIA_TableItemColumnHeaderItemsPropertyId, "ColumnHeaderItems").SetConverter(SHAutomationElementConverter.NativeArrayToManaged);
        public static readonly PropertyId RowHeaderItemsProperty = PropertyId.Register(AutomationType.UIA3, UIA.UIA_PropertyIds.UIA_TableItemRowHeaderItemsPropertyId, "RowHeaderItems").SetConverter(SHAutomationElementConverter.NativeArrayToManaged);

        public TableItemPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationTableItemPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }
    }

    public class TableItemPatternPropertyIds : ITableItemPatternPropertyIds
    {
        public PropertyId ColumnHeaderItems => TableItemPattern.ColumnHeaderItemsProperty;
        public PropertyId RowHeaderItems => TableItemPattern.RowHeaderItemsProperty;
    }
}
