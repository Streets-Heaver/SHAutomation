using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Identifiers;

namespace SHAutomation.Core.Patterns
{
    public interface ISelection2Pattern : ISelectionPattern
    {
        new ISelection2PatternPropertyIds PropertyIds { get; }

        AutomationProperty<SHAutomationElement> CurrentSelectedItem { get; }
        AutomationProperty<SHAutomationElement> FirstSelectedItem { get; }
        AutomationProperty<int> ItemCount { get; }
        AutomationProperty<SHAutomationElement> LastSelectedItem { get; }
    }

    public interface ISelection2PatternPropertyIds : ISelectionPatternPropertyIds
    {
        PropertyId CurrentSelectedItem { get; }
        PropertyId FirstSelectedItem { get; }
        PropertyId ItemCount { get; }
        PropertyId LastSelectedItem { get; }
    }

    public abstract class Selection2PatternBase<TNativePattern> : SelectionPatternBase<TNativePattern>, ISelection2Pattern
        where TNativePattern : class
    {
        private AutomationProperty<SHAutomationElement> _currentSelectedItem;
        private AutomationProperty<SHAutomationElement> _firstSelectedItem;
        private AutomationProperty<int> _itemCount;
        private AutomationProperty<SHAutomationElement> _lastSelectedItem;

        protected Selection2PatternBase(FrameworkAutomationElementBase frameworkAutomationElement, TNativePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        ISelection2PatternPropertyIds ISelection2Pattern.PropertyIds => Automation.PropertyLibrary.Selection2;

        public AutomationProperty<SHAutomationElement> CurrentSelectedItem => GetOrCreate(ref _currentSelectedItem, ((ISelection2Pattern)this).PropertyIds.CurrentSelectedItem);
        public AutomationProperty<SHAutomationElement> FirstSelectedItem => GetOrCreate(ref _firstSelectedItem, ((ISelection2Pattern)this).PropertyIds.FirstSelectedItem);
        public AutomationProperty<int> ItemCount => GetOrCreate(ref _itemCount, ((ISelection2Pattern)this).PropertyIds.ItemCount);
        public AutomationProperty<SHAutomationElement> LastSelectedItem => GetOrCreate(ref _lastSelectedItem, ((ISelection2Pattern)this).PropertyIds.LastSelectedItem);
    }
}
