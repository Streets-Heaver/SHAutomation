using StreetsHeaver.SHAutomation.Core.Identifiers;

namespace StreetsHeaver.SHAutomation.Core.Patterns
{
    public interface ITextEditPattern : ITextPattern
    {
        new ITextEditPatternEventIds EventIds { get; }

        ITextRange GetActiveComposition();
        ITextRange GetConversionTarget();
    }

    public interface ITextEditPatternEventIds : ITextPatternEventIds
    {
        EventId ConversionTargetChangedEvent { get; }
        EventId TextChangedEvent2 { get; }
    }
}
