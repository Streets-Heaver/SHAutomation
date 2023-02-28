using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SHAutomation.Core.Identifiers;
using SHAutomation.Core.Input;
using SHAutomation.Core.WindowsAPI;
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
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                window.Find("num3Button").ToString().Should().Be("AutomationId:num3Button, Name:Three, ControlType:button, FrameworkId:XAML");
            }

        }
        [TestMethod]
        public void TimeOutNotOverloadedFindFirstByXPath_FindFirstByXPath_Be()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                window.Exists(x => x.ByAutomationId("num2Button"), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5)).Should().BeTrue();
                var item = window.FindFirstByXPath("//*[@AutomationId='num3Button']");
                item.AutomationId.Should().Be("num3Button");
                item.FoundByXPath.Should().BeTrue();
            }
        }
        [TestMethod]
        public void TimeOutWithOverloadedFindFirstByXPath_FindFirstByXPath_Be()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var item = window.FindFirstByXPath("//*[@AutomationId='num3Button']", TimeSpan.FromSeconds(10));
                item.AutomationId.Should().Be("num3Button");
                item.FoundByXPath.Should().BeTrue();
            }
        }
        [TestMethod]
        public void TimeOutWaitsFullAmountFindFirstByXPath_FindFirstByXPath_Be()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                DateTime previousTime = DateTime.Now;
                window.FindFirstByXPath("//*[@AutomationId='randomThing']", TimeSpan.FromSeconds(10)).Should().BeNull();
                DateTime.Now.Subtract(previousTime).Should().BeGreaterOrEqualTo(TimeSpan.FromSeconds(9));
                DateTime.Now.Subtract(previousTime).Should().BeLessOrEqualTo(TimeSpan.FromSeconds(12));
            }
        }

        [TestMethod]
        public void ReturnsExpectedValue_GetCurrentPropertyValue_BeTrue()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                var pad = window.Find(x => x.ByAutomationId("NumberPad"));
                var offscreenValue = pad.GetCurrentPropertyValue(PropertyId.Register(AutomationType.UIA3, 30022, "IsOffscreen"));

                ((bool)offscreenValue == false).Should().BeTrue();
            }
        }

        [TestMethod]
        public void ReturnsUpdatedValue_GetCurrentPropertyValue_BeTrue()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                window.Find(x => x.ByAutomationId("num3Button")).Click();
                var results = window.Find(x => x.ByAutomationId("CalculatorResults"));
                var resultsName = results.Name;
                window.Find(x => x.ByAutomationId("clearEntryButton")).Click();

                results.WaitUntilPropertyEquals(PropertyId.Register(AutomationType.UIA3, 30005, "Name"), "Display is 0", TimeSpan.FromMilliseconds(500));
                var nameValue = results.GetCurrentPropertyValue(PropertyId.Register(AutomationType.UIA3, 30005, "Name"));

                ((string)nameValue != resultsName).Should().BeTrue();
            }
        }
        [TestMethod]
        public void HoverOverMovesMouse_HoverOver_Be()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                var btn = window.Find("num3Button");
                btn.HoverOver();
                Mouse.Position.X.Should().BeInRange(btn.CentreLeft().X, btn.CentreRight().X);
                Mouse.Position.Y.Should().BeInRange(btn.CentreTop().Y, btn.CentreBottom().Y);
            }
        }
        [TestMethod]
        public void HoverOverErrorMessageGivesElementDetails_HoverOver_Be()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                var btn = window.Find("PaneTitleTextBlock");
                Action act = () => btn.HoverOver();
                act.Should().Throw<InvalidOperationException>().WithMessage("HoverOver bounding rectangle is not supported for AutomationId:PaneTitleTextBlock, Name:, ControlType:text, FrameworkId:XAML");
            }
        }

    }
}
