using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StreetsHeaver.SHAutomation.Core.StaticClasses;
using StreetsHeaver.SHAutomation.UIA3;
using System;

namespace StreetsHeaver.SHAutomation.Core.Tests.UI
{
    [TestClass]

    public class ExistsTests : UITestBase
    {


        [TestMethod]
        public void ReturnsFalseIfTimeoutElapsesBeforeElementFound_Exists_BeFalse()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            {
                calc.GetMainWindow().Exists(a => a.ByAutomationId("num343Button"), TimeSpan.FromMilliseconds(200)).Should().BeFalse();

            }
        }

        [TestMethod]
        public void TimeoutFailureIsWithinExpectedValue_Exists_BeTrue()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            {
                PerformanceDiagnostics.Time(() =>
                {
                    calc.GetMainWindow().Exists(a => a.ByAutomationId("num343Button"), TimeSpan.FromSeconds(1));
                }).Should().BeCloseTo(TimeSpan.FromSeconds(1), TimeSpan.FromMilliseconds(300));

            }
        }


    }
}
