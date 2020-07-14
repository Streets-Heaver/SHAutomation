using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SHAutomation.UIA3;

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
    }
}
