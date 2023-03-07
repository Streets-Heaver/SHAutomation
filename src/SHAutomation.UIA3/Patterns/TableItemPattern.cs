using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.UIA3.Converters;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class TableItemPattern : TableItemPatternBase<UIA.IUIAutomationTableItemPattern>
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_TableItemPatternId, "TableItem", AutomationObjectIds.IsTableItemPatternAvailableProperty);
        public static readonly PropertyId ColumnHeaderItemsProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_TableItemColumnHeaderItemsPropertyId, "ColumnHeaderItems").SetConverter(SHAutomationElementConverter.NativeArrayToManaged);
        public static readonly PropertyId RowHeaderItemsProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_TableItemRowHeaderItemsPropertyId, "RowHeaderItems").SetConverter(SHAutomationElementConverter.NativeArrayToManaged);

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
