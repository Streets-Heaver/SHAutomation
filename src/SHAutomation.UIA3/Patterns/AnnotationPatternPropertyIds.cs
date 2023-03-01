using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Patterns;

namespace SHAutomation.UIA3.Patterns
{
    public class AnnotationPatternPropertyIds : IAnnotationPatternPropertyIds
    {
        public PropertyId AnnotationTypeId => AnnotationPattern.AnnotationTypeIdProperty;
        public PropertyId AnnotationTypeName => AnnotationPattern.AnnotationTypeNameProperty;
        public PropertyId Author => AnnotationPattern.AuthorProperty;
        public PropertyId DateTime => AnnotationPattern.DateTimeProperty;
        public PropertyId Target => AnnotationPattern.TargetProperty;
    }
}
