using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.UIA3.Converters;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3
{
    /// <summary>
    /// Class for a UIA3 tree walker.
    /// </summary>
    public class UIA3TreeWalker : ITreeWalker
    {
        /// <summary>
        /// Creates a UIA3 tree walker.
        /// </summary>
        public UIA3TreeWalker(UIA3Automation automation, UIA.IUIAutomationTreeWalker nativeTreeWalker)
        {
            Automation = automation;
            NativeTreeWalker = nativeTreeWalker;
        }

        /// <summary>
        /// The current <see cref="AutomationBase"/> object.
        /// </summary>
        public UIA3Automation Automation { get; }

        /// <summary>
        /// The native tree walker object.
        /// </summary>
        public UIA.IUIAutomationTreeWalker NativeTreeWalker { get; }

        /// <inheritdoc />
        public SHAutomationElement GetParent(SHAutomationElement element)
        {
            var parent =
                NativeTreeWalker.GetParentElement(SHAutomationElementConverter.ToNative(element));
            return Automation.WrapNativeElement(parent);
        }

        /// <inheritdoc />
        public SHAutomationElement GetFirstChild(SHAutomationElement element)
        {
            var child =
                NativeTreeWalker.GetFirstChildElement(SHAutomationElementConverter.ToNative(element));
             
            return Automation.WrapNativeElement(child);
        }

        /// <inheritdoc />
        public SHAutomationElement GetLastChild(SHAutomationElement element)
        {
            var child =
                NativeTreeWalker.GetLastChildElement(SHAutomationElementConverter.ToNative(element));
              
            return Automation.WrapNativeElement(child);
        }

        /// <inheritdoc />
        public SHAutomationElement GetNextSibling(SHAutomationElement element)
        {
            var child =
                NativeTreeWalker.GetNextSiblingElement(SHAutomationElementConverter.ToNative(element));
              
            return Automation.WrapNativeElement(child);
        }

        /// <inheritdoc />
        public SHAutomationElement GetPreviousSibling(SHAutomationElement element)
        {
            var child =
                NativeTreeWalker.GetPreviousSiblingElement(SHAutomationElementConverter.ToNative(element));
               
            return Automation.WrapNativeElement(child);
        }
    }
}
