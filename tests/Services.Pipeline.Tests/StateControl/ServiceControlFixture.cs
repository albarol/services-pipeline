namespace Services.Pipeline.Tests.StateControl
{
    using System;

    using NUnit.Framework;

    using Services.Pipeline.StateControl;

    using SharpTestsEx;

    [TestFixture, Category("StateControl")]
    public class ServiceControlFixture
    {
        [Test]
        public void KeepState_ShouldSaveInformationAboutService()
        {
            // Arrange:
            var serviceInfo = new ServiceInfo
            {
                Name = "simple_service",
                State = ServiceState.Running,
                LatestExecution = DateTime.Now
            };

            // Act:
            ServiceControl.KeepState(serviceInfo);

            // Assert:
            var info = ServiceControl.RecoverState<ServiceInfo>("simple_service");
            info.State.Should().Not.Be.EqualTo(ServiceState.Stopped);
        }

        [Test]
        public void KeepState_ShouldUpdateStatusService()
        {
            // Arrange:
            var serviceInfo = new ServiceInfo
            {
                Name = "simple_service",
                State = ServiceState.Stopped,
                LatestExecution = DateTime.Now
            };
            ServiceControl.KeepState(serviceInfo);


            // Act:
            serviceInfo.State = ServiceState.Running;
            ServiceControl.KeepState(serviceInfo);

            // Arrange:
            var info = ServiceControl.RecoverState<ServiceInfo>("simple_service");
            info.State.Should().Be.EqualTo(ServiceState.Running);
        }

        [Test]
        public void RemoveState_ShouldDeleteCurrentStatus()
        {
            // Arrange:
            var serviceInfo = new ServiceInfo
            {
                Name = "simple_service",
                State = ServiceState.Stopped,
                LatestExecution = DateTime.Now
            };
            ServiceControl.KeepState(serviceInfo);


            // Act:
            ServiceControl.RemoveState("simple_service");

            // Arrange:
            var info = ServiceControl.RecoverState<ServiceInfo>("simple_service");
            info.Should().Be.Null();
        }
    }
}
