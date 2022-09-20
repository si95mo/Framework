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
            manager.BasePath = IoUtility.GetDesktopFolder(); // To test the append

            await manager.AddEntry(new ReportEntry(12d, "Another description", "Another note"));

            await SaveAndTest(manager);
        }

        [Test]
        public async Task TestHtml()
        {
            HtmlReportManager manager = new HtmlReportManager("html_report");
            manager.BasePath = IoUtility.GetDesktopFolder(); // To test the append

            await manager.AddEntry(new ReportEntry(12d, "Another description", "Another note"));

            await SaveAndTest(manager);
        }

        [Test]
        public async Task TestXml()
        {
            XmlReportManager manager = new XmlReportManager("xml_report");
            await SaveAndTest(manager);
        }

        [Test]
        public async Task TestTxt()
        {
            TextReportManager manager = new TextReportManager("txt_report");
            await SaveAndTest(manager);
        }

        [Test]
        public async Task TestCsv()
        {
            CsvReportManager manager = new CsvReportManager("csv_manager");
            await SaveAndTest(manager);
        }
    }
}
