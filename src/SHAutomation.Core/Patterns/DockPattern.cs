using SHAutomation.Core.Definitions;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns.Infrastructure;

namespace SHAutomation.Core.Patterns
{
    public interface IDockPattern : IPattern
    {
        IDockPatternPropertyIds PropertyIds { get; }

        AutomationProperty<DockPosition> DockPosition { get; }

        void SetDockPosition(DockPosition dockPos);
    }

    public interface IDockPatternPropertyIds
    {
        PropertyId DockPosition { get; }
    }

    public abstract class DockPatternBase<TNativePattern> : PatternBase<TNativePattern>, IDockPattern
        where TNativePattern : class
    {
        private AutomationProperty<DockPosition> _dockPosition;

        protected DockPatternBase(FrameworkAutomationElementBase frameworkAutomationElement, TNativePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public IDockPatternPropertyIds PropertyIds => Automation.PropertyLibrary.Dock;

        public AutomationProperty<DockPosition> DockPosition => GetOrCreate(ref _dockPosition, PropertyIds.DockPosition);

        public abstract void SetDockPosition(DockPosition dockPos);
    }
}
