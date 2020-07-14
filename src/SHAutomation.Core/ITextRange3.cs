using SHAutomation.Core.Identifiers;

namespace SHAutomation.Core
{
    public interface ITextRange3 : ITextRange2
    {
  
        object[] GetAttributeValues(TextAttributeId[] attributeIds);
    }
}
