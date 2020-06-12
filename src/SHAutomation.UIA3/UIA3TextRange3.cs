using System;
using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Tools;
using SHAutomation.UIA3.Converters;
using SHAutomation.UIA3.Extensions;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3
{
    public class UIA3TextRange3 : UIA3TextRange2, ITextRange3
    {
        public UIA.IUIAutomationTextRange3 NativeRange3 { get; }

        public UIA3TextRange3(UIA3Automation automation, UIA.IUIAutomationTextRange3 nativeRange)
            : base(automation, nativeRange)
        {
            NativeRange3 = nativeRange;
        }

        public SHAutomationElement GetEnclosingElementBuildCache(CacheRequest cacheRequest)
        {
            var nativeCacheRequest = cacheRequest.ToNative(Automation);
            var nativeElement = Com.Call(() => NativeRange3.GetEnclosingElementBuildCache(nativeCacheRequest));
            return SHAutomationElementConverter.NativeToManaged(Automation, nativeElement);
        }

        public SHAutomationElement[] GetChildrenBuildCache(CacheRequest cacheRequest)
        {
            var nativeCacheRequest = cacheRequest.ToNative(Automation);
            var nativeElements = Com.Call(() => NativeRange3.GetChildrenBuildCache(nativeCacheRequest));
            return SHAutomationElementConverter.NativeArrayToManaged(Automation, nativeElements);
        }

        public object[] GetAttributeValues(TextAttributeId[] attributeIds)
        {
            throw new NotImplementedException("Currently not done as the parameter of the interop is wrong.");
        }
    }
}
