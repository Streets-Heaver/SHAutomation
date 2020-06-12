using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns.Infrastructure;

namespace SHAutomation.Core.Patterns
{
    public interface ITableItemPattern : IPattern
    {
        ITableItemPatternPropertyIds PropertyIds { get; }

        AutomationProperty<SHAutomationElement[]> ColumnHeaderItems { get; }
        AutomationProperty<SHAutomationElement[]> RowHeaderItems { get; }
    }

    public interface ITableItemPatternPropertyIds
    {
        PropertyId ColumnHeaderItems { get; }
        PropertyId RowHeaderItems { get; }
    }

    public abstract class TableItemPatternBase<TNativePattern> : PatternBase<TNativePattern>, ITableItemPattern
        where TNativePattern : class
    {
        private AutomationProperty<SHAutomationElement[]> _columnHeaderItems;
        private AutomationProperty<SHAutomationElement[]> _rowHeaderItems;

        protected TableItemPatternBase(FrameworkAutomationElementBase frameworkAutomationElement, TNativePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public ITableItemPatternPropertyIds PropertyIds => Automation.PropertyLibrary.TableItem;

        public AutomationProperty<SHAutomationElement[]> ColumnHeaderItems => GetOrCreate(ref _columnHeaderItems, PropertyIds.ColumnHeaderItems);
        public AutomationProperty<SHAutomationElement[]> RowHeaderItems => GetOrCreate(ref _rowHeaderItems, PropertyIds.RowHeaderItems);
    }
}
