using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns.Infrastructure;

namespace SHAutomation.Core.Patterns
{
    public interface ITablePattern : IPattern
    {
        ITablePatternPropertyIds PropertyIds { get; }

        AutomationProperty<SHAutomationElement[]> ColumnHeaders { get; }
        AutomationProperty<SHAutomationElement[]> RowHeaders { get; }
        AutomationProperty<RowOrColumnMajor> RowOrColumnMajor { get; }
    }

    public interface ITablePatternPropertyIds
    {
        PropertyId ColumnHeaders { get; }
        PropertyId RowHeaders { get; }
        PropertyId RowOrColumnMajor { get; }
    }

    public abstract class TablePatternBase<TNativePattern> : PatternBase<TNativePattern>, ITablePattern
        where TNativePattern : class
    {
        private AutomationProperty<SHAutomationElement[]> _columnHeaders;
        private AutomationProperty<SHAutomationElement[]> _rowHeaders;
        private AutomationProperty<RowOrColumnMajor> _rowOrColumnMajor;

        protected TablePatternBase(FrameworkAutomationElementBase frameworkAutomationElement, TNativePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public ITablePatternPropertyIds PropertyIds => Automation.PropertyLibrary.Table;

        public AutomationProperty<SHAutomationElement[]> ColumnHeaders => GetOrCreate(ref _columnHeaders, PropertyIds.ColumnHeaders);
        public AutomationProperty<SHAutomationElement[]> RowHeaders => GetOrCreate(ref _rowHeaders, PropertyIds.RowHeaders);
        public AutomationProperty<RowOrColumnMajor> RowOrColumnMajor => GetOrCreate(ref _rowOrColumnMajor, PropertyIds.RowOrColumnMajor);
    }
}
