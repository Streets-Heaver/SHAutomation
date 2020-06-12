using SHAutomation.Core.AutomationElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHAutomation.Core.StaticClasses
{
    public static class ElementHighlighter
    {
        public static bool UseElementHighlighter = false;
        public static Color HighlightColour = Color.PaleVioletRed;
        public static void HighlightElement(SHAutomationElement sHAutomationElement)
        {
            if (UseElementHighlighter)
            {
                try
                {
                    if (sHAutomationElement.SupportsBoundingRectangle)
                    {
                        sHAutomationElement.DrawHighlight(true, HighlightColour, TimeSpan.FromMilliseconds(100));
                    }
                }
                catch
                {

                }
            }
        }
    }
}
