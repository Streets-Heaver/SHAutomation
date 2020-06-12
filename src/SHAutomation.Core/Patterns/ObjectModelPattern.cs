using SHAutomation.Core.Patterns.Infrastructure;

namespace SHAutomation.Core.Patterns
{
    public interface IObjectModelPattern : IPattern
    {
        object GetUnderlyingObjectModel();
    }
}
