using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SHAutomation.Core.StaticClasses;
using System;

namespace SHAutomation.Core.Tests.Unit
{
    [TestClass]
    public class SpinWaitTests
    {

        [TestMethod]
        public void CheckZeroTimeOutIsInvalid_SpinWait_ThrowInvalidOperationException()
        {

            Action act = () => SHSpinWait.SpinUntil(() => true, TimeSpan.Zero);

            act.Should().Throw<InvalidOperationException>("Should throw invalid operation exception if 0 timeout is passed");


        }

        [TestMethod]
        public void CheckNegativeTimeOutIsInvalid_SpinWait_ThrowInvalidOperationException()
        {
            Action act = () => SHSpinWait.SpinUntil(() => true, TimeSpan.FromMilliseconds(-10));

            act.Should().Throw<InvalidOperationException>("Should throw invalid operation exception if timeout < 0 is passed");

        }

        [TestMethod]
        public void CheckNonZeroTimeoutIsValid_SpinWait_NotThrowInvalidOperationException()
        {

            Action act = () => SHSpinWait.SpinUntil(() => true, TimeSpan.FromMilliseconds(10));

            act.Should().NotThrow<InvalidOperationException>("Should not throw invalid operation exception if timeout >0 is passed");


        }

        [TestMethod]
        public void TimesOutAfterTimeoutElapses_SpinWait_BeGreaterOrEqualTo()
        {

            var elapsed = PerformanceDiagnostics.Time(() =>
                {
                    SHSpinWait.SpinUntil(() => true == false, TimeSpan.FromMilliseconds(50));

                });

            //Because of the way spinuntil works there is a slight margin of error, this checks it's within that margin
            elapsed.Should().BeGreaterOrEqualTo(TimeSpan.FromMilliseconds(30)).And.BeLessOrEqualTo(TimeSpan.FromMilliseconds(70));


        }

        [TestMethod]
        public void ReturnsFalseWhenTimedOut_SpinWait_BeFalse()
        {
            SHSpinWait.SpinUntil(() => true == false, TimeSpan.FromMilliseconds(50)).Should().BeFalse();
        }

    }
}
