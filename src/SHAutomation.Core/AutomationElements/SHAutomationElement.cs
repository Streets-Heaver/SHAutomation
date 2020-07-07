using System;
using System.Drawing;
using System.Linq;
using SHAutomation.Core.AutomationElements.Infrastructure;
using SHAutomation.Core.Conditions;
using SHAutomation.Core.Definitions;
using SHAutomation.Core.EventHandlers;
using SHAutomation.Core.Exceptions;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Input;
using SHAutomation.Core.WindowsAPI;
using SHAutomation.Core.StaticClasses;
using System.Globalization;
using SHAutomation.Core.Enums;

namespace SHAutomation.Core.AutomationElements
{
    public partial class SHAutomationElement : IEquatable<SHAutomationElement>, IAutomationElementEventSubscriber, ISHAutomationElement
    {
        public SHAutomationElement(FrameworkAutomationElementBase frameworkAutomationElement)
        {
            FrameworkAutomationElement = frameworkAutomationElement;
            ElementHighlighter.HighlightElement(this);
        }

        /// <summary>
        /// Object which contains the native wrapper element (UIA2 or UIA3) for this element.
        /// </summary>
        public FrameworkAutomationElementBase FrameworkAutomationElement { get; }

        /// <summary>
        /// Get the parent <see cref="AutomationElement"/>.
        /// </summary>
      //  public SHAutomationElement Parent => Automation.TreeWalkerFactory.GetRawViewWalker().GetParent(this);

        /// <summary>
        /// The current used automation object.
        /// </summary>
        public AutomationBase Automation => FrameworkAutomationElement.Automation;

        /// <summary>
        /// Shortcut to the condition factory for the current automation.
        /// </summary>
        public ConditionFactory ConditionFactory => FrameworkAutomationElement.Automation.ConditionFactory;

        /// <summary>
        /// The current <see cref="AutomationType" /> for this element.
        /// </summary>
        public AutomationType AutomationType => FrameworkAutomationElement.Automation.AutomationType;

        /// <summary>
        /// Standard UIA patterns of this element.
        /// </summary>
        public FrameworkAutomationElementBase.IFrameworkPatterns Patterns => FrameworkAutomationElement.Patterns;

        /// <summary>
        /// Standard UIA properties of this element.
        /// </summary>
        public FrameworkAutomationElementBase.IProperties Properties => FrameworkAutomationElement.Properties;

