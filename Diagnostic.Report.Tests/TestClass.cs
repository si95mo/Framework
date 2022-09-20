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

        private async Task SaveAndTest(ReportManager manager)
        {
            manager.BasePath = IoUtility.GetDesktopFolder();

            bool saved = await manager.AddEntry(entry);
            saved &= IoUtility.DoesFileExist(manager.Path);

            saved.Should().BeTrue();
        }

        [Test]
        public async Task TestJson()
        {
            JsonReportManager manager = new JsonReportManager("json_report");
            await SaveAndTest(manager);
        }

        [Test]
        public async Task TestXlsx()
        {
            XlsxReportManager manager = new XlsxReportManager("xlsx_report");
            await SaveAndTest(manager);
        }
    }
}
