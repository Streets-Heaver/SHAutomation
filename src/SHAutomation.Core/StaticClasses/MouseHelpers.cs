using System;
using System.Collections.Generic;
using System.Linq;
using SHAutomation.Core.Input;
using System.Threading;
using System.Drawing;
using SHAutomation.Core.Tools;
using SHAutomation.Core.Enums;
using SHAutomation.Core.WindowsAPI;

namespace SHAutomation.Core.StaticClasses
{
    public static class MouseHelpers
    {
        /// <summary>
        /// The current position of the mouse cursor.
        /// </summary>
        public static Point Position
        {
            get
            {
                User32.GetCursorPos(out var point);
                return new Point(point.X, point.Y);
            }
            set
            {
                User32.SetCursorPos(value.X, value.Y);
                // There is a bug that in a multi-monitor scenario with different sizes,
                // the mouse is only moved to x=0 on the target monitor.
                // In that case, just redo the move a 2nd time and it works
                // as the mouse is on the correct monitor alreay.
                // See https://stackoverflow.com/questions/58753372/winapi-setcursorpos-seems-like-not-working-properly-on-multiple-monitors-with-di
                User32.GetCursorPos(out var point);
                if (point.X != value.X || point.Y != value.Y)
                {
                    User32.SetCursorPos(value.X, value.Y);
                }
            }
        }

        /// <summary>
        /// The number of pixels the mouse is moved per millisecond.
        /// Used to calculate the duration of a mouse move.
        /// </summary>
        public static double MovePixelsPerMillisecond { get; set; } = 2;

        /// <summary>
        /// The number of pixels the mouse is moved per step.
        /// Used to calculate the interval of a mouse move.
        /// </summary>
        public static double MovePixelsPerStep { get; set; } = 10;

        /// <summary>
        /// Moves the mouse to a new position.
        /// </summary>
        /// <param name="newX">The new position on the x-axis.</param>
        /// <param name="newY">The new position on the y-axis.</param>
        public static void MoveTo(int newX, int newY)
        {
            // Get starting position
            var startPos = Position;
            var endPos = new Point(newX, newY);

            // Break out if there is no positional change
            if (startPos == endPos)
            {
                return;
            }

            // Calculate some values for duration and interval
            var totalDistance = startPos.Distance(newX, newY);
            var duration = TimeSpan.FromMilliseconds(Convert.ToInt32(totalDistance / MovePixelsPerMillisecond));
            var steps = Math.Max(Convert.ToInt32(totalDistance / MovePixelsPerStep), 1); // Make sure to have et least one step
            var interval = TimeSpan.FromMilliseconds(duration.TotalMilliseconds / steps);

            // Execute the movement
            Interpolation.Execute(point => { Position = point; }, startPos, endPos, duration, interval, true);
            Wait.UntilInputIsProcessed();
        }

        /// <summary>
        /// Moves the mouse to a new position.
        /// </summary>
        /// <param name="newPosition">The new position for the mouse.</param>
        public static void MouseMoveTo(Point newPosition, double mouseSpeed = 1)
        {
            double currentSpeed = MovePixelsPerMillisecond;
            MovePixelsPerMillisecond = mouseSpeed;
            MoveTo(newPosition.X, newPosition.Y);
            MovePixelsPerMillisecond = currentSpeed;
        }

        public static void MouseClick(MouseAction button)
        {
            switch (button)
            {
                case MouseAction.LeftClick:
                    Mouse.Click(MouseButton.Left);
                    break;
                case MouseAction.RightClick:
                    Mouse.Click(MouseButton.Right);
                    break;
                case MouseAction.DoubleLeftClick:
                    Mouse.LeftDoubleClick();
                    break;
                case MouseAction.DoubleRightClick:
                    Mouse.RightDoubleClick();
                    break;
            }
        }
    }
}
