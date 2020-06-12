using SHAutomation.Core.Definitions;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns.Infrastructure;

namespace SHAutomation.Core.Patterns
{
    public interface IExpandCollapsePattern : IPattern
    {
        IExpandCollapsePatternPropertyIds PropertyIds { get; }

        AutomationProperty<ExpandCollapseState> ExpandCollapseState { get; }

        void Collapse();
        void Expand();
    }

    public interface IExpandCollapsePatternPropertyIds
    {
        PropertyId ExpandCollapseState { get; }
    }

    public abstract class ExpandCollapsePatternBase<TNativePattern> : PatternBase<TNativePattern>, IExpandCollapsePattern
        where TNativePattern : class
    {
        private AutomationProperty<ExpandCollapseState> _expandCollapseState;

        protected ExpandCollapsePatternBase(FrameworkAutomationElementBase frameworkAutomationElement, TNativePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public IExpandCollapsePatternPropertyIds PropertyIds => Automation.PropertyLibrary.ExpandCollapse;

        public AutomationProperty<ExpandCollapseState> ExpandCollapseState => GetOrCreate(ref _expandCollapseState, PropertyIds.ExpandCollapseState);

        public abstract void Collapse();
        public abstract void Expand();
    }
}
