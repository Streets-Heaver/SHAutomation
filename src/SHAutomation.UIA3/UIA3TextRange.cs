using System;
using System.Drawing;
using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Tools;
using SHAutomation.UIA3.Converters;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3
{
    public class UIA3TextRange : ITextRange
    {
        public UIA3Automation Automation { get; }

        public UIA.IUIAutomationTextRange NativeRange { get; }

        internal UIA3TextRange(UIA3Automation automation, UIA.IUIAutomationTextRange nativeRange)
        {
            Automation = automation;
            NativeRange = nativeRange;
        }

        public void AddToSelection()
        {
            Com.Call(() => NativeRange.AddToSelection());
        }

        public ITextRange Clone()
        {
            var clonedTextRangeNative = Com.Call(() => NativeRange.Clone());
            return TextRangeConverter.NativeToManaged(Automation, clonedTextRangeNative);
        }

        public bool Compare(ITextRange range)
        {
            var nativeRange = ToNativeRange(range);
            return Com.Call(() => NativeRange.Compare(nativeRange)) != 0;
        }

        public int CompareEndpoints(TextPatternRangeEndpoint srcEndPoint, ITextRange targetRange, TextPatternRangeEndpoint targetEndPoint)
        {
            var nativeRange = ToNativeRange(targetRange);
            return Com.Call(() => NativeRange.CompareEndpoints((UIA.TextPatternRangeEndpoint)srcEndPoint, nativeRange, (UIA.TextPatternRangeEndpoint)targetEndPoint));
        }

        public void ExpandToEnclosingUnit(TextUnit textUnit)
        {
            Com.Call(() => NativeRange.ExpandToEnclosingUnit((UIA.TextUnit)textUnit));
        }

        public ITextRange FindAttribute(TextAttributeId attribute, object value, bool backward)
        {
            var nativeValue = ValueConverter.ToNative(value);
            var nativeTextRange = Com.Call(() => NativeRange.FindAttribute(attribute.Id, nativeValue, backward.ToInt()));
            return TextRangeConverter.NativeToManaged(Automation, nativeTextRange);
        }

        public ITextRange FindText(string text, bool backward, bool ignoreCase)
        {
            var nativeTextRange = Com.Call(() => NativeRange.FindText(text, backward.ToInt(), ignoreCase.ToInt()));
            return TextRangeConverter.NativeToManaged(Automation, nativeTextRange);
        }

        public object GetAttributeValue(TextAttributeId attribute)
        {
            var nativeValue = Com.Call(() => NativeRange.GetAttributeValue(attribute.Id));
            return attribute.Convert<object>(Automation, nativeValue);
        }

        public Rectangle[] GetBoundingRectangles()
        {
            var unrolledRects = Com.Call(() => NativeRange.GetBoundingRectangles()) as double[]; 
            if (unrolledRects == null)
            {
                return null;
            }
            // If unrolledRects is somehow not a multiple of 4, we still will not 
            // overrun it, since (x / 4) * 4 <= x for C# integer math.
            var result = new Rectangle[unrolledRects.Length / 4];
            for (var i = 0; i < result.Length; i++)
            {
                var j = i * 4;
                result[i] = new Rectangle(unrolledRects[j].ToInt(), unrolledRects[j + 1].ToInt(), unrolledRects[j + 2].ToInt(), unrolledRects[j + 3].ToInt());
            }
            return result;
        }

        public SHAutomationElement[] GetChildren()
        {
            var nativeChildren = Com.Call(() => NativeRange.GetChildren());
            return SHAutomationElementConverter.NativeArrayToManaged(Automation, nativeChildren);
        }

        public SHAutomationElement GetEnclosingElement()
        {
            var nativeElement = Com.Call(() => NativeRange.GetEnclosingElement());
            return SHAutomationElementConverter.NativeToManaged(Automation, nativeElement);
        }

        public string GetText(int maxLength)
        {
            return Com.Call(() => NativeRange.GetText(maxLength));
        }

        public int Move(TextUnit unit, int count)
        {
            return Com.Call(() => NativeRange.Move((UIA.TextUnit)unit, count));
        }

        public void MoveEndpointByRange(TextPatternRangeEndpoint srcEndPoint, ITextRange targetRange, TextPatternRangeEndpoint targetEndPoint)
        {
            var nativeRange = ToNativeRange(targetRange);
            Com.Call(() => NativeRange.MoveEndpointByRange((UIA.TextPatternRangeEndpoint)srcEndPoint, nativeRange, (UIA.TextPatternRangeEndpoint)targetEndPoint));
        }

        public int MoveEndpointByUnit(TextPatternRangeEndpoint endpoint, TextUnit unit, int count)
        {
            return Com.Call(() => NativeRange.MoveEndpointByUnit((UIA.TextPatternRangeEndpoint)endpoint, (UIA.TextUnit)unit, count));
        }

        public void RemoveFromSelection()
        {
            Com.Call(() => NativeRange.RemoveFromSelection());
        }

        public void ScrollIntoView(bool alignToTop)
        {
            Com.Call(() => NativeRange.ScrollIntoView(alignToTop.ToInt()));
        }

        public void Select()
        {
            Com.Call(() => NativeRange.Select());
        }

        public UIA3TextRange2 AsTextRange2()
        {
            var nativeRange2 = (UIA.IUIAutomationTextRange2)NativeRange;
            return TextRangeConverter.NativeToManaged(Automation, nativeRange2);
        }

        public UIA3TextRange3 AsTextRange3()
        {
            var nativeRange3 = (UIA.IUIAutomationTextRange3)NativeRange;
            return TextRangeConverter.NativeToManaged(Automation, nativeRange3);
        }

        protected UIA.IUIAutomationTextRange ToNativeRange(ITextRange range)
        {
            var concreteTextRange = range as UIA3TextRange;
            if (concreteTextRange == null)
            {
                throw new Exception("TextRange is no UIA3 TextRange");
            }
            return concreteTextRange.NativeRange;
        }
    }
}
