using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SHAutomation.UIA3;
using System;

namespace SHAutomation.Core.Tests.UI
{
    [TestClass]

    public class SHAutomationElementTests : UITestBase
    {
      

        [TestMethod]
        public void StringRepresentationReturnedCorrectly_ToString_ShouldBeCorrectString()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            window.Find("num3Button").ToString().Should().Be("AutomationId:num3Button, Name:Three, ControlType:button, FrameworkId:XAML");

        }
        [TestMethod]
        public void TimeOutNotOverloadedFindFirstByXPath_FindFirstByXPath_Be()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            window.FindFirstByXPath("//*[@AutomationId='num3Button']").AutomationId.Should().Be("num3Button");
        }
        [TestMethod]
        public void TimeOutWithOverloadedFindFirstByXPath_FindFirstByXPath_Be()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            window.FindFirstByXPath("//*[@AutomationId='num3Button']",TimeSpan.FromSeconds(10)).AutomationId.Should().Be("num3Button");
        }
        [TestMethod]
        public void TimeOutWaitsFullAmountFindFirstByXPath_FindFirstByXPath_Be()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);
            DateTime previousTime = DateTime.Now;
            window.FindFirstByXPath("//*[@AutomationId='randomThing']", TimeSpan.FromSeconds(10)).Should().BeNull();
            DateTime.Now.Subtract(previousTime).Should().BeGreaterOrEqualTo(TimeSpan.FromSeconds(10));
            DateTime.Now.Subtract(previousTime).Should().BeLessOrEqualTo(TimeSpan.FromSeconds(12));
        }
    }
}
