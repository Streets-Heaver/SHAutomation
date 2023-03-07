using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.UIA3.Converters;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class SpreadsheetItemPattern : SpreadsheetItemPatternBase<UIA.IUIAutomationSpreadsheetItemPattern>
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_SpreadsheetItemPatternId, "SpreadsheetItem", AutomationObjectIds.IsSpreadsheetItemPatternAvailableProperty);
        public static readonly PropertyId FormulaProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_SpreadsheetItemFormulaPropertyId, "Formula");
        public static readonly PropertyId AnnotationObjectsProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_SpreadsheetItemAnnotationObjectsPropertyId, "AnnotationObjects").SetConverter(SHAutomationElementConverter.NativeArrayToManaged);
        public static readonly PropertyId AnnotationTypesProperty = PropertyId.Register( UIA.UIA_PropertyIds.UIA_SpreadsheetItemAnnotationTypesPropertyId, "AnnotationTypes").SetConverter((a, o) => AnnotationTypeConverter.ToAnnotationTypeArray(o));

        public SpreadsheetItemPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationSpreadsheetItemPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }
    }

    public class SpreadsheetItemPatternPropertyIds : ISpreadsheetItemPatternPropertyIds
    {
        public PropertyId Formula => SpreadsheetItemPattern.FormulaProperty;
        public PropertyId AnnotationObjects => SpreadsheetItemPattern.AnnotationObjectsProperty;
        public PropertyId AnnotationTypes => SpreadsheetItemPattern.AnnotationTypesProperty;
    }
}
