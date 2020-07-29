using SHAutomation.Core.Logging;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace SHAutomation.Core.StaticClasses
{
    public static class PerformanceDiagnostics
    {
        public static TimeSpan Time(Action action)
        {
            return Time(action, null, null);
        }
        public static TimeSpan Time(Action action, ILoggingService loggingService)
        {
            return Time(action, null, loggingService, null);
        }

        public static TimeSpan Time(Action action, string descriptor, ILoggingService loggingService)
        {
            return Time(action, descriptor, loggingService, null);
        }

        public static TimeSpan Time(Action action, string descriptor, ILoggingService loggingService, double? warnMilliseconds)
        {
            string logDescription = string.IsNullOrEmpty(descriptor) ?  Regex.Match(action.Method.Name, @"\<([^>]*)\>").Groups[1].Value : descriptor;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            action.Invoke();
            stopwatch.Stop();

            if (loggingService != null)
            {
                loggingService.Info(logDescription + " took " + stopwatch.Elapsed.TotalSeconds + " to execute");

                if (warnMilliseconds.HasValue && stopwatch.Elapsed.TotalMilliseconds > warnMilliseconds.Value)
                    loggingService.Warn(logDescription + " breached limit of " + warnMilliseconds.Value + " milliseconds. Took " + stopwatch.Elapsed.TotalMilliseconds + " to execute");
            }


            return stopwatch.Elapsed;

        }
    }
}
