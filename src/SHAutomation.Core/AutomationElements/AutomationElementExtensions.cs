using System;
using System.Drawing;
using SHAutomation.Core.StaticClasses;
using SHAutomation.Core.Tools;

namespace SHAutomation.Core.AutomationElements
{
    /// <summary>
    /// Contains extension methods for <see cref="AutomationElement"/>s.
    /// </summary>
    public static partial class SHAutomationElementExtensions
    {
        /// <summary>
        /// Draws a red highlight around the element.
        /// </summary>
        public static T DrawHighlight<T>(this T self) where T : SHAutomationElement
        {
            return DrawHighlight(self, Color.Red);
        }

        /// <summary>
        /// Draws a manually colored highlight around the element.
        /// </summary>
        public static T DrawHighlight<T>(this T self, Color color) where T : SHAutomationElement
        {
            return DrawHighlight(self, true, color);
        }

        /// <summary>
        /// Draw a highlight around the element with the given settings.
        /// </summary>
        /// <param name="blocking">Flag to indicate if further execution waits until the highlight is removed.</param>
        /// <param name="color">The color to draw the highlight.</param>
        /// <param name="duration">The duration how long the highlight is shown.</param>
        /// <remarks>Override for winforms color.</remarks>
        public static T DrawHighlight<T>(this T self, bool blocking, Color color, TimeSpan? duration = null, string displayText = null) where T : SHAutomationElement
        {
            var rectangle = self.Properties.BoundingRectangle.Value;
            if (!rectangle.IsEmpty)
            {
                var durationInMs = (int)(duration ?? TimeSpan.FromSeconds(2)).TotalMilliseconds;
                if (blocking)
                {
                    self.Automation.OverlayManager.ShowBlocking(rectangle, color, durationInMs, displayText);
                }
                else
                {
                    self.Automation.OverlayManager.Show(rectangle, color, durationInMs, displayText);
                }
            }
            return self;
        }

        /// <summary>
        /// Waits until the element has a clickable point.
        /// </summary>
        public static T WaitUntilClickable<T>(this T self, TimeSpan? timeout = null) where T : SHAutomationElement
        {
            if (self != null)
            {
                SHSpinWait.SpinUntil(() => self.TryGetClickablePoint(out var _), timeout.HasValue ? timeout.Value : TimeSpan.FromMilliseconds(500));
            }
            return self;
        }

        /// <summary>
        /// Waits until the element is enabled.
        /// </summary>
        public static T WaitUntilEnabled<T>(this T self, TimeSpan? timeout = null) where T : SHAutomationElement
        {
            if (self != null)
            {
                SHSpinWait.SpinUntil(() => self.IsEnabled, timeout.HasValue ? timeout.Value : TimeSpan.FromMilliseconds(500));
            }
            return self;
        }
    }
}
