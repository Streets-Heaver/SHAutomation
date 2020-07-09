using SHAutomation.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SHAutomation.Core.StaticClasses
{
    public static class Diagnostics
    {
        public static TimeSpan Time(Action action, ILoggingService loggingService)
        {
            return Time(action, loggingService, null);
        }
        public static TimeSpan Time(Action action, ILoggingService loggingService, double? warnMilliseconds)
        {
            string methodName = Regex.Match(action.Method.Name, @"\<([^>]*)\>").Groups[1].Value;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            action.Invoke();
            stopwatch.Stop();

            loggingService.Info(methodName + " took " + stopwatch.Elapsed.TotalSeconds + " to execute");

            if (warnMilliseconds.HasValue && stopwatch.Elapsed.TotalMilliseconds > warnMilliseconds.Value)
                loggingService.Warn(methodName + " breached limit of " + warnMilliseconds.Value + " milliseconds. Took " + stopwatch.Elapsed.TotalMilliseconds + " to execute");


            return stopwatch.Elapsed;

        }
    }
}
