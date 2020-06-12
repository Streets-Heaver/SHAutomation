using System.Drawing;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.Identifiers;

namespace SHAutomation.Core
{
    public interface ITextRange
    {
        void AddToSelection();
        ITextRange Clone();
        bool Compare(ITextRange range);
        int CompareEndpoints(TextPatternRangeEndpoint srcEndPoint, ITextRange targetRange, TextPatternRangeEndpoint targetEndPoint);
        void ExpandToEnclosingUnit(TextUnit textUnit);
        ITextRange FindAttribute(TextAttributeId attribute, object value, bool backward);
        ITextRange FindText(string text, bool backward, bool ignoreCase);
        object GetAttributeValue(TextAttributeId attribute);
        Rectangle[] GetBoundingRectangles();
      SHAutomationElement[] GetChildren();
      SHAutomationElement GetEnclosingElement();
        string GetText(int maxLength);
        int Move(TextUnit unit, int count);
        void MoveEndpointByRange(TextPatternRangeEndpoint srcEndPoint, ITextRange targetRange, TextPatternRangeEndpoint targetEndPoint);
        int MoveEndpointByUnit(TextPatternRangeEndpoint endpoint, TextUnit unit, int count);
        void RemoveFromSelection();
        void ScrollIntoView(bool alignToTop);
        void Select();
    }
}
