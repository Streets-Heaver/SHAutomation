using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SHAutomation.Core.Input;
using SHAutomation.UIA3;
using System;
using System.Collections.Generic;
using System.Text;

namespace SHAutomation.Core.Tests.UI
{
    [TestClass]
    public class MouseTests : UITestBase
    {

        [TestMethod]
        public void ActionIsExecuted_MouseDrag_ShouldBeCorrectValue()
        {
            using var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            using var automation = new UIA3Automation();
            var window = calc.GetMainWindow(automation);

           window.Find("num3Button").HoverOver();

            Mouse.Drag(MouseButton.Left, () =>
            {
                Mouse.Up(MouseButton.Left);
                window.Find("num4Button").HoverOver();
                Mouse.Down(MouseButton.Left);
            });

            window.Find("CalculatorResults").Name.Should().Be("Display is 34");

        }
    }
}
