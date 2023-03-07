using StreetsHeaver.SHAutomation.Core.Identifiers;

namespace StreetsHeaver.SHAutomation.Core
{
    public interface ITextRange3 : ITextRange2
    {
  
        object[] GetAttributeValues(TextAttributeId[] attributeIds);
    }
}
