using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
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
