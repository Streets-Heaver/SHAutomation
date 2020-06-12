using System;
using System.Collections.Generic;
using System.Text;

namespace SHAutomation.Core.WindowsAPI
{
    public static class Delegates
    {
        public delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref RECT lprcMonitor, IntPtr dwData);
    }
}
