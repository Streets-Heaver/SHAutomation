using System.Drawing;
using System.Drawing.Drawing2D;
using SHAutomation.Core.Tools;

namespace SHAutomation.Core.Capturing
{
    /// <summary>
    /// An overlay to draw the current mouse cursor.
    /// </summary>
    public class MouseOverlay : OverlayBase
    {
        /// <summary>
        /// Creates a <see cref="MouseOverlay"/> object for the current captured image.
        /// </summary>
        /// <param name="captureImage">The captured image.</param>
        /// <returns>The created object.</returns>
        public MouseOverlay(CaptureImage captureImage) : base(captureImage)
        {
        }

        /// <inheritdoc />
        public override void Draw(Graphics g)
        {
            var outputPoint = new Point();
            var cursorBitmap = CaptureUtilities.CaptureCursor(ref outputPoint);

            if (cursorBitmap == null)
            {
                outputPoint = CaptureUtilities.GetMousePosition();

            }
            // Fix the coordinates for multi-screen scenarios
            outputPoint.X -= CaptureImage.OriginalBounds.Left;
            outputPoint.Y -= CaptureImage.OriginalBounds.Top;
            // Check for scaling and handle that
            var scale = CaptureUtilities.GetScale(CaptureImage.OriginalBounds, CaptureImage.Settings);
            if (scale != 1)
            {
                outputPoint.X = (outputPoint.X * scale).ToInt();
                outputPoint.Y = (outputPoint.Y * scale).ToInt();
                var origInterpolationMode = g.InterpolationMode;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                if (cursorBitmap != null)
                    g.DrawImage(cursorBitmap, outputPoint.X, outputPoint.Y, (cursorBitmap.Width * scale).ToInt(),
                        (cursorBitmap.Height * scale).ToInt());
                else
                    g.FillEllipse(new SolidBrush(Color.Red), outputPoint.X, outputPoint.Y, 30, 30);


                g.InterpolationMode = origInterpolationMode;
            }
            else
            {
                if (cursorBitmap != null)
                    g.DrawImage(cursorBitmap, outputPoint.X, outputPoint.Y);
                else
                    g.FillEllipse(new SolidBrush(Color.Red), outputPoint.X, outputPoint.Y, 30, 30);
            }
            // Cleanup
            if (cursorBitmap != null)
                cursorBitmap.Dispose();
        }
    }
}
