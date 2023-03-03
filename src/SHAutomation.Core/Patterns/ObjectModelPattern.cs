using StreetsHeaver.SHAutomation.Core.Patterns.Infrastructure;

namespace StreetsHeaver.SHAutomation.Core.Patterns
{
    public interface IObjectModelPattern : IPattern
    {
        object GetUnderlyingObjectModel();
    }
}
