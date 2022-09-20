using FluentAssertions;
using IO;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Diagnostic.Report.Tests
{
    [TestFixture]
    public class TestClass
    {
        IReportEntry entry;

        [OneTimeSetUp]
        public void Setup()
        {
            entry = new ReportEntry(10d, "Entry description", "Entry notes");
        }

        [Test]
        public async Task TestJson()
        {
            JsonReportManager manager = new JsonReportManager("json_report");
            manager.BasePath = IoUtility.GetDesktopFolder();

            bool saved = await manager.AddEntry(entry);
            saved.Should().BeTrue();
        }
    }
}
