using SHAutomation.Core;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;
using SHAutomation.UIA3.Converters;
using SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Patterns
{
    public class TablePattern : TablePatternBase<UIA.IUIAutomationTablePattern>
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_TablePatternId, "Table", AutomationObjectIds.IsTablePatternAvailableProperty);
        public static readonly PropertyId ColumnHeadersProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_TableColumnHeadersPropertyId, "ColumnHeaders").SetConverter(SHAutomationElementConverter.NativeArrayToManaged);
        public static readonly PropertyId RowHeadersProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_TableRowHeadersPropertyId, "RowHeaders").SetConverter(SHAutomationElementConverter.NativeArrayToManaged);
        public static readonly PropertyId RowOrColumnMajorProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_TableRowOrColumnMajorPropertyId, "RowOrColumnMajor");

        public TablePattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationTablePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }
    }

    public class TablePatternPropertyIds : ITablePatternPropertyIds
    {
        public PropertyId ColumnHeaders => TablePattern.ColumnHeadersProperty;
        public PropertyId RowHeaders => TablePattern.RowHeadersProperty;
        public PropertyId RowOrColumnMajor => TablePattern.RowOrColumnMajorProperty;
    }
}
