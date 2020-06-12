using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns.Infrastructure;

namespace SHAutomation.Core.Patterns
{
    public interface ISelectionPattern : IPattern
    {
        ISelectionPatternPropertyIds PropertyIds { get; }
        ISelectionPatternEventIds EventIds { get; }

        AutomationProperty<bool> CanSelectMultiple { get; }
        AutomationProperty<bool> IsSelectionRequired { get; }
        AutomationProperty<SHAutomationElement[]> Selection { get; }
    }

    public interface ISelectionPatternPropertyIds
    {
        PropertyId CanSelectMultiple { get; }
        PropertyId IsSelectionRequired { get; }
        PropertyId Selection { get; }
    }

    public interface ISelectionPatternEventIds
    {
        EventId InvalidatedEvent { get; }
    }

    public abstract class SelectionPatternBase<TNativePattern> : PatternBase<TNativePattern>, ISelectionPattern
        where TNativePattern : class
    {
        private AutomationProperty<bool> _canSelectMultiple;
        private AutomationProperty<bool> _isSelectionRequired;
        private AutomationProperty<SHAutomationElement[]> _selection;

        protected SelectionPatternBase(FrameworkAutomationElementBase frameworkAutomationElement, TNativePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public ISelectionPatternPropertyIds PropertyIds => Automation.PropertyLibrary.Selection;
        public ISelectionPatternEventIds EventIds => Automation.EventLibrary.Selection;

        public AutomationProperty<bool> CanSelectMultiple => GetOrCreate(ref _canSelectMultiple, PropertyIds.CanSelectMultiple);
        public AutomationProperty<bool> IsSelectionRequired => GetOrCreate(ref _isSelectionRequired, PropertyIds.IsSelectionRequired);
        public AutomationProperty<SHAutomationElement[]> Selection => GetOrCreate(ref _selection, PropertyIds.Selection);
    }
}
