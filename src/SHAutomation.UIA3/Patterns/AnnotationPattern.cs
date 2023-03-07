using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns;
using StreetsHeaver.SHAutomation.UIA3.Converters;
using StreetsHeaver.SHAutomation.UIA3.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.Patterns
{
    public class AnnotationPattern : AnnotationPatternBase<UIA.IUIAutomationAnnotationPattern>
    {
        public static readonly PatternId Pattern = PatternId.Register(UIA.UIA_PatternIds.UIA_AnnotationPatternId, "Annotation", AutomationObjectIds.IsAnnotationPatternAvailableProperty);
        public static readonly PropertyId AnnotationTypeIdProperty = PropertyId.Register(UIA.UIA_PropertyIds.UIA_AnnotationAnnotationTypeIdPropertyId, "AnnotationTypeId");
        public static readonly PropertyId AnnotationTypeNameProperty = PropertyId.Register(UIA.UIA_PropertyIds.UIA_AnnotationAnnotationTypeNamePropertyId, "AnnotationTypeName");
        public static readonly PropertyId AuthorProperty = PropertyId.Register(UIA.UIA_PropertyIds.UIA_AnnotationAuthorPropertyId, "Author");
        public static readonly PropertyId DateTimeProperty = PropertyId.Register(UIA.UIA_PropertyIds.UIA_AnnotationDateTimePropertyId, "DateTime");
        public static readonly PropertyId TargetProperty = PropertyId.Register(UIA.UIA_PropertyIds.UIA_AnnotationTargetPropertyId, "Target").SetConverter(SHAutomationElementConverter.NativeToManaged);

        public AnnotationPattern(FrameworkAutomationElementBase frameworkAutomationElement, UIA.IUIAutomationAnnotationPattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }
    }
}
