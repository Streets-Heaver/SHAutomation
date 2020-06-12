using System;
using System.Drawing;
using System.Runtime.InteropServices;
using SHAutomation.Core;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.EventHandlers;
using SHAutomation.Core.Tools;
using SHAutomation.UIA3.Converters;
using SHAutomation.UIA3.EventHandlers;
using SHAutomation.UIA3.Extensions;
using UIA = Interop.UIAutomationClient;

namespace SHAutomation.UIA3
{
    /// <summary>
    /// Automation implementation for UIA3.
    /// </summary>
    public class UIA3Automation : AutomationBase
    {
        public UIA3Automation() : base(new UIA3PropertyLibrary(), new UIA3EventLibrary(), new UIA3PatternLibrary(), new UIA3TextAttributeLibrary())
        {
            NativeAutomation = InitializeAutomation();
            TreeWalkerFactory = new UIA3TreeWalkerFactory(this);
        }

        /// <inheritdoc />
        public override ITreeWalkerFactory TreeWalkerFactory { get; }

        /// <inheritdoc />
        public override AutomationType AutomationType => AutomationType.UIA3;

        /// <inheritdoc />
        public override object NotSupportedValue => NativeAutomation.ReservedNotSupportedValue;

        /// <inheritdoc />
        public override TimeSpan TransactionTimeout
        {
            get => TimeSpan.FromMilliseconds(NativeAutomation2.TransactionTimeout);
            set => NativeAutomation2.TransactionTimeout = (uint)value.TotalMilliseconds;
        }

        /// <inheritdoc />
        public override TimeSpan ConnectionTimeout
        {
            get => TimeSpan.FromMilliseconds(NativeAutomation2.ConnectionTimeout);
            set => NativeAutomation2.ConnectionTimeout = (uint)value.TotalMilliseconds;
        }

        /// <inheritdoc />
        public override ConnectionRecoveryBehaviorOptions ConnectionRecoveryBehavior
        {
            get => (ConnectionRecoveryBehaviorOptions)NativeAutomation6.ConnectionRecoveryBehavior;
            set => NativeAutomation6.ConnectionRecoveryBehavior = (UIA.ConnectionRecoveryBehaviorOptions)value;
        }

        /// <inheritdoc />
        public override CoalesceEventsOptions CoalesceEvents
        {
            get => (CoalesceEventsOptions)NativeAutomation6.CoalesceEvents;
            set => NativeAutomation6.CoalesceEvents = (UIA.CoalesceEventsOptions)value;
        }
        /// <summary>
        /// Native object for the ui automation.
        /// </summary>
        public UIA.IUIAutomation NativeAutomation { get; }

        /// <summary>
        /// Native object for Windows 8 automation.
        /// </summary>
        public UIA.IUIAutomation2 NativeAutomation2 => GetAutomationAs<UIA.IUIAutomation2>();

        /// <summary>
        /// Native object for Windows 8.1 automation.
        /// </summary>
        public UIA.IUIAutomation3 NativeAutomation3 => GetAutomationAs<UIA.IUIAutomation3>();

        /// <summary>
        /// Native object for Windows 10 automation.
        /// </summary>
        public UIA.IUIAutomation4 NativeAutomation4 => GetAutomationAs<UIA.IUIAutomation4>();

        /// <summary>
        /// Native object for second Windows 10 automation.
        /// </summary>
        public UIA.IUIAutomation5 NativeAutomation5 => GetAutomationAs<UIA.IUIAutomation5>();
        /// <summary>
        /// Native object for third Windows 10 automation.
        /// </summary>
        public UIA.IUIAutomation6 NativeAutomation6 => GetAutomationAs<UIA.IUIAutomation6>();

        /// <inheritdoc />
        public override SHAutomationElement GetDesktop()
        {
            return Com.Call(() =>
            {
                var desktop = CacheRequest.IsCachingActive
                    ? NativeAutomation.GetRootElementBuildCache(CacheRequest.Current.ToNative(this))
                    : NativeAutomation.GetRootElement();
                return WrapNativeElement(desktop);
            });
        }

