using System.Collections.Generic;
using System.Threading.Tasks;
using static IO.FileHandler;

namespace Diagnostic.Report
{
    /// <summary>
    /// Define an html <see cref="ReportManager"/>
    /// </summary>
    public class HtmlReportManager : ReportManager
    {
        private List<IReportEntry> entries;

        /// <summary>
        /// Create a new instance of <see cref="HtmlReportManager"/>
        /// </summary>
        /// <param name="fileName">The report file name (only the file name, no extension and full path)</param>
        public HtmlReportManager(string fileName) : base(fileName, ReportExtension.Html)
        {
            entries = new List<IReportEntry>();
        }

        public override async Task<bool> AddEntry(IReportEntry entry)
        {
            entries.Add(entry);

            string text = entries.ToHtmlTable();
            bool succeded = await SaveEntryTextAsync(text, SaveMode.Overwrite); // This method always overwrite the report file and recreate the table

            return succeded;
        }
    }
}