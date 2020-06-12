using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Identifiers;

namespace SHAutomation.Core
{
    public interface ITextRange3 : ITextRange2
    {
      SHAutomationElement GetEnclosingElementBuildCache(CacheRequest cacheRequest);
      SHAutomationElement[] GetChildrenBuildCache(CacheRequest cacheRequest);
        object[] GetAttributeValues(TextAttributeId[] attributeIds);
    }
}
