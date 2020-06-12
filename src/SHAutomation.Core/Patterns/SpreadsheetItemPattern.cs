using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns.Infrastructure;

namespace SHAutomation.Core.Patterns
{
    public interface ISpreadsheetItemPattern : IPattern
    {
        ISpreadsheetItemPatternPropertyIds PropertyIds { get; }

        AutomationProperty<string> Formula { get; }
        AutomationProperty<SHAutomationElement[]> AnnotationObjects { get; }
        AutomationProperty<AnnotationType[]> AnnotationTypes { get; }
    }

    public interface ISpreadsheetItemPatternPropertyIds
    {
        PropertyId Formula { get; }
        PropertyId AnnotationObjects { get; }
        PropertyId AnnotationTypes { get; }
    }

    public abstract class SpreadsheetItemPatternBase<TNativePattern> : PatternBase<TNativePattern>, ISpreadsheetItemPattern
        where TNativePattern : class
    {
        private AutomationProperty<string> _formula;
        private AutomationProperty<SHAutomationElement[]> _annotationObjects;
        private AutomationProperty<AnnotationType[]> _annotationTypes;

        protected SpreadsheetItemPatternBase(FrameworkAutomationElementBase frameworkAutomationElement, TNativePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public ISpreadsheetItemPatternPropertyIds PropertyIds => Automation.PropertyLibrary.SpreadsheetItem;

        public AutomationProperty<string> Formula => GetOrCreate(ref _formula, PropertyIds.Formula);
        public AutomationProperty<SHAutomationElement[]> AnnotationObjects => GetOrCreate(ref _annotationObjects, PropertyIds.AnnotationObjects);
        public AutomationProperty<AnnotationType[]> AnnotationTypes => GetOrCreate(ref _annotationTypes, PropertyIds.AnnotationTypes);
    }
}