        /// <summary>
        /// A flag that indicates if the element is still available. Can be false if the element is already unloaded from the ui.
        /// </summary>
        public bool IsAvailable
        {
            get
            {
                try
                {
                    // ReSharper disable once UnusedVariable
                    var processId = Properties.ProcessId.Value;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        #region Convenience properties
        /// <summary>
        /// The direct framework type of the element.
        /// Results in "FrameworkType.Unknown" if it couldn't be resolved.
        /// </summary>
        public FrameworkType FrameworkType
        {
            get
            {
                var hasProperty = Properties.FrameworkId.TryGetValue(out string currentFrameworkId);
                return hasProperty ? FrameworkIds.ConvertToFrameworkType(currentFrameworkId) : FrameworkType.Unknown;
            }
        }


        /// <summary>
        /// The processId of the element.
        /// </summary>
        public int ProcessId => Properties.ProcessId.Value;

        /// <summary>
        /// The processId of the element in string format.
        /// </summary>
        public string ProcessIdstring => Properties.ProcessId.Value.ToString();

        /// <summary>
        /// The automation id of the element.
        /// </summary>
        //public string AutomationId => Properties.AutomationId.Value;

        ///// <summary>
        ///// The name of the element.
        ///// </summary>
        //public string Name => Properties.Name.Value;

        ///// <summary>
        ///// The class name of the element.
        ///// </summary>
        //public string ClassName => Properties.ClassName.Value;

        ///// <summary>
        ///// The control type of the element.
        ///// </summary>
        //public ControlType ControlType => Properties.ControlType.Value;

        ///// <summary>
        ///// Flag if the element is enabled or not.
        ///// </summary>
        //public bool IsEnabled => Properties.IsEnabled.Value;

        /// <summary>
        /// Flag if the element off-screen or on-screen(visible).
        /// </summary>
        public bool IsOffscreen => Properties.IsOffscreen.Value;

        /// <summary>
        /// The bounding rectangle of this element.
        /// </summary>
        public Rectangle BoundingRectangle => Properties.BoundingRectangle.ValueOrDefault;

        /// <summary>
        /// The width of this element.
        /// </summary>
        public double ActualWidth => BoundingRectangle.Width;

        /// <summary>
        /// The height of this element.
        /// </summary>
        public double ActualHeight => BoundingRectangle.Height;

        /// <summary>
        /// The item status of this element.
        /// </summary>
        public string ItemStatus => Properties.ItemStatus.Value;

        /// <summary>
        /// The help text of this element.
        /// </summary>
        // public string HelpText => Properties.HelpText.Value;
        #endregion Convenience properties


        public void Click(int mouseSpeed = 5)
        {
            Click(MouseAction.LeftClick, mouseSpeed);
        }
        public void Click(MouseAction buttonToPress, int mouseSpeed = 5)
        {
            HoverOver(mouseSpeed);
            MouseHelpers.MouseClick(buttonToPress);
        }

        public void HoverOver(int mouseSpeed = 5)
        {
            SHSpinWait.SpinUntil(() => SupportsBoundingRectangle, 5000);
            if (SupportsBoundingRectangle)
            {
                SHSpinWait.SpinUntil(() => !BoundingRectangle.IsEmpty, 10000);
                if (!BoundingRectangle.IsEmpty)
                {
                    MouseHelpers.MouseMoveTo(Centre(), mouseSpeed);
                }
                else
                {
                    throw new InvalidOperationException("HoverOver bounding rectangle failed to load or is not supported (You may need an onscreen check)");
                }
            }
            else
            {
                throw new ElementNotFoundException("HoverOver bounding rectangle is not supported");
            }
        }

        public void TryFocus()
        {
            try
            {
                Focus();
            }
            catch
            {

            }
        }

        public Point Centre()
        {
            return new Point((Properties.BoundingRectangle.ValueOrDefault.Left + Properties.BoundingRectangle.ValueOrDefault.Right) / 2, (Properties.BoundingRectangle.ValueOrDefault.Top + Properties.BoundingRectangle.ValueOrDefault.Bottom) / 2);
        }
        public Point CentreLeft()
        {
            return new Point(Properties.BoundingRectangle.ValueOrDefault.Left, (Properties.BoundingRectangle.ValueOrDefault.Top + Properties.BoundingRectangle.ValueOrDefault.Bottom) / 2);
        }

        public Point CentreRight()
        {
            return new Point(Properties.BoundingRectangle.ValueOrDefault.Right, (Properties.BoundingRectangle.ValueOrDefault.Top + Properties.BoundingRectangle.ValueOrDefault.Bottom) / 2);
        }

        public Point CentreTop()
        {
            return new Point((Properties.BoundingRectangle.ValueOrDefault.Left + Properties.BoundingRectangle.ValueOrDefault.Right) / 2, Properties.BoundingRectangle.ValueOrDefault.Top);
        }
        public Point CentreBottom()
        {
            return new Point((Properties.BoundingRectangle.ValueOrDefault.Left + Properties.BoundingRectangle.ValueOrDefault.Right) / 2, Properties.BoundingRectangle.ValueOrDefault.Bottom);
        }
        public Point TopLeft()
        {
            return new Point(Properties.BoundingRectangle.ValueOrDefault.Left, Properties.BoundingRectangle.ValueOrDefault.Top);
        }
        public Point TopRight()
        {
            return new Point(Properties.BoundingRectangle.ValueOrDefault.Right, Properties.BoundingRectangle.ValueOrDefault.Top);
        }
        public Point BottomLeft()
        {
            return new Point(Properties.BoundingRectangle.ValueOrDefault.Left, Properties.BoundingRectangle.ValueOrDefault.Bottom);
        }
        public Point BottomRight()
        {
            return new Point(Properties.BoundingRectangle.ValueOrDefault.Right, Properties.BoundingRectangle.ValueOrDefault.Bottom);
        }

        private void PerformMouseAction(bool moveMouse, Action action)
        {
            var clickablePoint = GetClickablePoint();
            if (moveMouse)
            {
                Mouse.MoveTo(clickablePoint);
            }
            else
            {
                Mouse.Position = clickablePoint;
            }
            action();
            Wait.UntilInputIsProcessed();
        }

        /// <summary>
        /// Sets the focus to a control. If the control is a window, brings it to the foreground.
        /// </summary>
        public void Focus()
        {
            if (ControlType == ControlType.Window)
            {
                SetForeground();
            }
            else
            {
                FocusNative();
            }
        }

        /// <summary>
        /// Sets the focus by using the Win32 SetFocus() method.
        /// </summary>
        public void FocusNative()
        {
            if (Properties.NativeWindowHandle.IsSupported)
            {
                var windowHandle = Properties.NativeWindowHandle.ValueOrDefault;
                if (windowHandle != new IntPtr(0))
                {
                    User32.SetFocus(windowHandle);
                    Wait.UntilResponsive(this);
                    return;
                }
            }
            // Fallback to the UIA Version
            SetFocus();
        }

        /// <summary>
        /// Brings a window to the foreground.
        /// </summary>
        public void SetForeground()
        {
            if (Properties.NativeWindowHandle.IsSupported)
            {
                var windowHandle = Properties.NativeWindowHandle.ValueOrDefault;
                if (windowHandle != new IntPtr(0))
                {
                    User32.SetForegroundWindow(windowHandle);
                    Wait.UntilResponsive(this);
                    return;
                }
            }
            // Fallback to the UIA Version
            SetFocus();
        }

        /// <summary>
        /// Captures the object as screenshot in <see cref="Bitmap"/> format.
        /// </summary>
        public Bitmap Capture()
        {
            return Capturing.Capture.Element(this).Bitmap;
        }

#if NETFRAMEWORK
        /// <summary>
        /// Captures the object as screenshot in a WPF friendly <see cref="System.Windows.Media.Imaging.BitmapImage"/> format.
        /// </summary>
        System.Windows.Media.Imaging.BitmapImage CaptureWpf()
        {
            return Capturing.Capture.Element(this).BitmapImage;
        }
#endif

        /// <summary>
        /// Captures the object as screenshot directly into the given file.
        /// </summary>
        /// <param name="filePath">The filepath where the screenshot should be saved.</param>
        public void CaptureToFile(string filePath)
        {
            Capturing.Capture.Element(this).ToFile(filePath);
        }

        /// <summary>
        /// Gets a clickable point of the element.
        /// </summary>
        /// <exception cref="Exceptions.NoClickablePointException">Thrown when no clickable point was found</exception>
        public Point GetClickablePoint()
        {
            return FrameworkAutomationElement.GetClickablePoint();
        }

        /// <summary>
        /// Tries to get a clickable point of the element.
        /// </summary>
        /// <param name="point">The clickable point or null, if no point was found</param>
        /// <returns>True if a point was found, false otherwise</returns>
        public bool TryGetClickablePoint(out Point point)
        {
            return FrameworkAutomationElement.TryGetClickablePoint(out point);
        }

        /// <inheritdoc />
        public ActiveTextPositionChangedEventHandlerBase RegisterActiveTextPositionChangedEvent(TreeScope treeScope, Action<SHAutomationElement, ITextRange> action)
        {
            return FrameworkAutomationElement.RegisterActiveTextPositionChangedEvent(treeScope, action);
        }

        /// <inheritdoc />
        public AutomationEventHandlerBase RegisterAutomationEvent(EventId @event, TreeScope treeScope, Action<SHAutomationElement, EventId> action)
        {
            if (Equals(@event, EventId.NotSupportedByFramework))
            {
                throw new NotSupportedByFrameworkException();
            }
            return FrameworkAutomationElement.RegisterAutomationEvent(@event, treeScope, action);
        }

        /// <inheritdoc />
        public PropertyChangedEventHandlerBase RegisterPropertyChangedEvent(TreeScope treeScope, Action<SHAutomationElement, PropertyId, object> action, params PropertyId[] properties)
        {
            return FrameworkAutomationElement.RegisterPropertyChangedEvent(treeScope, action, properties);
        }

        /// <inheritdoc />
        public StructureChangedEventHandlerBase RegisterStructureChangedEvent(TreeScope treeScope, Action<SHAutomationElement, StructureChangeType, int[]> action)
        {
            return FrameworkAutomationElement.RegisterStructureChangedEvent(treeScope, action);
        }

        /// <inheritdoc />
        public NotificationEventHandlerBase RegisterNotificationEvent(TreeScope treeScope, Action<SHAutomationElement, NotificationKind, NotificationProcessing, string, string> action)
        {
            return FrameworkAutomationElement.RegisterNotificationEvent(treeScope, action);
        }

        /// <inheritdoc />
        public TextEditTextChangedEventHandlerBase RegisterTextEditTextChangedEventHandler(TreeScope treeScope, TextEditChangeType textEditChangeType, Action<SHAutomationElement, TextEditChangeType, string[]> action)
        {
            return FrameworkAutomationElement.RegisterTextEditTextChangedEventHandler(treeScope, textEditChangeType, action);
        }

        /// <summary>
        /// Gets the available patterns for an element via properties.
        /// </summary>
        public PatternId[] GetSupportedPatterns()
        {
            return Automation.PatternLibrary.AllForCurrentFramework.Where(IsPatternSupported).ToArray();
        }

        /// <summary>
        /// Checks if the given pattern is available for the element via properties.
        /// </summary>
        public bool IsPatternSupported(PatternId pattern)
        {
            if (Equals(pattern, PatternId.NotSupportedByFramework))
            {
                return false;
            }
            if (pattern.AvailabilityProperty == null)
            {
                throw new ArgumentException("Pattern doesn't have an AvailabilityProperty");
            }
            var success = FrameworkAutomationElement.TryGetPropertyValue(pattern.AvailabilityProperty, out bool isPatternAvailable);
            return success && isPatternAvailable;
        }

        /// <summary>
        /// Gets the available patterns for an element via UIA method.
        /// Does not work with cached elements and might be unreliable.
        /// </summary>
        public PatternId[] GetSupportedPatternsDirect()
        {
            return FrameworkAutomationElement.GetSupportedPatterns();
        }

        /// <summary>
        /// Checks if the given pattern is available for the element via UIA method.
        /// Does not work with cached elements and might be unreliable.
        /// </summary>
        public bool IsPatternSupportedDirect(PatternId pattern)
        {
            return GetSupportedPatternsDirect().Contains(pattern);
        }

        /// <summary>
        /// Gets the available properties for an element via UIA method.
        /// Does not work with cached elements and might be unreliable.
        /// </summary>
        public PropertyId[] GetSupportedPropertiesDirect()
        {
            return FrameworkAutomationElement.GetSupportedProperties();
        }

        /// <summary>
        /// Method to check if the element supports the given property via UIA method.
        /// Does not work with cached elements and might be unreliable.
        /// </summary>
        public bool IsPropertySupportedDirect(PropertyId property)
        {
            return GetSupportedPropertiesDirect().Contains(property);
        }

        /// <summary>
        /// Gets metadata from the UI Automation element that indicates how the information should be interpreted. 
        /// </summary>
        /// <param name="targetId">The property to retrieve.</param>
        /// <param name="metadataId">Specifies the type of metadata to retrieve.</param>
        /// <returns>The metadata.</returns>
        public object GetCurrentMetadataValue(PropertyId targetId, int metadataId)
        {
            return FrameworkAutomationElement.GetCurrentMetadataValue(targetId, metadataId);
        }

        /// <summary>
        /// Compares two elements.
        /// </summary>
        public bool Equals(SHAutomationElement other)
        {
            return other != null && Automation.Compare(this, other);
        }

        /// <inheritdoc />
        public new bool Equals(object obj)
        {
            return Equals(obj as SHAutomationElement);
        }

        /// <inheritdoc />
        public new int GetHashCode()
        {
            return FrameworkAutomationElement?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Overrides the string representation of the element with something useful.
        /// </summary>
        public new string ToString()
        {
            return string.Format("AutomationId:{0}, Name:{1}, ControlType:{2}, FrameworkId:{3}",
                Properties.AutomationId.ValueOrDefault, Properties.Name.ValueOrDefault,
                Properties.LocalizedControlType.ValueOrDefault, Properties.FrameworkId.ValueOrDefault);
        }

        /// <summary>
        /// Executes the given action on the given pattern.
        /// </summary>
        /// <typeparam name="TPattern">The type of the pattern.</typeparam>
        /// <param name="pattern">The pattern.</param>
        /// <param name="throwIfNotSupported">Flag to indicate if an exception should be thrown if the pattern is not supported.</param>
        /// <param name="action">The action to execute on the pattern</param>
        protected internal void ExecuteInPattern<TPattern>(TPattern pattern, bool throwIfNotSupported, Action<TPattern> action)
        {
            if (pattern != null)
            {
                action(pattern);
            }
            else if (throwIfNotSupported)
            {
                throw new System.NotSupportedException();
            }
        }

        /// <summary>
        /// Executes the given func on the given pattern returning the received value.
        /// </summary>
        /// <typeparam name="TPattern">The type of the pattern.</typeparam>
        /// <typeparam name="TRet">The type of the return value.</typeparam>
        /// <param name="pattern">Zhe pattern.</param>
        /// <param name="throwIfNotSupported">Flag to indicate if an exception should be thrown if the pattern is not supported.</param>
        /// <param name="func">The function to execute on the pattern.</param>
        /// <returns>The value received from the pattern or the default if the pattern is not supported.</returns>
        protected internal TRet ExecuteInPattern<TPattern, TRet>(TPattern pattern, bool throwIfNotSupported, Func<TPattern, TRet> func)
        {
            if (pattern != null)
            {
                return func(pattern);
            }
            if (throwIfNotSupported)
            {
                throw new System.NotSupportedException();
            }
            return default;
        }

        /// <summary>
        /// Sets focus onto control using UIA native element
        /// </summary>
        protected virtual void SetFocus()
        {
            FrameworkAutomationElement.SetFocus();
        }

        #region Properties
        public ISHAutomationElement Parent => Automation?.TreeWalkerFactory?.GetRawViewWalker()?.GetParent(this)?.FrameworkAutomationElement != null ? Automation.TreeWalkerFactory.GetRawViewWalker().GetParent(this) : null;

        public string Name
        {
            get
            {
                try
                {
                    if (Properties != null && Properties.Name != null && Properties.Name.IsSupported)
                    {
                        return Properties.Name.Value;
                    }
                    else
                        return null;
                }
                catch // com exception
                {
                    return null;
                }
            }
        }

        public string AutomationId
        {
            get
            {
                try
                {
                    if (Properties != null && Properties.AutomationId != null && Properties.AutomationId.IsSupported)
                    {
                        return Properties.AutomationId.Value;
                    }
                    else
                        return null;
                }
                catch // com exception
                {
                    return null;
                }
            }
        }

        public string ClassName
        {
            get
            {
                try
                {
                    if (Properties != null && Properties.ClassName != null && Properties.ClassName.IsSupported)
                    {
                        return Properties.ClassName.Value;
                    }
                    else
                        return null;
                }
                catch // com exception
                {
                    return null;
                }
            }
        }

        public string HelpText
        {
            get
            {
                try
                {
                    if (Properties != null && Properties.HelpText != null && Properties.HelpText.IsSupported)
                    {
                        return Properties.HelpText.Value;
                    }
                    else
                        return null;
                }
                catch // com exception
                {
                    return null;
                }
            }
        }

        public bool IsEnabled
        {
            get
            {
                try
                {
                    if (Properties != null && Properties.IsEnabled != null && Properties.IsEnabled.IsSupported)
                    {
                        return Properties.IsEnabled.Value;
                    }
                    else
                        return false;
                }
                catch // com exception
                {
                    return false;
                }
            }
        }


        public bool IsOnscreen
        {
            get
            {
                try
                {
                    if (Properties != null && Properties.IsOffscreen != null && Properties.IsOffscreen.IsSupported)
                    {
                        return !Properties.IsOffscreen.Value;
                    }
                    else
                        return false;
                }
                catch // com exception
                {
                    return false;
                }
            }
        }

        public ControlType ControlType
        {
            get
            {
                try
                {
                    if (Properties != null && Properties.ControlType != null && Properties.ControlType.IsSupported)
                    {
                        return Properties.ControlType.Value;
                    }
                    else
                        return ControlType.Unknown;
                }
                catch // com exception
                {
                    return ControlType.Unknown;
                }
            }
        }

        public Point? ClickablePoint
        {
            get
            {
                try
                {
                    if (Properties != null && Properties.ControlType != null && Properties.ControlType.IsSupported)
                    {
                        return Properties.ClickablePoint.Value;
                    }
                    else
                        return null;
                }
                catch // com exception
                {
                    return null;
                }
            }
        }

        public bool SupportsName
        {
            get
            {
                try
                {
                    return Properties.Name.IsSupported;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool SupportsOnscreen
        {
            get
            {
                try
                {
                    return Properties.IsOffscreen.IsSupported;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool SupportsAutomationId
        {
            get
            {
                try
                {
                    return Properties.AutomationId.IsSupported;
                }
                catch
                {
                    return false;
                }
            }
        }



        public bool SupportsControlType
        {
            get
            {
                try
                {
                    return Properties.ControlType.IsSupported;
                }
                catch
                {
                    return false;
                }
            }
        }



        public bool SupportsEnabled
        {
            get
            {
                try
                {
                    return Properties.IsEnabled.IsSupported;
                }
                catch
                {
                    return false;
                }
            }
        }


        public bool SupportsClassName
        {
            get
            {
                try
                {
                    return Properties.ClassName.IsSupported;
                }
                catch
                {
                    return false;
                }
            }
        }



        public bool SupportsHelpText
        {
            get
            {
                try
                {
                    return Properties.HelpText.IsSupported;
                }
                catch
                {
                    return false;
                }
            }
        }


        public bool SupportsClickablePoint
        {
            get
            {
                try
                {
                    return Properties.ClickablePoint.IsSupported;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool SupportsTogglePattern
        {
            get
            {
                try
                {
                    return Patterns.Toggle.PatternOrDefault.ToggleState.IsSupported;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool SupportsBoundingRectangle
        {
            get
            {
                try
                {
                    return Properties.BoundingRectangle.IsSupported;
                }
                catch
                {
                    return false;
                }
            }
        }


        #endregion

        #region WaitUntilProperty

        public void WaitUntilPropertyEquals(PropertyId property, string expected, int timeout = 10000)
        {
            object value = string.Empty;
            SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value.ToString() == expected, timeout);
        }
        public void WaitUntilPropertyEquals(PropertyId property, double expected, int timeout = 10000)
        {
            object value = null;
            SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as double? == expected, timeout);
        }
        public void WaitUntilPropertyEquals(PropertyId property, bool expected, int timeout = 10000)
        {
            object value = null;
            if (property == PropertyId.Register(AutomationType.UIA3, 30022, "IsOffscreen"))
            {
                SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as bool? == !expected, timeout);
            }
            else
            {
                SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as bool? == expected, timeout);
            }
        }
        public void WaitUntilPropertyEquals(PropertyId property, Rectangle expected, int timeout = 10000)
        {
            object value = null;
            SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as Rectangle? == expected, timeout);
        }
        public void WaitUntilPropertyEquals(PropertyId property, int expected, int timeout = 10000)
        {
            object value = null;
            SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as int? == expected, timeout);
        }

        public void WaitUntilPropertyEquals(PropertyId property, int[] expected, int timeout = 10000)
        {
            object value = new int[0];
            SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as int[] == expected, timeout);
        }
        public void WaitUntilPropertyEquals(PropertyId property, VisualEffects expected, int timeout = 10000)
        {
            object value = null;
            SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as VisualEffects? == expected, timeout);
        }
        public void WaitUntilPropertyEquals(PropertyId property, OrientationType expected, int timeout = 10000)
        {
            object value = null;
            SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as OrientationType? == expected, timeout);
        }

        public void WaitUntilPropertyEquals(PropertyId property, LiveSetting expected, int timeout = 10000)
        {
            object value = null;
            SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as LiveSetting? == expected, timeout);
        }
        public void WaitUntilPropertyEquals(PropertyId property, IntPtr expected, int timeout = 10000)
        {
            object value = null;
            SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as IntPtr? == expected, timeout);
        }
        public void WaitUntilPropertyEquals(PropertyId property, SHAutomationElement expected, int timeout = 10000)
        {
            object value = null;
            SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as SHAutomationElement == expected, timeout);
        }
        public void WaitUntilPropertyEquals(PropertyId property, CultureInfo expected, int timeout = 10000)
        {
            object value = null;
            SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as CultureInfo == expected, timeout);
        }
        public void WaitUntilPropertyEquals(PropertyId property, ControlType expected, int timeout = 10000)
        {
            object value = null;
            SHSpinWait.SpinUntil(() => FrameworkAutomationElement.TryGetPropertyValue(property, out value) && value as ControlType? == expected, timeout);
        }


        #endregion
    }
}
