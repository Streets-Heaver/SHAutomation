#if NETFRAMEWORK || NETCOREAPP
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using SHAutomation.Core.WindowsAPI;
using System.Windows.Forms;

namespace SHAutomation.Core.Overlay
{
    public class WinFormsOverlayManager : IOverlayManager
    {
        public int Size { get; set; }
        public int Margin { get; set; }

        public WinFormsOverlayManager()
        {
            Size = 3;
            Margin = 0;
        }

        public void Show(Rectangle rectangle, Color color, int durationInMs, string displayText = null)
        {
            if (!rectangle.IsEmpty)
            {
#if NET35
                new Thread(() =>
                {
                    CreateAndShowForms(rectangle, color, durationInMs);
                }).Start();
#elif NET40
                System.Threading.Tasks.Task.Factory.StartNew(() => {
                    CreateAndShowForms(rectangle, color, durationInMs);
                });
#else
                System.Threading.Tasks.Task.Run(() => CreateAndShowForms(rectangle, color, durationInMs, displayText));
#endif
            }
        }

        public void ShowBlocking(Rectangle rectangle, Color color, int durationInMs, string displayText = null)
        {
            CreateAndShowForms(rectangle, color, durationInMs, displayText);
        }

        private void CreateAndShowForms(Rectangle rectangle, Color color, int durationInMs,string displayText = null)
        {
            var leftBorder = new Rectangle(rectangle.X - Margin, rectangle.Y - Margin, Size, rectangle.Height + 2 * Margin);
            var topBorder = new Rectangle(rectangle.X - Margin, rectangle.Y - Margin, rectangle.Width + 2 * Margin, Size);
            var rightBorder = new Rectangle(rectangle.X + rectangle.Width - Size + Margin, rectangle.Y - Margin, Size, rectangle.Height + 2 * Margin);
            var bottomBorder = new Rectangle(rectangle.X - Margin, rectangle.Y + rectangle.Height - Size + Margin, rectangle.Width + 2 * Margin, Size);
            var allBorders = new[] { leftBorder, topBorder, rightBorder, bottomBorder };

            var gdiColor = Color.FromArgb(color.A, color.R, color.G, color.B);
            var forms = new List<OverlayRectangleForm>();
            foreach (var border in allBorders)
            {
                var form = new OverlayRectangleForm { BackColor = gdiColor };
                if (displayText != null)
                {
                    TextBox myText = new TextBox();
                    myText.Text = displayText;
                    myText.Location = new Point(25, 25);
                    form.Controls.Add(myText);
                }
                forms.Add(form);
                // Position the window
                User32.SetWindowPos(form.Handle, new IntPtr(-1), border.X, border.Y,
                    border.Width, border.Height, SetWindowPosFlags.SWP_NOACTIVATE);
                // Show the window
                User32.ShowWindow(form.Handle, ShowWindowTypes.SW_SHOWNA);
            }
            Thread.Sleep(durationInMs);
            foreach (var form in forms)
            {
                // Cleanup
                form.Hide();
                form.Close();
                form.Dispose();
            }
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}
#endif
