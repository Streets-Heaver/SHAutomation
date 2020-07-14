using System;
using System.Threading;

namespace SHAutomation.Core.StaticClasses
{
    public static class SHSpinWait
    {
        public static bool SpinUntil(Func<bool> condition, int timeout)
        {
            if (timeout <= 0)
                throw new InvalidOperationException("TIMEOUT HAS TO BE GREATER THAN 0");
            DateTime date = DateTime.Now;
            SpinWait.SpinUntil(() => (DateTime.Now > date.AddMilliseconds(timeout) || condition.Invoke()), timeout);
            return false;
        }

        public static bool SpinUntil(Func<bool> condition, TimeSpan timeout)
        {
            if (timeout.TotalMilliseconds <= 0)
                throw new InvalidOperationException("TIMEOUT HAS TO BE GREATER THAN 0");

            return SpinWait.SpinUntil(condition, timeout);
        }
    }
}
