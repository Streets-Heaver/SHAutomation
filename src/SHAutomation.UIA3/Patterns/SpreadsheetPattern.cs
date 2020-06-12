using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;
using SHAutomation.Core.Patterns.Infrastructure;
using SHAutomation.Core.Tools;
using SHAutomation.UIA3.Converters;
using SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Patterns
{
    public class SpreadsheetPattern : PatternBase<UIA.IUIAutomationSpreadsheetPattern>, ISpreadsheetPattern
    {
        public static readonly PatternId Pattern = PatternId.Register(AutomationType.UIA3, UIA.UIA_PatternIds.UIA_SpreadsheetPatternId, "Spreadsheet", AutomationObjectIds.IsSpreadsheetPatternAvailableProperty);

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
