namespace Services.Pipeline.Tests.Report
{
    using System.IO;

    using NUnit.Framework;

    using Services.Pipeline.Report.Logging;
    using Services.Pipeline.Report.Logging.Providers;

    using SharpTestsEx;

    [TestFixture]
    public class TextLogFacilityFixture
    {
        private const string FileName = "log.txt";
        private ILogFacility logFacility;


        [SetUp]
        public void SetUp()
        {
            this.logFacility = new TextLogFacility(FileName);
        }

        [TearDown]
        public void TearDown()
        {
            this.logFacility.Clear();
            File.Delete(FileName);
        }
        
        [Test]
        public void WriteError_CanCreateMessageWithoutArguments()
        {
            // Arrange:
            var message = "NotImplementatedException";

            // Act:
            this.logFacility.WriteError(message);

            // Assert:
            var entry = this.logFacility.ReadAll()[0];
            entry.EntryType.Should().Be.EqualTo(EntryType.Error);
        }

        [Test]
        public void WriteError_CanCreateMessageWitArguments()
        {
            // Arrange:
            var message = "NotImplementatedException: {0}";

            // Act:
            this.logFacility.WriteError(message, "Trace");

            // Assert:
            var entry = this.logFacility.ReadAll()[0];
            entry.Message.Should().Be.EqualTo("NotImplementatedException: Trace");
        }
    }
}
