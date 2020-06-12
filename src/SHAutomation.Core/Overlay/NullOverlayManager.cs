using System.Drawing;
using System.Threading;

namespace SHAutomation.Core.Overlay
{
    /// <summary>
    /// An overlay manager that does nothing.
    /// </summary>
    public class NullOverlayManager : IOverlayManager
    {
        public int Size { get; set; }
        public int Margin { get; set; }

        public void Dispose()
        {
            // Noop
        }

        public void Show(Rectangle rectangle, Color color, int durationInMs,string displayText = null)
        {
            // Noop
        }

        public void ShowBlocking(Rectangle rectangle, Color color, int durationInMs, string displayText = null)
        {
            Thread.Sleep(durationInMs);
        }
    }
}
