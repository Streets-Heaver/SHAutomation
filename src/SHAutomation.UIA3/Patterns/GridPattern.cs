using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.Core.Tools;
using StreetsHeaver.SHAutomation.UIA3.Converters;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class GridPattern : GridPatternBase<UIA.IUIAutomationGridPattern>
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_GridPatternId, "Grid", AutomationObjectIds.IsGridPatternAvailableProperty);
        public static readonly PropertyId ColumnCountProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_GridColumnCountPropertyId, "ColumnCount");
        public static readonly PropertyId RowCountProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_GridRowCountPropertyId, "RowCount");

        public GridPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationGridPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public override SHAutomationElement GetItem(int row, int column)
        {
            var nativeItem = Com.Call(() => NativePattern.GetItem(row, column));
            return SHAutomationElementConverter.NativeToManaged((UIA3Automation)FrameworkAutomationElement.Automation, nativeItem);
        }
    }

    public class GridPatternPropertyIds : IGridPatternPropertyIds
    {
        public PropertyId ColumnCount => GridPattern.ColumnCountProperty;

        public PropertyId RowCount => GridPattern.RowCountProperty;
    }
}
