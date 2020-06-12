using SHAutomation.Core;
using SHAutomation.Core.Tools;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3
{
    public class UIA3TextRange2 : UIA3TextRange, ITextRange2
    {
        public UIA.IUIAutomationTextRange2 NativeRange2 { get; }

        public UIA3TextRange2(UIA3Automation automation, UIA.IUIAutomationTextRange2 nativeRange)
            : base(automation, nativeRange)
        {
            NativeRange2 = nativeRange;
        }

        public void ShowContextMenu()
        {
            Com.Call(() => NativeRange2.ShowContextMenu());
        }
    }
}
