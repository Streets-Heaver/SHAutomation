using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.UIA3.Converters;
using SHAutomation.UIA3.Extensions;
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
            var parent = CacheRequest.Current == null ?
                NativeTreeWalker.GetParentElement(SHAutomationElementConverter.ToNative(element)) :
                NativeTreeWalker.GetParentElementBuildCache(SHAutomationElementConverter.ToNative(element), CacheRequest.Current.ToNative(Automation));
            return Automation.WrapNativeElement(parent);
        }

        /// <inheritdoc />
        public SHAutomationElement GetFirstChild(SHAutomationElement element)
        {
            var child = CacheRequest.Current == null ?
                NativeTreeWalker.GetFirstChildElement(SHAutomationElementConverter.ToNative(element)) :
                NativeTreeWalker.GetFirstChildElementBuildCache(SHAutomationElementConverter.ToNative(element), CacheRequest.Current.ToNative(Automation));
            return Automation.WrapNativeElement(child);
        }

        /// <inheritdoc />
        public SHAutomationElement GetLastChild(SHAutomationElement element)
        {
            var child = CacheRequest.Current == null ?
                NativeTreeWalker.GetLastChildElement(SHAutomationElementConverter.ToNative(element)) :
                NativeTreeWalker.GetLastChildElementBuildCache(SHAutomationElementConverter.ToNative(element), CacheRequest.Current.ToNative(Automation));
            return Automation.WrapNativeElement(child);
        }

        /// <inheritdoc />
        public SHAutomationElement GetNextSibling(SHAutomationElement element)
        {
            var child = CacheRequest.Current == null ?
                NativeTreeWalker.GetNextSiblingElement(SHAutomationElementConverter.ToNative(element)) :
                NativeTreeWalker.GetNextSiblingElementBuildCache(SHAutomationElementConverter.ToNative(element), CacheRequest.Current.ToNative(Automation));
            return Automation.WrapNativeElement(child);
        }

        /// <inheritdoc />
        public SHAutomationElement GetPreviousSibling(SHAutomationElement element)
        {
            var child = CacheRequest.Current == null ?
                NativeTreeWalker.GetPreviousSiblingElement(SHAutomationElementConverter.ToNative(element)) :
                NativeTreeWalker.GetPreviousSiblingElementBuildCache(SHAutomationElementConverter.ToNative(element), CacheRequest.Current.ToNative(Automation));
            return Automation.WrapNativeElement(child);
        }
    }
}
