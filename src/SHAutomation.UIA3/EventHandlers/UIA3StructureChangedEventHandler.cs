using System;
using StreetsHeaver.SHAutomation.Core;
using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.Definitions;
using StreetsHeaver.SHAutomation.Core.EventHandlers;
using UIA = Interop.UIAutomationClient;

namespace StreetsHeaver.SHAutomation.UIA3.EventHandlers
{
    /// <summary>
    /// UIA3 implementation of a structure changed event handler.
    /// </summary>
    public class UIA3StructureChangedEventHandler : StructureChangedEventHandlerBase, UIA.IUIAutomationStructureChangedEventHandler
    {
        public UIA3StructureChangedEventHandler(FrameworkAutomationElementBase frameworkElement, Action<SHAutomationElement, StructureChangeType, int[]> callAction) : base(frameworkElement, callAction)
        {
        }

        public void HandleStructureChangedEvent(UIA.IUIAutomationElement sender, UIA.StructureChangeType changeType, int[] runtimeId)
        {
            var frameworkElement = new UIA3FrameworkAutomationElement((UIA3Automation)Automation, sender);
            var senderElement = new SHAutomationElement(frameworkElement);
            HandleStructureChangedEvent(senderElement, (StructureChangeType)changeType, runtimeId);
        }
    }
}
