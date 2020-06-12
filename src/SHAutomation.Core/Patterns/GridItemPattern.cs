using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns.Infrastructure;

namespace SHAutomation.Core.Patterns
{
    public interface IGridItemPattern : IPattern
    {
        IGridItemPatternPropertyIds PropertyIds { get; }

        AutomationProperty<int> Column { get; }
        AutomationProperty<int> ColumnSpan { get; }
        AutomationProperty<SHAutomationElement> ContainingGrid { get; }
        AutomationProperty<int> Row { get; }
        AutomationProperty<int> RowSpan { get; }
    }

    public interface IGridItemPatternPropertyIds
    {
        PropertyId Column { get; }
        PropertyId ColumnSpan { get; }
        PropertyId ContainingGrid { get; }
        PropertyId Row { get; }
        PropertyId RowSpan { get; }
    }

    public abstract class GridItemPatternBase<TNativePattern> : PatternBase<TNativePattern>, IGridItemPattern
        where TNativePattern : class
    {
        private AutomationProperty<int> _column;
        private AutomationProperty<int> _columnSpan;
        private AutomationProperty<SHAutomationElement> _containingGrid;
        private AutomationProperty<int> _row;
        private AutomationProperty<int> _rowSpan;

        protected GridItemPatternBase(FrameworkAutomationElementBase frameworkAutomationElement, TNativePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public IGridItemPatternPropertyIds PropertyIds => Automation.PropertyLibrary.GridItem;

        public AutomationProperty<int> Column => GetOrCreate(ref _column, PropertyIds.Column);
        public AutomationProperty<int> ColumnSpan => GetOrCreate(ref _columnSpan, PropertyIds.ColumnSpan);
        public AutomationProperty<SHAutomationElement> ContainingGrid => GetOrCreate(ref _containingGrid, PropertyIds.ContainingGrid);
        public AutomationProperty<int> Row => GetOrCreate(ref _row, PropertyIds.Row);
        public AutomationProperty<int> RowSpan => GetOrCreate(ref _rowSpan, PropertyIds.RowSpan);
    }
}
