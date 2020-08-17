using System;
using System.Threading;

namespace SHAutomation.Core.StaticClasses
{
    public static class SHSpinWait
    {
        public static bool SpinUntil(Func<bool> condition, int timeout)
        {
            return SpinUntil(condition, TimeSpan.FromMilliseconds(timeout));
        }

        public static bool SpinUntil(Func<bool> condition, TimeSpan timeout)
        {
            if (timeout.TotalMilliseconds <= 0)
                throw new InvalidOperationException("TIMEOUT HAS TO BE GREATER THAN 0");
            DateTime date = DateTime.Now;

            try
            {
                return SpinWait.SpinUntil(() =>
                {
                    if (DateTime.Now > date.AddMilliseconds(timeout.TotalMilliseconds))
                        throw new TimeoutException();
                    else
                        return condition.Invoke();

                }, timeout);
            }
            catch(TimeoutException)
            {
                return false;
            }
        }
    }
}
