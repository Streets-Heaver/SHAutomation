using System;
using System.Collections.Generic;
using System.Linq;
using SHAutomation.Core.Input;
using System.Threading;
using System.Drawing;
using SHAutomation.Core.Tools;
using SHAutomation.Core.Enums;

namespace SHAutomation.Core.StaticClasses
{
    public static class MouseHelpers
    {
        public static void MouseMoveTo(Point pos, int mouseSpeed = 1)
        {

            if (mouseSpeed != 0)
            {
                // Get starting position
                var startPos = Mouse.Position;
                var startX = startPos.X;
                var startY = startPos.Y;

                // Prepare variables
                var totalDistance = startPos.Distance(pos.X, pos.Y);

                // Calculate the duration for the speed
                var optimalPixelsPerMillisecond = mouseSpeed;
                var minDuration = 1;
                var maxDuration = 500;
                var duration = Convert.ToInt32(totalDistance / optimalPixelsPerMillisecond).Clamp(minDuration, maxDuration);

                // Calculate the steps for the smoothness
                var optimalPixelsPerStep = mouseSpeed;
                var minSteps = 10;
                var maxSteps = 50;
                var steps = Convert.ToInt32(totalDistance / optimalPixelsPerStep).Clamp(minSteps, maxSteps);

                // Calculate the interval and the step size
                var interval = duration / steps;
                var stepX = (pos.X - startX) / steps;
                var stepY = (pos.Y - startY) / steps;

                // Build a list of movement points (except the last one, to set that one perfectly)
                var movements = new List<Point>();
                for (var i = 1; i < steps; i++)
                {
                    var tempX = (double)startX + i * stepX;
                    var tempY = (double)startY + i * stepY;
                    movements.Add(new Point(tempX.ToInt(), tempY.ToInt()));
                }

                // Add an exact point for the last one, if it does not fit exactly
                var lastPoint = movements.Last();
                if (lastPoint.X != pos.X || lastPoint.Y != pos.Y)
                {
                    movements.Add(new Point(pos.X, pos.Y));
                }

                // Loop thru the steps and set them
                foreach (var point in movements)
                {
                    Mouse.Position = point;
                    Thread.Sleep(interval);
                }
            }
            else
                Mouse.Position = pos;


            Wait.UntilInputIsProcessed();
            SHSpinWait.SpinUntil(() => (Mouse.Position.X == pos.X) && (Mouse.Position.Y == pos.Y), 5000);
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
