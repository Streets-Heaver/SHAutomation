using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StreetsHeaver.SHAutomation.Core.AutomationElements;
using StreetsHeaver.SHAutomation.Core.Identifiers;
using StreetsHeaver.SHAutomation.Core.Input;
using StreetsHeaver.SHAutomation.UIA3;
using System;

namespace StreetsHeaver.SHAutomation.Core.Tests.UI
{
    [TestClass]

    public class SHAutomationElementTests : UITestBase
    {


        [TestMethod]
        public void StringRepresentationReturnedCorrectly_ToString_ShouldBeCorrectString()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            {
                var window = calc.GetMainWindow();

                window.Find("num3Button").ToString().Should().Be("AutomationId:num3Button, Name:Three, ControlType:button, FrameworkId:XAML");
            }

        }
        [TestMethod]
        public void TimeOutNotOverloadedFindFirstByXPath_FindFirstByXPath_Be()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            {
                var window = calc.GetMainWindow();
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
            {
                var window = calc.GetMainWindow();

                var item = window.FindFirstByXPath("//*[@AutomationId='num3Button']", TimeSpan.FromSeconds(10));
                item.AutomationId.Should().Be("num3Button");
                item.FoundByXPath.Should().BeTrue();
            }
        }
        [TestMethod]
        public void TimeOutWaitsFullAmountFindFirstByXPath_FindFirstByXPath_Be()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            {
                var window = calc.GetMainWindow();
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
            {
                var window = calc.GetMainWindow();
                var pad = window.Find(x => x.ByAutomationId("NumberPad"));
                var offscreenValue = pad.GetCurrentPropertyValue(PropertyId.Register(30022, "IsOffscreen"));

                ((bool)offscreenValue == false).Should().BeTrue();
            }
        }

        [TestMethod]
        public void ReturnsUpdatedValue_GetCurrentPropertyValue_BeTrue()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            {
                var window = calc.GetMainWindow();
                window.Find(x => x.ByAutomationId("num3Button")).Click();
                var results = window.Find(x => x.ByAutomationId("CalculatorResults"));
                var resultsName = results.Name;
                window.Find(x => x.ByAutomationId("clearEntryButton")).Click();

                results.WaitUntilPropertyEquals(PropertyId.Register(30005, "Name"), "Display is 0", TimeSpan.FromMilliseconds(500));
                var nameValue = results.GetCurrentPropertyValue(PropertyId.Register(30005, "Name"));

                ((string)nameValue != resultsName).Should().BeTrue();
            }
        }

        [TestMethod]
        public void ElementIsClickedSuccessfully_Click_ShouldBeTrue()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            {
                var window = calc.GetMainWindow();
                window.Find(x => x.ByAutomationId("num1Button")).Click();
                window.Find(x => x.ByAutomationId("num2Button")).Click();
                window.Find(x => x.ByAutomationId("num3Button")).Click();
                window.Find(x => x.ByAutomationId("num4Button")).Click();
                window.Find(x => x.ByAutomationId("num5Button")).Click();
                window.Find(x => x.ByAutomationId("num6Button")).Click();
                window.Find(x => x.ByAutomationId("num7Button")).Click();
                window.Find(x => x.ByAutomationId("num8Button")).Click();
                window.Find(x => x.ByAutomationId("num9Button")).Click();
                window.Find(x => x.ByAutomationId("num1Button")).Click();
                window.Find(x => x.ByAutomationId("num2Button")).Click();
                window.Find(x => x.ByAutomationId("num3Button")).Click();
                window.Find(x => x.ByAutomationId("num4Button")).Click();
                window.Find(x => x.ByAutomationId("num5Button")).Click();
                window.Find(x => x.ByAutomationId("num6Button")).Click();
                window.Find(x => x.ByAutomationId("num7Button")).Click();
                window.Find(x => x.ByAutomationId("num8Button")).Click();
                window.Find(x => x.ByAutomationId("num9Button")).Click();

                window.Find("CalculatorResults").Name.Should().Be("Display is 1,234,567,891,234,567");

            }
        }

        [TestMethod]
        public void ActionsTest()
        {
            using (var calc = Application.Launch("calc"))
            {
                var window = calc.GetMainWindow();
                window.Find(x => x.ByName("6")).Click();


                window.Find(x => x.ByName("Result")).AsTextBox().Text.Should().Be("6");

            }
        }

        [TestMethod]
        public void HoverOverMovesMouse_HoverOver_Be()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            {
                var window = calc.GetMainWindow();
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
            {
                var window = calc.GetMainWindow();
                var btn = window.Find("PaneTitleTextBlock");
                Action act = () => btn.HoverOver();
                act.Should().Throw<InvalidOperationException>().WithMessage("HoverOver bounding rectangle is not supported for AutomationId:PaneTitleTextBlock, Name:, ControlType:text, FrameworkId:XAML");
            }
        }

    }
}
