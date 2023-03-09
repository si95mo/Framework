using Extensions;
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
        private string tableStyle, headerStyle, rowStyle, alternateRowStyle;

        /// <summary>
        /// Create a new instance of <see cref="HtmlReportManager"/>
        /// </summary>
        /// <param name="fileName">The report file name (only the file name, no extension and full path)</param>
        /// <param name="tableStyle">The table style</param>
        /// <param name="headerStyle">The header style</param>
        /// <param name="rowStyle">The row style</param>
        /// <param name="alternateRowStyle">The alternate row style</param>
        public HtmlReportManager(string fileName, string tableStyle = "", string headerStyle = "", string rowStyle = "", string alternateRowStyle = "")
            : base(fileName, ReportExtension.Html)
        {
            entries = new List<IReportEntry>();

            this.tableStyle = tableStyle;
            this.headerStyle = headerStyle;
            this.rowStyle = rowStyle;
            this.alternateRowStyle = alternateRowStyle;
        }

        public override async Task<bool> AddEntry(IReportEntry entry)
        {
            entries.Add(entry);

            string text = entries.ToHtmlTable(tableStyle, headerStyle, rowStyle, alternateRowStyle);
            // Always overwrite the report file and recreate the table (the tags must be added at the end of the table)
            bool succeded = await SaveEntryTextAsync(text, SaveMode.Overwrite);

            return succeded;
        }
    }
}