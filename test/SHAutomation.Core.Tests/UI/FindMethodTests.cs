using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SHAutomation.Core.Enums;
using SHAutomation.UIA3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
        public void FoundElementXPathStoredInCache_Find_BeEquivalentTo()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);


            var num3Button = window.Find("num3Button");
            var xpath = window.XPathList.Should().BeEquivalentTo((identifier: "num3Button", property: "AutomationId", xpath: "/Group/Group[5]/Button[@AutomationId='num3Button']"));
            
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
            num3Button.Count().Should().Be(1);

        }

        [TestMethod]
        public void FindsMultipleElements_FindAllDescendants_BeGreaterThanOne()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

            var buttons = window.FindAllDescendants(x => x.ByControlType(Definitions.ControlType.Button));
            buttons.Count().Should().BeGreaterThan(1);
        }


    }
}