        /// <inheritdoc />
        public override SHAutomationElement FromPoint(Point point)
        {
            try
            {
                return Com.Call(() =>
                {
                    var nativePoint = point.ToTagPoint();
                    var nativeElement = CacheRequest.IsCachingActive
                        ? NativeAutomation.ElementFromPointBuildCache(nativePoint, CacheRequest.Current.ToNative(this))
                        : NativeAutomation.ElementFromPoint(nativePoint);
                    return WrapNativeElement(nativeElement);
                });
            }
            catch (TimeoutException)
            {
                return null;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public override SHAutomationElement FromHandle(IntPtr hwnd)
        {
            return Com.Call(() =>
            {
                var nativeElement = CacheRequest.IsCachingActive
                    ? NativeAutomation.ElementFromHandleBuildCache(hwnd, CacheRequest.Current.ToNative(this))
                    : NativeAutomation.ElementFromHandle(hwnd);
                return WrapNativeElement(nativeElement);
            });
        }

        /// <inheritdoc />
        public override SHAutomationElement FocusedElement()
        {
            return Com.Call(() =>
            {
                var nativeElement = CacheRequest.IsCachingActive
                    ? NativeAutomation.GetFocusedElementBuildCache(CacheRequest.Current.ToNative(this))
                    : NativeAutomation.GetFocusedElement();
                return WrapNativeElement(nativeElement);
            });
        }

        /// <inheritdoc />
        public override FocusChangedEventHandlerBase RegisterFocusChangedEvent(Action<SHAutomationElement> action)
        {
            var eventHandler = new UIA3FocusChangedEventHandler(this, action);
            Com.Call(() => NativeAutomation.AddFocusChangedEventHandler(null, eventHandler));
            return eventHandler;
        }

        /// <inheritdoc />
        public override void UnregisterFocusChangedEvent(FocusChangedEventHandlerBase eventHandler)
        {
            NativeAutomation.RemoveFocusChangedEventHandler((UIA3FocusChangedEventHandler)eventHandler);
        }

        /// <inheritdoc />
        public override void UnregisterAllEvents()
        {
            try
            {
                NativeAutomation.RemoveAllEventHandlers();
            }
            catch
            {
                // Noop
            }
        }

        /// <inheritdoc />
        public override bool Compare(SHAutomationElement element1,SHAutomationElement element2)
        {
            return NativeAutomation.CompareElements(SHAutomationElementConverter.ToNative(element1), SHAutomationElementConverter.ToNative(element2)) != 0;
        }

        /// <summary>
        /// Initializes the automation object with the correct instance.
        /// </summary>
        private UIA.IUIAutomation InitializeAutomation()
        {
            UIA.IUIAutomation nativeAutomation;
            // Try CUIAutomation8 (Windows 8 or higher)
            try
            {
                nativeAutomation = new UIA.CUIAutomation8();
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                // Fall back to CUIAutomation
                nativeAutomation = new UIA.CUIAutomation();
            }
            return nativeAutomation;
        }

        /// <summary>
        /// Tries to cast the automation to a specific interface.
        /// Throws an exception if that is not possible.
        /// </summary>
        private T GetAutomationAs<T>() where T : class, UIA.IUIAutomation
        {
            var element = NativeAutomation as T;
            if (element == null)
            {
                throw new NotSupportedException($"OS does not have {typeof(T).Name} support.");
            }
            return element;
        }

        public SHAutomationElement WrapNativeElement(UIA.IUIAutomationElement nativeElement)
        {
            return nativeElement == null ? null : new SHAutomationElement(new UIA3FrameworkAutomationElement(this, nativeElement));
        }
        public SHAutomationElement GetParent(SHAutomationElement element, Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            UIA.IUIAutomationCondition nativeCondition = ConditionConverter.ToNative(this, conditionFunc(new ConditionFactory(PropertyLibrary)));
            UIA.IUIAutomationTreeWalker treeWalker = NativeAutomation.CreateTreeWalker(nativeCondition);

            UIA.IUIAutomationCacheRequest cacheRequestTest =
                NativeAutomation.CreateCacheRequest();

            cacheRequestTest.AutomationElementMode = UIA.AutomationElementMode.AutomationElementMode_Full;

            UIA3FrameworkAutomationElement temp = (element.FrameworkAutomationElement as UIA3FrameworkAutomationElement);

            UIA.IUIAutomationElement elementAncestor =
                treeWalker.NormalizeElementBuildCache(temp.NativeElement, cacheRequestTest);

            if (elementAncestor != null)
            {
              SHAutomationElement ancestor = WrapNativeElement(elementAncestor as UIA.IUIAutomationElement);
                return ancestor;
            }
            else
            {
                return null;
            }
        }
        public SHAutomationElement GetContainingWindow(ISHAutomationElement element)
        {
            Func<ConditionFactory, ConditionBase> condition = x => x.ByControlType(ControlType.Window);
            return GetParent(element as SHAutomationElement, condition);
        }
    }
}
