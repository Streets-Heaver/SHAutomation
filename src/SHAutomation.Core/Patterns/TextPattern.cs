using System.Drawing;
using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.Definitions;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Patterns.Infrastructure;

namespace StreetsHeaver.SHAutomation.Core.Patterns
{
    public interface ITextPattern : IPattern
    {
        ITextPatternEventIds EventIds { get; }

        ITextRange DocumentRange { get; }
        SupportedTextSelection SupportedTextSelection { get; }

        ITextRange[] GetSelection();
        ITextRange[] GetVisibleRanges();
        ITextRange RangeFromChild(SHAutomationElement child);
        ITextRange RangeFromPoint(Point point);
    }

    public interface ITextPatternEventIds
    {
        EventId TextChangedEvent { get; }
        EventId TextSelectionChangedEvent { get; }
    }

    public abstract class TextPatternBase<TNativePattern> : PatternBase<TNativePattern>, ITextPattern
        where TNativePattern : class
    {
        protected TextPatternBase(FrameworkAutomationElementBase frameworkAutomationElement, TNativePattern nativePattern) : base(frameworkAutomationElement, nativePattern)
        {
        }

        public ITextPatternEventIds EventIds => Automation.EventLibrary.Text;

        public abstract ITextRange DocumentRange { get; }
        public abstract SupportedTextSelection SupportedTextSelection { get; }

        public abstract ITextRange[] GetSelection();
        public abstract ITextRange[] GetVisibleRanges();
        public abstract ITextRange RangeFromChild(SHAutomationElement child);
        public abstract ITextRange RangeFromPoint(Point point);
    }
}
