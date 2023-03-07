using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.UIA3.Converters;
using System;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3
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
            try
            {
                var parent =
                NativeTreeWalker.GetParentElement(SHAutomationElementConverter.ToNative(element));
                return Automation.WrapNativeElement(parent);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public SHAutomationElement GetFirstChild(SHAutomationElement element)
        {
            try
            {
                var child =
                NativeTreeWalker.GetFirstChildElement(SHAutomationElementConverter.ToNative(element));

                return Automation.WrapNativeElement(child);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public SHAutomationElement GetLastChild(SHAutomationElement element)
        {
            try
            {
                var child =
                NativeTreeWalker.GetLastChildElement(SHAutomationElementConverter.ToNative(element));

                return Automation.WrapNativeElement(child);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public SHAutomationElement GetNextSibling(SHAutomationElement element)
        {
            try
            {
                var child =
                    NativeTreeWalker.GetNextSiblingElement(SHAutomationElementConverter.ToNative(element));

                return Automation.WrapNativeElement(child);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public SHAutomationElement GetPreviousSibling(SHAutomationElement element)
        {
            try
            {
                var child =
                NativeTreeWalker.GetPreviousSiblingElement(SHAutomationElementConverter.ToNative(element));

                return Automation.WrapNativeElement(child);
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }
    }
}
