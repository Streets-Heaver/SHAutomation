using System;
using System.Drawing;
using System.Linq;
using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.EventHandlers;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Tools;
using SHAutomation.UIA3.Converters;
using SHAutomation.UIA3.EventHandlers;
using SHAutomation.UIA3.Extensions;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3
{
    public partial class UIA3FrameworkAutomationElement : FrameworkAutomationElementBase
    {
        public UIA3FrameworkAutomationElement(UIA3Automation automation, UIA.IUIAutomationElement nativeElement) : base(automation)
        {
            Automation = automation;
            NativeElement = nativeElement;
        }

        /// <summary>
        /// Concrete implementation of the automation object
        /// </summary>
        public new UIA3Automation Automation { get; }

        /// <summary>
        /// Native object for the ui element
        /// </summary>
        public UIA.IUIAutomationElement NativeElement { get; }

        /// <summary>
        /// Native object for Windows 8 ui element
        /// </summary>
        public UIA.IUIAutomationElement2 NativeElement2 => GetAutomationElementAs<UIA.IUIAutomationElement2>();

        /// <summary>
        /// Native object for Windows 8.1 ui element
        /// </summary>
        public UIA.IUIAutomationElement3 NativeElement3 => GetAutomationElementAs<UIA.IUIAutomationElement3>();

        /// <summary>
        /// Native object for Windows 10 ui element
        /// </summary>
        public UIA.IUIAutomationElement4 NativeElement4 => GetAutomationElementAs<UIA.IUIAutomationElement4>();

        /// <summary>
        /// Native object for second Windows 10 ui element
        /// </summary>
        public UIA.IUIAutomationElement5 NativeElement5 => GetAutomationElementAs<UIA.IUIAutomationElement5>();

        /// <summary>
        /// Native object for third Windows 10 ui element
        /// </summary>
        public UIA.IUIAutomationElement6 NativeElement6 => GetAutomationElementAs<UIA.IUIAutomationElement6>();

        /// <summary>
        /// Native object fourth for Windows 10 ui element
        /// </summary>
        public UIA.IUIAutomationElement7 NativeElement7 => GetAutomationElementAs<UIA.IUIAutomationElement7>();
        /// <summary>
        /// Native object for the fifth for Windows 10 ui element.
        /// </summary>
        public UIA.IUIAutomationElement8 NativeElement8 => GetAutomationElementAs<UIA.IUIAutomationElement8>();

        /// <summary>
        /// Native object for the sixth for Windows 10 ui element.
        /// </summary>
        public UIA.IUIAutomationElement9 NativeElement9 => GetAutomationElementAs<UIA.IUIAutomationElement9>();

        public override void SetFocus()
        {
            NativeElement.SetFocus();
        }
        /// <inheritdoc />
        public override ActiveTextPositionChangedEventHandlerBase RegisterActiveTextPositionChangedEventHandler(TreeScope treeScope, Action<SHAutomationElement, ITextRange> action)
        {
            var eventHandler = new UIA3ActiveTextPositionChangedEventHandler(this, action);
            Automation.NativeAutomation6.AddActiveTextPositionChangedEventHandler(NativeElement, (UIA.TreeScope)treeScope, null, eventHandler);
            return eventHandler;
        }

        protected override object InternalGetPropertyValue(int propertyId,  bool useDefaultIfNotSupported)
        {
            var ignoreDefaultValue = useDefaultIfNotSupported ? 0 : 1;
            var returnValue = 
                NativeElement.GetCurrentPropertyValueEx(propertyId, ignoreDefaultValue);
            return returnValue;
        }

        protected override object InternalGetPattern(int patternId)
        {
            var returnedValue = 
                 NativeElement.GetCurrentPattern(patternId);
            return returnedValue;
        }

        /// <inheritdoc />
        public override SHAutomationElement[] FindAll(TreeScope treeScope, ConditionBase condition)
        {
            var nativeFoundElements = NativeElement.FindAll((UIA.TreeScope)treeScope, ConditionConverter.ToNative(Automation, condition));
            return SHAutomationElementConverter.NativeArrayToManaged(Automation, nativeFoundElements);
        }

        /// <inheritdoc />
        public override SHAutomationElement FindFirst(TreeScope treeScope, ConditionBase condition)
        {
            var nativeFoundElement = NativeElement.FindFirst((UIA.TreeScope)treeScope, ConditionConverter.ToNative(Automation, condition));
            return SHAutomationElementConverter.NativeToManaged(Automation, nativeFoundElement);
        }

        /// <inheritdoc />
        public override SHAutomationElement[] FindAllWithOptions(TreeScope treeScope, ConditionBase condition,
            TreeTraversalOption traversalOptions,SHAutomationElement root)
        {
            var nativeFoundElements =  NativeElement7.FindAllWithOptions((UIA.TreeScope)treeScope, ConditionConverter.ToNative(Automation, condition), (UIA.TreeTraversalOptions)traversalOptions, SHAutomationElementConverter.ToNative(root));
            return SHAutomationElementConverter.NativeArrayToManaged(Automation, nativeFoundElements);
        }

        /// <inheritdoc />
        public override SHAutomationElement FindFirstWithOptions(TreeScope treeScope, ConditionBase condition,
            TreeTraversalOption traversalOptions,SHAutomationElement root)
        {
            var nativeFoundElement = NativeElement7.FindFirstWithOptions((UIA.TreeScope)treeScope, ConditionConverter.ToNative(Automation, condition), (UIA.TreeTraversalOptions)traversalOptions, SHAutomationElementConverter.ToNative(root));
            return SHAutomationElementConverter.NativeToManaged(Automation, nativeFoundElement);
        }

        /// <inheritdoc />
        public override SHAutomationElement FindIndexed(TreeScope treeScope, int index, ConditionBase condition)
        {
            var nativeFoundElements =  NativeElement.FindAll((UIA.TreeScope)treeScope, ConditionConverter.ToNative(Automation, condition));
            var nativeElement = nativeFoundElements.GetElement(index);
            return nativeElement == null ? null :SHAutomationElementConverter.NativeToManaged(Automation, nativeElement);
        }

        /// <inheritdoc />
        public override bool TryGetClickablePoint(out Point point)
        {
            var tagPoint = new UIA.tagPOINT { x = 0, y = 0 };
            var success = Com.Call(() => NativeElement.GetClickablePoint(out tagPoint)) != 0;
            if (success)
            {
                point = new Point(tagPoint.x, tagPoint.y);
            }
            else
            {
                success = Properties.ClickablePoint.TryGetValue(out point);
            }
            return success;
        }

        /// <inheritdoc />
        public override ActiveTextPositionChangedEventHandlerBase RegisterActiveTextPositionChangedEvent(TreeScope treeScope, Action<SHAutomationElement, ITextRange> action)
        {
            var eventHandler = new UIA3ActiveTextPositionChangedEventHandler(this, action);
            Automation.NativeAutomation6.AddActiveTextPositionChangedEventHandler(NativeElement, (UIA.TreeScope)treeScope, null, eventHandler);
            return eventHandler;
        }

        /// <inheritdoc />
        public override AutomationEventHandlerBase RegisterAutomationEvent(EventId @event, TreeScope treeScope, Action<SHAutomationElement, EventId> action)
        {
            var eventHandler = new UIA3AutomationEventHandler(this, @event, action);
            Automation.NativeAutomation.AddAutomationEventHandler(@event.Id, NativeElement, (UIA.TreeScope)treeScope, null, eventHandler);
            return eventHandler;
        }
        /// <inheritdoc />
        public override void UnregisterActiveTextPositionChangedEventHandler(ActiveTextPositionChangedEventHandlerBase eventHandler)
        {
            Automation.NativeAutomation6.RemoveActiveTextPositionChangedEventHandler(NativeElement, (UIA3ActiveTextPositionChangedEventHandler)eventHandler);
        }

        /// <inheritdoc />
        public override PropertyChangedEventHandlerBase RegisterPropertyChangedEvent(TreeScope treeScope, Action<SHAutomationElement, PropertyId, object> action, PropertyId[] properties)
        {
            var eventHandler = new UIA3PropertyChangedEventHandler(this, action);
            var propertyIds = properties.Select(p => p.Id).ToArray();
            Automation.NativeAutomation.AddPropertyChangedEventHandler(NativeElement,
                (UIA.TreeScope)treeScope, null, eventHandler, propertyIds);
            return eventHandler;
        }

        /// <inheritdoc />
        public override StructureChangedEventHandlerBase RegisterStructureChangedEvent(TreeScope treeScope, Action<SHAutomationElement, StructureChangeType, int[]> action)
        {
            var eventHandler = new UIA3StructureChangedEventHandler(this, action);
            Automation.NativeAutomation.AddStructureChangedEventHandler(NativeElement, (UIA.TreeScope)treeScope, null, eventHandler);
            return eventHandler;
        }

        /// <inheritdoc />
        public override NotificationEventHandlerBase RegisterNotificationEvent(TreeScope treeScope, Action<SHAutomationElement, NotificationKind, NotificationProcessing, string, string> action)
        {
            var eventHandler = new UIA3NotificationEventHandler(this, action);
            Automation.NativeAutomation5.AddNotificationEventHandler(NativeElement, (UIA.TreeScope)treeScope, null, eventHandler);
            return eventHandler;
        }

        /// <inheritdoc />
        public override TextEditTextChangedEventHandlerBase RegisterTextEditTextChangedEventHandler(TreeScope treeScope, TextEditChangeType textEditChangeType, Action<SHAutomationElement, TextEditChangeType, string[]> action)
        {
            var eventHandler = new UIA3TextEditTextChangedEventHandler(this, action);
            Automation.NativeAutomation3.AddTextEditTextChangedEventHandler(NativeElement, (UIA.TreeScope)treeScope, (UIA.TextEditChangeType)textEditChangeType, null, eventHandler);
            return eventHandler;
        }

        /// <inheritdoc />
        public override void UnregisterAutomationEventHandler(AutomationEventHandlerBase eventHandler)
        {
            var frameworkEventHandler = (UIA3AutomationEventHandler)eventHandler;
            Automation.NativeAutomation.RemoveAutomationEventHandler(frameworkEventHandler.Event.Id, NativeElement, frameworkEventHandler);
        }

        /// <inheritdoc />
        public override void UnregisterPropertyChangedEventHandler(PropertyChangedEventHandlerBase eventHandler)
        {
            Automation.NativeAutomation.RemovePropertyChangedEventHandler(NativeElement, (UIA3PropertyChangedEventHandler)eventHandler);
        }

        /// <inheritdoc />
        public override void UnregisterStructureChangedEventHandler(StructureChangedEventHandlerBase eventHandler)
        {
            Automation.NativeAutomation.RemoveStructureChangedEventHandler(NativeElement, (UIA3StructureChangedEventHandler)eventHandler);
        }

        /// <inheritdoc />
        public override void UnregisterNotificationEventHandler(NotificationEventHandlerBase eventHandler)
        {
            Automation.NativeAutomation5.RemoveNotificationEventHandler(NativeElement, (UIA3NotificationEventHandler)eventHandler);
        }

        /// <inheritdoc />
        public override void UnregisterTextEditTextChangedEventHandler(TextEditTextChangedEventHandlerBase eventHandler)
        {
            Automation.NativeAutomation3.RemoveTextEditTextChangedEventHandler(NativeElement, (UIA3TextEditTextChangedEventHandler)eventHandler);
        }

        public override PatternId[] GetSupportedPatterns()
        {
            Automation.NativeAutomation.PollForPotentialSupportedPatterns(NativeElement, out int[] rawIds, out string[] _);
            return rawIds.Select(id => PatternId.Find(Automation.AutomationType, id)).ToArray();
        }

        public override PropertyId[] GetSupportedProperties()
        {
            Automation.NativeAutomation.PollForPotentialSupportedProperties(NativeElement, out int[] rawIds, out string[] _);
            return rawIds.Select(id => PropertyId.Find(Automation.AutomationType, id)).ToArray();
        }

       
        public override object GetCurrentMetadataValue(PropertyId targetId, int metadataId)
        {
            return NativeElement7.GetCurrentMetadataValue(targetId.Id, metadataId);
        }

        public override int GetHashCode()
        {
            return NativeElement.GetHashCode();
        }

        /// <summary>
        /// Tries to cast the automation element to a specific interface.
        /// Throws an exception if that is not possible.
        /// </summary>
        private T GetAutomationElementAs<T>() where T : class, UIA.IUIAutomationElement
        {
            var element = NativeElement as T;
            if (element == null)
            {
                throw new NotSupportedException($"OS does not have {typeof(T).Name} support.");
            }
            return element;
        }
    }
}
