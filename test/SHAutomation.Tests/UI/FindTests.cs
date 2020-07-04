using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SHAutomation.Core.Enums;
using SHAutomation.UIA3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SHAutomation.Core.Tests.UI
{
    [TestClass]

    public class FindTests : UITestBase
    {
        [TestMethod]
        public void ElementFoundUsingAutomationIdAndCache_Find_BeTrue()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);
           
            window.GetXPathCache(TestContext.TestName);
            
            var num3Button = window.Find("num3Button");
            num3Button.Should().NotBeNull();

            window.SaveXPathCache(TestContext.TestName);
        }
    }
}
