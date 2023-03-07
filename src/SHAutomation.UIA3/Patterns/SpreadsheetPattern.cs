using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.Core.Patterns.Infrastructure;
using StreetsHeaver.SHAutomation.Core.Tools;
using StreetsHeaver.SHAutomation.UIA3.Converters;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class SpreadsheetPattern : PatternBase<UIA.IUIAutomationSpreadsheetPattern>, ISpreadsheetPattern
    {
        public static readonly PatternId Pattern = PatternId.Register( UIA.UIA_PatternIds.UIA_SpreadsheetPatternId, "Spreadsheet", AutomationObjectIds.IsSpreadsheetPatternAvailableProperty);

        public SpreadsheetPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationSpreadsheetPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public SHAutomationElement GetItemByName(string name)
        {
            var nativeElement = Com.Call(() => NativePattern.GetItemByName(name));
            return SHAutomationElementConverter.NativeToManaged((UIA3Automation)FrameworkAutomationElement.Automation, nativeElement);
        }
    }
}
