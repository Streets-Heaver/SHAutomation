using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SHAutomation.Core.AutomationElements;
using SHAutomation.Core.Exceptions;
using SHAutomation.Core.Input;
using SHAutomation.Core.StaticClasses;
using SHAutomation.Core.WindowsAPI;
using SHAutomation.UIA3;
using System;
using System.Linq;

namespace SHAutomation.Core.Tests.UI
{
    [TestClass]

    public class FindMethodTests : UITestBase
    {
        [TestMethod]
        public void NewXPathIsSaved_Find_BeEquivalentTo()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                window.XPathList.Clear();
                var num3Button = window.Find("num3Button");

                window.SaveXPathCache(TestContext.TestName);
                window.GetXPathCache(TestContext.TestName);
                window.XPathList.First().Should().BeEquivalentTo(("num3Button", "AutomationId", "/Group/Group[5]/Button[@AutomationId='num3Button']"));
            }
        }



        [TestMethod]
        public void FoundElementXPathStoredInCache_Find_BeEquivalentTo()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                window.XPathList.Clear();
                window.XPathList.Add(("num3Button", "AutomationId", @"/Group/Group[5]/Button[@AutomationId='num3Button']"));
                var num3Button = window.Find("num3Button");
                window.XPathList.Should().BeEquivalentTo((identifier: "num3Button", property: "AutomationId", xpath: "/Group/Group[5]/Button[@AutomationId='num3Button']"));
            }

        }
        [TestMethod]
        public void FoundElementXPathRequestCancelled_Find_BeEquivalentTo()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                window.XPathList.Clear();
                window.XPathList.Add(("num3Button", "AutomationId", @"/Group/Group[5]/Button[@AutomationId='num32Button']"));
                Action act = () =>
                {
                    var num3Button = window.Find("num3Button", TimeSpan.FromSeconds(20), null);
                    num3Button.FoundByXPath.Should().BeFalse();
                };
                var elapsed = PerformanceDiagnostics.Time(() =>
                {
                    act.Invoke();
                });
                elapsed.Should().BeLessOrEqualTo(TimeSpan.FromSeconds(10));
            }
        }
        [TestMethod]
        public void FoundElementConditionRequestCancelled_Find_BeEquivalentTo()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                window.XPathList.Clear();
                window.XPathList.Add(("num34Button", "AutomationId", @"/Group/Group[5]/Button[@AutomationId='num3Button']"));
                Action act = () =>
                {
                    var num3Button = window.Find("num34Button", TimeSpan.FromSeconds(20), null);
                    num3Button.FoundByXPath.Should().BeTrue();
                };
                var elapsed = PerformanceDiagnostics.Time(() =>
                {
                    act.Invoke();
                });
                elapsed.Should().BeLessOrEqualTo(TimeSpan.FromSeconds(10));
            }
        }
        [TestMethod]
        public void ElementFoundUsingConditionAndCache_Find_NotBeNull()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                window.GetXPathCache(TestContext.TestName);

                var num3Button = window.Find(x => x.ByAutomationId("num3Button"));
                num3Button.Should().NotBeNull();

                window.SaveXPathCache(TestContext.TestName);
            }

        }

        [TestMethod]
        public void ElementFoundUsingAutomationId_Find_NotBeNull()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var num3Button = window.Find("num3Button");
                num3Button.Should().NotBeNull();
            }

        }


        [TestMethod]
        public void ElementFoundUsingCondition_Find_NotBeNull()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var num3Button = window.Find(x => x.ByAutomationId("num3Button"));
                num3Button.Should().NotBeNull();
            }

        }
        [TestMethod]
        public void ElementFoundUsingParent_Find_NotBeNull()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                var numberPad = window.Find(x => x.ByAutomationId("NumberPad"));
                var num3Button = window.Find(x => x.ByAutomationId("num3Button"), parent: numberPad);
                num3Button.Should().NotBeNull();
            }
        }
        [TestMethod]
        public void ElementNotFoundWaitsForTimeout_Find_BeGreaterOrEqualTo()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                Action act = () => window.Find(x => x.ByAutomationId("nonExistentId"), TimeSpan.FromSeconds(5));
                var elapsed = PerformanceDiagnostics.Time(() =>
                {
                    act.Should().Throw<ElementNotFoundException>();
                });
                elapsed.Should().BeGreaterOrEqualTo(TimeSpan.FromSeconds(4.9)).And.BeLessOrEqualTo(TimeSpan.FromSeconds(6));
            }
        }
        [TestMethod]
        public void ElementNotFoundUsingParentWaitsForTimeout_Find_BeGreaterOrEqualTo()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                var numberPad = window.Find(x => x.ByAutomationId("NumberPad"));
                Action act = () => window.Find(x => x.ByAutomationId("nonExistentId"), TimeSpan.FromSeconds(5), parent: numberPad);
                var elapsed = PerformanceDiagnostics.Time(() =>
                {
                    act.Should().Throw<ElementNotFoundException>();
                });
                elapsed.Should().BeGreaterOrEqualTo(TimeSpan.FromSeconds(4.9)).And.BeLessOrEqualTo(TimeSpan.FromSeconds(6));
            }
        }
        [TestMethod]
        public void ElementFound_FindAllByXPath_HaveCount()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var num3Button = window.FindAllByXPath("/Group/Group[5]/Button[@AutomationId='num3Button']");
                num3Button.Should().HaveCount(1);
                num3Button[0].FoundByXPath.Should().BeTrue();
            }
        }

        [TestMethod]
        public void ElementNotFound_FindAllByXPath_HaveCount()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var num3Button = window.FindAllByXPath("/Group/Group[5]/Button[@AutomationId='randomthing']", TimeSpan.FromSeconds(2));
                num3Button.Should().BeEmpty();
            }

        }

        [TestMethod]
        public void ElementFound_FindFirstDescendant_NotBeNull()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var num3Button = window.FindFirstDescendant("num3Button");
                num3Button.FoundByXPath.Should().BeFalse();
                num3Button.Should().NotBeNull();
            }

        }

        [TestMethod]
        public void FindsSingleElement_FindAllDescendants_BeOne()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var num3Button = window.FindAllDescendants(x => x.ByAutomationId("num3Button"));
                num3Button.Length.Should().Be(1);
            }

        }

        [TestMethod]
        public void FindsMultipleElements_FindAllDescendants_BeGreaterThanOne()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var buttons = window.FindAllDescendants(x => x.ByControlType(Definitions.ControlType.Button));
                buttons.Length.Should().BeGreaterThan(1);
            }
        }



        [TestMethod]
        public void ExceptionThrownWhenParentNoLongerExists_Find_ThrowElementNotFoundException()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                var automationElement = new Mock<ISHAutomationElement>();
                Action act = () => window.Find("EquationsIconX", automationElement.Object);
                act.Should().Throw<ElementNotFoundException>().WithMessage("Parent not found");
            }
        }

        [TestMethod]
        public void ExceptionNotThrownWhenParentExists_Find_NotThrowElementNotFoundException()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                Action act = () => window.Find("num9Button", window.Find("NumberPad"));
                act.Should().NotThrow<ElementNotFoundException>();
            }
        }
        [TestMethod]
        public void CorrectElementReturned_FindFirstDescendantNameStartsWith_BeTrue()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var pad = window.FindFirstDescendantNameStartsWith("Number");
                pad.Name.Should().Be("Number pad");
                pad = window.FindFirstDescendantNameStartsWith("umber pad", TimeSpan.FromSeconds(5));
                pad.Should().BeNull();
            }
        }
        [TestMethod]
        public void CorrectElementReturned_FindFirstDescendantNameEndsWith_BeTrue()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var pad = window.FindFirstDescendantNameEndsWith("pad");
                pad.Name.Should().Be("Number pad");
                pad = window.FindFirstDescendantNameEndsWith("Number", TimeSpan.FromSeconds(5));
                pad.Should().BeNull();
            }
        }
        [TestMethod]
        public void CorrectElementReturned_FindFirstDescendantNameContains_BeTrue()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var pad = window.FindFirstDescendantNameContains("pad");
                pad.Name.Should().Be("Number pad");
                pad = window.FindFirstDescendantNameContains("pad", TimeSpan.FromSeconds(5));
                pad.Name.Should().Be("Number pad");
                pad = window.FindFirstDescendantNameContains("ber pa", TimeSpan.FromSeconds(5));
                pad.Name.Should().Be("Number pad");
            }
        }
        [TestMethod]
        public void CorrectElementReturned_FindAllDescendantNameStartsWith_BeTrue()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);
                Wait.UntilTrue(() => window.FindAllDescendantsNameStartsWith("Standard").Count() == 4, TimeSpan.FromSeconds(10));
                var pad = window.FindAllDescendantsNameStartsWith("Standard");
                pad.Count().Should().Be(4);
            }
        }
        [TestMethod]
        public void CorrectElementReturned_FindAllDescendantNameEndsWith_BeTrue()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var pad = window.FindAllDescendantsNameEndsWith("controls");
                pad.Count().Should().Be(2);
            }
        }
        [TestMethod]
        public void CorrectElementReturned_FindAllDescendantNameContains_BeTrue()
        {
            using (var calc = Application.LaunchStoreApp("Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"))
            using (var automation = new UIA3Automation())
            {
                var window = calc.GetMainWindow(automation);

                var pad = window.FindAllDescendantsNameContains("Memory");
                var test = pad[0].FindFirstChild();
                pad.Count().Should().Be(7);
            }
        }
    }
}
