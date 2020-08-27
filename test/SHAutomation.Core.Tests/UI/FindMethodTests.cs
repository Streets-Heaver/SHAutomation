using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SHAutomation.Core.Exceptions;
using SHAutomation.Core.StaticClasses;
using SHAutomation.UIA3;
using System;
using System.Linq;

namespace SHAutomation.Core.Tests.UI
{
    [TestClass]

    public class FindMethodTests : UITestBase
    {
        [TestMethod]
        public void ElementFoundUsingAutomationIdAndCache_Find_NotBeNull()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            window.GetXPathCache(TestContext.TestName);

            var num3Button = window.Find("num3Button");
            num3Button.Should().NotBeNull();

            window.SaveXPathCache(TestContext.TestName);
        }

        [TestMethod]
        public void NewXPathIsSaved_Find_BeEquivalentTo()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            var num3Button = window.Find("num3Button");
            
            window.SaveXPathCache(TestContext.TestName);
            window.GetXPathCache(TestContext.TestName);
            window.XPathList.First().Should().BeEquivalentTo(("num3Button", "AutomationId", "/Group/Group[5]/Button[@AutomationId='num3Button']"));
        }


        [TestMethod]
        public void FoundElementXPathStoredInCache_Find_BeEquivalentTo()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);


            var num3Button = window.Find("num3Button");
            window.XPathList.Should().BeEquivalentTo((identifier: "num3Button", property: "AutomationId", xpath: "/Group/Group[5]/Button[@AutomationId='num3Button']"));

        }

        [TestMethod]
        public void ElementFoundUsingConditionAndCache_Find_NotBeNull()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            window.GetXPathCache(TestContext.TestName);

            var num3Button = window.Find(x => x.ByAutomationId("num3Button"));
            num3Button.Should().NotBeNull();

            window.SaveXPathCache(TestContext.TestName);

        }

        [TestMethod]
        public void ElementFoundUsingAutomationId_Find_NotBeNull()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            var num3Button = window.Find("num3Button");
            num3Button.Should().NotBeNull();

        }


        [TestMethod]
        public void ElementFoundUsingCondition_Find_NotBeNull()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            var num3Button = window.Find(x => x.ByAutomationId("num3Button"));
            num3Button.Should().NotBeNull();

        }
        [TestMethod]
        public void ElementFoundUsingParent_Find_NotBeNull()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);
            var numberPad = window.Find(x => x.ByAutomationId("NumberPad"));
            var num3Button = window.Find(x => x.ByAutomationId("num3Button"),parent: numberPad);
            num3Button.Should().NotBeNull();
        }
        [TestMethod]
        public void ElementNotFoundWaitsForTimeout_Find_BeGreaterOrEqualTo()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);
            Action act = () => window.Find(x => x.ByAutomationId("nonExistentId"), timeout: 5000);
            var elapsed = PerformanceDiagnostics.Time(() =>
            {
                act.Should().Throw<ElementNotFoundException>();
            });
            elapsed.Should().BeGreaterOrEqualTo(TimeSpan.FromSeconds(5)).And.BeLessOrEqualTo(TimeSpan.FromSeconds(6));
        }
        [TestMethod]
        public void ElementNotFoundUsingParentWaitsForTimeout_Find_BeGreaterOrEqualTo()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);
            var numberPad = window.Find(x => x.ByAutomationId("NumberPad"));
            Action act = () => window.Find(x => x.ByAutomationId("nonExistentId"), parent: numberPad, timeout: 5000);
            var elapsed = PerformanceDiagnostics.Time(() =>
            {
                act.Should().Throw<ElementNotFoundException>();
            });
            elapsed.Should().BeGreaterOrEqualTo(TimeSpan.FromSeconds(5)).And.BeLessOrEqualTo(TimeSpan.FromSeconds(6));
        }
        [TestMethod]
        public void ElementFound_FindAllByXPath_NotBeNull()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            var num3Button = window.FindAllByXPath("/Group/Group[5]/Button[@AutomationId='num3Button']");
            num3Button.Should().NotBeNull();

        }

        [TestMethod]
        public void ElementFound_FindFirstDescendant_NotBeNull()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            var num3Button = window.FindFirstDescendant("num3Button");
            num3Button.Should().NotBeNull();

        }

        [TestMethod]
        public void FindsSingleElement_FindAllDescendants_BeOne()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            var num3Button = window.FindAllDescendants(x => x.ByAutomationId("num3Button"));
            num3Button.Length.Should().Be(1);

        }

        [TestMethod]
        public void FindsMultipleElements_FindAllDescendants_BeGreaterThanOne()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            var buttons = window.FindAllDescendants(x => x.ByControlType(Definitions.ControlType.Button));
            buttons.Length.Should().BeGreaterThan(1);
        }


    }
}
