using System;

namespace Services.Pipeline.Tests.StateControl
{
    using System.Threading;

    using NUnit.Framework;

    using Services.Pipeline.StateControl;

    using SharpTestsEx;

    [TestFixture, Category("Unit.Services")]
    public class InitializerTimerFixture
    {
        [Test]
        public void Execute_TimerShouldWaitToAssert()
        {
            // Arrange:
            var time = DateTime.Now.TimeOfDay;
            
            // Act:
            ServiceTimer.FromSeconds(1).Execute(() => this.Invoke(time).Should().Be.True()).Start();
            Thread.Sleep(TimeSpan.FromSeconds(3));
            
            // Assert:
            var result = Convert.ToInt32(DateTime.Now.TimeOfDay.TotalSeconds - time.TotalSeconds);
            result.Should().Be.EqualTo(3);
        }

        [Test]
        public void Execute_TimerShouldInvokeMethod()
        {
            // Arrange:
            var time = DateTime.Now.TimeOfDay;

            // Act:
            ServiceTimer.FromSeconds(3).Execute(() => this.Invoke(time).Should().Be.False()).Start();
            Thread.Sleep(TimeSpan.FromSeconds(3));
        }

        protected bool Invoke(TimeSpan time)
        {
            return time < DateTime.Now.TimeOfDay;
        }
    }
}
