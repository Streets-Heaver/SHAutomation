﻿using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns.Infrastructure;

namespace SHAutomation.Core.Patterns
{
    public interface IAnnotationPattern : IPattern
    {
        IAnnotationPatternPropertyIds PropertyIds { get; }

        AutomationProperty<AnnotationType> AnnotationType { get; }
        AutomationProperty<string> AnnotationTypeName { get; }
        AutomationProperty<string> Author { get; }
        AutomationProperty<string> DateTime { get; }
        AutomationProperty<SHAutomationElement> Target { get; }
    }

    public interface IAnnotationPatternPropertyIds
    {
        PropertyId AnnotationTypeId { get; }
        PropertyId AnnotationTypeName { get; }
        PropertyId Author { get; }
        PropertyId DateTime { get; }
        PropertyId Target { get; }
    }

    public abstract class AnnotationPatternBase<TNativePattern> : PatternBase<TNativePattern>, IAnnotationPattern
        where TNativePattern : class
    {
        private AutomationProperty<AnnotationType> _annotationType;
        private AutomationProperty<string> _annotationTypeName;
        private AutomationProperty<string> _author;
        private AutomationProperty<string> _dateTime;
        private AutomationProperty<SHAutomationElement> _target;

        protected AnnotationPatternBase(FrameworkAutomationElementBase frameworkAutomationElement, TNativePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public IAnnotationPatternPropertyIds PropertyIds => Automation.PropertyLibrary.Annotation;

        public AutomationProperty<AnnotationType> AnnotationType => GetOrCreate(ref _annotationType, PropertyIds.AnnotationTypeId);
        public AutomationProperty<string> AnnotationTypeName => GetOrCreate(ref _annotationTypeName, PropertyIds.AnnotationTypeName);
        public AutomationProperty<string> Author => GetOrCreate(ref _author, PropertyIds.Author);
        public AutomationProperty<string> DateTime => GetOrCreate(ref _dateTime, PropertyIds.DateTime);
        public AutomationProperty<SHAutomationElement> Target => GetOrCreate(ref _target, PropertyIds.Target);
    }
}