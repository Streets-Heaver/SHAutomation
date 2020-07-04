﻿using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SHAutomation.UIA3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace SHAutomation.Core.Tests.UI
{
    [TestClass]

    public class FindTests
    {
        [TestMethod]
        public void ElementFoundUsingAutomationId_Find_BeTrue()
        {
            var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);
            var num3Button = window.Find("num3Button");
            num3Button.Should().NotBeNull();

            calc.Close();
        }
    }
}
