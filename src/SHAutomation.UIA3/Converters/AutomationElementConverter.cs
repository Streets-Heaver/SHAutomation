using System;
using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3.Converters
{
    public static class SHAutomationElementConverter
    {
        public static SHAutomationElement[] NativeArrayToManaged(AutomationBase automation, object nativeElements)
        {
            if (nativeElements == null)
            {
                return Array.Empty<SHAutomationElement>(); 
            }
            var uia3Automation = (UIA3Automation)automation;
            var nativeElementsCasted = (UIA.IUIAutomationElementArray)nativeElements;
            var retArray = new SHAutomationElement[nativeElementsCasted.Length];
            for (var i = 0; i < nativeElementsCasted.Length; i++)
            {
                var nativeElement = nativeElementsCasted.GetElement(i);
                var SHAutomationElement = uia3Automation.WrapNativeElement(nativeElement);
                retArray[i] =SHAutomationElement;
            }
            return retArray;
        }

        public static SHAutomationElement NativeToManaged(AutomationBase automation, object nativeElement)
        {
            var uia3Automation = (UIA3Automation)automation;
            return uia3Automation.WrapNativeElement((UIA.IUIAutomationElement)nativeElement);
        }

        public static UIA.IUIAutomationElement ToNative(SHAutomationElement sHAutomationElement)
        {
            if (sHAutomationElement?.FrameworkAutomationElement == null)
            {
                return null;
            }
            var frameworkElement = sHAutomationElement.FrameworkAutomationElement as UIA3FrameworkAutomationElement;
            if (frameworkElement == null)
            {
                throw new Exception("Element is not an UIA3 element");
            }
            return frameworkElement.NativeElement;
        }
    }
}
