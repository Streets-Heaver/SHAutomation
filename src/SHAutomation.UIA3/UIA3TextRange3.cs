using System;
using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3
{
    public class UIA3TextRange3 : UIA3TextRange2, ITextRange3
    {
        public UIA.IUIAutomationTextRange3 NativeRange3 { get; }

        public UIA3TextRange3(UIA3Automation automation, UIA.IUIAutomationTextRange3 nativeRange)
            : base(automation, nativeRange)
        {
            NativeRange3 = nativeRange;
        }

      
        public object[] GetAttributeValues(TextAttributeId[] attributeIds)
        {
            throw new NotImplementedException("Currently not done as the parameter of the interop is wrong.");
        }
    }
}
